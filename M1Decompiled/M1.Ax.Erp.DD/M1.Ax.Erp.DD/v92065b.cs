using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.065", "Add fields to Warehouses table", "2017-01-06")]
public class v92065b
{
	public v92065b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Warehouses", "imwDefaultBinCount"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Warehouses", "imwDefaultBinCount", "int", 4, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
