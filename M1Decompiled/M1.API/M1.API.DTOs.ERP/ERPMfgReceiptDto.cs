using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPMfgReceiptDto
{
	[JsonProperty("rmmMfgReceiptID", Order = 1)]
	[MaxLength(10)]
	public string rmmMfgReceiptID { get; set; }

	[JsonProperty("rmmCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string rmmCreatedBy { get; set; }

	[JsonProperty("rmmCreatedDate", Order = 3)]
	public DateTime? rmmCreatedDate { get; set; }

	[JsonProperty("rmmUniqueID", Order = 4)]
	public Guid rmmUniqueID { get; set; }

	[JsonProperty("rmmEstimatedQuantity", Order = 5)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmEstimatedQuantity { get; set; }

	[JsonProperty("rmmExtendedCostBase", Order = 6)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmExtendedCostBase { get; set; }

	[JsonProperty("rmmHeatLot", Order = 7)]
	[MaxLength(50)]
	public string rmmHeatLot { get; set; }

	[JsonProperty("rmmImCostingMethod", Order = 8)]
	public byte rmmImCostingMethod { get; set; }

	[JsonProperty("rmmInventoryQuantity", Order = 9)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmInventoryQuantity { get; set; }

	[JsonProperty("rmmInventoryQuantityReceived", Order = 10)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmInventoryQuantityReceived { get; set; }

	[JsonProperty("rmmInventoryUnitOfMeasure", Order = 11)]
	[MaxLength(2)]
	public string rmmInventoryUnitOfMeasure { get; set; }

	[JsonProperty("rmmCreateJobSeq", Order = 12)]
	public bool rmmCreateJobSeq { get; set; }

	[JsonProperty("rmmInInspection", Order = 13)]
	public bool rmmInInspection { get; set; }

	[JsonProperty("rmmInspectionComplete", Order = 14)]
	public bool rmmInspectionComplete { get; set; }

	[JsonProperty("rmmKitPart", Order = 15)]
	public bool rmmKitPart { get; set; }

	[JsonProperty("rmmNotUpdateJobQtyComplete", Order = 16)]
	public bool rmmNotUpdateJobQtyComplete { get; set; }

	[JsonProperty("rmmPoLineReceivedComplete", Order = 17)]
	public bool rmmPoLineReceivedComplete { get; set; }

	[JsonProperty("rmmPosted", Order = 18)]
	public bool rmmPosted { get; set; }

	[JsonProperty("rmmProductionComplete", Order = 19)]
	public bool rmmProductionComplete { get; set; }

	[JsonProperty("rmmReceivedComplete", Order = 20)]
	public bool rmmReceivedComplete { get; set; }

	[JsonProperty("rmmRequiresInspection", Order = 21)]
	public bool rmmRequiresInspection { get; set; }

	[JsonProperty("rmmReversalEntry", Order = 22)]
	public bool rmmReversalEntry { get; set; }

	[JsonProperty("rmmReversed", Order = 23)]
	public bool rmmReversed { get; set; }

	[JsonProperty("rmmJobAsmQuantityReceived", Order = 24)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmJobAsmQuantityReceived { get; set; }

	[JsonProperty("rmmJobAssemblyID", Order = 25)]
	public int rmmJobAssemblyID { get; set; }

	[JsonProperty("rmmJobID", Order = 26)]
	[MaxLength(20)]
	public string rmmJobID { get; set; }

	[JsonProperty("rmmJobMaterialID", Order = 27)]
	public int rmmJobMaterialID { get; set; }

	[JsonProperty("rmmJobMatQuantityReceived", Order = 28)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmJobMatQuantityReceived { get; set; }

	[JsonProperty("rmmJobOpenQuantity", Order = 29)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmJobOpenQuantity { get; set; }

	[JsonProperty("rmmJobOperationID", Order = 30)]
	public int rmmJobOperationID { get; set; }

	[JsonProperty("rmmJobOprQuantityReceived", Order = 31)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmJobOprQuantityReceived { get; set; }

	[JsonProperty("rmmJobScrapQuantity", Order = 32)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmJobScrapQuantity { get; set; }

	[JsonProperty("rmmJobType", Order = 33)]
	public byte rmmJobType { get; set; }

	[JsonProperty("rmmLongDescriptionRtf", Order = 34)]
	public string rmmLongDescriptionRtf { get; set; }

	[JsonProperty("rmmLongDescriptionText", Order = 35)]
	public string rmmLongDescriptionText { get; set; }

	[JsonProperty("rmmMfgCostType", Order = 36)]
	[Required(ErrorMessage = "rmmMfgCostType is required.")]
	public byte rmmMfgCostType { get; set; }

	[JsonProperty("rmmMiscInvQuantityReceived", Order = 37)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmMiscInvQuantityReceived { get; set; }

	[JsonProperty("rmmPartBinID", Order = 38)]
	[Required(ErrorMessage = "rmmPartBinID is required.")]
	[MaxLength(15)]
	public string rmmPartBinID { get; set; }

	[JsonProperty("rmmPartID", Order = 39)]
	[Required(ErrorMessage = "rmmPartID is required.")]
	[MaxLength(30)]
	public string rmmPartID { get; set; }

	[JsonProperty("rmmPartRevisionID", Order = 40)]
	[MaxLength(15)]
	public string rmmPartRevisionID { get; set; }

	[JsonProperty("rmmPartWarehouseLocationID", Order = 41)]
	[Required(ErrorMessage = "rmmPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string rmmPartWarehouseLocationID { get; set; }

	[JsonProperty("rmmPlantDepartmentID", Order = 42)]
	[MaxLength(5)]
	public string rmmPlantDepartmentID { get; set; }

	[JsonProperty("rmmPlantID", Order = 43)]
	[MaxLength(5)]
	public string rmmPlantID { get; set; }

	[JsonProperty("rmmPoOpenQuantity", Order = 44)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmPoOpenQuantity { get; set; }

	[JsonProperty("rmmPostedDate", Order = 45)]
	public DateTime? rmmPostedDate { get; set; }

	[JsonProperty("rmmProductionQuantity", Order = 46)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmProductionQuantity { get; set; }

	[JsonProperty("rmmProjectAreaID", Order = 47)]
	[MaxLength(15)]
	public string rmmProjectAreaID { get; set; }

	[JsonProperty("rmmProjectID", Order = 48)]
	[MaxLength(10)]
	public string rmmProjectID { get; set; }

	[JsonProperty("rmmPurchaseLocationID", Order = 49)]
	[MaxLength(5)]
	public string rmmPurchaseLocationID { get; set; }

	[JsonProperty("rmmPurchaseOrderID", Order = 50)]
	[MaxLength(10)]
	public string rmmPurchaseOrderID { get; set; }

	[JsonProperty("rmmPurchaseOrderLineID", Order = 51)]
	public short rmmPurchaseOrderLineID { get; set; }

	[JsonProperty("rmmPurchaseQuantity", Order = 52)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmPurchaseQuantity { get; set; }

	[JsonProperty("rmmPurchaseQuantityReceived", Order = 53)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmPurchaseQuantityReceived { get; set; }

	[JsonProperty("rmmPurchaseUnitCost", Order = 54)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmPurchaseUnitCost { get; set; }

	[JsonProperty("rmmPurchaseUnitOfMeasure", Order = 55)]
	[MaxLength(2)]
	public string rmmPurchaseUnitOfMeasure { get; set; }

	[JsonProperty("rmmQuantityCompleted", Order = 56)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmQuantityCompleted { get; set; }

	[JsonProperty("rmmQuantityOnHand", Order = 57)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmQuantityOnHand { get; set; }

	[JsonProperty("rmmQuantityReceivedToInventory", Order = 58)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmQuantityReceivedToInventory { get; set; }

	[JsonProperty("rmmQuantityToInspect", Order = 59)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmQuantityToInspect { get; set; }

	[JsonProperty("rmmReceiptDate", Order = 60)]
	[Required(ErrorMessage = "rmmReceiptDate is required.")]
	public DateTime? rmmReceiptDate { get; set; }

	[JsonProperty("rmmReceiptType", Order = 61)]
	[Required(ErrorMessage = "rmmReceiptType is required.")]
	public byte rmmReceiptType { get; set; }

	[JsonProperty("rmmReference", Order = 62)]
	[MaxLength(30)]
	public string rmmReference { get; set; }

	[JsonProperty("rmmReverseMfgReceiptID", Order = 63)]
	[MaxLength(10)]
	public string rmmReverseMfgReceiptID { get; set; }

	[JsonProperty("rmmRowVersion", Order = 64)]
	public byte[] rmmRowVersion { get; set; }

	[JsonProperty("rmmScrapQuantity", Order = 65)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmScrapQuantity { get; set; }

	[JsonProperty("rmmSetupCharge", Order = 66)]
	[Range(0.0, 9999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmSetupCharge { get; set; }

	[JsonProperty("rmmSupplierOrganizationID", Order = 67)]
	[MaxLength(10)]
	public string rmmSupplierOrganizationID { get; set; }

	[JsonProperty("rmmTotalComponentCosts", Order = 68)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmTotalComponentCosts { get; set; }

	[JsonProperty("rmmTotalUnitCost", Order = 69)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmTotalUnitCost { get; set; }

	[JsonProperty("rmmUnitLaborCost", Order = 70)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmUnitLaborCost { get; set; }

	[JsonProperty("rmmUnitMaterialCost", Order = 71)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmUnitMaterialCost { get; set; }

	[JsonProperty("rmmUnitOverheadCost", Order = 72)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmUnitOverheadCost { get; set; }

	[JsonProperty("rmmUnitSubcontractCost", Order = 73)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal rmmUnitSubcontractCost { get; set; }

	[JsonProperty("customFields", Order = 74)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
