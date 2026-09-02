using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace M1.Ax.Erp.JobSchedule;

public class ScheduleTaskCollection : KeyedCollection<int, ScheduleTask>
{
	protected override int GetKeyForItem(ScheduleTask item)
	{
		return item.TaskID;
	}

	public void AddRange(IEnumerable<ScheduleTask> items)
	{
		foreach (ScheduleTask item in items)
		{
			Add(item);
		}
	}
}
