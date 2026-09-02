using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPurchaseOrderApprovalDto
{
	[JsonProperty("pmaApprovalEmployeeID", Order = 1)]
	[MaxLength(10)]
	public string pmaApprovalEmployeeID { get; set; }

	[JsonProperty("pmaCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string pmaCreatedBy { get; set; }

	[JsonProperty("pmaCreatedDate", Order = 3)]
	public DateTime? pmaCreatedDate { get; set; }

	[JsonProperty("pmaDescription", Order = 4)]
	[MaxLength(50)]
	public string pmaDescription { get; set; }

	[JsonProperty("pmaUniqueID", Order = 5)]
	public Guid pmaUniqueID { get; set; }

	[JsonProperty("pmaPurchaseOrderID", Order = 6)]
	[Required(ErrorMessage = "pmaPurchaseOrderID is required.")]
	[MaxLength(10)]
	public string pmaPurchaseOrderID { get; set; }

	[JsonProperty("pmaRowVersion", Order = 7)]
	public byte[] pmaRowVersion { get; set; }

	[JsonProperty("pmaPurchaseOrderApprovalID", Order = 8)]
	[Required(ErrorMessage = "pmaPurchaseOrderApprovalID is required.")]
	public byte pmaPurchaseOrderApprovalID { get; set; }

	[JsonProperty("pmaStatus", Order = 9)]
	public byte pmaStatus { get; set; }

	[JsonProperty("pmaStatusDate", Order = 10)]
	public DateTime? pmaStatusDate { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
