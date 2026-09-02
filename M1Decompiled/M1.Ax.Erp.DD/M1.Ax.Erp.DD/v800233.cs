using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.233", "Add machines to schedule fields", "2012-03-08")]
public class v800233
{
	public v800233(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartOperations", "imoMachinesToSchedule"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartOperations", "imoMachinesToSchedule", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE PartOperations SET imoMachinesToSchedule = 1");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteOperations", "qmoMachinesToSchedule"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteOperations", "qmoMachinesToSchedule", "numeric", 3, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE QuoteOperations SET qmoMachinesToSchedule = 1");
		}
	}
}
