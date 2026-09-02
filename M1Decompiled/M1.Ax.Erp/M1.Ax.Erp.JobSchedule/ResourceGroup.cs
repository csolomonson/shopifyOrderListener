using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace M1.Ax.Erp.JobSchedule;

[DebuggerDisplay("{Calendar} NumOfResources = {ResourceGuids.Count} Mon S={DayOfWeekDefaults[System.DayOfWeek.Monday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Monday].Total.TotalMinutes}, Tue S={DayOfWeekDefaults[System.DayOfWeek.Tuesday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Tuesday].Total.TotalMinutes}, Wed S={DayOfWeekDefaults[System.DayOfWeek.Wednesday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Wednesday].Total.TotalMinutes}, Thu S={DayOfWeekDefaults[System.DayOfWeek.Thursday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Thursday].Total.TotalMinutes}, Fri S={DayOfWeekDefaults[System.DayOfWeek.Friday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Friday].Total.TotalMinutes}, Sat S={DayOfWeekDefaults[System.DayOfWeek.Saturday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Saturday].Total.TotalMinutes}, Sun S={DayOfWeekDefaults[System.DayOfWeek.Sunday].StartTimeMinutes} H={DayOfWeekDefaults[System.DayOfWeek.Sunday].Total.TotalMinutes}")]
public class ResourceGroup : IResourceGroup, IDisposable
{
	public decimal FiniteTolerance;

	private List<Guid> _ResourceGuids = new List<Guid>();

	public object DisplayID { get; set; }

	public ResourceCalendarDefinition Calendar { get; set; }

	public Guid GroupID { get; set; }

	public byte ResourceType { get; set; }

	public string PlantID { get; set; }

	public string ProcessID { get; set; }

	public bool InfiniteCapacity { get; set; }

	public short PeoplePerMachineSetup { get; set; }

	public short PeoplePerMachineProduction { get; set; }

	public List<Guid> ResourceGuids => _ResourceGuids;

	public ResourceGroup(ResourceCalendarDefinition calendar, DataRow row, DataRow[] workcenterMachines)
	{
		Calendar = calendar;
		ResourceType = ScheduleProcess.ResourceTypes.WorkCenters;
		DisplayID = row.Field<string>("xawWorkCenterID");
		GroupID = row.Field<Guid>("xawUniqueID");
		PlantID = row.Field<string>("xawPlantID");
		ProcessID = row.Field<string>("xawProcessID");
		FiniteTolerance = row.Field<decimal>("xawFiniteTolerance");
		InfiniteCapacity = row.Field<bool>("xawInfiniteCapacity");
		PeoplePerMachineSetup = row.Field<short>("xawPeoplePerMachineSetup");
		PeoplePerMachineProduction = row.Field<short>("xawPeoplePerMachineProd");
		foreach (DataRow row2 in workcenterMachines)
		{
			ResourceGuids.Add(row2.Field<Guid>("xaqUniqueID"));
		}
	}

	public ResourceGroup(ResourceCalendarDefinition calendar, short id, Guid groupID, string plantID)
	{
		Calendar = calendar;
		FiniteTolerance = default(decimal);
		InfiniteCapacity = false;
		PeoplePerMachineSetup = 0;
		PeoplePerMachineProduction = 0;
		PlantID = plantID;
		ProcessID = null;
		ResourceType = ScheduleProcess.ResourceTypes.Shifts;
		DisplayID = id;
		GroupID = groupID;
	}

	public void Dispose()
	{
	}
}
