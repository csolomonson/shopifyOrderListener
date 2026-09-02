using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.598", "Add field to InventoryCounts table", "2017-12-13")]
public class v92598a
{
	public v92598a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InventoryCounts", "imnPartBinIDs"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InventoryCounts", "imnPartBinIDs", "nvarchar(max)", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
