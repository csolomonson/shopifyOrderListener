using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.100", "Add xadEnableM1Email field to DatasetProperties table", "2021-06-28")]
public class v94100b
{
	public v94100b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadEnableM1Email"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadEnableM1Email", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadEnableM1Email = '1';");
		}
	}
}
