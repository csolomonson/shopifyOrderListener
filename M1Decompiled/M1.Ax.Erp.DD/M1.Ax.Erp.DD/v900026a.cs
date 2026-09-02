using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.026", "Remove resources table", "2015-03-31")]
public class v900026a
{
	public v900026a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleResources"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "ScheduleResources");
		}
	}
}
