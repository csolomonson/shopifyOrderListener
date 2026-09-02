using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add AP field to Financial Properties", "2008-05-30")]
public class v710000s
{
	public v710000s(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAPIncludeTaxInExpAmt"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAPIncludeTaxInExpAmt", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
