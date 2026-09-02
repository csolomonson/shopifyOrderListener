using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DBDefaults;

[DBCreateDefault("Create default Contact Methods")]
public class ContactMethods
{
	public ContactMethods(DBCreateDefaultParms parm)
	{
		foreach (KeyValuePair<string, string> item in new Dictionary<string, string>
		{
			{ "PHONE", "Phone" },
			{ "FAX", "Fax" },
			{ "EMAIL", "Email" }
		})
		{
			SqlDataAdapter adapter;
			DataTable dataTable = parm.ServerManager.GetDataTable(null, parm.User, parm.DatabaseName, 0, "Select * From ContactMethods", fillSchema: true, out adapter);
			DataRow dataRow = dataTable.NewRow();
			dataRow.BeginEdit();
			dataRow.BlankRow();
			dataRow.SetField("kbcContactMethodID", item.Key.ToString());
			dataRow.SetField("kbcDescription", item.Value.ToString());
			dataRow.SetField("kbcCreatedDate", DateTime.Now);
			dataRow.SetField("kbcCreatedBy", parm.User.ID);
			dataRow.EndEdit();
			dataTable.Rows.Add(dataRow);
			parm.ServerManager.UpdateData(null, parm.User, parm.DatabaseName, new DataRow[1] { dataRow }, adapter);
		}
	}
}
