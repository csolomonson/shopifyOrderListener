using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPToolMemoDto
{
	[JsonProperty("xtmCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string xtmCreatedBy { get; set; }

	[JsonProperty("xtmCreatedDate", Order = 2)]
	public DateTime? xtmCreatedDate { get; set; }

	[JsonProperty("xtmUniqueID", Order = 3)]
	public Guid xtmUniqueID { get; set; }

	[JsonProperty("xtmLongDescriptionRtf", Order = 4)]
	public string xtmLongDescriptionRtf { get; set; }

	[JsonProperty("xtmLongDescriptionText", Order = 5)]
	public string xtmLongDescriptionText { get; set; }

	[JsonProperty("xtmMemoDate", Order = 6)]
	public DateTime? xtmMemoDate { get; set; }

	[JsonProperty("xtmRowVersion", Order = 7)]
	public byte[] xtmRowVersion { get; set; }

	[JsonProperty("xtmToolMemoID", Order = 8)]
	public short xtmToolMemoID { get; set; }

	[JsonProperty("xtmShortDescription", Order = 9)]
	[Required(ErrorMessage = "xtmShortDescription is required.")]
	[MaxLength(50)]
	public string xtmShortDescription { get; set; }

	[JsonProperty("xtmToolID", Order = 10)]
	[MaxLength(10)]
	public string xtmToolID { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
