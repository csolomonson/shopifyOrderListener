using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.003", "Add material, operation and inspect quantity fields to ReceiptLines table", "2014-09-25")]
public class v900003a
{
	public v900003a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlJobOprQuantityReceived"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlJobOprQuantityReceived", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlJobMatQuantityReceived"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlJobMatQuantityReceived", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlJobQuantityReceived"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ReceiptLines Set rmlJobMatQuantityReceived = Case When rmlJobType = 1 Then rmlJobQuantityReceived Else 0 End, rmlJobOprQuantityReceived = Case When rmlJobType = 2 Then rmlJobQuantityReceived Else 0 End Where rmlJobID <> ''");
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlJobQuantityReceived", dropTriggers: true);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlQuantityToInspect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlQuantityToInspect", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ReceiptLines SET rmlQuantityToInspect = Case When rmlJobID = '' Then rmlInventoryQuantityReceived Else Case rmlJobType When 1 Then rmlJobMatQuantityReceived When 2 Then rmlJobOprQuantityReceived Else 0 End End, rmlInventoryQuantityReceived = 0, rmlJobMatQuantityReceived = 0, rmlJobOprQuantityReceived = 0 WHERE rmlRequiresInspection = 1");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlPOOpenQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlPOOpenQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlExtendedCostForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlExtendedCostForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlExtendedCostBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlExtendedCostBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlPOPurchaseQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlPOPurchaseQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlJobOpenQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlJobOpenQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlJobEstimatedQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlJobEstimatedQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlExtendedCostBase"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ReceiptLines Set rmlExtendedCostBase = (rmlInventoryQuantityReceived+rmlJobMatQuantityReceived+rmlJobOprQuantityReceived+rmlQuantityToInspect)*rmlInventoryUnitCost+rmlSetupCharge");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlExtendedCostForeign"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ReceiptLines Set rmlExtendedCostForeign = (rmlInventoryQuantityReceived+rmlJobMatQuantityReceived+rmlJobOprQuantityReceived+rmlQuantityToInspect)*rmlInventoryUnitCostForeign+rmlSetupChargeForeign");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlCustomRate"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlCustomRate", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlExchangeRate"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlExchangeRate", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlCurrencyRateID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlCurrencyRateID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlReceiptDate"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlReceiptDate", dropTriggers: true);
		}
	}
}
