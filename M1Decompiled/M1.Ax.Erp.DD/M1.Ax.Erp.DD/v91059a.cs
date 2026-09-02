using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.059", "Add fields to PurchasePlannerLines table", "2016-05-20")]
public class v91059a
{
	public v91059a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerLines", "pplDataMissing"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerLines", "pplDataMissing", "int", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
