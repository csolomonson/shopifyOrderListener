using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartUnitSalePriceInformationDto
{
	public string imhCreatedBy { get; set; }

	public DateTime? imhCreatedDate { get; set; }

	public string imhCurrencyRateID { get; set; }

	public DateTime? imhEndDate { get; set; }

	public Guid imhUniqueID { get; set; }

	public string imhPartID { get; set; }

	public string imhPartRevisionID { get; set; }

	public byte[] imhRowVersion { get; set; }

	public short imhPartUnitSalePriceID { get; set; }

	public DateTime? imhStartDate { get; set; }

	public decimal imhUnitSalePrice { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
