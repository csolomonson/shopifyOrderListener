using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPChangeRequestGroupDto
{
	[JsonProperty("chgChangeRequestGroupID", Order = 1)]
	[Required(ErrorMessage = "chgChangeRequestGroupID is required.")]
	[MaxLength(5)]
	public string chgChangeRequestGroupID { get; set; }

	[JsonProperty("chgCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string chgCreatedBy { get; set; }

	[JsonProperty("chgCreatedDate", Order = 3)]
	public DateTime? chgCreatedDate { get; set; }

	[JsonProperty("chgDescription", Order = 4)]
	[MaxLength(50)]
	public string chgDescription { get; set; }

	[JsonProperty("chgUniqueID", Order = 5)]
	public Guid chgUniqueID { get; set; }

	[JsonProperty("chgInactiveDate", Order = 6)]
	public DateTime? chgInactiveDate { get; set; }

	[JsonProperty("chgInactive", Order = 7)]
	public bool chgInactive { get; set; }

	[JsonProperty("chgRowVersion", Order = 8)]
	public byte[] chgRowVersion { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
