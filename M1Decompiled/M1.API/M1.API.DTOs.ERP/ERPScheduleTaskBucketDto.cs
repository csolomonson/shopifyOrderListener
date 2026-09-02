using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPScheduleTaskBucketDto
{
	[JsonProperty("sxeCompletedMinutes", Order = 1)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int sxeCompletedMinutes { get; set; }

	[JsonProperty("sxeUniqueID", Order = 2)]
	public Guid sxeUniqueID { get; set; }

	[JsonProperty("sxeCompleted", Order = 3)]
	public bool sxeCompleted { get; set; }

	[JsonProperty("sxeMinutes", Order = 4)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int sxeMinutes { get; set; }

	[JsonProperty("sxePercentComplete", Order = 5)]
	[Range(0.0, 9999999999.0, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int sxePercentComplete { get; set; }

	[JsonProperty("sxeRowVersion", Order = 6)]
	public byte[] sxeRowVersion { get; set; }

	[JsonProperty("sxeScheduleBranchID", Order = 7)]
	[Required(ErrorMessage = "sxeScheduleBranchID is required.")]
	public int sxeScheduleBranchID { get; set; }

	[JsonProperty("sxeScheduleTaskID", Order = 8)]
	[Required(ErrorMessage = "sxeScheduleTaskID is required.")]
	public int sxeScheduleTaskID { get; set; }

	[JsonProperty("sxeScheduleTreeID", Order = 9)]
	[Required(ErrorMessage = "sxeScheduleTreeID is required.")]
	public int sxeScheduleTreeID { get; set; }

	[JsonProperty("sxeScheduleTypeBucketID", Order = 10)]
	public byte sxeScheduleTypeBucketID { get; set; }

	[JsonProperty("sxeScheduleTypeID", Order = 11)]
	public byte sxeScheduleTypeID { get; set; }

	[JsonProperty("sxeScheduleTaskBucketID", Order = 12)]
	[Required(ErrorMessage = "sxeScheduleTaskBucketID is required.")]
	public byte sxeScheduleTaskBucketID { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
