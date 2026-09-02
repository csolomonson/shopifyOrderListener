using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.035", "Add Field to ProductionProperties", "2008-07-16")]
public class v710035b
{
	public v710035b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapIMTransferDescriptions"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapIMTransferDescriptions", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ProductionProperties Set xapIMTransferDescriptions = 1");
		}
	}
}
