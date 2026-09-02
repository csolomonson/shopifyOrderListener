using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("WarehouseBins")]
public class WarehouseBinsChangeID : IChangeIDProcessing
{
	private class PartBinInfo
	{
		public string PartId { get; set; }

		public string PartRevisionId { get; set; }

		public string WarehouseId { get; set; }

		public string PartBinId { get; set; }

		public bool IsMerge { get; set; }

		public bool HasPartBinDetails { get; set; }

		public decimal QuantityOnHand { get; set; }

		public decimal QuantityAllocated { get; set; }

		public decimal QuantityToInspect { get; set; }

		public decimal QuantityToReturn { get; set; }

		public decimal QuantityOnOrderSales { get; set; }

		public decimal QuantityOnOrderPurchases { get; set; }

		public decimal QuantityToReturnJob { get; set; }

		public decimal QuantityBinConversionFactor { get; set; }

		public decimal BinQuantityOnHand { get; set; }

		public bool PartBinAsDefault { get; set; }

		public bool PartBinInactive { get; set; }

		public DateTime? PartBinInactiveDate { get; set; }
	}

	private class WarehouseBinInfo
	{
		public string WarehouseId { get; set; }

		public string WarehouseBinId { get; set; }

		public string WarehouseBinDescription { get; set; }

		public bool WarehouseBinInactive { get; set; }

		public DateTime? WarehouseBinInactiveDate { get; set; }

		public bool WarehouseBinAsDefault { get; set; }
	}

	private WarehouseBinInfo WarehouseBinSource = new WarehouseBinInfo();

