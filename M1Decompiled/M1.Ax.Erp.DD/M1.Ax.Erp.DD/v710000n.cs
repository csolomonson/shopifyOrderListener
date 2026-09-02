using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add Payroll field to Financial Properties", "2008-05-16")]
public class v710000n
{
	public v710000n(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafPAUseDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafPAUseDate", "numeric", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update FinancialProperties Set xafPAUseDate = 1 ");
		}
	}
}
