using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPProductCategoryLineDto
{
	[JsonProperty("insCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string insCreatedBy { get; set; }

	[JsonProperty("insCreatedDate", Order = 2)]
	public DateTime? insCreatedDate { get; set; }

	[JsonProperty("insDescription", Order = 3)]
	[Required(ErrorMessage = "insDescription is required.")]
	[MaxLength(50)]
	public string insDescription { get; set; }

	[JsonProperty("insUniqueID", Order = 4)]
	public Guid insUniqueID { get; set; }

	[JsonProperty("insImageFilePath", Order = 5)]
	[MaxLength(50)]
	public string insImageFilePath { get; set; }

	[JsonProperty("insInactiveDate", Order = 6)]
	public DateTime? insInactiveDate { get; set; }

	[JsonProperty("insInactive", Order = 7)]
	public bool insInactive { get; set; }

	[JsonProperty("insLevel", Order = 8)]
	[Required(ErrorMessage = "insLevel is required.")]
	public byte insLevel { get; set; }

	[JsonProperty("insParentLineID", Order = 9)]
	public short insParentLineID { get; set; }

	[JsonProperty("insProductCategoryID", Order = 10)]
	[Required(ErrorMessage = "insProductCategoryID is required.")]
	[MaxLength(30)]
	public string insProductCategoryID { get; set; }

	[JsonProperty("INSRowVersion", Order = 11)]
	public byte[] INSRowVersion { get; set; }

	[JsonProperty("insProductCategoryLineID", Order = 12)]
	[Required(ErrorMessage = "insProductCategoryLineID is required.")]
	public short insProductCategoryLineID { get; set; }

	[JsonProperty("insStructureCode", Order = 13)]
	[MaxLength(14)]
	public string insStructureCode { get; set; }

	[JsonProperty("insStructureID", Order = 14)]
	[MaxLength(2)]
	public string insStructureID { get; set; }

	[JsonProperty("customFields", Order = 15)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
