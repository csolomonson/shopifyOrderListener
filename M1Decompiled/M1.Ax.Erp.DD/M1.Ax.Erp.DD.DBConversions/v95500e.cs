using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.500", "Rename fields on Form 1099-MISC", "2022-10-31")]
public class v95500e
{
	public v95500e(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox17"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox17", "apfBox18", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox16"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox16", "apfBox17", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox15"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox15", "apfBox16", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox14"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox14", "apfBox15", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox13"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox13", "apfBox14", dropTriggers: true);
		}
	}
}
