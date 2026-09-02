using M1.API.DTOs.EDI;

namespace M1.API.Models.EDI;

public class GetOrganizationDataParam
{
	public string CustomerOrganizationID { get; set; }

	public string SalesOrderID { get; set; }

	public string CustomerPO { get; set; }

	public M1Location ShipLocationID { get; set; }

	public M1Location ARInvoiceLocationID { get; set; }

	public GetOrganizationDataParam(string customerOrganizationID, string salesOrderID, string customerPO, M1Location shipLocationID, M1Location aRInvoiceLocationID)
	{
		CustomerOrganizationID = customerOrganizationID;
		SalesOrderID = salesOrderID;
		CustomerPO = customerPO;
		ShipLocationID = shipLocationID;
		ARInvoiceLocationID = aRInvoiceLocationID;
	}
}
