using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPIndustryTypeDto
{
	[JsonProperty("cmiIndustryTypeID", Order = 1)]
	[Required(ErrorMessage = "cmiIndustryTypeID is required.")]
	[MaxLength(10)]
	public string cmiIndustryTypeID { get; set; }

	[JsonProperty("cmiCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string cmiCreatedBy { get; set; }

	[JsonProperty("cmiCreatedDate", Order = 3)]
	public DateTime? cmiCreatedDate { get; set; }

	[JsonProperty("cmiUniqueID", Order = 4)]
	public Guid cmiUniqueID { get; set; }

	[JsonProperty("cmiLongDescriptionRtf", Order = 5)]
	public string cmiLongDescriptionRtf { get; set; }

	[JsonProperty("cmiLongDescriptionText", Order = 6)]
	public string cmiLongDescriptionText { get; set; }

	[JsonProperty("cmiRowVersion", Order = 7)]
	public byte[] cmiRowVersion { get; set; }

	[JsonProperty("cmiShortDescription", Order = 8)]
	[Required(ErrorMessage = "cmiShortDescription is required.")]
	[MaxLength(50)]
	public string cmiShortDescription { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
