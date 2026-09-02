using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.621", "Add 940 tables and fields", "2016-04-06")]
public class v800621
{
	public v800621(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form940Years"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form940Years");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form940YearTotals"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form940YearTotals");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form940YearTotalStates"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form940YearTotalStates");
		}
	}
}
