using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.032", "Add reverse field to WarehouseReceipts table", "2016-11-25")]
public class v92032b
{
	public v92032b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseReceipts", "wrpReversed"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseReceipts", "wrpReversed", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
