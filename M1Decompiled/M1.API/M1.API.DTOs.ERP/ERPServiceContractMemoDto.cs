using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPServiceContractMemoDto
{
	[JsonProperty("kbmCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string kbmCreatedBy { get; set; }

	[JsonProperty("kbmCreatedDate", Order = 2)]
	public DateTime? kbmCreatedDate { get; set; }

	[JsonProperty("kbmUniqueID", Order = 3)]
	public Guid kbmUniqueID { get; set; }

	[JsonProperty("kbmLongDescriptionRtf", Order = 4)]
	public string kbmLongDescriptionRtf { get; set; }

	[JsonProperty("kbmLongDescriptionText", Order = 5)]
	public string kbmLongDescriptionText { get; set; }

	[JsonProperty("kbmMemoDate", Order = 6)]
	[Required(ErrorMessage = "kbmMemoDate is required.")]
	public DateTime? kbmMemoDate { get; set; }

	[JsonProperty("kbmRowVersion", Order = 7)]
	public byte[] kbmRowVersion { get; set; }

	[JsonProperty("kbmServiceContractMemoID", Order = 8)]
	[Required(ErrorMessage = "kbmServiceContractMemoID is required.")]
	public short kbmServiceContractMemoID { get; set; }

	[JsonProperty("kbmServiceContractID", Order = 9)]
	[Required(ErrorMessage = "kbmServiceContractID is required.")]
	[MaxLength(10)]
	public string kbmServiceContractID { get; set; }

	[JsonProperty("kbmShortDescription", Order = 10)]
	[Required(ErrorMessage = "kbmShortDescription is required.")]
	[MaxLength(50)]
	public string kbmShortDescription { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
