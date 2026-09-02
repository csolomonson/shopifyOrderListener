using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add COGS fields to Dataset Properties", "2008-05-15")]
public class v710000o
{
	public v710000o(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadGLCreateStockJournals"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadGLCreateStockJournals", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadCOGSUseAccounts"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadCOGSUseAccounts", "numeric", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadIMCostingMethod"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadIMCostingMethod", "numeric", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadPMCostingMethod"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadPMCostingMethod", "numeric", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadGLCreateStockJournals = xafARCreateStockJournals, xadCOGSUseAccounts = 1, xadIMCostingMethod = (Case When xapIMCostingMethod = 2 Then 3 Else xapIMCostingMethod End), xadPMCostingMethod = (Case When xapPMCostingMethod = 1 Then 3 Else xapPMCostingMethod End) From DatasetProperties, FinancialProperties, ProductionProperties Where xafARCreateStockJournals = -1 ");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ProductionProperties Set xapIMCostingMethod = xadIMCostingMethod, xapPMCostingMethod = xadPMCostingMethod From DatasetProperties, ProductionProperties Where xadGLCreateStockJournals = -1 ");
		}
	}
}
