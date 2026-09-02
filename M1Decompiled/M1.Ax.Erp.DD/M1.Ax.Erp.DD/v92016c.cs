using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.016", "Add fields to WarehouseTransferComponents table", "2016-11-12")]
public class v92016c
{
	public v92016c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseTransferComponents", "mwoPosted"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferComponents", "mwoPosted", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
