using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.746", "Update FinancialProperties credit card method options", "2018-07-04")]
public class v92746a
{
	public v92746a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafCreditCardMethod"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FinancialProperties Set xafCreditCardMethod = 0 Where xafCreditCardMethod = 1");
		}
	}
}
