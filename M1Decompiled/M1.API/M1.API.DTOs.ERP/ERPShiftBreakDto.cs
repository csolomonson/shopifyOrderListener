using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPShiftBreakDto
{
	[JsonProperty("lmtBreak1EndTime", Order = 1)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmtBreak1EndTime { get; set; }

	[JsonProperty("lmtBreak1StartTime", Order = 2)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmtBreak1StartTime { get; set; }

	[JsonProperty("lmtBreak2EndTime", Order = 3)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmtBreak2EndTime { get; set; }

	[JsonProperty("lmtBreak2StartTime", Order = 4)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmtBreak2StartTime { get; set; }

	[JsonProperty("lmtBreak3EndTime", Order = 5)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmtBreak3EndTime { get; set; }

	[JsonProperty("lmtBreak3StartTime", Order = 6)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmtBreak3StartTime { get; set; }

	[JsonProperty("lmtCreatedBy", Order = 7)]
	[MaxLength(20)]
	public string lmtCreatedBy { get; set; }

	[JsonProperty("lmtCreatedDate", Order = 8)]
	public DateTime? lmtCreatedDate { get; set; }

	[JsonProperty("lmtDay", Order = 9)]
	[Required(ErrorMessage = "lmtDay is required.")]
	public byte lmtDay { get; set; }

	[JsonProperty("lmtEndTime", Order = 10)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmtEndTime { get; set; }

	[JsonProperty("lmtUniqueID", Order = 11)]
	public Guid lmtUniqueID { get; set; }

	[JsonProperty("lmtBreak1Paid", Order = 12)]
	public bool lmtBreak1Paid { get; set; }

	[JsonProperty("lmtBreak2Paid", Order = 13)]
	public bool lmtBreak2Paid { get; set; }

	[JsonProperty("lmtBreak3Paid", Order = 14)]
	public bool lmtBreak3Paid { get; set; }

	[JsonProperty("lmtRowVersion", Order = 15)]
	public byte[] lmtRowVersion { get; set; }

	[JsonProperty("lmtShiftID", Order = 16)]
	[Required(ErrorMessage = "lmtShiftID is required.")]
	public short lmtShiftID { get; set; }

	[JsonProperty("lmtStartTime", Order = 17)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lmtStartTime { get; set; }

	[JsonProperty("customFields", Order = 18)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
