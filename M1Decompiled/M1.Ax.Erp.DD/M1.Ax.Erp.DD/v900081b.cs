using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.081", "Add fields to OrganizationLocations table", "2015-09-14")]
public class v900081b
{
	public v900081b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlAddressValidationResult"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlAddressValidationResult", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
