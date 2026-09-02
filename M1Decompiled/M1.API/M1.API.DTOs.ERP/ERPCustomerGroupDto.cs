using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPCustomerGroupDto
{
	[JsonProperty("cmuCustomerGroupID", Order = 1)]
	[Required(ErrorMessage = "cmuCustomerGroupID is required.")]
	[MaxLength(5)]
	public string cmuCustomerGroupID { get; set; }

	[JsonProperty("cmuCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string cmuCreatedBy { get; set; }

	[JsonProperty("cmuCreatedDate", Order = 3)]
	public DateTime? cmuCreatedDate { get; set; }

	[JsonProperty("cmuDescription", Order = 4)]
	[Required(ErrorMessage = "cmuDescription is required.")]
	[MaxLength(50)]
	public string cmuDescription { get; set; }

	[JsonProperty("cmuUniqueID", Order = 5)]
	public Guid cmuUniqueID { get; set; }

	[JsonProperty("cmuRowVersion", Order = 6)]
	public byte[] cmuRowVersion { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
