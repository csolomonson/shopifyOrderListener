using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPCallTypeDto
{
	[JsonProperty("kbtCallStatus", Order = 1)]
	[MaxLength(1)]
	public string kbtCallStatus { get; set; }

	[JsonProperty("kbtCallTypeID", Order = 2)]
	[Required(ErrorMessage = "kbtCallTypeID is required.")]
	[MaxLength(5)]
	public string kbtCallTypeID { get; set; }

	[JsonProperty("kbtCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string kbtCreatedBy { get; set; }

	[JsonProperty("kbtCreatedDate", Order = 4)]
	public DateTime? kbtCreatedDate { get; set; }

	[JsonProperty("kbtDescription", Order = 5)]
	[Required(ErrorMessage = "kbtDescription is required.")]
	[MaxLength(50)]
	public string kbtDescription { get; set; }

	[JsonProperty("kbtUniqueID", Order = 6)]
	public Guid kbtUniqueID { get; set; }

	[JsonProperty("kbtInactiveDate", Order = 7)]
	public DateTime? kbtInactiveDate { get; set; }

	[JsonProperty("kbtInactive", Order = 8)]
	public bool kbtInactive { get; set; }

	[JsonProperty("kbtBillableCall", Order = 9)]
	public bool kbtBillableCall { get; set; }

	[JsonProperty("kbtFieldServiceCall", Order = 10)]
	public bool kbtFieldServiceCall { get; set; }

	[JsonProperty("kbtInboundCall", Order = 11)]
	public bool kbtInboundCall { get; set; }

	[JsonProperty("kbtInternalOnlyCall", Order = 12)]
	public bool kbtInternalOnlyCall { get; set; }

	[JsonProperty("kbtRowVersion", Order = 13)]
	public byte[] kbtRowVersion { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
