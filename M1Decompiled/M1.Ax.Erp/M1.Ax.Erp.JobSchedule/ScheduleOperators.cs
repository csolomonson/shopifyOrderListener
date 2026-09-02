using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.JobSchedule;

public static class ScheduleOperators
{
	public static void SaveJobOperationInSchedulesTables(string jobId, int assemblyId, int operationId, M1Database database, SqlTransaction sqlTransaction, string scenario, int scheduleTreeId)
	{
		string queryString = $"SELECT jmoStartDate, jmoStartHour FROM JobOperations WHERE jmoJobID = {jobId.ToSql()} AND jmoJobAssemblyID = {assemblyId} AND jmoJobOperationId = {operationId}";
		DataRowCollection rows = database.GetDataTable(queryString, sqlTransaction).Rows;
		if (rows.Count <= 0)
		{
			return;
		}
		DataRow row = rows[0];
		DateTime initialDate = row.Field<DateTime>("jmoStartDate");
		decimal initialHour = row.Field<decimal>("jmoStartHour");
		ScheduleParameters parameters = new ScheduleParameters
		{
			Direction = ScheduleDirection.Forward,
			AssemblyScope = ScheduleAssemblyScope.CurrentAssembly,
			OperationScope = ScheduleOperationScope.CurrentOperation,
			BaseAssemblyID = 0,
			InitialAssemblyID = assemblyId,
			InitialOperationID = operationId,
			InitialDate = initialDate,
			InitialHour = initialHour,
			InitialDateType = 1,
			ScenarioID = scenario
		};
		ScheduleCache cache = ScheduleProcess.LoadCache(database);
		LoadSchedule loadSchedule = new LoadSchedule();
		LoadJob loadJob = new LoadJob();
		using ScheduleTree scheduleTree = loadSchedule.Load(database, scheduleTreeId, cache);
		ScheduleTree scheduleTree2 = loadJob.Load(database, new object[1] { jobId }, cache);
		ScheduleProcess.StartSchedule(database, cache, parameters, scheduleTree2);
		ScheduleTask scheduleTask = scheduleTree2.AllTasks.FirstOrDefault((ScheduleTask x) => x.BranchID == assemblyId && x.TaskID == operationId);
		if (scheduleTask != null)
		{
			scheduleTask.TreeID = scheduleTreeId;
			scheduleTree.AllTasks.Add(scheduleTask);
		}
		new ScheduleSave().SaveSchedule(database, scheduleTree);
		if (scheduleTree.ScheduleType == 1)
		{
			ScheduleProcess.ScheduleRefreshOpsAndMat(database, jobId, 0);
		}
	}

	public static void UpdateJobOperation(DataRow jobOperation, string jobIdToUpdate, int assemblyIdToUpdate, int operationIdToUpdate, M1Database database, SqlTransaction sqlTransaction, bool updateWithStartDates = true)
	{
		DateTime d = (updateWithStartDates ? jobOperation.Field<DateTime>("jmoStartDate") : jobOperation.Field<DateTime>("jmoDueDate"));
		decimal d2 = (updateWithStartDates ? jobOperation.Field<decimal>("jmoStartHour") : jobOperation.Field<decimal>("jmoDueHour"));
		string queryString = $"UPDATE JobOperations SET jmoStartDate = {d.ToSql()}, jmoStartHour = {d2.ToSql()} WHERE jmoJobID = {jobIdToUpdate.ToSql()} AND jmoJobAssemblyID = {assemblyIdToUpdate} AND jmoJobOperationID = {operationIdToUpdate}";
		database.ExecuteCommand(queryString, sqlTransaction);
	}

	public static void UpdateKeysJobOperationScenariosOnScheduleTables(string oldJobId, string newJobId, int oldAssemblyId, int newAssemblyId, int oldOperationId, int newOperationId, M1Database database, SqlTransaction sqlTransaction)
	{
		foreach (string jobScenariosByJob in GetJobScenariosByJobs(new List<string> { oldJobId, newJobId }, database, sqlTransaction))
		{
			List<int> scheduleTreesId = GetScheduleTreesId(newJobId, database, sqlTransaction, includeMasterScenario: false, jobScenariosByJob);
			List<int> scheduleTreesId2 = GetScheduleTreesId(oldJobId, database, sqlTransaction, includeMasterScenario: false, jobScenariosByJob);
			int num = ((scheduleTreesId.Count >= scheduleTreesId2.Count) ? scheduleTreesId2.Count : scheduleTreesId.Count);
			for (int i = num; i < scheduleTreesId.Count; i++)
			{
				int scheduleTreeId = scheduleTreesId[i];
				DeleteJobOperationOnScheduleTablesQueries(newAssemblyId, newOperationId, scheduleTreeId, database, sqlTransaction);
			}
			for (int j = num; j < scheduleTreesId2.Count; j++)
			{
				int scheduleTreeId2 = scheduleTreesId2[j];
				DeleteJobOperationOnScheduleTablesQueries(oldAssemblyId, oldOperationId, scheduleTreeId2, database, sqlTransaction);
			}
			for (int k = 0; k < num; k++)
			{
				int newScheduleTreeId = scheduleTreesId[k];
				int oldScheduleTreeId = scheduleTreesId2[k];
				UpdateJobOperationOnScheduleTablesQueries(oldAssemblyId, newAssemblyId, oldOperationId, newOperationId, oldScheduleTreeId, newScheduleTreeId, database, sqlTransaction);
			}
		}
	}

