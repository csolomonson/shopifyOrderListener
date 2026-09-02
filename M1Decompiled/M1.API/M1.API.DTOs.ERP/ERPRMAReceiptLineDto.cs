using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPRMAReceiptLineDto
{
	[JsonProperty("rrlConversionFactor", Order = 1)]
	[Range(0.0, 999999.99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrlConversionFactor { get; set; }

	[JsonProperty("rrlCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string rrlCreatedBy { get; set; }

	[JsonProperty("rrlCreatedDate", Order = 3)]
	public DateTime? rrlCreatedDate { get; set; }

	[JsonProperty("rrlDescription", Order = 4)]
	[MaxLength(50)]
	public string rrlDescription { get; set; }

	[JsonProperty("rrlUniqueID", Order = 5)]
	public Guid rrlUniqueID { get; set; }

	[JsonProperty("rrlExtendedCost", Order = 6)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrlExtendedCost { get; set; }

	[JsonProperty("rrlExtendedCostForeign", Order = 7)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrlExtendedCostForeign { get; set; }

	[JsonProperty("rrlHeatLot", Order = 8)]
	[MaxLength(50)]
	public string rrlHeatLot { get; set; }

	[JsonProperty("rrlInventoryQuantityReceived", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrlInventoryQuantityReceived { get; set; }

	[JsonProperty("rrlInventoryUnitOfMeasure", Order = 10)]
	[MaxLength(2)]
	public string rrlInventoryUnitOfMeasure { get; set; }

	[JsonProperty("rrlClosed", Order = 11)]
	public bool rrlClosed { get; set; }

	[JsonProperty("rrlInInspection", Order = 12)]
	public bool rrlInInspection { get; set; }

	[JsonProperty("rrlInspectionComplete", Order = 13)]
	public bool rrlInspectionComplete { get; set; }

	[JsonProperty("rrlInvoicedComplete", Order = 14)]
	public bool rrlInvoicedComplete { get; set; }

	[JsonProperty("rrlKitPart", Order = 15)]
	public bool rrlKitPart { get; set; }

	[JsonProperty("rrlPosted", Order = 16)]
	public bool rrlPosted { get; set; }

	[JsonProperty("rrlReceivedComplete", Order = 17)]
	public bool rrlReceivedComplete { get; set; }

	[JsonProperty("rrlRequiresInspection", Order = 18)]
	public bool rrlRequiresInspection { get; set; }

	[JsonProperty("rrlReversed", Order = 19)]
	public bool rrlReversed { get; set; }

	[JsonProperty("rrlOrgPartID", Order = 20)]
	[MaxLength(30)]
	public string rrlOrgPartID { get; set; }

	[JsonProperty("rrlOrgPartShortDescription", Order = 21)]
	[MaxLength(50)]
	public string rrlOrgPartShortDescription { get; set; }

	[JsonProperty("rrlPartBinID", Order = 22)]
	[Required(ErrorMessage = "rrlPartBinID is required.")]
	[MaxLength(15)]
	public string rrlPartBinID { get; set; }

	[JsonProperty("rrlPartID", Order = 23)]
	[Required(ErrorMessage = "rrlPartID is required.")]
	[MaxLength(30)]
	public string rrlPartID { get; set; }

	[JsonProperty("rrlPartLongDescriptionRtf", Order = 24)]
	public string rrlPartLongDescriptionRtf { get; set; }

	[JsonProperty("rrlPartLongDescriptionText", Order = 25)]
	public string rrlPartLongDescriptionText { get; set; }

	[JsonProperty("rrlPartRevisionID", Order = 26)]
	[MaxLength(15)]
	public string rrlPartRevisionID { get; set; }

	[JsonProperty("rrlPartWarehouseLocationID", Order = 27)]
	[Required(ErrorMessage = "rrlPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string rrlPartWarehouseLocationID { get; set; }

	[JsonProperty("rrlProjectAreaID", Order = 28)]
	[MaxLength(15)]
	public string rrlProjectAreaID { get; set; }

	[JsonProperty("rrlProjectID", Order = 29)]
	[MaxLength(10)]
	public string rrlProjectID { get; set; }

	[JsonProperty("rrlQuantityToInspect", Order = 30)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrlQuantityToInspect { get; set; }

	[JsonProperty("rrlReference", Order = 31)]
	[MaxLength(30)]
	public string rrlReference { get; set; }

	[JsonProperty("rrlReverseRmaReceiptID", Order = 32)]
	[MaxLength(10)]
	public string rrlReverseRmaReceiptID { get; set; }

	[JsonProperty("rrlReverseRmaReceiptLineID", Order = 33)]
	public short rrlReverseRmaReceiptLineID { get; set; }

	[JsonProperty("rrlRmaClaimID", Order = 34)]
	[MaxLength(10)]
	public string rrlRmaClaimID { get; set; }

	[JsonProperty("rrlRmaClaimLineID", Order = 35)]
	public short rrlRmaClaimLineID { get; set; }

	[JsonProperty("rrlRmaClaimQuantity", Order = 36)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrlRmaClaimQuantity { get; set; }

	[JsonProperty("rrlRmaOpenQuantity", Order = 37)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrlRmaOpenQuantity { get; set; }

	[JsonProperty("rrlRmaReceiptID", Order = 38)]
	[Required(ErrorMessage = "rrlRmaReceiptID is required.")]
	[MaxLength(10)]
	public string rrlRmaReceiptID { get; set; }

	[JsonProperty("rrlRowVersion", Order = 39)]
	public byte[] rrlRowVersion { get; set; }

	[JsonProperty("rrlSalesQuantityReceived", Order = 40)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrlSalesQuantityReceived { get; set; }

	[JsonProperty("rrlSalesUnitOfMeasure", Order = 41)]
	[MaxLength(2)]
	public string rrlSalesUnitOfMeasure { get; set; }

	[JsonProperty("rrlRmaReceiptLineID", Order = 42)]
	[Required(ErrorMessage = "rrlRmaReceiptLineID is required.")]
	public short rrlRmaReceiptLineID { get; set; }

	[JsonProperty("rrlTotalComponentCosts", Order = 43)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrlTotalComponentCosts { get; set; }

	[JsonProperty("rrlUnitCost", Order = 44)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrlUnitCost { get; set; }

	[JsonProperty("rrlUnitCostForeign", Order = 45)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rrlUnitCostForeign { get; set; }

	[JsonProperty("customFields", Order = 46)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
