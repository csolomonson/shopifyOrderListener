using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPAgingBucketDto
{
	[JsonProperty("xaaBucket1DaysOver", Order = 1)]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xaaBucket1DaysOver { get; set; }

	[JsonProperty("xaaBucket1Description", Order = 2)]
	[Required(ErrorMessage = "xaaBucket1Description is required.")]
	[MaxLength(10)]
	public string xaaBucket1Description { get; set; }

	[JsonProperty("xaaBucket2DaysOver", Order = 3)]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xaaBucket2DaysOver { get; set; }

	[JsonProperty("xaaBucket2Description", Order = 4)]
	[MaxLength(10)]
	public string xaaBucket2Description { get; set; }

	[JsonProperty("xaaBucket3DaysOver", Order = 5)]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xaaBucket3DaysOver { get; set; }

	[JsonProperty("xaaBucket3Description", Order = 6)]
	[MaxLength(10)]
	public string xaaBucket3Description { get; set; }

	[JsonProperty("xaaBucket4DaysOver", Order = 7)]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xaaBucket4DaysOver { get; set; }

	[JsonProperty("xaaBucket4Description", Order = 8)]
	[MaxLength(10)]
	public string xaaBucket4Description { get; set; }

	[JsonProperty("xaaBucket5DaysOver", Order = 9)]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int xaaBucket5DaysOver { get; set; }

	[JsonProperty("xaaBucket5Description", Order = 10)]
	[MaxLength(10)]
	public string xaaBucket5Description { get; set; }

	[JsonProperty("xaaCalculationType", Order = 11)]
	[Required(ErrorMessage = "xaaCalculationType is required.")]
	public byte xaaCalculationType { get; set; }

	[JsonProperty("xaaAgingBucketID", Order = 12)]
	[Required(ErrorMessage = "xaaAgingBucketID is required.")]
	[MaxLength(5)]
	public string xaaAgingBucketID { get; set; }

	[JsonProperty("xaaCreatedBy", Order = 13)]
	[MaxLength(20)]
	public string xaaCreatedBy { get; set; }

	[JsonProperty("xaaCreatedDate", Order = 14)]
	public DateTime? xaaCreatedDate { get; set; }

	[JsonProperty("xaaDescription", Order = 15)]
	[Required(ErrorMessage = "xaaDescription is required.")]
	[MaxLength(50)]
	public string xaaDescription { get; set; }

	[JsonProperty("xaaUniqueID", Order = 16)]
	public Guid xaaUniqueID { get; set; }

	[JsonProperty("xaaRowVersion", Order = 17)]
	public byte[] xaaRowVersion { get; set; }

	[JsonProperty("customFields", Order = 18)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
