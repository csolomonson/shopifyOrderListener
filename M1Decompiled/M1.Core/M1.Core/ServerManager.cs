using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using ADODB;
using M1.Extensions;

namespace M1.Core;

public class ServerManager
{
	public class ConnectionDroppedEventArgs : EventArgs
	{
		public SqlException SqlException;

		public M1User User;

		public AppContext Context;

		public string DatabaseName;

		public SqlConnection SqlConnection;

		public SqlTransaction SqlTransaction;

		public bool LeaveOpenOnReturn;

		public bool Cancel = true;

		public ConnectionDroppedEventArgs(SqlException exc, AppContext context, M1User m1User, string databaseName, ref SqlConnection sqlConnection, SqlTransaction sqlTransaction, bool leaveOpenOnReturn)
		{
			SqlException = exc;
			Context = context;
			User = m1User;
			DatabaseName = databaseName;
			SqlConnection = sqlConnection;
			SqlTransaction = sqlTransaction;
			LeaveOpenOnReturn = leaveOpenOnReturn;
		}
	}

	public delegate void M1ErrorCallback(SqlException ex);

	public delegate void M1CommandCallback(int rowsAffected);

	private class commandCallbackObject
	{
		public M1CommandCallback callback;

		public SqlCommand command;

		public Control callbackControl;

		public M1ErrorCallback errorCallback;

		public commandCallbackObject(M1CommandCallback callback, Control callbackControl, SqlCommand command, M1ErrorCallback errorCallback)
		{
			this.callback = callback;
			this.callbackControl = callbackControl;
			this.command = command;
			this.errorCallback = errorCallback;
		}
	}

	public delegate void M1ReaderCallback(SqlDataReader reader);

	private class readerCallbackObject
	{
		public M1ReaderCallback callback;

		public Control callbackControl;

		public SqlCommand command;

		public M1ErrorCallback errorCallback;

		public readerCallbackObject(M1ReaderCallback callback, Control callbackControl, SqlCommand command, M1ErrorCallback errorCallback)
		{
			this.callback = callback;
			this.callbackControl = callbackControl;
			this.command = command;
			this.errorCallback = errorCallback;
		}
	}

	public delegate void M1ScalarCallback(object value);

	private class scalerCallbackObject
	{
		public M1ScalarCallback callback;

		public Control callbackControl;

		public SqlCommand command;

		public M1ErrorCallback errorCallback;

		public scalerCallbackObject(M1ScalarCallback callback, Control callbackControl, SqlCommand command, M1ErrorCallback errorCallback)
		{
			this.callback = callback;
			this.callbackControl = callbackControl;
			this.command = command;
			this.errorCallback = errorCallback;
		}
	}

	public ConnectionInfo ConnectionInfo = new ConnectionInfo();

	public string sqlPassword = string.Empty;

	public Dmo Dmo;

	public Backup Backup;

	public ServerFileSystem FileSystem;

	internal SqlConnection singleUserConnection;

	private string singleUserDatabaseName = string.Empty;

	private bool _useDataDictionarySettings;

	private AppContext currentContext;

	private const int Keysize = 256;

	private const int DerivationIterations = 1000;

	private string String1 => "lw7IT%.:gs4GS^>=kyBO@,_m0EU(`fvCR);j2K!?ezJ#|n6Q{c1N]d5W\\qH[iD*oL/uYbM-A'FaXt<V3p+~hxP}$&9r8Z";

