using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.018", "Add fields to MfgReceipts table", "2016-11-15")]
public class v92018a
{
	public v92018a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmReverseMfgReceiptID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmReverseMfgReceiptID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmReversalEntry"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmReversalEntry", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
