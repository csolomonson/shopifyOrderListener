using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.065", "Add fields to WarehouseBins table", "2017-01-06")]
public class v92065a
{
	public v92065a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseBins", "inbDefaultBin"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseBins", "inbDefaultBin", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
