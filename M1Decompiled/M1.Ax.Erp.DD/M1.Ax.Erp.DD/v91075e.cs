using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.075", "Add fields to PurchasePlannerLines table", "2016-06-14")]
public class v91075e
{
	public v91075e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerLines", "pplPhantomOrKitPart"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerLines", "pplPhantomOrKitPart", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
