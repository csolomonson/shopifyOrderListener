using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPWorkCenterDto
{
	[JsonProperty("xawCalendarColor", Order = 1)]
	public byte xawCalendarColor { get; set; }

	[JsonProperty("xawCalendarLocation", Order = 2)]
	[MaxLength(50)]
	public string xawCalendarLocation { get; set; }

	[JsonProperty("xawWorkCenterID", Order = 3)]
	[Required(ErrorMessage = "xawWorkCenterID is required.")]
	[MaxLength(5)]
	public string xawWorkCenterID { get; set; }

	[JsonProperty("xawCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string xawCreatedBy { get; set; }

	[JsonProperty("xawCreatedDate", Order = 5)]
	public DateTime? xawCreatedDate { get; set; }

	[JsonProperty("xawDayStartTimeFri", Order = 6)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawDayStartTimeFri { get; set; }

	[JsonProperty("xawDayStartTimeMon", Order = 7)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawDayStartTimeMon { get; set; }

	[JsonProperty("xawDayStartTimeSat", Order = 8)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawDayStartTimeSat { get; set; }

	[JsonProperty("xawDayStartTimeSun", Order = 9)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawDayStartTimeSun { get; set; }

	[JsonProperty("xawDayStartTimeThu", Order = 10)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawDayStartTimeThu { get; set; }

	[JsonProperty("xawDayStartTimeTue", Order = 11)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawDayStartTimeTue { get; set; }

	[JsonProperty("xawDayStartTimeWed", Order = 12)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawDayStartTimeWed { get; set; }

	[JsonProperty("xawDescription", Order = 13)]
	[Required(ErrorMessage = "xawDescription is required.")]
	[MaxLength(50)]
	public string xawDescription { get; set; }

	[JsonProperty("xawUniqueID", Order = 14)]
	public Guid xawUniqueID { get; set; }

	[JsonProperty("xawFiniteTolerance", Order = 15)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawFiniteTolerance { get; set; }

	[JsonProperty("xawHoursFri", Order = 16)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawHoursFri { get; set; }

	[JsonProperty("xawHoursMon", Order = 17)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawHoursMon { get; set; }

	[JsonProperty("xawHoursSat", Order = 18)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawHoursSat { get; set; }

	[JsonProperty("xawHoursSun", Order = 19)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawHoursSun { get; set; }

	[JsonProperty("xawHoursThu", Order = 20)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawHoursThu { get; set; }

	[JsonProperty("xawHoursTue", Order = 21)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawHoursTue { get; set; }

	[JsonProperty("xawHoursWed", Order = 22)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawHoursWed { get; set; }

	[JsonProperty("xawInactiveDate", Order = 23)]
	public DateTime? xawInactiveDate { get; set; }

	[JsonProperty("xawInactive", Order = 24)]
	public bool xawInactive { get; set; }

	[JsonProperty("xawEnableCalendar", Order = 25)]
	public bool xawEnableCalendar { get; set; }

	[JsonProperty("xawExcludeFromShopLoad", Order = 26)]
	public bool xawExcludeFromShopLoad { get; set; }

	[JsonProperty("xawExportToCalendar", Order = 27)]
	public bool xawExportToCalendar { get; set; }

	[JsonProperty("xawInfiniteCapacity", Order = 28)]
	public bool xawInfiniteCapacity { get; set; }

	[JsonProperty("xawOutsideProcessing", Order = 29)]
	public bool xawOutsideProcessing { get; set; }

	[JsonProperty("xawSetMachineToLaborHours", Order = 30)]
	public bool xawSetMachineToLaborHours { get; set; }

	[JsonProperty("xawSplitMachineHours", Order = 31)]
	public bool xawSplitMachineHours { get; set; }

	[JsonProperty("xawMoveTime", Order = 32)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawMoveTime { get; set; }

	[JsonProperty("xawNumberOfMachines", Order = 33)]
	[Required(ErrorMessage = "xawNumberOfMachines is required.")]
	public short xawNumberOfMachines { get; set; }

	[JsonProperty("xawOverheadCalculationType", Order = 34)]
	[Required(ErrorMessage = "xawOverheadCalculationType is required.")]
	public byte xawOverheadCalculationType { get; set; }

	[JsonProperty("xawOverheadRate", Order = 35)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawOverheadRate { get; set; }

	[JsonProperty("xawPeoplePerMachineProd", Order = 36)]
	[Required(ErrorMessage = "xawPeoplePerMachineProd is required.")]
	public short xawPeoplePerMachineProd { get; set; }

	[JsonProperty("xawPeoplePerMachineSetup", Order = 37)]
	[Required(ErrorMessage = "xawPeoplePerMachineSetup is required.")]
	public short xawPeoplePerMachineSetup { get; set; }

	[JsonProperty("xawPlantID", Order = 38)]
	[MaxLength(5)]
	public string xawPlantID { get; set; }

	[JsonProperty("xawProcessID", Order = 39)]
	[MaxLength(5)]
	public string xawProcessID { get; set; }

	[JsonProperty("xawProductionDepartmentID", Order = 40)]
	[Required(ErrorMessage = "xawProductionDepartmentID is required.")]
	[MaxLength(5)]
	public string xawProductionDepartmentID { get; set; }

	[JsonProperty("xawProductionStandard", Order = 41)]
	[Range(0.0, 999999.9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawProductionStandard { get; set; }

	[JsonProperty("xawQueueTime", Order = 42)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawQueueTime { get; set; }

	[JsonProperty("xawQuotingRate", Order = 43)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawQuotingRate { get; set; }

	[JsonProperty("xawRowVersion", Order = 44)]
	public byte[] xawRowVersion { get; set; }

	[JsonProperty("xawSetupHours", Order = 45)]
	[Range(0.0, 999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawSetupHours { get; set; }

	[JsonProperty("xawStandardFactor", Order = 46)]
	[MaxLength(2)]
	public string xawStandardFactor { get; set; }

	[JsonProperty("xawStartHour", Order = 47)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xawStartHour { get; set; }

	[JsonProperty("customFields", Order = 48)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
