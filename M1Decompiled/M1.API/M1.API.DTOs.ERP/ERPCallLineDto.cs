using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPCallLineDto
{
	[JsonProperty("kblAddedByEmployeeID", Order = 1)]
	[Required(ErrorMessage = "kblAddedByEmployeeID is required.")]
	[MaxLength(10)]
	public string kblAddedByEmployeeID { get; set; }

	[JsonProperty("kblAddedDate", Order = 2)]
	[Required(ErrorMessage = "kblAddedDate is required.")]
	public DateTime? kblAddedDate { get; set; }

	[JsonProperty("kblCallID", Order = 3)]
	[Required(ErrorMessage = "kblCallID is required.")]
	[MaxLength(10)]
	public string kblCallID { get; set; }

	[JsonProperty("kblContactMethodID", Order = 4)]
	[Required(ErrorMessage = "kblContactMethodID is required.")]
	[MaxLength(5)]
	public string kblContactMethodID { get; set; }

	[JsonProperty("kblCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string kblCreatedBy { get; set; }

	[JsonProperty("kblCreatedDate", Order = 6)]
	public DateTime? kblCreatedDate { get; set; }

	[JsonProperty("kblUniqueID", Order = 7)]
	public Guid kblUniqueID { get; set; }

	[JsonProperty("kblExtraTime", Order = 8)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal kblExtraTime { get; set; }

	[JsonProperty("kblBillable", Order = 9)]
	public bool kblBillable { get; set; }

	[JsonProperty("kblCreatedFromMobile", Order = 10)]
	public bool kblCreatedFromMobile { get; set; }

	[JsonProperty("kblInbound", Order = 11)]
	public bool kblInbound { get; set; }

	[JsonProperty("kblInternalOnly", Order = 12)]
	public bool kblInternalOnly { get; set; }

	[JsonProperty("kblLongDescriptionRtf", Order = 13)]
	public string kblLongDescriptionRtf { get; set; }

	[JsonProperty("kblLongDescriptionText", Order = 14)]
	public string kblLongDescriptionText { get; set; }

	[JsonProperty("kblRowVersion", Order = 15)]
	public byte[] kblRowVersion { get; set; }

	[JsonProperty("kblCallLineID", Order = 16)]
	[Required(ErrorMessage = "kblCallLineID is required.")]
	public short kblCallLineID { get; set; }

	[JsonProperty("kblShortDescription", Order = 17)]
	[Required(ErrorMessage = "kblShortDescription is required.")]
	[MaxLength(70)]
	public string kblShortDescription { get; set; }

	[JsonProperty("kblTimeSpent", Order = 18)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal kblTimeSpent { get; set; }

	[JsonProperty("kblTotalTime", Order = 19)]
	[Range(0.0, 99999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal kblTotalTime { get; set; }

	[JsonProperty("customFields", Order = 20)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
