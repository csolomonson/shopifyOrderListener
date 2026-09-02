using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.075", "Add fields to PurchasePlannerLines table", "2016-06-10")]
public class v91075a
{
	public v91075a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerLines", "pplExtendedCostBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerLines", "pplExtendedCostBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
