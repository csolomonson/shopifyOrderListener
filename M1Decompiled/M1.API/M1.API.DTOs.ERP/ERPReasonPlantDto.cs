using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPReasonPlantDto
{
	[JsonProperty("xajReasonPlantID", Order = 1)]
	[Required(ErrorMessage = "xajReasonPlantID is required.")]
	[MaxLength(5)]
	public string xajReasonPlantID { get; set; }

	[JsonProperty("xajCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string xajCreatedBy { get; set; }

	[JsonProperty("xajCreatedDate", Order = 3)]
	public DateTime? xajCreatedDate { get; set; }

	[JsonProperty("xajUniqueID", Order = 4)]
	public Guid xajUniqueID { get; set; }

	[JsonProperty("xajReasonGlAccountID", Order = 5)]
	[MaxLength(11)]
	public string xajReasonGlAccountID { get; set; }

	[JsonProperty("xajReasonID", Order = 6)]
	[Required(ErrorMessage = "xajReasonID is required.")]
	[MaxLength(5)]
	public string xajReasonID { get; set; }

	[JsonProperty("xajRowVersion", Order = 7)]
	public byte[] xajRowVersion { get; set; }

	[JsonProperty("xajScrapGlAccountID", Order = 8)]
	[MaxLength(11)]
	public string xajScrapGlAccountID { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
