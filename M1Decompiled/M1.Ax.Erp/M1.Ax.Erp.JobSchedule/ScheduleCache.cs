using System;
using System.Collections.Generic;

namespace M1.Ax.Erp.JobSchedule;

public class ScheduleCache : IDisposable
{
	public class LoadedResourceCache
	{
		public string ID;

		public Dictionary<Guid, ScheduleAllocation> ResourceAllocations = new Dictionary<Guid, ScheduleAllocation>();

		public Dictionary<int, bool[]> LoadedYears = new Dictionary<int, bool[]>();
	}

	public string ScenarioID = string.Empty;

	public Dictionary<string, ResourceCalendarDefinition> PlantCalendars;

	public Dictionary<string, WorkProcess> Processes;

	public Dictionary<byte, Dictionary<Guid, IResourceGroup>> ResourceGroups = new Dictionary<byte, Dictionary<Guid, IResourceGroup>>();

	public Dictionary<byte, Dictionary<DayOfWeek, List<CalendarDayOfWeekInfo>>> CalendarMatrix;

	public Dictionary<byte, ScheduleType> ScheduleTypes = new Dictionary<byte, ScheduleType>();

	protected Dictionary<byte, Dictionary<Guid, LoadedResourceCache>> LoadedResources = new Dictionary<byte, Dictionary<Guid, LoadedResourceCache>>();

	public ScheduleTypeBucket GetTypeBucket(byte typeID, byte typeBucketID)
	{
		return ScheduleTypes[typeID].GetByID(typeBucketID);
	}

	public bool IsYearMonthLoaded(byte resourceType, Guid id, int year, int month)
	{
		LoadedResourceCache resources = GetResources(resourceType, id);
		if (resources.LoadedYears.ContainsKey(year))
		{
			return resources.LoadedYears[year][month - 1];
		}
		return false;
	}

	public LoadedResourceCache GetResources(byte resourceType, Guid id)
	{
		if (!LoadedResources.ContainsKey(resourceType))
		{
			LoadedResources.Add(resourceType, new Dictionary<Guid, LoadedResourceCache>());
		}
		Dictionary<Guid, LoadedResourceCache> dictionary = LoadedResources[resourceType];
		if (!dictionary.ContainsKey(id))
		{
			dictionary.Add(id, new LoadedResourceCache());
		}
		return dictionary[id];
	}

	public void SetYearMonthLoaded(byte resourceType, Guid id, int year, int month)
	{
		LoadedResourceCache resources = GetResources(resourceType, id);
		if (!resources.LoadedYears.ContainsKey(year))
		{
			resources.LoadedYears.Add(year, new bool[12]);
		}
		resources.LoadedYears[year][month - 1] = true;
	}

	public void Dispose()
	{
		if (ResourceGroups != null)
		{
			foreach (Dictionary<Guid, IResourceGroup> value in ResourceGroups.Values)
			{
				foreach (KeyValuePair<Guid, IResourceGroup> item in value)
				{
					item.Value.Dispose();
				}
				value.Clear();
			}
			ResourceGroups = null;
		}
		if (PlantCalendars != null)
		{
			foreach (ResourceCalendarDefinition value2 in PlantCalendars.Values)
			{
				value2.Dispose();
			}
			PlantCalendars.Clear();
			PlantCalendars = null;
		}
		if (Processes != null)
		{
			Processes.Clear();
			Processes = null;
		}
	}
}
