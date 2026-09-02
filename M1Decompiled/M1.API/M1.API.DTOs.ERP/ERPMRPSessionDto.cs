using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPMRPSessionDto
{
	[JsonProperty("mrpCompletedDate", Order = 1)]
	public DateTime? mrpCompletedDate { get; set; }

	[JsonProperty("mrpCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string mrpCreatedBy { get; set; }

	[JsonProperty("mrpCreatedDate", Order = 3)]
	public DateTime? mrpCreatedDate { get; set; }

	[JsonProperty("mrpCustomerIDs", Order = 4)]
	[MaxLength(4)]
	public string mrpCustomerIDs { get; set; }

	[JsonProperty("mrpCutoffDate", Order = 5)]
	[Required(ErrorMessage = "mrpCutoffDate is required.")]
	public DateTime? mrpCutoffDate { get; set; }

	[JsonProperty("mrpUniqueID", Order = 6)]
	public Guid mrpUniqueID { get; set; }

	[JsonProperty("mrpCompleted", Order = 7)]
	public bool mrpCompleted { get; set; }

	[JsonProperty("mrpConsolidatePartForecastJobs", Order = 8)]
	public bool mrpConsolidatePartForecastJobs { get; set; }

	[JsonProperty("mrpGenerated", Order = 9)]
	public bool mrpGenerated { get; set; }

	[JsonProperty("mrpIncludePartForecasts", Order = 10)]
	public bool mrpIncludePartForecasts { get; set; }

	[JsonProperty("mrpPartClassIDs", Order = 11)]
	[MaxLength(4)]
	public string mrpPartClassIDs { get; set; }

	[JsonProperty("mrpPartGroupIDs", Order = 12)]
	[MaxLength(4)]
	public string mrpPartGroupIDs { get; set; }

	[JsonProperty("mrpPartIDs", Order = 13)]
	[MaxLength(4)]
	public string mrpPartIDs { get; set; }

	[JsonProperty("mrpPlantIDs", Order = 14)]
	[MaxLength(5)]
	public string mrpPlantIDs { get; set; }

	[JsonProperty("mrpRowVersion", Order = 15)]
	public byte[] mrpRowVersion { get; set; }

	[JsonProperty("mrpSessionID", Order = 16)]
	[Required(ErrorMessage = "mrpSessionID is required.")]
	[MaxLength(10)]
	public string mrpSessionID { get; set; }

	[JsonProperty("mrpWarehouseIDs", Order = 17)]
	[MaxLength(5)]
	public string mrpWarehouseIDs { get; set; }

	[JsonProperty("customFields", Order = 18)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
