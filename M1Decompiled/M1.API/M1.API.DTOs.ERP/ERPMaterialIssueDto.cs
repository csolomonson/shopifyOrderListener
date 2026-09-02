using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPMaterialIssueDto
{
	[JsonProperty("iniMaterialIssueID", Order = 1)]
	[Required(ErrorMessage = "iniMaterialIssueID is required.")]
	[MaxLength(10)]
	public string iniMaterialIssueID { get; set; }

	[JsonProperty("iniCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string iniCreatedBy { get; set; }

	[JsonProperty("iniCreatedDate", Order = 3)]
	public DateTime? iniCreatedDate { get; set; }

	[JsonProperty("iniUniqueID", Order = 4)]
	public Guid iniUniqueID { get; set; }

	[JsonProperty("iniPosted", Order = 5)]
	public bool iniPosted { get; set; }

	[JsonProperty("iniReversalEntry", Order = 6)]
	public bool iniReversalEntry { get; set; }

	[JsonProperty("iniReversed", Order = 7)]
	public bool iniReversed { get; set; }

	[JsonProperty("iniMaterialIssueDate", Order = 8)]
	[Required(ErrorMessage = "iniMaterialIssueDate is required.")]
	public DateTime? iniMaterialIssueDate { get; set; }

	[JsonProperty("iniPostedDate", Order = 9)]
	public DateTime? iniPostedDate { get; set; }

	[JsonProperty("iniRowVersion", Order = 10)]
	public byte[] iniRowVersion { get; set; }

	[JsonProperty("iniSourceTableUniqueID", Order = 11)]
	public Guid iniSourceTableUniqueID { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
