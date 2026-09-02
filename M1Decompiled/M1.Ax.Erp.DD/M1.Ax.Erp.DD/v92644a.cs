using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.644", "Add fields to SalesOrders table (EasyOrder)", "2018-02-23")]
public class v92644a
{
	public v92644a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrders", "ompEasyOrderExternalStatus"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrders", "ompEasyOrderExternalStatus", "nvarchar", 3, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrders", "ompEasyOrderPaid"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrders", "ompEasyOrderPaid", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
