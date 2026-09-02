using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DBDefaults;

[DBCreateDefault("Create default Job Priorities")]
public class JobPriorities
{
	public JobPriorities(DBCreateDefaultParms parm)
	{
		foreach (KeyValuePair<string, string> item in new Dictionary<string, string>
		{
			{ "1", "Low" },
			{ "2", "Normal" },
			{ "3", "High" },
			{ "4", "Urgent" },
			{ "5", "Critical" }
		})
		{
			SqlDataAdapter adapter;
			DataTable dataTable = parm.ServerManager.GetDataTable(null, parm.User, parm.DatabaseName, 0, "Select * From jobPriorities", fillSchema: true, out adapter);
			DataRow dataRow = dataTable.NewRow();
			dataRow.BeginEdit();
			dataRow.BlankRow();
			dataRow.SetField("jmjJobPriorityID", item.Key.ToString());
			dataRow.SetField("jmjDescription", item.Value.ToString());
			dataRow.EndEdit();
			dataTable.Rows.Add(dataRow);
			parm.ServerManager.UpdateData(null, parm.User, parm.DatabaseName, new DataRow[1] { dataRow }, adapter);
		}
	}
}
