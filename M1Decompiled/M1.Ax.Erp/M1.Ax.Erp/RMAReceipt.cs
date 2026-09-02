using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M1.Ax.Erp.IntegrationService;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class RMAReceipt
{
	public bool RMAReceiptPeriodCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DateTime dateTime = default(DateTime);
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		bool result = false;
		if (currentAsDataRow != null)
		{
			if (currentAsDataRow.Table.Columns.Contains("rrpReceiptDate"))
			{
				dateTime = currentAsDataRow.Field<DateTime>("rrpReceiptDate");
			}
			if (new Financial().GetYearAndPeriod(database, dateTime.Date, "").Success)
			{
				result = true;
			}
		}
		return result;
	}

	public void PostRMAReceipt(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		SqlTransaction sqlTransaction = bindingSource.Transaction ?? database.BeginTransaction();
		try
		{
			if (bindingSource.CurrentAsDataRow == null)
			{
				return;
			}
			bindingSource.CurrentAsDataRow.SetField("rrpPosted", value: true);
			string value = bindingSource.CurrentAsDataRow.Field<string>("rrpRMAReceiptID");
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}
			SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) AS sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, rrlUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntNegativeTransaction from RMAReceiptLines inner join SerialNumberTransactions on rrlUniqueID = sntTableUniqueID where rrlRMAReceiptID = @ID and rrlPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			DataTable dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
				foreach (DataRow row in dataTable.Rows)
				{
					byte status = 0;
					byte transType = 0;
					serialNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row.Field<string>("sntSerialNumberID"));
					bool flag = row.Field<bool>("sntNegativeTransaction");
					switch (row.Field<byte>("sntTransactionType"))
					{
					case 64:
						status = (byte)(flag ? GetRMAReceiptTransactionInitialStatus(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID")) : 2);
						transType = 2;
						break;
					case 65:
						status = (byte)(flag ? 4 : 5);
						transType = 14;
						break;
					}
					serialNumberDefinition.AddSerialTransaction(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), row.Field<decimal>("sntQuantity"), status, transType, "RMAReceiptLines", row.Field<Guid>("rrlUniqueID"), row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), 0, row.Field<bool>("sntNegativeTransaction"), row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) AS sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, rroUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from RMAReceiptComponents inner join SerialNumberTransactions on rroUniqueID = sntTableUniqueID where rroRMAReceiptID = @ID and rroPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition2 = new SerialNumberDefinition();
				foreach (DataRow row2 in dataTable.Rows)
				{
					byte status2 = 0;
					byte transType2 = 0;
					serialNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row2.Field<string>("sntSerialNumberID"));
					bool flag2 = row2.Field<bool>("sntNegativeTransaction");
					switch (row2.Field<byte>("sntTransactionType"))
					{
					case 64:
						status2 = (byte)(flag2 ? GetRMAReceiptTransactionInitialStatus(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntSerialNumberID")) : 2);
						transType2 = 2;
						break;
					case 65:
						status2 = (byte)(flag2 ? 4 : 5);
						transType2 = 14;
						break;
					}
					serialNumberDefinition2.AddSerialTransaction(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntPartWarehouseLocationID"), row2.Field<string>("sntPartBinID"), row2.Field<string>("sntSerialNumberID"), row2.Field<decimal>("sntQuantity"), status2, transType2, "RMAReceiptComponents", row2.Field<Guid>("rroUniqueID"), row2.Field<string>("sntJobID"), Convert.ToInt32(row2["sntJobAssemblyID"]), Convert.ToInt32(row2["sntJobMaterialID"]), Convert.ToInt32(row2["sntJobMaterialComponentID"]), row2.Field<bool>("sntNegativeTransaction"), row2.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition2.RefreshStatuses(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) AS abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, rrlUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from RMAReceiptLines inner join LotNumberTransactions on rrlUniqueID = abtTableUniqueID where rrlRMAReceiptID = @ID and rrlPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
				foreach (DataRow row3 in dataTable.Rows)
				{
					byte status3 = 0;
					byte transType3 = 0;
					lotNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row3.Field<string>("abtLotNumberID"));
					bool flag3 = row3.Field<bool>("abtNegativeTransaction");
					switch (row3.Field<byte>("abtTransactionType"))
					{
					case 64:
						status3 = (byte)(flag3 ? 4 : 2);
						transType3 = 2;
						break;
					case 65:
						status3 = (byte)(flag3 ? 4 : 5);
						transType3 = 14;
						break;
					}
					lotNumberDefinition.AddLotTransaction(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtPartWarehouseLocationID"), row3.Field<string>("abtPartBinID"), row3.Field<string>("abtLotNumberID"), row3.Field<decimal>("abtQuantity"), status3, transType3, "RMAReceiptLines", row3.Field<Guid>("rrlUniqueID"), row3.Field<string>("abtJobID"), Convert.ToInt32(row3["abtJobAssemblyID"]), Convert.ToInt32(row3["abtJobMaterialID"]), Convert.ToInt32(row3["abtJobMaterialComponentID"]), row3.Field<bool>("abtNegativeTransaction"), row3.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition.RefreshStatuses(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtLotNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) AS abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, rroUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from RMAReceiptComponents inner join LotNumberTransactions on rroUniqueID = abtTableUniqueID where rroRMAReceiptID = @ID and rroPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition2 = new LotNumberDefinition();
				foreach (DataRow row4 in dataTable.Rows)
				{
					byte status4 = 0;
					byte transType4 = 0;
					lotNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row4.Field<string>("abtLotNumberID"));
					bool flag4 = row4.Field<bool>("abtNegativeTransaction");
					switch (row4.Field<byte>("abtTransactionType"))
					{
					case 64:
						status4 = (byte)(flag4 ? 4 : 2);
						transType4 = 2;
						break;
					case 65:
						status4 = (byte)(flag4 ? 4 : 5);
						transType4 = 14;
						break;
					}
					lotNumberDefinition2.AddLotTransaction(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtPartWarehouseLocationID"), row4.Field<string>("abtPartBinID"), row4.Field<string>("abtLotNumberID"), row4.Field<decimal>("abtQuantity"), status4, transType4, "RMAReceiptComponents", row4.Field<Guid>("rroUniqueID"), row4.Field<string>("abtJobID"), Convert.ToInt32(row4["abtJobAssemblyID"]), Convert.ToInt32(row4["abtJobMaterialID"]), Convert.ToInt32(row4["abtJobMaterialComponentID"]), row4.Field<bool>("abtNegativeTransaction"), row4.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition2.RefreshStatuses(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtLotNumberID"));
				}
			}
			M1BindingSource m1BindingSource = bindingSource.PrimaryTable?.GetChildBindingSource("RMAReceiptLines");
			if (m1BindingSource != null && m1BindingSource.Count > 0)
			{
				IntegrationServiceConstants.EntityType entityType = ((!bindingSource.CurrentAsDataRow.Field<bool>("rrpReversalEntry")) ? IntegrationServiceConstants.EntityType.CreditMemo : IntegrationServiceConstants.EntityType.Invoice);
				new M1.Ax.Erp.IntegrationService.IntegrationService().CreateTransactionQueueRecord(database, sqlTransaction, IntegrationServiceConstants.IntegrationType.Financial, IntegrationServiceConstants.ApiAction.Create, entityType, IntegrationServiceConstants.Status.Pending, "RMAReceipts", bindingSource.CurrentAsDataRow.Field<Guid>("rrpUniqueId"), 13);
			}
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	public string CheckRMAReceiptForZeroDollarTotals(M1BindingSource bindingSource)
	{
		if (bindingSource.CurrentAsDataRow == null)
		{
			return string.Empty;
		}
		if (bindingSource.CurrentAsDataRow.Field<bool>("rrpReversalEntry"))
		{
			return string.Empty;
		}
		DataTable dataTable = bindingSource.PrimaryTable.GetChildBindingSource("RMAReceiptLines").GetDataTable();
		if (dataTable == null || dataTable.Rows.Count == 0)
		{
			return string.Empty;
		}
		SqlCommand sqlCommand = bindingSource.Database.NewSqlCommand("select IsNull(ralUnitPriceForeign,0) as ralUnitPriceForeign from RMAReceipts r LEFT JOIN RMAReceiptLines rl ON rl.rrlRMAReceiptID = r.rrpRMAReceiptID  LEFT JOIN RMAClaimLines rcl on rl.rrlRMAClaimID = rcl.ralRMAClaimID and rl.rrlRMAClaimLineID = rcl.ralRMAClaimLineID Where r.rrpRMAReceiptID = @RmaReceiptID");
		sqlCommand.Parameters.Add(new SqlParameter("@RmaReceiptID", SqlDbType.NVarChar)).Value = bindingSource.CurrentAsDataRow.Field<string>("rrpRmaReceiptID");
		if (bindingSource.Database.GetDataTable(sqlCommand).Rows.Cast<DataRow>().Any((DataRow lineRow) => lineRow.Field<decimal>("ralUnitPriceForeign").Equals(0m)))
		{
			return "There are rma receipt lines that either have no linked rma claim line, or have rma claim lines with zero dollar total amounts. If you continue, this will result in a zero dollar credit memo line in your financial package.\n\nDo you wish to continue posting?";
		}
		return string.Empty;
	}

	public bool RMAReceiptPostCheck(M1BindingSource bindingSource)
	{
		bool flag = true;
		M1Database database = bindingSource.Database;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		string msg;
		if (currentAsDataRow.Field<bool>("rrpReversalEntry"))
		{
			if (!string.IsNullOrEmpty(VerifyQuantityForInactiveBins(bindingSource.Database, currentAsDataRow.Field<string>("rrpRMAReceiptID"))))
			{
				return false;
			}
			bool flag2 = (bool)database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
			bool flag3 = (bool)database.Props("IM")["xapIMEnableWarningWhenNegative"];
			DataTable dataTable = database.GetDataTable("Select rrlReverseRMAReceiptID From RMAReceiptLines Where rrlRMAReceiptID = " + M1Util.ConvertToSql(currentAsDataRow.Field<string>("rrpRMAReceiptID")));
			IList<string> list = new List<string>();
			IDictionary<PartInformation, decimal> dictionary = new Dictionary<PartInformation, decimal>(new PartInformationEqualityComparer());
			M1BindingSource childBindingSource = bindingSource.PrimaryTable.GetChildBindingSource("RMAReceiptLines");
			M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("RMAReceiptComponents");
			string fieldPrefix = childBindingSource.PrimaryTable.FieldPrefix;
			string fieldPrefix2 = childBindingSource2.PrimaryTable.FieldPrefix;
			if (dataTable.Rows.Count != 0)
			{
				string o = dataTable.Rows[0].Field<string>("rrlReverseRMAReceiptID");
				foreach (DataRow row in database.GetDataTable("SELECT rrlRMAReceiptID, rrlRMAReceiptLineID, rrlKitPart, rrlPartID, rrlPartRevisionID, rrlPartWarehouseLocationID, \r\n                                                                        rrlPartBinID, rrlUniqueID, rrlInventoryQuantityReceived, rrlQuantityToInspect \r\n                                                                        FROM RMAReceiptLines \r\n                                                                        WHERE rrlRMAReceiptID = " + M1Util.ConvertToSql(o) + " ORDER BY rrlRMAReceiptID").Rows)
				{
					if (row.Field<decimal>("rrlQuantityToInspect") != 0m)
					{
						continue;
					}
					if (row.Field<bool>("rrlKitPart"))
					{
						DataTable dataTable2 = database.GetDataTable("SELECT rroPartID, rroPartRevisionID, rroPartWarehouseLocationID,\r\n                                                                        rroPartBinID, rroUniqueID, rroQuantityReceived \r\n                                                                        FROM RMAReceiptComponents \r\n                                                                        WHERE rroRMAReceiptID = " + M1Util.ConvertToSql(row.Field<string>("rrlRMAReceiptID")) + "\r\n                                                                        AND rroRMAReceiptLineID = " + M1Util.ConvertToSql(row.Field<short>("rrlRMAReceiptLineID")));
						if (dataTable2 != null && dataTable2.Rows.Count != 0)
						{
							foreach (DataRow row2 in dataTable2.Rows)
							{
								decimal num = row2.Field<decimal>("rroQuantityReceived");
								flag = PostCheckingUtility.CheckReceiptToInventory(row2.Field<Guid>("rroUniqueID"), row2.Field<string>("rroPartID"), database, num);
								if (!flag && !flag2)
								{
									break;
								}
								PartInformation key = CreatePartInformation(database, fieldPrefix2, row2);
								if (dictionary.ContainsKey(key))
								{
									dictionary[key] += num;
								}
								else
								{
									dictionary.Add(key, num);
								}
							}
						}
					}
					else
					{
						decimal num = row.Field<decimal>("rrlInventoryQuantityReceived");
						flag = PostCheckingUtility.CheckReceiptToInventory(row.Field<Guid>("rrlUniqueID"), row.Field<string>("rrlPartID"), database, num);
						if (flag2)
						{
							PartInformation key2 = CreatePartInformation(database, fieldPrefix, row);
							if (dictionary.ContainsKey(key2))
							{
								dictionary[key2] += num;
							}
							else
							{
								dictionary.Add(key2, num);
							}
						}
					}
					if (!flag && !flag2)
					{
						break;
					}
				}
			}
			list = VerifyQuantityOnHand(database, dictionary);
			if (flag2)
			{
				if (list.Any())
				{
					if (dictionary.Any((KeyValuePair<PartInformation, decimal> keyValuePair) => keyValuePair.Key.IsSerialLotPart && keyValuePair.Key.HasNegativeQOH))
					{
						MessageBox.Show("This transaction CAN NOT be posted because it will result in a negative quantity on hand for the serial/lot tracked part(s) indicated.\n\n" + string.Join("\n", list), "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
					if (flag3 && MessageBox.Show("This transaction WILL RESULT in a negative quantity on hand for the part(s) indicated. Are you sure?\n\n" + string.Join("\n", list), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
					{
						return false;
					}
				}
				return true;
			}
			if (!flag)
			{
				MessageBox.Show("This RMA Receipt reversal cannot be posted. The received parts have been issued or there is insufficient remaining quantity.\n\n" + string.Join("\n", list), "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
		}
		else if (!VerifyClaimQuantity(bindingSource) || !GetMessageForInactivePartBins(database, currentAsDataRow, out msg))
		{
			flag = false;
		}
		return flag;
	}

	private bool VerifyClaimQuantity(M1BindingSource bindingSource)
	{
		bool result = true;
		List<string> list = new List<string>();
		foreach (DataRow row in bindingSource.PrimaryTable.GetChildBindingSource("RMAReceiptLines").GetDataTable().Rows)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string s = row.Field<string>("rrlRMAClaimID");
			short num = row.Field<short>("rrlRMAClaimLineID");
			string text = "ralRMAClaimID=" + s.ToSql() + " And ralRMAClaimLineID=" + num.ToSql();
			DataTable dataTable = bindingSource.Database.GetDataTable("SELECT ralSalesQuantity, ralRMAClaimID, ralRMAClaimLineID, IsNull((Select Sum(rrlSalesQuantityReceived) From RMAReceiptLines Where rrlRMAClaimID = ralRMAClaimID And rrlRMAClaimLineID = ralRMAClaimLineID),0) As alreadyReceiptedQuantity FROM RMAClaimLines WHERE " + text);
			if (dataTable.Rows.Count > 0 && dataTable.Rows[0].Field<decimal>("ralSalesQuantity") < dataTable.Rows[0].Field<decimal>("alreadyReceiptedQuantity"))
			{
				string text2 = row.Field<string>("rrlPartID");
				string text3 = row.Field<string>("rrlPartRevisionID");
				stringBuilder.AppendLine("[Part: '" + text2 + "', Revision: '" + text3 + "'].");
				list.Add(stringBuilder.ToString());
			}
		}
		if (list.Count > 0)
		{
			result = false;
			MessageBox.Show("This transaction CANNOT be posted because the sum of the Sales Qty Rec'd for all RMA Receipts for the following parts is greater than the Sales Quantity of their RMA Claim: \n\n" + string.Join("\n", list), "RMAReceipts", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		return result;
	}

	public string VerifyQuantityForInactiveBins(M1Database database, string receiptID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (SqlCommand sqlCommand = new SqlCommand("SELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbInactiveBin, rrlInventoryQuantityReceived AS quantity, imbQuantityOnHand FROM PartBins inner join RMAReceiptLines ON imbPartID = rrlPartID and imbPartRevisionID = rrlPartRevisionID and imbWarehouseID = rrlPartWarehouseLocationID and imbPartBinID = rrlPartBinID inner join Parts ON impPartID = imbPartID WHERE rrlRMAReceiptID = " + receiptID.ToSql() + " and imbQuantityOnHand+rrlInventoryQuantityReceived < 0 and imbInactiveBin = 1 and impPhantomOrKitPart = 0 and impNonStockedItem = 0\r\nUNION\r\nSELECT imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbInactiveBin, rroQuantityReceived AS quantity, imbQuantityOnHand FROM PartBins inner join RMAReceiptComponents ON imbPartID = rroPartID and imbPartRevisionID = rroPartRevisionID and imbWarehouseID = rroPartWarehouseLocationID and imbPartBinID = rroPartBinID inner join Parts ON impPartID = imbPartID WHERE rroRMAReceiptID = " + receiptID.ToSql() + " and imbQuantityOnHand+rroQuantityReceived < 0 and imbInactiveBin = 1 and impPhantomOrKitPart = 0"))
		{
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					decimal value = row.Field<decimal>("quantity");
					decimal num = row.Field<decimal>("imbQuantityOnHand");
					string text = row.Field<string>("imbPartID");
					string text2 = row.Field<string>("imbPartRevisionID");
					string text3 = row.Field<string>("imbWarehouseID");
					string text4 = row.Field<string>("imbPartBinID");
					stringBuilder.AppendLine($"[Reversed Inv Qty Rec'd[{Math.Abs(value)}] IS GREATER THAN Quantity on Hand [{num}]");
					stringBuilder.AppendLine("[Part: '" + text + "', Revision: '" + text2 + "', Warehouse: '" + text3 + "', Bin: '" + text4 + "']");
					stringBuilder.AppendLine();
				}
				return stringBuilder.ToString();
			}
		}
		return string.Empty;
	}

	public bool GetMessageForInactivePartBins(M1Database database, DataRow rmaReceiptRow, out string msg)
	{
		msg = "";
		DataTable dataTable = database.GetDataTable("Select * From RMAReceiptLines Inner Join RMAReceipts On rrpRMAReceiptID = rrlRMAReceiptID Where rrlRMAReceiptID = " + M1Util.ConvertToSql(rmaReceiptRow.Field<string>("rrpRMAReceiptID")));
		StringBuilder stringBuilder = new StringBuilder();
		Part part = new Part();
		foreach (DataRow row3 in dataTable.Rows)
		{
			string text = row3.Field<string>("rrlPartID");
			string text2 = row3.Field<string>("rrlPartRevisionID");
			string text3 = row3.Field<string>("rrlPartWarehouseLocationID");
			string text4 = row3.Field<string>("rrlPartBinID");
			if (part.IsPartBinInactive(database, text, text2, text3, text4) && !part.IsPartNonStockedOrKit(database, text) && !row3.Field<bool>("rrpReversalEntry"))
			{
				stringBuilder.AppendLine("[Part: '" + text + "', Revision: '" + text2 + "', Warehouse: '" + text3 + "', Bin: '" + text4 + "' is inactive]");
			}
			if (!row3.Field<bool>("rrlKitPart") || row3.Field<bool>("rrpReversalEntry"))
			{
				continue;
			}
			DataTable dataTable2 = database.GetDataTable("Select * From RMAReceiptComponents Where rroRMAReceiptID = " + M1Util.ConvertToSql(row3.Field<string>("rrlRMAReceiptID")) + " And rroRMAReceiptLineID = " + M1Util.ConvertToSql(row3.Field<short>("rrlRMAReceiptLineID")));
			if (dataTable2 == null || dataTable2.Rows.Count == 0)
			{
				continue;
			}
			foreach (DataRow row4 in dataTable2.Rows)
			{
				string text5 = row4.Field<string>("rroPartID");
				string text6 = row4.Field<string>("rroPartRevisionID");
				string text7 = row4.Field<string>("rroPartWarehouseLocationID");
				string text8 = row4.Field<string>("rroPartBinID");
				if (part.IsPartBinInactive(database, text5, text6, text7, text8))
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

	public decimal GetTotalComponentsCost(M1BindingSource bindingSource, DataRow currentRow)
	{
		decimal result = default(decimal);
		if (bindingSource != null)
		{
			M1BindingSource childBindingSource = bindingSource.PrimaryTable.GetChildBindingSource("RMAReceiptComponents");
			if (childBindingSource != null)
			{
				foreach (DataRow row in childBindingSource.GetDataView(currentRow).ToTable().Rows)
				{
					result += (row.Field<decimal>("rroQuantityReceived") + row.Field<decimal>("rroQuantityToInspect") + row.Field<decimal>("rroAdditionalQuantity")) * row.Field<decimal>("rroUnitCostForeign");
				}
			}
		}
		return result;
	}

	public byte GetRMAReceiptTransactionInitialStatus(M1Database database, SqlTransaction transaction, string partID, string revisionID, string ID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT Top 1 IsNull(sntStatus,0), sntPartWarehouseLocationID, sntPartBinID FROM SerialNumberTransactions WHERE sntPartID = @PartID AND sntPartRevisionID = @PartRevisionID AND sntSerialNumberID = @ID AND sntTransactionType <> 64 AND sntTransactionType <> 2 ORDER BY sntTransactionDate Desc, sntSerialNumberTransactionID Desc");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = ID;
		return Convert.ToByte(database.ExecuteScalar(sqlCommand, transaction));
	}

	public DataTable GetRmaReceiptLinkedShipmentCostsDataTable(M1Database database, SqlTransaction transaction, string sourceTable, string rmaReceiptId, int rmaReceiptLineId, int rmaReceiptComponentId = 0)
	{
		string text = (((PartTransactionDefinition.CostingMethod)database.Props("PN")["xapIMCostingMethod"] == PartTransactionDefinition.CostingMethod.LIFO) ? " DESC" : " ASC");
		DataTable result = new DataTable();
		if (sourceTable.Equals("RMAReceiptLines", StringComparison.InvariantCultureIgnoreCase))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, IsNull(imgPartBinDetailID,1) as imgPartBinDetailID  from RMAReceiptLines inner join RMAClaimLines on rrlRMAClaimID = ralRMAClaimID and rrlRMAClaimLineID = ralRMAClaimLineID  inner join ShipmentLines on ralShipmentID = smlShipmentID and ralShipmentLineID = smlShipmentLineID  inner join PartTransactions on smlUniqueID = imtTableUniqueID inner join PartTransactionCosts on intPartTransactionID = imtPartTransactionID  left join PartBinDetails on intSourceTableUniqueID = imgUniqueID  where rrlRMAReceiptID = @RmaReceiptID and rrlRMAReceiptLineID = @LineID ORDER BY imgTransactionDate " + text + ", imgPartBinDetailID ASC");
			sqlCommand.Parameters.Add(new SqlParameter("@RmaReceiptID", SqlDbType.NVarChar)).Value = rmaReceiptId;
			sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = rmaReceiptLineId;
			result = database.GetDataTable(sqlCommand, transaction);
		}
		else if (sourceTable.Equals("RMAReceiptComponents", StringComparison.InvariantCultureIgnoreCase))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select intCostType, intQuantity, intUnitLaborCost, intUnitOverheadCost, intUnitMaterialCost, intUnitSubcontractCost, intUnitDutyCost, intUnitFreightCost, intUnitMiscCost, imgPartBinDetailID  from RMAReceiptComponents inner join RMAClaimComponents on rroRMAClaimID = raoRMAClaimID and rroRMAClaimLineID = raoRMAClaimLineID and rroRMAClaimComponentID = raoRMAClaimComponentID  inner join ShipmentComponents on raoShipmentID = smoShipmentID and raoShipmentLineID = smoShipmentLineID and raoShipmentComponentID = smoShipmentComponentID  inner join PartTransactions on smoUniqueID = imtTableUniqueID inner join PartTransactionCosts on intPartTransactionID = imtPartTransactionID  inner join PartBinDetails on intSourceTableUniqueID = imgUniqueID  where rroRMAReceiptID = @RMAReceiptID and rroRMAReceiptLineID = @LineID and rroRMAReceiptComponentID = @CompID ORDER BY imgTransactionDate " + text + ", imgPartBinDetailID ASC");
			sqlCommand.Parameters.Add(new SqlParameter("@RmaReceiptID", SqlDbType.NVarChar)).Value = rmaReceiptId;
			sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = rmaReceiptLineId;
			sqlCommand.Parameters.Add(new SqlParameter("@CompID", SqlDbType.Int)).Value = rmaReceiptComponentId;
			result = database.GetDataTable(sqlCommand, transaction);
		}
		return result;
	}

	public PartCost GetPartCostObjectForRmaReceipt(M1Database database, SqlTransaction transaction, DataRow currentDataRow)
	{
		PartCost partCost = new PartCost();
		if (currentDataRow == null)
		{
			return partCost;
		}
		PartTransactionDefinition.CostingMethod costingMethod = (PartTransactionDefinition.CostingMethod)database.Props("PN")["xapIMCostingMethod"];
		if (costingMethod == PartTransactionDefinition.CostingMethod.FIFO)
		{
			costingMethod = PartTransactionDefinition.CostingMethod.LIFO;
		}
		string text;
		string text2;
		if (currentDataRow.Table.Columns.Contains("rroRMAReceiptComponentId"))
		{
			text = "RMAReceiptComponents";
			text2 = "rro";
		}
		else
		{
			if (!currentDataRow.Table.Columns.Contains("rrlRMAReceiptLineId"))
			{
				return partCost;
			}
			text = "RMAReceiptLines";
			text2 = "rrl";
		}
		if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(text2))
		{
			return partCost;
		}
		string rmaReceiptId = currentDataRow.Field<string>(text2 + "RMAReceiptId");
		short rmaReceiptLineId = currentDataRow.Field<short>(text2 + "RMAReceiptLineId");
		int rmaReceiptComponentId = (text.Equals("RMAReceiptComponents", StringComparison.InvariantCultureIgnoreCase) ? currentDataRow.Field<int>(text2 + "RMAReceiptComponentId") : 0);
		DataRow[] array = GetRmaReceiptLinkedShipmentCostsDataTable(database, transaction, text, rmaReceiptId, rmaReceiptLineId, rmaReceiptComponentId).Select($"intCostType = {(byte)costingMethod}");
		if (array.Length == 0)
		{
			return new Part().GetPartCosts(database, transaction, currentDataRow.Field<string>(text2 + "PartId"), currentDataRow.Field<string>(text2 + "PartRevisionId"));
		}
		partCost.CostType = (PartTransactionDefinition.CostType)array[0].Field<byte>("intCostType");
		partCost.LaborCost = array[0].Field<decimal>("intUnitLaborCost");
		partCost.OverheadCost = array[0].Field<decimal>("intUnitOverheadCost");
		partCost.MaterialCost = array[0].Field<decimal>("intUnitMaterialCost");
		partCost.SubcontractCost = array[0].Field<decimal>("intUnitSubcontractCost");
		partCost.DutyCost = array[0].Field<decimal>("intUnitDutyCost");
		partCost.FreightCost = array[0].Field<decimal>("intUnitFreightCost");
		partCost.MiscCost = array[0].Field<decimal>("intUnitMiscCost");
		return partCost;
	}

	private static PartInformation CreatePartInformation(M1Database database, string prefix, DataRow lineRow)
	{
		Part part = new Part();
		if (lineRow != null)
		{
			return new PartInformation
			{
				Part = lineRow.Field<string>(prefix + "PartID").Trim(),
				PartRevision = lineRow.Field<string>(prefix + "PartRevisionID").Trim(),
				PartWarehouse = lineRow.Field<string>(prefix + "PartWarehouseLocationID").Trim(),
				PartBin = lineRow.Field<string>(prefix + "PartBinID").Trim(),
				IsSerialLotPart = part.IsSerialOrLotTracked(database, lineRow.Field<string>(prefix + "PartID").Trim(), null)
			};
		}
		return null;
	}

	private IList<string> VerifyQuantityOnHand(M1Database database, IDictionary<PartInformation, decimal> partsAndQuantities)
	{
		IList<string> list = new List<string>();
		IList<string> list2 = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<PartInformation, decimal> partsAndQuantity in partsAndQuantities)
		{
			stringBuilder.Length = 0;
			SqlCommand sqlCommand = new SqlCommand("Select impNonStockedItem from Parts where impPartID = @partID");
			sqlCommand.Parameters.Add(new SqlParameter("@partID", partsAndQuantity.Key.Part));
			object obj = database.ExecuteScalar(sqlCommand);
			bool value = obj == null || Convert.ToBoolean(obj);
			if (obj == null || Convert.ToBoolean(value))
			{
				continue;
			}
			using SqlCommand sqlCommand2 = new SqlCommand("SELECT ISNULL(imbQuantityOnHand, 0) AS imbQuantityOnHand FROM PartBins WHERE (imbPartID = @PartID) AND (imbPartRevisionID = @PartRevisionID) AND (imbWarehouseID = @WarehouseID) AND (imbPartBinID = @PartBinID)");
			sqlCommand2.Parameters.AddWithValue("@PartID", partsAndQuantity.Key.Part);
			sqlCommand2.Parameters.AddWithValue("@PartRevisionID", partsAndQuantity.Key.PartRevision);
			sqlCommand2.Parameters.AddWithValue("@WarehouseID", partsAndQuantity.Key.PartWarehouse);
			sqlCommand2.Parameters.AddWithValue("@PartBinID", partsAndQuantity.Key.PartBin);
			obj = database.ExecuteScalar(sqlCommand2);
			decimal num = ((obj == null) ? 0m : Convert.ToDecimal(obj));
			if (num - partsAndQuantity.Value < 0m)
			{
				partsAndQuantity.Key.HasNegativeQOH = true;
				if (partsAndQuantity.Key.IsSerialLotPart)
				{
					stringBuilder.AppendLine($"Reversed Inv Qty Rec'd [{partsAndQuantity.Value}] is greater than Quantity On Hand [{num}]\n[Part: '{partsAndQuantity.Key.Part}', Revision: '{partsAndQuantity.Key.PartRevision}', Warehouse: '{partsAndQuantity.Key.PartWarehouse}', Bin: '{partsAndQuantity.Key.PartBin}'].");
					list.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
				else
				{
					stringBuilder.AppendLine($"Reversed Inv Qty Rec'd [{partsAndQuantity.Value}] is greater than Quantity On Hand [{num}]\n[Part: '{partsAndQuantity.Key.Part}', Revision: '{partsAndQuantity.Key.PartRevision}', Warehouse: '{partsAndQuantity.Key.PartWarehouse}', Bin: '{partsAndQuantity.Key.PartBin}'].");
					list2.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
			}
		}
		if (!list.Any())
		{
			return list2;
		}
		return list;
	}
}
