using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAssetTypeDto
{
	[JsonProperty("fatAccumDeprGlAccountID", Order = 1)]
	[Required(ErrorMessage = "fatAccumDeprGlAccountID is required.")]
	[MaxLength(11)]
	public string fatAccumDeprGlAccountID { get; set; }

	[JsonProperty("fatAssetGlAccountID", Order = 2)]
	[Required(ErrorMessage = "fatAssetGlAccountID is required.")]
	[MaxLength(11)]
	public string fatAssetGlAccountID { get; set; }

	[JsonProperty("fatAssetTypeID", Order = 3)]
	[Required(ErrorMessage = "fatAssetTypeID is required.")]
	[MaxLength(5)]
	public string fatAssetTypeID { get; set; }

	[JsonProperty("fatCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string fatCreatedBy { get; set; }

	[JsonProperty("fatCreatedDate", Order = 5)]
	public DateTime? fatCreatedDate { get; set; }

	[JsonProperty("fatDepreciationGlAccountID", Order = 6)]
	[Required(ErrorMessage = "fatDepreciationGlAccountID is required.")]
	[MaxLength(11)]
	public string fatDepreciationGlAccountID { get; set; }

	[JsonProperty("fatDescription", Order = 7)]
	[Required(ErrorMessage = "fatDescription is required.")]
	[MaxLength(50)]
	public string fatDescription { get; set; }

	[JsonProperty("fatUniqueID", Order = 8)]
	public Guid fatUniqueID { get; set; }

	[JsonProperty("fatExpenseGlAccountID", Order = 9)]
	[Required(ErrorMessage = "fatExpenseGlAccountID is required.")]
	[MaxLength(11)]
	public string fatExpenseGlAccountID { get; set; }

	[JsonProperty("fatLossGlAccountID", Order = 10)]
	[Required(ErrorMessage = "fatLossGlAccountID is required.")]
	[MaxLength(11)]
	public string fatLossGlAccountID { get; set; }

	[JsonProperty("fatProfitGlAccountID", Order = 11)]
	[Required(ErrorMessage = "fatProfitGlAccountID is required.")]
	[MaxLength(11)]
	public string fatProfitGlAccountID { get; set; }

	[JsonProperty("fatRepairsGlAccountID", Order = 12)]
	[Required(ErrorMessage = "fatRepairsGlAccountID is required.")]
	[MaxLength(11)]
	public string fatRepairsGlAccountID { get; set; }

	[JsonProperty("fatRevaluationGlAccountID", Order = 13)]
	[Required(ErrorMessage = "fatRevaluationGlAccountID is required.")]
	[MaxLength(11)]
	public string fatRevaluationGlAccountID { get; set; }

	[JsonProperty("fatRowVersion", Order = 14)]
	public byte[] fatRowVersion { get; set; }

	[JsonProperty("customFields", Order = 15)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
