using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPIndustryTypeInformationDto
{
	public string cmiIndustryTypeID { get; set; }

	public string cmiCreatedBy { get; set; }

	public DateTime? cmiCreatedDate { get; set; }

	public Guid cmiUniqueID { get; set; }

	public string cmiLongDescriptionRtf { get; set; }

	public string cmiLongDescriptionText { get; set; }

	public byte[] cmiRowVersion { get; set; }

	public string cmiShortDescription { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
