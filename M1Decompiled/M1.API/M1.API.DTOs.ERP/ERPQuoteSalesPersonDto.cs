using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPQuoteSalesPersonDto
{
	[JsonProperty("qmjCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string qmjCreatedBy { get; set; }

	[JsonProperty("qmjCreatedDate", Order = 2)]
	public DateTime? qmjCreatedDate { get; set; }

	[JsonProperty("qmjUniqueID", Order = 3)]
	public Guid qmjUniqueID { get; set; }

	[JsonProperty("qmjClosed", Order = 4)]
	public bool qmjClosed { get; set; }

	[JsonProperty("qmjCreatedFromMobile", Order = 5)]
	public bool qmjCreatedFromMobile { get; set; }

	[JsonProperty("qmjPercent", Order = 6)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qmjPercent { get; set; }

	[JsonProperty("qmjQuoteID", Order = 7)]
	[Required(ErrorMessage = "qmjQuoteID is required.")]
	[MaxLength(10)]
	public string qmjQuoteID { get; set; }

	[JsonProperty("qmjRowVersion", Order = 8)]
	public byte[] qmjRowVersion { get; set; }

	[JsonProperty("qmjSalesEmployeeID", Order = 9)]
	[Required(ErrorMessage = "qmjSalesEmployeeID is required.")]
	[MaxLength(10)]
	public string qmjSalesEmployeeID { get; set; }

	[JsonProperty("qmjSequenceID", Order = 10)]
	[Required(ErrorMessage = "qmjSequenceID is required.")]
	public short qmjSequenceID { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
