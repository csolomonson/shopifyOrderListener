using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("PartBins")]
public class PartBinsChangeID : IChangeIDProcessing
{
	private class PartBinQuantity
	{
		public decimal QuantityOnHand { get; set; }

		public decimal QuantityAllocated { get; set; }

		public decimal QuantityToInspect { get; set; }

		public decimal QuantityToReturn { get; set; }

		public decimal QuantityOnOrderSales { get; set; }

		public decimal QuantityOnOrderPurchases { get; set; }

		public decimal QuantityToReturnJob { get; set; }

		public decimal QuantityBinConversionFactor { get; set; }

		public decimal BinQuantityOnHand { get; set; }

		public string BinID { get; set; }
	}

	private class PartBinAttributeToKeep
	{
		public decimal QuantityBinConversionFactor { get; set; }

		public bool PartBinAsDefault { get; set; }

		public bool PartBinInactive { get; set; }

		public DateTime? PartBinInactiveDate { get; set; }

		public string BinID { get; set; }
	}

	private PartBinQuantity QuantitySource = new PartBinQuantity();

	private PartBinQuantity QuantityDestination = new PartBinQuantity();

	private PartBinAttributeToKeep BinAttributeAfterMerge = new PartBinAttributeToKeep();

	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
		if (parm.ChangeIDType != 1)
		{
			string text = "SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbQuantityOnHand, imbConversionFactor, imbQuantityAllocated, imbQuantityToInspect, imbQuantityToReturn, imbQuantityOnOrderSales, imbQuantityOnOrderPurchases, imbQuantityToReturnJob FROM PartBins ";
			text = text + "WHERE imbPartID = " + parm.OldKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.OldKeyValues[1].ToSql() + " and imbWarehouseID = " + parm.OldKeyValues[2].ToSql() + " and imbPartBinID = " + parm.OldKeyValues[3].ToSql();
			SqlCommand sqlCommand = new SqlCommand(text);
			DataTable dataTable = parm.Database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row = dataTable.Rows[0];
				QuantitySource.BinID = row.Field<string>("imbPartBinID");
				QuantitySource.QuantityOnHand = row.Field<decimal>("imbQuantityOnHand");
				QuantitySource.QuantityAllocated = row.Field<decimal>("imbQuantityAllocated");
				QuantitySource.QuantityToInspect = row.Field<decimal>("imbQuantityToInspect");
				QuantitySource.QuantityToReturn = row.Field<decimal>("imbQuantityToReturn");
				QuantitySource.QuantityOnOrderSales = row.Field<decimal>("imbQuantityOnOrderSales");
				QuantitySource.QuantityOnOrderPurchases = row.Field<decimal>("imbQuantityOnOrderPurchases");
				QuantitySource.QuantityToReturnJob = row.Field<decimal>("imbQuantityToReturnJob");
				QuantitySource.QuantityBinConversionFactor = ((row.Field<decimal>("imbConversionFactor") == 0m) ? 1m : row.Field<decimal>("imbConversionFactor"));
			}
			text = "SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbQuantityOnHand, imbConversionFactor, imbQuantityAllocated, imbQuantityToInspect, imbQuantityToReturn, imbQuantityOnOrderSales, imbQuantityOnOrderPurchases, imbQuantityToReturnJob FROM PartBins ";
			text = text + "WHERE imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " and imbWarehouseID = " + parm.NewKeyValues[2].ToSql() + " and imbPartBinID = " + parm.NewKeyValues[3].ToSql();
			sqlCommand.CommandText = text;
			dataTable = parm.Database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row2 = dataTable.Rows[0];
				QuantityDestination.BinID = row2.Field<string>("imbPartBinID");
				QuantityDestination.QuantityOnHand = row2.Field<decimal>("imbQuantityOnHand");
				QuantityDestination.QuantityAllocated = row2.Field<decimal>("imbQuantityAllocated");
				QuantityDestination.QuantityToInspect = row2.Field<decimal>("imbQuantityToInspect");
				QuantityDestination.QuantityToReturn = row2.Field<decimal>("imbQuantityToReturn");
				QuantityDestination.QuantityOnOrderSales = row2.Field<decimal>("imbQuantityOnOrderSales");
				QuantityDestination.QuantityOnOrderPurchases = row2.Field<decimal>("imbQuantityOnOrderPurchases");
				QuantityDestination.QuantityToReturnJob = row2.Field<decimal>("imbQuantityToReturnJob");
				QuantityDestination.QuantityBinConversionFactor = ((row2.Field<decimal>("imbConversionFactor") == 0m) ? 1m : row2.Field<decimal>("imbConversionFactor"));
			}
			string text2 = "";
			text2 = ((parm.ChangeIDType != 2) ? ("SELECT imbPartBinID, imbConversionFactor, imbInactiveBin, imbInactiveBinDate, imbDefaultBin FROM PartBins WHERE imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " and imbWarehouseID = " + parm.NewKeyValues[2].ToSql() + " and imbPartBinID = " + parm.NewKeyValues[3].ToSql()) : ("SELECT imbPartBinID, imbConversionFactor, imbInactiveBin, imbInactiveBinDate, imbDefaultBin FROM PartBins WHERE imbPartID = " + parm.OldKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.OldKeyValues[1].ToSql() + " and imbWarehouseID = " + parm.OldKeyValues[2].ToSql() + " and imbPartBinID = " + parm.OldKeyValues[3].ToSql()));
			sqlCommand.CommandText = text2;
			dataTable = parm.Database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row3 = dataTable.Rows[0];
				BinAttributeAfterMerge.BinID = row3.Field<string>("imbPartBinID");
				BinAttributeAfterMerge.QuantityBinConversionFactor = ((row3.Field<decimal>("imbConversionFactor") == 0m) ? 1m : row3.Field<decimal>("imbConversionFactor"));
				BinAttributeAfterMerge.PartBinInactive = row3.Field<bool>("imbInactiveBin");
				BinAttributeAfterMerge.PartBinInactiveDate = row3.Field<DateTime?>("imbInactiveBinDate");
				BinAttributeAfterMerge.PartBinAsDefault = row3.Field<bool>("imbDefaultBin");
			}
			string s = parm.Database.NextIDs.GetNextIDForTable("warehouseBins", new object[1] { parm.OldKeyValues[2].ToString() }).ToString();
			string queryString = "Update PartBinDetails Set imgPartBinID =  " + s.ToSql() + " where imgPartID = " + parm.OldKeyValues[0].ToSql() + " and imgPartRevisionID = " + parm.OldKeyValues[1].ToSql() + " and imgWarehouseID = " + parm.OldKeyValues[2].ToSql() + " and imgPartBinID = " + parm.OldKeyValues[3].ToSql();
			parm.Database.ExecuteCommand(queryString, parm.SqlTransaction);
			queryString = "UPDATE a SET a.imgPartBinDetailID = b.newOrder FROM PartBinDetails a INNER JOIN (";
			queryString += "SELECT  ROW_NUMBER() OVER ( ORDER BY imgtransactionDate ) AS newOrder, imgPartID, imgPartRevisionID, imgWarehouseID, imgPartBinID, imgUniqueID ";
			queryString = queryString + "FROM PartBinDetails WHERE imgPartID = " + parm.NewKeyValues[0].ToSql() + " and imgPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " and imgWarehouseID = " + parm.NewKeyValues[2].ToSql() + " and (imgPartBinID = " + s.ToSql() + " OR imgPartBinID = " + parm.NewKeyValues[3].ToSql() + ")) b ON b.imgUniqueID = a.imgUniqueID";
			parm.Database.ExecuteCommand(queryString, parm.SqlTransaction);
			queryString = "Update PartBinDetails Set imgPartBinID = " + parm.NewKeyValues[3].ToSql() + " where imgPartID = " + parm.OldKeyValues[0].ToSql() + " and imgPartRevisionID = " + parm.OldKeyValues[1].ToSql() + " and imgWarehouseID = " + parm.OldKeyValues[2].ToSql() + " and imgPartBinID =  " + s.ToSql();
			parm.Database.ExecuteCommand(queryString, parm.SqlTransaction);
		}
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
		M1Database database = parm.Database;
		string text = "";
		if (parm.ChangeIDType == 1)
		{
			text = "Update PartBins Set imbDescription = inbDescription, imbDefaultBin = 0 From PartBins inner join WarehouseBins on imbWarehouseID = inbWarehouseID and imbPartBinID = inbWarehouseBinID where imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " and imbWarehouseID = " + parm.NewKeyValues[2].ToSql() + " and imbPartBinID = " + parm.NewKeyValues[3].ToSql();
			database.ExecuteCommand(text, parm.SqlTransaction);
			text = "Update PartBins Set imbInactiveBin = 1, imbInactiveBinDate = GETDATE() From PartBins inner join WarehouseBins on imbWarehouseID = inbWarehouseID and imbPartBinID = inbWarehouseBinID where imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " and imbWarehouseID = " + parm.NewKeyValues[2].ToSql() + " and imbPartBinID = " + parm.NewKeyValues[3].ToSql() + " and inbInactive = 1";
			database.ExecuteCommand(text, parm.SqlTransaction);
		}
		else
		{
			text = $"Update PartBins Set imbQuantityOnHand = {QuantitySource.QuantityOnHand} + {QuantityDestination.QuantityOnHand}, imbQuantityAllocated = {QuantitySource.QuantityAllocated} + {QuantityDestination.QuantityAllocated}, imbQuantityToInspect = {QuantitySource.QuantityToInspect} + {QuantityDestination.QuantityToInspect}, ";
			text += $"imbQuantityToReturn = {QuantitySource.QuantityToReturn} + {QuantityDestination.QuantityToReturn}, imbQuantityOnOrderSales = {QuantitySource.QuantityOnOrderSales} + {QuantityDestination.QuantityOnOrderSales}, imbQuantityOnOrderPurchases = {QuantitySource.QuantityOnOrderPurchases} + {QuantityDestination.QuantityOnOrderPurchases}, imbQuantityToReturnJob = {QuantitySource.QuantityToReturnJob} + {QuantityDestination.QuantityToReturnJob}, ";
			text += $"imbBinQuantityOnHand = ({QuantityDestination.QuantityOnHand} +  {QuantitySource.QuantityOnHand}) / {BinAttributeAfterMerge.QuantityBinConversionFactor} ";
			text = text + "Where imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " and imbWarehouseID = " + parm.NewKeyValues[2].ToSql() + " and imbPartBinID = " + parm.NewKeyValues[3].ToSql();
			database.ExecuteCommand(text, parm.SqlTransaction);
			text = "Update PartBins Set imbDefaultBin =  " + BinAttributeAfterMerge.PartBinAsDefault.ToSql() + " Where imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " and imbWarehouseID = " + parm.NewKeyValues[2].ToSql() + " and imbPartBinID = " + parm.NewKeyValues[3].ToSql();
			database.ExecuteCommand(text, parm.SqlTransaction);
			text = ((!BinAttributeAfterMerge.PartBinInactive) ? ("Update PartBins Set imbInactiveBin = 0, imbInactiveBinDate = null Where imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " and imbWarehouseID = " + parm.NewKeyValues[2].ToSql() + " and imbPartBinID = " + parm.NewKeyValues[3].ToSql()) : ("Update PartBins Set imbInactiveBin = 1, imbInactiveBinDate =  " + BinAttributeAfterMerge.PartBinInactiveDate.ToSql() + " Where imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " and imbWarehouseID = " + parm.NewKeyValues[2].ToSql() + " and imbPartBinID = " + parm.NewKeyValues[3].ToSql()));
			database.ExecuteCommand(text, parm.SqlTransaction);
		}
		string queryString = "select count(*) from partBins Where imbWarehouseID = " + parm.OldKeyValues[2].ToSql() + " and imbPartBinID = " + parm.OldKeyValues[3].ToSql() + " And (imbQuantityOnHand > 0 OR imbQuantityToInspect > 0)";
		if (Convert.ToInt32(database.ExecuteScalar(queryString, parm.SqlTransaction)) == 0)
		{
			text = "UPDATE WarehouseBins SET inbHasQOHQTI = 0 Where inbWarehouseID = " + parm.OldKeyValues[2].ToSql() + " and inbWarehouseBinID = " + parm.OldKeyValues[3].ToSql();
			database.ExecuteCommand(text, parm.SqlTransaction);
		}
		queryString = "select count(*) from partBins Where imbWarehouseID = " + parm.NewKeyValues[2].ToSql() + " and imbPartBinID = " + parm.NewKeyValues[3].ToSql() + " And (imbQuantityOnHand > 0 OR imbQuantityToInspect > 0)";
		if (Convert.ToInt32(database.ExecuteScalar(queryString, parm.SqlTransaction)) > 0)
		{
			text = "UPDATE WarehouseBins SET inbHasQOHQTI = 1 Where inbWarehouseID = " + parm.NewKeyValues[2].ToSql() + " and inbWarehouseBinID = " + parm.NewKeyValues[3].ToSql();
			database.ExecuteCommand(text, parm.SqlTransaction);
		}
		database.OnTableChanged("PartRevisions");
		database.OnTableChanged("PartBins");
		database.OnTableChanged("Warehouses");
		database.OnTableChanged("WarehouseBins");
		database.OnTableChanged("QuantityAdjustments");
	}
}
