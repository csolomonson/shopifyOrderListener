using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.027", "Add fields to ScheduleDates table", "2015-03-31")]
public class v900027c
{
	public v900027c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ScheduleDates", "sxdResourceLane"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleDates", "sxdResourceLane", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