	private WarehouseBinInfo WarehouseBinDestination = new WarehouseBinInfo();

	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
		string text = "Select inbDescription, inbInactive, inbInactiveDate, inbDefaultBin FROM WarehouseBins ";
		text = text + " Where inbWarehouseID = " + parm.OldKeyValues[0].ToSql() + " and inbWarehouseBinID = " + parm.OldKeyValues[1].ToSql() + " ";
		SqlCommand sqlCommand = new SqlCommand(text);
		DataTable dataTable = parm.Database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 1)
		{
			WarehouseBinSource.WarehouseId = parm.OldKeyValues[0].ToString();
			WarehouseBinSource.WarehouseBinId = parm.OldKeyValues[1].ToString();
			WarehouseBinSource.WarehouseBinDescription = dataTable.Rows[0]["inbDescription"].ToString();
			WarehouseBinSource.WarehouseBinInactive = Convert.ToBoolean(dataTable.Rows[0]["inbInactive"]);
			if (WarehouseBinSource.WarehouseBinInactive)
			{
				WarehouseBinSource.WarehouseBinInactiveDate = Convert.ToDateTime(dataTable.Rows[0]["inbInactiveDate"]);
			}
			WarehouseBinSource.WarehouseBinAsDefault = Convert.ToBoolean(dataTable.Rows[0]["inbDefaultBin"]);
			if (parm.ChangeIDType != 1)
			{
				text = "Select inbDescription, inbInactive, inbInactiveDate, inbDefaultBin FROM WarehouseBins ";
				text = text + " Where inbWarehouseID = " + parm.NewKeyValues[0].ToSql() + " and inbWarehouseBinID = " + parm.NewKeyValues[1].ToSql() + " ";
				sqlCommand.CommandText = text;
				DataTable dataTable2 = parm.Database.GetDataTable(sqlCommand);
				if (dataTable2.Rows.Count != 1)
				{
					throw new M1Exception("WarehouseBin ID update/merge fails to read destination bin info: " + Environment.NewLine + parm.NewKeyValues[0].ToString() + "/" + parm.NewKeyValues[1].ToString());
				}
				WarehouseBinDestination.WarehouseId = parm.NewKeyValues[0].ToString();
				WarehouseBinDestination.WarehouseBinId = parm.NewKeyValues[1].ToString();
				WarehouseBinDestination.WarehouseBinDescription = dataTable2.Rows[0]["inbDescription"].ToString();
				WarehouseBinDestination.WarehouseBinInactive = Convert.ToBoolean(dataTable2.Rows[0]["inbInactive"]);
				if (WarehouseBinDestination.WarehouseBinInactive)
				{
					WarehouseBinDestination.WarehouseBinInactiveDate = Convert.ToDateTime(dataTable2.Rows[0]["inbInactiveDate"]);
				}
				WarehouseBinDestination.WarehouseBinAsDefault = Convert.ToBoolean(dataTable2.Rows[0]["inbDefaultBin"]);
			}
			text = "Select imbPartID, imbPartRevisionID,";
			text += " imbQuantityOnHand, imbConversionFactor, imbQuantityAllocated, imbQuantityToInspect, imbQuantityToReturn, imbQuantityOnOrderSales, imbQuantityOnOrderPurchases, imbQuantityToReturnJob,";
			text += " imbDefaultBin, imbInactiveBin, imbInactiveBinDate,";
			text = text + " (Select count(*) FROM PartBins DesternationBin Where DesternationBin.imbPartID = Source.imbPartID and DesternationBin.imbPartRevisionID = Source.imbPartRevisionID and DesternationBin.imbWarehouseID = " + parm.OldKeyValues[0].ToSql() + " and DesternationBin.imbPartBinID = " + parm.NewKeyValues[1].ToSql() + ") As IsMerge,";
			text = text + " (Select count(*) FROM PartBinDetails BinDetails Where BinDetails.imgPartID = Source.imbPartID and BinDetails.imgPartRevisionID = Source.imbPartRevisionID and BinDetails.imgWarehouseID = " + parm.OldKeyValues[0].ToSql() + " and BinDetails.imgPartBinID = " + parm.OldKeyValues[1].ToSql() + ") As HasPartBinDetails ";
			text = text + "FROM PartBins Source Where imbWarehouseID = " + parm.OldKeyValues[0].ToSql() + " and imbPartBinID = " + parm.OldKeyValues[1].ToSql() + " ";
			text += "Order by imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID";
			sqlCommand.CommandText = text;
			DataTable dataTable3 = parm.Database.GetDataTable(sqlCommand);
			if (dataTable3.Rows.Count <= 0)
			{
				return;
			}
			string text2 = string.Empty;
			{
				foreach (DataRow row in dataTable3.Rows)
				{
					PartBinInfo partBinInfo = new PartBinInfo();
					PartBinInfo partBinInfo2 = new PartBinInfo();
					partBinInfo.PartId = row["imbPartID"].ToString();
					partBinInfo.PartRevisionId = row["imbPartRevisionID"].ToString();
					partBinInfo.WarehouseId = parm.OldKeyValues[0].ToString();
					partBinInfo.PartBinId = parm.OldKeyValues[1].ToString();
					partBinInfo.IsMerge = Convert.ToBoolean(row["IsMerge"]);
					partBinInfo.HasPartBinDetails = Convert.ToBoolean(row["HasPartBinDetails"]);
					if (partBinInfo.IsMerge)
					{
						partBinInfo.QuantityOnHand = Convert.ToDecimal(row["imbQuantityOnHand"]);
						partBinInfo.QuantityAllocated = Convert.ToDecimal(row["imbQuantityAllocated"]);
						partBinInfo.QuantityToInspect = Convert.ToDecimal(row["imbQuantityToInspect"]);
						partBinInfo.QuantityToReturn = Convert.ToDecimal(row["imbQuantityToReturn"]);
						partBinInfo.QuantityOnOrderSales = Convert.ToDecimal(row["imbQuantityOnOrderSales"]);
						partBinInfo.QuantityOnOrderPurchases = Convert.ToDecimal(row["imbQuantityOnOrderPurchases"]);
						partBinInfo.QuantityToReturnJob = Convert.ToDecimal(row["imbQuantityToReturnJob"]);
						partBinInfo.QuantityBinConversionFactor = ((Convert.ToDecimal(row["imbConversionFactor"]) == 0m) ? 1m : Convert.ToDecimal(row["imbConversionFactor"]));
					}
					partBinInfo.PartBinAsDefault = Convert.ToBoolean(row["imbDefaultBin"]);
					partBinInfo.PartBinInactive = Convert.ToBoolean(row["imbInactiveBin"]);
					if (partBinInfo.PartBinInactive)
					{
						partBinInfo.PartBinInactiveDate = Convert.ToDateTime(row["imbInactiveBinDate"]);
					}
					if (partBinInfo.IsMerge && parm.ChangeIDType != 1)
					{
						string text3 = "Select imbPartID, imbPartRevisionID,";
						text3 += " imbQuantityOnHand, imbConversionFactor, imbQuantityAllocated, imbQuantityToInspect, imbQuantityToReturn, imbQuantityOnOrderSales, imbQuantityOnOrderPurchases, imbQuantityToReturnJob,";
						text3 += " imbDefaultBin, imbInactiveBin, imbInactiveBinDate ";
						text3 = text3 + "FROM PartBins Where imbPartID = " + partBinInfo.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo.PartRevisionId.ToSql() + " and imbWarehouseID = " + parm.OldKeyValues[0].ToSql() + " and imbPartBinID = " + parm.NewKeyValues[1].ToSql() + " ";
						sqlCommand.CommandText = text3;
						DataTable dataTable4 = parm.Database.GetDataTable(sqlCommand);
						if (dataTable4.Rows.Count <= 0)
						{
							throw new M1Exception("WarehouseBin ID update/merge fails to read part info on destination bin: " + Environment.NewLine + partBinInfo2.PartId.ToString() + "/" + partBinInfo2.PartRevisionId.ToString() + "/" + parm.NewKeyValues[0].ToString() + "/" + parm.NewKeyValues[1].ToString());
						}
						DataRow dataRow2 = dataTable4.Rows[0];
						partBinInfo2.PartId = dataRow2["imbPartID"].ToString();
						partBinInfo2.PartRevisionId = dataRow2["imbPartRevisionID"].ToString();
						partBinInfo2.WarehouseId = parm.OldKeyValues[0].ToString();
						partBinInfo2.PartBinId = parm.NewKeyValues[1].ToString();
						partBinInfo2.QuantityOnHand = Convert.ToDecimal(dataRow2["imbQuantityOnHand"]);
						partBinInfo2.QuantityAllocated = Convert.ToDecimal(dataRow2["imbQuantityAllocated"]);
						partBinInfo2.QuantityToInspect = Convert.ToDecimal(dataRow2["imbQuantityToInspect"]);
						partBinInfo2.QuantityToReturn = Convert.ToDecimal(dataRow2["imbQuantityToReturn"]);
						partBinInfo2.QuantityOnOrderSales = Convert.ToDecimal(dataRow2["imbQuantityOnOrderSales"]);
						partBinInfo2.QuantityOnOrderPurchases = Convert.ToDecimal(dataRow2["imbQuantityOnOrderPurchases"]);
						partBinInfo2.QuantityToReturnJob = Convert.ToDecimal(dataRow2["imbQuantityToReturnJob"]);
						partBinInfo2.QuantityBinConversionFactor = ((Convert.ToDecimal(dataRow2["imbConversionFactor"]) == 0m) ? 1m : Convert.ToDecimal(dataRow2["imbConversionFactor"]));
						partBinInfo2.PartBinAsDefault = Convert.ToBoolean(dataRow2["imbDefaultBin"]);
						partBinInfo2.PartBinInactive = Convert.ToBoolean(dataRow2["imbInactiveBin"]);
						if (partBinInfo2.PartBinInactive)
						{
							partBinInfo2.PartBinInactiveDate = Convert.ToDateTime(dataRow2["imbInactiveBinDate"]);
						}
						text2 = $"Update PartBins Set imbQuantityOnHand = {partBinInfo.QuantityOnHand} + {partBinInfo2.QuantityOnHand}, imbQuantityAllocated = {partBinInfo.QuantityAllocated} + {partBinInfo2.QuantityAllocated}, imbQuantityToInspect = {partBinInfo.QuantityToInspect} + {partBinInfo2.QuantityToInspect}, ";
						text2 += $"imbQuantityToReturn = {partBinInfo.QuantityToReturn} + {partBinInfo2.QuantityToReturn}, imbQuantityOnOrderSales = {partBinInfo.QuantityOnOrderSales} + {partBinInfo2.QuantityOnOrderSales}, imbQuantityOnOrderPurchases = {partBinInfo.QuantityOnOrderPurchases} + {partBinInfo2.QuantityOnOrderPurchases}, imbQuantityToReturnJob = {partBinInfo.QuantityToReturnJob} + {partBinInfo2.QuantityToReturnJob}, ";
						if (parm.ChangeIDType == 2)
						{
							text2 += $"imbBinQuantityOnHand = ({partBinInfo2.QuantityOnHand} +  {partBinInfo.QuantityOnHand}) / {partBinInfo.QuantityBinConversionFactor} ";
							text2 = text2 + "Where imbPartID = " + partBinInfo.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo.PartRevisionId.ToSql() + " and imbWarehouseID = " + partBinInfo.WarehouseId.ToSql() + " and imbPartBinID = " + partBinInfo.PartBinId.ToSql();
						}
						else
						{
							text2 += $"imbBinQuantityOnHand = ({partBinInfo2.QuantityOnHand} +  {partBinInfo.QuantityOnHand}) / {partBinInfo2.QuantityBinConversionFactor} ";
							text2 = text2 + "Where imbPartID = " + partBinInfo2.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo2.PartRevisionId.ToSql() + " and imbWarehouseID = " + partBinInfo2.WarehouseId.ToSql() + " and imbPartBinID = " + partBinInfo2.PartBinId.ToSql();
						}
						parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
						if (parm.ChangeIDType == 2 && partBinInfo.PartBinAsDefault && !partBinInfo2.PartBinInactive)
						{
							if (!partBinInfo2.PartBinAsDefault)
							{
								text2 = "Update PartBins Set imbDefaultBin = 0 Where imbPartID = " + partBinInfo2.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo2.PartRevisionId.ToSql() + " and imbDefaultBin = 1";
								parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
							}
							text2 = "Update PartBins Set imbDefaultBin = 1 Where imbPartID = " + partBinInfo.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo.PartRevisionId.ToSql() + " and imbWarehouseID = " + partBinInfo.WarehouseId.ToSql() + " and imbPartBinID = " + partBinInfo.PartBinId.ToSql();
							parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
						}
						if (parm.ChangeIDType == 3 && partBinInfo2.PartBinAsDefault && !partBinInfo.PartBinInactive)
						{
							if (!partBinInfo.PartBinAsDefault)
							{
								text2 = "Update PartBins Set imbDefaultBin = 0 Where imbPartID = " + partBinInfo.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo.PartRevisionId.ToSql() + " and imbDefaultBin = 1";
								parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
							}
							text2 = "Update PartBins Set imbDefaultBin = 1 Where imbPartID = " + partBinInfo2.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo2.PartRevisionId.ToSql() + " and imbWarehouseID = " + partBinInfo2.WarehouseId.ToSql() + " and imbPartBinID = " + partBinInfo2.PartBinId.ToSql();
							parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
						}
						if (parm.ChangeIDType == 2 && partBinInfo2.PartBinInactive != partBinInfo.PartBinInactive)
						{
							text2 = ((!partBinInfo.PartBinInactive) ? ("Update PartBins Set imbInactiveBin = 0, imbInactiveBinDate = Null Where imbPartID = " + partBinInfo2.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo2.PartRevisionId.ToSql() + " and imbWarehouseID = " + partBinInfo2.WarehouseId.ToSql() + " and imbPartBinID = " + partBinInfo2.PartBinId.ToSql()) : ("Update PartBins Set imbInactiveBin = 1, imbInactiveBinDate =  " + partBinInfo.PartBinInactiveDate.ToSql() + " Where imbPartID = " + partBinInfo2.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo2.PartRevisionId.ToSql() + " and imbWarehouseID = " + partBinInfo2.WarehouseId.ToSql() + " and imbPartBinID = " + partBinInfo2.PartBinId.ToSql()));
							parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
						}
						if (parm.ChangeIDType == 3 && partBinInfo2.PartBinInactive != partBinInfo.PartBinInactive)
						{
							text2 = ((!partBinInfo2.PartBinInactive) ? ("Update PartBins Set imbInactiveBin = 0, imbInactiveBinDate = Null Where imbPartID = " + partBinInfo.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo.PartRevisionId.ToSql() + " and imbWarehouseID = " + partBinInfo.WarehouseId.ToSql() + " and imbPartBinID = " + partBinInfo.PartBinId.ToSql()) : ("Update PartBins Set imbInactiveBin = 1, imbInactiveBinDate =  " + partBinInfo2.PartBinInactiveDate.ToSql() + " Where imbPartID = " + partBinInfo.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo.PartRevisionId.ToSql() + " and imbWarehouseID = " + partBinInfo.WarehouseId.ToSql() + " and imbPartBinID = " + partBinInfo.PartBinId.ToSql()));
							parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
						}
					}
					if (partBinInfo.HasPartBinDetails)
					{
						try
						{
							string s = parm.Database.NextIDs.GetNextIDForTable("warehouseBins", new object[1] { parm.OldKeyValues[0].ToString() }).ToString();
							text2 = "Update PartBinDetails Set imgPartBinID =  " + s.ToSql() + " where imgPartID = " + partBinInfo.PartId.ToSql() + " and imgPartRevisionID = " + partBinInfo.PartRevisionId.ToSql() + " and imgWarehouseID = " + partBinInfo.WarehouseId.ToSql() + " and imgPartBinID = " + partBinInfo.PartBinId.ToSql();
							parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
							text2 = "UPDATE a SET a.imgPartBinDetailID = b.newOrder FROM PartBinDetails a INNER JOIN (";
							text2 += "SELECT ROW_NUMBER() OVER ( ORDER BY imgtransactionDate ) AS newOrder, imgPartID, imgPartRevisionID, imgWarehouseID, imgPartBinID, imgUniqueID ";
							text2 = text2 + "FROM PartBinDetails WHERE imgPartID = " + partBinInfo.PartId.ToSql() + " and imgPartRevisionID = " + partBinInfo.PartRevisionId.ToSql() + " and imgWarehouseID = " + partBinInfo.WarehouseId.ToSql() + " and (imgPartBinID = " + s.ToSql() + " OR imgPartBinID = " + parm.NewKeyValues[1].ToSql() + ")) b ON b.imgUniqueID = a.imgUniqueID";
							parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
							text2 = "Update PartBinDetails Set imgPartBinID = " + parm.NewKeyValues[1].ToSql() + " where imgPartID = " + partBinInfo.PartId.ToSql() + " and imgPartRevisionID = " + partBinInfo.PartRevisionId.ToSql() + " and imgWarehouseID = " + partBinInfo.WarehouseId.ToSql() + " and imgPartBinID =  " + s.ToSql();
							parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
						}
						catch (Exception ex)
						{
							throw new M1Exception("WarehouseBin ID update/merge fails to commit transaction: " + Environment.NewLine + text2 + Environment.NewLine + ex.Message);
						}
					}
					if (parm.UsersChoiceOfCascadingChangeOnDefaultBin)
					{
						if (parm.ChangeIDType == 2 && WarehouseBinSource.WarehouseBinAsDefault && !partBinInfo.PartBinInactive)
						{
							text2 = "UPDATE PartBins SET imbDefaultBin = 0 WHERE imbPartID = " + partBinInfo.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo.PartRevisionId.ToSql() + " ";
							parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
							text2 = "UPDATE PartBins SET imbDefaultBin = 1 WHERE imbPartID = " + partBinInfo.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo.PartRevisionId.ToSql() + " and imbWarehouseID = " + partBinInfo.WarehouseId.ToSql() + " and imbPartBinID = " + partBinInfo.PartBinId.ToSql() + " ";
							parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
						}
						if (parm.ChangeIDType == 3 && WarehouseBinDestination.WarehouseBinAsDefault && !partBinInfo2.PartBinInactive)
						{
							text2 = "UPDATE PartBins SET imbDefaultBin = 0 WHERE imbPartID = " + partBinInfo2.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo2.PartRevisionId.ToSql() + " ";
							parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
							text2 = "UPDATE PartBins SET imbDefaultBin = 1 WHERE imbPartID = " + partBinInfo2.PartId.ToSql() + " and imbPartRevisionID = " + partBinInfo2.PartRevisionId.ToSql() + " and imbWarehouseID = " + partBinInfo2.WarehouseId.ToSql() + " and imbPartBinID = " + partBinInfo2.PartBinId.ToSql() + " ";
							parm.Database.ExecuteCommand(text2, parm.SqlTransaction);
						}
					}
				}
				return;
			}
		}
		throw new M1Exception("WarehouseBin ID update/merge fails to read source bin info: " + Environment.NewLine + parm.OldKeyValues[0].ToString() + "/" + parm.OldKeyValues[1].ToString());
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
		M1Database database = parm.Database;
		string text = "";
		if (parm.ChangeIDType != 1)
		{
			if (parm.ChangeIDType == 2 && WarehouseBinDestination.WarehouseBinInactive != WarehouseBinSource.WarehouseBinInactive && WarehouseBinDestination.WarehouseBinInactive)
			{
				text = "UPDATE WarehouseBins SET inbInactive = 0, inbInactiveDate = Null Where inbWarehouseID = " + WarehouseBinDestination.WarehouseId.ToSql() + " and inbWarehouseBinID = " + WarehouseBinDestination.WarehouseBinId.ToSql();
				database.ExecuteCommand(text, parm.SqlTransaction);
			}
			if (parm.ChangeIDType == 3)
			{
				text = "UPDATE WarehouseBins SET inbDescription =  " + WarehouseBinDestination.WarehouseBinDescription.ToSql() + ", inbDefaultBin =  " + WarehouseBinDestination.WarehouseBinAsDefault.ToSql() + ", inbInactive =  " + WarehouseBinDestination.WarehouseBinInactive.ToSql();
				text = ((!WarehouseBinDestination.WarehouseBinInactive) ? (text + ", inbInactiveDate = Null ") : (text + ", inbInactiveDate = " + WarehouseBinDestination.WarehouseBinInactiveDate.ToSql() + " "));
				text = text + " Where inbWarehouseID = " + WarehouseBinDestination.WarehouseId.ToSql() + " and inbWarehouseBinID = " + WarehouseBinDestination.WarehouseBinId.ToSql();
				database.ExecuteCommand(text, parm.SqlTransaction);
			}
		}
		text = "Update PartBins Set imbInactiveBin = 1, imbInactiveBinDate = GETDATE() From PartBins inner join WarehouseBins on imbWarehouseID = inbWarehouseID and imbPartBinID = inbWarehouseBinID where inbWarehouseID = " + parm.NewKeyValues[0].ToSql() + " and inbWarehouseBinID = " + parm.NewKeyValues[1].ToSql() + " and inbInactive = 1";
		database.ExecuteCommand(text, parm.SqlTransaction);
		string queryString = "select count(*) from partBins Where imbWarehouseID = " + parm.NewKeyValues[0].ToSql() + " and imbPartBinID = " + parm.NewKeyValues[1].ToSql() + " And (imbQuantityOnHand > 0 OR imbQuantityToInspect > 0)";
		if (Convert.ToInt32(database.ExecuteScalar(queryString, parm.SqlTransaction)) > 0)
		{
			text = "UPDATE WarehouseBins SET inbHasQOHQTI = 1 Where inbWarehouseID = " + parm.NewKeyValues[0].ToSql() + " and inbWarehouseBinID = " + parm.NewKeyValues[1].ToSql();
			database.ExecuteCommand(text, parm.SqlTransaction);
		}
		database.OnTableChanged("PartRevisions");
		database.OnTableChanged("PartBins");
		database.OnTableChanged("Warehouses");
		database.OnTableChanged("WarehouseBins");
		database.OnTableChanged("QuantityAdjustments");
	}
}