	public static void UpdateKeysJobOperationOnScheduleTables(string oldJobId, string newJobId, int oldAssemblyId, int newAssemblyId, int oldOperationId, int newOperationId, M1Database database, SqlTransaction sqlTransaction)
	{
		if (oldJobId == newJobId)
		{
			foreach (int item in GetScheduleTreesId(oldJobId, database, sqlTransaction, includeMasterScenario: true))
			{
				UpdateJobOperationOnScheduleTablesQueries(oldAssemblyId, newAssemblyId, oldOperationId, newOperationId, item, item, database, sqlTransaction);
			}
			return;
		}
		int masterScheduleTreeId = GetMasterScheduleTreeId(newJobId, database, sqlTransaction);
		int masterScheduleTreeId2 = GetMasterScheduleTreeId(oldJobId, database, sqlTransaction);
		UpdateJobOperationOnScheduleTablesQueries(oldAssemblyId, newAssemblyId, oldOperationId, newOperationId, masterScheduleTreeId2, masterScheduleTreeId, database, sqlTransaction);
	}

	public static void UpdateKeysJobAssemblyOnScheduleTables(string jobId, int oldAssemblyId, int newAssemblyId, M1Database database, SqlTransaction sqlTransaction)
	{
		foreach (int item in GetScheduleTreesId(jobId, database, sqlTransaction, includeMasterScenario: true))
		{
			UpdateAssemblyOnScheduleTablesQueries(oldAssemblyId, newAssemblyId, item, database, sqlTransaction);
		}
	}

	public static void DeleteJobOperationOnScheduleTables(string jobId, int assemblyId, int jobOperationId, M1Database database, SqlTransaction sqlTransaction, bool includeMasterScenario, string scenarioId = "")
	{
		foreach (int item in GetScheduleTreesId(jobId, database, sqlTransaction, includeMasterScenario, scenarioId))
		{
			DeleteJobOperationOnScheduleTablesQueries(assemblyId, jobOperationId, item, database, sqlTransaction);
		}
	}

	public static void DeleteAssemblyWithSubAssembliesOnScheduleTables(string jobId, int parentAssemblyId, M1Database database, SqlTransaction sqlTransaction, bool includeMasterScenario)
	{
		List<int> scheduleTreesId = GetScheduleTreesId(jobId, database, sqlTransaction, includeMasterScenario);
		string queryString = "SELECT jmaJobAssemblyID,jmaParentAssemblyID FROM JobAssemblies WHERE jmaJobID = " + jobId.ToSql();
		DataTable dataTable = database.GetDataTable(queryString, sqlTransaction);
		if (dataTable.Rows.Count != 0)
		{
			DeleteNextAssemblyLevel(database, sqlTransaction, dataTable, parentAssemblyId, scheduleTreesId);
			DeleteAssemblyOnScheduleTables(database, sqlTransaction, parentAssemblyId, scheduleTreesId);
		}
	}

	public static void DeleteJobOnScheduleTables(string jobId, M1Database database, SqlTransaction sqlTransaction, bool includeMasterScenario)
	{
		foreach (int item in GetScheduleTreesId(jobId, database, sqlTransaction, includeMasterScenario))
		{
			DeleteJobOnScheduleTablesQueries(item, database, sqlTransaction);
		}
	}

	public static void SetDefaultJobOperationDates(string jobId, int assemblyId, int operationId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = $"UPDATE JobOperations SET jmoStartDate = NULL, jmoStartHour = 0, jmoDueDate = NULL, jmoDueHour = 0 WHERE jmoJobID = {jobId.ToSql()} AND jmoJobAssemblyID = {assemblyId} AND jmoJobOperationID = {operationId}";
		database.ExecuteCommand(queryString, sqlTransaction);
	}

