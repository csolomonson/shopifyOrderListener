using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.7.100", "Add field xafSTPSetGrossPayAsETP to FinancialProperties table", "2024-02-14")]
public class v97100e
{
	public v97100e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafSTPSetGrossPayAsETP"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafSTPSetGrossPayAsETP", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
