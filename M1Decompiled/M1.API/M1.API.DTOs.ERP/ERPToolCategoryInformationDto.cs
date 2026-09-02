using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPToolCategoryInformationDto
{
	public string xtcToolCategoryID { get; set; }

	public string xtcCreatedBy { get; set; }

	public DateTime? xtcCreatedDate { get; set; }

	public string xtcDescription { get; set; }

	public Guid xtcUniqueID { get; set; }

	public DateTime? xtcInactiveDate { get; set; }

	public bool xtcInactive { get; set; }

	public byte[] xtcRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
