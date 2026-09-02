using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.6.200", "Add fields to PartForecastLines table", "2023-04-27")]
public class v96200
{
	public v96200(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartForecastLines", "inlIncludeInMRP"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartForecastLines", "inlIncludeInMRP", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
