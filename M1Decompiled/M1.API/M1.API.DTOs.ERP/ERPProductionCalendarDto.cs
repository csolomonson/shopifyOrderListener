using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPProductionCalendarDto
{
	[JsonProperty("jmlCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string jmlCreatedBy { get; set; }

	[JsonProperty("jmlCreatedDate", Order = 2)]
	public DateTime? jmlCreatedDate { get; set; }

	[JsonProperty("jmlUniqueID", Order = 3)]
	public Guid jmlUniqueID { get; set; }

	[JsonProperty("jmlPlantID", Order = 4)]
	[MaxLength(5)]
	public string jmlPlantID { get; set; }

	[JsonProperty("jmlProductionCalendarYearID", Order = 5)]
	[Required(ErrorMessage = "jmlProductionCalendarYearID is required.")]
	public short jmlProductionCalendarYearID { get; set; }

	[JsonProperty("jmlRowVersion", Order = 6)]
	public byte[] jmlRowVersion { get; set; }

	[JsonProperty("jmlWorkCenterID", Order = 7)]
	[MaxLength(5)]
	public string jmlWorkCenterID { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
