using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace M1.Core;

public class DDFind
{
	private const string WordSeparators = "\t\" ,.<>!?{}[]/\\|=+-&%$#@*():;'_\r\n";

	public List<DDFindInfo> FindTextInDD(AppContext context, SqlConnection sqlConnection, M1User user, string databaseName, string textToFind, DDFieldContentType? typesToSearch, bool customOnly, string replaceWithText)
	{
		List<DDFindInfo> list = new List<DDFindInfo>();
		if (textToFind.Length != 0)
		{
			string value = "%" + textToFind + "%";
			DDDatabaseDefinition dDDatabaseDefinition = new DDDatabaseDefinition();
			StringBuilder stringBuilder = new StringBuilder();
			string empty = string.Empty;
			bool flag = false;
			bool flag2 = false;
			StringBuilder stringBuilder2 = new StringBuilder();
			foreach (DDTableDefinition table in dDDatabaseDefinition.Tables)
			{
				if (table.PackageDisplayFields == null || table.PackageDisplayFields.Length == 0)
				{
					continue;
				}
				stringBuilder2.Length = 0;
				foreach (DDFieldDefinition field in table.Fields)
				{
					if ((field.FieldType.IndexOf("varchar", StringComparison.CurrentCultureIgnoreCase) != -1 || field.FieldType.IndexOf("text", StringComparison.CurrentCultureIgnoreCase) != -1) && (!typesToSearch.HasValue || (field.ContentType & typesToSearch) == field.ContentType))
					{
						if (stringBuilder2.Length != 0)
						{
							stringBuilder2.Append(" Or ");
						}
						stringBuilder2.Append(field.FieldName + " Like @p1");
					}
				}
				if (stringBuilder2.Length == 0)
				{
					continue;
				}
				SqlCommand sqlCommand = context.DDServerManager.NewSqlCommand(sqlConnection, user, databaseName, string.Empty);
				sqlCommand.Parameters.Add(new SqlParameter("@p1", SqlDbType.NVarChar)).Value = value;
				sqlCommand.CommandText = "Select * From " + table.TableName + " Where " + stringBuilder2.ToString();
				SqlDataAdapter adapter;
				DataTable dataTable = context.DDServerManager.GetDataTable(sqlConnection, user, databaseName, 0, sqlCommand, fillSchema: true, out adapter);
				if (dataTable.Rows.Count == 0)
				{
					continue;
				}
				bool flag3 = false;
				empty = table.GetCustomFilterField();
				foreach (DataRow row in dataTable.Rows)
				{
					flag = empty.Length != 0 && Convert.ToBoolean(row[empty]);
					object[] array;
					if (table.DesignerKeyFields == null || table.DesignerKeyFields.Length == 0)
					{
						array = null;
					}
					else
					{
						array = new object[table.DesignerKeyFields.Length];
						for (int i = 0; i < table.DesignerKeyFields.Length; i++)
						{
							array[i] = row[table.DesignerKeyFields[i]];
						}
					}
					object[] array2 = new object[table.PackageKeyFields.Length];
					for (int j = 0; j < table.PackageKeyFields.Length; j++)
					{
						array2[j] = row[table.PackageKeyFields[j]];
					}
					object[] array3 = new object[table.PackageDisplayFields.Length];
					for (int k = 0; k < table.PackageDisplayFields.Length; k++)
					{
						stringBuilder.Length = 0;
						string[] array4 = table.PackageDisplayFields[k].Split(',');
						foreach (string columnName in array4)
						{
							if (stringBuilder.Length != 0)
							{
								stringBuilder.Append(',');
							}
							stringBuilder.Append(Convert.ToString(row[columnName]));
						}
						array3[k] = stringBuilder.ToString();
					}
					foreach (DDFieldDefinition field2 in table.Fields)
					{
						if ((field2.FieldType.IndexOf("varchar", StringComparison.CurrentCultureIgnoreCase) != -1 || field2.FieldType.IndexOf("text", StringComparison.CurrentCultureIgnoreCase) != -1) && (!typesToSearch.HasValue || (field2.ContentType & typesToSearch) == field2.ContentType))
						{
							flag2 = flag || field2.Flag == DDFieldFlag.Custom;
							if ((!customOnly || flag2) && FindWordInText(row, textToFind, replaceWithText, table, field2, array, array2, array3, list, adapter, flag2) > 0)
							{
								flag3 = true;
							}
						}
					}
				}
				if (flag3)
				{
					context.DDServerManager.UpdateData(sqlConnection, user, databaseName, dataTable, adapter);
				}
			}
		}
		return list;
	}

