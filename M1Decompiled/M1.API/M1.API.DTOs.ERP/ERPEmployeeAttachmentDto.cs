using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPEmployeeAttachmentDto
{
	[JsonProperty("lmaAttachmentTypeID", Order = 1)]
	[MaxLength(5)]
	public string lmaAttachmentTypeID { get; set; }

	[JsonProperty("lmaEmployeeAttachmentID", Order = 2)]
	[Required(ErrorMessage = "lmaEmployeeAttachmentID is required.")]
	[MaxLength(10)]
	public string lmaEmployeeAttachmentID { get; set; }

	[JsonProperty("lmaCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string lmaCreatedBy { get; set; }

	[JsonProperty("lmaCreatedDate", Order = 4)]
	public DateTime? lmaCreatedDate { get; set; }

	[JsonProperty("lmaDate", Order = 5)]
	[Required(ErrorMessage = "lmaDate is required.")]
	public DateTime? lmaDate { get; set; }

	[JsonProperty("lmaEmployeeID", Order = 6)]
	[Required(ErrorMessage = "lmaEmployeeID is required.")]
	[MaxLength(10)]
	public string lmaEmployeeID { get; set; }

	[JsonProperty("lmaUniqueID", Order = 7)]
	public Guid lmaUniqueID { get; set; }

	[JsonProperty("lmaFileLocation", Order = 8)]
	[MaxLength(255)]
	public string lmaFileLocation { get; set; }

	[JsonProperty("lmaFileName", Order = 9)]
	[MaxLength(255)]
	public string lmaFileName { get; set; }

	[JsonProperty("lmaLongDescriptionRtf", Order = 10)]
	public string lmaLongDescriptionRtf { get; set; }

	[JsonProperty("lmaLongDescriptionText", Order = 11)]
	public string lmaLongDescriptionText { get; set; }

	[JsonProperty("lmaRowVersion", Order = 12)]
	public byte[] lmaRowVersion { get; set; }

	[JsonProperty("lmaShortDescription", Order = 13)]
	[Required(ErrorMessage = "lmaShortDescription is required.")]
	[MaxLength(70)]
	public string lmaShortDescription { get; set; }

	[JsonProperty("customFields", Order = 14)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
