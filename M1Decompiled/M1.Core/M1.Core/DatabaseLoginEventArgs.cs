using System;

namespace M1.Core;

public class DatabaseLoginEventArgs : EventArgs
{
	public M1Database Database;

	public M1User User;

	public DatabaseLoginEventArgs(M1User user, M1Database database)
	{
		Database = database;
		User = user;
	}
}
