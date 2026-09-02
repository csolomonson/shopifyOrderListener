using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.234", "Drop Tariffs table", "2017-04-28")]
public class v92234a
{
	public v92234a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Tariffs"))
		{
			parms.Dmo.DropTable(null, parms.User, parms.DatabaseName, "Tariffs");
		}
	}
}
