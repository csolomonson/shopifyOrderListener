using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace M1.Ax.Erp.JobSchedule;

public class ResourceCalendarDefinition : IDisposable
{
	private ResourceCalendarDefinition _ParentCalendar;

	private Dictionary<DayOfWeek, DayCalendar> _DayOfWeekDefaults = new Dictionary<DayOfWeek, DayCalendar>();

	private Dictionary<int, YearCalendar> _LoadedYears = new Dictionary<int, YearCalendar>();

	public ResourceCalendarDefinition ParentCalendar => _ParentCalendar;

	public string PlantID { get; set; }

	public string WorkCenterID { get; set; }

	public string CalendarTable { get; set; }

	[DebuggerDisplay("Mon S={DayOfWeekDefaults[System.DayOfWeek.Monday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Monday].Total.TotalMinutes}, Tue S={DayOfWeekDefaults[System.DayOfWeek.Tuesday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Tuesday].Total.TotalMinutes}, Wed S={DayOfWeekDefaults[System.DayOfWeek.Wednesday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Wednesday].Total.TotalMinutes}, Thu S={DayOfWeekDefaults[System.DayOfWeek.Thursday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Thursday].Total.TotalMinutes}, Fri S={DayOfWeekDefaults[System.DayOfWeek.Friday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Friday].Total.TotalMinutes}, Sat S={DayOfWeekDefaults[System.DayOfWeek.Saturday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Saturday].Total.TotalMinutes}, Sun S={DayOfWeekDefaults[System.DayOfWeek.Sunday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Sunday].Total.TotalMinutes}")]
	public Dictionary<DayOfWeek, DayCalendar> DayOfWeekDefaults
	{
		get
		{
			return _DayOfWeekDefaults;
		}
		set
		{
			_DayOfWeekDefaults = value;
		}
	}

	public Dictionary<int, YearCalendar> LoadedYears
	{
		get
		{
			return _LoadedYears;
		}
		set
		{
			_LoadedYears = value;
		}
	}

	public ResourceCalendarDefinition(ResourceCalendarDefinition parentCalendar, string calendarTable, DataRow row)
	{
		_ParentCalendar = parentCalendar;
		CalendarTable = calendarTable;
		if (calendarTable.Equals("WorkCenters", StringComparison.CurrentCultureIgnoreCase))
		{
			PlantID = row.Field<string>("xawPlantID");
			WorkCenterID = row.Field<string>("xawWorkCenterID");
			DayOfWeekDefaults.Add(DayOfWeek.Sunday, loadDayData(row.Field<decimal>("xawHoursSun"), row.Field<decimal>("xawDayStartTimeSun")));
			DayOfWeekDefaults.Add(DayOfWeek.Monday, loadDayData(row.Field<decimal>("xawHoursMon"), row.Field<decimal>("xawDayStartTimeMon")));
			DayOfWeekDefaults.Add(DayOfWeek.Tuesday, loadDayData(row.Field<decimal>("xawHoursTue"), row.Field<decimal>("xawDayStartTimeTue")));
			DayOfWeekDefaults.Add(DayOfWeek.Wednesday, loadDayData(row.Field<decimal>("xawHoursWed"), row.Field<decimal>("xawDayStartTimeWed")));
			DayOfWeekDefaults.Add(DayOfWeek.Thursday, loadDayData(row.Field<decimal>("xawHoursThu"), row.Field<decimal>("xawDayStartTimeThu")));
			DayOfWeekDefaults.Add(DayOfWeek.Friday, loadDayData(row.Field<decimal>("xawHoursFri"), row.Field<decimal>("xawDayStartTimeFri")));
			DayOfWeekDefaults.Add(DayOfWeek.Saturday, loadDayData(row.Field<decimal>("xawHoursSat"), row.Field<decimal>("xawDayStartTimeSat")));
		}
		else if (calendarTable.Equals("Plants", StringComparison.CurrentCultureIgnoreCase))
		{
			PlantID = row.Field<string>("xauPlantID");
			WorkCenterID = string.Empty;
			DayOfWeekDefaults.Add(DayOfWeek.Sunday, loadDayData(row.Field<decimal>("xauHoursSun"), row.Field<decimal>("xauDayStartTimeSun")));
			DayOfWeekDefaults.Add(DayOfWeek.Monday, loadDayData(row.Field<decimal>("xauHoursMon"), row.Field<decimal>("xauDayStartTimeMon")));
			DayOfWeekDefaults.Add(DayOfWeek.Tuesday, loadDayData(row.Field<decimal>("xauHoursTue"), row.Field<decimal>("xauDayStartTimeTue")));
			DayOfWeekDefaults.Add(DayOfWeek.Wednesday, loadDayData(row.Field<decimal>("xauHoursWed"), row.Field<decimal>("xauDayStartTimeWed")));
			DayOfWeekDefaults.Add(DayOfWeek.Thursday, loadDayData(row.Field<decimal>("xauHoursThu"), row.Field<decimal>("xauDayStartTimeThu")));
			DayOfWeekDefaults.Add(DayOfWeek.Friday, loadDayData(row.Field<decimal>("xauHoursFri"), row.Field<decimal>("xauDayStartTimeFri")));
			DayOfWeekDefaults.Add(DayOfWeek.Saturday, loadDayData(row.Field<decimal>("xauHoursSat"), row.Field<decimal>("xauDayStartTimeSat")));
		}
	}

