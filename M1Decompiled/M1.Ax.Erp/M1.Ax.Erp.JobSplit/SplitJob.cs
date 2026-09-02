using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using M1.Core;
using M1.Core.Database;
using M1.Extensions;
using M1.Forms.Controls.Forms;

namespace M1.Ax.Erp.JobSplit;

public class SplitJob
{
	public class ReversalFieldNames
	{
		public string LineIdFieldName;

		public string LineLineFieldName;

		public string ComponentIdFieldName;

		public string ComponentLineFieldName;

		public string ComponentComponentFieldName;
	}

	private const string ValidationNodeText = "Split Job Execution Validations";

	private readonly ErrorItemsList _errorList = new ErrorItemsList();

	private bool _sourceJobUpdated;

	private bool _sourceJobAssemblyUpdated;

	private DataTable _initialJobOperationsSourceTable = new DataTable();

	private bool _initialJobOperationsSaved;

	private DataTable _initialReceiptLinesSourceTable;

	private DataTable _initialReceiptComponentsSourceTable;

	private DataTable _initialTimecardLinesSourceTable;

	private DataTable _partTransactionCostsPreSplitSource;

	private DataTable _partTransactionsPreSplitSource;

	private DataTable _purchaseOrderLinesPreSplitSource;

	private DataTable _purchaseOrderAccountsPreSplitSource;

	private DataTable _purchaseOrderComponentsPreSplitSource;

	private DataTable _apInvoiceLinesPreSplitSource;

	private DataTable _apExpenseAccountsPreSplitSource;

	private DataTable _mfgReceiptPreSplitSourceTable;

	private DataTable _mfgReceiptComponentsPreSplitSourceTable;

	private DataTable _glJournalsPreSplitSource;

	private DataTable _glJournalLinesPreSplitSource;

	private readonly Dictionary<int, int> _newGLJournalIdForSourceGLJournalReversal = new Dictionary<int, int>();

	private readonly Dictionary<int, int> _newGLJournalIdForTargetGLJournal = new Dictionary<int, int>();

	private readonly Dictionary<string, object> _glJournalLinesToUpdate = new Dictionary<string, object>();

	private readonly Dictionary<string, double> _apTaxLines = new Dictionary<string, double>();

	private readonly Dictionary<string, APInvoiceForGL> _apInvoicesInGL = new Dictionary<string, APInvoiceForGL>();

	private readonly HashSet<string> _glJournalConsolidatedLines = new HashSet<string>();

	private readonly HashSet<string> _glJournalTargetLinesCreated = new HashSet<string>();

	public DataTable JobAssembliesSource { get; set; }

	public DataTable JobMaterialsSource { get; set; }

	public DataTable JobMaterialIssueLinesSource { get; set; }

	public DataTable JobMaterialComponentsSource { get; set; }

	public DataTable JobMaterialIssueComponents { get; set; }

	public int SelectedRootAssembly { get; set; } = -1;

	public IMessageAdapter Message { get; set; }

	private Dictionary<string, GLJournalInfo> UpdatePartTransactionID { get; set; } = new Dictionary<string, GLJournalInfo>();

	public SplitJob()
	{
		Message = new MessageAdapter();
	}

