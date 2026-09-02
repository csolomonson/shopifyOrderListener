using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.678", "Add fields to OrganizationLocations table", "2018-04-10")]
public class v92678a
{
	public v92678a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlEDILocationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlEDILocationID", "nvarchar", 30, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
