using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.6.300", "Update field xafCAEmployerDentalBenefits on FinancialProperties table", "2023-10-26")]
public class v96300b
{
	public v96300b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafCAEmployerDentalBenefits"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FinancialProperties SET xafCAEmployerDentalBenefits = 1");
		}
	}
}
