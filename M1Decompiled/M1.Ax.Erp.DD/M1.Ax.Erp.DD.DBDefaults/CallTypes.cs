using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DBDefaults;

[DBCreateDefault("Create default Call Types")]
public class CallTypes
{
	public CallTypes(DBCreateDefaultParms parm)
	{
		createDefaultCallTypesAdd("SALES", "Sales", "C", 0m, 0m, -1m, parm);
		createDefaultCallTypesAdd("SUPP", "Support", "O", -1m, -1m, 0m, parm);
		createDefaultCallTypesAdd("QUAL", "Quality", "O", 0m, 0m, -1m, parm);
		createDefaultCallTypesAdd("FIN", "Financial", "O", 0m, 0m, 0m, parm);
	}

	private void createDefaultCallTypesAdd(string id, string desc, string status, decimal inbound, decimal billable, decimal internalOnly, DBCreateDefaultParms parm)
	{
		SqlDataAdapter adapter;
		DataTable dataTable = parm.ServerManager.GetDataTable(null, parm.User, parm.DatabaseName, 0, "Select * From CallTypes", fillSchema: true, out adapter);
		DataRow dataRow = dataTable.NewRow();
		dataRow.BeginEdit();
		dataRow.BlankRow();
		dataRow.SetField("kbtCallTypeID", id);
		dataRow.SetField("kbtDescription", desc);
		dataRow.SetField("kbtCallStatus", status);
		dataRow.SetField("kbtInboundCall", inbound);
		dataRow.SetField("kbtBillableCall", billable);
		dataRow.SetField("kbtInternalOnlyCall", internalOnly);
		dataRow.SetField("kbtCreatedDate", DateTime.Now);
		dataRow.SetField("kbtCreatedBy", parm.User.ID);
		dataRow.EndEdit();
		dataTable.Rows.Add(dataRow);
		parm.ServerManager.UpdateData(null, parm.User, parm.DatabaseName, new DataRow[1] { dataRow }, adapter);
	}
}
