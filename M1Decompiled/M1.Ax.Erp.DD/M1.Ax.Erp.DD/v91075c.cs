using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.075", "Add fields to PurchasePlannerOrderDetails table", "2016-06-10")]
public class v91075c
{
	public v91075c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerOrderDetails", "ppoExtendedCostBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchasePlannerOrderDetails", "ppoExtendedCostBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchasePlannerOrderDetails", "ppoExtendedCostBase"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PurchasePlannerOrderDetails Set ppoExtendedCostBase = ppoUnitCostBase*ppoInventoryQuantity");
		}
	}
}
