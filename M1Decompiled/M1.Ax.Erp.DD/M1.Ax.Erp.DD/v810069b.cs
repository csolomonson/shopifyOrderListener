using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.071", "Drop fields from SalesOrderPickListSessions table", "2014-04-24")]
public class v810069b
{
	public v810069b(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderPickListSessions", "omsClosed"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderPickListSessions", "omsClosed", dropTriggers: true);
		}
	}
}
