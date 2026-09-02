using System.Data;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.828", "Add fields to ProductionProperties table", "2020-08-12")]
public class v92828b
{
	public v92828b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapIMMfgDefaultCostType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapIMMfgDefaultCostType", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			int num = ((parms.Database.GetDataTable("Select xapIMCostingMethod from ProductionProperties").Rows[0].Field<byte>("xapIMCostingMethod") != 3) ? 1 : 2);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, $"Update ProductionProperties Set xapIMMfgDefaultCostType = {num}");
		}
	}
}
