using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.014", "Add fields to ReceiptComponents table", "2014-12-15")]
public class v900014b
{
	public v900014b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoInspectionComplete"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoInspectionComplete", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoQuantityToInspect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoQuantityToInspect", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoInspParentQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoInspParentQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoQuantityToInspect"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ReceiptComponents Set rmoQuantityToInspect = rmoInspParentQuantity*rmoAdditionalQuantity");
		}
	}
}
