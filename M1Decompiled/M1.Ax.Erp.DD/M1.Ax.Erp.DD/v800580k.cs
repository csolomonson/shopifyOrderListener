using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.580", "Add fields to OrganizationLocations table", "2015-06-23")]
public class v800580k
{
	public v800580k(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlUPSWSBillingOption"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlUPSWSBillingOption", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
