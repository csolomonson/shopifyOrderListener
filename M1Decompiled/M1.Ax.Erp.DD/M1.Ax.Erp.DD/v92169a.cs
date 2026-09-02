using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.169", "Add fields to ReasonPlants table", "2017-02-23")]
public class v92169a
{
	public v92169a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReasonPlants", "xajScrapGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReasonPlants", "xajScrapGLAccountID", "nvarchar", 11, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
