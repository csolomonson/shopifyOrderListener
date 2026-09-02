using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPOrganizationLocSalesPersonDto
{
	[JsonProperty("cmkCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string cmkCreatedBy { get; set; }

	[JsonProperty("cmkCreatedDate", Order = 2)]
	public DateTime? cmkCreatedDate { get; set; }

	[JsonProperty("cmkUniqueID", Order = 3)]
	public Guid cmkUniqueID { get; set; }

	[JsonProperty("cmkLocationID", Order = 4)]
	[MaxLength(5)]
	public string cmkLocationID { get; set; }

	[JsonProperty("cmkOrganizationID", Order = 5)]
	[Required(ErrorMessage = "cmkOrganizationID is required.")]
	[MaxLength(10)]
	public string cmkOrganizationID { get; set; }

	[JsonProperty("cmkPercent", Order = 6)]
	[Range(0.0, 9999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal cmkPercent { get; set; }

	[JsonProperty("cmkRowVersion", Order = 7)]
	public byte[] cmkRowVersion { get; set; }

	[JsonProperty("cmkSalesEmployeeID", Order = 8)]
	[Required(ErrorMessage = "cmkSalesEmployeeID is required.")]
	[MaxLength(10)]
	public string cmkSalesEmployeeID { get; set; }

	[JsonProperty("cmkSequenceID", Order = 9)]
	[Required(ErrorMessage = "cmkSequenceID is required.")]
	public short cmkSequenceID { get; set; }

	[JsonProperty("customFields", Order = 10)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
