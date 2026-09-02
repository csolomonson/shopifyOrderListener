using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPLeadSalesPersonDto
{
	[JsonProperty("lojCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string lojCreatedBy { get; set; }

	[JsonProperty("lojCreatedDate", Order = 2)]
	public DateTime? lojCreatedDate { get; set; }

	[JsonProperty("lojUniqueID", Order = 3)]
	public Guid lojUniqueID { get; set; }

	[JsonProperty("lojLeadID", Order = 4)]
	[Required(ErrorMessage = "lojLeadID is required.")]
	[MaxLength(10)]
	public string lojLeadID { get; set; }

	[JsonProperty("lojPercent", Order = 5)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal lojPercent { get; set; }

	[JsonProperty("lojRowVersion", Order = 6)]
	public byte[] lojRowVersion { get; set; }

	[JsonProperty("lojSalesEmployeeID", Order = 7)]
	[Required(ErrorMessage = "lojSalesEmployeeID is required.")]
	[MaxLength(10)]
	public string lojSalesEmployeeID { get; set; }

	[JsonProperty("lojSequenceID", Order = 8)]
	[Required(ErrorMessage = "lojSequenceID is required.")]
	public short lojSequenceID { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
