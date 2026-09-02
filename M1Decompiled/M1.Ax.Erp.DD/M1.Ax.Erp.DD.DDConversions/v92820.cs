using System.Data;
using M1.Core;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("9.2.820", "", "")]
public class v92820
{
	public v92820(DDConversionParms parms)
	{
		string text = string.Empty;
		DataTable dataTable = parms.DmoDD.GetDataTable(parms.DatabaseName, "Select SERVERPROPERTY('collation') as server_collation");
		if (dataTable == null || dataTable.Rows.Count != 0)
		{
			text = dataTable?.Rows[0]["server_collation"].ToString();
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDSecurityGroups WHERE dzDataset NOT IN (SELECT name COLLATE " + text + " FROM sys.databases)");
			parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDSecurityReports WHERE drDataset NOT IN (SELECT name COLLATE " + text + " FROM sys.databases)");
			parms.DmoDD.ExecuteCommand(parms.DatabaseName, "DELETE FROM DDSecurityTables WHERE dtDataset NOT IN (SELECT name COLLATE " + text + " FROM sys.databases)");
		}
	}
}
