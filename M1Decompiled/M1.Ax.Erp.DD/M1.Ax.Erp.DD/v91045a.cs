using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.045", "Add fields to ScheduleTrees table", "2016-04-22")]
public class v91045a
{
	public v91045a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTrees", "sxtScheduleTreeID"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTrees", "sxtScheduleTreeID", "int", 4, 0, isNullable: false, parms.Messages);
		}
	}
}
