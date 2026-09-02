using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Ax.Erp.JobSchedule;
using M1.Core;

namespace M1.Ax.Erp;

public class TransferJobToScheduleProcess : ProcessParameters
{
	public TransferJobToScheduleProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[1] { "jmpJobID" };
		KeyValueTableName = "Jobs";
		Description = "Select the jobs to be scheduled.";
		GridID = "M1ADDFROMSCHEDULEJOBS";
		HelpLink = "SX_SchedJobs.htm";
		NullifyDateToSchedule();
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Unscheduled Jobs Only?")
		{
			AdoFilterExpression = "jmpScheduleComplete = 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "jmpScheduleComplete"
		});
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
		DefaultValueFieldNames = new string[3] { "ProductionProperties.xapJMScheduleType", "ProductionProperties.xapJMIgnoreMachines", "ProductionProperties.xapDateToSchedule" };
		ExtraFieldNames = new string[1] { "jmpJobPriorityID" };
	}

	private void NullifyDateToSchedule()
	{
		M1Database obj = ServiceProvider.GetService(typeof(M1Database)) as M1Database;
		SqlCommand sqlCommand = obj.NewSqlCommand("UPDATE ProductionProperties SET xapDateToSchedule=null");
		obj.ExecuteCommand(sqlCommand);
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		if (selectedItems.Count == 0)
		{
			return;
		}
		LoadJob loadJob = new LoadJob();
		ScheduleSave scheduleSave = new ScheduleSave();
		ScheduleParameters scheduleParameters = new ScheduleParameters();
		M1Database database = ServiceProvider.GetService(typeof(M1Database)) as M1Database;
		int num = selectedItems.Count((ProcessSelectedItemValues item) => item.EditableValues["DateToSchedule"] == DBNull.Value);
		if (num != 0)
		{
			if (arg.DefaultFieldValues["xapDateToSchedule"] == DBNull.Value)
			{
				arg.Cancel = true;
				arg.Messages.Add($"There are {num} item(s) with no date(s) entered for the Date To Schedule field in the grid.\n Fill this field in the grid or set a common date in the Date To Schedule field below the grid.");
				return;
			}
			foreach (ProcessSelectedItemValues item in arg.SelectedItems.Where((ProcessSelectedItemValues item) => item.EditableValues["DateToSchedule"] == DBNull.Value))
			{
				item.EditableValues["DateToSchedule"] = Convert.ToDateTime(arg.DefaultFieldValues["xapDateToSchedule"]);
			}
		}
		byte b = Convert.ToByte(arg.DefaultFieldValues["xapJMScheduleType"]);
		scheduleParameters.IgnoreOtherJobsForMachines = Convert.ToBoolean(arg.DefaultFieldValues["xapJMIgnoreMachines"]);
		scheduleParameters.IgnoreOtherJobsForEmployees = true;
		Job job = new Job();
		foreach (ProcessSelectedItemValues item2 in from item in selectedItems
			orderby Convert.ToDecimal(item.ExtraFieldValues["jmpJobPriorityID"]) descending, Convert.ToDateTime(item.EditableValues["DateToSchedule"]), Convert.ToDecimal(item.EditableValues["HourToSchedule"])
			select item)
		{
			string jobID = item2.KeyValues[0].ToString();
			job.UnscheduleJob(database, jobID);
		}
		using ScheduleCache cache = ScheduleProcess.LoadCache(database);
		foreach (ProcessSelectedItemValues item3 in from item in selectedItems
			orderby Convert.ToDecimal(item.ExtraFieldValues["jmpJobPriorityID"]) descending, Convert.ToDateTime(item.EditableValues["DateToSchedule"]), Convert.ToDecimal(item.EditableValues["HourToSchedule"])
			select item)
		{
			string jobID = item3.KeyValues[0].ToString();
			object[] sourceKeyValues = new string[1] { jobID };
			ScheduleTree scheduleTree = loadJob.Load(database, sourceKeyValues, cache);
			scheduleParameters.InitialDate = (DateTime)item3.EditableValues["DateToSchedule"];
			scheduleParameters.InitialHour = (decimal)item3.EditableValues["HourToSchedule"];
			if (b == 2)
			{
				scheduleParameters.InitialDateType = 1;
				if (scheduleTree.StartTask != null)
				{
					scheduleParameters.InitialAssemblyID = scheduleTree.StartTask.BranchID;
					scheduleParameters.InitialOperationID = scheduleTree.StartTask.TaskID;
				}
				else
				{
					DataTable firstOrLastTaskOperation = ScheduleOperators.GetFirstOrLastTaskOperation(jobID, "ASC", database);
					scheduleParameters.InitialAssemblyID = Convert.ToInt32(firstOrLastTaskOperation.Rows[0].ItemArray[0].ToString());
					scheduleParameters.InitialOperationID = Convert.ToInt32(firstOrLastTaskOperation.Rows[0].ItemArray[1].ToString());
				}
				scheduleParameters.Direction = ScheduleDirection.Forward;
			}
			else
			{
				scheduleParameters.InitialDateType = 5;
				if (scheduleTree.FinalTask != null)
				{
					scheduleParameters.InitialAssemblyID = scheduleTree.FinalTask.BranchID;
					scheduleParameters.InitialOperationID = scheduleTree.FinalTask.TaskID;
				}
				else
				{
					DataTable firstOrLastTaskOperation2 = ScheduleOperators.GetFirstOrLastTaskOperation(jobID, "DESC", database);
					scheduleParameters.InitialAssemblyID = Convert.ToInt32(firstOrLastTaskOperation2.Rows[0].ItemArray[0].ToString());
					scheduleParameters.InitialOperationID = Convert.ToInt32(firstOrLastTaskOperation2.Rows[0].ItemArray[1].ToString());
				}
				scheduleParameters.Direction = ScheduleDirection.Backward;
			}
			scheduleParameters.AssemblyScope = (ScheduleAssemblyScope)7;
			scheduleParameters.OperationScope = (ScheduleOperationScope)7;
			ScheduleProcess.StartSchedule(database, cache, scheduleParameters, scheduleTree);
			scheduleSave.SaveSchedule(database, scheduleTree);
			if (scheduleTree.ScheduleType == 1)
			{
				ScheduleProcess.ScheduleRefreshOpsAndMat(database, jobID, 0);
			}
			ScheduleProcess.LoadAllocationsIntoResourceGroups(scheduleTree, cache);
		}
	}
}
