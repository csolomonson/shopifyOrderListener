using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.206", "Add Machines to Schedule field to JobOperations", "2011-12-08")]
public class v800206
{
	public v800206(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoMachinesToSchedule"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoMachinesToSchedule", "numeric", 3, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE JobOperations SET jmoMachinesToSchedule = 1");
		}
	}
}
