using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPReasonDto
{
	[JsonProperty("xarReasonID", Order = 1)]
	[Required(ErrorMessage = "xarReasonID is required.")]
	[MaxLength(5)]
	public string xarReasonID { get; set; }

	[JsonProperty("xarCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string xarCreatedBy { get; set; }

	[JsonProperty("xarCreatedDate", Order = 3)]
	public DateTime? xarCreatedDate { get; set; }

	[JsonProperty("xarDescription", Order = 4)]
	[Required(ErrorMessage = "xarDescription is required.")]
	[MaxLength(50)]
	public string xarDescription { get; set; }

	[JsonProperty("xarUniqueID", Order = 5)]
	public Guid xarUniqueID { get; set; }

	[JsonProperty("xarReasonGlAccountID", Order = 6)]
	[MaxLength(11)]
	public string xarReasonGlAccountID { get; set; }

	[JsonProperty("xarReasonType", Order = 7)]
	[Required(ErrorMessage = "xarReasonType is required.")]
	[MaxLength(1)]
	public string xarReasonType { get; set; }

	[JsonProperty("xarRowVersion", Order = 8)]
	public byte[] xarRowVersion { get; set; }

	[JsonProperty("xarScrapGlAccountID", Order = 9)]
	[MaxLength(11)]
	public string xarScrapGlAccountID { get; set; }

	[JsonProperty("customFields", Order = 10)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
