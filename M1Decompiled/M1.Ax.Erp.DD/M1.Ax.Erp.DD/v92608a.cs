using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.608", "Add EasyOrderRecalculateOrders to ProductionProperties table", "2018-01-03")]
public class v92608a
{
	public v92608a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapEasyOrderRecalculateOrders"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapEasyOrderRecalculateOrders", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
