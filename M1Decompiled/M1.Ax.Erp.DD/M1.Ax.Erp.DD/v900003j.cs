using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.003", "Add fields to PurchaseOrderComponents table", "2014-09-25")]
public class v900003j
{
	public v900003j(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchaseOrderComponents", "pmoParentQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderComponents", "pmoParentQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PurchaseOrderComponents Set pmoParentQuantity = pmlInventoryQuantity From PurchaseOrderLines Inner Join PurchaseOrderComponents On PMLPURCHASEORDERID = PMOPURCHASEORDERID And PMLPURCHASEORDERLINEID = PMOPURCHASEORDERLINEID");
		}
	}
}
