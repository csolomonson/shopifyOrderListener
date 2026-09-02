using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M1.Extensions;

namespace M1.Core;

[DebuggerDisplay("ID = {ID}, Admin = {Administrator}")]
public class M1User : IServiceProvider, IServiceContainer, IDisposable
{
	public class AutoLogoutEventArgs : EventArgs
	{
		public string Message = string.Empty;

		public int MinutesToAllowForSave;

		public bool ForceLogout = true;

		public bool AllowCancel = true;

		public AppContext Context;

		public M1DataDictionary DataDictionary;

		public M1User User;

		public int MessageType;

		public AutoLogoutEventArgs(string msg, AppContext context, M1DataDictionary dataDictionary, M1User user)
		{
			Message = msg;
			Context = context;
			DataDictionary = dataDictionary;
			User = user;
		}
	}

	public class MovedMyFolderEventArgs : EventArgs
	{
		public Guid? MovedFolderID;
	}

	public bool AutoShutdownMode;

	public bool IsLoggingOut;

	public bool IsLoggingIn;

	public DateTime LastActivityTime = DateTime.Now;

	public M1UserSettings Settings;

	public M1DatabaseCollection Databases;

	public M1DataDictionary DataDictionary;

	public AppContext Context;

	public M1Theme Theme;

	private DateTime? mustBeOutTime;

	private Timer logoutTimer = new Timer
	{
		Interval = 90000,
		Enabled = false
	};

	private string _ID = string.Empty;

	private List<AutoLogoutSettings> AutoLogoutList = new List<AutoLogoutSettings>();

	private ServiceContainer serviceContainer;

	private int _userCurrentInactivePartBinDisplayOption;

	private int _userCurrentInactiveWarehouseBinDisplayOption;

	public Guid ClientID { get; private set; }

	public bool Administrator { get; private set; }

	public bool DBAdministrator { get; private set; }

	public bool Developer { get; private set; }

	public string Name { get; private set; }

	public short BackupVerifyDays { get; private set; }

	public short InactiveCheckMinutes { get; private set; }

	public bool DDAlertUser { get; private set; }

	public bool GridDeveloper { get; private set; }

	public bool PasswordLocked { get; private set; }

	public int? DaysLeftBeforePasswordMustBeChanged { get; private set; }

	public string ID
	{
		get
		{
			return _ID;
		}
		private set
		{
			_ID = value;
		}
	}

	public int UserCurrentInactivePartBinDisplayOption
	{
		get
		{
			return _userCurrentInactivePartBinDisplayOption;
		}
		set
		{
			_userCurrentInactivePartBinDisplayOption = value;
		}
	}

	public int UserCurrentInactiveWarehouseBinDisplayOption
	{
		get
		{
			return _userCurrentInactiveWarehouseBinDisplayOption;
		}
		set
		{
			_userCurrentInactiveWarehouseBinDisplayOption = value;
		}
	}

	public event EventHandler SettingsReloaded;

	public event EventHandler<LoggingOutEventArgs> LoggingOut;

	public event EventHandler<AutoLogoutEventArgs> AutoLogout;

	public event EventHandler<MovedMyFolderEventArgs> MovedMyFolder;

	public M1User(IServiceProvider parentProvider)
	{
		serviceContainer = new ServiceContainer(parentProvider);
		DataDictionary = (M1DataDictionary)parentProvider.GetService(typeof(M1DataDictionary));
		Context = (AppContext)parentProvider.GetService(typeof(AppContext));
		serviceContainer.AddService(typeof(M1User), this);
		Databases = new M1DatabaseCollection(this);
		Settings = new M1UserSettings(this);
		Theme = new M1Theme();
	}

	public bool Login(string userID)
	{
		bool result = false;
		setPropertiesToDefault();
		DataRow userRecord = getUserRecord(userID);
		if (userRecord != null)
		{
			result = true;
			Administrator = Convert.ToBoolean(userRecord["duAdministrator"]);
			DBAdministrator = userRecord.Field<bool>("duDBAdministrator");
			Developer = userRecord.Field<bool>("duDeveloper");
			GridDeveloper = userRecord.Field<bool>("duGridDeveloper");
			PasswordLocked = userRecord.Field<bool>("duPasswordLocked");
			BackupVerifyDays = userRecord.Field<short>("duBackupVerifyDays");
			DDAlertUser = userRecord.Field<bool>("duDDAlertUser");
			Name = userRecord.Field<string>("duName");
			Settings.LoadSettings(userRecord.Field<string>("duProperties"));
			Theme.LoadTheme(Settings.Theme);
		}
		ClientID = Guid.NewGuid();
		ID = userID;
		return result;
	}

	public void CheckPassword(string password, string additionalHashString)
	{
		DataRow userRecord = getUserRecord(ID);
		if (userRecord == null || !isMatchingPassword(Context, userRecord.Field<string>("duPassword"), password, additionalHashString))
		{
			throw new M1LoginInvalidUserIDOrPasswordException();
		}
	}

