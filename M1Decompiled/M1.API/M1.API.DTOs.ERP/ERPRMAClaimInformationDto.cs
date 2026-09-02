using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPRMAClaimInformationDto
{
	public decimal rapActualHoursTotal { get; set; }

	public string rapArInvoiceContactID { get; set; }

	public string rapArInvoiceLocationID { get; set; }

	public DateTime? rapAuthorizationDate { get; set; }

	public string rapAuthorizationNumber { get; set; }

	public string rapAuthorizedByEmployeeID { get; set; }

	public DateTime? rapClaimDate { get; set; }

	public decimal rapClaimTotal { get; set; }

	public decimal rapClaimTotalForeign { get; set; }

	public DateTime? rapClosedDate { get; set; }

	public string rapClosedReasonID { get; set; }

	public string rapRmaClaimID { get; set; }

	public string rapCreatedBy { get; set; }

	public DateTime? rapCreatedDate { get; set; }

	public string rapCurrencyRateID { get; set; }

	public string rapCustomerOrganizationID { get; set; }

	public decimal rapDiscountAmount { get; set; }

	public decimal rapDiscountAmountForeign { get; set; }

	public Guid rapUniqueID { get; set; }

	public decimal rapExchangeRate { get; set; }

	public decimal rapFreightAmount { get; set; }

	public decimal rapFreightAmountForeign { get; set; }

	public bool rapCustomRate { get; set; }

	public decimal rapLaborRate { get; set; }

	public decimal rapLaborRateForeign { get; set; }

	public decimal rapLaborTotal { get; set; }

	public decimal rapLaborTotalForeign { get; set; }

	public string rapLongDescriptionRtf { get; set; }

	public string rapLongDescriptionText { get; set; }

	public string rapPartID { get; set; }

	public string rapPartRevisionID { get; set; }

	public string rapPartShortDescription { get; set; }

	public decimal rapPartsTotal { get; set; }

	public decimal rapPartsTotalForeign { get; set; }

	public byte rapPayTo { get; set; }

	public string rapPlantDepartmentID { get; set; }

	public string rapPlantID { get; set; }

	public string rapProcessedByEmployeeID { get; set; }

	public string rapProjectID { get; set; }

	public string rapReference { get; set; }

	public DateTime? rapRequestedDate { get; set; }

	public string rapResellerContactID { get; set; }

	public string rapResellerLocationID { get; set; }

	public string rapResellerOrganizationID { get; set; }

	public byte[] rapRowVersion { get; set; }

	public string rapSerialNumberID { get; set; }

	public string rapShipContactID { get; set; }

	public string rapShipLocationID { get; set; }

	public string rapShipOrganizationID { get; set; }

	public string rapStatus { get; set; }

	public decimal rapSubcontractTotal { get; set; }

	public decimal rapSubcontractTotalForeign { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
