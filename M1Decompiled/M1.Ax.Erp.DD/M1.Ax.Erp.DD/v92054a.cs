using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.054", "Add fields to MfgReceipts table", "2016-12-19")]
public class v92054a
{
	public v92054a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MfgReceipts", "rmmTotalComponentCosts"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceipts", "rmmTotalComponentCosts", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
