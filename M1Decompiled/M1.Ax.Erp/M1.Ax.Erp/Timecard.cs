using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public static class Timecard
{
	public static void RecalculateHeaderTimes(M1Database database, int timecardID, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select * From Timecards Where lmpTimecardID = @TimecardID");
		sqlCommand.Parameters.Add(new SqlParameter("@TimecardID", SqlDbType.Int)).Value = timecardID;
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: true, out adapter, transaction);
		if (dataTable.Rows.Count != 0)
		{
			RecalculateHeaderTimes(database, dataTable.Rows[0], transaction);
			database.UpdateData(dataTable, adapter, transaction);
		}
	}

	public static void RecalculateHeaderTimes(M1Database database, DataRow row, SqlTransaction transaction)
	{
		DateTime? dateTime = null;
		DateTime? value = null;
		DateTime? dateTime2 = null;
		DateTime? value2 = null;
		SqlCommand sqlCommand = database.NewSqlCommand("Select Top 1 lmlRoundedStartTime  From TimecardLines Where lmlTimecardId = @TimecardID And lmlRoundedStartTime Is Not Null  Order By lmlRoundedStartTime");
		sqlCommand.Parameters.Add(new SqlParameter("@TimecardID", SqlDbType.Int)).Value = row.Field<int>("lmpTimecardID");
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			dateTime = dataTable.Rows[0].Field<DateTime?>("lmlRoundedStartTime");
		}
		sqlCommand = database.NewSqlCommand("Select Top 1 lmlActualStartTime  From TimecardLines Where lmlTimecardId = @TimecardID And lmlActualStartTime Is Not Null  Order By lmlActualStartTime");
		sqlCommand.Parameters.Add(new SqlParameter("@TimecardID", SqlDbType.Int)).Value = row.Field<int>("lmpTimecardID");
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			value = dataTable.Rows[0].Field<DateTime?>("lmlActualStartTime");
		}
		sqlCommand.CommandText = "Select Top 1 lmlRoundedEndTime  From TimecardLines Where lmlTimecardId = @TimecardID And lmlRoundedEndTime Is Not Null  Order By lmlRoundedEndTime Desc";
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			dateTime2 = dataTable.Rows[0].Field<DateTime?>("lmlRoundedEndTime");
		}
		sqlCommand.CommandText = "Select Top 1 lmlActualEndTime  From TimecardLines Where lmlTimecardId = @TimecardID And lmlActualEndTime Is Not Null  Order By lmlActualEndTime Desc";
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			value2 = dataTable.Rows[0].Field<DateTime?>("lmlActualEndTime");
		}
		changeTimeField(row, "lmpRoundedStartTime", dateTime);
		changeTimeField(row, "lmpActualStartTime", value);
		changeTimeField(row, "lmpRoundedEndTime", dateTime2);
		changeTimeField(row, "lmpActualEndTime", value2);
		if (database.Props("PN").Field<byte>("xapDCPayCalculationMethod") == 2)
		{
			SqlCommand sqlCommand2 = database.NewSqlCommand("select sum(lmlLaborHours) as lmlLaborHours from TimecardLines Where lmlTimecardID = @TimecardID");
			sqlCommand2.Parameters.Add(new SqlParameter("@TimecardID", SqlDbType.Int)).Value = row.Field<int>("lmpTimecardID");
			decimal value3 = Convert.ToDecimal(database.ExecuteScalar(sqlCommand2, transaction));
			row.SetField("lmpPayrollHours", value3);
		}
		else
		{
			row.SetField("lmpPayrollHours", Shift.CalculateHoursMinusBreaks(database, row.Field<short>("lmpShiftID"), dateTime, dateTime2, transaction));
		}
	}

	private static void changeTimeField(DataRow row, string fieldName, DateTime? value)
	{
		bool flag = false;
		if ((row.IsNull(fieldName) && value.HasValue) || (!row.IsNull(fieldName) && !value.HasValue))
		{
			flag = true;
		}
		else if (row.Field<DateTime?>(fieldName) != value)
		{
			flag = true;
		}
		if (flag)
		{
			row.SetField(fieldName, value);
		}
	}

	public static void RecalculateIdleTimes(M1Database database, DataRow row)
	{
	}
}
