using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.049", "Remove fields from FinancialProperties and ImplementationCheckList", "2013-10-30")]
public class v810049a
{
	public v810049a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafIMCostingMethod"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafIMCostingMethod", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafPMCostingMethod"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafPMCostingMethod", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ImplementationCheckList", "xicPercentage"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ImplementationCheckList", "xicPercentage", dropTriggers: true);
		}
	}
}
