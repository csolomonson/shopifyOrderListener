using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWorkCenterSkillCompetencyDto
{
	[JsonProperty("xbbCommentsRTF", Order = 1)]
	[MaxLength(50)]
	public string xbbCommentsRTF { get; set; }

	[JsonProperty("xbbCommentsText", Order = 2)]
	[MaxLength(50)]
	public string xbbCommentsText { get; set; }

	[JsonProperty("xbbCompetencyID", Order = 3)]
	[Required(ErrorMessage = "xbbCompetencyID is required.")]
	[MaxLength(10)]
	public string xbbCompetencyID { get; set; }

	[JsonProperty("xbbCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string xbbCreatedBy { get; set; }

	[JsonProperty("xbbCreatedDate", Order = 5)]
	public DateTime? xbbCreatedDate { get; set; }

	[JsonProperty("xbbDateAchieved", Order = 6)]
	public DateTime? xbbDateAchieved { get; set; }

	[JsonProperty("xbbDateExpires", Order = 7)]
	public DateTime? xbbDateExpires { get; set; }

	[JsonProperty("xbbUniqueID", Order = 8)]
	public Guid xbbUniqueID { get; set; }

	[JsonProperty("xbbRowVersion", Order = 9)]
	public byte[] xbbRowVersion { get; set; }

	[JsonProperty("xbbWorkCenterSkillCompetencyID", Order = 10)]
	[Required(ErrorMessage = "xbbWorkCenterSkillCompetencyID is required.")]
	public short xbbWorkCenterSkillCompetencyID { get; set; }

	[JsonProperty("xbbSkillID", Order = 11)]
	[Required(ErrorMessage = "xbbSkillID is required.")]
	[MaxLength(10)]
	public string xbbSkillID { get; set; }

	[JsonProperty("xbbWorkCenterID", Order = 12)]
	[Required(ErrorMessage = "xbbWorkCenterID is required.")]
	[MaxLength(5)]
	public string xbbWorkCenterID { get; set; }

	[JsonProperty("xbbWorkCenterSkillID", Order = 13)]
	[Required(ErrorMessage = "xbbWorkCenterSkillID is required.")]
	public short xbbWorkCenterSkillID { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
