using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPReasonPlantInformationDto
{
	public string xajReasonPlantID { get; set; }

	public string xajCreatedBy { get; set; }

	public DateTime? xajCreatedDate { get; set; }

	public Guid xajUniqueID { get; set; }

	public string xajReasonGlAccountID { get; set; }

	public string xajReasonID { get; set; }

	public byte[] xajRowVersion { get; set; }

	public string xajScrapGlAccountID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
