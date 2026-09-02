using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.684", "Add fields to ProductionProperties table", "2018-04-09")]
public class v92684a
{
	public v92684a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapIMOnlyAllowExistingBins"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapIMOnlyAllowExistingBins", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
