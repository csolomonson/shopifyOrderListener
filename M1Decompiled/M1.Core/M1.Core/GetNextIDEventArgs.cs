using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
public class GetNextIDEventArgs : DbAndRowEventArgs
{
	public object Value;

	public GetNextIDEventArgs(M1Database database, DataRow row, SqlTransaction transaction)
		: base(database, row, transaction)
	{
	}
}
