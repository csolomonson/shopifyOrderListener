using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPShipmentFreightReferenceDto
{
	[JsonProperty("smrCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string smrCreatedBy { get; set; }

	[JsonProperty("smrCreatedDate", Order = 2)]
	public DateTime? smrCreatedDate { get; set; }

	[JsonProperty("smrUniqueID", Order = 3)]
	public Guid smrUniqueID { get; set; }

	[JsonProperty("smrFreightShipmentID", Order = 4)]
	[Required(ErrorMessage = "smrFreightShipmentID is required.")]
	[MaxLength(10)]
	public string smrFreightShipmentID { get; set; }

	[JsonProperty("smrRowVersion", Order = 5)]
	public byte[] smrRowVersion { get; set; }

	[JsonProperty("smrShipmentFreightReferenceID", Order = 6)]
	[Required(ErrorMessage = "smrShipmentFreightReferenceID is required.")]
	public short smrShipmentFreightReferenceID { get; set; }

	[JsonProperty("smrShipmentID", Order = 7)]
	[Required(ErrorMessage = "smrShipmentID is required.")]
	[MaxLength(10)]
	public string smrShipmentID { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
