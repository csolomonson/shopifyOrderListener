using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPEmployeeSkillDto
{
	[JsonProperty("lnkCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string lnkCreatedBy { get; set; }

	[JsonProperty("lnkCreatedDate", Order = 2)]
	public DateTime? lnkCreatedDate { get; set; }

	[JsonProperty("lnkDocuments", Order = 3)]
	[MaxLength(50)]
	public string lnkDocuments { get; set; }

	[JsonProperty("lnkEmployeeID", Order = 4)]
	[Required(ErrorMessage = "lnkEmployeeID is required.")]
	[MaxLength(10)]
	public string lnkEmployeeID { get; set; }

	[JsonProperty("lnkUniqueID", Order = 5)]
	public Guid lnkUniqueID { get; set; }

	[JsonProperty("lnkNotesRTF", Order = 6)]
	[MaxLength(50)]
	public string lnkNotesRTF { get; set; }

	[JsonProperty("lnkNotesText", Order = 7)]
	[MaxLength(50)]
	public string lnkNotesText { get; set; }

	[JsonProperty("lnkRowVersion", Order = 8)]
	public byte[] lnkRowVersion { get; set; }

	[JsonProperty("lnkEmployeeSkillID", Order = 9)]
	[Required(ErrorMessage = "lnkEmployeeSkillID is required.")]
	public short lnkEmployeeSkillID { get; set; }

	[JsonProperty("lnkSkillID", Order = 10)]
	[Required(ErrorMessage = "lnkSkillID is required.")]
	[MaxLength(10)]
	public string lnkSkillID { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
