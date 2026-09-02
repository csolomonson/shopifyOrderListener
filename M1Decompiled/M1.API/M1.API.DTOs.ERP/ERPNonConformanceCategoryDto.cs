using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPNonConformanceCategoryDto
{
	[JsonProperty("qagNonConformanceCategoryID", Order = 1)]
	[Required(ErrorMessage = "qagNonConformanceCategoryID is required.")]
	[MaxLength(5)]
	public string qagNonConformanceCategoryID { get; set; }

	[JsonProperty("qagCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string qagCreatedBy { get; set; }

	[JsonProperty("qagCreatedDate", Order = 3)]
	public DateTime? qagCreatedDate { get; set; }

	[JsonProperty("qagDescription", Order = 4)]
	[Required(ErrorMessage = "qagDescription is required.")]
	[MaxLength(50)]
	public string qagDescription { get; set; }

	[JsonProperty("qagUniqueID", Order = 5)]
	public Guid qagUniqueID { get; set; }

	[JsonProperty("qagRowVersion", Order = 6)]
	public byte[] qagRowVersion { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
