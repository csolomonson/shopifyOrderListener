using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.6.200", "Add fields to ProductionProperties table", "2023-05-09")]
public class v96200c
{
	public v96200c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapJMMRPForecastFirmJob"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapJMMRPForecastFirmJob", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
