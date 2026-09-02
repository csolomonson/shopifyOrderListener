using System.IO;

namespace M1.Core;

public class M1EDISourceSessionManager
{
	public string DataDictionary;

	public string DBServer;

	public string LoginID;

	public string Password;

	public string NetworkLibrary;

	public bool TrustedConnection;

	public string M1ServiceURL = string.Empty;

	public M1EDISourceSessionManager LoadEDISourceSettings(InstallationInfo server)
	{
		try
		{
			string path = Path.Combine(server.Location, "m1.ini");
			if (File.Exists(path))
			{
				server.IniSettings = new IniSettings();
				server.IniSettings.setCurrentPath(server.Location);
				server.IniSettings.LoadM1IniSettings(path);
				return new M1EDISourceSessionManager
				{
					DataDictionary = server.IniSettings.Get("DataDictionary", "M1DD"),
					DBServer = server.IniSettings.Get("DBServer", "(local)"),
					LoginID = server.IniSettings.Get("DBUserID", "sa"),
					Password = server.IniSettings.Get("DBPwd", string.Empty),
					NetworkLibrary = server.IniSettings.Get("DBNetworkLibrary", "dbmssocn"),
					TrustedConnection = bool.Parse(server.IniSettings.Get("DBTrustedConnection", "False")),
					M1ServiceURL = server.IniSettings.Get("M1ServiceURL", "")
				};
			}
			return null;
		}
		catch
		{
			throw;
		}
	}
}
