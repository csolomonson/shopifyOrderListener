using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPInspectionLineApprovalDto
{
	[JsonProperty("qaaApprovalEmployeeID", Order = 1)]
	[MaxLength(10)]
	public string qaaApprovalEmployeeID { get; set; }

	[JsonProperty("qaaCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string qaaCreatedBy { get; set; }

	[JsonProperty("qaaCreatedDate", Order = 3)]
	public DateTime? qaaCreatedDate { get; set; }

	[JsonProperty("qaaDescription", Order = 4)]
	[MaxLength(50)]
	public string qaaDescription { get; set; }

	[JsonProperty("qaaUniqueID", Order = 5)]
	public Guid qaaUniqueID { get; set; }

	[JsonProperty("qaaInspectionID", Order = 6)]
	[Required(ErrorMessage = "qaaInspectionID is required.")]
	[MaxLength(10)]
	public string qaaInspectionID { get; set; }

	[JsonProperty("qaaInspectionLineID", Order = 7)]
	[Required(ErrorMessage = "qaaInspectionLineID is required.")]
	public short qaaInspectionLineID { get; set; }

	[JsonProperty("qaaInspectionLineApprovalID", Order = 8)]
	[Required(ErrorMessage = "qaaInspectionLineApprovalID is required.")]
	public byte qaaInspectionLineApprovalID { get; set; }

	[JsonProperty("qaaStatus", Order = 9)]
	public byte qaaStatus { get; set; }

	[JsonProperty("qaaStatusDate", Order = 10)]
	public DateTime? qaaStatusDate { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
