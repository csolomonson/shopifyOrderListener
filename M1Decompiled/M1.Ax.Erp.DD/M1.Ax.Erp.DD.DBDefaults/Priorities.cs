using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DBDefaults;

[DBCreateDefault("Create default Priorities")]
public class Priorities
{
	public Priorities(DBCreateDefaultParms parm)
	{
		foreach (KeyValuePair<int, string> item in new Dictionary<int, string>
		{
			{ 1, "Low" },
			{ 2, "Normal" },
			{ 3, "High" }
		})
		{
			SqlDataAdapter adapter;
			DataTable dataTable = parm.ServerManager.GetDataTable(null, parm.User, parm.DatabaseName, 0, "Select * From priorities", fillSchema: true, out adapter);
			DataRow dataRow = dataTable.NewRow();
			dataRow.BeginEdit();
			dataRow.BlankRow();
			dataRow.SetField("kbrPriorityID", item.Key.ToString());
			dataRow.SetField("kbrDescription", item.Value.ToString());
			dataRow.SetField("kbrCreatedDate", DateTime.Now);
			dataRow.SetField("kbrCreatedBy", parm.User.ID);
			dataRow.EndEdit();
			dataTable.Rows.Add(dataRow);
			parm.ServerManager.UpdateData(null, parm.User, parm.DatabaseName, new DataRow[1] { dataRow }, adapter);
		}
	}
}
