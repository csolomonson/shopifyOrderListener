using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPTaxCodeLineInformationDto
{
	public string xabCreatedBy { get; set; }

	public DateTime? xabCreatedDate { get; set; }

	public DateTime? xabEffectiveDate { get; set; }

	public Guid xabUniqueID { get; set; }

	public byte[] xabRowVersion { get; set; }

	public int xabTaxCodeLineID { get; set; }

	public string xabTaxCodeID { get; set; }

	public decimal xabTaxRate { get; set; }

	public string xabTaxRateNotesRTF { get; set; }

	public string xabTaxRateNotesText { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
