using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPTaxCodeDto
{
	[JsonProperty("xaxAccrualGlAccountID", Order = 1)]
	[MaxLength(11)]
	public string xaxAccrualGlAccountID { get; set; }

	[JsonProperty("xaxTaxCodeID", Order = 2)]
	[Required(ErrorMessage = "xaxTaxCodeID is required.")]
	[MaxLength(5)]
	public string xaxTaxCodeID { get; set; }

	[JsonProperty("xaxCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string xaxCreatedBy { get; set; }

	[JsonProperty("xaxCreatedDate", Order = 4)]
	public DateTime? xaxCreatedDate { get; set; }

	[JsonProperty("xaxDescription", Order = 5)]
	[Required(ErrorMessage = "xaxDescription is required.")]
	[MaxLength(50)]
	public string xaxDescription { get; set; }

	[JsonProperty("xaxUniqueID", Order = 6)]
	public Guid xaxUniqueID { get; set; }

	[JsonProperty("xaxInactiveDate", Order = 7)]
	public DateTime? xaxInactiveDate { get; set; }

	[JsonProperty("xaxInactive", Order = 8)]
	public bool xaxInactive { get; set; }

	[JsonProperty("xaxIncludePrimaryTax", Order = 9)]
	public bool xaxIncludePrimaryTax { get; set; }

	[JsonProperty("xaxRowVersion", Order = 10)]
	public byte[] xaxRowVersion { get; set; }

	[JsonProperty("xaxTaxOption", Order = 11)]
	[Required(ErrorMessage = "xaxTaxOption is required.")]
	[MaxLength(1)]
	public string xaxTaxOption { get; set; }

	[JsonProperty("xaxTaxType", Order = 12)]
	[Required(ErrorMessage = "xaxTaxType is required.")]
	public byte xaxTaxType { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
