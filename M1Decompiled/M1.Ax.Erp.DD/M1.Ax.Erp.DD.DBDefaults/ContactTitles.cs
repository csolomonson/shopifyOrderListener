using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DBDefaults;

[DBCreateDefault("Create default Contact Titles")]
public class ContactTitles
{
	public ContactTitles(DBCreateDefaultParms parm)
	{
		foreach (KeyValuePair<string, string> item in new Dictionary<string, string>
		{
			{ "PRES", "President" },
			{ "VP", "Vice President" },
			{ "GM", "General Manager" },
			{ "PRODM", "Production Manager" }
		})
		{
			SqlDataAdapter adapter;
			DataTable dataTable = parm.ServerManager.GetDataTable(null, parm.User, parm.DatabaseName, 0, "Select * From ContactTitles", fillSchema: true, out adapter);
			DataRow dataRow = dataTable.NewRow();
			dataRow.BeginEdit();
			dataRow.BlankRow();
			dataRow.SetField("cmeContactTitleID", item.Key.ToString());
			dataRow.SetField("cmeDescription", item.Value.ToString());
			dataRow.SetField("cmeCreatedDate", DateTime.Now);
			dataRow.SetField("cmeCreatedBy", parm.User.ID);
			dataRow.EndEdit();
			dataTable.Rows.Add(dataRow);
			parm.ServerManager.UpdateData(null, parm.User, parm.DatabaseName, new DataRow[1] { dataRow }, adapter);
		}
	}
}
