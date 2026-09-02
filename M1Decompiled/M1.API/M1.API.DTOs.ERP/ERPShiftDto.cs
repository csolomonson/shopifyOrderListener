using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPShiftDto
{
	[JsonProperty("lmsAutoClockOutLastRunTime", Order = 1)]
	public DateTime? lmsAutoClockOutLastRunTime { get; set; }

	[JsonProperty("lmsAutoClockOutTime", Order = 2)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmsAutoClockOutTime { get; set; }

	[JsonProperty("lmsClockInWindow", Order = 3)]
	public short lmsClockInWindow { get; set; }

	[JsonProperty("lmsClockOutWindow", Order = 4)]
	public short lmsClockOutWindow { get; set; }

	[JsonProperty("lmsCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string lmsCreatedBy { get; set; }

	[JsonProperty("lmsCreatedDate", Order = 6)]
	public DateTime? lmsCreatedDate { get; set; }

	[JsonProperty("lmsDescription", Order = 7)]
	[MaxLength(50)]
	public string lmsDescription { get; set; }

	[JsonProperty("lmsUniqueID", Order = 8)]
	public Guid lmsUniqueID { get; set; }

	[JsonProperty("lmsGraceTimeIn", Order = 9)]
	public short lmsGraceTimeIn { get; set; }

	[JsonProperty("lmsGraceTimeOut", Order = 10)]
	public short lmsGraceTimeOut { get; set; }

	[JsonProperty("lmsIdleTimeIndirectLaborID", Order = 11)]
	[MaxLength(5)]
	public string lmsIdleTimeIndirectLaborID { get; set; }

	[JsonProperty("lmsIdleTimeWorkCenterID", Order = 12)]
	[MaxLength(5)]
	public string lmsIdleTimeWorkCenterID { get; set; }

	[JsonProperty("lmsInactiveDate", Order = 13)]
	public DateTime? lmsInactiveDate { get; set; }

	[JsonProperty("lmsInactive", Order = 14)]
	public bool lmsInactive { get; set; }

	[JsonProperty("lmsRoundClockWithInShift", Order = 15)]
	public bool lmsRoundClockWithInShift { get; set; }

	[JsonProperty("lmsRoundJobsOutsideOfShift", Order = 16)]
	public bool lmsRoundJobsOutsideOfShift { get; set; }

	[JsonProperty("lmsRoundJobsWithinShift", Order = 17)]
	public bool lmsRoundJobsWithinShift { get; set; }

	[JsonProperty("lmsRoundOutsideOfShift", Order = 18)]
	public bool lmsRoundOutsideOfShift { get; set; }

	[JsonProperty("lmsPlantID", Order = 19)]
	[MaxLength(5)]
	public string lmsPlantID { get; set; }

	[JsonProperty("lmsRoundClockInDirection", Order = 20)]
	[MaxLength(1)]
	public string lmsRoundClockInDirection { get; set; }

	[JsonProperty("lmsRoundClockOutDirection", Order = 21)]
	[MaxLength(1)]
	public string lmsRoundClockOutDirection { get; set; }

	[JsonProperty("lmsRoundTo", Order = 22)]
	public byte lmsRoundTo { get; set; }

	[JsonProperty("lmsRowVersion", Order = 23)]
	public byte[] lmsRowVersion { get; set; }

	[JsonProperty("lmsShiftID", Order = 24)]
	[Required(ErrorMessage = "lmsShiftID is required.")]
	public short lmsShiftID { get; set; }

	[JsonProperty("lmsShiftGroup", Order = 25)]
	[Required(ErrorMessage = "lmsShiftGroup is required.")]
	public byte lmsShiftGroup { get; set; }

	[JsonProperty("customFields", Order = 26)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
