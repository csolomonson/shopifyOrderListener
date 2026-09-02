namespace M1.Core;

public class DBCreateDefaultParms
{
	public ServerManager ServerManager;

	public M1User User;

	public M1DataDictionary DataDictionary;

	public M1Database Database;

	public string DatabaseName;

	public DBCreateDefaultParms(ServerManager serverManager, M1User user, M1DataDictionary dataDictionary, string databaseName, M1Database database)
	{
		ServerManager = serverManager;
		User = user;
		DataDictionary = dataDictionary;
		DatabaseName = databaseName;
		Database = database;
	}
}
