using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.003", "Add fields to ReceiptComponents table", "2014-09-25")]
public class v900003b
{
	public v900003b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoParentQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoParentQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ReceiptComponents Set rmoParentQuantity = rmlInventoryQuantityReceived From ReceiptLines Inner Join ReceiptComponents On RMLRECEIPTID = RMORECEIPTID And RMLRECEIPTLINEID = RMORECEIPTLINEID");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoQuantityReceived"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ReceiptComponents Set rmoQuantityReceived = rmoParentQuantity*rmoAdditionalQuantity");
		}
	}
}
