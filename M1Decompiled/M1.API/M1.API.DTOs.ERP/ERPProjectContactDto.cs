using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPProjectContactDto
{
	[JsonProperty("prcContactID", Order = 1)]
	[Required(ErrorMessage = "prcContactID is required.")]
	[MaxLength(5)]
	public string prcContactID { get; set; }

	[JsonProperty("prcContactTitleID", Order = 2)]
	[MaxLength(5)]
	public string prcContactTitleID { get; set; }

	[JsonProperty("prcCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string prcCreatedBy { get; set; }

	[JsonProperty("prcCreatedDate", Order = 4)]
	public DateTime? prcCreatedDate { get; set; }

	[JsonProperty("prcUniqueID", Order = 5)]
	public Guid prcUniqueID { get; set; }

	[JsonProperty("prcLocationID", Order = 6)]
	[MaxLength(5)]
	public string prcLocationID { get; set; }

	[JsonProperty("prcNotesRTF", Order = 7)]
	[MaxLength(50)]
	public string prcNotesRTF { get; set; }

	[JsonProperty("prcNotesText", Order = 8)]
	[MaxLength(50)]
	public string prcNotesText { get; set; }

	[JsonProperty("prcOrganizationID", Order = 9)]
	[Required(ErrorMessage = "prcOrganizationID is required.")]
	[MaxLength(10)]
	public string prcOrganizationID { get; set; }

	[JsonProperty("prcProjectID", Order = 10)]
	[Required(ErrorMessage = "prcProjectID is required.")]
	[MaxLength(10)]
	public string prcProjectID { get; set; }

	[JsonProperty("prcRowVersion", Order = 11)]
	public byte[] prcRowVersion { get; set; }

	[JsonProperty("prcProjectContactID", Order = 12)]
	[Required(ErrorMessage = "prcProjectContactID is required.")]
	public short prcProjectContactID { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
