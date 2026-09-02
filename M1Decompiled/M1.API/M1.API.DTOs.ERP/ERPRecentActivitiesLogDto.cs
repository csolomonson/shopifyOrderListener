using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPRecentActivitiesLogDto
{
	[JsonProperty("rtlCount", Order = 1)]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int rtlCount { get; set; }

	[JsonProperty("rtlExplorerType", Order = 2)]
	[MaxLength(30)]
	public string rtlExplorerType { get; set; }

	[JsonProperty("rtlLastOpenedDateTime", Order = 3)]
	public DateTime rtlLastOpenedDateTime { get; set; }

	[JsonProperty("rtlObjectDataRun", Order = 4)]
	[MaxLength(200)]
	public string rtlObjectDataRun { get; set; }

	[JsonProperty("rtlObjectID", Order = 5)]
	[MaxLength(50)]
	public string rtlObjectID { get; set; }

	[JsonProperty("rtlObjectName", Order = 6)]
	[MaxLength(100)]
	public string rtlObjectName { get; set; }

	[JsonProperty("rtlParentKey", Order = 7)]
	[MaxLength(50)]
	public string rtlParentKey { get; set; }

	[JsonProperty("rtlRecentActivityID", Order = 8)]
	public int rtlRecentActivityID { get; set; }

	[JsonProperty("rtlRowVersion", Order = 9)]
	public byte[] rtlRowVersion { get; set; }

	[JsonProperty("rtlUserID", Order = 10)]
	[MaxLength(50)]
	public string rtlUserID { get; set; }

	[JsonProperty("customFields", Order = 11)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
