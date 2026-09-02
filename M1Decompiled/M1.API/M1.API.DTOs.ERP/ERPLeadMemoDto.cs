using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLeadMemoDto
{
	[JsonProperty("lokCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string lokCreatedBy { get; set; }

	[JsonProperty("lokCreatedDate", Order = 2)]
	public DateTime? lokCreatedDate { get; set; }

	[JsonProperty("lokUniqueID", Order = 3)]
	public Guid lokUniqueID { get; set; }

	[JsonProperty("lokLeadID", Order = 4)]
	[Required(ErrorMessage = "lokLeadID is required.")]
	[MaxLength(10)]
	public string lokLeadID { get; set; }

	[JsonProperty("lokLongDescriptionRtf", Order = 5)]
	public string lokLongDescriptionRtf { get; set; }

	[JsonProperty("lokLongDescriptionText", Order = 6)]
	public string lokLongDescriptionText { get; set; }

	[JsonProperty("lokMemoDate", Order = 7)]
	[Required(ErrorMessage = "lokMemoDate is required.")]
	public DateTime? lokMemoDate { get; set; }

	[JsonProperty("lokRowVersion", Order = 8)]
	public byte[] lokRowVersion { get; set; }

	[JsonProperty("lokLeadMemoID", Order = 9)]
	[Required(ErrorMessage = "lokLeadMemoID is required.")]
	public short lokLeadMemoID { get; set; }

	[JsonProperty("lokShortDescription", Order = 10)]
	[Required(ErrorMessage = "lokShortDescription is required.")]
	[MaxLength(50)]
	public string lokShortDescription { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
