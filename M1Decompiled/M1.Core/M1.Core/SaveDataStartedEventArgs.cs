using System;
using System.Data.SqlClient;

namespace M1.Core;

public class SaveDataStartedEventArgs : EventArgs
{
	public M1Database Database;

	public SqlTransaction SqlTransaction;

	public bool Cancel;

	public bool UpdateDeletedRowsOnly;
}
