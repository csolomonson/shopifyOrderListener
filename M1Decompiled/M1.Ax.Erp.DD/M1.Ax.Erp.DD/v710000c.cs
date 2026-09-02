using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add minimize gaps production property", "2008-03-27")]
public class v710000c
{
	public v710000c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapJMMinimizeGaps"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapJMMinimizeGaps", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ProductionProperties Set xapJMMinimizeGaps = 1");
		}
	}
}
