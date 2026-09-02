using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.561", "Add fields to FinancialProperties table", "2017-11-01")]
public class v92561
{
	public v92561(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafARDiscountOnFreight"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafARDiscountOnFreight", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FinancialProperties Set xafARDiscountOnFreight = 1");
		}
	}
}
