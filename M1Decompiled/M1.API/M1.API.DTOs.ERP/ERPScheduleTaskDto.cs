using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPScheduleTaskDto
{
	[JsonProperty("sxkCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string sxkCreatedBy { get; set; }

	[JsonProperty("sxkCreatedDate", Order = 2)]
	public DateTime? sxkCreatedDate { get; set; }

	[JsonProperty("sxkCurrentTaskDateType", Order = 3)]
	public byte sxkCurrentTaskDateType { get; set; }

	[JsonProperty("sxkEndActualDateTime", Order = 4)]
	public DateTime? sxkEndActualDateTime { get; set; }

	[JsonProperty("sxkEndDate", Order = 5)]
	public DateTime? sxkEndDate { get; set; }

	[JsonProperty("sxkEndMinute", Order = 6)]
	public short sxkEndMinute { get; set; }

	[JsonProperty("sxkUniqueID", Order = 7)]
	public Guid sxkUniqueID { get; set; }

	[JsonProperty("sxkExchangeID", Order = 8)]
	[MaxLength(50)]
	public string sxkExchangeID { get; set; }

	[JsonProperty("sxkLinkedTaskDateType", Order = 9)]
	public byte sxkLinkedTaskDateType { get; set; }

	[JsonProperty("sxkLinkedTaskID", Order = 10)]
	public int sxkLinkedTaskID { get; set; }

	[JsonProperty("sxkMinutes", Order = 11)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int sxkMinutes { get; set; }

	[JsonProperty("sxkOffsetMinutes", Order = 12)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int sxkOffsetMinutes { get; set; }

	[JsonProperty("sxkPlantDepartmentID", Order = 13)]
	[MaxLength(5)]
	public string sxkPlantDepartmentID { get; set; }

	[JsonProperty("sxkPlantID", Order = 14)]
	[MaxLength(5)]
	public string sxkPlantID { get; set; }

	[JsonProperty("sxkProcessID", Order = 15)]
	[Required(ErrorMessage = "sxkProcessID is required.")]
	[MaxLength(5)]
	public string sxkProcessID { get; set; }

	[JsonProperty("sxkRowVersion", Order = 16)]
	public byte[] sxkRowVersion { get; set; }

	[JsonProperty("sxkScheduleBranchID", Order = 17)]
	[Required(ErrorMessage = "sxkScheduleBranchID is required.")]
	public int sxkScheduleBranchID { get; set; }

	[JsonProperty("sxkScheduleTreeID", Order = 18)]
	[Required(ErrorMessage = "sxkScheduleTreeID is required.")]
	public int sxkScheduleTreeID { get; set; }

	[JsonProperty("sxkScheduleTypeID", Order = 19)]
	[Required(ErrorMessage = "sxkScheduleTypeID is required.")]
	public byte sxkScheduleTypeID { get; set; }

	[JsonProperty("sxkScheduleTaskID", Order = 20)]
	[Required(ErrorMessage = "sxkScheduleTaskID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int sxkScheduleTaskID { get; set; }

	[JsonProperty("sxkStartActualDateTime", Order = 21)]
	public DateTime? sxkStartActualDateTime { get; set; }

	[JsonProperty("sxkStartDate", Order = 22)]
	public DateTime? sxkStartDate { get; set; }

	[JsonProperty("sxkStartMinute", Order = 23)]
	public short sxkStartMinute { get; set; }

	[JsonProperty("customFields", Order = 24)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
