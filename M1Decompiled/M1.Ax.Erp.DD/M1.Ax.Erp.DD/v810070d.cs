using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.071", "Add fields to DatasetProperties table", "2014-05-02")]
public class v810070d
{
	public v810070d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadEnableMultiCurrency"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadEnableMultiCurrency", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadEnableMultiCurrency = 1");
		}
	}
}
