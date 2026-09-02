using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.200", "Add form type column on Form 1099 Year Totals form", "2021-08-11")]
public class v94200a
{
	public v94200a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Form1099YearTotals", "apfFormType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099YearTotals", "apfFormType", "tinyint", 1, 0, verifyIndexes: false, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
