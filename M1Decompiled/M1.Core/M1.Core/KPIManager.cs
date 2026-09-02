using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class KPIManager
{
	public void GetGroupsTable(M1Database database, M1DataDictionary dataDictionary, DataTable groupsTable)
	{
		groupsTable.Columns.AddRange(new DataColumn[4]
		{
			new DataColumn("GroupID", typeof(string)),
			new DataColumn("Description", typeof(string)),
			new DataColumn("Enabled", typeof(bool)),
			new DataColumn("Expanded", typeof(bool))
		});
		addGroupRow(groupsTable, "ACTION", dataDictionary.Language.GetLanguageText(database, "MISCSTARTPAGEACTIONITEMS", "Action Items"), expanded: true);
		addGroupRow(groupsTable, "REVIEW", dataDictionary.Language.GetLanguageText(database, "MISCSTARTPAGEREVIEWITEMS", "Review Items"), expanded: true);
	}

	public void GetOptionsTable(DataTable optionsTable, M1User user, M1Database database, M1DataDictionary dataDictionary)
	{
		optionsTable.Columns.AddRange(new DataColumn[20]
		{
			new DataColumn("GroupID", typeof(string)),
			new DataColumn("GroupText", typeof(string)),
			new DataColumn("Sequence", typeof(int)),
			new DataColumn("Description", typeof(string)),
			new DataColumn("Enabled", typeof(bool)),
			new DataColumn("Table", typeof(string)),
			new DataColumn("Module", typeof(string)),
			new DataColumn("Query", typeof(string)),
			new DataColumn("FromAndWhere", typeof(string)),
			new DataColumn("GridID", typeof(string)),
			new DataColumn("DataColumn1", typeof(string)),
			new DataColumn("DataColumn2", typeof(string)),
			new DataColumn("DataColumn3", typeof(string)),
			new DataColumn("DataColumn4", typeof(string)),
			new DataColumn("DataColumn5", typeof(string)),
			new DataColumn("DataColumn6", typeof(string)),
			new DataColumn("DataColumn7", typeof(string)),
			new DataColumn("DataColumn8", typeof(string)),
			new DataColumn("DataColumn9", typeof(string)),
			new DataColumn("DataColumn10", typeof(string))
		});
		DataTable dataTable = dataDictionary.GetDataTable("Select dgGridID,djTable," + dataDictionary.Language.GetdjDescField(database) + ",IsNull(dgFrom,'') As dgFrom,IsNull(dgWher,'') As dgWher,dgSPGroup,dgSPSeq," + dataDictionary.Language.GetdgSPTextField(database) + ",IsNull(dgSPCalc,'') As dgSPCalc,dtModule,IsNull(dgdatasets,'') As dgdatasets From DDGridDetails " + dataDictionary.Language.GetdgSPTextJoin(database) + " Inner Join DDGrids On dgGridID = djGridID " + dataDictionary.Language.GetdjDescJoin(database) + " Inner Join DDTables On djTable = dtTable Where dgSPGroup <> '' And dgUserID = " + user.ID.ToSql() + "Union All Select dgGridID,djTable," + dataDictionary.Language.GetdjDescField(database) + ",IsNull(dgFrom,'') As dgFrom,IsNull(dgWher,'') As dgWher,dgSPGroup,dgSPSeq," + dataDictionary.Language.GetdgSPTextField(database) + ",IsNull(dgSPCalc,'') As dgSPCalc,dtModule,IsNull(dgdatasets,'') As dgdatasets From DDGridDetails " + dataDictionary.Language.GetdgSPTextJoin(database) + " Inner Join DDGrids On dgGridID = djGridID " + dataDictionary.Language.GetdjDescJoin(database) + " Inner Join DDTables On djTable = dtTable Where dgSPGroup <> '' And dgUserID = 'DEFAULT' And dgGridID Not In (select dgGridID From DDGridDetails Where dgUserID = " + user.ID.ToSql() + ")");
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		DataRow[] array = dataTable.Select("", "dgSPSeq,djTable,djDesc");
		foreach (DataRow row in array)
		{
			string text = row.Field<string>("dgSPText").Trim();
			if (text.Length == 0)
			{
				text = row.Field<string>("djdesc").Trim();
			}
			string text2 = row.Field<string>("dgSPCalc").Trim();
			if (text2.Length == 0)
			{
				text2 = " Count(*) As RecCount ";
			}
			else
			{
				string text3 = string.Empty;
				string[] array2 = text2.Split(',');
				int num = 0;
				int num2 = 0;
				foreach (string item in array2)
				{
					string text5 = item;
					foreach (char num3 in text5)
					{
						if (num3 == '(')
						{
							num++;
						}
						if (num3 == ')')
						{
							num--;
						}
					}
					if (num != 0)
					{
						text3 = text3 + item + ",";
						continue;
					}
					num2++;
					text3 = text3 + item + " As Column" + num2 + ", ";
				}
				text3 = text3.Substring(0, text3.Length - 2);
				text2 = text3;
			}
			string selectNormal = "";
			string selectLoadOption = "";
			string extraFields = "";
			database.MakeSelectStatements(text2, row.Field<string>("dgfrom"), row.Field<string>("dgwher"), "", "", row.Field<string>("dgdatasets"), loadNow: true, fromGrid: false, ref selectNormal, ref selectLoadOption, ref extraFields);
			string text6 = " From " + row.Field<string>("dgfrom").Trim();
			if (row.Field<string>("dgwher").Trim().Length != 0)
			{
				text6 = text6 + " Where " + row.Field<string>("dgwher").Trim();
			}
			addOptionRow(optionsTable, row.Field<string>("dgSPGroup").Trim(), row.Field<short>("dgSPSeq"), "    " + text, row.Field<string>("djTable").Trim(), row.Field<string>("dtModule").Trim(), row.Field<string>("dgGridID").Trim(), selectNormal, text6, user, database, dataDictionary);
		}
	}

	private void addOptionRow(DataTable optionsTable, string groupID, int sequence, string description, string table, string module, string gridID, string query, string fromAndWhere, M1User user, M1Database database, M1DataDictionary dataDictionary)
	{
		bool flag = true;
		module = module.Trim();
		table = table.Trim();
		if (module.Length != 0)
		{
			if (module.Length > 2)
			{
				if (!database.Security.IsInRole(module))
				{
					flag = false;
				}
			}
			else if (!dataDictionary.ProductCode.IsModulePurchased(module, database))
			{
				flag = false;
			}
		}
		if (flag && table.Length != 0 && !database.Security.IsInRoleByTable(table, "VIEW"))
		{
			flag = false;
		}
		if (flag)
		{
			DataRow row = optionsTable.AddBlankRow();
			row.SetField("GroupID", groupID);
			row.SetField("GroupText", groupID.Substring(0, 1).ToUpper() + groupID.Substring(1).ToLower() + " items");
			row.SetField("Sequence", sequence);
			row.SetField("Description", description);
			row.SetField("Table", table);
			row.SetField("Module", module);
			row.SetField("Enabled", value: true);
			row.SetField("GridID", gridID);
			row.SetField("Query", query);
			row.SetField("FromAndWhere", fromAndWhere);
		}
	}

	private void addGroupRow(DataTable groupsTable, string groupID, string description, bool expanded)
	{
		DataRow row = groupsTable.AddBlankRow();
		row.SetField("GroupID", groupID);
		row.SetField("Description", description);
		row.SetField("Expanded", expanded);
	}

	public bool UpdateGridSPGroup(string gridID, M1User user, M1DataDictionary dataDictionary, string groupID, int sequence)
	{
		if (dataDictionary.ExecuteCommand("Update DDGridDetails Set dgSPGroup = " + groupID.ToSql() + ", dgSPSeq = " + sequence.ToSql() + " Where dgGridID = " + gridID.ToSql() + " And dgUserID = " + user.ID.ToSql()) == 0)
		{
			DataTable dataTable = dataDictionary.GetDataTable("Select * from DDGridDetails Where dgGridID = " + gridID.ToSql() + " And dgUserID = 'DEFAULT'");
			if (dataTable.Rows.Count == 0)
			{
				dataTable = dataDictionary.GetDataTable("Select * from DDGridDetails Where dgGridID = " + gridID.ToSql() + " And dgUserID = ''");
			}
			if (dataTable.Rows.Count > 0)
			{
				SqlDataAdapter adapter = null;
				DataTable dataTable2 = dataDictionary.GetDataTable("Select * From DDGridDetails Where dgGridID = " + gridID.ToSql() + " And dgUserID = " + user.ID.ToSql(), fillSchema: true, out adapter);
				DataRow dataRow = null;
				dataRow = ((dataTable2.Rows.Count != 0) ? dataTable2.Rows[0] : dataTable2.AddBlankRow());
				foreach (DataColumn column in dataTable.Columns)
				{
					dataRow[column.ColumnName] = dataTable.Rows[0][column.ColumnName];
				}
				dataRow.SetField("dgCustom", 1);
				dataRow.SetField("dgUserID", user.ID);
				dataRow.SetField("dgSPGroup", groupID);
				dataRow.SetField("dgSPSeq", sequence);
				dataDictionary.UpdateData(new DataRow[1] { dataRow }, adapter);
			}
		}
		return true;
	}

	public bool RecordsExist(string sToUser, M1DataDictionary datadictionary)
	{
		_ = string.Empty;
		SqlCommand sqlCommand = null;
		sqlCommand = new SqlCommand("Select COUNT(*) FROM DDGridDetails WHERE dgUserID = @p1 and dgSPGroup <> ''");
		sqlCommand.Parameters.AddWithValue("@p1", sToUser);
		if ((int)datadictionary.ExecuteScalar(sqlCommand) > 0)
		{
			return true;
		}
		return false;
	}

	public void MoveKpis(string sFromUser, string sToUser, M1DataDictionary datadictionary, bool bOverwriteExisting)
	{
		_ = string.Empty;
		SqlCommand sqlCommand = null;
		SqlTransaction sqlTransaction = null;
		DDTableDefinition table = new DDDatabaseDefinition().GetTable("DDGridDetails");
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		foreach (DDFieldDefinition field in table.Fields)
		{
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(",");
				stringBuilder2.Append(",");
			}
			stringBuilder2.Append(field.FieldName);
			if (field.FieldName.Equals("dgUserID", StringComparison.CurrentCultureIgnoreCase))
			{
				stringBuilder.Append("@p2");
			}
			else
			{
				stringBuilder.Append(field.FieldName);
			}
		}
		if (bOverwriteExisting)
		{
			sqlTransaction = datadictionary.BeginTransaction();
			sqlCommand = new SqlCommand("DELETE FROM DDGridDetails WHERE dgUserID = @p2 and (dgSPGroup <> '' or dgGridID in (select dgGridID FROM DDGridDetails WHERE dgUserID = @p1 and dgSPGroup <> '') );INSERT INTO DDGridDetails (" + stringBuilder2.ToString() + ") Select " + stringBuilder.ToString() + " FROM DDGridDetails WHERE dgUserID = @p1 and dgSPGroup <> ''");
			sqlCommand.Parameters.AddWithValue("@p1", sFromUser);
			sqlCommand.Parameters.AddWithValue("@p2", sToUser);
			datadictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			if (sqlTransaction != null)
			{
				datadictionary.CommitTransaction(sqlTransaction);
			}
		}
		else
		{
			sqlCommand = new SqlCommand("INSERT INTO DDGridDetails (" + stringBuilder2.ToString() + ") Select " + stringBuilder.ToString() + " FROM DDGridDetails WHERE dgUserID = @p1 and dgSPGroup <> '' and dgGridID Not In (Select dgGridID From DDGridDetails Where dgUserID = @p2 and dgSPGroup <> '')");
			sqlCommand.Parameters.AddWithValue("@p1", sFromUser);
			sqlCommand.Parameters.AddWithValue("@p2", sToUser);
			datadictionary.ExecuteCommand(sqlCommand, sqlTransaction);
			if (sqlTransaction != null)
			{
				datadictionary.CommitTransaction(sqlTransaction);
			}
		}
	}
}
