using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPEmployeeSkillCompetencyDto
{
	[JsonProperty("lnpCommentsRTF", Order = 1)]
	[MaxLength(50)]
	public string lnpCommentsRTF { get; set; }

	[JsonProperty("lnpCommentsText", Order = 2)]
	[MaxLength(50)]
	public string lnpCommentsText { get; set; }

	[JsonProperty("lnpCompetencyID", Order = 3)]
	[Required(ErrorMessage = "lnpCompetencyID is required.")]
	[MaxLength(10)]
	public string lnpCompetencyID { get; set; }

	[JsonProperty("lnpCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string lnpCreatedBy { get; set; }

	[JsonProperty("lnpCreatedDate", Order = 5)]
	public DateTime? lnpCreatedDate { get; set; }

	[JsonProperty("lnpDateAchieved", Order = 6)]
	public DateTime? lnpDateAchieved { get; set; }

	[JsonProperty("lnpDateExpires", Order = 7)]
	public DateTime? lnpDateExpires { get; set; }

	[JsonProperty("lnpEmployeeID", Order = 8)]
	[Required(ErrorMessage = "lnpEmployeeID is required.")]
	[MaxLength(10)]
	public string lnpEmployeeID { get; set; }

	[JsonProperty("lnpEmployeeSkillID", Order = 9)]
	[Required(ErrorMessage = "lnpEmployeeSkillID is required.")]
	public short lnpEmployeeSkillID { get; set; }

	[JsonProperty("lnpUniqueID", Order = 10)]
	public Guid lnpUniqueID { get; set; }

	[JsonProperty("lnpRowVersion", Order = 11)]
	public byte[] lnpRowVersion { get; set; }

	[JsonProperty("lnpEmployeeSkillCompetencyID", Order = 12)]
	[Required(ErrorMessage = "lnpEmployeeSkillCompetencyID is required.")]
	public short lnpEmployeeSkillCompetencyID { get; set; }

	[JsonProperty("lnpSkillID", Order = 13)]
	[Required(ErrorMessage = "lnpSkillID is required.")]
	[MaxLength(10)]
	public string lnpSkillID { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
