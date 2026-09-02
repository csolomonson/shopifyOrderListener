using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.500", "Add Forecast Fields to Jobs", "2009-02-12")]
public class v710500c
{
	public v710500c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Jobs", "jmpPartForecastYearID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Jobs", "jmpPartForecastYearID", "numeric", 4, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Jobs", "jmpPartForecastPeriodID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Jobs", "jmpPartForecastPeriodID", "numeric", 4, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
