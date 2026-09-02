using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPExpenseAccountSplitDto
{
	[JsonProperty("xazExpenseAccountSplitID", Order = 1)]
	[Required(ErrorMessage = "xazExpenseAccountSplitID is required.")]
	public Guid xazExpenseAccountSplitID { get; set; }

	[JsonProperty("xazCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string xazCreatedBy { get; set; }

	[JsonProperty("xazCreatedDate", Order = 3)]
	public DateTime? xazCreatedDate { get; set; }

	[JsonProperty("xazExpenseGlAccountID", Order = 4)]
	[Required(ErrorMessage = "xazExpenseGlAccountID is required.")]
	[MaxLength(11)]
	public string xazExpenseGlAccountID { get; set; }

	[JsonProperty("xazLandedCostCategoryID", Order = 5)]
	[MaxLength(5)]
	public string xazLandedCostCategoryID { get; set; }

	[JsonProperty("xazPartID", Order = 6)]
	[MaxLength(30)]
	public string xazPartID { get; set; }

	[JsonProperty("xazPartRevisionID", Order = 7)]
	[MaxLength(15)]
	public string xazPartRevisionID { get; set; }

	[JsonProperty("xazPercent", Order = 8)]
	[Range(0.0, 9999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal xazPercent { get; set; }

	[JsonProperty("xazRowVersion", Order = 9)]
	public byte[] xazRowVersion { get; set; }

	[JsonProperty("xazSequence", Order = 10)]
	[Required(ErrorMessage = "xazSequence is required.")]
	public short xazSequence { get; set; }

	[JsonProperty("xazSupplierOrganizationID", Order = 11)]
	[MaxLength(10)]
	public string xazSupplierOrganizationID { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