	public SplitJob(M1Database database, string sourceJobId, int sourceAsm)
		: this()
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select * from JobAssemblies where jmaJobID = @SourceTableJobID");
		sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceJobId;
		JobAssembliesSource = database.GetDataTable(sqlCommand, fillSchema: true, out var _, null);
		JobMaterialsSource = null;
		JobMaterialComponentsSource = null;
		SelectedRootAssembly = sourceAsm;
	}

	public bool Validate(M1Database database, DataRow sourceJob, DataRow sourceJobAssembly, M1BindingSource targetJobsLines, SplitCostOption splitCostOption, List<int> assembliesToIgnore)
	{
		_errorList.Clear();
		ValidationForJobIDSuffixValues(database, targetJobsLines, _errorList);
		ValidationForTotalTargetProductionQty(targetJobsLines, sourceJobAssembly, sourceJob, _errorList);
		ValidationForTargetOrderInventoryAndScrapQtys(targetJobsLines, sourceJobAssembly, _errorList);
		ValidationForZeroProduction(targetJobsLines, _errorList);
		ValidationForJobsToCreateValueOfZero(targetJobsLines, _errorList);
		ValidationForSubAssemblyMaterialIssues(database, sourceJobAssembly, _errorList, splitCostOption);
		ValidationForSubAssemblyMiscReceipt(database, sourceJobAssembly, _errorList, splitCostOption);
		if (splitCostOption == SplitCostOption.MoveCostsToTargetJob)
		{
			ValidationForMoveCostsToTargetJob(targetJobsLines, _errorList);
		}
		string text = (string)sourceJob["jmpJobID"];
		SqlCommand sqlCommand = database.NewSqlCommand("select * from JobAssemblies where jmaJobID = @SourceTableJobID");
		sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = text;
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: true, out adapter, null);
		sqlCommand = database.NewSqlCommand("select * from JobMaterials where jmmJobID = @SourceTableJobID");
		sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = text;
		DataTable dataTable2 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter, null);
		sqlCommand = database.NewSqlCommand("select * from JobMaterialComponents where jmtJobID = @SourceTableJobID");
		sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = text;
		DataTable dataTable3 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter, null);
		if (splitCostOption != SplitCostOption.KeepCostsOnSourceJob)
		{
			ValidateSerialAndLotStatus(database, null, dataTable, dataTable2, dataTable3, _errorList, assembliesToIgnore);
		}
		string text2 = JobActiveMrpSessions(database, text);
		if (text2.Length > 0)
		{
			ValidationInfo validationInfo = new ValidationInfo
			{
				RowDescription = "Job ID '" + text + "'"
			};
			validationInfo.AddError("Split Job cannot be executed because the Job ID is present as Demand or Supply in an active MRP session(s): " + text2 + ". Create Jobs or Clear Data for that MRP Session(s) to enable Split Job execution.");
			_errorList.Add(validationInfo);
		}
		if (_errorList.Count != 0)
		{
			using ShowErrorsDialog showErrorsDialog = new ShowErrorsDialog(database);
			showErrorsDialog.AttachErrors(_errorList);
			showErrorsDialog.ButtonMessageText = "Select the desired action.";
			if (Message.ShowErrorsDialog(showErrorsDialog) != DialogResult.OK)
			{
				return false;
			}
		}
		return true;
	}

	private void ValidationForSubAssemblyMaterialIssues(M1Database database, DataRow sourceJobAssembly, ErrorItemsList errorList, SplitCostOption splitCostOption)
	{
		if (sourceJobAssembly.Field<int>("jmaJobAssemblyID") > 0)
		{
			SqlCommand sqlCommand = database.NewSqlCommand(string.Format("SELECT COUNT(*) FROM JobAssemblies INNER JOIN MaterialIssueLines ON injJobID=jmaJobID AND injJobAssemblyID=jmaJobAssemblyID WHERE jmaJobID = '{0}' AND jmaJobAssemblyID={1} AND jmaQuantityIssued>0 AND injReversed=0", sourceJobAssembly.Field<string>("jmaJobID"), sourceJobAssembly.Field<int>("jmaJobAssemblyID")));
			if ((int)database.ExecuteScalar(sqlCommand) > 0 && splitCostOption != SplitCostOption.KeepCostsOnSourceJob)
			{
				ValidationInfo validationInfo = new ValidationInfo
				{
					RowDescription = "Job ID '" + sourceJobAssembly.Field<string>("jmaJobID") + "'"
				};
				validationInfo.AddError("The Source Assembly has Qty Issued and can ONLY be split if Split Cost Options is set to Keep Costs on Source Job");
				errorList.Add(validationInfo);
			}
		}
	}

	private void ValidationForSubAssemblyMiscReceipt(M1Database database, DataRow sourceJobAssembly, ErrorItemsList errorList, SplitCostOption splitCostOption)
	{
		if (sourceJobAssembly.Field<int>("jmaJobAssemblyID") > 0)
		{
			SqlCommand sqlCommand = database.NewSqlCommand(string.Format("SELECT COUNT(*) FROM JobAssemblies INNER JOIN MfgReceipts ON rmmJobID=jmaJobID AND rmmJobAssemblyID=jmaJobAssemblyID WHERE jmaJobID = '{0}' AND jmaJobAssemblyID={1} AND jmaQuantityIssued>0 AND rmmReversed=0 AND rmmReceiptType=1 AND rmmJobType=3", sourceJobAssembly.Field<string>("jmaJobID"), sourceJobAssembly.Field<int>("jmaJobAssemblyID")));
			if ((int)database.ExecuteScalar(sqlCommand) > 0 && splitCostOption != SplitCostOption.KeepCostsOnSourceJob)
			{
				ValidationInfo validationInfo = new ValidationInfo
				{
					RowDescription = "Job ID '" + sourceJobAssembly.Field<string>("jmaJobID") + "'"
				};
				validationInfo.AddError("The Source Assembly has Qty Issued and can ONLY be split if Split Cost Options is set to Keep Costs on Source Job");
				errorList.Add(validationInfo);
			}
		}
	}

	private void ValidationForJobIDSuffixValues(M1Database database, M1BindingSource jobsToCreateLines, ErrorItemsList errorList)
	{
		DataRowCollection rows = jobsToCreateLines.GetDataTable().Rows;
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		foreach (var item in from g in (from DataRow row in rows
				select string.Format("{0}{1}", row["JobID"], row["Suffix"])).Select((string j, int i) => new
			{
				Line = i + 1,
				JobID = j
			})
			group g by g.JobID into g
			where g.Count() > 1
			select g)
		{
			foreach (var item2 in item)
			{
				text += $" Line {item2.Line},";
			}
		}
		for (int num = 0; num < rows.Count; num++)
		{
			string text5 = rows[num].Field<string>("JobID") + rows[num].Field<string>("Suffix");
			bool flag = string.IsNullOrEmpty(rows[num].Field<string>("JobID"));
			if (flag)
			{
				text2 += $" Line {num + 1},";
			}
			SqlCommand sqlCommand = database.NewSqlCommand("Select count(*) From Jobs Where jmpJobID = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = text5;
			if ((int)database.ExecuteScalar(sqlCommand) > 0 && !flag)
			{
				text3 += $" Line {num + 1},";
			}
			if (text5.Length > 20)
			{
				text4 += $" Line {num + 1},";
			}
		}
		if (text.Any() || text2.Any() || text3.Any() || text4.Any())
		{
			ValidationInfo validationInfo = new ValidationInfo
			{
				RowDescription = "Split Job Execution Validations"
			};
			if (text.Any())
			{
				validationInfo.AddError("Duplicate Job ID/Suffix combinations exist on the following Target Job lines: '" + text.TrimEnd(',') + "'");
			}
			if (text2.Any())
			{
				validationInfo.AddError("Job ID must be entered for the following Target Job lines: '" + text2.TrimEnd(',') + "'");
			}
			if (text3.Any())
			{
				validationInfo.AddError("Job ID/Suffix combination already exists in the database for Target Job lines: '" + text3.TrimEnd(',') + "'");
			}
			if (text4.Any())
			{
				validationInfo.AddError("Concatenation of Job ID/Suffix must be 20 characters or less for Target Job lines: '" + text4.TrimEnd(',') + "'");
			}
			errorList.Add(validationInfo);
		}
	}

	public void ValidationForJobsToCreateValueOfZero(M1BindingSource jobsToCreateLines, ErrorItemsList errorList)
	{
		if (jobsToCreateLines.GetDataTable().Rows.Count == 0)
		{
			ValidationInfo validationInfo = new ValidationInfo
			{
				RowDescription = "Split Job Execution Validations"
			};
			validationInfo.AddError("At least ONE Target Job must be entered.");
			errorList.Add(validationInfo);
		}
	}

	public void ValidationForTotalTargetProductionQty(M1BindingSource jobsToCreateLines, DataRow jobAssembliesSource, DataRow jobSource, ErrorItemsList errorList)
	{
		DataRowCollection rows = jobsToCreateLines.GetDataTable().Rows;
		decimal num = default(decimal);
		for (int i = 0; i < rows.Count; i++)
		{
			num += rows[i].Field<decimal>("jmpProductionQuantity");
		}
		decimal num2 = jobAssembliesSource.Field<decimal>("jmaProductionQuantity") - (jobSource.Field<decimal>("jmpQuantityShipped") + jobAssembliesSource.Field<decimal>("jmaQuantityReceivedToInventory") + jobAssembliesSource.Field<decimal>("jmaQuantityToInspect") + jobAssembliesSource.Field<decimal>("jmaScrapQuantityCompleted"));
		decimal num3 = jobAssembliesSource.Field<decimal>("jmaQuantityCompleted");
		decimal num4 = jobAssembliesSource.Field<decimal>("jmaProductionQuantity") - jobAssembliesSource.Field<decimal>("jmaQuantityCompleted");
		if (num > num2 || (num3 > 0m && num > num4))
		{
			ValidationInfo validationInfo = new ValidationInfo
			{
				RowDescription = "Split Job Execution Validations"
			};
			validationInfo.AddError("The sum of the Production Qty for all Target Jobs can not be GREATER THAN the Source Job Production Qty LESS the quantity that has already gone through production.");
			errorList.Add(validationInfo);
		}
	}

	private void ValidationForTargetOrderInventoryAndScrapQtys(M1BindingSource jobsToCreateLines, DataRow jobAssembliesSource, ErrorItemsList errorList)
	{
		DataTable table = jobsToCreateLines.GetDataView().Table;
		if (table.Rows.Count > 0)
		{
			if (jobAssembliesSource.Field<decimal>("jmaOrderQuantity") - (decimal)table.Compute("Sum(jmpOrderQuantity)", string.Empty) < 0m)
			{
				ValidationInfo validationInfo = new ValidationInfo
				{
					RowDescription = "Split Job Execution Validations"
				};
				validationInfo.AddError("Sum of the Target Jobs Order Qty CANNOT BE GREATER than the Source Job Order Qty");
				errorList.Add(validationInfo);
			}
			if (jobAssembliesSource.Field<decimal>("jmaInventoryQuantity") - (decimal)table.Compute("Sum(jmpInventoryQuantity)", string.Empty) < 0m)
			{
				ValidationInfo validationInfo2 = new ValidationInfo
				{
					RowDescription = "Split Job Execution Validations"
				};
				validationInfo2.AddError("Sum of the Target Jobs Inventory Qty CANNOT BE GREATER than the Source Job Inventory Qty");
				errorList.Add(validationInfo2);
			}
			if (jobAssembliesSource.Field<decimal>("jmaScrapQuantity") - (decimal)table.Compute("Sum(jmpScrapQuantity)", string.Empty) < 0m)
			{
				ValidationInfo validationInfo3 = new ValidationInfo
				{
					RowDescription = "Split Job Execution Validations"
				};
				validationInfo3.AddError("Sum of the Target Jobs Scrap Qty CANNOT BE GREATER than the Source Job Scrap Qty");
				errorList.Add(validationInfo3);
			}
		}
	}

	public void ValidationForZeroProduction(M1BindingSource jobsToCreateLines, ErrorItemsList errorList)
	{
		DataRowCollection rows = jobsToCreateLines.GetDataTable().Rows;
		for (int i = 0; i < rows.Count; i++)
		{
			if (rows[i].Field<decimal>("jmpProductionQuantity") == 0m)
			{
				ValidationInfo validationInfo = new ValidationInfo
				{
					RowDescription = "Split Job Execution Validations"
				};
				string text = rows[i].Field<string>("JobID") + rows[i].Field<string>("Suffix");
				validationInfo.AddWarning("Target Job '" + text + "' will have a Production Qty of 0 after Split Job has been executed.\u00a0");
				errorList.Add(validationInfo);
			}
		}
	}

	public void ValidationForMoveCostsToTargetJob(M1BindingSource jobsToCreateLines, ErrorItemsList errorList)
	{
		if (jobsToCreateLines.GetDataTable().Rows.Count > 1)
		{
			ValidationInfo validationInfo = new ValidationInfo
			{
				RowDescription = "Split Job Execution Validations"
			};
			validationInfo.AddError("Only ONE Target Job is allowed when Split Cost Option is set to 'Move Costs to Target Job'.");
			errorList.Add(validationInfo);
		}
	}

	public bool JobSplit(M1Database database, string sourceTableJobID, int sourceTableJobAssemblyID, int startSeq, TargetJob targetJob, double totalOrderQuantity, double totalInventoryQuantity, double totalScrapQuantity, double totalProductionQuantity, SplitCostOption splitCosts, double initialDestPercent, bool removeJobFromSchedule, double sourcePercent, List<int> assembliesToIgnore, string oAssembliesToIgnore = "")
	{
		bool result = false;
		string jobId = targetJob.JobId;
		double orderQuantity = targetJob.OrderQuantity;
		double inventoryQuantity = targetJob.InventoryQuantity;
		double scrapQuantity = targetJob.ScrapQuantity;
		double productionQuantity = targetJob.ProductionQuantity;
		object productionDueDate = targetJob.ProductionDueDate;
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			if (oAssembliesToIgnore != "")
			{
				oAssembliesToIgnore = "," + oAssembliesToIgnore + ",";
			}
			DataTable dataTable = new DataTable();
			DataTable dataTable2 = new DataTable();
			DataTable dataTable3 = new DataTable();
			DataTable dataTable4 = new DataTable();
			DataTable dataTable5 = new DataTable();
			new DataTable();
			new DataTable();
			new DataTable();
			DataTable dataTable6 = new DataTable();
			DataTable dataTable7 = new DataTable();
			DataTable dataTable8 = new DataTable();
			DataTable dataTable9 = new DataTable();
			DataTable salesOrderJobLinksDestTable = new DataTable();
			DataTable jobMemosDestTable = new DataTable();
			DataTable attachmentsDestTable = new DataTable();
			DataTable dataTable10 = new DataTable();
			DataTable dataTable11 = new DataTable();
			DataTable dataTable12 = new DataTable();
			DataTable dataTable13 = new DataTable();
			DataTable dataTable14 = new DataTable();
			DataTable dataTable15 = new DataTable();
			DataTable dataTable16 = new DataTable();
			DataTable dataTable17 = new DataTable();
			DataTable dataTable18 = new DataTable();
			DataTable dataTable19 = new DataTable();
			DataTable dataTable20 = new DataTable();
			DataTable dataTable21 = new DataTable();
			DataTable dataTable22 = new DataTable();
			DataTable dataTable23 = new DataTable();
			DataTable dataTable24 = new DataTable();
			DataTable dataTable25 = new DataTable();
			new DataTable();
			new DataTable();
			DataTable dataTable26 = new DataTable();
			DataTable dataTable27 = new DataTable();
			DataTable dataTable28 = new DataTable();
			DataTable dataTable29 = new DataTable();
			DataTable dataTable30 = new DataTable();
			DataTable dataTable31 = new DataTable();
			DataTable dataTable32 = new DataTable();
			DataTable dataTable33 = new DataTable();
			DataTable dataTable34 = new DataTable();
			DataTable dataTable35 = new DataTable();
			DataTable dataTable36 = new DataTable();
			DataTable dataTable37 = new DataTable();
			DataTable dataTable38 = new DataTable();
			DataTable dataTable39 = new DataTable();
			DataTable dataTable40 = new DataTable();
			DataTable dataTable41 = new DataTable();
			DataTable dataTable42 = new DataTable();
			DataTable dataTable43 = new DataTable();
			DataTable dataTable44 = new DataTable();
			new DataTable();
			DataTable dataTable45 = new DataTable();
			DataTable dataTable46 = new DataTable();
			DataTable dataTable47 = new DataTable();
			DataTable dataTable48 = new DataTable();
			sourceTableJobID = sourceTableJobID.Trim().ToUpper();
			jobId = jobId.Trim().ToUpper();
			SqlCommand sqlCommand = database.NewSqlCommand("select * from Jobs where jmpJobID = @NewJobID");
			sqlCommand.Parameters.Add(new SqlParameter("@NewJobID", SqlDbType.NVarChar)).Value = jobId;
			database.GetDataTable(sqlCommand, fillSchema: true, out var adapter, sqlTransaction);
			dataTable6 = database.GetDataTable("select * from JobAssemblies where 0=1", fillSchema: true, out var adapter2, sqlTransaction);
			dataTable7 = database.GetDataTable("select * from JobMaterials where 0=1", fillSchema: true, out var adapter3, sqlTransaction);
			dataTable8 = database.GetDataTable("select * from JobMaterialComponents where 0=1", fillSchema: true, out var adapter4, sqlTransaction);
			dataTable9 = database.GetDataTable("select * from JobOperations where 0=1", fillSchema: true, out var adapter5, sqlTransaction);
			dataTable28 = database.GetDataTable("select * from PartTransactions where 0=1", fillSchema: true, out var adapter6, sqlTransaction);
			dataTable29 = database.GetDataTable("select * from PartTransactionCosts where 0=1", fillSchema: true, out var adapter7, sqlTransaction);
			dataTable30 = database.GetDataTable("select * from TimecardLines where 0=1", fillSchema: true, out var adapter8, sqlTransaction);
			dataTable31 = database.GetDataTable("select * from PurchaseOrderLines where 0=1", fillSchema: true, out var adapter9, sqlTransaction);
			dataTable33 = database.GetDataTable("Select * From PurchaseOrderAccounts Where 0=1", fillSchema: true, out var adapter10, sqlTransaction);
			dataTable32 = database.GetDataTable("Select * From PurchaseOrderComponents Where 0=1", fillSchema: true, out var adapter11, sqlTransaction);
			dataTable34 = database.GetDataTable("select * from ReceiptLines where 0=1", fillSchema: true, out var adapter12, sqlTransaction);
			dataTable35 = database.GetDataTable("Select * From ReceiptComponents Where 0=1", fillSchema: true, out var adapter13, sqlTransaction);
			dataTable36 = database.GetDataTable("select * from APInvoiceLines where 0=1", fillSchema: true, out var adapter14, sqlTransaction);
			dataTable37 = database.GetDataTable("Select * From APInvoiceExpenseAccounts Where 0=1", fillSchema: true, out var adapter15, sqlTransaction);
			dataTable38 = database.GetDataTable("select * from MaterialIssueLines where 0=1", fillSchema: true, out var adapter16, sqlTransaction);
			dataTable39 = database.GetDataTable("Select * From MaterialIssueComponents Where 0=1", fillSchema: true, out var adapter17, sqlTransaction);
			dataTable40 = database.GetDataTable("select * from MfgReceipts where 0=1", fillSchema: true, out var adapter18, sqlTransaction);
			dataTable41 = database.GetDataTable("Select * From MfgReceiptComponents Where 0=1", fillSchema: true, out var adapter19, sqlTransaction);
			dataTable42 = database.GetDataTable("Select * From GLJournals Where 0=1", fillSchema: true, out var adapter20, sqlTransaction);
			dataTable43 = database.GetDataTable("Select * From GLJournalLines Where 0=1", fillSchema: true, out var adapter21, sqlTransaction);
			dataTable44 = database.GetDataTable("select * from ShipmentLines where 0=1", fillSchema: true, out var _, sqlTransaction);
			SqlDataAdapter adapter23;
			DataTable dataTable49 = database.GetDataTable("Select * From ShipmentComponents Where 0=1", fillSchema: true, out adapter23, sqlTransaction);
			dataTable45 = database.GetDataTable("Select * From SerialNumberTransactions Where 0=1", fillSchema: true, out var adapter24, sqlTransaction);
			dataTable46 = database.GetDataTable("Select * From LotNumberTransactions Where 0=1", fillSchema: true, out var adapter25, sqlTransaction);
			dataTable45.Columns["sntSerialNumberTransactionID"].ReadOnly = false;
			dataTable46.Columns["abtLotNumberTransactionID"].ReadOnly = false;
			dataTable30.DefaultView.Sort = "lmlTimecardLineID ASC";
			dataTable31.DefaultView.Sort = "pmlPurchaseOrderLineID ASC";
			dataTable33.DefaultView.Sort = "pmxPurchaseOrderAccountID ASC";
			dataTable32.DefaultView.Sort = "pmoPurchaseOrderComponentID ASC";
			dataTable34.DefaultView.Sort = "rmlReceiptLineID ASC";
			dataTable35.DefaultView.Sort = "rmoReceiptComponentID ASC";
			dataTable36.DefaultView.Sort = "aplAPInvoiceLineID ASC";
			dataTable37.DefaultView.Sort = "apxAPInvoiceExpenseAccountID ASC";
			dataTable38.DefaultView.Sort = "injMaterialIssueLineID ASC";
			dataTable39.DefaultView.Sort = "inkMaterialIssueComponentID ASC";
			dataTable40.DefaultView.Sort = "rmmMfgReceiptID ASC";
			dataTable41.DefaultView.Sort = "rmnMfgReceiptComponentID ASC";
			dataTable42.DefaultView.Sort = "glpGLJournalID ASC";
			dataTable43.DefaultView.Sort = "gllGLJournalLineID ASC";
			dataTable44.DefaultView.Sort = "smlShipmentLineID ASC";
			dataTable49.DefaultView.Sort = "smoShipmentComponentID ASC";
			sqlCommand = database.NewSqlCommand("select * from Jobs where jmpJobID = @SourceTableJobID");
			sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
			dataTable = database.GetDataTable(sqlCommand, fillSchema: true, out var adapter26, sqlTransaction);
			if (dataTable.Rows.Count == 0)
			{
				throw new M1MissingOrInvalidDataException("M1 was unable to find job " + sourceTableJobID + " in the Jobs table. The job split will not continue.");
			}
			sqlCommand = database.NewSqlCommand("select * from JobAssemblies where jmaJobID = @SourceTableJobID");
			sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
			dataTable2 = database.GetDataTable(sqlCommand, fillSchema: true, out var adapter27, sqlTransaction);
			sqlCommand = database.NewSqlCommand("select * from JobMaterials where jmmJobID = @SourceTableJobID");
			sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
			dataTable3 = database.GetDataTable(sqlCommand, fillSchema: true, out var adapter28, sqlTransaction);
			JobMaterialsSource = JobMaterialsSource ?? database.GetDataTable(sqlCommand, fillSchema: true, out var adapter29, sqlTransaction);
			sqlCommand = database.NewSqlCommand("select * from JobMaterialComponents where jmtJobID = @SourceTableJobID");
			sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
			dataTable4 = database.GetDataTable(sqlCommand, fillSchema: true, out var adapter30, sqlTransaction);
			JobMaterialComponentsSource = JobMaterialComponentsSource ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter29, sqlTransaction);
			sqlCommand = database.NewSqlCommand("select * from JobOperations where jmoJobID = @SourceTableJobID");
			sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
			dataTable5 = database.GetDataTable(sqlCommand, fillSchema: true, out var adapter31, sqlTransaction);
			dataTable47 = database.GetDataTable("select * from JobSplitLog where 0=1", fillSchema: true, out var adapter32, sqlTransaction);
			dataTable48 = database.GetDataTable("select * from JobSplitLogLines where 0=1", fillSchema: true, out var adapter33, sqlTransaction);
			int num = (int)database.ExecuteScalar("Select IsNull(Max(jsgJobSplitLogID),0)+1 From JobSplitLog", sqlTransaction);
			dataTable47.Columns["jsgJobSplitLogID"].ReadOnly = false;
			SqlDataAdapter adapter34 = new SqlDataAdapter();
			SqlDataAdapter adapter35 = new SqlDataAdapter();
			SqlDataAdapter adapter36 = new SqlDataAdapter();
			SqlDataAdapter adapter37 = new SqlDataAdapter();
			SqlDataAdapter adapter38 = new SqlDataAdapter();
			SqlDataAdapter adapter39 = new SqlDataAdapter();
			SqlDataAdapter adapter40 = new SqlDataAdapter();
			SqlDataAdapter adapter41 = new SqlDataAdapter();
			SqlDataAdapter adapter42 = new SqlDataAdapter();
			SqlDataAdapter adapter43 = new SqlDataAdapter();
			new SqlDataAdapter();
			SqlDataAdapter soJobLinksDestDataAdapter = new SqlDataAdapter();
			SqlDataAdapter adapter44 = new SqlDataAdapter();
			SqlDataAdapter adapter45 = new SqlDataAdapter();
			SqlDataAdapter adapter46 = new SqlDataAdapter();
			SqlDataAdapter adapter47 = new SqlDataAdapter();
			SqlDataAdapter adapter48 = new SqlDataAdapter();
			SqlDataAdapter adapter49 = new SqlDataAdapter();
			SqlDataAdapter adapter50 = new SqlDataAdapter();
			SqlDataAdapter adapter51 = new SqlDataAdapter();
			SqlDataAdapter adapter52 = new SqlDataAdapter();
			SqlDataAdapter adapter53 = new SqlDataAdapter();
			SqlDataAdapter jobMemosDestDataAdapter = new SqlDataAdapter();
			SqlDataAdapter attachmentsDestDataAdapter = new SqlDataAdapter();
			if (splitCosts != SplitCostOption.KeepCostsOnSourceJob)
			{
				sqlCommand = database.NewSqlCommand("select * from PartTransactions where imtJobID = @SourceTableJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable10 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter34, sqlTransaction);
				_partTransactionsPreSplitSource = _partTransactionsPreSplitSource ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter34, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from PartTransactionCosts where EXISTS (select imtTableUniqueID from PartTransactions where imtJobID = @SourceTableJobID and imtPartTransactionID = intPartTransactionID)");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable11 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter35, sqlTransaction);
				_partTransactionCostsPreSplitSource = _partTransactionCostsPreSplitSource ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter35, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from TimecardLines where lmlJobID = @SourceTableJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable12 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter36, sqlTransaction);
				_initialTimecardLinesSourceTable = _initialTimecardLinesSourceTable ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter36, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from PurchaseOrderLines where pmlJobID = @SourceTableJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable13 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter37, sqlTransaction);
				_purchaseOrderLinesPreSplitSource = _purchaseOrderLinesPreSplitSource ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter37, sqlTransaction);
				sqlCommand = database.NewSqlCommand("SELECT * FROM PurchaseOrderComponents WHERE EXISTS (SELECT pmlPurchaseOrderID, pmlPurchaseOrderLineID FROM PurchaseOrderLines WHERE pmoPurchaseOrderID = pmlPurchaseOrderID AND pmoPurchaseOrderLineID = pmlPurchaseOrderLineID AND pmlJobID = @SourceTableJobID) ORDER BY pmoPurchaseOrderLineID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable15 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter39, sqlTransaction);
				_purchaseOrderComponentsPreSplitSource = _purchaseOrderComponentsPreSplitSource ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter39, sqlTransaction);
				sqlCommand = database.NewSqlCommand("Select * from PurchaseOrderAccounts Where pmxPurchaseOrderID + '-' + Convert(varchar(4), pmxPurchaseOrderLineID) IN (Select pmlPurchaseOrderID + '-' + Convert(varchar(4), pmlPurchaseOrderLineID) From PurchaseOrderLines Where pmlJobID = @SourceTableJobID)");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable14 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter38, sqlTransaction);
				_purchaseOrderAccountsPreSplitSource = _purchaseOrderAccountsPreSplitSource ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter38, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from ReceiptLines where rmlJobID = @SourceTableJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable16 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter40, sqlTransaction);
				_initialReceiptLinesSourceTable = _initialReceiptLinesSourceTable ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter40, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from ReceiptComponents where rmoJobID = @SourceTableJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable17 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter41, sqlTransaction);
				_initialReceiptComponentsSourceTable = _initialReceiptComponentsSourceTable ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter41, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from APInvoiceLines where aplJobID = @SourceTableJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable18 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter42, sqlTransaction);
				_apInvoiceLinesPreSplitSource = _apInvoiceLinesPreSplitSource ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter42, sqlTransaction);
				sqlCommand = database.NewSqlCommand("Select * From APInvoiceExpenseAccounts Where apxAPInvoiceID + '-' + Convert(varchar(4), apxAPInvoiceLineID) IN (Select aplAPInvoiceID + '-' + Convert(varchar(4), aplAPInvoiceLineID) From APInvoiceLines Where aplJobID = @SourceTableJobID)");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable19 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter43, sqlTransaction);
				_apExpenseAccountsPreSplitSource = _apExpenseAccountsPreSplitSource ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter43, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from MaterialIssueLines where injJobID = @SourceTableJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable20 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter44, sqlTransaction);
				JobMaterialIssueLinesSource = JobMaterialIssueLinesSource ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter29, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from MaterialIssueComponents where inkJobID = @SourceTableJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable21 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter45, sqlTransaction);
				JobMaterialIssueComponents = JobMaterialIssueComponents ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter29, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from MfgReceipts where rmmJobID = @SourceTableJobID and rmmReceiptType = 1");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable22 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter46, sqlTransaction);
				_mfgReceiptPreSplitSourceTable = _mfgReceiptPreSplitSourceTable ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter46, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from MfgReceiptComponents where rmnJobID = @SourceTableJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable23 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter47, sqlTransaction);
				_mfgReceiptComponentsPreSplitSourceTable = _mfgReceiptComponentsPreSplitSourceTable ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter47, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from GLJournals where glpDetailSource in (3,11,13,14) and glpGLJournalID in (select gllGLJournalID from GLJournalLines where gllJobID = @SourceTableJobID)\r\n\t\t\t\t\tand glpGLJournalID not in (Select Distinct gllGLJournalID From GLJournalLines Where gllTransactionType = 4)");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable24 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter48, sqlTransaction);
				_glJournalsPreSplitSource = _glJournalsPreSplitSource ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter48, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from GLJournalLines where gllGLJournalID in (select gllGLJournalID from GLJournalLines where gllJobID = @SourceTableJobID) and gllTransactionType <> 4");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable25 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter49, sqlTransaction);
				_glJournalLinesPreSplitSource = _glJournalLinesPreSplitSource ?? database.GetDataTable(sqlCommand, fillSchema: true, out adapter49, sqlTransaction);
				GenerateNewIdsForGLJournals(database, sourceTableJobID, assembliesToIgnore, _newGLJournalIdForSourceGLJournalReversal, sourceTableJobAssemblyID, startSeq);
				GenerateNewIdsForGLJournals(database, sourceTableJobID, assembliesToIgnore, _newGLJournalIdForTargetGLJournal, sourceTableJobAssemblyID, startSeq);
				sqlCommand = database.NewSqlCommand("select * from ShipmentLines where smlJobID = @SourceTableJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				database.GetDataTable(sqlCommand, fillSchema: true, out adapter50, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from ShipmentComponents where smoJobID = @SourceTableJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				database.GetDataTable(sqlCommand, fillSchema: true, out adapter51, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from SerialNumberTransactions where sntJobID = @SourceTableJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable26 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter52, sqlTransaction);
				sqlCommand = database.NewSqlCommand("select * from LotNumberTransactions where abtJobID = @SourceTableJobID");
				sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
				dataTable27 = database.GetDataTable(sqlCommand, fillSchema: true, out adapter53, sqlTransaction);
			}
			else
			{
				dataTable10 = database.GetDataTable("select * from PartTransactions where 0=1", fillSchema: true, out adapter34, sqlTransaction);
				dataTable11 = database.GetDataTable("select * from PartTransactionCosts where 0=1", fillSchema: true, out adapter35, sqlTransaction);
				dataTable12 = database.GetDataTable("select * from TimecardLines where 0=1", fillSchema: true, out adapter36, sqlTransaction);
				dataTable13 = database.GetDataTable("select * from PurchaseOrderLines where 0=1", fillSchema: true, out adapter37, sqlTransaction);
				dataTable15 = database.GetDataTable("select * from PurchaseOrderComponents where 0=1", fillSchema: true, out adapter39, sqlTransaction);
				dataTable14 = database.GetDataTable("Select * From PurchaseOrderAccounts Where 0=1", fillSchema: true, out adapter38, sqlTransaction);
				dataTable16 = database.GetDataTable("select * from ReceiptLines where 0=1", fillSchema: true, out adapter40, sqlTransaction);
				dataTable17 = database.GetDataTable("select * from ReceiptComponents where 0=1", fillSchema: true, out adapter41, sqlTransaction);
				dataTable18 = database.GetDataTable("select * from APInvoiceLines where 0=1", fillSchema: true, out adapter42, sqlTransaction);
				dataTable19 = database.GetDataTable("Select * From APInvoiceExpenseAccounts Where 0=1", fillSchema: true, out adapter43, sqlTransaction);
				dataTable20 = database.GetDataTable("Select * From MaterialIssueLines Where 0=1", fillSchema: true, out adapter44, sqlTransaction);
				dataTable21 = database.GetDataTable("Select * From MaterialIssueComponents Where 0=1", fillSchema: true, out adapter45, sqlTransaction);
				dataTable22 = database.GetDataTable("Select * From MfgReceipts Where 0=1", fillSchema: true, out adapter46, sqlTransaction);
				dataTable23 = database.GetDataTable("Select * From MfgReceiptComponents Where 0=1", fillSchema: true, out adapter47, sqlTransaction);
				dataTable24 = database.GetDataTable("Select * From GLJournals Where 0=1", fillSchema: true, out adapter48, sqlTransaction);
				dataTable25 = database.GetDataTable("Select * From GLJournalLines Where 0=1", fillSchema: true, out adapter49, sqlTransaction);
				database.GetDataTable("Select * From ShipmentLines Where 0=1", fillSchema: true, out adapter50, sqlTransaction);
				database.GetDataTable("Select * From ShipmentComponents Where 0=1", fillSchema: true, out adapter51, sqlTransaction);
				dataTable26 = database.GetDataTable("Select * From SerialNumberTransactions Where 0=1", fillSchema: true, out adapter52, sqlTransaction);
				dataTable27 = database.GetDataTable("Select * From LotNumberTransactions Where 0=1", fillSchema: true, out adapter53, sqlTransaction);
			}
			if (removeJobFromSchedule)
			{
				new Job().UnscheduleJob(database, sourceTableJobID, sqlTransaction);
			}
			if (dataTable2.Rows.Count != 0)
			{
				DataRow dataRow = dataTable2.Select("jmaJobAssemblyID = " + sourceTableJobAssemblyID.ToLinq()).SingleOrDefault();
				if (dataRow != null)
				{
					int nPartTransactionID = 0;
					int nSerialNumberTransactionID = 0;
					int nLotNumberTransactionID = 0;
					if (splitCosts != SplitCostOption.KeepCostsOnSourceJob)
					{
						nPartTransactionID = (int)database.ExecuteScalar("Select IsNull(Max(imtPartTransactionID),0)+1 From PartTransactions", sqlTransaction);
						nSerialNumberTransactionID = (int)database.ExecuteScalar("Select IsNull(Max(sntSerialNumberTransactionID),0)+1 From SerialNumberTransactions", sqlTransaction);
						nLotNumberTransactionID = (int)database.ExecuteScalar("Select IsNull(Max(abtLotNumberTransactionID),0)+1 From LotNumberTransactions", sqlTransaction);
					}
					DataRow dataRow2 = dataTable6.AddBlankRow();
					CopyAllFieldsToNewRow(dataRow, dataRow2);
					dataRow2["jmaCreatedBy"] = database.User.ID;
					dataRow2["jmaCreatedDate"] = DateTime.Now;
					dataRow2["jmaJobID"] = jobId;
					dataRow2["jmaParentAssemblyID"] = 0;
					dataRow2["jmaLevel"] = 1;
					dataRow2["jmaJobAssemblyID"] = 0;
					dataRow2["jmaQuantityPerParent"] = 1;
					dataRow2["jmaScheduledStartDate"] = DBNull.Value;
					dataRow2["jmaScheduledDueDate"] = DBNull.Value;
					dataRow2["jmaScheduledStartHour"] = 0;
					dataRow2["jmaScheduledDueHour"] = 0;
					dataRow2["jmaOrderQuantity"] = orderQuantity;
					dataRow2["jmaInventoryQuantity"] = inventoryQuantity;
					dataRow2["jmaScrapQuantity"] = scrapQuantity;
					dataRow2["jmaReworkQuantity"] = 0;
					dataRow2["jmaProductionQuantity"] = productionQuantity;
					dataRow2["jmaQuantityToMake"] = productionQuantity;
					dataRow2["jmaQuantityToPull"] = 0;
					dataRow2["jmaQuantityCompleted"] = 0;
					dataRow2["jmaQuantityReceivedToInventory"] = 0;
					dataRow2["jmaQuantityIssued"] = 0;
					dataRow2["jmaScrapQuantityCompleted"] = 0;
					dataRow2["jmaQuantityToInspect"] = 0;
					dataRow2["jmaQuantityToReturn"] = 0;
					double num2 = ((Convert.ToDouble(dataRow["jmaProductionQuantity"]) == 0.0) ? 1.0 : (productionQuantity / Convert.ToDouble(dataRow["jmaProductionQuantity"])));
					if (num2 > 1.0)
					{
						num2 = 1.0;
					}
					double nSourceTablePercent = Math.Max(1.0 - num2, 0.0);
					DataRow dataRow3 = dataTable.AddBlankRow();
					if (sourceTableJobAssemblyID == 0)
					{
						CopyAllFieldsToNewRow(dataTable.Rows[0], dataRow3);
						dataRow3["jmpCreatedBy"] = database.User.ID;
						dataRow3["jmpCreatedDate"] = DateTime.Now;
						dataRow3["jmpJobID"] = jobId;
						dataRow3["jmpOrderQuantity"] = orderQuantity;
						dataRow3["jmpInventoryQuantity"] = inventoryQuantity;
						dataRow3["jmpScrapQuantity"] = scrapQuantity;
						dataRow3["jmpReworkQuantity"] = 0;
						dataRow3["jmpReworkDate"] = DBNull.Value;
						dataRow3["jmpProductionQuantity"] = productionQuantity;
						dataRow3["jmpScheduledStartDate"] = DBNull.Value;
						dataRow3["jmpScheduledDueDate"] = DBNull.Value;
						dataRow3["jmpScheduledStartHour"] = 0;
						dataRow3["jmpScheduledDueHour"] = 0;
						dataRow3["jmpScheduleComplete"] = false;
						dataRow3["jmpScheduleLocked"] = false;
						dataRow3["jmpReleasedToFloor"] = false;
						dataRow3["jmpReadyToPrint"] = true;
						dataRow3["jmpQuantityReceivedToInventory"] = 0;
						dataRow3["jmpScrapQuantityCompleted"] = 0;
						dataRow3["jmpQuantityCompleted"] = 0;
						dataRow3["jmpQuantityShipped"] = 0;
						dataRow3["jmpCompletedDate"] = DBNull.Value;
						dataRow3["jmpProductionDueDate"] = (Convert.IsDBNull(productionDueDate) ? DBNull.Value : productionDueDate);
						if (!_sourceJobUpdated)
						{
							_sourceJobUpdated = true;
							dataTable.Rows[0]["jmpOrderQuantity"] = Math.Round(Convert.ToDouble(dataTable.Rows[0]["jmpOrderQuantity"]) - totalOrderQuantity, database.Props("DS").Field<byte>("xadSellQuantityDecimals"));
							dataTable.Rows[0]["jmpInventoryQuantity"] = Math.Round(Convert.ToDouble(dataTable.Rows[0]["jmpInventoryQuantity"]) - totalInventoryQuantity, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
							dataTable.Rows[0]["jmpScrapQuantity"] = Math.Round(Convert.ToDouble(dataTable.Rows[0]["jmpScrapQuantity"]) - totalScrapQuantity, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
							dataTable.Rows[0]["jmpProductionQuantity"] = Math.Max(Convert.ToDouble(dataTable.Rows[0]["jmpProductionQuantity"]) - totalProductionQuantity, 0.0);
							dataRow["jmaProductionQuantity"] = dataTable.Rows[0]["jmpProductionQuantity"];
							dataRow["jmaOrderQuantity"] = Math.Round(Convert.ToDouble(dataRow["jmaOrderQuantity"]) - totalOrderQuantity, database.Props("DS").Field<byte>("xadSellQuantityDecimals"));
							dataRow["jmaInventoryQuantity"] = Math.Round(Convert.ToDouble(dataRow["jmaInventoryQuantity"]) - totalInventoryQuantity, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
							dataRow["jmaScrapQuantity"] = Math.Round(Convert.ToDouble(dataRow["jmaScrapQuantity"]) - totalScrapQuantity, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
							dataRow["jmaQuantityToMake"] = dataRow["jmaProductionQuantity"];
						}
					}
					else
					{
						DataRow dataRow4 = dataTable2.Select("jmaJobAssemblyID = " + dataRow["jmaParentAssemblyID"].ToLinq()).SingleOrDefault();
						CopyAllFieldsToNewRow(dataTable.Rows[0], dataRow3);
						dataRow3["jmpJobID"] = jobId;
						dataRow3["jmpOrderQuantity"] = orderQuantity;
						dataRow3["jmpInventoryQuantity"] = inventoryQuantity;
						dataRow3["jmpScrapQuantity"] = scrapQuantity;
						dataRow3["jmpReworkQuantity"] = 0;
						dataRow3["jmpProductionQuantity"] = productionQuantity;
						dataRow3["jmpQuantityShipped"] = 0;
						dataRow3["jmpQuantityReceivedToInventory"] = 0;
						dataRow3["jmpScrapQuantityCompleted"] = 0;
						dataRow3["jmpQuantityCompleted"] = 0;
						dataRow3["jmpClosedDate"] = DBNull.Value;
						dataRow3["jmpCompletedDate"] = DBNull.Value;
						dataRow3["jmpScheduledStartDate"] = DBNull.Value;
						dataRow3["jmpScheduledDueDate"] = DBNull.Value;
						dataRow3["jmpReworkDate"] = DBNull.Value;
						dataRow3["jmpScheduledStartHour"] = 0;
						dataRow3["jmpScheduledDueHour"] = 0;
						dataRow3["jmpScheduleComplete"] = false;
						dataRow3["jmpScheduleLocked"] = false;
						dataRow3["jmpReleasedToFloor"] = false;
						dataRow3["jmpReadyToPrint"] = true;
						dataRow3["jmpClosed"] = false;
						dataRow3["jmpProductionDueDate"] = (Convert.IsDBNull(productionDueDate) ? DBNull.Value : productionDueDate);
						dataRow3["jmpPartID"] = dataRow2["jmaPartID"];
						dataRow3["jmpPartRevisionID"] = dataRow2["jmaPartRevisionID"];
						dataRow3["jmpUnitOfMeasure"] = dataRow2["jmaUnitOfMeasure"];
						dataRow3["jmpPartShortDescription"] = dataRow2["jmaPartShortDescription"];
						dataRow3["jmpPartLongDescriptionRTF"] = dataRow2["jmaPartLongDescriptionRTF"];
						dataRow3["jmpPartLongDescriptionText"] = dataRow2["jmaPartLongDescriptionText"];
						dataRow3["jmpProductionNotesRTF"] = dataRow2["jmaProductionNotesRTF"];
						dataRow3["jmpProductionNotesText"] = dataRow2["jmaProductionNotesText"];
						dataRow3["jmpDocuments"] = dataRow2["jmaDocuments"];
						if (!_sourceJobAssemblyUpdated)
						{
							_sourceJobAssemblyUpdated = true;
							double num3 = Convert.ToDouble(dataRow4["jmaProductionQuantity"]);
							dataRow["jmaProductionQuantity"] = Convert.ToDouble(dataRow["jmaProductionQuantity"]) - totalProductionQuantity;
							dataRow["jmaOrderQuantity"] = Math.Round(Convert.ToDouble(dataRow["jmaOrderQuantity"]) - totalOrderQuantity, database.Props("DS").Field<byte>("xadSellQuantityDecimals"));
							dataRow["jmaInventoryQuantity"] = Math.Round(Convert.ToDouble(dataRow["jmaInventoryQuantity"]) - totalInventoryQuantity, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
							dataRow["jmaScrapQuantity"] = Math.Round(Convert.ToDouble(dataRow["jmaScrapQuantity"]) - totalScrapQuantity, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
							if (Convert.ToDouble(dataRow["jmaQuantityPerParent"]) != 0.0 && Convert.ToDouble(dataRow["jmaProductionQuantity"]) != 0.0)
							{
								dataRow["jmaQuantityPerParent"] = Math.Round(Convert.ToDouble(dataRow["jmaOrderQuantity"]) / num3, 5);
							}
							if (Convert.ToDouble(dataRow["jmaQuantityToMake"]) > 0.0)
							{
								if (Convert.ToDouble(dataRow["jmaQuantityToPull"]) > 0.0)
								{
									dataRow["jmaQuantityToMake"] = Math.Round(Convert.ToDouble(dataRow["jmaProductionQuantity"]) - Convert.ToDouble(dataRow["jmaQuantityToPull"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
								}
								else
								{
									dataRow["jmaQuantityToMake"] = Math.Round(Convert.ToDouble(dataRow["jmaProductionQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
								}
							}
							else
							{
								dataRow["jmaQuantityToMake"] = 0;
								dataRow["jmaQuantityToPull"] = Convert.ToDouble(dataRow["jmaProductionQuantity"]);
							}
						}
					}
					AddJobSplitLogRecord(dataTable47, database.User.ID, "Jobs", dataTable.Rows[0]["jmpUniqueID"], "Jobs", dataRow3["jmpUniqueID"], productionQuantity, splitCosts, productionDueDate, num);
					AddJobSplitLogLineRecord(database, sqlTransaction, dataTable48, database.User.ID, "JobAssemblies", dataRow["jmaUniqueID"], "JobAssemblies", dataRow2["jmaUniqueID"], num);
					SplitGLJournals(database, sqlTransaction, sourceTableJobID, jobId, initialDestPercent, assembliesToIgnore, startSeq, num, splitCosts, dataTable24, dataTable25, dataTable48, dataTable42, dataTable43, new Dictionary<string, string>());
					SplitAndTransferJobAssembly(database, sqlTransaction, dataTable10, dataTable11, dataTable12, dataTable16, dataTable17, dataTable18, dataTable19, dataTable20, dataTable21, dataTable22, dataTable23, dataTable25, dataTable24, dataTable26, dataTable27, dataRow2, dataTable28, dataTable29, dataTable30, dataTable34, dataTable35, dataTable36, dataTable37, dataTable38, dataTable39, dataTable40, dataTable41, dataTable43, dataTable42, dataTable45, dataTable46, dataRow, Convert.ToInt32(dataRow["jmaJobAssemblyID"]), jobId, splitCosts, nSourceTablePercent, num2, ref nPartTransactionID, ref nSerialNumberTransactionID, ref nLotNumberTransactionID, dataTable48, num, initialDestPercent, sourcePercent, sourceTableJobID, assembliesToIgnore, startSeq);
					SqlCommand sqlCommand2 = database.NewSqlCommand("INSERT INTO FormInputValues (xaiFormID,xaiControlName,xaiValue,xaiSourceUniqueID,xaiSourceTable,xaiParentFormID,xaiTopLevelFormID,xaiLastRunDate) SELECT xaiFormID,xaiControlName,xaiValue,@NewJobUniqueID,xaiSourceTable,xaiParentFormID,xaiTopLevelFormID,CURRENT_TIMESTAMP FROM FormInputValues F Where F.xaiSourceUniqueID = @OriginalJobUniqueID");
					sqlCommand2.Parameters.Add(new SqlParameter("@OriginalJobUniqueID", SqlDbType.UniqueIdentifier)).Value = dataTable.Rows[0]["jmpUniqueID"];
					sqlCommand2.Parameters.Add(new SqlParameter("@NewJobUniqueID", SqlDbType.UniqueIdentifier)).Value = dataRow3["jmpUniqueID"];
					database.ExecuteCommand(sqlCommand2, sqlTransaction);
					if (SplitNextJobAssembly(database, sqlTransaction, dataTable2, dataTable3, dataTable4, dataTable5, dataTable10, dataTable11, dataTable12, dataTable13, dataTable14, dataTable15, dataTable16, dataTable17, dataTable18, dataTable19, dataTable20, dataTable21, dataTable22, dataTable23, dataTable25, dataTable24, dataTable26, dataTable27, dataTable6, dataTable7, dataTable8, dataTable9, dataTable28, dataTable29, dataTable30, dataTable31, dataTable32, dataTable33, dataTable34, dataTable35, dataTable36, dataTable37, dataTable38, dataTable39, dataTable40, dataTable41, dataTable43, dataTable42, dataTable45, dataTable46, Convert.ToInt32(dataRow["jmaJobAssemblyID"]), Convert.ToDouble(dataRow["jmaQuantityToMake"]), startSeq, jobId, 0, 1, Convert.ToDouble(dataTable6.Rows[0]["jmaQuantityToMake"]), splitCosts, nSourceTablePercent, num2, ref nPartTransactionID, ref nSerialNumberTransactionID, ref nLotNumberTransactionID, oAssembliesToIgnore, dataTable48, num, initialDestPercent, sourcePercent, sourceTableJobID, assembliesToIgnore))
					{
						if (sourceTableJobAssemblyID == 0)
						{
							CopySalesOrderJobLinkToTargetJob(database, sourceTableJobID, sqlTransaction, jobId, dataTable48, num, out salesOrderJobLinksDestTable, out soJobLinksDestDataAdapter);
							CopyJobMemosIntoTargetJob(database, sourceTableJobID, sqlTransaction, jobId, dataTable48, num, out jobMemosDestTable, out jobMemosDestDataAdapter);
							CopyAttachmentsIntoTargetJob(database, sourceTableJobID, sqlTransaction, jobId, dataTable48, num, out attachmentsDestTable, out attachmentsDestDataAdapter);
						}
						database.UpdateData(dataTable, adapter, sqlTransaction);
						database.UpdateData(dataTable6, adapter2, sqlTransaction);
						database.UpdateData(dataTable7, adapter3, sqlTransaction);
						database.UpdateData(dataTable8, adapter4, sqlTransaction);
						database.UpdateData(dataTable9, adapter5, sqlTransaction);
						if (splitCosts != SplitCostOption.KeepCostsOnSourceJob)
						{
							database.UpdateData(dataTable28, adapter6, sqlTransaction);
							database.UpdateData(dataTable29, adapter7, sqlTransaction);
							database.UpdateData(dataTable30, adapter8, sqlTransaction);
							database.UpdateData(dataTable31, adapter9, sqlTransaction);
							database.UpdateData(dataTable32, adapter11, sqlTransaction);
							database.UpdateData(dataTable33, adapter10, sqlTransaction);
							database.UpdateData(dataTable34, adapter12, sqlTransaction);
							database.UpdateData(dataTable35, adapter13, sqlTransaction);
							database.UpdateData(dataTable36, adapter14, sqlTransaction);
							database.UpdateData(dataTable37, adapter15, sqlTransaction);
							database.UpdateData(dataTable38, adapter16, sqlTransaction);
							database.UpdateData(dataTable39, adapter17, sqlTransaction);
							database.UpdateData(dataTable40, adapter18, sqlTransaction);
							database.UpdateData(dataTable41, adapter19, sqlTransaction);
							database.UpdateData(dataTable42, adapter20, sqlTransaction);
							database.UpdateData(dataTable43, adapter21, sqlTransaction);
							database.UpdateData(dataTable45, adapter24, sqlTransaction);
							database.UpdateData(dataTable46, adapter25, sqlTransaction);
						}
						if (sourceTableJobAssemblyID == 0)
						{
							database.UpdateData(salesOrderJobLinksDestTable, soJobLinksDestDataAdapter, sqlTransaction);
							database.UpdateData(jobMemosDestTable, jobMemosDestDataAdapter, sqlTransaction);
							database.UpdateData(attachmentsDestTable, attachmentsDestDataAdapter, sqlTransaction);
						}
						database.UpdateData(dataTable, adapter26, sqlTransaction);
						database.UpdateData(dataTable2, adapter27, sqlTransaction);
						database.UpdateData(dataTable3, adapter28, sqlTransaction);
						database.UpdateData(dataTable4, adapter30, sqlTransaction);
						database.UpdateData(dataTable5, adapter31, sqlTransaction);
						if (splitCosts != SplitCostOption.KeepCostsOnSourceJob)
						{
							database.UpdateData(dataTable10, adapter34, sqlTransaction);
							database.UpdateData(dataTable11, adapter35, sqlTransaction);
							database.UpdateData(dataTable12, adapter36, sqlTransaction);
							database.UpdateData(dataTable13, adapter37, sqlTransaction);
							database.UpdateData(dataTable15, adapter39, sqlTransaction);
							database.UpdateData(dataTable14, adapter38, sqlTransaction);
							database.UpdateData(dataTable16, adapter40, sqlTransaction);
							database.UpdateData(dataTable17, adapter41, sqlTransaction);
							database.UpdateData(dataTable18, adapter42, sqlTransaction);
							database.UpdateData(dataTable19, adapter43, sqlTransaction);
							database.UpdateData(dataTable20, adapter44, sqlTransaction);
							database.UpdateData(dataTable21, adapter45, sqlTransaction);
							database.UpdateData(dataTable22, adapter46, sqlTransaction);
							database.UpdateData(dataTable23, adapter47, sqlTransaction);
							database.UpdateData(dataTable24, adapter48, sqlTransaction);
							database.UpdateData(dataTable25, adapter49, sqlTransaction);
							database.UpdateData(dataTable26, adapter52, sqlTransaction);
							database.UpdateData(dataTable27, adapter53, sqlTransaction);
						}
						database.UpdateData(dataTable47, adapter32, sqlTransaction);
						database.UpdateData(dataTable48, adapter33, sqlTransaction);
						if (!(string.IsNullOrEmpty((string)dataRow2["jmaSourceMethodID"]) & string.IsNullOrEmpty((string)dataRow2["jmaSourceRevisionID"])))
						{
							if ((short)dataRow["jmaLevel"] != (short)dataRow2["jmaLevel"] && (short)dataRow2["jmaLevel"] == 1)
							{
								SqlCommand sqlCommand3 = database.NewSqlCommand("INSERT INTO FormInputValues (xaiFormID,xaiControlName,xaiValue,xaiSourceUniqueID,xaiSourceTable,xaiParentFormID,xaiTopLevelFormID,xaiLastRunDate) SELECT xaiFormID,xaiControlName,xaiValue,@NewJobUniqueID,'JOBS',xaiParentFormID,xaiTopLevelFormID,CURRENT_TIMESTAMP FROM FormInputValues F Where F.xaiSourceUniqueID = @OriginalJobUniqueID");
								sqlCommand3.Parameters.Add(new SqlParameter("@OriginalJobUniqueID", SqlDbType.UniqueIdentifier)).Value = dataRow["jmaUniqueID"];
								sqlCommand3.Parameters.Add(new SqlParameter("@NewJobUniqueID", SqlDbType.UniqueIdentifier)).Value = dataRow3["jmpUniqueID"];
								database.ExecuteCommand(sqlCommand3, sqlTransaction);
							}
							else
							{
								SqlCommand sqlCommand4 = database.NewSqlCommand("INSERT INTO FormInputValues (xaiFormID,xaiControlName,xaiValue,xaiSourceUniqueID,xaiSourceTable,xaiParentFormID,xaiTopLevelFormID,xaiLastRunDate) SELECT xaiFormID,xaiControlName,xaiValue,@NewJobUniqueID,xaiSourceTable,xaiParentFormID,xaiTopLevelFormID,CURRENT_TIMESTAMP FROM FormInputValues F Where F.xaiSourceUniqueID = @OriginalJobUniqueID");
								sqlCommand4.Parameters.Add(new SqlParameter("@OriginalJobUniqueID", SqlDbType.UniqueIdentifier)).Value = dataRow["jmaUniqueID"];
								sqlCommand4.Parameters.Add(new SqlParameter("@NewJobUniqueID", SqlDbType.UniqueIdentifier)).Value = dataRow2["jmaUniqueID"];
								database.ExecuteCommand(sqlCommand4, sqlTransaction);
							}
						}
						new Part().RefreshPartAllocations(database, sqlTransaction);
						database.CommitTransaction(sqlTransaction);
						result = true;
					}
				}
			}
			return result;
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	public void CopySalesOrderJobLinkToTargetJob(M1Database database, string sourceTableJobID, SqlTransaction transaction, string newJobId, DataTable jobSplitLogLinesTable, int nJobSplitLogID, out DataTable salesOrderJobLinksDestTable, out SqlDataAdapter soJobLinksDestDataAdapter)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select * from SalesOrderJobLinks where omjJobID = @SourceTableJobID");
		sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: true, out adapter, transaction);
		salesOrderJobLinksDestTable = database.GetDataTable("select * from SalesOrderJobLinks where 0=1", fillSchema: true, out soJobLinksDestDataAdapter, transaction);
		salesOrderJobLinksDestTable.DefaultView.Sort = "omjSalesOrderJobLinkID ASC";
		if (dataTable.Rows.Count != 0)
		{
			dataTable.Rows[0]["omjLinkType"] = 3;
			dataTable.Rows[0]["omjSalesOrderDeliveryID"] = 0;
			database.UpdateData(dataTable, adapter, transaction);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				int num = (int)database.ExecuteScalar("select Max(omjSalesOrderJobLinkID) as omjSalesOrderJobLinkID from SalesOrderJobLinks where omjSalesOrderID = " + dataTable.Rows[i]["omjSalesOrderID"].ToSql() + " and omjSalesOrderLineID = " + dataTable.Rows[i]["omjSalesOrderLineID"].ToSql(), transaction);
				DataRow dataRow = salesOrderJobLinksDestTable.AddBlankRow();
				CopyAllFieldsToNewRow(dataTable.Rows[i], dataRow);
				dataRow["omjCreatedBy"] = database.User.ID;
				dataRow["omjCreatedDate"] = DateTime.Now;
				dataRow["omjJobID"] = newJobId;
				dataRow["omjSalesOrderJobLinkID"] = num + 1;
				dataRow["omjLinkType"] = 3;
				dataRow["omjSalesOrderDeliveryID"] = 0;
				AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "SalesOrderJobLinks", dataTable.Rows[0]["omjUniqueID"], "SalesOrderJobLinks", dataRow["omjUniqueID"], nJobSplitLogID);
			}
		}
	}

	public void CopyAttachmentsIntoTargetJob(M1Database database, string sourceTableJobID, SqlTransaction transaction, string newJobId, DataTable jobSplitLogLinesTable, int nJobSplitLogID, out DataTable attachmentsDestTable, out SqlDataAdapter attachmentsDestDataAdapter)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select * from Attachments where cmaJobID = @SourceTableJobID");
		sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		attachmentsDestTable = database.GetDataTable("select * from Attachments where 0=1", fillSchema: true, out attachmentsDestDataAdapter, transaction);
		foreach (DataRow row in dataTable.Rows)
		{
			DataRow dataRow2 = attachmentsDestTable.AddBlankRow();
			CopyAllFieldsToNewRow(row, dataRow2);
			dataRow2["cmaAttachmentID"] = database.NextIDs.GetNextIDForTable("Attachments", null, transaction);
			dataRow2["cmaJobID"] = newJobId;
			dataRow2["cmaCreatedBy"] = database.User.ID;
			dataRow2["cmaCreatedDate"] = DateTime.Now;
			AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "Attachments", row["cmaUniqueID"], "Attachments", dataRow2["cmaUniqueID"], nJobSplitLogID);
		}
	}

	public void CopyJobMemosIntoTargetJob(M1Database database, string sourceTableJobID, SqlTransaction transaction, string newJobId, DataTable jobSplitLogLinesTable, int nJobSplitLogID, out DataTable jobMemosDestTable, out SqlDataAdapter jobMemosDestDataAdapter)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select * from JobMemos where jmkJobID = @SourceTableJobID");
		sqlCommand.Parameters.Add(new SqlParameter("@SourceTableJobID", SqlDbType.NVarChar)).Value = sourceTableJobID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		jobMemosDestTable = database.GetDataTable("select * from JobMemos where 0=1", fillSchema: true, out jobMemosDestDataAdapter, transaction);
		foreach (DataRow row in dataTable.Rows)
		{
			DataRow dataRow2 = jobMemosDestTable.AddBlankRow();
			CopyAllFieldsToNewRow(row, dataRow2);
			dataRow2["jmkJobID"] = newJobId;
			dataRow2["jmkCreatedBy"] = database.User.ID;
			dataRow2["jmkCreatedDate"] = DateTime.Now;
			AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "JobMemos", row["jmkUniqueID"], "JobMemos", dataRow2["jmkUniqueID"], nJobSplitLogID);
		}
	}

	private bool SplitAndTransferJobAssembly(M1Database database, SqlTransaction transaction, DataTable partTransactionsSourceTable, DataTable partTransactionsCostsSourceTable, DataTable timecardLinesSourceTable, DataTable receiptLinesSourceTable, DataTable receiptComponentsSourceTable, DataTable aPInvoiceLinesSourceTable, DataTable aPInvoiceExpenseAccountsSourceTable, DataTable materialIssueLinesSourceTable, DataTable materialIssueComponentsSourceTable, DataTable mfgReceiptsSourceTable, DataTable mfgReceiptsComponentsSourceTable, DataTable glJournalLinesSourceTable, DataTable glJournalsSourceTable, DataTable serialNumberTransactionsSourceTable, DataTable lotNumberTransactionsSourceTable, DataRow jobAssembliesDestTableRow, DataTable partTransactionsDestTable, DataTable partTransactionsCostsDestTable, DataTable timecardLinesDestTable, DataTable receiptLinesDestTable, DataTable receiptComponentsDestTable, DataTable aPInvoiceLinesDestTable, DataTable aPInvoiceExpenseAccountsDestTable, DataTable materialIssueLinesDestTable, DataTable materialIssueComponentsDestTable, DataTable mfgReceiptsDestTable, DataTable mfgReceiptsComponentsDestTable, DataTable glJournalLinesDestTable, DataTable glJournalsDestTable, DataTable serialNumberTransactionsDestTable, DataTable lotNumberTransactionsDestTable, DataRow jobAssemblySourceTable, int nParentAsm, string cNewJobID, SplitCostOption nSplitCosts, double nSourceTablePercent, double nDestPercent, ref int nPartTransactionID, ref int nSerialNumberTransactionID, ref int nLotNumberTransactionID, DataTable jobSplitLogLinesTable, int nJobSplitLogID, double initialDestPercent, double sourcePercent, string sourceTableJobID, List<int> assembliesToIgnore, int startSequence)
	{
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary6 = new Dictionary<string, string>();
			if (nSplitCosts != SplitCostOption.KeepCostsOnSourceJob && initialDestPercent > 0.0)
			{
				dictionary5.Clear();
				dictionary.Clear();
				dictionary2.Clear();
				dictionary6.Clear();
				DataRow[] array = receiptLinesSourceTable.Select("rmlJobAssemblyID = " + nParentAsm.ToLinq() + " and rmlJobMaterialID = 0 and rmlJobOperationID = 0");
				foreach (DataRow dataRow in array)
				{
					DataRow initialReceiptLinesSourceRow = _initialReceiptLinesSourceTable.Select("rmlReceiptID = " + dataRow["rmlReceiptID"].ToLinq() + " And rmlReceiptLineID = " + dataRow["rmlReceiptLineID"].ToLinq()).FirstOrDefault();
					DataRow[] foundReceiptComponentsSourceRows = receiptComponentsSourceTable.Select("rmoReceiptID = " + dataRow["rmlReceiptID"].ToLinq() + " And rmoReceiptLineID = " + dataRow["rmlReceiptLineID"].ToLinq());
					DataRow[] initialFoundReceiptComponentsSourceRows = _initialReceiptComponentsSourceTable.Select("rmoReceiptID = " + dataRow["rmlReceiptID"].ToLinq() + " And rmoReceiptLineID = " + dataRow["rmlReceiptLineID"].ToLinq());
					SplitAndTransferReceiptLines(database, transaction, initialReceiptLinesSourceRow, dataRow, receiptLinesDestTable, initialFoundReceiptComponentsSourceRows, foundReceiptComponentsSourceRows, receiptComponentsDestTable, jobAssembliesDestTableRow["jmaJobID"].ToString(), Convert.ToInt32(jobAssembliesDestTableRow["jmaJobAssemblyID"]), nSourceTablePercent, initialDestPercent, dictionary, dictionary2, dictionary5, dictionary6, jobSplitLogLinesTable, nJobSplitLogID, nSplitCosts);
				}
				ProcessReversals(receiptLinesDestTable, receiptComponentsDestTable, dictionary6, new ReversalFieldNames
				{
					LineIdFieldName = "rmlReverseReceiptID",
					LineLineFieldName = "rmlReverseReceiptLineID",
					ComponentIdFieldName = "rmoReverseReceiptID",
					ComponentLineFieldName = "rmoReverseReceiptLineID",
					ComponentComponentFieldName = "rmoReverseReceiptComponentID"
				});
				array = aPInvoiceLinesSourceTable.Select("aplJobAssemblyID = " + nParentAsm.ToLinq() + " and aplJobMaterialID = 0 and aplJobOperationID = 0");
				foreach (DataRow dataRow2 in array)
				{
					DataRow apInvoiceLinesPreSplitSource = _apInvoiceLinesPreSplitSource.Select("aplAPInvoiceID = " + dataRow2["aplAPInvoiceID"].ToLinq() + " and aplAPInvoiceLineID = " + dataRow2["aplAPInvoiceLineID"].ToLinq()).FirstOrDefault();
					DataRow[] foundApInvoiceExpenseAccountsRowsSource = aPInvoiceExpenseAccountsSourceTable.Select("apxAPInvoiceID = " + Convert.ToString(dataRow2["aplAPInvoiceID"]).ToLinq() + " And apxAPInvoiceLineID = " + Convert.ToDouble(dataRow2["aplAPInvoiceLineID"]).ToLinq());
					SplitAndTransferAPInvoiceLines(database, transaction, apInvoiceLinesPreSplitSource, dataRow2, aPInvoiceLinesDestTable, foundApInvoiceExpenseAccountsRowsSource, aPInvoiceExpenseAccountsDestTable, jobAssembliesDestTableRow["jmaJobID"].ToString(), Convert.ToInt32(jobAssembliesDestTableRow["jmaJobAssemblyID"]), nSourceTablePercent, initialDestPercent, nSplitCosts, dictionary, dictionary2, dictionary5, jobSplitLogLinesTable, nJobSplitLogID);
				}
				dictionary3.Clear();
				array = materialIssueLinesSourceTable.Select(string.Format("injJobAssemblyID = {0} and injJobMaterialID = 0", jobAssembliesDestTableRow.Field<int>("jmaJobAssemblyID")));
				foreach (DataRow dataRow3 in array)
				{
					DataRow[] foundMaterialIssueComponentsSourceRows = materialIssueComponentsSourceTable.Select("inkMaterialIssueID = " + dataRow3["injMaterialIssueID"].ToLinq() + " And inkMaterialIssueLineID = " + dataRow3["injMaterialIssueLineID"].ToLinq());
					SplitAndTransferMaterialIssueLines(database, transaction, dataRow3, materialIssueLinesDestTable, foundMaterialIssueComponentsSourceRows, materialIssueComponentsDestTable, jobAssembliesDestTableRow["jmaJobID"].ToString(), Convert.ToInt32(jobAssembliesDestTableRow["jmaJobAssemblyID"]), nSourceTablePercent, initialDestPercent, nDestPercent, dictionary5, dictionary3, jobSplitLogLinesTable, nJobSplitLogID, nSplitCosts);
				}
				ProcessReversals(materialIssueLinesDestTable, materialIssueComponentsDestTable, dictionary3, new ReversalFieldNames
				{
					LineIdFieldName = "injReverseMaterialIssueID",
					LineLineFieldName = "injReverseMaterialIssueLineID",
					ComponentIdFieldName = "inkReverseMaterialIssueID",
					ComponentLineFieldName = "inkReverseMaterialIssueLineID",
					ComponentComponentFieldName = "inkReverseMaterialIssueCompID"
				});
				int num = Convert.ToInt32(jobAssemblySourceTable["jmaJobAssemblyID"]);
				if (num != 0)
				{
					dictionary4.Clear();
					DataRow[] array2 = mfgReceiptsSourceTable.Select("rmmJobAssemblyID = " + num.ToLinq() + " and rmmJobMaterialID = 0 and rmmJobOperationID = 0");
					DataRow[] source = _mfgReceiptPreSplitSourceTable.Select("rmmJobAssemblyID = " + num.ToLinq() + " and rmmJobMaterialID = 0 and rmmJobOperationID = 0");
					array = array2;
					foreach (DataRow mfgReceiptSourceRow in array)
					{
						DataRow preSplitMfgReceiptSourceRow = source.FirstOrDefault((DataRow row) => Convert.ToInt32(row["rmmMfgReceiptID"]) == Convert.ToInt32(mfgReceiptSourceRow["rmmMfgReceiptID"]));
						DataRow[] foundMfgReceiptComponentsSourceRows = mfgReceiptsComponentsSourceTable.Select("rmnMfgReceiptID = " + mfgReceiptSourceRow["rmmMfgReceiptID"].ToLinq());
						DataRow[] initialFoundMfgReceiptComponentsSourceRows = _mfgReceiptComponentsPreSplitSourceTable.Select("rmnMfgReceiptID = " + mfgReceiptSourceRow["rmmMfgReceiptID"].ToLinq());
						SplitAndTransferMfgReceipts(database, transaction, preSplitMfgReceiptSourceRow, mfgReceiptSourceRow, mfgReceiptsDestTable, initialFoundMfgReceiptComponentsSourceRows, foundMfgReceiptComponentsSourceRows, mfgReceiptsComponentsDestTable, jobAssembliesDestTableRow["jmaJobID"].ToString(), Convert.ToInt32(jobAssembliesDestTableRow["jmaJobAssemblyID"]), initialDestPercent, dictionary5, dictionary4, dictionary, jobSplitLogLinesTable, nJobSplitLogID, nSplitCosts);
					}
					ProcessReversals(mfgReceiptsDestTable, mfgReceiptsComponentsDestTable, dictionary4, new ReversalFieldNames
					{
						LineIdFieldName = "rmmReverseMfgReceiptID",
						LineLineFieldName = "",
						ComponentIdFieldName = "rmnReverseMfgReceiptID",
						ComponentLineFieldName = "",
						ComponentComponentFieldName = "rmnReverseMfgReceiptCompID"
					});
				}
				array = serialNumberTransactionsSourceTable.Select("sntJobAssemblyID = " + nParentAsm.ToLinq() + " and sntJobMaterialID = 0");
				foreach (DataRow serialNumberTransactionsSourceTableRow in array)
				{
					SplitAndTransferSerialNumberTransactions(database, transaction, serialNumberTransactionsSourceTableRow, serialNumberTransactionsDestTable, jobAssembliesDestTableRow["jmaJobID"].ToString(), Convert.ToInt32(jobAssembliesDestTableRow["jmaJobAssemblyID"]), nSourceTablePercent, nDestPercent, dictionary5, ref nSerialNumberTransactionID, jobSplitLogLinesTable, nJobSplitLogID);
				}
				array = lotNumberTransactionsSourceTable.Select("abtJobAssemblyID = " + nParentAsm.ToLinq() + " and abtJobMaterialID = 0");
				foreach (DataRow lotNumberTransactionsSourceTableRow in array)
				{
					SplitAndTransferLotNumberTransactions(database, transaction, lotNumberTransactionsSourceTableRow, lotNumberTransactionsDestTable, jobAssembliesDestTableRow["jmaJobID"].ToString(), Convert.ToInt32(jobAssembliesDestTableRow["jmaJobAssemblyID"]), nSourceTablePercent, nDestPercent, dictionary5, ref nLotNumberTransactionID, jobSplitLogLinesTable, nJobSplitLogID);
				}
				array = partTransactionsSourceTable.Select("imtJobAssemblyID = " + nParentAsm.ToLinq() + " and imtJobMaterialID = 0 and imtJobOperationID = 0 AND ((imtTransactionType = 1 AND (imtSource = 2 OR imtSource = 3)) OR (imtTransactionType = 2 AND imtSource = 3))");
				foreach (DataRow dataRow4 in array)
				{
					DataRow partTransactionPreSplitSource = _partTransactionsPreSplitSource.Select("imtUniqueID = " + dataRow4["imtUniqueID"].ToLinq()).FirstOrDefault();
					DataRow[] partTransactionsCostsSourceTableRows = partTransactionsCostsSourceTable.Select("intPartTransactionID = " + dataRow4["imtPartTransactionID"].ToLinq());
					SplitAndTransferPartTransactions(database, transaction, partTransactionPreSplitSource, dataRow4, partTransactionsDestTable, partTransactionsCostsSourceTableRows, partTransactionsCostsDestTable, glJournalLinesDestTable, jobAssembliesDestTableRow["jmaJobID"].ToString(), Convert.ToInt32(jobAssembliesDestTableRow["jmaJobAssemblyID"]), initialDestPercent, ref nPartTransactionID, dictionary5, jobSplitLogLinesTable, nJobSplitLogID, nSplitCosts);
				}
			}
			return true;
		}
		catch
		{
			throw;
		}
	}

	private bool SplitNextJobAssembly(M1Database database, SqlTransaction transaction, DataTable jobAssembliesSourceTable, DataTable jobMaterialsSourceTable, DataTable jobMaterialComponentsSourceTable, DataTable jobOperationsSourceTable, DataTable partTransactionsSourceTable, DataTable partTransactionsCostsSourceTable, DataTable timecardLinesSourceTable, DataTable purchaseOrderLinesSourceTable, DataTable purchaseOrderAccountsSourceTable, DataTable purchaseOrderComponentsSourceTable, DataTable receiptLinesSourceTable, DataTable receiptComponentsSourceTable, DataTable aPInvoiceLinesSourceTable, DataTable aPInvoiceExpenseAccountsSourceTable, DataTable materialIssueLinesSourceTable, DataTable materialIssueComponentsSourceTable, DataTable mfgReceiptsSourceTable, DataTable mfgReceiptsComponentsSourceTable, DataTable glJournalLinesSourceTable, DataTable glJournalsSourceTable, DataTable serialNumberTransactionsSourceTable, DataTable lotNumberTransactionsSourceTable, DataTable jobAssembliesDestTable, DataTable jobMaterialsDestTable, DataTable jobMaterialComponentsDestTable, DataTable jobOperationsDestTable, DataTable partTransactionsDestTable, DataTable partTransactionsCostsDestTable, DataTable timecardLinesDestTable, DataTable purchaseOrderLinesDestTable, DataTable rsPurchaseOrderComponentsDestTable, DataTable purchaseOrderAccountsDestTable, DataTable receiptLinesDestTable, DataTable receiptComponentsDestTable, DataTable aPInvoiceLinesDestTable, DataTable aPInvoiceExpenseAccountsDestTable, DataTable materialIssueLinesDestTable, DataTable materialIssueComponentsDestTable, DataTable mfgReceiptsDestTable, DataTable mfgReceiptsComponentsDestTable, DataTable glJournalLinesDestTable, DataTable glJournalsDestTable, DataTable serialNumberTransactionsDestTable, DataTable lotNumberTransactionsDestTable, int nParentAsm, double nOldParentAsmProdQty, int nStartSeq, string cNewJobID, int nNewParentAsm, int nNewParentLevel, double nNewParentAsmProdQty, SplitCostOption nSplitCosts, double nSourceTablePercent, double nDestPercent, ref int nPartTransactionID, ref int nSerialNumberTransactionID, ref int nLotNumberTransactionID, string oAssembliesToIgnore, DataTable jobSplitLogLinesTable, int nJobSplitLogID, double initialDestPercent, double sourcePercent, string sourceTableJobID, List<int> assembliesToIgnore)
	{
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			new Dictionary<string, string>();
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
			bool flag = false;
			double num = 0.0;
			DataRow[] array = jobOperationsSourceTable.Select("jmoJobAssemblyID = " + nParentAsm.ToLinq());
			DataRow[] array2;
			if (!_initialJobOperationsSaved)
			{
				_initialJobOperationsSourceTable = database.GetDataTable("select * from JobOperations where 0=1", fillSchema: true, out var _, null);
				array2 = jobOperationsSourceTable.Select();
				foreach (DataRow sourceTableRow in array2)
				{
					DataRow destRow = _initialJobOperationsSourceTable.AddBlankRow();
					CopyAllFieldsToNewRow(sourceTableRow, destRow);
				}
				_initialJobOperationsSaved = true;
			}
			array2 = array;
			foreach (DataRow dataRow in array2)
			{
				num = nSourceTablePercent;
				if (Convert.ToDouble(dataRow["jmoJobOperationID"]) < (double)nStartSeq)
				{
					if (nOldParentAsmProdQty != 0.0)
					{
						dataRow["jmoQuantityPerAssembly"] = Math.Round(Convert.ToDouble(dataRow["jmoOperationQuantity"]) / nOldParentAsmProdQty, 5);
					}
				}
				else
				{
					DataRow dataRow2 = jobOperationsDestTable.AddBlankRow();
					CopyAllFieldsToNewRow(dataRow, dataRow2);
					dataRow2["jmoCreatedBy"] = database.User.ID;
					dataRow2["jmoCreatedDate"] = DateTime.Now;
					Job job = new Job();
					dataRow2["jmoJobID"] = cNewJobID;
					dataRow2["jmoJobAssemblyID"] = nNewParentAsm;
					dataRow2["jmoOperationQuantity"] = Math.Round(nNewParentAsmProdQty * Convert.ToDouble(dataRow2["jmoQuantityPerAssembly"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					dataRow2["jmoEstimatedProductionHours"] = (decimal)job.CalculateProductionHours(database, Convert.ToDouble(dataRow2["jmoOperationQuantity"]), Convert.ToDouble(dataRow2["jmoProductionStandard"]), Convert.ToString(dataRow2["jmoStandardFactor"]), Convert.ToString(dataRow2["jmoWorkCenterID"]), 0);
					dataRow["jmoOperationQuantity"] = Convert.ToDouble(dataRow["jmoOperationQuantity"]) - Convert.ToDouble(dataRow2["jmoOperationQuantity"]);
					dataRow["jmoEstimatedProductionHours"] = (decimal)job.CalculateProductionHours(database, Convert.ToDouble(dataRow["jmoOperationQuantity"]), Convert.ToDouble(dataRow["jmoProductionStandard"]), Convert.ToString(dataRow["jmoStandardFactor"]), Convert.ToString(dataRow["jmoWorkCenterID"]), 0);
					switch (nSplitCosts)
					{
					case SplitCostOption.MoveCostsToTargetJob:
						dataRow["jmoQuantityComplete"] = 0;
						dataRow["jmoActualProductionHours"] = 0;
						dataRow["jmoSetupPercentComplete"] = 0;
						dataRow["jmoActualSetupHours"] = 0;
						break;
					case SplitCostOption.SplitCostsBasedOnQuantity:
					{
						int num2 = Convert.ToInt32(dataRow["jmoJobAssemblyID"]);
						int num3 = Convert.ToInt32(dataRow["jmoJobOperationID"]);
						DataRow dataRow3 = _initialJobOperationsSourceTable.Select("jmoJobAssemblyID = " + num2.ToLinq() + " and jmoJobOperationID = " + num3.ToLinq()).SingleOrDefault();
						dataRow2["jmoQuantityComplete"] = Convert.ToDouble(dataRow3["jmoQuantityComplete"]) * initialDestPercent;
						dataRow2["jmoActualSetupHours"] = Convert.ToDouble(dataRow3["jmoActualSetupHours"]) * initialDestPercent;
						dataRow2["jmoActualProductionHours"] = Math.Round(Convert.ToDouble(dataRow3["jmoActualProductionHours"]) * initialDestPercent, 5);
						dataRow["jmoQuantityComplete"] = Convert.ToDouble(dataRow["jmoQuantityComplete"]) - Convert.ToDouble(dataRow2["jmoQuantityComplete"]);
						dataRow["jmoActualSetupHours"] = Convert.ToDouble(dataRow["jmoActualSetupHours"]) - Convert.ToDouble(dataRow2["jmoActualSetupHours"]);
						dataRow["jmoActualProductionHours"] = Math.Round(Convert.ToDouble(dataRow["jmoActualProductionHours"]) - Convert.ToDouble(dataRow2["jmoActualProductionHours"]), 5);
						if (Convert.ToByte(dataRow["jmoProductionComplete"]) == 0)
						{
							dataRow2["jmoSetupComplete"] = 0;
							if (Convert.ToInt32(dataRow["jmoSetupPercentComplete"]) != 0)
							{
								dataRow2["jmoSetupPercentComplete"] = Convert.ToInt32(Convert.ToDouble(dataRow3["jmoSetupPercentComplete"]) * initialDestPercent);
								dataRow["jmoSetupPercentComplete"] = Convert.ToInt32(Convert.ToDouble(dataRow["jmoSetupPercentComplete"]) - Convert.ToDouble(dataRow2["jmoSetupPercentComplete"]));
							}
						}
						break;
					}
					default:
						dataRow2["jmoQuantityComplete"] = 0;
						dataRow2["jmoSetupPercentComplete"] = 0;
						dataRow2["jmoCompletedSetupHours"] = 0;
						dataRow2["jmoCompletedProductionHours"] = 0;
						dataRow2["jmoActualSetupHours"] = 0;
						dataRow2["jmoActualProductionHours"] = 0;
						dataRow2["jmoSetupComplete"] = 0;
						dataRow2["jmoProductionComplete"] = 0;
						break;
					}
					dataRow2["jmoStartDate"] = DBNull.Value;
					dataRow2["jmoDueDate"] = DBNull.Value;
					dataRow2["jmoStartHour"] = 0;
					dataRow2["jmoDueHour"] = 0;
					dataRow2["jmoScrapQuantityReceived"] = 0;
					dataRow2["jmoQuantityToInspect"] = 0;
					dataRow2["jmoQuantityToReturn"] = 0;
					AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "JobOperations", dataRow["jmoUniqueID"], "JobOperations", dataRow2["jmoUniqueID"], nJobSplitLogID);
					if (nSplitCosts != SplitCostOption.KeepCostsOnSourceJob && initialDestPercent > 0.0)
					{
						dictionary4.Clear();
						dictionary4.Add(dataRow["jmoUniqueID"].ToString().Trim(), dataRow2["jmoUniqueID"].ToString());
						DataRow[] array3 = timecardLinesSourceTable.Select("lmlJobAssemblyID = " + nParentAsm.ToLinq() + " and lmlJobOperationID = " + Convert.ToDouble(dataRow["jmoJobOperationID"]).ToLinq());
						DataRow[] source = _initialTimecardLinesSourceTable.Select("lmlJobAssemblyID = " + nParentAsm.ToLinq() + " and lmlJobOperationID = " + Convert.ToDouble(dataRow["jmoJobOperationID"]).ToLinq());
						DataRow[] array4 = array3;
						foreach (DataRow timecardLineSourceTableRow in array4)
						{
							DataRow initialTimecardLinesSourceTableRow = source.FirstOrDefault((DataRow row) => Convert.ToInt32(row["lmlTimecardLineID"]) == Convert.ToInt32(timecardLineSourceTableRow["lmlTimecardLineID"]) && Convert.ToInt32(row["lmlTimecardID"]) == Convert.ToInt32(timecardLineSourceTableRow["lmlTimecardID"]));
							SplitAndTransferTimecards(database, transaction, initialTimecardLinesSourceTableRow, timecardLineSourceTableRow, timecardLinesDestTable, nSplitCosts, Convert.ToString(dataRow2["jmoJobID"]), Convert.ToInt32(dataRow2["jmoJobAssemblyID"]), initialDestPercent, jobSplitLogLinesTable, nJobSplitLogID);
						}
						dictionary.Clear();
						array4 = purchaseOrderLinesSourceTable.Select("pmlJobAssemblyID = " + nParentAsm.ToLinq() + " and pmlJobOperationID = " + Convert.ToDouble(dataRow["jmoJobOperationID"]).ToLinq());
						foreach (DataRow dataRow4 in array4)
						{
							DataRow purchaseOrderLinePreSplitSource = _purchaseOrderLinesPreSplitSource.Select("pmlPurchaseOrderID = " + dataRow4["pmlPurchaseOrderID"].ToLinq() + " and pmlPurchaseOrderLineID = " + Convert.ToDouble(dataRow4["pmlPurchaseOrderLineID"]).ToLinq()).FirstOrDefault();
							DataRow[] purchaseOrderComponentsSourceRows = purchaseOrderComponentsSourceTable.Select("pmoPurchaseOrderID = " + dataRow4["pmlPurchaseOrderID"].ToLinq() + " And pmoPurchaseOrderLineID = " + dataRow4["pmlPurchaseOrderLineID"].ToLinq());
							DataRow[] purchaseOrderAccountsSourceRows = purchaseOrderAccountsSourceTable.Select("pmxPurchaseOrderID = " + dataRow4["pmlPurchaseOrderID"].ToLinq() + " And pmxPurchaseOrderLineID = " + dataRow4["pmlPurchaseOrderLineID"].ToLinq());
							SplitAndTransferPurchaseOrderLines(database, transaction, purchaseOrderLinePreSplitSource, dataRow4, purchaseOrderLinesDestTable, purchaseOrderComponentsSourceRows, rsPurchaseOrderComponentsDestTable, purchaseOrderAccountsSourceRows, purchaseOrderAccountsDestTable, dataRow2["jmoJobID"].ToString(), Convert.ToInt32(dataRow2["jmoJobAssemblyID"]), sourcePercent, initialDestPercent, dictionary, jobSplitLogLinesTable, nJobSplitLogID, nSplitCosts, dictionary4);
						}
						dictionary2.Clear();
						dictionary5.Clear();
						array4 = receiptLinesSourceTable.Select(string.Format("rmlJobAssemblyID = {0} and rmlJobOperationID = {1}", nParentAsm.ToLinq(), Convert.ToDouble(dataRow["jmoJobOperationID"].ToLinq())));
						foreach (DataRow dataRow5 in array4)
						{
							DataRow initialReceiptLinesSourceRow = _initialReceiptLinesSourceTable.Select("rmlReceiptID = " + dataRow5["rmlReceiptID"].ToLinq() + " And rmlReceiptLineID = " + dataRow5["rmlReceiptLineID"].ToLinq()).FirstOrDefault();
							DataRow[] foundReceiptComponentsSourceRows = receiptComponentsSourceTable.Select("rmoReceiptID = " + dataRow5["rmlReceiptID"].ToLinq() + " And rmoReceiptLineID = " + dataRow5["rmlReceiptLineID"].ToLinq());
							DataRow[] initialFoundReceiptComponentsSourceRows = _initialReceiptComponentsSourceTable.Select("rmoReceiptID = " + dataRow5["rmlReceiptID"].ToLinq() + " And rmoReceiptLineID = " + dataRow5["rmlReceiptLineID"].ToLinq());
							SplitAndTransferReceiptLines(database, transaction, initialReceiptLinesSourceRow, dataRow5, receiptLinesDestTable, initialFoundReceiptComponentsSourceRows, foundReceiptComponentsSourceRows, receiptComponentsDestTable, Convert.ToString(dataRow2["jmoJobID"]), Convert.ToInt32(dataRow2["jmoJobAssemblyID"]), num, initialDestPercent, dictionary, dictionary2, dictionary4, dictionary5, jobSplitLogLinesTable, nJobSplitLogID, nSplitCosts);
						}
						ProcessReversals(receiptLinesDestTable, receiptComponentsDestTable, dictionary5, new ReversalFieldNames
						{
							LineIdFieldName = "rmlReverseReceiptID",
							LineLineFieldName = "rmlReverseReceiptLineID",
							ComponentIdFieldName = "rmoReverseReceiptID",
							ComponentLineFieldName = "rmoReverseReceiptLineID",
							ComponentComponentFieldName = "rmoReverseReceiptComponentID"
						});
						array4 = aPInvoiceLinesSourceTable.Select("aplJobAssemblyID = " + nParentAsm.ToLinq() + " and aplJobOperationID = " + Convert.ToDouble(dataRow["jmoJobOperationID"]).ToLinq());
						foreach (DataRow dataRow6 in array4)
						{
							DataRow apInvoiceLinesPreSplitSource = _apInvoiceLinesPreSplitSource.Select("aplAPInvoiceID = " + dataRow6["aplAPInvoiceID"].ToLinq() + " and aplAPInvoiceLineID = " + dataRow6["aplAPInvoiceLineID"].ToLinq()).FirstOrDefault();
							DataRow[] foundApInvoiceExpenseAccountsRowsSource = aPInvoiceExpenseAccountsSourceTable.Select("apxAPInvoiceID = " + Convert.ToString(dataRow6["aplAPInvoiceID"]).ToLinq() + " And apxAPInvoiceLineID = " + Convert.ToDouble(dataRow6["aplAPInvoiceLineID"]).ToLinq());
							SplitAndTransferAPInvoiceLines(database, transaction, apInvoiceLinesPreSplitSource, dataRow6, aPInvoiceLinesDestTable, foundApInvoiceExpenseAccountsRowsSource, aPInvoiceExpenseAccountsDestTable, Convert.ToString(dataRow2["jmoJobID"]), Convert.ToInt32(dataRow2["jmoJobAssemblyID"]), num, initialDestPercent, nSplitCosts, dictionary, dictionary2, dictionary4, jobSplitLogLinesTable, nJobSplitLogID);
						}
						dictionary3.Clear();
						DataRow[] array5 = mfgReceiptsSourceTable.Select("rmmJobAssemblyID = " + nParentAsm.ToLinq() + " and rmmJobOperationID = " + Convert.ToDouble(dataRow["jmoJobOperationID"]).ToLinq());
						DataRow[] source2 = _mfgReceiptPreSplitSourceTable.Select("rmmJobAssemblyID = " + nParentAsm.ToLinq() + " and rmmJobOperationID = " + Convert.ToDouble(dataRow["jmoJobOperationID"]).ToLinq());
						array4 = array5;
						foreach (DataRow mfgReceiptSourceRow in array4)
						{
							DataRow preSplitMfgReceiptSourceRow = source2.FirstOrDefault((DataRow row) => Convert.ToInt32(row["rmmMfgReceiptID"]) == Convert.ToInt32(mfgReceiptSourceRow["rmmMfgReceiptID"]));
							DataRow[] foundMfgReceiptComponentsSourceRows = mfgReceiptsComponentsSourceTable.Select("rmnMfgReceiptID = " + mfgReceiptSourceRow["rmmMfgReceiptID"].ToLinq());
							DataRow[] initialFoundMfgReceiptComponentsSourceRows = _mfgReceiptComponentsPreSplitSourceTable.Select("rmnMfgReceiptID = " + mfgReceiptSourceRow["rmmMfgReceiptID"].ToLinq());
							SplitAndTransferMfgReceipts(database, transaction, preSplitMfgReceiptSourceRow, mfgReceiptSourceRow, mfgReceiptsDestTable, initialFoundMfgReceiptComponentsSourceRows, foundMfgReceiptComponentsSourceRows, mfgReceiptsComponentsDestTable, Convert.ToString(dataRow2["jmoJobID"]), Convert.ToInt32(dataRow2["jmoJobAssemblyID"]), initialDestPercent, dictionary4, dictionary3, dictionary, jobSplitLogLinesTable, nJobSplitLogID, nSplitCosts);
						}
						ProcessReversals(mfgReceiptsDestTable, mfgReceiptsComponentsDestTable, dictionary3, new ReversalFieldNames
						{
							LineIdFieldName = "rmmReverseMfgReceiptID",
							LineLineFieldName = "",
							ComponentIdFieldName = "rmnReverseMfgReceiptID",
							ComponentLineFieldName = "",
							ComponentComponentFieldName = "rmnReverseMfgReceiptCompID"
						});
						array4 = partTransactionsSourceTable.Select("imtJobAssemblyID = " + nParentAsm.ToLinq() + " and imtJobMaterialID = 0 and imtJobOperationID = " + Convert.ToDouble(dataRow["jmoJobOperationID"]).ToLinq() + " and imtJobType = 2 AND ((imtTransactionType = 1 AND imtSource = 2) OR (imtTransactionType = 2 AND imtSource = 3))");
						foreach (DataRow dataRow7 in array4)
						{
							DataRow partTransactionPreSplitSource = _partTransactionsPreSplitSource.Select("imtUniqueID = " + dataRow7["imtUniqueID"].ToLinq()).FirstOrDefault();
							DataRow[] partTransactionsCostsSourceTableRows = partTransactionsCostsSourceTable.Select("intPartTransactionID = " + dataRow7["imtPartTransactionID"].ToLinq());
							SplitAndTransferPartTransactions(database, transaction, partTransactionPreSplitSource, dataRow7, partTransactionsDestTable, partTransactionsCostsSourceTableRows, partTransactionsCostsDestTable, glJournalLinesDestTable, Convert.ToString(dataRow2["jmoJobID"]), Convert.ToInt32(dataRow2["jmoJobAssemblyID"]), initialDestPercent, ref nPartTransactionID, dictionary4, jobSplitLogLinesTable, nJobSplitLogID, nSplitCosts);
						}
					}
				}
				SplitAndTransferJobMaterial(database, transaction, jobAssembliesDestTable, jobMaterialsSourceTable, jobMaterialComponentsSourceTable, partTransactionsSourceTable, partTransactionsCostsSourceTable, timecardLinesSourceTable, purchaseOrderLinesSourceTable, purchaseOrderComponentsSourceTable, purchaseOrderAccountsSourceTable, receiptLinesSourceTable, receiptComponentsSourceTable, aPInvoiceLinesSourceTable, aPInvoiceExpenseAccountsSourceTable, materialIssueLinesSourceTable, materialIssueComponentsSourceTable, mfgReceiptsSourceTable, mfgReceiptsComponentsSourceTable, glJournalLinesSourceTable, glJournalsSourceTable, serialNumberTransactionsSourceTable, lotNumberTransactionsSourceTable, jobMaterialsDestTable, jobMaterialComponentsDestTable, partTransactionsDestTable, partTransactionsCostsDestTable, timecardLinesDestTable, purchaseOrderLinesDestTable, rsPurchaseOrderComponentsDestTable, purchaseOrderAccountsDestTable, receiptLinesDestTable, receiptComponentsDestTable, aPInvoiceLinesDestTable, aPInvoiceExpenseAccountsDestTable, materialIssueLinesDestTable, materialIssueComponentsDestTable, mfgReceiptsDestTable, mfgReceiptsComponentsDestTable, glJournalLinesDestTable, glJournalsDestTable, serialNumberTransactionsDestTable, lotNumberTransactionsDestTable, nParentAsm, nOldParentAsmProdQty, nStartSeq, cNewJobID, nNewParentAsm, nNewParentLevel, nNewParentAsmProdQty, nSplitCosts, sourcePercent, initialDestPercent, nDestPercent, ref nPartTransactionID, ref nSerialNumberTransactionID, ref nLotNumberTransactionID, Convert.ToInt32(dataRow["jmoJobOperationID"]), sourceTableJobID, assembliesToIgnore, jobSplitLogLinesTable, nJobSplitLogID);
			}
			SplitAndTransferJobMaterial(database, transaction, jobAssembliesDestTable, jobMaterialsSourceTable, jobMaterialComponentsSourceTable, partTransactionsSourceTable, partTransactionsCostsSourceTable, timecardLinesSourceTable, purchaseOrderLinesSourceTable, purchaseOrderComponentsSourceTable, purchaseOrderAccountsSourceTable, receiptLinesSourceTable, receiptComponentsSourceTable, aPInvoiceLinesSourceTable, aPInvoiceExpenseAccountsSourceTable, materialIssueLinesSourceTable, materialIssueComponentsSourceTable, mfgReceiptsSourceTable, mfgReceiptsComponentsSourceTable, glJournalLinesSourceTable, glJournalsSourceTable, serialNumberTransactionsSourceTable, lotNumberTransactionsSourceTable, jobMaterialsDestTable, jobMaterialComponentsDestTable, partTransactionsDestTable, partTransactionsCostsDestTable, timecardLinesDestTable, purchaseOrderLinesDestTable, rsPurchaseOrderComponentsDestTable, purchaseOrderAccountsDestTable, receiptLinesDestTable, receiptComponentsDestTable, aPInvoiceLinesDestTable, aPInvoiceExpenseAccountsDestTable, materialIssueLinesDestTable, materialIssueComponentsDestTable, mfgReceiptsDestTable, mfgReceiptsComponentsDestTable, glJournalLinesDestTable, glJournalsDestTable, serialNumberTransactionsDestTable, lotNumberTransactionsDestTable, nParentAsm, nOldParentAsmProdQty, nStartSeq, cNewJobID, nNewParentAsm, nNewParentLevel, nNewParentAsmProdQty, nSplitCosts, sourcePercent, initialDestPercent, nDestPercent, ref nPartTransactionID, ref nSerialNumberTransactionID, ref nLotNumberTransactionID, 0, sourceTableJobID, assembliesToIgnore, jobSplitLogLinesTable, nJobSplitLogID);
			DataRow dataRow8 = jobAssembliesDestTable.Select("jmaJobAssemblyID = " + nNewParentAsm.ToLinq()).FirstOrDefault();
			DataRow dataRow9 = jobAssembliesSourceTable.Select("jmaJobAssemblyID = " + nParentAsm.ToLinq()).FirstOrDefault();
			array2 = jobAssembliesSourceTable.Select("jmaParentAssemblyID = " + nParentAsm.ToLinq() + " and jmaJobAssemblyID <> 0");
			foreach (DataRow dataRow10 in array2)
			{
				DataRow dataRow11 = JobAssembliesSource.Select("jmaJobAssemblyID = " + dataRow10["jmaJobAssemblyID"].ToLinq()).FirstOrDefault();
				flag = false;
				if (oAssembliesToIgnore != "" && oAssembliesToIgnore.IndexOf("," + Convert.ToDouble(dataRow10["jmaJobAssemblyID"]).ToSql() + ",", StringComparison.CurrentCultureIgnoreCase) >= 0)
				{
					flag = true;
				}
				if (flag)
				{
					if (nOldParentAsmProdQty != 0.0)
					{
						dataRow10["jmaQuantityPerParent"] = Math.Round(Convert.ToDouble(dataRow11["jmaOrderQuantity"]) / nOldParentAsmProdQty, 5);
					}
					continue;
				}
				DataRow dataRow12 = jobAssembliesDestTable.AddBlankRow();
				CopyAllFieldsToNewRow(dataRow11, dataRow12);
				dataRow12["jmaCreatedBy"] = database.User.ID;
				dataRow12["jmaCreatedDate"] = DateTime.Now;
				dataRow12["jmaJobID"] = cNewJobID;
				dataRow12["jmaParentAssemblyID"] = nNewParentAsm;
				dataRow12["jmaLevel"] = nNewParentLevel + 1;
				dataRow12["jmaScheduledStartDate"] = DBNull.Value;
				dataRow12["jmaScheduledDueDate"] = DBNull.Value;
				dataRow12["jmaScheduledStartHour"] = 0;
				dataRow12["jmaScheduledDueHour"] = 0;
				dataRow12["jmaOrderQuantity"] = Math.Round(Convert.ToDouble(dataRow8["jmaQuantityToMake"]) * Convert.ToDouble(dataRow12["jmaQuantityPerParent"]), database.Props("DS").Field<byte>("xadSellQuantityDecimals"));
				dataRow12["jmaInventoryQuantity"] = Math.Round(Convert.ToDouble(dataRow12["jmaInventoryQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow12["jmaScrapQuantity"] = Math.Round(Convert.ToDouble(dataRow12["jmaScrapQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow12["jmaReworkQuantity"] = 0;
				dataRow12["jmaProductionQuantity"] = Convert.ToDouble(dataRow12["jmaOrderQuantity"]) + Convert.ToDouble(dataRow12["jmaInventoryQuantity"]) + Convert.ToDouble(dataRow12["jmaScrapQuantity"]);
				dataRow12["jmaQuantityReceivedToInventory"] = 0;
				dataRow12["jmaScrapQuantityCompleted"] = 0;
				dataRow12["jmaQuantityCompleted"] = 0;
				dataRow12["jmaQuantityToInspect"] = 0;
				dataRow12["jmaQuantityToReturn"] = 0;
				dataRow12["jmaScheduledStartDate"] = DBNull.Value;
				dataRow12["jmaScheduledDueDate"] = DBNull.Value;
				dataRow12["jmaScheduledStartHour"] = 0;
				dataRow12["jmaScheduledDueHour"] = 0;
				double num4 = Convert.ToDouble(dataRow11["jmaQuantityToMake"]);
				double num5 = Convert.ToDouble(dataRow11["jmaQuantityToPull"]);
				if (num4 > 0.0)
				{
					if (num5 > 0.0)
					{
						dataRow12["jmaQuantityToMake"] = Math.Round(Convert.ToDouble(dataRow12["jmaProductionQuantity"]) * (num4 / (num4 + num5)), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
						dataRow12["jmaQuantityToPull"] = Math.Round(Convert.ToDouble(dataRow12["jmaProductionQuantity"]) * (num5 / (num4 + num5)), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					}
					else
					{
						dataRow12["jmaQuantityToMake"] = Math.Round(Convert.ToDouble(dataRow12["jmaProductionQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
						dataRow12["jmaQuantityToPull"] = 0;
					}
				}
				else
				{
					dataRow12["jmaQuantityToMake"] = 0;
					if (num5 > 0.0)
					{
						dataRow12["jmaQuantityToPull"] = Math.Round(Convert.ToDouble(dataRow12["jmaProductionQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					}
				}
				switch (nSplitCosts)
				{
				case SplitCostOption.KeepCostsOnSourceJob:
					dataRow12["jmaQuantityIssued"] = 0;
					break;
				case SplitCostOption.SplitCostsBasedOnQuantity:
					dataRow12["jmaQuantityIssued"] = Math.Round(Convert.ToDouble(dataRow11["jmaQuantityIssued"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					break;
				}
				if (DataRowComparer<DataRow>.Default.Equals(dataRow11, dataRow10))
				{
					dataRow10["jmaOrderQuantity"] = Math.Round(Convert.ToDouble(dataRow9["jmaQuantityToMake"]) * Convert.ToDouble(dataRow10["jmaQuantityPerParent"]), database.Props("DS").Field<byte>("xadSellQuantityDecimals"));
					dataRow10["jmaInventoryQuantity"] = Math.Round(Convert.ToDouble(dataRow10["jmaInventoryQuantity"]) * sourcePercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					dataRow10["jmaScrapQuantity"] = Math.Round(Convert.ToDouble(dataRow10["jmaScrapQuantity"]) * sourcePercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					dataRow10["jmaProductionQuantity"] = Convert.ToDouble(dataRow10["jmaOrderQuantity"]) + Convert.ToDouble(dataRow10["jmaInventoryQuantity"]) + Convert.ToDouble(dataRow10["jmaScrapQuantity"]) + Convert.ToDouble(dataRow10["jmaReworkQuantity"]);
					double num6 = Convert.ToDouble(dataRow11["jmaQuantityToMake"]);
					double num7 = Convert.ToDouble(dataRow11["jmaQuantityToPull"]);
					if (num6 > 0.0)
					{
						if (num7 > 0.0)
						{
							dataRow10["jmaQuantityToMake"] = Math.Round(Convert.ToDouble(dataRow10["jmaProductionQuantity"]) * (num6 / (num6 + num7)), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
							dataRow10["jmaQuantityToPull"] = Math.Round(Convert.ToDouble(dataRow10["jmaProductionQuantity"]) * (num7 / (num6 + num7)), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
						}
						else
						{
							dataRow10["jmaQuantityToMake"] = dataRow10["jmaProductionQuantity"];
							dataRow10["jmaQuantityToPull"] = 0;
						}
					}
					else
					{
						dataRow10["jmaQuantityToMake"] = 0;
						if (num7 > 0.0)
						{
							dataRow10["jmaQuantityToPull"] = dataRow10["jmaProductionQuantity"];
						}
					}
					switch (nSplitCosts)
					{
					case SplitCostOption.MoveCostsToTargetJob:
						dataRow10["jmaQuantityIssued"] = 0;
						break;
					case SplitCostOption.SplitCostsBasedOnQuantity:
						dataRow10["jmaQuantityIssued"] = Math.Round(Convert.ToDouble(dataRow11["jmaQuantityIssued"]) * sourcePercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
						break;
					}
				}
				AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "JobAssemblies", dataRow10["jmaUniqueID"], "JobAssemblies", dataRow12["jmaUniqueID"], nJobSplitLogID);
				int nParentAsm2 = Convert.ToInt32(dataRow10["jmaJobAssemblyID"]);
				SplitAndTransferJobAssembly(database, transaction, partTransactionsSourceTable, partTransactionsCostsSourceTable, timecardLinesSourceTable, receiptLinesSourceTable, receiptComponentsSourceTable, aPInvoiceLinesSourceTable, aPInvoiceExpenseAccountsSourceTable, materialIssueLinesSourceTable, materialIssueComponentsSourceTable, mfgReceiptsSourceTable, mfgReceiptsComponentsSourceTable, glJournalLinesSourceTable, glJournalsSourceTable, serialNumberTransactionsSourceTable, lotNumberTransactionsSourceTable, dataRow12, partTransactionsDestTable, partTransactionsCostsDestTable, timecardLinesDestTable, receiptLinesDestTable, receiptComponentsDestTable, aPInvoiceLinesDestTable, aPInvoiceExpenseAccountsDestTable, materialIssueLinesDestTable, materialIssueComponentsDestTable, mfgReceiptsDestTable, mfgReceiptsComponentsDestTable, glJournalLinesDestTable, glJournalsDestTable, serialNumberTransactionsDestTable, lotNumberTransactionsDestTable, dataRow10, nParentAsm2, cNewJobID, nSplitCosts, nSourceTablePercent, nDestPercent, ref nPartTransactionID, ref nSerialNumberTransactionID, ref nLotNumberTransactionID, jobSplitLogLinesTable, nJobSplitLogID, initialDestPercent, sourcePercent, sourceTableJobID, assembliesToIgnore, nStartSeq);
				if (!(string.IsNullOrEmpty((string)dataRow10["jmaSourceMethodID"]) & string.IsNullOrEmpty((string)dataRow10["jmaSourceRevisionID"])))
				{
					SqlCommand sqlCommand = database.NewSqlCommand("INSERT INTO FormInputValues (xaiFormID,xaiControlName,xaiValue,xaiSourceUniqueID,xaiSourceTable,xaiParentFormID,xaiTopLevelFormID,xaiLastRunDate) SELECT xaiFormID,xaiControlName,xaiValue,@NewJobAssemblyUniqueID,xaiSourceTable,xaiParentFormID,xaiTopLevelFormID,CURRENT_TIMESTAMP FROM FormInputValues F Where F.xaiSourceUniqueID = @OriginalJobAssemblyUniqueID");
					sqlCommand.Parameters.Add(new SqlParameter("@OriginalJobAssemblyUniqueID", SqlDbType.UniqueIdentifier)).Value = dataRow10["jmaUniqueID"];
					sqlCommand.Parameters.Add(new SqlParameter("@NewJobAssemblyUniqueID", SqlDbType.UniqueIdentifier)).Value = dataRow12["jmaUniqueID"];
					database.ExecuteCommand(sqlCommand, transaction);
				}
				SplitNextJobAssembly(database, transaction, jobAssembliesSourceTable, jobMaterialsSourceTable, jobMaterialComponentsSourceTable, jobOperationsSourceTable, partTransactionsSourceTable, partTransactionsCostsSourceTable, timecardLinesSourceTable, purchaseOrderLinesSourceTable, purchaseOrderAccountsSourceTable, purchaseOrderComponentsSourceTable, receiptLinesSourceTable, receiptComponentsSourceTable, aPInvoiceLinesSourceTable, aPInvoiceExpenseAccountsSourceTable, materialIssueLinesSourceTable, materialIssueComponentsSourceTable, mfgReceiptsSourceTable, mfgReceiptsComponentsSourceTable, glJournalLinesSourceTable, glJournalsSourceTable, serialNumberTransactionsSourceTable, lotNumberTransactionsSourceTable, jobAssembliesDestTable, jobMaterialsDestTable, jobMaterialComponentsDestTable, jobOperationsDestTable, partTransactionsDestTable, partTransactionsCostsDestTable, timecardLinesDestTable, purchaseOrderLinesDestTable, rsPurchaseOrderComponentsDestTable, purchaseOrderAccountsDestTable, receiptLinesDestTable, receiptComponentsDestTable, aPInvoiceLinesDestTable, aPInvoiceExpenseAccountsDestTable, materialIssueLinesDestTable, materialIssueComponentsDestTable, mfgReceiptsDestTable, mfgReceiptsComponentsDestTable, glJournalLinesDestTable, glJournalsDestTable, serialNumberTransactionsDestTable, lotNumberTransactionsDestTable, nParentAsm2, Convert.ToDouble(dataRow10["jmaQuantityToMake"]), 0, cNewJobID, Convert.ToInt32(dataRow12["jmaJobAssemblyID"]), nNewParentLevel + 1, Convert.ToDouble(dataRow12["jmaQuantityToMake"]), nSplitCosts, nSourceTablePercent, nDestPercent, ref nPartTransactionID, ref nSerialNumberTransactionID, ref nLotNumberTransactionID, oAssembliesToIgnore, jobSplitLogLinesTable, nJobSplitLogID, initialDestPercent, sourcePercent, sourceTableJobID, assembliesToIgnore);
			}
			return true;
		}
		catch
		{
			throw;
		}
	}

	private void RecalculateTargetMaterialComponent(DataRow targetJobMaterialComponent, DataRow targetJobAssembly, DataRow sourceJobMaterialComponent, DataRow sourceJobAssembly, SplitCostOption nSplitCosts)
	{
		decimal num = targetJobAssembly.Field<decimal>("jmaProductionQuantity") / sourceJobAssembly.Field<decimal>("jmaProductionQuantity");
		targetJobMaterialComponent["jmtParentQuantity"] = sourceJobMaterialComponent.Field<decimal>("jmtParentQuantity") * num;
		targetJobMaterialComponent["jmtAdditionalQuantity"] = sourceJobMaterialComponent.Field<decimal>("jmtAdditionalQuantity") * num;
		targetJobMaterialComponent["jmtMaterialQuantity"] = targetJobMaterialComponent.Field<decimal>("jmtParentQuantity") * targetJobMaterialComponent.Field<decimal>("jmtQuantityPerParent") + targetJobMaterialComponent.Field<decimal>("jmtAdditionalQuantity");
		targetJobMaterialComponent["jmtQuantityToInspect"] = 0;
		targetJobMaterialComponent["jmtQuantityToReturn"] = 0;
		targetJobMaterialComponent["jmtScrapQuantityReceived"] = 0;
		switch (nSplitCosts)
		{
		case SplitCostOption.KeepCostsOnSourceJob:
			targetJobMaterialComponent["jmtQuantityReceived"] = 0;
			break;
		case SplitCostOption.SplitCostsBasedOnQuantity:
			targetJobMaterialComponent["jmtQuantityReceived"] = sourceJobMaterialComponent.Field<decimal>("jmtQuantityReceived") * num;
			break;
		}
		if (Convert.ToDouble(targetJobMaterialComponent["jmtMaterialQuantity"]) - Convert.ToDouble(targetJobMaterialComponent["jmtQuantityReceived"]) <= 0.0 || Convert.ToBoolean(targetJobMaterialComponent["jmtReceivedComplete"]))
		{
			targetJobMaterialComponent["jmtQuantityAllocated"] = 0;
		}
		else
		{
			targetJobMaterialComponent["jmtQuantityAllocated"] = Convert.ToDouble(targetJobMaterialComponent["jmtMaterialQuantity"]) - Convert.ToDouble(targetJobMaterialComponent["jmtQuantityReceived"]);
		}
	}

	private void RecalculateSourceMaterialComponent(DataRow targetJobMaterialComponent, DataRow targetJobAssembly, DataRow sourceJobMaterialComponent, DataRow sourceJobAssembly, SplitCostOption nSplitCosts)
	{
		string[] array = new string[3] { "jmtParentQuantity", "jmtAdditionalQuantity", "jmtMaterialQuantity" };
		foreach (string columnName in array)
		{
			sourceJobMaterialComponent[columnName] = sourceJobMaterialComponent.Field<decimal>(columnName) - targetJobMaterialComponent.Field<decimal>(columnName);
		}
		switch (nSplitCosts)
		{
		case SplitCostOption.SplitCostsBasedOnQuantity:
			array = new string[1] { "jmtQuantityReceived" };
			foreach (string columnName2 in array)
			{
				sourceJobMaterialComponent[columnName2] = sourceJobMaterialComponent.Field<decimal>(columnName2) - targetJobMaterialComponent.Field<decimal>(columnName2);
			}
			break;
		case SplitCostOption.MoveCostsToTargetJob:
			sourceJobMaterialComponent["jmtQuantityReceived"] = 0;
			break;
		}
		if (Convert.ToDouble(sourceJobMaterialComponent["jmtMaterialQuantity"]) - Convert.ToDouble(sourceJobMaterialComponent["jmtQuantityReceived"]) <= 0.0 || Convert.ToBoolean(sourceJobMaterialComponent["jmtReceivedComplete"]))
		{
			sourceJobMaterialComponent["jmtQuantityAllocated"] = 0;
		}
		else
		{
			sourceJobMaterialComponent["jmtQuantityAllocated"] = Convert.ToDouble(sourceJobMaterialComponent["jmtMaterialQuantity"]) - Convert.ToDouble(sourceJobMaterialComponent["jmtQuantityReceived"]);
		}
	}

	private void RecalculateTargetMaterial(DataRow targetJobMaterial, DataRow targetJobAssembly, DataRow sourceJobMaterial, DataRow sourceJobAssembly, SplitCostOption nSplitCosts, M1Database database)
	{
		decimal num = ((sourceJobAssembly.Field<decimal>("jmaProductionQuantity") == 0m) ? 0m : (targetJobAssembly.Field<decimal>("jmaProductionQuantity") / sourceJobAssembly.Field<decimal>("jmaProductionQuantity")));
		int decimals = database.Props("DS").Field<byte>("xadInventoryQuantityDecimals");
		targetJobMaterial["jmmQuantityToReturn"] = 0;
		targetJobMaterial["jmmQuantityToInspect"] = 0;
		targetJobMaterial["jmmPullFromStockQuantity"] = Math.Round(sourceJobMaterial.Field<decimal>("jmmPullFromStockQuantity") * num, decimals);
		targetJobMaterial["jmmPurchaseToJobQuantity"] = Math.Round(sourceJobMaterial.Field<decimal>("jmmPurchaseToJobQuantity") * num, decimals);
		targetJobMaterial["jmmEstimatedQuantity"] = Math.Round(sourceJobMaterial.Field<decimal>("jmmEstimatedQuantity") * num, decimals);
		targetJobMaterial["jmmScrapQuantity"] = Math.Round(sourceJobMaterial.Field<decimal>("jmmScrapQuantity") * num, decimals);
		targetJobMaterial["jmmScrapQuantityReceived"] = 0;
		switch (nSplitCosts)
		{
		case SplitCostOption.KeepCostsOnSourceJob:
			targetJobMaterial["jmmQuantityReceived"] = 0;
			break;
		case SplitCostOption.SplitCostsBasedOnQuantity:
			targetJobMaterial["jmmQuantityReceived"] = Math.Round(sourceJobMaterial.Field<decimal>("jmmQuantityReceived") * num, decimals);
			break;
		}
		if (Convert.ToDouble(targetJobMaterial["jmmEstimatedQuantity"]) - Convert.ToDouble(targetJobMaterial["jmmQuantityReceived"]) <= 0.0 || Convert.ToBoolean(targetJobMaterial["jmmReceivedComplete"]))
		{
			targetJobMaterial["jmmQuantityAllocated"] = 0;
		}
		else
		{
			targetJobMaterial["jmmQuantityAllocated"] = Convert.ToDouble(targetJobMaterial["jmmEstimatedQuantity"]) - Convert.ToDouble(targetJobMaterial["jmmQuantityReceived"]);
		}
	}

	public static void RecalculateSourceMaterial(DataRow sourceJobMaterial, DataRow targetJobMaterial, SplitCostOption nSplitCosts)
	{
		string[] array = new string[4] { "jmmScrapQuantity", "jmmEstimatedQuantity", "jmmPurchaseToJobQuantity", "jmmPullFromStockQuantity" };
		foreach (string columnName in array)
		{
			sourceJobMaterial[columnName] = sourceJobMaterial.Field<decimal>(columnName) - targetJobMaterial.Field<decimal>(columnName);
		}
		switch (nSplitCosts)
		{
		case SplitCostOption.SplitCostsBasedOnQuantity:
			array = new string[1] { "jmmQuantityReceived" };
			foreach (string columnName2 in array)
			{
				sourceJobMaterial[columnName2] = sourceJobMaterial.Field<decimal>(columnName2) - targetJobMaterial.Field<decimal>(columnName2);
			}
			break;
		case SplitCostOption.MoveCostsToTargetJob:
			sourceJobMaterial["jmmQuantityReceived"] = 0;
			break;
		}
		if (Convert.ToDouble(sourceJobMaterial["jmmEstimatedQuantity"]) - Convert.ToDouble(sourceJobMaterial["jmmQuantityReceived"]) <= 0.0 || Convert.ToBoolean(sourceJobMaterial["jmmReceivedComplete"]))
		{
			sourceJobMaterial["jmmQuantityAllocated"] = 0;
		}
		else
		{
			sourceJobMaterial["jmmQuantityAllocated"] = Convert.ToDouble(sourceJobMaterial["jmmEstimatedQuantity"]) - Convert.ToDouble(sourceJobMaterial["jmmQuantityReceived"]);
		}
	}

	private void SplitAndTransferJobMaterial(M1Database database, SqlTransaction transaction, DataTable jobAssembliesDestTable, DataTable jobMaterialsSourceTable, DataTable jobMaterialComponentsSourceTable, DataTable partTransactionsSourceTable, DataTable partTransactionsCostsSourceTable, DataTable rsTimecardLinesSourceTable, DataTable purchaseOrderLinesSourceTable, DataTable purchaseOrderComponentsSourceTable, DataTable purchaseOrderAccountsSourceTable, DataTable receiptLinesSourceTable, DataTable receiptComponentsSourceTable, DataTable aPInvoiceLinesSourceTable, DataTable aPInvoiceExpenseAccountsSourceTable, DataTable materialIssueLinesSourceTable, DataTable materialIssueComponentsSourceTable, DataTable mfgReceiptsSourceTable, DataTable mfgReceiptsComponentsSourceTable, DataTable glJournalLinesSourceTable, DataTable glJournalsSourceTable, DataTable serialNumberTransactionsSourceTable, DataTable lotNumberTransactionsSourceTable, DataTable jobMaterialsDestTable, DataTable jobMaterialComponentsDest, DataTable partTransactionsDest, DataTable partTransactionsCostsDest, DataTable rsTimecardLinesDestTable, DataTable purchaseOrderLinesDestTable, DataTable purchaseOrderComponentsDestTable, DataTable puTablerchaseOrderAccountsDest, DataTable receiptLinesDestTable, DataTable receiptComponentsDestTable, DataTable aPInvoiceLinesDestTable, DataTable aPInvoiceExpenseAccountsDestTable, DataTable materialIssueLinesDestTable, DataTable materialIssueComponentsDestTable, DataTable mfgReceiptsDestTable, DataTable mfgReceiptsComponentsDestTable, DataTable glJournalLinesDestTable, DataTable glJournalsDestTable, DataTable serialNumberTransactionsDestTable, DataTable lotNumberTransactionsDestTable, int nParentAsm, double nOldParentAsmProdQty, int nStartSeq, string cNewJobID, int nNewParentAsm, int nNewParentLevel, double nNewParentAsmProdQty, SplitCostOption nSplitCosts, double sourcePercent, double targetPercent, double nDestPercent, ref int nPartTransactionID, ref int nSerialNumberTransactionID, ref int nLotNumberTransactionID, int nRelatedOperation, string sourceTableJobID, List<int> assembliesToIgnore, DataTable jobSplitLogLinesTable, int nJobSplitLogID)
	{
		try
		{
			DataRow sourceJobAssembly = JobAssembliesSource.Select("jmaJobAssemblyID=" + nParentAsm.ToSql()).First();
			int d = nParentAsm;
			if (jobAssembliesDestTable.Select("jmaJobAssemblyID=" + nParentAsm.ToSql()).Count() == 0)
			{
				if (SelectedRootAssembly == 0)
				{
					return;
				}
				d = 0;
			}
			DataRow targetJobAssembly = jobAssembliesDestTable.Select("jmaJobAssemblyID=" + d.ToSql()).First();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary4 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
			Dictionary<string, string> dictionary6 = new Dictionary<string, string>();
			DataRow[] array = jobMaterialsSourceTable.Select("jmmJobAssemblyID = " + nParentAsm.ToLinq() + " And jmmRelatedJobOperationID = " + nRelatedOperation.ToLinq());
			foreach (DataRow dataRow in array)
			{
				if (Convert.ToDouble(dataRow["jmmRelatedJobOperationID"]) != 0.0 && Convert.ToDouble(dataRow["jmmRelatedJobOperationID"]) < (double)nStartSeq)
				{
					if (nOldParentAsmProdQty != 0.0)
					{
						dataRow["jmmQuantityPerAssembly"] = Math.Round((nOldParentAsmProdQty + nNewParentAsmProdQty) * Convert.ToDouble(dataRow["jmmQuantityPerAssembly"]) / nOldParentAsmProdQty, 5);
					}
					continue;
				}
				DataRow dataRow2 = jobMaterialsDestTable.AddBlankRow();
				CopyAllFieldsToNewRow(dataRow, dataRow2);
				dataRow2["jmmCreatedBy"] = database.User.ID;
				dataRow2["jmmCreatedDate"] = DateTime.Now;
				dataRow2["jmmJobID"] = cNewJobID;
				dataRow2["jmmJobAssemblyID"] = nNewParentAsm;
				int num = dataRow.Field<int>("jmmJobMaterialID");
				DataRow sourceJobMaterial = JobMaterialsSource.Select($"jmmJobAssemblyID = {nParentAsm.ToLinq()} And jmmRelatedJobOperationID = {nRelatedOperation.ToLinq()} and jmmJobMaterialID={num}").First();
				RecalculateTargetMaterial(dataRow2, targetJobAssembly, sourceJobMaterial, sourceJobAssembly, nSplitCosts, database);
				RecalculateSourceMaterial(dataRow, dataRow2, nSplitCosts);
				AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "JobMaterials", dataRow["jmmUniqueID"], "JobMaterials", dataRow2["jmmUniqueID"], nJobSplitLogID);
				DataRow[] array3;
				if (nSplitCosts != SplitCostOption.KeepCostsOnSourceJob && targetPercent > 0.0)
				{
					dictionary4.Clear();
					DataRow[] array2 = purchaseOrderLinesSourceTable.Select("pmlJobAssemblyID = " + nParentAsm.ToLinq() + " and pmlJobMaterialID = " + Convert.ToDouble(dataRow["jmmJobMaterialID"]).ToLinq());
					dictionary.Clear();
					array3 = array2;
					foreach (DataRow dataRow3 in array3)
					{
						DataRow purchaseOrderLinePreSplitSource = _purchaseOrderLinesPreSplitSource.Select("pmlPurchaseOrderID = " + dataRow3["pmlPurchaseOrderID"].ToLinq() + " and pmlPurchaseOrderLineID = " + Convert.ToDouble(dataRow3["pmlPurchaseOrderLineID"]).ToLinq()).FirstOrDefault();
						DataRow[] purchaseOrderComponentsSourceRows = purchaseOrderComponentsSourceTable.Select("pmoPurchaseOrderID = " + dataRow3["pmlPurchaseOrderID"].ToLinq() + " And pmoPurchaseOrderLineID = " + dataRow3["pmlPurchaseOrderLineID"].ToLinq());
						DataRow[] purchaseOrderAccountsSourceRows = purchaseOrderAccountsSourceTable.Select("pmxPurchaseOrderID = " + dataRow3["pmlPurchaseOrderID"].ToLinq() + " And pmxPurchaseOrderLineID = " + dataRow3["pmlPurchaseOrderLineID"].ToLinq());
						SplitAndTransferPurchaseOrderLines(database, transaction, purchaseOrderLinePreSplitSource, dataRow3, purchaseOrderLinesDestTable, purchaseOrderComponentsSourceRows, purchaseOrderComponentsDestTable, purchaseOrderAccountsSourceRows, puTablerchaseOrderAccountsDest, dataRow2["jmmJobID"].ToString(), Convert.ToInt32(dataRow2["jmmJobAssemblyID"]), sourcePercent, targetPercent, dictionary, jobSplitLogLinesTable, nJobSplitLogID, nSplitCosts);
					}
					dictionary2.Clear();
					dictionary6.Clear();
					array3 = receiptLinesSourceTable.Select(string.Format("rmlJobAssemblyID = {0} and rmlJobMaterialID = {1}", nParentAsm.ToLinq(), Convert.ToDouble(dataRow["jmmJobMaterialID"].ToLinq())));
					foreach (DataRow dataRow4 in array3)
					{
						DataRow initialReceiptLinesSourceRow = _initialReceiptLinesSourceTable.Select("rmlReceiptID = " + dataRow4["rmlReceiptID"].ToLinq() + " And rmlReceiptLineID = " + dataRow4["rmlReceiptLineID"].ToLinq()).FirstOrDefault();
						DataRow[] foundReceiptComponentsSourceRows = receiptComponentsSourceTable.Select("rmoReceiptID = " + dataRow4["rmlReceiptID"].ToLinq() + " And rmoReceiptLineID = " + dataRow4["rmlReceiptLineID"].ToLinq());
						DataRow[] initialFoundReceiptComponentsSourceRows = _initialReceiptComponentsSourceTable.Select("rmoReceiptID = " + dataRow4["rmlReceiptID"].ToLinq() + " And rmoReceiptLineID = " + dataRow4["rmlReceiptLineID"].ToLinq());
						SplitAndTransferReceiptLines(database, transaction, initialReceiptLinesSourceRow, dataRow4, receiptLinesDestTable, initialFoundReceiptComponentsSourceRows, foundReceiptComponentsSourceRows, receiptComponentsDestTable, Convert.ToString(dataRow2["jmmJobID"]), Convert.ToInt32(dataRow2["jmmJobAssemblyID"]), sourcePercent, targetPercent, dictionary, dictionary2, dictionary4, dictionary6, jobSplitLogLinesTable, nJobSplitLogID, nSplitCosts);
					}
					ProcessReversals(receiptLinesDestTable, receiptComponentsDestTable, dictionary6, new ReversalFieldNames
					{
						LineIdFieldName = "rmlReverseReceiptID",
						LineLineFieldName = "rmlReverseReceiptLineID",
						ComponentIdFieldName = "rmoReverseReceiptID",
						ComponentLineFieldName = "rmoReverseReceiptLineID",
						ComponentComponentFieldName = "rmoReverseReceiptComponentID"
					});
					array3 = aPInvoiceLinesSourceTable.Select("aplJobAssemblyID = " + nParentAsm.ToLinq() + " and aplJobMaterialID = " + Convert.ToDouble(dataRow["jmmJobMaterialID"]).ToLinq());
					foreach (DataRow dataRow5 in array3)
					{
						DataRow apInvoiceLinesPreSplitSource = _apInvoiceLinesPreSplitSource.Select("aplAPInvoiceID = " + dataRow5["aplAPInvoiceID"].ToLinq() + " and aplAPInvoiceLineID = " + dataRow5["aplAPInvoiceLineID"].ToLinq()).FirstOrDefault();
						DataRow[] foundApInvoiceExpenseAccountsRowsSource = aPInvoiceExpenseAccountsSourceTable.Select("apxAPInvoiceID = " + Convert.ToString(dataRow5["aplAPInvoiceID"]).ToLinq() + " And apxAPInvoiceLineID = " + Convert.ToDouble(dataRow5["aplAPInvoiceLineID"]).ToLinq());
						SplitAndTransferAPInvoiceLines(database, transaction, apInvoiceLinesPreSplitSource, dataRow5, aPInvoiceLinesDestTable, foundApInvoiceExpenseAccountsRowsSource, aPInvoiceExpenseAccountsDestTable, Convert.ToString(dataRow2["jmmJobID"]), Convert.ToInt32(dataRow2["jmmJobAssemblyID"]), sourcePercent, targetPercent, nSplitCosts, dictionary, dictionary2, dictionary4, jobSplitLogLinesTable, nJobSplitLogID);
					}
					dictionary3.Clear();
					array3 = materialIssueLinesSourceTable.Select("injJobAssemblyID = " + nParentAsm.ToLinq() + " and injJobMaterialID = " + Convert.ToDouble(dataRow["jmmJobMaterialID"]).ToLinq());
					foreach (DataRow dataRow6 in array3)
					{
						DataRow[] foundMaterialIssueComponentsSourceRows = materialIssueComponentsSourceTable.Select("inkMaterialIssueID = " + dataRow6["injMaterialIssueID"].ToLinq() + " And inkMaterialIssueLineID = " + dataRow6["injMaterialIssueLineID"].ToLinq());
						SplitAndTransferMaterialIssueLines(database, transaction, dataRow6, materialIssueLinesDestTable, foundMaterialIssueComponentsSourceRows, materialIssueComponentsDestTable, Convert.ToString(dataRow2["jmmJobID"]), Convert.ToInt32(dataRow2["jmmJobAssemblyID"]), sourcePercent, targetPercent, nDestPercent, dictionary4, dictionary3, jobSplitLogLinesTable, nJobSplitLogID, nSplitCosts);
					}
					ProcessReversals(materialIssueLinesDestTable, materialIssueComponentsDestTable, dictionary3, new ReversalFieldNames
					{
						LineIdFieldName = "injReverseMaterialIssueID",
						LineLineFieldName = "injReverseMaterialIssueLineID",
						ComponentIdFieldName = "inkReverseMaterialIssueID",
						ComponentLineFieldName = "inkReverseMaterialIssueLineID",
						ComponentComponentFieldName = "inkReverseMaterialIssueCompID"
					});
					dictionary5.Clear();
					DataRow[] array4 = mfgReceiptsSourceTable.Select("rmmJobAssemblyID = " + nParentAsm.ToLinq() + " and rmmJobMaterialID = " + Convert.ToDouble(dataRow["jmmJobMaterialID"]).ToLinq());
					DataRow[] source = _mfgReceiptPreSplitSourceTable.Select("rmmJobAssemblyID = " + nParentAsm.ToLinq() + " and rmmJobMaterialID = " + Convert.ToDouble(dataRow["jmmJobMaterialID"]).ToLinq());
					array3 = array4;
					foreach (DataRow mfgReceiptSourceRow in array3)
					{
						DataRow preSplitMfgReceiptSourceRow = source.FirstOrDefault((DataRow row) => Convert.ToInt32(row["rmmMfgReceiptID"]) == Convert.ToInt32(mfgReceiptSourceRow["rmmMfgReceiptID"]));
						DataRow[] foundMfgReceiptComponentsSourceRows = mfgReceiptsComponentsSourceTable.Select("rmnMfgReceiptID = " + mfgReceiptSourceRow["rmmMfgReceiptID"].ToLinq());
						DataRow[] initialFoundMfgReceiptComponentsSourceRows = _mfgReceiptComponentsPreSplitSourceTable.Select("rmnMfgReceiptID = " + mfgReceiptSourceRow["rmmMfgReceiptID"].ToLinq());
						SplitAndTransferMfgReceipts(database, transaction, preSplitMfgReceiptSourceRow, mfgReceiptSourceRow, mfgReceiptsDestTable, initialFoundMfgReceiptComponentsSourceRows, foundMfgReceiptComponentsSourceRows, mfgReceiptsComponentsDestTable, Convert.ToString(dataRow2["jmmJobID"]), Convert.ToInt32(dataRow2["jmmJobAssemblyID"]), targetPercent, dictionary4, dictionary5, dictionary, jobSplitLogLinesTable, nJobSplitLogID, nSplitCosts);
					}
					ProcessReversals(mfgReceiptsDestTable, mfgReceiptsComponentsDestTable, dictionary5, new ReversalFieldNames
					{
						LineIdFieldName = "rmmReverseMfgReceiptID",
						LineLineFieldName = "",
						ComponentIdFieldName = "rmnReverseMfgReceiptID",
						ComponentLineFieldName = "",
						ComponentComponentFieldName = "rmnReverseMfgReceiptCompID"
					});
					array3 = serialNumberTransactionsSourceTable.Select("sntJobAssemblyID = " + nParentAsm.ToLinq() + " and sntJobMaterialID = " + Convert.ToDouble(dataRow["jmmJobMaterialID"]).ToLinq());
					foreach (DataRow serialNumberTransactionsSourceTableRow in array3)
					{
						SplitAndTransferSerialNumberTransactions(database, transaction, serialNumberTransactionsSourceTableRow, serialNumberTransactionsDestTable, Convert.ToString(dataRow2["jmmJobID"]), Convert.ToInt32(dataRow2["jmmJobAssemblyID"]), sourcePercent, targetPercent, dictionary4, ref nSerialNumberTransactionID, jobSplitLogLinesTable, nJobSplitLogID);
					}
					array3 = lotNumberTransactionsSourceTable.Select("abtJobAssemblyID = " + nParentAsm.ToLinq() + " and abtJobMaterialID = " + Convert.ToDouble(dataRow["jmmJobMaterialID"]).ToLinq());
					foreach (DataRow lotNumberTransactionsSourceTableRow in array3)
					{
						SplitAndTransferLotNumberTransactions(database, transaction, lotNumberTransactionsSourceTableRow, lotNumberTransactionsDestTable, Convert.ToString(dataRow2["jmmJobID"]), Convert.ToInt32(dataRow2["jmmJobAssemblyID"]), sourcePercent, targetPercent, dictionary4, ref nLotNumberTransactionID, jobSplitLogLinesTable, nJobSplitLogID);
					}
					array3 = partTransactionsSourceTable.Select("imtJobAssemblyID = " + nParentAsm.ToLinq() + " and imtJobMaterialID = " + Convert.ToDouble(dataRow["jmmJobMaterialID"]).ToLinq() + " and imtJobType = 1 AND ((imtTransactionType = 1 AND imtSource = 2) OR (imtTransactionType = 2 AND imtSource = 3))");
					foreach (DataRow dataRow7 in array3)
					{
						DataRow partTransactionPreSplitSource = _partTransactionsPreSplitSource.Select("imtUniqueID = " + dataRow7["imtUniqueID"].ToLinq()).FirstOrDefault();
						DataRow[] partTransactionsCostsSourceTableRows = partTransactionsCostsSourceTable.Select("intPartTransactionID = " + dataRow7["imtPartTransactionID"].ToLinq());
						SplitAndTransferPartTransactions(database, transaction, partTransactionPreSplitSource, dataRow7, partTransactionsDest, partTransactionsCostsSourceTableRows, partTransactionsCostsDest, glJournalLinesDestTable, Convert.ToString(dataRow2["jmmJobID"]), Convert.ToInt32(dataRow2["jmmJobAssemblyID"]), targetPercent, ref nPartTransactionID, dictionary4, jobSplitLogLinesTable, nJobSplitLogID, nSplitCosts);
					}
				}
				num = dataRow.Field<int>("jmmJobMaterialID");
				array3 = jobMaterialComponentsSourceTable.Select("jmtJobAssemblyID = " + nParentAsm.ToLinq() + " And jmtJobMaterialID = " + num.ToLinq());
				foreach (DataRow dataRow8 in array3)
				{
					DataRow dataRow9 = jobMaterialComponentsDest.AddBlankRow();
					CopyAllFieldsToNewRow(dataRow8, dataRow9);
					dataRow9["jmtCreatedBy"] = database.User.ID;
					dataRow9["jmtCreatedDate"] = DateTime.Now;
					dataRow9["jmtJobID"] = cNewJobID;
					dataRow9["jmtJobAssemblyID"] = nNewParentAsm;
					DataRow sourceJobMaterialComponent = JobMaterialComponentsSource.Select(string.Format("jmtJobAssemblyID = {0} AND jmtJobMaterialID={1} AND jmtJobMaterialComponentID = {2}", nParentAsm.ToLinq(), num, dataRow8.Field<int>("jmtJobMaterialComponentID").ToLinq())).First();
					RecalculateTargetMaterialComponent(dataRow9, targetJobAssembly, sourceJobMaterialComponent, sourceJobAssembly, nSplitCosts);
					RecalculateSourceMaterialComponent(dataRow9, targetJobAssembly, dataRow8, sourceJobAssembly, nSplitCosts);
					AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "JobMaterialComponents", dataRow8["jmtUniqueID"], "JobMaterialComponents", dataRow9["jmtUniqueID"], nJobSplitLogID);
				}
			}
		}
		catch
		{
			throw;
		}
	}

	private void SplitAndTransferPurchaseOrderLines(M1Database database, SqlTransaction transaction, DataRow purchaseOrderLinePreSplitSource, DataRow poLineRowSourceTable, DataTable purchaseOrderLinesDestTable, DataRow[] purchaseOrderComponentsSourceRows, DataTable purchaseOrderComponentsDestTable, DataRow[] purchaseOrderAccountsSourceRows, DataTable purchaseOrderAccountsDestTable, string cNewJob, int nNewAsm, double sourcePercent, double targetPercent, Dictionary<string, string> poMatcherDictionary, DataTable jobSplitLogLinesTable, int nJobSplitLogID, SplitCostOption splitCostOption, Dictionary<string, string> sourceTableUniqueIdMatcherDictionary = null)
	{
		try
		{
			DataRowComparer<DataRow> dataRowComparer = DataRowComparer<DataRow>.Default;
			int nextLineForTable = GetNextLineForTable(database, transaction, purchaseOrderLinesDestTable, "PurchaseOrderLines", poLineRowSourceTable["pmlPurchaseOrderID"].ToString());
			DataRow dataRow = purchaseOrderLinesDestTable.AddBlankRow();
			CopyAllFieldsToNewRow(purchaseOrderLinePreSplitSource, dataRow);
			dataRow["pmlCreatedBy"] = database.User.ID;
			dataRow["pmlCreatedDate"] = DateTime.Now;
			dataRow["pmlJobID"] = cNewJob;
			dataRow["pmlJobAssemblyID"] = nNewAsm;
			dataRow["pmlPurchaseOrderLineID"] = nextLineForTable;
			poMatcherDictionary.Add(poLineRowSourceTable["pmlPurchaseOrderID"].ToString().Trim() + "\t" + Convert.ToDouble(poLineRowSourceTable["pmlPurchaseOrderLineID"]).ToSql(), dataRow["pmlPurchaseOrderLineID"].ToString());
			if (sourceTableUniqueIdMatcherDictionary != null && Guid.TryParse(dataRow["pmlSourceTableUniqueID"].ToString().Trim(), out var _) && sourceTableUniqueIdMatcherDictionary.ContainsKey(Convert.ToString(poLineRowSourceTable["pmlSourceTableUniqueID"]).Trim()))
			{
				dataRow["pmlSourceTableUniqueID"] = sourceTableUniqueIdMatcherDictionary[Convert.ToString(poLineRowSourceTable["pmlSourceTableUniqueID"]).Trim()];
			}
			switch (splitCostOption)
			{
			case SplitCostOption.SplitCostsBasedOnQuantity:
				dataRow["pmlPurchaseQuantity"] = Math.Round(Convert.ToDouble(dataRow["pmlPurchaseQuantity"]) * targetPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				dataRow["pmlInventoryQuantity"] = Math.Round(Convert.ToDouble(dataRow["pmlInventoryQuantity"]) * targetPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["pmlSetupChargeBase"] = Math.Round(Convert.ToDouble(dataRow["pmlSetupChargeBase"]) * targetPercent, 2);
				dataRow["pmlSetupChargeForeign"] = Math.Round(Convert.ToDouble(dataRow["pmlSetupChargeForeign"]) * targetPercent, 2);
				dataRow["pmlExtendedCostBase"] = Math.Round(Convert.ToDouble(dataRow["pmlExtendedCostBase"]) * targetPercent, 2);
				dataRow["pmlExtendedCostForeign"] = Math.Round(Convert.ToDouble(dataRow["pmlExtendedCostForeign"]) * targetPercent, 2);
				dataRow["pmlPurchaseQuantityReceived"] = Math.Round(Convert.ToDouble(dataRow["pmlPurchaseQuantityReceived"]) * targetPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				dataRow["pmlInventoryQuantityReceived"] = Math.Round(Convert.ToDouble(dataRow["pmlInventoryQuantityReceived"]) * targetPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["pmlTaxAmountBase"] = Math.Round(Convert.ToDouble(dataRow["pmlTaxAmountBase"]) * targetPercent, 2);
				dataRow["pmlTaxAmountForeign"] = Math.Round(Convert.ToDouble(dataRow["pmlTaxAmountForeign"]) * targetPercent, 2);
				dataRow["pmlSecondTaxAmountBase"] = Math.Round(Convert.ToDouble(dataRow["pmlSecondTaxAmountBase"]) * targetPercent, 2);
				dataRow["pmlSecondTaxAmountForeign"] = Math.Round(Convert.ToDouble(dataRow["pmlSecondTaxAmountForeign"]) * targetPercent, 2);
				dataRow["pmlQuantityOnOrder"] = Math.Round(Convert.ToDouble(dataRow["pmlQuantityOnOrder"]) * targetPercent, database.Props("DS").Field<byte>("xadSellQuantityDecimals"));
				dataRow["pmlJobOpenQuantity"] = Math.Round(Convert.ToDouble(dataRow["pmlJobOpenQuantity"]) * targetPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["pmlTotalExtendedCostBase"] = Math.Round(Convert.ToDouble(dataRow["pmlTotalExtendedCostBase"]) * targetPercent, 2);
				dataRow["pmlTotalExtendedCostForeign"] = Math.Round(Convert.ToDouble(dataRow["pmlTotalExtendedCostForeign"]) * targetPercent, 2);
				dataRow["pmlTotalComponentCosts"] = Math.Round(Convert.ToDouble(dataRow["pmlTotalComponentCosts"]) * targetPercent, 2);
				poLineRowSourceTable["pmlPurchaseQuantity"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlPurchaseQuantity"]) - Convert.ToDouble(dataRow["pmlPurchaseQuantity"]), database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				poLineRowSourceTable["pmlInventoryQuantity"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlInventoryQuantity"]) - Convert.ToDouble(dataRow["pmlInventoryQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				poLineRowSourceTable["pmlSetupChargeBase"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlSetupChargeBase"]) - Convert.ToDouble(dataRow["pmlSetupChargeBase"]), 2);
				poLineRowSourceTable["pmlSetupChargeForeign"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlSetupChargeForeign"]) - Convert.ToDouble(dataRow["pmlSetupChargeForeign"]), 2);
				poLineRowSourceTable["pmlExtendedCostBase"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlExtendedCostBase"]) - Convert.ToDouble(dataRow["pmlExtendedCostBase"]), 2);
				poLineRowSourceTable["pmlExtendedCostForeign"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlExtendedCostForeign"]) - Convert.ToDouble(dataRow["pmlExtendedCostForeign"]), 2);
				poLineRowSourceTable["pmlPurchaseQuantityReceived"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlPurchaseQuantityReceived"]) - Convert.ToDouble(dataRow["pmlPurchaseQuantityReceived"]), database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				poLineRowSourceTable["pmlInventoryQuantityReceived"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlInventoryQuantityReceived"]) - Convert.ToDouble(dataRow["pmlInventoryQuantityReceived"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				poLineRowSourceTable["pmlTaxAmountBase"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlTaxAmountBase"]) - Convert.ToDouble(dataRow["pmlTaxAmountBase"]), 2);
				poLineRowSourceTable["pmlTaxAmountForeign"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlTaxAmountForeign"]) - Convert.ToDouble(dataRow["pmlTaxAmountForeign"]), 2);
				poLineRowSourceTable["pmlSecondTaxAmountBase"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlSecondTaxAmountBase"]) - Convert.ToDouble(dataRow["pmlSecondTaxAmountBase"]), 2);
				poLineRowSourceTable["pmlSecondTaxAmountForeign"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlSecondTaxAmountForeign"]) - Convert.ToDouble(dataRow["pmlSecondTaxAmountForeign"]), 2);
				poLineRowSourceTable["pmlQuantityOnOrder"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlQuantityOnOrder"]) - Convert.ToDouble(dataRow["pmlQuantityOnOrder"]), database.Props("DS").Field<byte>("xadSellQuantityDecimals"));
				poLineRowSourceTable["pmlJobOpenQuantity"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlJobOpenQuantity"]) - Convert.ToDouble(dataRow["pmlJobOpenQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				poLineRowSourceTable["pmlTotalExtendedCostBase"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlTotalExtendedCostBase"]) - Convert.ToDouble(dataRow["pmlTotalExtendedCostBase"]), 2);
				poLineRowSourceTable["pmlTotalExtendedCostForeign"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlTotalExtendedCostForeign"]) - Convert.ToDouble(dataRow["pmlTotalExtendedCostForeign"]), 2);
				poLineRowSourceTable["pmlTotalComponentCosts"] = Math.Round(Convert.ToDouble(poLineRowSourceTable["pmlTotalComponentCosts"]) - Convert.ToDouble(dataRow["pmlTotalComponentCosts"]), 2);
				break;
			case SplitCostOption.MoveCostsToTargetJob:
				poLineRowSourceTable["pmlPurchaseQuantity"] = 0;
				poLineRowSourceTable["pmlInventoryQuantity"] = 0;
				poLineRowSourceTable["pmlSetupChargeBase"] = 0;
				poLineRowSourceTable["pmlSetupChargeForeign"] = 0;
				poLineRowSourceTable["pmlExtendedCostBase"] = 0;
				poLineRowSourceTable["pmlExtendedCostForeign"] = 0;
				poLineRowSourceTable["pmlPurchaseQuantityReceived"] = 0;
				poLineRowSourceTable["pmlInventoryQuantityReceived"] = 0;
				poLineRowSourceTable["pmlTaxAmountBase"] = 0;
				poLineRowSourceTable["pmlTaxAmountForeign"] = 0;
				poLineRowSourceTable["pmlSecondTaxAmountBase"] = 0;
				poLineRowSourceTable["pmlSecondTaxAmountForeign"] = 0;
				poLineRowSourceTable["pmlQuantityOnOrder"] = 0;
				poLineRowSourceTable["pmlJobOpenQuantity"] = 0;
				poLineRowSourceTable["pmlTotalExtendedCostBase"] = 0;
				poLineRowSourceTable["pmlTotalExtendedCostForeign"] = 0;
				poLineRowSourceTable["pmlTotalComponentCosts"] = 0;
				break;
			}
			AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "PurchaseOrderLines", poLineRowSourceTable["pmlUniqueID"], "PurchaseOrderLines", dataRow["pmlUniqueID"], nJobSplitLogID);
			DataRow[] array = purchaseOrderAccountsSourceRows;
			foreach (DataRow dataRow2 in array)
			{
				DataRow dataRow3 = _purchaseOrderAccountsPreSplitSource.Select("pmxPurchaseOrderID = " + dataRow2["pmxPurchaseOrderID"].ToLinq() + " \r\n                                        And pmxPurchaseOrderLineID = " + dataRow2["pmxPurchaseOrderLineID"].ToLinq() + " \r\n                                        And pmxPurchaseOrderAccountID = " + dataRow2["pmxPurchaseOrderAccountID"].ToLinq()).FirstOrDefault();
				DataRow dataRow4 = purchaseOrderAccountsDestTable.AddBlankRow();
				CopyAllFieldsToNewRow(dataRow3, dataRow4);
				dataRow4["pmxCreatedBy"] = database.User.ID;
				dataRow4["pmxCreatedDate"] = DateTime.Now;
				dataRow4["pmxPurchaseOrderLineID"] = nextLineForTable;
				switch (splitCostOption)
				{
				case SplitCostOption.MoveCostsToTargetJob:
					dataRow2["pmxAmount"] = 0;
					break;
				case SplitCostOption.SplitCostsBasedOnQuantity:
					dataRow4["pmxAmount"] = Math.Round(Convert.ToDouble(dataRow4["pmxAmount"]) * targetPercent, 2);
					if (dataRowComparer.Equals(dataRow3, dataRow2))
					{
						dataRow2["pmxAmount"] = Math.Round(Convert.ToDouble(dataRow3?["pmxAmount"]) * sourcePercent, 2);
					}
					break;
				}
				AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "PurchaseOrderAccounts", dataRow2["pmxUniqueID"], "PurchaseOrderAccounts", dataRow4["pmxUniqueID"], nJobSplitLogID);
			}
			array = purchaseOrderComponentsSourceRows;
			foreach (DataRow dataRow5 in array)
			{
				DataRow dataRow6 = _purchaseOrderComponentsPreSplitSource.Select("pmoPurchaseOrderID = " + dataRow5["pmoPurchaseOrderID"].ToLinq() + " \r\n                                        And pmoPurchaseOrderLineID = " + dataRow5["pmoPurchaseOrderLineID"].ToLinq() + " \r\n                                        And pmoPurchaseOrderComponentID = " + dataRow5["pmoPurchaseOrderComponentID"].ToLinq()).FirstOrDefault();
				DataRow dataRow7 = purchaseOrderComponentsDestTable.AddBlankRow();
				CopyAllFieldsToNewRow(dataRow6, dataRow7);
				dataRow7["pmoCreatedBy"] = database.User.ID;
				dataRow7["pmoCreatedDate"] = DateTime.Now;
				dataRow7["pmoPurchaseOrderLineID"] = nextLineForTable;
				dataRow7["pmoJobID"] = cNewJob;
				dataRow7["pmoJobAssemblyID"] = nNewAsm;
				poMatcherDictionary.Add(dataRow5["pmoPurchaseOrderID"].ToString().Trim() + "\t" + Convert.ToDouble(dataRow5["pmoPurchaseOrderLineID"]).ToSql() + "\t" + Convert.ToDouble(dataRow5["pmoPurchaseOrderComponentID"]).ToSql(), dataRow7["pmoPurchaseOrderComponentID"].ToString());
				switch (splitCostOption)
				{
				case SplitCostOption.MoveCostsToTargetJob:
					dataRow5["pmoAdditionalQuantity"] = 0;
					dataRow5["pmoDeliveryQuantity"] = 0;
					dataRow5["pmoQuantityReceived"] = 0;
					dataRow5["pmoParentQuantity"] = 0;
					dataRow5["pmoExtendedCostBase"] = 0;
					dataRow5["pmoExtendedCostForeign"] = 0;
					break;
				case SplitCostOption.SplitCostsBasedOnQuantity:
					dataRow7["pmoAdditionalQuantity"] = Math.Round(Convert.ToDouble(dataRow7["pmoAdditionalQuantity"]) * targetPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
					dataRow7["pmoDeliveryQuantity"] = Math.Round(Convert.ToDouble(dataRow7["pmoDeliveryQuantity"]) * targetPercent, database.Props("DatasetProperties").Field<byte>("xadInventoryQuantityDecimals"));
					dataRow7["pmoQuantityReceived"] = Math.Round(Convert.ToDouble(dataRow7["pmoQuantityReceived"]) * targetPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
					dataRow7["pmoParentQuantity"] = Math.Round(Convert.ToDouble(dataRow7["pmoParentQuantity"]) * targetPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
					dataRow7["pmoExtendedCostBase"] = Math.Round(Convert.ToDouble(dataRow7["pmoExtendedCostBase"]) * targetPercent, 2);
					dataRow7["pmoExtendedCostForeign"] = Math.Round(Convert.ToDouble(dataRow7["pmoExtendedCostForeign"]) * targetPercent, 2);
					if (dataRowComparer.Equals(dataRow6, dataRow5))
					{
						dataRow5["pmoAdditionalQuantity"] = Math.Round(Convert.ToDouble(dataRow5["pmoAdditionalQuantity"]) * sourcePercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
						dataRow5["pmoDeliveryQuantity"] = Math.Round(Convert.ToDouble(dataRow5["pmoDeliveryQuantity"]) * sourcePercent, database.Props("DatasetProperties").Field<byte>("xadInventoryQuantityDecimals"));
						dataRow5["pmoQuantityReceived"] = Math.Round(Convert.ToDouble(dataRow5["pmoQuantityReceived"]) * sourcePercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
						dataRow5["pmoParentQuantity"] = Math.Round(Convert.ToDouble(dataRow5["pmoParentQuantity"]) * sourcePercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
						dataRow5["pmoExtendedCostBase"] = Math.Round(Convert.ToDouble(dataRow5["pmoExtendedCostBase"]) * sourcePercent, 2);
						dataRow5["pmoExtendedCostForeign"] = Math.Round(Convert.ToDouble(dataRow5["pmoExtendedCostForeign"]) * sourcePercent, 2);
					}
					break;
				}
				AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "PurchaseOrderComponents", dataRow5["pmoUniqueID"], "PurchaseOrderComponents", dataRow7["pmoUniqueID"], nJobSplitLogID);
			}
		}
		catch
		{
			throw;
		}
	}

	private void SplitAndTransferReceiptLines(M1Database database, SqlTransaction transaction, DataRow initialReceiptLinesSourceRow, DataRow receiptLineRowSourceRow, DataTable receiptLinesDestTable, DataRow[] initialFoundReceiptComponentsSourceRows, DataRow[] foundReceiptComponentsSourceRows, DataTable receiptComponentsDestTable, string cNewJob, int nNewAsm, double nSourceTablePercent, double initialDestPercent, Dictionary<string, string> poMatcherDictionary, Dictionary<string, string> receiptMatcherDictionary, Dictionary<string, string> sourceTableUniqueIdMatcherDictionary, Dictionary<string, string> receiptReversalMatcherDictionary, DataTable jobSplitLogLinesTable, int nJobSplitLogID, SplitCostOption nSplitCosts)
	{
		try
		{
			int num = 0;
			num = GetNextLineForTable(database, transaction, receiptLinesDestTable, "ReceiptLines", receiptLineRowSourceRow["rmlReceiptID"].ToString());
			DataRow dataRow = receiptLinesDestTable.AddBlankRow();
			dataRow["rmlCreatedBy"] = database.User.ID;
			dataRow["rmlCreatedDate"] = DateTime.Now;
			CopyAllFieldsToNewRow(receiptLineRowSourceRow, dataRow);
			dataRow["rmlJobID"] = cNewJob;
			dataRow["rmlJobAssemblyID"] = nNewAsm;
			dataRow["rmlReceiptLineID"] = num;
			if (Convert.ToString(dataRow["rmlPurchaseOrderID"]).Trim() != "" && Convert.ToDouble(dataRow["rmlPurchaseOrderLineID"]) != 0.0 && poMatcherDictionary.ContainsKey(Convert.ToString(dataRow["rmlPurchaseOrderID"]).Trim() + "\t" + Convert.ToDouble(dataRow["rmlPurchaseOrderLineID"]).ToSql()))
			{
				dataRow["rmlPurchaseOrderLineID"] = poMatcherDictionary[Convert.ToString(dataRow["rmlPurchaseOrderID"]).Trim() + "\t" + Convert.ToDouble(dataRow["rmlPurchaseOrderLineID"]).ToSql()];
			}
			receiptMatcherDictionary.Add(receiptLineRowSourceRow["rmlReceiptID"].ToString().Trim() + "\t" + Convert.ToDouble(receiptLineRowSourceRow["rmlReceiptLineID"]).ToSql(), dataRow["rmlReceiptLineID"].ToString());
			if (Convert.ToString(receiptLineRowSourceRow["rmlReverseReceiptID"]).Trim() != "" && Convert.ToDouble(receiptLineRowSourceRow["rmlReverseReceiptLineID"]) != 0.0)
			{
				receiptReversalMatcherDictionary.Add(receiptLineRowSourceRow["rmlReverseReceiptID"].ToString().Trim() + "\t" + Convert.ToDouble(receiptLineRowSourceRow["rmlReverseReceiptLineID"]).ToSql(), dataRow["rmlReceiptLineID"].ToString());
			}
			sourceTableUniqueIdMatcherDictionary.Add(receiptLineRowSourceRow["rmlUniqueID"].ToString().Trim(), dataRow["rmlUniqueID"].ToString());
			if (nSplitCosts == SplitCostOption.SplitCostsBasedOnQuantity)
			{
				dataRow["rmlPOPurchaseQuantity"] = Math.Round(Convert.ToDouble(initialReceiptLinesSourceRow["rmlPOPurchaseQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				dataRow["rmlPOOpenQuantity"] = Math.Round(Convert.ToDouble(initialReceiptLinesSourceRow["rmlPOOpenQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				dataRow["rmlPurchaseQuantityReceived"] = Math.Round(Convert.ToDouble(initialReceiptLinesSourceRow["rmlPurchaseQuantityReceived"]) * initialDestPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				dataRow["rmlInventoryQuantityReceived"] = Math.Round(Convert.ToDouble(initialReceiptLinesSourceRow["rmlInventoryQuantityReceived"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmlJobMatQuantityReceived"] = Math.Round(Convert.ToDouble(initialReceiptLinesSourceRow["rmlJobMatQuantityReceived"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmlJobOprQuantityReceived"] = Math.Round(Convert.ToDouble(initialReceiptLinesSourceRow["rmlJobOprQuantityReceived"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmlSetupCharge"] = Math.Round(Convert.ToDouble(initialReceiptLinesSourceRow["rmlSetupCharge"]) * initialDestPercent, 2);
				dataRow["rmlSetupChargeForeign"] = Math.Round(Convert.ToDouble(initialReceiptLinesSourceRow["rmlSetupChargeForeign"]) * initialDestPercent, 2);
				dataRow["rmlExtendedCostBase"] = Math.Round(Convert.ToDouble(initialReceiptLinesSourceRow["rmlExtendedCostBase"]) * initialDestPercent, 2);
				dataRow["rmlExtendedCostForeign"] = Math.Round(Convert.ToDouble(initialReceiptLinesSourceRow["rmlExtendedCostForeign"]) * initialDestPercent, 2);
				dataRow["rmlJobOpenQuantity"] = Math.Round(Convert.ToDouble(initialReceiptLinesSourceRow["rmlJobOpenQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmlJobEstimatedQuantity"] = Math.Round(Convert.ToDouble(initialReceiptLinesSourceRow["rmlJobEstimatedQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmlTotalComponentCosts"] = Math.Round(Convert.ToDouble(initialReceiptLinesSourceRow["rmlTotalComponentCosts"]) * initialDestPercent, 2);
				receiptLineRowSourceRow["rmlPOPurchaseQuantity"] = Math.Round(Convert.ToDouble(receiptLineRowSourceRow["rmlPOPurchaseQuantity"]) - Convert.ToDouble(dataRow["rmlPOPurchaseQuantity"]), database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				receiptLineRowSourceRow["rmlPOOpenQuantity"] = Math.Round(Convert.ToDouble(receiptLineRowSourceRow["rmlPOOpenQuantity"]) - Convert.ToDouble(dataRow["rmlPOOpenQuantity"]), database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				receiptLineRowSourceRow["rmlPurchaseQuantityReceived"] = Math.Round(Convert.ToDouble(receiptLineRowSourceRow["rmlPurchaseQuantityReceived"]) - Convert.ToDouble(dataRow["rmlPurchaseQuantityReceived"]), database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				receiptLineRowSourceRow["rmlInventoryQuantityReceived"] = Math.Round(Convert.ToDouble(receiptLineRowSourceRow["rmlInventoryQuantityReceived"]) - Convert.ToDouble(dataRow["rmlInventoryQuantityReceived"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				receiptLineRowSourceRow["rmlJobMatQuantityReceived"] = Math.Round(Convert.ToDouble(receiptLineRowSourceRow["rmlJobMatQuantityReceived"]) - Convert.ToDouble(dataRow["rmlJobMatQuantityReceived"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				receiptLineRowSourceRow["rmlJobOprQuantityReceived"] = Math.Round(Convert.ToDouble(receiptLineRowSourceRow["rmlJobOprQuantityReceived"]) - Convert.ToDouble(dataRow["rmlJobOprQuantityReceived"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				receiptLineRowSourceRow["rmlSetupCharge"] = Math.Round(Convert.ToDouble(receiptLineRowSourceRow["rmlSetupCharge"]) - Convert.ToDouble(dataRow["rmlSetupCharge"]), 2);
				receiptLineRowSourceRow["rmlSetupChargeForeign"] = Math.Round(Convert.ToDouble(receiptLineRowSourceRow["rmlSetupChargeForeign"]) - Convert.ToDouble(dataRow["rmlSetupChargeForeign"]), 2);
				receiptLineRowSourceRow["rmlExtendedCostBase"] = Math.Round(Convert.ToDouble(receiptLineRowSourceRow["rmlExtendedCostBase"]) - Convert.ToDouble(dataRow["rmlExtendedCostBase"]), 2);
				receiptLineRowSourceRow["rmlExtendedCostForeign"] = Math.Round(Convert.ToDouble(receiptLineRowSourceRow["rmlExtendedCostForeign"]) - Convert.ToDouble(dataRow["rmlExtendedCostForeign"]), 2);
				receiptLineRowSourceRow["rmlJobOpenQuantity"] = Math.Round(Convert.ToDouble(receiptLineRowSourceRow["rmlJobOpenQuantity"]) - Convert.ToDouble(dataRow["rmlJobOpenQuantity"]), 2);
				receiptLineRowSourceRow["rmlJobEstimatedQuantity"] = Math.Round(Convert.ToDouble(receiptLineRowSourceRow["rmlJobEstimatedQuantity"]) - Convert.ToDouble(dataRow["rmlJobEstimatedQuantity"]), 2);
				receiptLineRowSourceRow["rmlTotalComponentCosts"] = Math.Round(Convert.ToDouble(receiptLineRowSourceRow["rmlTotalComponentCosts"]) - Convert.ToDouble(dataRow["rmlTotalComponentCosts"]), 2);
			}
			else
			{
				receiptLineRowSourceRow["rmlPOPurchaseQuantity"] = 0;
				receiptLineRowSourceRow["rmlPOOpenQuantity"] = 0;
				receiptLineRowSourceRow["rmlPurchaseQuantityReceived"] = 0;
				receiptLineRowSourceRow["rmlInventoryQuantityReceived"] = 0;
				receiptLineRowSourceRow["rmlJobMatQuantityReceived"] = 0;
				receiptLineRowSourceRow["rmlJobOprQuantityReceived"] = 0;
				receiptLineRowSourceRow["rmlSetupCharge"] = 0;
				receiptLineRowSourceRow["rmlSetupChargeForeign"] = 0;
				receiptLineRowSourceRow["rmlExtendedCostBase"] = 0;
				receiptLineRowSourceRow["rmlExtendedCostForeign"] = 0;
				receiptLineRowSourceRow["rmlJobOpenQuantity"] = 0;
				receiptLineRowSourceRow["rmlJobEstimatedQuantity"] = 0;
				receiptLineRowSourceRow["rmlTotalComponentCosts"] = 0;
			}
			dataRow["rmlQuantityToInspect"] = 0;
			dataRow["rmlRequiresInspection"] = 0;
			dataRow["rmlInInspection"] = 0;
			dataRow["rmlInspectionComplete"] = 0;
			AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "ReceiptLines", receiptLineRowSourceRow["rmlUniqueID"], "ReceiptLines", dataRow["rmlUniqueID"], nJobSplitLogID);
			foreach (DataRow receiptComponentSourceRow in foundReceiptComponentsSourceRows)
			{
				DataRow dataRow2 = initialFoundReceiptComponentsSourceRows.FirstOrDefault((DataRow row) => Convert.ToInt32(row["rmoReceiptComponentID"]) == Convert.ToInt32(receiptComponentSourceRow["rmoReceiptComponentID"]));
				DataRow dataRow3 = receiptComponentsDestTable.AddBlankRow();
				CopyAllFieldsToNewRow(receiptComponentSourceRow, dataRow3);
				dataRow3["rmoCreatedBy"] = database.User.ID;
				dataRow3["rmoCreatedDate"] = DateTime.Now;
				dataRow3["rmoReceiptLineID"] = num;
				dataRow3["rmoJobID"] = cNewJob;
				dataRow3["rmoJobAssemblyID"] = nNewAsm;
				if (Convert.ToString(dataRow3["rmoPurchaseOrderID"]).Trim() != "" && Convert.ToDouble(dataRow3["rmoReceiptLineID"]) != 0.0 && poMatcherDictionary.ContainsKey(Convert.ToString(dataRow3["rmoPurchaseOrderID"]).Trim() + "\t" + Convert.ToDouble(dataRow3["rmoPurchaseOrderLineID"]).ToSql()))
				{
					dataRow3["rmoPurchaseOrderLineID"] = poMatcherDictionary[Convert.ToString(dataRow3["rmoPurchaseOrderID"]).Trim() + "\t" + Convert.ToDouble(dataRow3["rmoPurchaseOrderLineID"]).ToSql()];
				}
				if (Convert.ToString(dataRow3["rmoPurchaseOrderID"]).Trim() != "" && Convert.ToDouble(dataRow3["rmoReceiptLineID"]) != 0.0 && Convert.ToDouble(dataRow3["rmoReceiptComponentID"]) != 0.0 && poMatcherDictionary.ContainsKey(Convert.ToString(dataRow3["rmoPurchaseOrderID"]).Trim() + "\t" + Convert.ToDouble(dataRow3["rmoPurchaseOrderLineID"]).ToSql() + "\t" + Convert.ToDouble(dataRow3["rmoPurchaseOrderComponentID"]).ToSql()))
				{
					dataRow3["rmoPurchaseOrderComponentID"] = poMatcherDictionary[Convert.ToString(dataRow3["rmoPurchaseOrderID"]).Trim() + "\t" + Convert.ToDouble(dataRow3["rmoPurchaseOrderLineID"]).ToSql() + "\t" + Convert.ToDouble(dataRow3["rmoPurchaseOrderComponentID"]).ToSql()];
				}
				receiptMatcherDictionary.Add(receiptComponentSourceRow["rmoReceiptID"].ToString().Trim() + "\t" + Convert.ToDouble(receiptComponentSourceRow["rmoReceiptLineID"]).ToSql() + "\t" + Convert.ToDouble(receiptComponentSourceRow["rmoReceiptComponentID"]).ToSql(), dataRow3["rmoReceiptComponentID"].ToString());
				if (Convert.ToString(receiptComponentSourceRow["rmoReverseReceiptID"]).Trim() != "" && Convert.ToDouble(receiptComponentSourceRow["rmoReverseReceiptLineID"]) != 0.0 && Convert.ToDouble(receiptComponentSourceRow["rmoReverseReceiptComponentID"]) != 0.0)
				{
					receiptReversalMatcherDictionary.Add(receiptComponentSourceRow["rmoReverseReceiptID"].ToString().Trim() + "\t" + Convert.ToDouble(receiptComponentSourceRow["rmoReverseReceiptLineID"]).ToSql() + "\t" + Convert.ToDouble(receiptComponentSourceRow["rmoReverseReceiptComponentID"]).ToSql(), dataRow3["rmoReceiptComponentID"].ToString());
				}
				sourceTableUniqueIdMatcherDictionary.Add(receiptComponentSourceRow["rmoUniqueID"].ToString().Trim(), dataRow3["rmoUniqueID"].ToString());
				if (nSplitCosts == SplitCostOption.SplitCostsBasedOnQuantity)
				{
					dataRow3["rmoAdditionalQuantity"] = Math.Round(Convert.ToDouble(dataRow2["rmoAdditionalQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
					dataRow3["rmoInvParentQuantity"] = Math.Round(Convert.ToDouble(dataRow2["rmoInvParentQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
					dataRow3["rmoInvQuantityReceived"] = Math.Round(Convert.ToDouble(dataRow2["rmoInvQuantityReceived"]) * initialDestPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
					dataRow3["rmoJobParentQuantity"] = Math.Round(Convert.ToDouble(dataRow2["rmoJobParentQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
					dataRow3["rmoJobQuantityReceived"] = Math.Round(Convert.ToDouble(dataRow2["rmoJobQuantityReceived"]) * initialDestPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
					dataRow3["rmoExtendedCostBase"] = Math.Round(Convert.ToDouble(dataRow2["rmoExtendedCostBase"]) * initialDestPercent, 2);
					dataRow3["rmoExtendedCostForeign"] = Math.Round(Convert.ToDouble(dataRow2["rmoExtendedCostForeign"]) * initialDestPercent, 2);
					receiptComponentSourceRow["rmoAdditionalQuantity"] = Math.Round(Convert.ToDouble(receiptComponentSourceRow["rmoAdditionalQuantity"]) - Convert.ToDouble(dataRow3["rmoAdditionalQuantity"]), database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
					receiptComponentSourceRow["rmoInvParentQuantity"] = Math.Round(Convert.ToDouble(receiptComponentSourceRow["rmoInvParentQuantity"]) - Convert.ToDouble(dataRow3["rmoInvParentQuantity"]), database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
					receiptComponentSourceRow["rmoInvQuantityReceived"] = Math.Round(Convert.ToDouble(receiptComponentSourceRow["rmoInvQuantityReceived"]) - Convert.ToDouble(dataRow3["rmoInvQuantityReceived"]), database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
					receiptComponentSourceRow["rmoJobParentQuantity"] = Math.Round(Convert.ToDouble(receiptComponentSourceRow["rmoJobParentQuantity"]) - Convert.ToDouble(dataRow3["rmoJobParentQuantity"]), database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
					receiptComponentSourceRow["rmoJobQuantityReceived"] = Math.Round(Convert.ToDouble(receiptComponentSourceRow["rmoJobQuantityReceived"]) - Convert.ToDouble(dataRow3["rmoJobQuantityReceived"]), database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
					receiptComponentSourceRow["rmoExtendedCostBase"] = Math.Round(Convert.ToDouble(receiptComponentSourceRow["rmoExtendedCostBase"]) - Convert.ToDouble(dataRow3["rmoExtendedCostBase"]), 2);
					receiptComponentSourceRow["rmoExtendedCostForeign"] = Math.Round(Convert.ToDouble(receiptComponentSourceRow["rmoExtendedCostForeign"]) - Convert.ToDouble(dataRow3["rmoExtendedCostForeign"]), 2);
				}
				else
				{
					receiptComponentSourceRow["rmoAdditionalQuantity"] = 0;
					receiptComponentSourceRow["rmoInvParentQuantity"] = 0;
					receiptComponentSourceRow["rmoInvQuantityReceived"] = 0;
					receiptComponentSourceRow["rmoJobParentQuantity"] = 0;
					receiptComponentSourceRow["rmoJobQuantityReceived"] = 0;
					receiptComponentSourceRow["rmoExtendedCostBase"] = 0;
					receiptComponentSourceRow["rmoExtendedCostForeign"] = 0;
				}
				dataRow3["rmoQuantityToInspect"] = 0;
				dataRow3["rmoInspParentQuantity"] = 0;
				AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "ReceiptComponents", receiptComponentSourceRow["rmoUniqueID"], "ReceiptComponents", dataRow3["rmoUniqueID"], nJobSplitLogID);
			}
		}
		catch
		{
			throw;
		}
	}

	private void SplitAndTransferPartTransactions(M1Database database, SqlTransaction transaction, DataRow partTransactionPreSplitSource, DataRow partTransactionsSourceTableRow, DataTable partTransactionsDestTable, DataRow[] partTransactionsCostsSourceTableRows, DataTable partTransactionsCostsDestTable, DataTable glJournalLinesDestTable, string targetJobId, int nNewAsm, double targetPercent, ref int partTransactionId, Dictionary<string, string> sourceTableUniqueIdMatcherDictionary, DataTable jobSplitLogLinesTable, int jobSplitLogId, SplitCostOption splitCostOption)
	{
		DataRow dataRow = partTransactionsDestTable.AddBlankRow();
		dataRow["imtCreatedBy"] = database.User.ID;
		dataRow["imtCreatedDate"] = DateTime.Now;
		CopyAllFieldsToNewRow(partTransactionPreSplitSource, dataRow);
		partTransactionId++;
		dataRow["imtPartTransactionID"] = partTransactionId;
		for (int i = 0; i < glJournalLinesDestTable.Rows.Count; i++)
		{
			DataRow dataRow2 = glJournalLinesDestTable.Rows[i];
			for (int j = 0; j < UpdatePartTransactionID[targetJobId].GllGLJournalID.Count; j++)
			{
				if ((int)dataRow2["gllGLJournalID"] == UpdatePartTransactionID[targetJobId].GllGLJournalID[j] && (int)dataRow2["gllGLJournalLineID"] == UpdatePartTransactionID[targetJobId].GllGLJournalLineID[j] && (int)partTransactionPreSplitSource["imtPartTransactionID"] == UpdatePartTransactionID[targetJobId].GllSourcePartTransaction[j])
				{
					dataRow2["gllPartTransactionID"] = partTransactionId;
				}
			}
		}
		dataRow["imtJobID"] = targetJobId;
		dataRow["imtJobAssemblyID"] = nNewAsm;
		if (Guid.TryParse(dataRow["imtTableUniqueID"].ToString().Trim(), out var _) && sourceTableUniqueIdMatcherDictionary.ContainsKey(Convert.ToString(partTransactionsSourceTableRow["imtTableUniqueID"]).Trim()))
		{
			dataRow["imtTableUniqueID"] = sourceTableUniqueIdMatcherDictionary[Convert.ToString(partTransactionsSourceTableRow["imtTableUniqueID"]).Trim()];
		}
		switch (splitCostOption)
		{
		case SplitCostOption.SplitCostsBasedOnQuantity:
			dataRow["imtInventoryQuantityReceived"] = Math.Round(Convert.ToDouble(dataRow["imtInventoryQuantityReceived"]) * targetPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
			partTransactionsSourceTableRow["imtInventoryQuantityReceived"] = Math.Round(Convert.ToDouble(partTransactionsSourceTableRow["imtInventoryQuantityReceived"]) - Convert.ToDouble(dataRow["imtInventoryQuantityReceived"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
			break;
		case SplitCostOption.MoveCostsToTargetJob:
			partTransactionsSourceTableRow["imtInventoryQuantityReceived"] = 0;
			break;
		}
		foreach (DataRow dataRow3 in partTransactionsCostsSourceTableRows)
		{
			DataRow sourceTableRow = _partTransactionCostsPreSplitSource.Select("intUniqueID = " + dataRow3["intUniqueID"].ToLinq()).FirstOrDefault();
			DataRow dataRow4 = partTransactionsCostsDestTable.AddBlankRow();
			dataRow4["intCreatedBy"] = database.User.ID;
			dataRow4["intCreatedDate"] = DateTime.Now;
			CopyAllFieldsToNewRow(sourceTableRow, dataRow4);
			dataRow4["intPartTransactionID"] = dataRow["imtPartTransactionID"];
			switch (splitCostOption)
			{
			case SplitCostOption.SplitCostsBasedOnQuantity:
				dataRow4["intQuantity"] = Math.Round(Convert.ToDouble(dataRow4["intQuantity"]) * targetPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow3["intQuantity"] = Math.Round(Convert.ToDouble(dataRow3["intQuantity"]) - Convert.ToDouble(dataRow4["intQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				break;
			case SplitCostOption.MoveCostsToTargetJob:
				dataRow3["intQuantity"] = 0;
				break;
			}
		}
		AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "PartTransactions", partTransactionsSourceTableRow["imtUniqueID"], "PartTransactions", dataRow["imtUniqueID"], jobSplitLogId);
	}

	private void SplitAndTransferSerialNumberTransactions(M1Database database, SqlTransaction transaction, DataRow serialNumberTransactionsSourceTableRow, DataTable serialNumberTransactionsDestTable, string cNewJob, int nNewAsm, double nSourceTablePercent, double nDestPercent, Dictionary<string, string> sourceTableUniqueIdMatcherDictionary, ref int nSerialNumberTransactionID, DataTable jobSplitLogLinesTable, int nJobSplitLogID)
	{
		try
		{
			DataRow dataRow = serialNumberTransactionsDestTable.AddBlankRow();
			dataRow["sntCreatedBy"] = database.User.ID;
			dataRow["sntCreatedDate"] = DateTime.Now;
			CopyAllFieldsToNewRow(serialNumberTransactionsSourceTableRow, dataRow);
			dataRow["sntJobID"] = cNewJob;
			dataRow["sntJobAssemblyID"] = nNewAsm;
			nSerialNumberTransactionID++;
			dataRow["sntSerialNumberTransactionID"] = nSerialNumberTransactionID;
			if (Guid.TryParse(dataRow["sntTableUniqueID"].ToString().Trim(), out var _) && Convert.ToString(dataRow["sntTableName"]).Trim() != "" && sourceTableUniqueIdMatcherDictionary.ContainsKey(Convert.ToString(serialNumberTransactionsSourceTableRow["sntTableUniqueID"]).Trim()))
			{
				dataRow["sntTableUniqueID"] = sourceTableUniqueIdMatcherDictionary[Convert.ToString(serialNumberTransactionsSourceTableRow["sntTableUniqueID"]).Trim()];
			}
			dataRow["sntQuantity"] = Math.Round(Convert.ToDouble(dataRow["sntQuantity"]) * nDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
			serialNumberTransactionsSourceTableRow["sntQuantity"] = Math.Round(Convert.ToDouble(serialNumberTransactionsSourceTableRow["sntQuantity"]) * nSourceTablePercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
			AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "SerialNumberTransactions", serialNumberTransactionsSourceTableRow["sntUniqueID"], "SerialNumberTransactions", dataRow["sntUniqueID"], nJobSplitLogID);
		}
		catch
		{
			throw;
		}
	}

	private void SplitAndTransferLotNumberTransactions(M1Database database, SqlTransaction transaction, DataRow lotNumberTransactionsSourceTableRow, DataTable lotNumberTransactionsDestTable, string cNewJob, int nNewAsm, double nSourceTablePercent, double nDestPercent, Dictionary<string, string> sourceTableUniqueIdMatcherDictionary, ref int nLotNumberTransactionID, DataTable jobSplitLogLinesTable, int nJobSplitLogID)
	{
		try
		{
			DataRow dataRow = lotNumberTransactionsDestTable.AddBlankRow();
			dataRow["abtCreatedBy"] = database.User.ID;
			dataRow["abtCreatedDate"] = DateTime.Now;
			CopyAllFieldsToNewRow(lotNumberTransactionsSourceTableRow, dataRow);
			dataRow["abtJobID"] = cNewJob;
			dataRow["abtJobAssemblyID"] = nNewAsm;
			nLotNumberTransactionID++;
			dataRow["abtLotNumberTransactionID"] = nLotNumberTransactionID;
			if (Guid.TryParse(dataRow["abtTableUniqueID"].ToString().Trim(), out var _) && Convert.ToString(dataRow["abtTableName"]).Trim() != "" && sourceTableUniqueIdMatcherDictionary.ContainsKey(Convert.ToString(lotNumberTransactionsSourceTableRow["abtTableUniqueID"]).Trim()))
			{
				dataRow["abtTableUniqueID"] = sourceTableUniqueIdMatcherDictionary[Convert.ToString(lotNumberTransactionsSourceTableRow["abtTableUniqueID"]).Trim()];
			}
			dataRow["abtQuantity"] = Math.Round(Convert.ToDouble(dataRow["abtQuantity"]) * nDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
			lotNumberTransactionsSourceTableRow["abtQuantity"] = Math.Round(Convert.ToDouble(lotNumberTransactionsSourceTableRow["abtQuantity"]) * nSourceTablePercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
			AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "LotNumberTransactions", lotNumberTransactionsSourceTableRow["abtUniqueID"], "LotNumberTransactions", dataRow["abtUniqueID"], nJobSplitLogID);
		}
		catch
		{
			throw;
		}
	}

	private void SplitAndTransferTimecards(M1Database database, SqlTransaction transaction, DataRow initialTimecardLinesSourceTableRow, DataRow timecardLinesSourceTableRow, DataTable timecardLinesDestTable, SplitCostOption nSplitCosts, string cNewJob, int nNewAsm, double nDestPercent, DataTable jobSplitLogLinesTable, int nJobSplitLogID)
	{
		int nextLineForTable = GetNextLineForTable(database, transaction, timecardLinesDestTable, "TimecardLines", Convert.ToInt32(timecardLinesSourceTableRow["lmlTimecardID"]));
		DataRow dataRow = timecardLinesDestTable.AddBlankRow();
		dataRow["lmlCreatedBy"] = database.User.ID;
		dataRow["lmlCreatedDate"] = DateTime.Now;
		CopyAllFieldsToNewRow(timecardLinesSourceTableRow, dataRow);
		dataRow["lmlJobID"] = cNewJob;
		dataRow["lmlJobAssemblyID"] = nNewAsm;
		dataRow["lmlTimecardLineID"] = nextLineForTable;
		switch (nSplitCosts)
		{
		case SplitCostOption.MoveCostsToTargetJob:
			dataRow["lmlGoodQuantity"] = Convert.ToDouble(timecardLinesSourceTableRow["lmlGoodQuantity"]);
			dataRow["lmlScrapQuantity"] = 0.0;
			dataRow["lmlReworkQuantity"] = 0.0;
			dataRow["lmlSetupPercentCompleted"] = Convert.ToInt32(timecardLinesSourceTableRow["lmlSetupPercentCompleted"]);
			dataRow["lmlLaborCost"] = Convert.ToDouble(timecardLinesSourceTableRow["lmlLaborCost"]);
			dataRow["lmlOverheadCost"] = Convert.ToDouble(timecardLinesSourceTableRow["lmlOverheadCost"]);
			dataRow["lmlLaborHours"] = Convert.ToDouble(timecardLinesSourceTableRow["lmlLaborHours"]);
			dataRow["lmlMachineHours"] = Convert.ToDouble(timecardLinesSourceTableRow["lmlMachineHours"]);
			timecardLinesSourceTableRow["lmlGoodQuantity"] = 0.0;
			timecardLinesSourceTableRow["lmlSetupPercentCompleted"] = 0;
			timecardLinesSourceTableRow["lmlLaborCost"] = 0.0;
			timecardLinesSourceTableRow["lmlOverheadCost"] = 0.0;
			timecardLinesSourceTableRow["lmlLaborHours"] = 0.0;
			timecardLinesSourceTableRow["lmlMachineHours"] = 0.0;
			break;
		case SplitCostOption.SplitCostsBasedOnQuantity:
			dataRow["lmlGoodQuantity"] = Convert.ToDouble(initialTimecardLinesSourceTableRow["lmlGoodQuantity"]) * nDestPercent;
			dataRow["lmlScrapQuantity"] = 0.0;
			dataRow["lmlReworkQuantity"] = 0.0;
			dataRow["lmlSetupPercentCompleted"] = Convert.ToInt32(Convert.ToDouble(initialTimecardLinesSourceTableRow["lmlSetupPercentCompleted"]) * nDestPercent);
			dataRow["lmlLaborCost"] = Convert.ToDouble(initialTimecardLinesSourceTableRow["lmlLaborCost"]) * nDestPercent;
			dataRow["lmlOverheadCost"] = Convert.ToDouble(initialTimecardLinesSourceTableRow["lmlOverheadCost"]) * nDestPercent;
			dataRow["lmlLaborHours"] = Convert.ToDouble(initialTimecardLinesSourceTableRow["lmlLaborHours"]) * nDestPercent;
			dataRow["lmlMachineHours"] = Convert.ToDouble(initialTimecardLinesSourceTableRow["lmlMachineHours"]) * nDestPercent;
			timecardLinesSourceTableRow["lmlGoodQuantity"] = Convert.ToDouble(timecardLinesSourceTableRow["lmlGoodQuantity"]) - Convert.ToDouble(dataRow["lmlGoodQuantity"]);
			timecardLinesSourceTableRow["lmlSetupPercentCompleted"] = Convert.ToInt32(Convert.ToDouble(timecardLinesSourceTableRow["lmlSetupPercentCompleted"]) - (double)Convert.ToInt32(dataRow["lmlSetupPercentCompleted"]));
			timecardLinesSourceTableRow["lmlLaborCost"] = Convert.ToDouble(timecardLinesSourceTableRow["lmlLaborCost"]) - Convert.ToDouble(dataRow["lmlLaborCost"]);
			timecardLinesSourceTableRow["lmlOverheadCost"] = Convert.ToDouble(timecardLinesSourceTableRow["lmlOverheadCost"]) - Convert.ToDouble(dataRow["lmlOverheadCost"]);
			timecardLinesSourceTableRow["lmlLaborHours"] = Convert.ToDouble(timecardLinesSourceTableRow["lmlLaborHours"]) - Convert.ToDouble(dataRow["lmlLaborHours"]);
			timecardLinesSourceTableRow["lmlMachineHours"] = Convert.ToDouble(timecardLinesSourceTableRow["lmlMachineHours"]) - Convert.ToDouble(dataRow["lmlMachineHours"]);
			break;
		}
		AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "TimecardLines", timecardLinesSourceTableRow["lmlUniqueID"], "TimecardLines", dataRow["lmlUniqueID"], nJobSplitLogID);
	}

	private void SplitAndTransferAPInvoiceLines(M1Database database, SqlTransaction transaction, DataRow apInvoiceLinesPreSplitSource, DataRow aPInvoiceLinesRowSource, DataTable aPInvoiceLinesTableDest, DataRow[] foundApInvoiceExpenseAccountsRowsSource, DataTable aPInvoiceExpenseAccountsTableDest, string cNewJob, int nNewAsm, double nSourcePercent, double nDestPercent, SplitCostOption nSplitCosts, Dictionary<string, string> poMatcherDictionary, Dictionary<string, string> receiptMatcherDictionary, Dictionary<string, string> sourceTableUniqueIdMatcherDictionary, DataTable jobSplitLogLinesTable, int nJobSplitLogID)
	{
		try
		{
			int nextLineForTable = GetNextLineForTable(database, transaction, aPInvoiceLinesTableDest, "APInvoiceLines", Convert.ToString(aPInvoiceLinesRowSource["aplAPInvoiceID"]));
			DataRow dataRow = aPInvoiceLinesTableDest.AddBlankRow();
			dataRow["aplCreatedBy"] = database.User.ID;
			dataRow["aplCreatedDate"] = DateTime.Now;
			CopyAllFieldsToNewRow(apInvoiceLinesPreSplitSource, dataRow);
			dataRow["aplJobID"] = cNewJob;
			dataRow["aplJobAssemblyID"] = nNewAsm;
			dataRow["aplAPInvoiceLineID"] = nextLineForTable;
			dataRow["aplPurchaseUnitCostBase"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplPurchaseUnitCostBase"]);
			dataRow["aplPurchaseUnitCostForeign"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplPurchaseUnitCostForeign"]);
			dataRow["aplRetentionPercent"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplRetentionPercent"]);
			if (Convert.ToString(dataRow["aplPurchaseOrderID"]).Trim() != "" && Convert.ToDouble(dataRow["aplPurchaseOrderLineID"]) != 0.0 && poMatcherDictionary.ContainsKey(Convert.ToString(dataRow["aplPurchaseOrderID"]).Trim() + "\t" + Convert.ToDouble(dataRow["aplPurchaseOrderLineID"]).ToSql()))
			{
				dataRow["aplPurchaseOrderLineID"] = poMatcherDictionary[Convert.ToString(dataRow["aplPurchaseOrderID"]).Trim() + "\t" + Convert.ToDouble(dataRow["aplPurchaseOrderLineID"]).ToSql()];
			}
			if (Convert.ToString(dataRow["aplReceiptID"]).Trim() != "" && Convert.ToDouble(dataRow["aplReceiptLineID"]) != 0.0 && receiptMatcherDictionary.ContainsKey(Convert.ToString(dataRow["aplReceiptID"]).Trim() + "\t" + Convert.ToDouble(dataRow["aplReceiptLineID"]).ToSql()))
			{
				dataRow["aplReceiptLineID"] = receiptMatcherDictionary[Convert.ToString(dataRow["aplReceiptID"]).Trim() + "\t" + Convert.ToDouble(dataRow["aplReceiptLineID"]).ToSql()];
			}
			switch (nSplitCosts)
			{
			case SplitCostOption.MoveCostsToTargetJob:
				dataRow["aplPurchaseQuantity"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplPurchaseQuantity"]);
				dataRow["aplReceivedQuantity"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplReceivedQuantity"]);
				dataRow["aplSetupChargeBase"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplSetupChargeBase"]);
				dataRow["aplSetupChargeForeign"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplSetupChargeForeign"]);
				dataRow["aplTaxAmountBase"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplTaxAmountBase"]);
				dataRow["aplTaxAmountForeign"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplTaxAmountForeign"]);
				dataRow["aplSecondTaxAmountBase"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplSecondTaxAmountBase"]);
				dataRow["aplSecondTaxAmountForeign"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplSecondTaxAmountForeign"]);
				dataRow["aplExtendedCostBase"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplExtendedCostBase"]);
				dataRow["aplExtendedCostForeign"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplExtendedCostForeign"]);
				dataRow["aplTotalExtendedCostBase"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplTotalExtendedCostBase"]);
				dataRow["aplTotalExtendedCostForeign"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplTotalExtendedCostForeign"]);
				dataRow["aplRetentionAmountBase"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplRetentionAmountBase"]);
				dataRow["aplRetentionAmountForeign"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplRetentionAmountForeign"]);
				aPInvoiceLinesRowSource["aplPurchaseQuantity"] = 0.0;
				aPInvoiceLinesRowSource["aplReceivedQuantity"] = 0.0;
				aPInvoiceLinesRowSource["aplSetupChargeBase"] = 0.0;
				aPInvoiceLinesRowSource["aplSetupChargeForeign"] = 0.0;
				aPInvoiceLinesRowSource["aplTaxAmountBase"] = 0.0;
				aPInvoiceLinesRowSource["aplTaxAmountForeign"] = 0.0;
				aPInvoiceLinesRowSource["aplSecondTaxAmountBase"] = 0.0;
				aPInvoiceLinesRowSource["aplSecondTaxAmountForeign"] = 0.0;
				aPInvoiceLinesRowSource["aplExtendedCostBase"] = 0.0;
				aPInvoiceLinesRowSource["aplExtendedCostForeign"] = 0.0;
				aPInvoiceLinesRowSource["aplTotalExtendedCostBase"] = 0.0;
				aPInvoiceLinesRowSource["aplTotalExtendedCostForeign"] = 0.0;
				aPInvoiceLinesRowSource["aplRetentionAmountBase"] = 0.0;
				aPInvoiceLinesRowSource["aplRetentionAmountForeign"] = 0.0;
				break;
			case SplitCostOption.SplitCostsBasedOnQuantity:
				dataRow["aplPurchaseQuantity"] = Convert.ToDouble(dataRow["aplPurchaseQuantity"]) * nDestPercent;
				dataRow["aplReceivedQuantity"] = Convert.ToDouble(dataRow["aplReceivedQuantity"]) * nDestPercent;
				dataRow["aplSetupChargeBase"] = Convert.ToDouble(dataRow["aplSetupChargeBase"]) * nDestPercent;
				dataRow["aplSetupChargeForeign"] = Convert.ToDouble(dataRow["aplSetupChargeForeign"]) * nDestPercent;
				dataRow["aplTaxAmountBase"] = Convert.ToDouble(dataRow["aplTaxAmountBase"]) * nDestPercent;
				dataRow["aplTaxAmountForeign"] = Convert.ToDouble(dataRow["aplTaxAmountForeign"]) * nDestPercent;
				dataRow["aplSecondTaxAmountBase"] = Convert.ToDouble(dataRow["aplSecondTaxAmountBase"]) * nDestPercent;
				dataRow["aplSecondTaxAmountForeign"] = Convert.ToDouble(dataRow["aplSecondTaxAmountForeign"]) * nDestPercent;
				dataRow["aplExtendedCostBase"] = Convert.ToDouble(dataRow["aplExtendedCostBase"]) * nDestPercent;
				dataRow["aplExtendedCostForeign"] = Convert.ToDouble(dataRow["aplExtendedCostForeign"]) * nDestPercent;
				dataRow["aplTotalExtendedCostBase"] = Convert.ToDouble(dataRow["aplTotalExtendedCostBase"]) * nDestPercent;
				dataRow["aplTotalExtendedCostForeign"] = Convert.ToDouble(dataRow["aplTotalExtendedCostForeign"]) * nDestPercent;
				dataRow["aplRetentionAmountBase"] = Convert.ToDouble(dataRow["aplRetentionAmountBase"]) * nDestPercent;
				dataRow["aplRetentionAmountForeign"] = Convert.ToDouble(dataRow["aplRetentionAmountForeign"]) * nDestPercent;
				aPInvoiceLinesRowSource["aplPurchaseQuantity"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplPurchaseQuantity"]) - Convert.ToDouble(dataRow["aplPurchaseQuantity"]);
				aPInvoiceLinesRowSource["aplReceivedQuantity"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplReceivedQuantity"]) - Convert.ToDouble(dataRow["aplReceivedQuantity"]);
				aPInvoiceLinesRowSource["aplSetupChargeBase"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplSetupChargeBase"]) - Convert.ToDouble(dataRow["aplSetupChargeBase"]);
				aPInvoiceLinesRowSource["aplSetupChargeForeign"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplSetupChargeForeign"]) - Convert.ToDouble(dataRow["aplSetupChargeForeign"]);
				aPInvoiceLinesRowSource["aplTaxAmountBase"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplTaxAmountBase"]) - Convert.ToDouble(dataRow["aplTaxAmountBase"]);
				aPInvoiceLinesRowSource["aplTaxAmountForeign"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplTaxAmountForeign"]) - Convert.ToDouble(dataRow["aplTaxAmountForeign"]);
				aPInvoiceLinesRowSource["aplSecondTaxAmountBase"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplSecondTaxAmountBase"]) - Convert.ToDouble(dataRow["aplSecondTaxAmountBase"]);
				aPInvoiceLinesRowSource["aplSecondTaxAmountForeign"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplSecondTaxAmountForeign"]) - Convert.ToDouble(dataRow["aplSecondTaxAmountForeign"]);
				aPInvoiceLinesRowSource["aplExtendedCostBase"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplExtendedCostBase"]) - Convert.ToDouble(dataRow["aplExtendedCostBase"]);
				aPInvoiceLinesRowSource["aplExtendedCostForeign"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplExtendedCostForeign"]) - Convert.ToDouble(dataRow["aplExtendedCostForeign"]);
				aPInvoiceLinesRowSource["aplTotalExtendedCostBase"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplTotalExtendedCostBase"]) - Convert.ToDouble(dataRow["aplTotalExtendedCostBase"]);
				aPInvoiceLinesRowSource["aplTotalExtendedCostForeign"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplTotalExtendedCostForeign"]) - Convert.ToDouble(dataRow["aplTotalExtendedCostForeign"]);
				aPInvoiceLinesRowSource["aplRetentionAmountBase"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplRetentionAmountBase"]) - Convert.ToDouble(dataRow["aplRetentionAmountBase"]);
				aPInvoiceLinesRowSource["aplRetentionAmountForeign"] = Convert.ToDouble(aPInvoiceLinesRowSource["aplRetentionAmountForeign"]) - Convert.ToDouble(dataRow["aplRetentionAmountForeign"]);
				break;
			}
			AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "APInvoiceLines", aPInvoiceLinesRowSource["aplUniqueID"], "APInvoiceLines", dataRow["aplUniqueID"], nJobSplitLogID);
			foreach (DataRow dataRow2 in foundApInvoiceExpenseAccountsRowsSource)
			{
				DataRow sourceTableRow = _apExpenseAccountsPreSplitSource.Select("apxAPInvoiceID = " + dataRow2["apxAPInvoiceID"].ToLinq() + " \r\n                                        And apxAPInvoiceLineID = " + dataRow2["apxAPInvoiceLineID"].ToLinq() + " \r\n                                        And apxAPInvoiceExpenseAccountID = " + dataRow2["apxAPInvoiceExpenseAccountID"].ToLinq()).FirstOrDefault();
				DataRow dataRow3 = aPInvoiceExpenseAccountsTableDest.AddBlankRow();
				dataRow3["apxCreatedBy"] = database.User.ID;
				dataRow3["apxCreatedDate"] = DateTime.Now;
				CopyAllFieldsToNewRow(sourceTableRow, dataRow3);
				dataRow3["apxAPInvoiceLineID"] = nextLineForTable;
				dataRow3["apxPercent"] = Convert.ToDouble(dataRow2["apxPercent"]);
				sourceTableUniqueIdMatcherDictionary.Add(dataRow2["apxUniqueID"].ToString().Trim(), dataRow3["apxUniqueID"].ToString());
				if (nSplitCosts == SplitCostOption.MoveCostsToTargetJob)
				{
					dataRow3["apxAmount"] = Convert.ToDouble(dataRow2["apxAmount"]);
					dataRow2["apxAmount"] = 0.0;
				}
				else
				{
					dataRow3["apxAmount"] = Convert.ToDouble(dataRow3["apxAmount"]) * nDestPercent;
					dataRow2["apxAmount"] = Convert.ToDouble(dataRow2["apxAmount"]) - Convert.ToDouble(dataRow3["apxAmount"]);
				}
				AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "APInvoiceExpenseAccounts", aPInvoiceLinesRowSource["aplUniqueID"], "APInvoiceExpenseAccounts", dataRow["aplUniqueID"], nJobSplitLogID);
			}
		}
		catch
		{
			throw;
		}
	}

	private void RecalculateTargetMaterialIssueLines(DataRow targetMaterialIssueLine, DataRow sourceMaterialIssueLine, double nDestPercent, byte decimals, SplitCostOption splitCostOption)
	{
		targetMaterialIssueLine["injEstimatedQuantity"] = Math.Round(Convert.ToDouble(sourceMaterialIssueLine["injEstimatedQuantity"]) * nDestPercent, decimals);
		targetMaterialIssueLine["injJobOpenQuantity"] = Math.Round(Convert.ToDouble(sourceMaterialIssueLine["injJobOpenQuantity"]) * nDestPercent, decimals);
		switch (splitCostOption)
		{
		case SplitCostOption.SplitCostsBasedOnQuantity:
			targetMaterialIssueLine["injJobMatIssueQuantity"] = Math.Round(Convert.ToDouble(sourceMaterialIssueLine["injJobMatIssueQuantity"]) * nDestPercent, decimals);
			targetMaterialIssueLine["injJobAsmIssueQuantity"] = Math.Round(Convert.ToDouble(sourceMaterialIssueLine["injJobAsmIssueQuantity"]) * nDestPercent, decimals);
			targetMaterialIssueLine["injJobMatReturnIssueQuantity"] = Math.Round(Convert.ToDouble(sourceMaterialIssueLine["injJobMatReturnIssueQuantity"]) * nDestPercent, decimals);
			break;
		}
		targetMaterialIssueLine["injInvIssueQuantity"] = 0;
		targetMaterialIssueLine["injInvScrapQuantity"] = 0;
		targetMaterialIssueLine["injJobMatScrapQuantity"] = 0;
		targetMaterialIssueLine["injJobAsmScrapQuantity"] = 0;
		targetMaterialIssueLine["injJobMatReturnScrapQuantity"] = 0;
	}

	private void RecalculateSourceMaterialIssueLines(DataRow targetMaterialIssueLine, DataRow sourceMaterialIssueLine, double nSourceTablePercent, byte decimals, SplitCostOption splitCostOption)
	{
		sourceMaterialIssueLine["injEstimatedQuantity"] = sourceMaterialIssueLine.Field<decimal>("injEstimatedQuantity") - targetMaterialIssueLine.Field<decimal>("injEstimatedQuantity");
		sourceMaterialIssueLine["injJobOpenQuantity"] = sourceMaterialIssueLine.Field<decimal>("injJobOpenQuantity") - targetMaterialIssueLine.Field<decimal>("injJobOpenQuantity");
		switch (splitCostOption)
		{
		case SplitCostOption.SplitCostsBasedOnQuantity:
			sourceMaterialIssueLine["injJobMatIssueQuantity"] = sourceMaterialIssueLine.Field<decimal>("injJobMatIssueQuantity") - targetMaterialIssueLine.Field<decimal>("injJobMatIssueQuantity");
			sourceMaterialIssueLine["injJobAsmIssueQuantity"] = sourceMaterialIssueLine.Field<decimal>("injJobAsmIssueQuantity") - targetMaterialIssueLine.Field<decimal>("injJobAsmIssueQuantity");
			sourceMaterialIssueLine["injJobMatReturnIssueQuantity"] = sourceMaterialIssueLine.Field<decimal>("injJobMatReturnIssueQuantity") - targetMaterialIssueLine.Field<decimal>("injJobMatReturnIssueQuantity");
			break;
		case SplitCostOption.MoveCostsToTargetJob:
			sourceMaterialIssueLine["injJobMatIssueQuantity"] = 0;
			sourceMaterialIssueLine["injJobAsmIssueQuantity"] = 0;
			sourceMaterialIssueLine["injJobMatReturnIssueQuantity"] = 0;
			break;
		case SplitCostOption.KeepCostsOnSourceJob:
			break;
		}
	}

	private void RecalculateTargetMaterialIssueComponents(DataRow targetMaterialIssueComponent, DataRow sourceMIC, double initialDestPercent, SplitCostOption splitCostOption, byte decimals)
	{
		targetMaterialIssueComponent["inkJobMatParentQuantity"] = Math.Round(Convert.ToDouble(sourceMIC["inkJobMatParentQuantity"]) * initialDestPercent, decimals);
		switch (splitCostOption)
		{
		case SplitCostOption.SplitCostsBasedOnQuantity:
			targetMaterialIssueComponent["inkAdditionalQuantity"] = Math.Round(Convert.ToDouble(sourceMIC["inkAdditionalQuantity"]) * initialDestPercent, decimals);
			targetMaterialIssueComponent["inkJobMatIssueQuantity"] = Math.Round(Convert.ToDouble(sourceMIC["inkJobMatIssueQuantity"]) * initialDestPercent, decimals);
			targetMaterialIssueComponent["inkJobMatReturnIssueQuantity"] = Math.Round(Convert.ToDouble(sourceMIC["inkJobMatReturnIssueQuantity"]) * initialDestPercent, decimals);
			break;
		}
		targetMaterialIssueComponent["inkInvParentQuantity"] = 0;
		targetMaterialIssueComponent["inkInvParentQuantityScrap"] = 0;
		targetMaterialIssueComponent["inkJobMatScrapQuantity"] = 0;
		targetMaterialIssueComponent["inkJobMatParentQuantityScrap"] = 0;
		targetMaterialIssueComponent["inkJobMatReturnScrapQuantity"] = 0;
		targetMaterialIssueComponent["inkJobMatParentReturnQtyScrap"] = 0;
	}

	private void RecalculateSourceMaterialIssueComponents(DataRow targetMaterialIssueComponent, DataRow sourceMaterialIssueComponent, SplitCostOption splitCostOption)
	{
		sourceMaterialIssueComponent["inkJobMatParentQuantity"] = sourceMaterialIssueComponent.Field<decimal>("inkJobMatParentQuantity") - targetMaterialIssueComponent.Field<decimal>("inkJobMatParentQuantity");
		sourceMaterialIssueComponent["inkAdditionalQuantity"] = sourceMaterialIssueComponent.Field<decimal>("inkAdditionalQuantity") - targetMaterialIssueComponent.Field<decimal>("inkAdditionalQuantity");
		switch (splitCostOption)
		{
		case SplitCostOption.SplitCostsBasedOnQuantity:
			sourceMaterialIssueComponent["inkJobMatIssueQuantity"] = sourceMaterialIssueComponent.Field<decimal>("inkJobMatIssueQuantity") - targetMaterialIssueComponent.Field<decimal>("inkJobMatIssueQuantity");
			sourceMaterialIssueComponent["inkJobMatReturnIssueQuantity"] = sourceMaterialIssueComponent.Field<decimal>("inkJobMatReturnIssueQuantity") - targetMaterialIssueComponent.Field<decimal>("inkJobMatReturnIssueQuantity");
			break;
		case SplitCostOption.MoveCostsToTargetJob:
			sourceMaterialIssueComponent["inkJobMatIssueQuantity"] = 0;
			sourceMaterialIssueComponent["inkJobMatReturnIssueQuantity"] = 0;
			break;
		case SplitCostOption.KeepCostsOnSourceJob:
			break;
		}
	}

	private void SplitAndTransferMaterialIssueLines(M1Database database, SqlTransaction transaction, DataRow sourceMaterialIssueLine, DataTable materialIssueLinesDestTable, DataRow[] foundMaterialIssueComponentsSourceRows, DataTable materialIssueComponentsDestTable, string cNewJob, int nNewAsm, double nSourceTablePercent, double initialDestPercent, double nDestPercent, Dictionary<string, string> sourceTableUniqueIdMatcherDictionary, Dictionary<string, string> materialIssueMatcherDictionary, DataTable jobSplitLogLinesTable, int nJobSplitLogID, SplitCostOption splitCostOption)
	{
		try
		{
			if (splitCostOption == SplitCostOption.KeepCostsOnSourceJob)
			{
				return;
			}
			DataRow sourceMaterialIssueLine2 = JobMaterialIssueLinesSource.Select("injMaterialIssueID='" + sourceMaterialIssueLine.Field<string>("injMaterialIssueID") + "' AND " + string.Format("injMaterialIssueLineID={0}", sourceMaterialIssueLine.Field<short>("injMaterialIssueLineID"))).FirstOrDefault();
			byte decimals = database.Props("DS").Field<byte>("xadInventoryQuantityDecimals");
			int nextLineForTable = GetNextLineForTable(database, transaction, materialIssueLinesDestTable, "MaterialIssueLines", sourceMaterialIssueLine["injMaterialIssueID"].ToString());
			DataRow dataRow = materialIssueLinesDestTable.AddBlankRow();
			CopyAllFieldsToNewRow(sourceMaterialIssueLine, dataRow);
			dataRow["injCreatedBy"] = database.User.ID;
			dataRow["injCreatedDate"] = DateTime.Now;
			dataRow["injJobID"] = cNewJob;
			dataRow["injJobAssemblyID"] = nNewAsm;
			dataRow["injMaterialIssueLineID"] = nextLineForTable;
			if (Convert.ToString(sourceMaterialIssueLine["injReverseMaterialIssueID"]).Trim() != "" && Convert.ToDouble(sourceMaterialIssueLine["injReverseMaterialIssueLineID"]) != 0.0)
			{
				materialIssueMatcherDictionary.Add(sourceMaterialIssueLine["injReverseMaterialIssueID"].ToString().Trim() + "\t" + Convert.ToDouble(sourceMaterialIssueLine["injReverseMaterialIssueLineID"]).ToSql(), dataRow["injMaterialIssueLineID"].ToString());
			}
			sourceTableUniqueIdMatcherDictionary.Add(sourceMaterialIssueLine["injUniqueID"].ToString().Trim(), dataRow["injUniqueID"].ToString());
			RecalculateTargetMaterialIssueLines(dataRow, sourceMaterialIssueLine2, initialDestPercent, decimals, splitCostOption);
			RecalculateSourceMaterialIssueLines(dataRow, sourceMaterialIssueLine, nSourceTablePercent, decimals, splitCostOption);
			AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "MaterialIssueLines", sourceMaterialIssueLine["injUniqueID"], "MaterialIssueLines", dataRow["injUniqueID"], nJobSplitLogID);
			foreach (DataRow dataRow2 in foundMaterialIssueComponentsSourceRows)
			{
				DataRow sourceMIC = JobMaterialIssueComponents.Select(string.Format("inkMaterialIssueID='{0}' AND inkMaterialIssueLineID={1} AND inkMaterialIssueComponentID={2}", dataRow2.Field<string>("inkMaterialIssueID"), dataRow2.Field<short>("inkMaterialIssueLineID"), dataRow2.Field<int>("inkMaterialIssueComponentID"))).FirstOrDefault();
				DataRow dataRow3 = materialIssueComponentsDestTable.AddBlankRow();
				CopyAllFieldsToNewRow(dataRow2, dataRow3);
				dataRow3["inkCreatedBy"] = database.User.ID;
				dataRow3["inkCreatedDate"] = DateTime.Now;
				dataRow3["inkMaterialIssueLineID"] = nextLineForTable;
				dataRow3["inkJobID"] = cNewJob;
				dataRow3["inkJobAssemblyID"] = nNewAsm;
				if (Convert.ToString(dataRow2["inkReverseMaterialIssueID"]).Trim() != "" && Convert.ToDouble(dataRow2["inkReverseMaterialIssueLineID"]) != 0.0 && Convert.ToDouble(dataRow2["inkReverseMaterialIssueCompID"]) != 0.0)
				{
					materialIssueMatcherDictionary.Add(dataRow2["inkReverseMaterialIssueID"].ToString().Trim() + "\t" + Convert.ToDouble(dataRow2["inkReverseMaterialIssueLineID"]).ToSql() + "\t" + Convert.ToDouble(dataRow2["inkReverseMaterialIssueCompID"]).ToSql(), dataRow3["inkMaterialIssueComponentID"].ToString());
				}
				sourceTableUniqueIdMatcherDictionary.Add(dataRow2["inkUniqueID"].ToString().Trim(), dataRow3["inkUniqueID"].ToString());
				RecalculateTargetMaterialIssueComponents(dataRow3, sourceMIC, initialDestPercent, splitCostOption, decimals);
				RecalculateSourceMaterialIssueComponents(dataRow3, dataRow2, splitCostOption);
				AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "MaterialIssueComponents", dataRow2["inkUniqueID"], "MaterialIssueComponents", dataRow3["inkUniqueID"], nJobSplitLogID);
			}
		}
		catch
		{
			throw;
		}
	}

	private void ProcessReversals(DataTable linesDestTable, DataTable componentsDestTable, Dictionary<string, string> matcherDictionary, ReversalFieldNames fieldNames)
	{
		try
		{
			if (!matcherDictionary.Any())
			{
				return;
			}
			foreach (DataRow row in linesDestTable.Rows)
			{
				if (!string.IsNullOrWhiteSpace(fieldNames.LineIdFieldName) && !string.IsNullOrWhiteSpace(fieldNames.LineLineFieldName))
				{
					if (Convert.ToString(row[fieldNames.LineIdFieldName]).Trim() != "" && Convert.ToDouble(row[fieldNames.LineLineFieldName]) != 0.0 && matcherDictionary.ContainsKey(Convert.ToString(row[fieldNames.LineIdFieldName]).Trim() + "\t" + Convert.ToDouble(row[fieldNames.LineLineFieldName]).ToSql()))
					{
						row[fieldNames.LineLineFieldName] = matcherDictionary[Convert.ToString(row[fieldNames.LineIdFieldName]).Trim() + "\t" + Convert.ToDouble(row[fieldNames.LineLineFieldName]).ToSql()];
					}
				}
				else if (Convert.ToString(row[fieldNames.LineIdFieldName]).Trim() != "" && matcherDictionary.ContainsKey(Convert.ToString(row[fieldNames.LineIdFieldName]).Trim()))
				{
					row[fieldNames.LineIdFieldName] = matcherDictionary[Convert.ToString(row[fieldNames.LineIdFieldName]).Trim()];
				}
			}
			foreach (DataRow row2 in componentsDestTable.Rows)
			{
				if (!string.IsNullOrWhiteSpace(fieldNames.ComponentIdFieldName) && !string.IsNullOrWhiteSpace(fieldNames.ComponentLineFieldName) && !string.IsNullOrWhiteSpace(fieldNames.ComponentComponentFieldName))
				{
					if (Convert.ToString(row2[fieldNames.ComponentIdFieldName]).Trim() != "" && Convert.ToDouble(row2[fieldNames.ComponentLineFieldName]) != 0.0 && matcherDictionary.ContainsKey(Convert.ToString(row2[fieldNames.ComponentIdFieldName]).Trim() + "\t" + Convert.ToDouble(row2[fieldNames.ComponentLineFieldName]).ToSql()))
					{
						row2[fieldNames.ComponentLineFieldName] = matcherDictionary[Convert.ToString(row2[fieldNames.ComponentIdFieldName]).Trim() + "\t" + Convert.ToDouble(row2[fieldNames.ComponentLineFieldName]).ToSql()];
					}
					if (Convert.ToString(row2[fieldNames.ComponentIdFieldName]).Trim() != "" && Convert.ToDouble(row2[fieldNames.ComponentLineFieldName]) != 0.0 && Convert.ToDouble(row2[fieldNames.ComponentComponentFieldName]) != 0.0 && matcherDictionary.ContainsKey(Convert.ToString(row2[fieldNames.ComponentIdFieldName]).Trim() + "\t" + Convert.ToDouble(row2[fieldNames.ComponentLineFieldName]).ToSql() + "\t" + Convert.ToDouble(row2[fieldNames.ComponentComponentFieldName]).ToSql()))
					{
						row2[fieldNames.ComponentComponentFieldName] = matcherDictionary[Convert.ToString(row2[fieldNames.ComponentIdFieldName]).Trim() + "\t" + Convert.ToDouble(row2[fieldNames.ComponentLineFieldName]).ToSql() + "\t" + Convert.ToDouble(row2[fieldNames.ComponentComponentFieldName]).ToSql()];
					}
				}
				else
				{
					if (Convert.ToString(row2[fieldNames.ComponentIdFieldName]).Trim() != "" && matcherDictionary.ContainsKey(Convert.ToString(row2[fieldNames.ComponentIdFieldName]).Trim()))
					{
						row2[fieldNames.ComponentIdFieldName] = matcherDictionary[Convert.ToString(row2[fieldNames.ComponentIdFieldName]).Trim()];
					}
					if (Convert.ToString(row2[fieldNames.ComponentIdFieldName]).Trim() != "" && Convert.ToDouble(row2[fieldNames.ComponentComponentFieldName]) != 0.0 && matcherDictionary.ContainsKey(Convert.ToString(row2[fieldNames.ComponentIdFieldName]).Trim() + "\t" + Convert.ToDouble(row2[fieldNames.ComponentComponentFieldName]).ToSql()))
					{
						row2[fieldNames.ComponentComponentFieldName] = matcherDictionary[Convert.ToString(row2[fieldNames.ComponentIdFieldName]).Trim() + "\t" + Convert.ToDouble(row2[fieldNames.ComponentComponentFieldName]).ToSql()];
					}
				}
			}
		}
		catch
		{
			throw;
		}
	}

	private void SplitAndTransferMfgReceipts(M1Database database, SqlTransaction transaction, DataRow preSplitMfgReceiptSourceRow, DataRow mfgReceiptsSourceRow, DataTable mfgReceiptsDestTable, DataRow[] initialFoundMfgReceiptComponentsSourceRows, DataRow[] foundMfgReceiptComponentsSourceRows, DataTable mfgReceiptComponentsDestTable, string cNewJob, int nNewAsm, double initialDestPercent, Dictionary<string, string> sourceTableUniqueIdMatcherDictionary, Dictionary<string, string> mfgReceiptMatcherDictionary, Dictionary<string, string> poMatcherDictionary, DataTable jobSplitLogLinesTable, int nJobSplitLogID, SplitCostOption nSplitCosts)
	{
		try
		{
			DataRow dataRow = mfgReceiptsDestTable.AddBlankRow();
			dataRow["rmmCreatedBy"] = database.User.ID;
			dataRow["rmmCreatedDate"] = DateTime.Now;
			CopyAllFieldsToNewRow(mfgReceiptsSourceRow, dataRow);
			dataRow["rmmJobID"] = cNewJob;
			dataRow["rmmJobAssemblyID"] = nNewAsm;
			dataRow["rmmMfgReceiptID"] = database.NextIDs.GetNextIDForTable("MfgReceipts", null, transaction);
			if (Convert.ToString(dataRow["rmmPurchaseOrderID"]).Trim() != "" && Convert.ToDouble(dataRow["rmmPurchaseOrderLineID"]) != 0.0 && poMatcherDictionary.ContainsKey(Convert.ToString(dataRow["rmmPurchaseOrderID"]).Trim() + "\t" + Convert.ToDouble(dataRow["rmmPurchaseOrderLineID"]).ToSql()))
			{
				dataRow["rmmPurchaseOrderLineID"] = poMatcherDictionary[Convert.ToString(dataRow["rmmPurchaseOrderID"]).Trim() + "\t" + Convert.ToDouble(dataRow["rmmPurchaseOrderLineID"]).ToSql()];
			}
			if (Convert.ToString(mfgReceiptsSourceRow["rmmReverseMfgReceiptID"]).Trim() != "")
			{
				mfgReceiptMatcherDictionary.Add(mfgReceiptsSourceRow["rmmReverseMfgReceiptID"].ToString().Trim(), dataRow["rmmMfgReceiptID"].ToString());
			}
			sourceTableUniqueIdMatcherDictionary.Add(mfgReceiptsSourceRow["rmmUniqueID"].ToString().Trim(), dataRow["rmmUniqueID"].ToString());
			if (nSplitCosts == SplitCostOption.SplitCostsBasedOnQuantity)
			{
				dataRow["rmmPurchaseQuantity"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmPurchaseQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				dataRow["rmmPOOpenQuantity"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmPOOpenQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				dataRow["rmmEstimatedQuantity"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmEstimatedQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmmJobOpenQuantity"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmJobOpenQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmmInventoryQuantity"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmInventoryQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmmProductionQuantity"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmProductionQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmmQuantityCompleted"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmQuantityCompleted"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmmQuantityReceivedToInventory"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmQuantityReceivedToInventory"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmmJobScrapQuantity"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmJobScrapQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmmPurchaseQuantityReceived"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmPurchaseQuantityReceived"]) * initialDestPercent, database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				dataRow["rmmSetupCharge"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmSetupCharge"]) * initialDestPercent, 2);
				dataRow["rmmInventoryQuantityReceived"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmInventoryQuantityReceived"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmmJobOprQuantityReceived"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmJobOprQuantityReceived"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmmQuantityToInspect"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmQuantityToInspect"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmmJobMatQuantityReceived"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmJobMatQuantityReceived"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmmExtendedCostBase"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmExtendedCostBase"]) * initialDestPercent, 2);
				dataRow["rmmJobAsmQuantityReceived"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmJobAsmQuantityReceived"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				dataRow["rmmTotalComponentCosts"] = Math.Round(Convert.ToDouble(preSplitMfgReceiptSourceRow["rmmTotalComponentCosts"]) * initialDestPercent, 2);
				mfgReceiptsSourceRow["rmmPurchaseQuantity"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmPurchaseQuantity"]) - Convert.ToDouble(dataRow["rmmPurchaseQuantity"]), database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				mfgReceiptsSourceRow["rmmPOOpenQuantity"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmPOOpenQuantity"]) - Convert.ToDouble(dataRow["rmmPOOpenQuantity"]), database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				mfgReceiptsSourceRow["rmmEstimatedQuantity"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmEstimatedQuantity"]) - Convert.ToDouble(dataRow["rmmEstimatedQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				mfgReceiptsSourceRow["rmmJobOpenQuantity"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmJobOpenQuantity"]) - Convert.ToDouble(dataRow["rmmJobOpenQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				mfgReceiptsSourceRow["rmmInventoryQuantity"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmInventoryQuantity"]) - Convert.ToDouble(dataRow["rmmInventoryQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				mfgReceiptsSourceRow["rmmProductionQuantity"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmProductionQuantity"]) - Convert.ToDouble(dataRow["rmmProductionQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				mfgReceiptsSourceRow["rmmQuantityCompleted"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmQuantityCompleted"]) - Convert.ToDouble(dataRow["rmmQuantityCompleted"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				mfgReceiptsSourceRow["rmmQuantityReceivedToInventory"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmQuantityReceivedToInventory"]) - Convert.ToDouble(dataRow["rmmQuantityReceivedToInventory"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				mfgReceiptsSourceRow["rmmJobScrapQuantity"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmJobScrapQuantity"]) - Convert.ToDouble(dataRow["rmmJobScrapQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				mfgReceiptsSourceRow["rmmPurchaseQuantityReceived"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmPurchaseQuantityReceived"]) - Convert.ToDouble(dataRow["rmmPurchaseQuantityReceived"]), database.Props("DS").Field<byte>("xadBuyQuantityDecimals"));
				mfgReceiptsSourceRow["rmmSetupCharge"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmSetupCharge"]) - Convert.ToDouble(dataRow["rmmSetupCharge"]), 2);
				mfgReceiptsSourceRow["rmmInventoryQuantityReceived"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmInventoryQuantityReceived"]) - Convert.ToDouble(dataRow["rmmInventoryQuantityReceived"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				mfgReceiptsSourceRow["rmmJobOprQuantityReceived"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmJobOprQuantityReceived"]) - Convert.ToDouble(dataRow["rmmJobOprQuantityReceived"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				mfgReceiptsSourceRow["rmmQuantityToInspect"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmQuantityToInspect"]) - Convert.ToDouble(dataRow["rmmQuantityToInspect"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				mfgReceiptsSourceRow["rmmJobMatQuantityReceived"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmJobMatQuantityReceived"]) - Convert.ToDouble(dataRow["rmmJobMatQuantityReceived"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				mfgReceiptsSourceRow["rmmExtendedCostBase"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmExtendedCostBase"]) - Convert.ToDouble(dataRow["rmmExtendedCostBase"]), 2);
				mfgReceiptsSourceRow["rmmJobAsmQuantityReceived"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmJobAsmQuantityReceived"]) - Convert.ToDouble(dataRow["rmmJobAsmQuantityReceived"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
				mfgReceiptsSourceRow["rmmTotalComponentCosts"] = Math.Round(Convert.ToDouble(mfgReceiptsSourceRow["rmmTotalComponentCosts"]) - Convert.ToDouble(dataRow["rmmTotalComponentCosts"]), 2);
			}
			else
			{
				mfgReceiptsSourceRow["rmmPurchaseQuantity"] = 0;
				mfgReceiptsSourceRow["rmmPOOpenQuantity"] = 0;
				mfgReceiptsSourceRow["rmmEstimatedQuantity"] = 0;
				mfgReceiptsSourceRow["rmmJobOpenQuantity"] = 0;
				mfgReceiptsSourceRow["rmmInventoryQuantity"] = 0;
				mfgReceiptsSourceRow["rmmProductionQuantity"] = 0;
				mfgReceiptsSourceRow["rmmQuantityCompleted"] = 0;
				mfgReceiptsSourceRow["rmmQuantityReceivedToInventory"] = 0;
				mfgReceiptsSourceRow["rmmJobScrapQuantity"] = 0;
				mfgReceiptsSourceRow["rmmPurchaseQuantityReceived"] = 0;
				mfgReceiptsSourceRow["rmmSetupCharge"] = 0;
				mfgReceiptsSourceRow["rmmInventoryQuantityReceived"] = 0;
				mfgReceiptsSourceRow["rmmJobOprQuantityReceived"] = 0;
				mfgReceiptsSourceRow["rmmQuantityToInspect"] = 0;
				mfgReceiptsSourceRow["rmmJobMatQuantityReceived"] = 0;
				mfgReceiptsSourceRow["rmmExtendedCostBase"] = 0;
				mfgReceiptsSourceRow["rmmJobAsmQuantityReceived"] = 0;
				mfgReceiptsSourceRow["rmmTotalComponentCosts"] = 0;
			}
			dataRow["rmmQuantityToInspect"] = false;
			dataRow["rmmRequiresInspection"] = false;
			dataRow["rmmInInspection"] = false;
			dataRow["rmmInspectionComplete"] = false;
			dataRow["rmmScrapQuantity"] = 0;
			AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "MfgReceipts", mfgReceiptsSourceRow["rmmUniqueID"], "MfgReceipts", dataRow["rmmUniqueID"], nJobSplitLogID);
			foreach (DataRow mfgReceiptComponentSourceRow in foundMfgReceiptComponentsSourceRows)
			{
				DataRow dataRow2 = initialFoundMfgReceiptComponentsSourceRows.FirstOrDefault((DataRow row) => Convert.ToString(row["rmnMfgReceiptID"]) == Convert.ToString(mfgReceiptComponentSourceRow["rmnMfgReceiptID"]) && Convert.ToInt32(row["rmnMfgReceiptComponentID"]) == Convert.ToInt32(mfgReceiptComponentSourceRow["rmnMfgReceiptComponentID"]));
				DataRow dataRow3 = mfgReceiptComponentsDestTable.AddBlankRow();
				CopyAllFieldsToNewRow(mfgReceiptComponentSourceRow, dataRow3);
				dataRow3["rmnCreatedBy"] = database.User.ID;
				dataRow3["rmnCreatedDate"] = DateTime.Now;
				dataRow3["rmnMfgReceiptID"] = dataRow["rmmMfgReceiptID"];
				dataRow3["rmnJobID"] = cNewJob;
				dataRow3["rmnJobAssemblyID"] = nNewAsm;
				if (Convert.ToString(mfgReceiptComponentSourceRow["rmnReverseMfgReceiptID"]).Trim() != "" && Convert.ToDouble(mfgReceiptComponentSourceRow["rmnReverseMfgReceiptCompID"]) != 0.0)
				{
					mfgReceiptMatcherDictionary.Add(mfgReceiptComponentSourceRow["rmnReverseMfgReceiptID"].ToString().Trim() + "\t" + Convert.ToDouble(mfgReceiptComponentSourceRow["rmnReverseMfgReceiptCompID"]).ToSql(), dataRow3["rmnMfgReceiptComponentID"].ToString());
				}
				sourceTableUniqueIdMatcherDictionary.Add(mfgReceiptComponentSourceRow["rmnUniqueID"].ToString().Trim(), dataRow3["rmnUniqueID"].ToString());
				if (nSplitCosts == SplitCostOption.SplitCostsBasedOnQuantity)
				{
					dataRow3["rmnInvParentQuantity"] = Math.Round(Convert.ToDouble(dataRow2["rmnInvParentQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					dataRow3["rmnJobMatParentQuantity"] = Math.Round(Convert.ToDouble(dataRow2["rmnJobMatParentQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					dataRow3["rmnAdditionalQuantity"] = Math.Round(Convert.ToDouble(dataRow2["rmnAdditionalQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					dataRow3["rmnInvReceiptQuantity"] = Math.Round(Convert.ToDouble(dataRow2["rmnInvReceiptQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					dataRow3["rmnJobMatReceiptQuantity"] = Math.Round(Convert.ToDouble(dataRow2["rmnJobMatReceiptQuantity"]) * initialDestPercent, database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					dataRow3["rmnExtendedCost"] = Math.Round(Convert.ToDouble(dataRow2["rmnExtendedCost"]) * initialDestPercent, 2);
					mfgReceiptComponentSourceRow["rmnInvParentQuantity"] = Math.Round(Convert.ToDouble(mfgReceiptComponentSourceRow["rmnInvParentQuantity"]) - Convert.ToDouble(dataRow3["rmnInvParentQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					mfgReceiptComponentSourceRow["rmnJobMatParentQuantity"] = Math.Round(Convert.ToDouble(mfgReceiptComponentSourceRow["rmnJobMatParentQuantity"]) - Convert.ToDouble(dataRow3["rmnJobMatParentQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					mfgReceiptComponentSourceRow["rmnAdditionalQuantity"] = Math.Round(Convert.ToDouble(mfgReceiptComponentSourceRow["rmnAdditionalQuantity"]) - Convert.ToDouble(dataRow3["rmnAdditionalQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					mfgReceiptComponentSourceRow["rmnInvReceiptQuantity"] = Math.Round(Convert.ToDouble(mfgReceiptComponentSourceRow["rmnInvReceiptQuantity"]) - Convert.ToDouble(dataRow3["rmnInvReceiptQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					mfgReceiptComponentSourceRow["rmnJobMatReceiptQuantity"] = Math.Round(Convert.ToDouble(mfgReceiptComponentSourceRow["rmnJobMatReceiptQuantity"]) - Convert.ToDouble(dataRow3["rmnJobMatReceiptQuantity"]), database.Props("DS").Field<byte>("xadInventoryQuantityDecimals"));
					mfgReceiptComponentSourceRow["rmnExtendedCost"] = Math.Round(Convert.ToDouble(mfgReceiptComponentSourceRow["rmnExtendedCost"]) - Convert.ToDouble(dataRow3["rmnExtendedCost"]), 2);
				}
				else
				{
					mfgReceiptComponentSourceRow["rmnInvParentQuantity"] = 0;
					mfgReceiptComponentSourceRow["rmnJobMatParentQuantity"] = 0;
					mfgReceiptComponentSourceRow["rmnAdditionalQuantity"] = 0;
					mfgReceiptComponentSourceRow["rmnInvReceiptQuantity"] = 0;
					mfgReceiptComponentSourceRow["rmnJobMatReceiptQuantity"] = 0;
					mfgReceiptComponentSourceRow["rmnExtendedCost"] = 0;
				}
				AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "MfgReceiptComponents", mfgReceiptComponentSourceRow["rmnUniqueID"], "MfgReceiptComponents", dataRow3["rmnUniqueID"], nJobSplitLogID);
			}
		}
		catch
		{
			throw;
		}
	}

	private void SplitGLJournals(M1Database database, SqlTransaction transaction, string sourceTableJobID, string targetJobId, double targetPercent, List<int> assembliesToIgnore, int startSequence, int jobSplitLogId, SplitCostOption splitCostOption, DataTable glJournalsSourceTable, DataTable glJournalLinesSourceTable, DataTable jobSplitLogLinesTable, DataTable glJournalsDestinationTable, DataTable glJournalLinesDestTable, Dictionary<string, string> sourceTableUniqueIdMatcherDictionary)
	{
		ReverseGLJournalAndLines(database, transaction, glJournalLinesDestTable, glJournalLinesSourceTable, glJournalsSourceTable, glJournalsDestinationTable, sourceTableUniqueIdMatcherDictionary, sourceTableJobID, assembliesToIgnore, startSequence, jobSplitLogLinesTable, jobSplitLogId);
		SplitAndTransferGLJournalAndLines(database, transaction, targetJobId, targetPercent, jobSplitLogId, splitCostOption, glJournalsSourceTable, glJournalLinesSourceTable, jobSplitLogLinesTable, glJournalsDestinationTable, glJournalLinesDestTable, sourceTableUniqueIdMatcherDictionary, sourceTableJobID, assembliesToIgnore, startSequence);
	}

	private void ReverseGLJournalAndLines(M1Database database, SqlTransaction transaction, DataTable glJournalLinesDestTable, DataTable glJournalLinesSourceTable, DataTable glJournalsSourceTable, DataTable glJournalsDestTable, Dictionary<string, string> sourceTableUniqueIdMatcherDictionary, string sourceTableJobID, List<int> assembliesToIgnore, int startSequence, DataTable jobSplitLogLinesTable, int nJobSplitLogID)
	{
		try
		{
			foreach (KeyValuePair<int, int> item in _newGLJournalIdForSourceGLJournalReversal)
			{
				int num = (int)database.ExecuteScalar($"SELECT COUNT(*) FROM GLJournals WHERE glpGLJournalID = {item.Value}");
				int num2 = glJournalsDestTable.Select("glpGLJournalID = " + item.Value.ToLinq()).Length;
				if (num != 0 || num2 != 0)
				{
					continue;
				}
				DataRow dataRow = glJournalsSourceTable.Select("glpGLJournalID = " + item.Key.ToLinq()).FirstOrDefault();
				bool flag = Convert.ToBoolean(dataRow["glpPosted"]) && ((Convert.ToInt32(dataRow["glpDetailSource"]) == 3) ? database.Props("AP").Field<bool>("xafAPExpressPost") : database.Props("AP").Field<bool>("xafProductionExpressPost"));
				DataRow dataRow2 = _glJournalsPreSplitSource.Select("glpGLJournalID = " + item.Key.ToLinq()).FirstOrDefault();
				int value = item.Value;
				DataRow dataRow3 = glJournalsDestTable.AddBlankRow();
				CopyAllFieldsToNewRow(dataRow2, dataRow3);
				dataRow3["glpGLJournalID"] = value;
				dataRow3["glpReference"] = $"SplitJobRev/OrigJour '{item.Key}'";
				dataRow3["glpPosted"] = flag;
				dataRow3["glpPostedDate"] = (flag ? ((object)DateTime.Now) : DBNull.Value);
				dataRow3["glpCreatedBy"] = database.User.ID;
				dataRow3["glpCreatedDate"] = DateTime.Now;
				dataRow3["glpTotalCredits"] = 0;
				dataRow3["glpTotalDebits"] = 0;
				int num3 = 1;
				bool flag2 = 14 == Convert.ToInt32(dataRow2["glpDetailSource"]);
				bool flag3 = 3 == Convert.ToInt32(dataRow2["glpDetailSource"]);
				string text = "And gllJobID = " + sourceTableJobID.ToLinq();
				string text2 = "or (gllJobID = '' and gllGLJournalID = " + item.Key.ToLinq() + ")";
				string text3 = ((assembliesToIgnore.Count == 0) ? string.Empty : (" and gllJobAssemblyID not in (" + string.Join(",", assembliesToIgnore) + ")"));
				string text4 = " And (gllJobID = " + sourceTableJobID.ToLinq() + " or gllJobID = '')";
				DataRow[] array = glJournalLinesSourceTable.Select("gllGLJournalID = " + item.Key.ToLinq() + " " + text3 + " " + (flag2 ? text : string.Empty) + " " + (flag3 ? (text4 + text2) : string.Empty));
				APInvoiceForGL aPInvoiceForGL = (flag3 ? new APInvoiceForGL(database.GetDataTable(new SqlCommand("SELECT * FROM APInvoices WHERE appAPInvoiceID = " + dataRow["glpAPInvoiceID"].ToLinq())).Select().FirstOrDefault()) : null);
				bool flag4 = flag3 && aPInvoiceForGL != null && aPInvoiceForGL.IsCredit;
				string text5 = ((assembliesToIgnore.Count == 0) ? string.Empty : (" and aplJobAssemblyID not in (" + string.Join(",", assembliesToIgnore) + ")"));
				DataRow[] array2 = _apInvoiceLinesPreSplitSource.Select("aplAPInvoiceID = " + dataRow2["glpAPInvoiceID"].ToLinq() + " and aplJobID = " + sourceTableJobID.ToLinq() + " " + text5);
				double num4 = 0.0;
				DataRow[] array3 = array2;
				foreach (DataRow dataRow4 in array3)
				{
					if (startSequence != 0 && Convert.ToInt32(dataRow4["aplJobMaterialID"]) == 0 && Convert.ToInt32(dataRow4["aplJobAssemblyID"]) == SelectedRootAssembly && Convert.ToInt32(dataRow4["aplJobOperationID"]) < startSequence)
					{
						continue;
					}
					if (!string.IsNullOrEmpty(Convert.ToString(dataRow4["aplTaxCodeID"])))
					{
						string key = string.Format("{0}-{1}-{2}", item.Key, dataRow2["glpAPInvoiceID"], dataRow4["aplTaxCodeID"]);
						double num5 = Math.Round(Convert.ToDouble(dataRow4["aplTaxAmountBase"]), 2);
						num4 += num5;
						if (_apTaxLines.ContainsKey(key))
						{
							_apTaxLines[key] += num5;
						}
						else
						{
							_apTaxLines.Add(key, num5);
						}
					}
					if (!string.IsNullOrEmpty(Convert.ToString(dataRow4["aplSecondTaxCodeID"])))
					{
						string key2 = string.Format("{0}-{1}-{2}", item.Key, dataRow2["glpAPInvoiceID"], dataRow4["aplSecondTaxCodeID"]);
						double num6 = Math.Round(Convert.ToDouble(dataRow4["aplSecondTaxAmountBase"]), 2);
						num4 += num6;
						if (_apTaxLines.ContainsKey(key2))
						{
							_apTaxLines[key2] += num6;
						}
						else
						{
							_apTaxLines.Add(key2, num6);
						}
					}
				}
				double num7 = 0.0;
				array3 = array;
				foreach (DataRow dataRow5 in array3)
				{
					if ((startSequence == 0 || Convert.ToInt32(dataRow5["gllJobMaterialID"]) != 0 || Convert.ToInt32(dataRow5["gllJobAssemblyID"]) != SelectedRootAssembly || Convert.ToInt32(dataRow5["gllJobOperationID"]) >= startSequence) && !string.IsNullOrEmpty(Convert.ToString(dataRow5["gllJobID"])))
					{
						num7 += Convert.ToDouble(dataRow5[flag4 ? "gllCreditAmount" : "gllDebitAmount"]);
					}
				}
				double num8 = num7 + Math.Abs(num4);
				if (aPInvoiceForGL != null)
				{
					aPInvoiceForGL.SetJobsAmount(num7);
					aPInvoiceForGL.AddTaxesToDictionary(_apTaxLines, item.Key);
					_apInvoicesInGL.Add(aPInvoiceForGL.InvoiceId, aPInvoiceForGL);
					num8 += aPInvoiceForGL.PortionTotal;
				}
				array3 = array;
				foreach (DataRow dataRow6 in array3)
				{
					if ((!string.IsNullOrEmpty(dataRow6["gllJobID"].ToString()) || Convert.ToInt32(dataRow6["gllJobMaterialID"]) != 0 || Convert.ToInt32(dataRow6["gllJobOperationID"]) != 0) && startSequence != 0 && Convert.ToInt32(dataRow6["gllJobMaterialID"]) == 0 && Convert.ToInt32(dataRow6["gllJobAssemblyID"]) == SelectedRootAssembly && Convert.ToInt32(dataRow6["gllJobOperationID"]) < startSequence)
					{
						continue;
					}
					bool flag5 = flag3 && !string.IsNullOrEmpty(Convert.ToString(dataRow6["gllTaxCodeID"]));
					bool flag6 = flag5 && Convert.ToBoolean(database.ExecuteScalar("select xaxIncludePrimaryTax from TaxCodes where xaxTaxCodeID = " + dataRow6["gllTaxCodeID"].ToLinq()));
					double num9 = 0.0;
					if (aPInvoiceForGL != null)
					{
						num9 = (flag6 ? aPInvoiceForGL.FreightIncludePrimaryTax : aPInvoiceForGL.FreightAmount);
					}
					bool flag7 = flag5 && num9 != 0.0 && Convert.ToDouble(dataRow6["gllTaxableAmount"]) == num9;
					string key3 = string.Format("{0}-{1}-{2}{3}", item.Key, dataRow2["glpAPInvoiceID"], dataRow6["gllTaxCodeID"], flag7 ? "-Freight" : string.Empty);
					if (!flag5 || _apTaxLines.ContainsKey(key3))
					{
						DataRow sourceTableRow = _glJournalLinesPreSplitSource.Select("gllUniqueID = " + dataRow6["gllUniqueID"].ToLinq()).FirstOrDefault();
						DataRow dataRow7 = glJournalLinesDestTable.AddBlankRow();
						CopyAllFieldsToNewRow(sourceTableRow, dataRow7);
						dataRow7["gllGLJournalLineID"] = num3++;
						dataRow7["gllGLJournalID"] = value;
						dataRow7["gllReference"] = $"SplitJobRev/OrigJour '{item.Key}'";
						dataRow7["gllPosted"] = flag;
						dataRow7["gllCreatedBy"] = database.User.ID;
						dataRow7["gllCreatedDate"] = DateTime.Now;
						if (flag3 && string.IsNullOrEmpty(Convert.ToString(dataRow6["gllJobID"])) && string.IsNullOrEmpty(Convert.ToString(dataRow6["gllTaxCodeID"])) && Convert.ToDouble(dataRow6[flag4 ? "gllCreditAmount" : "gllDebitAmount"]) == 0.0)
						{
							dataRow7["gllTransactionAmount"] = (flag4 ? (num8 * -1.0) : num8);
							dataRow7["gllDebitAmount"] = (flag4 ? 0.0 : num8);
							dataRow7["gllCreditAmount"] = (flag4 ? num8 : 0.0);
							dataRow7["gllTaxableAmount"] = Convert.ToDouble(dataRow6["gllTaxableAmount"]);
							dataRow7["gllJobID"] = sourceTableJobID;
						}
						else if (flag3 && string.IsNullOrEmpty(Convert.ToString(dataRow6["gllJobID"])) && string.IsNullOrEmpty(Convert.ToString(dataRow6["gllTaxCodeID"])) && Convert.ToDouble(dataRow6[flag4 ? "gllCreditAmount" : "gllDebitAmount"]) != 0.0)
						{
							double portionFreightAmount = aPInvoiceForGL.PortionFreightAmount;
							dataRow7["gllTransactionAmount"] = (flag4 ? portionFreightAmount : (portionFreightAmount * -1.0));
							dataRow7["gllDebitAmount"] = (flag4 ? portionFreightAmount : 0.0);
							dataRow7["gllCreditAmount"] = (flag4 ? 0.0 : portionFreightAmount);
							dataRow7["gllJobID"] = sourceTableJobID;
						}
						else if (flag5)
						{
							double num10 = Math.Abs(_apTaxLines[key3]);
							double num11 = (flag6 ? aPInvoiceForGL.PortionFreightIncludePrimaryTax : aPInvoiceForGL.PortionFreightAmount);
							double num12 = (flag7 ? num11 : num7);
							dataRow7["gllTransactionAmount"] = (flag4 ? num10 : (num10 * -1.0));
							dataRow7["gllDebitAmount"] = (flag4 ? num10 : 0.0);
							dataRow7["gllCreditAmount"] = (flag4 ? 0.0 : num10);
							dataRow7["gllTaxableAmount"] = (flag4 ? num12 : (num12 * -1.0));
							dataRow7["gllTaxCodeID"] = dataRow6["gllTaxCodeID"];
							dataRow7["gllJobID"] = sourceTableJobID;
						}
						else
						{
							dataRow7["gllTransactionAmount"] = Convert.ToDouble(dataRow6["gllTransactionAmount"]) * -1.0;
							dataRow7["gllDebitAmount"] = Convert.ToDouble(dataRow6["gllCreditAmount"]);
							dataRow7["gllCreditAmount"] = Convert.ToDouble(dataRow6["gllDebitAmount"]);
							dataRow7["gllTaxableAmount"] = Convert.ToDouble(dataRow6["gllTaxableAmount"]);
						}
						dataRow3["glpTotalCredits"] = Convert.ToDouble(dataRow3["glpTotalCredits"]) + Convert.ToDouble(dataRow7["gllCreditAmount"]);
						dataRow3["glpTotalDebits"] = Convert.ToDouble(dataRow3["glpTotalDebits"]) + Convert.ToDouble(dataRow7["gllDebitAmount"]);
						if (Guid.TryParse(dataRow7["gllSourceTableUniqueID"].ToString().Trim(), out var _) && Convert.ToString(dataRow7["gllSourceTableName"]).Trim() != "" && sourceTableUniqueIdMatcherDictionary.ContainsKey(Convert.ToString(dataRow6["gllSourceTableUniqueID"]).Trim()))
						{
							dataRow7["gllSourceTableUniqueID"] = sourceTableUniqueIdMatcherDictionary[Convert.ToString(dataRow6["gllSourceTableUniqueID"]).Trim()];
						}
						AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "GLJournalLines", dataRow6["gllUniqueID"], "GLJournalLines", dataRow7["gllUniqueID"], nJobSplitLogID);
					}
				}
				AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "GLJournals", dataRow["glpUniqueID"], "GLJournalLines", dataRow3["glpUniqueID"], nJobSplitLogID);
			}
		}
		catch
		{
			throw;
		}
	}

	private void SplitAndTransferGLJournalAndLines(M1Database database, SqlTransaction transaction, string targetJobId, double targetPercent, int jobSplitLogId, SplitCostOption splitCostOption, DataTable gLJournalsSourceTable, DataTable gLJournalLinesSourceTable, DataTable jobSplitLogLinesTable, DataTable gLJournalsDestinationTable, DataTable glJournalLinesDestTable, Dictionary<string, string> sourceTableUniqueIdMatcherDictionary, string sourceTableJobID, List<int> assembliesToIgnore, int startSequence)
	{
		foreach (KeyValuePair<int, int> item3 in _newGLJournalIdForTargetGLJournal)
		{
			DataRow dataRow = gLJournalsSourceTable.Select("glpGLJournalID = " + item3.Key.ToLinq()).FirstOrDefault();
			bool flag = Convert.ToBoolean(dataRow["glpPosted"]) && ((Convert.ToInt32(dataRow["glpDetailSource"]) == 3) ? database.Props("AP").Field<bool>("xafAPExpressPost") : database.Props("AP").Field<bool>("xafProductionExpressPost"));
			int num = (int)database.ExecuteScalar($"SELECT COUNT(*) FROM GLJournals WHERE glpGLJournalID = {item3.Value}");
			int num2 = gLJournalsDestinationTable.Select("glpGLJournalID = " + item3.Value.ToLinq()).Length;
			DataRow[] array = gLJournalsDestinationTable.Select("glpGLJournalID = " + _newGLJournalIdForSourceGLJournalReversal[item3.Key].ToLinq());
			if (num == 0 && num2 == 0)
			{
				DataRow sourceTableRow = _glJournalsPreSplitSource.Select("glpGLJournalID = " + item3.Key.ToLinq()).FirstOrDefault();
				DataRow dataRow2 = gLJournalsDestinationTable.AddBlankRow();
				CopyAllFieldsToNewRow(sourceTableRow, dataRow2);
				dataRow2["glpGLJournalID"] = item3.Value;
				dataRow2["glpReference"] = $"SplitJob/OrigJournal '{item3.Key}'";
				dataRow2["glpPosted"] = flag;
				dataRow2["glpPostedDate"] = (flag ? ((object)DateTime.Now) : DBNull.Value);
				dataRow2["glpCreatedBy"] = database.User.ID;
				dataRow2["glpCreatedDate"] = DateTime.Now;
				if (array.Length != 0)
				{
					dataRow2["glpTotalDebits"] = array.FirstOrDefault()["glpTotalDebits"];
					dataRow2["glpTotalCredits"] = array.FirstOrDefault()["glpTotalCredits"];
				}
				AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "GLJournals", dataRow["glpUniqueID"], "GLJournalLines", dataRow2["glpUniqueID"], jobSplitLogId);
			}
			bool flag2 = (int)database.ExecuteScalar($"SELECT COUNT(*) FROM GLJournals WHERE glpGLJournalID = {item3.Key} and glpDetailSource = 14") > 0;
			bool flag3 = (int)database.ExecuteScalar($"SELECT COUNT(*) FROM GLJournals WHERE glpGLJournalID = {item3.Key} and glpDetailSource = 3") > 0;
			string text = ((assembliesToIgnore.Count == 0) ? string.Empty : (" and gllJobAssemblyID not in (" + string.Join(",", assembliesToIgnore) + ")"));
			string text2 = "or (gllJobID = '' and gllGLJournalID = " + item3.Key.ToLinq() + ")";
			string text3 = " And (gllJobID = " + sourceTableJobID.ToLinq() + " or gllJobID = '')";
			string text4 = "And gllJobID = " + sourceTableJobID.ToLinq();
			DataRow[] array2 = gLJournalLinesSourceTable.Select("gllGLJournalID = " + item3.Key.ToLinq() + " " + text + " " + (flag2 ? text4 : string.Empty) + " " + (flag3 ? (text3 + text2) : string.Empty));
			APInvoiceForGL aPInvoiceForGL = ((!string.IsNullOrEmpty(dataRow["glpAPInvoiceID"].ToString())) ? _apInvoicesInGL[dataRow["glpAPInvoiceID"].ToString()] : null);
			bool flag4 = flag3 && aPInvoiceForGL != null && aPInvoiceForGL.IsCredit;
			DataRow[] array3 = array2;
			foreach (DataRow dataRow3 in array3)
			{
				if ((!string.IsNullOrEmpty(dataRow3["gllJobID"].ToString()) || Convert.ToInt32(dataRow3["gllJobMaterialID"]) != 0 || Convert.ToInt32(dataRow3["gllJobOperationID"]) != 0) && startSequence != 0 && Convert.ToInt32(dataRow3["gllJobMaterialID"]) == 0 && Convert.ToInt32(dataRow3["gllJobAssemblyID"]) == SelectedRootAssembly && Convert.ToInt32(dataRow3["gllJobOperationID"]) < startSequence)
				{
					continue;
				}
				bool flag5 = flag3 && !string.IsNullOrEmpty(Convert.ToString(dataRow3["gllTaxCodeID"]));
				bool flag6 = flag5 && Convert.ToBoolean(database.ExecuteScalar("select xaxIncludePrimaryTax from TaxCodes where xaxTaxCodeID = " + dataRow3["gllTaxCodeID"].ToLinq()));
				double num3 = 0.0;
				if (aPInvoiceForGL != null)
				{
					num3 = (flag6 ? aPInvoiceForGL.FreightIncludePrimaryTax : aPInvoiceForGL.FreightAmount);
				}
				bool flag7 = flag5 && num3 != 0.0 && Convert.ToDouble(dataRow3["gllTaxableAmount"]) == num3;
				string key = string.Format("{0}-{1}-{2}{3}", item3.Key, dataRow["glpAPInvoiceID"], dataRow3["gllTaxCodeID"], flag7 ? "-Freight" : string.Empty);
				if (flag5 && !_apTaxLines.ContainsKey(key))
				{
					continue;
				}
				DataRow dataRow4 = _glJournalLinesPreSplitSource.Select("gllGLJournalID = " + item3.Key.ToLinq() + " and gllGLJournalLineID = " + dataRow3["gllGLJournalLineID"].ToLinq()).FirstOrDefault();
				bool flag8 = flag3 && string.IsNullOrEmpty(Convert.ToString(dataRow4["gllJobID"]));
				string item = (flag8 ? string.Format("{0}-{1}-{2}", _newGLJournalIdForTargetGLJournal[item3.Key], dataRow3["gllGLJournalID"], dataRow3["gllGLJournalLineID"]) : string.Empty);
				string item2 = string.Format("{0}-{1}-{2}-{3}", _newGLJournalIdForTargetGLJournal[item3.Key], dataRow3["gllGLJournalID"], dataRow3["gllGLJournalLineID"], targetJobId);
				if (_glJournalConsolidatedLines.Contains(item) || _glJournalTargetLinesCreated.Contains(item2))
				{
					continue;
				}
				DataRow dataRow5 = glJournalLinesDestTable.AddBlankRow();
				CopyAllFieldsToNewRow(dataRow4, dataRow5);
				dataRow5["gllGLJournalLineID"] = GetNextLineForTable(database, transaction, glJournalLinesDestTable, "GLJournalLines", item3.Value);
				dataRow5["gllGLJournalID"] = item3.Value;
				dataRow5["gllReference"] = $"SplitJob/OrigJournal '{item3.Key}'";
				dataRow5["gllPosted"] = flag;
				dataRow5["gllCreatedBy"] = database.User.ID;
				dataRow5["gllCreatedDate"] = DateTime.Now;
				if (!flag8)
				{
					dataRow5["gllJobID"] = targetJobId;
					if (!UpdatePartTransactionID.ContainsKey(targetJobId))
					{
						UpdatePartTransactionID.Add(targetJobId, new GLJournalInfo());
					}
					UpdatePartTransactionID[targetJobId].GllGLJournalLineID.Add((int)dataRow5["gllGLJournalLineID"]);
					UpdatePartTransactionID[targetJobId].GllGLJournalID.Add((int)dataRow5["gllGLJournalID"]);
					UpdatePartTransactionID[targetJobId].GllSourcePartTransaction.Add((int)dataRow4["gllPartTransactionID"]);
					if (SelectedRootAssembly != 0 && SelectedRootAssembly == (int)dataRow4["gllJobAssemblyID"])
					{
						dataRow5["gllJobAssemblyID"] = 0;
					}
					_glJournalTargetLinesCreated.Add(item2);
				}
				else
				{
					if (string.IsNullOrEmpty(Convert.ToString(dataRow4["gllTaxCodeID"])) && Convert.ToDouble(dataRow3[flag4 ? "gllCreditAmount" : "gllDebitAmount"]) == 0.0)
					{
						double num4 = Convert.ToDouble(array.FirstOrDefault()["glpTotalDebits"]);
						dataRow5["gllTransactionAmount"] = (flag4 ? num4 : (num4 * -1.0));
						dataRow5["gllDebitAmount"] = (flag4 ? num4 : 0.0);
						dataRow5["gllCreditAmount"] = (flag4 ? 0.0 : num4);
					}
					else if (string.IsNullOrEmpty(Convert.ToString(dataRow4["gllTaxCodeID"])) && Convert.ToDouble(dataRow3[flag4 ? "gllCreditAmount" : "gllDebitAmount"]) != 0.0)
					{
						double portionFreightAmount = aPInvoiceForGL.PortionFreightAmount;
						dataRow5["gllTransactionAmount"] = (flag4 ? (portionFreightAmount * -1.0) : portionFreightAmount);
						dataRow5["gllDebitAmount"] = (flag4 ? 0.0 : portionFreightAmount);
						dataRow5["gllCreditAmount"] = (flag4 ? portionFreightAmount : 0.0);
					}
					else
					{
						double num5 = Math.Abs(_apTaxLines[key]);
						double num6 = (flag6 ? aPInvoiceForGL.PortionFreightIncludePrimaryTax : aPInvoiceForGL.PortionFreightAmount);
						double num7 = (flag7 ? num6 : aPInvoiceForGL.JobsAmountPortion);
						dataRow5["gllTransactionAmount"] = (flag4 ? (num5 * -1.0) : num5);
						dataRow5["gllDebitAmount"] = (flag4 ? 0.0 : num5);
						dataRow5["gllCreditAmount"] = (flag4 ? num5 : 0.0);
						dataRow5["gllTaxableAmount"] = (flag4 ? (num7 * -1.0) : num7);
					}
					_glJournalConsolidatedLines.Add(item);
				}
				if (Guid.TryParse(dataRow5["gllSourceTableUniqueID"].ToString().Trim(), out var result) && Convert.ToString(dataRow5["gllSourceTableName"]).Trim() != "" && sourceTableUniqueIdMatcherDictionary.ContainsKey(Convert.ToString(dataRow3["gllSourceTableUniqueID"]).Trim()))
				{
					dataRow5["gllSourceTableUniqueID"] = sourceTableUniqueIdMatcherDictionary[Convert.ToString(dataRow3["gllSourceTableUniqueID"]).Trim()];
				}
				if (splitCostOption == SplitCostOption.SplitCostsBasedOnQuantity && !flag8)
				{
					string key2 = string.Format("{0}-{1}-{2}", _newGLJournalIdForTargetGLJournal[item3.Key], dataRow3["gllGLJournalID"], dataRow3["gllGLJournalLineID"]);
					DataRow dataRow6;
					if (!_glJournalLinesToUpdate.ContainsKey(key2))
					{
						dataRow6 = glJournalLinesDestTable.AddBlankRow();
						CopyAllFieldsToNewRow(dataRow4, dataRow6);
						dataRow6["gllGLJournalLineID"] = GetNextLineForTable(database, transaction, glJournalLinesDestTable, "GLJournalLines", item3.Value);
						dataRow6["gllGLJournalID"] = item3.Value;
						dataRow6["gllReference"] = $"SplitJob/OrigJournal '{item3.Key}'";
						dataRow6["gllPosted"] = flag;
						dataRow6["gllCreatedBy"] = database.User.ID;
						dataRow6["gllCreatedDate"] = DateTime.Now;
						_glJournalLinesToUpdate.Add(key2, dataRow6["gllUniqueID"]);
					}
					else
					{
						dataRow6 = gLJournalLinesSourceTable.Select("gllUniqueID = " + _glJournalLinesToUpdate[key2].ToLinq()).FirstOrDefault();
					}
					dataRow5["gllTransactionAmount"] = Math.Round(Convert.ToDouble(dataRow5["gllTransactionAmount"]) * targetPercent, 2);
					dataRow5["gllDebitAmount"] = Math.Round(Convert.ToDouble(dataRow5["gllDebitAmount"]) * targetPercent, 2);
					dataRow5["gllCreditAmount"] = Math.Round(Convert.ToDouble(dataRow5["gllCreditAmount"]) * targetPercent, 2);
					dataRow5["gllTaxableAmount"] = Math.Round(Convert.ToDouble(dataRow5["gllTaxableAmount"]) * targetPercent, 2);
					dataRow6["gllTransactionAmount"] = Convert.ToDouble(dataRow6["gllTransactionAmount"]) - Convert.ToDouble(dataRow5["gllTransactionAmount"]);
					dataRow6["gllDebitAmount"] = Convert.ToDouble(dataRow6["gllDebitAmount"]) - Convert.ToDouble(dataRow5["gllDebitAmount"]);
					dataRow6["gllCreditAmount"] = Convert.ToDouble(dataRow6["gllCreditAmount"]) - Convert.ToDouble(dataRow5["gllCreditAmount"]);
					dataRow6["gllTaxableAmount"] = Convert.ToDouble(dataRow6["gllTaxableAmount"]) - Convert.ToDouble(dataRow5["gllTaxableAmount"]);
					if (Guid.TryParse(dataRow6["gllSourceTableUniqueID"].ToString().Trim(), out result) && Convert.ToString(dataRow6["gllSourceTableName"]).Trim() != "" && sourceTableUniqueIdMatcherDictionary.ContainsKey(Convert.ToString(dataRow3["gllSourceTableUniqueID"]).Trim()))
					{
						dataRow6["gllSourceTableUniqueID"] = sourceTableUniqueIdMatcherDictionary[Convert.ToString(dataRow3["gllSourceTableUniqueID"]).Trim()];
					}
					AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "GLJournalLines", dataRow3["gllUniqueID"], "GLJournalLines", dataRow6["gllUniqueID"], jobSplitLogId);
				}
				AddJobSplitLogLineRecord(database, transaction, jobSplitLogLinesTable, database.User.ID, "GLJournalLines", dataRow3["gllUniqueID"], "GLJournalLines", dataRow5["gllUniqueID"], jobSplitLogId);
			}
		}
	}

	private void AddJobSplitLogRecord(DataTable jobSplitLogTable, string userID, string sourceTableName, object sourceTableUniqueID, string destTableName, object destTableUniqueID, double splitQty, SplitCostOption splitCosts, object reqDate, int nJobSplitLogID)
	{
		DataRow dataRow = jobSplitLogTable.AddBlankRow();
		dataRow["jsgJobSplitLogID"] = nJobSplitLogID;
		dataRow["jsgSourceTableName"] = sourceTableName;
		dataRow["jsgSourceTableUniqueID"] = sourceTableUniqueID;
		dataRow["jsgDestTableName"] = destTableName;
		dataRow["jsgDestTableUniqueID"] = destTableUniqueID;
		dataRow["jsgSplitCostsOption"] = splitCosts;
		dataRow["jsgSplitQuantity"] = splitQty;
		dataRow["jsgRequiredDate"] = ((!Convert.IsDBNull(reqDate)) ? reqDate : DBNull.Value);
		dataRow["jsgCreatedBy"] = userID;
		dataRow["jsgCreatedDate"] = DateTime.Now;
	}

	private void AddJobSplitLogLineRecord(M1Database database, SqlTransaction transaction, DataTable jobSplitLogLinesTable, string userID, string sourceTableName, object sourceTableUniqueID, string destTableName, object destTableUniqueID, int nJobSplitLogID)
	{
		DataRow dataRow = jobSplitLogLinesTable.AddBlankRow();
		dataRow["jslJobSplitLogID"] = nJobSplitLogID;
		dataRow["jslJobSplitLogLineID"] = GetNextLineForTable(database, transaction, jobSplitLogLinesTable, "JobSplitLogLines", nJobSplitLogID.ToString());
		dataRow["jslSourceTableName"] = sourceTableName;
		dataRow["jslSourceTableUniqueID"] = sourceTableUniqueID;
		dataRow["jslDestTableName"] = destTableName;
		dataRow["jslDestTableUniqueID"] = destTableUniqueID;
	}

	private void ValidateSerialAndLotStatus(M1Database database, SqlTransaction transaction, DataTable jobAssembliesSourceTable, DataTable jobMaterialsSourceTable, DataTable jobMaterialComponentsSourceTable, ErrorItemsList errorList, List<int> assembliesToIgnore)
	{
		bool flag = HasSerialOrLotParts(database, transaction, jobAssembliesSourceTable, "jma", assembliesToIgnore);
		if (!flag)
		{
			flag = HasSerialOrLotParts(database, transaction, jobMaterialsSourceTable, "jmm", assembliesToIgnore);
		}
		if (!flag)
		{
			flag = HasSerialOrLotParts(database, transaction, jobMaterialComponentsSourceTable, "jmt", assembliesToIgnore);
		}
		if (flag)
		{
			ValidationInfo validationInfo = new ValidationInfo
			{
				RowDescription = "Split Job Execution Validations"
			};
			validationInfo.AddError("The Source Job, a selected Sub-Assembly, or a Material/Material Component within a selected Assembly has Serial and/or Lot tracked Parts and can ONLY be split if Split Cost Options is set to Keep Costs on Source Job");
			errorList.Add(validationInfo);
		}
	}

	private bool HasSerialOrLotParts(M1Database database, SqlTransaction transaction, DataTable dataTable, string prefixTable, List<int> assembliesToIgnore)
	{
		Part part = new Part();
		foreach (DataRow row in dataTable.Rows)
		{
			if (!assembliesToIgnore.Contains(Convert.ToInt32(row[prefixTable + "JobAssemblyID"])) && part.IsSerialOrLotTracked(database, Convert.ToString(row[prefixTable + "PartID"]), transaction))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsJobInActiveMRPSession(M1Database database, SqlTransaction transaction, string jobID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select IsNull(count(*),0) from MRPJobDetails inner join MRPSessions on mrjSessionID = mrpSessionID where mrjJobID = @JobID and mrpCompleted = 0");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		return Convert.ToInt32(database.ExecuteScalar(sqlCommand, transaction)) != 0;
	}

	private int GetNextLineForTable(M1Database database, SqlTransaction transaction, DataTable currentRecordsTable, string tableName, object tableID)
	{
		try
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			string empty3 = string.Empty;
			switch (tableName)
			{
			case "PurchaseOrderLines":
				empty = "select IsNull(Max(pmlPurchaseOrderLineID),0) as pmlPurchaseOrderLineID from PurchaseOrderLines where pmlPurchaseOrderID = " + tableID.ToSql();
				empty2 = "pmlPurchaseOrderID";
				empty3 = "pmlPurchaseOrderLineID";
				break;
			case "ReceiptLines":
				empty = "select IsNull(Max(rmlReceiptLineID),0) as rmlReceiptLineID from ReceiptLines where rmlReceiptID = " + tableID.ToSql();
				empty2 = "rmlReceiptID";
				empty3 = "rmlReceiptLineID";
				break;
			case "TimecardLines":
				empty = "select IsNull(Max(lmlTimecardLineID),0) as lmlTimecardLineID from TimecardLines where lmlTimecardID = " + tableID.ToSql();
				empty2 = "lmlTimecardID";
				empty3 = "lmlTimecardLineID";
				break;
			case "APInvoiceLines":
				empty = "select Max(aplAPInvoiceLineID) as aplAPInvoiceLineID from APInvoiceLines where aplAPInvoiceID = " + tableID.ToSql();
				empty2 = "aplAPInvoiceID";
				empty3 = "aplAPInvoiceLineID";
				break;
			case "MaterialIssueLines":
				empty = "select IsNull(Max(injMaterialIssueLineID),0) as injMaterialIssueLineID from MaterialIssueLines where injMaterialIssueID = " + tableID.ToSql();
				empty2 = "injMaterialIssueID";
				empty3 = "injMaterialIssueLineID";
				break;
			case "GLJournalLines":
				empty = "select IsNull(Max(gllGLJournalLineID),0) as gllGLJournalLineID from GLJournalLines where gllGLJournalID = " + tableID.ToSql();
				empty2 = "gllGLJournalID";
				empty3 = "gllGLJournalLineID";
				break;
			case "InspectionLines":
				empty = "select IsNull(Max(qalInspectionLineID),0) as qalInspectionLineID from InspectionLines where qalInspectionID = " + tableID.ToSql();
				empty2 = "qalInspectionID";
				empty3 = "qalInspectionLineID";
				break;
			case "DMRClaimLines":
				empty = "select IsNull(Max(dmlDMRClaimLineID),0) as dmlDMRClaimLineID from DMRClaimLines where dmlDMRClaimID = " + tableID.ToSql();
				empty2 = "dmlDMRClaimID";
				empty3 = "dmlDMRClaimLineID";
				break;
			case "DMRShipmentLines":
				empty = "select IsNull(Max(dslDMRShipmentLineID),0) as dslDMRShipmentLineID from DMRShipmentLines where dslDMRShipmentID = " + tableID.ToSql();
				empty2 = "dslDMRShipmentID";
				empty3 = "dslDMRShipmentLineID";
				break;
			case "JobSplitLogLines":
				empty = "select IsNull(Max(jslJobSplitLogLineID),0) as jslJobSplitLogLineID from JobSplitLogLines where jslJobSplitLogID = " + tableID.ToSql();
				empty2 = "jslJobSplitLogID";
				empty3 = "jslJobSplitLogLineID";
				break;
			default:
				return 1;
			}
			DataRow[] source = currentRecordsTable.Select(empty2 + " = " + tableID.ToLinq());
			object obj = ((!source.Any()) ? database.ExecuteScalar(empty, transaction) : source.Last()[empty3]);
			if (short.TryParse(obj.ToString(), out var result))
			{
				return result + 1;
			}
			if (int.TryParse(obj.ToString(), out var result2))
			{
				return result2 + 1;
			}
			return (int)obj + 1;
		}
		catch
		{
			throw;
		}
	}

	public string JobActiveMrpSessions(M1Database database, string jobID)
	{
		List<string> list = new List<string>();
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT mrrSessionID FROM MRPDemands INNER JOIN MRPSessions ON mrrSessionID = mrpSessionID\r\n                                                    WHERE mrpCompleted = 0 and mrrJobID = @jobID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		DataTable dataTable = database.GetDataTable(sqlCommand, null);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row3 in dataTable.Rows)
			{
				list.Add(row3.Field<string>("mrrSessionID"));
			}
		}
		sqlCommand = database.NewSqlCommand("SELECT mrsSessionID FROM MRPSupply INNER JOIN MRPSessions ON mrsSessionID = mrpSessionID\r\n                                                WHERE mrpCompleted = 0 and mrsJobID = @jobID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobID;
		dataTable = database.GetDataTable(sqlCommand, null);
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row4 in dataTable.Rows)
			{
				list.Add(row4.Field<string>("mrsSessionID"));
			}
		}
		return string.Join(", ", list.Distinct().ToArray());
	}

	private static void CopyAllFieldsToNewRow(DataRow sourceTableRow, DataRow destRow)
	{
		foreach (DataColumn column in destRow.Table.Columns)
		{
			if (!SystemGeneratedFields.IsGenerated(column.ColumnName) && !column.AutoIncrement)
			{
				destRow[column.ColumnName] = sourceTableRow[column.ColumnName];
			}
		}
	}

	private void GenerateNewIdsForGLJournals(M1Database database, string jobId, List<int> assembliesToIgnore, Dictionary<int, int> dictionaryForIds, int startJobAssemblyID, int startSequence)
	{
		if (dictionaryForIds.Count > 0)
		{
			return;
		}
		foreach (DataRow row in _glJournalsPreSplitSource.Rows)
		{
			DataRow[] array = _glJournalLinesPreSplitSource.Select(string.Format("gllGLJournalID = {0}", row["glpGLJournalID"]));
			bool flag = false;
			bool flag2 = false;
			if (startSequence != 0 && array.Any((DataRow journalLine) => Convert.ToInt32(journalLine["gllJobAssemblyID"]) == startJobAssemblyID && Convert.ToInt32(journalLine["gllJobMaterialID"]) == 0 && Convert.ToInt32(journalLine["gllJobOperationID"]) != 0))
			{
				flag = !array.Any((DataRow journalLine) => Convert.ToInt32(journalLine["gllJobOperationID"]) >= startSequence && journalLine["gllJobID"].ToString() == jobId);
			}
			if (assembliesToIgnore.Count != 0)
			{
				int num = 0;
				DataRow[] array2 = array;
				foreach (DataRow dataRow2 in array2)
				{
					if (string.IsNullOrEmpty(dataRow2["gllJobID"].ToString()) && Convert.ToInt32(dataRow2["gllJobMaterialID"]) == 0 && Convert.ToInt32(dataRow2["gllJobOperationID"]) == 0)
					{
						num++;
					}
					else if (!string.IsNullOrEmpty(dataRow2["gllJobID"].ToString()) && dataRow2["gllJobID"].ToString() != jobId)
					{
						num++;
					}
					else if (assembliesToIgnore.Contains(Convert.ToInt32(dataRow2["gllJobAssemblyID"])))
					{
						num++;
					}
				}
				if (num == array.Length)
				{
					flag2 = true;
				}
			}
			if (!(flag || flag2))
			{
				dictionaryForIds.Add(Convert.ToInt32(row["glpGLJournalID"]), Convert.ToInt32(database.NextIDs.GetNextIDForTable("GLJournals")));
			}
		}
	}
}
