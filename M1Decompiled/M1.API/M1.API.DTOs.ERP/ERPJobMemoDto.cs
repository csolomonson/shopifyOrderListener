using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPJobMemoDto
{
	[JsonProperty("jmkCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string jmkCreatedBy { get; set; }

	[JsonProperty("jmkCreatedDate", Order = 2)]
	public DateTime? jmkCreatedDate { get; set; }

	[JsonProperty("jmkUniqueID", Order = 3)]
	public Guid jmkUniqueID { get; set; }

	[JsonProperty("jmkClosed", Order = 4)]
	public bool jmkClosed { get; set; }

	[JsonProperty("jmkJobID", Order = 5)]
	[Required(ErrorMessage = "jmkJobID is required.")]
	[MaxLength(20)]
	public string jmkJobID { get; set; }

	[JsonProperty("jmkLongDescriptionRtf", Order = 6)]
	public string jmkLongDescriptionRtf { get; set; }

	[JsonProperty("jmkLongDescriptionText", Order = 7)]
	public string jmkLongDescriptionText { get; set; }

	[JsonProperty("jmkMemoDate", Order = 8)]
	[Required(ErrorMessage = "jmkMemoDate is required.")]
	public DateTime? jmkMemoDate { get; set; }

	[JsonProperty("jmkRowVersion", Order = 9)]
	public byte[] jmkRowVersion { get; set; }

	[JsonProperty("jmkJobMemoID", Order = 10)]
	[Required(ErrorMessage = "jmkJobMemoID is required.")]
	public short jmkJobMemoID { get; set; }

	[JsonProperty("jmkShortDescription", Order = 11)]
	[Required(ErrorMessage = "jmkShortDescription is required.")]
	[MaxLength(50)]
	public string jmkShortDescription { get; set; }

	[JsonProperty("jmkShowInJobs", Order = 12)]
	public bool jmkShowInJobs { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
