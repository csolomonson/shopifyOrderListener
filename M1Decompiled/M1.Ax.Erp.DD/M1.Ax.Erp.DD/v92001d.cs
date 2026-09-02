using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.001", "Add costing method field to ProductionProperties table", "2016-10-23")]
public class v92001d
{
	public v92001d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapCostingMethodHistory"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapCostingMethodHistory", "nvarchar(max)", 50, 0, verifyIndexes: false, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
