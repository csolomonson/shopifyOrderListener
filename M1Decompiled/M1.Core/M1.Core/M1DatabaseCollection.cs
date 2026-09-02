using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace M1.Core;

public class M1DatabaseCollection : KeyedCollection<string, M1Database>, IDisposable
{
	private M1DataDictionary dataDictionary;

	private M1User user;

	private IServiceProvider serviceProvider;

	public event EventHandler<DatabaseVersionCheckEventArgs> DatabaseVersionCheck;

	public event EventHandler<DatabaseLoginEventArgs> DatabaseLogin;

	public event EventHandler<DatabaseLoginEventArgs> DatabaseLogout;

	public M1DatabaseCollection(IServiceProvider provider)
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
		serviceProvider = provider;
		dataDictionary = (M1DataDictionary)provider.GetService(typeof(M1DataDictionary));
		user = (M1User)provider.GetService(typeof(M1User));
	}

	protected override string GetKeyForItem(M1Database item)
	{
		return item.ID;
	}

	protected void OnDatabaseVersionCheck(DatabaseVersionCheckEventArgs args)
	{
		this.DatabaseVersionCheck?.Invoke(this, args);
	}

	protected void OnDatabaseLogin(M1Database database)
	{
		this.DatabaseLogin?.Invoke(this, new DatabaseLoginEventArgs(user, database));
	}

	protected void OnDatabaseLogout(M1Database database)
	{
		this.DatabaseLogout?.Invoke(this, new DatabaseLoginEventArgs(user, database));
	}

	public bool CheckVersion(string databaseName, string extraMsg)
	{
		bool result = false;
		string empty = string.Empty;
		DatabaseInfo datasetProperties = user.Context.DBServerManager.GetDatasetProperties(null, user, databaseName);
		if (datasetProperties != null)
		{
			string versionString = dataDictionary.AppExtensions.GetVersionString();
			empty = user.Context.Version;
			if (!empty.Equals(datasetProperties.Version) || !versionString.Equals(datasetProperties.ExtensionVersions))
			{
				DatabaseVersionCheckEventArgs e = new DatabaseVersionCheckEventArgs(serviceProvider, empty, versionString, databaseName, datasetProperties.Version, datasetProperties.ExtensionVersions, datasetProperties.Description, extraMsg);
				OnDatabaseVersionCheck(e);
				return !e.Cancel;
			}
			result = true;
		}
		return result;
	}

	public M1Database GetDatabaseRef(M1Database defaultDb, string databaseName, LoginCredentials loginCredentials, bool readOnlyLogin)
	{
		M1Database result = null;
		if (databaseName == null || databaseName.Length == 0 || databaseName.Equals(defaultDb.ID, StringComparison.CurrentCultureIgnoreCase))
		{
			if (!readOnlyLogin && defaultDb.ReadOnlyLogin)
			{
				defaultDb.UpgradeToFullLogin();
			}
			result = defaultDb;
		}
		else
		{
			LoginReturnInfo loginReturnInfo = LoginUsingPassedCredentials(databaseName, loginCredentials, readOnlyLogin);
			if (loginReturnInfo != null && loginReturnInfo.Database != null)
			{
				result = loginReturnInfo.Database;
			}
		}
		return result;
	}

	public LoginReturnInfo LoginUsingPassedCredentials(string database, LoginCredentials loginCredentials, bool readOnlyLogin)
	{
		LoginReturnInfo loginReturnInfo = new LoginReturnInfo();
		foreach (M1Database database2 in user.Databases)
		{
			if (database2.ID.Equals(database, StringComparison.CurrentCultureIgnoreCase) && database2.LoginCredentials.IsMatchingCredentials(loginCredentials))
			{
				if (!readOnlyLogin && database2.ReadOnlyLogin)
				{
					database2.UpgradeToFullLogin();
				}
				loginReturnInfo.Database = database2;
				break;
			}
		}
		if (loginReturnInfo.Database == null)
		{
			if (!CheckVersion(database, string.Empty))
			{
				ByPassUI byPassUI = (ByPassUI)user.GetService(typeof(ByPassUI));
				string text = "The version of database " + database + " does not match the current version of the application.";
				byPassUI?.ExceptionOutput.Add(database + ":" + text);
				throw new M1Exception(text);
			}
			Mutex mutex = new Mutex(initiallyOwned: false, "M1" + dataDictionary.Version + "_" + dataDictionary.ID.ToUpper() + "_" + user.ID.ToUpper() + "_" + database.ToUpper());
			mutex.WaitOne();
			try
			{
				loginReturnInfo = new LoginReturnInfo();
				foreach (M1Database database3 in user.Databases)
				{
					if (database3.ID == database && database3.LoginCredentials.IsMatchingCredentials(loginCredentials))
					{
						loginReturnInfo.Database = database3;
						break;
					}
				}
				if (loginReturnInfo.Database == null)
				{
					loginReturnInfo.Database = new M1Database(serviceProvider, user.Context.DBServerManager);
					loginReturnInfo.Database.Login(database, user, loginCredentials, readOnlyLogin);
					if (loginReturnInfo.Database.IsOpen)
					{
						loginReturnInfo.DatabaseCreated = true;
						Add(loginReturnInfo.Database);
						OnDatabaseLogin(loginReturnInfo.Database);
					}
				}
			}
			finally
			{
				mutex.ReleaseMutex();
				mutex = null;
			}
		}
		return loginReturnInfo;
	}

	public bool LogoutAndRemove(M1Database m1Database)
	{
		M1Database m1Database2 = null;
		for (int num = base.Count - 1; num >= 0; num--)
		{
			if (m1Database == null || base[num] == m1Database)
			{
				m1Database2 = base[num];
				if (!m1Database2.IsLoggingOut)
				{
					string iD = m1Database2.ID;
					if (!m1Database2.Logout())
					{
						return false;
					}
					OnDatabaseLogout(m1Database2);
					if (base.Dictionary.ContainsKey(iD))
					{
						base.Dictionary.Remove(iD);
					}
					if (Contains(m1Database2))
					{
						Remove(m1Database2);
					}
				}
			}
		}
		return true;
	}

	public void Dispose()
	{
		this.DatabaseLogin = null;
		this.DatabaseVersionCheck = null;
		this.DatabaseLogout = null;
		serviceProvider = null;
		dataDictionary = null;
		user = null;
	}
}
