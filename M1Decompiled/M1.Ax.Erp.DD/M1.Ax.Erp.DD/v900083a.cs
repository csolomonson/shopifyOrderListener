using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.083", "Add fields to OrganizationLocations table", "2015-09-16")]
public class v900083a
{
	public v900083a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlCustomerShippingCarrier"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlCustomerShippingCarrier", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
