using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.145", "Update Quotes table", "2008-05-16")]
public class v700145b
{
	public v700145b(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE Quotes SET qmpPaymentTermID = cmlCustomerPaymentTermID FROM Quotes INNER JOIN OrganizationLocations on cmlOrganizationID = qmpCustomerOrganizationID And cmlLocationID = qmpARInvoiceLocationID where qmpPaymentTermID = '' ");
	}
}
