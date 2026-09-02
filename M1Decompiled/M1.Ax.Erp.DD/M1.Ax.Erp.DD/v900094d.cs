using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.094", "Add fields to SalesOrders table", "2015-10-16")]
public class v900094d
{
	public v900094d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrders", "ompSplitPercentTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrders", "ompSplitPercentTotal", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
