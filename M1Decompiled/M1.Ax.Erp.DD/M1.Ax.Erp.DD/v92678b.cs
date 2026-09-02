using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.678", "Add fields to SalesOrderLines table", "2018-04-10")]
public class v92678b
{
	public v92678b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderLines", "omlReleaseNumber"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderLines", "omlReleaseNumber", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
