using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Add Quantity To Inspect to MRPLines table", "2021-11-30")]
public class v95200c
{
	public v95200c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPLines", "mrlQuantityToInspect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPLines", "mrlQuantityToInspect", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
