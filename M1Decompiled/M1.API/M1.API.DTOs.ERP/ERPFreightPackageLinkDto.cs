using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPFreightPackageLinkDto
{
	[JsonProperty("fplCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string fplCreatedBy { get; set; }

	[JsonProperty("fplCreatedDate", Order = 2)]
	public DateTime? fplCreatedDate { get; set; }

	[JsonProperty("fplUniqueID", Order = 3)]
	public Guid fplUniqueID { get; set; }

	[JsonProperty("fplFreightPackageID", Order = 4)]
	public short fplFreightPackageID { get; set; }

	[JsonProperty("fplFreightPackageLineID", Order = 5)]
	public short fplFreightPackageLineID { get; set; }

	[JsonProperty("fplFreightShipmentID", Order = 6)]
	[MaxLength(10)]
	public string fplFreightShipmentID { get; set; }

	[JsonProperty("fplRowVersion", Order = 7)]
	public byte[] fplRowVersion { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
