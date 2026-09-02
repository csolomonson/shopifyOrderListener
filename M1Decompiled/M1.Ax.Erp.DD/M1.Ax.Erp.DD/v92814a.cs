using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.814", "Update Receipt Lines Fields", "2019-02-19")]
public class v92814a
{
	public v92814a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlExtendedCostBase"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ReceiptLines Set rmlExtendedCostBase = (rmlInventoryQuantityReceived+rmlJobMatQuantityReceived+rmlJobOprQuantityReceived+rmlQuantityToInspect)*rmlInventoryUnitCost+rmlSetupCharge");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlExtendedCostForeign"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ReceiptLines Set rmlExtendedCostForeign = (rmlInventoryQuantityReceived+rmlJobMatQuantityReceived+rmlJobOprQuantityReceived+rmlQuantityToInspect)*rmlInventoryUnitCostForeign+rmlSetupChargeForeign");
		}
	}
}
