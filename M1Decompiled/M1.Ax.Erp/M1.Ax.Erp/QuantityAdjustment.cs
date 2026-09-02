using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class QuantityAdjustment
{
	private class PartBinRecordKey
	{
		public string PartID { get; set; }

		public string PartRevision { get; set; }

		public string PartWHouse { get; set; }

		public string PartWHBin { get; set; }
	}

	private string IsQuantityOnHandNegative(M1BindingSource bindingsource, KeyValuePair<PartBinRecordKey, decimal> element)
	{
		bool flag = (bool)bindingsource.Database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
		using (SqlCommand sqlCommand = new SqlCommand("SELECT ISNULL(imbQuantityOnHand, 0) AS imbQuantityOnHand FROM PartBins WHERE (imbPartID = @PartID) AND (imbPartRevisionID = @PartRevisionID) AND (imbWarehouseID = @WarehouseID) AND (imbPartBinID = @PartBinID) " + (flag ? string.Empty : " AND (imbQuantityOnHand > 0)")))
		{
			decimal num = default(decimal);
			sqlCommand.Parameters.AddWithValue("@PartID", element.Key.PartID);
			sqlCommand.Parameters.AddWithValue("@PartRevisionID", element.Key.PartRevision);
			sqlCommand.Parameters.AddWithValue("@WarehouseID", element.Key.PartWHouse);
			sqlCommand.Parameters.AddWithValue("@PartBinID", element.Key.PartWHBin);
			object obj = bindingsource.Database.ExecuteScalar(sqlCommand);
			num = ((obj == null) ? 0m : Convert.ToDecimal(obj));
			if (num - element.Value < 0m)
			{
				return $"Quantity to Transfer [{element.Value}] is greater than Quantity On Hand [{num}] [Part: '{element.Key.PartID}', Revision: '{element.Key.PartRevision}', Warehouse: '{element.Key.PartWHouse}' ,Bin: '{element.Key.PartWHBin}'].";
			}
		}
		return string.Empty;
	}

	private void PostSerialLotNumbersBinTransfer(M1Database database, SqlTransaction transaction, string ID, string destWHID, string destBinID)
	{
		if (string.IsNullOrWhiteSpace(ID))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, inqUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from QuantityAdjustments inner join SerialNumberTransactions on inqUniqueID = sntTableUniqueID where inqQuantityAdjustmentID = @ID and inqPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = ID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
			foreach (DataRow row in dataTable.Rows)
			{
				byte status = 0;
				byte transType = 0;
				serialNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row.Field<string>("sntSerialNumberID"));
				if (row.Field<byte>("sntTransactionType") == 8)
				{
					status = 2;
					transType = 26;
				}
				serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), destWHID, destBinID, row.Field<string>("sntSerialNumberID"), row.Field<decimal>("sntQuantity"), status, transType, "QuantityAdjustments", row.Field<Guid>("inqUniqueID"), row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), row.Field<bool>("sntNegativeTransaction"), row.Field<DateTime>("sntTransactionDate"));
				serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, inqUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from QuantityAdjustments inner join LotNumberTransactions on inqUniqueID = abtTableUniqueID where inqQuantityAdjustmentID = @ID and inqPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = ID;
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
		foreach (DataRow row2 in dataTable.Rows)
		{
			byte status2 = 0;
			byte transType2 = 0;
			lotNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row2.Field<string>("abtLotNumberID"));
			if (row2.Field<byte>("abtTransactionType") == 8)
			{
				status2 = 2;
				transType2 = 26;
			}
			lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), destWHID, destBinID, row2.Field<string>("abtLotNumberID"), row2.Field<decimal>("abtQuantity"), status2, transType2, "QUANTITYADJUSTMENTS", row2.Field<Guid>("inqUniqueID"), row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), row2.Field<bool>("abtNegativeTransaction"), row2.Field<DateTime>("abtTransactionDate"));
			lotNumberDefinition.RefreshStatuses(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtLotNumberID"));
		}
	}

	private void PostSerialLotNumbersQtyOnHand(M1Database database, SqlTransaction transaction, string ID)
	{
		if (string.IsNullOrWhiteSpace(ID))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, inqUniqueID, inqChangeQuantity, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from QuantityAdjustments inner join SerialNumberTransactions on inqUniqueID = sntTableUniqueID where inqQuantityAdjustmentID = @ID and inqPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = ID;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
			foreach (DataRow row2 in dataTable.Rows)
			{
				byte status = 0;
				byte transType = 0;
				serialNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row2.Field<string>("sntSerialNumberID"));
				if (row2.Field<byte>("sntTransactionType") == 7)
				{
					status = (byte)((!serialNumberDefinition.IsNegativeTransaction(row2.Field<decimal>("inqChangeQuantity"))) ? 2 : 0);
					transType = 46;
				}
				serialNumberDefinition.AddSerialTransaction(database, transaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntPartWarehouseLocationID"), row2.Field<string>("sntPartBinID"), row2.Field<string>("sntSerialNumberID"), row2.Field<decimal>("sntQuantity"), status, transType, "QuantityAdjustments", row2.Field<Guid>("inqUniqueID"), row2.Field<string>("sntJobID"), Convert.ToInt32(row2["sntJobAssemblyID"]), Convert.ToInt32(row2["sntJobMaterialID"]), Convert.ToInt32(row2["sntJobMaterialComponentID"]), row2.Field<bool>("sntNegativeTransaction"), row2.Field<DateTime>("sntTransactionDate"));
				serialNumberDefinition.RefreshStatuses(database, transaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntSerialNumberID"));
			}
		}
		sqlCommand = database.NewSqlCommand("select absLotNumberID, absPartID, absPartRevisionID, absPartWarehouseLocationID, absPartBinID, absQuantity, inqUniqueID from QuantityAdjustments inner join LotNumberStatuses on absPartID = inqPartID And absPartRevisionID = inqPartRevisionID And absPartWarehouseLocationID = inqPartWarehouseLocationID And absPartBinID = inqPartBinID inner join LotNumbers on absLotNumberID = ablLotNumberID and absPartID = ablPartID and absPartRevisionID = ablPartRevisionID where inqQuantityAdjustmentID = @ID and inqPosted = 0 And absStatus = 2 And absQuantity <> 0 and absPartID+absPartRevisionID+absPartWarehouseLocationID+absPartBinID+absLotNumberID Not In (Select abtPartID+abtPartRevisionID+abtPartWarehouseLocationID+abtPartBinID+abtLotNumberID From LotNumberTransactions Where inqUniqueID = abtTableUniqueID And abtTransactionType <> 19 And abtTransactionType = 7) and absPartID+absPartRevisionID+absPartWarehouseLocationID+absPartBinID+absLotNumberID Not In (Select abtPartID+abtPartRevisionID+abtLotNumberID From LotNumberTransactions Where abtTableUniqueID <> inqUniqueID And  abtTransactionDate > inqAdjustmentDate) order by absLotNumberID, absPartID, absPartRevisionID, absPartWarehouseLocationID, absPartBinID");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = ID;
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
			foreach (DataRow row3 in dataTable.Rows)
			{
				lotNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row3.Field<string>("absLotNumberID"));
				byte status2 = 0;
				byte transType2 = 46;
				lotNumberDefinition.AddLotTransaction(database, transaction, row3.Field<string>("absPartID"), row3.Field<string>("absPartRevisionID"), row3.Field<string>("absPartWarehouseLocationID"), row3.Field<string>("absPartBinID"), row3.Field<string>("absLotNumberID"), 0m, status2, transType2, "QuantityAdjustments", row3.Field<Guid>("inqUniqueID"), string.Empty, 0, 0, 0, negativeTrans: false);
				lotNumberDefinition.RefreshStatuses(database, transaction, row3.Field<string>("absPartID"), row3.Field<string>("absPartRevisionID"), row3.Field<string>("absLotNumberID"));
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, inqUniqueID, inqChangeQuantity, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from QuantityAdjustments inner join LotNumberTransactions on inqUniqueID = abtTableUniqueID where inqQuantityAdjustmentID = @ID and inqPosted = 0 and abtTransactionType = 7 and abtPartID+abtPartRevisionID+abtPartWarehouseLocationID+abtPartBinID+abtLotNumberID Not In (Select abtPartID+abtPartRevisionID+abtPartWarehouseLocationID+abtPartBinID+abtLotNumberID From LotNumberTransactions Where abtTableUniqueID <> inqUniqueID And abtTransactionType <> 19 And abtTransactionDate > inqAdjustmentDate) order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = ID;
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		LotNumberDefinition lotNumberDefinition2 = new LotNumberDefinition();
		foreach (DataRow row4 in dataTable.Rows)
		{
			byte status3 = 0;
			byte transType3 = 0;
			lotNumberDefinition2.LoadLotOrSerialNumbers(database, transaction, row4.Field<string>("abtLotNumberID"));
			if (row4.Field<byte>("abtTransactionType") == 7)
			{
				status3 = (byte)((!lotNumberDefinition2.IsNegativeTransaction(row4.Field<decimal>("inqChangeQuantity"))) ? 2 : 0);
				transType3 = 46;
			}
			lotNumberDefinition2.AddLotTransaction(database, transaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtPartWarehouseLocationID"), row4.Field<string>("abtPartBinID"), row4.Field<string>("abtLotNumberID"), row4.Field<decimal>("abtQuantity"), status3, transType3, "QuantityAdjustments", row4.Field<Guid>("inqUniqueID"), row4.Field<string>("abtJobID"), Convert.ToInt32(row4["abtJobAssemblyID"]), Convert.ToInt32(row4["abtJobMaterialID"]), Convert.ToInt32(row4["abtJobMaterialComponentID"]), row4.Field<bool>("abtNegativeTransaction"), row4.Field<DateTime>("abtTransactionDate"));
			lotNumberDefinition2.RefreshStatuses(database, transaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtLotNumberID"));
		}
	}

	public string PostQuantityAdjustmentCheck(M1BindingSource bindingsource)
	{
		IDictionary<PartBinRecordKey, decimal> dictionary = new Dictionary<PartBinRecordKey, decimal>();
		if (bindingsource.CurrentAsDataRow != null && bindingsource.CurrentAsDataRow.Field<byte>("inqAdjustmentType") == 2)
		{
			decimal value = bindingsource.CurrentAsDataRow.Field<decimal>("inqBinQuantityTransferred");
			PartBinRecordKey key = new PartBinRecordKey
			{
				PartID = bindingsource.CurrentAsDataRow.Field<string>("inqPartID").Trim(),
				PartRevision = bindingsource.CurrentAsDataRow.Field<string>("inqPartRevisionID").Trim(),
				PartWHouse = bindingsource.CurrentAsDataRow.Field<string>("inqPartWarehouseLocationID").Trim(),
				PartWHBin = bindingsource.CurrentAsDataRow.Field<string>("inqPartBinID").Trim()
			};
			dictionary.Add(key, value);
			if (dictionary.Count > 0)
			{
				foreach (KeyValuePair<PartBinRecordKey, decimal> item in dictionary)
				{
					string text = IsQuantityOnHandNegative(bindingsource, item);
					if (!string.IsNullOrEmpty(text))
					{
						return text;
					}
				}
			}
		}
		return string.Empty;
	}

	public bool QuantityAdjustmentPeriodCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DateTime dateTime = default(DateTime);
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		bool result = false;
		if (currentAsDataRow != null)
		{
			if (currentAsDataRow.Table.Columns.Contains("inqAdjustmentDate"))
			{
				dateTime = currentAsDataRow.Field<DateTime>("inqAdjustmentDate");
			}
			if (new Financial().GetYearAndPeriod(database, dateTime.Date, "").Success)
			{
				result = true;
			}
		}
		return result;
	}

	public void PostQuantityAdjustment(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		SqlTransaction sqlTransaction = bindingSource.Transaction;
		bool flag = true;
		if (sqlTransaction == null)
		{
			sqlTransaction = database.BeginTransaction();
			flag = false;
		}
		try
		{
			if (bindingSource.CurrentAsDataRow == null)
			{
				return;
			}
			string text = bindingSource.CurrentAsDataRow.Field<string>("inqQuantityAdjustmentID");
			string destWHID = bindingSource.CurrentAsDataRow.Field<string>("inqDestinationWarehouseID");
			string destBinID = bindingSource.CurrentAsDataRow.Field<string>("inqDestinationPartBinID");
			byte b = bindingSource.CurrentAsDataRow.Field<byte>("inqAdjustmentType");
			if (b != 1 || new Part().CheckPendingTransactions(bindingSource.CurrentDatabase, bindingSource.CurrentAsDataRow.Field<string>("inqPartID").Trim(), bindingSource.CurrentAsDataRow.Field<string>("inqPartRevisionID").Trim(), text))
			{
				if (b == 2)
				{
					PostSerialLotNumbersBinTransfer(database, sqlTransaction, text, destWHID, destBinID);
				}
				else
				{
					PostSerialLotNumbersQtyOnHand(database, sqlTransaction, text);
				}
				bindingSource.CurrentAsDataRow.SetField("inqPosted", value: true);
				if (!flag)
				{
					database.CommitTransaction(sqlTransaction);
				}
				bindingSource.SaveData();
				if (flag)
				{
					database.CommitTransaction(sqlTransaction);
				}
				database.OnTableChanged(new TableChangedEventArgs("PartRevisions", null, null, null));
				database.OnTableChanged("Warehouses");
			}
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			bindingSource.CurrentAsDataRow?.SetField("inqPosted", value: false);
			throw;
		}
	}

	public void OpenBinTransfer(M1Database _Database)
	{
		(_Database.GetService(typeof(IOpenObject)) as IOpenObject).OpenObject("QuantityAdjustment", null, string.Empty, newForm: false, string.Empty, null, new object[2] { "inqAdjustmentType", 2 });
	}
}
