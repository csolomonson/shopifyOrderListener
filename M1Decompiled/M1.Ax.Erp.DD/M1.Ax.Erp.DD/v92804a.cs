using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.804", "Truncate JobSchedules table", "2018-12-18")]
public class v92804a
{
	public v92804a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "JobSchedules"))
		{
			parms.Database.ExecuteCommand("TRUNCATE TABLE JobSchedules");
		}
	}
}
