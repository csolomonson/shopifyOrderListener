using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAssetTypePlantDto
{
	[JsonProperty("fayAccumDeprGlAccountID", Order = 1)]
	[Required(ErrorMessage = "fayAccumDeprGlAccountID is required.")]
	[MaxLength(11)]
	public string fayAccumDeprGlAccountID { get; set; }

	[JsonProperty("fayAssetGlAccountID", Order = 2)]
	[Required(ErrorMessage = "fayAssetGlAccountID is required.")]
	[MaxLength(11)]
	public string fayAssetGlAccountID { get; set; }

	[JsonProperty("fayAssetTypeID", Order = 3)]
	[Required(ErrorMessage = "fayAssetTypeID is required.")]
	[MaxLength(5)]
	public string fayAssetTypeID { get; set; }

	[JsonProperty("fayAssetTypePlantID", Order = 4)]
	[Required(ErrorMessage = "fayAssetTypePlantID is required.")]
	[MaxLength(5)]
	public string fayAssetTypePlantID { get; set; }

	[JsonProperty("fayCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string fayCreatedBy { get; set; }

	[JsonProperty("fayCreatedDate", Order = 6)]
	public DateTime? fayCreatedDate { get; set; }

	[JsonProperty("fayDepreciationGlAccountID", Order = 7)]
	[Required(ErrorMessage = "fayDepreciationGlAccountID is required.")]
	[MaxLength(11)]
	public string fayDepreciationGlAccountID { get; set; }

	[JsonProperty("fayUniqueID", Order = 8)]
	public Guid fayUniqueID { get; set; }

	[JsonProperty("fayExpenseGlAccountID", Order = 9)]
	[Required(ErrorMessage = "fayExpenseGlAccountID is required.")]
	[MaxLength(11)]
	public string fayExpenseGlAccountID { get; set; }

	[JsonProperty("fayLossGlAccountID", Order = 10)]
	[Required(ErrorMessage = "fayLossGlAccountID is required.")]
	[MaxLength(11)]
	public string fayLossGlAccountID { get; set; }

	[JsonProperty("fayProfitGlAccountID", Order = 11)]
	[Required(ErrorMessage = "fayProfitGlAccountID is required.")]
	[MaxLength(11)]
	public string fayProfitGlAccountID { get; set; }

	[JsonProperty("fayRepairsGlAccountID", Order = 12)]
	[Required(ErrorMessage = "fayRepairsGlAccountID is required.")]
	[MaxLength(11)]
	public string fayRepairsGlAccountID { get; set; }

	[JsonProperty("fayRevaluationGlAccountID", Order = 13)]
	[Required(ErrorMessage = "fayRevaluationGlAccountID is required.")]
	[MaxLength(11)]
	public string fayRevaluationGlAccountID { get; set; }

	[JsonProperty("fayRowVersion", Order = 14)]
	public byte[] fayRowVersion { get; set; }

	[JsonProperty("customFields", Order = 15)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
