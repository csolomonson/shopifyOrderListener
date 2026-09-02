using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPProcessDto
{
	[JsonProperty("xacProcessID", Order = 1)]
	[Required(ErrorMessage = "xacProcessID is required.")]
	[MaxLength(5)]
	public string xacProcessID { get; set; }

	[JsonProperty("xacCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string xacCreatedBy { get; set; }

	[JsonProperty("xacCreatedDate", Order = 3)]
	public DateTime? xacCreatedDate { get; set; }

	[JsonProperty("xacUniqueID", Order = 4)]
	public Guid xacUniqueID { get; set; }

	[JsonProperty("xacInactiveDate", Order = 5)]
	public DateTime? xacInactiveDate { get; set; }

	[JsonProperty("xacInspectionType", Order = 6)]
	public byte xacInspectionType { get; set; }

	[JsonProperty("xacInactive", Order = 7)]
	public bool xacInactive { get; set; }

	[JsonProperty("xacExcludeFromTMJobs", Order = 8)]
	public bool xacExcludeFromTMJobs { get; set; }

	[JsonProperty("xacIgnoreCalendarMove", Order = 9)]
	public bool xacIgnoreCalendarMove { get; set; }

	[JsonProperty("xacIgnoreCalendarQueue", Order = 10)]
	public bool xacIgnoreCalendarQueue { get; set; }

	[JsonProperty("xacPrintInspectionLine", Order = 11)]
	public bool xacPrintInspectionLine { get; set; }

	[JsonProperty("xacLongDescriptionRtf", Order = 12)]
	public string xacLongDescriptionRtf { get; set; }

	[JsonProperty("xacLongDescriptionText", Order = 13)]
	public string xacLongDescriptionText { get; set; }

	[JsonProperty("xacProductionStandard", Order = 14)]
	[Range(0.0, 999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xacProductionStandard { get; set; }

	[JsonProperty("xacProjectedProductionRate", Order = 15)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xacProjectedProductionRate { get; set; }

	[JsonProperty("xacProjectedSetupRate", Order = 16)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xacProjectedSetupRate { get; set; }

	[JsonProperty("xacRowVersion", Order = 17)]
	public byte[] xacRowVersion { get; set; }

	[JsonProperty("xacSetupHours", Order = 18)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xacSetupHours { get; set; }

	[JsonProperty("xacShortDescription", Order = 19)]
	[Required(ErrorMessage = "xacShortDescription is required.")]
	[MaxLength(50)]
	public string xacShortDescription { get; set; }

	[JsonProperty("xacStandardFactor", Order = 20)]
	[MaxLength(2)]
	public string xacStandardFactor { get; set; }

	[JsonProperty("customFields", Order = 21)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
