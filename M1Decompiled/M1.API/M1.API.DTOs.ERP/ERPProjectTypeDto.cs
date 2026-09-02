using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPProjectTypeDto
{
	[JsonProperty("prtProjectTypeID", Order = 1)]
	[Required(ErrorMessage = "prtProjectTypeID is required.")]
	[MaxLength(5)]
	public string prtProjectTypeID { get; set; }

	[JsonProperty("prtCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string prtCreatedBy { get; set; }

	[JsonProperty("prtCreatedDate", Order = 3)]
	public DateTime? prtCreatedDate { get; set; }

	[JsonProperty("prtDescription", Order = 4)]
	[Required(ErrorMessage = "prtDescription is required.")]
	[MaxLength(50)]
	public string prtDescription { get; set; }

	[JsonProperty("prtUniqueID", Order = 5)]
	public Guid prtUniqueID { get; set; }

	[JsonProperty("prtInactiveDate", Order = 6)]
	public DateTime? prtInactiveDate { get; set; }

	[JsonProperty("prtInactive", Order = 7)]
	public bool prtInactive { get; set; }

	[JsonProperty("prtRowVersion", Order = 8)]
	public byte[] prtRowVersion { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
