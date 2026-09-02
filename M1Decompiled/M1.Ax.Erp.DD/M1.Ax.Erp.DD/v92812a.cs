using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.812", "Resize PercentComplete field in ScheduleTaskBuckets table", "2019-01-30")]
public class v92812a
{
	public v92812a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleTaskBuckets", "sxePercentComplete"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleTaskBuckets", "sxePercentComplete", "int", 10, 0, isNullable: false, parms.Messages);
		}
	}
}
