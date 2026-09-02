using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPServiceContractTypeDto
{
	[JsonProperty("kbyServiceContractTypeID", Order = 1)]
	[Required(ErrorMessage = "kbyServiceContractTypeID is required.")]
	[MaxLength(5)]
	public string kbyServiceContractTypeID { get; set; }

	[JsonProperty("kbyCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string kbyCreatedBy { get; set; }

	[JsonProperty("kbyCreatedDate", Order = 3)]
	public DateTime? kbyCreatedDate { get; set; }

	[JsonProperty("kbyDescription", Order = 4)]
	[Required(ErrorMessage = "kbyDescription is required.")]
	[MaxLength(50)]
	public string kbyDescription { get; set; }

	[JsonProperty("kbyUniqueID", Order = 5)]
	public Guid kbyUniqueID { get; set; }

	[JsonProperty("kbyInactiveDate", Order = 6)]
	public DateTime? kbyInactiveDate { get; set; }

	[JsonProperty("kbyInactive", Order = 7)]
	public bool kbyInactive { get; set; }

	[JsonProperty("kbyRowVersion", Order = 8)]
	public byte[] kbyRowVersion { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
