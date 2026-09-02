using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public static class Shift
{
	public static DateTime? CalculateEndTime(M1Database database, short shiftID, DateTime? startTime, decimal hours, SqlTransaction transaction)
	{
		if (startTime.HasValue && hours != 0m)
		{
			DateTime? result = startTime.Value.AddSeconds((double)hours * 60.0 * 60.0);
			SqlCommand sqlCommand = database.NewSqlCommand("Select lmtDay,lmtBreak1Paid,lmtBreak1StartTime,lmtBreak1EndTime,lmtBreak2Paid,lmtBreak2StartTime,lmtBreak2EndTime,lmtBreak3Paid,lmtBreak3StartTime,lmtBreak3EndTime From Shifts Inner join ShiftBreaks on lmsShiftID=lmtShiftID Where lmsShiftID = @ShiftID");
			sqlCommand.Parameters.Add(new SqlParameter("@ShiftID", SqlDbType.SmallInt)).Value = shiftID;
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable.Rows.Count != 0)
			{
				DateTime dateTime = startTime.Value.Date;
				do
				{
					byte b = DayOfWeekToShiftDay(dateTime.DayOfWeek);
					DataRow dataRow = dataTable.Select("lmtDay = " + M1Util.ConvertToLinq(b)).FirstOrDefault();
					if (dataRow != null)
					{
						result = result.Value.AddMinutes(getBreakOverlapMinutes(startTime.Value, result.Value, !dataRow.Field<bool>("lmtBreak1Paid"), dataRow.Field<decimal>("lmtBreak1StartTime"), dataRow.Field<decimal>("lmtBreak1EndTime")));
						result = result.Value.AddMinutes(getBreakOverlapMinutes(startTime.Value, result.Value, !dataRow.Field<bool>("lmtBreak2Paid"), dataRow.Field<decimal>("lmtBreak2StartTime"), dataRow.Field<decimal>("lmtBreak2EndTime")));
						result = result.Value.AddMinutes(getBreakOverlapMinutes(startTime.Value, result.Value, !dataRow.Field<bool>("lmtBreak3Paid"), dataRow.Field<decimal>("lmtBreak3StartTime"), dataRow.Field<decimal>("lmtBreak3EndTime")));
					}
					dateTime = dateTime.AddDays(1.0);
				}
				while (dateTime <= result.Value.Date);
			}
			return result;
		}
		return startTime;
	}

	private static byte DayOfWeekToShiftDay(DayOfWeek dow)
	{
		byte b = Convert.ToByte(dow);
		if (b == 0)
		{
			b = 7;
		}
		return b;
	}

	private static int getBreakOverlapMinutes(DateTime startDateTime, DateTime endDateTime, bool addBreakTime, decimal breakStartTime, decimal breakEndTime)
	{
		int num = 0;
		if (addBreakTime && (breakStartTime != 0m || breakEndTime != 0m))
		{
			DateTime dateTime = startDateTime.Date.AddMinutes(M1Time.ConvertDecimalTimeToMinutes(breakStartTime));
			DateTime dateTime2 = startDateTime.Date.AddMinutes(M1Time.ConvertDecimalTimeToMinutes(breakEndTime));
			if (dateTime2 < dateTime)
			{
				dateTime2 = dateTime2.AddHours(24.0);
			}
			if (dateTime >= startDateTime && dateTime2 <= endDateTime)
			{
				num += (int)dateTime2.Subtract(dateTime).TotalMinutes;
			}
			else if (dateTime < startDateTime && dateTime2 > startDateTime && dateTime2 < endDateTime)
			{
				num += (int)dateTime2.Subtract(startDateTime).TotalMinutes;
			}
			else if (dateTime < endDateTime && dateTime2 > endDateTime && dateTime > startDateTime)
			{
				num += (int)endDateTime.Subtract(dateTime).TotalMinutes;
			}
			else if (dateTime < startDateTime && dateTime2 <= endDateTime && endDateTime.Hour >= 24 && startDateTime < dateTime.AddHours(24.0) && endDateTime >= dateTime2.AddHours(24.0))
			{
				num += (int)dateTime2.Subtract(dateTime).TotalMinutes;
			}
		}
		return num;
	}

	public static DateTime? GetDayStartTime(M1Database database, short shiftID, DateTime? workDate, SqlTransaction transaction)
	{
		DateTime? result = null;
		if (shiftID > 0 && workDate.HasValue)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select lmtStartTime From ShiftBreaks Where lmtShiftID = @ShiftID And lmtDay = @Day");
			byte b = DayOfWeekToShiftDay(workDate.Value.DayOfWeek);
			sqlCommand.Parameters.Add(new SqlParameter("@Day", SqlDbType.TinyInt)).Value = b;
			sqlCommand.Parameters.Add(new SqlParameter("@ShiftID", SqlDbType.SmallInt)).Value = shiftID;
			object obj = database.ExecuteScalar(sqlCommand, transaction);
			if (obj != DBNull.Value)
			{
				result = workDate.Value.AddMinutes(M1Time.ConvertDecimalTimeToMinutes(Convert.ToDecimal(obj)));
			}
		}
		return result;
	}

	public static DateTime? GetDayEndTime(M1Database database, short shiftID, DateTime? workDate, SqlTransaction transaction)
	{
		DateTime? result = null;
		if (shiftID > 0 && workDate.HasValue)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("Select lmtEndTime From ShiftBreaks Where lmtShiftID = @ShiftID And lmtDay = @Day");
			byte b = DayOfWeekToShiftDay(workDate.Value.DayOfWeek);
			sqlCommand.Parameters.Add(new SqlParameter("@Day", SqlDbType.TinyInt)).Value = b;
			sqlCommand.Parameters.Add(new SqlParameter("@ShiftID", SqlDbType.SmallInt)).Value = shiftID;
			object obj = database.ExecuteScalar(sqlCommand, transaction);
			if (obj != DBNull.Value)
			{
				result = workDate.Value.AddMinutes(M1Time.ConvertDecimalTimeToMinutes(Convert.ToDecimal(obj)));
			}
		}
		return result;
	}

	public static decimal CalculateHoursMinusBreaks(M1Database database, short shiftID, DateTime? startTime, DateTime? endTime, SqlTransaction transaction)
	{
		int num = 0;
		if (shiftID > 0 && startTime.HasValue && endTime.HasValue)
		{
			num = (int)endTime.Value.Subtract(startTime.Value).TotalMinutes;
			endTime.Value.Subtract(startTime.Value);
			byte b = DayOfWeekToShiftDay(startTime.Value.DayOfWeek);
			SqlCommand sqlCommand = database.NewSqlCommand("Select lmtBreak1Paid,lmtBreak1StartTime,lmtBreak1EndTime,lmtBreak2Paid,lmtBreak2StartTime,lmtBreak2EndTime,lmtBreak3Paid,lmtBreak3StartTime,lmtBreak3EndTime From Shifts Inner join ShiftBreaks on lmsShiftID=lmtShiftID and lmtDay=@Day Where lmsShiftID = @ShiftID");
			sqlCommand.Parameters.Add(new SqlParameter("@Day", SqlDbType.TinyInt)).Value = b;
			sqlCommand.Parameters.Add(new SqlParameter("@ShiftID", SqlDbType.SmallInt)).Value = shiftID;
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row = dataTable.Rows[0];
				num -= getBreakOverlapMinutes(startTime.Value, endTime.Value, !row.Field<bool>("lmtBreak1Paid"), row.Field<decimal>("lmtBreak1StartTime"), row.Field<decimal>("lmtBreak1EndTime"));
				num -= getBreakOverlapMinutes(startTime.Value, endTime.Value, !row.Field<bool>("lmtBreak2Paid"), row.Field<decimal>("lmtBreak2StartTime"), row.Field<decimal>("lmtBreak2EndTime"));
				num -= getBreakOverlapMinutes(startTime.Value, endTime.Value, !row.Field<bool>("lmtBreak3Paid"), row.Field<decimal>("lmtBreak3StartTime"), row.Field<decimal>("lmtBreak3EndTime"));
			}
		}
		return (decimal)num / 60.0m;
	}
}
