using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPEmployeePOApprovalDto
{
	[JsonProperty("lmhApprovalEmployeeID", Order = 1)]
	[Required(ErrorMessage = "lmhApprovalEmployeeID is required.")]
	[MaxLength(10)]
	public string lmhApprovalEmployeeID { get; set; }

	[JsonProperty("lmhCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string lmhCreatedBy { get; set; }

	[JsonProperty("lmhCreatedDate", Order = 3)]
	public DateTime? lmhCreatedDate { get; set; }

	[JsonProperty("lmhEmployeeID", Order = 4)]
	[Required(ErrorMessage = "lmhEmployeeID is required.")]
	[MaxLength(10)]
	public string lmhEmployeeID { get; set; }

	[JsonProperty("lmhUniqueID", Order = 5)]
	public Guid lmhUniqueID { get; set; }

	[JsonProperty("lmhRowVersion", Order = 6)]
	public byte[] lmhRowVersion { get; set; }

	[JsonProperty("lmhEmployeePoApprovalID", Order = 7)]
	[Required(ErrorMessage = "lmhEmployeePoApprovalID is required.")]
	public byte lmhEmployeePoApprovalID { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
