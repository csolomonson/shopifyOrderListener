using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace M1.Ax.Erp.JobSchedule;

public class ScheduleBaseFields : IEntityUniqueID
{
	public ScheduleTask SourceTask;

	protected int _TreeID;

	protected int _BranchID;

	protected int _TaskID;

	[Column("ScheduleTreeID")]
	public virtual int TreeID
	{
		get
		{
			if (_TreeID != 0)
			{
				return _TreeID;
			}
			if (SourceTask != null)
			{
				return SourceTask.TreeID;
			}
			return 0;
		}
		set
		{
			_TreeID = value;
		}
	}

	[Column("ScheduleBranchID")]
	public virtual int BranchID
	{
		get
		{
			if (_BranchID != 0)
			{
				return _BranchID;
			}
			if (SourceTask != null)
			{
				return SourceTask.BranchID;
			}
			return 0;
		}
		set
		{
			_BranchID = value;
		}
	}

	[Column("ScheduleTaskID")]
	public virtual int TaskID
	{
		get
		{
			if (_TaskID != 0)
			{
				return _TaskID;
			}
			if (SourceTask != null)
			{
				return SourceTask.TaskID;
			}
			return 0;
		}
		set
		{
			_TaskID = value;
		}
	}

	public virtual Guid? UniqueID { get; set; }
}
