using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPKnowledgeBasePageDto
{
	[JsonProperty("kbbAccessedCount", Order = 1)]
	[Range(0.0, 9999999999.0, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal kbbAccessedCount { get; set; }

	[JsonProperty("kbbClosedByEmployeeID", Order = 2)]
	[MaxLength(10)]
	public string kbbClosedByEmployeeID { get; set; }

	[JsonProperty("kbbClosedDate", Order = 3)]
	public DateTime? kbbClosedDate { get; set; }

	[JsonProperty("kbbKnowledgeBasePageID", Order = 4)]
	[Required(ErrorMessage = "kbbKnowledgeBasePageID is required.")]
	[MaxLength(10)]
	public string kbbKnowledgeBasePageID { get; set; }

	[JsonProperty("kbbCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string kbbCreatedBy { get; set; }

	[JsonProperty("kbbCreatedDate", Order = 6)]
	public DateTime? kbbCreatedDate { get; set; }

	[JsonProperty("kbbDescription", Order = 7)]
	[Required(ErrorMessage = "kbbDescription is required.")]
	[MaxLength(70)]
	public string kbbDescription { get; set; }

	[JsonProperty("kbbUniqueID", Order = 8)]
	public Guid kbbUniqueID { get; set; }

	[JsonProperty("kbbOpenedByEmployeeID", Order = 9)]
	[Required(ErrorMessage = "kbbOpenedByEmployeeID is required.")]
	[MaxLength(10)]
	public string kbbOpenedByEmployeeID { get; set; }

	[JsonProperty("kbbOpenedDate", Order = 10)]
	[Required(ErrorMessage = "kbbOpenedDate is required.")]
	public DateTime? kbbOpenedDate { get; set; }

	[JsonProperty("kbbPartID", Order = 11)]
	[Required(ErrorMessage = "kbbPartID is required.")]
	[MaxLength(30)]
	public string kbbPartID { get; set; }

	[JsonProperty("kbbPartRevisionID", Order = 12)]
	[MaxLength(15)]
	public string kbbPartRevisionID { get; set; }

	[JsonProperty("kbbProblemDescriptionRtf", Order = 13)]
	[MaxLength(50)]
	public string kbbProblemDescriptionRtf { get; set; }

	[JsonProperty("kbbProblemDescriptionText", Order = 14)]
	[MaxLength(50)]
	public string kbbProblemDescriptionText { get; set; }

	[JsonProperty("kbbResolutionDescriptionRtf", Order = 15)]
	[MaxLength(50)]
	public string kbbResolutionDescriptionRtf { get; set; }

	[JsonProperty("kbbResolutionDescriptionText", Order = 16)]
	[MaxLength(50)]
	public string kbbResolutionDescriptionText { get; set; }

	[JsonProperty("kbbResolvedPartID", Order = 17)]
	[MaxLength(30)]
	public string kbbResolvedPartID { get; set; }

	[JsonProperty("kbbResolvedPartRevisionID", Order = 18)]
	[MaxLength(15)]
	public string kbbResolvedPartRevisionID { get; set; }

	[JsonProperty("kbbRowVersion", Order = 19)]
	public byte[] kbbRowVersion { get; set; }

	[JsonProperty("kbbStatus", Order = 20)]
	[Required(ErrorMessage = "kbbStatus is required.")]
	[MaxLength(1)]
	public string kbbStatus { get; set; }

	[JsonProperty("kbbWorkAroundDescriptionRtf", Order = 21)]
	[MaxLength(50)]
	public string kbbWorkAroundDescriptionRtf { get; set; }

	[JsonProperty("kbbWorkAroundDescriptionText", Order = 22)]
	[MaxLength(50)]
	public string kbbWorkAroundDescriptionText { get; set; }

	[JsonProperty("customFields", Order = 23)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
