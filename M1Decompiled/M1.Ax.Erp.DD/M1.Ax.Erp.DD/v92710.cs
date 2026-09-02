using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.710", "Add fields to FinancialProperties table", "2018-05-17")]
public class v92710
{
	public v92710(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafExactDaysInPaymentTerms"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafExactDaysInPaymentTerms", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
