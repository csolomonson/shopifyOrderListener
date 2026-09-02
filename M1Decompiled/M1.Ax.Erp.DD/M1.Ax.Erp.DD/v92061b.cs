using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.061", "Add fields to PartRevisions table", "2016-12-23")]
public class v92061b
{
	public v92061b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrQuantityToReturnJob"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrQuantityToReturnJob", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
