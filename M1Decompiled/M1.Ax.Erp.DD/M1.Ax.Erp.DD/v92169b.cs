using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.169", "Add fields to Reasons table", "2017-02-23")]
public class v92169b
{
	public v92169b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Reasons", "xarScrapGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Reasons", "xarScrapGLAccountID", "nvarchar", 11, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
