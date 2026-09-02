using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.200", "Remove apfBox15A, apfBox15B and apfBox18 fields from Form1099MISCYearTotals table", "2021-08-16")]
public class v94200g
{
	public v94200g(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox15A"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox15A", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox15B"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox15B", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox18"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox18", dropTriggers: true);
		}
	}
}
