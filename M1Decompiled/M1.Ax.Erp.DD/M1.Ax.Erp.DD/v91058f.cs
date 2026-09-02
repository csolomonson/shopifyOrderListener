using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.058", "Add fields to Purchase Order Lines table", "2016-05-18")]
public class v91058f
{
	public v91058f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchaseOrderLines", "pmlSourceTableUniqueID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderLines", "pmlSourceTableUniqueID", "uniqueidentifier", 16, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchaseOrderLines", "pmlSourceTableName"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderLines", "pmlSourceTableName", "nvarchar", 30, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchaseOrderLines", "pplCompleted"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderLines", "pplCompleted", dropTriggers: true);
		}
	}
}
