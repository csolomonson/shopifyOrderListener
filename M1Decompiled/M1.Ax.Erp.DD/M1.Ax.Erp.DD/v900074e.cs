using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.074", "Add fields to PurchaseOrderLines table", "2015-08-14")]
public class v900074e
{
	public v900074e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchaseOrderLines", "pmlQuantityOnOrder"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderLines", "pmlQuantityOnOrder", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchaseOrderLines", "pmlQuantityOnOrder"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PurchaseOrderLines Set pmlQuantityOnOrder = CASE WHEN pmlInventoryQuantity-pmlInventoryQuantityReceived <= 0 OR pmlReceivedComplete <> 0 THEN 0 ELSE pmlInventoryQuantity-pmlInventoryQuantityReceived END");
		}
	}
}
