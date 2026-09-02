using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartPriceBreakInformationDto
{
	public string imjCreatedBy { get; set; }

	public DateTime? imjCreatedDate { get; set; }

	public decimal imjDiscount { get; set; }

	public Guid imjUniqueID { get; set; }

	public short imjLeadTime { get; set; }

	public int imjPartPriceID { get; set; }

	public decimal imjProposedNewPrice { get; set; }

	public decimal imjQuantity { get; set; }

	public byte[] imjRowVersion { get; set; }

	public short imjPartPriceBreakID { get; set; }

	public decimal imjUnitPrice { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
