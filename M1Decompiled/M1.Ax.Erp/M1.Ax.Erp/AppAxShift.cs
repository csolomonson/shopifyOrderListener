using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Shift")]
[ComVisible(true)]
public class AppAxShift
{
	private IServiceProvider provider;

	public AppAxShift(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public DateTime? CalculateEndTime(short shiftID, object startTime, decimal hours, object transaction)
	{
		if (startTime == DBNull.Value)
		{
			startTime = null;
		}
		return Shift.CalculateEndTime(provider.GetService(typeof(M1Database)) as M1Database, shiftID, (DateTime?)startTime, hours, (SqlTransaction)transaction);
	}

	public DateTime? GetDayStartTime(short shiftID, object workDate, object transaction)
	{
		if (workDate == DBNull.Value)
		{
			workDate = null;
		}
		return Shift.GetDayStartTime(provider.GetService(typeof(M1Database)) as M1Database, shiftID, (DateTime?)workDate, (SqlTransaction)transaction);
	}

	public decimal CalculateHours(short shiftID, object startTime, object endTime, object transaction)
	{
		if (startTime == DBNull.Value)
		{
			startTime = null;
		}
		if (endTime == DBNull.Value)
		{
			endTime = null;
		}
		return Shift.CalculateHoursMinusBreaks(provider.GetService(typeof(M1Database)) as M1Database, shiftID, (DateTime?)startTime, (DateTime?)endTime, (SqlTransaction)transaction);
	}

	public void RecalculateIdleTimes(object timecardRow)
	{
	}
}
