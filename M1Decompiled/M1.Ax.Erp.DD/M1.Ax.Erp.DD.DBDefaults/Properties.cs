using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.DD.DBDefaults;

[DBCreateDefault("Create default properties")]
public class Properties
{
	public Properties(DBCreateDefaultParms parm)
	{
		foreach (string item in new List<string> { "ProductionProperties", "FinancialProperties", "ShippingProperties", "WebGearProperties" })
		{
			SqlDataAdapter adapter;
			DataTable dataTable = parm.Database.GetDataTable("SELECT * FROM " + item, fillSchema: true, out adapter);
			if (dataTable.Rows.Count == 0)
			{
				M1BindingSource m1BindingSource = new M1BindingSource(parm.Database);
				m1BindingSource.LoadDefinition(string.Empty, item, dataTable);
				m1BindingSource.Query.DataAdapter = adapter;
				m1BindingSource.AddNew();
				m1BindingSource.SaveData();
			}
		}
	}
}
