using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLeadCompetitorDto
{
	[JsonProperty("locCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string locCreatedBy { get; set; }

	[JsonProperty("locCreatedDate", Order = 2)]
	public DateTime? locCreatedDate { get; set; }

	[JsonProperty("locUniqueID", Order = 3)]
	public Guid locUniqueID { get; set; }

	[JsonProperty("locLeadID", Order = 4)]
	[Required(ErrorMessage = "locLeadID is required.")]
	[MaxLength(10)]
	public string locLeadID { get; set; }

	[JsonProperty("locLeadNotesRTF", Order = 5)]
	[MaxLength(50)]
	public string locLeadNotesRTF { get; set; }

	[JsonProperty("locLeadNotesText", Order = 6)]
	[MaxLength(50)]
	public string locLeadNotesText { get; set; }

	[JsonProperty("locOrganizationID", Order = 7)]
	[Required(ErrorMessage = "locOrganizationID is required.")]
	[MaxLength(10)]
	public string locOrganizationID { get; set; }

	[JsonProperty("locProductName", Order = 8)]
	[MaxLength(50)]
	public string locProductName { get; set; }

	[JsonProperty("locRowVersion", Order = 9)]
	public byte[] locRowVersion { get; set; }

	[JsonProperty("locLeadCompetitorID", Order = 10)]
	[Required(ErrorMessage = "locLeadCompetitorID is required.")]
	public short locLeadCompetitorID { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
