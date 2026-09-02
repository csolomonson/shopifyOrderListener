using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.065", "Add Field to Organizations/OrganizationLocations", "2008-07-18")]
public class v710065
{
	public v710065(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoFreeOnBoardDescription"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoFreeOnBoardDescription", "char", 15, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlFreeOnBoardDescription"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlFreeOnBoardDescription", "char", 15, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
