using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSalesOrderSalesPersonDto
{
	[JsonProperty("omiCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string omiCreatedBy { get; set; }

	[JsonProperty("omiCreatedDate", Order = 2)]
	public DateTime? omiCreatedDate { get; set; }

	[JsonProperty("omiUniqueID", Order = 3)]
	public Guid omiUniqueID { get; set; }

	[JsonProperty("omiClosed", Order = 4)]
	public bool omiClosed { get; set; }

	[JsonProperty("omiPercent", Order = 5)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal omiPercent { get; set; }

	[JsonProperty("omiRowVersion", Order = 6)]
	public byte[] omiRowVersion { get; set; }

	[JsonProperty("omiSalesEmployeeID", Order = 7)]
	[Required(ErrorMessage = "omiSalesEmployeeID is required.")]
	[MaxLength(10)]
	public string omiSalesEmployeeID { get; set; }

	[JsonProperty("omiSalesOrderID", Order = 8)]
	[Required(ErrorMessage = "omiSalesOrderID is required.")]
	[MaxLength(10)]
	public string omiSalesOrderID { get; set; }

	[JsonProperty("omiSequenceID", Order = 9)]
	[Required(ErrorMessage = "omiSequenceID is required.")]
	public short omiSequenceID { get; set; }

	[JsonProperty("customFields", Order = 10)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
