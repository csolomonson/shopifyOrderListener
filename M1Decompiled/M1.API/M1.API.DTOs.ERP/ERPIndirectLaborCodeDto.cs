using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPIndirectLaborCodeDto
{
	[JsonProperty("lmiCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string lmiCreatedBy { get; set; }

	[JsonProperty("lmiCreatedDate", Order = 2)]
	public DateTime? lmiCreatedDate { get; set; }

	[JsonProperty("lmiDescription", Order = 3)]
	[Required(ErrorMessage = "lmiDescription is required.")]
	[MaxLength(50)]
	public string lmiDescription { get; set; }

	[JsonProperty("lmiUniqueID", Order = 4)]
	public Guid lmiUniqueID { get; set; }

	[JsonProperty("lmiInactiveDate", Order = 5)]
	public DateTime? lmiInactiveDate { get; set; }

	[JsonProperty("lmiIndirectLaborID", Order = 6)]
	[Required(ErrorMessage = "lmiIndirectLaborID is required.")]
	[MaxLength(5)]
	public string lmiIndirectLaborID { get; set; }

	[JsonProperty("lmiIndirectLaborType", Order = 7)]
	public byte lmiIndirectLaborType { get; set; }

	[JsonProperty("lmiInactive", Order = 8)]
	public bool lmiInactive { get; set; }

	[JsonProperty("lmiRowVersion", Order = 9)]
	public byte[] lmiRowVersion { get; set; }

	[JsonProperty("customFields", Order = 10)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
