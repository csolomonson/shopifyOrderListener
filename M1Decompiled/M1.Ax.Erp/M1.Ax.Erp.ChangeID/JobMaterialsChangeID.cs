using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Core.Database;
using M1.Extensions;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("JobMaterials")]
public class JobMaterialsChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
		SqlDataAdapter adapter;
		DataTable dataTable = parm.Database.GetDataTable("SELECT * FROM JobCosts WHERE jmcJobID = " + parm.OldKeyValues[0].ToSql() + " And jmcJobAssemblyID = " + parm.OldKeyValues[1].ToSql() + " And jmcJobMaterialID = " + parm.OldKeyValues[2].ToSql() + " ORDER BY jmcJobID, jmcJobAssemblyID, jmcJobType, jmcJobSequence, jmcCostSequence", fillSchema: false, out adapter, parm.SqlTransaction);
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
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
			dataRow2["jmcJobMaterialID"] = parm.NewKeyValues[2];
			dataRow2["jmcJobSequence"] = parm.NewKeyValues[2];
			dataRow2.EndEdit();
			dataTable2.Rows.Add(dataRow2);
		}
		parm.Database.UpdateData(dataTable2, adapter, parm.SqlTransaction);
		parm.Database.ExecuteCommand("DELETE FROM JobCosts WHERE jmcJobID = " + parm.OldKeyValues[0].ToSql() + " And jmcJobAssemblyID = " + parm.OldKeyValues[1].ToSql() + " And jmcJobMaterialID = " + parm.OldKeyValues[2].ToSql(), parm.SqlTransaction);
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
	}
}
