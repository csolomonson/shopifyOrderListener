using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPToolCategoryDto
{
	[JsonProperty("xtcToolCategoryID", Order = 1)]
	[Required(ErrorMessage = "xtcToolCategoryID is required.")]
	[MaxLength(10)]
	public string xtcToolCategoryID { get; set; }

	[JsonProperty("xtcCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string xtcCreatedBy { get; set; }

	[JsonProperty("xtcCreatedDate", Order = 3)]
	public DateTime? xtcCreatedDate { get; set; }

	[JsonProperty("xtcDescription", Order = 4)]
	[Required(ErrorMessage = "xtcDescription is required.")]
	[MaxLength(50)]
	public string xtcDescription { get; set; }

	[JsonProperty("xtcUniqueID", Order = 5)]
	public Guid xtcUniqueID { get; set; }

	[JsonProperty("xtcInactiveDate", Order = 6)]
	public DateTime? xtcInactiveDate { get; set; }

	[JsonProperty("xtcInactive", Order = 7)]
	public bool xtcInactive { get; set; }

	[JsonProperty("xtcRowVersion", Order = 8)]
	public byte[] xtcRowVersion { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
