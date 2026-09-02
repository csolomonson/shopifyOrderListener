using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPGLChartDto
{
	[JsonProperty("glcAccountType", Order = 1)]
	[Required(ErrorMessage = "glcAccountType is required.")]
	public byte glcAccountType { get; set; }

	[JsonProperty("glcCashFlowCategory", Order = 2)]
	public byte glcCashFlowCategory { get; set; }

	[JsonProperty("glcGlChartID", Order = 3)]
	[Required(ErrorMessage = "glcGlChartID is required.")]
	[MaxLength(5)]
	public string glcGlChartID { get; set; }

	[JsonProperty("glcCogsAccountType", Order = 4)]
	public byte glcCogsAccountType { get; set; }

	[JsonProperty("glcCreatedBy", Order = 5)]
	[MaxLength(20)]
	public string glcCreatedBy { get; set; }

	[JsonProperty("glcCreatedDate", Order = 6)]
	public DateTime? glcCreatedDate { get; set; }

	[JsonProperty("glcDescription", Order = 7)]
	[Required(ErrorMessage = "glcDescription is required.")]
	[MaxLength(35)]
	public string glcDescription { get; set; }

	[JsonProperty("glcUniqueID", Order = 8)]
	public Guid glcUniqueID { get; set; }

	[JsonProperty("glcGlCategoryID", Order = 9)]
	[Required(ErrorMessage = "glcGlCategoryID is required.")]
	[MaxLength(5)]
	public string glcGlCategoryID { get; set; }

	[JsonProperty("glcCashEquivalents", Order = 10)]
	public bool glcCashEquivalents { get; set; }

	[JsonProperty("glcParentAccount", Order = 11)]
	public bool glcParentAccount { get; set; }

	[JsonProperty("glcNormalBalance", Order = 12)]
	[Required(ErrorMessage = "glcNormalBalance is required.")]
	public byte glcNormalBalance { get; set; }

	[JsonProperty("glcParentDescription", Order = 13)]
	[MaxLength(35)]
	public string glcParentDescription { get; set; }

	[JsonProperty("glcParentGlChartID", Order = 14)]
	[MaxLength(5)]
	public string glcParentGlChartID { get; set; }

	[JsonProperty("glcRowVersion", Order = 15)]
	public byte[] glcRowVersion { get; set; }

	[JsonProperty("customFields", Order = 16)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
