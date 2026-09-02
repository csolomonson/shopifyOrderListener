using System;
using System.Data;
using System.Data.SqlClient;
using M1.Ax.Erp.JobSchedule;
using M1.Core;
using M1.Core.Database;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("JobOperations")]
public class JobOperationsChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
		M1Database database = parm.Database;
		short changeIDType = parm.ChangeIDType;
		SqlTransaction sqlTransaction = parm.SqlTransaction;
		string oldJobId = parm.OldKeyValues[0].ToString();
		int oldAssemblyId = Convert.ToInt32(parm.OldKeyValues[1]);
		int oldOperationId = Convert.ToInt32(parm.OldKeyValues[2]);
		string newJobId = parm.NewKeyValues[0].ToString();
		int newAssemblyId = Convert.ToInt32(parm.NewKeyValues[1]);
		if (changeIDType != 1)
		{
			return;
		}
		DataRowCollection duplicateRelatedMaterialsKeyBetweenAssemblies = ScheduleOperators.GetDuplicateRelatedMaterialsKeyBetweenAssemblies(oldJobId, newJobId, oldAssemblyId, newAssemblyId, oldOperationId, database, sqlTransaction);
		if (duplicateRelatedMaterialsKeyBetweenAssemblies.Count <= 0)
		{
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		foreach (DataRow item in duplicateRelatedMaterialsKeyBetweenAssemblies)
		{
			text2 += string.Format("{0},", item.Field<int>("jmmJobMaterialID"));
			text = text + item.Field<string>("jmmPartID") + ",";
		}
		text2 = text2.Trim(',');
		text = text.Trim(',');
		throw new M1Exception("The job operation can not be moved because it has a related job material (" + text + ") with the same sequence ID (" + text2 + ") as the destination assembly. Removing the relationship or changing the sequence ID for the job material will allow you to move this job operation to the destination assembly.");
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
		SqlDataAdapter adapter;
		DataTable dataTable = parm.Database.GetDataTable("SELECT * FROM JobCosts WHERE jmcJobID = " + parm.OldKeyValues[0].ToSql() + " And jmcJobAssemblyID = " + parm.OldKeyValues[1].ToSql() + " And jmcJobOperationID = " + parm.OldKeyValues[2].ToSql() + " ORDER BY jmcJobID, jmcJobAssemblyID, jmcJobType, jmcJobSequence, jmcCostSequence", fillSchema: false, out adapter, parm.SqlTransaction);
		if (dataTable.Rows.Count > 0)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = -1;
			DataTable dataTable2 = parm.Database.GetDataTable("SELECT * FROM JobCosts WHERE jmcJobID = " + parm.NewKeyValues[0].ToSql() + " AND jmcJobAssemblyID = " + parm.NewKeyValues[1].ToSql() + " ORDER BY jmcJobID, jmcJobAssemblyID, jmcJobType, jmcJobSequence, jmcCostSequence", fillSchema: false, out adapter, parm.SqlTransaction);
			foreach (DataRow row in dataTable.Rows)
			{
				if (Convert.ToInt16(row["jmcJobAssemblyID"]) == num || Convert.ToInt16(row["jmcJobType"]) == num3 || Convert.ToInt16(row["jmcJobSequence"]) == num2 || num4 == -1)
				{
					num4 = 0;
					num = Convert.ToInt16(row["jmcJobAssemblyID"]);
					num2 = Convert.ToInt16(row["jmcJobSequence"]);
					num3 = Convert.ToInt16(row["jmcJobType"]);
					DataRow[] array = dataTable2.Select("jmcJobID = " + parm.NewKeyValues[0].ToLinq() + " AND jmcJobAssemblyID = " + parm.NewKeyValues[1].ToLinq() + " AND jmcJobType = " + row["jmcJobType"].ToLinq() + " AND jmcJobSequence = " + parm.NewKeyValues[2].ToLinq(), "jmcJobID,jmcJobAssemblyID,jmcJobType,jmcJobSequence,jmcCostSequence");
					if (array.Length != 0)
					{
						num4 = Convert.ToInt16(array[array.GetUpperBound(0)]["jmcCostSeQuence"]);
					}
				}
				DataRow dataRow2 = dataTable2.NewRow().BlankRow();
				dataRow2.BeginEdit();
				foreach (DataColumn column in dataRow2.Table.Columns)
				{
					if (!SystemGeneratedFields.IsGenerated(column.ColumnName))
					{
						dataRow2[column.ColumnName] = row[column.ColumnName];
					}
				}
				num4++;
				dataRow2["jmcCostSequence"] = num4;
				dataRow2["jmcJobID"] = parm.NewKeyValues[0];
				dataRow2["jmcJobAssemblyID"] = parm.NewKeyValues[1];
				dataRow2["jmcJobOperationID"] = parm.NewKeyValues[2];
				dataRow2["jmcJobSequence"] = parm.NewKeyValues[2];
				dataRow2.EndEdit();
				dataTable2.Rows.Add(dataRow2);
			}
			parm.Database.UpdateData(dataTable2, adapter, parm.SqlTransaction);
			parm.Database.ExecuteCommand("DELETE FROM JobCosts WHERE jmcJobID = " + parm.OldKeyValues[0].ToSql() + " And jmcJobAssemblyID = " + parm.OldKeyValues[1].ToSql() + " And jmcJobOperationID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
		}
		UpdateKeysOnSchedulesTables(parm);
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
		M1Database database = parm.Database;
		SqlTransaction sqlTransaction = parm.SqlTransaction;
		string jobId = parm.OldKeyValues[0].ToString();
		int assemblyId = Convert.ToInt32(parm.OldKeyValues[1]);
		string jobId2 = parm.NewKeyValues[0].ToString();
		int assemblyId2 = Convert.ToInt32(parm.NewKeyValues[1]);
		ScheduleOperators.SetDefaultAssemblyDates(jobId, assemblyId, database, sqlTransaction);
		ScheduleOperators.SetDefaultAssemblyDates(jobId2, assemblyId2, database, sqlTransaction);
		ScheduleOperators.SetDefaultMaterialsDatesByAssembly(jobId, assemblyId, database, sqlTransaction);
		ScheduleOperators.SetDefaultMaterialsDatesByAssembly(jobId2, assemblyId2, database, sqlTransaction);
		ScheduleOperators.SetDefaultJobDates(jobId, database, sqlTransaction);
		ScheduleOperators.SetDefaultJobDates(jobId2, database, sqlTransaction);
	}

	private static void UpdateKeysOnSchedulesTables(ChangeIDProcessingParms parm)
	{
		M1Database database = parm.Database;
		short changeIDType = parm.ChangeIDType;
		SqlTransaction sqlTransaction = parm.SqlTransaction;
		string text = parm.OldKeyValues[0].ToString();
		int num = Convert.ToInt32(parm.OldKeyValues[1]);
		int num2 = Convert.ToInt32(parm.OldKeyValues[2]);
		string text2 = parm.NewKeyValues[0].ToString();
		int num3 = Convert.ToInt32(parm.NewKeyValues[1]);
		int num4 = Convert.ToInt32(parm.NewKeyValues[2]);
		bool num5 = ScheduleOperators.IsJobOnScheduleTables(text, database, sqlTransaction);
		bool flag = ScheduleOperators.IsJobOnScheduleTables(text2, database, sqlTransaction);
		if (!num5 && !flag)
		{
			return;
		}
		bool flag2 = ScheduleOperators.IsJobOperationOnScheduleTables(text, num, num2, database, sqlTransaction);
		bool flag3 = ScheduleOperators.IsJobAssemblyOnScheduleTables(text2, num3, database, sqlTransaction);
		if (changeIDType == 1 && flag2)
		{
			if (flag3)
			{
				ScheduleOperators.UpdateKeysJobOperationOnScheduleTables(text, text2, num, num3, num2, num4, database, sqlTransaction);
				if (text != text2)
				{
					ScheduleOperators.UpdateKeysJobOperationScenariosOnScheduleTables(text, text2, num, num3, num2, num4, database, sqlTransaction);
				}
			}
			else
			{
				ScheduleOperators.SetDefaultJobOperationDates(text, num, num2, database, sqlTransaction);
				ScheduleOperators.SetDefaultMaterialsDatesWithRelatedJob(text2, num, num2, database, sqlTransaction);
				ScheduleOperators.DeleteJobOperationOnScheduleTables(text, num, num2, database, sqlTransaction, includeMasterScenario: true);
			}
		}
		else if (changeIDType == 2 || changeIDType == 3)
		{
			ScheduleOperators.SetDefaultJobOperationDates(text, num, num2, database, sqlTransaction);
			ScheduleOperators.SetDefaultMaterialsDatesWithRelatedJob(text2, num, num2, database, sqlTransaction);
			ScheduleOperators.DeleteJobOperationOnScheduleTables(text, num, num2, database, sqlTransaction, includeMasterScenario: true);
			ScheduleOperators.SetDefaultJobOperationDates(text2, num3, num4, database, sqlTransaction);
			ScheduleOperators.SetDefaultMaterialsDatesWithRelatedJob(text2, num3, num4, database, sqlTransaction);
			ScheduleOperators.DeleteJobOperationOnScheduleTables(text2, num3, num4, database, sqlTransaction, includeMasterScenario: true);
		}
	}
}
