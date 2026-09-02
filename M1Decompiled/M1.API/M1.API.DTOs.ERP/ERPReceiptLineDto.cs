using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPReceiptLineDto
{
	[JsonProperty("rmlConversionFactor", Order = 1)]
	[Range(0.0, 999999.99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlConversionFactor { get; set; }

	[JsonProperty("rmlCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string rmlCreatedBy { get; set; }

	[JsonProperty("rmlCreatedDate", Order = 3)]
	public DateTime? rmlCreatedDate { get; set; }

	[JsonProperty("rmlDescription", Order = 4)]
	[MaxLength(50)]
	public string rmlDescription { get; set; }

	[JsonProperty("rmlDmrClaimID", Order = 5)]
	[MaxLength(10)]
	public string rmlDmrClaimID { get; set; }

	[JsonProperty("rmlDmrClaimLineID", Order = 6)]
	public short rmlDmrClaimLineID { get; set; }

	[JsonProperty("rmlDutyUnitCost", Order = 7)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlDutyUnitCost { get; set; }

	[JsonProperty("rmlUniqueID", Order = 8)]
	public Guid rmlUniqueID { get; set; }

	[JsonProperty("rmlExtendedCostBase", Order = 9)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlExtendedCostBase { get; set; }

	[JsonProperty("rmlExtendedCostForeign", Order = 10)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlExtendedCostForeign { get; set; }

	[JsonProperty("rmlForm1099Box", Order = 11)]
	public byte rmlForm1099Box { get; set; }

	[JsonProperty("rmlFreightUnitCost", Order = 12)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlFreightUnitCost { get; set; }

	[JsonProperty("rmlHeatLot", Order = 13)]
	[MaxLength(50)]
	public string rmlHeatLot { get; set; }

	[JsonProperty("rmlInspectionNotesRTF", Order = 14)]
	[MaxLength(50)]
	public string rmlInspectionNotesRTF { get; set; }

	[JsonProperty("rmlInspectionNotesText", Order = 15)]
	[MaxLength(50)]
	public string rmlInspectionNotesText { get; set; }

	[JsonProperty("rmlInventoryQuantityReceived", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlInventoryQuantityReceived { get; set; }

	[JsonProperty("rmlInventoryUnitCost", Order = 17)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlInventoryUnitCost { get; set; }

	[JsonProperty("rmlInventoryUnitCostForeign", Order = 18)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlInventoryUnitCostForeign { get; set; }

	[JsonProperty("rmlInventoryUnitOfMeasure", Order = 19)]
	[MaxLength(2)]
	public string rmlInventoryUnitOfMeasure { get; set; }

	[JsonProperty("rmlClosed", Order = 20)]
	public bool rmlClosed { get; set; }

	[JsonProperty("rmlInInspection", Order = 21)]
	public bool rmlInInspection { get; set; }

	[JsonProperty("rmlInspectionComplete", Order = 22)]
	public bool rmlInspectionComplete { get; set; }

	[JsonProperty("rmlInvoicedComplete", Order = 23)]
	public bool rmlInvoicedComplete { get; set; }

	[JsonProperty("rmlJobReceivedComplete", Order = 24)]
	public bool rmlJobReceivedComplete { get; set; }

	[JsonProperty("rmlKitPart", Order = 25)]
	public bool rmlKitPart { get; set; }

	[JsonProperty("rmlPoReceivedComplete", Order = 26)]
	public bool rmlPoReceivedComplete { get; set; }

	[JsonProperty("rmlPostedToGl", Order = 27)]
	public bool rmlPostedToGl { get; set; }

	[JsonProperty("rmlRequiresInspection", Order = 28)]
	public bool rmlRequiresInspection { get; set; }

	[JsonProperty("rmlReversed", Order = 29)]
	public bool rmlReversed { get; set; }

	[JsonProperty("rmlTrackSerialNumbers", Order = 30)]
	public bool rmlTrackSerialNumbers { get; set; }

	[JsonProperty("rmlJobAssemblyID", Order = 31)]
	public int rmlJobAssemblyID { get; set; }

	[JsonProperty("rmlJobEstimatedQuantity", Order = 32)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlJobEstimatedQuantity { get; set; }

	[JsonProperty("rmlJobID", Order = 33)]
	[MaxLength(20)]
	public string rmlJobID { get; set; }

	[JsonProperty("rmlJobMaterialID", Order = 34)]
	public int rmlJobMaterialID { get; set; }

	[JsonProperty("rmlJobMatQuantityReceived", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlJobMatQuantityReceived { get; set; }

	[JsonProperty("rmlJobOpenQuantity", Order = 36)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlJobOpenQuantity { get; set; }

	[JsonProperty("rmlJobOperationID", Order = 37)]
	public int rmlJobOperationID { get; set; }

	[JsonProperty("rmlJobOprQuantityReceived", Order = 38)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlJobOprQuantityReceived { get; set; }

	[JsonProperty("rmlJobType", Order = 39)]
	public byte rmlJobType { get; set; }

	[JsonProperty("rmlMiscUnitCost", Order = 40)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlMiscUnitCost { get; set; }

	[JsonProperty("rmlOrgPartID", Order = 41)]
	[MaxLength(30)]
	public string rmlOrgPartID { get; set; }

	[JsonProperty("rmlOrgPartShortDescription", Order = 42)]
	[MaxLength(50)]
	public string rmlOrgPartShortDescription { get; set; }

	[JsonProperty("rmlPartBinID", Order = 43)]
	[MaxLength(15)]
	public string rmlPartBinID { get; set; }

	[JsonProperty("rmlPartID", Order = 44)]
	[MaxLength(30)]
	public string rmlPartID { get; set; }

	[JsonProperty("rmlPartLongDescriptionRtf", Order = 45)]
	public string rmlPartLongDescriptionRtf { get; set; }

	[JsonProperty("rmlPartLongDescriptionText", Order = 46)]
	public string rmlPartLongDescriptionText { get; set; }

	[JsonProperty("rmlPartRevisionID", Order = 47)]
	[MaxLength(15)]
	public string rmlPartRevisionID { get; set; }

	[JsonProperty("rmlPartWarehouseLocationID", Order = 48)]
	[MaxLength(5)]
	public string rmlPartWarehouseLocationID { get; set; }

	[JsonProperty("rmlPoOpenQuantity", Order = 49)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlPoOpenQuantity { get; set; }

	[JsonProperty("rmlPoPurchaseQuantity", Order = 50)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlPoPurchaseQuantity { get; set; }

	[JsonProperty("rmlProjectAreaID", Order = 51)]
	[MaxLength(15)]
	public string rmlProjectAreaID { get; set; }

	[JsonProperty("rmlProjectID", Order = 52)]
	[MaxLength(10)]
	public string rmlProjectID { get; set; }

	[JsonProperty("rmlPurchaseOrderID", Order = 53)]
	[MaxLength(10)]
	public string rmlPurchaseOrderID { get; set; }

	[JsonProperty("rmlPurchaseOrderLineID", Order = 54)]
	public short rmlPurchaseOrderLineID { get; set; }

	[JsonProperty("rmlPurchaseQuantityReceived", Order = 55)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlPurchaseQuantityReceived { get; set; }

	[JsonProperty("rmlPurchaseUnitCost", Order = 56)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlPurchaseUnitCost { get; set; }

	[JsonProperty("rmlPurchaseUnitCostForeign", Order = 57)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlPurchaseUnitCostForeign { get; set; }

	[JsonProperty("rmlPurchaseUnitOfMeasure", Order = 58)]
	[MaxLength(2)]
	public string rmlPurchaseUnitOfMeasure { get; set; }

	[JsonProperty("rmlQuantityToInspect", Order = 59)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlQuantityToInspect { get; set; }

	[JsonProperty("rmlReceiptID", Order = 60)]
	[Required(ErrorMessage = "rmlReceiptID is required.")]
	[MaxLength(10)]
	public string rmlReceiptID { get; set; }

	[JsonProperty("rmlReference", Order = 61)]
	[MaxLength(30)]
	public string rmlReference { get; set; }

	[JsonProperty("rmlReverseReceiptID", Order = 62)]
	[MaxLength(10)]
	public string rmlReverseReceiptID { get; set; }

	[JsonProperty("rmlReverseReceiptLineID", Order = 63)]
	public short rmlReverseReceiptLineID { get; set; }

	[JsonProperty("rmlRmaClaimID", Order = 64)]
	[MaxLength(10)]
	public string rmlRmaClaimID { get; set; }

	[JsonProperty("rmlRmaClaimLineID", Order = 65)]
	public short rmlRmaClaimLineID { get; set; }

	[JsonProperty("rmlRowVersion", Order = 66)]
	public byte[] rmlRowVersion { get; set; }

	[JsonProperty("rmlSalesOrderDeliveryID", Order = 67)]
	public short rmlSalesOrderDeliveryID { get; set; }

	[JsonProperty("rmlSalesOrderID", Order = 68)]
	[MaxLength(10)]
	public string rmlSalesOrderID { get; set; }

	[JsonProperty("rmlSalesOrderLineID", Order = 69)]
	public short rmlSalesOrderLineID { get; set; }

	[JsonProperty("rmlReceiptLineID", Order = 70)]
	[Required(ErrorMessage = "rmlReceiptLineID is required.")]
	public short rmlReceiptLineID { get; set; }

	[JsonProperty("rmlSetupCharge", Order = 71)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlSetupCharge { get; set; }

	[JsonProperty("rmlSetupChargeForeign", Order = 72)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlSetupChargeForeign { get; set; }

	[JsonProperty("rmlTotalComponentCosts", Order = 73)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmlTotalComponentCosts { get; set; }

	[JsonProperty("customFields", Order = 74)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
