using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace M1.Ax.Erp.JobSchedule;

public class LaneCell : IEntityUniqueID
{
	public ResourceLane SourceLane;

	private int _TreeID;

	private int _BranchID;

	private int _TaskID;

	public Guid? ResourceUniqueID;

	[Column("sxcTreeID")]
	public int TreeID
	{
		get
		{
			if (_TreeID != 0)
			{
				return _TreeID;
			}
			if (SourceLane != null)
			{
				return SourceLane.TreeID;
			}
			return 0;
		}
		set
		{
			_TreeID = value;
		}
	}

	[Column("sxcBranchID")]
	public int BranchID
	{
		get
		{
			if (_BranchID != 0)
			{
				return _BranchID;
			}
			if (SourceLane != null)
			{
				return SourceLane.BranchID;
			}
			return 0;
		}
		set
		{
			_BranchID = value;
		}
	}

	[Column("sxcTaskID")]
	public int TaskID
	{
		get
		{
			if (_TaskID != 0)
			{
				return _TaskID;
			}
			if (SourceLane != null)
			{
				return SourceLane.TaskID;
			}
			return 0;
		}
		set
		{
			_TaskID = value;
		}
	}

	[Column("sxcResourceLaneID")]
	public short LaneID => SourceLane.LaneID;

	[Column("sxcResourceCellID")]
	public byte CellID { get; set; }

	public Guid? UniqueID { get; set; }

	public LaneCell(ResourceLane lane, byte cellID, Guid? resourceUniqueID, Guid? uniqueID)
	{
		SourceLane = lane;
		CellID = cellID;
		ResourceUniqueID = resourceUniqueID;
		UniqueID = uniqueID;
	}
}
