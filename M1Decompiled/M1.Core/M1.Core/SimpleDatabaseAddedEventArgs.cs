using System;

namespace M1.Core;

public class SimpleDatabaseAddedEventArgs : EventArgs
{
	public M1Database Database;

	public SimpleDatabaseAddedEventArgs(M1Database database)
	{
		Database = database;
	}
}
