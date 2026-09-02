using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
public class RemoveEventArgs : DbAndRowEventArgs
{
	public bool Cancel;

	public RemoveEventArgs(M1Database database, DataRow row, SqlTransaction transaction)
		: base(database, row, transaction)
	{
	}
}
