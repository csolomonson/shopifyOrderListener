using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.500", "Adding FATCA Filling Requirement field on Form 1099-MISC", "2022-10-31")]
public class v95500f
{
	public v95500f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox13"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099MISCYearTotals", "apfBox13", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
