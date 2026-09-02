using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSalesOrderApprovalDto
{
	[JsonProperty("omaApprovalEmployeeID", Order = 1)]
	[MaxLength(10)]
	public string omaApprovalEmployeeID { get; set; }

	[JsonProperty("omaCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string omaCreatedBy { get; set; }

	[JsonProperty("omaCreatedDate", Order = 3)]
	public DateTime? omaCreatedDate { get; set; }

	[JsonProperty("omaDescription", Order = 4)]
	[MaxLength(50)]
	public string omaDescription { get; set; }

	[JsonProperty("omaUniqueID", Order = 5)]
	public Guid omaUniqueID { get; set; }

	[JsonProperty("omaRowVersion", Order = 6)]
	public byte[] omaRowVersion { get; set; }

	[JsonProperty("omaSalesOrderID", Order = 7)]
	[Required(ErrorMessage = "omaSalesOrderID is required.")]
	[MaxLength(10)]
	public string omaSalesOrderID { get; set; }

	[JsonProperty("omaSalesOrderApprovalID", Order = 8)]
	[Required(ErrorMessage = "omaSalesOrderApprovalID is required.")]
	public byte omaSalesOrderApprovalID { get; set; }

	[JsonProperty("omaStatus", Order = 9)]
	public byte omaStatus { get; set; }

	[JsonProperty("omaStatusDate", Order = 10)]
	public DateTime? omaStatusDate { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
