using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLJournalMemoDto
{
	[JsonProperty("glmCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string glmCreatedBy { get; set; }

	[JsonProperty("glmCreatedDate", Order = 2)]
	public DateTime? glmCreatedDate { get; set; }

	[JsonProperty("glmUniqueID", Order = 3)]
	public Guid glmUniqueID { get; set; }

	[JsonProperty("glmGlJournalID", Order = 4)]
	[Required(ErrorMessage = "glmGlJournalID is required.")]
	public int glmGlJournalID { get; set; }

	[JsonProperty("glmClosed", Order = 5)]
	public bool glmClosed { get; set; }

	[JsonProperty("glmLongDescriptionRtf", Order = 6)]
	public string glmLongDescriptionRtf { get; set; }

	[JsonProperty("glmLongDescriptionText", Order = 7)]
	public string glmLongDescriptionText { get; set; }

	[JsonProperty("glmMemoDate", Order = 8)]
	[Required(ErrorMessage = "glmMemoDate is required.")]
	public DateTime? glmMemoDate { get; set; }

	[JsonProperty("glmRowVersion", Order = 9)]
	public byte[] glmRowVersion { get; set; }

	[JsonProperty("glmGlJournalMemoID", Order = 10)]
	[Required(ErrorMessage = "glmGlJournalMemoID is required.")]
	public short glmGlJournalMemoID { get; set; }

	[JsonProperty("glmShortDescription", Order = 11)]
	[Required(ErrorMessage = "glmShortDescription is required.")]
	[MaxLength(50)]
	public string glmShortDescription { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
