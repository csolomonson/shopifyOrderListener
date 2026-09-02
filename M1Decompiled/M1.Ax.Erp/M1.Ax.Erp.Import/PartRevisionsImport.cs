using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.Import;

[ImportProcessing("PartRevisions")]
public class PartRevisionsImport : IImportProcessing
{
	public void BeforeUpdate(ImportProcessingParms parm)
	{
		PartRevisionSaveTriggerBefore(parm);
	}

	public void AfterUpdate(ImportProcessingParms parm)
	{
		string o = string.Empty;
		string o2 = string.Empty;
		DataRow dataRow = parm.Database.GetDataTable("SELECT top 1 inbWarehouseID, inbWarehouseBinID FROM WarehouseBins INNER JOIN Warehouses ON imwWarehouseID = inbWarehouseID WHERE imwDefaultWarehouse = 1 and inbDefaultBin = 1").AsEnumerable().FirstOrDefault();
		if (dataRow != null)
		{
			o = dataRow.Field<string>("inbWarehouseID");
			o2 = dataRow.Field<string>("inbWarehouseBinID");
		}
		parm.Database.ExecuteCommand(new SqlCommand("Insert Into Parts (impPartID,impShortDescription,impLongDescriptionRTF,impLongDescriptionText,impPartType) Select imrPartID,Max(imrShortDescription),Max(Convert(nvarchar(max),imrLongDescriptionRTF)),Max(Convert(nvarchar(max),imrLongDescriptionText)),2 From PartRevisions Where imrPartID Not In (Select impPartID From Parts) And imrPartID+imrPartRevisionID In (Select imrPartID+imrPartRevisionID From " + parm.TempTable + ") Group By imrPartID"));
		parm.Database.ExecuteCommand(new SqlCommand("INSERT INTO PartWarehouseLocations (imlPartID,imlPartRevisionID,imlPartWarehouseID) SELECT imrPartID as imlPartID,imrPartRevisionID as imlPartRevisionID," + M1Util.ConvertToSql(o) + " as imlPartWarehouseID FROM PartRevisions WHERE imrPartID+imrPartRevisionID NOT IN (SELECT imlPartID+imlPartRevisionID FROM PartWarehouseLocations) And imrPartID+imrPartRevisionID In (Select imrPartID+imrPartRevisionID From " + parm.TempTable + ")"));
		parm.Database.ExecuteCommand(new SqlCommand("INSERT INTO PartBins (imbPartID,imbPartRevisionID,imbQuantityOnHand,imbBinQuantityOnHand,imbConversionFactor,imbQuantityAllocated,imbWarehouseID,imbPartBinID) SELECT imrPartID as imbPartID,imrPartRevisionID as imbPartRevisionID,imrQuantityOnHand as imbQuantityOnHand,imrQuantityOnHand as imbBinQuantityOnHand,1 as imbConversionFactor,imrQuantityAllocated as imbQuantityAllocated, " + M1Util.ConvertToSql(o) + " as imbWarehouseID, " + M1Util.ConvertToSql(o2) + " as imbPartBinID FROM PartRevisions WHERE imrPartID+imrPartRevisionID NOT IN (SELECT imbPartID+imbPartRevisionID FROM PartBins) And imrPartID+imrPartRevisionID In (Select imrPartID+imrPartRevisionID From " + parm.TempTable + ")"));
		parm.Database.ExecuteCommand(new SqlCommand("INSERT INTO PartAssemblies (imaMethodID,imaMethodRevisionID,imaMethodAssemblyID,imaLevel,imaParentAssemblyID,imaPartID,imaPartRevisionID,imaUnitOfMeasure,imaPartShortDescription,imaPartLongDescriptionRTF,imaPartLongDescriptionText,imaProductionNotesRTF,imaProductionNotesText,imaDocuments,imaQuantityPerParent) SELECT imrPartID,imrPartRevisionID,0,1,0,imrPartID,imrPartRevisionID,imrInventoryUnitOfMeasure,imrShortDescription,imrLongDescriptionRTF,imrLongDescriptionText,imrProductionNotesRTF,imrProductionNotesText,imrDocuments,1 FROM PartRevisions WHERE imrPartID + imrPartRevisionID In (Select imrPartID+imrPartRevisionID From " + parm.TempTable + ") And imrPartID + imrPartRevisionID NOT IN (SELECT imaMethodID + imaMethodRevisionID FROM PartAssemblies WHERE imaMethodAssemblyID = 0 And imaMethodID+imaMethodRevisionID In (Select imrPartID+imrPartRevisionID From " + parm.TempTable + ") )"));
		PartRevisionSaveTriggerAfter(parm);
	}

