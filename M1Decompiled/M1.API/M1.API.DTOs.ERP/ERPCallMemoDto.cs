using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPCallMemoDto
{
	[JsonProperty("kbkCallID", Order = 1)]
	[Required(ErrorMessage = "kbkCallID is required.")]
	[MaxLength(10)]
	public string kbkCallID { get; set; }

	[JsonProperty("kbkCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string kbkCreatedBy { get; set; }

	[JsonProperty("kbkCreatedDate", Order = 3)]
	public DateTime? kbkCreatedDate { get; set; }

	[JsonProperty("kbkUniqueID", Order = 4)]
	public Guid kbkUniqueID { get; set; }

	[JsonProperty("kbkLongDescriptionRtf", Order = 5)]
	public string kbkLongDescriptionRtf { get; set; }

	[JsonProperty("kbkLongDescriptionText", Order = 6)]
	public string kbkLongDescriptionText { get; set; }

	[JsonProperty("kbkMemoDate", Order = 7)]
	[Required(ErrorMessage = "kbkMemoDate is required.")]
	public DateTime? kbkMemoDate { get; set; }

	[JsonProperty("kbkRowVersion", Order = 8)]
	public byte[] kbkRowVersion { get; set; }

	[JsonProperty("kbkCallMemoID", Order = 9)]
	[Required(ErrorMessage = "kbkCallMemoID is required.")]
	public short kbkCallMemoID { get; set; }

	[JsonProperty("kbkShortDescription", Order = 10)]
	[Required(ErrorMessage = "kbkShortDescription is required.")]
	[MaxLength(50)]
	public string kbkShortDescription { get; set; }

	[JsonProperty("kbkShowInCalls", Order = 11)]
	public bool kbkShowInCalls { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
