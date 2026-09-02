using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Ax.Erp.JobSchedule;
using M1.Core;

namespace M1.Ax.Erp;

public class ScheduleRescheduleProcess : ProcessParameters
{
	public ScheduleRescheduleProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[1] { "sxtScheduleTreeID" };
		KeyValueTableName = "ScheduleTrees";
		Description = "Select the schedules to be rescheduled.";
		GridID = "M1ADDFROMSCHEDULETREES";
		HelpLink = "SX_RefreshSched.htm";
		NullifyDateToSchedule();
		PromptFieldValidations.Add(new PromptFieldValidationBool("jmpClosed", fieldValue: false, "Job is closed."));
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Prod Due Dates")
		{
			IgnoreWhenEmpty = true,
			ValueField = "jmpProductionDueDate",
			AdditionalFields = "jmpProductionDueDate"
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Plants", null, new string[2] { "jmpPlantID", "jmpPlantDepartmentID" })
		{
			AdditionalFields = "jmpPlantID,jmpPlantDepartmentID",
			ValueFields = new string[2] { "jmpPlantID", "jmpPlantDepartmentID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Customers", null, new string[1] { "jmpCustomerOrganizationID" })
		{
			AdditionalFields = "jmpCustomerOrganizationID",
			ValueFields = new string[1] { "jmpCustomerOrganizationID" },
			IgnoreWhenEmpty = false
		});
		DefaultValueFieldNames = new string[4] { "ProductionProperties.xapJMScheduleType", "ProductionProperties.xapJMIgnoreMachines", "ProductionProperties.xapJMRefreshHours", "ProductionProperties.xapDateToSchedule" };
		ExtraFieldNames = new string[2] { "jmpJobPriorityID", "jmpClosed" };
	}

