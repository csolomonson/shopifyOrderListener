using System.Collections.ObjectModel;

namespace M1.Ax.Erp.JobSchedule;

public class ScheduleBranchCollection : KeyedCollection<int, ScheduleBranch>
{
	protected override int GetKeyForItem(ScheduleBranch item)
	{
		return item.BranchID;
	}
}
