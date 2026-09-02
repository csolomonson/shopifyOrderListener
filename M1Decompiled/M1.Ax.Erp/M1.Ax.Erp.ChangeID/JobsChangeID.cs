using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Ax.Erp.JobSchedule;
using M1.Core;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("Jobs")]
public class JobsChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
		UpdateKeysOnSchedulesTables(parm);
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
		M1Database database = parm.Database;
		SqlTransaction sqlTransaction = parm.SqlTransaction;
		string jobId = parm.NewKeyValues[0].ToString();
		foreach (DataRow row in ScheduleOperators.GetAssembliesFromJob(jobId, database, sqlTransaction).Rows)
		{
			int assemblyId = row.Field<int>("jmaJobAssemblyID");
			ScheduleOperators.SetDefaultAssemblyDates(jobId, assemblyId, database, sqlTransaction);
			ScheduleOperators.SetDefaultMaterialsDatesByAssembly(jobId, assemblyId, database, sqlTransaction);
		}
		ScheduleOperators.SetDefaultJobDates(jobId, database, sqlTransaction);
	}

	private static void UpdateKeysOnSchedulesTables(ChangeIDProcessingParms parm)
	{
		M1Database database = parm.Database;
		short changeIDType = parm.ChangeIDType;
		SqlTransaction sqlTransaction = parm.SqlTransaction;
		string text = parm.NewKeyValues[0].ToString();
		string text2 = parm.OldKeyValues[0].ToString();
		bool num = ScheduleOperators.IsJobOnScheduleTables(text, database, sqlTransaction);
		bool flag = ScheduleOperators.IsJobOnScheduleTables(text2, database, sqlTransaction);
		if ((!num && !flag) || (changeIDType != 2 && changeIDType != 3))
		{
			return;
		}
		DataTable jobOperationsFromJob = ScheduleOperators.GetJobOperationsFromJob(text, database, sqlTransaction);
		DataTable jobOperationsFromJob2 = ScheduleOperators.GetJobOperationsFromJob(text2, database, sqlTransaction);
		List<OperationParams> list = new List<OperationParams>();
		List<OperationParams> list2 = new List<OperationParams>();
		foreach (DataRow row3 in jobOperationsFromJob.Rows)
		{
			int assemblyId = row3.Field<int>("jmoJobAssemblyID");
			int operationId = row3.Field<int>("jmoJobOperationID");
			OperationParams item = new OperationParams
			{
				AssemblyId = assemblyId,
				OperationId = operationId
			};
			list.Add(item);
		}
		foreach (DataRow row4 in jobOperationsFromJob2.Rows)
		{
			int assemblyId2 = row4.Field<int>("jmoJobAssemblyID");
			int operationId2 = row4.Field<int>("jmoJobOperationID");
			OperationParams item2 = new OperationParams
			{
				AssemblyId = assemblyId2,
				OperationId = operationId2
			};
			list2.Add(item2);
		}
		foreach (OperationParams item3 in list2)
		{
			int oldJobAssemblyId = item3.AssemblyId;
			int oldJobOperationId = item3.OperationId;
			if (list.Any((OperationParams operation) => operation.OperationId == oldJobOperationId && operation.AssemblyId == oldJobAssemblyId))
			{
				list.Find((OperationParams operation) => operation.OperationId == oldJobOperationId && operation.AssemblyId == oldJobAssemblyId).ShouldBeDeleted = true;
				item3.ShouldBeDeleted = true;
				continue;
			}
			bool flag2 = ScheduleOperators.IsJobAssemblyOnScheduleTables(text, oldJobAssemblyId, database, sqlTransaction);
			if (ScheduleOperators.IsJobOperationOnScheduleTables(text2, oldJobAssemblyId, oldJobOperationId, database, sqlTransaction) && flag2)
			{
				item3.ShouldBeUpdated = true;
			}
			else
			{
				item3.ShouldBeDeleted = true;
			}
		}
		foreach (OperationParams item4 in list)
		{
			if (item4.ShouldBeDeleted)
			{
				int assemblyId3 = item4.AssemblyId;
				int operationId3 = item4.OperationId;
				ScheduleOperators.SetDefaultJobOperationDates(text, assemblyId3, operationId3, database, sqlTransaction);
				ScheduleOperators.SetDefaultMaterialsDatesWithRelatedJob(text, assemblyId3, operationId3, database, sqlTransaction);
				ScheduleOperators.DeleteJobOperationOnScheduleTables(text, assemblyId3, operationId3, database, sqlTransaction, includeMasterScenario: true);
			}
		}
		foreach (OperationParams item5 in list2)
		{
			int assemblyId4 = item5.AssemblyId;
			int operationId4 = item5.OperationId;
			if (item5.ShouldBeDeleted)
			{
				ScheduleOperators.SetDefaultJobOperationDates(text2, assemblyId4, operationId4, database, sqlTransaction);
				ScheduleOperators.SetDefaultMaterialsDatesWithRelatedJob(text2, assemblyId4, operationId4, database, sqlTransaction);
				ScheduleOperators.DeleteJobOperationOnScheduleTables(text2, assemblyId4, operationId4, database, sqlTransaction, includeMasterScenario: true);
			}
			else if (item5.ShouldBeUpdated)
			{
				ScheduleOperators.UpdateKeysJobOperationOnScheduleTables(text2, text, assemblyId4, assemblyId4, operationId4, operationId4, database, sqlTransaction);
				ScheduleOperators.UpdateKeysJobOperationScenariosOnScheduleTables(text2, text, assemblyId4, assemblyId4, operationId4, operationId4, database, sqlTransaction);
			}
		}
		ScheduleOperators.DeleteJobOnScheduleTables(text2, database, sqlTransaction, includeMasterScenario: true);
	}
}
