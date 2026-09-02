using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.056", "Add cogs option to ProductionProperties", "2013-10-30")]
public class v810056
{
	public v810056(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapGLCreateStockJournals"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapGLCreateStockJournals", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafGLCreateStockJournals"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE ProductionProperties SET xapGLCreateStockJournals = (Select IsNull(xafGLCreateStockJournals,0) From FinancialProperties)");
		}
	}
}
