using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPEmployeeSOApprovalDto
{
	[JsonProperty("lmoApprovalEmployeeID", Order = 1)]
	[Required(ErrorMessage = "lmoApprovalEmployeeID is required.")]
	[MaxLength(10)]
	public string lmoApprovalEmployeeID { get; set; }

	[JsonProperty("lmoCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string lmoCreatedBy { get; set; }

	[JsonProperty("lmoCreatedDate", Order = 3)]
	public DateTime? lmoCreatedDate { get; set; }

	[JsonProperty("lmoEmployeeID", Order = 4)]
	[Required(ErrorMessage = "lmoEmployeeID is required.")]
	[MaxLength(10)]
	public string lmoEmployeeID { get; set; }

	[JsonProperty("lmoUniqueID", Order = 5)]
	public Guid lmoUniqueID { get; set; }

	[JsonProperty("lmoRowVersion", Order = 6)]
	public byte[] lmoRowVersion { get; set; }

	[JsonProperty("lmoEmployeeSOApprovalID", Order = 7)]
	[Required(ErrorMessage = "lmoEmployeeSOApprovalID is required.")]
	public byte lmoEmployeeSOApprovalID { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
