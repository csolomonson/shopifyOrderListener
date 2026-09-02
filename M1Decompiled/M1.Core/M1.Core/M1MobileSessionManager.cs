using System.IO;

namespace M1.Core;

public class M1MobileSessionManager
{
	public string DataDictionary;

	public string Server;

	public string LoginID;

	public string Password;

	public string NetworkLibrary;

	public bool TrustedConnection;

	public M1MobileSessionManager LoadMobileSettings(InstallationInfo server)
	{
		string path = server.Location + "m1.ini";
		if (File.Exists(path))
		{
			server.IniSettings = new IniSettings();
			server.IniSettings.setCurrentPath(server.Location);
			server.IniSettings.LoadM1IniSettings(path);
			DataDictionary = server.IniSettings.Get("DataDictionary", "M1DD");
			Server = server.IniSettings.Get("DBServer", "(local)");
			LoginID = server.IniSettings.Get("LoginID", "sa");
			Password = server.IniSettings.Get("DBPwd", string.Empty);
			NetworkLibrary = server.IniSettings.Get("DBNetworkLibrary", "dbmssocn");
			TrustedConnection = bool.Parse(server.IniSettings.Get("TrustedConnection", "False"));
			return this;
		}
		return null;
	}

	public M1MobileSessionManager LoadMobileSettings(AppContext client)
	{
		try
		{
			return client.IsHosted ? LoadHostedConfiguration(client) : LoadFromINIFile(client);
		}
		catch
		{
			throw;
		}
	}

	public M1MobileSessionManager LoadSessionSettings(AppContext client)
	{
		if (File.Exists(client.Server.Location + "m1.ini"))
		{
			InstallationInfo server = client.Server;
			server.IniSettings = new IniSettings();
			server.IniSettings.setCurrentPath(client.Server.Location);
			return this;
		}
		return null;
	}

	private M1MobileSessionManager LoadHostedConfiguration(AppContext client)
	{
		_ = client.Server;
		return new M1MobileSessionManager
		{
			DataDictionary = client.Metadata.GetMetaData("DataDictionary"),
			Server = client.Metadata.GetMetaData("Server_Instance"),
			LoginID = client.Metadata.GetMetaData("Elevated_User"),
			Password = string.Empty,
			NetworkLibrary = "dbmssocn",
			TrustedConnection = true
		};
	}

	private M1MobileSessionManager LoadFromINIFile(AppContext client)
	{
		M1MobileSessionManager result = null;
		string path = client.Server.Location + "m1.ini";
		if (File.Exists(path))
		{
			InstallationInfo server = client.Server;
			server.IniSettings = new IniSettings();
			server.IniSettings.setCurrentPath(client.Server.Location);
			server.IniSettings.LoadM1IniSettings(path);
			result = new M1MobileSessionManager
			{
				DataDictionary = server.IniSettings.Get("DataDictionary", "M1DD"),
				Server = server.IniSettings.Get("DBServer", "(local)"),
				LoginID = server.IniSettings.Get("LoginID", "sa"),
				Password = server.IniSettings.Get("DBPwd", string.Empty),
				NetworkLibrary = server.IniSettings.Get("DBNetworkLibrary", "dbmssocn"),
				TrustedConnection = bool.Parse(server.IniSettings.Get("TrustedConnection", "False"))
			};
		}
		return result;
	}
}
