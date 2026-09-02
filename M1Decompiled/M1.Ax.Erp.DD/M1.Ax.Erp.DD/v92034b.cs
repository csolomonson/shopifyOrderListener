using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.034", "Add fields to MfgReceiptComponents table", "2016-11-29")]
public class v92034b
{
	public v92034b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceiptComponents", "rmnReverseMfgReceiptID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceiptComponents", "rmnReverseMfgReceiptID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceiptComponents", "rmnReverseMfgReceiptCompID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceiptComponents", "rmnReverseMfgReceiptCompID", "int", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceiptComponents", "rmnReversed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceiptComponents", "rmnReversed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
