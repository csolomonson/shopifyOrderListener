using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("Timecard")]
[ComVisible(true)]
public class AppAxTimecard
{
	private IServiceProvider provider;

	public AppAxTimecard(IServiceProvider parentProvider)
	{
		provider = parentProvider;
	}

	public void RecalculateHeaderTimes(object timecard, object transaction)
	{
		if (timecard is DataRow)
		{
			Timecard.RecalculateHeaderTimes(provider.GetService(typeof(M1Database)) as M1Database, (DataRow)timecard, (SqlTransaction)transaction);
		}
		else
		{
			Timecard.RecalculateHeaderTimes(provider.GetService(typeof(M1Database)) as M1Database, Convert.ToInt32(timecard), (SqlTransaction)transaction);
		}
	}

	public void RecalculateIdleTimes(object timecardRow)
	{
	}
}
