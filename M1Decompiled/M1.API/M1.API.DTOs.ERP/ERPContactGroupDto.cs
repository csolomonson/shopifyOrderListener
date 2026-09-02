using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPContactGroupDto
{
	[JsonProperty("cmgContactGroupID", Order = 1)]
	[Required(ErrorMessage = "cmgContactGroupID is required.")]
	[MaxLength(5)]
	public string cmgContactGroupID { get; set; }

	[JsonProperty("cmgCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string cmgCreatedBy { get; set; }

	[JsonProperty("cmgCreatedDate", Order = 3)]
	public DateTime? cmgCreatedDate { get; set; }

	[JsonProperty("cmgDescription", Order = 4)]
	[Required(ErrorMessage = "cmgDescription is required.")]
	[MaxLength(50)]
	public string cmgDescription { get; set; }

	[JsonProperty("cmgUniqueID", Order = 5)]
	public Guid cmgUniqueID { get; set; }

	[JsonProperty("cmgRowVersion", Order = 6)]
	public byte[] cmgRowVersion { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
