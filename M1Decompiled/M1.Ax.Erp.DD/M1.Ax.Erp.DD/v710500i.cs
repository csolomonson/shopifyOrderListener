using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.500", "Update Require Part to Exist in Inventory field", "2009-05-12")]
public class v710500i
{
	public v710500i(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE DatasetProperties SET xadPartsMustExist = -1 WHERE xadGLCreateStockJournals = -1 ");
	}
}
