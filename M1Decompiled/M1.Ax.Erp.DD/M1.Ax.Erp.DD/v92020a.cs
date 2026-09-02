using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.020", "Add fields to PartRevisions table", "2016-11-16")]
public class v92020a
{
	public v92020a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrWebConfigMode"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrWebConfigMode", "bit", 1, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrRequiresInspection"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrRequiresInspection", "tinyint", 1, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrSuppressShortDescription"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrSuppressShortDescription", "bit", 1, 0, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrWebConfigPriceRule"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrWebConfigPriceRule", "bit", 1, 0, isNullable: false, parms.Messages);
		}
	}
}
