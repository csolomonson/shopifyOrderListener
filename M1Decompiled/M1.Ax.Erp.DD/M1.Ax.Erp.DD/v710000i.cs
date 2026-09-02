using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add fields to Jobs", "2008-05-12")]
public class v710000i
{
	public v710000i(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Jobs", "jmpShipOrganizationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Jobs", "jmpShipOrganizationID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Jobs", "jmpShipLocationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Jobs", "jmpShipLocationID", "char", 5, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
