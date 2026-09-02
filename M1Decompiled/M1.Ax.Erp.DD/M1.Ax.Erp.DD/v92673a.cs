using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.673", "Add fields to SalesOrderLines table", "2018-04-02")]
public class v92673a
{
	public v92673a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderLines", "omlEasyOrderExternalStatus"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderLines", "omlEasyOrderExternalStatus", "nvarchar", 3, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
