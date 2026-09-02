using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Ax.Erp.IntegrationService;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class Receipts
{
	public bool IsReceiptLandedCost(M1Database database, SqlTransaction transaction, string receiptID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(rmpLandedCost,0) From Receipts Where rmpReceiptID = @ReceiptID");
		sqlCommand.Parameters.Add(new SqlParameter("ReceiptID", SqlDbType.NVarChar)).Value = receiptID;
		return Convert.ToBoolean(database.ExecuteScalar(sqlCommand, transaction));
	}

	public string GetReceiptSupplierID(M1Database database, SqlTransaction transaction, string receiptID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(rmpSupplierOrganizationID,'') From Receipts Where rmpReceiptID = @ReceiptID");
		sqlCommand.Parameters.Add(new SqlParameter("ReceiptID", SqlDbType.NVarChar)).Value = receiptID;
		return Convert.ToString(database.ExecuteScalar(sqlCommand, transaction));
	}

	public void PostReceipt(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		SqlTransaction sqlTransaction = bindingSource.Transaction ?? database.BeginTransaction();
		try
		{
			bindingSource.CurrentAsDataRow.BeginEdit();
			bindingSource.CurrentAsDataRow.SetField("rmpPostedToGL", value: true);
			bindingSource.CurrentAsDataRow.AcceptChanges();
			string value = bindingSource.CurrentAsDataRow.Field<string>("rmpReceiptID");
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}
			SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) AS sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, rmlUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from ReceiptLines inner join SerialNumberTransactions on rmlUniqueID = sntTableUniqueID where rmlReceiptID = @ID and rmlPostedToGL = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			DataTable dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
				foreach (DataRow row in dataTable.Rows)
				{
					byte status = 0;
					byte transType = 0;
					bool flag = row.Field<bool>("sntNegativeTransaction");
					serialNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row.Field<string>("sntSerialNumberID"));
					switch (row.Field<byte>("sntTransactionType"))
					{
					case 48:
						status = (byte)((!flag) ? 2 : 0);
						transType = 2;
						break;
					case 49:
						status = (byte)((!flag) ? 3 : 0);
						transType = 4;
						break;
					case 50:
						status = (byte)((!flag) ? 5 : 0);
						transType = 14;
						break;
					}
					serialNumberDefinition.AddSerialTransaction(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), row.Field<decimal>("sntQuantity"), status, transType, "ReceiptLines", row.Field<Guid>("rmlUniqueID"), row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), row.Field<bool>("sntNegativeTransaction"), row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, rmoUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from ReceiptComponents inner join SerialNumberTransactions on rmoUniqueID = sntTableUniqueID where rmoReceiptID = @ID and rmoPostedToGL = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition2 = new SerialNumberDefinition();
				foreach (DataRow row2 in dataTable.Rows)
				{
					byte status2 = 0;
					byte transType2 = 0;
					bool flag2 = row2.Field<bool>("sntNegativeTransaction");
					serialNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row2.Field<string>("sntSerialNumberID"));
					switch (row2.Field<byte>("sntTransactionType"))
					{
					case 48:
						status2 = (byte)((!flag2) ? 2 : 0);
						transType2 = 2;
						break;
					case 49:
						status2 = (byte)((!flag2) ? 3 : 0);
						transType2 = 4;
						break;
					case 50:
						status2 = (byte)((!flag2) ? 5 : 0);
						transType2 = 14;
						break;
					}
					serialNumberDefinition2.AddSerialTransaction(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntPartWarehouseLocationID"), row2.Field<string>("sntPartBinID"), row2.Field<string>("sntSerialNumberID"), row2.Field<decimal>("sntQuantity"), status2, transType2, "ReceiptComponents", row2.Field<Guid>("rmoUniqueID"), row2.Field<string>("sntJobID"), Convert.ToInt32(row2["sntJobAssemblyID"]), Convert.ToInt32(row2["sntJobMaterialID"]), Convert.ToInt32(row2["sntJobMaterialComponentID"]), row2.Field<bool>("sntNegativeTransaction"), row2.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition2.RefreshStatuses(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, rmlUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from ReceiptLines inner join LotNumberTransactions on rmlUniqueID = abtTableUniqueID where rmlReceiptID = @ID and rmlPostedToGL = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
				foreach (DataRow row3 in dataTable.Rows)
				{
					byte status3 = 0;
					byte transType3 = 0;
					bool flag3 = row3.Field<bool>("abtNegativeTransaction");
					lotNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row3.Field<string>("abtLotNumberID"));
					switch (row3.Field<byte>("abtTransactionType"))
					{
					case 48:
						status3 = (byte)((!flag3) ? 2 : 0);
						transType3 = 2;
						break;
					case 49:
						status3 = (byte)((!flag3) ? 3 : 0);
						transType3 = 4;
						break;
					case 50:
						status3 = (byte)((!flag3) ? 5 : 0);
						transType3 = 14;
						break;
					}
					lotNumberDefinition.AddLotTransaction(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtPartWarehouseLocationID"), row3.Field<string>("abtPartBinID"), row3.Field<string>("abtLotNumberID"), row3.Field<decimal>("abtQuantity"), status3, transType3, "ReceiptLines", row3.Field<Guid>("rmlUniqueID"), row3.Field<string>("abtJobID"), Convert.ToInt32(row3["abtJobAssemblyID"]), Convert.ToInt32(row3["abtJobMaterialID"]), Convert.ToInt32(row3["abtJobMaterialComponentID"]), row3.Field<bool>("abtNegativeTransaction"), row3.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition.RefreshStatuses(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtLotNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, rmoUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from ReceiptComponents inner join LotNumberTransactions on rmoUniqueID = abtTableUniqueID where rmoReceiptID = @ID and rmoPostedToGL = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition2 = new LotNumberDefinition();
				foreach (DataRow row4 in dataTable.Rows)
				{
					byte status4 = 0;
					byte transType4 = 0;
					bool flag4 = row4.Field<bool>("abtNegativeTransaction");
					lotNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row4.Field<string>("abtLotNumberID"));
					switch (row4.Field<byte>("abtTransactionType"))
					{
					case 48:
						status4 = (byte)((!flag4) ? 2 : 0);
						transType4 = 2;
						break;
					case 49:
						status4 = (byte)((!flag4) ? 3 : 0);
						transType4 = 4;
						break;
					case 50:
						status4 = (byte)((!flag4) ? 5 : 0);
						transType4 = 14;
						break;
					}
					lotNumberDefinition2.AddLotTransaction(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtPartWarehouseLocationID"), row4.Field<string>("abtPartBinID"), row4.Field<string>("abtLotNumberID"), row4.Field<decimal>("abtQuantity"), status4, transType4, "ReceiptComponents", row4.Field<Guid>("rmoUniqueID"), row4.Field<string>("abtJobID"), Convert.ToInt32(row4["abtJobAssemblyID"]), Convert.ToInt32(row4["abtJobMaterialID"]), Convert.ToInt32(row4["abtJobMaterialComponentID"]), row4.Field<bool>("abtNegativeTransaction"), row4.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition2.RefreshStatuses(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtLotNumberID"));
				}
			}
			M1BindingSource m1BindingSource = bindingSource.PrimaryTable?.GetChildBindingSource("ReceiptLines");
			if (m1BindingSource != null && m1BindingSource.Count > 0)
			{
				IntegrationServiceConstants.EntityType entityType = ((!bindingSource.CurrentAsDataRow.Field<bool>("rmpReversalEntry")) ? IntegrationServiceConstants.EntityType.Bill : IntegrationServiceConstants.EntityType.VendorCredit);
				new M1.Ax.Erp.IntegrationService.IntegrationService().CreateTransactionQueueRecord(database, sqlTransaction, IntegrationServiceConstants.IntegrationType.Financial, IntegrationServiceConstants.ApiAction.Create, entityType, IntegrationServiceConstants.Status.Pending, "Receipts", bindingSource.CurrentAsDataRow.Field<Guid>("rmpUniqueId"), 13);
			}
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	public bool ReceiptPeriodCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DateTime dateTime = default(DateTime);
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		bool result = false;
		if (currentAsDataRow != null)
		{
			if (currentAsDataRow.Table.Columns.Contains("rmpReceiptDate"))
			{
				dateTime = currentAsDataRow.Field<DateTime>("rmpReceiptDate");
			}
			if (new Financial().GetYearAndPeriod(database, dateTime.Date, "").Success)
			{
				result = true;
			}
		}
		return result;
	}

	public string GetMessageForNegativeParts(M1BindingSource bindingSource)
	{
		string text = (NegativeSerialLotPartCheck(bindingSource) ? " AND (impTrackLotNumbers = 1 OR impTrackSerialNumbers = 1)" : "");
		DataTable dataTable = bindingSource.Database.GetDataTable("SELECT * FROM PartRevisions INNER JOIN ReceiptLines ON rmlPartID=imrPartID and imrPartRevisionID=rmlPartRevisionID INNER JOIN Parts on impPartID=imrPartID WHERE rmlReceiptID=" + bindingSource.CurrentAsDataRow.Field<string>("rmpReceiptID").ToSql() + " AND imrQuantityOnHand+rmlInventoryQuantityReceived<0 " + text);
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			DataRow row = dataTable.Rows[i];
			if (row.Field<bool>("rmlKitPart"))
			{
				if (!flag)
				{
					DataTable dataTable2 = bindingSource.Database.GetDataTable("SELECT * FROM PartRevisions INNER JOIN ReceiptComponents ON rmoPartID=imrPartID and imrPartRevisionID=rmoPartRevisionID WHERE rmoReceiptID=" + bindingSource.CurrentAsDataRow.Field<string>("rmpReceiptID").ToSql() + " AND imrQuantityOnHand+rmoInvQuantityReceived<0");
					for (int j = 0; j < dataTable2.Rows.Count; j++)
					{
						DataRow row2 = dataTable2.Rows[j];
						stringBuilder.Append(string.Format("\n\nReversed Inv Qty Rec'd [{0}] IS GREATER THAN Quantity On Hand [{1}] [Part: '{2}', Revision: '{3}', Warehouse: '{4}', Bin: '{5}']", Math.Abs(row2.Field<decimal>("rmoInvQuantityReceived")), row2.Field<decimal>("imrQuantityOnHand"), row2.Field<string>("rmoPartID"), row2.Field<string>("rmoPartRevisionID"), row2.Field<string>("rmoPartWarehouseLocationID"), row2.Field<string>("rmoPartBinID")));
					}
					flag = true;
				}
			}
			else
			{
				stringBuilder.Append(string.Format("\n\nReversed Inv Qty Rec'd [{0}] IS GREATER THAN Quantity On Hand [{1}] [Part: '{2}', Revision: '{3}', Warehouse: '{4}', Bin: '{5}']", Math.Abs(row.Field<decimal>("rmlInventoryQuantityReceived")), row.Field<decimal>("imrQuantityOnHand"), row.Field<string>("rmlPartID"), row.Field<string>("rmlPartRevisionID"), row.Field<string>("rmlPartWarehouseLocationID"), row.Field<string>("rmlPartBinID")));
			}
		}
		return stringBuilder.ToString();
	}

	public bool NegativeSerialLotPartCheck(M1BindingSource bindingSource)
	{
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		string queryString = "SELECT rmlPartID \r\n                FROM Receipts \r\n                INNER JOIN ReceiptLines ON rmpReceiptID = rmlReceiptID \r\n                INNER JOIN PartRevisions ON imrPartRevisionID = rmlPartRevisionID AND imrPartID = rmlPartID \r\n                INNER JOIN PartWarehouseLocations ON imlPartWarehouseID = rmlPartWarehouseLocationID \r\n                INNER JOIN Parts on impPartID = imrPartID \r\n                WHERE rmpReceiptID = " + currentAsDataRow.Field<string>("rmpReceiptID").ToSql() + " AND imrQuantityOnHand + rmlInventoryQuantityReceived < 0 AND (impTrackSerialNumbers = 1 OR impTrackLotNumbers = 1) AND rmpReversalEntry = 1 \r\n                GROUP BY rmlPartID";
		return bindingSource.Database.GetDataTable(queryString).Rows.Count > 0;
	}

	public bool ReceiptPostCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		bool flag = false;
		decimal num = default(decimal);
		if (ReceiptPostedCheck(database, null, currentAsDataRow.Field<string>("rmpReceiptID")))
		{
			return false;
		}
		if (currentAsDataRow.Field<bool>("rmpLandedCost") && string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("rmpLandedCostID")))
		{
			return false;
		}
		if (currentAsDataRow.Field<bool>("rmpReversalEntry"))
		{
			if (!string.IsNullOrEmpty(VerifyQuantityForInactiveBins(bindingSource.Database, currentAsDataRow.Field<string>("rmpReceiptID"))))
			{
				return false;
			}
			DataTable dataTable = database.GetDataTable("Select rmlReverseReceiptID From ReceiptLines Where rmlReceiptID = " + M1Util.ConvertToSql(currentAsDataRow.Field<string>("rmpReceiptID")));
			if (dataTable.Rows.Count != 0)
			{
				string o = dataTable.Rows[0].Field<string>("rmlReverseReceiptID");
				DataTable dataTable2 = database.GetDataTable("Select rmlReceiptID, rmlReceiptLineID, rmlJobID, rmlJobType, rmlUniqueID, rmlKitPart, rmlPartID, rmlInventoryQuantityReceived, rmlJobMatQuantityReceived, rmlJobOprQuantityReceived, rmlQuantityToInspect,rmlPartRevisionID,rmlPartWarehouseLocationID From ReceiptLines Where rmlReceiptID = " + M1Util.ConvertToSql(o) + " order by rmlReceiptID");
				bool flag2 = (bool)database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
				foreach (DataRow row3 in dataTable2.Rows)
				{
					if (row3.Field<decimal>("rmlQuantityToInspect") != 0m)
					{
						continue;
					}
					int num2 = ((row3.Field<byte>("rmlJobType") != 0) ? 1 : 2);
					if (row3.Field<bool>("rmlKitPart"))
					{
						DataTable dataTable3 = database.GetDataTable("Select rmoJobID, rmoPartID, rmoUniqueID,rmoInvQuantityReceived,rmoPartRevisionID,rmoPartWarehouseLocationID From ReceiptComponents Where rmoReceiptID = " + M1Util.ConvertToSql(row3.Field<string>("rmlReceiptID")) + " And rmoReceiptLineID = " + M1Util.ConvertToSql(row3.Field<short>("rmlReceiptLineID")));
						if (dataTable3 != null && dataTable3.Rows.Count != 0)
						{
							foreach (DataRow row4 in dataTable3.Rows)
							{
								if (num2 == 1)
								{
									flag = PostCheckingUtility.CheckReceiptToJob(row4.Field<string>("rmoJobID"), database);
								}
								else
								{
									num = row4.Field<decimal>("rmoInvQuantityReceived");
									flag = PostCheckingUtility.CheckReceiptToInventory(row4.Field<Guid>("rmoUniqueID"), row4.Field<string>("rmoPartID"), database, num, flag2 ? row4.Field<string>("rmoPartRevisionID") : "", flag2 ? row4.Field<string>("rmoPartWarehouseLocationID") : "");
								}
								if (!flag && (num2 == 1 || !flag2 || flag2))
								{
									break;
								}
							}
						}
					}
					else if (num2 == 1)
					{
						flag = PostCheckingUtility.CheckReceiptToJob(row3.Field<string>("rmlJobID"), database);
					}
					else
					{
						num = row3.Field<decimal>("rmlInventoryQuantityReceived");
						flag = PostCheckingUtility.CheckReceiptToInventory(row3.Field<Guid>("rmlUniqueID"), row3.Field<string>("rmlPartID"), database, num, flag2 ? row3.Field<string>("rmlPartRevisionID") : "", flag2 ? row3.Field<string>("rmlPartWarehouseLocationID") : "");
					}
					if (!flag)
					{
						break;
					}
				}
			}
		}
		else
		{
			flag = GetMessageForInactivePartBins(database, currentAsDataRow, out var _);
		}
		return flag;
	}

	public bool GetMessageForInactivePartBins(M1Database database, DataRow receiptRow, out string msg)
	{
		msg = "";
		DataTable dataTable = database.GetDataTable("Select rmlReceiptID, rmlReceiptLineID, rmlJobID, rmlJobType, rmlUniqueID, rmlKitPart, rmlPartID, rmlPurchaseOrderID, rmlPurchaseOrderLineID, rmlInventoryQuantityReceived, rmlJobMatQuantityReceived, rmlJobOprQuantityReceived, rmlQuantityToInspect,rmlPartRevisionID,rmlPartWarehouseLocationID, rmlPartBinID, rmpReversalEntry From ReceiptLines Inner Join Receipts on rmpReceiptID = rmlReceiptID Where rmlReceiptID = " + M1Util.ConvertToSql(receiptRow.Field<string>("rmpReceiptID")) + " and rmlJobID='' order by rmlReceiptID");
		StringBuilder stringBuilder = new StringBuilder();
		Part part = new Part();
		PurchaseOrders purchaseOrders = new PurchaseOrders();
		foreach (DataRow row3 in dataTable.Rows)
		{
			string text = row3.Field<string>("rmlPartID");
			string text2 = row3.Field<string>("rmlPartRevisionID");
			string text3 = row3.Field<string>("rmlPartWarehouseLocationID");
			string text4 = row3.Field<string>("rmlPartBinID");
			string poID = row3.Field<string>("rmlPurchaseOrderID");
			short poLineID = row3.Field<short>("rmlPurchaseOrderLineID");
			byte purchaseOrderLineType = purchaseOrders.GetPurchaseOrderLineType(database, null, poID, poLineID);
			if (part.IsPartBinInactive(database, text, text2, text3, text4) && !part.IsPartNonStockedOrKit(database, text) && !row3.Field<bool>("rmpReversalEntry") && purchaseOrderLineType != 4)
			{
				stringBuilder.AppendLine("[Part: '" + text + "', Revision: '" + text2 + "', Warehouse: '" + text3 + "', Bin: '" + text4 + "' is inactive]");
			}
			if (!row3.Field<bool>("rmlKitPart") || row3.Field<bool>("rmpReversalEntry"))
			{
				continue;
			}
			DataTable dataTable2 = database.GetDataTable("Select rmoJobID, rmoPartID, rmoUniqueID,rmoInvQuantityReceived,rmoPartRevisionID,rmoPartWarehouseLocationID,rmoPartBinID From ReceiptComponents Where rmoReceiptID = " + M1Util.ConvertToSql(row3.Field<string>("rmlReceiptID")) + " And rmoReceiptLineID = " + M1Util.ConvertToSql(row3.Field<short>("rmlReceiptLineID")));
			if (dataTable2 == null || dataTable2.Rows.Count == 0)
			{
				continue;
			}
			foreach (DataRow row4 in dataTable2.Rows)
			{
				string text5 = row4.Field<string>("rmoPartID");
				string text6 = row4.Field<string>("rmoPartRevisionID");
				string text7 = row4.Field<string>("rmoPartWarehouseLocationID");
				string text8 = row4.Field<string>("rmoPartBinID");
				if (part.IsPartBinInactive(database, text5, text6, text7, text8) && purchaseOrderLineType != 4)
				{
					stringBuilder.AppendLine("[Part: '" + text5 + "', Revision: '" + text6 + "', Warehouse: '" + text7 + "', Bin: '" + text8 + "' is inactive]");
				}
			}
		}
		if (stringBuilder.Length > 0)
		{
			msg = "This transaction CAN NOT be posted because an INACTIVE bin exists for the part(s) indicated\n\n" + stringBuilder.ToString();
			return false;
		}
		return true;
	}

	public string VerifyQuantityForInactiveBins(M1Database database, string receiptID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (SqlCommand sqlCommand = new SqlCommand("SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbInactiveBin, rmlInventoryQuantityReceived AS quantity, imbQuantityOnHand FROM PartBins inner join ReceiptLines ON imbPartID = rmlPartID and imbPartRevisionID = rmlPartRevisionID and imbWarehouseID = rmlPartWarehouseLocationID and imbPartBinID = rmlPartBinID inner join Parts ON impPartID = imbPartID WHERE rmlReceiptID = " + receiptID.ToSql() + " and imbQuantityOnHand+rmlInventoryQuantityReceived < 0 and imbInactiveBin = 1 and impPhantomOrKitPart = 0\r\nUNION\r\nSELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbInactiveBin, rmoInvQuantityReceived AS quantity, imbQuantityOnHand FROM PartBins inner join ReceiptComponents ON imbPartID = rmoPartID and imbPartRevisionID = rmoPartRevisionID and imbWarehouseID = rmoPartWarehouseLocationID and imbPartBinID = rmoPartBinID inner join Parts ON impPartID = imbPartID WHERE rmoReceiptID = " + receiptID.ToSql() + " and imbQuantityOnHand+rmoInvQuantityReceived < 0 and imbInactiveBin = 1 and impPhantomOrKitPart = 0"))
		{
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					decimal num = row.Field<decimal>("quantity");
					decimal num2 = row.Field<decimal>("imbQuantityOnHand");
					string text = row.Field<string>("imbPartID");
					string text2 = row.Field<string>("imbPartRevisionID");
					string text3 = row.Field<string>("imbWarehouseID");
					string text4 = row.Field<string>("imbPartBinID");
					stringBuilder.AppendLine($"[Reversed Inv Qty Rec'd[{num}] IS GREATER THAN Quantity on Hand [{num2}]");
					stringBuilder.AppendLine("[Part: '" + text + "', Revision: '" + text2 + "', Warehouse: '" + text3 + "', Bin: '" + text4 + "']");
					stringBuilder.AppendLine();
				}
				return stringBuilder.ToString();
			}
		}
		return string.Empty;
	}

	public decimal GetTotalComponentsCost(M1BindingSource bindingSource, DataRow currentRow)
	{
		decimal result = default(decimal);
		if (bindingSource != null)
		{
			M1BindingSource childBindingSource = bindingSource.PrimaryTable.GetChildBindingSource("ReceiptComponents");
			if (childBindingSource != null)
			{
				foreach (DataRow row in childBindingSource.GetDataView(currentRow).ToTable().Rows)
				{
					result += (row.Field<decimal>("rmoInvQuantityReceived") + row.Field<decimal>("rmoJobQuantityReceived") + row.Field<decimal>("rmoQuantityToInspect") + row.Field<decimal>("rmoAdditionalQuantity")) * row.Field<decimal>("rmoInventoryUnitCostForeign");
				}
			}
		}
		return result;
	}

	public bool ReceiptPostedCheck(M1Database database, SqlTransaction transaction, string receiptID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(rmpPostedToGL,0) As rmpPostedToGL From Receipts Where rmpReceiptID = @ID");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = receiptID;
		object obj = database.ExecuteScalar(sqlCommand, transaction);
		if (obj == null)
		{
			return false;
		}
		return (bool)obj;
	}

	public string CheckReceiptForZeroDollarTotals(M1BindingSource bindingSource)
	{
		if (bindingSource.CurrentAsDataRow == null)
		{
			return string.Empty;
		}
		if (bindingSource.CurrentAsDataRow.Field<bool>("rmpReversalEntry"))
		{
			return string.Empty;
		}
		DataTable dataTable = bindingSource.PrimaryTable.GetChildBindingSource("ReceiptLines").GetDataTable();
		if (dataTable == null || dataTable.Rows.Count == 0)
		{
			return string.Empty;
		}
		if (dataTable.Rows.Cast<DataRow>().Any((DataRow lineRow) => (lineRow.Field<decimal>("rmlPurchaseUnitCostForeign") * lineRow.Field<decimal>("rmlPurchaseQuantityReceived")).Equals(0m)))
		{
			return "There are receipt lines that have zero dollar total amounts. If you continue, this will result in a zero dollar bill line in your financial package.\n\nDo you wish to continue posting?";
		}
		return string.Empty;
	}
}
