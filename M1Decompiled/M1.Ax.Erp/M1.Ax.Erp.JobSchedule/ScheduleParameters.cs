using System;

namespace M1.Ax.Erp.JobSchedule;

public class ScheduleParameters
{
	public string ScenarioID = string.Empty;

	public int BaseAssemblyID;

	public int InitialAssemblyID;

	public int InitialOperationID;

	public byte InitialDateType;

	public DateTime InitialDate;

	public decimal InitialHour;

	public bool IgnoreOtherJobsForMachines;

	public bool IgnoreOtherJobsForEmployees = true;

	public bool IncludeSubSequentOperations = true;

	public bool IncludePreviousOperations = true;

	public ScheduleDirection Direction = ScheduleDirection.Backward;

	public ScheduleOperationScope OperationScope = (ScheduleOperationScope)7;

	public ScheduleAssemblyScope AssemblyScope = (ScheduleAssemblyScope)7;
}
