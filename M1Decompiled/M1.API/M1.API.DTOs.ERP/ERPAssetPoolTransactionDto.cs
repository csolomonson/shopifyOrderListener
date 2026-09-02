using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAssetPoolTransactionDto
{
	[JsonProperty("fawAmount", Order = 1)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fawAmount { get; set; }

	[JsonProperty("fawAssetAdjustmentID", Order = 2)]
	public int fawAssetAdjustmentID { get; set; }

	[JsonProperty("fawAssetID", Order = 3)]
	[Required(ErrorMessage = "fawAssetID is required.")]
	[MaxLength(10)]
	public string fawAssetID { get; set; }

	[JsonProperty("fawCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string fawCreatedBy { get; set; }

	[JsonProperty("fawCreatedDate", Order = 5)]
	public DateTime? fawCreatedDate { get; set; }

	[JsonProperty("fawUniqueID", Order = 6)]
	public Guid fawUniqueID { get; set; }

	[JsonProperty("fawPoolTransactionID", Order = 7)]
	[Required(ErrorMessage = "fawPoolTransactionID is required.")]
	public int fawPoolTransactionID { get; set; }

	[JsonProperty("fawPoolYearID", Order = 8)]
	[Required(ErrorMessage = "fawPoolYearID is required.")]
	public short fawPoolYearID { get; set; }

	[JsonProperty("fawRowVersion", Order = 9)]
	public byte[] fawRowVersion { get; set; }

	[JsonProperty("fawTransactionDate", Order = 10)]
	public DateTime? fawTransactionDate { get; set; }

	[JsonProperty("fawTransactionType", Order = 11)]
	[MaxLength(1)]
	public string fawTransactionType { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
