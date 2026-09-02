using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using M1.Core.Script;
using M1.Extensions;

namespace M1.Core;

public class M1Database : IServiceProvider, IServiceContainer
{
	public LoginCredentials LoginCredentials = new LoginCredentials();

	private ServerManager serverManager;

	private M1User currentUser;

	private M1DataDictionary currentDataDictionary;

	protected AppContext currentContext;

	public M1Security Security;

	public DateTime LastActivityTime = DateTime.Now;

	public bool IsLoggingOut;

	public bool IsLoggingIn;

	public bool KeepOpen;

	private string _ID = string.Empty;

	private string _Region = string.Empty;

	public TimeFormatType TimeFormat = TimeFormatType.TwentyFourHour;

	private DataSet tablesDataset = new DataSet();

	private ExplorerItemCollection _ExplorerItems;

	private ExplorerItemCollection _ShortcutItems;

	private ExplorerItemCollection _HelpItems;

	public bool IsOpen;

	public int MaxGridRow;

	public string HomeCurrencyDescription = string.Empty;

	public string HomeCurrencySymbol = string.Empty;

	private string _HomeCurrencyID = string.Empty;

	public string SystemCurrencySymbol = string.Empty;

	public bool M1HomeEnabled;

	private Dictionary<string, object> _Formatters = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);

	private NextIDList _NextIDs;

	private ExplorerItemCollection _CustomFormItems;

	private ExplorerItemCollection _CustomReportItems;

	private string _CompanyMessage;

	private bool? _AllowEditInExplorer;

	private bool _ReadOnlyLogin;

	private string _UserEmailAddress;

	private string _LanguageTable = string.Empty;

	private bool _InUserChangeEvent;

	private Scripting _Scripting;

	private ScriptingBase _ScriptingQuick;

	protected ServiceContainer serviceContainer;

	public Guid ClientID { get; private set; }

	public string Description { get; private set; }

	public string HomeCurrencyID
	{
		get
		{
			return _HomeCurrencyID;
		}
		set
		{
			_HomeCurrencyID = value;
			HomeCurrencySymbol = string.Empty;
			HomeCurrencyDescription = string.Empty;
			if (HomeCurrencyID.Length == 0)
			{
				return;
			}
			using SqlCommand sqlCommand = NewSqlCommand("SELECT mcpSymbol,mcpDescription FROM CurrencyRates WHERE mcpCurrencyRateID = @CurrencyID");
			sqlCommand.Parameters.Add(new SqlParameter("@CurrencyID", SqlDbType.NVarChar)).Value = HomeCurrencyID;
			DataTable dataTable = GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				HomeCurrencySymbol = dataTable.Rows[0].Field<string>("mcpSymbol").Trim();
				HomeCurrencyDescription = dataTable.Rows[0].Field<string>("mcpDescription").Trim();
			}
		}
	}

	public Dictionary<string, object> Formatters
	{
		get
		{
			return _Formatters;
		}
		set
		{
			_Formatters = value;
		}
	}

	public NextIDList NextIDs
	{
		get
		{
			if (_NextIDs == null)
			{
				_NextIDs = new NextIDList(this);
			}
			return _NextIDs;
		}
	}

	public ExplorerItemCollection CustomFormItems
	{
		get
		{
			if (_CustomFormItems == null && IsOpen)
			{
				_CustomFormItems = new ExplorerItemCollection(currentUser, this, currentDataDictionary, currentContext, "TREE");
				_CustomFormItems.LoadCustomForms();
			}
			return _CustomFormItems;
		}
	}

	public ExplorerItemCollection CustomReportItems
	{
		get
		{
			if (_CustomReportItems == null && IsOpen)
			{
				_CustomReportItems = new ExplorerItemCollection(currentUser, this, currentDataDictionary, currentContext, "TREE");
				_CustomReportItems.LoadCustomReports();
			}
			return _CustomReportItems;
		}
	}

	public ExplorerItemCollection ExplorerItems
	{
		get
		{
			if (_ExplorerItems == null && IsOpen)
			{
				_ExplorerItems = new ExplorerItemCollection(currentUser, this, currentDataDictionary, currentContext, "TREE");
				_ExplorerItems.LoadItems();
			}
			return _ExplorerItems;
		}
		private set
		{
			_ExplorerItems = value;
		}
	}

	public ExplorerItemCollection ShortcutItems
	{
		get
		{
			if (_ShortcutItems == null && IsOpen)
			{
				_ShortcutItems = new ExplorerItemCollection(currentUser, this, currentDataDictionary, currentContext, "SBAR");
				_ShortcutItems.LoadItems();
			}
			return _ShortcutItems;
		}
		private set
		{
			_ShortcutItems = value;
		}
	}

	public string CompanyMessage
	{
		get
		{
			if (_CompanyMessage == null)
			{
				_CompanyMessage = Props("DatasetProperties").Field<string>("xadCompanyMessageText");
				if (_CompanyMessage == null)
				{
					_CompanyMessage = string.Empty;
				}
			}
			return _CompanyMessage;
		}
		set
		{
			_CompanyMessage = value;
			OnCompanyMessageChanged(EventArgs.Empty);
		}
	}

	public bool AllowEditInExplorer
	{
		get
		{
			if (!_AllowEditInExplorer.HasValue)
			{
				_AllowEditInExplorer = Props("DatasetProperties").Field<bool>("xadEditInExplorers");
			}
			return _AllowEditInExplorer.Value;
		}
	}

	public string Region
	{
		get
		{
			return _Region;
		}
		private set
		{
			_Region = value;
		}
	}

	public string ID
	{
		get
		{
			return _ID;
		}
		set
		{
			_ID = value;
		}
	}

	public M1User User
	{
		get
		{
			return currentUser;
		}
		set
		{
			currentUser = value;
		}
	}

	public bool ReadOnlyLogin => _ReadOnlyLogin;

	public string UserEmailAddress
	{
		get
		{
			if (_UserEmailAddress == null)
			{
				if (string.IsNullOrWhiteSpace(User.Settings.ProviderEmailAddress) || User.Settings.ProviderEmailAddress.IndexOf('@') == -1)
				{
					SqlCommand sqlCommand = NewSqlCommand("select TOP 1 lmeWorkEmailAddress from Employees where lmeUserID = @UserID and lmeTerminationDate IS NULL order by lmeEmployeeID");
					sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = User.ID;
					_UserEmailAddress = Convert.ToString(ExecuteScalar(sqlCommand));
				}
				else
				{
					_UserEmailAddress = User.Settings.ProviderEmailAddress;
				}
			}
			return _UserEmailAddress;
		}
		set
		{
			_UserEmailAddress = value;
		}
	}

	public string LanguageTable
	{
		get
		{
			return _LanguageTable;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				_LanguageTable = string.Empty;
			}
			else if (!_LanguageTable.Equals(value, StringComparison.CurrentCultureIgnoreCase) && currentDataDictionary.Language.DoesLanguageTableExist(value))
			{
				_LanguageTable = value;
			}
		}
	}

	public bool InUserChangeEvent
	{
		get
		{
			return _InUserChangeEvent;
		}
		set
		{
			_InUserChangeEvent = value;
		}
	}

	public Scripting Scripting
	{
		get
		{
			if (_Scripting == null)
			{
				_Scripting = new Scripting(this);
			}
			return _Scripting;
		}
	}

	public ScriptingBase ScriptingQuick
	{
		get
		{
			if (_ScriptingQuick == null)
			{
				_ScriptingQuick = new ScriptingBase(this);
				_ScriptingQuick.LoadEnvironment();
			}
			return _ScriptingQuick;
		}
	}

	public event EventHandler<ForeignKeyInvalidEventArgs> ForeignKeyInvalid;

	public event EventHandler<ShowErrorEventArgs> ShowError;

	public event EventHandler CompanyMessageChanged;

	public event EventHandler PropsRefreshed;

	public event EventHandler<LoggingOutEventArgs> LoggingOut;

	public event EventHandler<DatabaseLoginEventArgs> LoggedOut;

	public event EventHandler<TableChangedEventArgs> TableChanged;

	public event EventHandler<EmailMessageSentEventArgs> EmailMessageSent;

	public event EventHandler<ConstructWhereEventArgs> ConstructWhere;

	public void OnForeignKeyInvalid(ForeignKeyInvalidEventArgs e)
	{
		this.ForeignKeyInvalid?.Invoke(this, e);
	}

	public void OnShowError(ShowErrorEventArgs e)
	{
		this.ShowError?.Invoke(this, e);
	}

	public void OnCompanyMessageChanged(EventArgs e)
	{
		this.CompanyMessageChanged?.Invoke(this, e);
	}

	public bool CheckHomeCurrency(string currencyID)
	{
		currencyID = ((currencyID == null) ? string.Empty : currencyID.Trim());
		if (currencyID.Length != 0)
		{
			return HomeCurrencyID.Equals(currencyID, StringComparison.CurrentCultureIgnoreCase);
		}
		return true;
	}

	public M1Database(IServiceProvider parentProvider, ServerManager m1ServerManager)
	{
		serviceContainer = new ServiceContainer(parentProvider);
		if (GetType() == typeof(M1Database))
		{
			serviceContainer.AddService(typeof(M1Database), this);
		}
		currentUser = (M1User)parentProvider.GetService(typeof(M1User));
		currentContext = (AppContext)parentProvider.GetService(typeof(AppContext));
		currentDataDictionary = (M1DataDictionary)parentProvider.GetService(typeof(M1DataDictionary));
		if (currentDataDictionary == null && this is M1DataDictionary)
		{
			currentDataDictionary = (M1DataDictionary)this;
		}
		serverManager = m1ServerManager;
		Security = new M1Security(currentUser, this, currentDataDictionary, currentContext);
	}

	public DataRow Props(string module)
	{
		DataTable dataTable = null;
		string empty = string.Empty;
		module = module.Trim().ToLower();
		empty = ((module.Length <= 2) ? currentDataDictionary.Modules[module].PropertiesTable : module);
		if (empty.Length != 0)
		{
			if (!tablesDataset.Tables.Contains(empty))
			{
				DataTable dataTable2 = GetDataTable($"select * from {empty}");
				dataTable2.TableName = empty;
				tablesDataset.Tables.Add(dataTable2);
				dataTable = tablesDataset.Tables[empty];
				if (dataTable.Rows.Count == 0)
				{
					dataTable.AddBlankRow(allowNullForDefaultValue: false);
				}
			}
			if (tablesDataset.Tables.Contains(empty))
			{
				dataTable = tablesDataset.Tables[empty];
			}
			return dataTable.Rows[0];
		}
		return null;
	}

	public void PropsRefresh()
	{
		tablesDataset.Tables.Clear();
		loadSettings();
		checkForLanguage();
		Security.ClearCache();
		this.PropsRefreshed?.Invoke(this, EventArgs.Empty);
	}

	private void loadSettings()
	{
		DataRow row = Props("DatasetProperties");
		Region = row.Field<string>("xadRegion").Trim();
		if (row.Field<byte>("xadTimeFormat") == 2)
		{
			TimeFormat = TimeFormatType.AmPm;
		}
		else
		{
			TimeFormat = TimeFormatType.TwentyFourHour;
		}
		HomeCurrencyID = row.Field<string>("xadCurrencyRateID").Trim();
		MaxGridRow = row.Field<int>("xadMaxGridRow");
		M1HomeEnabled = row.Field<bool>("xadEnableM1Home");
	}

	public string GetForeignCurrencySymbolForRateId(string rateId)
	{
		string result = string.Empty;
		if (rateId.Length == 0 || rateId.Equals(HomeCurrencyID))
		{
			result = HomeCurrencySymbol;
		}
		else
		{
			using SqlCommand sqlCommand = NewSqlCommand("Select mcpSymbol From CurrencyRates Where mcpCurrencyRateID = @CurrencyID");
			sqlCommand.Parameters.Add(new SqlParameter("@CurrencyID", SqlDbType.NVarChar)).Value = rateId;
			object obj = ExecuteScalar(sqlCommand);
			if (obj != null && obj is string)
			{
				result = ((string)obj).Trim();
			}
		}
		return result;
	}

	public void OnLoggingOut(LoggingOutEventArgs e)
	{
		this.LoggingOut?.Invoke(this, e);
	}

	public void OnLoggedOut(DatabaseLoginEventArgs e)
	{
		this.LoggedOut?.Invoke(this, e);
	}

	public bool Logout()
	{
		if (ID.Length != 0)
		{
			IsLoggingOut = true;
			try
			{
				LoggingOutEventArgs e = new LoggingOutEventArgs();
				OnLoggingOut(e);
				if (e.Cancel)
				{
					return false;
				}
				if (Scripting != null)
				{
					Scripting.RunCustomAppCode(currentDataDictionary, "App_Logout");
				}
				currentUser.DeleteUserFromLog(this);
			}
			finally
			{
				IsLoggingOut = false;
			}
		}
		OnLoggedOut(new DatabaseLoginEventArgs(User, this));
		if (_Scripting != null)
		{
			_Scripting.Dispose();
			_Scripting = null;
		}
		if (GetService(typeof(ScriptApp)) is ScriptApp scriptApp)
		{
			RemoveService(typeof(ScriptApp));
			scriptApp.CloseComConnection();
			scriptApp.Dispose();
			ScriptApp scriptApp2 = null;
		}
		serverManager.ClearPool(currentUser, ID);
		currentUser = null;
		this.CompanyMessageChanged = null;
		Close();
		return true;
	}

	public void QuickExit()
	{
		if (GetService(typeof(ScriptApp)) is ScriptApp scriptApp)
		{
			RemoveService(typeof(ScriptApp));
			scriptApp.CloseComConnection();
			scriptApp.Dispose();
			ScriptApp scriptApp2 = null;
		}
	}

	public virtual void Close()
	{
		if (_ExplorerItems != null)
		{
			_ExplorerItems.Clear();
			_ExplorerItems = null;
		}
		if (_ShortcutItems != null)
		{
			_ShortcutItems.Clear();
			_ShortcutItems = null;
		}
		if (_CustomFormItems != null)
		{
			_CustomFormItems.Clear();
			_CustomFormItems = null;
		}
		if (_CustomReportItems != null)
		{
			_CustomReportItems.Clear();
			_CustomReportItems = null;
		}
		Description = string.Empty;
		ID = string.Empty;
		LoginCredentials = new LoginCredentials();
		IsOpen = false;
		_CompanyMessage = null;
		_AllowEditInExplorer = null;
		this.PropsRefreshed = null;
	}

	public string PrepareQuery(string queryString)
	{
		int num = 0;
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		object obj = new object();
		while ((num = queryString.IndexOf("{!")) > 0)
		{
			empty = queryString.Substring(0, num);
			empty2 = queryString.Substring(num + 2);
			num = empty2.IndexOf("!}");
			if (num <= 0)
			{
				break;
			}
			empty3 = empty2.Substring(0, num);
			empty2 = empty2.Substring(num + 2);
			obj = ScriptingQuick.Eval(empty3);
			queryString = empty + obj.ToString() + empty2;
		}
		return queryString;
	}

	public void CommitTransaction(SqlTransaction sqlTransaction)
	{
		SqlConnection connection = sqlTransaction.Connection;
		sqlTransaction.Commit();
		if (serverManager.singleUserConnection != connection)
		{
			connection.Close();
		}
	}

	public void RollbackTransaction(SqlTransaction sqlTransaction)
	{
		SqlConnection connection = sqlTransaction.Connection;
		sqlTransaction.Rollback();
		if (serverManager.singleUserConnection != connection)
		{
			connection.Close();
		}
	}

	public SqlTransaction BeginTransaction()
	{
		return serverManager.GetConnection(currentUser, ID, openImmediately: true)?.BeginTransaction();
	}

	public DataSet GetDataSet(string queryString)
	{
		return serverManager.GetDataSet(null, currentUser, ID, queryString);
	}

	public DataTable GetDataTable(SqlCommand sqlCommand)
	{
		SqlDataAdapter adapter = null;
		return serverManager.GetDataTable(null, currentUser, ID, MaxGridRow, sqlCommand, fillSchema: false, out adapter);
	}

	public DataTable GetDataTable(SqlCommand sqlCommand, SqlTransaction sqlTransaction)
	{
		SqlDataAdapter adapter = null;
		return serverManager.GetDataTable(null, currentUser, ID, MaxGridRow, sqlCommand, fillSchema: false, out adapter, sqlTransaction);
	}

	public DataTable GetDataTable(SqlCommand sqlCommand, bool fillSchema, out SqlDataAdapter adapter)
	{
		return serverManager.GetDataTable(null, currentUser, ID, MaxGridRow, sqlCommand, fillSchema: false, out adapter);
	}

	public DataTable GetDataTable(SqlCommand sqlCommand, bool fillSchema, out SqlDataAdapter adapter, SqlTransaction sqlTransaction)
	{
		return serverManager.GetDataTable(null, currentUser, ID, MaxGridRow, sqlCommand, fillSchema: false, out adapter, sqlTransaction);
	}

	public DataTable GetDataTable(string queryString)
	{
		return serverManager.GetDataTable(null, currentUser, ID, MaxGridRow, queryString);
	}

	public DataTable GetDataTable(string queryString, SqlTransaction sqlTransaction)
	{
		SqlDataAdapter adapter;
		return serverManager.GetDataTable(null, currentUser, ID, MaxGridRow, queryString, fillSchema: false, out adapter, sqlTransaction);
	}

	public DataTable GetDataTable(string queryString, bool fillSchema)
	{
		return serverManager.GetDataTable(null, currentUser, ID, MaxGridRow, queryString, fillSchema);
	}

	public DataTable GetDataTable(string queryString, bool fillSchema, out SqlDataAdapter adapter)
	{
		return serverManager.GetDataTable(null, currentUser, ID, MaxGridRow, queryString, fillSchema, out adapter);
	}

	public DataTable GetDataTable(string queryString, bool fillSchema, out SqlDataAdapter adapter, SqlTransaction sqlTransaction)
	{
		return serverManager.GetDataTable(sqlTransaction?.Connection, currentUser, ID, MaxGridRow, queryString, fillSchema, out adapter, sqlTransaction);
	}

	public SqlDataReader ExecuteReader(SqlCommand sqlCommand)
	{
		return serverManager.ExecuteReader(null, currentUser, ID, sqlCommand);
	}

	public void Fill(DataTable dataTable, SqlCommand sqlCommand)
	{
		SqlDataAdapter adapter = null;
		serverManager.Fill(null, currentUser, ID, dataTable, sqlCommand, fillSchema: false, out adapter, MaxGridRow, null);
	}

	public void Fill(DataTable dataTable, SqlCommand sqlCommand, SqlTransaction transaction)
	{
		SqlDataAdapter adapter = null;
		serverManager.Fill(null, currentUser, ID, dataTable, sqlCommand, fillSchema: false, out adapter, MaxGridRow, transaction);
	}

	public void Fill(DataTable dataTable, string queryString)
	{
		serverManager.Fill(null, currentUser, ID, dataTable, MaxGridRow, queryString);
	}

	public void Fill(DataTable dataTable, string queryString, bool fillSchema)
	{
		serverManager.Fill(null, currentUser, ID, dataTable, MaxGridRow, queryString, fillSchema);
	}

	public void Fill(DataTable dataTable, string queryString, bool fillSchema, out SqlDataAdapter adapter)
	{
		serverManager.Fill(null, currentUser, ID, dataTable, MaxGridRow, queryString, fillSchema, out adapter);
	}

	public void Fill(DataTable dataTable, string queryString, bool fillSchema, out SqlDataAdapter adapter, SqlTransaction transaction)
	{
		serverManager.Fill(null, currentUser, ID, dataTable, MaxGridRow, queryString, fillSchema, out adapter, transaction);
	}

	public bool UpdateData(DataRow[] dataToUpdate, SqlDataAdapter adapter)
	{
		return UpdateData(dataToUpdate, adapter, null);
	}

	public bool UpdateData(DataRow[] dataToUpdate, SqlDataAdapter adapter, SqlTransaction sqlTransaction)
	{
		return serverManager.UpdateData(null, currentUser, ID, dataToUpdate, adapter, sqlTransaction);
	}

	public bool UpdateData(DataRow[] dataToUpdate, SqlDataAdapter adapter, SqlTransaction sqlTransaction, bool generateCommands)
	{
		return serverManager.UpdateData(null, currentUser, ID, dataToUpdate, adapter, sqlTransaction, generateCommands);
	}

	public bool UpdateData(DataTable dataToUpdate, SqlDataAdapter adapter)
	{
		return UpdateData(dataToUpdate, adapter, null);
	}

	public bool UpdateData(DataTable dataToUpdate, SqlDataAdapter adapter, SqlTransaction sqlTransaction)
	{
		return serverManager.UpdateData(null, currentUser, ID, dataToUpdate, adapter, sqlTransaction);
	}

	public SqlCommand NewSqlCommand(string queryString)
	{
		return serverManager.NewSqlCommand(null, currentUser, ID, queryString);
	}

	public object ExecuteScalar(SqlCommand sqlCommand)
	{
		return ExecuteScalar(sqlCommand, null);
	}

	public object ExecuteScalar(SqlCommand sqlCommand, SqlTransaction sqlTransaction)
	{
		return serverManager.ExecuteScalar(null, currentUser, ID, sqlCommand, sqlTransaction);
	}

	public object ExecuteScalar(string queryString)
	{
		return ExecuteScalar(queryString, null);
	}

	public object ExecuteScalar(string queryString, SqlTransaction sqlTransaction)
	{
		return serverManager.ExecuteScalar(null, currentUser, ID, queryString, sqlTransaction);
	}

	public int ExecuteCommand(SqlCommand sqlCommand)
	{
		return ExecuteCommand(sqlCommand, null);
	}

	public int ExecuteCommand(SqlCommand sqlCommand, SqlTransaction sqlTransaction)
	{
		return serverManager.ExecuteCommand(null, currentUser, ID, sqlCommand, sqlTransaction);
	}

	public int ExecuteCommand(string queryString)
	{
		return ExecuteCommand(queryString, null);
	}

	public int ExecuteCommand(string queryString, SqlTransaction sqlTransaction)
	{
		return serverManager.ExecuteCommand(null, currentUser, ID, queryString, sqlTransaction);
	}

	public void ExecuteBulkCopy(DataTable dtInput, string tablename)
	{
		serverManager.ExecuteSQLBulkCopy(dtInput, tablename, null, currentUser, ID, null);
	}

	public void ExecuteBulkCopy(DataTable dtInput, string tablename, SqlTransaction transaction)
	{
		serverManager.ExecuteSQLBulkCopy(dtInput, tablename, null, User, ID, transaction);
	}

	public void BeginExecuteCommand(SqlCommand command)
	{
		BeginExecuteCommand(command, null, null, null);
	}

	public void BeginExecuteCommand(SqlCommand command, ServerManager.M1CommandCallback callback)
	{
		BeginExecuteCommand(command, callback, null, null);
	}

	public void BeginExecuteCommand(SqlCommand command, ServerManager.M1CommandCallback callback, Control callbackControl)
	{
		BeginExecuteCommand(command, callback, callbackControl, null);
	}

	public void BeginExecuteCommand(SqlCommand command, ServerManager.M1CommandCallback callback, Control callbackControl, ServerManager.M1ErrorCallback errorCallback)
	{
		command.Connection = serverManager.GetConnection(currentUser, ID, openImmediately: true);
		serverManager.BeginExecuteCommand(command, callback, callbackControl, errorCallback);
	}

	public void BeginExecuteReader(SqlCommand command)
	{
		BeginExecuteReader(command, null, null, null);
	}

	public void BeginExecuteReader(SqlCommand command, ServerManager.M1ReaderCallback callback)
	{
		BeginExecuteReader(command, callback, null, null);
	}

	public void BeginExecuteReader(SqlCommand command, ServerManager.M1ReaderCallback callback, Control callbackControl)
	{
		BeginExecuteReader(command, callback, callbackControl, null);
	}

	public void BeginExecuteReader(SqlCommand command, ServerManager.M1ReaderCallback callback, Control callbackControl, ServerManager.M1ErrorCallback errorCallback)
	{
		command.Connection = serverManager.GetConnection(currentUser, ID, openImmediately: true);
		serverManager.BeginExecuteReader(command, callback, callbackControl, errorCallback);
	}

	public void BeginExecuteScalar(SqlCommand command)
	{
		BeginExecuteScalar(command, null, null, null);
	}

	public void BeginExecuteScalar(SqlCommand command, ServerManager.M1ScalarCallback callback)
	{
		BeginExecuteScalar(command, callback, null, null);
	}

	public void BeginExecuteScalar(SqlCommand command, ServerManager.M1ScalarCallback callback, Control callbackControl)
	{
		BeginExecuteScalar(command, callback, callbackControl, null);
	}

	public void BeginExecuteScalar(SqlCommand command, ServerManager.M1ScalarCallback callback, Control callbackControl, ServerManager.M1ErrorCallback errorCallback)
	{
		command.Connection = serverManager.GetConnection(currentUser, ID, openImmediately: true);
		serverManager.BeginExecuteScalar(command, callback, callbackControl, errorCallback);
	}

	public void LoginLite(string newDatabaseFullName, M1User m1User)
	{
		ID = newDatabaseFullName;
		User = m1User;
		ClientID = Guid.NewGuid();
		SystemCurrencySymbol = RegionInfo.CurrentRegion.CurrencySymbol;
		AddService(typeof(ScriptApp), new ScriptApp(this));
		IsOpen = true;
	}

	public void Login(string newDatabaseFullName, M1User m1User, LoginCredentials loginCredentials, bool readOnlyLogin)
	{
		bool isLoggingIn = IsLoggingIn;
		IsLoggingIn = true;
		IsOpen = false;
		User = m1User;
		newDatabaseFullName = newDatabaseFullName.Trim().ToUpper();
		if (newDatabaseFullName.Length == 0)
		{
			throw new ArgumentException("No database was specified.");
		}
		ID = newDatabaseFullName;
		_ReadOnlyLogin = readOnlyLogin;
		Security.Login();
		if (Security.GetDatabaseAccessLevel(SecurityAccessLevel.Default) == SecurityAccessLevel.None)
		{
			throw new M1SecurityException("You do not have permission to access database " + newDatabaseFullName + ".");
		}
		LoginCredentials = loginCredentials;
		ClientID = Guid.NewGuid();
		DatabaseInfo datasetProperties = serverManager.GetDatasetProperties(null, m1User, newDatabaseFullName);
		Description = datasetProperties.Description;
		string versionString = currentDataDictionary.AppExtensions.GetVersionString();
		if (!currentContext.Version.Equals(datasetProperties.Version))
		{
			object[] args = new string[3] { newDatabaseFullName, datasetProperties.Version, currentContext.Version };
			throw new M1LoginInvalidVersionException(string.Format("The database {0} is at version {1}, which is different than this installation of M1 ({2}).", args));
		}
		if (!datasetProperties.ExtensionVersions.Equals(versionString))
		{
			object[] args = new string[1] { newDatabaseFullName };
			throw new M1LoginInvalidVersionException(string.Format("The database {0} has app extensions that need to update the database.", args));
		}
		SystemCurrencySymbol = RegionInfo.CurrentRegion.CurrencySymbol;
		loadSettings();
		checkForLanguage();
		m1User.AddUserToLog(this);
		IsOpen = true;
		IsLoggingIn = isLoggingIn;
		AddService(typeof(ScriptApp), new ScriptApp(this));
		foreach (AppExtension appExtension in currentDataDictionary.AppExtensions)
		{
			Assembly codeAssembly = appExtension.GetCodeAssembly();
			if (!(codeAssembly != null))
			{
				continue;
			}
			Type[] exportedTypes = codeAssembly.GetExportedTypes();
			foreach (Type type in exportedTypes)
			{
				if (typeof(IAppExtensionLogin).IsAssignableFrom(type))
				{
					((IAppExtensionLogin)Activator.CreateInstance(type)).OnLogin(this);
				}
			}
		}
	}

	public void Reload()
	{
		DatabaseInfo datasetProperties = serverManager.GetDatasetProperties(null, currentUser, ID);
		Description = datasetProperties.Description;
		PropsRefresh();
	}

	public void UpgradeToFullLogin()
	{
		if (_ReadOnlyLogin)
		{
			_ReadOnlyLogin = false;
			currentUser.AddUserToLog(this);
		}
	}

	public string CheckDbSizeLimit()
	{
		string dbType = string.Empty;
		if (currentContext.DBServerManager.IsMSDEOrSqlExpress(null, currentUser, ID, ref dbType))
		{
			using DataSet dataSet = GetDataSet("exec sp_spaceused");
			if (dataSet.Tables.Count > 1)
			{
				DataTable dataTable = dataSet.Tables[1];
				double result = 0.0;
				double result2 = 0.0;
				if (dataTable.Rows.Count > 0)
				{
					double.TryParse(dataTable.Rows[0].Field<string>("reserved").Replace("KB", string.Empty).Trim(), out result);
					double.TryParse(dataTable.Rows[0].Field<string>("unused").Replace("KB", string.Empty).Trim(), out result2);
				}
				if (result > 0.0 && dbType.Contains("SQLEXPRESS"))
				{
					double num = 10240000.0;
					double num2 = 0.0;
					num2 = (result - result2) / num;
					if (num2 > 0.8)
					{
						return "*****WARNING*****\rThe database is approximately " + M1Math.Round((decimal)num2 * 100m, 0) + "% of the maximum allowed for this installation. Once the database is full you will need to upgrade to the full version of SQL Server.";
					}
				}
			}
		}
		return string.Empty;
	}

	public string AddDatasetsToQuery(string fields, string fromClause, string whereClause, string databases)
	{
		string selectNormal = "";
		string selectLoadOption = "";
		string extraFields = "";
		if (whereClause == null)
		{
			whereClause = "";
		}
		MakeSelectStatements(fields, fromClause, whereClause, "", "", databases, loadNow: false, fromGrid: true, ref selectNormal, ref selectLoadOption, ref extraFields);
		return selectNormal;
	}

	public void MakeSelectStatements(string fields, string fromClause, string whereClause, string groupClause, string orderClause, string databases, bool loadNow, bool fromGrid, ref string selectNormal, ref string selectLoadOption, ref string extraFields)
	{
		databases = databases.Trim().ToUpper();
		if (databases.Length == 0 || databases == "CURRENT")
		{
			selectNormal = "select " + ((fields.Length == 0) ? "*" : fields) + " FROM " + fromClause + ((whereClause.Length == 0) ? string.Empty : (" WHERE " + whereClause)) + ((groupClause.Length == 0) ? string.Empty : (" GROUP BY " + groupClause)) + ((orderClause.Length == 0) ? string.Empty : (" ORDER BY " + orderClause));
			if (!loadNow)
			{
				whereClause = ((whereClause.Length != 0) ? ("(" + whereClause + ") AND 0=1") : "0=1");
			}
			selectNormal = checkSQLForDatabaseVar(selectNormal, ID);
			selectLoadOption = "select " + ((fields.Length == 0) ? "*" : fields) + " FROM " + fromClause + ((whereClause.Length == 0) ? string.Empty : (" WHERE " + whereClause)) + ((groupClause.Length == 0) ? string.Empty : (" GROUP BY " + groupClause)) + ((orderClause.Length == 0) ? string.Empty : (" ORDER BY " + orderClause));
			selectLoadOption = checkSQLForDatabaseVar(selectLoadOption, ID);
			return;
		}
		string text = string.Empty;
		Array array = new string[0];
		int maxGridRow = MaxGridRow;
		if (databases == "ALL")
		{
			foreach (DatabaseInfo installedDatabase in currentContext.InstalledDatabases)
			{
				text = text + ((text.Length != 0) ? " union all " : string.Empty) + checkSQLForDatabaseVar(addDBToSelect(installedDatabase.Name.ToUpper() + ".dbo.", (fromGrid ? ("Convert(char(5),'" + installedDatabase.Name.ToUpper() + "') As Dataset,") : string.Empty) + fields, fromClause, whereClause, groupClause, maxGridRow), installedDatabase.Name.ToUpper());
			}
		}
		else
		{
			array = databases.Split(',');
			foreach (string item in array)
			{
				if (currentContext.InstalledDatabases.Contains(item))
				{
					text = text + ((text.Length != 0) ? " union all " : string.Empty) + checkSQLForDatabaseVar(addDBToSelect(item.Trim() + ".dbo.", (fromGrid ? ("Convert(char(5),'" + item.Trim() + "') As Dataset,") : string.Empty) + fields, fromClause, whereClause, groupClause, maxGridRow), item.Trim());
				}
			}
		}
		if (MaxGridRow > 0)
		{
			text = "Select Top " + MaxGridRow + " * From (" + text + ") As MultiDatabase";
		}
		text += ((orderClause.Length == 0) ? string.Empty : (" ORDER BY " + orderClause));
		selectNormal = text;
		if (!loadNow)
		{
			whereClause = ((whereClause.Length != 0) ? ("(" + whereClause + ") AND 0=1") : "0=1");
		}
		text = string.Empty;
		if (databases == "ALL")
		{
			foreach (DatabaseInfo installedDatabase2 in currentContext.InstalledDatabases)
			{
				text = text + ((text.Length != 0) ? " union all " : string.Empty) + checkSQLForDatabaseVar(addDBToSelect(installedDatabase2.Name.ToUpper() + ".dbo.", (fromGrid ? ("Convert(char(5),'" + installedDatabase2.Name.ToUpper() + "') As Dataset,") : string.Empty) + fields, fromClause, whereClause, groupClause, maxGridRow), installedDatabase2.Name.ToUpper());
			}
		}
		else
		{
			foreach (string item2 in array)
			{
				if (currentContext.InstalledDatabases.Contains(item2))
				{
					text = text + ((text.Length != 0) ? " union all " : string.Empty) + checkSQLForDatabaseVar(addDBToSelect(item2.Trim().ToUpper() + ".dbo.", (fromGrid ? ("Convert(char(5),'" + item2.Trim().ToUpper() + "') As Dataset,") : string.Empty) + fields, fromClause, whereClause, groupClause, maxGridRow), item2.Trim().ToUpper());
				}
			}
		}
		if (MaxGridRow > 0)
		{
			text = "Select Top " + MaxGridRow + " * From (" + text + ") as MultiDatabase";
		}
		text += ((orderClause.Length == 0) ? string.Empty : (" ORDER BY " + orderClause));
		selectLoadOption = text;
		if (fromGrid)
		{
			extraFields = "Dataset,";
		}
	}

	private static string checkSQLForDatabaseVar(string query, string database)
	{
		return query.Replace("{!current_database!}", database.Trim().ToUpper().ToSql(), caseInsensitive: true);
	}

	private static string addDBToSelect(string database, string fields, string fromClause, string whereClause, string groupClause, int maxRow)
	{
		string empty = string.Empty;
		if (maxRow > 0)
		{
			return " select Top " + maxRow + " " + ((fields.Length == 0) ? "*" : addDBToClause(database, fields)) + addDBToClause(database, " FROM " + fromClause) + ((whereClause.Length == 0) ? string.Empty : (" WHERE " + addDBToClause(database, whereClause))) + ((groupClause.Length == 0) ? string.Empty : (" GROUP BY " + groupClause));
		}
		return " select " + ((fields.Length == 0) ? "*" : addDBToClause(database, fields)) + addDBToClause(database, " FROM " + fromClause) + ((whereClause.Length == 0) ? string.Empty : (" WHERE " + addDBToClause(database, whereClause))) + ((groupClause.Length == 0) ? string.Empty : (" GROUP BY " + groupClause));
	}

	private static string addDBToClause(string database, string clause)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		database = database.Trim();
		clause = clause.Replace("\t", " ");
		for (int num = clause.IndexOf(" FROM ", StringComparison.CurrentCultureIgnoreCase); num >= 0; num = clause.IndexOf(" FROM ", num + 6, StringComparison.CurrentCultureIgnoreCase))
		{
			empty = clause.Substring(0, num + 6);
			empty2 = clause.Substring(num + 6);
			clause = ((!empty2.StartsWith("(")) ? (empty + database + empty2) : (empty + empty2));
		}
		for (int num = clause.IndexOf(" JOIN ", StringComparison.CurrentCultureIgnoreCase); num >= 0; num = clause.IndexOf(" JOIN ", num + 6, StringComparison.CurrentCultureIgnoreCase))
		{
			empty = clause.Substring(0, num + 6);
			empty2 = clause.Substring(num + 6);
			clause = ((!empty2.StartsWith("(")) ? (empty + database + empty2) : (empty + empty2));
		}
		return clause;
	}

	private void checkForLanguage()
	{
		_LanguageTable = string.Empty;
		string text = Props("DatasetProperties").Field<string>("xadLanguage").Trim();
		if (text.Length != 0)
		{
			LanguageTable = text;
		}
	}

	public decimal GetExchangeRate(string currencyRateID, DateTime? dateToUse)
	{
		return GetExchangeRate(currencyRateID, dateToUse, null);
	}

	public decimal GetExchangeRate(string currencyRateID, DateTime? dateToUse, SqlTransaction transaction)
	{
		decimal num = 1m;
		if (Props("DS").Field<bool>("xadEnableMultiCurrency"))
		{
			currencyRateID = currencyRateID.Trim();
			if (currencyRateID.Length != 0)
			{
				DateTime dateTime = (dateToUse.HasValue ? dateToUse.Value : DateTime.Today);
				using SqlCommand sqlCommand = NewSqlCommand("select top 1 mclExchangeRate from CurrencyRateLines where mclCurrencyRateID = @CurrencyID and mclEffectiveDate <= @DateValue order by mclEffectiveDate desc");
				sqlCommand.Parameters.Add(new SqlParameter("@CurrencyID", SqlDbType.NVarChar)).Value = currencyRateID;
				sqlCommand.Parameters.Add(new SqlParameter("@DateValue", SqlDbType.DateTime)).Value = dateTime.Date;
				object obj = ExecuteScalar(sqlCommand, transaction);
				if (obj != null)
				{
					num = Convert.ToDecimal(obj);
					if (num == 0m)
					{
						num = 1m;
					}
				}
			}
		}
		return num;
	}

	public void SetDefaultReportForFolder(string folderName, string reportName)
	{
		SqlCommand sqlCommand = currentDataDictionary.NewSqlCommand("Select * From DDSecurityReports Where drUserID = @UserID And drDataset = @Database And drFolder = @Folder And drReport = ''");
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = currentUser.ID;
		sqlCommand.Parameters.Add(new SqlParameter("@Database", SqlDbType.NVarChar)).Value = ID;
		sqlCommand.Parameters.Add(new SqlParameter("@Folder", SqlDbType.NVarChar)).Value = folderName;
		SqlDataAdapter adapter;
		DataTable dataTable = currentDataDictionary.GetDataTable(sqlCommand, fillSchema: false, out adapter);
		DataRow row;
		if (dataTable.Rows.Count == 0)
		{
			row = dataTable.AddBlankRow();
			row.SetField("drUserID", currentUser.ID);
			row.SetField("drDataset", ID);
			row.SetField("drFolder", folderName);
			row.SetField("drLevel", SecurityAccessLevel.Default);
		}
		else
		{
			row = dataTable.Rows[0];
		}
		reportName = Path.GetFileNameWithoutExtension(reportName);
		row.SetField("drSettings", "DefaultReport=" + reportName);
		currentDataDictionary.UpdateData(dataTable, adapter);
	}

	public string GetDefaultReportForFolder(string folderName)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		using (SqlCommand sqlCommand = currentDataDictionary.NewSqlCommand("Select drSettings From DDSecurityReports Where drUserID = @UserID And drDataset = @Database And drFolder = @Folder And drReport = ''"))
		{
			sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = currentUser.ID;
			sqlCommand.Parameters.Add(new SqlParameter("@Database", SqlDbType.NVarChar)).Value = ID;
			sqlCommand.Parameters.Add(new SqlParameter("@Folder", SqlDbType.NVarChar)).Value = folderName;
			text2 = currentDataDictionary.ExecuteScalar(sqlCommand) as string;
		}
		if (text2 != null && text2.Length != 0)
		{
			int num = text2.IndexOf("DefaultReport=", StringComparison.CurrentCultureIgnoreCase);
			if (num != -1)
			{
				text = text2.Substring(num + 14);
				if (text.Length != 0 && !File.Exists(currentContext.Reports.Location + folderName + "\\" + text + ".rpt"))
				{
					text = string.Empty;
				}
				return text;
			}
		}
		using (SqlCommand sqlCommand2 = currentDataDictionary.NewSqlCommand("Select drReport From DDSecurityReports Where drUserID = @UserID And drDataset = @Database And drFolder = @Folder"))
		{
			sqlCommand2.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = currentUser.ID;
			sqlCommand2.Parameters.Add(new SqlParameter("@Database", SqlDbType.NVarChar)).Value = ID;
			sqlCommand2.Parameters.Add(new SqlParameter("@Folder", SqlDbType.NVarChar)).Value = folderName;
			text = currentDataDictionary.ExecuteScalar(sqlCommand2) as string;
		}
		if (text == null)
		{
			return string.Empty;
		}
		return text;
	}

	public string GetDefaultPrinterForReport(string report)
	{
		string result = string.Empty;
		string text = GetDefaultReportForFolder(report);
		if (text.Length == 0)
		{
			text = report;
		}
		if (text.Length != 0)
		{
			string reportSettings = GetReportSettings(report, text);
			if (reportSettings != null && reportSettings.Length != 0)
			{
				string[] array = reportSettings.Replace("\n", string.Empty).Split('\r');
				foreach (string text2 in array)
				{
					if (text2.StartsWith("Printer", StringComparison.CurrentCultureIgnoreCase))
					{
						int num = text2.IndexOf('=');
						if (num != -1)
						{
							result = text2.Substring(num + 1).Trim().Replace("\"", string.Empty);
						}
						break;
					}
				}
			}
		}
		return result;
	}

	public string GetDefaultPrinterForReportAndFolder(string folderName, string report)
	{
		string result = string.Empty;
		if (report.Length != 0)
		{
			string reportSettings = GetReportSettings(folderName, report);
			if (reportSettings != null && reportSettings.Length != 0)
			{
				string[] array = reportSettings.Replace("\n", string.Empty).Split('\r');
				foreach (string text in array)
				{
					if (text.StartsWith("Printer", StringComparison.CurrentCultureIgnoreCase))
					{
						int num = text.IndexOf('=');
						if (num != -1)
						{
							result = text.Substring(num + 1).Trim().Replace("\"", string.Empty);
						}
						break;
					}
				}
			}
		}
		return result;
	}

	public string GetReportSettings(string folderName, string reportName)
	{
		SqlCommand sqlCommand = currentDataDictionary.NewSqlCommand("Select drSettings From DDSecurityReports Where drUserID = @UserID And drDataset = @Database And drFolder = @Folder And drReport = @Report");
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = currentUser.ID;
		sqlCommand.Parameters.Add(new SqlParameter("@Database", SqlDbType.NVarChar)).Value = ID;
		sqlCommand.Parameters.Add(new SqlParameter("@Folder", SqlDbType.NVarChar)).Value = folderName;
		sqlCommand.Parameters.Add(new SqlParameter("@Report", SqlDbType.NVarChar)).Value = reportName;
		return currentDataDictionary.ExecuteScalar(sqlCommand) as string;
	}

	public void OnTableChanged(TableChangedEventArgs e)
	{
		this.TableChanged?.Invoke(this, e);
	}

	public void OnTableChanged(string tableName)
	{
		OnTableChanged(new TableChangedEventArgs(tableName, null, null, null));
	}

	public void OnEmailMessageSent(EmailMessageSentEventArgs e)
	{
		this.EmailMessageSent?.Invoke(this, e);
	}

	public void OnConstructWhere(ConstructWhereEventArgs e)
	{
		this.ConstructWhere?.Invoke(this, e);
		e.AddToWhereClause(Security.GetRowFilter(e.TableName));
	}

	public object GetService(Type serviceType)
	{
		return serviceContainer.GetService(serviceType);
	}

	public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
	{
		serviceContainer.AddService(serviceType, callback, promote);
	}

	public void AddService(Type serviceType, ServiceCreatorCallback callback)
	{
		serviceContainer.AddService(serviceType, callback);
	}

	public void AddService(Type serviceType, object serviceInstance, bool promote)
	{
		serviceContainer.AddService(serviceType, serviceInstance, promote);
	}

	public void AddService(Type serviceType, object serviceInstance)
	{
		serviceContainer.AddService(serviceType, serviceInstance);
	}

	public void RemoveService(Type serviceType, bool promote)
	{
		serviceContainer.RemoveService(serviceType, promote);
	}

	public void RemoveService(Type serviceType)
	{
		serviceContainer.RemoveService(serviceType);
	}
}
