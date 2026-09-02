using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPScheduleBranchDto
{
	[JsonProperty("sxbCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string sxbCreatedBy { get; set; }

	[JsonProperty("sxbCreatedDate", Order = 2)]
	public DateTime? sxbCreatedDate { get; set; }

	[JsonProperty("sxbCurrentLinkedTaskDateType", Order = 3)]
	public byte sxbCurrentLinkedTaskDateType { get; set; }

	[JsonProperty("sxbCurrentLinkedTaskID", Order = 4)]
	public int sxbCurrentLinkedTaskID { get; set; }

	[JsonProperty("sxbUniqueID", Order = 5)]
	public Guid sxbUniqueID { get; set; }

	[JsonProperty("sxbOffsetMinutes", Order = 6)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int sxbOffsetMinutes { get; set; }

	[JsonProperty("sxbParentLinkedTaskDateType", Order = 7)]
	public byte sxbParentLinkedTaskDateType { get; set; }

	[JsonProperty("sxbParentLinkedTaskID", Order = 8)]
	public int sxbParentLinkedTaskID { get; set; }

	[JsonProperty("sxbParentScheduleBranchID", Order = 9)]
	public int sxbParentScheduleBranchID { get; set; }

	[JsonProperty("sxbRowVersion", Order = 10)]
	public byte[] sxbRowVersion { get; set; }

	[JsonProperty("sxbScheduleTreeID", Order = 11)]
	[Required(ErrorMessage = "sxbScheduleTreeID is required.")]
	public int sxbScheduleTreeID { get; set; }

	[JsonProperty("sxbScheduleBranchID", Order = 12)]
	[Required(ErrorMessage = "sxbScheduleBranchID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int sxbScheduleBranchID { get; set; }

	[JsonProperty("sxbSiblingBranchLink", Order = 13)]
	public byte sxbSiblingBranchLink { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
