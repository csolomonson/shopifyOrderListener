using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.057", "Add fields to PurchaseOrderLines table", "2016-12-19")]
public class v92057a
{
	public v92057a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchaseOrderLines", "pmlTotalComponentCosts"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderLines", "pmlTotalComponentCosts", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PurchaseOrderLines Set pmlTotalComponentCosts = DetailAmount From PurchaseOrderLines Inner Join (Select pmoPurchaseOrderID,pmoPurchaseOrderLineID,Sum(pmoExtendedCostBase) As DetailAmount From PurchaseOrderComponents Group By pmoPurchaseOrderID,pmoPurchaseOrderLineID) As DetailTable On pmlPurchaseOrderID = pmoPurchaseOrderID And pmlPurchaseOrderLineID = pmoPurchaseOrderLineID");
		}
	}
}
