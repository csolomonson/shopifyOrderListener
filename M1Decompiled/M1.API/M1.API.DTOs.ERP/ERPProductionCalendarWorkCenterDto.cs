using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPProductionCalendarWorkCenterDto
{
	[JsonProperty("jmrCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string jmrCreatedBy { get; set; }

	[JsonProperty("jmrCreatedDate", Order = 2)]
	public DateTime? jmrCreatedDate { get; set; }

	[JsonProperty("jmrUniqueID", Order = 3)]
	public Guid jmrUniqueID { get; set; }

	[JsonProperty("jmrProductionCalendarLineID", Order = 4)]
	[Required(ErrorMessage = "jmrProductionCalendarLineID is required.")]
	public short jmrProductionCalendarLineID { get; set; }

	[JsonProperty("jmrProductionCalendarYearID", Order = 5)]
	[Required(ErrorMessage = "jmrProductionCalendarYearID is required.")]
	public short jmrProductionCalendarYearID { get; set; }

	[JsonProperty("jmrRowVersion", Order = 6)]
	public byte[] jmrRowVersion { get; set; }

	[JsonProperty("jmrWorkCenterID", Order = 7)]
	[Required(ErrorMessage = "jmrWorkCenterID is required.")]
	[MaxLength(5)]
	public string jmrWorkCenterID { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
