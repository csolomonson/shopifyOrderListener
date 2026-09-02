using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
public class AddNewCompletedEventArgs : DbAndRowEventArgs
{
	public string FocusField = string.Empty;

	public AddNewCompletedEventArgs(M1Database database, DataRow row, SqlTransaction transaction, string focusField)
		: base(database, row, transaction)
	{
		FocusField = focusField;
	}
}
