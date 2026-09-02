using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPProductCategoryDto
{
	[JsonProperty("incProductCategoryID", Order = 1)]
	[Required(ErrorMessage = "incProductCategoryID is required.")]
	[MaxLength(30)]
	public string incProductCategoryID { get; set; }

	[JsonProperty("incCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string incCreatedBy { get; set; }

	[JsonProperty("incCreatedDate", Order = 3)]
	public DateTime? incCreatedDate { get; set; }

	[JsonProperty("incDescription", Order = 4)]
	[Required(ErrorMessage = "incDescription is required.")]
	[MaxLength(50)]
	public string incDescription { get; set; }

	[JsonProperty("incUniqueID", Order = 5)]
	public Guid incUniqueID { get; set; }

	[JsonProperty("incImageFilePath", Order = 6)]
	[MaxLength(50)]
	public string incImageFilePath { get; set; }

	[JsonProperty("incInactiveDate", Order = 7)]
	public DateTime? incInactiveDate { get; set; }

	[JsonProperty("incInactive", Order = 8)]
	public bool incInactive { get; set; }

	[JsonProperty("INCRowVersion", Order = 9)]
	public byte[] INCRowVersion { get; set; }

	[JsonProperty("incStructureCode", Order = 10)]
	[MaxLength(2)]
	public string incStructureCode { get; set; }

	[JsonProperty("incStructureID", Order = 11)]
	[MaxLength(2)]
	public string incStructureID { get; set; }

	[JsonProperty("customFields", Order = 12)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
