using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.071", "Add fields to SalesOrderPickListLines table", "2014-04-24")]
public class v810069a
{
	public v810069a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderPickListLines", "omyStatus"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderPickListLines", "omyStatus", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
