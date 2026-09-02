using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.073", "Add MailProvider to DatasetProperties table", "2014-06-10")]
public class v810073b
{
	public v810073b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadMailProvider"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadMailProvider", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadMailProvider = 'MAPI'");
		}
	}
}
