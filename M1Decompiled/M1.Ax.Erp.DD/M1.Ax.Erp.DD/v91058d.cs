using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.058", "Add fields to PartRevisions table", "2016-05-18")]
public class v91058d
{
	public v91058d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrLastRunDatePurchasePlanner"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrLastRunDatePurchasePlanner", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
