using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSalesOrderPickListSessionDto
{
	[JsonProperty("omsCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string omsCreatedBy { get; set; }

	[JsonProperty("omsCreatedDate", Order = 2)]
	public DateTime? omsCreatedDate { get; set; }

	[JsonProperty("omsDevice", Order = 3)]
	public byte omsDevice { get; set; }

	[JsonProperty("omsUniqueID", Order = 4)]
	public Guid omsUniqueID { get; set; }

	[JsonProperty("omsPullFromStockOnly", Order = 5)]
	public bool omsPullFromStockOnly { get; set; }

	[JsonProperty("omsPickListSessionID", Order = 6)]
	[Required(ErrorMessage = "omsPickListSessionID is required.")]
	public int omsPickListSessionID { get; set; }

	[JsonProperty("omsPlantDepartmentID", Order = 7)]
	[MaxLength(5)]
	public string omsPlantDepartmentID { get; set; }

	[JsonProperty("omsPlantID", Order = 8)]
	[MaxLength(5)]
	public string omsPlantID { get; set; }

	[JsonProperty("omsPostedDate", Order = 9)]
	public DateTime? omsPostedDate { get; set; }

	[JsonProperty("omsRowVersion", Order = 10)]
	public byte[] omsRowVersion { get; set; }

	[JsonProperty("omsSessionDate", Order = 11)]
	[Required(ErrorMessage = "omsSessionDate is required.")]
	public DateTime? omsSessionDate { get; set; }

	[JsonProperty("omsStatus", Order = 12)]
	public byte omsStatus { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