	public static void SetDefaultMaterialsDatesWithRelatedJob(string jobId, int assemblyId, int operationId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = $"UPDATE JobMaterials SET jmmRequiredDate = Null, jmmOrderByDate = Null WHERE jmmJobID = {jobId.ToSql()} AND jmmJobAssemblyId = {assemblyId} AND jmmRelatedJobOperationID = {operationId}";
		database.ExecuteCommand(queryString, sqlTransaction);
	}

	public static void SetDefaultJobDates(string jobId, M1Database database, SqlTransaction sqlTransaction)
	{
		DataTable assembliesFromJob = GetAssembliesFromJob(jobId, database, sqlTransaction);
		bool flag = true;
		foreach (DataRow row in assembliesFromJob.Rows)
		{
			int assemblyId = row.Field<int>("jmaJobAssemblyID");
			if (IsJobAssemblyOnScheduleTables(jobId, assemblyId, database, sqlTransaction))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			string queryString = "UPDATE Jobs SET jmpScheduledStartDate = NULL, jmpScheduledDueDate = NULL, jmpScheduledStartHour = 0, jmpScheduledDueHour = 0 WHERE jmpJobID = " + jobId.ToSql();
			database.ExecuteCommand(queryString, sqlTransaction);
			DeleteJobOnScheduleTables(jobId, database, sqlTransaction, includeMasterScenario: true);
		}
	}

	public static void SetDefaultAssemblyDates(string jobId, int assemblyId, M1Database database, SqlTransaction sqlTransaction)
	{
		if (!IsJobAssemblyOnScheduleTables(jobId, assemblyId, database, sqlTransaction))
		{
			string queryString = $"UPDATE JobAssemblies SET jmaScheduledStartDate = NULL, jmaScheduledDueDate = NULL WHERE jmaJobID = {jobId.ToSql()} AND jmaJobAssemblyID = {assemblyId}";
			database.ExecuteCommand(queryString, sqlTransaction);
		}
	}

	public static void SetDefaultMaterialsDatesByAssembly(string jobId, int assemblyId, M1Database database, SqlTransaction sqlTransaction)
	{
		if (!IsJobAssemblyOnScheduleTables(jobId, assemblyId, database, sqlTransaction))
		{
			string queryString = $"UPDATE JobMaterials SET jmmRequiredDate = Null, jmmOrderByDate = Null WHERE jmmJobID = {jobId.ToSql()} AND jmmJobAssemblyId = {assemblyId}";
			database.ExecuteCommand(queryString, sqlTransaction);
		}
	}

	public static JobOperationData GetPreviousScheduledJobOperation(string jobId, int assemblyId, int operationId, M1Database database, SqlTransaction sqlTransaction)
	{
		DataRowCollection rows = GetScheduledJobOperationsFromAssembly(jobId, assemblyId, database, sqlTransaction).Rows;
		int count = rows.Count;
		int num = count - 1;
		JobOperationData jobOperationData = new JobOperationData();
		if (count >= 1)
		{
			int num2 = rows[0].Field<int>("jmoJobOperationID");
			int num3 = rows[num].Field<int>("jmoJobOperationID");
			if (operationId < num2)
			{
				jobOperationData.JobOperation = rows[0];
				jobOperationData.ShouldUseStartDate = true;
			}
			else if (operationId > num3)
			{
				jobOperationData.JobOperation = rows[num];
				jobOperationData.ShouldUseStartDate = false;
			}
			else
			{
				for (int i = 1; i <= num; i++)
				{
					if (rows[i].Field<int>("jmoJobOperationID") > operationId)
					{
						jobOperationData.JobOperation = rows[i - 1];
						jobOperationData.ShouldUseStartDate = false;
						break;
					}
				}
			}
		}
		return jobOperationData;
	}

	public static IEnumerable<string> GetJobScenariosByJobs(List<string> jobs, M1Database database, SqlTransaction sqlTransaction)
	{
		string text = string.Empty;
		foreach (string job in jobs)
		{
			text += job.ToSql();
			text += ",";
		}
		text = text.Trim(',');
		string queryString = "SELECT DISTINCT sxtJobScenarioID FROM ScheduleTrees INNER JOIN Jobs ON jmpUniqueID = sxtGroupUniqueID WHERE jmpJobID IN (" + text + ") AND sxtJobScenarioID <> ''";
		DataRowCollection rows = database.GetDataTable(queryString, sqlTransaction).Rows;
		List<string> list = new List<string>();
		foreach (DataRow item in rows)
		{
			list.Add(item.Field<string>("sxtJobScenarioID"));
		}
		return list;
	}