	public static bool CheckPassword(M1DataDictionary dataDictionary, string userID, string password, string additionalHashString)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select duPassword From DDUsers Where duUserID = @User And duType = 0");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		string serverPwd = Convert.ToString(dataDictionary.ExecuteScalar(sqlCommand));
		return isMatchingPassword(dataDictionary.GetService(typeof(AppContext)) as AppContext, serverPwd, password, additionalHashString);
	}

	private static bool isMatchingPassword(AppContext context, string serverPwd, string password, string additionalHashString)
	{
		serverPwd = context.DDServerManager.Decrypt(serverPwd);
		serverPwd = M1Util.HashString(serverPwd);
		if (additionalHashString.Length != 0)
		{
			serverPwd = M1Util.HashString(additionalHashString + serverPwd);
		}
		return serverPwd.Equals(password);
	}

	public void Login(string userID, string password)
	{
		Login(userID, password, string.Empty);
	}

	public void Login(string userID, string password, string additionalHashString)
	{
		DataRow userRecord = getUserRecord(userID);
		if (userRecord != null && isMatchingPassword(Context, userRecord.Field<string>("duPassword"), password, additionalHashString))
		{
			bool flag = false;
			if (userRecord.Field<DateTime?>("duInactiveDate").HasValue && userRecord.Field<DateTime>("duInactiveDate").CompareTo(DateTime.Now) <= 0)
			{
				throw new M1LoginException("The system could not log you on because this account has been marked as inactive.");
			}
			flag = userRecord.Field<bool>("duMustChangePassword");
			if (!flag && userRecord.Field<short>("duPasswordExpirationDays") > 0 && userRecord.Field<DateTime?>("duPasswordSetDate").HasValue)
			{
				DaysLeftBeforePasswordMustBeChanged = userRecord.Field<short>("duPasswordExpirationDays") - (DateTime.Now - userRecord.Field<DateTime>("duPasswordSetDate")).Days;
				if (DaysLeftBeforePasswordMustBeChanged <= 0)
				{
					flag = true;
				}
			}
			if (flag)
			{
				throw new M1LoginPasswordExpiredException("Your password has expired and must be changed.");
			}
			loadAutoLogoutList(userID);
			if (GetCurrentUserCount(string.Empty, userID) >= DataDictionary.ProductCode.MaxUsers)
			{
				throw new M1LoginException("All of the user licenses for this installation of M1 are currently in use.");
			}
			if (!autoLogoutCheckAllForValidTime())
			{
				throw new M1LoginException("You are not allowed to login to M1 because there is a time restriction in place.");
			}
			Administrator = userRecord.Field<bool>("duAdministrator");
			DBAdministrator = userRecord.Field<bool>("duDBAdministrator");
			Developer = userRecord.Field<bool>("duDeveloper");
			GridDeveloper = userRecord.Field<bool>("duGridDeveloper");
			PasswordLocked = userRecord.Field<bool>("duPasswordLocked");
			BackupVerifyDays = userRecord.Field<short>("duBackupVerifyDays");
			DDAlertUser = userRecord.Field<bool>("duDDAlertUser");
			Name = userRecord.Field<string>("duName");
			Settings.LoadSettings(userRecord.Field<string>("duProperties"));
			Theme.LoadTheme(Settings.Theme);
			ClientID = Guid.NewGuid();
			ID = userID;
			AddUserToLog(null);
			SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Update DDUsers Set duLastLoginTime = GetDate(), duLastLoginMachine = @Machine Where duUserID = @User");
			sqlCommand.Parameters.Add(new SqlParameter("@Machine", SqlDbType.NVarChar)).Value = Environment.MachineName;
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = ID;
			DataDictionary.ExecuteCommand(sqlCommand);
			logoutTimer.Tick += logoutTimer_Tick;
			logoutTimer.Enabled = true;
			return;
		}
		throw new M1LoginInvalidUserIDOrPasswordException();
	}

	public void Login(string userID, string password, string additionalHashString, string databaseName)
	{
		DataRow userRecord = getUserRecord(userID);
		if (userRecord != null && isMatchingPassword(Context, userRecord.Field<string>("duPassword"), password, additionalHashString))
		{
			bool flag = false;
			if (userRecord.Field<DateTime?>("duInactiveDate").HasValue && userRecord.Field<DateTime>("duInactiveDate").CompareTo(DateTime.Now) <= 0)
			{
				throw new M1LoginException("The system could not log you on because this account has been marked as inactive.");
			}
			flag = userRecord.Field<bool>("duMustChangePassword");
			if (!flag && userRecord.Field<short>("duPasswordExpirationDays") > 0 && userRecord.Field<DateTime?>("duPasswordSetDate").HasValue)
			{
				DaysLeftBeforePasswordMustBeChanged = userRecord.Field<short>("duPasswordExpirationDays") - (DateTime.Now - userRecord.Field<DateTime>("duPasswordSetDate")).Days;
				if (DaysLeftBeforePasswordMustBeChanged <= 0)
				{
					flag = true;
				}
			}
			if (flag)
			{
				throw new M1LoginPasswordExpiredException("Your password has expired and must be changed.");
			}
			loadAutoLogoutList(userID);
			if (!IsUserLicenseViewOnly(userID, databaseName) && GetCurrentUserCount(string.Empty, userID) >= DataDictionary.ProductCode.MaxUsers)
			{
				throw new M1LoginException("All of the user licenses for this installation of M1 are currently in use.");
			}
			if (!autoLogoutCheckAllForValidTime())
			{
				throw new M1LoginException("You are not allowed to login to M1 because there is a time restriction in place.");
			}
			Administrator = userRecord.Field<bool>("duAdministrator");
			DBAdministrator = userRecord.Field<bool>("duDBAdministrator");
			Developer = userRecord.Field<bool>("duDeveloper");
			GridDeveloper = userRecord.Field<bool>("duGridDeveloper");
			PasswordLocked = userRecord.Field<bool>("duPasswordLocked");
			BackupVerifyDays = userRecord.Field<short>("duBackupVerifyDays");
			DDAlertUser = userRecord.Field<bool>("duDDAlertUser");
			Name = userRecord.Field<string>("duName");
			Settings.LoadSettings(userRecord.Field<string>("duProperties"));
			Theme.LoadTheme(Settings.Theme);
			ClientID = Guid.NewGuid();
			ID = userID;
			AddUserToLog(null);
			SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Update DDUsers Set duLastLoginTime = GetDate(), duLastLoginMachine = @Machine Where duUserID = @User");
			sqlCommand.Parameters.Add(new SqlParameter("@Machine", SqlDbType.NVarChar)).Value = Environment.MachineName;
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = ID;
			DataDictionary.ExecuteCommand(sqlCommand);
			logoutTimer.Tick += logoutTimer_Tick;
			logoutTimer.Enabled = true;
			return;
		}
		throw new M1LoginInvalidUserIDOrPasswordException();
	}

	public void ReloadUser()
	{
		DataRow userRecord = getUserRecord(ID);
		if (userRecord == null)
		{
			return;
		}
		loadAutoLogoutList(ID);
		Administrator = userRecord.Field<bool>("duAdministrator");
		DBAdministrator = userRecord.Field<bool>("duDBAdministrator");
		Developer = userRecord.Field<bool>("duDeveloper");
		GridDeveloper = userRecord.Field<bool>("duGridDeveloper");
		PasswordLocked = userRecord.Field<bool>("duPasswordLocked");
		BackupVerifyDays = userRecord.Field<short>("duBackupVerifyDays");
		DDAlertUser = userRecord.Field<bool>("duDDAlertUser");
		Name = userRecord.Field<string>("duName");
		ReloadSettings(userRecord.Field<string>("duProperties"));
		Theme.LoadTheme(Settings.Theme);
		foreach (M1Database database in Databases)
		{
			database.PropsRefresh();
		}
	}

	private void OnSettingsReloaded()
	{
		this.SettingsReloaded?.Invoke(this, EventArgs.Empty);
	}

	public void ReloadSettings()
	{
		ReloadSettings(Settings.GetUserProperties(DataDictionary, ID));
	}

	private void ReloadSettings(string properties)
	{
		Settings.LoadSettings(properties);
		Theme.LoadTheme(Settings.Theme);
		OnSettingsReloaded();
	}

	private void loadAutoLogoutList(string userID)
	{
		AutoLogoutList.Clear();
		short num = 0;
		short num2 = 0;
		SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Select duAutoLogout,duInactiveCheckMinutes From DDUsers Where duUserID = @User Or duUserID In (Select dzGroupID From DDSecurityGroups Where dzUserID = @User)");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		foreach (DataRow row in DataDictionary.GetDataTable(sqlCommand).Rows)
		{
			if (row["duAutoLogout"] != DBNull.Value)
			{
				AutoLogoutList.Add(new AutoLogoutSettings(row.Field<string>("duAutoLogout")));
			}
			num2 = row.Field<short>("duInactiveCheckMinutes");
			if (num2 > 0 && (num == 0 || num2 < num))
			{
				num = num2;
			}
		}
		InactiveCheckMinutes = num;
	}

	public string CheckForFailedBackups()
	{
		if (BackupVerifyDays > 0)
		{
			DateTime value = DateTime.Now.Date.AddDays(-BackupVerifyDays);
			DateTime? dateTime = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (Context.InstalledDatabases.Count != 0)
			{
				using SqlConnection sqlConnection = Context.DBServerManager.GetConnection(this, "master", openImmediately: true);
				Backup backup = new Backup(Context, Context.DBServerManager);
				foreach (DatabaseInfo installedDatabase in Context.InstalledDatabases)
				{
					if (installedDatabase.Version.CompareTo("5.00.080") >= 0 && Convert.ToByte(Context.DBServerManager.ExecuteScalar(sqlConnection, this, "master", "Select Cast(xadBackupCheck As Bit) As xadBackupCheck From " + installedDatabase.Name + ".dbo.DatasetProperties")) != 0)
					{
						dateTime = backup.GetLastBackupTimeForDatabase(installedDatabase.Name);
						if (!dateTime.HasValue || dateTime.Value.CompareTo(value) < 0)
						{
							stringBuilder.Append(installedDatabase.ToString(includeVersion: false) + "<br/>");
						}
					}
				}
			}
			if (DDAlertUser)
			{
				dateTime = new Backup(Context, Context.DDServerManager).GetLastBackupTimeForDatabase(DataDictionary.ID);
				if (!dateTime.HasValue || dateTime.Value.CompareTo(value) < 0)
				{
					stringBuilder.Append(DataDictionary.ID + " - Data Dictionary<br/>");
				}
			}
			if (stringBuilder.Length != 0)
			{
				return "The following databases have not had a successful backup in the past " + BackupVerifyDays + " day(s):<br/><br/>" + stringBuilder.ToString();
			}
		}
		return string.Empty;
	}

	private void logoutTimer_Tick(object sender, EventArgs e)
	{
		if (!Context.SuspendTimerEvents)
		{
			autoLogoutCheck();
		}
	}

	public void OnLoggingOut(LoggingOutEventArgs e)
	{
		this.LoggingOut?.Invoke(this, e);
	}

	public bool Logout()
	{
		if (ClientID != Guid.Empty)
		{
			IsLoggingOut = true;
			try
			{
				if (!Databases.LogoutAndRemove(null))
				{
					return false;
				}
				LoggingOutEventArgs e = new LoggingOutEventArgs();
				OnLoggingOut(e);
				if (e.Cancel)
				{
					return false;
				}
				DeleteUserFromLog(null);
				SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Update DDUsers Set duLastLogoutTime = GetDate() Where duUserID = @User");
				sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = ID;
				DataDictionary.ExecuteCommand(sqlCommand);
			}
			finally
			{
				IsLoggingOut = false;
			}
		}
		ID = string.Empty;
		if (Settings != null)
		{
			Settings.LoadDefaults();
		}
		ClientID = Guid.Empty;
		if (logoutTimer != null)
		{
			logoutTimer.Tick -= logoutTimer_Tick;
			logoutTimer.Enabled = false;
		}
		return true;
	}

	private bool doActivityCheck()
	{
		bool flag = true;
		bool flag2 = true;
		AutoLogoutEventArgs e = new AutoLogoutEventArgs(string.Empty, Context, DataDictionary, this);
		if (ClientID != Guid.Empty && DataDictionary.Version.CompareTo("7.50.006") >= 0)
		{
			SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Update DDUserLog Set ulLastActivityTime = GetDate(),ulLastActionTime = @LastActionTime Where ulUserClientID = @UserClientID And ulDatabaseClientID Is Null");
			sqlCommand.Parameters.Add(new SqlParameter("@LastActionTime", SqlDbType.DateTime)).Value = LastActivityTime;
			sqlCommand.Parameters.Add(new SqlParameter("@UserClientID", SqlDbType.UniqueIdentifier)).Value = ClientID;
			if (DataDictionary.ExecuteCommand(sqlCommand) == 0)
			{
				AddUserToLog(null);
			}
			sqlCommand = DataDictionary.NewSqlCommand("Update DDUserLog Set ulLastActivityTime = GetDate(),ulLastActionTime = @LastActionTime Where ulUserClientID = @UserClientID And ulDatabaseClientID = @DatabaseClientID");
			sqlCommand.Parameters.Add(new SqlParameter("@LastActionTime", SqlDbType.DateTime));
			sqlCommand.Parameters.Add(new SqlParameter("@UserClientID", SqlDbType.UniqueIdentifier)).Value = ClientID;
			sqlCommand.Parameters.Add(new SqlParameter("@DatabaseClientID", SqlDbType.UniqueIdentifier));
			foreach (M1Database database in Databases)
			{
				sqlCommand.Parameters["@LastActionTime"].Value = database.LastActivityTime;
				sqlCommand.Parameters["@DatabaseClientID"].Value = database.ClientID;
				if (DataDictionary.ExecuteCommand(sqlCommand) == 0)
				{
					AddUserToLog(database);
				}
				if (database.Security.GetDatabaseAccessLevel(SecurityAccessLevel.Default) != SecurityAccessLevel.View)
				{
					flag2 = false;
				}
			}
			sqlCommand = DataDictionary.NewSqlCommand("Select ulMessageType,ulMessageText,ulDatabaseClientID From DDUserLog Where ulUserClientID = @UserClientID And ulMessageType <> 0");
			sqlCommand.Parameters.Add(new SqlParameter("@UserClientID", SqlDbType.UniqueIdentifier)).Value = ClientID;
			using (DataTable dataTable = DataDictionary.GetDataTable(sqlCommand))
			{
				foreach (DataRow row in dataTable.Rows)
				{
					bool flag3 = true;
					switch (row.Field<byte>("ulMessageType"))
					{
					case 2:
					{
						DataRow userRecord = getUserRecord(ID);
						if (userRecord != null)
						{
							Settings.LoadSettings(userRecord.Field<string>("duProperties"));
							Theme.LoadTheme(Settings.Theme);
						}
						break;
					}
					case 4:
						e.Message = row.Field<string>("ulMessageText").Trim();
						e.MessageType = 4;
						e.AllowCancel = true;
						e.ForceLogout = false;
						break;
					case 8:
						e.Message = row.Field<string>("ulMessageText").Trim();
						e.MessageType = 8;
						e.AllowCancel = true;
						e.ForceLogout = false;
						break;
					case 16:
						e.Message = row.Field<string>("ulMessageText").Trim();
						if (e.Message.Length == 0)
						{
							e.Message = "The M1 Administrator has requested that you log off and close this session of M1.";
						}
						flag3 = false;
						e.MessageType = 0;
						e.AllowCancel = true;
						e.ForceLogout = true;
						break;
					case 32:
						e.Message = row.Field<string>("ulMessageText").Trim();
						if (e.Message.Length == 0)
						{
							e.Message = "The M1 Administrator has requested that you log off and close this session of M1.";
						}
						e.MessageType = 0;
						e.AllowCancel = false;
						e.ForceLogout = true;
						break;
					}
					if (row.Field<byte>("ulMessageType") != 0 && flag3)
					{
						Guid? guid = row.Field<Guid?>("ulDatabaseClientID");
						if (!guid.HasValue || guid == Guid.Empty)
						{
							sqlCommand = DataDictionary.NewSqlCommand("Update DDUserLog Set ulMessageType = 0, ulMessageText = '' Where ulUserClientID = @UserClientID And ulDatabaseClientID Is Null");
							sqlCommand.Parameters.Add(new SqlParameter("@UserClientID", SqlDbType.UniqueIdentifier)).Value = ClientID;
							DataDictionary.ExecuteCommand(sqlCommand);
						}
						else
						{
							sqlCommand = DataDictionary.NewSqlCommand("Update DDUserLog Set ulMessageType = 0, ulMessageText = '' Where ulUserClientID = @UserClientID And ulDatabaseClientID = @DatabaseClientID");
							sqlCommand.Parameters.Add(new SqlParameter("@UserClientID", SqlDbType.UniqueIdentifier)).Value = ClientID;
							sqlCommand.Parameters.Add(new SqlParameter("@DatabaseClientID", SqlDbType.UniqueIdentifier)).Value = guid;
							DataDictionary.ExecuteCommand(sqlCommand);
						}
					}
				}
			}
			if (!flag2 && flag && (int)DataDictionary.ExecuteScalar("Select IsNull(Count(*),0) As rec_count From DDUserLog Where ulUserType = 'M1' And Not ulDatabaseClientID Is Null") > DataDictionary.ProductCode.MaxUsers && GetCurrentUserCount(string.Empty, string.Empty) > DataDictionary.ProductCode.MaxUsers)
			{
				e.Message = "This session will be closed because the maximum number of users has been reached. If you press cancel you will keep being prompted to exit M1 until the active user count is lower than the maximum user count.";
				logoutTimer.Interval = 60000;
			}
			if (e.Message.Length != 0)
			{
				OnAutoLogout(e);
				flag = !e.ForceLogout;
			}
		}
		return flag;
	}

	private bool autoLogoutCheckAllForValidTime()
	{
		foreach (AutoLogoutSettings autoLogout in AutoLogoutList)
		{
			if (!autoLogout.IsNowAValidTime())
			{
				return false;
			}
		}
		return true;
	}

	public void OnAutoLogout(AutoLogoutEventArgs e)
	{
		this.AutoLogout?.Invoke(this, e);
	}

	private void autoLogoutCheck()
	{
		bool flag = false;
		logoutTimer.Tick -= logoutTimer_Tick;
		logoutTimer.Enabled = false;
		if (!doActivityCheck())
		{
			flag = true;
		}
		if (!flag && !autoLogoutCheckAllForValidTime())
		{
			if (mustBeOutTime.HasValue)
			{
				if (DateTime.Now.CompareTo(mustBeOutTime) >= 0)
				{
					flag = true;
				}
			}
			else
			{
				AutoLogoutEventArgs e = new AutoLogoutEventArgs("This session will be closed because a time constraint has been defined for now. If you press cancel you will have 5 minutes to save your work and exit M1 before you will be automatically logged out.", Context, DataDictionary, this);
				e.MinutesToAllowForSave = 5;
				OnAutoLogout(e);
				flag = e.ForceLogout;
				if (!e.ForceLogout && e.MinutesToAllowForSave > 0)
				{
					mustBeOutTime = DateTime.Now.AddMinutes(e.MinutesToAllowForSave);
					logoutTimer.Interval = 60000;
				}
			}
		}
		if (!flag && InactiveCheckMinutes > 0)
		{
			double totalMinutes = DateTime.Now.Subtract(LastActivityTime).TotalMinutes;
			if ((double)InactiveCheckMinutes <= totalMinutes)
			{
				AutoLogoutEventArgs e2 = new AutoLogoutEventArgs("This session has been inactive for " + totalMinutes.ToString("0") + " minutes and will be closed.", Context, DataDictionary, this);
				OnAutoLogout(e2);
				flag = e2.ForceLogout;
			}
		}
		if (flag)
		{
			AutoShutdownMode = true;
			if (Logout())
			{
				if (DataDictionary.Users.Contains(this))
				{
					DataDictionary.Users.Remove(this);
				}
				if (DataDictionary.Users.Count == 0)
				{
					Application.Exit();
				}
			}
			AutoShutdownMode = false;
		}
		logoutTimer.Tick += logoutTimer_Tick;
		logoutTimer.Enabled = true;
	}

	public void DeleteUserFromLog(M1Database database)
	{
		if (DataDictionary != null && DataDictionary.Version.CompareTo("7.50.006") >= 0)
		{
			if (database == null)
			{
				SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Delete From DDUserLog Where ulUserClientID = @UserClientID And ulUserID = @User And ulDatabaseClientID Is Null");
				sqlCommand.Parameters.Add(new SqlParameter("@UserClientID", SqlDbType.UniqueIdentifier)).Value = ClientID;
				sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = ID;
				DataDictionary.ExecuteCommand(sqlCommand);
			}
			else
			{
				SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Delete From DDUserLog Where ulUserClientID = @UserClientID And ulUserID = @User And ulDatabaseClientID = @DatabaseClientID");
				sqlCommand.Parameters.Add(new SqlParameter("@UserClientID", SqlDbType.UniqueIdentifier)).Value = ClientID;
				sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = ID;
				sqlCommand.Parameters.Add(new SqlParameter("@DatabaseClientID", SqlDbType.UniqueIdentifier)).Value = database.ClientID;
				DataDictionary.ExecuteCommand(sqlCommand);
			}
		}
	}

	public void AddUserToLog(M1Database database)
	{
		string empty = string.Empty;
		empty = ((database != null && database.ID.Length != 0) ? ((!database.ReadOnlyLogin && !IsUserLicenseViewOnly(ID, database.ID)) ? "M1" : "VO") : "VO");
		DeleteUserFromLog(database);
		SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Insert Into DDUserLog (ulUserClientID,ulUserID,ulDatabaseClientID,ulDatabase,ulEmailAddress,ulMachine,ulUserName,ulUserType,ulLoginTime,ulLastActivityTime,ulLastActionTime) Values (@UserClientID, @User, @DatabaseClientID, @DatabaseID, @EmailAddress, @Machine, @UserName, @UserType, GetDate(), GetDate(), GetDate())");
		sqlCommand.Parameters.Add(new SqlParameter("@UserClientID", SqlDbType.UniqueIdentifier)).Value = ClientID;
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = ID;
		sqlCommand.Parameters.Add(new SqlParameter("@DatabaseClientID", SqlDbType.UniqueIdentifier)).IsNullable = true;
		sqlCommand.Parameters.Add(new SqlParameter("@DatabaseID", SqlDbType.NVarChar));
		sqlCommand.Parameters.Add(new SqlParameter("@EmailAddress", SqlDbType.NVarChar));
		sqlCommand.Parameters.Add(new SqlParameter("@Machine", SqlDbType.NVarChar)).Value = Environment.MachineName;
		sqlCommand.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar)).Value = Environment.UserName;
		sqlCommand.Parameters.Add(new SqlParameter("@UserType", SqlDbType.NVarChar)).Value = empty;
		if (database == null)
		{
			sqlCommand.Parameters["@DatabaseClientID"].SqlValue = DBNull.Value;
			sqlCommand.Parameters["@DatabaseID"].Value = string.Empty;
			sqlCommand.Parameters["@EmailAddress"].Value = string.Empty;
		}
		else
		{
			sqlCommand.Parameters["@DatabaseClientID"].Value = database.ClientID;
			sqlCommand.Parameters["@DatabaseID"].Value = database.ID;
			sqlCommand.Parameters["@EmailAddress"].Value = database.LoginCredentials.EmailAddress;
		}
		DataDictionary.ExecuteCommand(sqlCommand);
	}

	private DataRow getUserRecord(string userID)
	{
		DataTable dataTable = null;
		userID = userID.ToUpper();
		SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Select * From DDUsers Where duUserID = @User And duType = 0");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		dataTable = DataDictionary.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0 && userID.Equals("ADMIN", StringComparison.CurrentCultureIgnoreCase) && DataDictionary.Users.CreateUser("ADMIN", "Administrator", developer: true, administrator: true, dbadministrator: true, gridDeveloper: true, group: false))
		{
			DataDictionary.ExecuteCommand("Insert Into DDSecurityGroups (dzGroupID, dzUserID, dzDataset) SELECT duUserID,'ADMIN','M1_M1' FROM DDusers WHERE duType = 2");
			if (DataDictionary.ExecuteScalar("Select dtLevel From DDSecurityTables Where dtUserID = 'ADMIN' And dtDataset = 'M1_M1' And dtTable = '' And dtField = ''") == null)
			{
				DataDictionary.ExecuteCommand("Insert Into DDSecurityTables (dtUserID, dtDataset, dtTable, dtField, dtLevel) Values ('ADMIN', 'M1_M1', '', '', 60)");
			}
			dataTable = DataDictionary.GetDataTable(sqlCommand);
		}
		if (dataTable.Rows.Count == 0)
		{
			return null;
		}
		return dataTable.Rows[0];
	}

	private bool IsUserLicenseViewOnly(string userID, string databaseID)
	{
		if (string.IsNullOrWhiteSpace(databaseID))
		{
			return true;
		}
		bool result = false;
		int databaseAccessLevel = GetDatabaseAccessLevel(userID, databaseID);
		int tableAccessLevel = GetTableAccessLevel(userID, databaseID);
		int fieldAccessLevel = GetFieldAccessLevel(userID, databaseID);
		if (LicenseIsViewOrNone(databaseAccessLevel, tableAccessLevel, fieldAccessLevel))
		{
			result = true;
		}
		else if (databaseAccessLevel == 0 && tableAccessLevel == 0 && fieldAccessLevel == 0)
		{
			DataTable userGroups = GetUserGroups(userID, databaseID);
			if (userGroups.Rows.Count > 0)
			{
				result = true;
				foreach (DataRow row in userGroups.Rows)
				{
					int databaseAccessLevel2 = GetDatabaseAccessLevel(row.Field<string>("dzGroupID"), databaseID);
					int tableAccessLevel2 = GetTableAccessLevel(row.Field<string>("dzGroupID"), databaseID);
					int fieldAccessLevel2 = GetFieldAccessLevel(row.Field<string>("dzGroupID"), databaseID);
					if (!LicenseIsViewOrNone(databaseAccessLevel2, tableAccessLevel2, fieldAccessLevel2))
					{
						result = false;
						break;
					}
				}
			}
		}
		return result;
	}

	private bool LicenseIsViewOrNone(int databaseAccessLevel, int tableAccessLevel, int fieldAccessLevel)
	{
		bool result = false;
		if (databaseAccessLevel == 0 && tableAccessLevel == 2 && (fieldAccessLevel == 0 || fieldAccessLevel == 1))
		{
			result = true;
		}
		if (databaseAccessLevel == 0 && (tableAccessLevel == 0 || tableAccessLevel == 1) && fieldAccessLevel == 2)
		{
			result = true;
		}
		if (databaseAccessLevel == 0 && tableAccessLevel == 2 && fieldAccessLevel == 2)
		{
			result = true;
		}
		if (databaseAccessLevel == 2 && (tableAccessLevel == 0 || tableAccessLevel == 1 || tableAccessLevel == 2) && (fieldAccessLevel == 0 || fieldAccessLevel == 1 || fieldAccessLevel == 2))
		{
			result = true;
		}
		return result;
	}

	private int GetDatabaseAccessLevel(string userID, string databaseID)
	{
		SqlCommand sqlCommand = DataDictionary.NewSqlCommand("SELECT dtLevel FROM DDSecurityTables WHERE dtUserID = @User AND dtDataset = @Database AND dtTable = '' AND dtField = '' ");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		sqlCommand.Parameters.Add(new SqlParameter("@Database", SqlDbType.NVarChar)).Value = databaseID;
		return Convert.ToInt32(DataDictionary.ExecuteScalar(sqlCommand));
	}

	private int GetTableAccessLevel(string userID, string databaseID)
	{
		SqlCommand sqlCommand = DataDictionary.NewSqlCommand("SELECT TOP 1 dtLevel FROM DDSecurityTables WHERE dtuserid = @User AND dtDataset = @Database AND dtTable <> '' AND dtField = '' ORDER BY dtlevel DESC");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		sqlCommand.Parameters.Add(new SqlParameter("@Database", SqlDbType.NVarChar)).Value = databaseID;
		return Convert.ToInt32(DataDictionary.ExecuteScalar(sqlCommand));
	}

	private int GetFieldAccessLevel(string userID, string databaseID)
	{
		SqlCommand sqlCommand = DataDictionary.NewSqlCommand("SELECT TOP 1 dtLevel FROM DDSecurityTables WHERE dtuserid = @User AND dtDataset = @Database AND dtTable <> '' AND dtField <> '' ORDER BY dtlevel DESC");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		sqlCommand.Parameters.Add(new SqlParameter("@Database", SqlDbType.NVarChar)).Value = databaseID;
		return Convert.ToInt32(DataDictionary.ExecuteScalar(sqlCommand));
	}

	private DataTable GetUserGroups(string userID, string databaseID)
	{
		SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Select dzGroupID From DDSecurityGroups Inner Join DDUsers On dzGroupID = duUserID Where dzDataset = @Database And dzUserID = @User And duType = 1");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		sqlCommand.Parameters.Add(new SqlParameter("@Database", SqlDbType.NVarChar)).Value = databaseID;
		return DataDictionary.GetDataTable(sqlCommand);
	}

	private void setPropertiesToDefault()
	{
		Administrator = false;
		DBAdministrator = false;
		Developer = false;
		GridDeveloper = false;
		PasswordLocked = true;
	}

	public int GetCurrentUserCount(string userType, string userID)
	{
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = -1;
		bool flag = true;
		string empty = string.Empty;
		string value = Environment.MachineName.Trim().ToUpper();
		string value2 = Environment.UserName.Trim().ToUpper();
		userID = userID.Trim().ToUpper();
		StringBuilder stringBuilder = new StringBuilder();
		string text = string.Empty;
		foreach (DatabaseInfo installedDataDictionary in Context.InstalledDataDictionaries)
		{
			if (!installedDataDictionary.Version.Equals(DataDictionary.Version, StringComparison.CurrentCultureIgnoreCase))
			{
				continue;
			}
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(" Union All ");
				if (text.Length == 0)
				{
					SqlCommand sqlCommand = DataDictionary.NewSqlCommand("select DATABASEPROPERTYEX(@Database,'collation')");
					sqlCommand.Parameters.Add(new SqlParameter("@Database", SqlDbType.NVarChar)).Value = installedDataDictionary.Name;
					if (!string.IsNullOrWhiteSpace(Convert.ToString(DataDictionary.ExecuteScalar(sqlCommand))))
					{
						text = " COLLATE " + (string)DataDictionary.ExecuteScalar(sqlCommand);
					}
				}
			}
			stringBuilder.Append("select " + installedDataDictionary.Name.ToSql() + text + " As ddName,ulUserClientID,ulUserType" + text + ",ulUserID" + text + ",ulDatabaseClientID,ulDatabase" + text + ",ulMachine" + text + ",ulUserName" + text + ",IsNull(ulLastActivityTime,'19000101') As ulLastActivityTime,DateAdd(n,-5,GetDate()) As CheckTime from " + installedDataDictionary.Name + ".dbo.DDUserLog");
		}
		if (stringBuilder.Length != 0)
		{
			stringBuilder.Append(" Order By ulDatabase Desc");
		}
		if (stringBuilder.Length != 0)
		{
			using DataTable dataTable = DataDictionary.GetDataTable(stringBuilder.ToString());
			foreach (DataRow row in dataTable.Rows)
			{
				flag = true;
				if (row.Field<DateTime>("ulLastActivityTime").CompareTo(row.Field<DateTime>("CheckTime")) >= 0)
				{
					if (row.Field<string>("ulMachine").Equals(value, StringComparison.CurrentCultureIgnoreCase) && row.Field<string>("ulUserName").Equals(value2, StringComparison.CurrentCultureIgnoreCase) && row.Field<string>("ulUserID").Equals(userID, StringComparison.CurrentCultureIgnoreCase) && row.Field<Guid>("ulUserClientID") != ClientID)
					{
						string text2 = row.Field<string>("ulUserType");
						if (text2 == "VO" || text2 == "M1")
						{
							if (num5 == -1)
							{
								num5 = Process.GetProcessesByName("M1").Length;
							}
							if (num5 <= 1)
							{
								flag = false;
							}
						}
					}
				}
				else
				{
					flag = false;
				}
				if (flag)
				{
					empty = row.Field<Guid>("ulUserClientID").ToString();
					string text2 = row.Field<string>("ulUserType");
					if (!(text2 == "DC"))
					{
						if (text2 == "VO")
						{
							if (row.Field<string>("ulDatabase").Length != 0 || !list.Contains(empty, StringComparer.CurrentCultureIgnoreCase))
							{
								if (!list.Contains(empty, StringComparer.CurrentCultureIgnoreCase))
								{
									list.Add(empty);
								}
								num3++;
							}
						}
						else if (row.Field<string>("ulDatabase").Length == 0)
						{
							if (!list.Contains(empty, StringComparer.CurrentCultureIgnoreCase))
							{
								num3++;
							}
						}
						else
						{
							if (!list.Contains(empty, StringComparer.CurrentCultureIgnoreCase))
							{
								list.Add(empty);
							}
							num++;
						}
					}
					else
					{
						num2++;
					}
				}
				else
				{
					Guid? guid = row.Field<Guid?>("ulDatabaseClientID");
					if (!guid.HasValue || guid == Guid.Empty)
					{
						SqlCommand sqlCommand2 = DataDictionary.NewSqlCommand("DELETE FROM " + row.Field<string>("ddName") + ".dbo.DDUserLog WHERE ulUserClientID = @UserClientID And ulDatabaseClientID Is Null");
						sqlCommand2.Parameters.Add(new SqlParameter("@UserClientID", SqlDbType.UniqueIdentifier)).Value = row.Field<Guid>("ulUserClientID");
						DataDictionary.ExecuteCommand(sqlCommand2);
					}
					else
					{
						SqlCommand sqlCommand2 = DataDictionary.NewSqlCommand("DELETE FROM " + row.Field<string>("ddName") + ".dbo.DDUserLog WHERE ulUserClientID = @UserClientID And ulDatabaseClientID = @DatabaseClientID");
						sqlCommand2.Parameters.Add(new SqlParameter("@UserClientID", SqlDbType.UniqueIdentifier)).Value = row.Field<Guid>("ulUserClientID");
						sqlCommand2.Parameters.Add(new SqlParameter("@DatabaseClientID", SqlDbType.UniqueIdentifier)).Value = guid;
						DataDictionary.ExecuteCommand(sqlCommand2);
					}
				}
			}
		}
		if (!(userType == "DC"))
		{
			if (userType == "VO")
			{
				return num3;
			}
			return num;
		}
		return num2;
	}

	protected void OnMovedMyFolder(MovedMyFolderEventArgs e)
	{
		this.MovedMyFolder?.Invoke(this, e);
	}

	public void SelectMovedMyFolder(Guid folderID)
	{
		MovedMyFolderEventArgs e = new MovedMyFolderEventArgs();
		e.MovedFolderID = folderID;
		OnMovedMyFolder(e);
	}

	public object GetService(Type serviceType)
	{
		if (serviceContainer == null)
		{
			return null;
		}
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

	public DataTable ResolvedUsersSecurity(string userID)
	{
		SqlCommand sqlCommand = DataDictionary.NewSqlCommand("SELECT duUserID,duName FROM DDUsers WHERE duUserID = @UserID");
		sqlCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.NVarChar)).Value = userID;
		return DataDictionary.GetDataTable(sqlCommand);
	}

	public DataTable ResolvedDatabaseSecurity(string userID)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.AddRange(new DataColumn[3]
		{
			new DataColumn("dtDataset", typeof(string)),
			new DataColumn("Description", typeof(string)),
			new DataColumn("dtLevel", typeof(byte))
		});
		foreach (DatabaseInfo installedDatabase in Context.InstalledDatabases)
		{
			DataRow dataRow = dataTable.NewRow();
			dataRow.BeginEdit();
			string text = installedDatabase.Name.ToUpper();
			dataRow.SetField("dtDataset", text);
			dataRow.SetField("Description", installedDatabase.Description);
			dataRow.SetField("dtLevel", Databases[0].Security.GetDatabaseAccessLevel(SecurityAccessLevel.Default, text, userID));
			dataRow.EndEdit();
			dataTable.Rows.Add(dataRow);
		}
		return dataTable;
	}

	public DataTable ResolvedTableSecurity(string userID, bool showInReport)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.AddRange(new DataColumn[5]
		{
			new DataColumn("dtDataset", typeof(string)),
			new DataColumn("dtTable", typeof(string)),
			new DataColumn("dtCaption", typeof(string)),
			new DataColumn("dtModule", typeof(string)),
			new DataColumn("dtLevel", typeof(byte))
		});
		if (showInReport)
		{
			addTableRows(dataTable, string.Empty, userID);
			foreach (DatabaseInfo installedDatabase in Context.InstalledDatabases)
			{
				addTableRows(dataTable, installedDatabase.Name.ToUpper(), userID);
			}
		}
		return dataTable;
	}

	private void addTableRows(DataTable securityTable, string datasetID, string userID)
	{
		SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Select DDTables.dtTable,dtCaption,dtLevel,dtModule from DDTables left outer join DDSecurityTables on DDTables.dtTable = DDSecurityTables.dtTable AND DDSecurityTables.dtField = '' AND DDSecurityTables.dtDataset = @Database AND DDSecurityTables.dtUserID = @User ORDER BY DDTables.dtTable");
		sqlCommand.Parameters.Add(new SqlParameter("@Database", SqlDbType.NVarChar)).Value = datasetID;
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		foreach (DataRow row in DataDictionary.GetDataTable(sqlCommand).Rows)
		{
			DataRow dataRow2 = securityTable.NewRow();
			dataRow2["dtDataset"] = datasetID;
			dataRow2["dtTable"] = row["dtTable"];
			dataRow2["dtCaption"] = row["dtCaption"];
			dataRow2["dtModule"] = DataDictionary.Modules.GetModuleText(row.Field<string>("dtModule"));
			dataRow2["dtLevel"] = Databases[0].Security.GetTableAccessLevel(row.Field<string>("dtTable"), SecurityAccessLevel.None, datasetID, userID);
			securityTable.Rows.Add(dataRow2);
		}
	}

	public DataTable ResolvedFieldSecurity(string userID, bool showInReport)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.AddRange(new DataColumn[6]
		{
			new DataColumn("dtDataset", typeof(string)),
			new DataColumn("dfTable", typeof(string)),
			new DataColumn("dtCaption", typeof(string)),
			new DataColumn("dfField", typeof(string)),
			new DataColumn("dfCaption", typeof(string)),
			new DataColumn("dtLevel", typeof(byte))
		});
		if (showInReport)
		{
			addFieldRows(dataTable, string.Empty, userID);
			foreach (DatabaseInfo installedDatabase in Context.InstalledDatabases)
			{
				addFieldRows(dataTable, installedDatabase.Name.ToUpper(), userID);
			}
		}
		return dataTable;
	}

	private void addFieldRows(DataTable securityTable, string datasetID, string userID)
	{
		SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Select DDTables.dtTable As dfTable,DDTables.dtCaption,'' As dfField,'' As dfCaption,Convert(tinyint,null) As dtLevel From DDTables Union All Select dfTable,DDTables.dtCaption,dfField,dfCaption,dtLevel From DDFields Inner Join DDTables On dfTable = DDTables.dtTable Left Outer Join DDSecurityTables On DDSecurityTables.dtDataset=@Database And DDSecurityTables.dtUserID=@User And dfTable=DDSecurityTables.dtTable And dfField=DDSecurityTables.dtField");
		sqlCommand.Parameters.Add(new SqlParameter("@Database", SqlDbType.NVarChar)).Value = datasetID;
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		foreach (DataRow row in DataDictionary.GetDataTable(sqlCommand).Rows)
		{
			DataRow dataRow2 = securityTable.NewRow();
			dataRow2["dtDataset"] = datasetID;
			dataRow2["dfTable"] = row["dfTable"];
			dataRow2["dfField"] = row["dfField"];
			dataRow2["dtCaption"] = row["dtCaption"];
			dataRow2["dfCaption"] = row["dfCaption"];
			dataRow2["dtLevel"] = Databases[0].Security.GetFieldAccessLevel(row.Field<string>("dfTable"), row.Field<string>("dfField"), SecurityAccessLevel.Default, datasetID, userID);
			securityTable.Rows.Add(dataRow2);
		}
	}

	public DataTable ResolvedReportSecurity(string userID, bool showInReport)
	{
		DataTable dataTable = new DataTable();
		dataTable.Columns.AddRange(new DataColumn[4]
		{
			new DataColumn("drDataset", typeof(string)),
			new DataColumn("drFolder", typeof(string)),
			new DataColumn("drReport", typeof(string)),
			new DataColumn("drLevel", typeof(byte))
		});
		dataTable.PrimaryKey = new DataColumn[3]
		{
			dataTable.Columns["drDataset"],
			dataTable.Columns["drFolder"],
			dataTable.Columns["drReport"]
		};
		if (showInReport)
		{
			SqlCommand sqlCommand = DataDictionary.NewSqlCommand("Select drDataset,drFolder,drReport,drLevel from ddSecurityReports WHERE drUserID = @User");
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
			DataTable dataTable2 = DataDictionary.GetDataTable(sqlCommand);
			dataTable2.PrimaryKey = new DataColumn[3]
			{
				dataTable2.Columns["drDataset"],
				dataTable2.Columns["drFolder"],
				dataTable2.Columns["drReport"]
			};
			foreach (string reportFolder in Context.Reports.GetReportFolders())
			{
				addReportRow(dataTable, dataTable2, string.Empty, reportFolder, string.Empty, userID);
				foreach (DatabaseInfo installedDatabase in Context.InstalledDatabases)
				{
					addReportRow(dataTable, dataTable2, installedDatabase.Name.ToUpper(), reportFolder, string.Empty, userID);
				}
				foreach (FileInfo item in Context.Reports.GetReportsForTemplate(reportFolder, string.Empty))
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(item.Name);
					addReportRow(dataTable, dataTable2, string.Empty, reportFolder, fileNameWithoutExtension, userID);
					foreach (DatabaseInfo installedDatabase2 in Context.InstalledDatabases)
					{
						addReportRow(dataTable, dataTable2, installedDatabase2.Name.ToUpper(), reportFolder, fileNameWithoutExtension, userID);
					}
				}
			}
		}
		return dataTable;
	}

	private void addReportRow(DataTable rowTable, DataTable securityTable, string databaseName, string folderName, string reportName, string userID)
	{
		DataRow dataRow = rowTable.NewRow();
		dataRow.BeginEdit();
		dataRow["drDataset"] = databaseName;
		dataRow["drFolder"] = folderName;
		dataRow["drReport"] = reportName;
		dataRow["drLevel"] = Databases[0].Security.GetReportAccessLevel(dataRow.Field<string>("drFolder"), dataRow.Field<string>("drReport"), databaseName, userID);
		dataRow.EndEdit();
		rowTable.Rows.Add(dataRow);
	}

	public DataTable ResolvedComponentSecurity(string userID, bool showInReport)
	{
		DataTable dataTable;
		if (showInReport)
		{
			SqlCommand sqlCommand = DataDictionary.NewSqlCommand("SELECT dzGroupID, dzUserID, dzDataset, duName, Convert(int, 1) As OriginalState From DDSecurityGroups Inner Join DDUsers ON dzGroupID = duUserID Where duType = @Type And dzUserID = @User Order By duName");
			sqlCommand.Parameters.Add(new SqlParameter("@Type", SqlDbType.Int)).Value = 2;
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
			dataTable = DataDictionary.GetDataTable(sqlCommand);
			string text = string.Empty;
			foreach (DatabaseInfo installedDatabase in Context.InstalledDatabases)
			{
				text = text + " OR dzDataset='" + installedDatabase.Name + "'";
			}
			if (text.Length != 0)
			{
				if (text.StartsWith(" OR "))
				{
					text = text.Substring(4);
				}
				DataRow[] array = dataTable.Select(text);
				if (array.Length != 0)
				{
					return array.CopyToDataTable();
				}
			}
		}
		else
		{
			SqlCommand sqlCommand2 = DataDictionary.NewSqlCommand("SELECT dzGroupID, dzUserID, dzDataset, duName, Convert(int, 1) As OriginalState From DDSecurityGroups Inner Join DDUsers ON dzGroupID = duUserID Where 0=1");
			dataTable = DataDictionary.GetDataTable(sqlCommand2);
		}
		return dataTable;
	}

	public void Dispose()
	{
		if (Settings != null)
		{
			Settings.Dispose();
			Settings = null;
		}
		if (Databases != null)
		{
			Databases.Dispose();
			Databases = null;
		}
		Context = null;
		DataDictionary = null;
		if (serviceContainer != null)
		{
			serviceContainer.Dispose();
			serviceContainer = null;
		}
		Theme = null;
	}
}
