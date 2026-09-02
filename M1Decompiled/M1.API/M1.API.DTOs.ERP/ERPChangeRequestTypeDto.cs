using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPChangeRequestTypeDto
{
	[JsonProperty("chtChangeRequestTypeID", Order = 1)]
	[Required(ErrorMessage = "chtChangeRequestTypeID is required.")]
	[MaxLength(5)]
	public string chtChangeRequestTypeID { get; set; }

	[JsonProperty("chtCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string chtCreatedBy { get; set; }

	[JsonProperty("chtCreatedDate", Order = 3)]
	public DateTime? chtCreatedDate { get; set; }

	[JsonProperty("chtDescription", Order = 4)]
	[Required(ErrorMessage = "chtDescription is required.")]
	[MaxLength(50)]
	public string chtDescription { get; set; }

	[JsonProperty("chtUniqueID", Order = 5)]
	public Guid chtUniqueID { get; set; }

	[JsonProperty("chtInactiveDate", Order = 6)]
	public DateTime? chtInactiveDate { get; set; }

	[JsonProperty("chtInactive", Order = 7)]
	public bool chtInactive { get; set; }

	[JsonProperty("chtRowVersion", Order = 8)]
	public byte[] chtRowVersion { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
