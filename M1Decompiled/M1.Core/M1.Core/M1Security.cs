using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using M1.Extensions;

namespace M1.Core;

public class M1Security
{
	private SecurityAccessLevel accessLevel;

	public List<string> ComponentRoles = new List<string>();

	public List<string> Groups = new List<string>();

	public string GroupWhere = string.Empty;

	private M1User currentUser;

	private M1Database currentDatabase;

	private M1DataDictionary currentDataDictionary;

	private AppContext currentContext;

	private Dictionary<string, SecurityAccessLevel> tableSecurityCache = new Dictionary<string, SecurityAccessLevel>();

	protected Dictionary<string, TableSecurityExpressions> RowFilters;

	private SqlCommand getGridAccessCommand;

	private SqlCommand getTableAccessCommand;

	private SqlCommand getFieldAccessCommand;

	private SqlCommand getObjectAccessCommand;

	public event EventHandler<RoleCheckEventArgs> RoleCheck;

	public M1Security(M1User associatedUser, M1Database associatedDatabase, M1DataDictionary associatedDataDictionary, AppContext context)
	{
		currentUser = associatedUser;
		currentDatabase = associatedDatabase;
		currentDataDictionary = associatedDataDictionary;
		currentContext = context;
		RowFilters = new Dictionary<string, TableSecurityExpressions>(StringComparer.CurrentCultureIgnoreCase);
	}

	protected TableSecurityExpressions GetTableExpressionDirect(string db, string table)
	{
		SqlCommand sqlCommand = currentDataDictionary.NewSqlCommand("Select dtRowFilter,dtEditExpression,dtAddExpression,dtDeleteExpression,dtChangeIDExpression From DDSecurityTables Where  (dtRowFilter Is Not Null  Or dtEditExpression Is Not Null  Or dtAddExpression Is Not Null  Or dtDeleteExpression Is Not Null  Or dtChangeIDExpression Is Not Null) and dtDataset = @DatabaseID and dtTable = @TableName and dtField = ''  and (dtUserID = @UserID Or dtUserID In (Select dzGroupID From DDSecurityGroups Inner Join DDUsers On dzGroupID = duUserID Where dzDataset = @DatabaseID And dzUserID = @UserID And duType = 1)) ");
		sqlCommand.Parameters.Add(new SqlParameter("@DatabaseID", SqlDbType.NVarChar)).Value = db;
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = table;
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = currentUser.ID;
		DataTable dataTable = currentDataDictionary.GetDataTable(sqlCommand);
		return new TableSecurityExpressions(rowDataBuilder(dataTable, "dtRowFilter"), rowDataBuilder(dataTable, "dtEditExpression"), rowDataBuilder(dataTable, "dtAddExpression"), rowDataBuilder(dataTable, "dtDeleteExpression"), rowDataBuilder(dataTable, "dtChangeIDExpression"));
	}

	public TableSecurityExpressions GetTableExpression(string db, string table)
	{
		if (!string.IsNullOrEmpty(table))
		{
			if (db.Equals(currentDatabase.ID, StringComparison.CurrentCultureIgnoreCase))
			{
				if (!RowFilters.ContainsKey(table))
				{
					RowFilters.Add(table, GetTableExpressionDirect(db, table));
				}
				return RowFilters[table];
			}
			return GetTableExpressionDirect(db, table);
		}
		return null;
	}

	public string GetRowFilter(string table)
	{
		TableSecurityExpressions tableExpression = GetTableExpression(currentDatabase.ID, table);
		if (tableExpression != null)
		{
			return tableExpression.View;
		}
		return string.Empty;
	}

