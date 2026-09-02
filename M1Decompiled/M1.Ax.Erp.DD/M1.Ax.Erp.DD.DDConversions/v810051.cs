using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.DD.DDConversions;

[DDConversion("8.10.051", "Convert -1 to 1 in queries, -1 to True and 0 to False in code for all boolean fields", "")]
public class v810051
{
	private List<string> bitFields = new List<string>();

	public v810051(DDConversionParms parms)
	{
		processData(parms, new string[3] { "dgFlds", "dgFrom", "dgWher" }, "Select dgGridID,dgUserID,dgFlds,dgFrom,dgWher From DDGridDetails Where (dgFlds Like '%-1%' Or dgFrom Like '%-1%' Or dgWher Like '%-1%') And dgCustom <> 0");
		processData(parms, new string[2] { "dfFFil", "dfRelatedTableFilter" }, "Select dfUniqueID,dfTable,dfField,dfFFil,dfRelatedTableFilter From DDFields Where (dfFFil like '%-1%' Or dfRelatedTableFilter Like '%-1%') And dfCustom <> 0");
		processData(parms, new string[1] { "dlFilter" }, "Select dlObjectID,dlLine,dlFilter From DDObjectDetails Where (dlFilter like '%-1%') And dlCustom <> 0");
		processData(parms, new string[1] { "dtClosedExtraSetExpression" }, "Select dtUniqueID,dtTable,dtClosedExtraSetExpression From DDTables Where (dtClosedExtraSetExpression like '%-1%') And dtCustom <> 0");
		processData(parms, new string[1] { "dkCode" }, "Select dkCodeID,dkCode From DDCode Where (dkCode like '%-1%') And dkCustom <> 0");
		processData(parms, new string[1] { "dePropertiesUser" }, "Select deFormID, deClassID, deControlName, dePropertiesUser From DDFormDetails Where (dePropertiesUser like '%-1%') And deClassID like '%M1ComboBox%' And dePropertiesUser like '%Search.RowSource =%' And deCustom <> 0");
		processData(parms, new string[1] { "dePropertiesUser" }, "Select deFormID, deClassID, deControlName, dePropertiesUser From DDFormDetails Where (dePropertiesUser like '%- 1%') And deClassID like '%M1ComboBox%' And dePropertiesUser like '%Search.RowSource =%' And deCustom <> 0");
		processData(parms, new string[1] { "daDefault" }, "select daDefault from DDFieldUserSettings where daDefault like '%-1%'");
	}

	private void processData(DDConversionParms parms, string[] fields, string query)
	{
		SqlDataAdapter adapter;
		DataTable dataTable = parms.DmoDD.GetDataTable(null, parms.DatabaseName, query, fillSchema: false, out adapter);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			foreach (string field in fields)
			{
				processCodeField(row, field, parms);
			}
		}
		parms.DmoDD.UpdateData(null, null, parms.DatabaseName, dataTable, adapter, null);
	}

	private void processField(DataRow row, string field)
	{
		if (row[field] == DBNull.Value)
		{
			return;
		}
		string text = row.Field<string>(field);
		StringBuilder stringBuilder = new StringBuilder();
		int num = text.IndexOf("-1");
		bool flag = false;
		while (num != -1)
		{
			stringBuilder.Append(text.Substring(0, num));
			string text2 = text.Substring(0, num).TrimEnd(' ');
			text = text.Substring(num + 2);
			if (text2.EndsWith("=") || text2.EndsWith("<>") || text2.EndsWith("= '") || text2.EndsWith("<> '"))
			{
				stringBuilder.Append("1");
				flag = true;
			}
			else
			{
				stringBuilder.Append("-1");
			}
			num = text.IndexOf("-1");
		}
		if (flag)
		{
			stringBuilder.Append(text);
			row[field] = stringBuilder.ToString();
		}
	}

	private bool isFieldABit(DDConversionParms parms, string field)
	{
		if (bitFields.Contains(field, StringComparer.CurrentCultureIgnoreCase))
		{
			return true;
		}
		DataTable dataTable = parms.DmoDD.GetDataTable(parms.DatabaseName, "Select dfdbtype From DDFields Where dfField = " + M1Util.ConvertToSql(field));
		if (dataTable.Rows.Count != 0 && dataTable.Rows[0].Field<string>("dfdbtype").Equals("bit", StringComparison.CurrentCultureIgnoreCase))
		{
			bitFields.Add(field);
			return true;
		}
		return false;
	}

	private string getFieldInLine(string line, ref bool isFieldsValue)
	{
		line = line.TrimEnd(' ');
		if (line.EndsWith("="))
		{
			line = line.Substring(0, line.Length - 1);
		}
		else if (line.EndsWith("<>"))
		{
			line = line.Substring(0, line.Length - 2);
		}
		else if (line.EndsWith("= '"))
		{
			line = line.Substring(0, line.Length - 3);
		}
		else if (line.EndsWith("<> '"))
		{
			line = line.Substring(0, line.Length - 4);
		}
		line = line.TrimEnd(' ');
		if (line.EndsWith("\").Value", StringComparison.CurrentCultureIgnoreCase))
		{
			line = line.Substring(0, line.Length - 8);
			int num = line.LastIndexOf('"');
			if (num != -1)
			{
				isFieldsValue = true;
				return line.Substring(num + 1);
			}
		}
		else if (!line.EndsWith(")"))
		{
			int num = line.LastIndexOfAny(new char[4] { ',', ' ', '"', '(' });
			if (num != -1)
			{
				line = line.Substring(num + 1);
			}
			if (line.IndexOfAny(new char[3] { '.', '(', ')' }) == -1)
			{
				isFieldsValue = false;
				return line;
			}
		}
		return string.Empty;
	}

	private void processCodeField(DataRow row, string field, DDConversionParms parms)
	{
		if (row[field] == DBNull.Value)
		{
			return;
		}
		string text = row.Field<string>(field);
		StringBuilder stringBuilder = new StringBuilder();
		int num = text.IndexOf("-1");
		bool flag = false;
		bool isFieldsValue = false;
		while (num != -1)
		{
			stringBuilder.Append(text.Substring(0, num));
			string text2 = text.Substring(0, num).TrimEnd(' ');
			text = text.Substring(num + 2);
			num = text2.LastIndexOf('\r');
			if (num != -1)
			{
				text2 = text2.Substring(num + 1);
			}
			if (text2.EndsWith("=") || text2.EndsWith("<>") || text2.EndsWith("= '") || text2.EndsWith("<> '"))
			{
				string fieldInLine = getFieldInLine(text2, ref isFieldsValue);
				if (fieldInLine.Length != 0 && isFieldABit(parms, fieldInLine))
				{
					if (isFieldsValue)
					{
						stringBuilder.Append("True");
					}
					else
					{
						stringBuilder.Append("1");
					}
					flag = true;
				}
				else
				{
					stringBuilder.Append("-1");
				}
			}
			else
			{
				stringBuilder.Append("-1");
			}
			num = text.IndexOf("-1");
		}
		if (flag)
		{
			stringBuilder.Append(text);
			row[field] = stringBuilder.ToString();
		}
	}
}
