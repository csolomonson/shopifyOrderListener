using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPRFQDto
{
	[JsonProperty("rqpBuyerEmployeeID", Order = 1)]
	[MaxLength(10)]
	public string rqpBuyerEmployeeID { get; set; }

	[JsonProperty("rqpClosedDate", Order = 2)]
	public DateTime? rqpClosedDate { get; set; }

	[JsonProperty("rqpRfqID", Order = 3)]
	[Required(ErrorMessage = "rqpRfqID is required.")]
	[MaxLength(10)]
	public string rqpRfqID { get; set; }

	[JsonProperty("rqpCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string rqpCreatedBy { get; set; }

	[JsonProperty("rqpCreatedDate", Order = 5)]
	public DateTime? rqpCreatedDate { get; set; }

	[JsonProperty("rqpDueDate", Order = 6)]
	public DateTime? rqpDueDate { get; set; }

	[JsonProperty("rqpUniqueID", Order = 7)]
	public Guid rqpUniqueID { get; set; }

	[JsonProperty("rqpClosed", Order = 8)]
	public bool rqpClosed { get; set; }

	[JsonProperty("rqpReadyToPrint", Order = 9)]
	public bool rqpReadyToPrint { get; set; }

	[JsonProperty("rqpPlantDepartmentID", Order = 10)]
	[MaxLength(5)]
	public string rqpPlantDepartmentID { get; set; }

	[JsonProperty("rqpPlantID", Order = 11)]
	[MaxLength(5)]
	public string rqpPlantID { get; set; }

	[JsonProperty("rqpRfqDate", Order = 12)]
	public DateTime? rqpRfqDate { get; set; }

	[JsonProperty("rqpRowVersion", Order = 13)]
	public byte[] rqpRowVersion { get; set; }

	[JsonProperty("rqpStandardMessageID", Order = 14)]
	[MaxLength(10)]
	public string rqpStandardMessageID { get; set; }

	[JsonProperty("customFields", Order = 15)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
