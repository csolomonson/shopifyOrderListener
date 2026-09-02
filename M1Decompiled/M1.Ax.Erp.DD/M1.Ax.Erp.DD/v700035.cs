using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.035", "Add field to PartRevisions", "2008-02-28")]
public class v700035
{
	public v700035(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrWebSellableToAll"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrWebSellableToAll", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
