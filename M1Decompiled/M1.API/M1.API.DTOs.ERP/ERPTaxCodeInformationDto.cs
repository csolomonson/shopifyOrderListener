using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPTaxCodeInformationDto
{
	public string xaxAccrualGlAccountID { get; set; }

	public string xaxTaxCodeID { get; set; }

	public string xaxCreatedBy { get; set; }

	public DateTime? xaxCreatedDate { get; set; }

	public string xaxDescription { get; set; }

	public Guid xaxUniqueID { get; set; }

	public DateTime? xaxInactiveDate { get; set; }

	public bool xaxInactive { get; set; }

	public bool xaxIncludePrimaryTax { get; set; }

	public byte[] xaxRowVersion { get; set; }

	public string xaxTaxOption { get; set; }

	public byte xaxTaxType { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
