using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPFreightPackageLinkInformationDto
{
	public string fplCreatedBy { get; set; }

	public DateTime? fplCreatedDate { get; set; }

	public Guid fplUniqueID { get; set; }

	public short fplFreightPackageID { get; set; }

	public short fplFreightPackageLineID { get; set; }

	public string fplFreightShipmentID { get; set; }

	public byte[] fplRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
