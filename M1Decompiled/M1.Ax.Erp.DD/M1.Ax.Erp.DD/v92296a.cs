using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.296", "Add fields for supplier requirements", "2017-06-14")]
public class v92296a
{
	public v92296a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchaseOrderLines", "pmlSupplierRequirement"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderLines", "pmlSupplierRequirement", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerOrderDetails", "ppoSupplierRequirement"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerOrderDetails", "ppoSupplierRequirement", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
