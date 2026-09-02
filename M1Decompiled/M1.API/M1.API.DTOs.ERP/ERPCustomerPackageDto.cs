using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPCustomerPackageDto
{
	[JsonProperty("cpaCustomerPackageID", Order = 1)]
	[Required(ErrorMessage = "cpaCustomerPackageID is required.")]
	[MaxLength(10)]
	public string cpaCustomerPackageID { get; set; }

	[JsonProperty("cpaCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string cpaCreatedBy { get; set; }

	[JsonProperty("cpaCreatedDate", Order = 3)]
	public DateTime? cpaCreatedDate { get; set; }

	[JsonProperty("cpaUniqueID", Order = 4)]
	public Guid cpaUniqueID { get; set; }

	[JsonProperty("cpaInactiveDate", Order = 5)]
	public DateTime? cpaInactiveDate { get; set; }

	[JsonProperty("cpaInactive", Order = 6)]
	public bool cpaInactive { get; set; }

	[JsonProperty("cpaPackageDescription", Order = 7)]
	[Required(ErrorMessage = "cpaPackageDescription is required.")]
	[MaxLength(50)]
	public string cpaPackageDescription { get; set; }

	[JsonProperty("cpaPackageDimensionsUom", Order = 8)]
	[MaxLength(2)]
	public string cpaPackageDimensionsUom { get; set; }

	[JsonProperty("cpaPackageHeight", Order = 9)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int cpaPackageHeight { get; set; }

	[JsonProperty("cpaPackageLength", Order = 10)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int cpaPackageLength { get; set; }

	[JsonProperty("cpaPackageWidth", Order = 11)]
	[Range(0, 9999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int cpaPackageWidth { get; set; }

	[JsonProperty("cpaRowVersion", Order = 12)]
	public byte[] cpaRowVersion { get; set; }

	[JsonProperty("customFields", Order = 13)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
