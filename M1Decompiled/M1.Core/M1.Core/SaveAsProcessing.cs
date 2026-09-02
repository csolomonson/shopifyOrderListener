using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class SaveAsProcessing
{
	public void PerformSaveAs(M1Database database, string table, object[] oldKeyValues, object[] newKeyValues, M1BindingSource sourceBs)
	{
		string[] array = null;
		int num = 0;
		string tableDescription = string.Empty;
		string text = string.Empty;
		string text2 = string.Empty;
		string parentKeyFields = string.Empty;
		M1DataDictionary m1DataDictionary = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		using (SqlCommand sqlCommand = new SqlCommand($"SELECT dtKeyFields, {m1DataDictionary.Language.GetdtCaptionField(database)} FROM DDTables {m1DataDictionary.Language.GetdtCaptionJoin(database)} WHERE dtTable = @tableName"))
		{
			sqlCommand.Parameters.Add(new SqlParameter("@tableName", SqlDbType.NVarChar)).Value = table;
			DataTable dataTable = m1DataDictionary.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count > 0)
			{
				array = dataTable.Rows[0]["dtKeyFields"].ToString().Split(',');
				num = array.Length;
				tableDescription = dataTable.Rows[0].Field<string>("dtCaption");
			}
		}
		if (num > 1)
		{
			text = array[num - 2].Trim();
		}
		if (text.Length > 0)
		{
			using SqlCommand sqlCommand2 = new SqlCommand("SELECT dfRelatedTable, dtKeyFields FROM DDFields INNER JOIN DDTables ON dfRelatedTable = dtTable WHERE dfField = @parentFieldName");
			sqlCommand2.Parameters.Add(new SqlParameter("@parentFieldName", SqlDbType.NVarChar)).Value = text;
			DataTable dataTable2 = m1DataDictionary.GetDataTable(sqlCommand2);
			if (dataTable2.Rows.Count > 0)
			{
				text2 = dataTable2.Rows[0]["dfRelatedTable"].ToString().Trim();
				parentKeyFields = dataTable2.Rows[0]["dtKeyFields"].ToString().Trim();
			}
		}
		if (M1Util.IsNullOrEmpty(newKeyValues[newKeyValues.GetUpperBound(0)]))
		{
			throw new M1Exception("The destination key field may not be empty.");
		}
		SaveAsProcessingParms saveAsProcessingParms = new SaveAsProcessingParms(m1DataDictionary, database, table, oldKeyValues, newKeyValues, array, text2, parentKeyFields, tableDescription);
		checkSaveAsID(saveAsProcessingParms);
		if (!string.IsNullOrWhiteSpace(saveAsProcessingParms.ParentTable))
		{
			foreach (ISaveAsProcessing item in m1DataDictionary.AppExtensions.GetProcessHooksForTable<ISaveAsProcessing>(text2, typeof(SaveAsProcessingAttribute)))
			{
				item.BeforeUpdate(saveAsProcessingParms);
			}
			if (!saveAsProcessingParms.ParentIdExists)
			{
				throw new M1Exception("The new parent ID must exist when moving an item to a new parent record.");
			}
		}
		List<ISaveAsProcessing> processHooksForTable = m1DataDictionary.AppExtensions.GetProcessHooksForTable<ISaveAsProcessing>(table, typeof(SaveAsProcessingAttribute));
		foreach (ISaveAsProcessing item2 in processHooksForTable)
		{
			item2.BeforeUpdate(saveAsProcessingParms);
		}
		doSaveAsBs(database, table, oldKeyValues, newKeyValues, num, array, sourceBs);
		foreach (ISaveAsProcessing item3 in processHooksForTable)
		{
			item3.AfterUpdate(saveAsProcessingParms);
		}
	}

	private void checkSaveAsID(SaveAsProcessingParms parm)
	{
		object[] array = null;
		string text = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		StringBuilder stringBuilder2 = new StringBuilder();
		if (parm.ParentKeyFields.Length > 0)
		{
			object[] array2 = parm.ParentKeyFields.Split(',');
			array = array2;
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.AppendFormat(" AND {0} = {1}", array[i].ToString(), parm.NewKeyValues[i].ToSql());
			}
			text = stringBuilder.ToString().Substring(5);
		}
		for (int j = 0; j < parm.KeyFieldNames.Length; j++)
		{
			if (parm.OldKeyValues[j] != parm.NewKeyValues[j])
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			throw new M1Exception("The old id and the new id cannot be the same value.");
		}
		for (int k = 0; k < parm.KeyFieldNames.Length; k++)
		{
			stringBuilder2.AppendFormat(" AND {0} = {1}", parm.KeyFieldNames[k], parm.NewKeyValues[k].ToSql());
		}
		if (parm.Database.ExecuteScalar($"SELECT 1 As Dummy FROM {parm.Table} WHERE {stringBuilder2.ToString().Substring(5)}") != null)
		{
			throw new M1Exception("The destination key values already exist in the " + parm.TableDescription + " table. Please enter another destination before continuing.");
		}
		if (text.Length > 0)
		{
			parm.ParentIdExists = parm.Database.ExecuteScalar($"SELECT 1 As Dummy FROM {parm.ParentTable} WHERE {text}") != null;
		}
	}

	private void doSaveAsBs(M1Database database, string table, object[] sourceKeyValues, object[] destinationKeyValues, int keyFieldCount, string[] keyFieldNames, M1BindingSource sourceBs)
	{
		string[] array = null;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		if (table.Length <= 0)
		{
			return;
		}
		if (sourceBs == null || table.Equals("Organizations", StringComparison.CurrentCultureIgnoreCase) || table.Equals("OrganizationLocations", StringComparison.CurrentCultureIgnoreCase) || table.Equals("OrganizationContacts", StringComparison.CurrentCultureIgnoreCase) || table.Equals("Jobs", StringComparison.CurrentCultureIgnoreCase) || table.Equals("JobMaterials", StringComparison.CurrentCultureIgnoreCase) || table.Equals("JobOperations", StringComparison.CurrentCultureIgnoreCase) || table.Equals("Quotes", StringComparison.CurrentCultureIgnoreCase) || table.Equals("QuoteLines", StringComparison.CurrentCultureIgnoreCase) || table.Equals("Parts", StringComparison.CurrentCultureIgnoreCase) || table.Equals("PartRevisions", StringComparison.CurrentCultureIgnoreCase) || table.Equals("APInvoices", StringComparison.CurrentCultureIgnoreCase) || table.Equals("APInvoiceLines", StringComparison.CurrentCultureIgnoreCase) || table.Equals("APPayments", StringComparison.CurrentCultureIgnoreCase) || table.Equals("APPaymentHeaders", StringComparison.CurrentCultureIgnoreCase) || table.Equals("ARPayments", StringComparison.CurrentCultureIgnoreCase) || table.Equals("ARPaymentHeaders", StringComparison.CurrentCultureIgnoreCase) || table.Equals("Assets", StringComparison.CurrentCultureIgnoreCase) || table.Equals("AssetTypes", StringComparison.CurrentCultureIgnoreCase) || table.Equals("AssetAdjustments", StringComparison.CurrentCultureIgnoreCase) || table.Equals("AssetLowValuePool", StringComparison.CurrentCultureIgnoreCase) || table.Equals("BankStatements", StringComparison.CurrentCultureIgnoreCase) || table.Equals("GLAccounts", StringComparison.CurrentCultureIgnoreCase) || table.Equals("GLCategories", StringComparison.CurrentCultureIgnoreCase) || table.Equals("LandedCosts", StringComparison.CurrentCultureIgnoreCase) || table.Equals("LandedCostCharges", StringComparison.CurrentCultureIgnoreCase) || table.Equals("PartForecasts", StringComparison.CurrentCultureIgnoreCase) || table.Equals("PayrollHeaders", StringComparison.CurrentCultureIgnoreCase) || table.Equals("ProductionCalendars", StringComparison.CurrentCultureIgnoreCase) || table.Equals("ProductionCalendarWorkCenters", StringComparison.CurrentCultureIgnoreCase) || table.Equals("ProductCategories", StringComparison.CurrentCultureIgnoreCase) || table.Equals("Calls", StringComparison.CurrentCultureIgnoreCase) || table.Equals("PartClasses", StringComparison.CurrentCultureIgnoreCase))
		{
			doSaveAs(database, table, sourceKeyValues, destinationKeyValues, keyFieldCount, keyFieldNames);
			return;
		}
		M1DataDictionary obj = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		DataTable dataTable = obj.GetDataTable($"With RelationsCTE As ( SELECT drPTable, drCTable, drCField, drFilter FROM DDRelations WHERE drPTable = {table.ToSql()} AND drSaveAs <> 0  AND drPersist <> 0 Union All SELECT childRelations.drPTable, childRelations.drCTable, childRelations.drCField, childRelations.drFilter FROM RelationsCTE parentRelations Inner Join DDRelations childRelations on parentRelations.drCTable = childRelations.drPTable WHERE childRelations.drSaveAs <> 0 AND childRelations.drPersist <> 0 ) Select Distinct * from RelationsCTE;");
		DataRow destParentRow = null;
		DataTable dataTable2 = obj.GetDataTable($"With RelationsCTE As  ( SELECT drPTable, drCTable, drCField, 0 as Seq FROM DDRelations WHERE drCTable = {table.ToSql()} AND drSaveAs <> 0 AND drPersist <> 0 Union All SELECT parentRelations.drPTable, parentRelations.drCTable, parentRelations.drCField, childRelations.Seq + 1 As Seq FROM RelationsCTE childRelations Inner Join DDRelations parentRelations on childRelations.drPTable = parentRelations.drCTable WHERE parentRelations.drSaveAs <> 0 AND parentRelations.drPersist <> 0 ) Select * from RelationsCTE Order By Seq Desc; ");
		if (dataTable2.Rows.Count > 0)
		{
			M1BindingSource m1BindingSource = new M1BindingSource(sourceBs.Database);
			string dataSourceTable = dataTable2.Rows[0].Field<string>("drpTable");
			m1BindingSource.Query.AllowEditingOverride = true;
			m1BindingSource.DataSourceTable = dataSourceTable;
			StringBuilder stringBuilder3 = new StringBuilder();
			for (int i = 0; i < m1BindingSource.PrimaryTable.KeyFieldsArray.Length; i++)
			{
				stringBuilder3.AppendFormat(" AND {0} = {1}", m1BindingSource.PrimaryTable.KeyFieldsArray[i], destinationKeyValues[i].ToSql());
			}
			m1BindingSource.NavigateTo(m1BindingSource.Database, stringBuilder3.ToString().Substring(5));
			M1BindingSource m1BindingSource2 = m1BindingSource;
			M1BindingSource m1BindingSource3 = null;
			foreach (DataRow row3 in dataTable2.Rows)
			{
				m1BindingSource3 = m1BindingSource2;
				m1BindingSource2 = m1BindingSource2.PrimaryTable.GetChildBindingSource(row3.Field<string>("drcTable"));
			}
			StringBuilder stringBuilder4 = new StringBuilder();
			for (int j = 0; j < m1BindingSource3.PrimaryTable.KeyFieldsArray.Length; j++)
			{
				if (stringBuilder4.Length != 0)
				{
					stringBuilder4.Append(" And ");
				}
				stringBuilder4.Append(m1BindingSource3.PrimaryTable.KeyFieldsArray[j] + " = " + M1Util.ConvertToLinq(destinationKeyValues[j]));
			}
			destParentRow = m1BindingSource3.GetDataTable().Select(stringBuilder4.ToString())[0];
			DataRow currentAsDataRow = sourceBs.CurrentAsDataRow;
			doCopy(sourceBs, currentAsDataRow, m1BindingSource2, destParentRow, dataTable, table, destinationKeyValues);
			m1BindingSource.SaveData();
		}
		else
		{
			M1BindingSource m1BindingSource2 = new M1BindingSource(sourceBs.Database);
			m1BindingSource2.DataSourceTable = table;
			DataRow currentAsDataRow2 = sourceBs.CurrentAsDataRow;
			doCopy(sourceBs, currentAsDataRow2, m1BindingSource2, destParentRow, dataTable, table, destinationKeyValues);
			m1BindingSource2.SaveData();
		}
		for (int k = 0; k < keyFieldCount; k++)
		{
			stringBuilder2.AppendFormat(" AND {0} = {1}", keyFieldNames[k], destinationKeyValues[k].ToSql());
			stringBuilder.AppendFormat(" AND {0} = {1}", keyFieldNames[k], sourceKeyValues[k].ToSql());
		}
		runSaveAsTrigger(database, table, stringBuilder2.ToString().Substring(5), stringBuilder.ToString().Substring(5));
		foreach (DataRow row4 in dataTable.Rows)
		{
			array = row4.Field<string>("drCField").Split(',');
			stringBuilder2.Length = 0;
			stringBuilder.Length = 0;
			for (int l = 0; l < keyFieldCount; l++)
			{
				stringBuilder2.AppendFormat(" AND {0} = {1}", array[l], destinationKeyValues[l].ToSql());
				stringBuilder.AppendFormat(" AND {0} = {1}", array[l], sourceKeyValues[l].ToSql());
			}
			runSaveAsTrigger(database, row4.Field<string>("drCTable").Trim(), stringBuilder2.ToString().Substring(5), stringBuilder.ToString().Substring(5));
		}
	}

	private void doCopy(M1BindingSource sourceBs, DataRow sourceRow, M1BindingSource destBs, DataRow destParentRow, DataTable ddRelations, string table, object[] destKeys)
	{
		DataRow dataRow = destBs.AddNew(destBs.Database, destParentRow, null, null) as DataRow;
		if (destKeys != null)
		{
			for (int i = 0; i < destKeys.Length; i++)
			{
				dataRow[destBs.PrimaryTable.KeyFieldsArray[i]] = destKeys[i];
			}
		}
		else
		{
			destBs.SetKeyToNextAvailable(dataRow);
		}
		CopyColumns(destBs, sourceRow, dataRow);
		DataRow[] array = ddRelations.Select("drPTable = " + table.ToLinq());
		foreach (DataRow row in array)
		{
			M1BindingSource childBindingSource = sourceBs.PrimaryTable.GetChildBindingSource(row.Field<string>("drCTable"));
			M1BindingSource childBindingSource2 = destBs.PrimaryTable.GetChildBindingSource(row.Field<string>("drCTable"));
			if (row.Field<string>("drCTable").Equals("ExpenseAccountSplits", StringComparison.CurrentCultureIgnoreCase))
			{
				string childLinkField = row.Field<string>("drCField");
				childBindingSource2.ChildLinkField = childLinkField;
			}
			DataView dataView = childBindingSource.GetDataView(sourceRow);
			if (!string.IsNullOrWhiteSpace(row.Field<string>("drFilter")))
			{
				string.IsNullOrWhiteSpace(dataView.RowFilter);
			}
			childBindingSource2.RemoveWhere(string.Empty, dataRow);
			foreach (DataRowView item in dataView)
			{
				doCopy(childBindingSource, item.Row, childBindingSource2, dataRow, ddRelations, row.Field<string>("drCTable"), null);
			}
		}
	}

	private void CopyColumns(M1BindingSource destBs, DataRow sourceRow, DataRow newRow)
	{
		foreach (DataColumn column in newRow.Table.Columns)
		{
			FieldDefinition fieldDefinition = destBs.Fields[column.ColumnName];
			if (!fieldDefinition.IsPartOfKey && fieldDefinition.BoundParentFieldType != FieldDefinition.BoundParentFieldTypeEnum.FromParent && !fieldDefinition.IsUpdatedFromChildBoundField)
			{
				switch (fieldDefinition.FieldName.Substring(3).ToUpper())
				{
				case "UNIQUEID":
				case "PAIDDATE":
				case "REVERSED":
				case "POSTEDTOGL":
				case "POSTEDDATE":
				case "DATEPOSTED":
				case "CLOSEDDATE":
				case "MODIFIEDBY":
				case "ROWVERSION":
				case "POSTED":
				case "CLOSED":
				case "PAIDCOMPLETE":
				case "MODIFIEDDATE":
				case "CREATEDBY":
				case "CREATEDDATE":
				case "REVERSALENTRY":
				case "SOURCETABLENAME":
				case "SOURCETABLEUNIQUEID":
					continue;
				}
				newRow[column.ColumnName] = fieldDefinition.GetFieldValueForSaveAs(destBs.Database, sourceRow);
			}
		}
	}

	private void doSaveAs(M1Database database, string table, object[] sourceKeyValues, object[] destinationKeyValues, int keyFieldCount, string[] keyFieldNames)
	{
		StringBuilder stringBuilder = new StringBuilder();
		List<InsertKeys> list = new List<InsertKeys>();
		string empty = string.Empty;
		int num = 0;
		string[] array = null;
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		if (table.Length <= 0)
		{
			return;
		}
		for (int i = 0; i < keyFieldCount; i++)
		{
			list.Add(new InsertKeys(keyFieldNames[i], sourceKeyValues[i], destinationKeyValues[i]));
		}
		empty = GetInsertStatement(database, table, list);
		if (empty.Length <= 0)
		{
			throw new M1Exception("Unable to create insert statement for table " + table + ".");
		}
		num++;
		stringBuilder.Append(empty);
		stringBuilder.Append("\r\n");
		DataTable dataTable = (database.GetService(typeof(M1DataDictionary)) as M1DataDictionary).GetDataTable($"SELECT drCTable, drCField FROM DDRelations WHERE drPTable = {table.ToSql()} AND drSaveAs <> 0");
		foreach (DataRow row3 in dataTable.Rows)
		{
			array = row3.Field<string>("drCField").Split(',');
			for (int j = 0; j < keyFieldCount; j++)
			{
				list[j].Field = array[j];
			}
			empty = GetInsertStatement(database, row3.Field<string>("drCTable").Trim(), list);
			if (empty.Length <= 0)
			{
				throw new M1Exception("Unable to create insert statement for table " + table + ".");
			}
			num++;
			stringBuilder.Append(empty);
			stringBuilder.Append("\r\n");
		}
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			if (stringBuilder.Length != 0)
			{
				database.ExecuteCommand(stringBuilder.ToString(), sqlTransaction);
			}
			database.CommitTransaction(sqlTransaction);
		}
		catch (Exception ex)
		{
			database.RollbackTransaction(sqlTransaction);
			throw new M1Exception(ex.Message);
		}
		for (int k = 0; k < keyFieldCount; k++)
		{
			stringBuilder3.AppendFormat(" AND {0} = {1}", keyFieldNames[k], destinationKeyValues[k].ToSql());
			stringBuilder2.AppendFormat(" AND {0} = {1}", keyFieldNames[k], sourceKeyValues[k].ToSql());
		}
		runSaveAsTrigger(database, table, stringBuilder3.ToString().Substring(5), stringBuilder2.ToString().Substring(5));
		foreach (DataRow row4 in dataTable.Rows)
		{
			array = row4.Field<string>("drCField").Split(',');
			stringBuilder3.Length = 0;
			stringBuilder2.Length = 0;
			for (int l = 0; l < keyFieldCount; l++)
			{
				stringBuilder3.AppendFormat(" AND {0} = {1}", array[l], destinationKeyValues[l].ToSql());
				stringBuilder2.AppendFormat(" AND {0} = {1}", array[l], sourceKeyValues[l].ToSql());
			}
			runSaveAsTrigger(database, row4.Field<string>("drCTable").Trim(), stringBuilder3.ToString().Substring(5), stringBuilder2.ToString().Substring(5));
		}
	}

	private bool runSaveAsTrigger(M1Database database, string table, string newWhereClause, string sourceWhereClause)
	{
		object[] array = new object[2];
		if (newWhereClause.Length > 0)
		{
			using (SqlCommand sqlCommand = new SqlCommand("SELECT dkCode FROM DDTables Inner Join DDCode On dkSourceUniqueID = dtUniqueID And dkSourceTable = 'DDTables' WHERE dtTable = @table"))
			{
				sqlCommand.Parameters.Add(new SqlParameter("@table", SqlDbType.NVarChar)).Value = table;
				DataTable dataTable = (database.GetService(typeof(M1DataDictionary)) as M1DataDictionary).GetDataTable(sqlCommand);
				if (dataTable.Rows.Count > 0)
				{
					array[0] = newWhereClause;
					array[1] = sourceWhereClause;
					foreach (DataRow row in dataTable.Rows)
					{
						string text = row.Field<string>("dkCode");
						if (text != null && text.Length != 0)
						{
							database.Scripting.ExecuteEmbeddedFunction(text, table + "_SaveAs", new object[1] { array });
						}
					}
				}
			}
			return true;
		}
		return false;
	}

	public static string GetInsertStatement(M1Database database, string insertTable, List<InsertKeys> keys)
	{
		string text = string.Empty;
		string empty = string.Empty;
		bool flag = false;
		string text2 = string.Empty;
		string text3 = string.Empty;
		_ = string.Empty;
		foreach (InsertKeys key in keys)
		{
			text = text + key.Field + " = " + key.SourceValue.ToSql() + " AND ";
		}
		if (text.Substring(text.Length - 5) == " AND ")
		{
			text = text.Substring(0, text.Length - 5);
		}
		M1User m1User = database.GetService(typeof(M1User)) as M1User;
		foreach (DataColumn column in database.GetDataTable("SELECT * FROM " + insertTable + " WHERE 0=1").Columns)
		{
			empty = column.ColumnName.Trim();
			flag = false;
			switch (empty.Substring(3).ToUpper())
			{
			case "MODIFIEDBY":
			case "CREATEDBY":
				text3 = text3 + m1User.ID.ToSql() + " AS " + empty + ",";
				text2 = text2 + empty + ",";
				continue;
			case "MODIFIEDDATE":
			case "CREATEDDATE":
				text3 = text3 + DateTime.Now.ToSql() + " AS " + empty + ",";
				text2 = text2 + empty + ",";
				continue;
			case "UNIQUEID":
			case "PAIDDATE":
			case "ROWVERSION":
			case "POSTEDTOGL":
			case "DATEPOSTED":
			case "CLOSEDDATE":
			case "PAIDCOMPLETE":
			case "CLOSED":
				continue;
			}
			if (!(column.DataType != typeof(Guid)))
			{
				continue;
			}
			foreach (InsertKeys key2 in keys)
			{
				if (empty.Equals(key2.Field, StringComparison.CurrentCultureIgnoreCase))
				{
					text3 = text3 + key2.DestinationValue.ToSql() + " AS " + empty + ",";
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				text3 = text3 + empty + ",";
			}
			text2 = text2 + empty + ",";
		}
		if (text2.Length > 0)
		{
			text3 = text3.Substring(0, text3.Length - 1);
			text2 = text2.Substring(0, text2.Length - 1);
		}
		return "INSERT INTO " + insertTable + " (" + text2 + ") SELECT " + text3 + " FROM " + insertTable + " WHERE " + text;
	}
}
