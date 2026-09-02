using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPDocumentLinkDto
{
	[JsonProperty("xalAddedByUserID", Order = 1)]
	[Required(ErrorMessage = "xalAddedByUserID is required.")]
	[MaxLength(20)]
	public string xalAddedByUserID { get; set; }

	[JsonProperty("xalAddedDate", Order = 2)]
	[Required(ErrorMessage = "xalAddedDate is required.")]
	public DateTime? xalAddedDate { get; set; }

	[JsonProperty("xalCloudFileId", Order = 3)]
	[MaxLength(255)]
	public string xalCloudFileId { get; set; }

	[JsonProperty("xalCreatedBy", Order = 4)]
	[MaxLength(20)]
	public string xalCreatedBy { get; set; }

	[JsonProperty("xalCreatedDate", Order = 5)]
	public DateTime? xalCreatedDate { get; set; }

	[JsonProperty("xalDescription", Order = 6)]
	[Required(ErrorMessage = "xalDescription is required.")]
	[MaxLength(255)]
	public string xalDescription { get; set; }

	[JsonProperty("xalUniqueID", Order = 7)]
	public Guid xalUniqueID { get; set; }

	[JsonProperty("xalFileLastModifiedDate", Order = 8)]
	public DateTime? xalFileLastModifiedDate { get; set; }

	[JsonProperty("xalFileName", Order = 9)]
	[Required(ErrorMessage = "xalFileName is required.")]
	[MaxLength(254)]
	public string xalFileName { get; set; }

	[JsonProperty("xalFileNameWhenUploaded", Order = 10)]
	[MaxLength(50)]
	public string xalFileNameWhenUploaded { get; set; }

	[JsonProperty("xalEmailDefault", Order = 11)]
	public bool xalEmailDefault { get; set; }

	[JsonProperty("xalPrintDefault", Order = 12)]
	public bool xalPrintDefault { get; set; }

	[JsonProperty("xalReference", Order = 13)]
	[MaxLength(30)]
	public string xalReference { get; set; }

	[JsonProperty("xalRowVersion", Order = 14)]
	public byte[] xalRowVersion { get; set; }

	[JsonProperty("xalDocumentLinkID", Order = 15)]
	[Required(ErrorMessage = "xalDocumentLinkID is required.")]
	[Range(0, 999999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xalDocumentLinkID { get; set; }

	[JsonProperty("xalType", Order = 16)]
	[MaxLength(5)]
	public string xalType { get; set; }

	[JsonProperty("customFields", Order = 17)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
