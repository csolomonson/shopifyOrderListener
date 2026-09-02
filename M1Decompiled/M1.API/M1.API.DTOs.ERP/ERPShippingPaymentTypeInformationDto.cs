using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPShippingPaymentTypeInformationDto
{
	public string xayShippingPaymentTypeID { get; set; }

	public string xayCreatedBy { get; set; }

	public DateTime? xayCreatedDate { get; set; }

	public string xayDescription { get; set; }

	public Guid xayUniqueID { get; set; }

	public DateTime? xayInactiveDate { get; set; }

	public bool xayInactive { get; set; }

	public bool xayDoNotXferShipCostsToAr { get; set; }

	public byte[] xayRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
