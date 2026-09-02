using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.066", "Remove field from InventoryCounts", "2017-01-06")]
public class v92066a
{
	public v92066a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "InventoryCounts", "imnIncludeBlankWarehouse"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InventoryCounts", "imnIncludeBlankWarehouse", dropTriggers: true);
		}
	}
}
