using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPOrganizationIndustryTypeLinkDto
{
	[JsonProperty("cmdCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string cmdCreatedBy { get; set; }

	[JsonProperty("cmdCreatedDate", Order = 2)]
	public DateTime? cmdCreatedDate { get; set; }

	[JsonProperty("cmdUniqueID", Order = 3)]
	public Guid cmdUniqueID { get; set; }

	[JsonProperty("cmdIndustryTypeID", Order = 4)]
	[Required(ErrorMessage = "cmdIndustryTypeID is required.")]
	[MaxLength(10)]
	public string cmdIndustryTypeID { get; set; }

	[JsonProperty("cmdIndustryTypeLinkID", Order = 5)]
	public short cmdIndustryTypeLinkID { get; set; }

	[JsonProperty("cmdOrganizationID", Order = 6)]
	[Required(ErrorMessage = "cmdOrganizationID is required.")]
	[MaxLength(10)]
	public string cmdOrganizationID { get; set; }

	[JsonProperty("cmdRowVersion", Order = 7)]
	public byte[] cmdRowVersion { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
