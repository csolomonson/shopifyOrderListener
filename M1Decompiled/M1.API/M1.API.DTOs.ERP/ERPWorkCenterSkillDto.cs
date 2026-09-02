using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWorkCenterSkillDto
{
	[JsonProperty("xbaCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string xbaCreatedBy { get; set; }

	[JsonProperty("xbaCreatedDate", Order = 2)]
	public DateTime? xbaCreatedDate { get; set; }

	[JsonProperty("xbaDocuments", Order = 3)]
	[MaxLength(50)]
	public string xbaDocuments { get; set; }

	[JsonProperty("xbaUniqueID", Order = 4)]
	public Guid xbaUniqueID { get; set; }

	[JsonProperty("xbaNotesRTF", Order = 5)]
	[MaxLength(50)]
	public string xbaNotesRTF { get; set; }

	[JsonProperty("xbaNotesText", Order = 6)]
	[MaxLength(50)]
	public string xbaNotesText { get; set; }

	[JsonProperty("xbaRowVersion", Order = 7)]
	public byte[] xbaRowVersion { get; set; }

	[JsonProperty("xbaWorkCenterSkillID", Order = 8)]
	[Required(ErrorMessage = "xbaWorkCenterSkillID is required.")]
	public short xbaWorkCenterSkillID { get; set; }

	[JsonProperty("xbaSkillID", Order = 9)]
	[Required(ErrorMessage = "xbaSkillID is required.")]
	[MaxLength(10)]
	public string xbaSkillID { get; set; }

	[JsonProperty("xbaWorkCenterID", Order = 10)]
	[Required(ErrorMessage = "xbaWorkCenterID is required.")]
	[MaxLength(5)]
	public string xbaWorkCenterID { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
