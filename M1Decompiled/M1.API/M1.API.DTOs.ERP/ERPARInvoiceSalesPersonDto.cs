using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPARInvoiceSalesPersonDto
{
	[JsonProperty("arjAmount", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arjAmount { get; set; }

	[JsonProperty("arjArInvoiceID", Order = 2)]
	[Required(ErrorMessage = "arjArInvoiceID is required.")]
	[MaxLength(10)]
	public string arjArInvoiceID { get; set; }

	[JsonProperty("arjCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string arjCreatedBy { get; set; }

	[JsonProperty("arjCreatedDate", Order = 4)]
	public DateTime? arjCreatedDate { get; set; }

	[JsonProperty("arjUniqueID", Order = 5)]
	public Guid arjUniqueID { get; set; }

	[JsonProperty("arjPostedToGl", Order = 6)]
	public bool arjPostedToGl { get; set; }

	[JsonProperty("arjPercent", Order = 7)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arjPercent { get; set; }

	[JsonProperty("arjRate", Order = 8)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal arjRate { get; set; }

	[JsonProperty("arjRowVersion", Order = 9)]
	public byte[] arjRowVersion { get; set; }

	[JsonProperty("arjSalesEmployeeID", Order = 10)]
	[Required(ErrorMessage = "arjSalesEmployeeID is required.")]
	[MaxLength(10)]
	public string arjSalesEmployeeID { get; set; }

	[JsonProperty("arjSequenceID", Order = 11)]
	[Required(ErrorMessage = "arjSequenceID is required.")]
	public short arjSequenceID { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
