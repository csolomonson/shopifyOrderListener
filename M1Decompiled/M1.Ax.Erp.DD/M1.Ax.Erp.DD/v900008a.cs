using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.008", "Add fields to ReceiptComponents table", "2014-10-23")]
public class v900008a
{
	public v900008a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoParentQuantity") && !parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoInvParentQuantity"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoParentQuantity", "rmoInvParentQuantity", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoQuantityReceived") && !parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoInvQuantityReceived"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoQuantityReceived", "rmoInvQuantityReceived", dropTriggers: true);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoJobAssemblyID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoJobAssemblyID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoJobParentQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoJobParentQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoJobID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoJobID", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoJobMaterialComponentID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoJobMaterialComponentID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoJobMaterialID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoJobMaterialID", "int", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoJobQuantityReceived"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoJobQuantityReceived", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoInvQuantityReceived"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ReceiptComponents Set rmoInvQuantityReceived = rmoInvParentQuantity*rmoAdditionalQuantity");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoJobQuantityReceived"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ReceiptComponents Set rmoJobQuantityReceived = rmoJobParentQuantity*rmoAdditionalQuantity");
		}
	}
}
