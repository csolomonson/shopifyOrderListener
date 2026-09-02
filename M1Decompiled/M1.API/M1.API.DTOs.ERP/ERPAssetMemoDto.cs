using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAssetMemoDto
{
	[JsonProperty("fakAssetID", Order = 1)]
	[Required(ErrorMessage = "fakAssetID is required.")]
	[MaxLength(10)]
	public string fakAssetID { get; set; }

	[JsonProperty("fakCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string fakCreatedBy { get; set; }

	[JsonProperty("fakCreatedDate", Order = 3)]
	public DateTime? fakCreatedDate { get; set; }

	[JsonProperty("fakUniqueID", Order = 4)]
	public Guid fakUniqueID { get; set; }

	[JsonProperty("fakLongDescriptionRtf", Order = 5)]
	public string fakLongDescriptionRtf { get; set; }

	[JsonProperty("fakLongDescriptionText", Order = 6)]
	public string fakLongDescriptionText { get; set; }

	[JsonProperty("fakMemoDate", Order = 7)]
	[Required(ErrorMessage = "fakMemoDate is required.")]
	public DateTime? fakMemoDate { get; set; }

	[JsonProperty("fakRowVersion", Order = 8)]
	public byte[] fakRowVersion { get; set; }

	[JsonProperty("fakAssetMemoID", Order = 9)]
	[Required(ErrorMessage = "fakAssetMemoID is required.")]
	public short fakAssetMemoID { get; set; }

	[JsonProperty("fakShortDescription", Order = 10)]
	[Required(ErrorMessage = "fakShortDescription is required.")]
	[MaxLength(50)]
	public string fakShortDescription { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
