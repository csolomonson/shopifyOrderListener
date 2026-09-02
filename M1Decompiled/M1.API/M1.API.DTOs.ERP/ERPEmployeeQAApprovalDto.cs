using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPEmployeeQAApprovalDto
{
	[JsonProperty("lmbApprovalEmployeeID", Order = 1)]
	[Required(ErrorMessage = "lmbApprovalEmployeeID is required.")]
	[MaxLength(10)]
	public string lmbApprovalEmployeeID { get; set; }

	[JsonProperty("lmbCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string lmbCreatedBy { get; set; }

	[JsonProperty("lmbCreatedDate", Order = 3)]
	public DateTime? lmbCreatedDate { get; set; }

	[JsonProperty("lmbEmployeeID", Order = 4)]
	[Required(ErrorMessage = "lmbEmployeeID is required.")]
	[MaxLength(10)]
	public string lmbEmployeeID { get; set; }

	[JsonProperty("lmbUniqueID", Order = 5)]
	public Guid lmbUniqueID { get; set; }

	[JsonProperty("lmbRowVersion", Order = 6)]
	public byte[] lmbRowVersion { get; set; }

	[JsonProperty("lmbEmployeeQAApprovalID", Order = 7)]
	[Required(ErrorMessage = "lmbEmployeeQAApprovalID is required.")]
	public byte lmbEmployeeQAApprovalID { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
