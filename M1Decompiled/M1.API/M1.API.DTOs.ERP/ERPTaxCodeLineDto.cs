using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPTaxCodeLineDto
{
	[JsonProperty("xabCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string xabCreatedBy { get; set; }

	[JsonProperty("xabCreatedDate", Order = 2)]
	public DateTime? xabCreatedDate { get; set; }

	[JsonProperty("xabEffectiveDate", Order = 3)]
	[Required(ErrorMessage = "xabEffectiveDate is required.")]
	public DateTime? xabEffectiveDate { get; set; }

	[JsonProperty("xabUniqueID", Order = 4)]
	public Guid xabUniqueID { get; set; }

	[JsonProperty("xabRowVersion", Order = 5)]
	public byte[] xabRowVersion { get; set; }

	[JsonProperty("xabTaxCodeLineID", Order = 6)]
	[Required(ErrorMessage = "xabTaxCodeLineID is required.")]
	[Range(0, 9999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xabTaxCodeLineID { get; set; }

	[JsonProperty("xabTaxCodeID", Order = 7)]
	[Required(ErrorMessage = "xabTaxCodeID is required.")]
	[MaxLength(5)]
	public string xabTaxCodeID { get; set; }

	[JsonProperty("xabTaxRate", Order = 8)]
	[Range(0.0, 999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xabTaxRate { get; set; }

	[JsonProperty("xabTaxRateNotesRTF", Order = 9)]
	[MaxLength(50)]
	public string xabTaxRateNotesRTF { get; set; }

	[JsonProperty("xabTaxRateNotesText", Order = 10)]
	[MaxLength(50)]
	public string xabTaxRateNotesText { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
