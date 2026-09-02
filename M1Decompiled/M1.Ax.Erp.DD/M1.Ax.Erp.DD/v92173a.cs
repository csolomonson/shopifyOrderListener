using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.173", "Alter field in PartRevisions", "2017-02-24")]
public class v92173a
{
	public v92173a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrRequiresInspection"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrRequiresInspection", "tinyint", 1, 0, isNullable: false, parms.Messages);
		}
	}
}
