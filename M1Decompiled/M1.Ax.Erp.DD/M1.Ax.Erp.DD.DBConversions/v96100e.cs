using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.6.100", "Add Exclude Inactive Bin in Inventory Counts", "2023-03-31")]
public class v96100e
{
	public v96100e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InventoryCounts", "imnExcludeInactivePartBins"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InventoryCounts", "imnExcludeInactivePartBins", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
