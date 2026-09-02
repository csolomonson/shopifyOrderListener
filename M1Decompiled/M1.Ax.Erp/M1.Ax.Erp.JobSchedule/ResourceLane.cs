using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace M1.Ax.Erp.JobSchedule;

[Table("ScheduleResourceLanes")]
[TablePrefix("sxr")]
public class ResourceLane : ScheduleBaseFields
{
	private bool _OneResourcePerTask;

	private byte _ResourceType;

	public Dictionary<byte, LaneCell> Cells;

	public List<ScheduleAllocation> Allocations = new List<ScheduleAllocation>();

	[Column("sxrScheduleResourceLaneID")]
	public short LaneID { get; set; }

	public bool OneResourcePerTask
	{
		get
		{
			return _OneResourcePerTask;
		}
		set
		{
			_OneResourcePerTask = value;
		}
	}

	[Column("sxrResourceType")]
	public byte ResourceType
	{
		get
		{
			return _ResourceType;
		}
		set
		{
			_ResourceType = value;
			if (_ResourceType == ScheduleProcess.ResourceTypes.Shifts)
			{
				_OneResourcePerTask = false;
			}
			else
			{
				_OneResourcePerTask = true;
			}
		}
	}

	[Column("sxrGroupUniqueID")]
	public Guid? GroupUniqueID { get; set; }

	[Column("sxrLockedResourceUniqueID")]
	public Guid? LockedResourceUniqueID { get; set; }

	public ResourceLane(short id, Guid? groupUniqueID, byte resourceType, ScheduleTask sourceTask, DataRow[] laneCells)
	{
		SourceTask = sourceTask;
		UniqueID = Guid.NewGuid();
		LaneID = id;
		ResourceType = resourceType;
		GroupUniqueID = groupUniqueID;
		Cells = new Dictionary<byte, LaneCell>();
		if (laneCells != null)
		{
			foreach (DataRow row in laneCells)
			{
				Cells.Add(row.Field<byte>("sxcResourceCellID"), new LaneCell(this, row.Field<byte>("sxcResourceCellID"), row.Field<Guid?>("sxcResourceUniqueID"), row.Field<Guid?>("sxcUniqueID")));
			}
		}
	}

	public void SetAllocationIDs()
	{
		byte b = 0;
		foreach (ScheduleAllocation allocation in Allocations)
		{
			b = (allocation.AllocationID = (byte)(b + 1));
		}
	}

	public override string ToString()
	{
		return LaneID + " - " + ((ResourceType == ScheduleProcess.ResourceTypes.WorkCenters) ? "Machine" : ((ResourceType == ScheduleProcess.ResourceTypes.Shifts) ? "Employee" : ""));
	}
}
