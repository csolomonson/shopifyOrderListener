using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.033", "Update Customer/Supplier Status for blank locations", "2016-03-31")]
public class v91033a
{
	public v91033a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "OrganizationLocations"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update OrganizationLocations Set cmlQuoteLocation = 1, cmlShipLocation = 1, cmlARInvoiceLocation = 1 From OrganizationLocations Inner Join Organizations on cmlOrganizationID = cmoOrganizationID Where cmlLocationID = '' And cmoCustomerStatus In (1,2)");
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update OrganizationLocations Set cmlPurchaseLocation = 1, cmlAPInvoiceLocation = 1 From OrganizationLocations Inner Join Organizations on cmlOrganizationID = cmoOrganizationID Where cmlLocationID = '' And cmoSupplierStatus In (1,2)");
		}
	}
}