	public static DataTable GetScheduledJobOperationsFromAssembly(string jobId, int assemblyId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = $"SELECT JO.* FROM JobOperations JO LEFT JOIN Jobs J ON JO.jmoJobID = J.jmpJobID LEFT JOIN ScheduleTrees ST ON ST.sxtGroupUniqueID = J.jmpUniqueID AND ST.sxtType = 1 RIGHT JOIN ScheduleTasks STA ON ST.sxtScheduleTreeID = STA.sxkScheduleTreeID AND JO.jmoJobAssemblyID = STA.sxkScheduleBranchID AND JO.jmoJobOperationID = STA.sxkScheduleTaskID AND STA.sxkStartDate IS NOT NULL AND STA.sxkEndDate IS NOT NULL WHERE JO.jmoJobID = {jobId.ToSql()} AND JO.jmoJobAssemblyID = {assemblyId} ORDER BY jmoJobOperationID";
		return database.GetDataTable(queryString, sqlTransaction);
	}

	public static DataTable GetJobOperationsFromAssembly(string jobId, int assemblyId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = $"SELECT jmoJobID, jmoJobAssemblyID, jmoJobOperationID FROM JobOperations WHERE jmoJobID = {jobId.ToSql()} AND jmoJobAssemblyID = {assemblyId} ORDER BY jmoJobOperationID";
		return database.GetDataTable(queryString, sqlTransaction);
	}

	public static DataTable GetJobOperationsFromJob(string jobId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = "SELECT jmoJobID, jmoJobAssemblyID, jmoJobOperationID FROM JobOperations WHERE jmoJobID = " + jobId.ToSql();
		return database.GetDataTable(queryString, sqlTransaction);
	}

	public static DataTable GetAssembliesFromJob(string jobId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = "SELECT jmaJobID, jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = " + jobId.ToSql();
		return database.GetDataTable(queryString, sqlTransaction);
	}

	public static List<int> GetScheduleTreesId(string jobId, M1Database database, SqlTransaction sqlTransaction, bool includeMasterScenario, string scenarioId = "")
	{
		string text = (includeMasterScenario ? string.Empty : " AND sxtType = 0");
		text += (string.IsNullOrEmpty(scenarioId) ? text : (" AND sxtJobScenarioID = " + scenarioId.ToSql()));
		string queryString = "SELECT sxtScheduleTreeID FROM ScheduleTrees INNER JOIN Jobs ON jmpUniqueID = sxtGroupUniqueID WHERE jmpJobID = " + jobId.ToSql() + " " + text;
		DataTable dataTable = database.GetDataTable(queryString, sqlTransaction);
		List<int> list = new List<int>();
		foreach (DataRow row in dataTable.Rows)
		{
			int item = row.Field<int>("sxtScheduleTreeID");
			list.Add(item);
		}
		return list;
	}

	public static int GetMasterScheduleTreeId(string jobId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = "SELECT sxtScheduleTreeID FROM ScheduleTrees INNER JOIN Jobs ON jmpUniqueID = sxtGroupUniqueID WHERE jmpJobID = " + jobId.ToSql() + " AND sxtType = 1";
		DataRowCollection rows = database.GetDataTable(queryString, sqlTransaction).Rows;
		int index = rows.Count - 1;
		return rows[index].Field<int>("sxtScheduleTreeID");
	}

	public static DataRowCollection GetDuplicateRelatedMaterialsKeyBetweenAssemblies(string oldJobId, string newJobId, int oldAssemblyId, int newAssemblyId, int oldOperationId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = $"SELECT jm1.jmmJobMaterialID, jm1.jmmPartID from JobMaterials jm1 where jm1.jmmJobID = {oldJobId.ToSql()} AND jm1.jmmJobAssemblyID = {oldAssemblyId} AND jm1.jmmRelatedJobOperationID = {oldOperationId} AND jm1.jmmJobMaterialID = (SELECT jm2.jmmJobMaterialID from JobMaterials jm2 where jm2.jmmJobID = {newJobId.ToSql()} AND jm2.jmmJobAssemblyID = {newAssemblyId} AND jm2.jmmJobMaterialID = jm1.jmmJobMaterialID)";
		return database.GetDataTable(queryString, sqlTransaction).Rows;
	}

	public static DataTable GetAllScheduleInfo(string jobId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = "SELECT ST.* FROM ScheduleTrees ST INNER JOIN Jobs J ON J.jmpUniqueID = ST.sxtGroupUniqueID WHERE J.jmpJobID = " + jobId.ToSql();
		return database.GetDataTable(queryString, sqlTransaction);
	}

