using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DBDefaults;

[DBCreateDefault("Create default GLCategories")]
public class GLCategories
{
	public GLCategories(DBCreateDefaultParms parm)
	{
		addCategory("CA", "Current Assets", 1, 1, parm);
		addCategory("NCA", "Non-current Assets", 2, 1, parm);
		addCategory("IA", "Intangible Assets", 3, 1, parm);
		addCategory("OA", "Other Assets", 4, 1, parm);
		addCategory("CL", "Current Liabilities", 1, 2, parm);
		addCategory("NCL", "Non-current Liabilities", 2, 2, parm);
		addCategory("OL", "Other Liabilities", 3, 2, parm);
		addCategory("EQ", "Equity", 1, 3, parm);
		addCategory("IN", "Income", 1, 4, parm);
		addCategory("COS", "Cost of Sales", 1, 5, parm);
		addCategory("FE", "Fixed Expenses", 1, 6, parm);
	}

	private void addCategory(string id, string desc, int seq, int type, DBCreateDefaultParms parm)
	{
		SqlDataAdapter adapter;
		DataTable dataTable = parm.ServerManager.GetDataTable(null, parm.User, parm.DatabaseName, 0, "Select * From GLCategories", fillSchema: true, out adapter);
		DataRow dataRow = dataTable.NewRow();
		dataRow.BlankRow();
		dataRow.BeginEdit();
		dataRow.SetField("gltGLCategoryID", id);
		dataRow.SetField("gltDescription", desc);
		dataRow.SetField("gltReportSequence", seq);
		dataRow.SetField("gltCategoryType", type);
		dataRow.SetField("gltCreatedBy", parm.User.ID);
		dataRow.SetField("gltCreatedDate", DateTime.Now);
		dataRow.EndEdit();
		dataTable.Rows.Add(dataRow);
		parm.ServerManager.UpdateData(null, parm.User, parm.DatabaseName, new DataRow[1] { dataRow }, adapter);
	}
}