	private string BaseString => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()[]{},.<>/?\\|`~;':-_=+";

	public event EventHandler<ConnectionDroppedEventArgs> ConnectionDropped;

	public ServerManager()
	{
	}

	public ServerManager(AppContext context)
	{
		currentContext = context;
	}

	public void LoadMobileConnectionSettings(InstallationInfo server)
	{
		try
		{
			if (currentContext.IsHosted)
			{
				ConnectionInfo.Server = currentContext.Metadata.GetMetaData("Server_Instance");
				if (ConnectionInfo.Server.Length == 0)
				{
					throw new M1ConnectionSettingsMissingException("No database server information was specified in the connection settings.");
				}
				ConnectionInfo.SqlUserID = currentContext.Metadata.GetMetaData("M1Admin_User");
				sqlPassword = DecryptMeta(currentContext.Metadata.GetMetaData("M1Admin_PW"), currentContext.Metadata.GetMetaData("GUID"));
				ConnectionInfo.TrustedConnection = false;
				ConnectionInfo.NetworkLibrary = currentContext.Registry.NetworkLibrary;
			}
			else
			{
				M1MobileSessionManager m1MobileSessionManager = new M1MobileSessionManager().LoadMobileSettings(server);
				ConnectionInfo.Server = m1MobileSessionManager.Server;
				ConnectionInfo.SqlUserID = m1MobileSessionManager.LoginID;
				sqlPassword = ((m1MobileSessionManager.Password.Length > 0) ? Decrypt(m1MobileSessionManager.Password) : "");
				ConnectionInfo.TrustedConnection = m1MobileSessionManager.TrustedConnection;
				ConnectionInfo.NetworkLibrary = m1MobileSessionManager.NetworkLibrary;
			}
			Backup = new Backup(currentContext, this);
			Dmo = new Dmo(currentContext, this);
			FileSystem = new ServerFileSystem(currentContext, this);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void LoadProductConfiguratorConnectionSettings()
	{
		try
		{
			ConnectionInfo.Server = currentContext.DDServerManager.ConnectionInfo.Server;
			ConnectionInfo.SqlUserID = currentContext.DDServerManager.ConnectionInfo.SqlUserID;
			sqlPassword = currentContext.DDServerManager.sqlPassword;
			ConnectionInfo.TrustedConnection = currentContext.DDServerManager.ConnectionInfo.TrustedConnection;
			ConnectionInfo.NetworkLibrary = currentContext.DDServerManager.ConnectionInfo.NetworkLibrary;
			Dmo = new Dmo(currentContext, this);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public bool LoadFromSettings(IniSettings iniSettings, bool useDataDictionarySettings)
	{
		bool flag = false;
		_useDataDictionarySettings = useDataDictionarySettings;
		if (!currentContext.IsHosted)
		{
			if (useDataDictionarySettings)
			{
				ConnectionInfo.Server = iniSettings.Get("DDServer", string.Empty).Trim();
				if (ConnectionInfo.Server.Length == 0)
				{
					flag = LoadFromSettings(iniSettings, useDataDictionarySettings: false);
					_useDataDictionarySettings = true;
				}
				else
				{
					ConnectionInfo.SqlUserID = iniSettings.Get("DDUserID", "sa");
					sqlPassword = Decrypt(iniSettings.Get("DDPwd", string.Empty));
					ConnectionInfo.TrustedConnection = iniSettings.GetAsBool("DDTrustedConnection", defaultValue: false);
					ConnectionInfo.NetworkLibrary = iniSettings.Get("DDNetworkLibrary", "dbmssocn");
					flag = true;
				}
			}
			else
			{
				ConnectionInfo.Server = iniSettings.Get("DBServer", string.Empty).Trim();
				if (ConnectionInfo.Server.Length == 0)
				{
					throw new M1ConnectionSettingsMissingException("No database server information was specified in the connection settings.");
				}
				ConnectionInfo.SqlUserID = iniSettings.Get("DBUserID", "sa");
				sqlPassword = Decrypt(iniSettings.Get("DBPwd", string.Empty));
				ConnectionInfo.TrustedConnection = iniSettings.GetAsBool("DBTrustedConnection", defaultValue: false);
				ConnectionInfo.NetworkLibrary = iniSettings.Get("DBNetworkLibrary", "dbmssocn");
				flag = true;
			}
		}
		else
		{
			ConnectionInfo.Server = currentContext.Metadata.GetMetaData("Server_Instance");
			if (ConnectionInfo.Server.Length == 0)
			{
				throw new M1ConnectionSettingsMissingException("No database server information was specified in the connection settings.");
			}
			ConnectionInfo.SqlUserID = currentContext.Metadata.GetMetaData("M1Admin_User");
			sqlPassword = DecryptMeta(currentContext.Metadata.GetMetaData("M1Admin_PW"), currentContext.Metadata.GetMetaData("GUID"));
			ConnectionInfo.TrustedConnection = false;
			ConnectionInfo.NetworkLibrary = currentContext.Registry.NetworkLibrary;
			flag = true;
		}
		return flag;
	}

	public bool LoadSuppliedSettings(bool useDataDictionarySettings, string host, string port, string sqluser, string sqlpass, bool isTrusted, string netLib)
	{
		_useDataDictionarySettings = useDataDictionarySettings;
		ConnectionInfo.Server = host + "," + port;
		ConnectionInfo.SqlUserID = sqluser;
		sqlPassword = sqlpass;
		ConnectionInfo.TrustedConnection = isTrusted;
		ConnectionInfo.NetworkLibrary = netLib;
		return true;
	}

	public void ClearAllPools()
	{
		SqlConnection.ClearAllPools();
	}

	public void ClearPool(M1User m1User, string databaseName)
	{
		using SqlConnection connection = GetConnection(m1User, databaseName, openImmediately: true);
		SqlConnection.ClearPool(connection);
	}

	public SqlConnection GetConnection(M1User m1User, string databaseName, bool openImmediately)
	{
		return GetConnection((m1User == null) ? string.Empty : m1User.ID, databaseName, ConnectionInfo.Server, ConnectionInfo.SqlUserID, sqlPassword, ConnectionInfo.TrustedConnection, ConnectionInfo.NetworkLibrary, openImmediately);
	}

	public SqlConnection GetConnection(string appUserString, string databaseName, string databaseServer, string connectUserID, string connectPassword, bool trustedConnection, string networkLibrary, bool openImmediately)
	{
		if (singleUserConnection != null && databaseName == singleUserDatabaseName)
		{
			return singleUserConnection;
		}
		databaseServer = databaseServer.Trim();
		if (databaseServer.Length == 0)
		{
			databaseServer = "(local)";
		}
		networkLibrary = networkLibrary.Trim();
		if (networkLibrary.Length == 0)
		{
			networkLibrary = "dbmssocn";
		}
		if (databaseName.Length == 0)
		{
			databaseName = "master";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("Network Library={0} ;Server={1};", networkLibrary, databaseServer);
		if ((currentContext == null || !currentContext.IsHosted) && trustedConnection)
		{
			stringBuilder.Append("Integrated Security=SSPI;");
		}
		else
		{
			stringBuilder.AppendFormat("uid={0};pwd='{1}';", connectUserID, connectPassword);
		}
		stringBuilder.AppendFormat("Initial Catalog={0};", databaseName);
		stringBuilder.AppendFormat("app={0};Min Pool Size=1;Max Pool Size=2;Pooling=true;Connection Reset=false;Enlist=false;MultipleActiveResultSets=false;Asynchronous Processing=true;", appUserString);
		SqlConnection sqlConnection = new SqlConnection(stringBuilder.ToString());
		if (openImmediately)
		{
			sqlConnection.Open();
		}
		return sqlConnection;
	}

	public void TestComConnection(string databaseName, string appUserString, string databaseServer, string connectUserID, string connectPassword, bool trustedConnection, string networkLibrary)
	{
		GetComConnection(databaseName, appUserString, databaseServer, connectUserID, connectPassword, trustedConnection, networkLibrary).Close();
	}

	public Connection GetComConnection(string databaseName, string appUserString)
	{
		Connection existingConnection = new ConnectionClass();
		return GetComConnection(databaseName, appUserString, existingConnection);
	}

	public Connection GetComConnection(string databaseName, string appUserString, Connection existingConnection)
	{
		return GetComConnection(databaseName, appUserString, ConnectionInfo.Server, ConnectionInfo.SqlUserID, sqlPassword, ConnectionInfo.TrustedConnection, ConnectionInfo.NetworkLibrary, existingConnection);
	}

	public Connection GetComConnection(string databaseName, string appUserString, string databaseServer, string connectUserID, string connectPassword, bool trustedConnection, string networkLibrary)
	{
		Connection existingConnection = new ConnectionClass();
		return GetComConnection(databaseName, appUserString, databaseServer, connectUserID, connectPassword, trustedConnection, networkLibrary, existingConnection);
	}

	public Connection GetComConnection(string databaseName, string appUserString, string databaseServer, string connectUserID, string connectPassword, bool trustedConnection, string networkLibrary, Connection existingConnection)
	{
		if (existingConnection.State != 0)
		{
			existingConnection.Close();
		}
		if (existingConnection.State == 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Provider=SQLOLEDB;Driver={SQL Server};");
			stringBuilder.AppendFormat("Network Library={0};Server={1}", networkLibrary, databaseServer);
			stringBuilder.AppendFormat(";Initial catalog={0};app={1}", databaseName, appUserString);
			stringBuilder.Append(";Min Pool Size=1;Pooling=true;");
			if (trustedConnection)
			{
				stringBuilder.Append("Integrated Security=SSPI;Persist Security Info=False;");
			}
			else
			{
				stringBuilder.AppendFormat("uid={0};pwd={1};", connectUserID, connectPassword);
			}
			existingConnection.ConnectionString = stringBuilder.ToString();
			existingConnection.CursorLocation = CursorLocationEnum.adUseClient;
			existingConnection.CommandTimeout = 0;
			if (trustedConnection)
			{
				existingConnection.Open(stringBuilder.ToString(), string.Empty, string.Empty, 0);
			}
			else
			{
				existingConnection.Open(stringBuilder.ToString(), connectUserID, connectPassword, 0);
			}
		}
		return existingConnection;
	}

	public string IsCurrentMachineTheServer()
	{
		string machineName = Environment.MachineName;
		string sqlServerName = GetSqlServerName(null, null, "master");
		if (!machineName.Equals(sqlServerName, StringComparison.CurrentCultureIgnoreCase) && !sqlServerName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase) && !sqlServerName.Equals("(LOCAL)", StringComparison.CurrentCultureIgnoreCase))
		{
			return "This option is only available when M1 is started from the machine where SQL Server is running. The current machine name is " + machineName + " and the SQL Server machine name is " + sqlServerName + ".";
		}
		return string.Empty;
	}

	public SqlTransaction BeginTransaction(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		if (sqlConnection == null)
		{
			sqlConnection = GetConnection(m1User, databaseName, openImmediately: true);
		}
		return sqlConnection?.BeginTransaction();
	}

	public DataSet GetDataSet(SqlConnection sqlConnection, M1User m1User, string databaseName, string queryString)
	{
		bool needToClose = true;
		SqlConnection sqlConnection2 = GetConnection(m1User, databaseName, openImmediately: false, sqlConnection, null, ref needToClose);
		DataSet dataSet = new DataSet();
		SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(queryString, sqlConnection2);
		sqlDataAdapter.SelectCommand.CommandTimeout = 0;
		try
		{
			sqlDataAdapter.Fill(dataSet);
		}
		catch (SqlException exc)
		{
			checkForDroppedConnection(exc, m1User, databaseName, ref sqlConnection2, null, needToClose);
			sqlDataAdapter = new SqlDataAdapter(queryString, sqlConnection2);
			sqlDataAdapter.Fill(dataSet);
		}
		catch (InvalidOperationException)
		{
			if (sqlConnection2.State != ConnectionState.Broken && sqlConnection2.State != ConnectionState.Closed)
			{
				throw;
			}
			ClearAllPools();
			sqlConnection2 = GetConnection(m1User, databaseName, openImmediately: false, sqlConnection, null, ref needToClose);
			sqlDataAdapter = new SqlDataAdapter(queryString, sqlConnection2);
			sqlDataAdapter.Fill(dataSet);
		}
		finally
		{
			sqlDataAdapter.SelectCommand.Connection = null;
			if (needToClose)
			{
				sqlConnection2.Close();
			}
		}
		return dataSet;
	}

	public void Fill(SqlConnection sqlConnection, M1User m1User, string databaseName, DataTable dataTable, SqlCommand sqlCommand, bool fillSchema, out SqlDataAdapter adapter, int rowsToLoad, SqlTransaction sqlTransaction)
	{
		bool needToClose = true;
		SqlConnection sqlConnection2 = (sqlCommand.Connection = GetConnection(m1User, databaseName, openImmediately: false, sqlCommand.Connection, sqlTransaction, ref needToClose));
		sqlCommand.CommandTimeout = 0;
		if (sqlCommand.Transaction == null && sqlTransaction != null)
		{
			sqlCommand.Transaction = sqlTransaction;
		}
		adapter = new SqlDataAdapter(sqlCommand);
		try
		{
			try
			{
				if (!AppContext.InQuickExit)
				{
					adapter.Fill(dataTable);
				}
			}
			catch (SqlException exc)
			{
				checkForDroppedConnection(exc, m1User, databaseName, ref sqlConnection2, sqlTransaction, needToClose);
				sqlCommand.Connection = sqlConnection2;
				adapter = new SqlDataAdapter(sqlCommand);
				if (!AppContext.InQuickExit)
				{
					adapter.Fill(dataTable);
				}
			}
			catch (InvalidOperationException)
			{
				if (sqlConnection2.State != ConnectionState.Broken && sqlConnection2.State != ConnectionState.Closed)
				{
					throw;
				}
				ClearAllPools();
				sqlConnection2 = (sqlCommand.Connection = GetConnection(m1User, databaseName, openImmediately: false, sqlConnection, sqlTransaction, ref needToClose));
				adapter = new SqlDataAdapter(sqlCommand);
				if (!AppContext.InQuickExit)
				{
					adapter.Fill(dataTable);
				}
			}
			if (fillSchema)
			{
				try
				{
					adapter.FillSchema(dataTable, SchemaType.Source);
					return;
				}
				catch (SqlException exc2)
				{
					checkForDroppedConnection(exc2, m1User, databaseName, ref sqlConnection2, sqlTransaction, needToClose);
					adapter.FillSchema(dataTable, SchemaType.Source);
					return;
				}
			}
		}
		finally
		{
			adapter.SelectCommand.Connection = null;
			if (needToClose || AppContext.InQuickExit)
			{
				sqlConnection2.Close();
			}
		}
	}

	public void Fill(SqlConnection sqlConnection, M1User m1User, string databaseName, DataTable dataTable, int rowsToLoad, string queryString)
	{
		Fill(sqlConnection, m1User, databaseName, dataTable, rowsToLoad, queryString, fillSchema: false);
	}

	public void Fill(SqlConnection sqlConnection, M1User m1User, string databaseName, DataTable dataTable, int rowsToLoad, string queryString, bool fillSchema)
	{
		Fill(sqlConnection, m1User, databaseName, dataTable, rowsToLoad, queryString, fillSchema: false, out var _);
	}

	public void Fill(SqlConnection sqlConnection, M1User m1User, string databaseName, DataTable dataTable, int rowsToLoad, string queryString, bool fillSchema, out SqlDataAdapter adapter)
	{
		Fill(sqlConnection, m1User, databaseName, dataTable, rowsToLoad, queryString, fillSchema, out adapter, null);
	}

	public void Fill(SqlConnection sqlConnection, M1User m1User, string databaseName, DataTable dataTable, int rowsToLoad, string queryString, bool fillSchema, out SqlDataAdapter adapter, SqlTransaction sqlTransaction)
	{
		bool needToClose = true;
		SqlConnection sqlConnection2 = GetConnection(m1User, databaseName, openImmediately: false, sqlConnection, sqlTransaction, ref needToClose);
		queryString = queryString.Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ')
			.TrimStart(' ');
		string selectCommandText;
		if (rowsToLoad > 0 && queryString.Trim().Substring(0, 7).ToUpper() == "SELECT " && (queryString.Trim().IndexOf(" TOP ", StringComparison.CurrentCultureIgnoreCase) == -1 || queryString.Trim().IndexOf(" TOP ", StringComparison.CurrentCultureIgnoreCase) > queryString.Trim().IndexOf("FROM ", StringComparison.CurrentCultureIgnoreCase)))
		{
			int num = queryString.IndexOf("DISTINCT", StringComparison.CurrentCultureIgnoreCase);
			selectCommandText = ((num != -1 && num <= queryString.Trim().IndexOf("FROM ", StringComparison.CurrentCultureIgnoreCase)) ? ("SELECT DISTINCT TOP " + rowsToLoad + " " + queryString.Substring(num + 8)) : ("SELECT TOP " + rowsToLoad + " " + queryString.Substring(7)));
		}
		else
		{
			selectCommandText = queryString;
		}
		adapter = new SqlDataAdapter(selectCommandText, sqlConnection2);
		try
		{
			if (sqlTransaction != null)
			{
				adapter.SelectCommand.Transaction = sqlTransaction;
			}
			adapter.SelectCommand.CommandTimeout = 0;
			try
			{
				adapter.Fill(dataTable);
			}
			catch (SqlException exc)
			{
				checkForDroppedConnection(exc, m1User, databaseName, ref sqlConnection2, null, needToClose);
				adapter = new SqlDataAdapter(queryString, sqlConnection2);
				adapter.Fill(dataTable);
			}
			catch (InvalidOperationException)
			{
				if (sqlConnection2.State != ConnectionState.Broken && sqlConnection2.State != ConnectionState.Closed)
				{
					throw;
				}
				ClearAllPools();
				sqlConnection2 = GetConnection(m1User, databaseName, openImmediately: false, sqlConnection, null, ref needToClose);
				adapter = new SqlDataAdapter(queryString, sqlConnection2);
				adapter.Fill(dataTable);
			}
			if (fillSchema)
			{
				try
				{
					adapter.FillSchema(dataTable, SchemaType.Source);
					return;
				}
				catch (SqlException exc2)
				{
					checkForDroppedConnection(exc2, m1User, databaseName, ref sqlConnection2, null, needToClose);
					adapter.FillSchema(dataTable, SchemaType.Source);
					return;
				}
			}
		}
		finally
		{
			adapter.SelectCommand.Connection = null;
			if (needToClose)
			{
				sqlConnection2.Close();
			}
		}
	}

	public DataTable GetDataTable(SqlConnection sqlConnection, M1User m1User, string databaseName, int rowsToLoad, string queryString)
	{
		return GetDataTable(sqlConnection, m1User, databaseName, rowsToLoad, queryString, fillSchema: false);
	}

	public DataTable GetDataTable(SqlConnection sqlConnection, M1User m1User, string databaseName, int rowsToLoad, string queryString, bool fillSchema)
	{
		SqlDataAdapter adapter;
		return GetDataTable(sqlConnection, m1User, databaseName, rowsToLoad, queryString, fillSchema, out adapter);
	}

	public DataTable GetDataTable(SqlConnection sqlConnection, M1User m1User, string databaseName, int rowsToLoad, SqlCommand sqlCommand, bool fillSchema, out SqlDataAdapter adapter)
	{
		return GetDataTable(sqlConnection, m1User, databaseName, rowsToLoad, sqlCommand, fillSchema, out adapter, null);
	}

	public DataTable GetDataTable(SqlConnection sqlConnection, M1User m1User, string databaseName, int rowsToLoad, SqlCommand sqlCommand, bool fillSchema, out SqlDataAdapter adapter, SqlTransaction sqlTransaction)
	{
		DataTable dataTable = new DataTable();
		Fill(sqlConnection, m1User, databaseName, dataTable, sqlCommand, fillSchema, out adapter, rowsToLoad, sqlTransaction);
		return dataTable;
	}

	public DataTable GetDataTable(SqlConnection sqlConnection, M1User m1User, string databaseName, int rowsToLoad, string queryString, bool fillSchema, out SqlDataAdapter adapter)
	{
		return GetDataTable(sqlConnection, m1User, databaseName, rowsToLoad, queryString, fillSchema, out adapter, null);
	}

	public DataTable GetDataTable(SqlConnection sqlConnection, M1User m1User, string databaseName, int rowsToLoad, string queryString, bool fillSchema, out SqlDataAdapter adapter, SqlTransaction sqlTransaction)
	{
		DataTable dataTable = new DataTable();
		Fill(sqlConnection, m1User, databaseName, dataTable, rowsToLoad, queryString, fillSchema, out adapter, sqlTransaction);
		return dataTable;
	}

	protected void OnConnectionDropped(ConnectionDroppedEventArgs e)
	{
		this.ConnectionDropped?.Invoke(this, e);
	}

	private void checkForDroppedConnection(SqlException exc, M1User m1User, string databaseName, ref SqlConnection sqlConnection, SqlTransaction sqlTransaction, bool leaveOpenOnReturn)
	{
		if (exc.Class >= 20)
		{
			ConnectionDroppedEventArgs e = new ConnectionDroppedEventArgs(exc, currentContext, m1User, databaseName, ref sqlConnection, sqlTransaction, leaveOpenOnReturn);
			OnConnectionDropped(e);
			sqlConnection = e.SqlConnection;
			_ = e.Cancel;
			return;
		}
		throw exc;
	}

	public bool DatabaseCurrentlyInUse(M1User m1User, string databaseID)
	{
		ClearAllPools();
		return GetDataTable(null, m1User, string.Empty, 0, $"Select COUNT(*) AS dbProcesses From master.dbo.sysprocesses Where DBID = db_id({databaseID.ToSql()}) AND program_name != '.Net SqlClient Data Provider'").Rows[0].Field<int>("dbProcesses") != 0;
	}

	public virtual bool DoesTableExist(SqlConnection sqlConnection, M1User m1User, string databaseName, string tableName, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = new SqlCommand("SELECT Table_name FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = @TableName");
		sqlCommand.Parameters.Add(new SqlParameter("@TableName", SqlDbType.NVarChar)).Value = tableName;
		return ExecuteScalar(sqlConnection, m1User, databaseName, sqlCommand, transaction) != null;
	}

	public virtual bool DoesViewExist(SqlConnection sqlConnection, M1User m1User, string databaseName, string viewName, SqlTransaction transaction)
	{
		SqlCommand sqlCommand = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_TYPE='VIEW' AND TABLE_NAME=@ViewName");
		sqlCommand.Parameters.Add(new SqlParameter("@ViewName", SqlDbType.NVarChar)).Value = viewName;
		return ExecuteScalar(sqlConnection, m1User, databaseName, sqlCommand, transaction) != null;
	}

	public bool HasPermissionsToCreateDatabase(SqlConnection sqlConnection, M1User m1User)
	{
		SqlCommand sqlCommand = new SqlCommand("select HAS_PERMS_BY_NAME(NULL, 'DATABASE', 'CREATE DATABASE')");
		return (int)ExecuteScalar(sqlConnection, m1User, "master", sqlCommand) != 0;
	}

	public bool HasPermissionsToDatabase(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		SqlCommand sqlCommand = new SqlCommand("select HAS_PERMS_BY_NAME(@DbName, 'DATABASE', 'UPDATE')");
		sqlCommand.Parameters.Add(new SqlParameter("@DbName", SqlDbType.NVarChar)).Value = databaseName;
		return (int)ExecuteScalar(sqlConnection, m1User, "master", sqlCommand) != 0;
	}

	public bool DoesDatabaseExist(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		SqlCommand sqlCommand = new SqlCommand("select count(*) from sys.databases where name=@DbName");
		sqlCommand.Parameters.Add(new SqlParameter("@DbName", SqlDbType.NVarChar)).Value = databaseName;
		return (int)ExecuteScalar(sqlConnection, m1User, "master", sqlCommand) != 0;
	}

	public DatabaseInfo GetDatasetProperties(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		bool needToClose = true;
		DatabaseInfo databaseInfo = new DatabaseInfo(dataDictionary: false);
		try
		{
			DataTable dataTable = new DataTable();
			sqlConnection = GetConnection(m1User, "master", openImmediately: false, sqlConnection, null, ref needToClose);
			new SqlDataAdapter("select xadUniqueID,xadDescription,xadVersion from " + databaseName + ".dbo.DatasetProperties", sqlConnection).Fill(dataTable);
			if (dataTable.Rows.Count > 0)
			{
				databaseInfo.Name = databaseName;
				databaseInfo.Description = dataTable.Rows[0].Field<string>("xadDescription").Trim();
				databaseInfo.Version = dataTable.Rows[0].Field<string>("xadVersion").Trim();
				databaseInfo.TenantId = dataTable.Rows[0].Field<Guid>("xadUniqueID").ToString();
				if (databaseInfo.Version.CompareTo("8.10.040") > 0)
				{
					databaseInfo.ExtensionVersions = (string)ExecuteScalar(sqlConnection, m1User, "master", "select Top 1 IsNull(xadExtensionVersions,'') from " + databaseName + ".dbo.DatasetProperties");
				}
			}
			return databaseInfo;
		}
		catch
		{
			DataTable dataTable2 = new DataTable();
			sqlConnection = GetConnection(m1User, "master", openImmediately: false, sqlConnection, null, ref needToClose);
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select xdDescription,xdVersion from " + databaseName + ".dbo.DatasetProperties", sqlConnection);
			try
			{
				sqlDataAdapter.Fill(dataTable2);
			}
			catch
			{
				sqlConnection = GetConnection(m1User, "master", openImmediately: false, sqlConnection, null, ref needToClose);
				if (DoesDatabaseExist(sqlConnection, m1User, databaseName))
				{
					databaseInfo.Description = databaseName;
					databaseInfo.Version = "0.00.000";
					return databaseInfo;
				}
				return null;
			}
			if (dataTable2.Rows.Count > 0)
			{
				databaseInfo.Description = dataTable2.Rows[0].Field<string>("xdDescription").Trim();
				databaseInfo.Version = dataTable2.Rows[0].Field<string>("xdVersion").Trim();
			}
			return databaseInfo;
		}
		finally
		{
			if (needToClose)
			{
				sqlConnection.Close();
			}
		}
	}

	public bool IsSQLExpress(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		return GetSqlServerVersionString(sqlConnection, m1User, databaseName).Contains("Express Edition");
	}

	public bool IsMSDEOrSqlExpress(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		string dbType = string.Empty;
		return IsMSDEOrSqlExpress(sqlConnection, m1User, databaseName, ref dbType);
	}

	public bool IsMSDEOrSqlExpress(SqlConnection sqlConnection, M1User m1User, string databaseName, ref string dbType)
	{
		string sqlServerVersionString = GetSqlServerVersionString(sqlConnection, m1User, databaseName);
		if (sqlServerVersionString.Contains("Express Edition") && sqlServerVersionString.Contains("10.50."))
		{
			dbType = "SQLEXPRESSR2";
		}
		else if (sqlServerVersionString.Contains("Express Edition") && sqlServerVersionString.Contains("11.0."))
		{
			dbType = "SQLEXPRESS12";
		}
		else if (sqlServerVersionString.Contains("Express Edition") && sqlServerVersionString.Contains("12.0."))
		{
			dbType = "SQLEXPRESS14";
		}
		else if (sqlServerVersionString.Contains("Express Edition") && sqlServerVersionString.Contains("13.0."))
		{
			dbType = "SQLEXPRESS16";
		}
		else if (sqlServerVersionString.Contains("Express Edition"))
		{
			dbType = "SQLEXPRESS";
		}
		return dbType.Length != 0;
	}

	public string GetSqlServerVersionString(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		string text = string.Empty;
		DataTable dataTable = GetDataTable(sqlConnection, m1User, databaseName, 0, "select @@Version as Version");
		if (dataTable.Rows.Count > 0)
		{
			text = dataTable.Rows[0].Field<string>("Version").Trim();
		}
		if (text.Length > 0)
		{
			text = text.Replace('\n', '\r');
			int num = text.IndexOf('\r');
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
		}
		if (!text.Contains("7.00."))
		{
			dataTable = GetDataTable(sqlConnection, m1User, databaseName, 0, "SELECT  SERVERPROPERTY('productversion') as version, SERVERPROPERTY ('productlevel') as level, SERVERPROPERTY ('edition') as edition");
			if (dataTable.Rows.Count > 0)
			{
				text = dataTable.Rows[0].Field<string>("version").Trim() + ((dataTable.Rows[0].Field<string>("level").Trim().Length == 0) ? string.Empty : (" (" + dataTable.Rows[0].Field<string>("level").Trim() + ")")) + " - " + dataTable.Rows[0].Field<string>("Edition").Trim();
			}
		}
		return text;
	}

	public string GetSqlServerName(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		return (string)ExecuteScalar(sqlConnection, m1User, databaseName, "select SERVERPROPERTY('MachineName')");
	}

	public double GetSqlServerVersion(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		string text = (string)ExecuteScalar(sqlConnection, m1User, databaseName, "SELECT SERVERPROPERTY('productversion') as version");
		if (text != null)
		{
			int num = text.IndexOf(".", 0);
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			double.TryParse(text, out var result);
			return result;
		}
		return 0.0;
	}

	public string GetActualSqlServerName(SqlConnection sqlConnection, M1User m1User, string databaseName)
	{
		string result = string.Empty;
		DataTable dataTable = GetDataTable(sqlConnection, m1User, databaseName, 0, "select @@ServerName as ServerName");
		if (dataTable.Rows.Count > 0 && dataTable.Rows[0].Field<string>("ServerName") != null)
		{
			result = dataTable.Rows[0].Field<string>("ServerName").Trim();
		}
		return result;
	}

	public string GetUsersAccessingDatabase(M1User m1User, string databaseID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DataRow row in GetDataTable(null, m1User, string.Empty, 0, $"Select Distinct hostname, program_name From master.dbo.sysprocesses Where DBID = db_id({databaseID.ToSql()}) AND program_name != '.Net SqlClient Data Provider'").Rows)
		{
			if (stringBuilder.Length == 0)
			{
				stringBuilder.AppendFormat("[{0}\\{1}]", row.Field<string>("hostname").Trim(), row.Field<string>("program_name").Trim());
			}
			else
			{
				stringBuilder.AppendFormat(", [{0}\\{1}]", row.Field<string>("hostname").Trim(), row.Field<string>("program_name").Trim());
			}
		}
		return stringBuilder.ToString();
	}

	public SqlConnection GetConnection(M1User m1User, string databaseName, bool openImmediately, SqlConnection sqlConnection, SqlTransaction sqlTransaction, ref bool needToClose)
	{
		if (sqlConnection == null || sqlConnection.State == ConnectionState.Closed)
		{
			if (sqlTransaction != null)
			{
				sqlConnection = sqlTransaction.Connection;
				needToClose = false;
			}
			else
			{
				sqlConnection = GetConnection(m1User, databaseName, openImmediately);
				if (singleUserConnection != null && singleUserConnection == sqlConnection)
				{
					needToClose = false;
				}
				else
				{
					needToClose = openImmediately;
				}
			}
		}
		else
		{
			needToClose = false;
		}
		_ = needToClose;
		return sqlConnection;
	}

	public bool UpdateData(SqlConnection sqlConnection, M1User m1User, string databaseName, DataRow[] dataToUpdate, SqlDataAdapter adapter)
	{
		return UpdateData(sqlConnection, m1User, databaseName, dataToUpdate, adapter, null);
	}

	public bool UpdateData(SqlConnection sqlConnection, M1User m1User, string databaseName, DataRow[] dataToUpdate, SqlDataAdapter adapter, SqlTransaction sqlTransaction)
	{
		return UpdateData(sqlConnection, m1User, databaseName, dataToUpdate, adapter, sqlTransaction, generateCommands: true);
	}

	public bool UpdateData(SqlConnection sqlConnection, M1User m1User, string databaseName, DataRow[] dataToUpdate, SqlDataAdapter adapter, SqlTransaction sqlTransaction, bool generateCommands)
	{
		bool result = true;
		bool needToClose = true;
		SqlConnection connection = GetConnection(m1User, databaseName, openImmediately: false, sqlConnection, sqlTransaction, ref needToClose);
		try
		{
			adapter.SelectCommand.Connection = connection;
			adapter.SelectCommand.Transaction = sqlTransaction;
			adapter.SelectCommand.CommandTimeout = 0;
			if (generateCommands)
			{
				SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(adapter)
				{
					ConflictOption = ConflictOption.OverwriteChanges
				};
				adapter.DeleteCommand = sqlCommandBuilder.GetDeleteCommand();
				adapter.InsertCommand = sqlCommandBuilder.GetInsertCommand();
				adapter.UpdateCommand = sqlCommandBuilder.GetUpdateCommand();
			}
			adapter.InsertCommand.Connection = connection;
			adapter.InsertCommand.Transaction = sqlTransaction;
			adapter.InsertCommand.CommandTimeout = 0;
			adapter.UpdateCommand.Connection = connection;
			adapter.UpdateCommand.Transaction = sqlTransaction;
			adapter.UpdateCommand.CommandTimeout = 0;
			adapter.DeleteCommand.Connection = connection;
			adapter.DeleteCommand.Transaction = sqlTransaction;
			adapter.DeleteCommand.CommandTimeout = 0;
			adapter.Update(dataToUpdate);
			adapter.SelectCommand.Transaction = sqlTransaction;
		}
		catch (SqlException ex)
		{
			if (ex.Number == 2601)
			{
				throw;
			}
			adapter.UpdateCommand.Transaction.Rollback();
			result = false;
		}
		finally
		{
			if (needToClose)
			{
				connection.Close();
			}
		}
		return result;
	}

	public bool UpdateData(SqlConnection sqlConnection, M1User m1User, string databaseName, DataTable dataToUpdate, SqlDataAdapter adapter)
	{
		return UpdateData(sqlConnection, m1User, databaseName, dataToUpdate, adapter, null);
	}

	public bool UpdateData(SqlConnection sqlConnection, M1User m1User, string databaseName, DataTable dataToUpdate, SqlDataAdapter adapter, SqlTransaction sqlTransaction)
	{
		bool needToClose = true;
		SqlConnection connection = GetConnection(m1User, databaseName, openImmediately: false, sqlConnection, sqlTransaction, ref needToClose);
		try
		{
			adapter.SelectCommand.Connection = connection;
			adapter.SelectCommand.Transaction = sqlTransaction;
			SqlCommandBuilder sqlCommandBuilder = new SqlCommandBuilder(adapter);
			sqlCommandBuilder.ConflictOption = ConflictOption.OverwriteChanges;
			adapter.DeleteCommand = sqlCommandBuilder.GetDeleteCommand();
			adapter.InsertCommand = sqlCommandBuilder.GetInsertCommand();
			adapter.InsertCommand.CommandTimeout = 0;
			adapter.UpdateCommand = sqlCommandBuilder.GetUpdateCommand();
			adapter.UpdateCommand.Connection = connection;
			adapter.UpdateCommand.Transaction = sqlTransaction;
			adapter.UpdateCommand.CommandTimeout = 0;
			adapter.DeleteCommand.Connection = connection;
			adapter.DeleteCommand.Transaction = sqlTransaction;
			adapter.DeleteCommand.CommandTimeout = 0;
			adapter.Update(dataToUpdate);
			adapter.SelectCommand.Transaction = null;
		}
		finally
		{
			if (needToClose)
			{
				connection.Close();
			}
		}
		return true;
	}

	public SqlDataReader ExecuteReader(SqlConnection sqlConnection, M1User m1User, string databaseName, SqlCommand sqlCommand)
	{
		bool needToClose = true;
		sqlCommand.Connection = GetConnection(m1User, databaseName, openImmediately: true, sqlConnection, null, ref needToClose);
		sqlCommand.CommandTimeout = 0;
		try
		{
			return sqlCommand.ExecuteReader();
		}
		catch (SqlException exc)
		{
			SqlConnection sqlConnection2 = sqlCommand.Connection;
			try
			{
				checkForDroppedConnection(exc, m1User, databaseName, ref sqlConnection2, null, leaveOpenOnReturn: true);
				sqlCommand.Connection = sqlConnection2;
				return sqlCommand.ExecuteReader();
			}
			catch
			{
				if (needToClose && sqlConnection2 != null && sqlConnection2.State != ConnectionState.Closed)
				{
					sqlConnection2.Close();
				}
				throw;
			}
		}
		catch (InvalidOperationException)
		{
			if (sqlCommand.Connection.State == ConnectionState.Broken || sqlCommand.Connection.State == ConnectionState.Closed)
			{
				ClearAllPools();
				sqlCommand.Connection = GetConnection(m1User, databaseName, openImmediately: true, sqlConnection, null, ref needToClose);
				return sqlCommand.ExecuteReader();
			}
			throw;
		}
	}

	public void ExecuteSQLBulkCopy(DataTable dtInput, string DestinationTable, SqlConnection sqlConnection, M1User m1User, string databaseName, SqlTransaction sqlTransaction)
	{
		bool needToClose = true;
		try
		{
			SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(GetConnection(m1User, databaseName, openImmediately: true, sqlConnection, sqlTransaction, ref needToClose));
			sqlBulkCopy.DestinationTableName = DestinationTable;
			sqlBulkCopy.WriteToServer(dtInput);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public int ExecuteCommand(SqlConnection sqlConnection, M1User m1User, string databaseName, SqlCommand sqlCommand, SqlTransaction sqlTransaction)
	{
		int result = 0;
		bool needToClose = true;
		sqlCommand.Connection = GetConnection(m1User, databaseName, openImmediately: true, sqlConnection, sqlTransaction, ref needToClose);
		sqlCommand.CommandTimeout = 0;
		sqlCommand.Transaction = sqlTransaction;
		try
		{
			result = sqlCommand.ExecuteNonQuery();
		}
		catch (SqlException exc)
		{
			SqlConnection sqlConnection2 = sqlCommand.Connection;
			checkForDroppedConnection(exc, m1User, databaseName, ref sqlConnection2, null, leaveOpenOnReturn: true);
			sqlCommand.Connection = sqlConnection2;
			result = sqlCommand.ExecuteNonQuery();
		}
		catch (InvalidOperationException)
		{
			if (sqlCommand.Connection.State != ConnectionState.Broken && sqlCommand.Connection.State != ConnectionState.Closed)
			{
				throw;
			}
			ClearAllPools();
			sqlCommand.Connection = GetConnection(m1User, databaseName, openImmediately: true, sqlConnection, sqlTransaction, ref needToClose);
			result = sqlCommand.ExecuteNonQuery();
		}
		finally
		{
			if (needToClose && sqlCommand.Connection.State != ConnectionState.Closed)
			{
				sqlCommand.Connection.Close();
			}
		}
		return result;
	}

	public void BeginExecuteCommand(SqlCommand command, M1CommandCallback callback, Control callbackControl, M1ErrorCallback errorCallback)
	{
		try
		{
			command.BeginExecuteNonQuery(executeCommandCallback, new commandCallbackObject(callback, callbackControl, command, errorCallback));
		}
		catch (Exception ex)
		{
			command.Connection.Close();
			throw ex;
		}
	}

	private void executeCommandCallback(IAsyncResult result)
	{
		commandCallbackObject commandCallbackObject2 = (commandCallbackObject)result.AsyncState;
		try
		{
			int num = commandCallbackObject2.command.EndExecuteNonQuery(result);
			if (commandCallbackObject2.callback != null)
			{
				if (commandCallbackObject2.callbackControl != null)
				{
					commandCallbackObject2.callbackControl.Invoke(commandCallbackObject2.callback, num);
				}
				else
				{
					commandCallbackObject2.callback(num);
				}
			}
		}
		catch (SqlException ex)
		{
			try
			{
				if (commandCallbackObject2.callbackControl != null)
				{
					commandCallbackObject2.callbackControl.Invoke(commandCallbackObject2.errorCallback, ex);
				}
				else
				{
					commandCallbackObject2.errorCallback(ex);
				}
			}
			catch
			{
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			commandCallbackObject2.command.Connection.Close();
		}
	}

	public void BeginExecuteReader(SqlCommand command, M1ReaderCallback callback, Control callbackControl, M1ErrorCallback errorCallback)
	{
		try
		{
			command.BeginExecuteReader(executeReaderCallback, new readerCallbackObject(callback, callbackControl, command, errorCallback));
		}
		catch (Exception ex)
		{
			command.Connection.Close();
			throw ex;
		}
	}

	private void executeReaderCallback(IAsyncResult result)
	{
		readerCallbackObject readerCallbackObject2 = (readerCallbackObject)result.AsyncState;
		try
		{
			SqlDataReader sqlDataReader = (sqlDataReader = readerCallbackObject2.command.EndExecuteReader(result));
			if (readerCallbackObject2.callback != null)
			{
				if (readerCallbackObject2.callbackControl != null)
				{
					readerCallbackObject2.callbackControl.Invoke(readerCallbackObject2.callback, sqlDataReader);
				}
				else
				{
					readerCallbackObject2.callback(sqlDataReader);
				}
			}
		}
		catch (SqlException ex)
		{
			try
			{
				if (readerCallbackObject2.callbackControl != null)
				{
					readerCallbackObject2.callbackControl.Invoke(readerCallbackObject2.errorCallback, ex);
				}
				else
				{
					readerCallbackObject2.errorCallback(ex);
				}
			}
			catch
			{
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			readerCallbackObject2.command.Connection.Close();
		}
	}

	public void BeginExecuteScalar(SqlCommand command, M1ScalarCallback callback, Control callbackControl, M1ErrorCallback errorCallback)
	{
		try
		{
			command.BeginExecuteReader(executeScalarCallback, new scalerCallbackObject(callback, callbackControl, command, errorCallback));
		}
		catch (Exception ex)
		{
			command.Connection.Close();
			throw ex;
		}
	}

	private void executeScalarCallback(IAsyncResult result)
	{
		scalerCallbackObject scalerCallbackObject2 = (scalerCallbackObject)result.AsyncState;
		try
		{
			SqlDataReader sqlDataReader = scalerCallbackObject2.command.EndExecuteReader(result);
			object obj = null;
			if (sqlDataReader.HasRows && sqlDataReader.FieldCount > 0)
			{
				sqlDataReader.Read();
				obj = sqlDataReader.GetValue(0);
			}
			if (scalerCallbackObject2.callback != null)
			{
				if (scalerCallbackObject2.callbackControl != null)
				{
					scalerCallbackObject2.callbackControl.Invoke(scalerCallbackObject2.callback, obj);
				}
				else
				{
					scalerCallbackObject2.callback(obj);
				}
			}
		}
		catch (SqlException ex)
		{
			try
			{
				if (scalerCallbackObject2.callbackControl != null)
				{
					scalerCallbackObject2.callbackControl.Invoke(scalerCallbackObject2.errorCallback, ex);
				}
				else
				{
					scalerCallbackObject2.errorCallback(ex);
				}
			}
			catch
			{
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			scalerCallbackObject2.command.Connection.Close();
		}
	}

	public int ExecuteCommand(SqlConnection sqlConnection, M1User m1User, string databaseName, string queryString)
	{
		return ExecuteCommand(sqlConnection, m1User, databaseName, queryString, null);
	}

	public SqlCommand NewSqlCommand(SqlConnection sqlConnection, M1User m1User, string databaseName, string queryString)
	{
		bool needToClose = true;
		return new SqlCommand(queryString, GetConnection(m1User, databaseName, openImmediately: false, sqlConnection, null, ref needToClose))
		{
			CommandTimeout = 0
		};
	}

	public object ExecuteScalar(SqlConnection sqlConnection, M1User m1User, string databaseName, string queryString)
	{
		return ExecuteScalar(sqlConnection, m1User, databaseName, queryString, null);
	}

	public object ExecuteScalar(SqlConnection sqlConnection, M1User m1User, string databaseName, string queryString, SqlTransaction sqlTransaction)
	{
		using SqlCommand sqlCommand = new SqlCommand(queryString);
		sqlCommand.CommandTimeout = 0;
		return ExecuteScalar(sqlConnection, m1User, databaseName, sqlCommand, sqlTransaction);
	}

	public object ExecuteScalar(SqlConnection sqlConnection, M1User m1User, string databaseName, SqlCommand sqlCommand)
	{
		return ExecuteScalar(sqlConnection, m1User, databaseName, sqlCommand, null);
	}

	public object ExecuteScalar(SqlConnection sqlConnection, M1User m1User, string databaseName, SqlCommand sqlCommand, SqlTransaction sqlTransaction)
	{
		object result = null;
		bool needToClose = true;
		sqlCommand.Connection = GetConnection(m1User, databaseName, openImmediately: true, sqlConnection, sqlTransaction, ref needToClose);
		sqlCommand.CommandTimeout = 0;
		sqlCommand.Transaction = sqlTransaction;
		try
		{
			result = sqlCommand.ExecuteScalar();
		}
		catch (SqlException exc)
		{
			SqlConnection sqlConnection2 = sqlCommand.Connection;
			checkForDroppedConnection(exc, m1User, databaseName, ref sqlConnection2, null, leaveOpenOnReturn: true);
			sqlCommand.Connection = sqlConnection2;
			result = sqlCommand.ExecuteScalar();
		}
		catch (InvalidOperationException)
		{
			if (sqlCommand.Connection.State != ConnectionState.Broken && sqlCommand.Connection.State != ConnectionState.Closed)
			{
				throw;
			}
			ClearAllPools();
			sqlCommand.Connection = GetConnection(m1User, databaseName, openImmediately: true, sqlConnection, sqlTransaction, ref needToClose);
			result = sqlCommand.ExecuteScalar();
		}
		finally
		{
			if (needToClose && sqlCommand.Connection.State != ConnectionState.Closed)
			{
				sqlCommand.Connection.Close();
			}
		}
		return result;
	}

	public int ExecuteCommand(SqlConnection sqlConnection, M1User m1User, string databaseName, string queryString, SqlTransaction sqlTransaction)
	{
		SqlCommand sqlCommand = new SqlCommand(queryString);
		sqlCommand.CommandTimeout = 0;
		return ExecuteCommand(sqlConnection, m1User, databaseName, sqlCommand, sqlTransaction);
	}

	public void SetSingleUserMode(M1User m1User, string databaseName, bool turnOn)
	{
		if (singleUserConnection == null)
		{
			singleUserConnection = GetConnection(m1User, databaseName, openImmediately: true);
			singleUserDatabaseName = databaseName;
		}
		SqlCommand sqlCommand = NewSqlCommand(singleUserConnection, m1User, databaseName, string.Empty);
		if (turnOn)
		{
			ClearAllPools();
			sqlCommand.CommandText = $"ALTER DATABASE {databaseName.ToString()} Set SINGLE_USER WITH ROLLBACK IMMEDIATE";
			sqlCommand.ExecuteNonQuery();
		}
		else
		{
			sqlCommand.CommandText = $"ALTER DATABASE {databaseName.ToString()} Set MULTI_USER";
			sqlCommand.ExecuteNonQuery();
			singleUserConnection.Close();
			singleUserConnection = null;
			singleUserDatabaseName = string.Empty;
		}
	}

	public string Decrypt(string Text)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		int num = 0;
		int num2 = 0;
		string value = string.Empty;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		StringBuilder stringBuilder = null;
		Text = Text.Trim();
		num = Text.Length;
		empty2 = String1;
		empty = BaseString;
		stringBuilder = new StringBuilder();
		for (num2 = 0; num2 < Text.Length; num2++)
		{
			if (num2 != 2 && num2 != 3)
			{
				num6 += empty.IndexOf(Text.Substring(num2, 1), 0, StringComparison.CurrentCulture) + 1;
			}
		}
		if (Text.Length >= 1)
		{
			value = Text.ToString().Substring(0, 1);
			Text = Text.Substring(1);
		}
		num4 = String1.IndexOf(value, 0, StringComparison.CurrentCulture) + 1;
		if (Text.Length >= 1)
		{
			value = Text.ToString().Substring(0, 1);
			Text = Text.Substring(1);
		}
		empty2 = empty2.Substring(num4 - 1) + empty2.Substring(0, num4 - 1);
		num = empty2.IndexOf(value, 0, StringComparison.CurrentCulture) + 1;
		if (num < 0)
		{
			Console.WriteLine("Invalid character in password.");
			return string.Empty;
		}
		if (Text.Length >= 1)
		{
			value = Text.ToString().Substring(0, 1);
			Text = Text.Substring(1);
		}
		empty2 = empty2.Substring(num4 - 1) + empty2.Substring(0, num4 - 1);
		num5 = empty2.IndexOf(value, 0, StringComparison.CurrentCulture) + 1;
		num5 = (num5 - 1) * 92;
		if (Text.Length >= 1)
		{
			value = Text.ToString().Substring(0, 1);
			Text = Text.Substring(1);
		}
		empty2 = empty2.Substring(num4 - 1) + empty2.Substring(0, num4 - 1);
		num5 += empty2.IndexOf(value, 0, StringComparison.CurrentCulture);
		if (num6 != num5)
		{
			Console.WriteLine("Invalid password.");
			return string.Empty;
		}
		for (num2 = 1; num2 < num; num2++)
		{
			empty2 = empty2.Substring(num4 - 1) + empty2.Substring(0, num4 - 1);
			value = Text.Substring(0, 1);
			Text = Text.Substring(1);
			num3 = empty2.IndexOf(value, 0, StringComparison.CurrentCulture) + 1;
			if (num3 > 0)
			{
				value = empty.Substring(num3 - 1, 1);
				stringBuilder.Append(value);
				continue;
			}
			Console.WriteLine("Invalid character {0} in decryption function", value);
			return string.Empty;
		}
		return stringBuilder.ToString();
	}

	private string DecryptMeta(string cipherText, string passPhrase)
	{
		try
		{
			byte[] array = Convert.FromBase64String(cipherText);
			byte[] salt = array.Take(32).ToArray();
			byte[] rgbIV = array.Skip(32).Take(32).ToArray();
			byte[] array2 = array.Skip(64).Take(array.Length - 64).ToArray();
			using Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passPhrase, salt, 1000);
			byte[] bytes = rfc2898DeriveBytes.GetBytes(32);
			using RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.BlockSize = 256;
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.Padding = PaddingMode.PKCS7;
			using ICryptoTransform transform = rijndaelManaged.CreateDecryptor(bytes, rgbIV);
			using MemoryStream memoryStream = new MemoryStream(array2);
			using CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
			byte[] array3 = new byte[array2.Length];
			int count = cryptoStream.Read(array3, 0, array3.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(array3, 0, count);
		}
		catch
		{
			return cipherText;
		}
	}

	public string Encrypt(string Text, int MinLength)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		int num = 0;
		int num2 = 0;
		string empty3 = string.Empty;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		string empty4 = string.Empty;
		StringBuilder stringBuilder = null;
		Random random = new Random();
		double num8 = 0.0;
		stringBuilder = new StringBuilder();
		Text = Text.Trim();
		num = Text.Length;
		empty = BaseString;
		empty2 = String1;
		num8 = random.NextDouble();
		num4 = (int)(63.0 * num8 + 1.0);
		stringBuilder.Append(String1.Substring(num4 - 1, 1));
		empty2 = empty2.Substring(num4 - 1) + empty2.Substring(0, num4 - 1);
		stringBuilder.Append(empty2.Substring(num, 1));
		empty2 = empty2.Substring(num4 - 1) + empty2.Substring(0, num4 - 1);
		empty4 = empty2;
		empty2 = empty2.Substring(num4 - 1) + empty2.Substring(0, num4 - 1);
		for (num2 = 1; num2 <= num; num2++)
		{
			empty2 = empty2.Substring(num4 - 1) + empty2.Substring(0, num4 - 1);
			empty3 = Text.Substring(0, 1);
			Text = Text.Substring(1);
			num3 = empty.IndexOf(empty3, 0, StringComparison.CurrentCulture) + 1;
			if (num3 > 0)
			{
				empty3 = empty2.Substring(num3 - 1, 1);
				stringBuilder.Append(empty3);
				continue;
			}
			Console.WriteLine("Invalid character {0} in encryption function", empty3);
			return string.Empty;
		}
		num = stringBuilder.Length;
		for (num2 = num + 1; num2 <= MinLength - 2; num2++)
		{
			empty2 = empty2.Substring(num4 - 1) + empty2.Substring(0, num4 - 1);
			stringBuilder.Append(empty2.Substring((int)(93.0 * num8 + 1.0) - 1, 1));
		}
		num5 = 0;
		for (num2 = 1; num2 <= stringBuilder.Length; num2++)
		{
			empty3 = stringBuilder.ToString().Substring(num2 - 1, 1);
			num3 = empty.IndexOf(empty3, 0, StringComparison.CurrentCulture) + 1;
			num5 += num3;
		}
		num6 = num5 / 92;
		num7 = num5 - num6 * 92;
		empty2 = empty4;
		empty3 = empty2.Substring(num6, 1);
		stringBuilder.Insert(2, empty3);
		empty2 = empty2.Substring(num4 - 1) + empty2.Substring(0, num4 - 1);
		empty3 = empty2.Substring(num7, 1);
		stringBuilder.Insert(3, empty3);
		return stringBuilder.ToString();
	}
}
