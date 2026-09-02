using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPShipmentFreightLinkInformationDto
{
	public string smxCreatedBy { get; set; }

	public DateTime? smxCreatedDate { get; set; }

	public Guid smxUniqueID { get; set; }

	public decimal smxFreightCharges { get; set; }

	public short smxFreightPackageID { get; set; }

	public string smxFreightShipmentID { get; set; }

	public bool smxClosed { get; set; }

	public decimal smxLinkPctCharge { get; set; }

	public decimal smxPackagePartialCount { get; set; }

	public decimal smxPackagePartialWeight { get; set; }

	public byte[] smxRowVersion { get; set; }

	public short smxShipmentFreightLinkID { get; set; }

	public string smxShipmentID { get; set; }

	public short smxShipmentLineID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