	public static bool HasPreviousOperation(string jobId, int assemblyId, int operationId, M1Database database)
	{
		string queryString = $"SELECT ISNULL(MIN(jmoJobOperationID), \r\n                                                                                (SELECT TOP 1 MIN(JO.jmoJobOperationID) \r\n                                                                                FROM JobOperations JO \r\n                                                                                INNER JOIN JobAssemblies JA ON JA.jmaJobAssemblyID = JO.jmoJobAssemblyID AND JA.jmaJobID = JO.jmoJobID \r\n                                                                                WHERE JO.jmoJobID = {jobId.ToSql()})) AS jmoJobOperationID \r\n                                            FROM JobOperations \r\n                                            WHERE jmoJobID = {jobId.ToSql()} AND jmoJobAssemblyID = {assemblyId}";
		DataRowCollection rows = database.GetDataTable(queryString).Rows;
		return operationId > Convert.ToInt32(rows[0]["jmoJobOperationID"]);
	}

	public static bool HasSubSequentOperation(string jobId, int assemblyId, int operationId, M1Database database)
	{
		string queryString = $"SELECT ISNULL(MAX(jmoJobOperationID), \r\n                                                                                (SELECT TOP 1 MAX(JO.jmoJobOperationID) \r\n                                                                                FROM JobOperations JO \r\n                                                                                INNER JOIN JobAssemblies JA ON JA.jmaJobAssemblyID = JO.jmoJobAssemblyID AND JA.jmaJobID = JO.jmoJobID \r\n                                                                                WHERE JO.jmoJobID = {jobId.ToSql()})) AS jmoJobOperationID \r\n                                            FROM JobOperations \r\n                                            WHERE jmoJobID = {jobId.ToSql()} AND jmoJobAssemblyID = {assemblyId}";
		DataRowCollection rows = database.GetDataTable(queryString).Rows;
		return operationId < Convert.ToInt32(rows[0]["jmoJobOperationID"]);
	}

	public static DataTable GetInitialAssemblyOperation(string jobId, M1Database database)
	{
		string queryString = "SELECT TOP 1 JA.jmaJobAssemblyID, JO.jmoJobOperationID \r\n                                                        FROM JobOperations JO \r\n                                                        INNER JOIN JobAssemblies JA ON JA.jmaJobAssemblyID = JO.jmoJobAssemblyID AND JA.jmaJobID = JO.jmoJobID \r\n                                                        WHERE JO.jmoJobID = " + jobId.ToSql() + " \r\n                                                        ORDER BY JO.jmoJobOperationID ASC";
		return database.GetDataTable(queryString);
	}

	public static DataTable GetFirstOrLastTaskOperation(string jobId, string ascOrDescOperation, M1Database database)
	{
		string queryString = "SELECT TOP 1 jmoJobAssemblyID, jmoJobOperationID\r\n                                        FROM JobOperations\r\n                                        WHERE jmoJobID = " + jobId.ToSql() + " AND jmoJobAssemblyID = (\r\n                                                                                                    SELECT DISTINCT TOP 1 JA.jmaJobAssemblyID\r\n                                                                                                    FROM JobAssemblies ja\r\n                                                                                                    INNER JOIN JobOperations jo ON jo.jmoJobAssemblyID = ja.jmaJobAssemblyID \r\n                                                                                                                                AND jo.jmoJobID = ja.jmaJobID\r\n                                                                                                    WHERE jmaJobID = " + jobId.ToSql() + "\r\n                                                                                                    ORDER BY ja.jmaJobAssemblyID ASC\r\n                                                                                                )\r\n                                        ORDER BY jmoJobOperationID " + ascOrDescOperation.Replace("'", " ");
		return database.GetDataTable(queryString);
	}

	public static bool HasChildAssemblies(string jobId, int assemblyId, M1Database database)
	{
		string queryString = $"SELECT jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = {jobId.ToSql()} AND jmaParentAssemblyID = {assemblyId} ORDER BY jmaJobAssemblyID DESC";
		DataTable dataTable = database.GetDataTable(queryString);
		if (dataTable.Rows.Count <= 0)
		{
			return false;
		}
		int num = Convert.ToInt32(dataTable.Rows[0]["jmaJobAssemblyID"]);
		return assemblyId != num;
	}

	public static bool IsJobOperationOnScheduleTables(string jobId, int assemblyId, int operationId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = $"SELECT COUNT(*) AS quantity FROM ScheduleTasks INNER JOIN ScheduleTrees ON sxkScheduleTreeID = sxtScheduleTreeID INNER JOIN Jobs ON sxtGroupUniqueID = jmpUniqueID WHERE jmpJobID = {jobId.ToSql()} AND sxkScheduleBranchID = {assemblyId} AND sxkScheduleTaskID = {operationId}";
		return Convert.ToBoolean(database.GetDataTable(queryString, sqlTransaction).Rows[0]["quantity"]);
	}

