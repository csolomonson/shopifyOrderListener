using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.094", "Add fields to OrganizationLocations table", "2015-10-16")]
public class v900094c
{
	public v900094c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlSplitPercentTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlSplitPercentTotal", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
