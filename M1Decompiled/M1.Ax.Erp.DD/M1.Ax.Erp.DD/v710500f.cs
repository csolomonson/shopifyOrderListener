using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.500", "Add Calculate Discount on Freight to Financial properties", "2009-03-02")]
public class v710500f
{
	public v710500f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAPDiscountOnFreight"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAPDiscountOnFreight", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
