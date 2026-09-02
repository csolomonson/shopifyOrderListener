using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.500", "Update Include Tax in expense amount field", "2009-02-12")]
public class v710500
{
	public v710500(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FinancialProperties Set xafAPIncludeTaxInExpAmt = 0 Where IsNull((Select xadGLCreateStockJournals From DatasetProperties), 0) <> 0");
	}
}