	protected int FindWordInText(DataRow row, string textToFind, string replaceWithText, DDTableDefinition tableDef, DDFieldDefinition fieldDef, object[] designerValues, object[] keyValues, object[] displayValues, List<DDFindInfo> foundItems, SqlDataAdapter adapter, bool isCustom)
	{
		string text = row.Field<string>(fieldDef.FieldName);
		int num = 0;
		if (text != null && text.Length != 0)
		{
			string functionName = string.Empty;
			string functionType = string.Empty;
			int num2 = -1;
			bool flag = false;
			string empty = string.Empty;
			string[] array = text.Split('\r');
			for (int i = 0; i < array.Length; i++)
			{
				string curLine = array[i];
				if (curLine.Length > 0 && curLine[0] == '\n')
				{
					empty = "\n";
					curLine = curLine.Substring(1);
				}
				else
				{
					empty = string.Empty;
				}
				string text2 = curLine.Replace('\t', ' ').Trim();
				if (!text2.StartsWith("'"))
				{
					bool flag2 = false;
					if (text2.StartsWith("Sub ", StringComparison.CurrentCultureIgnoreCase))
					{
						text2 = text2.Substring(4).TrimStart();
						functionType = "Sub";
						flag2 = true;
					}
					else if (text2.StartsWith("Function ", StringComparison.CurrentCultureIgnoreCase))
					{
						text2 = text2.Substring(9).TrimStart();
						functionType = "Function";
						flag2 = true;
					}
					if (flag2)
					{
						num2 = -1;
						int num3 = text2.IndexOf('(');
						if (num3 != -1)
						{
							text2 = text2.Substring(0, num3).TrimEnd();
						}
						functionName = text2;
					}
				}
				num2++;
				flag = false;
				for (int num3 = curLine.IndexOf(textToFind, StringComparison.CurrentCultureIgnoreCase); num3 != -1; num3 = curLine.IndexOf(textToFind, num3 + textToFind.Length, StringComparison.CurrentCultureIgnoreCase))
				{
					DDFindInfo dDFindInfo = new DDFindInfo();
					dDFindInfo.DesignerFields = tableDef.DesignerKeyFields;
					dDFindInfo.DesignerValues = designerValues;
					dDFindInfo.KeyFields = tableDef.PackageKeyFields;
					dDFindInfo.KeyValues = keyValues;
					dDFindInfo.DisplayFields = tableDef.PackageDisplayFields;
					dDFindInfo.DisplayValues = displayValues;
					dDFindInfo.Table = tableDef.TableName;
					dDFindInfo.Field = fieldDef.FieldName;
					dDFindInfo.ContentType = fieldDef.ContentType;
					dDFindInfo.FunctionName = functionName;
					dDFindInfo.FunctionType = functionType;
					dDFindInfo.CharacterPosition = num3;
					dDFindInfo.FunctionLineNumber = num2;
					dDFindInfo.FileLineNumber = i + 1;
					dDFindInfo.LineText = curLine;
					dDFindInfo.Row = row;
					dDFindInfo.Adapter = adapter;
					dDFindInfo.CustomData = isCustom;
					dDFindInfo.FieldSize = fieldDef.GetSize();
					int num4 = num3 - 1;
					string text3 = string.Empty;
					dDFindInfo.FoundText = curLine.Substring(num3, textToFind.Length);
					dDFindInfo.WholeWord = dDFindInfo.FoundText;
					while (num4 >= 0)
					{
						char value = curLine[num4];
						if ("\t\" ,.<>!?{}[]/\\|=+-&%$#@*():;'_\r\n".IndexOf(value) != -1)
						{
							break;
						}
						num4--;
						dDFindInfo.WholeWord = value + dDFindInfo.WholeWord;
						text3 = value + text3;
					}
					num4 = num3 + textToFind.Length;
					string text4 = string.Empty;
					for (; num4 < curLine.Length; num4++)
					{
						char value = curLine[num4];
						if ("\t\" ,.<>!?{}[]/\\|=+-&%$#@*():;'_\r\n".IndexOf(value) != -1)
						{
							break;
						}
						dDFindInfo.WholeWord += value;
						text4 += value;
					}
					dDFindInfo.WholeWordMatch = dDFindInfo.WholeWord.Equals(textToFind, StringComparison.CurrentCultureIgnoreCase);
					dDFindInfo.CaseMatch = textToFind.Equals(dDFindInfo.FoundText);
					if (text2.StartsWith("Case", StringComparison.CurrentCultureIgnoreCase) || text2.StartsWith("If ", StringComparison.CurrentCultureIgnoreCase))
					{
						string text5 = dDFindInfo.LineText.Substring(0, dDFindInfo.CharacterPosition);
						string text6 = dDFindInfo.LineText.Substring(dDFindInfo.CharacterPosition + dDFindInfo.FoundText.Length);
						int num5 = text5.LastIndexOf('"');
						int num6 = text6.IndexOf('"');
						if (num5 != -1 && num6 != -1 && (!text5.Substring(num5 - 1, 1).Equals("(") || !text6.Substring(num6 + 1, 1).Equals(")")))
						{
							if (dDFindInfo.FoundText.ToUpper().Equals(dDFindInfo.FoundText))
							{
								dDFindInfo.ReplaceType = "UPPER";
							}
							else if (dDFindInfo.FoundText.ToLower().Equals(dDFindInfo.FoundText))
							{
								dDFindInfo.ReplaceType = "LOWER";
							}
						}
					}
					if (replaceWithText.Length != 0 && ReplaceWordInLine(replaceWithText, ref curLine, dDFindInfo))
					{
						flag = true;
						num3 += replaceWithText.Length - dDFindInfo.FoundText.Length;
					}
					foundItems.Add(dDFindInfo);
				}
				if (flag)
				{
					array[i] = empty + curLine;
					num++;
				}
			}
			if (num > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (array.Length != 0)
				{
					for (int j = 0; j < array.Length - 1; j++)
					{
						stringBuilder.Append(array[j] + "\r");
					}
					stringBuilder.Append(array[array.Length - 1]);
				}
				if (fieldDef.Nullable && stringBuilder.Length == 0)
				{
					row[fieldDef.FieldName] = DBNull.Value;
				}
				else
				{
					row.SetField(fieldDef.FieldName, stringBuilder.ToString());
				}
			}
		}
		return num;
	}

