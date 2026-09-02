using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAttachmentMemoDto
{
	[JsonProperty("cmqAttachmentID", Order = 1)]
	[Required(ErrorMessage = "cmqAttachmentID is required.")]
	[MaxLength(10)]
	public string cmqAttachmentID { get; set; }

	[JsonProperty("cmqCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string cmqCreatedBy { get; set; }

	[JsonProperty("cmqCreatedDate", Order = 3)]
	public DateTime? cmqCreatedDate { get; set; }

	[JsonProperty("cmqUniqueID", Order = 4)]
	public Guid cmqUniqueID { get; set; }

	[JsonProperty("cmqLongDescriptionRtf", Order = 5)]
	public string cmqLongDescriptionRtf { get; set; }

	[JsonProperty("cmqLongDescriptionText", Order = 6)]
	public string cmqLongDescriptionText { get; set; }

	[JsonProperty("cmqMemoDate", Order = 7)]
	[Required(ErrorMessage = "cmqMemoDate is required.")]
	public DateTime? cmqMemoDate { get; set; }

	[JsonProperty("cmqRowVersion", Order = 8)]
	public byte[] cmqRowVersion { get; set; }

	[JsonProperty("cmqAttachmentMemoID", Order = 9)]
	[Required(ErrorMessage = "cmqAttachmentMemoID is required.")]
	public short cmqAttachmentMemoID { get; set; }

	[JsonProperty("cmqShortDescription", Order = 10)]
	[Required(ErrorMessage = "cmqShortDescription is required.")]
	[MaxLength(50)]
	public string cmqShortDescription { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
