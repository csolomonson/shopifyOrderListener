using System;
using System.Collections.Generic;

namespace M1.Ax.Erp.JobSchedule;

public class YearCalendar
{
	public readonly int Year;

	private readonly DayCalendar[] Days;

	public readonly int Count;

	public readonly DayOfWeek StartDayOfWeek;

	public List<DateTime> Holidays;

	public DayCalendar this[int index]
	{
		get
		{
			return GetDay(index);
		}
		set
		{
			SetDay(index, value);
		}
	}

	public YearCalendar(int year)
	{
		Year = year;
		StartDayOfWeek = new DateTime(year, 1, 1).DayOfWeek;
		Count = new DateTime(year, 12, 31).DayOfYear;
		Days = new DayCalendar[Count];
	}

	public YearCalendar(YearCalendar sourceYear)
	{
		Year = sourceYear.Year;
		StartDayOfWeek = sourceYear.StartDayOfWeek;
		Count = sourceYear.Count;
		Days = new DayCalendar[Count];
		Array.Copy(sourceYear.Days, Days, Count);
	}

	public DayCalendar GetDay(int dayOfYear)
	{
		return Days[dayOfYear - 1];
	}

	public void SetDay(int dayOfYear, DayCalendar day)
	{
		if (dayOfYear > 0)
		{
			Days[dayOfYear - 1] = day;
		}
	}
}
