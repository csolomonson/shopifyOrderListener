using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.038", "Add ETP Payment Type Code to IncomeTaxYearTotals", "2013-09-19")]
public class v810038d
{
	public v810038d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "IncomeTaxYearTotals", "pahAUSETPPaymentType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxYearTotals", "pahAUSETPPaymentType", "char", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
