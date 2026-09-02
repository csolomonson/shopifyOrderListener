using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.069", "Add fields to PurchasePlannerLines table", "2016-06-03")]
public class v91069a
{
	public v91069a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerLines", "pplNonStockedItem"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerLines", "pplNonStockedItem", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
