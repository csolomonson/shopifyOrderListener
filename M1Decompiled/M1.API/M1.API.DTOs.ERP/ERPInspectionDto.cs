using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPInspectionDto
{
	[JsonProperty("qapInspectionID", Order = 1)]
	[Required(ErrorMessage = "qapInspectionID is required.")]
	[MaxLength(10)]
	public string qapInspectionID { get; set; }

	[JsonProperty("qapCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string qapCreatedBy { get; set; }

	[JsonProperty("qapCreatedDate", Order = 3)]
	public DateTime? qapCreatedDate { get; set; }

	[JsonProperty("qapUniqueID", Order = 4)]
	public Guid qapUniqueID { get; set; }

	[JsonProperty("qapPosted", Order = 5)]
	public bool qapPosted { get; set; }

	[JsonProperty("qapReversalEntry", Order = 6)]
	public bool qapReversalEntry { get; set; }

	[JsonProperty("qapOpenedByEmployeeID", Order = 7)]
	[MaxLength(10)]
	public string qapOpenedByEmployeeID { get; set; }

	[JsonProperty("qapOpenedDate", Order = 8)]
	public DateTime? qapOpenedDate { get; set; }

	[JsonProperty("qapPlantDepartmentID", Order = 9)]
	[MaxLength(5)]
	public string qapPlantDepartmentID { get; set; }

	[JsonProperty("qapPlantID", Order = 10)]
	[MaxLength(5)]
	public string qapPlantID { get; set; }

	[JsonProperty("qapPostedDate", Order = 11)]
	public DateTime? qapPostedDate { get; set; }

	[JsonProperty("qapProjectID", Order = 12)]
	[MaxLength(10)]
	public string qapProjectID { get; set; }

	[JsonProperty("qapRowVersion", Order = 13)]
	public byte[] qapRowVersion { get; set; }

	[JsonProperty("qapSourceTableName", Order = 14)]
	[MaxLength(30)]
	public string qapSourceTableName { get; set; }

	[JsonProperty("qapSourceTableUniqueID", Order = 15)]
	public Guid qapSourceTableUniqueID { get; set; }

	[JsonProperty("customFields", Order = 16)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
