using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.200", "Rename Form1099YearTotals table to Form1099MISCYearTotals", "2021-08-12")]
public class v94200e
{
	public v94200e(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form1099YearTotals"))
		{
			parms.Dmo.RenameTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099YearTotals", "Form1099MISCYearTotals");
		}
	}
}
