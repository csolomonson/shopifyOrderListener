using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.030", "Add fields to InventoryCounts table", "2015-04-15")]
public class v900030a
{
	public v900030a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InventoryCounts", "imnNumberofRecordsGenerated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InventoryCounts", "imnNumberofRecordsGenerated", "int", 8, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update InventoryCounts Set imnNumberofRecordsGenerated = IsNull((Select IsNull(Count(*),0) From InventoryCountLines Where imqInventoryCountID = imnInventoryCountID),0)");
		}
	}
}
