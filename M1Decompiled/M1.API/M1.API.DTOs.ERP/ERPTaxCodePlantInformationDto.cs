using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPTaxCodePlantInformationDto
{
	public string xtpAccrualGlAccountID { get; set; }

	public string xtpCreatedBy { get; set; }

	public DateTime? xtpCreatedDate { get; set; }

	public Guid xtpUniqueID { get; set; }

	public string xtpPlantID { get; set; }

	public byte[] xtpRowVersion { get; set; }

	public string xtpTaxCodeID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
