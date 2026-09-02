using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M1.Core;

namespace M1.Ax.Erp;

public class WHReceipt
{
	private class PartInfo
	{
		public string partID { get; set; }

		public string partRevisionID { get; set; }

		public string sourceWarehouseID { get; set; }

		public string sourcePartBinID { get; set; }

		public string destinationWarehouseID { get; set; }

		public string destinationPartBinID { get; set; }

		public bool isDestinationPartBinInactive { get; set; }

		public decimal shippedQuantity { get; set; }

		public bool reversalReceipt { get; set; }

		public bool isSerialOrLotTracked { get; set; }

		public string negativeQuantityOnHandWarningMessage { get; set; }

		public string ReverseWHReceiptID { get; set; }

		public short ReverseWHReceiptLineID { get; set; }

		public bool isKitPartComponent { get; set; }
	}

	public bool WHReceiptPeriodCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DateTime dateTime = default(DateTime);
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		bool result = false;
		if (currentAsDataRow != null)
		{
			if (currentAsDataRow.Table.Columns.Contains("wrpReceiptDate"))
			{
				dateTime = currentAsDataRow.Field<DateTime>("wrpReceiptDate");
			}
			if (new Financial().GetYearAndPeriod(database, dateTime.Date, "").Success)
			{
				result = true;
			}
		}
		return result;
	}

	public void PostWHReceipt(M1BindingSource bindingsource)
	{
		M1Database database = bindingsource.Database;
		SqlTransaction sqlTransaction = bindingsource.Transaction;
		if (sqlTransaction == null)
		{
			sqlTransaction = database.BeginTransaction();
		}
		try
		{
			if (bindingsource.CurrentAsDataRow == null)
			{
				return;
			}
			bindingsource.CurrentAsDataRow.SetField("wrpPosted", value: true);
			string value = bindingsource.CurrentAsDataRow.Field<string>("wrpWarehouseReceiptID");
			string warehouseID = string.Empty;
			string binID = string.Empty;
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}
			SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, wrlUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntNegativeTransaction from WarehouseReceiptLines inner join SerialNumberTransactions on wrlUniqueID = sntTableUniqueID where wrlWarehouseReceiptID = @ID and wrlPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
					if (row.Field<byte>("sntTransactionType") == 67)
					{
						if (flag)
						{
							status = 4;
							DataRow wHReceiptData = GetWHReceiptData(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
							if (wHReceiptData != null)
							{
								warehouseID = wHReceiptData.Field<string>("sntPartWarehouseLocationID");
								binID = wHReceiptData.Field<string>("sntPartBinID");
							}
						}
						else
						{
							status = 2;
							warehouseID = row.Field<string>("sntPartWarehouseLocationID");
							binID = row.Field<string>("sntPartBinID");
						}
						transType = 12;
					}
					serialNumberDefinition.AddSerialTransaction(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), warehouseID, binID, row.Field<string>("sntSerialNumberID"), row.Field<decimal>("sntQuantity"), status, transType, "WarehouseReceiptLines", row.Field<Guid>("wrlUniqueID"), row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), 0, row.Field<bool>("sntNegativeTransaction"), row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, wroUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from WarehouseReceiptComponents inner join SerialNumberTransactions on wroUniqueID = sntTableUniqueID where wroWarehouseReceiptID = @ID and wroPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
					if (row2.Field<byte>("sntTransactionType") == 67)
					{
						if (flag2)
						{
							status2 = 4;
							DataRow wHReceiptData2 = GetWHReceiptData(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntSerialNumberID"));
							if (wHReceiptData2 != null)
							{
								warehouseID = wHReceiptData2.Field<string>("sntPartWarehouseLocationID");
								binID = wHReceiptData2.Field<string>("sntPartBinID");
							}
						}
						else
						{
							status2 = 2;
							warehouseID = row2.Field<string>("sntPartWarehouseLocationID");
							binID = row2.Field<string>("sntPartBinID");
						}
						transType2 = 12;
					}
					serialNumberDefinition2.AddSerialTransaction(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), warehouseID, binID, row2.Field<string>("sntSerialNumberID"), row2.Field<decimal>("sntQuantity"), status2, transType2, "WarehouseReceiptComponents", row2.Field<Guid>("wroUniqueID"), row2.Field<string>("sntJobID"), Convert.ToInt32(row2["sntJobAssemblyID"]), Convert.ToInt32(row2["sntJobMaterialID"]), Convert.ToInt32(row2["sntJobMaterialComponentID"]), row2.Field<bool>("sntNegativeTransaction"), row2.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition2.RefreshStatuses(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, wrlUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from WarehouseReceiptLines inner join LotNumberTransactions on wrlUniqueID = abtTableUniqueID where wrlWarehouseReceiptID = @ID and wrlPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
					if (row3.Field<byte>("abtTransactionType") == 67)
					{
						status3 = (byte)(flag3 ? 2 : 2);
						transType3 = 12;
					}
					lotNumberDefinition.AddLotTransaction(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtPartWarehouseLocationID"), row3.Field<string>("abtPartBinID"), row3.Field<string>("abtLotNumberID"), row3.Field<decimal>("abtQuantity"), status3, transType3, "WarehouseReceiptLines", row3.Field<Guid>("wrlUniqueID"), row3.Field<string>("abtJobID"), Convert.ToInt32(row3["abtJobAssemblyID"]), Convert.ToInt32(row3["abtJobMaterialID"]), Convert.ToInt32(row3["abtJobMaterialComponentID"]), row3.Field<bool>("abtNegativeTransaction"), row3.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition.RefreshStatuses(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtLotNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, wroUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from WarehouseReceiptComponents inner join LotNumberTransactions on wroUniqueID = abtTableUniqueID where wroWarehouseReceiptID = @ID and wroPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
					if (row4.Field<byte>("abtTransactionType") == 67)
					{
						status4 = (byte)(flag4 ? 2 : 2);
						transType4 = 12;
					}
					lotNumberDefinition2.AddLotTransaction(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtPartWarehouseLocationID"), row4.Field<string>("abtPartBinID"), row4.Field<string>("abtLotNumberID"), row4.Field<decimal>("abtQuantity"), status4, transType4, "WarehouseReceiptComponents", row4.Field<Guid>("wroUniqueID"), row4.Field<string>("abtJobID"), Convert.ToInt32(row4["abtJobAssemblyID"]), Convert.ToInt32(row4["abtJobMaterialID"]), Convert.ToInt32(row4["abtJobMaterialComponentID"]), row4.Field<bool>("abtNegativeTransaction"), row4.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition2.RefreshStatuses(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtLotNumberID"));
				}
			}
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	public bool PostWHReceiptCheck(M1BindingSource bindingSource)
	{
		IList<PartInfo> list = new List<PartInfo>();
		if (bindingSource.CurrentAsDataRow != null)
		{
			M1BindingSource childBindingSource = bindingSource.PrimaryTable.GetChildBindingSource("WarehouseReceiptLines");
			DataTable dataTable = childBindingSource.GetDataTable();
			M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("WarehouseReceiptComponents");
			string fieldPrefix = childBindingSource.PrimaryTable.FieldPrefix;
			string fieldPrefix2 = childBindingSource2.PrimaryTable.FieldPrefix;
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				foreach (DataRow row2 in dataTable.Rows)
				{
					if (row2.Field<bool>("wrlKitPart"))
					{
						DataTable dataTable2 = childBindingSource2.GetDataView(row2).ToTable();
						if (dataTable2 == null || dataTable2.Rows.Count == 0)
						{
							continue;
						}
						foreach (DataRow row3 in dataTable2.Rows)
						{
							AddPartInfoToList(bindingSource.Database, list, fieldPrefix2, row3);
						}
					}
					else
					{
						AddPartInfoToList(bindingSource.Database, list, fieldPrefix, row2);
					}
				}
				if (bindingSource.CurrentAsDataRow.Field<bool>("wrpReversalEntry"))
				{
					bool flag = (bool)bindingSource.Database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
					bool flag2 = (bool)bindingSource.Database.Props("IM")["xapIMEnableWarningWhenNegative"];
					IList<string> list2 = VerifyQuantityOnHandAvailable(list, childBindingSource);
					if (list2.Count > 0)
					{
						StringBuilder stringBuilder = new StringBuilder();
						foreach (PartInfo item in list)
						{
							if (item.isDestinationPartBinInactive && !string.IsNullOrEmpty(item.negativeQuantityOnHandWarningMessage))
							{
								stringBuilder.AppendLine(item.negativeQuantityOnHandWarningMessage);
							}
						}
						if (stringBuilder.Length > 0)
						{
							MessageBox.Show("This transaction CAN NOT be posted because it will result in a negative quantity on hand for an INACTIVE bin for the part(s) indicated.\n\n" + string.Join("\n", stringBuilder), "WarehouseReceipts", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return false;
						}
						if (flag)
						{
							IList<string> list3 = VerifySerialOrLotTrackedPartQuantityOnHand(list);
							if (list3.Count > 0)
							{
								MessageBox.Show("This transaction CAN NOT be posted because it will result in a negative quantity on hand for the serial/lot tracked part(s) indicated.\n\n" + string.Join("\n", list3), "WarehouseReceipts", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								return false;
							}
							if (flag2)
							{
								if (MessageBox.Show("This transaction WILL RESULT in a negative quantity on hand for the part(s) indicated. Are you sure?\n\n" + string.Join("\n", list2), "WarehouseReceipts", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
								{
									return true;
								}
								return false;
							}
							return true;
						}
						MessageBox.Show("This transaction CAN NOT be posted because it will result in a negative quantity on hand for the part(s) indicated.\n\n" + string.Join("\n", list2), "WarehouseReceipts", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
				}
				else
				{
					IList<string> list4 = VerifyPartBinInactive(list);
					if (list4.Count > 0)
					{
						MessageBox.Show("This transaction CAN NOT be posted because an INACTIVE bin exists for the part(s) indicated.\n\n" + string.Join("\n", list4), "WarehouseReceipts", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return false;
					}
				}
			}
		}
		return true;
	}

	public DataRow GetWHReceiptData(M1Database database, SqlTransaction transaction, string partID, string revisionID, string ID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT Top 1 IsNull(sntStatus,0), sntPartWarehouseLocationID, sntPartBinID FROM SerialNumberTransactions WHERE sntPartID = @PartID AND sntPartRevisionID = @PartRevisionID AND sntSerialNumberID = @ID AND sntTransactionType = 11 ORDER BY sntTransactionDate Desc, sntSerialNumberTransactionID Desc");
		sqlCommand.Parameters.Add(new SqlParameter("@PartID", SqlDbType.NVarChar)).Value = partID;
		sqlCommand.Parameters.Add(new SqlParameter("@PartRevisionID", SqlDbType.NVarChar)).Value = revisionID;
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = ID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			return dataTable.Rows[0];
		}
		return null;
	}

	private IList<string> VerifyQuantityOnHandAvailable(IList<PartInfo> partInfoList, M1BindingSource bindingsource)
	{
		Part part = new Part();
		IList<string> list = new List<string>();
		foreach (PartInfo partInfo in partInfoList)
		{
			partInfo.isSerialOrLotTracked = part.IsSerialOrLotTracked(bindingsource.Database, partInfo.partID, null);
			StringBuilder stringBuilder = new StringBuilder();
			using SqlCommand sqlCommand = new SqlCommand("SELECT ISNULL(imbQuantityOnHand, 0) AS imbQuantityOnHand FROM PartBins WHERE (imbPartID = @PartID) AND (imbPartRevisionID = @PartRevisionID) AND (imbWarehouseID = @WarehouseID) AND (imbPartBinID = @PartBinID) ");
			sqlCommand.Parameters.AddWithValue("@PartID", partInfo.partID);
			sqlCommand.Parameters.AddWithValue("@PartRevisionID", partInfo.partRevisionID);
			sqlCommand.Parameters.AddWithValue("@WarehouseID", partInfo.destinationWarehouseID);
			sqlCommand.Parameters.AddWithValue("@PartBinID", partInfo.destinationPartBinID);
			object obj = bindingsource.Database.ExecuteScalar(sqlCommand);
			decimal num = ((obj == null) ? 0m : Convert.ToDecimal(obj));
			decimal num2 = (partInfo.isKitPartComponent ? partInfo.shippedQuantity : GetPreviousQuantityReceived(partInfo, bindingsource));
			if (partInfo.reversalReceipt && num - num2 < 0m)
			{
				stringBuilder.AppendLine("Reversed Qty Rec'd [" + $"{partInfo.shippedQuantity:0.00000}" + "] IS GREATER THAN Quantity On Hand [" + $"{num}" + "]\n[Part: '" + partInfo.partID + "', Revision: '" + partInfo.partRevisionID + "', Warehouse: '" + partInfo.destinationWarehouseID + "' ,Bin: '" + partInfo.destinationPartBinID + "'].");
				list.Add(stringBuilder.ToString());
				partInfo.negativeQuantityOnHandWarningMessage = stringBuilder.ToString();
			}
		}
		return list;
	}

	private IList<string> VerifySerialOrLotTrackedPartQuantityOnHand(IList<PartInfo> partInfoList)
	{
		IList<string> list = new List<string>();
		foreach (PartInfo partInfo in partInfoList)
		{
			if (partInfo.isSerialOrLotTracked && !string.IsNullOrEmpty(partInfo.negativeQuantityOnHandWarningMessage))
			{
				list.Add(partInfo.negativeQuantityOnHandWarningMessage);
			}
		}
		return list;
	}

	private IList<string> VerifyPartBinInactive(IList<PartInfo> partInfoList)
	{
		IList<string> list = new List<string>();
		foreach (PartInfo partInfo in partInfoList)
		{
			if (partInfo.isDestinationPartBinInactive)
			{
				list.Add("Part: '" + partInfo.partID + "', Revision: '" + partInfo.partRevisionID + "', Warehouse: '" + partInfo.destinationWarehouseID + "', Bin: '" + partInfo.destinationPartBinID + "' is inactive");
			}
		}
		return list;
	}

	private void AddPartInfoToList(M1Database database, IList<PartInfo> partInfoList, string prefix, DataRow row)
	{
		PartInfo partInfo = partInfoList.Where((PartInfo partInfo3) => partInfo3.partID.Equals(row.Field<string>(prefix + "PartID"), StringComparison.CurrentCultureIgnoreCase) && partInfo3.partRevisionID.Equals(row.Field<string>(prefix + "PartRevisionID"), StringComparison.CurrentCultureIgnoreCase) && partInfo3.sourceWarehouseID.Equals(row.Field<string>(prefix + "SourceWarehouseID"), StringComparison.CurrentCultureIgnoreCase) && partInfo3.sourcePartBinID.Equals(row.Field<string>(prefix + "SourcePartBinID"), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
		if (partInfo == null)
		{
			PartInfo partInfo2 = new PartInfo
			{
				partID = row.Field<string>(prefix + "PartID"),
				partRevisionID = row.Field<string>(prefix + "PartRevisionID"),
				sourceWarehouseID = row.Field<string>(prefix + "SourceWarehouseID"),
				sourcePartBinID = row.Field<string>(prefix + "SourcePartBinID"),
				destinationWarehouseID = row.Field<string>(prefix + "DestinationWarehouseID"),
				destinationPartBinID = row.Field<string>(prefix + "DestinationPartBinID"),
				shippedQuantity = ((prefix == "wrl") ? row.Field<decimal>(prefix + "WTShippedQuantity") : row.Field<decimal>("wroParentQuantity")),
				reversalReceipt = !string.IsNullOrEmpty(row.Field<string>(prefix + "ReverseWHReceiptID")),
				ReverseWHReceiptID = row.Field<string>(prefix + "ReverseWHReceiptID"),
				ReverseWHReceiptLineID = row.Field<short>(prefix + "ReverseWHReceiptLineID"),
				isKitPartComponent = (prefix == "wro")
			};
			Part part = new Part();
			partInfo2.isDestinationPartBinInactive = part.IsPartBinInactive(database, partInfo2.partID, partInfo2.partRevisionID, partInfo2.destinationWarehouseID, partInfo2.destinationPartBinID);
			partInfoList.Add(partInfo2);
		}
		else
		{
			partInfo.shippedQuantity += ((prefix == "wrl") ? row.Field<decimal>(prefix + "WTShippedQuantity") : row.Field<decimal>("wroParentQuantity"));
		}
	}

	private decimal GetPreviousQuantityReceived(PartInfo partInfo, M1BindingSource bindingsource)
	{
		using SqlCommand sqlCommand = new SqlCommand("SELECT ISNULL(wrlQuantityReceived, 0) AS wrlQuantityReceived FROM WarehouseReceiptLines WHERE wrlWarehouseReceiptID = @WarehouseReceiptID AND wrlWarehouseReceiptLineID = @WarehouseReceiptLineID AND wrlPartID = @PartID AND wrlPartRevisionID = @PartRevisionID AND wrlDestinationWarehouseID = @WarehouseID AND wrlDestinationPartBinID = @PartBinID AND wrlPosted = '1'");
		sqlCommand.Parameters.AddWithValue("@WarehouseReceiptID", partInfo.ReverseWHReceiptID);
		sqlCommand.Parameters.AddWithValue("@WarehouseReceiptLineID", partInfo.ReverseWHReceiptLineID);
		sqlCommand.Parameters.AddWithValue("@PartID", partInfo.partID);
		sqlCommand.Parameters.AddWithValue("@PartRevisionID", partInfo.partRevisionID);
		sqlCommand.Parameters.AddWithValue("@WarehouseID", partInfo.destinationWarehouseID);
		sqlCommand.Parameters.AddWithValue("@PartBinID", partInfo.destinationPartBinID);
		object obj = bindingsource.Database.ExecuteScalar(sqlCommand);
		return (obj == null) ? 0m : Convert.ToDecimal(obj);
	}
}
