using M1.Extensions;

namespace M1.Ax.Erp.JobSchedule;

public class TimeBucket
{
	public readonly int TotalMinutes;

	public readonly int StartTimeMinutes;

	public readonly int EndTimeMinutes;

	public TimeBucket(decimal hours, decimal startTime)
	{
		TotalMinutes = M1Time.ConvertDecimalHoursToMinutes(hours);
		StartTimeMinutes = M1Time.ConvertDecimalTimeToMinutes(startTime);
		EndTimeMinutes = StartTimeMinutes + TotalMinutes;
	}

	public TimeBucket(decimal startTime, decimal endTime, bool calculateHours)
	{
		StartTimeMinutes = M1Time.ConvertDecimalTimeToMinutes(startTime);
		if (endTime < startTime)
		{
			endTime += 24.0m;
		}
		EndTimeMinutes = M1Time.ConvertDecimalTimeToMinutes(endTime);
		if (EndTimeMinutes >= StartTimeMinutes)
		{
			TotalMinutes = EndTimeMinutes - StartTimeMinutes;
		}
		else
		{
			TotalMinutes = StartTimeMinutes - EndTimeMinutes;
		}
	}

	public bool Equals(TimeBucket obj)
	{
		if (obj == null || obj.TotalMinutes != TotalMinutes || obj.StartTimeMinutes != StartTimeMinutes || obj.EndTimeMinutes != EndTimeMinutes)
		{
			return false;
		}
		return true;
	}
}
