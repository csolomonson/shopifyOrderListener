using System;
using System.Text;
using Microsoft.Win32;

namespace M1.Core;

public class M1MobileDbSettings
{
	private static string subKeyPath = "Software\\M1Mobile\\DbConfigKeys";

	private static string dataDicKey = "M1_DATADICTIONARY_KEY";

	private static string dbServerKey = "M1_DBSERVER_KEY";

	private static string dbLoginIDKey = "M1_DBLOGINID_KEY";

	private static string dbPasswordKey = "M1_DBPASSWORD_KEY";

	private static string dbNetworkLibraryKey = "M1_NETWORKLIBRARY_KEY";

	private static string dbTrustedKey = "M1_TRUSTED_KEY";

	private static string subKeySessionPath = "Software\\M1Mobile\\ManageSession";

	private static string sessionDefaultExpKey = "M1_SESSION_DEF_EXP_KEY";

	private static string sessionExpUnitKey = "M1_SESSION_EXP_UNIT_KEY";

	private static string sessionExpUnitValueKey = "M1_SESSION_EXP_UNITVALUE_KEY";

	public string DataDictionary;

	public string Server;

	public string LoginID;

	public string Password;

	public string NetworkLibrary;

	public bool TrustedConnection;

	public bool DefaultSessionExp = true;

	public string SessionExpUnit = "";

	public string SessionExpUnitValue = "";

	public M1MobileDbSettings LoadSessionSettings(string App)
	{
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine;
			string name = new StringBuilder(subKeySessionPath).Append($"\\{App}").ToString();
			using RegistryKey registryKey2 = registryKey.OpenSubKey(name);
			if (registryKey2 != null)
			{
				return new M1MobileDbSettings
				{
					DefaultSessionExp = (registryKey2.GetValue(sessionDefaultExpKey) == null || Convert.ToBoolean(registryKey2.GetValue(sessionDefaultExpKey))),
					SessionExpUnit = ((registryKey2.GetValue(sessionExpUnitKey) != null) ? registryKey2.GetValue(sessionExpUnitKey).ToString() : ""),
					SessionExpUnitValue = ((registryKey2.GetValue(sessionExpUnitValueKey) != null) ? registryKey2.GetValue(sessionExpUnitValueKey).ToString() : "")
				};
			}
			return null;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public M1MobileDbSettings LoadMobileSettings()
	{
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine;
			using RegistryKey registryKey2 = registryKey.OpenSubKey(subKeyPath);
			if (registryKey2 == null)
			{
				throw new Exception("Database not defined, please configure database info first");
			}
			return new M1MobileDbSettings
			{
				DataDictionary = ((registryKey2.GetValue(dataDicKey) != null) ? registryKey2.GetValue(dataDicKey).ToString() : "M1DD"),
				Server = ((registryKey2.GetValue(dbServerKey) != null) ? registryKey2.GetValue(dbServerKey).ToString() : string.Empty),
				LoginID = ((registryKey2.GetValue(dbLoginIDKey) != null) ? registryKey2.GetValue(dbLoginIDKey).ToString() : "sa"),
				Password = ((registryKey2.GetValue(dbPasswordKey) != null) ? registryKey2.GetValue(dbPasswordKey).ToString() : string.Empty),
				NetworkLibrary = ((registryKey2.GetValue(dbNetworkLibraryKey) != null) ? registryKey2.GetValue(dbNetworkLibraryKey).ToString() : "dbmssocn"),
				TrustedConnection = (registryKey2.GetValue(dbTrustedKey) == null || Convert.ToBoolean(registryKey2.GetValue(dbTrustedKey)))
			};
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void SaveSessionInfo(string App)
	{
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine;
			string text = new StringBuilder(subKeySessionPath).Append($"\\{App}").ToString();
			RegistryKey registryKey2 = registryKey.OpenSubKey(text, RegistryKeyPermissionCheck.ReadWriteSubTree);
			if (registryKey2 == null)
			{
				registryKey.CreateSubKey(text);
				registryKey2 = registryKey.OpenSubKey(text, RegistryKeyPermissionCheck.ReadWriteSubTree);
			}
			registryKey2.SetValue(sessionDefaultExpKey, DefaultSessionExp, RegistryValueKind.String);
			registryKey2.SetValue(sessionExpUnitKey, SessionExpUnit, RegistryValueKind.String);
			registryKey2.SetValue(sessionExpUnitValueKey, SessionExpUnitValue, RegistryValueKind.String);
			registryKey2.Close();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void SaveDBInfo()
	{
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine;
			using RegistryKey registryKey2 = registryKey.OpenSubKey(subKeyPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
			registryKey2.SetValue(dataDicKey, DataDictionary, RegistryValueKind.String);
			registryKey2.SetValue(dbServerKey, Server, RegistryValueKind.String);
			registryKey2.SetValue(dbLoginIDKey, LoginID, RegistryValueKind.String);
			registryKey2.SetValue(dbPasswordKey, Password, RegistryValueKind.String);
			registryKey2.SetValue(dbNetworkLibraryKey, NetworkLibrary, RegistryValueKind.String);
			registryKey2.SetValue(dbTrustedKey, TrustedConnection, RegistryValueKind.String);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void DeleteRegInfo()
	{
		try
		{
			RegistryKey localMachine = Registry.LocalMachine;
			localMachine.OpenSubKey(subKeyPath);
			localMachine.DeleteSubKeyTree(subKeyPath);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void SaveSubKeyPath()
	{
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine;
			using RegistryKey registryKey2 = registryKey.OpenSubKey(subKeyPath);
			if (registryKey2 == null)
			{
				registryKey.CreateSubKey(subKeyPath);
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
