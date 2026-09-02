using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPurchaseOrderAccountDto
{
	[JsonProperty("pmxAmount", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmxAmount { get; set; }

	[JsonProperty("pmxCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string pmxCreatedBy { get; set; }

	[JsonProperty("pmxCreatedDate", Order = 3)]
	public DateTime? pmxCreatedDate { get; set; }

	[JsonProperty("pmxUniqueID", Order = 4)]
	public Guid pmxUniqueID { get; set; }

	[JsonProperty("pmxExpenseGlAccountID", Order = 5)]
	[Required(ErrorMessage = "pmxExpenseGlAccountID is required.")]
	[MaxLength(11)]
	public string pmxExpenseGlAccountID { get; set; }

	[JsonProperty("pmxClosed", Order = 6)]
	public bool pmxClosed { get; set; }

	[JsonProperty("pmxPercent", Order = 7)]
	[Range(0.0, 9999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal pmxPercent { get; set; }

	[JsonProperty("pmxPurchaseOrderID", Order = 8)]
	[Required(ErrorMessage = "pmxPurchaseOrderID is required.")]
	[MaxLength(10)]
	public string pmxPurchaseOrderID { get; set; }

	[JsonProperty("pmxPurchaseOrderLineID", Order = 9)]
	[Required(ErrorMessage = "pmxPurchaseOrderLineID is required.")]
	public short pmxPurchaseOrderLineID { get; set; }

	[JsonProperty("pmxRowVersion", Order = 10)]
	public byte[] pmxRowVersion { get; set; }

	[JsonProperty("pmxPurchaseOrderAccountID", Order = 11)]
	[Required(ErrorMessage = "pmxPurchaseOrderAccountID is required.")]
	public short pmxPurchaseOrderAccountID { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
