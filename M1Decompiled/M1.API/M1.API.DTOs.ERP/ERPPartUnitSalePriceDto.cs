using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartUnitSalePriceDto
{
	[JsonProperty("imhCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string imhCreatedBy { get; set; }

	[JsonProperty("imhCreatedDate", Order = 2)]
	public DateTime? imhCreatedDate { get; set; }

	[JsonProperty("imhCurrencyRateID", Order = 3)]
	[MaxLength(5)]
	public string imhCurrencyRateID { get; set; }

	[JsonProperty("imhEndDate", Order = 4)]
	public DateTime? imhEndDate { get; set; }

	[JsonProperty("imhUniqueID", Order = 5)]
	public Guid imhUniqueID { get; set; }

	[JsonProperty("imhPartID", Order = 6)]
	[MaxLength(30)]
	public string imhPartID { get; set; }

	[JsonProperty("imhPartRevisionID", Order = 7)]
	[MaxLength(15)]
	public string imhPartRevisionID { get; set; }

	[JsonProperty("imhRowVersion", Order = 8)]
	public byte[] imhRowVersion { get; set; }

	[JsonProperty("imhPartUnitSalePriceID", Order = 9)]
	public short imhPartUnitSalePriceID { get; set; }

	[JsonProperty("imhStartDate", Order = 10)]
	public DateTime? imhStartDate { get; set; }

	[JsonProperty("imhUnitSalePrice", Order = 11)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imhUnitSalePrice { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
