using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Ax.Erp.JobSchedule;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("JobAssemblies")]
public class JobAssembliesChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
		if (!parm.NewKeyValues[0].ToString().Trim().Equals(parm.OldKeyValues[0].ToString().Trim(), StringComparison.CurrentCultureIgnoreCase))
		{
			throw new M1Exception("Job Assemblies may not be moved between jobs.");
		}
		parm.Database.ExecuteCommand("UPDATE JobAssemblies SET jmaParentAssemblyID = " + parm.NewKeyValues[1].ToSql() + " WHERE jmaJobID = " + parm.OldKeyValues[0].ToSql() + " AND jmaParentAssemblyID = " + parm.OldKeyValues[1].ToSql(), parm.SqlTransaction);
		UpdateKeysOnSchedulesTables(parm);
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
		M1Database database = parm.Database;
		SqlTransaction sqlTransaction = parm.SqlTransaction;
		string jobId = parm.NewKeyValues[0].ToString();
		int assemblyId = Convert.ToInt32(parm.NewKeyValues[1]);
		ScheduleOperators.SetDefaultAssemblyDates(jobId, assemblyId, database, sqlTransaction);
		ScheduleOperators.SetDefaultMaterialsDatesByAssembly(jobId, assemblyId, database, sqlTransaction);
		ScheduleOperators.SetDefaultJobDates(jobId, database, sqlTransaction);
	}

	private static void UpdateKeysOnSchedulesTables(ChangeIDProcessingParms parm)
	{
		M1Database database = parm.Database;
		short changeIDType = parm.ChangeIDType;
		SqlTransaction sqlTransaction = parm.SqlTransaction;
		string text = parm.OldKeyValues[0].ToString();
		int num = Convert.ToInt32(parm.OldKeyValues[1]);
		int num2 = Convert.ToInt32(parm.NewKeyValues[1]);
		bool flag = ScheduleOperators.IsJobAssemblyOnScheduleTables(text, num, database, sqlTransaction);
		bool flag2 = ScheduleOperators.IsJobAssemblyOnScheduleTables(text, num2, database, sqlTransaction);
		if (!flag && !flag2)
		{
			return;
		}
		switch (changeIDType)
		{
		case 1:
			ScheduleOperators.UpdateKeysJobAssemblyOnScheduleTables(text, num, num2, database, sqlTransaction);
			break;
		case 2:
		case 3:
		{
			DataTable jobOperationsFromAssembly = ScheduleOperators.GetJobOperationsFromAssembly(text, num2, database, sqlTransaction);
			DataTable jobOperationsFromAssembly2 = ScheduleOperators.GetJobOperationsFromAssembly(text, num, database, sqlTransaction);
			List<int> list = new List<int>();
			foreach (DataRow row in jobOperationsFromAssembly.Rows)
			{
				int item = row.Field<int>("jmoJobOperationID");
				list.Add(item);
			}
			{
				foreach (DataRow row2 in jobOperationsFromAssembly2.Rows)
				{
					int num3 = row2.Field<int>("jmoJobOperationID");
					if (list.Contains(num3))
					{
						ScheduleOperators.SetDefaultJobOperationDates(text, num, num3, database, sqlTransaction);
						ScheduleOperators.SetDefaultMaterialsDatesWithRelatedJob(text, num, num3, database, sqlTransaction);
						ScheduleOperators.DeleteJobOperationOnScheduleTables(text, num, num3, database, sqlTransaction, includeMasterScenario: true);
						ScheduleOperators.SetDefaultJobOperationDates(text, num2, num3, database, sqlTransaction);
						ScheduleOperators.SetDefaultMaterialsDatesWithRelatedJob(text, num2, num3, database, sqlTransaction);
						ScheduleOperators.DeleteJobOperationOnScheduleTables(text, num2, num3, database, sqlTransaction, includeMasterScenario: true);
					}
					else if (ScheduleOperators.IsJobOperationOnScheduleTables(text, num, num3, database, sqlTransaction) && flag2)
					{
						ScheduleOperators.UpdateKeysJobOperationOnScheduleTables(text, text, num, num2, num3, num3, database, sqlTransaction);
					}
					else
					{
						ScheduleOperators.SetDefaultJobOperationDates(text, num, num3, database, sqlTransaction);
						ScheduleOperators.SetDefaultMaterialsDatesWithRelatedJob(text, num, num3, database, sqlTransaction);
						ScheduleOperators.DeleteJobOperationOnScheduleTables(text, num, num3, database, sqlTransaction, includeMasterScenario: true);
					}
				}
				break;
			}
		}
		}
	}
}
