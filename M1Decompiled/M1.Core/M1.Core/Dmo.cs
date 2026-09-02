using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class Dmo
{
	public delegate void SetCollationDelegate(string tableName);

	public class DuplicatesRemovedEventArgs : EventArgs
	{
		public string Message = string.Empty;
	}

	private class MakeFieldListInfo
	{
		public bool IsIdentity;

		public StringBuilder InsertFieldList = new StringBuilder();

		public StringBuilder SelectFieldList = new StringBuilder();
	}

	protected class IndexInfo
	{
		public string Name = string.Empty;

		public bool IsUnique;

		public bool AutoCreated;

		public List<string> Columns = new List<string>();

		public IndexInfo(string name, bool isUnique, bool autoCreated)
		{
			Name = name;
			IsUnique = isUnique;
			AutoCreated = autoCreated;
		}
	}

	public delegate void VerifyDelegate(string tableName);

	private ServerManager serverManager;

	private AppContext currentContext;

	private IniSettings iniSettings;

	public DmoDD DmoDD;

	public event EventHandler<DuplicatesRemovedEventArgs> DuplicatesRemoved;

	public Dmo(AppContext context, ServerManager newServerManager, IniSettings newIniSettings)
	{
		currentContext = context;
		serverManager = newServerManager;
		iniSettings = newIniSettings;
		DmoDD = new DmoDD(this, context);
	}

	public Dmo(AppContext context, ServerManager newServerManager)
	{
		currentContext = context;
		serverManager = newServerManager;
		DmoDD = new DmoDD(this, context);
	}

	public string CreateDatabase(M1User m1User, M1DataDictionary m1DataDictionary, AppContext context, Dictionary<string, object> dsProps, int databaseSize, DBCreateCode.CreateDelegate func)
	{
		return new DBCreateCode().CreateDatabase(m1User, m1DataDictionary, context, dsProps, databaseSize, func);
	}

	public byte GetCompatibilityLevel(M1User m1User, string databaseName)
	{
		SqlCommand sqlCommand = serverManager.NewSqlCommand(null, m1User, databaseName, "select compatibility_level from sys.databases where name = @Db");
		sqlCommand.Parameters.Add(new SqlParameter("@Db", SqlDbType.NVarChar)).Value = databaseName;
		return (byte)serverManager.ExecuteScalar(null, m1User, databaseName, sqlCommand);
	}

	public void SetCompatibilityLevel(M1User m1User, string database, byte newLevel)
	{
		serverManager.ClearAllPools();
		serverManager.ExecuteCommand(null, m1User, database, "Alter Database " + database + " Set Compatibility_Level = " + newLevel.ToSql());
	}

	public string GetCollation(M1User m1User, string databaseName)
	{
		if (databaseName.Length == 0)
		{
			return (string)serverManager.ExecuteScalar(null, m1User, databaseName, "Select SERVERPROPERTY('collation')");
		}
		return (string)serverManager.ExecuteScalar(null, m1User, databaseName, "select DATABASEPROPERTYEX('" + databaseName + "','collation')");
	}

	public void SetCollation(M1User m1User, M1DataDictionary m1DataDictionary, string dataBase, string newCollation, List<string> messages)
	{
		SetCollation(m1User, m1DataDictionary, dataBase, newCollation, messages, null);
	}

	public void SetCollation(M1User m1User, M1DataDictionary m1DataDictionary, string dataBaseName, string newCollation, List<string> messages, SetCollationDelegate func)
	{
		serverManager.ClearAllPools();
		using SqlConnection sqlConnection = serverManager.GetConnection(m1User, dataBaseName, openImmediately: true);
		func?.Invoke("Opening " + dataBaseName + " in single user mode");
		serverManager.ExecuteCommand(sqlConnection, m1User, "master", "ALTER DATABASE " + dataBaseName.ToString() + " Set SINGLE_USER WITH ROLLBACK IMMEDIATE");
		try
		{
			SetCompatibilityLevel(sqlConnection, m1User, dataBaseName);
			string database = sqlConnection.Database;
			string text = (string)serverManager.ExecuteScalar(sqlConnection, m1User, dataBaseName, "select DATABASEPROPERTYEX('" + dataBaseName + "','collation')");
			serverManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "USE master");
			serverManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "ALTER DATABASE " + dataBaseName + " COLLATE " + newCollation);
			serverManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "USE " + database);
			try
			{
				foreach (DataRow row in serverManager.GetDataTable(sqlConnection, m1User, dataBaseName, 0, "exec sp_tables @table_type = \"'TABLE'\"").Rows)
				{
					if (row.Field<string>("table_name") != "dtproperties" && !row.Field<string>("table_owner").Trim().Equals("sys", StringComparison.CurrentCultureIgnoreCase))
					{
						func(row.Field<string>("table_name").Trim());
						RebuildTable(sqlConnection, m1User, m1DataDictionary, dataBaseName, row.Field<string>("table_name").Trim());
					}
				}
			}
			catch
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "USE master");
				serverManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "ALTER DATABASE " + dataBaseName + " COLLATE " + text);
				serverManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "USE " + database);
				throw;
			}
			finally
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "USE " + database);
			}
		}
		finally
		{
			serverManager.ExecuteCommand(sqlConnection, m1User, dataBaseName, "ALTER DATABASE " + dataBaseName.ToString() + " Set MULTI_USER");
			func?.Invoke(string.Empty);
		}
	}

	private void dropTableTriggers(SqlConnection sqlConnection, M1User m1User, string databaseName, Dictionary<string, string> triggers)
	{
		if (triggers == null || triggers.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<string, string> trigger in triggers)
		{
			serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, $"Drop Trigger [{trigger.Key}]");
		}
	}

	private void addTableTriggers(SqlConnection sqlConnection, M1User m1User, string databaseName, Dictionary<string, string> triggers, bool disableTriggers, string tableName)
	{
		if (triggers == null || triggers.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<string, string> trigger in triggers)
		{
			serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, trigger.Value);
			if (disableTriggers)
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, $"DISABLE TRIGGER [{trigger.Key}] ON {tableName}");
			}
		}
	}

	private Dictionary<string, string> getTableTriggers(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
		DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "SELECT Tables.Name TableName, Triggers.name TriggerName, Triggers.crdate TriggerCreatedDate, Comments.Text TriggerText FROM sysobjects Triggers Inner Join sysobjects Tables On Triggers.parent_obj = Tables.id Inner Join syscomments Comments On Triggers.id = Comments.id WHERE Triggers.xtype = 'TR' And Tables.xtype = 'U' And Tables.Name = " + tableName.ToSql() + " ORDER BY Tables.Name, Triggers.name");
		if (dataTable.Rows.Count != 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				string key = row.Field<string>("TriggerName");
				if (dictionary.ContainsKey(key))
				{
					dictionary[key] += row.Field<string>("TriggerText");
				}
				else
				{
					dictionary.Add(key, row.Field<string>("TriggerText"));
				}
			}
		}
		return dictionary;
	}

	public void RebuildTable(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, bool disableTriggers, List<string> messages)
	{
		DmoTable tableInfo = GetTableInfo(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, null);
		if (tableInfo != null && tableInfo.Fields != null && tableInfo.Fields.Length != 0)
		{
			VerifyTableIndexes(tableInfo, tableName, messages);
			RebuildTable(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, tableInfo.Fields, tableInfo.Indexes, mergeCustomFields: false, disableTriggers);
		}
	}

	public void RebuildTable(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName)
	{
		List<string> messages = new List<string>();
		RebuildTable(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, disableTriggers: false, messages);
	}

	public void RebuildTable(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, DmoField[] fields, DmoIndex[] indexes)
	{
		RebuildTable(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, fields, indexes, mergeCustomFields: true, disableTriggers: false);
	}

	public void AddDependencies(SqlConnection sqlConnection, M1User m1User, string databaseName, Dictionary<string, string> dependencies, Dictionary<string, List<string>> errorMessages)
	{
		foreach (KeyValuePair<string, string> item in dependencies.Reverse())
		{
			if (string.IsNullOrEmpty(item.Value))
			{
				continue;
			}
			try
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, item.Value);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("Operand data type bit is invalid for min operator") || ex.Message.Contains("Operand data type bit is invalid for max operator"))
				{
					string message = $"The following objects were not added, you need to manually correct the MIN/MAX operator in current object definition or in referenced objects: ";
					addErrorMessage(message, item.Key, errorMessages);
					continue;
				}
				throw;
			}
		}
	}

	public void RefreshView(SqlConnection sqlConnection, M1User m1User, string databaseName, string viewName, Dictionary<string, List<string>> errorMessages)
	{
		try
		{
			string queryString = $"SELECT referenced_entity_name AS entityName, sysObject.type AS typeObj FROM sys.sql_expression_dependencies AS dependencies INNER JOIN sys.objects AS sysObject ON dependencies.referenced_id = sysObject.object_id WHERE  sysObject.type = 'V' And referencing_id = OBJECT_ID(N'{viewName}')";
			DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, queryString);
			string empty = string.Empty;
			if (dataTable.Rows.Count != 0)
			{
				string empty2 = string.Empty;
				foreach (DataRow row in dataTable.Rows)
				{
					empty2 = row.Field<string>("entityName");
					RefreshView(sqlConnection, m1User, databaseName, empty2, errorMessages);
				}
				empty = $"EXECUTE sp_refreshview N'{viewName}'";
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, empty);
			}
			else
			{
				empty = $"EXECUTE sp_refreshview N'{viewName}'";
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, empty);
			}
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("Operand data type bit is invalid for min operator") || ex.Message.Contains("Operand data type bit is invalid for max operator"))
			{
				string message = $"The following views were not rebuilt, you need to manually correct the MIN/MAX operator in current view definition or in referenced views: ";
				addErrorMessage(message, viewName, errorMessages);
				return;
			}
			throw;
		}
	}

	public void RemoveDependencies(SqlConnection sqlConnection, M1User m1User, string databaseName, string objectName, string objectType, string objectDefinition, bool enforced, Dictionary<string, string> dependencies)
	{
		try
		{
			string empty = string.Empty;
			empty = ((!enforced) ? $"SELECT OBJECT_SCHEMA_NAME (referencing_id) AS SchemaName, OBJECT_NAME(referencing_id) AS EntityName, sysObject.type AS EntityType, OBJECT_DEFINITION(referencing_id) AS EntityDefinition, is_schema_bound_reference,\treferenced_minor_id FROM sys.sql_expression_dependencies AS dependencies INNER JOIN sys.objects AS sysObject ON dependencies.referencing_id = sysObject.object_id WHERE referenced_minor_id = 0 And referenced_id = OBJECT_ID(N'{objectName}')" : $"SELECT OBJECT_SCHEMA_NAME (referencing_id) AS SchemaName, OBJECT_NAME(referencing_id) AS EntityName, sysObject.type AS EntityType, OBJECT_DEFINITION(referencing_id) AS EntityDefinition, is_schema_bound_reference,\treferenced_minor_id FROM sys.sql_expression_dependencies AS dependencies INNER JOIN sys.objects AS sysObject ON dependencies.referencing_id = sysObject.object_id WHERE referenced_minor_id = 0 And is_schema_bound_reference = 1 And referenced_id = OBJECT_ID(N'{objectName}')");
			DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, empty);
			if (dataTable.Rows.Count != 0)
			{
				string empty2 = string.Empty;
				string empty3 = string.Empty;
				string empty4 = string.Empty;
				{
					foreach (DataRow row in dataTable.Rows)
					{
						empty2 = string.Format("{0}.{1}", row.Field<string>("SchemaName").Trim(), row.Field<string>("EntityName").Trim());
						empty3 = row.Field<string>("EntityType").Trim();
						empty4 = row.Field<string>("EntityDefinition");
						RemoveDependencies(sqlConnection, m1User, databaseName, empty2, empty3, empty4, enforced, dependencies);
						if (!dependencies.ContainsKey(empty2))
						{
							dependencies.Add(empty2, empty4);
							DropObject(sqlConnection, m1User, databaseName, empty2, empty3);
						}
					}
					return;
				}
			}
			if (!string.IsNullOrEmpty(objectType) && !dependencies.ContainsKey(objectName))
			{
				dependencies.Add(objectName, objectDefinition);
				DropObject(sqlConnection, m1User, databaseName, objectName, objectType);
			}
		}
		catch
		{
			throw;
		}
	}

	public void DropObject(SqlConnection sqlConnection, M1User m1User, string databaseName, string objectName, string objectType)
	{
		try
		{
			string text = string.Empty;
			switch (objectType)
			{
			case "FN":
				text = $"DROP FUNCTION {objectName} ";
				break;
			case "P":
				text = $"DROP PROCEDURE {objectName} ";
				break;
			case "V":
				text = $"DROP VIEW {objectName} ";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, text);
			}
		}
		catch
		{
			throw;
		}
	}

	private void addErrorMessage(string message, string objectName, Dictionary<string, List<string>> errorMessages)
	{
		if (errorMessages.ContainsKey(message))
		{
			List<string> list = errorMessages[message];
			if (!list.Contains(objectName))
			{
				list.Add(objectName);
			}
		}
		else
		{
			List<string> list2 = new List<string>();
			list2.Add(objectName);
			errorMessages.Add(message, list2);
		}
	}

	private void VerifyTableIndexes(DmoTable tableInfo, string tableName, List<string> messages)
	{
		List<DmoIndex> list = new List<DmoIndex>();
		DmoIndex[] indexes = tableInfo.Indexes;
		foreach (DmoIndex dmoIndex in indexes)
		{
			bool flag = true;
			string[] array = dmoIndex.Fields.Split(',');
			foreach (string field in array)
			{
				if (tableInfo.Fields.FirstOrDefault((DmoField x) => x.FieldName.Equals(field.Trim(), StringComparison.OrdinalIgnoreCase)) == null)
				{
					string item = $"Column name '{field.Trim()}' does not exist in table definition '{tableName}'. Please review it to avoid future problems.";
					messages.Add(item);
					flag = false;
					break;
				}
			}
			if (flag)
			{
				list.Add(dmoIndex);
			}
		}
		if (tableInfo.Indexes.Length != list.Count())
		{
			tableInfo.Indexes = list.ToArray();
		}
	}

	private void customCheck(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, DmoField[] fields)
	{
		DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "Select * From " + tableName + " Where 0=1");
		List<string> list = new List<string>();
		foreach (DataColumn col in dataTable.Columns)
		{
			if (!Array.Exists(fields, (DmoField f) => f.FieldName.Equals(col.ColumnName, StringComparison.CurrentCultureIgnoreCase)))
			{
				list.Add(col.ColumnName);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(tableName);
		foreach (string item in list)
		{
			stringBuilder.AppendLine(item);
		}
		File.WriteAllText("c:\\m1dev\\missing" + tableName + ".txt", stringBuilder.ToString());
	}

	public void RebuildTable(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, DmoField[] fields, DmoIndex[] indexes, bool mergeCustomFields)
	{
		RebuildTable(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, fields, indexes, mergeCustomFields, disableTriggers: false);
	}

	public void RebuildTable(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, DmoField[] fields, DmoIndex[] indexes, bool mergeCustomFields, bool disableTriggers)
	{
		tableName = tableName.Trim();
		string text = tableName + "_Temp";
		if (fields == null || fields.Length == 0)
		{
			return;
		}
		if (mergeCustomFields)
		{
			DmoTable tableInfo = GetTableInfo(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, true);
			if (tableInfo != null)
			{
				if (tableInfo.Fields != null && tableInfo.Fields.Length != 0)
				{
					List<DmoField> list = new List<DmoField>(fields);
					list.AddRange(tableInfo.Fields);
					fields = list.ToArray();
				}
				if (indexes != null && tableInfo.Indexes != null && tableInfo.Indexes.Length != 0)
				{
					List<DmoIndex> list2 = new List<DmoIndex>(indexes);
					list2.AddRange(tableInfo.Indexes);
					indexes = list2.ToArray();
				}
			}
		}
		if (DoesTableExist(sqlConnection, m1User, databaseName, text))
		{
			DropTable(sqlConnection, m1User, databaseName, text);
		}
		Dictionary<string, string> tableTriggers = getTableTriggers(sqlConnection, m1User, databaseName, tableName);
		dropTableTriggers(sqlConnection, m1User, databaseName, tableTriggers);
		try
		{
			bool flag = DoesTableExist(sqlConnection, m1User, databaseName, tableName);
			if (flag)
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "EXEC sp_rename '" + tableName + "', '" + text + "'");
			}
			try
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, MakeCreateTableString(fields, tableName));
				if (flag)
				{
					MakeFieldListInfo makeFieldListInfo = MakeFieldList(sqlConnection, m1User, databaseName, text, databaseName, tableName);
					if (makeFieldListInfo.IsIdentity)
					{
						serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "SET IDENTITY_INSERT dbo." + tableName + " ON");
					}
					try
					{
						serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "INSERT INTO dbo." + tableName + " (" + makeFieldListInfo.InsertFieldList?.ToString() + ") SELECT " + makeFieldListInfo.SelectFieldList?.ToString() + " FROM dbo." + text);
					}
					finally
					{
						if (makeFieldListInfo.IsIdentity)
						{
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "SET IDENTITY_INSERT dbo." + tableName + " OFF;");
						}
					}
				}
			}
			catch
			{
				DropTable(sqlConnection, m1User, databaseName, tableName);
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "EXEC sp_rename '" + text + "', '" + tableName + "'");
				throw;
			}
			if (flag)
			{
				DropTable(sqlConnection, m1User, databaseName, text);
			}
			string text2 = MakeIndexString(indexes, tableName);
			if (text2.Length != 0)
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, text2);
			}
		}
		finally
		{
			if (tableTriggers != null && tableTriggers.Count != 0)
			{
				VerifyLogTriggersForTable(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, disableTriggers);
				if (tableTriggers.ContainsKey(tableName + "_LogInsert"))
				{
					tableTriggers.Remove(tableName + "_LogInsert");
				}
				if (tableTriggers.ContainsKey(tableName + "_LogUpdate"))
				{
					tableTriggers.Remove(tableName + "_LogUpdate");
				}
				if (tableTriggers.ContainsKey(tableName + "_LogDelete"))
				{
					tableTriggers.Remove(tableName + "_LogDelete");
				}
				addTableTriggers(sqlConnection, m1User, databaseName, tableTriggers, disableTriggers, tableName);
			}
		}
	}

	public void RestoreDefaultM1(string backupFile)
	{
		string text = "M1_M1";
		long num = 1L;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string s = currentContext.Server.IniSettings.Get("DataLocation", "C:\\M1Data\\");
		s = s.AddBackslash();
		ServerFileSystem serverFileSystem = new ServerFileSystem(currentContext.DBServerManager);
		if (!serverFileSystem.FolderExists(s))
		{
			SetConfigure();
			serverFileSystem.CreateFolder(s);
			if (!serverFileSystem.FolderExists(s))
			{
				throw new M1Exception("M1 was unable to create folder " + s + ".");
			}
		}
		using SqlConnection sqlConnection = serverManager.GetConnection(null, string.Empty, openImmediately: true);
		DataTable dataTable = serverManager.GetDataTable(sqlConnection, null, string.Empty, 0, "RESTORE FILELISTONLY FROM DISK = " + backupFile.ToSql() + " WITH FILE = " + num);
		if (dataTable.Rows.Count > 0)
		{
			DataRow[] array = dataTable.Select("type = 'D'");
			if (array.Length != 0)
			{
				text2 = array[0].Field<string>("LogicalName").Trim();
			}
			array = dataTable.Select("type = 'L'");
			if (array.Length != 0)
			{
				text3 = array[0].Field<string>("LogicalName").Trim();
			}
		}
		if (text2.Length == 0 || text3.Length == 0)
		{
			throw new M1Exception("Unable to retrieve logical file name information from the backup file. The restore will not continue.");
		}
		serverManager.ExecuteCommand(sqlConnection, null, string.Empty, "RESTORE DATABASE " + text + " FROM DISK = " + backupFile.ToSql() + " WITH FILE = " + num + ",  MOVE " + text2.ToSql() + " TO '" + s + text + ".MDF',  MOVE " + text3.ToSql() + " TO '" + s + text + "_log.LDF', REPLACE");
		SetCompatibilityLevel(sqlConnection, null, text);
	}

	public void SetCompatibilityLevel(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		bool needToClose = true;
		SqlConnection connection = serverManager.GetConnection(m1User, databaseName, openImmediately: true, sqlConnection, null, ref needToClose);
		try
		{
			if (connection.ServerVersion != null)
			{
				string text = connection.ServerVersion.Trim();
				if (text.Contains("8.00."))
				{
					serverManager.ExecuteCommand(connection, m1User, databaseName, "sp_dbcmptlevel " + databaseName.ToSql() + ", 80");
				}
				else if (text.Contains("9.00."))
				{
					serverManager.ExecuteCommand(connection, m1User, databaseName, "sp_dbcmptlevel " + databaseName.ToSql() + ", 90");
				}
			}
		}
		finally
		{
			if (needToClose)
			{
				connection.Close();
			}
		}
	}

	public bool DropLogTriggersForTable(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName)
	{
		tableName = tableName.Trim().ToUpper();
		int num = 0;
		foreach (DataRow row in serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "exec sp_helptrigger @tabname = " + tableName.ToSql()).Rows)
		{
			string text = row.Field<string>("trigger_name").Trim().ToUpper();
			if (text == tableName + "_LOGUPDATE" || text == tableName + "_LOGDELETE" || text == tableName + "_LOGINSERT")
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "Drop Trigger " + text);
				num++;
			}
		}
		return num != 0;
	}

	public bool DisableLogTriggersForTable(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		tableName = tableName.Trim().ToUpper();
		int num = 0;
		foreach (DataRow row in serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "exec sp_helptrigger @tabname = " + tableName.ToSql()).Rows)
		{
			string text = row.Field<string>("trigger_name").Trim().ToUpper();
			if (text == tableName + "_LOGUPDATE" || text == tableName + "_LOGDELETE" || text == tableName + "_LOGINSERT")
			{
				stringBuilder.Length = 0;
				stringBuilder.Append("DISABLE TRIGGER " + text + " ON " + tableName);
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, stringBuilder.ToString());
				num++;
			}
		}
		return num != 0;
	}

	public void AddLogTriggersForTable(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, int logChanges, bool disableTriggers)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		string text = tableName.Trim();
		tableName = tableName.Trim().ToUpper();
		foreach (DataRow row in serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "exec sp_helptrigger @tabname = " + tableName.ToSql()).Rows)
		{
			string text2 = row.Field<string>("trigger_name").Trim().ToUpper();
			if (text2 == tableName + "_LOGUPDATE")
			{
				flag3 = true;
			}
			if (text2 == tableName + "_LOGDELETE")
			{
				flag2 = true;
			}
			if (text2 == tableName + "_LOGINSERT")
			{
				flag = true;
			}
		}
		if (flag && flag3 && flag2)
		{
			return;
		}
		string text3 = string.Empty;
		string text4 = string.Empty;
		DataTable dataTable = m1DataDictionary.GetDataTable("select dtUniqueField,dtKeyFields from DDTables Where dtTable = " + tableName.ToSql());
		if (dataTable.Rows.Count != 0)
		{
			text3 = dataTable.Rows[0].Field<string>("dtUniqueField").Trim();
			text4 = dataTable.Rows[0].Field<string>("dtKeyFields").Trim();
		}
		if (text3.Length != 0)
		{
			string[] keyFieldsArray = text4.Split(',');
			DataTable dataTable2 = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "exec sp_columns @table_name=" + tableName.ToSql());
			if (!flag)
			{
				CreateInsertTrigger(sqlConnection, m1User, dataTable2, keyFieldsArray, databaseName, tableName, text, text3, logChanges);
			}
			if (!flag3)
			{
				CreateUpdateTrigger(sqlConnection, m1User, dataTable2, keyFieldsArray, databaseName, tableName, text, text3, logChanges);
			}
			if (!flag2)
			{
				CreateDeleteTrigger(sqlConnection, m1User, dataTable2, keyFieldsArray, databaseName, tableName, text, text3, logChanges);
			}
			if (disableTriggers)
			{
				DisableLogTriggersForTable(sqlConnection, m1User, databaseName, text);
			}
		}
	}

	private void CreateInsertTrigger(SqlConnection sqlConnection, M1User m1User, DataTable fieldsTable, IEnumerable<string> keyFieldsArray, string databaseName, string tableName, string tableProper, string uniqueField, int logChangesType)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("CREATE TRIGGER " + tableProper + "_LogInsert ON [" + tableProper + "]\r");
		stringBuilder.Append("FOR INSERT \rAS \r");
		stringBuilder.Append("SET NOCOUNT ON\r");
		stringBuilder.Append("DECLARE @cUserID As varchar(35)\r");
		stringBuilder.Append("SET @cUserID = Upper(Left(App_Name(),20))\r");
		stringBuilder.Append("If @cUserID <> ''\r");
		stringBuilder.Append("BEGIN\r");
		if (logChangesType == 2)
		{
			stringBuilder.Append(GenerateLogDeclares(fieldsTable, "insert"));
			stringBuilder.Append("\tInsert Into ChangeLog (xagChangeType, xagTableName, xagTableUniqueID, xagChangeDate, xagChangeUserID, xagTableKeyValues, xagTableOldValues, xagTableNewValues) Select 'I'," + tableName.ToSql() + ", ins." + uniqueField + ", GetDate(), @cUserID " + GenerateLogKeyClause(keyFieldsArray, fieldsTable, "insert") + GenerateLogClause(fieldsTable, "insert") + GenerateLogFrom(fieldsTable, "insert", tableProper, uniqueField) + "\r");
		}
		else
		{
			stringBuilder.Append("\tInsert Into ChangeLog (xagChangeType, xagTableName, xagTableUniqueID, xagChangeDate, xagChangeUserID, xagTableKeyValues) Select 'I'," + tableName.ToSql() + ", ins." + uniqueField + ", GetDate(), @cUserID " + GenerateLogKeyClause(keyFieldsArray, fieldsTable, "insert") + " From inserted ins\r");
		}
		stringBuilder.Append("END\r");
		stringBuilder.Append("SET NOCOUNT OFF");
		serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, stringBuilder.ToString());
	}

	private void CreateUpdateTrigger(SqlConnection sqlConnection, M1User m1User, DataTable fieldsTable, IEnumerable keyFieldsArray, string databaseName, string tableName, string tableProper, string uniqueField, int logChangesType)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("CREATE TRIGGER " + tableProper + "_LogUpdate ON [" + tableProper + "]\r");
		stringBuilder.Append("FOR UPDATE \rAS \r");
		stringBuilder.Append("SET NOCOUNT ON\r");
		stringBuilder.Append("DECLARE @cUserID As varchar(35)\r");
		stringBuilder.Append("SET @cUserID = Upper(Left(App_Name(),20))\r");
		stringBuilder.Append("If @cUserID <> ''\r");
		stringBuilder.Append("BEGIN\r");
		stringBuilder.Append(GenerateLogDeclares(fieldsTable, "update"));
		GenerateUpdateLogClause(fieldsTable, out var oldValueText, out var newValueText);
		string text = GenerateLogFrom(fieldsTable, "update", tableProper, uniqueField);
		stringBuilder.Append("\tINSERT INTO @tempTableOldValues(uniqueId, oldValues)\r");
		stringBuilder.Append("\tSELECT del." + uniqueField + ", " + oldValueText + " \r" + text + "\r\r");
		stringBuilder.Append("\tINSERT INTO @tempTableNewValues(uniqueId, newValues)\r");
		stringBuilder.Append("\tSELECT ins." + uniqueField + ", " + newValueText + " \r" + text + "\r\r");
		stringBuilder.Append("\tIF (EXISTS (SELECT oldValues FROM @tempTableOldValues WHERE oldValues <> '') AND EXISTS (SELECT newValues FROM @tempTableNewValues WHERE newValues <> ''))\r");
		stringBuilder.Append("\tBEGIN\r");
		string text2 = "(SELECT oldValues FROM @tempTableOldValues WHERE uniqueId = del." + uniqueField + " AND oldValues <> '')\r";
		string text3 = "(SELECT newValues FROM @tempTableNewValues WHERE uniqueId = ins." + uniqueField + " AND newValues <> '')\r";
		string text4 = "WHERE ins." + uniqueField + " IN (SELECT uniqueId FROM @tempTableNewValues WHERE newValues <> '')";
		if (logChangesType == 2)
		{
			stringBuilder.Append("\t\tINSERT INTO ChangeLog (xagChangeType, xagTableName, xagTableUniqueID, xagChangeDate, xagChangeUserID, xagTableKeyValues, xagTableOldValues, xagTableNewValues) SELECT 'U'," + tableName.ToSql() + ", ins." + uniqueField + ", GetDate(), @cUserID " + GenerateLogKeyClause(keyFieldsArray, fieldsTable, "update") + "," + text2 + "," + text3 + text + "\r" + text4);
		}
		else
		{
			stringBuilder.Append("\tINSERT INTO ChangeLog (xagChangeType, xagTableName, xagTableUniqueID, xagChangeDate, xagChangeUserID, xagTableKeyValues) Select 'U'," + tableName.ToSql() + ", ins." + uniqueField + ", GetDate(), @cUserID " + GenerateLogKeyClause(keyFieldsArray, fieldsTable, "update") + " FROM inserted ins\r");
		}
		stringBuilder.Append("\tEND\r\r");
		stringBuilder.Append("END\r");
		stringBuilder.Append("SET NOCOUNT OFF");
		serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, stringBuilder.ToString());
	}

	private void GenerateUpdateLogClause(DataTable fieldsTable, out string oldValueText, out string newValueText)
	{
		string text = string.Empty;
		string text2 = (oldValueText = string.Empty);
		newValueText = text;
		if (fieldsTable.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in fieldsTable.Rows)
		{
			string text3 = row.Field<string>("column_name").Trim();
			string fieldType = row.Field<string>("type_name").Trim().ToLower();
			switch (SetSSDataType(row, fieldType))
			{
			case "char":
			case "varchar":
			case "nchar":
			case "nvarchar":
				text2 = text2 + "\r + Case When IsNull(ins." + text3 + ",'') <> IsNull(del." + text3 + ",'') Then '" + text3 + " = ' + RTrim(IsNull(del." + text3 + ",'')) + Char(13) Else '' End ";
				text = text + "\r + Case When IsNull(ins." + text3 + ",'') <> IsNull(del." + text3 + ",'') Then '" + text3 + " = ' + RTrim(IsNull(ins." + text3 + ",'')) + Char(13) Else '' End ";
				break;
			case "datetime":
			case "smalldatetime":
				text2 = text2 + "\r + Case when CAST(IsNull(Convert(nvarchar(25),ins." + text3 + "),'19000101') AS date) <> CAST(IsNull(Convert(nvarchar(25),del." + text3 + "),'19000101') AS date) Then '" + text3 + " = ' + IsNull(Convert(nvarchar(25),del." + text3 + "),'') + Char(13) Else '' End ";
				text = text + "\r + Case when CAST(IsNull(Convert(nvarchar(25),ins." + text3 + "),'19000101') AS date) <> CAST(IsNull(Convert(nvarchar(25),del." + text3 + "),'19000101') AS date) Then '" + text3 + " = ' + IsNull(Convert(nvarchar(25),ins." + text3 + "),'') + Char(13) Else '' End ";
				break;
			case "real":
			case "tinyint":
			case "decimal":
			case "float":
			case "smallint":
			case "bit":
			case "int":
				text2 = text2 + "\r + Case When IsNull(ins." + text3 + ",0) <> IsNull(del." + text3 + ",0) Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(del." + text3 + ",0)) + Char(13) Else '' End ";
				text = text + "\r + Case When IsNull(ins." + text3 + ",0) <> IsNull(del." + text3 + ",0) Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(ins." + text3 + ",0)) + Char(13) Else '' End ";
				break;
			case "numeric":
				text2 = text2 + "\r + Case When ins." + text3 + " <> del." + text3 + " Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(del." + text3 + ",0)) + Char(13) Else '' End ";
				text = text + "\r + Case When ins." + text3 + " <> del." + text3 + " Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(ins." + text3 + ",0)) + Char(13) Else '' End ";
				break;
			case "money":
			case "smallmoney":
				text2 = text2 + "\r + Case When IsNull(ins." + text3 + ",0) <> IsNull(del." + text3 + ",0) Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(del." + text3 + ",0)) + Char(13) Else '' End ";
				text = text + "\r + Case When IsNull(ins." + text3 + ",0) <> IsNull(del." + text3 + ",0) Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(ins." + text3 + ",0)) + Char(13) Else '' End ";
				break;
			case "text":
			case "ntext":
				if (!text3.EndsWith("rtf", StringComparison.CurrentCultureIgnoreCase))
				{
					string text4 = text3.Substring(0, 1).ToUpper() + text3.Substring(1);
					text2 = text2 + "\r + Case When @b" + text4 + " <> 0 Then '" + text3 + " = ?' + Char(13) Else '' End ";
					text = text + "\r + Case When @b" + text4 + " <> 0 Then '" + text3 + " = ' + Case When Convert(nvarchar(6),IsNull(source." + text3 + ",'')) = '{\\rtf1' Then '?' Else Convert(nvarchar(1024),IsNull(source." + text3 + ",'')) End + Char(13) Else '' End ";
				}
				break;
			}
		}
		if (text2.Length != 0)
		{
			text2 = text2.Substring(3);
		}
		if (text2.Length == 0)
		{
			text2 = "''";
		}
		if (text.Length != 0)
		{
			text = text.Substring(3);
		}
		if (text.Length == 0)
		{
			text = "''";
		}
		oldValueText = text2;
		newValueText = text;
	}

	private void CreateDeleteTrigger(SqlConnection sqlConnection, M1User m1User, DataTable fieldsTable, IEnumerable keyFieldsArray, string databaseName, string tableName, string tableProper, string uniqueField, int logChangesType)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("CREATE TRIGGER " + tableProper + "_LogDelete ON [" + tableProper + "]\r");
		stringBuilder.Append("FOR DELETE \rAS \r");
		stringBuilder.Append("SET NOCOUNT ON\r");
		stringBuilder.Append("DECLARE @cUserID As varchar(35)\r");
		stringBuilder.Append("SET @cUserID = Upper(Left(App_Name(),20))\r");
		stringBuilder.Append("If @cUserID <> ''\r");
		stringBuilder.Append("BEGIN\r");
		if (logChangesType == 2)
		{
			stringBuilder.Append("\tInsert Into ChangeLog (xagChangeType, xagTableName, xagTableUniqueID, xagChangeDate, xagChangeUserID, xagTableKeyValues, xagTableOldValues, xagTableNewValues) Select 'D'," + tableName.ToSql() + ", del." + uniqueField + ", GetDate(), @cUserID " + GenerateLogKeyClause(keyFieldsArray, fieldsTable, "delete") + GenerateLogClause(fieldsTable, "delete") + " From deleted del\r");
		}
		else
		{
			stringBuilder.Append("\tInsert Into ChangeLog (xagChangeType, xagTableName, xagTableUniqueID, xagChangeDate, xagChangeUserID, xagTableKeyValues) Select 'D'," + tableName.ToSql() + ", del." + uniqueField + ", GetDate(), @cUserID " + GenerateLogKeyClause(keyFieldsArray, fieldsTable, "delete") + " From deleted del\r");
		}
		stringBuilder.Append("END\r");
		stringBuilder.Append("SET NOCOUNT OFF");
		serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, stringBuilder.ToString());
	}

	private string GenerateLogClause(DataTable fieldsTable, string queryType)
	{
		GenerateLogClause(fieldsTable, queryType, out var oldValueText, out var newValueText);
		return ",\r" + oldValueText + ", \r" + newValueText + "\r";
	}

	private void GenerateLogClause(DataTable fieldsTable, string queryType, out string oldValueText, out string newValueText)
	{
		string text = string.Empty;
		string text2 = (oldValueText = string.Empty);
		newValueText = text;
		if (fieldsTable.Rows.Count == 0)
		{
			return;
		}
		queryType = queryType.Trim().ToLower();
		foreach (DataRow row in fieldsTable.Rows)
		{
			string text3 = row.Field<string>("column_name").Trim();
			string fieldType = row.Field<string>("type_name").Trim().ToLower();
			switch (SetSSDataType(row, fieldType))
			{
			case "char":
			case "varchar":
			case "nchar":
			case "nvarchar":
				if (!(queryType == "insert"))
				{
					if (queryType == "delete")
					{
						text2 = text2 + "\r + Case When IsNull(del." + text3 + ",'') <> '' Then '" + text3 + " = ' + RTrim(IsNull(del." + text3 + ",'')) + Char(13) Else '' End ";
						break;
					}
					text2 = text2 + "\r + Case When IsNull(ins." + text3 + ",'') <> IsNull(del." + text3 + ",'') Then '" + text3 + " = ' + RTrim(IsNull(del." + text3 + ",'')) + Char(13) Else '' End ";
					text = text + "\r + Case When IsNull(ins." + text3 + ",'') <> IsNull(del." + text3 + ",'') Then '" + text3 + " = ' + RTrim(IsNull(ins." + text3 + ",'')) + Char(13) Else '' End ";
				}
				else
				{
					text = text + "\r + Case When IsNull(ins." + text3 + ",'') <> '' Then '" + text3 + " = ' + RTrim(IsNull(ins." + text3 + ",'')) + Char(13) Else '' End ";
				}
				break;
			case "datetime":
			case "smalldatetime":
				if (!(queryType == "insert"))
				{
					if (queryType == "delete")
					{
						text2 = text2 + "\r + Case When del." + text3 + " Is Not Null Then '" + text3 + " = ' + IsNull(Convert(nvarchar(25),del." + text3 + "),'') + Char(13) Else '' End ";
						break;
					}
					text2 = text2 + "\r + Case When IsNull(ins." + text3 + ",'19000101') <> IsNull(del." + text3 + ",'19000101') Then '" + text3 + " = ' + IsNull(Convert(nvarchar(25),del." + text3 + "),'') + Char(13) Else '' End ";
					text = text + "\r + Case When IsNull(ins." + text3 + ",'19000101') <> IsNull(del." + text3 + ",'19000101') Then '" + text3 + " = ' + IsNull(Convert(nvarchar(25),ins." + text3 + "),'') + Char(13) Else '' End ";
				}
				else
				{
					text = text + "\r + Case When ins." + text3 + " Is Not Null Then '" + text3 + " = ' + IsNull(Convert(nvarchar(25),ins." + text3 + "),'') + Char(13) Else '' End ";
				}
				break;
			case "real":
			case "tinyint":
			case "decimal":
			case "numeric":
			case "float":
			case "smallint":
			case "bit":
			case "int":
				if (!(queryType == "insert"))
				{
					if (queryType == "delete")
					{
						text2 = text2 + "\r + Case When IsNull(del." + text3 + ",0) <> 0 Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(del." + text3 + ",0)) + Char(13) Else '' End ";
						break;
					}
					text2 = text2 + "\r + Case When IsNull(ins." + text3 + ",0) <> IsNull(del." + text3 + ",0) Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(del." + text3 + ",0)) + Char(13) Else '' End ";
					text = text + "\r + Case When IsNull(ins." + text3 + ",0) <> IsNull(del." + text3 + ",0) Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(ins." + text3 + ",0)) + Char(13) Else '' End ";
				}
				else
				{
					text = text + "\r + Case When IsNull(ins." + text3 + ",0) <> 0 Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(ins." + text3 + ",0)) + Char(13) Else '' End ";
				}
				break;
			case "money":
			case "smallmoney":
				if (!(queryType == "insert"))
				{
					if (queryType == "delete")
					{
						text2 = text2 + "\r + Case When IsNull(del." + text3 + ",0) <> 0 Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(del." + text3 + ",0)) + Char(13) Else '' End ";
						break;
					}
					text2 = text2 + "\r + Case When IsNull(ins." + text3 + ",0) <> IsNull(del." + text3 + ",0) Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(del." + text3 + ",0)) + Char(13) Else '' End ";
					text = text + "\r + Case When IsNull(ins." + text3 + ",0) <> IsNull(del." + text3 + ",0) Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(ins." + text3 + ",0)) + Char(13) Else '' End ";
				}
				else
				{
					text = text + "\r + Case When IsNull(ins." + text3 + ",0) <> 0 Then '" + text3 + " = ' + Convert(nvarchar(25),IsNull(ins." + text3 + ",0)) + Char(13) Else '' End ";
				}
				break;
			case "text":
			case "ntext":
				if (!(queryType == "insert"))
				{
					if (!(queryType == "delete") && !text3.EndsWith("rtf", StringComparison.CurrentCultureIgnoreCase))
					{
						string text4 = text3.Substring(0, 1).ToUpper() + text3.Substring(1);
						text2 = text2 + "\r + Case When @b" + text4 + " <> 0 Then '" + text3 + " = ?' + Char(13) Else '' End ";
						text = text + "\r + Case When @b" + text4 + " <> 0 Then '" + text3 + " = ' + Case When Convert(nvarchar(6),IsNull(source." + text3 + ",'')) = '{\\rtf1' Then '?' Else Convert(nvarchar(1024),IsNull(source." + text3 + ",'')) End + Char(13) Else '' End ";
					}
				}
				else if (!text3.EndsWith("rtf", StringComparison.CurrentCultureIgnoreCase))
				{
					string text5 = text3.Substring(0, 1).ToUpper() + text3.Substring(1);
					text = text + "\r + Case When @b" + text5 + " <> 0 Then '" + text3 + " = ' + Case When Convert(nvarchar(6),IsNull(source." + text3 + ",'')) = '{\\rtf1' Then '?' Else Convert(nvarchar(1024),IsNull(source." + text3 + ",'')) End + Char(13) Else '' End ";
				}
				break;
			}
		}
		if (text2.Length != 0)
		{
			text2 = text2.Substring(3);
		}
		if (text2.Length == 0)
		{
			text2 = "''";
		}
		if (text.Length != 0)
		{
			text = text.Substring(3);
		}
		if (text.Length == 0)
		{
			text = "''";
		}
		oldValueText = text2;
		newValueText = text;
	}

	private static string GenerateLogFrom(DataTable fieldsTable, string queryType, string tableName, string uniqueField)
	{
		bool flag = false;
		queryType = queryType.Trim().ToLower();
		if (fieldsTable.Rows.Count != 0)
		{
			foreach (DataRow row in fieldsTable.Rows)
			{
				string fieldType = row.Field<string>("type_name").Trim().ToLower();
				fieldType = SetSSDataType(row, fieldType);
				if ((fieldType == "text" || fieldType == "ntext") && !row.Field<string>("column_name").Trim().EndsWith("rtf", StringComparison.CurrentCultureIgnoreCase))
				{
					flag = true;
					break;
				}
			}
		}
		if (!(queryType == "insert"))
		{
			if (queryType == "delete")
			{
				return " FROM deleted del";
			}
			if (flag)
			{
				return " FROM inserted ins INNER JOIN deleted del ON ins." + uniqueField + " = del." + uniqueField + " LEFT JOIN " + tableName + " source ON ins." + uniqueField + " = source." + uniqueField;
			}
			return " FROM inserted ins INNER JOIN deleted del ON ins." + uniqueField + " = del." + uniqueField;
		}
		if (flag)
		{
			return " FROM inserted ins INNER JOIN " + tableName + " source ON ins." + uniqueField + " = source." + uniqueField;
		}
		return " FROM inserted ins";
	}

	private static string SetSSDataType(DataRow item, string fieldType)
	{
		if (item.Field<byte>("SS_DATA_TYPE") == Convert.ToByte(39))
		{
			if (fieldType == "text")
			{
				fieldType = "varchar";
			}
			else if (fieldType == "ntext")
			{
				fieldType = "nvarchar";
			}
		}
		return fieldType;
	}

	private string GenerateLogDeclares(DataTable fieldsTable, string queryType)
	{
		StringBuilder stringBuilder = new StringBuilder();
		queryType = queryType.Trim().ToLower();
		if (fieldsTable.Rows.Count != 0 && (queryType == "insert" || queryType == "update"))
		{
			foreach (DataRow row in fieldsTable.Rows)
			{
				string fieldType = row.Field<string>("type_name").Trim().ToLower();
				fieldType = SetSSDataType(row, fieldType);
				string text = row.Field<string>("column_name").Trim();
				if ((fieldType == "text" || fieldType == "ntext") && !text.EndsWith("rtf", StringComparison.CurrentCultureIgnoreCase))
				{
					string text2 = text.Substring(0, 1).ToUpper() + text.Substring(1);
					stringBuilder.Append("\tDECLARE @b" + text2 + " As bit\r");
					stringBuilder.Append("\tSET @b" + text2 + " = 0\r");
					stringBuilder.Append("\tIf Update(" + text + ")\r");
					stringBuilder.Append("\t\tSET @b" + text2 + " =1\r\r");
				}
			}
			if (queryType == "update")
			{
				stringBuilder.Append("\tDECLARE @tempTableOldValues TABLE (uniqueId uniqueidentifier , oldValues nvarchar(max))\r");
				stringBuilder.Append("\tDECLARE @tempTableNewValues TABLE (uniqueId uniqueidentifier, newValues nvarchar(max))\r\r");
			}
		}
		return stringBuilder.ToString();
	}

	private string GenerateLogKeyClause(IEnumerable keyFieldsArray, DataTable fieldsTable, string queryType)
	{
		string text = string.Empty;
		if (fieldsTable.Rows.Count != 0)
		{
			queryType = queryType.Trim().ToLower();
			foreach (string item in keyFieldsArray)
			{
				string text2 = item.Trim();
				if (text2.Length == 0)
				{
					continue;
				}
				DataRow[] array = fieldsTable.Select("column_name = " + text2.ToLinq());
				if (array.Length != 0)
				{
					text2 = array[0].Field<string>("column_name").Trim();
					if (text2.Length != 0)
					{
						text = ((queryType == "insert") ? (text + "\r + '" + text2 + " = ' + RTrim(IsNull(Convert(nvarchar(50),ins." + text2 + "),'')) + Char(13)") : ((!(queryType == "delete")) ? (text + "\r + '" + text2 + " = ' + RTrim(IsNull(Convert(nvarchar(50),ins." + text2 + "),'')) + Char(13)") : (text + "\r + '" + text2 + " = ' + RTrim(IsNull(Convert(nvarchar(50),del." + text2 + "),'')) + Char(13)")));
					}
				}
			}
			if (text.Length != 0)
			{
				text = text.Substring(3);
			}
		}
		if (text.Length == 0)
		{
			text = "''";
		}
		return ",\r" + text;
	}

	public void VerifyLogTriggersForTable(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName)
	{
		VerifyLogTriggersForTable(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, disableTriggers: false);
	}

	public void VerifyLogTriggersForTable(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, bool disableTriggers)
	{
		if (DoesFieldExist(sqlConnection, m1User, databaseName, "NextIDs", "xanLogChanges"))
		{
			int num = 0;
			DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "select xanLogChanges From NextIDs Where xanTable = " + tableName.Trim().ToUpper().ToSql());
			if (dataTable.Rows.Count != 0)
			{
				num = Convert.ToInt16(dataTable.Rows[0]["xanLogChanges"]);
			}
			if (num > 0)
			{
				AddLogTriggersForTable(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, num, disableTriggers);
			}
			else
			{
				DropLogTriggersForTable(sqlConnection, m1User, databaseName, tableName);
			}
		}
	}

	public void RefreshLogTriggersForAllTables(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName)
	{
		foreach (DataRow row in serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "exec sp_tables @table_type = \"'TABLE'\"").Rows)
		{
			string tableName = row.Field<string>("table_name").Trim();
			if (DoesTableExist(sqlConnection, m1User, databaseName, tableName))
			{
				DropLogTriggersForTable(sqlConnection, m1User, databaseName, tableName);
				VerifyLogTriggersForTable(sqlConnection, m1User, m1DataDictionary, databaseName, tableName);
			}
		}
	}

	public void DropIndexes(SqlConnection sqlConnection, M1User m1User, string databaseName, string table, DmoIndex[] indexes, List<string> messages)
	{
		Dictionary<string, IndexInfo> indexesForTable = GetIndexesForTable(sqlConnection, m1User, databaseName, table);
		foreach (DmoIndex dmoIndex in indexes)
		{
			if (indexesForTable.ContainsKey(dmoIndex.IndexName))
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "DROP INDEX " + table + "." + dmoIndex.IndexName);
			}
		}
	}

	public void VerifyIndexes(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, DmoIndex[] indexes, List<string> messages)
	{
		if (indexes == null || indexes.Length == 0)
		{
			return;
		}
		Dictionary<string, IndexInfo> indexesForTable = GetIndexesForTable(sqlConnection, m1User, databaseName, tableName);
		foreach (DmoIndex dmoIndex in indexes)
		{
			if (!indexesForTable.ContainsKey(dmoIndex.IndexName))
			{
				CreateIndex(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, "Create " + (dmoIndex.Unique ? "Unique " : string.Empty) + " Index " + dmoIndex.IndexName + " On " + tableName + " (" + dmoIndex.Fields + ")", messages);
			}
		}
	}

	private void CreateIndex(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, string command, List<string> messages)
	{
		try
		{
			serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, command);
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("CREATE UNIQUE INDEX statement terminated because a duplicate key was found"))
			{
				RemoveDuplicates(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, messages);
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, command);
				return;
			}
			throw;
		}
	}

	public void VerifyIndexesOnField(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, string fieldName, List<string> messages)
	{
		if (m1DataDictionary == null)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		string text = string.Empty;
		string text2 = string.Empty;
		Dictionary<string, IndexInfo> indexesForTable = GetIndexesForTable(sqlConnection, m1User, databaseName, tableName);
		flag = indexesForTable.Count != 0;
		DataTable dataTable = m1DataDictionary.GetDataTable("select dtKeyFields,dtUniqueField from DDTables where dtTable =" + tableName.Trim().ToUpper().ToSql());
		if (dataTable.Rows.Count != 0)
		{
			if (dataTable.Rows[0].Field<string>("dtKeyFields").Trim().Contains(fieldName.Trim()))
			{
				text = dataTable.Rows[0].Field<string>("dtKeyFields").Trim().Replace(",", "_");
				if (text.Trim().Length != 0)
				{
					flag2 = false;
					if (flag)
					{
						flag2 = indexesForTable.ContainsKey(text);
					}
					if (!flag2)
					{
						CreateIndex(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, "CREATE UNIQUE INDEX " + text + " ON " + tableName + " (" + dataTable.Rows[0].Field<string>("dtKeyFields").Trim() + ")", messages);
					}
				}
			}
			if (dataTable.Rows[0].Field<string>("dtUniqueField").Trim().Contains(fieldName.Trim()))
			{
				text2 = dataTable.Rows[0].Field<string>("dtUniqueField").Trim().Replace(",", "_");
				if (text2.Trim().Length != 0)
				{
					flag2 = false;
					if (flag)
					{
						flag2 = indexesForTable.ContainsKey(text2);
					}
					if (!flag2)
					{
						CreateIndex(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, "CREATE UNIQUE INDEX " + text2 + " ON " + tableName + " (" + dataTable.Rows[0].Field<string>("dtUniqueField").Trim() + ")", messages);
					}
				}
			}
		}
		dataTable = m1DataDictionary.GetDataTable("select dfField from DDFields where dfTable = " + tableName.Trim().ToUpper().ToSql() + " and dfField = " + fieldName.Trim().ToUpper().ToSql() + " and dfindexed <> 0");
		if (dataTable.Rows.Count != 0 && dataTable.Rows[0].Field<string>("dfField").Trim().Contains(fieldName.Trim()))
		{
			string text3 = dataTable.Rows[0].Field<string>("dfField").Trim().Replace(",", "_");
			flag2 = false;
			if (flag)
			{
				flag2 = indexesForTable.ContainsKey(text3);
			}
			if (!flag2 && text3.Trim().ToUpper() == text.Trim().ToUpper() && text3.Trim().ToUpper() == text2.Trim().ToUpper())
			{
				CreateIndex(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, "CREATE INDEX " + text3 + " ON " + tableName + " (" + dataTable.Rows[0].Field<string>("dfField").Trim() + ")", messages);
			}
		}
	}

	public void OnDuplicatesRemoved(DuplicatesRemovedEventArgs e)
	{
		this.DuplicatesRemoved?.Invoke(this, e);
	}

	public void RemoveDuplicates(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, List<string> messages)
	{
		DataTable dataTable = m1DataDictionary.GetDataTable("select dtKeyFields from DDTables where dtTable =" + tableName.Trim().ToUpper().ToSql());
		if (dataTable.Rows.Count != 0)
		{
			string text = dataTable.Rows[0].Field<string>("dtKeyFields").Trim();
			string[] array = text.Split(',');
			string text2 = array[0];
			if (text.Trim().Length == 0)
			{
				return;
			}
			string text3 = string.Empty;
			array.Count();
			dataTable = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "select " + text + ",count(" + text2 + ") as dupcount from " + tableName + " group by " + text + " having count(" + text2 + ") > 1");
			foreach (DataRow row in dataTable.Rows)
			{
				string text4 = "SELECT * FROM " + tableName.Trim() + " WHERE ";
				string text5 = string.Empty;
				string[] array2 = array;
				foreach (string text6 in array2)
				{
					text5 = text5 + text6 + " = " + row[text6.Trim()].ToSql() + " AND ";
					text3 = text3 + text6 + " = " + row[text6.Trim()].ToSql() + " AND ";
				}
				if (text5.ToUpper().EndsWith(" AND "))
				{
					text5 = text5.Remove(text5.Length - 5);
					text3 = text3.Remove(text3.Length - 5);
					text3 += "\n\r";
				}
				text4 += text5;
				DataTable dataTable2 = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, text4);
				if (dataTable2.Rows.Count > 0)
				{
					serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "DELETE FROM " + tableName + " WHERE " + text5);
					DataTable dataTable3 = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "SELECT * FROM " + tableName + " WHERE 0=1");
					DataRow dataRow2 = dataTable3.NewRow();
					foreach (DataColumn column in dataTable3.Columns)
					{
						dataRow2[column] = dataTable2.Rows[0][column.ColumnName];
					}
					dataTable3.Rows.Add(dataRow2);
					SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(serverManager.NewSqlCommand(sqlConnection, m1User, databaseName, "SELECT * FROM " + tableName + " WHERE 0=1"));
					new SqlCommandBuilder(sqlDataAdapter);
					sqlDataAdapter.Update(dataTable3.GetChanges());
					continue;
				}
				throw new M1Exception("Unable to retrieve records from " + tableName + " in duplicate check.");
			}
			if (text3.Trim().Length != 0 && messages != null)
			{
				text3 = "The following records were duplicated in " + tableName.Trim() + ":\n\r" + text3;
				messages.Add(text3);
			}
			return;
		}
		throw new M1Exception("Key field information for table " + tableName + " could not be found in DDTables.");
	}

	public DmoTable GetTableInfo(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, bool? customOnly)
	{
		DmoTable dmoTable = new DmoTable();
		SqlCommand sqlCommand = m1DataDictionary.NewSqlCommand("select dfTable,dfField,dfdbtype,dfLength,dfDecimals,dfAllowNulls,dfindexed,dfCustom from DDFields where dfTable = @TableName" + ((!customOnly.HasValue) ? string.Empty : (customOnly.Value ? " And dfCustom <> 0" : " And dfCustom = 0")) + " order by dfSequence");
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = tableName;
		DataTable dataTable = m1DataDictionary.GetDataTable(sqlCommand);
		sqlCommand = m1DataDictionary.NewSqlCommand("select dtTable,dtKeyFields,dtUniqueField from DDTables where dtTable = @TableName");
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = tableName;
		DataTable dataTable2 = m1DataDictionary.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			List<DmoIndex> list = new List<DmoIndex>();
			List<string> list2 = new List<string>();
			if (dataTable2.Rows.Count != 0)
			{
				if (dataTable2.Rows[0].Field<string>("dtKeyFields").Length != 0)
				{
					if (!customOnly.HasValue || !customOnly.Value)
					{
						list.Add(new DmoIndex(dataTable2.Rows[0].Field<string>("dtKeyFields"), unique: true));
					}
					list2.Add(dataTable2.Rows[0].Field<string>("dtKeyFields"));
				}
				if (dataTable2.Rows[0].Field<string>("dtUniqueField").Length != 0 && !list2.Contains(dataTable2.Rows[0].Field<string>("dtUniqueField"), StringComparer.CurrentCultureIgnoreCase))
				{
					if (!customOnly.HasValue || !customOnly.Value)
					{
						list.Add(new DmoIndex(dataTable2.Rows[0].Field<string>("dtUniqueField"), unique: true));
					}
					list2.Add(dataTable2.Rows[0].Field<string>("dtUniqueField"));
				}
			}
			List<DmoField> list3 = new List<DmoField>();
			foreach (DataRow row in dataTable.Rows)
			{
				list3.Add(new DmoField(row.Field<string>("dfField"), row.Field<string>("dfDBType"), row.Field<byte>("dfLength"), row.Field<byte>("dfDecimals"), row.Field<bool>("dfAllowNulls")));
				if (row.Field<bool>("dfIndexed") && !list2.Contains(row.Field<string>("dfField"), StringComparer.CurrentCultureIgnoreCase))
				{
					list.Add(new DmoIndex(row.Field<string>("dfField"), unique: false));
				}
			}
			dmoTable.Fields = list3.ToArray();
			dmoTable.Indexes = list.ToArray();
		}
		return dmoTable;
	}

	public void CreateTable(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName)
	{
		DmoTable tableInfo = GetTableInfo(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, null);
		if (tableInfo != null && tableInfo.Fields != null && tableInfo.Fields.Length != 0)
		{
			CreateTable(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, tableInfo.Fields, tableInfo.Indexes);
			return;
		}
		throw new M1Exception("Cannot create table " + tableName + " because no fields have been defined.");
	}

	public void CreateTable(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, DmoField[] fields, DmoIndex[] indexes)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(MakeCreateTableString(fields, tableName));
		if (stringBuilder.Length != 0)
		{
			stringBuilder.Append('\r');
			stringBuilder.Append(MakeIndexString(indexes, tableName));
			serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, stringBuilder.ToString());
			return;
		}
		throw new M1Exception("Cannot create table " + tableName + " because no fields have been defined.");
	}

	private MakeFieldListInfo MakeFieldList(SqlConnection sqlConnection, M1User m1User, string sourceDatabase, string sourceTable, string destDatabase, string destTable)
	{
		MakeFieldListInfo makeFieldListInfo = new MakeFieldListInfo();
		if (destTable.Length > 0)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, sourceDatabase, 0, "exec sp_columns @table_name =" + sourceTable.ToSql());
			foreach (DataRow row in serverManager.GetDataTable(sqlConnection, m1User, destDatabase, 0, "exec sp_columns @table_name =" + destTable.ToSql()).Rows)
			{
				DataRow dataRow = null;
				foreach (DataRow row2 in dataTable.Rows)
				{
					if (row2.Field<string>("column_name").Equals(row.Field<string>("column_name"), StringComparison.CurrentCultureIgnoreCase))
					{
						dataRow = row2;
						break;
					}
				}
				if (dataRow == null)
				{
					continue;
				}
				empty = row.Field<string>("type_name").Trim().ToLower();
				empty2 = row.Field<string>("column_name").Trim();
				if (empty.IndexOf("identity", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					makeFieldListInfo.IsIdentity = true;
				}
				if (empty.IndexOf("timestamp", StringComparison.CurrentCultureIgnoreCase) != -1)
				{
					continue;
				}
				stringBuilder.Append(", " + empty2);
				if (empty.Equals(dataRow.Field<string>("type_name").Trim(), StringComparison.CurrentCultureIgnoreCase) && row.Field<short>("nullable").Equals(dataRow.Field<short>("nullable")))
				{
					stringBuilder2.Append(", " + empty2);
				}
				else if (row.Field<short>("nullable") != 0)
				{
					switch (empty)
					{
					case "varchar":
					case "nvarchar":
						stringBuilder2.Append(", Case When RTrim(Convert(" + empty + "(max)," + empty2 + ")) = '' Then Null Else RTrim(Convert(" + empty + "(max)," + empty2 + ")) End");
						break;
					case "text":
						stringBuilder2.Append(", Case When RTrim(Convert(nvarchar(max)," + empty2 + ")) = '' Then Null Else " + empty2 + " End");
						break;
					case "ntext":
						stringBuilder2.Append(", Case When RTrim(Convert(nvarchar(max)," + empty2 + ")) = '' Then Null Else " + empty2 + " End");
						break;
					default:
						stringBuilder2.Append(", " + empty2);
						break;
					}
				}
				else
				{
					switch (empty)
					{
					case "varchar":
					case "nvarchar":
						stringBuilder2.Append(", RTrim(Convert(" + empty + "(max)," + empty2 + "))");
						break;
					case "bit":
						stringBuilder2.Append(", Case When " + empty2 + " = 0 Then 0 Else 1 End");
						break;
					default:
						stringBuilder2.Append(", " + empty2);
						break;
					}
				}
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Remove(0, 1);
			}
			if (stringBuilder2.Length > 0)
			{
				stringBuilder2.Remove(0, 1);
			}
			makeFieldListInfo.InsertFieldList = stringBuilder;
			makeFieldListInfo.SelectFieldList = stringBuilder2;
		}
		return makeFieldListInfo;
	}

	private MakeFieldListInfo MakeFieldList(DmoField[] fields)
	{
		MakeFieldListInfo makeFieldListInfo = new MakeFieldListInfo();
		if (fields != null && fields.Length != 0)
		{
			foreach (DmoField dmoField in fields)
			{
				if (dmoField.FieldType.Equals("identity", StringComparison.CurrentCultureIgnoreCase))
				{
					makeFieldListInfo.IsIdentity = true;
				}
				if (makeFieldListInfo.SelectFieldList.Length != 0)
				{
					makeFieldListInfo.SelectFieldList.Append(',');
				}
				makeFieldListInfo.SelectFieldList.Append(dmoField.FieldName);
				if (makeFieldListInfo.InsertFieldList.Length != 0)
				{
					makeFieldListInfo.InsertFieldList.Append(',');
				}
				makeFieldListInfo.InsertFieldList.Append(dmoField.FieldName);
			}
		}
		return makeFieldListInfo;
	}

	public string MakeCreateTableString(DmoField[] fields, string tableName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (fields != null && fields.Length != 0)
		{
			stringBuilder.Append("CREATE TABLE dbo." + tableName + " (");
			foreach (DmoField dmoField in fields)
			{
				stringBuilder.Append(dmoField.FieldName);
				stringBuilder.Append(" " + dmoField.SqlType);
				if (!dmoField.Nullable)
				{
					stringBuilder.Append(" Not Null ");
					if (!string.IsNullOrEmpty(dmoField.DefaultValue))
					{
						stringBuilder.Append(dmoField.DefaultValue);
					}
				}
				stringBuilder.Append(',');
			}
			stringBuilder.Length--;
			stringBuilder.Append(")");
		}
		return stringBuilder.ToString();
	}

	public string MakeIndexString(DmoIndex[] indexes, string tableName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (indexes != null && indexes.Length != 0)
		{
			foreach (DmoIndex dmoIndex in indexes)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append('\r');
				}
				stringBuilder.Append("Create " + (dmoIndex.Unique ? "Unique " : string.Empty) + " Index " + dmoIndex.IndexName + " On " + tableName + " (" + dmoIndex.Fields + ")");
			}
		}
		return stringBuilder.ToString();
	}

	public void AddColumn(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string table, string field, string type, int precision, int scale, bool verifyIndexes, bool dropTriggers, List<string> messages)
	{
		AddColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, type, precision, scale, verifyIndexes, dropTriggers, isNullable: false, messages);
	}

	public void AddColumn(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string table, string field, string type, int precision, int scale, bool verifyIndexes, bool dropTriggers, bool isNullable, List<string> messages)
	{
		type = type.Trim();
		table = table.Trim();
		bool flag = true;
		if (DoesFieldExist(sqlConnection, m1User, databaseName, table, field))
		{
			throw new M1Exception("Column " + field + " already exists.");
		}
		bool flag2 = false;
		if (dropTriggers)
		{
			flag2 = DropLogTriggersForTable(sqlConnection, m1User, databaseName, table);
		}
		serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "ALTER TABLE " + table + " ADD " + field + getSQLServerType(type, precision, scale) + getAllowNullForType(type, isNullable) + getDefaultForType(type, isNullable));
		if (flag2)
		{
			VerifyLogTriggersForTable(sqlConnection, m1User, m1DataDictionary, databaseName, table);
		}
		if (flag && verifyIndexes)
		{
			VerifyIndexesOnField(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, messages);
		}
	}

	public bool AddMultipleColumns(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string table, bool verifyIndexes, bool dropTriggers, List<string> messages, params object[] fieldDefs)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		int num = 0;
		int num2 = 0;
		string text = string.Empty;
		bool flag = false;
		bool flag2 = false;
		table = table.Trim();
		for (int i = 0; i < fieldDefs.Length; i++)
		{
			object[] array = (object[])fieldDefs[i];
			empty = array[0].ToString().Trim();
			empty2 = array[1].ToString().Trim();
			num = (int)array[2];
			num2 = (int)array[3];
			flag2 = array.Length > 4 && (bool)array[4];
			if (!DoesFieldExist(sqlConnection, m1User, databaseName, table, empty))
			{
				text = text + ", " + empty + getSQLServerType(empty2, num, num2) + getAllowNullForType(empty2, flag2) + getDefaultForType(empty2, flag2);
			}
		}
		if (text.Trim().Length > 0)
		{
			if (dropTriggers)
			{
				flag = DropLogTriggersForTable(sqlConnection, m1User, databaseName, table);
			}
			text = text.Substring(2);
			serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "ALTER TABLE " + table + " ADD " + text);
			if (flag)
			{
				VerifyLogTriggersForTable(sqlConnection, m1User, m1DataDictionary, databaseName, table);
			}
		}
		for (int j = 0; j < fieldDefs.Length; j++)
		{
			object[] array = (object[])fieldDefs[j];
			if (verifyIndexes)
			{
				VerifyIndexesOnField(sqlConnection, m1User, m1DataDictionary, databaseName, table, array[0].ToString().Trim(), messages);
			}
		}
		return true;
	}

	public void DropColumn(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string table, string field, bool dropTriggers)
	{
		table = table.Trim();
		field = field.Trim().ToUpper();
		DropDefaultConstraint(sqlConnection, m1User, databaseName, table, field);
		DropRelatedIndexes(sqlConnection, m1User, databaseName, table, field);
		bool flag = false;
		if (dropTriggers)
		{
			flag = DropLogTriggersForTable(sqlConnection, m1User, databaseName, table);
		}
		serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "ALTER TABLE " + table + " DROP COLUMN " + field);
		if (flag)
		{
			VerifyLogTriggersForTable(sqlConnection, m1User, m1DataDictionary, databaseName, table);
		}
	}

	public void DropColumnEx(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string table, string field)
	{
		if (DoesFieldExist(sqlConnection, m1User, databaseName, table, field))
		{
			DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, dropTriggers: true);
		}
	}

	protected List<string> GetStatisticsForField(SqlConnection sqlConnection, M1User m1User, string databaseName, string table, string field)
	{
		string queryString = "SELECT S.NAME FROM   SYS.OBJECTS AS O INNER JOIN SYS.STATS AS S (NOLOCK) ON O.OBJECT_ID = S.OBJECT_ID INNER JOIN SYS.STATS_COLUMNS AS SC (NOLOCK) ON SC.OBJECT_ID = S.OBJECT_ID AND S.STATS_ID = SC.STATS_ID WHERE  (O.OBJECT_ID = OBJECT_ID(@TableName,'local')) AND (O.TYPE IN ('U')) AND (INDEXPROPERTY(S.OBJECT_ID,S.NAME,'IsStatistics') = 1) AND (COL_NAME(SC.OBJECT_ID,SC.COLUMN_ID) = @ColumnName)";
		SqlCommand sqlCommand = serverManager.NewSqlCommand(sqlConnection, m1User, databaseName, queryString);
		sqlCommand.Parameters.Add(new SqlParameter("@tablename", SqlDbType.NVarChar)).Value = table;
		sqlCommand.Parameters.Add(new SqlParameter("@columnname", SqlDbType.NVarChar)).Value = field;
		SqlDataAdapter adapter;
		DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, sqlCommand, fillSchema: false, out adapter);
		List<string> list = new List<string>();
		foreach (DataRow row in dataTable.Rows)
		{
			list.Add(row.Field<string>("name"));
		}
		return list;
	}

	protected Dictionary<string, IndexInfo> GetIndexesForTable(SqlConnection sqlConnection, M1User m1User, string databaseName, string table)
	{
		string queryString = "SELECT      ind.name as index_name     ,ic.index_column_id      ,col.name as column_name     ,ind.is_unique FROM sys.indexes ind  INNER JOIN sys.index_columns ic      ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id  INNER JOIN sys.columns col      ON ic.object_id = col.object_id and ic.column_id = col.column_id  INNER JOIN sys.tables t      ON ind.object_id = t.object_id  WHERE     t.name = @tablename ORDER BY     ind.name, ic.index_column_id ";
		SqlCommand sqlCommand = serverManager.NewSqlCommand(sqlConnection, m1User, databaseName, queryString);
		sqlCommand.Parameters.Add(new SqlParameter("@tablename", SqlDbType.NVarChar)).Value = table;
		Dictionary<string, IndexInfo> dictionary = new Dictionary<string, IndexInfo>(StringComparer.CurrentCultureIgnoreCase);
		SqlDataAdapter adapter;
		foreach (DataRow row in serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, sqlCommand, fillSchema: false, out adapter).Rows)
		{
			string text = row.Field<string>("index_name");
			if (!dictionary.ContainsKey(text))
			{
				dictionary.Add(text, new IndexInfo(text, Convert.ToBoolean(row["is_unique"]), text.StartsWith("_")));
			}
			dictionary[text].Columns.Add(row.Field<string>("column_name"));
		}
		return dictionary;
	}

	public void DropRelatedIndexes(SqlConnection sqlConnection, M1User m1User, string databaseName, string table, string field)
	{
		field = field.Trim().ToUpper();
		table = table.Trim().ToUpper();
		foreach (KeyValuePair<string, IndexInfo> item in GetIndexesForTable(sqlConnection, m1User, databaseName, table))
		{
			if (item.Value.Columns.Contains(field, StringComparer.CurrentCultureIgnoreCase) && !item.Value.AutoCreated)
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "DROP INDEX " + table + "." + item.Value.Name);
			}
		}
		DropRelatedStatistics(sqlConnection, m1User, databaseName, table, field);
	}

	public void DropRelatedStatistics(SqlConnection sqlConnection, M1User m1User, string databaseName, string table, string field)
	{
		field = field.Trim().ToUpper();
		table = table.Trim().ToUpper();
		foreach (string item in GetStatisticsForField(sqlConnection, m1User, databaseName, table, field))
		{
			serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "DROP STATISTICS " + table + "." + item);
		}
	}

	public void AlterColumn(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string table, string field, string type, int precision, int scale, List<string> messages)
	{
		AlterColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, type, precision, scale, isNullable: false, messages);
	}

	public void AlterColumn(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string table, string field, string type, int precision, int scale, bool isNullable, List<string> messages)
	{
		bool flag = false;
		bool isNullable2 = false;
		string currentFieldType = GetCurrentFieldType(sqlConnection, m1User, databaseName, table, field, ref isNullable2);
		type = type.Trim().ToLower();
		table = table.Trim();
		bool flag2 = DropLogTriggersForTable(sqlConnection, m1User, databaseName, table);
		if (currentFieldType == "text")
		{
			switch (type)
			{
			case "char":
			case "nchar":
				if (DoesFieldExist(sqlConnection, m1User, databaseName, table, "atemp1"))
				{
					DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: true);
				}
				RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, "atemp1", dropTriggers: true);
				try
				{
					AddColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, type, precision, scale, verifyIndexes: true, dropTriggers: true, isNullable, messages);
					serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "=LEFT(CONVERT(" + type + " (" + precision + "),atemp1)," + precision + ")");
				}
				catch
				{
					if (DoesFieldExist(sqlConnection, m1User, databaseName, table, field))
					{
						DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, dropTriggers: true);
					}
					RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", field, dropTriggers: true);
					throw;
				}
				DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: true);
				flag = true;
				break;
			case "varchar":
			case "nvarchar":
				if (DoesFieldExist(sqlConnection, m1User, databaseName, table, "atemp1"))
				{
					DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: true);
				}
				RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, "atemp1", dropTriggers: true);
				try
				{
					AddColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, type, precision, scale, verifyIndexes: true, dropTriggers: true, isNullable, messages);
					serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "=RTrim(LEFT(CONVERT(" + type + " (" + precision + "),atemp1)," + precision + "))");
				}
				catch
				{
					if (DoesFieldExist(sqlConnection, m1User, databaseName, table, field))
					{
						DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, dropTriggers: true);
					}
					RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", field, dropTriggers: true);
					throw;
				}
				DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: true);
				flag = true;
				break;
			default:
				DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, dropTriggers: true);
				AddColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, type, precision, scale, verifyIndexes: true, dropTriggers: true, isNullable, messages);
				break;
			}
		}
		else
		{
			short num = 0;
			if ((currentFieldType == "char" && type == "char") || (currentFieldType == "varchar" && type == "varchar") || (currentFieldType == "nchar" && type == "nchar") || (currentFieldType == "nvarchar" && type == "nvarchar"))
			{
				if (GetCurrentFieldLength(sqlConnection, m1User, databaseName, table, field) == precision && isNullable2 == isNullable)
				{
					flag = true;
				}
				else
				{
					DropRelatedIndexes(sqlConnection, m1User, databaseName, table, field);
					serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "ALTER TABLE " + table + " ALTER COLUMN " + field + " " + type + "(" + precision + ") " + (isNullable ? "" : "NOT NULL"));
					VerifyIndexesOnField(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, messages);
					flag = true;
				}
			}
			else if (currentFieldType == "numeric" && type == "numeric")
			{
				int currentFieldLength = GetCurrentFieldLength(sqlConnection, m1User, databaseName, table, field);
				num = GetCurrentFieldScale(sqlConnection, m1User, databaseName, table, field);
				if (currentFieldLength == precision && num == scale)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				DropDefaultConstraint(sqlConnection, m1User, databaseName, table, field);
				DropRelatedIndexes(sqlConnection, m1User, databaseName, table, field);
				flag = true;
				switch (type)
				{
				case "bit":
				case "int":
				case "real":
				case "char":
				case "money":
				case "nchar":
				case "float":
				case "numeric":
				case "tinyint":
				case "varchar":
				case "nvarchar":
				case "smallint":
				case "bigint":
				case "smallmoney":
					flag = false;
					if (DoesFieldExist(sqlConnection, m1User, databaseName, table, "atemp1"))
					{
						DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: true);
					}
					RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, "atemp1", dropTriggers: true);
					try
					{
						AddColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, type, precision, scale, verifyIndexes: true, dropTriggers: true, isNullable, messages);
						switch (type)
						{
						case "numeric":
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "=ISNULL(CONVERT(numeric(" + precision + "," + scale.ToSql() + "),Case When IsNumeric(Convert(nvarchar(30),atemp1))=0 Then 0 Else atemp1 End),0)");
							break;
						case "tinyint":
						case "float":
						case "money":
						case "int":
						case "real":
						case "smallint":
						case "bigint":
						case "smallmoney":
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "=ISNULL(CONVERT(" + type + ",Case When IsNumeric(Convert(nvarchar(30),atemp1))=0 Then 0 Else atemp1 End),0)");
							break;
						case "varchar":
						case "nchar":
						case "char":
						case "nvarchar":
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "=RTrim(LEFT(ISNULL(CONVERT(" + type + "(255),atemp1),'')," + precision + "))");
							break;
						case "bit":
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "=CASE WHEN ISNULL(CONVERT(int,atemp1),0) = 0 THEN 0 ELSE 1 END");
							break;
						default:
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "=ISNULL(atemp1,0)");
							break;
						}
					}
					catch
					{
						if (DoesFieldExist(sqlConnection, m1User, databaseName, table, field))
						{
							DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, dropTriggers: true);
						}
						RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", field, dropTriggers: true);
						throw;
					}
					DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: true);
					flag = true;
					VerifyIndexesOnField(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, messages);
					break;
				case "identity":
					flag = false;
					if (DoesFieldExist(sqlConnection, m1User, databaseName, table, field))
					{
						DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, dropTriggers: true);
						AddColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, type, precision, scale, verifyIndexes: true, dropTriggers: true, isNullable, messages);
						flag = true;
					}
					else
					{
						AddColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, type, precision, scale, verifyIndexes: true, dropTriggers: true, isNullable, messages);
						flag = true;
					}
					break;
				case "text":
				case "varchar(max)":
					flag = false;
					if (DoesFieldExist(sqlConnection, m1User, databaseName, table, "atemp1"))
					{
						DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: true);
					}
					RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, "atemp1", dropTriggers: true);
					try
					{
						AddColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, type, precision, scale, verifyIndexes: true, dropTriggers: true, isNullable, messages);
						if (isNullable)
						{
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "=RTRIM(CONVERT(varchar(max),atemp1))");
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "= Null Where CONVERT(varchar(10)," + field + ") = ''");
						}
						else
						{
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "=ISNULL(RTRIM(CONVERT(varchar(max),atemp1)),'')");
						}
					}
					catch
					{
						if (DoesFieldExist(sqlConnection, m1User, databaseName, table, field))
						{
							DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, dropTriggers: true);
						}
						RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", field, dropTriggers: true);
						throw;
					}
					DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: true);
					flag = true;
					break;
				case "ntext":
				case "nvarchar(max)":
					flag = false;
					if (DoesFieldExist(sqlConnection, m1User, databaseName, table, "atemp1"))
					{
						DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: true);
					}
					RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, "atemp1", dropTriggers: true);
					try
					{
						AddColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, type, precision, scale, verifyIndexes: true, dropTriggers: true, isNullable, messages);
						if (isNullable)
						{
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "=RTRIM(CONVERT(nvarchar(max),atemp1))");
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "= Null Where CONVERT(nvarchar(10)," + field + ") = ''");
						}
						else
						{
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "=ISNULL(RTRIM(CONVERT(nvarchar(max),atemp1)),'')");
						}
					}
					catch
					{
						if (DoesFieldExist(sqlConnection, m1User, databaseName, table, field))
						{
							DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, dropTriggers: true);
						}
						RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", field, dropTriggers: true);
						throw;
					}
					DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: true);
					flag = true;
					break;
				case "date":
				case "datetime":
					flag = false;
					if (DoesFieldExist(sqlConnection, m1User, databaseName, table, "atemp1"))
					{
						DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: true);
					}
					RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, "atemp1", dropTriggers: true);
					try
					{
						AddColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, type, precision, scale, verifyIndexes: true, dropTriggers: true, isNullable, messages);
						serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "=ISNULL(atemp1,NULL)");
					}
					catch
					{
						if (DoesFieldExist(sqlConnection, m1User, databaseName, table, field))
						{
							DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, dropTriggers: true);
						}
						RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", field, dropTriggers: true);
						throw;
					}
					DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: true);
					flag = true;
					break;
				default:
					if (!type.Equals(currentFieldType, StringComparison.CurrentCultureIgnoreCase))
					{
						flag = false;
						DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, dropTriggers: true);
						AddColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, type, precision, scale, verifyIndexes: true, dropTriggers: true, isNullable, messages);
						flag = true;
					}
					break;
				}
				if (flag)
				{
					VerifyIndexesOnField(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, messages);
				}
			}
		}
		if (flag2)
		{
			VerifyLogTriggersForTable(sqlConnection, m1User, m1DataDictionary, databaseName, table);
		}
	}

	private void AlterColumnCollation(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string table, string field, string collation, List<string> messages)
	{
		bool isNullable = false;
		string currentFieldType = GetCurrentFieldType(sqlConnection, m1User, databaseName, table, field, ref isNullable);
		table = table.Trim();
		switch (currentFieldType)
		{
		case "char":
		case "varchar":
		case "nchar":
		case "nvarchar":
		{
			int currentFieldLength = GetCurrentFieldLength(sqlConnection, m1User, databaseName, table, field);
			DropRelatedIndexes(sqlConnection, m1User, databaseName, table, field);
			serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "ALTER TABLE " + table + " ALTER COLUMN [" + field + "] " + currentFieldType + "(" + currentFieldLength + ") COLLATE " + collation + " NOT NULL");
			VerifyIndexesOnField(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, messages);
			break;
		}
		case "text":
		case "ntext":
			if (DoesFieldExist(sqlConnection, m1User, databaseName, table, "atemp1"))
			{
				DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: false);
			}
			RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, "atemp1", dropTriggers: false);
			AddColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, field, currentFieldType, 4, 0, verifyIndexes: true, dropTriggers: false, isNullable, messages);
			serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "UPDATE " + table + " SET " + field + "=atemp1");
			DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, "atemp1", dropTriggers: false);
			break;
		}
	}

	public string CompareDatabases(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string database1, string database2)
	{
		long num = 0L;
		string text = string.Empty;
		DataTable dataTable = m1DataDictionary.GetDataTable("select dtTable,dtKeyFields from DDTables Order by dtTable");
		if (dataTable.Rows.Count > 0)
		{
			DataTable dataTable2 = m1DataDictionary.GetDataTable("select Upper(dfTable) As dfTable,dffield,dfdbtype from DDFields Order By dfTable,dfSequence");
			foreach (DataRow row in dataTable.Rows)
			{
				string text2 = row.Field<string>("dtTable").Trim();
				long num2 = 0L;
				string text3 = compareDBWhere(dataTable2, text2);
				DataTable dataTable3 = serverManager.GetDataTable(sqlConnection, m1User, "master", 0, "select count(*) as rec_count from " + database1 + ".dbo." + text2 + " db1 inner join " + database2 + ".dbo." + text2 + " db2 on " + text3);
				if (dataTable3.Rows.Count > 0)
				{
					num2 = dataTable3.Rows[0].Field<long>("rec_count");
				}
				dataTable3 = serverManager.GetDataTable(sqlConnection, m1User, "master", 0, "Select isnull((select count(*) from " + database1 + ".dbo." + text2 + "),0) as Table1Count, isnull((select count(*) from " + database2 + ".dbo." + text2 + "),0) as Table2Count");
				if (dataTable3.Rows.Count > 0)
				{
					if (dataTable3.Rows[0].Field<long>("Table1Count") != dataTable3.Rows[0].Field<long>("Table2Count"))
					{
						text = text + "Table " + text2 + " has a different record count\r";
						num++;
					}
					if (dataTable3.Rows[0].Field<long>("Table1Count") != num2 || dataTable3.Rows[0].Field<long>("Table2Count") != num2)
					{
						text = text + "Table " + num + " has mismatched join cont " + num2 + " matches out of " + dataTable3.Rows[0].Field<long>("Table1Count") + " total record\r";
					}
				}
			}
		}
		if (text.Trim().Length > 0)
		{
			return "Databases match.";
		}
		return num + " tables don't have matching record count:\r" + text;
	}

	private string compareDBWhere(DataTable tempDDFields, string table)
	{
		string text = string.Empty;
		table = table.Trim().ToUpper();
		DataRow[] array = tempDDFields.Select("dfTable = " + table.ToLinq());
		foreach (DataRow row in array)
		{
			if (row.Field<string>("dfTable").Trim().ToUpper() == table)
			{
				string text2 = row.Field<string>("dfField").Trim().ToUpper();
				switch (row.Field<string>("dfDBType").Trim().ToLower())
				{
				case "real":
				case "char":
				case "money":
				case "nchar":
				case "float":
				case "numeric":
				case "tinyint":
				case "varchar":
				case "nvarchar":
				case "nvarchar(max)":
				case "bit":
				case "int":
				case "varchar(max)":
				case "bigint":
				case "smallmoney":
					text = text + " And db1." + text2 + " = db2." + text2;
					break;
				case "text":
					text = text + " And Convert(varchar(2000),db1." + text2 + ") = Convert(varchar(2000),db2." + text2 + ")";
					break;
				case "ntext":
					text = text + " And Convert(nvarchar(2000),db1." + text2 + ") = Convert(nvarchar(2000),db2." + text2 + ")";
					break;
				case "date":
				case "datetime":
				case "smalldatetime":
					text = text + " And IsNull(db1." + text2 + ",'19000101') = IsNull(db2." + text2 + ",'19000101')";
					break;
				}
			}
		}
		return text.Substring(4);
	}

	public void CopyAndTransformTable(SqlConnection sqlConnection, M1User m1User, string sourceDatabase, string sourceTable, string destDatabase, string destTable)
	{
		if (destTable.Length <= 0)
		{
			return;
		}
		string empty = string.Empty;
		string empty2 = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		bool flag = false;
		DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, sourceDatabase, 0, "exec sp_columns @table_name =" + destTable.ToSql());
		foreach (DataRow row in serverManager.GetDataTable(sqlConnection, m1User, destDatabase, 0, "exec sp_columns @table_name =" + destTable.ToSql()).Rows)
		{
			DataRow dataRow = null;
			foreach (DataRow row2 in dataTable.Rows)
			{
				if (row2.Field<string>("column_name").Equals(row.Field<string>("column_name"), StringComparison.CurrentCultureIgnoreCase))
				{
					dataRow = row2;
					break;
				}
			}
			if (dataRow == null)
			{
				continue;
			}
			empty = row.Field<string>("type_name").Trim().ToLower();
			empty2 = row.Field<string>("column_name").Trim();
			if (empty.IndexOf("identity", StringComparison.CurrentCultureIgnoreCase) != -1)
			{
				flag = true;
			}
			if (empty.IndexOf("timestamp", StringComparison.CurrentCultureIgnoreCase) != -1)
			{
				continue;
			}
			stringBuilder.Append(", " + empty2);
			if (empty.Equals(dataRow.Field<string>("type_name").Trim(), StringComparison.CurrentCultureIgnoreCase) && row.Field<short>("nullable").Equals(dataRow.Field<short>("nullable")))
			{
				stringBuilder2.Append(", " + empty2);
			}
			else if (row.Field<short>("nullable") != 0)
			{
				switch (empty)
				{
				case "varchar":
				case "nvarchar":
					stringBuilder2.Append(", Case When RTrim(Convert(" + empty + "(max)," + empty2 + ")) = '' Then Null Else RTrim(Convert(" + empty + "(max)," + empty2 + ")) End");
					break;
				case "text":
					stringBuilder2.Append(", Case When RTrim(Convert(nvarchar(max)," + empty2 + ")) = '' Then Null Else " + empty2 + " End");
					break;
				case "ntext":
					stringBuilder2.Append(", Case When RTrim(Convert(nvarchar(max)," + empty2 + ")) = '' Then Null Else " + empty2 + " End");
					break;
				default:
					stringBuilder2.Append(", " + empty2);
					break;
				}
			}
			else
			{
				switch (empty)
				{
				case "varchar":
				case "nvarchar":
					stringBuilder2.Append(", RTrim(Convert(" + empty + "(max)," + empty2 + "))");
					break;
				case "bit":
					stringBuilder2.Append(", Case When " + empty2 + " = 0 Then 0 Else 1 End");
					break;
				default:
					stringBuilder2.Append(", " + empty2);
					break;
				}
			}
		}
		stringBuilder.Remove(0, 1);
		stringBuilder2.Remove(0, 1);
		serverManager.ExecuteCommand(sqlConnection, m1User, destDatabase, "TRUNCATE TABLE " + destDatabase + ".DBO " + destTable);
		if (flag)
		{
			serverManager.ExecuteCommand(sqlConnection, m1User, destDatabase, "SET IDENTITY_INSERT dbo." + destTable + " ON");
		}
		try
		{
			serverManager.ExecuteCommand(sqlConnection, m1User, sourceDatabase, "INSERT INTO " + destDatabase + ".DBO" + destTable + " (" + stringBuilder?.ToString() + ") SELECT " + stringBuilder2?.ToString() + " FROM " + sourceDatabase + ".DBO" + sourceTable);
		}
		finally
		{
			if (flag)
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, destDatabase, "SET IDENTITY_INSERT dbo." + destTable + " OFF;");
			}
		}
	}

	public void CopyAllData(SqlConnection sqlConnection, M1User m1User, string sourceDatabase, string destDatabase)
	{
		foreach (DataRow row2 in serverManager.GetDataTable(sqlConnection, m1User, sourceDatabase, 0, "exec sp_tables @table_type = 'TABLE'").Rows)
		{
			string text = row2.Field<string>("table_name").Trim();
			if (text.Length <= 0 || !(text.ToLower() != "dtproperties"))
			{
				continue;
			}
			string text2 = string.Empty;
			foreach (DataRow row3 in serverManager.GetDataTable(sqlConnection, m1User, sourceDatabase, 0, "exec sp_columns @table_name =" + text.ToSql()).Rows)
			{
				if (row3.Field<string>("type_name").Trim().ToLower()
					.Contains("identity"))
				{
					text2 = text2 + ", " + row3.Field<string>("column_name").Trim();
				}
			}
			text2 = text2.Substring(1);
			string queryString = "INSERT INTO " + destDatabase + ".DBO" + text + " (" + text2 + ") SELECT " + text2 + " FROM " + sourceDatabase + ".DBO" + text;
			serverManager.ExecuteCommand(sqlConnection, m1User, sourceDatabase, "DELETE FROM  " + destDatabase + ".DBO " + text);
			serverManager.ExecuteCommand(sqlConnection, m1User, sourceDatabase, queryString);
		}
	}

	public void DropAllIndexesOnTable(SqlConnection sqlConnection, M1User m1User, string databaseName, string table)
	{
		table = table.Trim().ToUpper();
		foreach (KeyValuePair<string, IndexInfo> item in GetIndexesForTable(sqlConnection, m1User, databaseName, table))
		{
			if (!item.Value.AutoCreated)
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "DROP INDEX " + table + "." + item.Value.Name);
			}
		}
	}

	public void DropTableInAllDatasets(SqlConnection sqlConnection, M1User m1User, string table)
	{
		foreach (DatabaseInfo installedDatabase in currentContext.InstalledDatabases)
		{
			if (installedDatabase.Name.Length > 0)
			{
				DoesTableExist(sqlConnection, m1User, installedDatabase.Name, table);
				DropTable(sqlConnection, m1User, installedDatabase.Name, table);
			}
		}
	}

	public void RenameColumnInAllDatasets(M1User m1User, M1DataDictionary m1DataDictionary, string table, string curName, string newName)
	{
		using SqlConnection sqlConnection = serverManager.GetConnection(m1User, "master", openImmediately: true);
		foreach (DatabaseInfo installedDatabase in currentContext.InstalledDatabases)
		{
			string name = installedDatabase.Name;
			if (name.Length > 0 && DoesFieldExist(sqlConnection, m1User, name, table, curName))
			{
				RenameColumn(sqlConnection, m1User, m1DataDictionary, name, table, curName, newName, dropTriggers: true);
			}
		}
	}

	public void RenameColumnInIndexes(SqlConnection sqlConnection, M1User m1User, string databaseName, string table, string curName, string newName)
	{
		curName = curName.Trim().ToUpper();
		newName = newName.Trim();
		table = table.Trim().ToUpper();
		foreach (KeyValuePair<string, IndexInfo> item in GetIndexesForTable(sqlConnection, m1User, databaseName, table))
		{
			if (item.Value.Columns.Contains(curName, StringComparer.CurrentCultureIgnoreCase) && item.Value.Name.IndexOf(curName, StringComparison.CurrentCultureIgnoreCase) != -1 && !item.Value.AutoCreated)
			{
				string text = item.Value.Name.Replace(curName, newName);
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "sp_rename '" + table + "." + item.Value.Name + "', '" + text + "', 'INDEX'");
			}
		}
	}

	public void RenameTable(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string curName, string newName)
	{
		curName = curName.Trim().ToUpper();
		newName = newName.Trim();
		bool num = DropLogTriggersForTable(sqlConnection, m1User, databaseName, curName);
		serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "EXEC sp_rename " + curName.ToSql() + ", " + newName.ToSql());
		if (num)
		{
			VerifyLogTriggersForTable(sqlConnection, m1User, m1DataDictionary, databaseName, newName);
		}
	}

	public void RenameTableEX(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string curName, string newName)
	{
		RenameTable(sqlConnection, m1User, m1DataDictionary, databaseName, curName, newName);
	}

	public void VerifyIndexesOnTable(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string table, List<string> messages, List<string> changes)
	{
		bool flag = false;
		bool flag2 = false;
		string text = string.Empty;
		string text2 = string.Empty;
		Dictionary<string, IndexInfo> indexesForTable = GetIndexesForTable(sqlConnection, m1User, databaseName, table);
		flag = indexesForTable.Count != 0;
		DataTable dataTable = m1DataDictionary.GetDataTable("select dtKeyFields,dtUniqueField from DDTables where dtTable =" + table.Trim().ToUpper().ToSql());
		if (dataTable.Rows.Count != 0)
		{
			text = dataTable.Rows[0].Field<string>("dtKeyFields").Trim().Replace(",", "_");
			if (text.Trim().Length != 0)
			{
				flag2 = false;
				if (flag)
				{
					flag2 = indexesForTable.ContainsKey(text);
				}
				if (!flag2)
				{
					string queryString = "CREATE UNIQUE INDEX " + text + " ON " + table + " (" + dataTable.Rows[0].Field<string>("dtKeyFields").Trim() + ")";
					try
					{
						serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, queryString);
						changes?.Add("Primary index on table " + table + " was created");
					}
					catch (Exception ex)
					{
						if (ex.Message.Contains("CREATE UNIQUE INDEX statement terminated because a duplicate key was found"))
						{
							RemoveDuplicates(sqlConnection, m1User, m1DataDictionary, databaseName, table, messages);
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, queryString);
						}
					}
				}
			}
			text2 = dataTable.Rows[0].Field<string>("dtUniqueField").Trim().Replace(",", "_");
			if (text2.Trim().Length != 0 && text != text2)
			{
				flag2 = false;
				if (flag)
				{
					flag2 = indexesForTable.ContainsKey(text2);
				}
				if (!flag2)
				{
					string queryString2 = "CREATE UNIQUE INDEX " + text2 + " ON " + table + " (" + dataTable.Rows[0].Field<string>("dtUniqueField").Trim() + ")";
					try
					{
						serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, queryString2);
						changes?.Add("Unique ID index on table " + table + " was created");
					}
					catch (Exception ex2)
					{
						if (ex2.Message.Contains("CREATE UNIQUE INDEX statement terminated because a duplicate key was found"))
						{
							RemoveDuplicates(sqlConnection, m1User, m1DataDictionary, databaseName, table, messages);
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, queryString2);
						}
					}
				}
			}
		}
		if (flag)
		{
			foreach (KeyValuePair<string, IndexInfo> item in indexesForTable)
			{
				if (item.Value.Columns.Count > 1 && !item.Value.Name.Equals(text, StringComparison.CurrentCultureIgnoreCase) && !item.Value.Name.Equals(text2, StringComparison.CurrentCultureIgnoreCase) && !item.Value.AutoCreated)
				{
					serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "DROP INDEX " + table + "." + item.Value.Name);
					changes?.Add("Index " + item.Value.Name + " on table " + table + " was dropped");
				}
			}
		}
		dataTable = m1DataDictionary.GetDataTable("select dfField,dfIndexed from DDFields where dfTable = " + table.Trim().ToUpper().ToSql());
		foreach (DataRow row in dataTable.Rows)
		{
			if (row.Field<bool>("dfIndexed"))
			{
				string text3 = row.Field<string>("dfField").Trim();
				flag2 = false;
				if (flag)
				{
					flag2 = indexesForTable.ContainsKey(text3);
				}
				if (!flag2 && text3.Trim().ToUpper() != text.Trim().ToUpper() && text3.Trim().ToUpper() != text2.Trim().ToUpper() && DoesFieldExist(sqlConnection, m1User, databaseName, table, text3))
				{
					CreateIndex(sqlConnection, m1User, m1DataDictionary, databaseName, table, "CREATE INDEX " + text3 + " ON " + table + " (" + row.Field<string>("dfField").Trim() + ")", messages);
					changes?.Add("Index " + text3 + " on table " + table + " was created");
				}
				continue;
			}
			flag2 = false;
			if (!flag)
			{
				continue;
			}
			string text4 = row.Field<string>("dfField").Trim();
			foreach (KeyValuePair<string, IndexInfo> item2 in indexesForTable)
			{
				if (item2.Value.Columns.Count == 1 && item2.Value.Columns[0].Equals(text4, StringComparison.CurrentCultureIgnoreCase))
				{
					flag2 = true;
					if (!text4.Equals(text, StringComparison.CurrentCultureIgnoreCase) && !text4.Equals(text2, StringComparison.CurrentCultureIgnoreCase) && !item2.Value.AutoCreated)
					{
						serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "DROP INDEX " + table + "." + item2.Value.Name);
						changes?.Add("Index " + item2.Value.Name + " on table " + table + " was dropped");
					}
				}
			}
		}
	}

	public void VerifyTable(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string table, List<string> messages, List<string> changes)
	{
		if (changes == null)
		{
			changes = new List<string>();
		}
		table = table.Trim();
		string text = table.ToUpper();
		bool flag = false;
		string text2 = string.Empty;
		bool flag2 = false;
		_ = string.Empty;
		bool flag3 = false;
		DataTable dataTable = m1DataDictionary.GetDataTable("select dtSQLView,IsNull(dtViewDef,'') As dtViewDef from DDTables Where dtTable = " + text.ToSql());
		if (dataTable.Rows.Count > 0)
		{
			flag = dataTable.Rows[0].Field<bool>("dtSQLView");
			text2 = dataTable.Rows[0].Field<string>("dtViewDef");
		}
		DataTable dataTable2 = m1DataDictionary.GetDataTable("select * from DDFields where dfTable=" + text.ToSql() + " order by dfSequence");
		if (flag)
		{
			if (text2.Length <= 0)
			{
				return;
			}
			DataTable dataTable3 = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "exec sp_tables " + text.ToSql());
			if (dataTable3.Rows.Count > 0)
			{
				flag2 = true;
				if (dataTable3.Rows[0].Field<string>("Table_Owner").Trim().ToUpper() != "DBO" && dataTable3.Rows[0].Field<string>("Table_Owner").Trim().Length > 0)
				{
					_ = "Table " + table + " has " + dataTable3.Rows[0].Field<string>("Table_Owner").Trim() + " defined as the owner. M1 was unable to change the owner to dbo, which is required for M1 to access the table correctly.";
					serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "exec sp_changeobjectowner '" + dataTable3.Rows[0].Field<string>("Table_Owner").Trim() + "." + text + "','dbo'");
					_ = string.Empty;
				}
			}
			else
			{
				flag2 = false;
			}
			if (flag2)
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "DROP VIEW " + table);
			}
			if (!text2.Contains("CREATE VIEW "))
			{
				text2 = "CREATE VIEW dbo." + table + " AS " + text2;
			}
			serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, text2);
			return;
		}
		if (dataTable2.Rows.Count == 0)
		{
			if (DoesTableExist(sqlConnection, m1User, databaseName, text))
			{
				DropTable(sqlConnection, m1User, databaseName, text);
				changes.Add("Table " + table + " was dropped");
			}
			return;
		}
		DataTable dataTable4 = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "exec sp_tables '" + text + "'");
		if (dataTable4.Rows.Count > 0)
		{
			flag2 = true;
			if (dataTable4.Rows[0].Field<string>("Table_Owner").Trim().ToUpper() != "DBO" && dataTable4.Rows[0].Field<string>("Table_Owner").Trim().Length > 0)
			{
				_ = "Table " + table + " has " + dataTable4.Rows[0].Field<string>("Table_Owner").Trim() + " defined as the owner. M1 was unable to change the owner to dbo, which is required for M1 to access the table correctly.";
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "exec sp_changeobjectowner '" + dataTable4.Rows[0].Field<string>("Table_Owner").Trim() + "." + text + "','dbo'");
				_ = string.Empty;
			}
		}
		else
		{
			flag2 = false;
		}
		if (!flag2)
		{
			CreateTable(sqlConnection, m1User, m1DataDictionary, databaseName, table);
			changes.Add("Table " + table + " was created");
			return;
		}
		SqlCommand sqlCommand = serverManager.NewSqlCommand(sqlConnection, m1User, databaseName, "Select dobj.name as Table_Name,col.name as Column_Name,sys.types.name as Type_Name,col.max_length,col.precision,col.scale,col.is_nullable as nullable,col.is_identity From sys.columns col Inner Join sys.objects dobj on dobj.object_id = col.object_id Inner Join sys.types on col.user_type_id = sys.types.user_type_id Where col.object_id = object_id(@TableName)");
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = text;
		SqlDataAdapter adapter;
		DataTable dataTable5 = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, sqlCommand, fillSchema: false, out adapter, null);
		if (dataTable5.Rows.Count > 0)
		{
			bool flag4 = false;
			string empty = string.Empty;
			string empty2 = string.Empty;
			Dictionary<string, string> defaultConstraints = GetDefaultConstraints(sqlConnection, m1User, databaseName, text);
			flag4 = defaultConstraints.Count != 0;
			foreach (DataRow row in dataTable2.Rows)
			{
				empty = row.Field<string>("dfField").Trim();
				empty2 = M1DataDictionary.GetDisplayName(row.Field<string>("dfField"), row.Field<string>("dfDisplayName"));
				DataRow[] array = dataTable5.Select("column_name = " + empty.ToLinq());
				if (array.Length != 0)
				{
					string text3 = array[0].Field<string>("type_name").Trim().ToUpper();
					bool flag5 = array[0].Field<bool>("nullable") != row.Field<bool>("dfAllowNulls");
					int num = array[0].Field<short>("max_length");
					if (num > 0 && text3.StartsWith("N", StringComparison.CurrentCultureIgnoreCase))
					{
						num /= 2;
					}
					if (!flag5)
					{
						flag5 = row.Field<string>("dfDBType").ToLower() switch
						{
							"varchar(max)" => text3 != "VARCHAR", 
							"nvarchar(max)" => text3 != "NVARCHAR", 
							"identity" => text3 != "INT" || !array[0].Field<bool>("is_identity"), 
							"date" => text3 != "DATETIME", 
							_ => !row.Field<string>("dfDBType").Equals(text3, StringComparison.CurrentCultureIgnoreCase), 
						};
					}
					if (!flag5)
					{
						switch (row.Field<string>("dfDBType").ToLower())
						{
						case "varchar(max)":
						case "nvarchar(max)":
							if (num != -1)
							{
								flag5 = true;
							}
							break;
						case "varchar":
						case "char":
						case "nchar":
						case "nvarchar":
							if (num != row.Field<byte>("dfLength"))
							{
								flag5 = true;
							}
							break;
						case "numeric":
							if (array[0].Field<byte>("precision") != row.Field<byte>("dfLength"))
							{
								flag5 = true;
							}
							else if (array[0].Field<byte>("scale") != row.Field<byte>("dfDecimals"))
							{
								flag5 = true;
							}
							break;
						}
					}
					if (flag5)
					{
						AlterColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, empty2, row.Field<string>("dfDBType"), row.Field<byte>("dfLength"), row.Field<byte>("dfDecimals"), row.Field<bool>("dfAllowNulls"), messages);
						changes.Add("Field " + empty2 + " in table " + table + " was altered");
						if (!defaultConstraints.ContainsKey(empty.Trim()))
						{
							defaultConstraints.Add(empty.Trim(), string.Empty);
						}
					}
					if (row.Field<string>("dfDBType").Equals("identity", StringComparison.CurrentCultureIgnoreCase) || row.Field<string>("dfDBType").Equals("timestamp", StringComparison.CurrentCultureIgnoreCase))
					{
						continue;
					}
					bool flag6 = false;
					if (flag4)
					{
						flag6 = defaultConstraints.ContainsKey(empty.Trim());
					}
					if (!flag6)
					{
						serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "Alter Table [" + table + "] Alter Column [" + empty + "] " + getSQLServerType(row.Field<string>("dfdbtype"), row.Field<byte>("dfLength"), row.Field<byte>("dfDecimals")) + getAllowNullForType(row.Field<string>("dfdbtype"), row.Field<bool>("dfallownulls")));
						string defaultForType = getDefaultForType(row.Field<string>("dfdbtype"), row.Field<bool>("dfAllowNulls"));
						if (!string.IsNullOrWhiteSpace(defaultForType))
						{
							string text4 = Guid.NewGuid().ToString();
							text4 = text4.Replace("{", string.Empty);
							text4 = text4.Replace("}", string.Empty);
							text4 = text4.Replace("-", string.Empty);
							text4 = ("DF__" + table.Substring(0, Math.Min(table.Length, 9)) + "_" + text4).Substring(0, 30);
							serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "Alter Table [" + table + "] Add Constraint [" + text4 + "] " + defaultForType + " for [" + empty + "]");
							changes.Add("Field " + empty + " in table " + table + " had a default constraint added");
						}
					}
				}
				else
				{
					AddColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, empty2, row.Field<string>("dfDBType"), row.Field<byte>("dfLength"), row.Field<byte>("dfDecimals"), verifyIndexes: true, dropTriggers: true, row.Field<bool>("dfAllowNulls"), messages);
					changes.Add("Field " + empty2 + " in table " + table + " was added");
				}
			}
			if (dataTable2.Rows.Count > 0)
			{
				foreach (DataRow row2 in dataTable5.Rows)
				{
					empty = row2.Field<string>("column_name").Trim().ToUpper();
					if (dataTable2.Select("dfField = " + empty.ToLinq()).Length == 0)
					{
						DropColumn(sqlConnection, m1User, m1DataDictionary, databaseName, table, empty, dropTriggers: true);
						changes.Add("Field " + empty + " in table " + table + " was dropped");
					}
				}
			}
		}
		if (dataTable2.Rows.Count > 0)
		{
			VerifyIndexesOnTable(sqlConnection, m1User, m1DataDictionary, databaseName, table, messages, changes);
		}
		VerifyLogTriggersForTable(sqlConnection, m1User, m1DataDictionary, databaseName, table);
	}

	public void VerifyDatabase(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, List<string> messages, List<string> changes)
	{
		VerifyDatabase(sqlConnection, m1User, m1DataDictionary, databaseName, messages, changes, null);
	}

	public void VerifyDatabase(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, List<string> messages, List<string> changes, VerifyDelegate func)
	{
		DataTable dataTable = m1DataDictionary.GetDataTable("select Case When dtDisplayName = '' Then dtTable Else dtDisplayName End As dtDisplayName from DDTables Order by dtTable");
		if (dataTable.Rows.Count <= 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			func?.Invoke(row.Field<string>("dtDisplayName").Trim());
			VerifyTable(sqlConnection, m1User, m1DataDictionary, databaseName, row.Field<string>("dtDisplayName").Trim(), messages, changes);
		}
		func?.Invoke(string.Empty);
	}

	public void ShrinkDatabase(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		ShrinkDatabase(sqlConnection, m1User, databaseName, logFileOnly: false);
	}

	public void ShrinkDatabase(SqlConnection sqlConnection, M1User m1User, string databaseName, bool logFileOnly)
	{
		string text = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		string queryString = $"SELECT DATABASEPROPERTYEX('{databaseName}', 'RECOVERY') as CurrentSetting";
		foreach (DataRow row2 in serverManager.GetDataTable(sqlConnection, m1User, "master", 0, queryString).Rows)
		{
			text = row2.Field<string>("CurrentSetting");
		}
		serverManager.ExecuteCommand(sqlConnection, m1User, "master", "ALTER DATABASE " + databaseName + " Set RECOVERY SIMPLE");
		foreach (DataRow row3 in serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "sp_helpfile").Rows)
		{
			stringBuilder2.Length = 0;
			stringBuilder.Length = 0;
			stringBuilder2.Append(row3.Field<string>("filename"));
			stringBuilder.Append(row3.Field<string>("name"));
			if (!logFileOnly || !stringBuilder2.ToString().ToLower().Trim()
				.EndsWith(".mdf"))
			{
				serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "dbcc shrinkfile('" + stringBuilder.ToString().Trim() + "',5,'TRUNCATEONLY')");
			}
		}
		serverManager.ExecuteCommand(sqlConnection, m1User, "master", "ALTER DATABASE " + databaseName + " Set RECOVERY " + text.ToUpper());
	}

	public void DetachDatabase(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		if (databaseName.Length != 0)
		{
			serverManager.ClearAllPools();
			serverManager.ExecuteCommand(sqlConnection, m1User, "master", "sp_detach_db @dbname = '" + databaseName + "'");
			return;
		}
		throw new ArgumentException("Database name cannot be empty.");
	}

	public void AttachDatabase(SqlConnection sqlConnection, M1User m1User, string databaseName, string destDatabase, string destLog, bool detachExisting)
	{
		if (databaseName.Length != 0)
		{
			if (DoesDatabaseExist(sqlConnection, m1User, databaseName))
			{
				if (!detachExisting)
				{
					throw new M1Exception("There is already a database named " + databaseName + " attached to the SQL Server.");
				}
				DetachDatabase(sqlConnection, m1User, databaseName);
			}
			FileAttributes fileAttributes = File.GetAttributes(destDatabase);
			if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				fileAttributes &= ~FileAttributes.ReadOnly;
				File.SetAttributes(destDatabase, fileAttributes);
			}
			if ((fileAttributes & FileAttributes.Hidden) == FileAttributes.Hidden)
			{
				fileAttributes &= ~FileAttributes.Hidden;
				File.SetAttributes(destDatabase, fileAttributes);
			}
			fileAttributes = File.GetAttributes(destLog);
			if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				fileAttributes &= ~FileAttributes.ReadOnly;
				File.SetAttributes(destLog, fileAttributes);
			}
			if ((fileAttributes & FileAttributes.Hidden) == FileAttributes.Hidden)
			{
				fileAttributes &= ~FileAttributes.Hidden;
				File.SetAttributes(destLog, fileAttributes);
			}
			serverManager.ExecuteCommand(sqlConnection, m1User, "master", "sp_attach_db @dbname = " + databaseName.ToSql() + ",@filename1 = " + destDatabase.ToSql() + ",@filename2 = " + destLog.ToSql());
			SetCompatibilityLevel(sqlConnection, m1User, databaseName);
			return;
		}
		throw new ArgumentException("Database name cannot be empty.");
	}

	public void DeleteDatabase(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName)
	{
		if (databaseName.Length != 0)
		{
			serverManager.ClearPool(m1User, databaseName);
			if (serverManager.ExecuteCommand(sqlConnection, m1User, "master", "DROP DATABASE " + databaseName) == -1 && !DoesDatabaseExist(sqlConnection, m1User, databaseName))
			{
				m1DataDictionary.DeleteSecurityForDatabase(databaseName);
			}
			return;
		}
		throw new ArgumentException("Database name cannot be empty.");
	}

	public void RelocateDatabases(M1User m1User, string newFolder)
	{
		if (newFolder.Length != 0)
		{
			newFolder = newFolder.AddBackslash();
			currentContext.DDServerManager.ClearAllPools();
			currentContext.DBServerManager.ClearAllPools();
			relocateProcessList(m1User, currentContext.InstalledDatabases, newFolder);
			relocateProcessList(m1User, currentContext.InstalledDataDictionaries, newFolder);
			return;
		}
		throw new ArgumentException("New Folder cannot be empty.");
	}

	private void relocateProcessList(M1User m1User, DatabaseInfoCollection dbList, string newFolder)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		int num = 0;
		ServerFileSystem serverFileSystem = new ServerFileSystem(serverManager);
		foreach (DatabaseInfo db in dbList)
		{
			empty = db.Name;
			num = 0;
			empty4 = string.Empty;
			DataTable dataTable = serverManager.GetDataTable(null, m1User, empty, 0, "sp_helpfile");
			serverManager.ClearAllPools();
			serverManager.ExecuteCommand(null, m1User, "master", "EXEC sp_detach_db @dbname = " + empty.ToSql());
			foreach (DataRow row in dataTable.Rows)
			{
				empty2 = row.Field<string>("filename");
				if (empty2.Length == 0)
				{
					continue;
				}
				num++;
				if (!serverFileSystem.FileExists(empty2))
				{
					continue;
				}
				empty3 = Path.GetFileName(empty2);
				empty3 = newFolder + empty3;
				empty4 = empty4 + ", @filename" + num + " = " + empty3.ToSql();
				if (!empty2.Equals(empty3, StringComparison.CurrentCultureIgnoreCase))
				{
					if (serverFileSystem.FileExists(empty3))
					{
						serverFileSystem.DeleteFile(empty3);
					}
					serverFileSystem.MoveFile(empty2, empty3);
				}
			}
			serverManager.ExecuteCommand(null, m1User, "master", "EXEC sp_attach_db @dbname = " + empty.ToSql() + empty4);
		}
	}

	public bool DoesFieldExist(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName, string fieldName)
	{
		return serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "exec sp_columns @table_name=" + tableName.Trim().ToLower().ToSql() + ", @column_name=" + fieldName.Trim().ToLower().ToSql()).Rows.Count != 0;
	}

	public bool DoesViewExist(SqlConnection sqlConnection, M1User m1User, string databaseName, string viewName)
	{
		return serverManager.DoesViewExist(sqlConnection, m1User, databaseName, viewName, null);
	}

	public virtual bool DoesTableExist(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName)
	{
		return DoesTableExist(sqlConnection, m1User, databaseName, tableName, null);
	}

	public virtual bool DoesTableExist(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName, SqlTransaction transaction)
	{
		return serverManager.DoesTableExist(sqlConnection, m1User, databaseName, tableName, null);
	}

	public bool DoesDatabaseExist(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		return serverManager.DoesDatabaseExist(sqlConnection, m1User, databaseName);
	}

	public void RenameDatabase(SqlConnection sqlConnection, M1User m1User, string oldName, string newName)
	{
		SetConfigure();
		serverManager.ClearAllPools();
		serverManager.ExecuteCommand(sqlConnection, m1User, "master", "ALTER DATABASE " + oldName.ToString() + " Set SINGLE_USER WITH ROLLBACK IMMEDIATE");
		serverManager.ExecuteCommand(sqlConnection, m1User, "master", "sp_rename " + oldName.ToSql() + ", " + newName.ToSql() + ", 'DATABASE'");
		serverManager.ExecuteCommand(sqlConnection, m1User, "master", "ALTER DATABASE " + newName.ToString() + " Set MULTI_USER");
		DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, newName, 0, "sp_helpfile");
		int num = 0;
		string text = string.Empty;
		serverManager.ClearAllPools();
		serverManager.ExecuteCommand(sqlConnection, m1User, "master", "EXEC sp_detach_db @dbname = " + newName.ToSql());
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		ServerFileSystem serverFileSystem = new ServerFileSystem(serverManager);
		foreach (DataRow row in dataTable.Rows)
		{
			empty = row.Field<string>("filename").Trim();
			if (empty.Length == 0)
			{
				continue;
			}
			num++;
			empty3 = Path.GetFileName(empty).Replace(oldName, newName, caseInsensitive: true);
			empty2 = Path.GetDirectoryName(empty).AddBackslash() + empty3;
			text = text + ", @filename" + num + " = " + empty2.ToSql();
			if (!empty.Equals(empty2, StringComparison.CurrentCultureIgnoreCase))
			{
				if (serverFileSystem.FileExists(empty2))
				{
					serverFileSystem.DeleteFile(empty2);
				}
				serverFileSystem.MoveFile(empty, empty2);
			}
		}
		serverManager.ExecuteCommand(sqlConnection, m1User, "master", "EXEC sp_attach_db @dbname = " + newName.ToSql() + text);
		if (oldName.StartsWith("M1_", StringComparison.CurrentCultureIgnoreCase))
		{
			m1User.DataDictionary.Users.RenameDatabaseSecurityUpdate(oldName, newName);
		}
	}

	public void SetConfigure()
	{
		bool flag = false;
		int num = 0;
		DataTable dataTable = serverManager.GetDataTable(null, null, "msdb", 0, "EXEC sp_configure 'show advanced options'");
		if (dataTable.Rows.Count > 0 && dataTable.Rows[0].Field<int>("run_value") == 1)
		{
			flag = true;
		}
		if (!flag)
		{
			serverManager.ExecuteCommand(null, null, "msdb", "sp_configure 'show advanced options', 1");
			serverManager.ExecuteCommand(null, null, "msdb", "EXEC('RECONFIGURE')");
		}
		dataTable = serverManager.GetDataTable(null, null, "msdb", 0, "EXEC sp_configure 'xp_cmdshell'");
		if (dataTable.Rows.Count > 0)
		{
			num = dataTable.Rows[0].Field<int>("run_value");
		}
		if (num == 0)
		{
			serverManager.ExecuteCommand(null, null, "msdb", "sp_configure 'xp_cmdshell', 1");
			serverManager.ExecuteCommand(null, null, "msdb", "EXEC('RECONFIGURE')");
		}
	}

	public string GetCurrentFieldType(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName, string fieldName, ref bool isNullable)
	{
		DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "Select dobj.name as Table_Name,col.name as Column_Name,sys.types.name as Type_Name,col.max_length,col.precision,col.scale,col.is_nullable as nullable From sys.columns col Inner Join sys.objects dobj on dobj.object_id = col.object_id Inner Join sys.types on col.user_type_id = sys.types.user_type_id Where col.object_id = object_id(" + tableName.ToSql() + ") And col.name = " + fieldName.ToSql());
		if (dataTable.Rows.Count > 0)
		{
			isNullable = dataTable.Rows[0].Field<bool>("nullable");
			return dataTable.Rows[0].Field<string>("type_name").Trim().ToLower();
		}
		isNullable = false;
		return string.Empty;
	}

	private string getSQLServerType(string fieldType, int fieldPrecision, int fieldScale)
	{
		return fieldType.Trim().ToLower() switch
		{
			"char" => " char(" + fieldPrecision + ") ", 
			"varchar" => " varchar(" + fieldPrecision + ") ", 
			"nvarchar" => " nvarchar(" + fieldPrecision + ") ", 
			"date" => " datetime ", 
			"numeric" => " numeric(" + fieldPrecision + "," + fieldScale + ") ", 
			"int" => " int ", 
			"identity" => " int ", 
			_ => " " + fieldType, 
		};
	}

	private string getDefaultForType(string fieldType, bool isNullable)
	{
		if (isNullable)
		{
			return string.Empty;
		}
		switch (fieldType.Trim().ToLower())
		{
		case "char":
		case "text":
		case "varchar":
		case "ntext":
		case "nchar":
		case "nvarchar":
		case "varchar(max)":
		case "nvarchar(max)":
			return " DEFAULT('')";
		case "real":
		case "numeric":
		case "tinyint":
		case "money":
		case "float":
		case "smallint":
		case "bit":
		case "int":
		case "bigint":
		case "smallmoney":
			return " DEFAULT 0";
		case "identity":
			return " IDENTITY(1,1)";
		case "uniqueidentifier":
			return " DEFAULT(NEWID())";
		default:
			return string.Empty;
		}
	}

	public int GetCurrentFieldLength(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName, string fieldName)
	{
		DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "exec sp_columns @table_name=" + tableName.Trim().ToLower().ToSql() + ", @column_name=" + fieldName.Trim().ToLower().ToSql());
		if (dataTable.Rows.Count > 0)
		{
			return dataTable.Rows[0].Field<int>("precision");
		}
		return 0;
	}

	public short GetCurrentFieldScale(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName, string fieldName)
	{
		DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "exec sp_columns @table_name=" + tableName.Trim().ToLower().ToSql() + ", @column_name=" + fieldName.Trim().ToLower().ToSql());
		if (dataTable.Rows.Count > 0)
		{
			return dataTable.Rows[0].Field<short>("scale");
		}
		return 0;
	}

	private string getAllowNullForType(string fieldType, bool isNullable)
	{
		if (isNullable)
		{
			return " NULL ";
		}
		string empty = string.Empty;
		switch (fieldType.Trim().ToLower())
		{
		case "date":
		case "datetime":
		case "smalldatetime":
		case "image":
		case "binary":
		case "varbinary":
			return " NULL ";
		default:
			return " NOT NULL ";
		}
	}

	public void DropTable(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName)
	{
		if (DoesTableExist(sqlConnection, m1User, databaseName, tableName))
		{
			serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "Drop Table " + databaseName + ".dbo." + tableName);
		}
	}

	public void DropTableEx(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName)
	{
		DropTable(sqlConnection, m1User, databaseName, tableName);
	}

	public void RenameColumn(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, string currentName, string newName, bool dropTriggers)
	{
		if (DoesFieldExist(sqlConnection, m1User, databaseName, tableName, currentName))
		{
			bool flag = false;
			if (dropTriggers)
			{
				flag = DropLogTriggersForTable(sqlConnection, m1User, databaseName, tableName);
			}
			serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "EXEC sp_rename '" + tableName + "." + currentName + "', '" + newName + "', 'COLUMN'");
			if (flag)
			{
				VerifyLogTriggersForTable(sqlConnection, m1User, m1DataDictionary, databaseName, tableName);
			}
		}
	}

	public void RenameColumnEx(SqlConnection sqlConnection, M1User m1User, M1DataDictionary m1DataDictionary, string databaseName, string tableName, string currentName, string newName, bool dropTriggers)
	{
		RenameColumn(sqlConnection, m1User, m1DataDictionary, databaseName, tableName, currentName, newName, dropTriggers);
	}

	public void DropDefaultConstraint(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName, string fieldName)
	{
		Dictionary<string, string> defaultConstraints = GetDefaultConstraints(sqlConnection, m1User, databaseName, tableName);
		if (defaultConstraints.ContainsKey(fieldName))
		{
			serverManager.ExecuteCommand(sqlConnection, m1User, databaseName, "ALTER TABLE " + tableName + " DROP CONSTRAINT " + defaultConstraints[fieldName]);
		}
	}

	protected Dictionary<string, string> GetDefaultConstraints(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName)
	{
		string queryString = "select     col.name as col_name,      dobj.name as def_name from syscolumns col      inner join sysobjects dobj          on dobj.id = col.cdefault and OBJECTPROPERTY(col.cdefault, N'IsDefaultCnst') <> 0 where col.id = object_id(@tablename) ";
		SqlCommand sqlCommand = serverManager.NewSqlCommand(sqlConnection, m1User, databaseName, queryString);
		sqlCommand.Parameters.Add(new SqlParameter("@tablename", SqlDbType.NVarChar)).Value = tableName.Trim();
		SqlDataAdapter adapter;
		DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, sqlCommand, fillSchema: false, out adapter);
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
		foreach (DataRow row in dataTable.Rows)
		{
			dictionary.Add(row.Field<string>("col_name"), row.Field<string>("def_name"));
		}
		return dictionary;
	}

	public bool CheckAvailableSpace(SqlConnection sqlConnection, M1User m1User, string databaseName, List<string> errors)
	{
		bool result = true;
		DataTable dataTable = serverManager.GetDataTable(sqlConnection, m1User, databaseName, 0, "SELECT physical_name, size FROM sys.database_files");
		string text = string.Empty;
		double num = 0.0;
		foreach (DataRow row in dataTable.Rows)
		{
			if (string.IsNullOrEmpty(text))
			{
				text = Path.GetPathRoot(row.Field<string>("physical_name"));
			}
			num += (double)(row.Field<int>("size") * 8) / 1024.0;
		}
		double driveFreeSpaceInMB = GetDriveFreeSpaceInMB(serverManager.GetSqlServerName(sqlConnection, m1User, databaseName), text);
		double num2 = num * 2.0;
		if (serverManager.IsSQLExpress(sqlConnection, m1User, databaseName))
		{
			string[] array = ((string)serverManager.ExecuteScalar(sqlConnection, m1User, databaseName, "SELECT SERVERPROPERTY('productversion') as version")).Split('.');
			if (decimal.Compare(Convert.ToDecimal($"{array[0]}.{array[1]}"), 10.00m) > 0)
			{
				if (num2 > 10240.0)
				{
					result = false;
					errors.Add($"The upgraded database might exceed the database limit size for this SQL server version ({serverManager.GetSqlServerVersionString(sqlConnection, m1User, databaseName)}).");
				}
			}
			else if (num2 > 4096.0)
			{
				result = false;
				errors.Add($"The upgraded database might exceed the database limit size for this SQL server version ({serverManager.GetSqlServerVersionString(sqlConnection, m1User, databaseName)}).");
			}
		}
		if (num2 > driveFreeSpaceInMB)
		{
			result = false;
			ByPassUI byPassUI = (ByPassUI)currentContext.GetService(typeof(ByPassUI));
			string item = "There may not be enough space to complete the upgrade.";
			if (byPassUI == null)
			{
				errors.Add(item);
			}
			else
			{
				byPassUI.Output.Add(item);
			}
		}
		return result;
	}

	private double GetDriveFreeSpaceInMB(string sqlServer, string driveName)
	{
		try
		{
			ConnectionOptions options = new ConnectionOptions();
			if (string.IsNullOrEmpty(sqlServer))
			{
				sqlServer = ".";
			}
			ManagementScope scope = new ManagementScope($"\\\\{sqlServer}\\root\\cimv2", options);
			ObjectQuery query = new ObjectQuery("select FreeSpace,Name from Win32_LogicalDisk where DriveType=3");
			foreach (ManagementObject item in new ManagementObjectSearcher(scope, query).Get())
			{
				if ((item["Name"].ToString() + "\\").Equals(driveName, StringComparison.OrdinalIgnoreCase))
				{
					return Convert.ToDouble(item["FreeSpace"].ToString()) / 1048576.0;
				}
			}
			return -1.0;
		}
		catch
		{
			return -1.0;
		}
	}
}
