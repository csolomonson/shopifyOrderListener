using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPPartReviewDto
{
	[JsonProperty("wgrComments", Order = 1)]
	[MaxLength(4)]
	public string wgrComments { get; set; }

	[JsonProperty("wgrPartID", Order = 2)]
	[Required(ErrorMessage = "wgrPartID is required.")]
	[MaxLength(30)]
	public string wgrPartID { get; set; }

	[JsonProperty("wgrRating", Order = 3)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int wgrRating { get; set; }

	[JsonProperty("wgrReviewerEmailAddress", Order = 4)]
	[MaxLength(50)]
	public string wgrReviewerEmailAddress { get; set; }

	[JsonProperty("wgrReviewerName", Order = 5)]
	[MaxLength(50)]
	public string wgrReviewerName { get; set; }

	[JsonProperty("wgrRowVersion", Order = 6)]
	public byte[] wgrRowVersion { get; set; }

	[JsonProperty("wgrPartReviewID", Order = 7)]
	[Required(ErrorMessage = "wgrPartReviewID is required.")]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int wgrPartReviewID { get; set; }

	[JsonProperty("customFields", Order = 8)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
