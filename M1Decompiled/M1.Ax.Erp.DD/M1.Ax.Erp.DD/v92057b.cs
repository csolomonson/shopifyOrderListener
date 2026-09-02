using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.057", "Rename field in RMAReceiptComponents table", "2016-12-19")]
public class v92057b
{
	public v92057b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAReceiptComponents", "rroReverseRMAReceiptIDLineID") && !parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAReceiptComponents", "rroReverseRMAReceiptLineID"))
		{
			parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAReceiptComponents", "rroReverseRMAReceiptIDLineID", "rroReverseRMAReceiptLineID", dropTriggers: true);
		}
	}
}
