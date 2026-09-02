using System;
using System.Collections.Generic;

namespace M1.Core;

public class DBConversionParms : IDisposable
{
	public ServerManager ServerManager;

	public Dmo Dmo;

	public M1User User;

	public M1DataDictionary DataDictionary;

	public M1Database Database;

	public string DatabaseName;

	public string InitialVersion;

	public List<string> Messages = new List<string>();

	public string HeaderMessage { get; set; }

	public string FileToSave { get; set; }

	public bool ShowSaveMessageButton { get; set; } = true;

	public event EventHandler<DBConversionStatusUpdatedEventArgs> StatusUpdated;

	public DBConversionParms(ServerManager serverManager, Dmo dmo, M1User user, M1DataDictionary dataDictionary, string databaseName)
	{
		ServerManager = serverManager;
		Dmo = dmo;
		User = user;
		DataDictionary = dataDictionary;
		DatabaseName = databaseName;
		Database = new M1Database(user, serverManager);
		Database.LoginLite(DatabaseName, user);
	}

	public void OnStatusUpdated(DBConversionStatusUpdatedEventArgs e)
	{
		this.StatusUpdated?.Invoke(this, e);
	}

	public void Dispose()
	{
		if (Database != null)
		{
			Database.QuickExit();
			Database = null;
		}
	}
}
