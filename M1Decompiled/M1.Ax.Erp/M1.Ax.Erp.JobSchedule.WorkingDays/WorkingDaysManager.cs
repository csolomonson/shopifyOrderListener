using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;

namespace M1.Ax.Erp.JobSchedule.WorkingDays;

public class WorkingDaysManager
{
	private readonly Dictionary<DateTime, decimal> _productionCalendarDays;

	private readonly Dictionary<string, decimal> _defaultDays;

	public WorkingDaysManager()
	{
		_productionCalendarDays = new Dictionary<DateTime, decimal>();
		_defaultDays = new Dictionary<string, decimal>();
	}

	public bool IsNonWorkingDay(DateTime date)
	{
		if (_productionCalendarDays.Count <= 0)
		{
			return IsNonWorkingDayDefault(date);
		}
		return IsNonWorkingDayProductionCalendar(date);
	}

	public bool TryAddProductionCalendar(DataTable productionCalendarResponse)
	{
		int num;
		if (productionCalendarResponse != null)
		{
			num = ((productionCalendarResponse.Rows.Count > 0) ? 1 : 0);
			if (num != 0)
			{
				StoreProductionCalendarResponse(productionCalendarResponse);
			}
		}
		else
		{
			num = 0;
		}
		return (byte)num != 0;
	}

	public void LoadProductionCalendarNonWorkingDays(DateTime since, DateTime until, M1Database database, ResourceCalendarDefinition calendar)
	{
		SqlCommand productionCalendarWorkingDaysQuery = GetProductionCalendarWorkingDaysQuery(since, until, database, calendar);
		DataTable dataTable = database.GetDataTable(productionCalendarWorkingDaysQuery);
		if (!TryAddProductionCalendar(dataTable) && !string.IsNullOrWhiteSpace(calendar.WorkCenterID))
		{
			productionCalendarWorkingDaysQuery.Parameters["@WorkCenterID"].Value = "";
			dataTable = database.GetDataTable(productionCalendarWorkingDaysQuery);
		}
		if (!TryAddProductionCalendar(dataTable) && !string.IsNullOrWhiteSpace(calendar.PlantID))
		{
			productionCalendarWorkingDaysQuery.Parameters["@PlantID"].Value = "";
			dataTable = database.GetDataTable(productionCalendarWorkingDaysQuery);
			TryAddProductionCalendar(dataTable);
		}
	}

	public SqlCommand GetProductionCalendarWorkingDaysQuery(DateTime since, DateTime until, M1Database database, ResourceCalendarDefinition calendar)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT jmyproductioncalendarday, jmyproductioncalendarmonth, jmyProductionCalendarYearID, jmyhours \r\n              FROM productioncalendardays                                                                       \r\n              WHERE DATEFROMPARTS (jmyproductioncalendaryearid, jmyProductionCalendarMonth, jmyProductionCalendarDay ) BETWEEN @StartDate AND @EndDate\r\n                    AND jmyplantid = @PlantID                                                                    \r\n                    AND jmyworkcenterid = @WorkCenterID                                                                   \r\n              ORDER BY jmyproductioncalendarmonth, jmyproductioncalendarday, jmyProductionCalendarYearID");
		sqlCommand.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime)).Value = since;
		sqlCommand.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime)).Value = until;
		sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = calendar.PlantID;
		sqlCommand.Parameters.Add(new SqlParameter("@WorkCenterID", SqlDbType.NVarChar)).Value = calendar.WorkCenterID;
		return sqlCommand;
	}

	public void LoadDefaultNonWorkingDays(M1Database database, ResourceCalendarDefinition calendar)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT xawhoursmon HoursMon, xawhourstue HoursTue, xawhourswed HoursWed, \r\n                                                              xawhoursthu HoursThu, xawhoursfri HoursFri, xawhourssat HoursSat, \r\n                                                              xawhourssun HoursSun                                             \r\n                                                         FROM   workcenters                                                       \r\n                                                         WHERE  xawworkcenterid = @WorkCenterId                                   \r\n                                                                AND xawEnableCalendar = 1                                         \r\n                                                         UNION ALL                                                                \r\n                                                         SELECT xauhoursmon HoursMon, xauhourstue HoursTue, xauhourswed HoursWed, \r\n                                                                xauhoursthu HoursThu, xauhoursfri HoursFri, xauhourssat HoursSat, \r\n                                                                xauhourssun HoursSun                                              \r\n                                                         FROM plants                                                              \r\n                                                         WHERE xauplantid = @PlantID                                              \r\n                                                         UNION ALL                                                                \r\n                                                         SELECT xadhoursmon HoursMon, xadhourstue HoursTue, xadhourswed HoursWed, \r\n                                                                xadhoursthu HoursThu, xadhoursfri HoursFri, xadhourssat HoursSat, \r\n                                                                xadhourssun HoursSun                                              \r\n                                                         FROM   datasetproperties");
		sqlCommand.Parameters.Add(new SqlParameter("@PlantID", SqlDbType.NVarChar)).Value = calendar.PlantID;
		sqlCommand.Parameters.Add(new SqlParameter("@WorkCenterID", SqlDbType.NVarChar)).Value = calendar.WorkCenterID;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable != null && dataTable.Rows.Count > 0)
		{
			DataRow row = dataTable.Rows[0];
			for (int i = 0; i < dataTable.Columns.Count; i++)
			{
				string text = dataTable.Columns[i].ToString();
				_defaultDays[text] = row.Field<decimal>(text);
			}
		}
	}

	private bool IsNonWorkingDayDefault(DateTime date)
	{
		string value = date.DayOfWeek.ToString().Substring(0, 3);
		StringBuilder stringBuilder = new StringBuilder("Hours").Append(value);
		if (_defaultDays.TryGetValue(stringBuilder.ToString(), out var value2))
		{
			return value2 == 0m;
		}
		return false;
	}

	private bool IsNonWorkingDayProductionCalendar(DateTime date)
	{
		if (_productionCalendarDays.TryGetValue(date.Date, out var value))
		{
			return value == 0m;
		}
		return false;
	}

	private void StoreProductionCalendarResponse(DataTable productionCalendarResponse)
	{
		foreach (DataRow row in productionCalendarResponse.Rows)
		{
			byte day = row.Field<byte>("jmyProductionCalendarDay");
			byte month = row.Field<byte>("jmyProductionCalendarMonth");
			short year = row.Field<short>("jmyProductionCalendarYearID");
			DateTime dateTime = new DateTime(year, month, day);
			_productionCalendarDays[dateTime.Date] = row.Field<decimal>("jmyhours");
		}
	}
}
