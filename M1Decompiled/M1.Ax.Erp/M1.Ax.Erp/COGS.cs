using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class COGS
{
	public class JobInfo
	{
		public string JobID;

		public int JobAssemblyID;

		public int JobMaterialID;

		public int JobOperationID;

		public int JobMaterialComponentID;
	}

	public COGSAccounts GetCOGSAccounts(M1Database database, SqlTransaction transaction, string partID, string plantID, string partGroupID, string reasonID)
	{
		COGSAccounts cOGSAccounts = new COGSAccounts();
		cOGSAccounts.PartID = partID;
		cOGSAccounts.PartGroupID = partGroupID;
		cOGSAccounts.PlantID = plantID;
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		string text5 = string.Empty;
		string text6 = string.Empty;
		string text7 = string.Empty;
		string text8 = string.Empty;
		string text9 = string.Empty;
		string text10 = string.Empty;
		string text11 = string.Empty;
		string text12 = string.Empty;
		SqlCommand sqlCommand = database.NewSqlCommand("Select impPartGroupID,impPartClassID,imcInventoryGLAccountID,imfInventoryGLAccountID,imcInvToReturnGLAccountID,imfInvToReturnGLAccountID,imcInvInInspectionGLAccountID,imfInvInInspectionGLAccountID,imfInvInTransferGLAccountID,imcInvInTransferGLAccountID From Parts Left Outer Join PartClasses On impPartClassID = imcPartClassID Left Outer Join PartClassPlants On imfPartClassID = imcPartClassID And imfPartClassPlantID = @PlantID Where impPartID = @PartID");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = plantID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			DataRow dataRow = dataTable.Rows[0];
			cOGSAccounts.PartClassID = dataRow.Field<string>("impPartClassID");
			if (string.IsNullOrWhiteSpace(partGroupID))
			{
				cOGSAccounts.PartGroupID = dataRow.Field<string>("impPartGroupID");
			}
			if (dataRow["imcInventoryGLAccountID"] != DBNull.Value)
			{
				text = dataRow.Field<string>("imcInventoryGLAccountID");
			}
			if (dataRow["imfInventoryGLAccountID"] != DBNull.Value)
			{
				text2 = dataRow.Field<string>("imfInventoryGLAccountID");
			}
			if (dataRow["imcInvToReturnGLAccountID"] != DBNull.Value)
			{
				text5 = dataRow.Field<string>("imcInvToReturnGLAccountID");
			}
			if (dataRow["imfInvToReturnGLAccountID"] != DBNull.Value)
			{
				text6 = dataRow.Field<string>("imfInvToReturnGLAccountID");
			}
			if (dataRow["imcInvInInspectionGLAccountID"] != DBNull.Value)
			{
				text7 = dataRow.Field<string>("imcInvInInspectionGLAccountID");
			}
			if (dataRow["imfInvInInspectionGLAccountID"] != DBNull.Value)
			{
				text8 = dataRow.Field<string>("imfInvInInspectionGLAccountID");
			}
			if (dataRow["imcInvInTransferGLAccountID"] != DBNull.Value)
			{
				text9 = dataRow.Field<string>("imcInvInTransferGLAccountID");
			}
			if (dataRow["imfInvInTransferGLAccountID"] != DBNull.Value)
			{
				text10 = dataRow.Field<string>("imfInvInTransferGLAccountID");
			}
		}
		if (!string.IsNullOrWhiteSpace(reasonID))
		{
			sqlCommand = database.NewSqlCommand("select xarReasonGLAccountID, xajReasonGLAccountID, xarScrapGLAccountID, xajScrapGLAccountID from Reasons left outer join ReasonPlants on xarReasonID = xajReasonID and xajReasonPlantID = @PlantID where xarReasonID = @ReasonID ");
			sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = plantID;
			sqlCommand.Parameters.Add(new SqlParameter("@ReasonID", SqlDbType.NVarChar)).Value = reasonID;
			dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow dataRow2 = dataTable.Rows[0];
				if (dataRow2["xarReasonGLAccountID"] != DBNull.Value)
				{
					text3 = dataRow2.Field<string>("xarReasonGLAccountID");
				}
				if (dataRow2["xajReasonGLAccountID"] != DBNull.Value)
				{
					text4 = dataRow2.Field<string>("xajReasonGLAccountID");
				}
				if (dataRow2["xarScrapGLAccountID"] != DBNull.Value)
				{
					text11 = dataRow2.Field<string>("xarScrapGLAccountID");
				}
				if (dataRow2["xajScrapGLAccountID"] != DBNull.Value)
				{
					text12 = dataRow2.Field<string>("xajScrapGLAccountID");
				}
			}
		}
		sqlCommand = database.NewSqlCommand("Select imvCOGSLaborGLAccountID, imvCOGSMaterialGLAccountID, imvCOGSSubcontractGLAccountID, imvCOGSOverheadGLAccountID, '' AS imvSVarLaborGLAccountID, '' AS imvSVarMaterialGLAccountID, '' AS imvSVarSubcontractGLAccountID, '' AS imvSVarOverheadGLAccountID, '' AS imvPurchaseVarianceGLAccountID, '' AS imvWIPLaborGLAccountID, '' AS imvWIPMaterialGLAccountID, '' AS imvWIPSubcontractGLAccountID, '' AS imvWIPOverheadGLAccountID, '' AS imvAccruedCreditorsGLAccountID, '' AS imvLaborClearingGLAccountID, '' AS imvOverheadClearingGLAccountID, '' AS imvStockRevaluationGLAccountID, '' As xafShipAwaitInvoiceGLAccountID, '' As xafStockInTransitGLAccountID, 'PARTGROUPPLANTS' As TableSource From PartGroupPlants Where imvPartGroupID = @PartGroupID And imvPartGroupPlantID = @PlantID Union All (Select imuCOGSLaborGLAccountID, imuCOGSMaterialGLAccountID, imuCOGSSubcontractGLAccountID, imuCOGSOverheadGLAccountID, '' AS imvSVarLaborGLAccountID, '' AS imvSVarMaterialGLAccountID, '' AS imvSVarSubcontractGLAccountID, '' AS imvSVarOverheadGLAccountID, '' AS imvPurchaseVarianceGLAccountID, '' AS imvWIPLaborGLAccountID, '' AS imvWIPMaterialGLAccountID, '' AS imvWIPSubcontractGLAccountID, '' AS imvWIPOverheadGLAccountID, '' AS imvAccruedCreditorsGLAccountID, '' AS imvLaborClearingGLAccountID, '' AS imvOverheadClearingGLAccountID, '' AS imvStockRevaluationGLAccountID, '' As xafShipAwaitInvoiceGLAccountID, '' As xafStockInTransitGLAccountID, 'PARTGROUPS' As TableSource From PartGroups Where imuPartGroupID = @PartGroupID) Union All (Select '' AS imvCOGSLaborGLAccountID, '' AS imvCOGSMaterialGLAccountID, '' AS imvCOGSSubcontractGLAccountID, '' AS imvCOGSOverheadGLAccountID, xauSVarLaborGLAccountID, xauSVarMaterialGLAccountID, xauSVarSubcontractGLAccountID, xauSVarOverheadGLAccountID, xauPurchaseVarianceGLAccountID, xauWIPLaborGLAccountID, xauWIPMaterialGLAccountID, xauWIPSubcontractGLAccountID, xauWIPOverheadGLAccountID, xauAccruedCreditorsGLAccountID, xauLaborClearingGLAccountID, xauOverheadClearingGLAccountID, xauStockRevaluationGLAccountID, xauShipAwaitInvoiceGLAccountID As xafShipAwaitInvoiceGLAccountID, xauStockInTransitGLAccountID As xafStockInTransitGLAccountID, 'PLANTS' As TableSource From Plants Where xauPlantID = @PlantID) Union All (Select '' AS imvCOGSLaborGLAccountID, '' AS imvCOGSMaterialGLAccountID, '' AS imvCOGSSubcontractGLAccountID, '' AS imvCOGSOverheadGLAccountID, xafSVarLaborGLAccountID, xafSVarMaterialGLAccountID, xafSVarSubcontractGLAccountID, xafSVarOverheadGLAccountID, xafPurchaseVarianceGLAccountID, xafWIPLaborGLAccountID, xafWIPMaterialGLAccountID, xafWIPSubcontractGLAccountID, xafWIPOverheadGLAccountID, xafAccruedCreditorsGLAccountID, xafLaborClearingGLAccountID, xafOverheadClearingGLAccountID, xafStockRevaluationGLAccountID, xafShipAwaitInvoiceGLAccountID, xafStockInTransitGLAccountID, 'FINANCIALPROPERTIES' As TableSource From FinancialProperties)");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = cOGSAccounts.PartID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartGroupID", SqlDbType.NVarChar)).Value = cOGSAccounts.PartGroupID;
		sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = cOGSAccounts.PlantID;
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			switch (database.Props("FN").Field<byte>("xafCOGSUseAccounts"))
			{
			case 1:
				CopyAccountData(dataTable, "FINANCIALPROPERTIES", cOGSAccounts);
				CopyAccountData(dataTable, "PARTGROUPS", cOGSAccounts);
				if (!string.IsNullOrWhiteSpace(text))
				{
					cOGSAccounts.InventoryGLAccountID = text;
				}
				if (!string.IsNullOrWhiteSpace(text3))
				{
					cOGSAccounts.ReasonGLAccountID = text3;
				}
				if (!string.IsNullOrWhiteSpace(text11))
				{
					cOGSAccounts.ScrapGLAccountID = text11;
				}
				if (!string.IsNullOrWhiteSpace(text5))
				{
					cOGSAccounts.InventoryToReturnGLAccountID = text5;
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					cOGSAccounts.InventoryInInspectionGLAccountID = text7;
				}
				if (!string.IsNullOrWhiteSpace(text9))
				{
					cOGSAccounts.InventoryInTransferGLAccountID = text9;
				}
				break;
			case 2:
				CopyAccountData(dataTable, "FINANCIALPROPERTIES", cOGSAccounts);
				if (!string.IsNullOrWhiteSpace(text))
				{
					cOGSAccounts.InventoryGLAccountID = text;
				}
				if (!string.IsNullOrWhiteSpace(text3))
				{
					cOGSAccounts.ReasonGLAccountID = text3;
				}
				if (!string.IsNullOrWhiteSpace(text11))
				{
					cOGSAccounts.ScrapGLAccountID = text11;
				}
				if (!string.IsNullOrWhiteSpace(text5))
				{
					cOGSAccounts.InventoryToReturnGLAccountID = text5;
				}
				if (!string.IsNullOrWhiteSpace(text7))
				{
					cOGSAccounts.InventoryInInspectionGLAccountID = text7;
				}
				if (!string.IsNullOrWhiteSpace(text9))
				{
					cOGSAccounts.InventoryInTransferGLAccountID = text9;
				}
				CopyAccountData(dataTable, "PARTGROUPPLANTS", cOGSAccounts);
				if (!string.IsNullOrWhiteSpace(text2))
				{
					cOGSAccounts.InventoryGLAccountID = text2;
				}
				if (!string.IsNullOrWhiteSpace(text4))
				{
					cOGSAccounts.ReasonGLAccountID = text4;
				}
				if (!string.IsNullOrWhiteSpace(text12))
				{
					cOGSAccounts.ScrapGLAccountID = text12;
				}
				if (!string.IsNullOrWhiteSpace(text6))
				{
					cOGSAccounts.InventoryToReturnGLAccountID = text6;
				}
				if (!string.IsNullOrWhiteSpace(text8))
				{
					cOGSAccounts.InventoryInInspectionGLAccountID = text8;
				}
				if (!string.IsNullOrWhiteSpace(text10))
				{
					cOGSAccounts.InventoryInTransferGLAccountID = text10;
				}
				break;
			case 3:
			{
				CopyAccountData(dataTable, "PLANTS", cOGSAccounts);
				CopyAccountData(dataTable, "PARTGROUPPLANTS", cOGSAccounts);
				if (!string.IsNullOrWhiteSpace(text2))
				{
					cOGSAccounts.InventoryGLAccountID = text2;
				}
				if (!string.IsNullOrWhiteSpace(text4))
				{
					cOGSAccounts.ReasonGLAccountID = text4;
				}
				if (!string.IsNullOrWhiteSpace(text12))
				{
					cOGSAccounts.ScrapGLAccountID = text12;
				}
				if (!string.IsNullOrWhiteSpace(text6))
				{
					cOGSAccounts.InventoryToReturnGLAccountID = text6;
				}
				if (!string.IsNullOrWhiteSpace(text8))
				{
					cOGSAccounts.InventoryInInspectionGLAccountID = text8;
				}
				if (!string.IsNullOrWhiteSpace(text10))
				{
					cOGSAccounts.InventoryInTransferGLAccountID = text10;
				}
				if (!string.IsNullOrWhiteSpace(cOGSAccounts.StockRevaluationGLAccountID))
				{
					break;
				}
				string s = "FINANCIALPROPERTIES";
				DataRow[] array = dataTable.Select("TableSource = " + s.ToLinq());
				if (array != null && array.Length != 0)
				{
					DataRow row = array[0];
					if (!string.IsNullOrWhiteSpace(row.Field<string>("imvStockRevaluationGLAccountID")))
					{
						cOGSAccounts.StockRevaluationGLAccountID = row.Field<string>("imvStockRevaluationGLAccountID");
					}
				}
				break;
			}
			}
		}
		return cOGSAccounts;
	}

	protected void CopyAccountData(DataTable data, string tableFilter, COGSAccounts accounts)
	{
		DataRow[] array = data.Select("TableSource = " + tableFilter.ToLinq());
		if (array != null && array.Length != 0)
		{
			DataRow row = array[0];
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvCOGSLaborGLAccountID")))
			{
				accounts.COGSLaborGLAccountID = row.Field<string>("imvCOGSLaborGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvCOGSMaterialGLAccountID")))
			{
				accounts.COGSMaterialGLAccountID = row.Field<string>("imvCOGSMaterialGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvCOGSSubcontractGLAccountID")))
			{
				accounts.COGSSubcontractGLAccountID = row.Field<string>("imvCOGSSubcontractGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvCOGSOverheadGLAccountID")))
			{
				accounts.COGSOverheadGLAccountID = row.Field<string>("imvCOGSOverheadGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvSVarLaborGLAccountID")))
			{
				accounts.SVarLaborGLAccountID = row.Field<string>("imvSVarLaborGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvSVarMaterialGLAccountID")))
			{
				accounts.SVarMaterialGLAccountID = row.Field<string>("imvSVarMaterialGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvSVarSubcontractGLAccountID")))
			{
				accounts.SVarSubcontractGLAccountID = row.Field<string>("imvSVarSubcontractGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvSVarOverheadGLAccountID")))
			{
				accounts.SVarOverheadGLAccountID = row.Field<string>("imvSVarOverheadGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvPurchaseVarianceGLAccountID")))
			{
				accounts.PurchaseVarianceGLAccountID = row.Field<string>("imvPurchaseVarianceGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvWIPLaborGLAccountID")))
			{
				accounts.WIPLaborGLAccountID = row.Field<string>("imvWIPLaborGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvWIPMaterialGLAccountID")))
			{
				accounts.WIPMaterialGLAccountID = row.Field<string>("imvWIPMaterialGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvWIPSubcontractGLAccountID")))
			{
				accounts.WIPSubcontractGLAccountID = row.Field<string>("imvWIPSubcontractGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvWIPOverheadGLAccountID")))
			{
				accounts.WIPOverheadGLAccountID = row.Field<string>("imvWIPOverheadGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvAccruedCreditorsGLAccountID")))
			{
				accounts.AccruedCreditorsGLAccountID = row.Field<string>("imvAccruedCreditorsGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvLaborClearingGLAccountID")))
			{
				accounts.LaborClearingGLAccountID = row.Field<string>("imvLaborClearingGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvOverheadClearingGLAccountID")))
			{
				accounts.OverheadClearingGLAccountID = row.Field<string>("imvOverheadClearingGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("imvStockRevaluationGLAccountID")))
			{
				accounts.StockRevaluationGLAccountID = row.Field<string>("imvStockRevaluationGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("xafShipAwaitInvoiceGLAccountID")))
			{
				accounts.ShipAwaitInvoiceGLAccountID = row.Field<string>("xafShipAwaitInvoiceGLAccountID");
			}
			if (!string.IsNullOrWhiteSpace(row.Field<string>("xafStockInTransitGLAccountID")))
			{
				accounts.StockInTransitGLAccountID = row.Field<string>("xafStockInTransitGLAccountID");
			}
			accounts.Success = true;
			accounts.TableSource = tableFilter;
		}
	}

	public CostOfGoodSoldDefinition.JournalLine BuildJournalLineObject(M1Database database, SqlTransaction transaction, CostOfGoodSoldDefinition.Journal journal, int lineID, decimal cost, string glAccountID, Guid sourceUniqueId, string description, CostOfGoodSoldDefinition.JournalLineTransactionType journalLineTransType, JobInfo jobInfo)
	{
		CostOfGoodSoldDefinition.JournalLine journalLine = new CostOfGoodSoldDefinition.JournalLine();
		journalLine.LineID = lineID;
		journalLine.JournalLineTransactionType = journalLineTransType;
		journalLine.TransactionAmount = cost;
		if (cost > 0m)
		{
			journalLine.DebitAmount = Math.Abs(cost);
			journalLine.CreditAmount = default(decimal);
			journalLine.JournalType = CostOfGoodSoldDefinition.JournalType.Debit;
		}
		else
		{
			journalLine.DebitAmount = default(decimal);
			journalLine.CreditAmount = Math.Abs(cost);
			journalLine.JournalType = CostOfGoodSoldDefinition.JournalType.Credit;
		}
		journalLine.GLAccountID = glAccountID;
		journalLine.Reference = string.Empty;
		journalLine.Description = description;
		journalLine.TransactionDate = journal.TransactionDate;
		journalLine.PartTransactionID = 0;
		journalLine.OrganizationID = string.Empty;
		journalLine.LocationID = string.Empty;
		if (jobInfo != null)
		{
			journalLine.JobID = jobInfo.JobID;
			journalLine.JobAssemblyID = jobInfo.JobAssemblyID;
			journalLine.JobMaterialID = jobInfo.JobMaterialID;
			journalLine.JobOperationID = jobInfo.JobOperationID;
			journalLine.JobMaterialComponentID = jobInfo.JobMaterialComponentID;
		}
		journalLine.SourceGuid = sourceUniqueId;
		return journalLine;
	}

	public CostOfGoodSoldDefinition.Journal BuildJournalObject(M1Database database, SqlTransaction transaction, DateTime transactionDate, short fiscalYearId, byte fiscalYearPeriodId, CostOfGoodSoldDefinition.JournalSource glHeaderSource, CostOfGoodSoldDefinition.DetailSource glDetailSource, string description)
	{
		return new CostOfGoodSoldDefinition.Journal
		{
			TransactionDate = transactionDate,
			Reference = string.Empty,
			Description = description,
			GLFiscalYearID = fiscalYearId,
			GLFiscalYearPeriodID = fiscalYearPeriodId,
			Source = glHeaderSource,
			DetailSource = glDetailSource,
			OrganizationID = string.Empty,
			LocationID = string.Empty
		};
	}

	public void AddJournal(M1Database database, SqlTransaction transaction, CostOfGoodSoldDefinition.Journal journalObject, string journalSource, DataRow sourceRow, string fieldPrefix)
	{
		SqlDataAdapter adapter = null;
		SqlDataAdapter adapter2 = null;
		DataTable dataTable = database.GetDataTable("Select * From GLJournals Where 0=1", fillSchema: false, out adapter, transaction);
		DataRow dataRow = dataTable.NewRow().BlankRow();
		dataRow["glpTransactionDate"] = journalObject.TransactionDate;
		dataRow["glpReference"] = journalObject.Reference;
		dataRow["glpDescription"] = journalObject.Description;
		dataRow["glpLongDescriptionText"] = journalObject.LongDescriptionText;
		dataRow["glpGLFiscalYearID"] = journalObject.GLFiscalYearID;
		dataRow["glpGLFiscalYearPeriodID"] = journalObject.GLFiscalYearPeriodID;
		dataRow["glpSource"] = journalObject.Source;
		dataRow["glpDetailSource"] = journalObject.DetailSource;
		dataRow["glpOrganizationID"] = journalObject.OrganizationID;
		dataRow["glpLocationID"] = journalObject.LocationID;
		dataRow["glpTotalDebits"] = journalObject.TotalDebits;
		dataRow["glpTotalCredits"] = journalObject.TotalCredits;
		new COGS().PopulateJournalSourceInformation(dataRow, sourceRow, DataRowVersion.Current, fieldPrefix, string.Empty, 0);
		dataRow["glpCreatedBy"] = database.User.ID;
		dataRow["glpCreatedDate"] = DateTime.Now;
		dataRow["glpGLJournalID"] = database.ExecuteScalar("Select IsNull(Max(glpGLJournalID),0)+1 From GLJournals", transaction);
		dataTable.Rows.Add(dataRow);
		database.UpdateData(new DataRow[1] { dataRow }, adapter, transaction);
		journalObject.ID = dataRow["glpGLJournalID"].ToString();
		DataTable dataTable2 = database.GetDataTable("Select * From GLJournalLines Where 0=1", fillSchema: false, out adapter2, transaction);
		foreach (CostOfGoodSoldDefinition.JournalLine journalLine in journalObject.JournalLines)
		{
			DataRow dataRow2 = dataTable2.NewRow().BlankRow();
			dataRow2["gllGLJournalID"] = journalObject.ID;
			dataRow2["gllGLJournalLineID"] = journalLine.LineID;
			dataRow2["gllTransactionType"] = journalLine.JournalLineTransactionType;
			dataRow2["gllTransactionAmount"] = journalLine.TransactionAmount;
			dataRow2["gllDebitAmount"] = journalLine.DebitAmount;
			dataRow2["gllCreditAmount"] = journalLine.CreditAmount;
			dataRow2["gllTransactionDate"] = journalLine.TransactionDate;
			dataRow2["gllGLAccountID"] = journalLine.GLAccountID;
			dataRow2["gllGLFiscalYearID"] = journalObject.GLFiscalYearID;
			dataRow2["gllGLFiscalYearPeriodID"] = journalObject.GLFiscalYearPeriodID;
			dataRow2["gllReference"] = journalLine.Reference;
			dataRow2["gllDescription"] = journalLine.Description;
			dataRow2["gllPartTransactionID"] = 0;
			dataRow2["gllSourceTableName"] = journalSource;
			dataRow2["gllSourceTableUniqueID"] = journalLine.SourceGuid;
			dataRow2["gllJobID"] = journalLine.JobID;
			dataRow2["gllJobAssemblyID"] = journalLine.JobAssemblyID;
			dataRow2["gllJobMaterialID"] = journalLine.JobMaterialID;
			dataRow2["gllJobOperationID"] = journalLine.JobOperationID;
			dataRow2["gllJobMaterialComponentID"] = journalLine.JobMaterialComponentID;
			dataRow2["gllCreatedBy"] = database.User.ID;
			dataRow2["gllCreatedDate"] = DateTime.Now;
			dataTable2.Rows.Add(dataRow2);
		}
		database.UpdateData(dataTable2, adapter2, transaction);
		if (!string.IsNullOrWhiteSpace(dataRow["glpGLJournalID"].ToString()) && database.Props("FN").Field<bool>("xafProductionExpressPost"))
		{
			new GL().PostJournal(database, transaction, dataRow["glpGLJournalID"].ToString());
		}
	}

	public void PopulateJournalSourceInformation(DataRow row, DataRow sourceRow, DataRowVersion rowVersion, string fieldPrefix, string jobID, int jobAssemblyID)
	{
		row["glpJobID"] = jobID;
		row["glpJobAssemblyID"] = jobAssemblyID;
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "ARInvoiceID"))
		{
			row["glpARInvoiceID"] = sourceRow.Field<string>(fieldPrefix + "ARInvoiceID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "ARPaymentSessionID"))
		{
			row["glpARPaymentSessionID"] = sourceRow.Field<int>(fieldPrefix + "ARPaymentSessionID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "ARPaymentHeaderID"))
		{
			row["glpARPaymentHeaderID"] = sourceRow.Field<int>(fieldPrefix + "ARPaymentHeaderID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "APInvoiceID"))
		{
			row["glpAPInvoiceID"] = sourceRow.Field<string>(fieldPrefix + "APInvoiceID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "APPaymentSessionID"))
		{
			row["glpAPPaymentSessionID"] = sourceRow.Field<int>(fieldPrefix + "APPaymentSessionID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "APPaymentHeaderID"))
		{
			row["glpAPPaymentHeaderID"] = sourceRow.Field<int>(fieldPrefix + "APPaymentHeaderID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "PayrollSessionID"))
		{
			row["glpPayrollSessionID"] = sourceRow.Field<int>(fieldPrefix + "PayrollSessionID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "ReceiptID"))
		{
			row["glpReceiptID"] = sourceRow.Field<string>(fieldPrefix + "ReceiptID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "ShipmentID"))
		{
			row["glpShipmentID"] = sourceRow.Field<string>(fieldPrefix + "ShipmentID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "TimecardID"))
		{
			row["glpTimecardID"] = sourceRow.Field<int>(fieldPrefix + "TimecardID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "BankStatementID"))
		{
			row["glpBankStatementID"] = sourceRow.Field<int>(fieldPrefix + "BankStatementID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "AssetAdjustmentID"))
		{
			row["glpAssetAdjustmentID"] = sourceRow.Field<int>(fieldPrefix + "AssetAdjustmentID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "AssetID"))
		{
			row["glpAssetID"] = sourceRow.Field<string>(fieldPrefix + "AssetID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "RMAReceiptID"))
		{
			row["glpRMAReceiptID"] = sourceRow.Field<string>(fieldPrefix + "RMAReceiptID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "DMRShipmentID"))
		{
			row["glpDMRShipmentID"] = sourceRow.Field<string>(fieldPrefix + "DMRShipmentID", rowVersion);
		}
		if (sourceRow.Table.Columns.Contains(fieldPrefix + "LandedCostID"))
		{
			row["glpLandedCostID"] = sourceRow.Field<string>(fieldPrefix + "LandedCostID", rowVersion);
		}
	}
}
