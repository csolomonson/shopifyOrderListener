using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DBDefaults;

[DBCreateDefault("Create default Dataset Properties")]
public class DatasetProperties
{
	public DatasetProperties(DBCreateDefaultParms parm)
	{
		SqlDataAdapter adapter;
		DataTable dataTable = parm.ServerManager.GetDataTable(null, parm.User, parm.DatabaseName, 0, "Select * From DatasetProperties", fillSchema: true, out adapter);
		DataRow dataRow;
		if (dataTable.Rows.Count == 0)
		{
			dataRow = dataTable.NewRow();
			dataRow.BlankRow();
			dataTable.Rows.Add(dataRow);
		}
		else
		{
			dataRow = dataTable.Rows[0];
		}
		dataRow.SetField("xadBuyQuantityDecimals", 2);
		dataRow.SetField("xadSellQuantityDecimals", 2);
		dataRow.SetField("xadInventoryQuantityDecimals", 2);
		dataRow.SetField("xadExportFollowups", -1m);
		parm.ServerManager.UpdateData(null, parm.User, parm.DatabaseName, new DataRow[1] { dataRow }, adapter);
	}
}
