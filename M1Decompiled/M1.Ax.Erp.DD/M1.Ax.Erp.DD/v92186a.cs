using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.186", "Add fields to FinancialProperties table", "2017-03-10")]
public class v92186a
{
	public v92186a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafProductionExpressPost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafProductionExpressPost", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
