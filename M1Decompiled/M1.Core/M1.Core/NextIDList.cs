using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class NextIDList : KeyedCollection<string, NextIDInfo>
{
	private M1Database _Database;

	private M1DataDictionary _DataDictionary;

	public NextIDList(M1Database database)
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
		_Database = database;
		_DataDictionary = _Database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
	}

	protected override string GetKeyForItem(NextIDInfo item)
	{
		return item.Table;
	}

	public NextIDInfo GetNextIDInfo(string table)
	{
		if (Contains(table))
		{
			return base[table];
		}
		NextIDInfo nextIDInfo = new NextIDInfo();
		nextIDInfo.Table = table;
		SqlCommand sqlCommand = _Database.NewSqlCommand("Select xanAutoIncrement,xanIncrementAmount,xanNumericOnly,xanDatasets from NextIDs where xanTable = @Table");
		sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = table;
		DataTable dataTable = _Database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			DataRow row = dataTable.Rows[0];
			nextIDInfo.AutoIncrement = (DatabaseAutoIncrement)row.Field<byte>("xanAutoIncrement");
			nextIDInfo.IncrementAmount = row.Field<short>("xanIncrementAmount");
			nextIDInfo.Databases = row.Field<string>("xanDatasets");
			if (nextIDInfo.Databases == null)
			{
				nextIDInfo.Databases = string.Empty;
			}
			nextIDInfo.NumericOnly = (DatabaseNumericOnly)row.Field<byte>("xanNumericOnly");
		}
		Add(nextIDInfo);
		return nextIDInfo;
	}

	public void FreeUnusedNextIDForTable(string table, string value)
	{
		FreeUnusedNextIDForTable(table, value, GetNextIDInfo(table).Databases);
	}

	public void FreeUnusedNextIDForTable(string table, string value, string databases)
	{
		List<string> list = new List<string>();
		if (databases != null && databases.Length != 0)
		{
			list.AddRange(databases.Split(','));
		}
		if (!list.Contains(_Database.ID, StringComparer.CurrentCultureIgnoreCase))
		{
			list.Add(_Database.ID);
		}
		foreach (string item in list)
		{
			SqlCommand sqlCommand = _Database.NewSqlCommand("Update " + item + ".dbo.NextIDs Set xanNextID = @Value Where xanTable = @Table And xanNextID > @Value");
			sqlCommand.Parameters.Add(new SqlParameter("@Value", SqlDbType.NVarChar)).Value = value;
			sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = table;
			_Database.ExecuteCommand(sqlCommand);
		}
	}

	public void FreeDeletedNextIDForTable(string table, string value)
	{
		FreeDeletedNextIDForTable(table, value, GetNextIDInfo(table).Databases);
	}

	public void FreeDeletedNextIDForTable(string table, string value, string databases)
	{
		List<string> list = new List<string>();
		SqlCommand sqlCommand = _DataDictionary.NewSqlCommand("select dtKeyFields, dtIncrementAmountUser, dtIncrementAmount, dtNumericOnly, dtInitialValue from DDTables where dtTable = @table");
		sqlCommand.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar)).Value = table;
		DataTable dataTable = _DataDictionary.GetDataTable(sqlCommand);
		bool flag = false;
		short num = 0;
		string format = string.Empty;
		FieldTypeEnum fieldTypeEnum = FieldTypeEnum.None;
		if (dataTable.Rows.Count != 0)
		{
			string[] array = dataTable.Rows[0].Field<string>("dtKeyFields").Split(',');
			flag = array.Length == 1;
			string value2 = array[array.Length - 1];
			num = dataTable.Rows[0].Field<short>("dtIncrementAmountUser");
			if (num == 0)
			{
				num = dataTable.Rows[0].Field<short>("dtIncrementAmount");
			}
			sqlCommand = _DataDictionary.NewSqlCommand("Select dfDBType, dfLength, dfDecimals, dfFormat From DDFields Where dfTable = @Table And dfField = @Field");
			sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = table;
			sqlCommand.Parameters.Add(new SqlParameter("@Field", SqlDbType.NVarChar)).Value = value2;
			DataTable dataTable2 = _DataDictionary.GetDataTable(sqlCommand);
			if (dataTable2.Rows.Count != 0)
			{
				format = dataTable2.Rows[0].Field<string>("dfFormat");
				fieldTypeEnum = FieldDefinition.charToFieldType(dataTable2.Rows[0].Field<string>("dfDBType"));
			}
		}
		if (!(fieldTypeEnum != FieldTypeEnum.None && flag))
		{
			return;
		}
		object value3 = IncrementValue(value, num, format, fieldTypeEnum);
		if (databases != null && databases.Length != 0)
		{
			list.AddRange(databases.Split(','));
		}
		if (!list.Contains(_Database.ID, StringComparer.CurrentCultureIgnoreCase))
		{
			list.Add(_Database.ID);
		}
		foreach (string item in list)
		{
			sqlCommand = _Database.NewSqlCommand("Update " + item + ".dbo.NextIDs Set xanNextID = @Value Where xanTable = @Table And xanNextID = @NextValue");
			sqlCommand.Parameters.Add(new SqlParameter("@Value", SqlDbType.NVarChar)).Value = value;
			sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = table;
			sqlCommand.Parameters.Add(new SqlParameter("@NextValue", SqlDbType.NVarChar)).Value = value3;
			_Database.ExecuteCommand(sqlCommand);
		}
	}

	public object GetNextIDForTable(string tableID)
	{
		return GetNextIDForTable(tableID, null);
	}

	public object GetNextIDForTable(string tableID, object[] keyValues)
	{
		return GetNextIDForTable(tableID, keyValues, null, null);
	}

	public object GetNextIDForTable(string tableID, object[] keyValues, SqlTransaction trans)
	{
		return GetNextIDForTable(tableID, keyValues, null, trans);
	}

	public object GetNextIDForTable(string tableID, object[] keyValues, DataTable currentRows, SqlTransaction transaction)
	{
		List<string> list = new List<string>();
		list.Add(_Database.ID);
		SqlCommand sqlCommand = _DataDictionary.NewSqlCommand("select dtKeyFields, dtIncrementAmountUser, dtIncrementAmount, dtNumericOnly, dtInitialValue from DDTables where dtTable = @table");
		sqlCommand.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar)).Value = tableID;
		DataTable dataTable = _DataDictionary.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			throw new M1Exception("Table '" + tableID + "' does not exist in the data dictionary.");
		}
		DataRow row = dataTable.Rows[0];
		string[] array = row.Field<string>("dtKeyFields").Split(',');
		bool flag = array.Length == 1;
		string text = array[array.Length - 1];
		string text2 = row.Field<string>("dtInitialValue").Trim();
		short num = row.Field<short>("dtIncrementAmountUser");
		if (num == 0)
		{
			num = row.Field<short>("dtIncrementAmount");
		}
		TableKeyNumericOnlyEnum tableKeyNumericOnlyEnum = row.Field<TableKeyNumericOnlyEnum>("dtNumericOnly");
		sqlCommand = _DataDictionary.NewSqlCommand("Select dfDBType, dfLength, dfDecimals, dfFormat From DDFields Where dfTable = @Table And dfField = @Field");
		sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = tableID;
		sqlCommand.Parameters.Add(new SqlParameter("@Field", SqlDbType.NVarChar)).Value = text;
		DataTable dataTable2 = _DataDictionary.GetDataTable(sqlCommand);
		if (dataTable2.Rows.Count == 0)
		{
			throw new M1Exception("Field '" + text + "' does not exist in the data dictionary.");
		}
		DataRow row2 = dataTable2.Rows[0];
		byte b = row2.Field<byte>("dfLength");
		byte b2 = row2.Field<byte>("dfDecimals");
		FieldTypeEnum fieldTypeEnum = FieldDefinition.charToFieldType(row2.Field<string>("dfDBType"));
		string text3 = row2.Field<string>("dfFormat");
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		string filterExpression = string.Empty;
		if (keyValues != null && keyValues.Length != 0)
		{
			for (int i = 0; i < array.Length - 1; i++)
			{
				if (array[i].Length != 0)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(" And ");
						stringBuilder2.Append(" And ");
					}
					stringBuilder.Append(array[i] + " = " + keyValues[i].ToSql());
					stringBuilder2.Append(array[i] + " = " + keyValues[i].ToLinq());
				}
			}
			if (stringBuilder.Length != 0)
			{
				filterExpression = stringBuilder2.ToString();
				stringBuilder.Insert(0, " Where ");
			}
		}
		object obj = string.Empty;
		sqlCommand = _Database.NewSqlCommand("Select xanNextID,xanIncrementAmount,xanNumericOnly,xanDatasets from NextIDs where xanTable = @Table");
		sqlCommand.Parameters.Add(new SqlParameter("@Table", SqlDbType.NVarChar)).Value = tableID;
		DataTable dataTable3 = _Database.GetDataTable(sqlCommand, transaction);
		if (dataTable3.Rows.Count != 0)
		{
			DataRow dataRow = dataTable3.Rows[0];
			obj = dataRow.Field<string>("xanNextID").Trim();
			if (Convert.ToInt16(dataRow["xanIncrementAmount"]) > 0)
			{
				num = Convert.ToInt16(dataRow["xanIncrementAmount"]);
			}
			tableKeyNumericOnlyEnum = (TableKeyNumericOnlyEnum)Convert.ToByte(dataRow["xanNumericOnly"]);
			string text4 = dataRow.Field<string>("xanDatasets");
			if (!string.IsNullOrWhiteSpace(text4))
			{
				list = text4.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
				if (!list.Contains(_Database.ID, StringComparer.CurrentCultureIgnoreCase))
				{
					list.Add(_Database.ID);
				}
			}
		}
		if (tableKeyNumericOnlyEnum != TableKeyNumericOnlyEnum.No)
		{
			text3 = new string('9', b);
		}
		bool flag2 = M1Util.IsNumeric(text3);
		if (obj.ToString().Length == 0 || !flag)
		{
			object obj2 = null;
			if (currentRows == null)
			{
				string text5 = "Select Max(" + text + ") AS NextNo From " + tableID;
				if (FieldDefinition.IsFieldTypeAString(fieldTypeEnum))
				{
					StringBuilder stringBuilder3 = new StringBuilder();
					stringBuilder3.Append("Select Max(Len(" + text + ")) as max_len from " + tableID + stringBuilder.ToString());
					if (flag2 && stringBuilder.Length != 0)
					{
						stringBuilder3.Append(" And IsNumeric(" + text + ") <> 0");
					}
					object obj3 = _Database.ExecuteScalar(stringBuilder3.ToString(), transaction);
					if (obj3 != DBNull.Value && obj3 != null && Convert.ToInt32(obj3) > 0)
					{
						if (stringBuilder.Length == 0)
						{
							stringBuilder.Append(" Where Len(" + text + ") >= " + Convert.ToInt32(obj3).ToSql());
						}
						else
						{
							stringBuilder.Append(" And Len(" + text + ") >= " + Convert.ToInt32(obj3).ToSql());
						}
					}
					if (flag2)
					{
						text5 = "Select Max(Convert(bigint," + text + ")) As NextNo From " + tableID;
						if (stringBuilder.Length == 0)
						{
							stringBuilder.Append(" Where IsNumeric(" + text + ") <> 0");
						}
						else
						{
							stringBuilder.Append(" And IsNumeric(" + text + ") <> 0");
						}
					}
				}
				obj2 = _Database.ExecuteScalar(text5 + stringBuilder.ToString(), transaction);
			}
			else
			{
				DataRow[] array2 = currentRows.Select(filterExpression, text);
				if (array2.Length != 0)
				{
					obj2 = array2[array2.Length - 1][text];
				}
			}
			if (obj2 != DBNull.Value && obj2 != null)
			{
				obj = obj2.ToString();
				obj = IncrementValue(obj, num, text3, fieldTypeEnum);
			}
			if (obj.ToString().Trim().Length == 0 || obj.ToString().Trim().Equals("0"))
			{
				if ((text2.Length == 0 || text2.Equals("0")) && num > 0)
				{
					text2 = num.ToString();
				}
				obj = (FieldDefinition.IsFieldTypeAString(fieldTypeEnum) ? ((text2.Length != 0) ? text2 : (flag2 ? "1" : ((b <= 5) ? (new string('0', b - 1) + "1") : "00001"))) : ((text2.Length != 0 && !text2.Equals("0")) ? text2 : "1"));
			}
		}
		else
		{
			bool flag3 = true;
			StringBuilder stringBuilder4 = new StringBuilder();
			while (flag3)
			{
				flag3 = false;
				stringBuilder4.Length = 0;
				stringBuilder4.Append("Select ");
				foreach (string item in list)
				{
					stringBuilder4.Append("(Select " + text + " As NextNo From " + item + ".dbo." + tableID);
					if (stringBuilder.Length == 0)
					{
						stringBuilder4.Append(" Where " + text + " = " + obj.ToSql());
					}
					else
					{
						stringBuilder4.Append(stringBuilder.ToString() + " And " + text + " = " + obj.ToSql());
					}
					stringBuilder4.Append(") As Field" + item + ",");
				}
				stringBuilder4.Length--;
				DataTable dataTable4 = _Database.GetDataTable(stringBuilder4.ToString(), transaction);
				if (dataTable4.Rows.Count == 0)
				{
					continue;
				}
				foreach (DataColumn column in dataTable4.Columns)
				{
					if (dataTable4.Rows[0][column] != DBNull.Value && dataTable4.Rows[0][column] != null)
					{
						flag3 = true;
						obj = IncrementValue(obj, num, text3, fieldTypeEnum);
						break;
					}
				}
			}
		}
		if (flag)
		{
			object obj4 = IncrementValue(obj, num, text3, fieldTypeEnum);
			foreach (string item2 in list)
			{
				if (_Database.ExecuteCommand("Update " + item2 + ".dbo.NextIDs Set xanNextID = " + obj4.ToString().ToSql() + " Where xanTable = " + tableID.ToSql(), transaction) == 0)
				{
					M1Database database = _Database;
					string[] obj5 = new string[9]
					{
						"Insert Into ",
						item2,
						".dbo.NextIDs (xanTable,xanNextID,xanNumericOnly) Values (",
						tableID.ToSql(),
						",",
						obj4.ToString().ToSql(),
						",",
						null,
						null
					};
					byte b3 = (byte)tableKeyNumericOnlyEnum;
					obj5[7] = b3.ToString();
					obj5[8] = ")";
					database.ExecuteCommand(string.Concat(obj5), transaction);
				}
			}
		}
		switch (fieldTypeEnum)
		{
		case FieldTypeEnum.Numeric:
			if (b2 == 0)
			{
				return Convert.ToInt32(obj);
			}
			return Convert.ToDouble(obj);
		case FieldTypeEnum.Float:
		case FieldTypeEnum.Money:
		case FieldTypeEnum.Real:
			return Convert.ToDouble(obj);
		case FieldTypeEnum.SmallInt:
			return Convert.ToInt16(obj);
		case FieldTypeEnum.Int:
			return Convert.ToInt32(obj);
		case FieldTypeEnum.BigInt:
			return Convert.ToInt64(obj);
		default:
			return obj;
		}
	}

	public object IncrementValue(object value, short incrementAmount, string format, FieldTypeEnum fieldType)
	{
		object empty = string.Empty;
		format = format.Trim();
		if (incrementAmount <= 0)
		{
			incrementAmount = 1;
		}
		if (FieldDefinition.IsFieldTypeAString(fieldType))
		{
			string text = value.ToString().Trim();
			if (M1Util.IsNumeric(format))
			{
				if (text.Length == 0 || !M1Util.IsNumeric(text))
				{
					return "1";
				}
				return (Convert.ToDouble(value) + (double)incrementAmount).ToString();
			}
			string text2 = string.Empty;
			int num = text.IndexOf('-');
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			bool flag = false;
			while (!flag)
			{
				flag = true;
				if (text.Length == 0)
				{
					text2 = ((getFormatAtPos(format, 0) != 'A') ? ("1" + text2) : ("A" + text2));
					continue;
				}
				num = text.Length - 1;
				char c = text[text.Length - 1];
				text = ((text.Length != 1) ? text.Substring(0, text.Length - 1) : string.Empty);
				if ((c >= 'A' && c < 'Z') || (c >= '0' && c < '9'))
				{
					text2 = (char)(c + 1) + text2;
					continue;
				}
				switch (c)
				{
				case 'Z':
					text2 = ((getFormatAtPos(format, num) != 'A') ? ("0" + text2) : ("A" + text2));
					flag = false;
					break;
				case '9':
					if (getFormatAtPos(format, num) == '9')
					{
						text2 = "0" + text2;
						flag = false;
					}
					else
					{
						text2 = "A" + text2;
					}
					break;
				case '#':
					text2 = "0" + text2;
					break;
				default:
					throw new M1Exception("Invalid character " + c + " in id in IncrementValue.");
				}
			}
			return text + text2;
		}
		switch (fieldType)
		{
		case FieldTypeEnum.BigInt:
		case FieldTypeEnum.Float:
		case FieldTypeEnum.Int:
		case FieldTypeEnum.Money:
		case FieldTypeEnum.Numeric:
		case FieldTypeEnum.Real:
		case FieldTypeEnum.SmallInt:
		case FieldTypeEnum.SmallMoney:
		case FieldTypeEnum.TinyInt:
			return Convert.ToInt32(value) + incrementAmount;
		default:
			throw new M1Exception("Unknown type " + fieldType.ToString() + " in IncrementValue.");
		}
	}

	private char getFormatAtPos(string format, int pos)
	{
		if (format.Trim().Length == 0)
		{
			return 'X';
		}
		return format[pos];
	}
}
