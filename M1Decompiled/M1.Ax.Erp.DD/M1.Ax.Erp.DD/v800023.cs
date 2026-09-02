using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.023", "Add 941 tables and fields", "2010-05-03")]
public class v800023
{
	public v800023(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form941Years"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form941Years");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form941YearQuarters"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form941YearQuarters");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form941Schedules"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form941Schedules");
		}
	}
}
