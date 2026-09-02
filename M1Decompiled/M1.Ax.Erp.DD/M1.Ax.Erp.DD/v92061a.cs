using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.061", "Add fields to PartBins table", "2016-12-23")]
public class v92061a
{
	public v92061a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartBins", "imbQuantityToReturnJob"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartBins", "imbQuantityToReturnJob", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
