using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.272", "Add fields to IncomeTaxYearTotals table", "2017-05-11")]
public class v92272a
{
	public v92272a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahAUSFBTExemptType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahAUSFBTExemptType", "char", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
