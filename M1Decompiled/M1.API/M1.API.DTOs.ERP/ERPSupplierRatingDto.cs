using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPSupplierRatingDto
{
	[JsonProperty("cmsSupplierRatingID", Order = 1)]
	[Required(ErrorMessage = "cmsSupplierRatingID is required.")]
	[MaxLength(5)]
	public string cmsSupplierRatingID { get; set; }

	[JsonProperty("cmsCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string cmsCreatedBy { get; set; }

	[JsonProperty("cmsCreatedDate", Order = 3)]
	public DateTime? cmsCreatedDate { get; set; }

	[JsonProperty("cmsDescription", Order = 4)]
	[Required(ErrorMessage = "cmsDescription is required.")]
	[MaxLength(50)]
	public string cmsDescription { get; set; }

	[JsonProperty("cmsUniqueID", Order = 5)]
	public Guid cmsUniqueID { get; set; }

	[JsonProperty("cmsRowVersion", Order = 6)]
	public byte[] cmsRowVersion { get; set; }

	[JsonProperty("customFields", Order = 7)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
