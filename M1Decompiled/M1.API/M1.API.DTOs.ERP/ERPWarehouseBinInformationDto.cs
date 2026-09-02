using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWarehouseBinInformationDto
{
	public string inbWarehouseBinID { get; set; }

	public string inbCreatedBy { get; set; }

	public DateTime? inbCreatedDate { get; set; }

	public string inbDescription { get; set; }

	public Guid inbUniqueID { get; set; }

	public DateTime? inbInactiveDate { get; set; }

	public bool inbInactive { get; set; }

	public bool inbDefaultBin { get; set; }

	public bool inbHasQOHQTI { get; set; }

	public string inbLongDescriptionRtf { get; set; }

	public string inbLongDescriptionText { get; set; }

	public byte[] inbRowVersion { get; set; }

	public string inbWarehouseID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
