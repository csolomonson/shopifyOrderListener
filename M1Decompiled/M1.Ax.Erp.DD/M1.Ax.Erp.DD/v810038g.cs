using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.038", "Add Fields to PaymentTerms table", "2013-09-19")]
public class v810038g
{
	public v810038g(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PaymentTerms", "xatCalculationDayOfMonth"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PaymentTerms", "xatCalculationDayOfMonth", "numeric", 2, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE PaymentTerms SET xatCalculationDayOfMonth = 20 WHERE xatCalculationType = 3");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PaymentTerms", "xatDiscountDayOfMonth"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PaymentTerms", "xatDiscountDayOfMonth", "numeric", 2, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
