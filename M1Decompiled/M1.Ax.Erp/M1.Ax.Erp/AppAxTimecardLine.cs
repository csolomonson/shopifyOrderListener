using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("TimecardLine")]
[ComVisible(true)]
public class AppAxTimecardLine
{
	private IServiceProvider provider;

	public AppAxTimecardLine(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public void BackOutJobOperation(DataRow row, object transaction)
	{
		new TimecardLine().BackOutJobOperation(provider.GetService(typeof(M1Database)) as M1Database, row, (SqlTransaction)transaction);
	}

	public void AddToJobOperation(DataRow row, object transaction)
	{
		new TimecardLine().AddToJobOperation(provider.GetService(typeof(M1Database)) as M1Database, row, (SqlTransaction)transaction);
	}
}