	public static bool IsJobOnScheduleTables(string jobId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = "SELECT COUNT(*) AS quantity FROM ScheduleTrees INNER JOIN Jobs ON jmpUniqueID = sxtGroupUniqueID WHERE jmpJobID = " + jobId.ToSql() + " AND sxtType = 1";
		return Convert.ToBoolean(database.GetDataTable(queryString, sqlTransaction).Rows[0]["quantity"]);
	}

	public static bool IsJobAssemblyOnScheduleTables(string jobId, int assemblyId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = $"SELECT COUNT(*) AS quantity FROM ScheduleTasks INNER JOIN ScheduleTrees ON sxkScheduleTreeID = sxtScheduleTreeID INNER JOIN Jobs ON sxtGroupUniqueID = jmpUniqueID WHERE jmpJobID = {jobId.ToSql()} AND sxkScheduleBranchID = {assemblyId} AND sxkStartDate IS NOT NULL AND sxkEndDate IS NOT NULL AND sxtType = 1";
		return Convert.ToInt32(database.GetDataTable(queryString, sqlTransaction).Rows[0]["quantity"]) > 0;
	}

	private static void DeleteNextAssemblyLevel(M1Database database, SqlTransaction sqlTransaction, DataTable assembliesTable, int parentAssemblyId, List<int> scheduleTreesId)
	{
		DataRow[] array = assembliesTable.Select("jmaParentAssemblyID = " + M1Util.ConvertToLinq(parentAssemblyId) + " and jmaJobAssemblyID <> 0");
		for (int i = 0; i < array.Length; i++)
		{
			int num = Convert.ToInt32(array[i]["jmaJobAssemblyID"]);
			DeleteNextAssemblyLevel(database, sqlTransaction, assembliesTable, num, scheduleTreesId);
			DeleteAssemblyOnScheduleTables(database, sqlTransaction, num, scheduleTreesId);
		}
	}

	private static void DeleteAssemblyOnScheduleTables(M1Database database, SqlTransaction sqlTransaction, int assemblyId, IEnumerable<int> scheduleTreesId)
	{
		foreach (int item in scheduleTreesId)
		{
			string queryString = $"DELETE FROM ScheduleTasks WHERE sxkScheduleTreeID = {item} AND sxkScheduleBranchID = {assemblyId}";
			database.ExecuteCommand(queryString, sqlTransaction);
			string queryString2 = $"DELETE FROM ScheduleTaskBuckets WHERE sxeScheduleTreeID = {item} AND sxeScheduleBranchID = {assemblyId}";
			database.ExecuteCommand(queryString2, sqlTransaction);
			string queryString3 = $"DELETE FROM ScheduleAllocations WHERE sxdScheduleTreeID = {item} AND sxdScheduleBranchID = {assemblyId}";
			database.ExecuteCommand(queryString3, sqlTransaction);
			string queryString4 = $"DELETE FROM ScheduleResourceCells WHERE sxcTreeID = {item} AND sxcBranchID = {assemblyId}";
			database.ExecuteCommand(queryString4, sqlTransaction);
			string queryString5 = $"DELETE FROM ScheduleResourceLanes WHERE sxrScheduleTreeID = {item} AND sxrScheduleBranchID = {assemblyId}";
			database.ExecuteCommand(queryString5, sqlTransaction);
			string queryString6 = $"DELETE FROM ScheduleBranches WHERE sxbScheduleTreeID = {item} AND sxbScheduleBranchID = {assemblyId}";
			database.ExecuteCommand(queryString6, sqlTransaction);
		}
	}

	private static void DeleteJobOperationOnScheduleTablesQueries(int assemblyId, int jobOperationId, int scheduleTreeId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = $"DELETE FROM ScheduleTasks WHERE sxkScheduleTreeID = {scheduleTreeId} AND sxkScheduleBranchID = {assemblyId} AND sxkScheduleTaskID = {jobOperationId}";
		database.ExecuteCommand(queryString, sqlTransaction);
		string queryString2 = $"DELETE FROM ScheduleTaskBuckets WHERE sxeScheduleTreeID = {scheduleTreeId} AND sxeScheduleBranchID = {assemblyId} AND sxeScheduleTaskID = {jobOperationId}";
		database.ExecuteCommand(queryString2, sqlTransaction);
		string queryString3 = $"DELETE FROM ScheduleAllocations WHERE sxdScheduleTreeID = {scheduleTreeId} AND sxdScheduleBranchID = {assemblyId} AND sxdScheduleTaskID = {jobOperationId}";
		database.ExecuteCommand(queryString3, sqlTransaction);
		string queryString4 = $"DELETE FROM ScheduleResourceCells WHERE sxcTreeID = {scheduleTreeId} AND sxcBranchID = {assemblyId} AND sxcTaskID = {jobOperationId}";
		database.ExecuteCommand(queryString4, sqlTransaction);
		string queryString5 = $"DELETE FROM ScheduleResourceLanes WHERE sxrScheduleTreeID = {scheduleTreeId} AND sxrScheduleBranchID = {assemblyId} AND sxrScheduleTaskID = {jobOperationId}";
		database.ExecuteCommand(queryString5, sqlTransaction);
	}

