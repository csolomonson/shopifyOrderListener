using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DBDefaults;

[DBCreateDefault("Create default Change Request Types")]
public class ChangeRequestTypes
{
	public ChangeRequestTypes(DBCreateDefaultParms parm)
	{
		foreach (KeyValuePair<string, string> item in new Dictionary<string, string>
		{
			{ "DEF", "Defect" },
			{ "ENH", "Enhancement" }
		})
		{
			SqlDataAdapter adapter;
			DataTable dataTable = parm.ServerManager.GetDataTable(null, parm.User, parm.DatabaseName, 0, "Select * From changeRequestTypes", fillSchema: true, out adapter);
			DataRow dataRow = dataTable.NewRow();
			dataRow.BeginEdit();
			dataRow.BlankRow();
			dataRow.SetField("chtChangeRequestTypeID", item.Key.ToString());
			dataRow.SetField("chtDescription", item.Value.ToString());
			dataRow.SetField("chtCreatedDate", DateTime.Now);
			dataRow.SetField("chtCreatedBy", parm.User.ID);
			dataRow.EndEdit();
			dataTable.Rows.Add(dataRow);
			parm.ServerManager.UpdateData(null, parm.User, parm.DatabaseName, new DataRow[1] { dataRow }, adapter);
		}
	}
}
