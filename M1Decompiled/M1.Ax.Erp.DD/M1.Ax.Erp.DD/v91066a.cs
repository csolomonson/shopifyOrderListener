using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.066", "Add fields to PurchasePlannerLines table", "2016-06-01")]
public class v91066a
{
	public v91066a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerLines", "pplReorderMethod"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerLines", "pplReorderMethod", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
