using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPShipmentPackageDetailDto
{
	[JsonProperty("spdCommodityDescription", Order = 1)]
	[MaxLength(35)]
	public string spdCommodityDescription { get; set; }

	[JsonProperty("spdCountryOfManufacture", Order = 2)]
	[MaxLength(2)]
	public string spdCountryOfManufacture { get; set; }

	[JsonProperty("spdCreatedBy", Order = 3)]
	[MaxLength(20)]
	public string spdCreatedBy { get; set; }

	[JsonProperty("spdCreatedDate", Order = 4)]
	public DateTime? spdCreatedDate { get; set; }

	[JsonProperty("spdUniqueID", Order = 5)]
	public Guid spdUniqueID { get; set; }

	[JsonProperty("spdPartID", Order = 6)]
	[MaxLength(30)]
	public string spdPartID { get; set; }

	[JsonProperty("spdPartRevisionID", Order = 7)]
	[MaxLength(15)]
	public string spdPartRevisionID { get; set; }

	[JsonProperty("spdQuantity", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal spdQuantity { get; set; }

	[JsonProperty("SPDRowVersion", Order = 9)]
	public byte[] spdRowVersion { get; set; }

	[JsonProperty("spdShipmentID", Order = 10)]
	[MaxLength(10)]
	public string spdShipmentID { get; set; }

	[JsonProperty("spdShipmentIDNumber", Order = 11)]
	[MaxLength(20)]
	public string spdShipmentIDNumber { get; set; }

	[JsonProperty("spdShipmentLineID", Order = 12)]
	public short spdShipmentLineID { get; set; }

	[JsonProperty("spdShipmentPackageID", Order = 13)]
	[Required(ErrorMessage = "spdShipmentPackageID is required.")]
	public int spdShipmentPackageID { get; set; }

	[JsonProperty("spdShipmentPackageLineID", Order = 14)]
	public int spdShipmentPackageLineID { get; set; }

	[JsonProperty("spdTotalPriceBase", Order = 15)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal spdTotalPriceBase { get; set; }

	[JsonProperty("spdTotalPriceForeign", Order = 16)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal spdTotalPriceForeign { get; set; }

	[JsonProperty("spdWeight", Order = 17)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal spdWeight { get; set; }

	[JsonProperty("spdWeightUnitOfMeasure", Order = 18)]
	[MaxLength(3)]
	public string spdWeightUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 19)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
