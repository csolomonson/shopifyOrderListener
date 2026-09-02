using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.058", "Add fields to QuantityAdjustments table", "2015-07-09")]
public class v900058g
{
	public v900058g(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuantityAdjustments", "inqDestinationWarehouseID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuantityAdjustments", "inqDestinationWarehouseID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
