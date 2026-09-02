using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.100", "Add xadEnableOutlookOnline field to DatasetProperties table", "2021-06-28")]
public class v94100d
{
	public v94100d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadEnableOutlookOnline"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadEnableOutlookOnline", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadEnableOutlookOnline = '0';");
		}
	}
}
