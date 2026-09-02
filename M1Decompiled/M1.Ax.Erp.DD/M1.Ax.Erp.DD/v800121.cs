using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.121", "Verify properties tables for varchar fields", "2011-04-04")]
public class v800121
{
	public v800121(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ProductionProperties Set xapJMSplitCosts = 1 WHERE xapJMSplitCosts < 0");
		parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", parms.Messages, null);
		parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", parms.Messages, null);
		parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", parms.Messages, null);
		parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingProperties", parms.Messages, null);
		parms.Dmo.VerifyTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WebGearProperties", parms.Messages, null);
	}
}
