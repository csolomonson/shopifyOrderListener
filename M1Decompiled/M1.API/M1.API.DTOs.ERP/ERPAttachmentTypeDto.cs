using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAttachmentTypeDto
{
	[JsonProperty("cmtAttachmentTypeID", Order = 1)]
	[Required(ErrorMessage = "cmtAttachmentTypeID is required.")]
	[MaxLength(5)]
	public string cmtAttachmentTypeID { get; set; }

	[JsonProperty("cmtCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string cmtCreatedBy { get; set; }

	[JsonProperty("cmtCreatedDate", Order = 3)]
	public DateTime? cmtCreatedDate { get; set; }

	[JsonProperty("cmtDescription", Order = 4)]
	[Required(ErrorMessage = "cmtDescription is required.")]
	[MaxLength(50)]
	public string cmtDescription { get; set; }

	[JsonProperty("cmtUniqueID", Order = 5)]
	public Guid cmtUniqueID { get; set; }

	[JsonProperty("cmtRequiresLogin", Order = 6)]
	public bool cmtRequiresLogin { get; set; }

	[JsonProperty("cmtRequiresServiceContract", Order = 7)]
	public bool cmtRequiresServiceContract { get; set; }

	[JsonProperty("cmtRowVersion", Order = 8)]
	public byte[] cmtRowVersion { get; set; }

	[JsonProperty("customFields", Order = 9)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
