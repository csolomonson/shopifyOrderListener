using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.037", "Add fields to Organizations table", "2015-05-21")]
public class v900037a
{
	public v900037a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONS", "cmoSuperClearingHouse"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONS", "cmoSuperClearingHouse", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ORGANIZATIONS", "cmoSuperFundID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ORGANIZATIONS", "cmoSuperFundID", dropTriggers: true);
		}
	}
}
