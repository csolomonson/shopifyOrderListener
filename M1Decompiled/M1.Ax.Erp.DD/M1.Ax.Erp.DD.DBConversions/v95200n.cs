using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Update xadEmailAddress to empty if it is null in DatasetProperties table", "2022-01-25")]
public class v95200n
{
	public v95200n(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadEmailAddress"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE DatasetProperties SET xadEmailAddress = '' where xadEmailAddress is null");
		}
	}
}
