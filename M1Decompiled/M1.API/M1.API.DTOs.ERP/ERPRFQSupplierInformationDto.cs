using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPRFQSupplierInformationDto
{
	public string rqsCreatedBy { get; set; }

	public DateTime? rqsCreatedDate { get; set; }

	public string rqsCurrencyRateID { get; set; }

	public DateTime? rqsDueDate { get; set; }

	public Guid rqsUniqueID { get; set; }

	public decimal rqsExchangeRate { get; set; }

	public bool rqsClosed { get; set; }

	public bool rqsComplete { get; set; }

	public bool rqsCustomRate { get; set; }

	public bool rqsSelectedSupplier { get; set; }

	public bool rqsUpdatedPartPrices { get; set; }

	public string rqsOrgPartID { get; set; }

	public string rqsPurchaseContactID { get; set; }

	public string rqsPurchaseLocationID { get; set; }

	public string rqsRfqID { get; set; }

	public short rqsRfqLineID { get; set; }

	public byte[] rqsRowVersion { get; set; }

	public DateTime? rqsSelectedSupplierDate { get; set; }

	public short rqsRfqSupplierID { get; set; }

	public string rqsSupplierOrganizationID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
