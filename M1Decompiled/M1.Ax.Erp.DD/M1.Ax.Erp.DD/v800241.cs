using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.241", "Add Maximum No. of rows to load in grids in DatasetProperties table", "2012-05-24")]
public class v800241
{
	public v800241(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadMaxGridRow"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadMaxGridRow", "numeric", 7, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update DatasetProperties Set xadMaxGridRow = 100000");
		}
	}
}
