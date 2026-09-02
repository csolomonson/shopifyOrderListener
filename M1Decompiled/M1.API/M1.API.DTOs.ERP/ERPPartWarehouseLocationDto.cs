using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartWarehouseLocationDto
{
	[JsonProperty("imlCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string imlCreatedBy { get; set; }

	[JsonProperty("imlCreatedDate", Order = 2)]
	public DateTime? imlCreatedDate { get; set; }

	[JsonProperty("imlUniqueID", Order = 3)]
	public Guid imlUniqueID { get; set; }

	[JsonProperty("imlNonNettable", Order = 4)]
	public bool imlNonNettable { get; set; }

	[JsonProperty("imLLastRunDatePurchasePlanner", Order = 5)]
	public DateTime? imLLastRunDatePurchasePlanner { get; set; }

	[JsonProperty("imlMaximumQuantity", Order = 6)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imlMaximumQuantity { get; set; }

	[JsonProperty("imlMinimumQuantity", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imlMinimumQuantity { get; set; }

	[JsonProperty("imlPartID", Order = 8)]
	[Required(ErrorMessage = "imlPartID is required.")]
	[MaxLength(30)]
	public string imlPartID { get; set; }

	[JsonProperty("imlPartRevisionID", Order = 9)]
	[MaxLength(15)]
	public string imlPartRevisionID { get; set; }

	[JsonProperty("imlPartWarehouseID", Order = 10)]
	[Required(ErrorMessage = "imlPartWarehouseID is required.")]
	[MaxLength(5)]
	public string imlPartWarehouseID { get; set; }

	[JsonProperty("imlQuantityInTransit", Order = 11)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal imlQuantityInTransit { get; set; }

	[JsonProperty("imlRowVersion", Order = 12)]
	public byte[] imlRowVersion { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
