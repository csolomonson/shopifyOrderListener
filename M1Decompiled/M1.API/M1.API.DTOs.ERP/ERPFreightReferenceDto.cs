using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPFreightReferenceDto
{
	[JsonProperty("frcFreightReferenceID", Order = 1)]
	[Required(ErrorMessage = "frcFreightReferenceID is required.")]
	[MaxLength(10)]
	public string frcFreightReferenceID { get; set; }

	[JsonProperty("frcUniqueID", Order = 2)]
	public Guid frcUniqueID { get; set; }

	[JsonProperty("frcFreightShipmentID", Order = 3)]
	[Required(ErrorMessage = "frcFreightShipmentID is required.")]
	[MaxLength(10)]
	public string frcFreightShipmentID { get; set; }

	[JsonProperty("frcQuoteID", Order = 4)]
	[MaxLength(10)]
	public string frcQuoteID { get; set; }

	[JsonProperty("frcRowVersion", Order = 5)]
	public byte[] frcRowVersion { get; set; }

	[JsonProperty("frcShipmentID", Order = 6)]
	[MaxLength(10)]
	public string frcShipmentID { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
