using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPScheduleResourceLaneDto
{
	[JsonProperty("sxrUniqueID", Order = 1)]
	public Guid sxrUniqueID { get; set; }

	[JsonProperty("sxrGroupUniqueID", Order = 2)]
	public Guid? sxrGroupUniqueID { get; set; }

	[JsonProperty("sxrLockedResourceUniqueID", Order = 3)]
	public Guid? sxrLockedResourceUniqueID { get; set; }

	[JsonProperty("sxrResourceType", Order = 4)]
	public byte sxrResourceType { get; set; }

	[JsonProperty("sxrRowVersion", Order = 5)]
	public byte[] sxrRowVersion { get; set; }

	[JsonProperty("sxrScheduleBranchID", Order = 6)]
	[Required(ErrorMessage = "sxrScheduleBranchID is required.")]
	public int sxrScheduleBranchID { get; set; }

	[JsonProperty("sxrScheduleTaskID", Order = 7)]
	[Required(ErrorMessage = "sxrScheduleTaskID is required.")]
	public int sxrScheduleTaskID { get; set; }

	[JsonProperty("sxrScheduleTreeID", Order = 8)]
	[Required(ErrorMessage = "sxrScheduleTreeID is required.")]
	public int sxrScheduleTreeID { get; set; }

	[JsonProperty("sxrScheduleResourceLaneID", Order = 9)]
	[Required(ErrorMessage = "sxrScheduleResourceLaneID is required.")]
	public short sxrScheduleResourceLaneID { get; set; }

	[JsonProperty("customFields", Order = 10)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