	private void PartRevisionSaveTriggerBefore(ImportProcessingParms parm)
	{
		M1Database database = parm.Database;
		SqlTransaction sqlTransaction = parm.SqlTransaction;
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		string empty5 = string.Empty;
		string empty6 = string.Empty;
		string empty7 = string.Empty;
		_ = string.Empty;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			DropTempTable(parm.Database, "PartCostChanges");
			switch (parm.Database.Props("IM").Field<byte>("xapIMCostingMethod"))
			{
			case 2:
				empty = "imrLastLaborCost";
				empty2 = "imrLastOverheadCost";
				empty3 = "imrLastMaterialCost";
				empty4 = "imrLastSubcontractCost";
				empty5 = "imrLastDutyCost";
				empty6 = "imrLastFreightCost";
				empty7 = "imrLastMiscCost";
				break;
			case 3:
				empty = "imrStandardLaborCost";
				empty2 = "imrStandardOverheadCost";
				empty3 = "imrStandardMaterialCost";
				empty4 = "imrStandardSubcontractCost";
				empty5 = "imrStandardDutyCost";
				empty6 = "imrStandardFreightCost";
				empty7 = "imrStandardMiscCost";
				break;
			default:
				empty = "imrAverageLaborCost";
				empty2 = "imrAverageOverheadCost";
				empty3 = "imrAverageMaterialCost";
				empty4 = "imrAverageSubcontractCost";
				empty5 = "imrAverageDutyCost";
				empty6 = "imrAverageFreightCost";
				empty7 = "imrAverageMiscCost";
				break;
			}
			flag = parm.IsFieldInMap(empty);
			flag2 = parm.IsFieldInMap(empty2);
			flag3 = parm.IsFieldInMap(empty3);
			flag4 = parm.IsFieldInMap(empty4);
			flag5 = parm.IsFieldInMap(empty5);
			flag6 = parm.IsFieldInMap(empty6);
			flag7 = parm.IsFieldInMap(empty7);
			if (!(flag || flag2 || flag3 || flag4 || flag5 || flag6 || flag7))
			{
				return;
			}
			stringBuilder.Append("Select ");
			stringBuilder.Append(parm.TempTable + ".imrPartID As imrPartID," + parm.TempTable + ".imrPartRevisionID As imrPartRevisionID");
			stringBuilder.Append(" Into PartCostChanges ");
			stringBuilder.Append(" From " + parm.TempTable + " Inner Join PartRevisions On " + parm.TempTable + ".imrPartID = PartRevisions.imrPartID And " + parm.TempTable + ".imrPartRevisionID = PartRevisions.imrPartRevisionID ");
			stringBuilder.Append(" Where ");
			stringBuilder.Append(" (");
			stringBuilder.Append((!flag) ? "0=1 Or " : (parm.TempTable + "." + empty + " <> PartRevisions." + empty + " Or "));
			stringBuilder.Append((!flag2) ? "0=1 Or " : (parm.TempTable + "." + empty2 + " <> PartRevisions." + empty2 + " Or "));
			stringBuilder.Append((!flag3) ? "0=1 Or " : (parm.TempTable + "." + empty3 + " <> PartRevisions." + empty3 + " Or "));
			stringBuilder.Append((!flag4) ? "0=1 Or " : (parm.TempTable + "." + empty4 + " <> PartRevisions." + empty4 + " Or "));
			stringBuilder.Append((!flag5) ? "0=1 Or " : (parm.TempTable + "." + empty5 + " <> PartRevisions." + empty5 + " Or "));
			stringBuilder.Append((!flag6) ? "0=1 Or " : (parm.TempTable + "." + empty6 + " <> PartRevisions." + empty6 + " Or "));
			stringBuilder.Append((!flag7) ? "0=1" : (parm.TempTable + "." + empty7 + " <> PartRevisions." + empty7));
			stringBuilder.Append(" )");
			parm.Database.ExecuteCommand(new SqlCommand(stringBuilder.ToString().Trim()));
			ClearStringBuilder(stringBuilder);
			SqlCommand sqlCommand = database.NewSqlCommand("SELECT imrPartID, imrPartRevisionID FROM PartCostChanges");
			foreach (DataRow row in database.GetDataTable(sqlCommand, sqlTransaction).Rows)
			{
				using M1BindingSource m1BindingSource = new M1BindingSource(database, sqlTransaction);
				m1BindingSource.DataSourceTable = "PARTREVISIONS";
				m1BindingSource.NavigateTo(database, "imrPartID = " + M1Util.ConvertToSql(row["imrPartID"]) + " And imrPartRevisionID = " + M1Util.ConvertToSql(row["imrPartRevisionID"]));
				if (m1BindingSource.Count != 0)
				{
					DataRow currentAsDataRow = m1BindingSource.CurrentAsDataRow;
					new Part().AddCostsUpdatesTransaction(m1BindingSource, sqlTransaction);
					if (database.Props("FinancialProperties").Field<bool>("xafGLCreateStockJournals") && database.Props("ProductionProperties").Field<byte>("xapIMCostingMethod").Equals(3))
					{
						new CostOfGoodSoldDefinition(m1BindingSource, "imrQuantityOnHand", "imrPartRevisionID", DateTime.Now, 37, 3, reverseSign: false, currentAsDataRow.Field<decimal>("imrQuantityOnHand"), "ManualJournalCreation").AddJournal(m1BindingSource.Database, currentAsDataRow, DataRowVersion.Current, sqlTransaction);
					}
				}
			}
			DropTempTable(parm.Database, "PartCostChanges");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void PartRevisionSaveTriggerAfter(ImportProcessingParms parm)
	{
		M1Database database = parm.Database;
		SqlTransaction sqlTransaction = parm.SqlTransaction;
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		string empty5 = string.Empty;
		string empty6 = string.Empty;
		string empty7 = string.Empty;
		_ = string.Empty;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			DropTempTable(parm.Database, "PartCostChanges");
			switch (parm.Database.Props("IM").Field<byte>("xapIMCostingMethod"))
			{
			case 2:
				empty = "imrLastLaborCost";
				empty2 = "imrLastOverheadCost";
				empty3 = "imrLastMaterialCost";
				empty4 = "imrLastSubcontractCost";
				empty5 = "imrLastDutyCost";
				empty6 = "imrLastFreightCost";
				empty7 = "imrLastMiscCost";
				break;
			case 3:
				empty = "imrStandardLaborCost";
				empty2 = "imrStandardOverheadCost";
				empty3 = "imrStandardMaterialCost";
				empty4 = "imrStandardSubcontractCost";
				empty5 = "imrStandardDutyCost";
				empty6 = "imrStandardFreightCost";
				empty7 = "imrStandardMiscCost";
				break;
			default:
				empty = "imrAverageLaborCost";
				empty2 = "imrAverageOverheadCost";
				empty3 = "imrAverageMaterialCost";
				empty4 = "imrAverageSubcontractCost";
				empty5 = "imrAverageDutyCost";
				empty6 = "imrAverageFreightCost";
				empty7 = "imrAverageMiscCost";
				break;
			}
			flag = parm.IsFieldInMap(empty);
			flag2 = parm.IsFieldInMap(empty2);
			flag3 = parm.IsFieldInMap(empty3);
			flag4 = parm.IsFieldInMap(empty4);
			flag5 = parm.IsFieldInMap(empty5);
			flag6 = parm.IsFieldInMap(empty6);
			flag7 = parm.IsFieldInMap(empty7);
			if (!(flag || flag2 || flag3 || flag4 || flag5 || flag6 || flag7))
			{
				return;
			}
			stringBuilder.Append("Select ");
			stringBuilder.Append("imrPartID As imrPartID,imrPartRevisionID As imrPartRevisionID");
			stringBuilder.Append(" Into PartCostChanges ");
			stringBuilder.Append(" From " + parm.TempTable);
			stringBuilder.Append(" Where ");
			stringBuilder.Append(" (" + parm.TempTable + ".imrPartID+" + parm.TempTable + ".imrPartRevisionID NOT IN (Select imtPartID+imtPartRevisionID From PartTransactions) And ");
			stringBuilder.Append(" (");
			stringBuilder.Append((!flag) ? "0=1 Or " : (parm.TempTable + "." + empty + " <> 0 Or "));
			stringBuilder.Append((!flag2) ? "0=1 Or " : (parm.TempTable + "." + empty2 + " <> 0 Or "));
			stringBuilder.Append((!flag3) ? "0=1 Or " : (parm.TempTable + "." + empty3 + " <> 0 Or "));
			stringBuilder.Append((!flag4) ? "0=1 Or " : (parm.TempTable + "." + empty4 + " <> 0 Or "));
			stringBuilder.Append((!flag5) ? "0=1 Or " : (parm.TempTable + "." + empty5 + " <> 0 Or "));
			stringBuilder.Append((!flag6) ? "0=1 Or " : (parm.TempTable + "." + empty6 + " <> 0 Or "));
			stringBuilder.Append((!flag7) ? "0=1" : (parm.TempTable + "." + empty7 + " <> 0"));
			stringBuilder.Append(" ) )");
			parm.Database.ExecuteCommand(new SqlCommand(stringBuilder.ToString().Trim()));
			ClearStringBuilder(stringBuilder);
			SqlCommand sqlCommand = database.NewSqlCommand("SELECT imrPartID, imrPartRevisionID FROM PartCostChanges");
			foreach (DataRow row in database.GetDataTable(sqlCommand, sqlTransaction).Rows)
			{
				using M1BindingSource m1BindingSource = new M1BindingSource(database, sqlTransaction);
				m1BindingSource.DataSourceTable = "PARTREVISIONS";
				m1BindingSource.NavigateTo(database, "imrPartID = " + M1Util.ConvertToSql(row["imrPartID"]) + " And imrPartRevisionID = " + M1Util.ConvertToSql(row["imrPartRevisionID"]));
				if (m1BindingSource.Count != 0)
				{
					DataRow currentAsDataRow = m1BindingSource.CurrentAsDataRow;
					new Part().AddCostsUpdatesTransaction(m1BindingSource, sqlTransaction);
					if (database.Props("FinancialProperties").Field<bool>("xafGLCreateStockJournals") && database.Props("ProductionProperties").Field<byte>("xapIMCostingMethod").Equals(3))
					{
						new CostOfGoodSoldDefinition(m1BindingSource, "imrQuantityOnHand", "imrPartRevisionID", DateTime.Now, 37, 3, reverseSign: false, currentAsDataRow.Field<decimal>("imrQuantityOnHand"), "ManualJournalCreation").AddJournal(m1BindingSource.Database, currentAsDataRow, DataRowVersion.Current, sqlTransaction);
					}
				}
			}
			DropTempTable(parm.Database, "PartCostChanges");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void ClearStringBuilder(StringBuilder sb)
	{
		sb.Length = 0;
		sb.Capacity = 0;
	}

	private void DropTempTable(M1Database database, string sTempTable)
	{
		try
		{
			sTempTable = sTempTable.Trim();
			if (!string.IsNullOrEmpty(sTempTable))
			{
				database.ExecuteCommand(new SqlCommand("IF OBJECT_ID('" + sTempTable + "','U') IS NOT NULL Drop Table " + sTempTable));
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
