using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPDMRShipmentLineDto
{
	[JsonProperty("dslConversionFactor", Order = 1)]
	[Range(0.0, 999999.99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dslConversionFactor { get; set; }

	[JsonProperty("dslCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string dslCreatedBy { get; set; }

	[JsonProperty("dslCreatedDate", Order = 3)]
	public DateTime? dslCreatedDate { get; set; }

	[JsonProperty("dslDescription", Order = 4)]
	[MaxLength(50)]
	public string dslDescription { get; set; }

	[JsonProperty("dslDmrClaimID", Order = 5)]
	[MaxLength(10)]
	public string dslDmrClaimID { get; set; }

	[JsonProperty("dslDmrClaimLineID", Order = 6)]
	public short dslDmrClaimLineID { get; set; }

	[JsonProperty("dslDmrClaimQuantity", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dslDmrClaimQuantity { get; set; }

	[JsonProperty("dslDmrOpenQuantity", Order = 8)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dslDmrOpenQuantity { get; set; }

	[JsonProperty("dslDmrShipmentID", Order = 9)]
	[Required(ErrorMessage = "dslDmrShipmentID is required.")]
	[MaxLength(10)]
	public string dslDmrShipmentID { get; set; }

	[JsonProperty("dslUniqueID", Order = 10)]
	public Guid dslUniqueID { get; set; }

	[JsonProperty("dslInspectionID", Order = 11)]
	[MaxLength(10)]
	public string dslInspectionID { get; set; }

	[JsonProperty("dslInspectionLineID", Order = 12)]
	public short dslInspectionLineID { get; set; }

	[JsonProperty("dslInventoryQuantityShipped", Order = 13)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dslInventoryQuantityShipped { get; set; }

	[JsonProperty("dslInventoryUnitOfMeasure", Order = 14)]
	[MaxLength(2)]
	public string dslInventoryUnitOfMeasure { get; set; }

	[JsonProperty("dslClosed", Order = 15)]
	public bool dslClosed { get; set; }

	[JsonProperty("dslInvoicedComplete", Order = 16)]
	public bool dslInvoicedComplete { get; set; }

	[JsonProperty("dslKitPart", Order = 17)]
	public bool dslKitPart { get; set; }

	[JsonProperty("dslPosted", Order = 18)]
	public bool dslPosted { get; set; }

	[JsonProperty("dslReversed", Order = 19)]
	public bool dslReversed { get; set; }

	[JsonProperty("dslShippedComplete", Order = 20)]
	public bool dslShippedComplete { get; set; }

	[JsonProperty("dslJobAssemblyID", Order = 21)]
	public int dslJobAssemblyID { get; set; }

	[JsonProperty("dslJobID", Order = 22)]
	[MaxLength(20)]
	public string dslJobID { get; set; }

	[JsonProperty("dslJobMaterialID", Order = 23)]
	public int dslJobMaterialID { get; set; }

	[JsonProperty("dslJobMatQuantityShipped", Order = 24)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dslJobMatQuantityShipped { get; set; }

	[JsonProperty("dslJobOperationID", Order = 25)]
	public int dslJobOperationID { get; set; }

	[JsonProperty("dslJobOprQuantityShipped", Order = 26)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dslJobOprQuantityShipped { get; set; }

	[JsonProperty("dslPartBinID", Order = 27)]
	[MaxLength(15)]
	public string dslPartBinID { get; set; }

	[JsonProperty("dslPartID", Order = 28)]
	[MaxLength(30)]
	public string dslPartID { get; set; }

	[JsonProperty("dslPartLongDescriptionRtf", Order = 29)]
	public string dslPartLongDescriptionRtf { get; set; }

	[JsonProperty("dslPartLongDescriptionText", Order = 30)]
	public string dslPartLongDescriptionText { get; set; }

	[JsonProperty("dslPartRevisionID", Order = 31)]
	[MaxLength(15)]
	public string dslPartRevisionID { get; set; }

	[JsonProperty("dslPartWarehouseLocationID", Order = 32)]
	[MaxLength(5)]
	public string dslPartWarehouseLocationID { get; set; }

	[JsonProperty("dslProjectAreaID", Order = 33)]
	[MaxLength(15)]
	public string dslProjectAreaID { get; set; }

	[JsonProperty("dslProjectID", Order = 34)]
	[MaxLength(10)]
	public string dslProjectID { get; set; }

	[JsonProperty("dslQuantityShipped", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dslQuantityShipped { get; set; }

	[JsonProperty("dslReturnQuantityShipped", Order = 36)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dslReturnQuantityShipped { get; set; }

	[JsonProperty("dslReverseDmrShipmentID", Order = 37)]
	[MaxLength(10)]
	public string dslReverseDmrShipmentID { get; set; }

	[JsonProperty("dslReverseDmrShipmentLineID", Order = 38)]
	public short dslReverseDmrShipmentLineID { get; set; }

	[JsonProperty("dslRowVersion", Order = 39)]
	public byte[] dslRowVersion { get; set; }

	[JsonProperty("dslDmrShipmentLineID", Order = 40)]
	[Required(ErrorMessage = "dslDmrShipmentLineID is required.")]
	public short dslDmrShipmentLineID { get; set; }

	[JsonProperty("dslUnitOfMeasure", Order = 41)]
	[MaxLength(2)]
	public string dslUnitOfMeasure { get; set; }

	[JsonProperty("dslUnitPrice", Order = 42)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dslUnitPrice { get; set; }

	[JsonProperty("dslUnitPriceForeign", Order = 43)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dslUnitPriceForeign { get; set; }

	[JsonProperty("customFields", Order = 44)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
