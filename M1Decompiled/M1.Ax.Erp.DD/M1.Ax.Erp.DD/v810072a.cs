using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.072", "Add fields to Properties tables", "2014-05-27")]
public class v810072a
{
	public v810072a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadMailServer"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadMailServer", "nvarchar(max)", 50, 0, isNullable: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadMailServer = Null Where xadMailServer = ''");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapHDCreateCallForEmails"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapHDCreateCallForEmails", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
