using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace M1.Ax.Erp.JobSchedule;

[Table("ScheduleTrees")]
[TablePrefix("sxt")]
public class ScheduleTree : IDisposable, IEntityCreated, IEntityUniqueID
{
	public string SourceAssembliesTable;

	public object[] SourceKeyValues;

	public List<ScheduleTask> ScheduledOrder = new List<ScheduleTask>();

	public ScheduleTask FinalTask;

	public ScheduleTask StartTask;

	public List<ScheduleUnlinkedBranch> UnlinkedBranches = new List<ScheduleUnlinkedBranch>();

	public List<ScheduleTask> AllTasks = new List<ScheduleTask>();

	public List<ScheduleBranch> AllBranches = new List<ScheduleBranch>();

	[Column("sxtScheduleTreeID")]
	public int TreeID { get; set; }

	[Column("sxtSourceTable")]
	public string SourceTable { get; set; }

	[Column("sxtSourceUniqueID")]
	public Guid? SourceUniqueID { get; set; }

	[Column("sxtGroupUniqueID")]
	public Guid? GroupUniqueID { get; set; }

	[Column("sxtType")]
	public byte ScheduleType { get; set; }

	[Column("sxtDescription")]
	public string Description { get; set; }

	[Column("sxtCreatedBy")]
	public string CreatedBy { get; set; }

	[Column("sxtCreatedDate")]
	public DateTime? CreatedDate { get; set; }

	[Column("sxtUniqueID")]
	public Guid? UniqueID { get; set; }

	[Column("sxtJobScenarioID")]
	public string JobScenarioID { get; set; }

	public ScheduleTree(string createdBy)
	{
		CreatedDate = DateTime.Now;
		CreatedBy = createdBy;
		UniqueID = Guid.NewGuid();
	}

	public void Dispose()
	{
		StartTask = null;
		FinalTask = null;
		ScheduledOrder = null;
		if (AllTasks != null)
		{
			AllTasks.ForEach(delegate(ScheduleTask item)
			{
				item.Dispose();
			});
			AllTasks.Clear();
			AllTasks = null;
		}
		if (AllBranches != null)
		{
			AllBranches.ForEach(delegate(ScheduleBranch item)
			{
				item.Dispose();
			});
			AllBranches.Clear();
			AllBranches = null;
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (!string.IsNullOrWhiteSpace(SourceTable))
		{
			stringBuilder.Append(SourceTable);
		}
		if (SourceKeyValues != null)
		{
			stringBuilder.Append(" [");
			for (int i = 0; i < SourceKeyValues.Length; i++)
			{
				if (i != 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(SourceKeyValues[i].ToString());
			}
			stringBuilder.Append("]");
		}
		if (AllBranches != null)
		{
			stringBuilder.Append(", AsmCount = " + AllBranches.Count);
		}
		if (AllTasks != null)
		{
			stringBuilder.Append(", OprCount = " + AllTasks.Count);
		}
		stringBuilder.Append(", StartOpr = " + ((StartTask == null) ? "null" : (StartTask.BranchID + "-" + StartTask.TaskID)));
		stringBuilder.Append(", FinalOpr = " + ((FinalTask == null) ? "null" : (FinalTask.BranchID + "-" + FinalTask.TaskID)));
		if (ScheduledOrder != null)
		{
			stringBuilder.Append(", ScheduledCount = " + ScheduledOrder.Count);
		}
		return stringBuilder.ToString();
	}
}
