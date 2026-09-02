using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.012", "Add fields to RMAReceiptComponents table", "2016-11-06")]
public class v92012f
{
	public v92012f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAReceiptComponents", "rroPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAReceiptComponents", "rroPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
