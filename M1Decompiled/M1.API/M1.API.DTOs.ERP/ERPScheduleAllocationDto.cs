using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPScheduleAllocationDto
{
	[JsonProperty("sxdDateType", Order = 1)]
	public byte sxdDateType { get; set; }

	[JsonProperty("sxdEndActualDateTime", Order = 2)]
	public DateTime? sxdEndActualDateTime { get; set; }

	[JsonProperty("sxdEndDate", Order = 3)]
	public DateTime? sxdEndDate { get; set; }

	[JsonProperty("sxdEndMinute", Order = 4)]
	public short sxdEndMinute { get; set; }

	[JsonProperty("sxdUniqueID", Order = 5)]
	public Guid sxdUniqueID { get; set; }

	[JsonProperty("sxdGroupUniqueID", Order = 6)]
	public Guid? sxdGroupUniqueID { get; set; }

	[JsonProperty("sxdMinutes", Order = 7)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int sxdMinutes { get; set; }

	[JsonProperty("sxdResourceUniqueID", Order = 8)]
	public Guid? sxdResourceUniqueID { get; set; }

	[JsonProperty("sxdRowVersion", Order = 9)]
	public byte[] sxdRowVersion { get; set; }

	[JsonProperty("sxdScheduleBranchID", Order = 10)]
	public int sxdScheduleBranchID { get; set; }

	[JsonProperty("sxdScheduleResourceLaneID", Order = 11)]
	public short sxdScheduleResourceLaneID { get; set; }

	[JsonProperty("sxdScheduleTaskID", Order = 12)]
	public int sxdScheduleTaskID { get; set; }

	[JsonProperty("sxdScheduleTreeID", Order = 13)]
	public int sxdScheduleTreeID { get; set; }

	[JsonProperty("sxdScheduleAllocationID", Order = 14)]
	[Required(ErrorMessage = "sxdScheduleAllocationID is required.")]
	public byte sxdScheduleAllocationID { get; set; }

	[JsonProperty("sxdStartActualDateTime", Order = 15)]
	public DateTime? sxdStartActualDateTime { get; set; }

	[JsonProperty("sxdStartDate", Order = 16)]
	public DateTime? sxdStartDate { get; set; }

	[JsonProperty("sxdStartMinute", Order = 17)]
	public short sxdStartMinute { get; set; }

	[JsonProperty("customFields", Order = 18)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
