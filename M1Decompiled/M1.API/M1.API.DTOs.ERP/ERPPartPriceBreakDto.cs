using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartPriceBreakDto
{
	[JsonProperty("imjCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string imjCreatedBy { get; set; }

	[JsonProperty("imjCreatedDate", Order = 2)]
	public DateTime? imjCreatedDate { get; set; }

	[JsonProperty("imjDiscount", Order = 3)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imjDiscount { get; set; }

	[JsonProperty("imjUniqueID", Order = 4)]
	public Guid imjUniqueID { get; set; }

	[JsonProperty("imjLeadTime", Order = 5)]
	public short imjLeadTime { get; set; }

	[JsonProperty("imjPartPriceID", Order = 6)]
	[Required(ErrorMessage = "imjPartPriceID is required.")]
	public int imjPartPriceID { get; set; }

	[JsonProperty("imjProposedNewPrice", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imjProposedNewPrice { get; set; }

	[JsonProperty("imjQuantity", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imjQuantity { get; set; }

	[JsonProperty("imjRowVersion", Order = 9)]
	public byte[] imjRowVersion { get; set; }

	[JsonProperty("imjPartPriceBreakID", Order = 10)]
	[Required(ErrorMessage = "imjPartPriceBreakID is required.")]
	public short imjPartPriceBreakID { get; set; }

	[JsonProperty("imjUnitPrice", Order = 11)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imjUnitPrice { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
