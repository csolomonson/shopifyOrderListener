using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.010", "Add field to PartRevisions", "2008-02-25")]
public class v700010b
{
	public v700010b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrPurchaseableItem"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrPurchaseableItem", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PartRevisions Set imrPurchaseableitem <> 0");
		}
	}
}
