using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.059", "Add fields to PurchasePlannerOrderDetails table", "2016-05-20")]
public class v91059b
{
	public v91059b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerOrderDetails", "ppoDataMissing"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerOrderDetails", "ppoDataMissing", "int", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerOrderDetails", "ppoDataMissing"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PurchasePlannerOrderDetails Set ppoDataMissing = (case when RTrim(ppoSupplierOrganizationID) = '' Then 1 Else 0 End)");
		}
	}
}