	private static void DeleteJobOnScheduleTablesQueries(int scheduleTreeId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = $"DELETE FROM ScheduleTasks WHERE sxkScheduleTreeID = {scheduleTreeId}";
		database.ExecuteCommand(queryString, sqlTransaction);
		string queryString2 = $"DELETE FROM ScheduleTaskBuckets WHERE sxeScheduleTreeID = {scheduleTreeId}";
		database.ExecuteCommand(queryString2, sqlTransaction);
		string queryString3 = $"DELETE FROM ScheduleAllocations WHERE sxdScheduleTreeID = {scheduleTreeId}";
		database.ExecuteCommand(queryString3, sqlTransaction);
		string queryString4 = $"DELETE FROM ScheduleResourceCells WHERE sxcTreeID = {scheduleTreeId}";
		database.ExecuteCommand(queryString4, sqlTransaction);
		string queryString5 = $"DELETE FROM ScheduleResourceLanes WHERE sxrScheduleTreeID = {scheduleTreeId}";
		database.ExecuteCommand(queryString5, sqlTransaction);
		string queryString6 = $"DELETE FROM ScheduleBranches WHERE sxbScheduleTreeID = {scheduleTreeId}";
		database.ExecuteCommand(queryString6, sqlTransaction);
		string queryString7 = $"DELETE FROM ScheduleTrees WHERE sxtScheduleTreeID = {scheduleTreeId}";
		database.ExecuteCommand(queryString7, sqlTransaction);
	}

	private static void UpdateJobOperationOnScheduleTablesQueries(int oldAssemblyId, int newAssemblyId, int oldOperationId, int newOperationId, int oldScheduleTreeId, int newScheduleTreeId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = $"UPDATE ScheduleTasks SET sxkScheduleTreeID = {newScheduleTreeId}, sxkScheduleBranchID = {newAssemblyId}, sxkScheduleTaskID = {newOperationId} WHERE sxkScheduleTreeID = {oldScheduleTreeId} AND sxkScheduleBranchID = {oldAssemblyId} AND sxkScheduleTaskID = {oldOperationId}";
		database.ExecuteCommand(queryString, sqlTransaction);
		string queryString2 = $"UPDATE ScheduleTaskBuckets SET sxeScheduleTreeID = {newScheduleTreeId}, sxeScheduleBranchID = {newAssemblyId}, sxeScheduleTaskID = {newOperationId} WHERE sxeScheduleTreeID = {oldScheduleTreeId} AND sxeScheduleBranchID = {oldAssemblyId} AND sxeScheduleTaskID = {oldOperationId}";
		database.ExecuteCommand(queryString2, sqlTransaction);
		string queryString3 = $"UPDATE ScheduleAllocations SET sxdScheduleTreeID = {newScheduleTreeId}, sxdScheduleBranchID = {newAssemblyId}, sxdScheduleTaskID = {newOperationId} WHERE sxdScheduleTreeID = {oldScheduleTreeId} AND sxdScheduleBranchID = {oldAssemblyId} AND sxdScheduleTaskID = {oldOperationId}";
		database.ExecuteCommand(queryString3, sqlTransaction);
		string queryString4 = $"UPDATE ScheduleResourceCells SET sxcTreeID = {newScheduleTreeId}, sxcBranchID = {newAssemblyId}, sxcTaskID = {newOperationId} WHERE sxcTreeID = {oldScheduleTreeId} AND sxcBranchID = {oldAssemblyId} AND sxcTaskID = {oldOperationId}";
		database.ExecuteCommand(queryString4, sqlTransaction);
		string queryString5 = $"UPDATE ScheduleResourceLanes SET sxrScheduleTreeID = {newScheduleTreeId}, sxrScheduleBranchID = {newAssemblyId}, sxrScheduleTaskID = {newOperationId} WHERE sxrScheduleTreeID = {oldScheduleTreeId} AND sxrScheduleBranchID = {oldAssemblyId} AND sxrScheduleTaskID = {oldOperationId}";
		database.ExecuteCommand(queryString5, sqlTransaction);
	}

