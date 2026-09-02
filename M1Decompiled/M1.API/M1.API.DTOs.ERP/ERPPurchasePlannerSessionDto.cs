using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPurchasePlannerSessionDto
{
	[JsonProperty("ppsBuyerEmployeeID", Order = 1)]
	[Required(ErrorMessage = "ppsBuyerEmployeeID is required.")]
	[MaxLength(10)]
	public string ppsBuyerEmployeeID { get; set; }

	[JsonProperty("ppsCompletedDate", Order = 2)]
	public DateTime? ppsCompletedDate { get; set; }

	[JsonProperty("ppsCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string ppsCreatedBy { get; set; }

	[JsonProperty("ppsCreatedDate", Order = 4)]
	public DateTime? ppsCreatedDate { get; set; }

	[JsonProperty("ppsCutoffDate", Order = 5)]
	[Required(ErrorMessage = "ppsCutoffDate is required.")]
	public DateTime? ppsCutoffDate { get; set; }

	[JsonProperty("ppsCutoffDatePosupply", Order = 6)]
	public DateTime? ppsCutoffDatePosupply { get; set; }

	[JsonProperty("ppsUniqueID", Order = 7)]
	public Guid ppsUniqueID { get; set; }

	[JsonProperty("ppsCalculateForAllParts", Order = 8)]
	public bool ppsCalculateForAllParts { get; set; }

	[JsonProperty("ppsCompleted", Order = 9)]
	public bool ppsCompleted { get; set; }

	[JsonProperty("ppsFirmOnly", Order = 10)]
	public bool ppsFirmOnly { get; set; }

	[JsonProperty("ppsGenerated", Order = 11)]
	public bool ppsGenerated { get; set; }

	[JsonProperty("ppsJobIDs", Order = 12)]
	[MaxLength(4)]
	public string ppsJobIDs { get; set; }

	[JsonProperty("ppsPartClassIDs", Order = 13)]
	[MaxLength(4)]
	public string ppsPartClassIDs { get; set; }

	[JsonProperty("ppsPartIDs", Order = 14)]
	[MaxLength(4)]
	public string ppsPartIDs { get; set; }

	[JsonProperty("ppsPlantID", Order = 15)]
	[MaxLength(5)]
	public string ppsPlantID { get; set; }

	[JsonProperty("ppsRowVersion", Order = 16)]
	public byte[] ppsRowVersion { get; set; }

	[JsonProperty("ppsSalesOrderIDs", Order = 17)]
	[MaxLength(4)]
	public string ppsSalesOrderIDs { get; set; }

	[JsonProperty("ppsSessionID", Order = 18)]
	[Required(ErrorMessage = "ppsSessionID is required.")]
	[MaxLength(10)]
	public string ppsSessionID { get; set; }

	[JsonProperty("ppsSessionSubtotalBase", Order = 19)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal ppsSessionSubtotalBase { get; set; }

	[JsonProperty("ppsShowAllDemandForPartsOnJobs", Order = 20)]
	public bool ppsShowAllDemandForPartsOnJobs { get; set; }

	[JsonProperty("ppsSupplierIDs", Order = 21)]
	[MaxLength(4)]
	public string ppsSupplierIDs { get; set; }

	[JsonProperty("ppsWarehouseID", Order = 22)]
	[MaxLength(5)]
	public string ppsWarehouseID { get; set; }

	[JsonProperty("customFields", Order = 23)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
