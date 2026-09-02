using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class CostOfGoodSoldDefinition : FieldExtension
{
	public enum CostingMethod : byte
	{
		Average = 1,
		Last,
		Standard,
		LIFO,
		FIFO
	}

	public enum JournalSource : byte
	{
		AR = 1,
		AP,
		Payroll,
		GL,
		Production,
		FixedAssets
	}

	public enum DetailSource : byte
	{
		ARInvoice = 1,
		ARPayment,
		APInvoice,
		APPayment,
		Payroll,
		BankRec,
		RecurringJournals,
		GeneralJournal,
		FixedAssets,
		Shipments,
		Receipts,
		MfgReceipts,
		MaterialIssue,
		Timecards,
		InventoryAdj,
		MulticurrencyExchange,
		MfgVariance,
		LandedCosts,
		DMRShipments,
		RMAReceipts,
		WHTransfers,
		WHReceipts,
		PartClass,
		Inspections
	}

	public enum JournalLineTransactionType : byte
	{
		ShipFromInventory = 1,
		ShipFromJob,
		MaterialIssueToJobGoodQty,
		MaterialIssueToJobScrapQty,
		MiscIssueFromInventoryGoodQty,
		MiscIssueFromInventoryScrapQty,
		QuantityAdjustment,
		BinTransfer,
		ARInvoiceShipment,
		PurchaseOrderReceiptInvQty,
		PurchaseOrderReceiptJobMatQty,
		PurchaseOrderReceiptJobOprQty,
		PurchaseOrderReceiptQtyToInspect,
		DMRShipmentReturnQty,
		DMRShipmentInvQty,
		DMRShipmentJobMatQty,
		DMRShipmentJobOprQty,
		MiscReceiptToInventory,
		MiscReceiptToJobMatQty,
		MiscReceiptToJobOprQty,
		MiscReceiptToJobAsmQty,
		MfgReceiptInvQty,
		MfgReceiptScrapQty,
		MfgReceiptQtyToInspect,
		RMAReceiptInvQty,
		RMAReceiptQtyToInspect,
		BinReceipt,
		WarehouseTransfer,
		WarehouseReceipt,
		PartClassChange,
		InventoryCount,
		InspectionInvAcceptedQty,
		InspectionInvToReturnQty,
		InspectionInvToScrapQty,
		PartCostsAdjustment,
		ARInvoiceRMAReceipt,
		StandardCostRollup,
		InspectionJobMatAcceptedQty,
		InspectionJobOprAcceptedQty,
		InspectionJobToReturnQty,
		InspectionJobToScrapQty,
		MfgVarianceTransfer,
		LandedCostPOInTransit,
		LandedCostCharges,
		LandedCost,
		MaterialIssueToJobReturnGoodQty,
		MaterialIssueToJobReturnScrapQty
	}

	public enum JournalType : byte
	{
		Debit = 1,
		Credit
	}

	public class Journal
	{
		public string ID;

		public DateTime TransactionDate;

		public string Reference;

		public string Description;

		public string LongDescriptionText;

		public string OrganizationID;

		public string LocationID;

		public short GLFiscalYearID;

		public byte GLFiscalYearPeriodID;

		public JournalSource Source;

		public DetailSource DetailSource;

		public decimal TotalDebits;

		public decimal TotalCredits;

		public List<JournalLine> JournalLines = new List<JournalLine>();
	}

	public class JournalLine
	{
		public int LineID;

		public JournalType JournalType;

		public JournalLineTransactionType JournalLineTransactionType;

		public decimal TransactionAmount;

		public decimal DebitAmount;

		public decimal CreditAmount;

		public DateTime TransactionDate;

		public string GLAccountID;

		public string Reference;

		public string Description;

		public int PartTransactionID;

		public string OrganizationID;

		public string LocationID;

		public string JobID;

		public int JobAssemblyID;

		public int JobMaterialID;

		public int JobOperationID;

		public int JobMaterialComponentID;

		public Guid SourceGuid;
	}

	public class Costs
	{
		public decimal UnitMaterialCost;

		public decimal UnitSubcontractCost;

		public decimal UnitLaborCost;

		public decimal UnitOverheadCost;

		public decimal UnitDutyCost;

		public decimal UnitFreightCost;

		public decimal UnitMiscCost;
	}

	public string BinQuantityField = string.Empty;

	public string PartTransactionQuantityField = string.Empty;

	private FieldDefinition jobField;

	private FieldDefinition binField;

	private string jobID = string.Empty;

	private int jobAssemblyID;

	private int jobMaterialID;

	private int jobMaterialComponentID;

	private int jobOperationID;

	private string parms = string.Empty;

	private CostingMethod costingMethod;

	private CostingMethod overrideCostingMethod;

	private JournalLineTransactionType journalLineTransactionType;

	private bool backoutQty;

	private decimal manualQtyPassed = 1m;

	private string partTransFieldNames = "imtPartTransactionID, imtPartID, imtTableUniqueID, imtTransactionDate, imtUniqueID";

	private DateTime? passedTransactionDate;

	public bool UseFiscalYearAndPeriodFromJournal = true;

	public bool UseFiscalYearAndPeriodOnLinesFromJournal = true;

	public decimal ProvidedFiscalYear;

	public decimal ProvidedFiscalPeriod;

	public CostOfGoodSoldDefinition()
	{
	}

	public CostOfGoodSoldDefinition(JournalLineTransactionType partTransTransactionType)
	{
		journalLineTransactionType = partTransTransactionType;
	}

	public CostOfGoodSoldDefinition(M1BindingSource bs, string qtyFieldName, string binFieldName, DateTime transactionDate, byte transactionType, byte partTransTransactionType, bool reverseSign, decimal lineQty, string parameters, string uniqueIDTableName = "", string uniqueIDFieldName = "", string relatedJobFieldName = "", string passedJobID = "", int passedJobAssemblyID = 0, int passedJobMaterialID = 0, int passedJobMaterialComponentID = 0, int passedJobOperationID = 0)
	{
		PartBinField = binFieldName;
		binField = bs.Fields[binFieldName];
		FieldName = qtyFieldName;
		base.Field = bs.Fields[qtyFieldName];
		if (jobField == null && RelatedJobField.Length != 0)
		{
			jobField = bs.Fields[RelatedJobField];
		}
		else if (jobField == null && relatedJobFieldName.Length != 0)
		{
			jobField = bs.Fields[relatedJobFieldName];
		}
		if (jobField == null && !string.IsNullOrWhiteSpace(passedJobID))
		{
			jobID = passedJobID;
			jobAssemblyID = passedJobAssemblyID;
			jobMaterialID = passedJobMaterialID;
			jobMaterialComponentID = passedJobMaterialComponentID;
			jobOperationID = passedJobOperationID;
		}
		TransactionType = transactionType;
		journalLineTransactionType = (JournalLineTransactionType)TransactionType;
		Source = partTransTransactionType;
		Parameters = parameters;
		manualQtyPassed = lineQty;
		ReverseSign = reverseSign;
		passedTransactionDate = transactionDate;
		if (!string.IsNullOrWhiteSpace(uniqueIDFieldName))
		{
			base.Field.Table.UniqueField = uniqueIDFieldName;
		}
		if (!string.IsNullOrWhiteSpace(uniqueIDTableName))
		{
			base.Field.Table.TableName = uniqueIDTableName;
		}
	}

	public void AddJournal(M1Database database, DataRow sourceRow, DataRowVersion rowVersion, SqlTransaction transaction)
	{
		if (Parameters != null)
		{
			parms = Parameters.ToUpper();
		}
		if (checkParms(sourceRow, rowVersion, base.Field.Table.TableName, base.Field.Table.FieldPrefix, parms))
		{
			return;
		}
		costingMethod = (CostingMethod)database.Props("PN")["xapIMCostingMethod"];
		if (journalLineTransactionType == JournalLineTransactionType.PurchaseOrderReceiptInvQty || journalLineTransactionType == JournalLineTransactionType.PurchaseOrderReceiptJobMatQty || journalLineTransactionType == JournalLineTransactionType.PurchaseOrderReceiptJobOprQty || journalLineTransactionType == JournalLineTransactionType.PurchaseOrderReceiptQtyToInspect || journalLineTransactionType == JournalLineTransactionType.MiscReceiptToInventory || journalLineTransactionType == JournalLineTransactionType.MiscReceiptToJobMatQty || journalLineTransactionType == JournalLineTransactionType.MiscReceiptToJobOprQty || journalLineTransactionType == JournalLineTransactionType.MiscReceiptToJobAsmQty || (journalLineTransactionType == JournalLineTransactionType.DMRShipmentJobOprQty && costingMethod != CostingMethod.Standard) || (journalLineTransactionType == JournalLineTransactionType.DMRShipmentReturnQty && costingMethod != CostingMethod.Standard) || (journalLineTransactionType == JournalLineTransactionType.MfgReceiptInvQty && costingMethod != CostingMethod.Standard) || (journalLineTransactionType == JournalLineTransactionType.MfgReceiptQtyToInspect && costingMethod != CostingMethod.Standard))
		{
			overrideCostingMethod = CostingMethod.FIFO;
		}
		else if ((journalLineTransactionType == JournalLineTransactionType.InspectionJobMatAcceptedQty || journalLineTransactionType == JournalLineTransactionType.InspectionJobToReturnQty || journalLineTransactionType == JournalLineTransactionType.InspectionJobToScrapQty || journalLineTransactionType == JournalLineTransactionType.InspectionJobOprAcceptedQty || journalLineTransactionType == JournalLineTransactionType.InspectionInvAcceptedQty || journalLineTransactionType == JournalLineTransactionType.InspectionInvToReturnQty || journalLineTransactionType == JournalLineTransactionType.InspectionInvToScrapQty) && costingMethod != CostingMethod.Standard)
		{
			overrideCostingMethod = CostingMethod.FIFO;
		}
		else
		{
			overrideCostingMethod = costingMethod;
		}
		setJobInfo(sourceRow, rowVersion);
		string partID = sourceRow.Field<string>(binField.RelatedFieldsAndCurrentFieldArray[0], rowVersion);
		string partGroupID = string.Empty;
		if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "PartGroupID"))
		{
			partGroupID = sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PartGroupID");
		}
		string plantID = getPlantID(database, transaction, sourceRow, rowVersion);
		string destInventoryGLAccount = string.Empty;
		if (journalLineTransactionType.Equals(JournalLineTransactionType.WarehouseTransfer) || journalLineTransactionType.Equals(JournalLineTransactionType.BinTransfer) || journalLineTransactionType.Equals(JournalLineTransactionType.MaterialIssueToJobGoodQty) || journalLineTransactionType.Equals(JournalLineTransactionType.MaterialIssueToJobScrapQty) || journalLineTransactionType.Equals(JournalLineTransactionType.MaterialIssueToJobReturnGoodQty) || journalLineTransactionType.Equals(JournalLineTransactionType.MaterialIssueToJobReturnScrapQty))
		{
			string plantID2 = getPlantID(database, transaction, sourceRow, rowVersion, overrideToDestination: true);
			COGSAccounts cOGSAccounts = new COGS().GetCOGSAccounts(database, transaction, partID, plantID2, partGroupID, string.Empty);
			if (cOGSAccounts == null)
			{
				return;
			}
			destInventoryGLAccount = ((!journalLineTransactionType.Equals(JournalLineTransactionType.WarehouseTransfer) && !journalLineTransactionType.Equals(JournalLineTransactionType.BinTransfer)) ? cOGSAccounts.InventoryGLAccountID : cOGSAccounts.InventoryInTransferGLAccountID);
			cOGSAccounts = null;
		}
		string reasonID = string.Empty;
		if (!journalLineTransactionType.Equals(JournalLineTransactionType.MaterialIssueToJobGoodQty) && !journalLineTransactionType.Equals(JournalLineTransactionType.MaterialIssueToJobScrapQty))
		{
			reasonID = getReasonID(database, sourceRow, rowVersion, transaction, base.Field.Table.TableName);
		}
		COGSAccounts cOGSAccounts2 = new COGS().GetCOGSAccounts(database, transaction, partID, plantID, partGroupID, reasonID);
		if (cOGSAccounts2 == null)
		{
			return;
		}
		SqlCommand command = new SqlCommand();
		decimal qtyRatio = 1m;
		switch (journalLineTransactionType)
		{
		case JournalLineTransactionType.ARInvoiceShipment:
			getShipmentGuids(database, sourceRow, rowVersion, transaction, ref command, ref qtyRatio);
			break;
		case JournalLineTransactionType.ARInvoiceRMAReceipt:
			GetRmaReceiptGuids(database, sourceRow, rowVersion, transaction, ref command, ref qtyRatio);
			break;
		case JournalLineTransactionType.PurchaseOrderReceiptQtyToInspect:
		case JournalLineTransactionType.MfgReceiptQtyToInspect:
		case JournalLineTransactionType.RMAReceiptQtyToInspect:
		{
			Guid guid = (Guid)sourceRow[base.Field.Table.UniqueField, rowVersion];
			command = ((!base.Field.Table.TableName.ToUpper().Contains("COMPONENTS")) ? database.NewSqlCommand("Select " + partTransFieldNames + " from PartTransactions Inner Join InspectionLines on imtTableUniqueID = qalUniqueID Where qalSourceTableUniqueID = @UniqueID") : database.NewSqlCommand("Select " + partTransFieldNames + " from PartTransactions Inner Join InspectionComponents on imtTableUniqueID = qamUniqueID Where qamSourceTableUniqueID = @UniqueID"));
			command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = guid;
			break;
		}
		case JournalLineTransactionType.InspectionInvAcceptedQty:
		case JournalLineTransactionType.InspectionJobMatAcceptedQty:
		case JournalLineTransactionType.InspectionJobOprAcceptedQty:
		{
			Guid guid = (Guid)sourceRow[base.Field.Table.UniqueField, rowVersion];
			command = database.NewSqlCommand("Select " + partTransFieldNames + " from PartTransactions Where imtTableUniqueID = @UniqueID And imtTransactionType = @TransType and imtQuantityToInspect = 0 and imtInventoryQuantityReceived <> 0");
			command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = guid;
			command.Parameters.Add(new SqlParameter("@TransType", SqlDbType.TinyInt)).Value = Source;
			break;
		}
		case JournalLineTransactionType.InspectionInvToReturnQty:
		case JournalLineTransactionType.InspectionJobToReturnQty:
		{
			Guid guid = (Guid)sourceRow[base.Field.Table.UniqueField, rowVersion];
			command = database.NewSqlCommand("Select " + partTransFieldNames + " from PartTransactions Where imtTableUniqueID = @UniqueID And imtTransactionType = @TransType and imtQuantityToInspect = 0 and imtQuantityToReturn <> 0");
			command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = guid;
			command.Parameters.Add(new SqlParameter("@TransType", SqlDbType.TinyInt)).Value = Source;
			break;
		}
		case JournalLineTransactionType.InspectionInvToScrapQty:
		case JournalLineTransactionType.InspectionJobToScrapQty:
		{
			Guid guid = (Guid)sourceRow[base.Field.Table.UniqueField, rowVersion];
			command = database.NewSqlCommand("Select " + partTransFieldNames + " from PartTransactions Where imtTableUniqueID = @UniqueID And imtTransactionType = @TransType and imtQuantityToInspect = 0 and imtScrapQuantity <> 0");
			command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = guid;
			command.Parameters.Add(new SqlParameter("@TransType", SqlDbType.TinyInt)).Value = Source;
			break;
		}
		case JournalLineTransactionType.PartCostsAdjustment:
		case JournalLineTransactionType.StandardCostRollup:
		{
			Guid guid = (Guid)sourceRow[base.Field.Table.UniqueField, rowVersion];
			command = database.NewSqlCommand("Select " + partTransFieldNames + ", imtPartWarehouseLocationID from PartTransactions Where imtTableUniqueID = @UniqueID And imtTransactionType = @TransType And imtTransactionDate = @TransactionDate");
			command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = guid;
			command.Parameters.Add(new SqlParameter("@TransType", SqlDbType.TinyInt)).Value = Source;
			command.Parameters.Add(new SqlParameter("@TransactionDate", SqlDbType.DateTime)).Value = sourceRow.Field<DateTime>("imrLastTransactionDate");
			break;
		}
		case JournalLineTransactionType.MfgVarianceTransfer:
		{
			Guid guid = (Guid)sourceRow[base.Field.Table.UniqueField, rowVersion];
			command = database.NewSqlCommand("Select " + partTransFieldNames + ", imtPartWarehouseLocationID, imtSource, intPartTransactionCostID from PartTransactions inner join PartTransactionCosts on imtPartTransactionID = intPartTransactionID Where intUniqueID = @UniqueID ");
			command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = guid;
			break;
		}
		default:
		{
			Guid guid = (Guid)sourceRow[base.Field.Table.UniqueField, rowVersion];
			command = database.NewSqlCommand("Select " + partTransFieldNames + ", imtPartWarehouseLocationID from PartTransactions Where imtTableUniqueID = @UniqueID And imtTransactionType = @TransType");
			command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = guid;
			command.Parameters.Add(new SqlParameter("@TransType", SqlDbType.TinyInt)).Value = Source;
			break;
		}
		}
		if (string.IsNullOrEmpty(command.CommandText))
		{
			return;
		}
		DataTable dataTable = database.GetDataTable(command, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			if (journalLineTransactionType == JournalLineTransactionType.QuantityAdjustment || journalLineTransactionType == JournalLineTransactionType.MaterialIssueToJobGoodQty || journalLineTransactionType == JournalLineTransactionType.ShipFromInventory || journalLineTransactionType == JournalLineTransactionType.DMRShipmentInvQty || journalLineTransactionType == JournalLineTransactionType.WarehouseTransfer || journalLineTransactionType == JournalLineTransactionType.MiscIssueFromInventoryGoodQty)
			{
				bool flag = (bool)database.Props("FN")["xafGLCreateStockJournals"];
				string warehouseID = row["imtPartWarehouseLocationID"].ToString();
				if (GetWarehouseType(database, transaction, warehouseID, plantID) == 1 && flag)
				{
					break;
				}
			}
			if (journalLineTransactionType == JournalLineTransactionType.MfgVarianceTransfer)
			{
				command = database.NewSqlCommand("Select * from PartTransactionCosts Where intPartTransactionID = @ID and intPartTransactionCostID = @ID2");
				command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = row["imtPartTransactionID"];
				command.Parameters.Add(new SqlParameter("@ID2", SqlDbType.Int)).Value = row["intPartTransactionCostID"];
			}
			else
			{
				command = database.NewSqlCommand("Select * from PartTransactionCosts Where intPartTransactionID = @ID and intCostType = @CostType");
				command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = row["imtPartTransactionID"];
				command.Parameters.Add(new SqlParameter("@CostType", SqlDbType.SmallInt)).Value = (int)((overrideCostingMethod == CostingMethod.LIFO || overrideCostingMethod == CostingMethod.FIFO) ? CostingMethod.LIFO : overrideCostingMethod);
			}
			DataTable dataTable2 = database.GetDataTable(command, transaction);
			string text = string.Empty;
			if (dataTable2.Rows.Count != 0)
			{
				if ((base.Field.Table.TableName.Equals("InspectionLines", StringComparison.CurrentCultureIgnoreCase) || base.Field.Table.TableName.Equals("InspectionComponents", StringComparison.CurrentCultureIgnoreCase)) && sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "SourceTableName") && string.IsNullOrEmpty(sourceRow.Field<string>(base.Field.Table.FieldPrefix + "SourceTableName")) && dataTable.Rows.Count > 1 && dataTable2.Rows[0].Field<decimal>("intQuantity") < 0m)
				{
					continue;
				}
				Journal journal = PopulateJournalObject(database, transaction, row, dataTable2, sourceRow, rowVersion, cOGSAccounts2, qtyRatio, plantID, destInventoryGLAccount);
				if (journal != null)
				{
					SqlDataAdapter adapter;
					DataTable dataTable3 = database.GetDataTable("Select * From GLJournals Where 0=1", fillSchema: false, out adapter, transaction);
					DataRow dataRow2 = dataTable3.NewRow().BlankRow();
					dataRow2["glpTransactionDate"] = passedTransactionDate ?? journal.TransactionDate;
					dataRow2["glpReference"] = journal.Reference;
					dataRow2["glpDescription"] = journal.Description;
					dataRow2["glpLongDescriptionText"] = journal.LongDescriptionText;
					dataRow2["glpGLFiscalYearID"] = (UseFiscalYearAndPeriodFromJournal ? ((decimal)journal.GLFiscalYearID) : ProvidedFiscalYear);
					dataRow2["glpGLFiscalYearPeriodID"] = (UseFiscalYearAndPeriodFromJournal ? ((decimal)journal.GLFiscalYearPeriodID) : ProvidedFiscalPeriod);
					dataRow2["glpSource"] = journal.Source;
					dataRow2["glpDetailSource"] = journal.DetailSource;
					dataRow2["glpOrganizationID"] = journal.OrganizationID;
					dataRow2["glpLocationID"] = journal.LocationID;
					dataRow2["glpTotalDebits"] = journal.TotalDebits;
					dataRow2["glpTotalCredits"] = journal.TotalCredits;
					new COGS().PopulateJournalSourceInformation(dataRow2, sourceRow, rowVersion, base.Field.Table.FieldPrefix, jobID, jobAssemblyID);
					dataRow2["glpCreatedBy"] = database.User.ID;
					dataRow2["glpCreatedDate"] = DateTime.Now;
					dataRow2["glpGLJournalID"] = database.NextIDs.GetNextIDForTable("GLJournals", null, transaction);
					dataTable3.Rows.Add(dataRow2);
					database.UpdateData(new DataRow[1] { dataRow2 }, adapter, transaction);
					text = dataRow2["glpGLJournalID"].ToString();
					SqlDataAdapter adapter2;
					DataTable dataTable4 = database.GetDataTable("Select * From GLJournalLines Where 0=1", fillSchema: false, out adapter2, transaction);
					foreach (JournalLine journalLine in journal.JournalLines)
					{
						DataRow dataRow3 = dataTable4.NewRow().BlankRow();
						dataRow3["gllGLJournalID"] = dataRow2["glpGLJournalID"];
						dataRow3["gllGLJournalLineID"] = journalLine.LineID;
						dataRow3["gllTransactionType"] = journalLine.JournalLineTransactionType;
						dataRow3["gllTransactionAmount"] = journalLine.TransactionAmount;
						dataRow3["gllDebitAmount"] = journalLine.DebitAmount;
						dataRow3["gllCreditAmount"] = journalLine.CreditAmount;
						dataRow3["gllTransactionDate"] = passedTransactionDate ?? journalLine.TransactionDate;
						dataRow3["gllGLAccountID"] = journalLine.GLAccountID;
						dataRow3["gllGLFiscalYearID"] = (UseFiscalYearAndPeriodOnLinesFromJournal ? ((decimal)journal.GLFiscalYearID) : ProvidedFiscalYear);
						dataRow3["gllGLFiscalYearPeriodID"] = (UseFiscalYearAndPeriodOnLinesFromJournal ? ((decimal)journal.GLFiscalYearPeriodID) : ProvidedFiscalPeriod);
						dataRow3["gllReference"] = ((journalLine.Reference == string.Empty) ? journal.Reference : journalLine.Reference);
						dataRow3["gllDescription"] = ((journalLine.Description == string.Empty) ? journal.Description : journalLine.Description);
						dataRow3["gllPartTransactionID"] = journalLine.PartTransactionID;
						dataRow3["gllJobID"] = jobID;
						dataRow3["gllJobAssemblyID"] = jobAssemblyID;
						dataRow3["gllJobMaterialID"] = jobMaterialID;
						dataRow3["gllJobOperationID"] = jobOperationID;
						dataRow3["gllJobMaterialComponentID"] = jobMaterialComponentID;
						dataRow3["gllOrganizationID"] = journalLine.OrganizationID;
						dataRow3["gllLocationID"] = journalLine.LocationID;
						dataRow3["gllSourceTableName"] = base.Field.Table.TableName;
						dataRow3["gllSourceTableUniqueID"] = sourceRow[base.Field.Table.UniqueField, rowVersion];
						dataRow3["gllCreatedBy"] = database.User.ID;
						dataRow3["gllCreatedDate"] = DateTime.Now;
						dataTable4.Rows.Add(dataRow3);
					}
					database.UpdateData(dataTable4, adapter2, transaction);
				}
			}
			if (!string.IsNullOrWhiteSpace(text) && database.Props("FN").Field<bool>("xafProductionExpressPost"))
			{
				new GL().PostJournal(database, transaction, text);
			}
		}
	}

	private int GetWarehouseType(M1Database database, SqlTransaction transaction, string warehouseID, string plantID)
	{
		using SqlCommand sqlCommand = new SqlCommand("SELECT imwNonNettableType FROM Warehouses WHERE imwWarehouseID = @WarehouseID AND imwPlantID = @PlantID");
		sqlCommand.Parameters.AddWithValue("@WarehouseID", warehouseID);
		sqlCommand.Parameters.AddWithValue("@PlantID", plantID);
		object obj = database.ExecuteScalar(sqlCommand, transaction);
		return (obj != null) ? Convert.ToInt32(obj) : 0;
	}

	private string getPlantID(M1Database database, SqlTransaction transaction, DataRow sourceRow, DataRowVersion rowVersion)
	{
		return getPlantID(database, transaction, sourceRow, rowVersion, overrideToDestination: false);
	}

	private string getPlantID(M1Database database, SqlTransaction transaction, DataRow sourceRow, DataRowVersion rowVersion, bool overrideToDestination)
	{
		switch (journalLineTransactionType)
		{
		case JournalLineTransactionType.BinTransfer:
			if (!overrideToDestination)
			{
				return new Plant().GetWarehousePlant(database, transaction, sourceRow.Field<string>("inqPartWarehouseLocationID")).PlantID;
			}
			return new Plant().GetWarehousePlant(database, transaction, sourceRow.Field<string>("inqDestinationWarehouseID")).PlantID;
		case JournalLineTransactionType.BinReceipt:
			return new Plant().GetWarehousePlant(database, transaction, sourceRow.Field<string>("inqDestinationWarehouseID")).PlantID;
		case JournalLineTransactionType.WarehouseTransfer:
			if (!overrideToDestination)
			{
				return new Plant().GetWarehousePlant(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "SourceWarehouseID")).PlantID;
			}
			return new Plant().GetWarehousePlant(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "DestinationWarehouseID")).PlantID;
		case JournalLineTransactionType.WarehouseReceipt:
			return new Plant().GetWarehousePlant(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "DestinationWarehouseID")).PlantID;
		case JournalLineTransactionType.MaterialIssueToJobGoodQty:
		case JournalLineTransactionType.MaterialIssueToJobScrapQty:
			if (!overrideToDestination)
			{
				if (base.Field.Table.GetDocumentPlantID(database, sourceRow, transaction, rowVersion) != null)
				{
					return base.Field.Table.GetDocumentPlantID(database, sourceRow, transaction, rowVersion);
				}
				return string.Empty;
			}
			return new Plant().GetWarehousePlant(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PartWarehouseLocationID")).PlantID;
		case JournalLineTransactionType.InventoryCount:
			return sourceRow.Field<string>("imtPlantID");
		default:
			if (base.Field.Table.GetDocumentPlantID(database, sourceRow, transaction, rowVersion) != null)
			{
				return base.Field.Table.GetDocumentPlantID(database, sourceRow, transaction, rowVersion);
			}
			return string.Empty;
		}
	}

	private string getReasonID(M1Database database, DataRow sourceRow, DataRowVersion rowVersion, SqlTransaction transaction, string tableName)
	{
		string text = string.Empty;
		switch (base.Field.Table.TableName.ToUpper())
		{
		case "MATERIALISSUELINES":
		case "MATERIALISSUECOMPONENTS":
			text = "MiscIssueReasonID";
			break;
		case "INSPECTIONLINES":
		case "INSPECTIONCOMPONENTS":
			text = "ScrapReasonID";
			break;
		}
		string result = string.Empty;
		if (!string.IsNullOrWhiteSpace(text))
		{
			if (base.Field.BindingSource.Fields.Contains(base.Field.Table.FieldPrefix + text))
			{
				result = sourceRow.Field<string>(base.Field.Table.FieldPrefix + text, rowVersion);
			}
			else
			{
				try
				{
					if (base.Field.BindingSource.Fields[base.Field.Table.KeyFieldsArray[0]].RelatedTableGetDataRow(base.Field.Table.TopLevelKeyFields.Substring(0, 3) + text, database, sourceRow, alwaysReturnValidRow: true, transaction).Table.Columns.Contains(base.Field.Table.TopLevelKeyFields.Substring(0, 3) + text))
					{
						result = base.Field.BindingSource.Fields[base.Field.Table.KeyFieldsArray[0]].RelatedTableGetDataRow(base.Field.Table.TopLevelKeyFields.Substring(0, 3) + text, database, sourceRow, alwaysReturnValidRow: true, transaction).Field<string>(base.Field.Table.TopLevelKeyFields.Substring(0, 3) + text, rowVersion);
					}
					else
					{
						M1BindingSource parentBindingSource = base.Field.BindingSource.PrimaryTable.GetParentBindingSource(sourceRow);
						if (parentBindingSource != null && parentBindingSource.Fields.Contains(parentBindingSource.PrimaryTable.FieldPrefix + text) && parentBindingSource.CurrentAsDataRow != null)
						{
							result = parentBindingSource.CurrentAsDataRow.Field<string>(parentBindingSource.PrimaryTable.FieldPrefix + text, rowVersion);
						}
					}
				}
				catch
				{
				}
			}
		}
		return result;
	}

	private Journal PopulateJournalObject(M1Database database, SqlTransaction transaction, DataRow partTransactionRow, DataTable partTransactionCosts, DataRow sourceRow, DataRowVersion rowVersion, COGSAccounts accounts, decimal qtyRatio, string plantID, string destInventoryGLAccount)
	{
		Journal journal = new Journal();
		if (partTransactionCosts.Rows.Count != 0)
		{
			DateTime dateTime = partTransactionRow.Field<DateTime>("imtTransactionDate");
			short year = new Financial().GetYearAndPeriod(database, dateTime, "GL", IgnoreClosed: true, transaction).Year;
			byte period = new Financial().GetYearAndPeriod(database, dateTime, "GL", IgnoreClosed: true, transaction).Period;
			JournalSource headerSource = (JournalSource)getHeaderSource();
			DetailSource detailSource = getDetailSource();
			journal = new COGS().BuildJournalObject(database, transaction, dateTime, year, period, headerSource, detailSource, string.Empty);
			bool nonStockedStatus = getNonStockedStatus(database, transaction, partTransactionRow.Field<string>("imtPartID"));
			bool landedCost = false;
			if (sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "ReceiptID"))
			{
				landedCost = new Receipts().IsReceiptLandedCost(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "ReceiptID"));
			}
			decimal num = default(decimal);
			decimal num2 = default(decimal);
			decimal num3 = default(decimal);
			decimal num4 = default(decimal);
			int lineID = 0;
			foreach (DataRow row in partTransactionCosts.Rows)
			{
				num4 = setQuantity(row.Field<decimal>("intQuantity")) * Math.Abs(qtyRatio);
				Costs costs = setCostsFromPTCosts(row);
				if (journalLineTransactionType == JournalLineTransactionType.MfgVarianceTransfer)
				{
					costs = setCostsFromMfgVarianceCosts(database, row);
				}
				switch (journalLineTransactionType)
				{
				case JournalLineTransactionType.ShipFromInventory:
					setShipFromInventoryJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.ShipFromJob:
					setShipFromJobJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.MaterialIssueToJobGoodQty:
					setIssueMaterialToJobGoodQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, destInventoryGLAccount);
					break;
				case JournalLineTransactionType.MaterialIssueToJobScrapQty:
					setIssueMaterialToJobScrapQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, destInventoryGLAccount);
					break;
				case JournalLineTransactionType.MaterialIssueToJobReturnGoodQty:
					setIssueMaterialToJobGoodQtyJournalLines(partTransactionRow, accounts, journal, -num4, ref lineID, costs, destInventoryGLAccount);
					break;
				case JournalLineTransactionType.MaterialIssueToJobReturnScrapQty:
					setIssueMaterialToJobScrapQtyJournalLines(partTransactionRow, accounts, journal, -num4, ref lineID, costs, destInventoryGLAccount);
					break;
				case JournalLineTransactionType.MiscIssueFromInventoryGoodQty:
					setMiscIssueFromInventoryGoodQtyJournalLines(database, partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.MiscIssueFromInventoryScrapQty:
					setMiscIssueFromInventoryScrapQtyJournalLines(database, partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.QuantityAdjustment:
				case JournalLineTransactionType.InventoryCount:
					setQtyAdjustmentJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.BinTransfer:
				case JournalLineTransactionType.WarehouseTransfer:
					setWarehouseTransferJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, destInventoryGLAccount);
					break;
				case JournalLineTransactionType.BinReceipt:
				case JournalLineTransactionType.WarehouseReceipt:
					setWarehouseReceiptJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.ARInvoiceShipment:
					setInvoiceFromShipmentJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.ARInvoiceRMAReceipt:
					setInvoiceFromRmaReceiptJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.DMRShipmentInvQty:
					setDMRShipmentInvQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.DMRShipmentJobMatQty:
					num2 = default(decimal);
					if (base.Field.Table.TableName.Equals("DMRShipmentComponents", StringComparison.CurrentCultureIgnoreCase))
					{
						num2 = new DMRShipment().GetDMRComponentUnitCostsFromReceipt(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "DMRShipmentID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "DMRShipmentLineID"), sourceRow.Field<int>(base.Field.Table.FieldPrefix + "DMRShipmentComponentID"));
					}
					setDMRShipmentJobMatQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, sourceRow, num2);
					break;
				case JournalLineTransactionType.DMRShipmentJobOprQty:
					setDMRShipmentJobOprQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, sourceRow);
					break;
				case JournalLineTransactionType.DMRShipmentReturnQty:
					num2 = default(decimal);
					if (base.Field.Table.TableName.Equals("DMRShipmentComponents", StringComparison.CurrentCultureIgnoreCase))
					{
						num2 = new DMRShipment().GetDMRComponentUnitCostsFromReceipt(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "DMRShipmentID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "DMRShipmentLineID"), sourceRow.Field<int>(base.Field.Table.FieldPrefix + "DMRShipmentComponentID"));
					}
					setDMRShipmentReturnQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, sourceRow, num2);
					break;
				case JournalLineTransactionType.PurchaseOrderReceiptInvQty:
				{
					num = new PurchaseOrders().GetPurchaseOrderLineExtendedCost(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PurchaseOrderID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "PurchaseOrderLineID"));
					num3 = new PurchaseOrders().GetPurchaseOrderLineQuantity(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PurchaseOrderID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "PurchaseOrderLineID"));
					byte purchaseOrderLineType = new PurchaseOrders().GetPurchaseOrderLineType(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PurchaseOrderID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "PurchaseOrderLineID"));
					if (purchaseOrderLineType.Equals(4))
					{
						string purchaseOrderLineFixedAssetType = new PurchaseOrders().GetPurchaseOrderLineFixedAssetType(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PurchaseOrderID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "PurchaseOrderLineID"));
						setPOReceiptAssetJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, new FixedAsset().GetAssetTypeGLAccount(database, transaction, purchaseOrderLineFixedAssetType, plantID).AssetGLAccountID, landedCost, num, num3);
					}
					else if (purchaseOrderLineType.Equals(5))
					{
						List<ExpenseAccounts> purchaseOrderLineAccounts = new PurchaseAccounts().getPurchaseOrderLineAccounts(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PurchaseOrderID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "PurchaseOrderLineID"));
						if (purchaseOrderLineAccounts != null)
						{
							if (nonStockedStatus && costingMethod == CostingMethod.Standard)
							{
								setPOReceiptNonStockedJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, purchaseOrderLineAccounts, landedCost, num, num3);
							}
							else
							{
								setPOReceiptMiscJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, getPartTransactionCostsRecord(database, transaction, partTransactionRow.Field<int>("imtPartTransactionID"), CostingMethod.Standard), purchaseOrderLineAccounts, landedCost, num, num3);
							}
						}
					}
					else if (nonStockedStatus)
					{
						List<ExpenseAccounts> purchaseOrderLineAccounts2 = new PurchaseAccounts().getPurchaseOrderLineAccounts(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PurchaseOrderID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "PurchaseOrderLineID"));
						if (purchaseOrderLineAccounts2 != null && purchaseOrderLineAccounts2.Count > 0)
						{
							setPOReceiptNonStockedJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, purchaseOrderLineAccounts2, landedCost, num, num3);
							break;
						}
						List<ExpenseAccounts> accounts2 = new List<ExpenseAccounts>();
						accounts2 = new PurchaseAccounts().GetPartAccounts(database, transaction, accounts2, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PartID"), sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PartRevisionID"));
						if (accounts2 != null && accounts2.Count > 0)
						{
							setPOReceiptNonStockedJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, accounts2, landedCost, num, num3);
							break;
						}
						List<ExpenseAccounts> accounts3 = new List<ExpenseAccounts>();
						string receiptSupplierID = new Receipts().GetReceiptSupplierID(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "ReceiptID"));
						accounts3 = new PurchaseAccounts().GetSupplierAccounts(database, transaction, accounts3, receiptSupplierID);
						if (accounts3 != null)
						{
							setPOReceiptNonStockedJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, accounts3, landedCost, num, num3);
						}
					}
					else
					{
						setPOReceiptInvQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, getPartTransactionCostsRecord(database, transaction, partTransactionRow.Field<int>("imtPartTransactionID"), CostingMethod.Standard), landedCost, num, num3);
					}
					break;
				}
				case JournalLineTransactionType.PurchaseOrderReceiptJobMatQty:
					num = new PurchaseOrders().GetPurchaseOrderLineExtendedCost(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PurchaseOrderID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "PurchaseOrderLineID"));
					num3 = new PurchaseOrders().GetPurchaseOrderLineQuantity(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PurchaseOrderID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "PurchaseOrderLineID"));
					setPOReceiptJobMatQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, getPartTransactionCostsRecord(database, transaction, partTransactionRow.Field<int>("imtPartTransactionID"), CostingMethod.Standard), landedCost, num, num3);
					break;
				case JournalLineTransactionType.PurchaseOrderReceiptJobOprQty:
					num = new PurchaseOrders().GetPurchaseOrderLineExtendedCost(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PurchaseOrderID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "PurchaseOrderLineID"));
					num3 = new PurchaseOrders().GetPurchaseOrderLineQuantity(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PurchaseOrderID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "PurchaseOrderLineID"));
					setPOReceiptJobOprQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, getPartTransactionCostsRecord(database, transaction, partTransactionRow.Field<int>("imtPartTransactionID"), CostingMethod.LIFO), landedCost, num, num3);
					break;
				case JournalLineTransactionType.PurchaseOrderReceiptQtyToInspect:
					num = new PurchaseOrders().GetPurchaseOrderLineExtendedCost(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PurchaseOrderID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "PurchaseOrderLineID"));
					num3 = new PurchaseOrders().GetPurchaseOrderLineQuantity(database, transaction, sourceRow.Field<string>(base.Field.Table.FieldPrefix + "PurchaseOrderID"), sourceRow.Field<short>(base.Field.Table.FieldPrefix + "PurchaseOrderLineID"));
					if (jobOperationID != 0)
					{
						setPOReceiptQTISubcontractJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, getPartTransactionCostsRecord(database, transaction, partTransactionRow.Field<int>("imtPartTransactionID"), CostingMethod.Standard), landedCost, num, num3);
					}
					else
					{
						setPOReceiptQtyToInspectMaterialJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, getPartTransactionCostsRecord(database, transaction, partTransactionRow.Field<int>("imtPartTransactionID"), CostingMethod.Standard), landedCost, num, num3);
					}
					break;
				case JournalLineTransactionType.MiscReceiptToInventory:
					setMiscReceiptToInventoryJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, database.Props("FN").Field<byte>("xafMiscReceiptVarianceAccount"));
					break;
				case JournalLineTransactionType.MiscReceiptToJobMatQty:
					setMiscReceiptToJobMatQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, database.Props("FN").Field<byte>("xafMiscReceiptVarianceAccount"));
					break;
				case JournalLineTransactionType.MiscReceiptToJobAsmQty:
					setMiscReceiptToJobAsmQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, database.Props("FN").Field<byte>("xafMiscReceiptVarianceAccount"));
					break;
				case JournalLineTransactionType.MiscReceiptToJobOprQty:
					setMiscReceiptToJobOprQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, database.Props("FN").Field<byte>("xafMiscReceiptVarianceAccount"));
					break;
				case JournalLineTransactionType.MfgReceiptInvQty:
					SetMfgReceiptInvQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.MfgReceiptScrapQty:
					SetMfgReceiptScrapQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.MfgReceiptQtyToInspect:
					SetMfgReceiptQtyToInspectJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.RMAReceiptInvQty:
					setRmaReceiptInvQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.RMAReceiptQtyToInspect:
					setRmaReceiptQtyToInspectJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.InspectionInvAcceptedQty:
					setInspectionInvAcceptedQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, sourceRow);
					break;
				case JournalLineTransactionType.InspectionInvToReturnQty:
					setInspectionInvToReturnQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, sourceRow);
					break;
				case JournalLineTransactionType.InspectionInvToScrapQty:
					setInspectionInvToScrapQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, sourceRow);
					break;
				case JournalLineTransactionType.InspectionJobMatAcceptedQty:
					setInspectionJobMatAcceptedQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, sourceRow);
					break;
				case JournalLineTransactionType.InspectionJobOprAcceptedQty:
					setInspectionJobOprAcceptedQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, sourceRow);
					break;
				case JournalLineTransactionType.InspectionJobToReturnQty:
					setInspectionJobToReturnQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, sourceRow);
					break;
				case JournalLineTransactionType.InspectionJobToScrapQty:
					setInspectionJobToScrapQtyJournalLines(partTransactionRow, accounts, journal, num4, ref lineID, costs, sourceRow);
					break;
				case JournalLineTransactionType.PartCostsAdjustment:
				case JournalLineTransactionType.StandardCostRollup:
					setPartCostsAdjustmentJournalLines(database, transaction, partTransactionRow, sourceRow, journal, num4, ref lineID, costs);
					break;
				case JournalLineTransactionType.MfgVarianceTransfer:
					setMfgVarianceTransferJournalLines(database, transaction, partTransactionRow, sourceRow, journal, num4, ref lineID, costs);
					break;
				}
			}
			decimal num5 = journal.JournalLines.Sum((JournalLine x) => x.DebitAmount);
			decimal num6 = journal.JournalLines.Sum((JournalLine x) => x.CreditAmount);
			if (num5 != num6)
			{
				journal.JournalLines.Add(addRoundingJournalLine(journal.JournalLines.Max((JournalLine x) => x.LineID) + 1, num6 - num5, (string)database.Props("FN")["xafRoundingGLAccountID"]));
			}
			journal.TotalDebits = journal.JournalLines.Sum((JournalLine x) => x.DebitAmount);
			journal.TotalCredits = journal.JournalLines.Sum((JournalLine x) => x.CreditAmount);
			return journal;
		}
		return null;
	}

	private Costs setCostsFromPTCosts(DataRow costRow)
	{
		return new Costs
		{
			UnitMaterialCost = costRow.Field<decimal>("intUnitMaterialCost"),
			UnitSubcontractCost = costRow.Field<decimal>("intUnitSubcontractCost"),
			UnitLaborCost = costRow.Field<decimal>("intUnitLaborCost"),
			UnitOverheadCost = costRow.Field<decimal>("intUnitOverheadCost"),
			UnitDutyCost = costRow.Field<decimal>("intUnitDutyCost"),
			UnitFreightCost = costRow.Field<decimal>("intUnitFreightCost"),
			UnitMiscCost = costRow.Field<decimal>("intUnitMiscCost")
		};
	}

	private Costs setCostsFromMfgVarianceCosts(M1Database database, DataRow parttransactionRow)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("select top 1 mvlNewUnitMaterialCost    -  mvlOldUnitMaterialCost    as intUnitMaterialCost, mvlNewUnitSubcontractCost -  mvlOldUnitSubcontractCost as intUnitSubcontractCost, mvlNewUnitLaborCost       -  mvlOldUnitLaborCost       as intUnitLaborCost, mvlNewUnitOverheadCost    -  mvlOldUnitOverheadCost    as intUnitOverheadCost from ManufacturingVarianceLog where mvlPartTransactionID = @PartTransactionID and mvlPartTransactionCostID = @PartTransactionCostID order by mvlCreatedDate desc");
		sqlCommand.Parameters.Add(new SqlParameter("@PartTransactionID", SqlDbType.Int)).Value = parttransactionRow.Field<int>("intPartTransactionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartTransactionCostID", SqlDbType.Int)).Value = parttransactionRow.Field<int>("intPartTransactionCostID");
		DataTable dataTable = database.GetDataTable(sqlCommand);
		Costs costs = new Costs();
		if (dataTable.Rows.Count != 0)
		{
			costs.UnitMaterialCost = dataTable.Rows[0].Field<decimal>("intUnitMaterialCost");
			costs.UnitSubcontractCost = dataTable.Rows[0].Field<decimal>("intUnitSubcontractCost");
			costs.UnitLaborCost = dataTable.Rows[0].Field<decimal>("intUnitLaborCost");
			costs.UnitOverheadCost = dataTable.Rows[0].Field<decimal>("intUnitOverheadCost");
			costs.UnitDutyCost = default(decimal);
			costs.UnitFreightCost = default(decimal);
			costs.UnitMiscCost = default(decimal);
		}
		else
		{
			costs.UnitMaterialCost = default(decimal);
			costs.UnitSubcontractCost = default(decimal);
			costs.UnitLaborCost = default(decimal);
			costs.UnitOverheadCost = default(decimal);
			costs.UnitDutyCost = default(decimal);
			costs.UnitFreightCost = default(decimal);
			costs.UnitMiscCost = default(decimal);
		}
		return costs;
	}

	private void setMiscReceiptToJobAsmQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, byte miscReceiptVarAccount)
	{
		journal.Description = "Purchase to Job Transaction";
		decimal unitMaterialCost = costs.UnitMaterialCost;
		decimal unitSubcontractCost = costs.UnitSubcontractCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * unitMaterialCost, accounts.WIPMaterialGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * unitSubcontractCost, accounts.WIPSubcontractGLAccountID));
		if (miscReceiptVarAccount.Equals(1))
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitMaterialCost, accounts.SVarMaterialGLAccountID));
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitSubcontractCost, accounts.SVarSubcontractGLAccountID));
		}
		else if (miscReceiptVarAccount.Equals(2))
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * (unitMaterialCost + unitSubcontractCost), accounts.PurchaseVarianceGLAccountID));
		}
	}

	private void setMiscReceiptToJobMatQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, byte miscReceiptVarAccount)
	{
		journal.Description = "Purchase to Job Transaction";
		decimal unitMaterialCost = costs.UnitMaterialCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * unitMaterialCost, accounts.WIPMaterialGLAccountID));
		if (miscReceiptVarAccount.Equals(1))
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitMaterialCost, accounts.SVarMaterialGLAccountID));
		}
		else if (miscReceiptVarAccount.Equals(2))
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitMaterialCost, accounts.PurchaseVarianceGLAccountID));
		}
	}

	private void setMiscReceiptToJobOprQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, byte miscReceiptVarAccount)
	{
		journal.Description = "Purchase to Job Transaction";
		decimal unitSubcontractCost = costs.UnitSubcontractCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * unitSubcontractCost, accounts.WIPSubcontractGLAccountID));
		if (miscReceiptVarAccount.Equals(1))
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitSubcontractCost, accounts.SVarSubcontractGLAccountID));
		}
		else if (miscReceiptVarAccount.Equals(2))
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitSubcontractCost, accounts.PurchaseVarianceGLAccountID));
		}
	}

	private void setMiscReceiptToInventoryJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, byte miscReceiptVarAccount)
	{
		journal.Description = "Purchase to Stock Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryGLAccountID));
		if (miscReceiptVarAccount.Equals(1))
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.SVarMaterialGLAccountID));
		}
		else if (miscReceiptVarAccount.Equals(2))
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.PurchaseVarianceGLAccountID));
		}
	}

	private void setPOReceiptJobOprQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow stdCostRow, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		if (!landedCost)
		{
			journal.Description = "Purchase to Job Subcontract Transaction";
			decimal unitSubcontractCost = costs.UnitSubcontractCost;
			if (costingMethod != CostingMethod.Standard)
			{
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * unitSubcontractCost, accounts.WIPSubcontractGLAccountID));
			}
			else
			{
				decimal num = stdCostRow.Field<decimal>("intUnitSubcontractCost");
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.WIPSubcontractGLAccountID));
				decimal num2 = Math.Round(quantity * unitSubcontractCost, 2) - Math.Round(quantity * num, 2);
				if (num2 != 0m)
				{
					journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num2, accounts.PurchaseVarianceGLAccountID));
				}
			}
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitSubcontractCost, accounts.AccruedCreditorsGLAccountID));
		}
		else
		{
			setPOReceiptJobOprQtyLandedCostJournalLines(partTransactionRow, accounts, journal, quantity, ref lineID, costs, stdCostRow, landedCost, poLineExtCost, poLineInventoryQty);
		}
	}

	private void setPOReceiptJobOprQtyLandedCostJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow stdCostRow, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		journal.Description = "Purchase to Job Sub Transaction (Landed Cost)";
		decimal num = costs.UnitSubcontractCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		if (costingMethod != CostingMethod.Standard)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.WIPSubcontractGLAccountID));
			decimal num2 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitSubcontractCost, 2);
			if (num2 != 0m)
			{
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num2, accounts.AccruedCreditorsGLAccountID));
			}
			decimal num3 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num3, accounts.StockInTransitGLAccountID));
			return;
		}
		decimal num4 = stdCostRow.Field<decimal>("intUnitSubcontractCost");
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num4, accounts.WIPSubcontractGLAccountID));
		decimal num5 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitSubcontractCost, 2);
		if (num5 != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num5, accounts.AccruedCreditorsGLAccountID));
		}
		decimal num6 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num6, accounts.StockInTransitGLAccountID));
		decimal cost = Math.Round(quantity * num, 2) - Math.Round(quantity * num4, 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, cost, accounts.PurchaseVarianceGLAccountID));
	}

	private void setPOReceiptJobMatQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow stdCostRow, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		if (!landedCost)
		{
			journal.Description = "Purchase to Job Transaction";
			decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
			if (costingMethod != CostingMethod.Standard)
			{
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.WIPMaterialGLAccountID));
			}
			else
			{
				decimal num2 = stdCostRow.Field<decimal>("intUnitMaterialCost") + stdCostRow.Field<decimal>("intUnitDutyCost") + stdCostRow.Field<decimal>("intUnitFreightCost") + stdCostRow.Field<decimal>("intUnitMiscCost");
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num2, accounts.WIPMaterialGLAccountID));
				decimal num3 = Math.Round(quantity * num, 2) - Math.Round(quantity * num2, 2);
				if (num3 != 0m)
				{
					journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num3, accounts.PurchaseVarianceGLAccountID));
				}
			}
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.AccruedCreditorsGLAccountID));
		}
		else
		{
			setPOReceiptJobMatQtyLandedCostJournalLines(partTransactionRow, accounts, journal, quantity, ref lineID, costs, stdCostRow, landedCost, poLineExtCost, poLineInventoryQty);
		}
	}

	private void setPOReceiptJobMatQtyLandedCostJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow stdCostRow, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		journal.Description = "Purchase to Job Transaction (Landed Cost)";
		decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		if (costingMethod != CostingMethod.Standard)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.WIPMaterialGLAccountID));
			decimal num2 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitMaterialCost, 2);
			if (num2 != 0m)
			{
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num2, accounts.AccruedCreditorsGLAccountID));
			}
			decimal num3 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num3, accounts.StockInTransitGLAccountID));
			return;
		}
		decimal num4 = stdCostRow.Field<decimal>("intUnitMaterialCost") + stdCostRow.Field<decimal>("intUnitDutyCost") + stdCostRow.Field<decimal>("intUnitFreightCost") + stdCostRow.Field<decimal>("intUnitMiscCost");
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num4, accounts.WIPMaterialGLAccountID));
		decimal num5 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitMaterialCost, 2);
		if (num5 != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num5, accounts.AccruedCreditorsGLAccountID));
		}
		decimal num6 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num6, accounts.StockInTransitGLAccountID));
		decimal cost = Math.Round(quantity * num, 2) - Math.Round(quantity * num4, 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, cost, accounts.PurchaseVarianceGLAccountID));
	}

	private void setDMRShipmentReturnQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow sourceRow, decimal dmrComUnitCost)
	{
		journal.Description = "DMR Shipment Return Quantity";
		decimal num;
		if (costingMethod != CostingMethod.Standard)
		{
			num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.AccruedCreditorsGLAccountID));
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryToReturnGLAccountID));
			return;
		}
		num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		decimal num2 = default(decimal);
		num2 = ((!base.Field.Table.TableName.Equals("DMRShipmentLines", StringComparison.CurrentCultureIgnoreCase)) ? dmrComUnitCost : sourceRow.Field<decimal>("dslUnitPrice"));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num2, accounts.AccruedCreditorsGLAccountID));
		decimal num3 = Math.Round(quantity * num, 2) - Math.Round(quantity * num2, 2);
		if (num3 != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num3, accounts.PurchaseVarianceGLAccountID));
		}
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryToReturnGLAccountID));
	}

	private void setDMRShipmentInvQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		journal.Description = "DMR Shipment Invoice Quantity";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.AccruedCreditorsGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryGLAccountID));
	}

	private void setDMRShipmentJobMatQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow sourceRow, decimal dmrComUnitCost)
	{
		journal.Description = "DMR Shipment Job Material Quantity";
		decimal num;
		if (costingMethod != CostingMethod.Standard)
		{
			num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.AccruedCreditorsGLAccountID));
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryToReturnGLAccountID));
			return;
		}
		num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		decimal num2 = default(decimal);
		num2 = ((!base.Field.Table.TableName.Equals("DMRShipmentLines", StringComparison.CurrentCultureIgnoreCase)) ? dmrComUnitCost : sourceRow.Field<decimal>("dslUnitPrice"));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num2, accounts.AccruedCreditorsGLAccountID));
		decimal num3 = Math.Round(quantity * num, 2) - Math.Round(quantity * num2, 2);
		if (num3 != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num3, accounts.PurchaseVarianceGLAccountID));
		}
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryToReturnGLAccountID));
	}

	private void setDMRShipmentJobOprQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow sourceRow)
	{
		journal.Description = "DMR Shipment Job Operation Quantity";
		decimal num;
		if (costingMethod != CostingMethod.Standard)
		{
			num = costs.UnitSubcontractCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.AccruedCreditorsGLAccountID));
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryToReturnGLAccountID));
			return;
		}
		num = costs.UnitSubcontractCost;
		decimal num2 = default(decimal);
		num2 = sourceRow.Field<decimal>("dslUnitPrice");
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num2, accounts.AccruedCreditorsGLAccountID));
		decimal num3 = Math.Round(quantity * num, 2) - Math.Round(quantity * num2, 2);
		if (num3 != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num3, accounts.PurchaseVarianceGLAccountID));
		}
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryToReturnGLAccountID));
	}

	private void setPOReceiptInvQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow stdCostRow, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		if (!landedCost)
		{
			journal.Description = "Purchase to Stock Transaction";
			decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
			if (costingMethod != CostingMethod.Standard)
			{
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryGLAccountID));
			}
			else
			{
				decimal num2 = stdCostRow.Field<decimal>("intUnitMaterialCost") + stdCostRow.Field<decimal>("intUnitDutyCost") + stdCostRow.Field<decimal>("intUnitFreightCost") + stdCostRow.Field<decimal>("intUnitMiscCost");
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num2, accounts.InventoryGLAccountID));
				decimal num3 = Math.Round(quantity * num, 2) - Math.Round(quantity * num2, 2);
				if (num3 != 0m)
				{
					journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num3, accounts.PurchaseVarianceGLAccountID));
				}
			}
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.AccruedCreditorsGLAccountID));
		}
		else
		{
			setPOReceiptInvQtyLandedCostJournalLines(partTransactionRow, accounts, journal, quantity, ref lineID, costs, stdCostRow, landedCost, poLineExtCost, poLineInventoryQty);
		}
	}

	private void setPOReceiptInvQtyLandedCostJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow stdCostRow, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		journal.Description = "Purchase to Stock Transaction (Landed Cost)";
		decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		if (costingMethod != CostingMethod.Standard)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryGLAccountID));
			decimal num2 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitMaterialCost, 2);
			if (num2 != 0m)
			{
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num2, accounts.AccruedCreditorsGLAccountID));
			}
			decimal num3 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num3, accounts.StockInTransitGLAccountID));
			return;
		}
		decimal num4 = stdCostRow.Field<decimal>("intUnitMaterialCost") + stdCostRow.Field<decimal>("intUnitDutyCost") + stdCostRow.Field<decimal>("intUnitFreightCost") + stdCostRow.Field<decimal>("intUnitMiscCost");
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num4, accounts.InventoryGLAccountID));
		decimal num5 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitMaterialCost, 2);
		if (num5 != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num5, accounts.AccruedCreditorsGLAccountID));
		}
		decimal num6 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num6, accounts.StockInTransitGLAccountID));
		decimal cost = Math.Round(quantity * num, 2) - Math.Round(quantity * num4, 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, cost, accounts.PurchaseVarianceGLAccountID));
	}

	private void setPOReceiptQtyToInspectMaterialJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow stdCostRow, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		if (!landedCost)
		{
			journal.Description = "Purchase to Inspection Transaction";
			decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
			if (costingMethod != CostingMethod.Standard)
			{
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryInInspectionGLAccountID));
			}
			else
			{
				decimal num2 = stdCostRow.Field<decimal>("intUnitMaterialCost") + stdCostRow.Field<decimal>("intUnitDutyCost") + stdCostRow.Field<decimal>("intUnitFreightCost") + stdCostRow.Field<decimal>("intUnitMiscCost");
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num2, accounts.InventoryInInspectionGLAccountID));
				decimal num3 = Math.Round(quantity * num, 2) - Math.Round(quantity * num2, 2);
				if (num3 != 0m)
				{
					journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num3, accounts.PurchaseVarianceGLAccountID));
				}
			}
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.AccruedCreditorsGLAccountID));
		}
		else
		{
			setPOReceiptQtyToInspectMaterialLandedCostJournalLines(partTransactionRow, accounts, journal, quantity, ref lineID, costs, stdCostRow, landedCost, poLineExtCost, poLineInventoryQty);
		}
	}

	private void setPOReceiptQtyToInspectMaterialLandedCostJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow stdCostRow, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		journal.Description = "Purchase to Inspection Transaction (Landed Cost)";
		decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		if (costingMethod != CostingMethod.Standard)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryInInspectionGLAccountID));
			decimal num2 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitMaterialCost, 2);
			if (num2 != 0m)
			{
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num2, accounts.AccruedCreditorsGLAccountID));
			}
			decimal num3 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num3, accounts.StockInTransitGLAccountID));
			return;
		}
		decimal num4 = stdCostRow.Field<decimal>("intUnitMaterialCost") + stdCostRow.Field<decimal>("intUnitDutyCost") + stdCostRow.Field<decimal>("intUnitFreightCost") + stdCostRow.Field<decimal>("intUnitMiscCost");
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num4, accounts.InventoryInInspectionGLAccountID));
		decimal num5 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitMaterialCost, 2);
		if (num5 != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num5, accounts.AccruedCreditorsGLAccountID));
		}
		decimal num6 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num6, accounts.StockInTransitGLAccountID));
		decimal cost = Math.Round(quantity * num, 2) - Math.Round(quantity * num4, 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, cost, accounts.PurchaseVarianceGLAccountID));
	}

	private void setPOReceiptQTISubcontractJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow stdCostRow, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		if (!landedCost)
		{
			journal.Description = "Purchase to Inspection Transaction";
			decimal unitSubcontractCost = costs.UnitSubcontractCost;
			if (costingMethod != CostingMethod.Standard)
			{
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * unitSubcontractCost, accounts.InventoryInInspectionGLAccountID));
			}
			else
			{
				decimal num = stdCostRow.Field<decimal>("intUnitSubcontractCost");
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryInInspectionGLAccountID));
				decimal num2 = Math.Round(quantity * unitSubcontractCost, 2) - Math.Round(quantity * num, 2);
				if (num2 != 0m)
				{
					journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num2, accounts.PurchaseVarianceGLAccountID));
				}
			}
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitSubcontractCost, accounts.AccruedCreditorsGLAccountID));
		}
		else
		{
			setPOReceiptQTISubcontractLandedCostJournalLines(partTransactionRow, accounts, journal, quantity, ref lineID, costs, stdCostRow, landedCost, poLineExtCost, poLineInventoryQty);
		}
	}

	private void setPOReceiptQTISubcontractLandedCostJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow stdCostRow, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		journal.Description = "Purchase to Inspection Transaction (Landed Cost)";
		decimal num = costs.UnitSubcontractCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		if (costingMethod != CostingMethod.Standard)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryInInspectionGLAccountID));
			decimal num2 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitSubcontractCost, 2);
			if (num2 != 0m)
			{
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num2, accounts.AccruedCreditorsGLAccountID));
			}
			decimal num3 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num3, accounts.StockInTransitGLAccountID));
			return;
		}
		decimal num4 = stdCostRow.Field<decimal>("intUnitSubcontractCost");
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num4, accounts.InventoryInInspectionGLAccountID));
		decimal num5 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitSubcontractCost, 2);
		if (num5 != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num5, accounts.AccruedCreditorsGLAccountID));
		}
		decimal num6 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num6, accounts.StockInTransitGLAccountID));
		decimal cost = Math.Round(quantity * num, 2) - Math.Round(quantity * num4, 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, cost, accounts.PurchaseVarianceGLAccountID));
	}

	private void setPOReceiptMiscJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow stdCostRow, List<ExpenseAccounts> poLineAccounts, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		if (!landedCost)
		{
			journal.Description = "Purchase to Stock Transaction";
			decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
			if (costingMethod != CostingMethod.Standard)
			{
				addJournalLinesForPOLineExpenseAccounts(partTransactionRow, journal, quantity, ref lineID, poLineAccounts, num);
			}
			else
			{
				decimal num2 = stdCostRow.Field<decimal>("intUnitMaterialCost") + stdCostRow.Field<decimal>("intUnitDutyCost") + stdCostRow.Field<decimal>("intUnitFreightCost") + stdCostRow.Field<decimal>("intUnitMiscCost");
				addJournalLinesForPOLineExpenseAccounts(partTransactionRow, journal, quantity, ref lineID, poLineAccounts, num2);
				decimal num3 = Math.Round(quantity * num, 2) - Math.Round(quantity * num2, 2);
				if (num3 != 0m)
				{
					journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num3, accounts.PurchaseVarianceGLAccountID));
				}
			}
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.AccruedCreditorsGLAccountID));
		}
		else
		{
			setPOReceiptMiscLandedCostJournalLines(partTransactionRow, accounts, journal, quantity, ref lineID, costs, stdCostRow, poLineAccounts, landedCost, poLineExtCost, poLineInventoryQty);
		}
	}

	private void setPOReceiptMiscLandedCostJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow stdCostRow, List<ExpenseAccounts> poLineAccounts, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		journal.Description = "Purchase to Stock Transaction (Landed Cost)";
		decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		if (costingMethod != CostingMethod.Standard)
		{
			addJournalLinesForPOLineExpenseAccounts(partTransactionRow, journal, quantity, ref lineID, poLineAccounts, num);
			decimal num2 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitMaterialCost, 2);
			if (num2 != 0m)
			{
				journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num2, accounts.AccruedCreditorsGLAccountID));
			}
			decimal num3 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num3, accounts.StockInTransitGLAccountID));
			return;
		}
		decimal num4 = stdCostRow.Field<decimal>("intUnitMaterialCost") + stdCostRow.Field<decimal>("intUnitDutyCost") + stdCostRow.Field<decimal>("intUnitFreightCost") + stdCostRow.Field<decimal>("intUnitMiscCost");
		addJournalLinesForPOLineExpenseAccounts(partTransactionRow, journal, quantity, ref lineID, poLineAccounts, num4);
		decimal num5 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitMaterialCost, 2);
		if (num5 != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num5, accounts.AccruedCreditorsGLAccountID));
		}
		decimal num6 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num6, accounts.StockInTransitGLAccountID));
		decimal cost = Math.Round(quantity * num, 2) - Math.Round(quantity * num4, 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, cost, accounts.PurchaseVarianceGLAccountID));
	}

	private void setPOReceiptNonStockedJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, List<ExpenseAccounts> poLineAccounts, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		if (!landedCost)
		{
			journal.Description = "Non-Stock Purchase Transaction";
			decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
			addJournalLinesForPOLineExpenseAccounts(partTransactionRow, journal, quantity, ref lineID, poLineAccounts, num);
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.AccruedCreditorsGLAccountID));
		}
		else
		{
			setPOReceiptNonStockedLandedCostsJournalLines(partTransactionRow, accounts, journal, quantity, ref lineID, costs, poLineAccounts, landedCost, poLineExtCost, poLineInventoryQty);
		}
	}

	private void setPOReceiptNonStockedLandedCostsJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, List<ExpenseAccounts> poLineAccounts, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		journal.Description = "Non-Stock Purchase Transaction (Landed Cost)";
		decimal cost = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		addJournalLinesForPOLineExpenseAccounts(partTransactionRow, journal, quantity, ref lineID, poLineAccounts, cost);
		decimal num = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitMaterialCost, 2);
		if (num != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num, accounts.AccruedCreditorsGLAccountID));
		}
		decimal num2 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num2, accounts.StockInTransitGLAccountID));
	}

	private void setPOReceiptAssetJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, string assetTypeGLAccountID, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		if (!landedCost)
		{
			journal.Description = "Asset Purchase Transaction";
			decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, assetTypeGLAccountID));
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.AccruedCreditorsGLAccountID));
		}
		else
		{
			setPOReceiptAssetLandedCostJournalLines(partTransactionRow, accounts, journal, quantity, ref lineID, costs, assetTypeGLAccountID, landedCost, poLineExtCost, poLineInventoryQty);
		}
	}

	private void setPOReceiptAssetLandedCostJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, string assetTypeGLAccountID, bool landedCost, decimal poLineExtCost, decimal poLineInventoryQty)
	{
		journal.Description = "Asset Purchase Transaction (Landed Cost)";
		decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, assetTypeGLAccountID));
		decimal num2 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) - Math.Round(quantity * costs.UnitMaterialCost, 2);
		if (num2 != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, num2, accounts.AccruedCreditorsGLAccountID));
		}
		decimal num3 = Math.Round(poLineExtCost / poLineInventoryQty * quantity, 2) + Math.Round(quantity * (costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost), 2);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -num3, accounts.StockInTransitGLAccountID));
	}

	private void addJournalLinesForPOLineExpenseAccounts(DataRow partTransactionRow, Journal journal, decimal quantity, ref int lineID, List<ExpenseAccounts> poLineAccounts, decimal cost)
	{
		foreach (ExpenseAccounts poLineAccount in poLineAccounts)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * cost * (poLineAccount.Percent / 100m), poLineAccount.ExpenseAccountID));
		}
	}

	private void setShipFromInventoryJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		journal.Description = "Ship from Stock Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.ShipAwaitInvoiceGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryGLAccountID));
	}

	private void setShipFromJobJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		journal.Description = "Ship from Job Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.ShipAwaitInvoiceGLAccountID));
		num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.WIPMaterialGLAccountID));
		num = costs.UnitLaborCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.WIPLaborGLAccountID));
		num = costs.UnitSubcontractCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.WIPSubcontractGLAccountID));
		num = costs.UnitOverheadCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.WIPOverheadGLAccountID));
	}

	private void SetMfgReceiptInvQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		journal.Description = "WIP to Stock Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		decimal num2 = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		decimal unitLaborCost = costs.UnitLaborCost;
		decimal unitSubcontractCost = costs.UnitSubcontractCost;
		decimal unitOverheadCost = costs.UnitOverheadCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryGLAccountID));
		if (num2 != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num2, accounts.WIPMaterialGLAccountID));
		}
		if (unitLaborCost != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitLaborCost, accounts.WIPLaborGLAccountID));
		}
		if (unitSubcontractCost != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitSubcontractCost, accounts.WIPSubcontractGLAccountID));
		}
		if (unitOverheadCost != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitOverheadCost, accounts.WIPOverheadGLAccountID));
		}
	}

	private void SetMfgReceiptScrapQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		journal.Description = "WIP to Stock Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		decimal num2 = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		decimal unitLaborCost = costs.UnitLaborCost;
		decimal unitSubcontractCost = costs.UnitSubcontractCost;
		decimal unitOverheadCost = costs.UnitOverheadCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryGLAccountID));
		if (num2 != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num2, accounts.WIPMaterialGLAccountID));
		}
		if (unitLaborCost != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitLaborCost, accounts.WIPLaborGLAccountID));
		}
		if (unitSubcontractCost != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitSubcontractCost, accounts.WIPSubcontractGLAccountID));
		}
		if (unitOverheadCost != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitOverheadCost, accounts.WIPOverheadGLAccountID));
		}
	}

	private void SetMfgReceiptQtyToInspectJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		journal.Description = "WIP to Inspection Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		decimal num2 = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		decimal unitLaborCost = costs.UnitLaborCost;
		decimal unitSubcontractCost = costs.UnitSubcontractCost;
		decimal unitOverheadCost = costs.UnitOverheadCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryInInspectionGLAccountID));
		if (num2 != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num2, accounts.WIPMaterialGLAccountID));
		}
		if (unitLaborCost != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitLaborCost, accounts.WIPLaborGLAccountID));
		}
		if (unitSubcontractCost != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitSubcontractCost, accounts.WIPSubcontractGLAccountID));
		}
		if (unitOverheadCost != 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * unitOverheadCost, accounts.WIPOverheadGLAccountID));
		}
	}

	private void setIssueMaterialToJobGoodQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, string destInventoryGLAccount)
	{
		journal.Description = "Material Issue to Job Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.WIPMaterialGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, destInventoryGLAccount));
	}

	private void setIssueMaterialToJobScrapQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, string destInventoryGLAccount)
	{
		journal.Description = "Scrap Quantity from Material Issue to Job Transact";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.WIPMaterialGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, destInventoryGLAccount));
	}

	private void setMiscIssueFromInventoryGoodQtyJournalLines(M1Database database, DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		_ = string.Empty;
		journal.Description = "Miscellaneous Issue";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.ReasonGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryGLAccountID));
	}

	private void setMiscIssueFromInventoryScrapQtyJournalLines(M1Database database, DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		journal.Description = "Scrap Quantity from Miscellaneous Issue";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.ReasonGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryGLAccountID));
	}

	private void setQtyAdjustmentJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		journal.Description = "Stock Quantity Adjustment Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.StockRevaluationGLAccountID));
	}

	private void setWarehouseTransferJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, string destInventoryGLAccount)
	{
		journal.Description = "Warehouse Transfer Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, destInventoryGLAccount));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryGLAccountID));
	}

	private void setWarehouseReceiptJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		journal.Description = "Warehouse Receipt Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryInTransferGLAccountID));
	}

	private void setInvoiceFromShipmentJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		if (!string.IsNullOrWhiteSpace(jobID))
		{
			journal.Description = "Ship from Job Transaction";
		}
		else
		{
			journal.Description = "Ship from Stock Transaction";
		}
		decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.COGSMaterialGLAccountID));
		num = costs.UnitLaborCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.COGSLaborGLAccountID));
		num = costs.UnitSubcontractCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.COGSSubcontractGLAccountID));
		num = costs.UnitOverheadCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.COGSOverheadGLAccountID));
		num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.ShipAwaitInvoiceGLAccountID));
	}

	private void setInvoiceFromRmaReceiptJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		journal.Description = "Rma Receipt to Stock Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.ShipAwaitInvoiceGLAccountID));
		num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.COGSMaterialGLAccountID));
		num = costs.UnitLaborCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.COGSLaborGLAccountID));
		num = costs.UnitSubcontractCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.COGSSubcontractGLAccountID));
		num = costs.UnitOverheadCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.COGSOverheadGLAccountID));
	}

	private void setRmaReceiptInvQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		journal.Description = "Rma Receipt to Stock Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.ShipAwaitInvoiceGLAccountID));
	}

	private void setRmaReceiptQtyToInspectJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		journal.Description = "Rma Receipt to Inspection Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryInInspectionGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.ShipAwaitInvoiceGLAccountID));
	}

	private void setInspectionInvAcceptedQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow sourceRow)
	{
		journal.Description = "Inspection to Stock Transaction";
		decimal num = (base.Field.Table.TableName.Equals("InspectionLines", StringComparison.CurrentCultureIgnoreCase) ? ((!string.IsNullOrEmpty(sourceRow.Field<string>("qalSourceTableName")) && !sourceRow.Field<string>("qalSourceTableName").Trim().Equals("MfgReceipts", StringComparison.CurrentCultureIgnoreCase) && !sourceRow.Field<string>("qalSourceTableName").Trim().Equals("RMAReceiptLines", StringComparison.InvariantCultureIgnoreCase)) ? (costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost) : (costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost)) : ((!string.IsNullOrEmpty(sourceRow.Field<string>("qamSourceTableName")) && !sourceRow.Field<string>("qamSourceTableName").Trim().Equals("RMAReceiptComponents", StringComparison.InvariantCultureIgnoreCase)) ? (costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost) : (costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost)));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryInInspectionGLAccountID));
	}

	private void setInspectionInvToReturnQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow sourceRow)
	{
		journal.Description = "Inspection to Return from Stock Transaction";
		decimal num = (base.Field.Table.TableName.Equals("InspectionLines", StringComparison.CurrentCultureIgnoreCase) ? ((!string.IsNullOrEmpty(sourceRow.Field<string>("qalSourceTableName")) && !sourceRow.Field<string>("qalSourceTableName").Trim().Equals("MfgReceipts", StringComparison.InvariantCultureIgnoreCase) && !sourceRow.Field<string>("qalSourceTableName").Trim().Equals("RMAReceiptLines", StringComparison.InvariantCultureIgnoreCase)) ? (costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost) : (costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost)) : ((!string.IsNullOrEmpty(sourceRow.Field<string>("qamSourceTableName")) && !sourceRow.Field<string>("qamSourceTableName").Trim().Equals("RMAReceiptComponents", StringComparison.InvariantCultureIgnoreCase)) ? (costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost) : (costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost)));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryToReturnGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryInInspectionGLAccountID));
	}

	private void setInspectionInvToScrapQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow sourceRow)
	{
		journal.Description = "Inspection to Scrap from Stock Transaction";
		decimal num = (base.Field.Table.TableName.Equals("InspectionLines", StringComparison.CurrentCultureIgnoreCase) ? ((!string.IsNullOrEmpty(sourceRow.Field<string>("qalSourceTableName")) && !sourceRow.Field<string>("qalSourceTableName").Trim().Equals("MfgReceipts", StringComparison.CurrentCultureIgnoreCase) && !sourceRow.Field<string>("qalSourceTableName").Trim().Equals("RMAReceiptLines", StringComparison.InvariantCultureIgnoreCase)) ? (costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost) : (costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost)) : ((!string.IsNullOrEmpty(sourceRow.Field<string>("qamSourceTableName")) && !sourceRow.Field<string>("qamSourceTableName").Trim().Equals("RMAReceiptComponents", StringComparison.InvariantCultureIgnoreCase)) ? (costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost) : (costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost)));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.ScrapGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryInInspectionGLAccountID));
	}

	private void setInspectionJobMatAcceptedQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow sourceRow)
	{
		journal.Description = "Inspection to Job Material Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.WIPMaterialGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryInInspectionGLAccountID));
	}

	private void setInspectionJobOprAcceptedQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow sourceRow)
	{
		journal.Description = "Inspection to Job Operation Transaction";
		decimal num = costs.UnitSubcontractCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.WIPSubcontractGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryInInspectionGLAccountID));
	}

	private void setInspectionJobToReturnQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow sourceRow)
	{
		journal.Description = "Inspection to Return from Job Transaction";
		decimal num = (((base.Field.Table.TableName.Equals("InspectionComponents", StringComparison.CurrentCultureIgnoreCase) ? 1 : sourceRow.Field<byte>("qalJobType")) != 1) ? (costs.UnitSubcontractCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost) : (costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.InventoryToReturnGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryInInspectionGLAccountID));
	}

	private void setInspectionJobToScrapQtyJournalLines(DataRow partTransactionRow, COGSAccounts accounts, Journal journal, decimal quantity, ref int lineID, Costs costs, DataRow sourceRow)
	{
		journal.Description = "Inspection to Scrap from Job Transaction";
		decimal num = (((base.Field.Table.TableName.Equals("InspectionComponents", StringComparison.CurrentCultureIgnoreCase) ? 1 : sourceRow.Field<byte>("qalJobType")) != 1) ? (costs.UnitSubcontractCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost) : (costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, accounts.ScrapGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, accounts.InventoryInInspectionGLAccountID));
	}

	private void setPartCostsAdjustmentJournalLines(M1Database database, SqlTransaction transaction, DataRow partTransactionRow, DataRow sourceRow, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		journal.Description = "Stock Cost Adjustment Transaction";
		decimal num = costs.UnitMaterialCost + costs.UnitSubcontractCost + costs.UnitLaborCost + costs.UnitOverheadCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		string plantID = new Plant().GetWarehousePlant(database, transaction, partTransactionRow.Field<string>("imtPartWarehouseLocationID")).PlantID;
		COGSAccounts cOGSAccounts = new COGS().GetCOGSAccounts(database, transaction, sourceRow.Field<string>("imrPartID"), plantID, string.Empty, string.Empty);
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, quantity * num, cOGSAccounts.InventoryGLAccountID));
		journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, -quantity * num, cOGSAccounts.StockRevaluationGLAccountID));
	}

	private void setMfgVarianceTransferJournalLines(M1Database database, SqlTransaction transaction, DataRow partTransactionRow, DataRow sourceRow, Journal journal, decimal quantity, ref int lineID, Costs costs)
	{
		int num = 1;
		int num2 = 1;
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		string empty5 = string.Empty;
		string empty6 = string.Empty;
		string empty7 = string.Empty;
		string empty8 = string.Empty;
		string partGroupID = string.Empty;
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(smlPartGroupID,'') as PartGroupID from PartTransactions left outer join ShipmentLines on imtTableUniqueID = smlUniqueID where imtPartTransactionID = @TransactionID ");
		sqlCommand.Parameters.Add(new SqlParameter("@TransactionID", SqlDbType.Int)).Value = partTransactionRow.Field<int>("imtPartTransactionID");
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			partGroupID = dataTable.Rows[0].Field<string>("PartGroupID");
		}
		string plantID = new Plant().GetWarehousePlant(database, transaction, partTransactionRow.Field<string>("imtPartWarehouseLocationID")).PlantID;
		COGSAccounts cOGSAccounts = new COGS().GetCOGSAccounts(database, transaction, partTransactionRow.Field<string>("imtPartID"), plantID, partGroupID, string.Empty);
		switch (partTransactionRow.Field<byte>("imtSource"))
		{
		case 1:
			journal.Description = "Mfg Variance Transaction";
			journal.DetailSource = DetailSource.MfgVariance;
			num = 1;
			num2 = -1;
			empty = cOGSAccounts.SVarMaterialGLAccountID;
			empty2 = cOGSAccounts.SVarLaborGLAccountID;
			empty3 = cOGSAccounts.SVarSubcontractGLAccountID;
			empty4 = cOGSAccounts.SVarOverheadGLAccountID;
			empty5 = cOGSAccounts.WIPMaterialGLAccountID;
			empty6 = cOGSAccounts.WIPLaborGLAccountID;
			empty7 = cOGSAccounts.WIPSubcontractGLAccountID;
			empty8 = cOGSAccounts.WIPOverheadGLAccountID;
			setMfgVarianceTransferJournalLinesDetail(journal, costs, quantity, lineID, partTransactionRow, num, num2, empty, empty3, empty2, empty4, empty5, empty7, empty6, empty8);
			break;
		case 4:
		{
			int num3 = 0;
			journal.Description = "Shipment from Job Variance Transaction";
			journal.DetailSource = DetailSource.MfgVariance;
			sqlCommand = database.NewSqlCommand("Select omdDeliveryType From SalesOrderDeliveries Inner Join ShipmentLines On omdSalesOrderID = smlSalesOrderID And omdSalesOrderLineID = smlSalesOrderLineID And omdSalesOrderDeliveryID = smlSalesOrderDeliveryID Inner Join  PartTransactions On smlUniqueID = imtTableUniqueID Where imtPartTransactionID = @TransactionID");
			sqlCommand.Parameters.Add(new SqlParameter("@TransactionID", SqlDbType.Int)).Value = partTransactionRow.Field<int>("imtPartTransactionID");
			dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				num3 = dataTable.Rows[0].Field<byte>("omdDeliveryType");
				if (num3 == 5)
				{
					journal.Description = "Purchase to Order Variance Transaction";
				}
			}
			switch (num3)
			{
			case 1:
				num = -1;
				num2 = 1;
				empty = cOGSAccounts.COGSMaterialGLAccountID;
				empty2 = cOGSAccounts.COGSLaborGLAccountID;
				empty3 = cOGSAccounts.COGSSubcontractGLAccountID;
				empty4 = cOGSAccounts.COGSOverheadGLAccountID;
				empty5 = cOGSAccounts.WIPMaterialGLAccountID;
				empty6 = cOGSAccounts.WIPLaborGLAccountID;
				empty7 = cOGSAccounts.WIPSubcontractGLAccountID;
				empty8 = cOGSAccounts.WIPOverheadGLAccountID;
				setMfgVarianceTransferJournalLinesDetail(journal, costs, quantity, lineID, partTransactionRow, num, num2, empty, empty3, empty2, empty4, empty5, empty7, empty6, empty8);
				break;
			case 5:
				num = -1;
				num2 = 1;
				empty = cOGSAccounts.COGSMaterialGLAccountID;
				empty2 = cOGSAccounts.COGSLaborGLAccountID;
				empty3 = cOGSAccounts.COGSSubcontractGLAccountID;
				empty4 = cOGSAccounts.COGSOverheadGLAccountID;
				empty5 = cOGSAccounts.InventoryGLAccountID;
				empty6 = cOGSAccounts.InventoryGLAccountID;
				empty7 = cOGSAccounts.InventoryGLAccountID;
				empty8 = cOGSAccounts.InventoryGLAccountID;
				setMfgVarianceTransferJournalLinesDetail(journal, costs, quantity, lineID, partTransactionRow, num, num2, empty, empty3, empty2, empty4, empty5, empty7, empty6, empty8);
				break;
			default:
				num = 1;
				num2 = -1;
				empty = cOGSAccounts.SVarMaterialGLAccountID;
				empty2 = cOGSAccounts.SVarLaborGLAccountID;
				empty3 = cOGSAccounts.SVarSubcontractGLAccountID;
				empty4 = cOGSAccounts.SVarOverheadGLAccountID;
				empty5 = cOGSAccounts.WIPMaterialGLAccountID;
				empty6 = cOGSAccounts.WIPLaborGLAccountID;
				empty7 = cOGSAccounts.WIPSubcontractGLAccountID;
				empty8 = cOGSAccounts.WIPOverheadGLAccountID;
				setMfgVarianceTransferJournalLinesDetail(journal, costs, quantity, lineID, partTransactionRow, num, num2, empty, empty3, empty2, empty4, empty5, empty7, empty6, empty8);
				break;
			}
			break;
		}
		default:
			journal.Description = "Cost Variance Transaction";
			break;
		}
	}

	private void setMfgVarianceTransferJournalLinesDetail(Journal journal, Costs costs, decimal quantity, int lineID, DataRow partTransactionRow, int debitSign, int creditSign, string destMaterialAccountID, string destSubcontractAccountID, string destLabourAccountID, string destOverheadAccountID, string offsetMaterialAccountID, string offsetSubcontractAccountID, string offsetLabourAccountID, string offsetOverheadAccountID)
	{
		decimal num = default(decimal);
		num = costs.UnitMaterialCost + costs.UnitDutyCost + costs.UnitFreightCost + costs.UnitMiscCost;
		if (num > 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)debitSign * quantity * Math.Abs(num), destMaterialAccountID));
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)creditSign * quantity * Math.Abs(num), offsetMaterialAccountID));
		}
		else if (num < 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)debitSign * quantity * Math.Abs(num), offsetMaterialAccountID));
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)creditSign * quantity * Math.Abs(num), destMaterialAccountID));
		}
		num = costs.UnitLaborCost;
		if (num > 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)debitSign * quantity * Math.Abs(num), destLabourAccountID));
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)creditSign * quantity * Math.Abs(num), offsetLabourAccountID));
		}
		else if (num < 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)debitSign * quantity * Math.Abs(num), offsetLabourAccountID));
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)creditSign * quantity * Math.Abs(num), destLabourAccountID));
		}
		num = costs.UnitSubcontractCost;
		if (num > 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)debitSign * quantity * Math.Abs(num), destSubcontractAccountID));
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)creditSign * quantity * Math.Abs(num), offsetSubcontractAccountID));
		}
		else if (num < 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)debitSign * quantity * Math.Abs(num), offsetSubcontractAccountID));
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)creditSign * quantity * Math.Abs(num), destSubcontractAccountID));
		}
		num = costs.UnitOverheadCost;
		if (num > 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)debitSign * quantity * Math.Abs(num), destOverheadAccountID));
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)creditSign * quantity * Math.Abs(num), offsetOverheadAccountID));
		}
		else if (num < 0m)
		{
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)debitSign * quantity * Math.Abs(num), offsetOverheadAccountID));
			journal.JournalLines.Add(addJournalLine(partTransactionRow, ++lineID, (decimal)creditSign * quantity * Math.Abs(num), destOverheadAccountID));
		}
	}

	private DataRow getPartTransactionCostsRecord(M1Database database, SqlTransaction transaction, int partTransactionID, CostingMethod costMethod)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select * from PartTransactionCosts Where intPartTransactionID = @ID and intCostType = @CostType");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int)).Value = partTransactionID;
		sqlCommand.Parameters.Add(new SqlParameter("@CostType", SqlDbType.SmallInt)).Value = costMethod;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0];
		}
		return null;
	}

	private decimal setQuantity(decimal quantity)
	{
		quantity = ((!backoutQty) ? Convert.ToDecimal(quantity) : (-Convert.ToDecimal(quantity)));
		if (ReverseSign)
		{
			quantity *= -1m;
		}
		return quantity;
	}

	private JournalLine addJournalLine(DataRow partTransactionRow, int lineID, decimal cost, string glAccountID)
	{
		JournalLine journalLine = new JournalLine
		{
			LineID = lineID,
			JournalLineTransactionType = journalLineTransactionType
		};
		setGLLineAmount(journalLine, Math.Round(cost, 2));
		journalLine.GLAccountID = glAccountID;
		journalLine.Reference = string.Empty;
		journalLine.Description = string.Empty;
		journalLine.TransactionDate = partTransactionRow.Field<DateTime>("imtTransactionDate");
		journalLine.PartTransactionID = partTransactionRow.Field<int>("imtPartTransactionID");
		journalLine.OrganizationID = string.Empty;
		journalLine.LocationID = string.Empty;
		return journalLine;
	}

	private JournalLine addRoundingJournalLine(int lineID, decimal cost, string glAccountID)
	{
		JournalLine journalLine = new JournalLine
		{
			LineID = lineID,
			JournalLineTransactionType = journalLineTransactionType
		};
		setGLLineAmount(journalLine, Math.Round(cost, 2));
		journalLine.GLAccountID = glAccountID;
		journalLine.Reference = string.Empty;
		journalLine.Description = string.Empty;
		journalLine.TransactionDate = DateTime.Now;
		journalLine.PartTransactionID = 0;
		journalLine.OrganizationID = string.Empty;
		journalLine.LocationID = string.Empty;
		return journalLine;
	}

	private void setGLLineAmount(JournalLine line, decimal cost)
	{
		line.TransactionAmount = cost;
		if (cost > 0m)
		{
			line.DebitAmount = Math.Abs(cost);
			line.CreditAmount = default(decimal);
			line.JournalType = JournalType.Debit;
		}
		else
		{
			line.DebitAmount = default(decimal);
			line.CreditAmount = Math.Abs(cost);
			line.JournalType = JournalType.Credit;
		}
	}

	private void getShipmentGuids(M1Database database, DataRow sourceRow, DataRowVersion rowVersion, SqlTransaction transaction, ref SqlCommand command, ref decimal qtyRatio)
	{
		int num = 1;
		if (!sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "ShipmentID") || !sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "ShipmentLineID"))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("Select smlUniqueID,smlKitPart,smlQuantityShipped From ShipmentLines Where smlShipmentID = @ID And smlShipmentLineID = @LineID");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = sourceRow.Field<string>(base.Field.Table.FieldPrefix + "ShipmentID", rowVersion);
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = sourceRow.Field<short>(base.Field.Table.FieldPrefix + "ShipmentLineID", rowVersion);
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			if (dataTable.Rows[0].Field<decimal>("smlQuantityShipped") != 0m)
			{
				qtyRatio = manualQtyPassed / dataTable.Rows[0].Field<decimal>("smlQuantityShipped");
			}
			if (!dataTable.Rows[0].Field<bool>("smlKitPart"))
			{
				command = database.NewSqlCommand("Select " + partTransFieldNames + " from PartTransactions Where imtTableUniqueID = @UniqueID");
				command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = dataTable.Rows[0].Field<Guid>("smlUniqueID");
			}
			else
			{
				SqlCommand sqlCommand2 = database.NewSqlCommand("Select smoUniqueID From ShipmentComponents Where smoShipmentID = @ID And smoShipmentLineID = @LineID");
				sqlCommand2.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = sourceRow.Field<string>(base.Field.Table.FieldPrefix + "ShipmentID", rowVersion);
				sqlCommand2.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = sourceRow.Field<short>(base.Field.Table.FieldPrefix + "ShipmentLineID", rowVersion);
				DataTable dataTable2 = database.GetDataTable(sqlCommand2, transaction);
				List<string> list = new List<string>();
				foreach (DataRow row in dataTable2.Rows)
				{
					string text = list.Count.ToString("@Guid" + num, CultureInfo.InvariantCulture);
					list.Add(text);
					command.Parameters.Add(text, SqlDbType.UniqueIdentifier).Value = row.Field<Guid>("smoUniqueID");
					num++;
				}
				string newValue = string.Join(",", list.ToArray());
				command.CommandText = ("Select " + partTransFieldNames + " from PartTransactions Where imtTableUniqueID In ({@Values})").Replace("{@Values}", newValue);
				sqlCommand2 = null;
			}
		}
		dataTable = null;
		sqlCommand = null;
	}

	private void GetRmaReceiptGuids(M1Database database, DataRow sourceRow, DataRowVersion rowVersion, SqlTransaction transaction, ref SqlCommand command, ref decimal qtyRatio)
	{
		int num = 1;
		if (!sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "RMAReceiptID") || !sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "RMAReceiptLineID"))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("Select rrlUniqueID,rrlKitPart,rrlInventoryQuantityReceived,rrlQuantityToInspect From RMAReceiptLines Where rrlRmaReceiptID = @ID And rrlRmaReceiptLineID = @LineID");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = sourceRow.Field<string>(base.Field.Table.FieldPrefix + "RMAReceiptID", rowVersion);
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = sourceRow.Field<short>(base.Field.Table.FieldPrefix + "RMAReceiptLineID", rowVersion);
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		if (dataTable.Rows[0].Field<decimal>("rrlInventoryQuantityReceived") != 0m || dataTable.Rows[0].Field<decimal>("rrlQuantityToInspect") != 0m)
		{
			qtyRatio = manualQtyPassed / (dataTable.Rows[0].Field<decimal>("rrlInventoryQuantityReceived") + dataTable.Rows[0].Field<decimal>("rrlQuantityToInspect"));
		}
		if (!dataTable.Rows[0].Field<bool>("rrlKitPart"))
		{
			command = database.NewSqlCommand("Select " + partTransFieldNames + " from PartTransactions Where imtTableUniqueID = @UniqueID Union All Select " + partTransFieldNames + " from (Select " + partTransFieldNames + ", row_number() over (partition by qalSourceTableUniqueID order by imtPartTransactionId) as rownum    from PartTransactions Inner Join InspectionLines on imtTableUniqueID = qalUniqueID Where qalSourceTableUniqueID = @UniqueID) x Where x.rownum = 1");
			command.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = dataTable.Rows[0].Field<Guid>("rrlUniqueID");
			return;
		}
		SqlCommand sqlCommand2 = database.NewSqlCommand("Select rroUniqueID From RmaReceiptComponents Where rroRmaReceiptID = @ID And rroRmaReceiptLineID = @LineID");
		sqlCommand2.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = sourceRow.Field<string>(base.Field.Table.FieldPrefix + "RmaReceiptID", rowVersion);
		sqlCommand2.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = sourceRow.Field<short>(base.Field.Table.FieldPrefix + "RmaReceiptLineID", rowVersion);
		List<string> list;
		using (DataTable dataTable2 = database.GetDataTable(sqlCommand2, transaction))
		{
			list = new List<string>();
			foreach (DataRow row in dataTable2.Rows)
			{
				string text = list.Count.ToString("@Guid" + num, CultureInfo.InvariantCulture);
				list.Add(text);
				command.Parameters.Add(text, SqlDbType.UniqueIdentifier).Value = row.Field<Guid>("rroUniqueID");
				num++;
			}
		}
		string newValue = string.Join(",", list.ToArray());
		command.CommandText = ("Select " + partTransFieldNames + " from PartTransactions Where imtTableUniqueID In ({@Values}) Union All Select " + partTransFieldNames + " from (Select " + partTransFieldNames + ", row_number() over (partition by qamUniqueID order by imtPartTransactionId) as rownum    from PartTransactions Inner Join InspectionComponents on imtTableUniqueID = qamUniqueID Where qamSourceTableUniqueID In ({@Values})) x Where x.rownum = 1").Replace("{@Values}", newValue);
	}

	private void setJobInfo(DataRow sourceRow, DataRowVersion rowVersion)
	{
		if (jobField == null)
		{
			return;
		}
		jobID = string.Empty;
		jobAssemblyID = 0;
		jobMaterialID = 0;
		jobOperationID = 0;
		jobMaterialComponentID = 0;
		jobID = (string)sourceRow[jobField.RelatedFieldsAndCurrentFieldArray[0], rowVersion];
		if (jobField.RelatedFieldsAndCurrentFieldArray.Length <= 1)
		{
			return;
		}
		jobAssemblyID = (int)sourceRow[jobField.RelatedFieldsAndCurrentFieldArray[1], rowVersion];
		if (jobField.RelatedTable.Equals("JobMaterials", StringComparison.CurrentCultureIgnoreCase))
		{
			jobMaterialID = (int)sourceRow[jobField.FieldName, rowVersion];
		}
		else if (jobField.RelatedTable.Equals("JobOperations", StringComparison.CurrentCultureIgnoreCase))
		{
			jobOperationID = (int)sourceRow[jobField.FieldName, rowVersion];
			if (jobOperationID == 0 && sourceRow.Table.Columns.Contains(base.Field.Table.FieldPrefix + "JobMaterialID") && sourceRow.Field<int>(base.Field.Table.FieldPrefix + "JobMaterialID", rowVersion) != 0)
			{
				jobMaterialID = sourceRow.Field<int>(base.Field.Table.FieldPrefix + "JobMaterialID", rowVersion);
			}
		}
		else if (jobField.RelatedFieldsAndCurrentFieldArray.Length > 2 && jobField.RelatedTable.Equals("JobMaterialComponents", StringComparison.CurrentCultureIgnoreCase))
		{
			jobMaterialID = (int)sourceRow[jobField.RelatedFieldsAndCurrentFieldArray[2], rowVersion];
			jobMaterialComponentID = (int)sourceRow[jobField.FieldName, rowVersion];
		}
	}

	public DetailSource getDetailSource()
	{
		switch (journalLineTransactionType)
		{
		case JournalLineTransactionType.ShipFromInventory:
		case JournalLineTransactionType.ShipFromJob:
		case JournalLineTransactionType.ARInvoiceShipment:
			return DetailSource.Shipments;
		case JournalLineTransactionType.MaterialIssueToJobGoodQty:
		case JournalLineTransactionType.MaterialIssueToJobScrapQty:
		case JournalLineTransactionType.MiscIssueFromInventoryGoodQty:
		case JournalLineTransactionType.MiscIssueFromInventoryScrapQty:
		case JournalLineTransactionType.MaterialIssueToJobReturnGoodQty:
		case JournalLineTransactionType.MaterialIssueToJobReturnScrapQty:
			return DetailSource.MaterialIssue;
		case JournalLineTransactionType.QuantityAdjustment:
		case JournalLineTransactionType.BinTransfer:
		case JournalLineTransactionType.BinReceipt:
		case JournalLineTransactionType.InventoryCount:
		case JournalLineTransactionType.PartCostsAdjustment:
		case JournalLineTransactionType.StandardCostRollup:
			return DetailSource.InventoryAdj;
		case JournalLineTransactionType.PurchaseOrderReceiptInvQty:
		case JournalLineTransactionType.PurchaseOrderReceiptJobMatQty:
		case JournalLineTransactionType.PurchaseOrderReceiptJobOprQty:
		case JournalLineTransactionType.PurchaseOrderReceiptQtyToInspect:
		case JournalLineTransactionType.MiscReceiptToInventory:
		case JournalLineTransactionType.MiscReceiptToJobMatQty:
		case JournalLineTransactionType.MiscReceiptToJobOprQty:
		case JournalLineTransactionType.MiscReceiptToJobAsmQty:
			return DetailSource.Receipts;
		case JournalLineTransactionType.DMRShipmentReturnQty:
		case JournalLineTransactionType.DMRShipmentInvQty:
		case JournalLineTransactionType.DMRShipmentJobMatQty:
		case JournalLineTransactionType.DMRShipmentJobOprQty:
			return DetailSource.DMRShipments;
		case JournalLineTransactionType.MfgReceiptInvQty:
		case JournalLineTransactionType.MfgReceiptScrapQty:
		case JournalLineTransactionType.MfgReceiptQtyToInspect:
			return DetailSource.MfgReceipts;
		case JournalLineTransactionType.RMAReceiptInvQty:
		case JournalLineTransactionType.RMAReceiptQtyToInspect:
		case JournalLineTransactionType.ARInvoiceRMAReceipt:
			return DetailSource.RMAReceipts;
		case JournalLineTransactionType.WarehouseTransfer:
			return DetailSource.WHTransfers;
		case JournalLineTransactionType.WarehouseReceipt:
			return DetailSource.WHReceipts;
		case JournalLineTransactionType.PartClassChange:
			return DetailSource.PartClass;
		case JournalLineTransactionType.MfgVarianceTransfer:
			return DetailSource.MfgVariance;
		case JournalLineTransactionType.InspectionInvAcceptedQty:
		case JournalLineTransactionType.InspectionInvToReturnQty:
		case JournalLineTransactionType.InspectionInvToScrapQty:
		case JournalLineTransactionType.InspectionJobMatAcceptedQty:
		case JournalLineTransactionType.InspectionJobOprAcceptedQty:
		case JournalLineTransactionType.InspectionJobToReturnQty:
		case JournalLineTransactionType.InspectionJobToScrapQty:
			return DetailSource.Inspections;
		case JournalLineTransactionType.LandedCostPOInTransit:
		case JournalLineTransactionType.LandedCostCharges:
		case JournalLineTransactionType.LandedCost:
			return DetailSource.LandedCosts;
		default:
			return (DetailSource)0;
		}
	}

	public byte getHeaderSource()
	{
		byte result = 0;
		switch (getDetailSource())
		{
		case DetailSource.ARInvoice:
		case DetailSource.ARPayment:
			result = 1;
			break;
		case DetailSource.APInvoice:
		case DetailSource.APPayment:
			result = 2;
			break;
		case DetailSource.Payroll:
			result = 3;
			break;
		case DetailSource.BankRec:
		case DetailSource.RecurringJournals:
		case DetailSource.GeneralJournal:
			result = 4;
			break;
		case DetailSource.Shipments:
		case DetailSource.Receipts:
		case DetailSource.MfgReceipts:
		case DetailSource.MaterialIssue:
		case DetailSource.Timecards:
		case DetailSource.InventoryAdj:
		case DetailSource.MfgVariance:
		case DetailSource.LandedCosts:
		case DetailSource.DMRShipments:
		case DetailSource.RMAReceipts:
		case DetailSource.WHTransfers:
		case DetailSource.WHReceipts:
		case DetailSource.PartClass:
		case DetailSource.Inspections:
			result = 5;
			break;
		case DetailSource.FixedAssets:
			result = 6;
			break;
		default:
			result = 0;
			break;
		case DetailSource.MulticurrencyExchange:
			break;
		}
		return result;
	}

	public bool checkParms(DataRow sourceRow, DataRowVersion rowVersion, string sourceTableName, string fieldPrefix, string parms)
	{
		if (parms.Contains("MANUALJOURNALCREATION"))
		{
			return false;
		}
		if (!parms.Contains("IGNOREPOSTED"))
		{
			if (sourceRow.Table.Columns.Contains(fieldPrefix + "Posted") && sourceRow.Field<bool>(fieldPrefix + "Posted", rowVersion).Equals(obj: false))
			{
				return true;
			}
			if (sourceRow.Table.Columns.Contains(fieldPrefix + "PostedToGL") && sourceRow.Field<bool>(fieldPrefix + "PostedToGL", rowVersion).Equals(obj: false))
			{
				return true;
			}
		}
		else
		{
			if (sourceRow.Table.Columns.Contains(fieldPrefix + "Posted") && sourceRow.Field<bool>(fieldPrefix + "Posted", rowVersion).Equals(obj: true))
			{
				return true;
			}
			if (sourceRow.Table.Columns.Contains(fieldPrefix + "PostedToGL") && sourceRow.Field<bool>(fieldPrefix + "PostedToGL", rowVersion).Equals(obj: true))
			{
				return true;
			}
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "KitPart") && parms.Contains("CHECKFORKITPART") && sourceRow.Field<bool>(fieldPrefix + "KitPart", rowVersion).Equals(obj: true))
		{
			return true;
		}
		if (parms.Contains("CHECKFORINSP") && sourceRow.Table.Columns.Contains(fieldPrefix + "InspectionID") && !string.IsNullOrWhiteSpace(sourceRow.Field<string>(fieldPrefix + "InspectionID", rowVersion)))
		{
			return true;
		}
		if (parms.Contains("CHECKFORJOB") && sourceRow.Table.Columns.Contains(fieldPrefix + "JobID") && !string.IsNullOrWhiteSpace(sourceRow.Field<string>(fieldPrefix + "JobID", rowVersion)))
		{
			return true;
		}
		if (parms.Contains("CHECKFORSOURCE") && sourceTableName.Equals("INSPECTIONLINES", StringComparison.CurrentCultureIgnoreCase) && sourceRow.Table.Columns.Contains(fieldPrefix + "SourceTableName") && sourceRow.Table.Columns.Contains(fieldPrefix + "InspectionType"))
		{
			if (!string.IsNullOrWhiteSpace(sourceRow.Field<string>(fieldPrefix + "SourceTableName", rowVersion)))
			{
				return true;
			}
			if (!sourceRow.Field<byte>(fieldPrefix + "InspectionType", rowVersion).Equals(1))
			{
				return true;
			}
		}
		if (parms.Contains("CHECKFORSFESOURCE") && sourceTableName.Equals("INSPECTIONLINES", StringComparison.CurrentCultureIgnoreCase) && sourceRow.Table.Columns.Contains(fieldPrefix + "SourceTableName") && sourceRow.Table.Columns.Contains(fieldPrefix + "InspectionType") && sourceRow.Field<string>(fieldPrefix + "SourceTableName", rowVersion).Equals("SFE") && sourceRow.Field<byte>(fieldPrefix + "InspectionType", rowVersion).Equals(2))
		{
			return true;
		}
		if (parms.Contains("CHECKFORSOURCE") && sourceTableName.Equals("INSPECTIONCOMPONENTS", StringComparison.CurrentCultureIgnoreCase) && sourceRow.Table.Columns.Contains(fieldPrefix + "SourceTableName") && sourceRow.Table.Columns.Contains(fieldPrefix + "InspectionType"))
		{
			if (!string.IsNullOrWhiteSpace(sourceRow.Field<string>(fieldPrefix + "SourceTableName", rowVersion)))
			{
				return true;
			}
			if (!sourceRow.Field<byte>(fieldPrefix + "InspectionType", rowVersion).Equals(1))
			{
				return true;
			}
		}
		if (parms.Contains("CHECKFORSOURCE") && sourceTableName.Equals("SHIPMENTLINES", StringComparison.CurrentCultureIgnoreCase) && sourceRow.Table.Columns.Contains(fieldPrefix + "SourceTableName") && string.IsNullOrWhiteSpace(sourceRow.Field<string>(fieldPrefix + "SourceTableName", rowVersion)))
		{
			return true;
		}
		if (parms.Contains("CHECKFORSOURCE") && (sourceTableName.Equals("WAREHOUSERECEIPTLINES", StringComparison.CurrentCultureIgnoreCase) || sourceTableName.Equals("WAREHOUSERECEIPTCOMPONENTS", StringComparison.CurrentCultureIgnoreCase)) && sourceRow.Table.Columns.Contains(fieldPrefix + "SourceTableName") && string.IsNullOrWhiteSpace(sourceRow.Field<string>(fieldPrefix + "SourceTableName", rowVersion)))
		{
			return true;
		}
		if (parms.Contains("CHECKCOMPONENT") && sourceTableName.Trim().ToUpper().Contains("COMPONENTS"))
		{
			return true;
		}
		return false;
	}

	public override void LoadComplete(FieldCollection fields, bool allowEditing)
	{
		if (PartBinField.Length != 0 && TransactionType != 0 && Source != 0 && base.Field.DataDictionary != null && allowEditing && base.Field.Database.Props("FinancialProperties").Field<bool>("xafGLCreateStockJournals"))
		{
			base.LoadComplete(fields, add: true);
			if (jobField == null && RelatedJobField.Length != 0)
			{
				jobField = base.Field.BindingSource.Fields[RelatedJobField];
			}
			binField = base.Field.BindingSource.Fields[PartBinField];
			journalLineTransactionType = (JournalLineTransactionType)TransactionType;
			base.Field.BindingSource.RowUpdateAddAfter += BindingSource_RowUpdateAddAfter;
			base.Field.BindingSource.RowUpdateSaveAfter += BindingSource_RowUpdateSaveAfter;
			base.Field.BindingSource.RowUpdateDeleteBefore += BindingSource_RowUpdateDeleteBefore;
		}
		else
		{
			base.LoadComplete(fields, add: false);
			base.Field.BindingSource.RowUpdateAddAfter -= BindingSource_RowUpdateAddAfter;
			base.Field.BindingSource.RowUpdateSaveAfter -= BindingSource_RowUpdateSaveAfter;
			base.Field.BindingSource.RowUpdateDeleteBefore -= BindingSource_RowUpdateDeleteBefore;
		}
	}

	protected virtual void AddCurrentValues(RowUpdateEventArgs e)
	{
		string value = e.Row.Field<string>(binField.RelatedFieldsAndCurrentFieldArray[0]);
		decimal num = Convert.ToDecimal(e.Row[base.Field.FieldName]);
		if (!string.IsNullOrWhiteSpace(value) && num != 0m)
		{
			backoutQty = false;
			AddJournal(e.Database, e.Row, DataRowVersion.Current, e.SqlTransaction);
		}
	}

	protected virtual void RemoveOriginalValues(RowUpdateEventArgs e)
	{
		string value = e.Row.Field<string>(binField.RelatedFieldsAndCurrentFieldArray[0], DataRowVersion.Original);
		decimal num = Convert.ToDecimal(e.Row[base.Field.FieldName, DataRowVersion.Original]);
		if (!string.IsNullOrWhiteSpace(value) && num != 0m)
		{
			if (!base.Field.FieldName.Equals("qamComponentQtyToInspect", StringComparison.CurrentCultureIgnoreCase) && !base.Field.FieldName.Equals("qalQuantityToInspect", StringComparison.CurrentCultureIgnoreCase))
			{
				backoutQty = true;
				AddJournal(e.Database, e.Row, DataRowVersion.Original, e.SqlTransaction);
			}
			else if (num != Convert.ToDecimal(e.Row[base.Field.FieldName, DataRowVersion.Current]))
			{
				backoutQty = true;
				AddJournal(e.Database, e.Row, DataRowVersion.Original, e.SqlTransaction);
			}
		}
	}

	private void BindingSource_RowUpdateDeleteBefore(object sender, RowUpdateEventArgs e)
	{
		RemoveOriginalValues(e);
	}

	private void BindingSource_RowUpdateSaveAfter(object sender, RowUpdateEventArgs e)
	{
		SaveCheck(e);
	}

	protected virtual void SaveCheck(RowUpdateEventArgs e)
	{
		if (isRowChanged(e.Row))
		{
			AddCurrentValues(e);
		}
	}

	private void BindingSource_RowUpdateAddAfter(object sender, RowUpdateEventArgs e)
	{
		AddCurrentValues(e);
	}

	protected virtual bool isRowChanged(DataRow row)
	{
		string[] relatedFieldsAndCurrentFieldArray = binField.RelatedFieldsAndCurrentFieldArray;
		foreach (string columnName in relatedFieldsAndCurrentFieldArray)
		{
			if (!row.Field<string>(columnName).Trim().Equals(row.Field<string>(columnName, DataRowVersion.Original).Trim(), StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
		}
		if (!row.Field<decimal>(FieldName).Equals(row.Field<decimal>(FieldName, DataRowVersion.Original)))
		{
			return true;
		}
		if (jobField != null)
		{
			relatedFieldsAndCurrentFieldArray = jobField.RelatedFieldsAndCurrentFieldArray;
			foreach (string columnName2 in relatedFieldsAndCurrentFieldArray)
			{
				if (!row[columnName2].ToString().Equals(row[columnName2, DataRowVersion.Original].ToString(), StringComparison.CurrentCultureIgnoreCase))
				{
					return true;
				}
			}
		}
		if (!string.IsNullOrWhiteSpace(base.Field.Table.DocumentPlantIdField) && !row.Field<string>(base.Field.Table.DocumentPlantIdField).Equals(row.Field<string>(base.Field.Table.DocumentPlantIdField, DataRowVersion.Original)))
		{
			return true;
		}
		if (row.Table.Columns.Contains(base.Field.Table.FieldPrefix + "Posted") && !row.Field<bool>(base.Field.Table.FieldPrefix + "Posted").Equals(row.Field<bool>(base.Field.Table.FieldPrefix + "Posted", DataRowVersion.Original)) && !parms.Contains("IGNOREPOSTED"))
		{
			return true;
		}
		if (row.Table.Columns.Contains(base.Field.Table.FieldPrefix + "PostedToGL") && !row.Field<bool>(base.Field.Table.FieldPrefix + "PostedToGL").Equals(row.Field<bool>(base.Field.Table.FieldPrefix + "PostedToGL", DataRowVersion.Original)) && !parms.Contains("IGNOREPOSTED"))
		{
			return true;
		}
		return false;
	}

	private bool getNonStockedStatus(M1Database database, SqlTransaction transaction, string partID)
	{
		partID = partID.Trim();
		if (partID.Length != 0)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select impNonStockedItem From Parts Where impPartID = @PartID");
			sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				return dataTable.Rows[0].Field<bool>("impNonStockedItem");
			}
		}
		return false;
	}

	public override void Dispose()
	{
		if (base.Field?.BindingSource != null)
		{
			LoadComplete(base.Field.BindingSource.Fields, false);
		}
		if (jobField != null)
		{
			jobField.Dispose();
			jobField = null;
		}
		if (binField != null)
		{
			binField.Dispose();
			binField = null;
		}
		base.Dispose();
	}
}
