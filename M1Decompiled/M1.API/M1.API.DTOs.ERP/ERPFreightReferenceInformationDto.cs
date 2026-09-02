using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPFreightReferenceInformationDto
{
	public string frcFreightReferenceID { get; set; }

	public Guid frcUniqueID { get; set; }

	public string frcFreightShipmentID { get; set; }

	public string frcQuoteID { get; set; }

	public byte[] frcRowVersion { get; set; }

	public string frcShipmentID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
