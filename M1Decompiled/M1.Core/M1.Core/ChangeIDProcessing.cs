using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using M1.Core.Database;
using M1.Extensions;

namespace M1.Core;

public class ChangeIDProcessing
{
	public string ChangeID(M1Database database, string table, object[] oldKeyValues, object[] newKeyValues, short changeIDType, bool userChoice = false)
	{
		string[] array = null;
		int num = 0;
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		string text5 = string.Empty;
		string[] array2 = null;
		bool flag = false;
		string empty = string.Empty;
		string text6 = string.Empty;
		string[] array3 = null;
		string text7 = string.Empty;
		string text8 = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		M1DataDictionary m1DataDictionary = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		ChangeIDProcessingParms changeIDProcessingParms = new ChangeIDProcessingParms(table, oldKeyValues, newKeyValues, changeIDType, m1DataDictionary, database);
		if (userChoice)
		{
			changeIDProcessingParms.UsersChoiceOfCascadingChangeOnDefaultBin = true;
		}
		bool flag2 = false;
		List<IChangeIDProcessing> processHooksForTable = m1DataDictionary.AppExtensions.GetProcessHooksForTable<IChangeIDProcessing>(table, typeof(ChangeIDProcessingAttribute));
		using (SqlCommand sqlCommand = new SqlCommand("SELECT a.dtKeyFields,a.dtLastKeyCanBeEmpty,a.dtParentTable,IsNull(b.dtKeyFields,'') As ParentKeyFields FROM DDTables a Left Outer Join DDTables b On a.dtParentTable = b.dtTable WHERE a.dtTable = @table"))
		{
			sqlCommand.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar, 30)).Value = table;
			DataTable dataTable = m1DataDictionary.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count <= 0)
			{
				throw new M1Exception("Unable to find table " + table + " in DDTables.");
			}
			DataRow dataRow = dataTable.Rows[0];
			changeIDProcessingParms.LastKeyCanBeEmpty = dataRow.Field<bool>("dtLastKeyCanBeEmpty");
			array = dataRow["dtKeyFields"].ToString().Trim().Split(',');
			num = array.Length;
			text4 = dataRow.Field<string>("dtParentTable").Trim();
			text5 = dataRow.Field<string>("ParentKeyFields").Trim();
			if (string.IsNullOrEmpty(text4) || string.IsNullOrEmpty(text5))
			{
				string text9 = string.Empty;
				if (num - 1 > 0)
				{
					text9 = array[num - 2].ToUpper();
				}
				if (text9.Length > 0)
				{
					using SqlCommand sqlCommand2 = new SqlCommand("SELECT dfRelatedTable, dtKeyFields FROM DDFields INNER JOIN DDTables ON dfRelatedTable = dtTable WHERE dfField = @fieldName");
					sqlCommand2.Parameters.Add(new SqlParameter("@fieldName", SqlDbType.NVarChar)).Value = text9;
					DataTable dataTable2 = m1DataDictionary.GetDataTable(sqlCommand2);
					if (dataTable2.Rows.Count > 0)
					{
						text4 = dataTable2.Rows[0].Field<string>("dfRelatedTable");
						text5 = dataTable2.Rows[0].Field<string>("dtKeyFields");
					}
				}
			}
		}
		for (int i = 0; i < num; i++)
		{
			text = text + " AND " + array[i] + " = " + oldKeyValues[i].ToSql();
		}
		text = text.Substring(5);
		if (newKeyValues.Length != num)
		{
			throw new M1Exception("The number or elements in the passed array do not match the number of key fields for table " + table + ".");
		}
		for (int j = 0; j < num; j++)
		{
			text2 = text2 + " AND " + array[j] + " = " + newKeyValues[j].ToSql();
		}
		text2 = text2.Substring(5);
		if (text5.Length > 0)
		{
			array2 = text5.Split(',');
			for (int k = 0; k < array2.Length; k++)
			{
				text3 = text3 + " AND " + array2[k] + " = " + newKeyValues[k].ToSql();
			}
			text3 = text3.Substring(5);
		}
		for (int l = 0; l < num; l++)
		{
			object obj = null;
			object obj2 = null;
			if (oldKeyValues[l].GetType() == typeof(string) && newKeyValues[l].GetType() == typeof(string))
			{
				obj = ((string)oldKeyValues[l]).Trim();
				obj2 = ((string)newKeyValues[l]).Trim();
			}
			else
			{
				obj = oldKeyValues[l];
				obj2 = newKeyValues[l];
			}
			if (!obj.Equals(obj2))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			throw new M1Exception("The old ID and the new ID cannot be the same value.");
		}
		DataTable dataTable3 = database.GetDataTable("SELECT 1 AS Dummy FROM " + table + " WHERE " + text2);
		changeIDProcessingParms.NewIDExists = dataTable3.Rows.Count != 0;
		foreach (IChangeIDProcessing item in processHooksForTable)
		{
			item.PreProcessChangeID(changeIDProcessingParms);
		}
		if (text3.Length > 0 && changeIDProcessingParms.ParentIDMustExist && database.ExecuteScalar("SELECT 1 AS Dummy FROM " + text4 + " WHERE " + text3) == null)
		{
			throw new M1Exception("The new parent ID must exist when moving an item to a new parent record.");
		}
		if (!changeIDProcessingParms.NewIDExists && !changeIDProcessingParms.LastKeyCanBeEmpty && M1Util.IsNullOrEmpty(newKeyValues[num - 1]))
		{
			throw new M1Exception("The new ID cannot be empty if it doesn't already exist.");
		}
		if (changeIDType == 1 && changeIDProcessingParms.NewIDExists)
		{
			throw new M1Exception("You have selected the New ID option, but have specified an ID that already exists.");
		}
		if (changeIDProcessingParms.NewIDExists && text.Length > 0)
		{
			using (SqlCommand sqlCommand3 = new SqlCommand("SELECT drCTable, drCField, drFilter FROM DDRelations WHERE drPTable = @table AND drPersist <> 0 AND drFilter <> ''"))
			{
				sqlCommand3.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar)).Value = table;
				DataTable dataTable4 = m1DataDictionary.GetDataTable(sqlCommand3);
				if (dataTable4.Rows.Count > 0)
				{
					text6 = dataTable4.Rows[0].Field<string>("drCTable");
					array3 = dataTable4.Rows[0].Field<string>("drCField").Split(',');
					using SqlCommand sqlCommand4 = new SqlCommand("SELECT dtKeyFields FROM DDTables WHERE dtTable = @table");
					sqlCommand4.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar)).Value = text6;
					DataTable dataTable5 = m1DataDictionary.GetDataTable(sqlCommand4);
					if (dataTable5 != null && dataTable5.Rows.Count != 0)
					{
						string[] array4 = dataTable5.Rows[0].Field<string>("dtKeyFields").Split(',');
						if (array3.Length == array4.Length)
						{
							flag2 = true;
							for (int m = 0; m < num; m++)
							{
								text7 = text7 + array3[m] + " = " + oldKeyValues[m].ToSql() + " AND ";
							}
							text7 = text7 + " NOT (" + dataTable4.Rows[0].Field<string>("drFilter") + ")";
							for (int n = 0; n < num; n++)
							{
								text8 = text8 + array3[n] + " = " + newKeyValues[n].ToSql() + " AND ";
							}
							text8 = text8 + " NOT (" + dataTable4.Rows[0].Field<string>("drFilter") + ")";
							changeIDProcessingParms.DeleteStatements.AppendLine("DELETE FROM " + text6 + " WHERE " + text7);
						}
					}
				}
			}
			changeIDProcessingParms.DeleteStatements.AppendLine("DELETE FROM " + table + " WHERE " + text);
		}
		changeIDProcessingParms.SqlTransaction = database.BeginTransaction();
		try
		{
			if (changeIDProcessingParms.NewIDExists)
			{
				if (changeIDProcessingParms.UpdateStatements.Length != 0)
				{
					database.ExecuteCommand(changeIDProcessingParms.UpdateStatements.ToString(), changeIDProcessingParms.SqlTransaction);
				}
				ChangeIDMergeRecords(database, table, text, text2, array, changeIDType, changeIDProcessingParms.SqlTransaction);
				if (text6.Length > 0 && text8.Length > 0 && flag2)
				{
					ChangeIDMergeRecords(database, text6, text7, text8, array3, changeIDType, changeIDProcessingParms.SqlTransaction);
				}
				if (changeIDProcessingParms.DeleteStatements.Length != 0)
				{
					database.ExecuteCommand(changeIDProcessingParms.DeleteStatements.ToString(), changeIDProcessingParms.SqlTransaction);
				}
			}
			empty = ProcessChangeID(database, table, oldKeyValues, newKeyValues, changeIDType, changeIDProcessingParms.SqlTransaction, processHooksForTable);
			if (empty.Length == 0)
			{
				throw new M1Exception("Error processing Change ID.");
			}
			stringBuilder.AppendLine(empty);
			database.CommitTransaction(changeIDProcessingParms.SqlTransaction);
			changeIDProcessingParms.SqlTransaction = null;
			foreach (IChangeIDProcessing item2 in processHooksForTable)
			{
				item2.PostProcessChangeID(changeIDProcessingParms);
				if (changeIDProcessingParms.ProcessChangeIdMessage.Length != 0)
				{
					if (empty.Split(':')[1].TrimStart().Length != 0)
					{
						stringBuilder = stringBuilder.Remove(stringBuilder.ToString().LastIndexOf('\n'), 1);
					}
					stringBuilder.AppendLine(changeIDProcessingParms.ProcessChangeIdMessage.ToString());
				}
			}
			return stringBuilder.ToString();
		}
		catch (Exception ex)
		{
			if (changeIDProcessingParms.SqlTransaction != null)
			{
				database.RollbackTransaction(changeIDProcessingParms.SqlTransaction);
			}
			throw new M1Exception(ex.Message);
		}
	}

	public void ChangeIDMergeRecords(M1Database database, string table, string oldWhereClause, string newWhereClause, string[] keyFieldNames, int changeIDType, SqlTransaction transaction)
	{
		bool flag = false;
		SqlDataAdapter adapter;
		DataTable dataTable = database.GetDataTable("SELECT * FROM " + table + " WHERE " + oldWhereClause, fillSchema: false, out adapter, transaction);
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		DataTable dataTable2 = database.GetDataTable("SELECT * FROM " + table + " WHERE " + newWhereClause, fillSchema: false, out adapter, transaction);
		if (dataTable2.Rows.Count <= 0)
		{
			return;
		}
		foreach (DataColumn column in dataTable2.Columns)
		{
			flag = false;
			if (keyFieldNames.Length != 0)
			{
				for (int i = 0; i < keyFieldNames.Length; i++)
				{
					if (column.ColumnName.Equals(keyFieldNames[i], StringComparison.CurrentCultureIgnoreCase))
					{
						flag = true;
						break;
					}
				}
			}
			if (M1Util.IsNullOrEmpty(dataTable2.Rows[0][column.ColumnName]))
			{
				if (!flag)
				{
					dataTable2.Rows[0][column.ColumnName] = dataTable.Rows[0][column.ColumnName];
				}
			}
			else if (!M1Util.IsNullOrEmpty(dataTable2.Rows[0][column.ColumnName]) && !M1Util.IsNullOrEmpty(dataTable.Rows[0][column.ColumnName]) && dataTable2.Rows[0][column.ColumnName] != dataTable.Rows[0][column.ColumnName] && !flag && changeIDType == 2 && !SystemGeneratedFields.IsGenerated(column.ColumnName))
			{
				dataTable2.Rows[0][column.ColumnName] = dataTable.Rows[0][column.ColumnName];
			}
		}
		database.UpdateData(dataTable2, adapter, transaction);
	}

	public string ProcessChangeID(M1Database database, string table, object[] oldKeyValues, object[] newKeyValues, short changeIDType, SqlTransaction transaction, List<IChangeIDProcessing> processHooks)
	{
		string[] array = null;
		string empty = string.Empty;
		string empty2 = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		string empty3 = string.Empty;
		string text = string.Empty;
		M1DataDictionary m1DataDictionary = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		string text2 = string.Empty;
		StringBuilder stringBuilder3 = new StringBuilder();
		bool flag = false;
		stringBuilder3.AppendLine("Following tables were not updated because DDRelations were invalid:");
		ChangeIDProcessingParms changeIDProcessingParms = new ChangeIDProcessingParms(table, oldKeyValues, newKeyValues, changeIDType, m1DataDictionary, database);
		changeIDProcessingParms.SqlTransaction = transaction;
		if (processHooks == null)
		{
			processHooks = m1DataDictionary.AppExtensions.GetProcessHooksForTable<IChangeIDProcessing>(table, typeof(ChangeIDProcessingAttribute));
		}
		using (SqlCommand sqlCommand = new SqlCommand("SELECT dtKeyFields FROM DDTables WHERE dtTable = @table"))
		{
			sqlCommand.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar)).Value = table;
			DataTable dataTable = m1DataDictionary.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count > 0)
			{
				empty = dataTable.Rows[0].Field<string>("dtKeyFields").Trim();
				string empty4 = string.Empty;
				string text3 = string.Empty;
				if (table.Equals("PartBins", StringComparison.CurrentCultureIgnoreCase))
				{
					empty4 = "'inqPartID,inqPartRevisionID,inqDestinationWarehouseID,inqDestinationPartBinID'";
					text3 = " union select 'QuantityAdjustments' as drCTable, " + empty4 + " drCField,'' drFilter,'Quantity Adjustments' dtCaption ";
				}
				if (table.Equals("WarehouseBins", StringComparison.CurrentCultureIgnoreCase))
				{
					empty4 = "'inqDestinationWarehouseID,inqDestinationPartBinID'";
					text3 = " union select 'QuantityAdjustments' as drCTable, " + empty4 + " drCField,'' drFilter,'Quantity Adjustments' dtCaption ";
				}
				using SqlCommand sqlCommand2 = new SqlCommand("SELECT drCTable, drCField, drFilter, " + m1DataDictionary.Language.GetdtCaptionField(database) + " FROM DDRelations INNER JOIN DDTables ON drCTable = dtTable " + m1DataDictionary.Language.GetdtCaptionJoin(database) + " WHERE drPTable = @table AND ((drPersist = 0 AND LEFT(drPField,@keyLength) = @keyFields) OR drPersist <> 0) " + text3 + "  ORDER BY dtCaption");
				sqlCommand2.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar)).Value = table;
				sqlCommand2.Parameters.Add(new SqlParameter("@keyLength", SqlDbType.Int)).Value = empty.Length;
				sqlCommand2.Parameters.Add(new SqlParameter("@keyFields", SqlDbType.NVarChar)).Value = empty;
				DataTable dataTable2 = m1DataDictionary.GetDataTable(sqlCommand2);
				try
				{
					foreach (IChangeIDProcessing processHook in processHooks)
					{
						processHook.ProcessChangeID(changeIDProcessingParms);
					}
					array = empty.Split(',');
					for (int i = 0; i < oldKeyValues.Length; i++)
					{
						stringBuilder.AppendFormat(" AND {0} = {1}", array[i], oldKeyValues[i].ToSql());
						stringBuilder2.AppendFormat(", {0} = {1}", array[i], newKeyValues[i].ToSql());
					}
					empty2 = stringBuilder.ToString().Substring(5);
					empty3 = stringBuilder2.ToString().Substring(2);
					if (empty2.Length > 0)
					{
						text = table;
						database.ExecuteCommand($"UPDATE {table} SET {empty3} WHERE {empty2}", transaction);
					}
					if (dataTable2.Rows.Count > 0)
					{
						foreach (DataRow row in dataTable2.Rows)
						{
							empty2 = string.Empty;
							stringBuilder.Length = 0;
							stringBuilder2.Length = 0;
							empty3 = string.Empty;
							array = row.Field<string>("drCField").Split(',');
							for (int j = 0; j < oldKeyValues.Length; j++)
							{
								if (array[j].Substring(0, 1) == "'" || M1Util.IsNumeric(array[j]))
								{
									if (!M1Util.IsNullOrEmpty(oldKeyValues[j]))
									{
										empty2 = string.Empty;
										empty3 = string.Empty;
										break;
									}
								}
								else
								{
									stringBuilder.AppendFormat(" AND {0} = {1} ", array[j], oldKeyValues[j].ToSql());
									stringBuilder2.AppendFormat(", {0} = {1}", array[j], newKeyValues[j].ToSql());
								}
							}
							empty2 = stringBuilder.ToString();
							empty3 = stringBuilder2.ToString();
							if (empty2.Length <= 0)
							{
								continue;
							}
							if (row.Field<string>("drFilter").Trim().Length > 0 && row.Field<string>("drCTable").Trim().Equals("JOBCOSTS", StringComparison.CurrentCultureIgnoreCase))
							{
								empty2 = string.Format("{0} AND {1}", empty2, row.Field<string>("drFilter").Trim());
							}
							empty2 = empty2.Substring(5);
							empty3 = empty3.Substring(2);
							try
							{
								SqlDataAdapter adapter;
								DataTable dataTable3 = database.GetDataTable(string.Format("SELECT COUNT(*) As Rec_Count FROM {0}  WHERE {1}", row.Field<string>("drCTable").Trim(), empty2), fillSchema: false, out adapter, transaction);
								if (dataTable3.Rows.Count > 0 && dataTable3.Rows[0]["Rec_Count"] != DBNull.Value && Convert.ToInt32(dataTable3.Rows[0]["Rec_Count"]) > 0)
								{
									text2 = text2 + Convert.ToInt32(dataTable3.Rows[0]["Rec_Count"]) + " times in " + row.Field<string>("dtCaption") + "\n";
									if (changeIDType != 1 && !empty2.Trim().Equals(empty3.Trim(), StringComparison.CurrentCultureIgnoreCase))
									{
										CheckDuplicatedRowsBeforeUpdate(database, transaction, row, changeIDType, empty2, empty3);
									}
								}
								text = row.Field<string>("drCTable").Trim();
								database.ExecuteCommand("UPDATE " + text + " SET " + empty3 + " WHERE " + empty2, transaction);
							}
							catch (Exception ex)
							{
								flag = true;
								stringBuilder3.AppendLine("Table:" + row.Field<string>("drCTable").Trim() + ", Error:" + ex.Message);
							}
						}
					}
					StringBuilder stringBuilder4 = new StringBuilder();
					stringBuilder4.AppendLine("ID " + oldKeyValues[oldKeyValues.Length - 1]?.ToString() + " was referenced and changed in the following locations: \r\n" + text2);
					if (flag)
					{
						stringBuilder4.AppendLine(stringBuilder3.ToString());
					}
					return stringBuilder4.ToString();
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Substring(0, 31).Equals("CANNOT INSERT DUPLICATE KEY ROW", StringComparison.CurrentCultureIgnoreCase))
					{
						throw new M1Exception("M1 is unable to change the IDs for the " + text + " table because doing so would create records with duplicate keys. Please ensure that there are no duplicate IDs in this table for the records you are trying to merge before running this option again.", ex2);
					}
					throw new M1Exception(ex2.Message, ex2);
				}
			}
		}
		return string.Empty;
	}

	private void CheckDuplicatedRowsBeforeUpdate(M1Database database, SqlTransaction transaction, DataRow row, int changeIDType, string whereClause, string setClause)
	{
		string text = row.Field<string>("drCTable").Trim();
		DataTable dataTable = database.GetDataTable($"SELECT * FROM {text} WHERE {whereClause}", transaction);
		setClause = setClause.Replace(",", " AND ");
		DataTable dataTable2 = database.GetDataTable($"SELECT * FROM {text} WHERE {setClause}", transaction);
		if (dataTable == null || dataTable2 == null || dataTable2.Rows.Count <= 0)
		{
			return;
		}
		M1DataDictionary m1DataDictionary = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		using SqlCommand sqlCommand = new SqlCommand("SELECT dtKeyFields FROM DDTables WHERE dtTable = @table");
		sqlCommand.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar)).Value = text;
		DataTable dataTable3 = m1DataDictionary.GetDataTable(sqlCommand);
		if (dataTable3 == null || dataTable3.Rows.Count == 0)
		{
			return;
		}
		string[] keys = dataTable3.Rows[0].Field<string>("dtKeyFields").Split(',');
		foreach (DataRow row2 in dataTable.Rows)
		{
			updateRow(row2, setClause);
			if (findRowInTable(row2, dataTable2, keys))
			{
				string empty = string.Empty;
				if (changeIDType == 2)
				{
					empty = getRemoveWhereClause(row2, keys);
				}
				else
				{
					updateRow(row2, whereClause);
					empty = getRemoveWhereClause(row2, keys);
				}
				database.ExecuteCommand($"DELETE FROM {text} WHERE {empty}", transaction);
			}
		}
	}

	private void updateRow(DataRow row, string clause)
	{
		Regex regex = new Regex("\\bN'|\\b'");
		string[] array = clause.Split(new string[1] { "AND" }, StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = (from p in array[i].Split('=')
				select p.Trim()).ToArray();
			row[array2[0]] = regex.Replace(array2[1].Trim(), string.Empty);
		}
	}

	private bool findRowInTable(DataRow row, DataTable table, string[] keys)
	{
		bool flag = false;
		foreach (DataRow row2 in table.Rows)
		{
			flag = true;
			foreach (string columnName in keys)
			{
				if (!row2[columnName].Equals(row[columnName]))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return flag;
			}
		}
		return flag;
	}

	private string getRemoveWhereClause(DataRow row, string[] keys)
	{
		string text = string.Empty;
		int num = 0;
		foreach (string text2 in keys)
		{
			text = ((num >= keys.Length - 1) ? (text + $"{text2} = {row[text2].ToSql()}") : (text + $"{text2} = {row[text2].ToSql()} AND "));
			num++;
		}
		return text;
	}
}
