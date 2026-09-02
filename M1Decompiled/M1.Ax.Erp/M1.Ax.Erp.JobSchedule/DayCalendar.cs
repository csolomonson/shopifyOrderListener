using System;
using M1.Extensions;

namespace M1.Ax.Erp.JobSchedule;

public class DayCalendar
{
	private TimeBucket[] TimeBuckets;

	public TimeSpan Total;

	public readonly int StartTimeMinutes;

	public readonly int EndTimeMinutes;

	public readonly int AdjustedEndTimeMinutes;

	public byte Count;

	public DayCalendar(params TimeBucket[] parms)
	{
		TimeBuckets = parms;
		int num = 0;
		Count = (byte)TimeBuckets.Length;
		for (byte b = 0; b < Count; b++)
		{
			num += TimeBuckets[b].TotalMinutes;
		}
		Total = TimeSpan.FromMinutes(num);
		if (Count != 0)
		{
			StartTimeMinutes = TimeBuckets[0].StartTimeMinutes;
			EndTimeMinutes = TimeBuckets[Count - 1].EndTimeMinutes;
			if (StartTimeMinutes > EndTimeMinutes)
			{
				AdjustedEndTimeMinutes = EndTimeMinutes + 1440;
			}
			else
			{
				AdjustedEndTimeMinutes = EndTimeMinutes;
			}
		}
	}

	public DayCalendar(int startTime, int endTime)
	{
		StartTimeMinutes = M1Time.ConvertDecimalTimeToMinutes(startTime);
		EndTimeMinutes = M1Time.ConvertDecimalTimeToMinutes(endTime);
		Total = TimeSpan.FromMinutes(endTime - startTime);
	}

	public bool Equals(DayCalendar obj)
	{
		if (obj == null || obj.Total.TotalMinutes != Total.TotalMinutes || obj.StartTimeMinutes != StartTimeMinutes || obj.EndTimeMinutes != EndTimeMinutes || obj.Count != Count)
		{
			return false;
		}
		for (byte b = 0; b < Count; b++)
		{
			if (!TimeBuckets[b].Equals(obj.TimeBuckets[b]))
			{
				return false;
			}
		}
		return true;
	}

	public CalendarOverlap GetOverlapTime(DayCalendar dayCalendar, IResourceGroup shift)
	{
		return GetOverlapTime(dayCalendar.StartTimeMinutes, dayCalendar.AdjustedEndTimeMinutes, shift);
	}

	public CalendarOverlap GetOverlapTime(int startTimeMinutes, int endTimeMinutes, IResourceGroup shift)
	{
		int endTimeMinutes2 = ((AdjustedEndTimeMinutes >= endTimeMinutes) ? endTimeMinutes : AdjustedEndTimeMinutes);
		int startTimeMinutes2 = ((StartTimeMinutes <= startTimeMinutes) ? startTimeMinutes : StartTimeMinutes);
		return new CalendarOverlap(startTimeMinutes2, endTimeMinutes2, shift, this);
	}
}