	public bool ReplaceWordForRow(string replaceWord, DDFindInfo item, M1DataDictionary dataDictionary)
	{
		bool result = false;
		if (item.ContentType == DDFieldContentType.Code)
		{
			string[] array = item.Row.Field<string>(item.Field).Split('\r');
			string curLine = array[item.FileLineNumber - 1];
			string text = string.Empty;
			if (curLine.Length > 0 && curLine[0] == '\n')
			{
				text = "\n";
				curLine = curLine.Substring(1);
			}
			if (!ReplaceWordInLine(replaceWord, ref curLine, item))
			{
				throw new M1Exception("Word " + replaceWord + " could not be found in field " + item.Field + ": " + curLine);
			}
			result = true;
			StringBuilder stringBuilder = new StringBuilder();
			array[item.FileLineNumber - 1] = text + curLine;
			for (int i = 0; i < array.Length - 1; i++)
			{
				stringBuilder.Append(array[i] + "\r");
			}
			stringBuilder.Append(array[array.Length - 1]);
			item.Row[item.Field] = stringBuilder.ToString();
			dataDictionary.UpdateData(new DataRow[1] { item.Row }, item.Adapter);
		}
		else
		{
			string curLine2 = item.Row.Field<string>(item.Field);
			if (curLine2 != null)
			{
				if (!ReplaceWordInLine(replaceWord, ref curLine2, item))
				{
					throw new M1Exception("Word " + replaceWord + " could not be found in field " + item.Field + ": " + curLine2);
				}
				result = true;
				if (item.FieldSize > 0 && curLine2.Length > item.FieldSize)
				{
					throw new M1Exception("The size of field " + item.Field + " is defined as " + item.FieldSize + ", but the text length is " + curLine2.Length + " characters.");
				}
				item.Row[item.Field] = curLine2;
				dataDictionary.UpdateData(new DataRow[1] { item.Row }, item.Adapter);
			}
		}
		return result;
	}

	protected bool ReplaceWordInLine(string replaceWord, ref string curLine, DDFindInfo item)
	{
		int num = item.CharacterPosition;
		string text = (item.ReplaceType.Equals("UPPER", StringComparison.CurrentCultureIgnoreCase) ? replaceWord.ToUpper() : ((!item.ReplaceType.Equals("LOWER", StringComparison.CurrentCultureIgnoreCase)) ? replaceWord : replaceWord.ToLower()));
		string text2 = item.LineText;
		if (curLine.Length == text2.Length)
		{
			if (!curLine.Substring(num, item.FoundText.Length).Equals(item.FoundText, StringComparison.CurrentCultureIgnoreCase))
			{
				throw new M1Exception("The word " + item.FoundText + " could not be replaced in " + curLine + ".");
			}
			curLine = curLine.Substring(0, num) + text + curLine.Substring(num + item.FoundText.Length);
		}
		else
		{
			int num2 = text.Length - item.FoundText.Length;
			int num3;
			for (num3 = text2.IndexOf(item.FoundText, StringComparison.CurrentCultureIgnoreCase); num3 > 0; num3 = text2.IndexOf(item.FoundText, num3, StringComparison.CurrentCultureIgnoreCase))
			{
				string text3 = text2.Substring(0, num3) + text + text2.Substring(num3 + item.FoundText.Length);
				if (text3.Substring(0, num3 + text.Length).Equals(curLine.Substring(0, num3 + text.Length), StringComparison.CurrentCultureIgnoreCase))
				{
					num += num2;
					text2 = text3;
					num3 += text.Length;
				}
				else
				{
					num3 += item.FoundText.Length;
				}
			}
			if (curLine.Length == text2.Length)
			{
				if (!curLine.Substring(num, item.FoundText.Length).Equals(item.FoundText, StringComparison.CurrentCultureIgnoreCase))
				{
					throw new M1Exception("The word " + item.FoundText + " could not be replaced in " + curLine + ".");
				}
				curLine = curLine.Substring(0, num) + text + curLine.Substring(num + item.FoundText.Length);
			}
		}
		return true;
	}
}
