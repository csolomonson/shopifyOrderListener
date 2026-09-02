using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPFreightPackageDto
{
	[JsonProperty("fslCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string fslCreatedBy { get; set; }

	[JsonProperty("fslCreatedDate", Order = 2)]
	public DateTime? fslCreatedDate { get; set; }

	[JsonProperty("fslDimensionsUnitOfMeasure", Order = 3)]
	[MaxLength(3)]
	public string fslDimensionsUnitOfMeasure { get; set; }

	[JsonProperty("fslDistributeCostsOption", Order = 4)]
	public byte fslDistributeCostsOption { get; set; }

	[JsonProperty("fslUniqueID", Order = 5)]
	public Guid fslUniqueID { get; set; }

	[JsonProperty("fslFdxPackageHeight", Order = 6)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int fslFdxPackageHeight { get; set; }

	[JsonProperty("fslFdxPackageLength", Order = 7)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int fslFdxPackageLength { get; set; }

	[JsonProperty("fslFdxPackageWidth", Order = 8)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int fslFdxPackageWidth { get; set; }

	[JsonProperty("fslFdxPackaging", Order = 9)]
	[MaxLength(14)]
	public string fslFdxPackaging { get; set; }

	[JsonProperty("fslFreightShipmentID", Order = 10)]
	[Required(ErrorMessage = "fslFreightShipmentID is required.")]
	[MaxLength(10)]
	public string fslFreightShipmentID { get; set; }

	[JsonProperty("fslFdxNonstandardContainer", Order = 11)]
	public bool fslFdxNonstandardContainer { get; set; }

	[JsonProperty("fslVoidOnUps", Order = 12)]
	public bool fslVoidOnUps { get; set; }

	[JsonProperty("fslNotesRTF", Order = 13)]
	[MaxLength(50)]
	public string fslNotesRTF { get; set; }

	[JsonProperty("fslNotesText", Order = 14)]
	[MaxLength(50)]
	public string fslNotesText { get; set; }

	[JsonProperty("fslPackageCharge", Order = 15)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fslPackageCharge { get; set; }

	[JsonProperty("fslPackageFullWeight", Order = 16)]
	[Required(ErrorMessage = "fslPackageFullWeight is required.")]
	[Range(0.0, 999999.9, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fslPackageFullWeight { get; set; }

	[JsonProperty("fslPackagePublishedCharge", Order = 17)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal fslPackagePublishedCharge { get; set; }

	[JsonProperty("fslRowVersion", Order = 18)]
	public byte[] fslRowVersion { get; set; }

	[JsonProperty("fslFreightPackageID", Order = 19)]
	[Required(ErrorMessage = "fslFreightPackageID is required.")]
	public short fslFreightPackageID { get; set; }

	[JsonProperty("fslTrackingNumber", Order = 20)]
	[MaxLength(50)]
	public string fslTrackingNumber { get; set; }

	[JsonProperty("fslUpsPackageType", Order = 21)]
	[MaxLength(35)]
	public string fslUpsPackageType { get; set; }

	[JsonProperty("fslWeightUnitOfMeasure", Order = 22)]
	[MaxLength(3)]
	public string fslWeightUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 23)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
