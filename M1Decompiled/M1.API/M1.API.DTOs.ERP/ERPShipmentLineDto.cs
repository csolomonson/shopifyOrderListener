using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPShipmentLineDto
{
	[JsonProperty("smlCreatedBy", Order = 1)]
	[MaxLength(20)]
	public string smlCreatedBy { get; set; }

	[JsonProperty("smlCreatedDate", Order = 2)]
	public DateTime? smlCreatedDate { get; set; }

	[JsonProperty("smlDescription", Order = 3)]
	[MaxLength(50)]
	public string smlDescription { get; set; }

	[JsonProperty("smlUniqueID", Order = 4)]
	public Guid smlUniqueID { get; set; }

	[JsonProperty("smlExtendedPriceBase", Order = 5)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smlExtendedPriceBase { get; set; }

	[JsonProperty("smlExtendedPriceForeign", Order = 6)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smlExtendedPriceForeign { get; set; }

	[JsonProperty("smlExtendedWeight", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smlExtendedWeight { get; set; }

	[JsonProperty("smlFreightAmount", Order = 8)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smlFreightAmount { get; set; }

	[JsonProperty("smlFreightAmountForeign", Order = 9)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smlFreightAmountForeign { get; set; }

	[JsonProperty("smlHeatLot", Order = 10)]
	[MaxLength(50)]
	public string smlHeatLot { get; set; }

	[JsonProperty("smlClosed", Order = 11)]
	public bool smlClosed { get; set; }

	[JsonProperty("smlInvoicedComplete", Order = 12)]
	public bool smlInvoicedComplete { get; set; }

	[JsonProperty("smlKitPart", Order = 13)]
	public bool smlKitPart { get; set; }

	[JsonProperty("smlOverridePrice", Order = 14)]
	public bool smlOverridePrice { get; set; }

	[JsonProperty("smlPostedToGl", Order = 15)]
	public bool smlPostedToGl { get; set; }

	[JsonProperty("smlRequiresInspection", Order = 16)]
	public bool smlRequiresInspection { get; set; }

	[JsonProperty("smlReversed", Order = 17)]
	public bool smlReversed { get; set; }

	[JsonProperty("smlShippedComplete", Order = 18)]
	public bool smlShippedComplete { get; set; }

	[JsonProperty("smlJobID", Order = 19)]
	[MaxLength(20)]
	public string smlJobID { get; set; }

	[JsonProperty("smlJobQuantityShipped", Order = 20)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smlJobQuantityShipped { get; set; }

	[JsonProperty("smlOrgPartID", Order = 21)]
	[MaxLength(30)]
	public string smlOrgPartID { get; set; }

	[JsonProperty("smlOrgPartShortDescription", Order = 22)]
	[MaxLength(50)]
	public string smlOrgPartShortDescription { get; set; }

	[JsonProperty("smlPartBinID", Order = 23)]
	[Required(ErrorMessage = "smlPartBinID is required.")]
	[MaxLength(15)]
	public string smlPartBinID { get; set; }

	[JsonProperty("smlPartGroupID", Order = 24)]
	[MaxLength(5)]
	public string smlPartGroupID { get; set; }

	[JsonProperty("smlPartID", Order = 25)]
	[Required(ErrorMessage = "smlPartID is required.")]
	[MaxLength(30)]
	public string smlPartID { get; set; }

	[JsonProperty("smlPartLongDescriptionRtf", Order = 26)]
	public string smlPartLongDescriptionRtf { get; set; }

	[JsonProperty("smlPartLongDescriptionText", Order = 27)]
	public string smlPartLongDescriptionText { get; set; }

	[JsonProperty("smlPartRevisionID", Order = 28)]
	[MaxLength(15)]
	public string smlPartRevisionID { get; set; }

	[JsonProperty("smlPartWarehouseLocationID", Order = 29)]
	[Required(ErrorMessage = "smlPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string smlPartWarehouseLocationID { get; set; }

	[JsonProperty("smlProjectAreaID", Order = 30)]
	[MaxLength(15)]
	public string smlProjectAreaID { get; set; }

	[JsonProperty("smlProjectID", Order = 31)]
	[MaxLength(10)]
	public string smlProjectID { get; set; }

	[JsonProperty("smlQuantityShipped", Order = 32)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smlQuantityShipped { get; set; }

	[JsonProperty("smlReverseShipmentID", Order = 33)]
	[MaxLength(10)]
	public string smlReverseShipmentID { get; set; }

	[JsonProperty("smlReverseShipmentLineID", Order = 34)]
	public short smlReverseShipmentLineID { get; set; }

	[JsonProperty("smlRowVersion", Order = 35)]
	public byte[] smlRowVersion { get; set; }

	[JsonProperty("smlSalesOrderDeliveryID", Order = 36)]
	public short smlSalesOrderDeliveryID { get; set; }

	[JsonProperty("smlSalesOrderID", Order = 37)]
	[MaxLength(10)]
	public string smlSalesOrderID { get; set; }

	[JsonProperty("smlSalesOrderLineID", Order = 38)]
	public short smlSalesOrderLineID { get; set; }

	[JsonProperty("smlShipmentLineID", Order = 39)]
	[Required(ErrorMessage = "smlShipmentLineID is required.")]
	public short smlShipmentLineID { get; set; }

	[JsonProperty("smlShipmentID", Order = 40)]
	[Required(ErrorMessage = "smlShipmentID is required.")]
	[MaxLength(10)]
	public string smlShipmentID { get; set; }

	[JsonProperty("smlShipmentIDNumber", Order = 41)]
	[MaxLength(20)]
	public string smlShipmentIDNumber { get; set; }

	[JsonProperty("smlSODeliveryQuantity", Order = 42)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smlSODeliveryQuantity { get; set; }

	[JsonProperty("smlSOOpenQuantity", Order = 43)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smlSOOpenQuantity { get; set; }

	[JsonProperty("smlSourceTableName", Order = 44)]
	[MaxLength(30)]
	public string smlSourceTableName { get; set; }

	[JsonProperty("smlSourceTableUniqueID", Order = 45)]
	public Guid smlSourceTableUniqueID { get; set; }

	[JsonProperty("smlUnitOfMeasure", Order = 46)]
	[MaxLength(2)]
	public string smlUnitOfMeasure { get; set; }

	[JsonProperty("smlUnitPrice", Order = 47)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smlUnitPrice { get; set; }

	[JsonProperty("smlUnitPriceForeign", Order = 48)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smlUnitPriceForeign { get; set; }

	[JsonProperty("smlWeight", Order = 49)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal smlWeight { get; set; }

	[JsonProperty("smlWeightUnitOfMeasure", Order = 50)]
	[MaxLength(3)]
	public string smlWeightUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 51)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
