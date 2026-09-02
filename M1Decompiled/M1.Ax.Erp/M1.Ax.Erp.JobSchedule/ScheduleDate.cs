using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace M1.Ax.Erp.JobSchedule;

[ComVisible(true)]
[ComplexType]
public class ScheduleDate
{
	[Column("Date")]
	public DateTime? Date { get; private set; }

	[Column("Minute")]
	public short Minute { get; private set; }

	[Column("ActualDateTime")]
	public DateTime? ActualDateTime { get; private set; }

	public ScheduleDate(DateTime? date, short minute, DateTime? actualTime)
	{
		Date = date;
		Minute = minute;
		ActualDateTime = actualTime;
	}

	public ScheduleDate(DateTime? actualDateTime, ResourceCalendarDefinition calendar)
	{
		if (actualDateTime.HasValue)
		{
			ActualDateTime = actualDateTime;
			if (calendar != null)
			{
				DayCalendar dayCalendar = calendar.DayOfWeekDefaults[ActualDateTime.Value.DayOfWeek];
				if ((double)dayCalendar.StartTimeMinutes > ActualDateTime.Value.TimeOfDay.TotalMinutes)
				{
					Date = ActualDateTime.Value.Date;
					Minute = (short)(0.0 - ((double)dayCalendar.StartTimeMinutes - ActualDateTime.Value.TimeOfDay.TotalMinutes));
					Date = Date.Value.Date;
				}
				else
				{
					Date = ActualDateTime.Value.AddMinutes(0.0 - (double)dayCalendar.StartTimeMinutes);
					Minute = (short)(Date.Value.Hour * 60 + Date.Value.Minute);
					Date = Date.Value.Date;
				}
			}
			else
			{
				Date = ActualDateTime.Value;
				Minute = (short)(Date.Value.Hour * 60 + Date.Value.Minute);
				Date = Date.Value.Date;
			}
		}
		else
		{
			Date = null;
			Minute = 0;
			ActualDateTime = null;
		}
	}

	public override string ToString()
	{
		if (ActualDateTime.HasValue)
		{
			return ActualDateTime.Value.ToString("yyyy/MM/dd hh:mm:ss tt") + " [" + ((decimal)Minute / 60.0m).ToString("N2") + "]";
		}
		return "null";
	}
}
