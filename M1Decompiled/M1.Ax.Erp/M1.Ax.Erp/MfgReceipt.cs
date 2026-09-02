using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class MfgReceipt
{
	public bool MfgReceiptPeriodCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DateTime dateTime = default(DateTime);
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		bool result = false;
		if (currentAsDataRow != null)
		{
			if (currentAsDataRow.Table.Columns.Contains("rmmReceiptDate"))
			{
				dateTime = currentAsDataRow.Field<DateTime>("rmmReceiptDate");
			}
			if (new Financial().GetYearAndPeriod(database, dateTime.Date, "").Success)
			{
				result = true;
			}
		}
		return result;
	}

	public void PostMfgReceipt(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		SqlTransaction sqlTransaction = bindingSource.Transaction;
		if (bindingSource.CurrentAsDataRow != null)
		{
			bindingSource.CurrentAsDataRow.SetField("rmmPosted", value: true);
			if (bindingSource.CurrentAsDataRow.Field<bool>("rmmCreateJobSeq"))
			{
				int num = new Job().CreateJobSequenceFromMfgReceipt(bindingSource, sqlTransaction, bindingSource.CurrentAsDataRow);
				if (num == 0)
				{
					return;
				}
				if (bindingSource.CurrentAsDataRow.Field<byte>("rmmJobType") == 1)
				{
					bindingSource.CurrentAsDataRow.SetField("rmmJobMaterialID", num);
				}
				bindingSource.CurrentAsDataRow.SetField("rmmCreateJobSeq", value: false);
			}
		}
		if (sqlTransaction == null)
		{
			sqlTransaction = database.BeginTransaction();
		}
		try
		{
			string value = bindingSource.CurrentAsDataRow?.Field<string>("rmmMfgReceiptID");
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}
			SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, snt_outer.sntTransactionDate) As sntTransactionDate, snt_outer.sntSerialNumberID, snt_outer.sntTransactionType, snt_outer.sntPartID, snt_outer.sntPartRevisionID, snt_outer.sntPartWarehouseLocationID, snt_outer.sntPartBinID, snt_outer.sntQuantity, rmmUniqueID, snt_outer.sntJobID, snt_outer.sntJobAssemblyID, snt_outer.sntJobMaterialID, snt_outer.sntJobMaterialComponentID, snt_outer.sntNegativeTransaction, ISNULL((SELECT CASE WHEN MAX(sntSerialNumberTransactionID) IS NULL THEN CONVERT(BIT, 0) ELSE CONVERT(BIT, 1) END FROM SerialNumberTransactions snt_inner WHERE snt_inner.sntTransactionType = 1 AND snt_inner.sntSerialNumberID = snt_outer.sntSerialNumberID AND snt_inner.sntPartID = snt_outer.sntPartID AND snt_inner.sntPartRevisionID = snt_outer.sntPartRevisionID AND snt_inner.sntPartWarehouseLocationID = snt_outer.sntPartWarehouseLocationID AND snt_inner.sntPartBinID = snt_outer.sntPartBinID), 0) AS hasAssignedToJobTransaction from MfgReceipts inner join SerialNumberTransactions snt_outer on rmmUniqueID = snt_outer.sntTableUniqueID where rmmMfgReceiptID = @ID and rmmPosted = 0 order by snt_outer.sntSerialNumberID, snt_outer.sntPartID, snt_outer.sntPartRevisionID, snt_outer.sntPartWarehouseLocationID, snt_outer.sntPartBinID, snt_outer.sntTransactionDate");
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
					bool flag2 = row.Field<bool>("hasAssignedToJobTransaction");
					serialNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row.Field<string>("sntSerialNumberID"));
					switch (row.Field<byte>("sntTransactionType"))
					{
					case 34:
						status = (flag ? (flag2 ? ((byte)1) : ((byte)0)) : ((byte)2));
						transType = 3;
						break;
					case 35:
						status = (flag ? (flag2 ? ((byte)1) : ((byte)0)) : ((byte)6));
						transType = 23;
						break;
					case 36:
						status = (byte)(flag ? 2 : 3);
						transType = 20;
						break;
					case 37:
						status = (byte)((!flag) ? 3 : 0);
						transType = 4;
						break;
					case 38:
						status = (byte)((!flag) ? 2 : 0);
						transType = 2;
						break;
					case 39:
						status = (flag ? (flag2 ? ((byte)1) : ((byte)0)) : ((byte)5));
						transType = 14;
						break;
					}
					serialNumberDefinition.AddSerialTransaction(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), row.Field<decimal>("sntQuantity"), status, transType, "MfgReceipts", row.Field<Guid>("rmmUniqueID"), row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), row.Field<bool>("sntNegativeTransaction"), row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, rmnUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from MfgReceiptComponents inner join SerialNumberTransactions on rmnUniqueID = sntTableUniqueID where rmnMfgReceiptID = @ID and rmnPosted = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition2 = new SerialNumberDefinition();
				foreach (DataRow row2 in dataTable.Rows)
				{
					byte status2 = 0;
					byte transType2 = 0;
					bool flag3 = row2.Field<bool>("sntNegativeTransaction");
					serialNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row2.Field<string>("sntSerialNumberID"));
					switch (row2.Field<byte>("sntTransactionType"))
					{
					case 34:
						status2 = (byte)(flag3 ? 1 : 2);
						transType2 = 3;
						break;
					case 35:
						status2 = (byte)(flag3 ? 1 : 6);
						transType2 = 23;
						break;
					case 36:
						status2 = (byte)(flag3 ? 2 : 3);
						transType2 = 20;
						break;
					case 37:
						status2 = (byte)((!flag3) ? 3 : 0);
						transType2 = 4;
						break;
					case 38:
						status2 = (byte)((!flag3) ? 2 : 0);
						transType2 = 2;
						break;
					case 39:
						status2 = (byte)(flag3 ? 1 : 5);
						transType2 = 14;
						break;
					}
					serialNumberDefinition2.AddSerialTransaction(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntPartWarehouseLocationID"), row2.Field<string>("sntPartBinID"), row2.Field<string>("sntSerialNumberID"), row2.Field<decimal>("sntQuantity"), status2, transType2, "MfgReceiptComponents", row2.Field<Guid>("rmnUniqueID"), row2.Field<string>("sntJobID"), Convert.ToInt32(row2["sntJobAssemblyID"]), Convert.ToInt32(row2["sntJobMaterialID"]), Convert.ToInt32(row2["sntJobMaterialComponentID"]), row2.Field<bool>("sntNegativeTransaction"), row2.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition2.RefreshStatuses(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, rmmUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from MfgReceipts inner join LotNumberTransactions on rmmUniqueID = abtTableUniqueID where rmmMfgReceiptID = @ID and rmmPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
				foreach (DataRow row3 in dataTable.Rows)
				{
					byte status3 = 0;
					byte transType3 = 0;
					bool flag4 = row3.Field<bool>("abtNegativeTransaction");
					lotNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row3.Field<string>("abtLotNumberID"));
					switch (row3.Field<byte>("abtTransactionType"))
					{
					case 34:
						status3 = (byte)(flag4 ? 1 : 2);
						transType3 = 3;
						break;
					case 35:
						status3 = (byte)(flag4 ? 1 : 6);
						transType3 = 23;
						break;
					case 36:
						status3 = (byte)(flag4 ? 2 : 3);
						transType3 = 20;
						break;
					case 37:
						status3 = (byte)(flag4 ? 2 : 3);
						transType3 = 4;
						break;
					case 38:
						status3 = (byte)((!flag4) ? 2 : 0);
						transType3 = 2;
						break;
					case 39:
						status3 = (byte)(flag4 ? 1 : 5);
						transType3 = 14;
						break;
					}
					lotNumberDefinition.AddLotTransaction(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtPartWarehouseLocationID"), row3.Field<string>("abtPartBinID"), row3.Field<string>("abtLotNumberID"), row3.Field<decimal>("abtQuantity"), status3, transType3, "MfgReceipts", row3.Field<Guid>("rmmUniqueID"), row3.Field<string>("abtJobID"), Convert.ToInt32(row3["abtJobAssemblyID"]), Convert.ToInt32(row3["abtJobMaterialID"]), Convert.ToInt32(row3["abtJobMaterialComponentID"]), row3.Field<bool>("abtNegativeTransaction"), row3.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition.RefreshStatuses(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtLotNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, rmnUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from MfgReceiptComponents inner join LotNumberTransactions on rmnUniqueID = abtTableUniqueID where rmnMfgReceiptID = @ID and rmnPosted = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition2 = new LotNumberDefinition();
				foreach (DataRow row4 in dataTable.Rows)
				{
					byte status4 = 0;
					byte transType4 = 0;
					bool flag5 = row4.Field<bool>("abtNegativeTransaction");
					lotNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row4.Field<string>("abtLotNumberID"));
					switch (row4.Field<byte>("abtTransactionType"))
					{
					case 34:
						status4 = (byte)(flag5 ? 1 : 2);
						transType4 = 3;
						break;
					case 35:
						status4 = (byte)(flag5 ? 1 : 6);
						transType4 = 23;
						break;
					case 36:
						status4 = (byte)(flag5 ? 2 : 3);
						transType4 = 20;
						break;
					case 37:
						status4 = (byte)(flag5 ? 2 : 3);
						transType4 = 4;
						break;
					case 38:
						status4 = (byte)((!flag5) ? 2 : 0);
						transType4 = 2;
						break;
					case 39:
						status4 = (byte)(flag5 ? 1 : 5);
						transType4 = 14;
						break;
					}
					lotNumberDefinition2.AddLotTransaction(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtPartWarehouseLocationID"), row4.Field<string>("abtPartBinID"), row4.Field<string>("abtLotNumberID"), row4.Field<decimal>("abtQuantity"), status4, transType4, "MfgReceiptComponents", row4.Field<Guid>("rmnUniqueID"), row4.Field<string>("abtJobID"), Convert.ToInt32(row4["abtJobAssemblyID"]), Convert.ToInt32(row4["abtJobMaterialID"]), Convert.ToInt32(row4["abtJobMaterialComponentID"]), row4.Field<bool>("abtNegativeTransaction"), row4.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition2.RefreshStatuses(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtLotNumberID"));
				}
			}
			if (bindingSource.CurrentAsDataRow.Field<bool>("rmmReversalEntry"))
			{
				string text = bindingSource.CurrentAsDataRow.Field<string>("rmmJobID");
				string s = bindingSource.CurrentAsDataRow.Field<string>("rmmReverseMfgReceiptID");
				int num2 = (int)database.ExecuteScalar("select count(*) from ManufacturingVarianceLog where mvlJobID=" + text.ToSql() + " and mvlTransactionDate >= (select top 1 rmmPostedDate from MfgReceipts where rmmMfgReceiptID = " + s.ToSql() + ")");
				if (!string.IsNullOrEmpty(text) && num2 > 0)
				{
					int num3 = (int)database.ExecuteScalar("select top 1 imtPartTransactionID from PartTransactions where imtJobID=" + text.ToSql() + " and imtTableName='MfgReceipts'", sqlTransaction);
					int transactionCostID = (int)database.ExecuteScalar("select top 1 intPartTransactionCostID from PartTransactionCosts where intPartTransactionID=" + num3.ToSql() + "  order by intCreatedDate desc", sqlTransaction);
					AddEntriesToGLJournals(num3, transactionCostID, text, database);
				}
			}
			database.CommitTransaction(sqlTransaction);
			bindingSource.SaveData();
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	public void RefreshMfgReceiptQuantityComplete(DataRow row, SqlTransaction transaction, bool useActualRowOnCalculation, M1Database database)
	{
		if (transaction == null)
		{
			transaction = database.BeginTransaction();
		}
		UpdateMfgReceiptQuantityComplete(row, useActualRowOnCalculation, database, transaction);
	}

	public void UpdateMfgReceiptsToComplete(DataRow row, SqlTransaction transaction, M1Database database)
	{
		string jobId = row.Field<string>("rmmJobID");
		int jobAssemblyId = row.Field<int>("rmmJobAssemblyID");
		if (transaction == null)
		{
			transaction = database.BeginTransaction();
		}
		IList<int> allSubAssembliesIds = GetAllSubAssembliesIds(jobId, jobAssemblyId, database, transaction);
		UpdateMfgReceiptToComplete(row, database, transaction, allSubAssembliesIds);
	}

	public void CompareQtyWithJob(DataRow row, M1Database database, SqlTransaction transaction = null, bool updateMfgReceiptCost = true, string mfgReceiptToIgnore = "", bool completeDateChanged = true, bool updateJob = false)
	{
		bool num = row.Field<bool>("rmmNotUpdateJobQtyComplete");
		bool flag = row.Field<bool>("rmmReversalEntry");
		if (!(num || flag))
		{
			JobQuantity jobQtyCompleteInfo = GetJobQtyCompleteInfo(row, database, transaction, mfgReceiptToIgnore);
			UpdateJobQtyComplete(row, database, jobQtyCompleteInfo.QtyReceivedToInventory, jobQtyCompleteInfo.QtyToInspect, jobQtyCompleteInfo.QtyShipped, completeDateChanged, updateJob, transaction);
			if (updateMfgReceiptCost)
			{
				SetMfgReceiptCosts(row, database);
			}
		}
	}

	public void CompareLatestMfgQtyWithJob(string jobId, string mfgReceiptId, M1Database database, SqlTransaction transaction)
	{
		int num = 0;
		IList<int> allSubAssembliesIds = GetAllSubAssembliesIds(jobId, num, database, transaction);
		allSubAssembliesIds.Add(num);
		ResetQuantityCompleteOnJob(database, transaction, jobId);
		DataTable mfgReceiptsToUpdate = GetMfgReceiptsToUpdate(database, transaction, jobId, mfgReceiptId, allSubAssembliesIds);
		if (mfgReceiptsToUpdate.Rows.Count <= 0)
		{
			return;
		}
		Job job = new Job();
		foreach (DataRow row in mfgReceiptsToUpdate.Rows)
		{
			bool flag = row.Field<bool>("rmmProductionComplete");
			bool flag2 = row.Field<bool>("rmmPosted");
			bool flag3 = row.Field<bool>("rmmReversalEntry");
			if (flag || flag2 || flag3)
			{
				decimal value = (flag3 ? 0m : row.Field<decimal>("rmmQuantityCompleted"));
				int asmID = row.Field<int>("rmmJobAssemblyID");
				job.CompleteJob(database, transaction, jobId, flag, updateJobs: true, Convert.ToDouble(value), asmID, null, prodCompleteChanged: true, qtyCompleteChanged: true, completeDateChanged: false);
			}
			else
			{
				CompareQtyWithJob(row, database, transaction, updateMfgReceiptCost: false, mfgReceiptId, completeDateChanged: false, updateJob: true);
			}
		}
	}

	private static IList<int> GetAllSubAssembliesIds(string jobId, int jobAssemblyId, M1Database database, SqlTransaction sqlTransaction)
	{
		List<int> subAssemblyIds = new List<int>();
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT jmaJobAssemblyID,jmaParentAssemblyID FROM JobAssemblies WHERE jmaJobID = @JobID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobId;
		DataTable dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
		if (dataTable.Rows.Count != 0)
		{
			SaveNextAssemblyLevel(dataTable, jobAssemblyId, ref subAssemblyIds);
		}
		return subAssemblyIds;
	}

	private static void SaveNextAssemblyLevel(DataTable assembliesTable, int parentAssemblyId, ref List<int> subAssemblyIds)
	{
		DataRow[] array = assembliesTable.Select("jmaParentAssemblyID = " + M1Util.ConvertToLinq(parentAssemblyId) + " and jmaJobAssemblyID <> 0");
		for (int i = 0; i < array.Length; i++)
		{
			int num = Convert.ToInt32(array[i]["jmaJobAssemblyID"]);
			subAssemblyIds.Add(num);
			SaveNextAssemblyLevel(assembliesTable, num, ref subAssemblyIds);
		}
	}

	private decimal GetShippedQuantityFromJob(string jobId, M1Database database)
	{
		decimal result = default(decimal);
		try
		{
			SqlCommand sqlCommand = database.NewSqlCommand("SELECT jmpQuantityShipped\r\n                                      FROM Jobs\r\n                                      WHERE jmpJobID = @JobID");
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = jobId;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				return dataTable.Rows[0].Field<decimal>("jmpQuantityShipped");
			}
			return result;
		}
		catch
		{
			throw;
		}
	}

	private void UpdateMfgReceiptQuantityComplete(DataRow row, bool useActualRowOnCalculation, M1Database database, SqlTransaction transaction)
	{
		string text = row.Field<string>("rmmJobID");
		int num = row.Field<int>("rmmJobAssemblyID");
		string value = row.Field<string>("rmmMfgReceiptID");
		string text2 = (useActualRowOnCalculation ? string.Empty : "AND rmmMfgReceiptID<> @MfgReceiptID");
		string queryString = "SELECT rmmMfgReceiptID, rmmInventoryQuantityReceived, rmmQuantityToInspect, rmmQuantityCompleted, rmmMfgCostType, rmmReversalEntry, rmmPartID, rmmPartRevisionID, rmmCreatedDate, rmmNotUpdateJobQtyComplete,\r\n                                            rmmReversalEntry, rmmJobId, rmmJobAssemblyID, rmmProductionQuantity, rmmInventoryQuantity, rmmUnitLaborCost, rmmUnitOverheadCost, rmmUnitMaterialCost, rmmUnitSubcontractCost, rmmProductionComplete, rmmPosted\r\n                                    FROM MfgReceipts\r\n                                    WHERE rmmJobId = @JobID AND rmmJobAssemblyID = @JobAssemblyId " + text2 + "\r\n                                    ORDER BY rmmCreatedDate";
		try
		{
			SqlCommand sqlCommand = database.NewSqlCommand(queryString);
			sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = text;
			sqlCommand.Parameters.Add(new SqlParameter("@JobAssemblyId", SqlDbType.Int)).Value = num;
			sqlCommand.Parameters.Add(new SqlParameter("@MfgReceiptID", SqlDbType.NVarChar)).Value = value;
			SqlDataAdapter adapter;
			DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: true, out adapter, transaction);
			if (dataTable.Rows.Count == 0)
			{
				return;
			}
			decimal num2 = GetShippedQuantityFromJob(text, database);
			foreach (DataRow row2 in dataTable.Rows)
			{
				decimal num3 = row2.Field<decimal>("rmmInventoryQuantityReceived");
				decimal num4 = row2.Field<decimal>("rmmQuantityToInspect");
				decimal num5 = row2.Field<decimal>("rmmQuantityCompleted");
				decimal num6 = num3 + num4 + num2;
				bool flag = row2.Field<bool>("rmmProductionComplete");
				bool flag2 = row2.Field<bool>("rmmPosted");
				bool flag3 = row2.Field<bool>("rmmReversalEntry");
				bool flag4 = row2.Field<bool>("rmmNotUpdateJobQtyComplete");
				num2 = num6;
				if (num6 != num5 && !flag && !flag2 && !flag3 && !flag4)
				{
					row2.SetField("rmmQuantityCompleted", num6);
				}
			}
			database.UpdateData(dataTable, adapter, transaction);
			database.CommitTransaction(transaction);
		}
		catch
		{
			database.RollbackTransaction(transaction);
		}
	}

	private void UpdateMfgReceiptToComplete(DataRow row, M1Database database, SqlTransaction transaction, IList<int> subAssemblyIds)
	{
		string value = row.Field<string>("rmmJobID");
		int num = row.Field<int>("rmmJobAssemblyID");
		string value2 = row.Field<string>("rmmMfgReceiptID");
		decimal num2 = row.Field<decimal>("rmmQuantityCompleted");
		string text = $"{num}".ToSql();
		if (subAssemblyIds.Any())
		{
			subAssemblyIds.Add(num);
			text = string.Join(",", subAssemblyIds.Select((int x) => x.ToString().ToSql()));
		}
		string queryString = "UPDATE MfgReceipts SET rmmProductionComplete = 1, rmmQuantityCompleted = @QuantityComplete\r\n                                          WHERE rmmJobId = @JobID AND rmmJobAssemblyID IN (" + text + ") AND rmmMfgReceiptID <> @MfgReceiptID AND rmmPosted <> 1 AND rmmReversalEntry <> 1";
		try
		{
			SqlCommand sqlCommand = database.NewSqlCommand(queryString);
			sqlCommand.Parameters.AddWithValue("@QuantityComplete", num2);
			sqlCommand.Parameters.AddWithValue("@JobID", value);
			sqlCommand.Parameters.AddWithValue("@MfgReceiptID", value2);
			database.ExecuteCommand(sqlCommand, transaction);
			database.CommitTransaction(transaction);
		}
		catch
		{
			database.RollbackTransaction(transaction);
		}
	}

	private static void ResetQuantityCompleteOnJob(M1Database database, SqlTransaction transaction, string jobId)
	{
		new Job().CompleteJob(database, transaction, jobId, complete: false, updateJobs: true, Convert.ToDouble(0.0), 0, null, prodCompleteChanged: true, qtyCompleteChanged: true, completeDateChanged: false, resetJobOperations: true);
	}

	private DataTable GetMfgReceiptsToUpdate(M1Database database, SqlTransaction transaction, string jobId, string mfgReceiptId, IList<int> jobAssemblyIds)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = (string.IsNullOrEmpty(mfgReceiptId) ? string.Empty : ("AND rmmMfgReceiptID <> " + M1Util.ConvertToSql(mfgReceiptId)));
		foreach (int jobAssemblyId in jobAssemblyIds)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine("UNION ALL");
			}
			string value = "SELECT TOP 1 *\r\n\t                                    FROM MfgReceipts\r\n\t                                    WHERE rmmJobId = " + M1Util.ConvertToSql(jobId) + " AND rmmJobAssemblyID = " + M1Util.ConvertToSql(jobAssemblyId) + " " + text + "\r\n\t                                    ORDER BY rmmCreatedDate DESC";
			stringBuilder.AppendLine(value);
		}
		string queryString = $"SELECT MfgReceipts.*\r\n                                    FROM ({stringBuilder}) AS MfgReceipts\r\n                                    ORDER BY rmmCreatedDate";
		SqlCommand sqlCommand = database.NewSqlCommand(queryString);
		SqlDataAdapter adapter;
		return database.GetDataTable(sqlCommand, fillSchema: true, out adapter, transaction);
	}

	private void UpdateJobQtyComplete(DataRow row, M1Database database, decimal qtyReceivedToInventory, decimal qtyToInspect, decimal qtyShipped, bool completeDateChanged, bool updateJob, SqlTransaction transaction = null)
	{
		string text = row.Field<string>("rmmJobId");
		int num = row.Field<int>("rmmJobAssemblyID");
		decimal num2 = row.Field<decimal>("rmmQuantityCompleted");
		bool prodCompleteChanged = row.Field<bool>("rmmProductionComplete");
		decimal value = qtyReceivedToInventory + qtyToInspect + qtyShipped;
		bool qtyCompleteChanged = num2 >= 0m;
		if (updateJob)
		{
			new Job().CompleteJob(database, transaction, text, complete: false, updateJobs: true, Convert.ToDouble(value), num, DateTime.Today, prodCompleteChanged, qtyCompleteChanged, completeDateChanged);
			SqlCommand sqlCommand = database.NewSqlCommand("select jmaQuantityCompleted \r\n                                                            from JobAssemblies where jmaJobID = @jobId and jmaJobAssemblyID = @jobAssemblyId");
			sqlCommand.Parameters.Add(new SqlParameter("@jobId", SqlDbType.VarChar)).Value = text;
			sqlCommand.Parameters.Add(new SqlParameter("@jobAssemblyId", SqlDbType.Int)).Value = num;
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable.Rows.Count > 0)
			{
				row["rmmQuantityCompleted"] = dataTable.Rows[0]["jmaQuantityCompleted"];
			}
		}
		else
		{
			row["rmmQuantityCompleted"] = Convert.ToDouble(value);
		}
	}

	private JobQuantity GetJobQtyCompleteInfo(DataRow row, M1Database database, SqlTransaction transaction, string mfgReceiptToIgnore = "")
	{
		string value = row.Field<string>("rmmJobId");
		int num = row.Field<int>("rmmJobAssemblyID");
		decimal num2 = row.Field<decimal>("rmmInventoryQuantityReceived");
		decimal num3 = row.Field<decimal>("rmmQuantityToInspect");
		string value2 = row.Field<string>("rmmMfgReceiptID");
		string text = (string.IsNullOrEmpty(mfgReceiptToIgnore) ? string.Empty : "and rmmMfgReceiptID <> @mfgReceiptIdIgnored");
		SqlCommand sqlCommand = database.NewSqlCommand("select sum(jmpQuantityShipped) as QtyShipped, sum(jmaQuantityCompleted) as QtyCompleted,\r\n                    isnull((select sum(rmmInventoryQuantityReceived) from MfgReceipts where rmmPosted = 0 and rmmMfgReceiptID <> @mfgReceiptId " + text + " and rmmJobID=@jobId and rmmJobAssemblyID=@jobAssemblyId),0) + sum(jmaQuantityReceivedToInventory) as QtyReceivedToInventory,\r\n                    (isnull((select sum(qalQuantityToInspect) from InspectionLines where qalInspectionType = 3 and qalPosted = 0 and qalStatus <> 'C' and qalJobID=@jobId and qalJobAssemblyID=@jobAssemblyId),0) + isnull((select sum(rmmQuantityToInspect) from MfgReceipts where rmmPosted = 0 and rmmMfgReceiptID <> @mfgReceiptId " + text + " and rmmJobID=@jobId and rmmJobAssemblyID=@jobAssemblyId),0)) as QtyToInspect\r\n                    from JobAssemblies a inner join Jobs j on a.jmaJobID = j.jmpJobID where a.jmaJobID = @jobId and a.jmaJobAssemblyID = @jobAssemblyId\r\n                    group by a.jmaJobID, a.jmaJobAssemblyID");
		sqlCommand.Parameters.Add(new SqlParameter("@jobId", SqlDbType.VarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@jobAssemblyId", SqlDbType.Int)).Value = num;
		sqlCommand.Parameters.Add(new SqlParameter("@mfgReceiptId", SqlDbType.VarChar)).Value = value2;
		sqlCommand.Parameters.Add(new SqlParameter("@mfgReceiptIdIgnored", SqlDbType.VarChar)).Value = mfgReceiptToIgnore;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		decimal num4 = dataTable.Rows[0].Field<decimal>("QtyReceivedToInventory");
		decimal num5 = dataTable.Rows[0].Field<decimal>("QtyToInspect");
		decimal qtyShipped = dataTable.Rows[0].Field<decimal>("QtyShipped");
		decimal qtyCompleted = dataTable.Rows[0].Field<decimal>("QtyCompleted");
		if (dataTable.Rows.Count <= 0)
		{
			return null;
		}
		JobQuantity jobQuantity = new JobQuantity
		{
			QtyReceivedToInventory = num4 + num2,
			QtyToInspect = num5 + num3,
			QtyShipped = qtyShipped,
			QtyCompleted = qtyCompleted
		};
		decimal num6 = jobQuantity.QtyReceivedToInventory + jobQuantity.QtyToInspect + jobQuantity.QtyShipped;
		jobQuantity.IsEqualToQtyCompleted = num6 == jobQuantity.QtyCompleted;
		return jobQuantity;
	}

	public void SetMfgReceiptCosts(DataRow row, M1Database database)
	{
		MfgReceiptCostType mfgReceiptCostType = new MfgReceiptCostType(row, database);
		byte b = row.Field<byte>("rmmMfgCostType");
		if (!row.Field<bool>("rmmReversalEntry"))
		{
			switch (b)
			{
			case 1:
				mfgReceiptCostType.UseActualJobCosts();
				break;
			case 2:
				mfgReceiptCostType.UsePartRevisionCosts();
				break;
			case 3:
				mfgReceiptCostType.UseEstimatedJobCost();
				break;
			case 4:
				mfgReceiptCostType.UseManualOverride();
				break;
			}
		}
	}

	public IDictionary<PartInformation, decimal> GetPartInformantionAndQuantityToReturn(M1BindingSource bindingSource)
	{
		IDictionary<PartInformation, decimal> dictionary = new Dictionary<PartInformation, decimal>(new PartInformationEqualityComparer());
		if (bindingSource.Database.GetDataTable("Select rmmReverseMfgReceiptID From MfgReceipts \r\n                                                                     Where rmmMfgReceiptID = " + M1Util.ConvertToSql(bindingSource.CurrentAsDataRow.Field<string>("rmmMfgReceiptID"))).Rows.Count != 0)
		{
			string o = bindingSource.CurrentAsDataRow.Field<string>("rmmReverseMfgReceiptID");
			DataTable dataTable = bindingSource.Database.GetDataTable("SELECT rmmReceiptType, rmmJobID , rmmKitPart, rmmPartID, \r\n                                                                rmmPartRevisionID, rmmPartWarehouseLocationID, rmmPartBinID, \r\n                                                                rmmUniqueID, rmmInventoryQuantityReceived, rmmMiscInvQuantityReceived \r\n                                                                From MfgReceipts Where rmmMfgReceiptID = " + M1Util.ConvertToSql(o));
			if (dataTable.Rows.Count != 0)
			{
				if (dataTable.Rows[0].Field<bool>("rmmKitPart"))
				{
					DataTable dataTable2 = bindingSource.Database.GetDataTable("Select rmnJobID, rmnPartID, rmnUniqueID, rmnInvReceiptQuantity, \r\n                                                                               rmnPartRevisionID, rmnPartWarehouseLocationID, rmnPartBinID\r\n                                                                               From MfgReceiptComponents Where rmnMfgReceiptID = " + M1Util.ConvertToSql(o));
					if (dataTable2 != null && dataTable2.Rows.Count != 0)
					{
						foreach (DataRow row in dataTable2.Rows)
						{
							decimal num = row.Field<decimal>("rmnInvReceiptQuantity");
							PartInformation key = CreatePartInformation(bindingSource.Database, row, "rmn");
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
					decimal num = dataTable.Rows[0].Field<decimal>("rmmInventoryQuantityReceived") + dataTable.Rows[0].Field<decimal>("rmmMiscInvQuantityReceived");
					PartInformation key2 = CreatePartInformation(bindingSource.Database, dataTable.Rows[0], "rmm");
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
		}
		return dictionary;
	}

	public string MfgReceiptPostCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		SqlTransaction transaction = bindingSource.Transaction;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		bool flag = true;
		if (MfgReceiptPostedCheck(database, transaction, currentAsDataRow.Field<string>("rmmMfgReceiptID")))
		{
			return "This record cannot be saved or posted as it is already marked as being posted in the database.";
		}
		if (currentAsDataRow.Field<bool>("rmmReversalEntry"))
		{
			IList<string> list = new List<string>();
			bool flag2 = (bool)database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
			bool flag3 = (bool)database.Props("IM")["xapIMEnableWarningWhenNegative"];
			DataTable dataTable = database.GetDataTable("Select rmmReverseMfgReceiptID From MfgReceipts Where rmmMfgReceiptID = " + M1Util.ConvertToSql(currentAsDataRow.Field<string>("rmmMfgReceiptID")));
			IDictionary<PartInformation, decimal> dictionary = new Dictionary<PartInformation, decimal>(new PartInformationEqualityComparer());
			if (dataTable.Rows.Count != 0)
			{
				string o = dataTable.Rows[0].Field<string>("rmmReverseMfgReceiptID");
				DataTable dataTable2 = database.GetDataTable("SELECT rmmReceiptType, rmmJobID , rmmKitPart, rmmPartID, \r\n                                                                rmmPartRevisionID, rmmPartWarehouseLocationID, rmmPartBinID, \r\n                                                                rmmUniqueID, rmmInventoryQuantityReceived, rmmMiscInvQuantityReceived \r\n                                                                From MfgReceipts Where rmmMfgReceiptID = " + M1Util.ConvertToSql(o));
				if (dataTable2.Rows.Count != 0)
				{
					int num = dataTable2.Rows[0].Field<byte>("rmmReceiptType");
					if (dataTable2.Rows[0].Field<bool>("rmmKitPart"))
					{
						DataTable dataTable3 = database.GetDataTable("Select rmnJobID, rmnPartID, rmnUniqueID, rmnInvReceiptQuantity, \r\n                                                                               rmnPartRevisionID, rmnPartWarehouseLocationID, rmnPartBinID\r\n                                                                               From MfgReceiptComponents Where rmnMfgReceiptID = " + M1Util.ConvertToSql(o));
						if (dataTable3 != null && dataTable3.Rows.Count != 0)
						{
							foreach (DataRow row in dataTable3.Rows)
							{
								decimal qtyToReverse = row.Field<decimal>("rmnInvReceiptQuantity");
								flag = ((num == 1) ? PostCheckingUtility.CheckReceiptToJob(row.Field<string>("rmnJobID"), database) : PostCheckingUtility.CheckReceiptToInventory(row.Field<Guid>("rmnUniqueID"), row.Field<string>("rmnPartID"), database, qtyToReverse));
								if (!flag && (num == 1 || !flag2))
								{
									break;
								}
							}
							dictionary = GetPartInformantionAndQuantityToReturn(bindingSource);
							list = VerifyQuantityOnHand(database, dictionary);
						}
					}
					else if (num == 1)
					{
						flag = PostCheckingUtility.CheckReceiptToJob(dataTable2.Rows[0].Field<string>("rmmJobID"), database);
					}
					else
					{
						decimal qtyToReverse = dataTable2.Rows[0].Field<decimal>("rmmInventoryQuantityReceived") + dataTable2.Rows[0].Field<decimal>("rmmMiscInvQuantityReceived");
						if (flag2)
						{
							dictionary = GetPartInformantionAndQuantityToReturn(bindingSource);
							list = VerifyQuantityOnHand(database, dictionary);
						}
						flag = PostCheckingUtility.CheckReceiptToInventory(dataTable2.Rows[0].Field<Guid>("rmmUniqueID"), dataTable2.Rows[0].Field<string>("rmmPartID"), database, qtyToReverse);
					}
				}
			}
			if (flag2)
			{
				if (!list.Any())
				{
					return string.Empty;
				}
				if (dictionary.Any((KeyValuePair<PartInformation, decimal> keyValuePair) => keyValuePair.Key.IsBinInactive && keyValuePair.Key.HasNegativeQOH))
				{
					return "This transaction CAN NOT be posted because it will result in a negative quantity on hand for an INACTIVE bin for the part(s) indicated.\n\n" + string.Join("\n", list);
				}
				if (dictionary.Any((KeyValuePair<PartInformation, decimal> keyValuePair) => keyValuePair.Key.IsSerialLotPart && keyValuePair.Key.HasNegativeQOH))
				{
					return "This transaction CAN NOT be posted because it will result in a negative quantity on hand for the serial/lot tracked part(s) indicated.\n\n" + string.Join("\n", list);
				}
				if (flag3)
				{
					return "This transaction WILL RESULT in a negative quantity on hand for the part(s) indicated. Are you sure?\n\n" + string.Join("\n", list);
				}
			}
			if (!flag)
			{
				return "This MfgReceipt reversal cannot be posted. Either the job has been closed, the received parts have been issued, or, there is insufficient remaining quantity.";
			}
			return string.Empty;
		}
		if (!GetMfgReceiptInactivePartBinsMessage(database, currentAsDataRow, out var inactiveBinsMessage))
		{
			return inactiveBinsMessage;
		}
		return string.Empty;
	}

	public bool MfgReceiptPostedCheck(M1Database database, SqlTransaction transaction, string mfgReceiptID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(rmmPosted,0) As rmmPosted From MfgReceipts Where rmmMfgReceiptID = @MfgReceiptID");
		sqlCommand.Parameters.Add(new SqlParameter("@MfgReceiptID", SqlDbType.NVarChar)).Value = mfgReceiptID;
		object obj = database.ExecuteScalar(sqlCommand, transaction);
		if (obj == null)
		{
			return false;
		}
		return (bool)obj;
	}

	private static PartInformation CreatePartInformation(M1Database database, DataRow lineRow, string prefix)
	{
		Part part = new Part();
		if (lineRow != null)
		{
			string text = lineRow.Field<string>(prefix + "PartID").Trim();
			string partRevision = lineRow.Field<string>(prefix + "PartRevisionID").Trim();
			string text2 = lineRow.Field<string>(prefix + "PartWarehouseLocationID").Trim();
			string text3 = lineRow.Field<string>(prefix + "PartBinID").Trim();
			return new PartInformation
			{
				Part = text,
				PartRevision = partRevision,
				PartWarehouse = text2,
				PartBin = text3,
				IsSerialLotPart = part.IsSerialOrLotTracked(database, text, null),
				IsBinInactive = part.IsPartBinInactive(database, text, partRevision, text2, text3)
			};
		}
		return null;
	}

	public IList<string> VerifyQuantityOnHand(M1Database database, IDictionary<PartInformation, decimal> partsAndQuantities)
	{
		IList<string> list = new List<string>();
		IList<string> list2 = new List<string>();
		IList<string> list3 = new List<string>();
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
				if (partsAndQuantity.Key.IsBinInactive)
				{
					stringBuilder.AppendLine($"Reversed Receipt Qty [{partsAndQuantity.Value}] is GREATER THAN Quantity On Hand [{num}]\n[Part: '{partsAndQuantity.Key.Part}', Revision: '{partsAndQuantity.Key.PartRevision}', Warehouse: '{partsAndQuantity.Key.PartWarehouse}', Bin: '{partsAndQuantity.Key.PartBin}'].");
					list.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
				else if (partsAndQuantity.Key.IsSerialLotPart)
				{
					stringBuilder.AppendLine($"Reversed Inv Qty Rec'd [{partsAndQuantity.Value}] is greater than Quantity On Hand [{num}]\n[Part: '{partsAndQuantity.Key.Part}', Revision: '{partsAndQuantity.Key.PartRevision}', Warehouse: '{partsAndQuantity.Key.PartWarehouse}', Bin: '{partsAndQuantity.Key.PartBin}'].");
					list2.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
				else
				{
					stringBuilder.AppendLine($"Reversed Inv Qty Rec'd [{partsAndQuantity.Value}] is greater than Quantity On Hand [{num}]\n[Part: '{partsAndQuantity.Key.Part}', Revision: '{partsAndQuantity.Key.PartRevision}', Warehouse: '{partsAndQuantity.Key.PartWarehouse}', Bin: '{partsAndQuantity.Key.PartBin}'].");
					list3.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
			}
		}
		if (!list.Any())
		{
			if (!list2.Any())
			{
				return list3;
			}
			return list2;
		}
		return list;
	}

	public bool GetMfgReceiptInactivePartBinsMessage(M1Database database, DataRow mfgReceiptRow, out string inactiveBinsMessage)
	{
		inactiveBinsMessage = string.Empty;
		if (mfgReceiptRow.Field<byte>("rmmReceiptType") == 1)
		{
			return true;
		}
		StringBuilder stringBuilder = new StringBuilder();
		Part part = new Part();
		string text = mfgReceiptRow.Field<string>("rmmPartID");
		string text2 = mfgReceiptRow.Field<string>("rmmPartRevisionID");
		string text3 = mfgReceiptRow.Field<string>("rmmPartWarehouseLocationID");
		string text4 = mfgReceiptRow.Field<string>("rmmPartBinID");
		if (part.IsPartNonStockedOrKit(database, mfgReceiptRow.Field<string>("rmmPartID")))
		{
			DataTable dataTable = database.GetDataTable("SELECT * From MfgReceiptComponents WHERE rmnMfgReceiptID = " + M1Util.ConvertToSql(mfgReceiptRow.Field<string>("rmmMfgReceiptID")));
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					string text5 = row.Field<string>("rmnPartID");
					string text6 = row.Field<string>("rmnPartRevisionID");
					string text7 = row.Field<string>("rmnPartWarehouseLocationID");
					string text8 = row.Field<string>("rmnPartBinID");
					if (part.IsPartBinInactive(database, text5, text6, text7, text8))
					{
						stringBuilder.AppendLine("[Part: '" + text5 + "', Revision: '" + text6 + "', Warehouse: '" + text7 + "', Bin: '" + text8 + "' is inactive]");
					}
				}
			}
		}
		else if (part.IsPartBinInactive(database, text, text2, text3, text4))
		{
			stringBuilder.AppendLine("[Part: '" + text + "', Revision: '" + text2 + "', Warehouse: '" + text3 + "', Bin: '" + text4 + "' is inactive]");
		}
		if (stringBuilder.Length > 0)
		{
			inactiveBinsMessage = "This transaction CAN NOT be posted because an INACTIVE bin exists for the part(s) indicated\n\n" + stringBuilder.ToString();
			return false;
		}
		return true;
	}

	private void AddEntriesToGLJournals(int transactionID, int transactionCostID, string jobID, M1Database database)
	{
		M1BindingSource m1BindingSource = new M1BindingSource(database);
		m1BindingSource.DataSourceTable = "PARTTRANSACTIONCOSTS";
		m1BindingSource.NavigateTo(database, "intPartTransactionID = " + M1Util.ConvertToSql(transactionID) + " and intPartTransactionCostID = " + M1Util.ConvertToSql(transactionCostID));
		if (m1BindingSource.Count != 0)
		{
			DataRow currentAsDataRow = m1BindingSource.CurrentAsDataRow;
			CostOfGoodSoldDefinition costOfGoodSoldDefinition = new CostOfGoodSoldDefinition(m1BindingSource, "intQuantity", "intSourceTableName", DateTime.Now, 42, 2, reverseSign: true, Convert.ToDecimal(currentAsDataRow["intQuantity"]), "CHECKFORSOURCE", string.Empty, string.Empty, string.Empty, jobID);
			costOfGoodSoldDefinition.UseFiscalYearAndPeriodFromJournal = true;
			costOfGoodSoldDefinition.AddJournal(m1BindingSource.Database, currentAsDataRow, DataRowVersion.Current, m1BindingSource.Transaction);
		}
	}
}