	private static void UpdateAssemblyOnScheduleTablesQueries(int oldAssemblyId, int newAssemblyId, int scheduleTreeId, M1Database database, SqlTransaction sqlTransaction)
	{
		string queryString = $"UPDATE ScheduleTasks SET sxkScheduleBranchID = {newAssemblyId} WHERE sxkScheduleTreeID = {scheduleTreeId} AND sxkScheduleBranchID = {oldAssemblyId}";
		database.ExecuteCommand(queryString, sqlTransaction);
		string queryString2 = $"UPDATE ScheduleTaskBuckets SET sxeScheduleBranchID = {newAssemblyId} WHERE sxeScheduleTreeID = {scheduleTreeId} AND sxeScheduleBranchID = {oldAssemblyId}";
		database.ExecuteCommand(queryString2, sqlTransaction);
		string queryString3 = $"UPDATE ScheduleAllocations SET sxdScheduleBranchID = {newAssemblyId} WHERE sxdScheduleTreeID = {scheduleTreeId} AND sxdScheduleBranchID = {oldAssemblyId}";
		database.ExecuteCommand(queryString3, sqlTransaction);
		string queryString4 = $"UPDATE ScheduleResourceCells SET sxcBranchID = {newAssemblyId} WHERE sxcTreeID = {scheduleTreeId} AND sxcBranchID = {oldAssemblyId}";
		database.ExecuteCommand(queryString4, sqlTransaction);
		string queryString5 = $"UPDATE ScheduleResourceLanes SET sxrScheduleBranchID = {newAssemblyId} WHERE sxrScheduleTreeID = {scheduleTreeId} AND sxrScheduleBranchID = {oldAssemblyId}";
		database.ExecuteCommand(queryString5, sqlTransaction);
		string queryString6 = $"UPDATE ScheduleBranches SET sxbScheduleBranchID = {newAssemblyId} WHERE sxbScheduleTreeID = {scheduleTreeId} AND sxbScheduleBranchID = {oldAssemblyId}";
		database.ExecuteCommand(queryString6, sqlTransaction);
	}

	public static void UpdateScheduleDatesForSubAssemblies(M1Database database, SqlTransaction sqlTransaction, string jobId, int assemblyId)
	{
		string queryString = $"SELECT jmaJobAssemblyID FROM JobAssemblies WHERE jmaJobID = {jobId.ToSql()} AND jmaParentAssemblyID = {assemblyId} AND jmaProductionComplete = 0";
		database.ExecuteCommand(queryString, sqlTransaction);
		DataTable dataTable = database.GetDataTable(queryString, sqlTransaction);
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			int assemblyId2 = row.Field<int>("jmaJobAssemblyID");
			UpdateJobOperationDates(database, sqlTransaction, jobId, assemblyId2);
			UpdateJobMaterialDates(database, sqlTransaction, jobId, assemblyId2);
			UpdateJobAssemblyDates(database, sqlTransaction, jobId, assemblyId2);
			UpdateScheduleDatesForSubAssemblies(database, sqlTransaction, jobId, assemblyId2);
		}
	}

	public static void UpdateJobAssemblyDates(M1Database database, SqlTransaction sqlTransaction, string jobId, int assemblyId)
	{
		string queryString = $"UPDATE JobAssemblies SET jmaScheduledStartDate = NULL, jmaScheduledDueDate = NULL WHERE jmaJobID = {jobId.ToSql()} AND jmaJobAssemblyID = {assemblyId} AND jmaProductionComplete = 0";
		database.ExecuteCommand(queryString, sqlTransaction);
	}

	public static void UpdateJobOperationDates(M1Database database, SqlTransaction sqlTransaction, string jobId, int assemblyId)
	{
		string queryString = $"UPDATE JobOperations SET jmoStartDate = NULL, jmoStartHour = 0, jmoDueDate = NULL, jmoDueHour = 0 WHERE jmoJobID = {jobId.ToSql()} AND jmoJobAssemblyID = {assemblyId} AND jmoProductionComplete = 0";
		database.ExecuteCommand(queryString, sqlTransaction);
	}

	public static void UpdateJobMaterialDates(M1Database database, SqlTransaction sqlTransaction, string jobId, int assemblyId)
	{
		string queryString = $"UPDATE JobMaterials SET jmmRequiredDate = Null, jmmOrderByDate = Null WHERE jmmJobID = {jobId.ToSql()} AND jmmJobAssemblyId = {assemblyId} AND jmmReceivedComplete = 0";
		database.ExecuteCommand(queryString, sqlTransaction);
	}
}
