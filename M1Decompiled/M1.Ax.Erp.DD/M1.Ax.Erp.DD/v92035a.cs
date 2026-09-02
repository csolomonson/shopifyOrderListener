using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.035", "Add fields to WarehouseTransfers table", "2016-11-30")]
public class v92035a
{
	public v92035a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransfers", "mwpReversed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransfers", "mwpReversed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
