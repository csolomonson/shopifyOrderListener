using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartForecastDto
{
	[JsonProperty("inpAnnualQuantity", Order = 1)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal inpAnnualQuantity { get; set; }

	[JsonProperty("inpCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string inpCreatedBy { get; set; }

	[JsonProperty("inpCreatedDate", Order = 3)]
	public DateTime? inpCreatedDate { get; set; }

	[JsonProperty("inpEndDate", Order = 4)]
	public DateTime? inpEndDate { get; set; }

	[JsonProperty("inpUniqueID", Order = 5)]
	public Guid inpUniqueID { get; set; }

	[JsonProperty("inpForecastMethod", Order = 6)]
	[MaxLength(1)]
	public string inpForecastMethod { get; set; }

	[JsonProperty("inpForecastNumberOfYears", Order = 7)]
	public byte inpForecastNumberOfYears { get; set; }

	[JsonProperty("inpIntervalType", Order = 8)]
	[Required(ErrorMessage = "inpIntervalType is required.")]
	[MaxLength(1)]
	public string inpIntervalType { get; set; }

	[JsonProperty("inpPartForecastYearID", Order = 9)]
	[Required(ErrorMessage = "inpPartForecastYearID is required.")]
	public short inpPartForecastYearID { get; set; }

	[JsonProperty("inpPartID", Order = 10)]
	[Required(ErrorMessage = "inpPartID is required.")]
	[MaxLength(30)]
	public string inpPartID { get; set; }

	[JsonProperty("inpPartRevisionID", Order = 11)]
	[MaxLength(15)]
	public string inpPartRevisionID { get; set; }

	[JsonProperty("inpRowVersion", Order = 12)]
	public byte[] inpRowVersion { get; set; }

	[JsonProperty("inpStartDate", Order = 13)]
	public DateTime? inpStartDate { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
