using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.101", "Add utc offset for shop floor web to Timecards table", "2021-07-20")]
public class v94101b
{
	public v94101b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Timecards", "lmpUtcOffset"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Timecards", "lmpUtcOffset", "smallint", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
