using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.035", "Update field bindings", "2016-12-01")]
public class v92035b
{
	public v92035b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptComponents", "rmoConversionFactor"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptComponents", "rmoConversionFactor", "numeric", 14, 8, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ReceiptComponents Set rmoConversionFactor = rmlConversionFactor From ReceiptLines Inner Join ReceiptComponents On RMLRECEIPTID = RMORECEIPTID And RMLRECEIPTLINEID = RMORECEIPTLINEID");
		}
	}
}
