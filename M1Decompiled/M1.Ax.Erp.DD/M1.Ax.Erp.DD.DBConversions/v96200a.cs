using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.6.200", "Add fields to MRPSessions table", "2023-04-27")]
public class v96200a
{
	public v96200a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPSessions", "mrpIncludePartForecasts"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPSessions", "mrpIncludePartForecasts", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPSessions", "mrpConsolidatePartForecastJobs"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPSessions", "mrpConsolidatePartForecastJobs", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
