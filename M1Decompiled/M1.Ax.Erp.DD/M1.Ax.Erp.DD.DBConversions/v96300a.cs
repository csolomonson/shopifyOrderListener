using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.6.300", "Add xafCAEmployerDentalBenefits to FinancialProperties table", "2023-10-26")]
public class v96300a
{
	public v96300a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafCAEmployerDentalBenefits"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafCAEmployerDentalBenefits", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
