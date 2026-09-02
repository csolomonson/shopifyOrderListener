using System.Data;
using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
public class CurrentChangedEventArgs : DbAndRowEventArgs
{
	public CurrentChangedEventArgs(M1Database database, DataRow row)
		: base(database, row, null)
	{
	}
}
