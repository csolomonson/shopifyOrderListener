using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.008", "Add fields to MfgReceipts table", "2014-10-10")]
public class v900007a
{
	public v900007a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmJobOprQuantityReceived"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmJobOprQuantityReceived", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmQuantityToInspect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmQuantityToInspect", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmJobMatQuantityReceived"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmJobMatQuantityReceived", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmExtendedCostBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmExtendedCostBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmClosed"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmClosed", dropTriggers: true);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmPostedDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmPostedDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmExtendedCostBase"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update MfgReceipts Set rmmExtendedCostBase = rmmJobMatQuantityReceived+rmmInventoryQuantityReceived*rmmUnitMaterialCost+rmmUnitSubcontractCost+rmmSetupCharge");
		}
	}
}
