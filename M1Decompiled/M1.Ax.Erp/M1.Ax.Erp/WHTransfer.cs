using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class WHTransfer
{
	private class PartInfo
	{
		public string partID { get; set; }

		public string partRevisionID { get; set; }

		public string sourceWarehouseID { get; set; }

		public string sourcePartBinID { get; set; }

		public decimal shipQuantity { get; set; }
	}

	public void UpdateQuantitiesInGrid(DataRow row, string changedField)
	{
		bool flag = false;
		decimal num = default(decimal);
		if (!row.Table.Columns.Contains("ReceivedComplete") || !row.Table.Columns.Contains("OpenQty") || !row.Table.Columns.Contains("QtyReceived") || !row.Table.Columns.Contains("FieldSelected"))
		{
			return;
		}
		flag = ((!changedField.Equals("FieldSelected")) ? row.Field<bool>("ReceivedComplete") : (row.Field<bool>("FieldSelected") ? true : false));
		num = row.Field<decimal>("OpenQty");
		if (flag)
		{
			if (row.Field<decimal>("QtyReceived") == 0m)
			{
				row.SetField("QtyReceived", num);
			}
		}
		else if (changedField.Equals("FieldSelected"))
		{
			row.SetField("QtyReceived", 0m);
		}
		row.SetField("ReceivedComplete", flag);
	}

	public bool WHTransferPeriodCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DateTime dateTime = default(DateTime);
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		bool result = false;
		if (currentAsDataRow != null)
		{
			if (currentAsDataRow.Table.Columns.Contains("mwpShipDate"))
			{
				dateTime = currentAsDataRow.Field<DateTime>("mwpShipDate");
			}
			if (new Financial().GetYearAndPeriod(database, dateTime.Date, "").Success)
			{
				result = true;
			}
		}
		return result;
	}

	public void PostWHTransfer(M1BindingSource bindingsource)
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
			bindingsource.CurrentAsDataRow.SetField("mwpPosted", value: true);
			string value = bindingsource.CurrentAsDataRow.Field<string>("mwpWarehouseTransferID");
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}
			SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, mwlUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntNegativeTransaction from WarehouseTransferLines inner join SerialNumberTransactions on mwlUniqueID = sntTableUniqueID where mwlWarehouseTransferID = @ID and mwlPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
					if (row.Field<byte>("sntTransactionType") == 66)
					{
						status = (byte)(flag ? 2 : 4);
						transType = 11;
					}
					serialNumberDefinition.AddSerialTransaction(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), row.Field<decimal>("sntQuantity"), status, transType, "WarehouseTransferLines", row.Field<Guid>("mwlUniqueID"), row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), 0, row.Field<bool>("sntNegativeTransaction"), row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, mwoUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from WarehouseTransferComponents inner join SerialNumberTransactions on mwoUniqueID = sntTableUniqueID where mwoWarehouseTransferID = @ID and mwoPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
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
					if (row2.Field<byte>("sntTransactionType") == 66)
					{
						status2 = (byte)(flag2 ? 2 : 4);
						transType2 = 11;
					}
					serialNumberDefinition2.AddSerialTransaction(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntPartWarehouseLocationID"), row2.Field<string>("sntPartBinID"), row2.Field<string>("sntSerialNumberID"), row2.Field<decimal>("sntQuantity"), status2, transType2, "WarehouseTransferComponents", row2.Field<Guid>("mwoUniqueID"), row2.Field<string>("sntJobID"), Convert.ToInt32(row2["sntJobAssemblyID"]), Convert.ToInt32(row2["sntJobMaterialID"]), Convert.ToInt32(row2["sntJobMaterialComponentID"]), row2.Field<bool>("sntNegativeTransaction"), row2.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition2.RefreshStatuses(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, mwlUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from WarehouseTransferLines inner join LotNumberTransactions on mwlUniqueID = abtTableUniqueID where mwlWarehouseTransferID = @ID and mwlPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
					if (row3.Field<byte>("abtTransactionType") == 66)
					{
						status3 = (byte)(flag3 ? 2 : 4);
						transType3 = 11;
					}
					lotNumberDefinition.AddLotTransaction(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtPartWarehouseLocationID"), row3.Field<string>("abtPartBinID"), row3.Field<string>("abtLotNumberID"), row3.Field<decimal>("abtQuantity"), status3, transType3, "WarehouseTransferLines", row3.Field<Guid>("mwlUniqueID"), row3.Field<string>("abtJobID"), Convert.ToInt32(row3["abtJobAssemblyID"]), Convert.ToInt32(row3["abtJobMaterialID"]), Convert.ToInt32(row3["abtJobMaterialComponentID"]), row3.Field<bool>("abtNegativeTransaction"), row3.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition.RefreshStatuses(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtLotNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, mwoUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from WarehouseTransferComponents inner join LotNumberTransactions on mwoUniqueID = abtTableUniqueID where mwoWarehouseTransferID = @ID and mwoPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
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
					if (row4.Field<byte>("abtTransactionType") == 66)
					{
						status4 = (byte)(flag4 ? 2 : 4);
						transType4 = 11;
					}
					lotNumberDefinition2.AddLotTransaction(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtPartWarehouseLocationID"), row4.Field<string>("abtPartBinID"), row4.Field<string>("abtLotNumberID"), row4.Field<decimal>("abtQuantity"), status4, transType4, "WarehouseTransferComponents", row4.Field<Guid>("mwoUniqueID"), row4.Field<string>("abtJobID"), Convert.ToInt32(row4["abtJobAssemblyID"]), Convert.ToInt32(row4["abtJobMaterialID"]), Convert.ToInt32(row4["abtJobMaterialComponentID"]), row4.Field<bool>("abtNegativeTransaction"), row4.Field<DateTime>("abtTransactionDate"));
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

	public bool PostWHTransferCheck(M1BindingSource bindingSource)
	{
		IList<PartInfo> partInfoList = new List<PartInfo>();
		if (bindingSource.CurrentAsDataRow != null && !bindingSource.CurrentAsDataRow.Field<bool>("mwpReversalEntry"))
		{
			M1BindingSource childBindingSource = bindingSource.PrimaryTable.GetChildBindingSource("WarehouseTransferLines");
			DataTable dataTable = childBindingSource.GetDataTable();
			M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("WarehouseTransferComponents");
			string fieldPrefix = childBindingSource.PrimaryTable.FieldPrefix;
			string fieldPrefix2 = childBindingSource2.PrimaryTable.FieldPrefix;
			bool flag = (bool)bindingSource.Database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
			bool flag2 = (bool)bindingSource.Database.Props("IM")["xapIMEnableWarningWhenNegative"];
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				foreach (DataRow row2 in dataTable.Rows)
				{
					if (row2.Field<bool>("mwlKitPart"))
					{
						DataTable dataTable2 = childBindingSource2.GetDataView(row2).ToTable();
						if (dataTable2 == null || dataTable2.Rows.Count == 0)
						{
							continue;
						}
						foreach (DataRow row3 in dataTable2.Rows)
						{
							AddPartInfoToList(partInfoList, fieldPrefix2, row3);
						}
					}
					else
					{
						AddPartInfoToList(partInfoList, fieldPrefix, row2);
					}
				}
				string text = VerifyQuantityAndInactiveBin(bindingSource.Database, bindingSource.CurrentAsDataRow.Field<string>("mwpWarehouseTransferID"));
				if (!string.IsNullOrEmpty(text))
				{
					MessageBox.Show("This transaction CAN NOT be posted because it WILL RESULT in a negative quantity on hand for an INACTIVE bin for the part(s) indicated." + "\n\n" + text, "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}
				IList<string> list = VerifyQuantityOnHandAvailable(partInfoList, childBindingSource);
				if (list.Count > 0)
				{
					if (flag)
					{
						if (flag2)
						{
							if (MessageBox.Show("This transaction WILL RESULT in a negative quantity on hand for the part(s) indicated. Are you sure?\n\n" + string.Join("\n", list), "WarehouseTransfers", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
							{
								return true;
							}
							return false;
						}
						return true;
					}
					MessageBox.Show("This transaction CAN NOT be posted because it will result in a negative quantity on hand for the part(s) indicated.\n\n" + string.Join("\n", list), "WarehouseTransfers", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}
			}
		}
		return true;
	}

	private string VerifyQuantityAndInactiveBin(M1Database database, string mwpWarehouseTransferID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (SqlCommand sqlCommand = new SqlCommand("select imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID,imbInactiveBin, mwlShipQuantity as quantityShipped,imbQuantityOnHand from PartBins inner join WarehouseTransferLines on imbPartID=mwlPartID and imbPartRevisionID=mwlPartRevisionID and imbWarehouseID=mwlSourceWarehouseID and imbPartBinID=mwlSourcePartBinID inner join Parts on impPartID=imbPartID where imbQuantityOnHand<mwlShipQuantity and imbInactiveBin=1 and impPhantomOrKitPart=0 and mwlWarehouseTransferID=" + mwpWarehouseTransferID.ToSql() + "\r\nunion\r\nselect imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID,imbInactiveBin, mwoShipQuantity as quantityShipped,imbQuantityOnHand from PartBins inner join WarehouseTransferComponents on imbPartID=mwoPartID and imbPartRevisionID=mwoPartRevisionID and imbWarehouseID=mwoSourceWarehouseID and imbPartBinID=mwoSourcePartBinID inner join Parts on impPartID=imbPartID where imbQuantityOnHand<mwoShipQuantity and imbInactiveBin=1 and impPhantomOrKitPart=0 and mwoWarehouseTransferID=" + mwpWarehouseTransferID.ToSql()))
		{
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					decimal num = row.Field<decimal>("quantityShipped");
					decimal num2 = row.Field<decimal>("imbQuantityOnHand");
					string text = row.Field<string>("imbPartID");
					string text2 = row.Field<string>("imbPartRevisionID");
					string text3 = row.Field<string>("imbWarehouseID");
					string text4 = row.Field<string>("imbPartBinID");
					stringBuilder.AppendLine($"[Quantity to Transfer [{num}] IS GREATER THAN Quantity on Hand [{num2}]");
					stringBuilder.AppendLine("[Part: '" + text + "', Revision: '" + text2 + "', Warehouse: '" + text3 + "', Bin: '" + text4 + "']");
					stringBuilder.AppendLine();
				}
				return stringBuilder.ToString();
			}
		}
		return string.Empty;
	}

	private IList<string> VerifyQuantityOnHandAvailable(IList<PartInfo> partInfoList, M1BindingSource bindingsource)
	{
		IList<string> list = new List<string>();
		foreach (PartInfo partInfo in partInfoList)
		{
			StringBuilder stringBuilder = new StringBuilder();
			using SqlCommand sqlCommand = new SqlCommand("SELECT ISNULL(imbQuantityOnHand, 0) AS imbQuantityOnHand FROM PartBins WHERE (imbPartID = @PartID) AND (imbPartRevisionID = @PartRevisionID) AND (imbWarehouseID = @WarehouseID) AND (imbPartBinID = @PartBinID) ");
			sqlCommand.Parameters.AddWithValue("@PartID", partInfo.partID);
			sqlCommand.Parameters.AddWithValue("@PartRevisionID", partInfo.partRevisionID);
			sqlCommand.Parameters.AddWithValue("@WarehouseID", partInfo.sourceWarehouseID);
			sqlCommand.Parameters.AddWithValue("@PartBinID", partInfo.sourcePartBinID);
			object obj = bindingsource.Database.ExecuteScalar(sqlCommand);
			decimal num = ((obj == null) ? 0m : Convert.ToDecimal(obj));
			decimal shipQuantity = partInfo.shipQuantity;
			if (num - shipQuantity < 0m)
			{
				stringBuilder.AppendLine("Quantity to transfer [" + $"{shipQuantity}" + "] is greater than Quantity On Hand [" + $"{num}" + "]\n[Part: '" + partInfo.partID + "', Revision: '" + partInfo.partRevisionID + "', Warehouse: '" + partInfo.sourceWarehouseID + "' ,Bin: '" + partInfo.sourcePartBinID + "'].");
				list.Add(stringBuilder.ToString());
			}
		}
		return list;
	}

	private void AddPartInfoToList(IList<PartInfo> partInfoList, string prefix, DataRow row)
	{
		PartInfo partInfo = partInfoList.Where((PartInfo part) => part.partID.Equals(row.Field<string>(prefix + "PartID"), StringComparison.CurrentCultureIgnoreCase) && part.partRevisionID.Equals(row.Field<string>(prefix + "PartRevisionID"), StringComparison.CurrentCultureIgnoreCase) && part.sourceWarehouseID.Equals(row.Field<string>(prefix + "SourceWarehouseID"), StringComparison.CurrentCultureIgnoreCase) && part.sourcePartBinID.Equals(row.Field<string>(prefix + "SourcePartBinID"), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
		if (partInfo == null)
		{
			PartInfo item = new PartInfo
			{
				partID = row.Field<string>(prefix + "PartID"),
				partRevisionID = row.Field<string>(prefix + "PartRevisionID"),
				sourceWarehouseID = row.Field<string>(prefix + "SourceWarehouseID"),
				sourcePartBinID = row.Field<string>(prefix + "SourcePartBinID"),
				shipQuantity = row.Field<decimal>(prefix + "ShipQuantity")
			};
			partInfoList.Add(item);
		}
		else
		{
			partInfo.shipQuantity += row.Field<decimal>(prefix + "ShipQuantity");
		}
	}
}
