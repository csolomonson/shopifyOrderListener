using System;
using System.Collections.Generic;

namespace M1.Ax.Erp.JobSchedule;

public class ScheduleResult
{
	public string JobID;

	public DateTime? EarliestMaterialDate;

	public DateTime? ScheduledDate;

	public double ScheduledHour;

	public double TotalHours;

	public List<string> Messages;

	public List<string> ChangedWorkCenters;
}
