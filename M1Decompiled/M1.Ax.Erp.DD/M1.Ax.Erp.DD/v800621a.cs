using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.621", "Add C3 Form tables and fields", "2016-04-06")]
public class v800621a
{
	public v800621a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "StateUITaxYears"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "StateUITaxYears");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "StateUITaxYearQuarters"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "StateUITaxYearQuarters");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "StateUITaxYearQuarterTotals"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "StateUITaxYearQuarterTotals");
		}
	}
}
