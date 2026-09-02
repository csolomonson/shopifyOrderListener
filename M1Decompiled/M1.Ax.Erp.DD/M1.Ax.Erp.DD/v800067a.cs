using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.067", "Add xafUsePayPalProcessing to Financial Properties", "2010-09-20")]
public class v800067a
{
	public v800067a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafUsePayPalProcessing"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafUsePayPalProcessing", "numeric", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
