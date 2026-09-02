using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.500", "Add Part Forecast tables", "2009-02-12")]
public class v710500b
{
	public v710500b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PartForecasts"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartForecasts");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PartForecastLines"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartForecastLines");
		}
	}
}
