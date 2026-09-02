using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPChangeRequestGroupLinkDto
{
	[JsonProperty("chrChangeRequestGroupID", Order = 1)]
	[Required(ErrorMessage = "chrChangeRequestGroupID is required.")]
	[MaxLength(5)]
	public string chrChangeRequestGroupID { get; set; }

	[JsonProperty("chrChangeRequestID", Order = 2)]
	[Required(ErrorMessage = "chrChangeRequestID is required.")]
	[MaxLength(10)]
	public string chrChangeRequestID { get; set; }

	[JsonProperty("chrCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string chrCreatedBy { get; set; }

	[JsonProperty("chrCreatedDate", Order = 4)]
	public DateTime? chrCreatedDate { get; set; }

	[JsonProperty("chrUniqueID", Order = 5)]
	public Guid chrUniqueID { get; set; }

	[JsonProperty("chrRowVersion", Order = 6)]
	public byte[] chrRowVersion { get; set; }

	[JsonProperty("chrChangeRequestGroupLinkID", Order = 7)]
	[Required(ErrorMessage = "chrChangeRequestGroupLinkID is required.")]
	public short chrChangeRequestGroupLinkID { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