	public ResourceCalendarDefinition(ResourceCalendarDefinition parentCalendar, string calendarTable, DataRow[] shiftBreaks)
	{
		_ParentCalendar = parentCalendar;
		CalendarTable = calendarTable;
		foreach (DataRow dataRow in shiftBreaks)
		{
			byte b = dataRow.Field<byte>("lmtDay");
			if (b == 7)
			{
				b = 0;
			}
			DayOfWeekDefaults.Add((DayOfWeek)b, loadDayData(dataRow));
		}
	}

	private DayCalendar loadDayData(decimal hours, decimal startTime)
	{
		return new DayCalendar(new TimeBucket(hours, startTime));
	}

	private DayCalendar loadDayData(DataRow breakRow)
	{
		List<TimeBucket> list = new List<TimeBucket>();
		string columnName = "lmtStartTime";
		if (breakRow.Field<decimal>("lmtBreak1StartTime") != breakRow.Field<decimal>("lmtBreak1EndTime"))
		{
			list.Add(new TimeBucket(breakRow.Field<decimal>(columnName), breakRow.Field<decimal>("lmtBreak1StartTime"), calculateHours: true));
			columnName = "lmtBreak1EndTime";
		}
		if (breakRow.Field<decimal>("lmtBreak2StartTime") != breakRow.Field<decimal>("lmtBreak2EndTime"))
		{
			list.Add(new TimeBucket(breakRow.Field<decimal>(columnName), breakRow.Field<decimal>("lmtBreak2StartTime"), calculateHours: true));
			columnName = "lmtBreak2EndTime";
		}
		if (breakRow.Field<decimal>("lmtBreak3StartTime") != breakRow.Field<decimal>("lmtBreak3EndTime"))
		{
			list.Add(new TimeBucket(breakRow.Field<decimal>(columnName), breakRow.Field<decimal>("lmtBreak3StartTime"), calculateHours: true));
			columnName = "lmtBreak3EndTime";
		}
		if (breakRow.Field<decimal>(columnName) != breakRow.Field<decimal>("lmtEndTime"))
		{
			list.Add(new TimeBucket(breakRow.Field<decimal>(columnName), breakRow.Field<decimal>("lmtEndTime"), calculateHours: true));
		}
		return new DayCalendar(list.ToArray());
	}

	public void Dispose()
	{
		if (DayOfWeekDefaults != null)
		{
			DayOfWeekDefaults.Clear();
			DayOfWeekDefaults = null;
		}
	}
}
