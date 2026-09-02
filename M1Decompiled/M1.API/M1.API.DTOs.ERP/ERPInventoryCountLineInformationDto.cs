using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPInventoryCountLineInformationDto
{
	public string imqBinDescription { get; set; }

	public string imqCountedBy { get; set; }

	public DateTime? imqCountedDate { get; set; }

	public string imqCreatedBy { get; set; }

	public DateTime? imqCreatedDate { get; set; }

	public Guid imqUniqueID { get; set; }

	public decimal imqFinalCount { get; set; }

	public int imqInventoryCountID { get; set; }

	public string imqPartBinID { get; set; }

	public string imqPartClassID { get; set; }

	public string imqPartID { get; set; }

	public string imqPartRevisionID { get; set; }

	public string imqPartShortDescription { get; set; }

	public string imqPartWarehouseLocationID { get; set; }

	public decimal imqQuantityOnHand { get; set; }

	public byte[] imqRowVersion { get; set; }

	public int imqInventoryCountLineID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
