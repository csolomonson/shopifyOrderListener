using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPProductionCalendarDayDto
{
	[JsonProperty("jmyDayOfWeek", Order = 1)]
	public byte jmyDayOfWeek { get; set; }

	[JsonProperty("jmyDayStartTime", Order = 2)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmyDayStartTime { get; set; }

	[JsonProperty("jmyHours", Order = 3)]
	[Range(0.0, 999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal jmyHours { get; set; }

	[JsonProperty("jmyHoliday", Order = 4)]
	public bool jmyHoliday { get; set; }

	[JsonProperty("jmyPlantID", Order = 5)]
	[MaxLength(5)]
	public string jmyPlantID { get; set; }

	[JsonProperty("jmyProductionCalendarDay", Order = 6)]
	public byte jmyProductionCalendarDay { get; set; }

	[JsonProperty("jmyProductionCalendarMonth", Order = 7)]
	public byte jmyProductionCalendarMonth { get; set; }

	[JsonProperty("jmyProductionCalendarYearID", Order = 8)]
	public short jmyProductionCalendarYearID { get; set; }

	[JsonProperty("jmyRowVersion", Order = 9)]
	public byte[] jmyRowVersion { get; set; }

	[JsonProperty("jmyWorkCenterID", Order = 10)]
	[MaxLength(5)]
	public string jmyWorkCenterID { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
