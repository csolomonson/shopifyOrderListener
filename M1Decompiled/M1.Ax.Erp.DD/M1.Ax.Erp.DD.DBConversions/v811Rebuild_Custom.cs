using System.Collections.Generic;
using System.Data;
using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("8.10.050", "Convert custom tables to support unicode", "2013-10-17")]
public class v811Rebuild_Custom
{
	public v811Rebuild_Custom(DBConversionParms parms)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Dictionary<string, List<string>> dictionary2 = new Dictionary<string, List<string>>();
		DataTable dataTable = parms.DataDictionary.GetDataTable("Select dtTable, dtSQLView From DDTables Where dtCustom = 1 Order By dtTable");
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row3 in dataTable.Rows)
			{
				if (!row3.Field<bool>("dtSQLView"))
				{
					parms.Dmo.RemoveDependencies(null, parms.User, parms.DatabaseName, row3.Field<string>("dtTable").Trim(), string.Empty, string.Empty, enforced: true, dictionary);
					parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, row3.Field<string>("dtTable").Trim(), disableTriggers: true, parms.Messages);
					if (dictionary.Count != 0)
					{
						parms.Dmo.AddDependencies(null, parms.User, parms.DatabaseName, dictionary, dictionary2);
						dictionary.Clear();
					}
				}
			}
		}
		DataTable dataTable2 = parms.DataDictionary.GetDataTable("Select dtTable, dtSQLView, dtViewSeq From DDTables Where dtCustom = 1 and dtSQLView = 1 Order By dtViewSeq");
		if (dataTable2.Rows.Count != 0)
		{
			foreach (DataRow row4 in dataTable2.Rows)
			{
				parms.Dmo.RefreshView(null, parms.User, parms.DatabaseName, row4.Field<string>("dtTable").Trim(), dictionary2);
			}
		}
		if (dictionary2.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<string, List<string>> item in dictionary2)
		{
			string text = item.Key + "\n\r";
			foreach (string item2 in item.Value)
			{
				text = text + item2 + "\n\r";
			}
			parms.Messages.Add(text);
		}
	}
}
