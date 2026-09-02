using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPCountyCodeDto
{
	[JsonProperty("xccCountyCodeID", Order = 1)]
	[Required(ErrorMessage = "xccCountyCodeID is required.")]
	[MaxLength(5)]
	public string xccCountyCodeID { get; set; }

	[JsonProperty("xccCounty", Order = 2)]
	[MaxLength(30)]
	public string xccCounty { get; set; }

	[JsonProperty("xccCountyCode", Order = 3)]
	[MaxLength(3)]
	public string xccCountyCode { get; set; }

	[JsonProperty("xccCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string xccCreatedBy { get; set; }

	[JsonProperty("xccCreatedDate", Order = 5)]
	public DateTime? xccCreatedDate { get; set; }

	[JsonProperty("xccUniqueID", Order = 6)]
	public Guid xccUniqueID { get; set; }

	[JsonProperty("XCCRowVersion", Order = 7)]
	public byte[] xccRowVersion { get; set; }

	[JsonProperty("xccStateCode", Order = 8)]
	[MaxLength(2)]
	public string xccStateCode { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
