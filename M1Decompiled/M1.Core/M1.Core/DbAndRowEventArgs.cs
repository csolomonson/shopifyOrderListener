using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
public class DbAndRowEventArgs : EventArgs, IDisposable
{
	public SqlTransaction SqlTransaction;

	public M1Database Database;

	public DataRow Row;

	public DbAndRowEventArgs(M1Database database, DataRow row, SqlTransaction transaction)
	{
		Database = database;
		Row = row;
		SqlTransaction = transaction;
	}

	public void Dispose()
	{
		if (SqlTransaction != null)
		{
			SqlTransaction.Dispose();
			SqlTransaction = null;
		}
	}
}
