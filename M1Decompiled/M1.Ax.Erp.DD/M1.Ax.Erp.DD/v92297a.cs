using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.297", "Add field to productionproperties", "2017-06-14")]
public class v92297a
{
	public v92297a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapShowQtyOnHandMobInv"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapShowQtyOnHandMobInv", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