	private string rowDataBuilder(DataTable data, string field)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		foreach (DataRow row in data.Rows)
		{
			string text = row.Field<string>(field);
			if (!string.IsNullOrEmpty(text))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(" Or ");
				}
				stringBuilder.Append("(" + text + ")");
				num++;
			}
		}
		if (num > 1)
		{
			stringBuilder.Insert(0, "(");
			stringBuilder.Append(")");
		}
		return stringBuilder.ToString();
	}

	public bool CanReportBeRun(string reportFolder)
	{
		if (GetReportAccessLevel(reportFolder, string.Empty) == SecurityAccessLevel.None)
		{
			return false;
		}
		foreach (FileInfo item in currentContext.Reports.GetReportsForTemplate(reportFolder, string.Empty))
		{
			if (GetReportAccessLevel(reportFolder, item.Name) != SecurityAccessLevel.None)
			{
				return true;
			}
		}
		return false;
	}

	public bool CanReportBeRun(string reportFolder, string reportName)
	{
		return GetReportAccessLevel(reportFolder, reportName) != SecurityAccessLevel.None;
	}

	public void ClearCache()
	{
		tableSecurityCache.Clear();
	}

	protected void OnRoleCheck(RoleCheckEventArgs e)
	{
		this.RoleCheck?.Invoke(this, e);
	}

	public bool IsInRole(string roleID)
	{
		bool flag = false;
		string[] array = roleID.Trim().ToUpper().Split(',');
		foreach (string text in array)
		{
			if (text.StartsWith("TABLE:", StringComparison.CurrentCultureIgnoreCase) && currentDatabase != null && currentDatabase.IsOpen)
			{
				string text2 = text.Substring(6);
				int num = text2.IndexOf(':');
				string accessType;
				if (num != -1)
				{
					accessType = text2.Substring(num + 1);
					text2 = text2.Substring(0, num);
				}
				else
				{
					accessType = "view";
				}
				flag = IsInRoleByTable(text2, accessType);
			}
			else if (text.StartsWith("Script:", StringComparison.CurrentCultureIgnoreCase) && currentDatabase != null && currentDatabase.IsOpen)
			{
				flag = Convert.ToBoolean(currentDatabase.ScriptingQuick.Eval(text.Substring(7)));
			}
			else if (text.StartsWith("CUSTOMMODULE:", StringComparison.CurrentCultureIgnoreCase))
			{
				flag = currentDataDictionary.ProductCode.IsCustomModulePurchased(text.Substring(13));
			}
			else
			{
				flag = text switch
				{
					"ADMINISTRATOR" => currentUser.Administrator || currentUser.ID.Equals("ADMIN", StringComparison.CurrentCultureIgnoreCase), 
					"DBADMINISTRATOR" => currentUser.DBAdministrator, 
					"DEVELOPER" => currentUser.Developer, 
					"GRIDDEVELOPER" => currentUser.GridDeveloper, 
					"CHANGEPASSWORD" => !currentUser.PasswordLocked, 
					_ => ComponentRoles.Contains(text, StringComparer.CurrentCultureIgnoreCase), 
				};
				if (flag)
				{
					RoleCheckEventArgs e = new RoleCheckEventArgs(text);
					OnRoleCheck(e);
					if (e.Cancel)
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				return true;
			}
		}
		return flag;
	}

	public bool IsInRoleByTable(string table, string accessType)
	{
		return IsAccessType(GetTableAccessLevel(table), accessType);
	}

	public bool IsInRoleByField(string table, string field, string accessType)
	{
		return IsAccessType(GetFieldAccessLevel(table, field), accessType);
	}

	public string GetComponentSecurityText(string componentID)
	{
		SqlCommand sqlCommand = currentDataDictionary.NewSqlCommand("Select duName From DDUsers Where duUserID = @RoleID And duType = 2");
		sqlCommand.Parameters.Add(new SqlParameter("@RoleID", SqlDbType.NVarChar)).Value = componentID;
		string text = (string)currentDataDictionary.ExecuteScalar(sqlCommand);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		return componentID;
	}

	public SecurityAccessLevel GetGridAccessLevel(string gridID)
	{
		SecurityAccessLevel securityAccessLevel = SecurityAccessLevel.Default;
		if (getGridAccessCommand == null)
		{
			getGridAccessCommand = currentDataDictionary.NewSqlCommand("select djTable from DDGrids Where djGridID= @GridID");
			getGridAccessCommand.Parameters.Add(new SqlParameter("@GridID", SqlDbType.NVarChar)).Value = gridID;
		}
		else
		{
			getGridAccessCommand.Parameters["@GridID"].Value = gridID;
		}
		string text = (string)currentDataDictionary.ExecuteScalar(getGridAccessCommand);
		if (text == null)
		{
			text = string.Empty;
		}
		securityAccessLevel = ((text.Length != 0) ? GetTableAccessLevel(text, SecurityAccessLevel.Default) : GetDatabaseAccessLevel(SecurityAccessLevel.Default));
		if (securityAccessLevel == SecurityAccessLevel.Default)
		{
			securityAccessLevel = SecurityAccessLevel.None;
		}
		return securityAccessLevel;
	}

	public bool IsAccessType(SecurityAccessLevel accessLevel, string accessType)
	{
		return IsAccessType((short)accessLevel, accessType);
	}

	public bool IsAccessType(short accessLevel, string accessType)
	{
		return accessType.Trim().ToLower() switch
		{
			"add" => (accessLevel & 8) != 0, 
			"edit" => (accessLevel & 4) != 0, 
			"view" => (accessLevel & 1) == 0, 
			"delete" => (accessLevel & 0x10) != 0, 
			"changeid" => (accessLevel & 0x20) != 0, 
			_ => throw new M1Exception("Invalid type specified in IsAccessType call."), 
		};
	}

	public SecurityAccessLevel GetReportAccessLevel(string folder, string reportName)
	{
		return GetReportAccessLevel(folder, reportName, currentDatabase.ID, currentUser.ID);
	}

	public SecurityAccessLevel GetReportAccessLevel(string folder, string reportName, string databaseToCheck, string userToCheck)
	{
		SqlCommand sqlCommand = currentDataDictionary.NewSqlCommand("Select Top 1 drLevel, Case When drUserID = @UserID Then Case When drReport = '' Then 3 Else 1 End Else Case When drReport = '' Then 4 Else 2 End End As SecurityType From DDSecurityReports Where drLevel <> 0  And drDataset = @DatabaseID And drFolder = @Folder And (drReport = @Report      Or drReport = '') And (drUserID = @UserID Or drUserID In (Select dzGroupID From DDSecurityGroups Inner Join DDUsers On dzGroupID = duUserID Where dzDataset = @DatabaseID And dzUserID = @UserID And duType = 1)) Order By SecurityType,drLevel DESC");
		sqlCommand.Parameters.Add(new SqlParameter("@DatabaseID", SqlDbType.NVarChar)).Value = databaseToCheck;
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userToCheck;
		sqlCommand.Parameters.Add(new SqlParameter("@Folder", SqlDbType.NVarChar)).Value = folder;
		sqlCommand.Parameters.Add(new SqlParameter("@Report", SqlDbType.NVarChar)).Value = Path.GetFileNameWithoutExtension(reportName);
		object obj = currentDataDictionary.ExecuteScalar(sqlCommand);
		if (obj == null)
		{
			return GetDatabaseAccessLevel(SecurityAccessLevel.None, databaseToCheck, userToCheck);
		}
		return (SecurityAccessLevel)obj;
	}

	public SecurityAccessLevel GetFormAccessLevel(string formID)
	{
		SecurityAccessLevel securityAccessLevel = SecurityAccessLevel.Default;
		SqlCommand sqlCommand = currentDataDictionary.NewSqlCommand("select dmTable from DDForms Where dmFormID = @FormID");
		sqlCommand.Parameters.Add(new SqlParameter("@FormID", SqlDbType.NVarChar)).Value = formID;
		string text = (string)currentDataDictionary.ExecuteScalar(sqlCommand);
		if (text == null)
		{
			text = string.Empty;
		}
		securityAccessLevel = ((text.Length != 0) ? GetTableAccessLevel(text, SecurityAccessLevel.Default) : GetDatabaseAccessLevel(SecurityAccessLevel.Default));
		if (securityAccessLevel == SecurityAccessLevel.Default)
		{
			securityAccessLevel = (SecurityAccessLevel)28;
		}
		return securityAccessLevel;
	}

	public SecurityAccessLevel GetModuleAccessLevel(string module)
	{
		string text = string.Empty;
		module = module.Trim();
		if (currentDataDictionary.Modules.Contains(module))
		{
			text = currentDataDictionary.Modules[module].SecurityTables;
		}
		if (text.Length == 0)
		{
			return GetDatabaseAccessLevel((SecurityAccessLevel)28);
		}
		SecurityAccessLevel securityAccessLevel = SecurityAccessLevel.None;
		SecurityAccessLevel securityAccessLevel2 = SecurityAccessLevel.None;
		string[] array = text.Split(',');
		foreach (string table in array)
		{
			securityAccessLevel = GetTableAccessLevel(table);
			if ((int)securityAccessLevel > (int)securityAccessLevel2)
			{
				securityAccessLevel2 = securityAccessLevel;
			}
		}
		return securityAccessLevel2;
	}

	public SecurityAccessLevel GetDatabaseAccessLevel(SecurityAccessLevel defaultIfNotSet, string databaseToCheck)
	{
		return GetDatabaseAccessLevel(defaultIfNotSet, databaseToCheck, currentUser.ID);
	}

	public SecurityAccessLevel GetDatabaseAccessLevel(SecurityAccessLevel defaultIfNotSet, string databaseToCheck, string userToCheck)
	{
		if (currentDatabase.ID.Equals(databaseToCheck, StringComparison.CurrentCultureIgnoreCase) && currentUser.ID.Equals(userToCheck, StringComparison.CurrentCultureIgnoreCase))
		{
			return GetDatabaseAccessLevel(defaultIfNotSet);
		}
		SqlCommand sqlCommand = currentDataDictionary.NewSqlCommand("Select Top 1 dtLevel, Case When dtUserID = @UserID Then 1 Else 2 End As SecurityType From DDSecurityTables Where  dtLevel <> 0  and dtDataset = @DatabaseID and dtTable = '' and dtField = ''  and (dtUserID = @UserID Or dtUserID In (Select dzGroupID From DDSecurityGroups Inner Join DDUsers On dzGroupID = duUserID Where dzDataset = @DatabaseID And dzUserID = @UserID And duType = 1))  Order By SecurityType,dtLevel DESC");
		sqlCommand.Parameters.Add(new SqlParameter("@DatabaseID", SqlDbType.NVarChar)).Value = databaseToCheck;
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userToCheck;
		object obj = currentDataDictionary.ExecuteScalar(sqlCommand);
		if (obj == null)
		{
			return defaultIfNotSet;
		}
		return (SecurityAccessLevel)obj;
	}

	public SecurityAccessLevel GetDatabaseAccessLevel(SecurityAccessLevel defaultIfNotSet)
	{
		if (accessLevel == SecurityAccessLevel.Default)
		{
			return defaultIfNotSet;
		}
		return accessLevel;
	}

	public SecurityAccessLevel GetTableAccessLevel(string table)
	{
		return GetTableAccessLevel(table, SecurityAccessLevel.None);
	}

	public SecurityAccessLevel GetTableAccessLevel(string table, SecurityAccessLevel defaultIfNotSet)
	{
		return GetTableAccessLevel(table, defaultIfNotSet, currentDatabase.ID, currentUser.ID);
	}

	public SecurityAccessLevel GetTableAccessLevel(string table, SecurityAccessLevel defaultIfNotSet, string databaseToCheck, string userToCheck)
	{
		table = table.Trim().ToUpper();
		if (currentDatabase.ID.Equals(databaseToCheck, StringComparison.CurrentCultureIgnoreCase) && currentUser.ID.Equals(userToCheck, StringComparison.CurrentCultureIgnoreCase) && tableSecurityCache.ContainsKey(table))
		{
			if (tableSecurityCache[table] == SecurityAccessLevel.Default)
			{
				return defaultIfNotSet;
			}
			return tableSecurityCache[table];
		}
		if (getTableAccessCommand == null)
		{
			getTableAccessCommand = currentDataDictionary.NewSqlCommand("Select Top 1 dtLevel, Case When dtUserID = @UserID Then Case When dtTable = '' Then 3 Else 1 End Else Case When dtTable = '' Then 4 Else 2 End End As SecurityType From DDSecurityTables Where  dtLevel <> 0  and dtDataset = @DatabaseID and (dtTable = '' Or dtTable = @TableName) and dtField = ''  and (dtUserID = @UserID Or dtUserID In (Select dzGroupID From DDSecurityGroups Inner Join DDUsers On dzGroupID = duUserID Where dzDataset = @DatabaseID And dzUserID = @UserID And duType = 1))  Order By SecurityType,dtLevel DESC");
			getTableAccessCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userToCheck;
			getTableAccessCommand.Parameters.Add(new SqlParameter("@DatabaseID", SqlDbType.NVarChar)).Value = databaseToCheck;
			getTableAccessCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = table;
		}
		else
		{
			getTableAccessCommand.Parameters["@UserID"].Value = userToCheck;
			getTableAccessCommand.Parameters["@DatabaseID"].Value = databaseToCheck;
			getTableAccessCommand.Parameters["@TableName"].Value = table;
		}
		object obj = currentDataDictionary.ExecuteScalar(getTableAccessCommand);
		SecurityAccessLevel securityAccessLevel = ((obj != null) ? ((SecurityAccessLevel)obj) : defaultIfNotSet);
		if (currentDatabase.ID.Equals(databaseToCheck, StringComparison.CurrentCultureIgnoreCase) && currentUser.ID.Equals(userToCheck, StringComparison.CurrentCultureIgnoreCase))
		{
			if (accessLevel != SecurityAccessLevel.Default && (int)accessLevel < (int)securityAccessLevel && accessLevel == SecurityAccessLevel.View)
			{
				securityAccessLevel = accessLevel;
			}
			if (!tableSecurityCache.ContainsKey(table))
			{
				tableSecurityCache.Add(table, securityAccessLevel);
			}
		}
		return securityAccessLevel;
	}

	public SecurityAccessLevel GetTableAccessLevel(string table, M1Database dataSet, M1User userObj, bool isCache)
	{
		currentUser = userObj;
		currentDatabase = dataSet;
		return GetTableAccessLevel(table, SecurityAccessLevel.None);
	}

	public SecurityAccessLevel GetFieldAccessLevel(string table, string field)
	{
		return GetFieldAccessLevel(table, field, SecurityAccessLevel.None, currentDatabase.ID, currentUser.ID);
	}

	public SecurityAccessLevel GetFieldAccessLevel(string table, string field, SecurityAccessLevel defaultIfNotSet, string databaseToCheck)
	{
		return GetFieldAccessLevel(table, field, defaultIfNotSet, databaseToCheck, currentUser.ID);
	}

	public SecurityAccessLevel GetFieldAccessLevel(string table, string field, SecurityAccessLevel defaultIfNotSet, string databaseToCheck, string userToCheck)
	{
		table = table.Trim().ToUpper();
		field = field.Trim().ToUpper();
		if (getFieldAccessCommand == null)
		{
			getFieldAccessCommand = currentDataDictionary.NewSqlCommand("Select Top 1 dtLevel, Case When dtUserID = @UserID Then Case When dtTable = '' Then 5 Else Case When dtField = '' Then 3 Else 1 End End Else Case When dtTable = '' Then 6 Else Case When dtField = '' Then 4 Else 2 End End End As SecurityType From DDSecurityTables Where  dtLevel <> 0  and dtDataset = @DatabaseID and (dtTable = '' Or dtTable = @TableName) and (dtField = '' Or dtField = @FieldName)  and (dtUserID = @UserID Or dtUserID In (Select dzGroupID From DDSecurityGroups Inner Join DDUsers On dzGroupID = duUserID Where dzDataset = @DatabaseID And dzUserID = @UserID And duType = 1))  Order By SecurityType,dtLevel DESC");
			getFieldAccessCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userToCheck;
			getFieldAccessCommand.Parameters.Add(new SqlParameter("@DatabaseID", SqlDbType.NVarChar)).Value = databaseToCheck;
			getFieldAccessCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = table;
			getFieldAccessCommand.Parameters.Add(new SqlParameter("@FieldName", SqlDbType.NVarChar)).Value = field;
		}
		else
		{
			getFieldAccessCommand.Parameters["@UserID"].Value = userToCheck;
			getFieldAccessCommand.Parameters["@DatabaseID"].Value = databaseToCheck;
			getFieldAccessCommand.Parameters["@TableName"].Value = table;
			getFieldAccessCommand.Parameters["@FieldName"].Value = field;
		}
		object obj = currentDataDictionary.ExecuteScalar(getFieldAccessCommand);
		if (obj == null)
		{
			return defaultIfNotSet;
		}
		return (SecurityAccessLevel)obj;
	}

	public SecurityAccessLevel GetObjectAccessLevel(string objectID)
	{
		if (getObjectAccessCommand == null)
		{
			getObjectAccessCommand = currentDataDictionary.NewSqlCommand("Select Top 1 dtLevel From (select dtLevel, Case When dtTable = '' Then 3 Else 1 End As SecurityType From DDSecurityTables Where dtLevel <> 0 And dtDataset = @DatabaseID and (dtTable = '' Or dtTable = (Select doTable From DDObjects Where doObjectID = @ObjectID)) and dtField = ''  and dtUserID = @UserID Union All Select dtLevel, Case When dtTable = '' Then 4 Else 2 End As SecurityType From DDSecurityTables Where dtLevel <> 0 And dtDataset = @DatabaseID  And (dtTable = '' Or dtTable = (Select doTable From DDObjects Where doObjectID = @ObjectID)) and dtField = ''  And dtUserID In (Select dzGroupID From DDSecurityGroups Inner Join DDUsers On dzGroupID = duUserID Where dzDataset = @DatabaseID And dzUserID = @UserID And duType = 1)) As subquery Order By SecurityType,dtLevel DESC");
			getObjectAccessCommand.Parameters.Add(new SqlParameter("@DatabaseID", SqlDbType.NVarChar)).Value = currentDatabase.ID;
			getObjectAccessCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = currentUser.ID;
			getObjectAccessCommand.Parameters.Add(new SqlParameter("@ObjectID", SqlDbType.NVarChar)).Value = objectID;
		}
		else
		{
			getObjectAccessCommand.Parameters["@DatabaseID"].Value = currentDatabase.ID;
			getObjectAccessCommand.Parameters["@UserID"].Value = currentUser.ID;
			getObjectAccessCommand.Parameters["@ObjectID"].Value = objectID;
		}
		object obj = currentDataDictionary.ExecuteScalar(getObjectAccessCommand);
		if (obj == null)
		{
			return SecurityAccessLevel.None;
		}
		return (SecurityAccessLevel)obj;
	}

	public void Login()
	{
		accessLevel = GetTableAccessLevel(string.Empty, SecurityAccessLevel.Default);
		StringBuilder stringBuilder = new StringBuilder();
		SqlCommand sqlCommand = currentDataDictionary.NewSqlCommand("select Distinct dzGroupID,duType From DDSecurityGroups Inner Join DDUsers On dzGroupID = duUserID WHERE dzUserID = @UserID and dzDataset = @DatabaseID Union Select dzGroupID,duType From DDSecurityGroups Inner Join DDUsers On dzGroupID = duUserID Where dzUserID In (Select dzGroupID From DDSecurityGroups Where dzDataset = @DatabaseID And dzUserID = @UserID) and dzDataset = @DatabaseID");
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = currentUser.ID;
		sqlCommand.Parameters.Add(new SqlParameter("@DatabaseID", SqlDbType.NVarChar)).Value = currentDatabase.ID;
		ComponentRoles.Clear();
		GroupWhere = string.Empty;
		foreach (DataRow row in currentDataDictionary.GetDataTable(sqlCommand).Rows)
		{
			if (row.Field<byte>("duType") == 2)
			{
				ComponentRoles.Add(row.Field<string>("dzGroupID").Trim());
			}
			else if (row.Field<byte>("duType") == 1)
			{
				Groups.Add(row.Field<string>("dzGroupID").Trim());
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(" Or ");
				}
				stringBuilder.Append("dtUserID = " + row.Field<string>("dzGroupID").Trim().ToSql());
			}
		}
		if (stringBuilder.Length != 0)
		{
			GroupWhere = "(" + stringBuilder.ToString() + ")";
		}
	}
}
