using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPShipmentFreightReferenceInformationDto
{
	public string smrCreatedBy { get; set; }

	public DateTime? smrCreatedDate { get; set; }

	public Guid smrUniqueID { get; set; }

	public string smrFreightShipmentID { get; set; }

	public byte[] smrRowVersion { get; set; }

	public short smrShipmentFreightReferenceID { get; set; }

	public string smrShipmentID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
