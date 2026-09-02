using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPShipmentPackageDto
{
	[JsonProperty("spaCarrier", Order = 1)]
	[MaxLength(5)]
	public string spaCarrier { get; set; }

	[JsonProperty("spaCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string spaCreatedBy { get; set; }

	[JsonProperty("spaCreatedDate", Order = 3)]
	public DateTime? spaCreatedDate { get; set; }

	[JsonProperty("spaCustomerPackageID", Order = 4)]
	[MaxLength(10)]
	public string spaCustomerPackageID { get; set; }

	[JsonProperty("spaEdi856CustomLabel", Order = 5)]
	[MaxLength(35)]
	public string spaEdi856CustomLabel { get; set; }

	[JsonProperty("spaUniqueID", Order = 6)]
	public Guid spaUniqueID { get; set; }

	[JsonProperty("spaFedExPackageTypes", Order = 7)]
	[MaxLength(20)]
	public string spaFedExPackageTypes { get; set; }

	[JsonProperty("spaAdditionalHandlingRequired", Order = 8)]
	public bool spaAdditionalHandlingRequired { get; set; }

	[JsonProperty("spaLargePackage", Order = 9)]
	public bool spaLargePackage { get; set; }

	[JsonProperty("spaVerbalConfirmationRequired", Order = 10)]
	public bool spaVerbalConfirmationRequired { get; set; }

	[JsonProperty("spaLabelFilePath", Order = 11)]
	[MaxLength(50)]
	public string spaLabelFilePath { get; set; }

	[JsonProperty("spaPackageDimensionsUom", Order = 12)]
	[MaxLength(2)]
	public string spaPackageDimensionsUom { get; set; }

	[JsonProperty("spaPackageHeight", Order = 13)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int spaPackageHeight { get; set; }

	[JsonProperty("spaPackageLength", Order = 14)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int spaPackageLength { get; set; }

	[JsonProperty("spaPackageRate", Order = 15)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal spaPackageRate { get; set; }

	[JsonProperty("spaPackageRateForeign", Order = 16)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal spaPackageRateForeign { get; set; }

	[JsonProperty("spaPackageValue", Order = 17)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal spaPackageValue { get; set; }

	[JsonProperty("spaPackageValueForeign", Order = 18)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal spaPackageValueForeign { get; set; }

	[JsonProperty("spaPackageWeight", Order = 19)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal spaPackageWeight { get; set; }

	[JsonProperty("spaPackageWeightUom", Order = 20)]
	[MaxLength(3)]
	public string spaPackageWeightUom { get; set; }

	[JsonProperty("spaPackageWidth", Order = 21)]
	[Range(0, 999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int spaPackageWidth { get; set; }

	[JsonProperty("spaReference1", Order = 22)]
	[MaxLength(35)]
	public string spaReference1 { get; set; }

	[JsonProperty("spaReference2", Order = 23)]
	[MaxLength(35)]
	public string spaReference2 { get; set; }

	[JsonProperty("SPArowVersion", Order = 24)]
	public byte[] spaRowVersion { get; set; }

	[JsonProperty("spaShipmentPackageID", Order = 25)]
	[Range(0, 99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public int spaShipmentPackageID { get; set; }

	[JsonProperty("spaShipmentID", Order = 26)]
	[MaxLength(10)]
	public string spaShipmentID { get; set; }

	[JsonProperty("spaShipmentIDNumber", Order = 27)]
	[MaxLength(20)]
	public string spaShipmentIDNumber { get; set; }

	[JsonProperty("spaShippingMethodID", Order = 28)]
	[MaxLength(5)]
	public string spaShippingMethodID { get; set; }

	[JsonProperty("spaTrackingNo", Order = 29)]
	[MaxLength(20)]
	public string spaTrackingNo { get; set; }

	[JsonProperty("spaUpsPackageTypes", Order = 30)]
	[MaxLength(20)]
	public string spaUpsPackageTypes { get; set; }

	[JsonProperty("customFields", Order = 31)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
