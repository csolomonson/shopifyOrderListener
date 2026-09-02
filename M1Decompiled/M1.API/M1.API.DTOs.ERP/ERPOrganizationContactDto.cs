using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPOrganizationContactDto
{
	[JsonProperty("cmcAlternatePhoneNumber", Order = 1)]
	[MaxLength(20)]
	public string cmcAlternatePhoneNumber { get; set; }

	[JsonProperty("cmcContactID", Order = 2)]
	[Required(ErrorMessage = "cmcContactID is required.")]
	[MaxLength(5)]
	public string cmcContactID { get; set; }

	[JsonProperty("cmcContactTitleID", Order = 3)]
	[MaxLength(5)]
	public string cmcContactTitleID { get; set; }

	[JsonProperty("cmcCorrespondenceMethod", Order = 4)]
	[MaxLength(1)]
	public string cmcCorrespondenceMethod { get; set; }

	[JsonProperty("cmcCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string cmcCreatedBy { get; set; }

	[JsonProperty("cmcCreatedDate", Order = 6)]
	public DateTime? cmcCreatedDate { get; set; }

	[JsonProperty("cmcEmailAddress", Order = 7)]
	[MaxLength(50)]
	public string cmcEmailAddress { get; set; }

	[JsonProperty("cmcUniqueID", Order = 8)]
	public Guid cmcUniqueID { get; set; }

	[JsonProperty("cmcFaxNumber", Order = 9)]
	[MaxLength(20)]
	public string cmcFaxNumber { get; set; }

	[JsonProperty("cmcInactiveDate", Order = 10)]
	public DateTime? cmcInactiveDate { get; set; }

	[JsonProperty("cmcInactive", Order = 11)]
	public bool cmcInactive { get; set; }

	[JsonProperty("cmcCreatedFromMobile", Order = 12)]
	public bool cmcCreatedFromMobile { get; set; }

	[JsonProperty("cmcNoMailings", Order = 13)]
	public bool cmcNoMailings { get; set; }

	[JsonProperty("cmcLocationID", Order = 14)]
	[MaxLength(5)]
	public string cmcLocationID { get; set; }

	[JsonProperty("cmcMobileNumber", Order = 15)]
	[MaxLength(20)]
	public string cmcMobileNumber { get; set; }

	[JsonProperty("cmcName", Order = 16)]
	[Required(ErrorMessage = "cmcName is required.")]
	[MaxLength(50)]
	public string cmcName { get; set; }

	[JsonProperty("cmcNoteRtf", Order = 17)]
	[MaxLength(50)]
	public string cmcNoteRtf { get; set; }

	[JsonProperty("cmcNoteText", Order = 18)]
	[MaxLength(50)]
	public string cmcNoteText { get; set; }

	[JsonProperty("cmcOrganizationID", Order = 19)]
	[Required(ErrorMessage = "cmcOrganizationID is required.")]
	[MaxLength(10)]
	public string cmcOrganizationID { get; set; }

	[JsonProperty("cmcPhoneNumber", Order = 20)]
	[MaxLength(20)]
	public string cmcPhoneNumber { get; set; }

	[JsonProperty("cmcRowVersion", Order = 21)]
	public byte[] cmcRowVersion { get; set; }

	[JsonProperty("customFields", Order = 22)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