	private void NullifyDateToSchedule()
	{
		M1Database obj = ServiceProvider.GetService(typeof(M1Database)) as M1Database;
		SqlCommand sqlCommand = obj.NewSqlCommand("UPDATE ProductionProperties SET xapDateToSchedule=null");
		obj.ExecuteCommand(sqlCommand);
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		_ = arg.Messages;
		if (selectedItems.Count == 0)
		{
			return;
		}
		LoadJob loadJob = new LoadJob();
		LoadSchedule loadSchedule = new LoadSchedule();
		ScheduleSave scheduleSave = new ScheduleSave();
		ScheduleParameters scheduleParameters = new ScheduleParameters();
		M1Database m1Database = ServiceProvider.GetService(typeof(M1Database)) as M1Database;
		SqlCommand sqlCommand = m1Database.NewSqlCommand("Select jmpJobID From ScheduleTrees Inner Join Jobs On sxtGroupUniqueID = jmpUniqueID Where sxtScheduleTreeID = @TreeID And jmpClosed = 0");
		sqlCommand.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int));
		int num = selectedItems.Count((ProcessSelectedItemValues item) => item.EditableValues["DateToSchedule"] == DBNull.Value);
		if (num != 0)
		{
			if (arg.DefaultFieldValues["xapDateToSchedule"] == DBNull.Value)
			{
				arg.Cancel = true;
				arg.Messages.Add($"There are {num} item(s) with no date(s) entered for the Date To Schedule field in the grid.\n Fill this field in the grid or set a common date in the Date To Schedule field below the grid.");
				return;
			}
			foreach (ProcessSelectedItemValues selectedItem in arg.SelectedItems)
			{
				if (selectedItem.EditableValues["DateToSchedule"] == DBNull.Value)
				{
					selectedItem.EditableValues["DateToSchedule"] = Convert.ToDateTime(arg.DefaultFieldValues["xapDateToSchedule"]);
				}
			}
		}
		int num2 = selectedItems.Count((ProcessSelectedItemValues item) => item.ExtraFieldValues["jmpClosed"].Equals(true));
		if (num2 != 0)
		{
			arg.Cancel = true;
			arg.Messages.Add($"There are {num2} item(s) marked as closed. These items cannot be selected to be refreshed.");
			return;
		}
		bool flag = Convert.ToBoolean(arg.DefaultFieldValues["xapJMRefreshHours"]);
		byte b = Convert.ToByte(arg.DefaultFieldValues["xapJMScheduleType"]);
		scheduleParameters.IgnoreOtherJobsForMachines = Convert.ToBoolean(arg.DefaultFieldValues["xapJMIgnoreMachines"]);
		scheduleParameters.IgnoreOtherJobsForEmployees = true;
		SqlCommand sqlCommand2 = m1Database.NewSqlCommand("Delete From ScheduleAllocations Where sxdScheduleTreeID = @TreeID");
		sqlCommand2.Parameters.Add(new SqlParameter("@TreeID", SqlDbType.Int));
		foreach (ProcessSelectedItemValues item in from item in selectedItems
			orderby Convert.ToDecimal(item.ExtraFieldValues["jmpJobPriorityID"]) descending
			orderby Convert.ToDateTime(item.EditableValues["DateToSchedule"])
			orderby Convert.ToDecimal(item.EditableValues["HourToSchedule"])
			select item)
		{
			int num3 = Convert.ToInt32(item.KeyValues[0]);
			sqlCommand2.Parameters["@TreeID"].Value = num3;
			m1Database.ExecuteCommand(sqlCommand2);
		}
		using ScheduleCache cache = ScheduleProcess.LoadCache(m1Database);
		foreach (ProcessSelectedItemValues item2 in from item in selectedItems
			orderby Convert.ToDecimal(item.ExtraFieldValues["jmpJobPriorityID"]) descending
			orderby Convert.ToDateTime(item.EditableValues["DateToSchedule"])
			orderby Convert.ToDecimal(item.EditableValues["HourToSchedule"])
			select item)
		{
			int num3 = Convert.ToInt32(item2.KeyValues[0]);
			ScheduleTree scheduleTree = loadSchedule.Load(m1Database, num3, cache);
			sqlCommand.Parameters["@TreeID"].Value = num3;
			string text = Convert.ToString(m1Database.ExecuteScalar(sqlCommand));
			if (flag)
			{
				ScheduleProcess.CopyTaskHours(loadJob.Load(m1Database, new object[1] { text }, cache), scheduleTree);
			}
			scheduleParameters.InitialDate = (DateTime)item2.EditableValues["DateToSchedule"];
			scheduleParameters.InitialHour = (decimal)item2.EditableValues["HourToSchedule"];
			if (b == 2)
			{
				scheduleParameters.InitialDateType = 1;
				scheduleParameters.InitialAssemblyID = scheduleTree.StartTask.BranchID;
				scheduleParameters.InitialOperationID = scheduleTree.StartTask.TaskID;
				scheduleParameters.Direction = ScheduleDirection.Forward;
			}
			else
			{
				scheduleParameters.InitialDateType = 5;
				scheduleParameters.InitialAssemblyID = scheduleTree.FinalTask.BranchID;
				scheduleParameters.InitialOperationID = scheduleTree.FinalTask.TaskID;
				scheduleParameters.Direction = ScheduleDirection.Backward;
			}
			scheduleParameters.AssemblyScope = (ScheduleAssemblyScope)7;
			scheduleParameters.OperationScope = (ScheduleOperationScope)7;
			ScheduleProcess.StartSchedule(m1Database, cache, scheduleParameters, scheduleTree);
			scheduleSave.SaveSchedule(m1Database, scheduleTree);
			if (scheduleTree.ScheduleType == 1 && !string.IsNullOrWhiteSpace(text))
			{
				ScheduleProcess.ScheduleRefreshOpsAndMat(m1Database, text, 0);
			}
			ScheduleProcess.LoadAllocationsIntoResourceGroups(scheduleTree, cache);
		}
	}
}
