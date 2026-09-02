using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace M1.Ax.Erp.JobSchedule;

[Table("ScheduleBranches")]
[TablePrefix("sxb")]
public class ScheduleBranch : IDisposable, IEntityCreated, IEntityUniqueID
{
	public ScheduleBranch ParentBranch;

	private ScheduleTree _Source;

	public ScheduleBranchCollection Branches = new ScheduleBranchCollection();

	public ScheduleBranchCollection UnlinkedBranches = new ScheduleBranchCollection();

	public ScheduleTaskCollection Tasks = new ScheduleTaskCollection();

	public List<ScheduleTask> CurrentAndSubTasks = new List<ScheduleTask>();

	public List<ScheduleBranch> CurrentAndSubBranches = new List<ScheduleBranch>();

	public ScheduleTask FinalTask;

	public ScheduleTask StartTask;

	public List<ScheduleTaskCollection> DistinctTaskLists;

	public ScheduleTree Source
	{
		get
		{
			return _Source;
		}
		set
		{
			if (_Source != value)
			{
				_Source = value;
				TreeID = _Source.TreeID;
			}
		}
	}

	[Column("sxbScheduleTreeID")]
	public int TreeID { get; set; }

	[Column("sxbScheduleBranchID")]
	public int BranchID { get; set; }

	[Column("sxbParentScheduleBranchID")]
	public int ParentBranchID { get; set; }

	[Column("sxbSiblingBranchLink")]
	public BranchOverlap SiblingBranchOverlap { get; set; }

	[Column("sxbParentLinkedTaskID")]
	public int ParentBranchTaskID { get; set; }

	[Column("sxbCurrentLinkedTaskID")]
	public int CurrentBranchLinkedTaskID { get; set; }

	public ScheduleTask CurrentBranchLinkedTask { get; set; }

	[Column("sxbCurrentLinkedTaskDateType")]
	public byte OverlapSourceLink { get; set; }

	[Column("sxbParentLinkedTaskDateType")]
	public byte OverlapDestinationLink { get; set; }

	[Column("sxbOffsetMinutes")]
	public int OverlapOffsetMinutes { get; set; }

	[Column("sxbCreatedBy")]
	public string CreatedBy { get; set; }

	[Column("sxbCreatedDate")]
	public DateTime? CreatedDate { get; set; }

	[Column("sxbUniqueID")]
	public Guid? UniqueID { get; set; }

	public ScheduleBranch(ScheduleTree source, int assemblyID, int parentAssemblyID, int currentAssemblyLinkedOperationID, byte sourceLink, int parentAssemblyOperationID, byte destinationLink, int offsetMinutes, BranchOverlap assemblyOverlap, string createdBy)
	{
		CreatedBy = createdBy;
		CreatedDate = DateTime.Now;
		UniqueID = Guid.NewGuid();
		Source = source;
		BranchID = assemblyID;
		ParentBranchID = parentAssemblyID;
		ParentBranchTaskID = parentAssemblyOperationID;
		OverlapSourceLink = sourceLink;
		CurrentBranchLinkedTaskID = currentAssemblyLinkedOperationID;
		OverlapDestinationLink = destinationLink;
		OverlapOffsetMinutes = offsetMinutes;
		SiblingBranchOverlap = assemblyOverlap;
	}

	public void Dispose()
	{
		if (UnlinkedBranches != null)
		{
			foreach (ScheduleBranch unlinkedBranch in UnlinkedBranches)
			{
				unlinkedBranch.Dispose();
			}
			UnlinkedBranches.Clear();
			UnlinkedBranches = null;
		}
		if (Tasks != null)
		{
			foreach (ScheduleTask task in Tasks)
			{
				task.Dispose();
			}
			Tasks.Clear();
			Tasks = null;
		}
		ParentBranch = null;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("Asm {0} - OprCount = {1} - OprLink = {2}", BranchID, Tasks.Count, (CurrentBranchLinkedTask == null) ? "null" : CurrentBranchLinkedTask.TaskID.ToString());
		stringBuilder.Append(", StartOpr = " + ((StartTask == null) ? "null" : (StartTask.BranchID + "-" + StartTask.TaskID)));
		stringBuilder.Append(", FinalOpr = " + ((FinalTask == null) ? "null" : (FinalTask.BranchID + "-" + FinalTask.TaskID)));
		return stringBuilder.ToString();
	}
}
