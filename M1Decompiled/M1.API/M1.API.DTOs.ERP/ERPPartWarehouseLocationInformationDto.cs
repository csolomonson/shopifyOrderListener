using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartWarehouseLocationInformationDto
{
	public string imlCreatedBy { get; set; }

	public DateTime? imlCreatedDate { get; set; }

	public Guid imlUniqueID { get; set; }

	public bool imlNonNettable { get; set; }

	public DateTime? imLLastRunDatePurchasePlanner { get; set; }

	public decimal imlMaximumQuantity { get; set; }

	public decimal imlMinimumQuantity { get; set; }

	public string imlPartID { get; set; }

	public string imlPartRevisionID { get; set; }

	public string imlPartWarehouseID { get; set; }

	public decimal imlQuantityInTransit { get; set; }

	public byte[] imlRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
