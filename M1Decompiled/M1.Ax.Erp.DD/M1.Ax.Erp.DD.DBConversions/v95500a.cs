using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.500", "Add xadEnableM1Home field to DatasetProperties table", "2022-10-16")]
public class v95500a
{
	public v95500a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadEnableM1Home"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadEnableM1Home", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadEnableM1Home = '0';");
		}
	}
}
