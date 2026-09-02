using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.120", "Combine code rows for product configurator", "2016-01-07")]
public class v900120b
{
	public v900120b(DBConversionParms parms)
	{
		DataTable dataTable = parms.Database.GetDataTable("Select xaoFormID From FormDefinitions Where LTrim(IsNull(Convert(nvarchar(100),xaoCode),'')) <> '' Group By xaoFormID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		SqlDataAdapter adapter = new SqlDataAdapter();
		foreach (DataRow row2 in dataTable.Rows)
		{
			string text = row2.Field<string>("xaoFormID").Trim();
			DataTable dataTable2 = parms.Database.GetDataTable("Select xaoControlName,xaoClassID,xaoCode From FormDefinitions Where xaoFormID = '" + text.ToString() + "' And Not xaoCode Is Null");
			StringBuilder stringBuilder = CombineCodeForAllRows(dataTable2);
			if (stringBuilder != null && stringBuilder.Length != 0)
			{
				DataTable dataTable3 = parms.Database.GetDataTable("Select xaoFormID, xaoControlName, xaoCode From FormDefinitions Where xaoFormID = '" + text + "' And IsNull(Convert(nvarchar(100),xaoControlName),'') = ''", fillSchema: true, out adapter);
				if (dataTable3.Rows.Count == 0)
				{
					DataRow row = dataTable3.NewRow();
					row.SetField("xaoFormID", text);
					row.SetField("xaoControlName", string.Empty);
					dataTable3.Rows.Add(row);
				}
				dataTable3.Rows[0].SetField("xaoCode", stringBuilder.ToString());
				parms.Database.UpdateData(dataTable3, adapter, null);
			}
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FormDefinitions Set xaoCode = NULL Where xaoFormID = '" + text.ToString() + "' And IsNull(Convert(nvarchar(100),xaoControlName),'') <> ''");
		}
	}

	private StringBuilder CombineCodeForAllRows(DataTable data)
	{
		if (data != null && data.Rows.Count != 0)
		{
			List<DataRow> list = new List<DataRow>();
			List<DataRow> list2 = new List<DataRow>();
			List<DataRow> list3 = new List<DataRow>();
			DataRow[] array = data.Select(string.Empty, "xaoControlName");
			foreach (DataRow dataRow in array)
			{
				if (dataRow.Field<string>("xaoClassID").Trim().Length == 0)
				{
					list.Add(dataRow);
				}
				else if (dataRow.Field<string>("xaoClassID").Trim().EndsWith("M1DataControl", StringComparison.CurrentCultureIgnoreCase))
				{
					list2.Add(dataRow);
				}
				else
				{
					list3.Add(dataRow);
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			CombineCodeForAllRows(list, stringBuilder);
			CombineCodeForAllRows(list2, stringBuilder);
			CombineCodeForAllRows(list3, stringBuilder);
			return stringBuilder;
		}
		return null;
	}

	private void CombineCodeForAllRows(List<DataRow> rows, StringBuilder allCode)
	{
		if (rows == null || rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in rows)
		{
			string text = row.Field<string>("xaoCode");
			if (text != null && text.Length != 0)
			{
				if (allCode.Length != 0 && allCode[allCode.Length - 1] != '\r' && allCode[allCode.Length - 1] != '\n')
				{
					allCode.Append('\r');
				}
				allCode.Append(text);
			}
		}
		allCode.Replace("\r\n", "\r");
	}
}
