using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Ax.Erp.JobSchedule;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Schedule")]
[ComVisible(true)]
public class AppAxSchedule : IDisposable
{
	private IServiceProvider provider;

	private M1Database database;

	public AppAxSchedule(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public void DeleteScheduleForScenario(string scenarioID, object transaction)
	{
		ScheduleProcess.DeleteScheduleForScenario(database, scenarioID, (SqlTransaction)transaction);
	}

	public void CopyScheduleToScenario(string sourceScenarioID, string destScenarioID)
	{
		ScheduleProcess.CopyScheduleToScenario(database, sourceScenarioID, destScenarioID, null);
	}

	public ScriptWorkingRangeDictionary GetWorkingDaysInRange(object startDate, object startHour, object endDate, object endHour)
	{
		ScriptWorkingRangeDictionary scriptWorkingRangeDictionary = new ScriptWorkingRangeDictionary();
		DateTime startDate2 = Convert.ToDateTime(startDate);
		DateTime endDate2 = Convert.ToDateTime(endDate);
		Convert.ToDecimal(startHour);
		Convert.ToDecimal(endHour);
		using ScheduleCache scheduleCache = ScheduleProcess.LoadCache(database);
		int num = 0;
		decimal num2 = default(decimal);
		foreach (KeyValuePair<string, ResourceCalendarDefinition> plantCalendar in scheduleCache.PlantCalendars)
		{
			num = 0;
			num2 = default(decimal);
			foreach (KeyValuePair<DateTime, StartTimeAndHours> item in ScheduleProcess.GetWorkingDaysInRange(database, scheduleCache, plantCalendar.Value, startDate2, endDate2))
			{
				num++;
				num2 += item.Value.Hours;
			}
			scriptWorkingRangeDictionary.Add(plantCalendar.Value.PlantID, new ScriptWorkingRangeInfo
			{
				PlantID = plantCalendar.Value.PlantID,
				Days = num,
				Hours = num2
			});
		}
		return scriptWorkingRangeDictionary;
	}

	public ScheduleDate GetScheduleDate(string jobId, int assemblyId, int operationId, object initialDate, decimal initialHour, int treeId = 0)
	{
		ScheduleDate scheduleDate = null;
		LoadJob loadJob = new LoadJob();
		LoadSchedule loadSchedule = new LoadSchedule();
		DateTime value = Convert.ToDateTime(initialDate);
		object[] sourceKeyValues = new object[1] { jobId };
		using ScheduleCache cache = ScheduleProcess.LoadCache(database);
		using ScheduleTree scheduleTree = ((treeId != 0) ? loadSchedule.Load(database, treeId, cache) : loadJob.Load(database, sourceKeyValues, cache));
		ScheduleTask scheduleTask = scheduleTree.AllBranches.Find((ScheduleBranch item) => item.BranchID == assemblyId).CurrentAndSubTasks.Find((ScheduleTask item) => item.BranchID == assemblyId && item.TaskID == operationId);
		return ScheduleProcess.NewDate(database, (scheduleTask == null) ? null : ScheduleProcess.GetTaskCalendar(cache, scheduleTask), value, (short)(initialHour * 60.0m));
	}

	public decimal CalculateScheduleInitialHour(string jobId, int assemblyId, int operationId, object actualDateTime, int treeId = 0)
	{
		return ScheduleProcess.CalculateScheduleInitialHour(database, jobId, assemblyId, operationId, actualDateTime, treeId);
	}

	public void DeleteJobOperationFromScheduleTables(string jobId, int assemblyId, int operationId, SqlTransaction sqlTransaction)
	{
		ScheduleOperators.DeleteJobOperationOnScheduleTables(jobId, assemblyId, operationId, database, sqlTransaction, includeMasterScenario: true);
	}

	public void DeleteAssemblyFromScheduleTables(string jobId, int assemblyId, SqlTransaction sqlTransaction)
	{
		ScheduleOperators.DeleteAssemblyWithSubAssembliesOnScheduleTables(jobId, assemblyId, database, sqlTransaction, includeMasterScenario: true);
	}

	public void Dispose()
	{
		database = null;
		provider = null;
	}
}
