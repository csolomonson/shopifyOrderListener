using System;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public static class PostCheckingUtility
{
	public enum ReceiptType
	{
		ReceiptToJob = 1,
		ReceiptToInventory
	}

	private static bool CheckRemainingQuantity(DataRow binDetailsRow, decimal qtyToReverse, bool isInboundReversal)
	{
		decimal num = binDetailsRow.Field<decimal>("imgRemainingQuantity");
		if (isInboundReversal && binDetailsRow.Field<string>("imgCreatedBy").Equals("CONVERSION", StringComparison.CurrentCultureIgnoreCase) && num < qtyToReverse)
		{
			return false;
		}
		return true;
	}

	public static bool CheckReceiptToInventory(Guid receiptGUID, string partID, M1Database database, decimal qtyToReverse, string partRevisionID = "", string partWarehouseLocation = "")
	{
		DataTable dataTable = database.GetDataTable("Select impNonStockedItem From Parts Where impPartID = " + M1Util.ConvertToSql(partID));
		bool flag = (bool)database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
		if (dataTable.Rows.Count != 0)
		{
			if (dataTable.Rows[0].Field<bool>("impNonStockedItem"))
			{
				return true;
			}
			if (flag)
			{
				DataTable dataTable2 = database.GetDataTable("SELECT imrQuantityOnHand FROM PartRevisions INNER JOIN PartWarehouseLocations ON imlPartID=imrPartID AND imlPartRevisionID=imrPartRevisionID WHERE imrPartID=" + partID.ToSql() + " AND imrPartRevisionID=" + partRevisionID.ToSql() + " AND imlPartWarehouseID=" + partWarehouseLocation.ToSql());
				if (dataTable2.Rows.Count > 0)
				{
					return dataTable2.Rows[0].Field<decimal>("imrQuantityOnHand") >= qtyToReverse;
				}
			}
			else
			{
				DataTable dataTable3 = database.GetDataTable("Select imgPartID, imgOriginalQuantity,imgRemainingQuantity,imgCreatedBy From PartBinDetails Where imgSourceTableUniqueID = " + M1Util.ConvertToSql(receiptGUID));
				if (dataTable3.Rows.Count != 0)
				{
					DataRow dataRow = dataTable3.Rows[0];
					if (!CheckRemainingQuantity(dataRow, qtyToReverse, isInboundReversal: true))
					{
						return false;
					}
					if (dataRow.Field<decimal>("imgRemainingQuantity") == dataRow.Field<decimal>("imgOriginalQuantity"))
					{
						return true;
					}
				}
			}
			return false;
		}
		return true;
	}

	public static bool CheckReceiptToJob(string jobID, M1Database database)
	{
		DataTable dataTable = database.GetDataTable("Select jmpClosed From Jobs Where jmpJobID = " + M1Util.ConvertToSql(jobID));
		if (dataTable.Rows.Count != 0 && !dataTable.Rows[0].Field<bool>("jmpClosed"))
		{
			return true;
		}
		return false;
	}
}
