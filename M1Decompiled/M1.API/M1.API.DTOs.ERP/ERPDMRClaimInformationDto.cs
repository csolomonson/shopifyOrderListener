using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPDMRClaimInformationDto
{
	public string dmpApInvoiceContactID { get; set; }

	public string dmpApInvoiceLocationID { get; set; }

	public DateTime? dmpAuthorizationDate { get; set; }

	public string dmpAuthorizationNumber { get; set; }

	public string dmpAuthorizedByEmployeeID { get; set; }

	public DateTime? dmpClaimDate { get; set; }

	public decimal dmpClaimTotal { get; set; }

	public decimal dmpClaimTotalForeign { get; set; }

	public DateTime? dmpClosedDate { get; set; }

	public string dmpClosedReasonID { get; set; }

	public string dmpDmrClaimID { get; set; }

	public string dmpCreatedBy { get; set; }

	public DateTime? dmpCreatedDate { get; set; }

	public string dmpCurrencyRateID { get; set; }

	public Guid dmpUniqueID { get; set; }

	public decimal dmpExchangeRate { get; set; }

	public bool dmpCustomRate { get; set; }

	public string dmpPlantDepartmentID { get; set; }

	public string dmpPlantID { get; set; }

	public string dmpProcessedByEmployeeID { get; set; }

	public string dmpProjectID { get; set; }

	public string dmpPurchaseContactID { get; set; }

	public string dmpPurchaseLocationID { get; set; }

	public string dmpReference { get; set; }

	public DateTime? dmpRequestedDate { get; set; }

	public byte[] dmpRowVersion { get; set; }

	public string dmpStatus { get; set; }

	public string dmpSupplierOrganizationID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
