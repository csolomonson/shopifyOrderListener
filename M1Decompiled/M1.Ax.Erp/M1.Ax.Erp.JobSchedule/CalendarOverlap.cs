namespace M1.Ax.Erp.JobSchedule;

public class CalendarOverlap
{
	public int StartTimeMinutes;

	public int EndTimeMinutes;

	public int OverlapMinutes;

	public IResourceGroup ResourceGroup;

	public DayCalendar DayCalendar;

	public CalendarOverlap(int startTimeMinutes, int endTimeMinutes, IResourceGroup resourceGroup, DayCalendar dayCalendar)
	{
		StartTimeMinutes = startTimeMinutes;
		EndTimeMinutes = endTimeMinutes;
		OverlapMinutes = endTimeMinutes - startTimeMinutes;
		ResourceGroup = resourceGroup;
		DayCalendar = dayCalendar;
	}
}
