using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.016", "Add fields to MfgReceiptComponents table", "2016-11-13")]
public class v92016l
{
	public v92016l(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceiptComponents", "rmnExtendedCost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceiptComponents", "rmnExtendedCost", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceiptComponents", "rmnUnitCost"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceiptComponents", "rmnUnitCost", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
