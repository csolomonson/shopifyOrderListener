using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;

namespace M1.Ax.Erp;

public class TimecardLine
{
	public void BackOutJobOperation(M1Database database, DataRow row, SqlTransaction transaction)
	{
		if (!row.HasVersion(DataRowVersion.Original) || string.IsNullOrWhiteSpace(row.Field<string>("lmlJobID", DataRowVersion.Original)))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("");
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("UPDATE JobOperations SET ");
		stringBuilder.Append("jmoQuantityComplete = jmoQuantityComplete - @GoodQuantity");
		sqlCommand.Parameters.Add(new SqlParameter("@GoodQuantity", SqlDbType.Decimal)).Value = row.Field<decimal>("lmlGoodQuantity", DataRowVersion.Original);
		stringBuilder.Append(",jmoScrapQuantityReceived = jmoScrapQuantityReceived - @ScrapQuantity");
		sqlCommand.Parameters.Add(new SqlParameter("@ScrapQuantity", SqlDbType.Decimal)).Value = row.Field<decimal>("lmlScrapQuantity", DataRowVersion.Original);
		stringBuilder.Append(",jmoSetupPercentComplete = CASE WHEN jmoSetupPercentComplete - @SetupPercentCompleted > 999 THEN 999 WHEN jmoSetupPercentComplete - @SetupPercentCompleted < -999 THEN -999 ELSE jmoSetupPercentComplete - @SetupPercentCompleted END ");
		sqlCommand.Parameters.Add(new SqlParameter("@SetupPercentCompleted", SqlDbType.SmallInt)).Value = row.Field<short>("lmlSetupPercentCompleted", DataRowVersion.Original);
		if (row.Field<byte>("lmlWorkType", DataRowVersion.Original) == 1)
		{
			stringBuilder.Append(",jmoActualSetupHours = jmoActualSetupHours - @MachineHours");
			sqlCommand.Parameters.Add(new SqlParameter("@MachineHours", SqlDbType.Decimal)).Value = row.Field<decimal>("lmlMachineHours", DataRowVersion.Original);
			if (row.Field<byte>("lmlCompletionType", DataRowVersion.Original) == 2 || row.Field<byte>("lmlCompletionType", DataRowVersion.Original) == 4)
			{
				stringBuilder.Append(",jmoSetupComplete = 0");
			}
		}
		else
		{
			stringBuilder.Append(",jmoActualProductionHours = jmoActualProductionHours - @MachineHours");
			sqlCommand.Parameters.Add(new SqlParameter("@MachineHours", SqlDbType.Decimal)).Value = row.Field<decimal>("lmlMachineHours", DataRowVersion.Original);
			if (row.Field<byte>("lmlCompletionType", DataRowVersion.Original) == 2 || row.Field<byte>("lmlCompletionType", DataRowVersion.Original) == 4)
			{
				stringBuilder.Append(",jmoProductionComplete = 0");
			}
		}
		stringBuilder.Append(" WHERE jmoJobID = @JobID and jmoJobAssemblyID = @AsmID and jmoJobOperationID = @OperationID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = row.Field<string>("lmlJobID", DataRowVersion.Original);
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = row.Field<int>("lmlJobAssemblyID", DataRowVersion.Original);
		sqlCommand.Parameters.Add(new SqlParameter("@OperationID", SqlDbType.Int)).Value = row.Field<int>("lmlJobOperationID", DataRowVersion.Original);
		sqlCommand.CommandText = stringBuilder.ToString();
		database.ExecuteCommand(sqlCommand, transaction);
		new Job().RefreshScheduleActuals(database, row.Field<string>("lmlJobID", DataRowVersion.Original), row.Field<int>("lmlJobAssemblyID", DataRowVersion.Original), row.Field<int>("lmlJobOperationID", DataRowVersion.Original), transaction);
	}

	public void AddToJobOperation(M1Database database, DataRow row, SqlTransaction transaction)
	{
		string text = row.Field<string>("lmljobid");
		int asmID = row.Field<int>("lmlJobAssemblyID");
		int seq = row.Field<int>("lmlJobOperationID");
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("");
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("UPDATE JobOperations SET ");
		stringBuilder.Append("jmoQuantityComplete = jmoQuantityComplete + @GoodQuantity");
		sqlCommand.Parameters.Add(new SqlParameter("@GoodQuantity", SqlDbType.Decimal)).Value = row.Field<decimal>("lmlGoodQuantity");
		stringBuilder.Append(",jmoScrapQuantityReceived = jmoScrapQuantityReceived + @ScrapQuantity");
		sqlCommand.Parameters.Add(new SqlParameter("@ScrapQuantity", SqlDbType.Decimal)).Value = row.Field<decimal>("lmlScrapQuantity");
		stringBuilder.Append(",jmoSetupPercentComplete = CASE WHEN jmoSetupPercentComplete + @SetupPercentCompleted > 999 THEN 999 WHEN jmoSetupPercentComplete + @SetupPercentCompleted < -999 THEN -999 ELSE jmoSetupPercentComplete + @SetupPercentCompleted END ");
		sqlCommand.Parameters.Add(new SqlParameter("@SetupPercentCompleted", SqlDbType.SmallInt)).Value = row.Field<short>("lmlSetupPercentCompleted");
		if (row.Field<byte>("lmlWorkType") == 1)
		{
			stringBuilder.Append(",jmoActualSetupHours = jmoActualSetupHours + @MachineHours");
			sqlCommand.Parameters.Add(new SqlParameter("@MachineHours", SqlDbType.Decimal)).Value = row.Field<decimal>("lmlMachineHours");
			if (row.Field<byte>("lmlCompletionType") == 1 || row.Field<byte>("lmlCompletionType") == 3)
			{
				stringBuilder.Append(",jmoSetupComplete = CASE WHEN jmoSetupPercentComplete < 100 THEN 0 ELSE 1 END ");
			}
			else if (row.Field<byte>("lmlCompletionType") == 2 || row.Field<byte>("lmlCompletionType") == 4)
			{
				stringBuilder.Append(",jmoSetupComplete = 1");
			}
		}
		else
		{
			stringBuilder.Append(",jmoActualProductionHours = @MachineHours");
			SqlCommand sqlCommand2 = database.NewSqlCommand("select sum(lmlMachineHours) as lmlMachineHours from Timecardlines where lmlJobId = @JobID and lmlJobAssemblyid = @AsmID and lmlJobOperationID = @OperationID and lmlworktype = 2 and lmlUniqueID <> @UniqueID");
			sqlCommand2.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = row.Field<string>("lmlJobID");
			sqlCommand2.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = row.Field<int>("lmlJobAssemblyID");
			sqlCommand2.Parameters.Add(new SqlParameter("@OperationID", SqlDbType.Int)).Value = row.Field<int>("lmlJobOperationID");
			sqlCommand2.Parameters.Add(new SqlParameter("@UniqueID", SqlDbType.UniqueIdentifier)).Value = row.Field<Guid>("lmlUniqueID");
			object obj = database.ExecuteScalar(sqlCommand2, transaction);
			if (obj != DBNull.Value)
			{
				sqlCommand.Parameters.Add(new SqlParameter("@MachineHours", SqlDbType.Decimal)).Value = Convert.ToDecimal(obj) + row.Field<decimal>("lmlMachineHours");
			}
			else
			{
				sqlCommand.Parameters.Add(new SqlParameter("@MachineHours", SqlDbType.Decimal)).Value = row.Field<decimal>("lmlMachineHours");
			}
			if (row.Field<byte>("lmlCompletionType") == 2 || row.Field<byte>("lmlCompletionType") == 4)
			{
				stringBuilder.Append(",jmoProductionComplete = 1");
			}
		}
		stringBuilder.Append(" WHERE jmoJobID = @JobID and jmoJobAssemblyID = @AsmID and jmoJobOperationID = @OperationID");
		sqlCommand.Parameters.Add(new SqlParameter("@JobID", SqlDbType.NVarChar)).Value = row.Field<string>("lmlJobID");
		sqlCommand.Parameters.Add(new SqlParameter("@AsmID", SqlDbType.Int)).Value = row.Field<int>("lmlJobAssemblyID");
		sqlCommand.Parameters.Add(new SqlParameter("@OperationID", SqlDbType.Int)).Value = row.Field<int>("lmlJobOperationID");
		sqlCommand.CommandText = stringBuilder.ToString();
		database.ExecuteCommand(sqlCommand, transaction);
		new Job().RefreshScheduleActuals(database, text, asmID, seq, transaction);
	}
}
