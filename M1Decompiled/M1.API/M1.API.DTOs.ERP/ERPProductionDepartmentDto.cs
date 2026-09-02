using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPProductionDepartmentDto
{
	[JsonProperty("xaeProductionDepartmentID", Order = 1)]
	[Required(ErrorMessage = "xaeProductionDepartmentID is required.")]
	[MaxLength(5)]
	public string xaeProductionDepartmentID { get; set; }

	[JsonProperty("xaeCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string xaeCreatedBy { get; set; }

	[JsonProperty("xaeCreatedDate", Order = 3)]
	public DateTime? xaeCreatedDate { get; set; }

	[JsonProperty("xaeDescription", Order = 4)]
	[Required(ErrorMessage = "xaeDescription is required.")]
	[MaxLength(50)]
	public string xaeDescription { get; set; }

	[JsonProperty("xaeUniqueID", Order = 5)]
	public Guid xaeUniqueID { get; set; }

	[JsonProperty("xaeRowVersion", Order = 6)]
	public byte[] xaeRowVersion { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
