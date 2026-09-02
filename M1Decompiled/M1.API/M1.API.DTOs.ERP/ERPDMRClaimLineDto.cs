using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPDMRClaimLineDto
{
	[JsonProperty("dmlConversionFactor", Order = 1)]
	[Range(0.0, 999999.99999999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmlConversionFactor { get; set; }

	[JsonProperty("dmlCreatedBy", Order = 2)]
	[MaxLength(20)]
	public string dmlCreatedBy { get; set; }

	[JsonProperty("dmlCreatedDate", Order = 3)]
	public DateTime? dmlCreatedDate { get; set; }

	[JsonProperty("dmlDmrClaimID", Order = 4)]
	[Required(ErrorMessage = "dmlDmrClaimID is required.")]
	[MaxLength(10)]
	public string dmlDmrClaimID { get; set; }

	[JsonProperty("dmlDmrShipmentID", Order = 5)]
	[MaxLength(10)]
	public string dmlDmrShipmentID { get; set; }

	[JsonProperty("dmlDmrShipmentLineID", Order = 6)]
	public short dmlDmrShipmentLineID { get; set; }

	[JsonProperty("dmlUniqueID", Order = 7)]
	public Guid dmlUniqueID { get; set; }

	[JsonProperty("dmlExtendedCost", Order = 8)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmlExtendedCost { get; set; }

	[JsonProperty("dmlExtendedCostForeign", Order = 9)]
	[Range(0.0, 9999999999.99, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmlExtendedCostForeign { get; set; }

	[JsonProperty("dmlInspectionID", Order = 10)]
	[MaxLength(10)]
	public string dmlInspectionID { get; set; }

	[JsonProperty("dmlInspectionLineID", Order = 11)]
	public short dmlInspectionLineID { get; set; }

	[JsonProperty("dmlInventoryQuantity", Order = 12)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmlInventoryQuantity { get; set; }

	[JsonProperty("dmlInventoryQuantityShipped", Order = 13)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmlInventoryQuantityShipped { get; set; }

	[JsonProperty("dmlInventoryUnitOfMeasure", Order = 14)]
	[MaxLength(2)]
	public string dmlInventoryUnitOfMeasure { get; set; }

	[JsonProperty("dmlInvoicedComplete", Order = 15)]
	public bool dmlInvoicedComplete { get; set; }

	[JsonProperty("dmlKitPart", Order = 16)]
	public bool dmlKitPart { get; set; }

	[JsonProperty("dmlScrap", Order = 17)]
	public bool dmlScrap { get; set; }

	[JsonProperty("dmlShippedComplete", Order = 18)]
	public bool dmlShippedComplete { get; set; }

	[JsonProperty("dmlTransferredToDmrShipment", Order = 19)]
	public bool dmlTransferredToDmrShipment { get; set; }

	[JsonProperty("dmlTransferredToPurchaseOrder", Order = 20)]
	public bool dmlTransferredToPurchaseOrder { get; set; }

	[JsonProperty("dmlJobAssemblyID", Order = 21)]
	public int dmlJobAssemblyID { get; set; }

	[JsonProperty("dmlJobID", Order = 22)]
	[MaxLength(20)]
	public string dmlJobID { get; set; }

	[JsonProperty("dmlJobMaterialID", Order = 23)]
	public int dmlJobMaterialID { get; set; }

	[JsonProperty("dmlJobOperationID", Order = 24)]
	public int dmlJobOperationID { get; set; }

	[JsonProperty("dmlOrgPartID", Order = 25)]
	[MaxLength(30)]
	public string dmlOrgPartID { get; set; }

	[JsonProperty("dmlOrgPartShortDescription", Order = 26)]
	[MaxLength(50)]
	public string dmlOrgPartShortDescription { get; set; }

	[JsonProperty("dmlPartBinID", Order = 27)]
	[Required(ErrorMessage = "dmlPartBinID is required.")]
	[MaxLength(15)]
	public string dmlPartBinID { get; set; }

	[JsonProperty("dmlPartID", Order = 28)]
	[Required(ErrorMessage = "dmlPartID is required.")]
	[MaxLength(30)]
	public string dmlPartID { get; set; }

	[JsonProperty("dmlPartLongDescriptionRtf", Order = 29)]
	public string dmlPartLongDescriptionRtf { get; set; }

	[JsonProperty("dmlPartLongDescriptionText", Order = 30)]
	public string dmlPartLongDescriptionText { get; set; }

	[JsonProperty("dmlPartRevisionID", Order = 31)]
	[MaxLength(15)]
	public string dmlPartRevisionID { get; set; }

	[JsonProperty("dmlPartShortDescription", Order = 32)]
	[Required(ErrorMessage = "dmlPartShortDescription is required.")]
	[MaxLength(50)]
	public string dmlPartShortDescription { get; set; }

	[JsonProperty("dmlPartWarehouseLocationID", Order = 33)]
	[Required(ErrorMessage = "dmlPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string dmlPartWarehouseLocationID { get; set; }

	[JsonProperty("dmlProjectAreaID", Order = 34)]
	[MaxLength(15)]
	public string dmlProjectAreaID { get; set; }

	[JsonProperty("dmlProjectID", Order = 35)]
	[MaxLength(10)]
	public string dmlProjectID { get; set; }

	[JsonProperty("dmlPurchaseOrderID", Order = 36)]
	[MaxLength(10)]
	public string dmlPurchaseOrderID { get; set; }

	[JsonProperty("dmlPurchaseOrderLineID", Order = 37)]
	public short dmlPurchaseOrderLineID { get; set; }

	[JsonProperty("dmlQuantity", Order = 38)]
	[Required(ErrorMessage = "dmlQuantity is required.")]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmlQuantity { get; set; }

	[JsonProperty("dmlQuantityShipped", Order = 39)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmlQuantityShipped { get; set; }

	[JsonProperty("dmlReceiptID", Order = 40)]
	[MaxLength(10)]
	public string dmlReceiptID { get; set; }

	[JsonProperty("dmlReceiptLineID", Order = 41)]
	public short dmlReceiptLineID { get; set; }

	[JsonProperty("dmlReceivedDate", Order = 42)]
	public DateTime? dmlReceivedDate { get; set; }

	[JsonProperty("dmlRequiredDate", Order = 43)]
	public DateTime? dmlRequiredDate { get; set; }

	[JsonProperty("dmlReturnedDate", Order = 44)]
	public DateTime? dmlReturnedDate { get; set; }

	[JsonProperty("dmlReturnReasonID", Order = 45)]
	[MaxLength(5)]
	public string dmlReturnReasonID { get; set; }

	[JsonProperty("dmlRowVersion", Order = 46)]
	public byte[] dmlRowVersion { get; set; }

	[JsonProperty("dmlDmrClaimLineID", Order = 47)]
	[Required(ErrorMessage = "dmlDmrClaimLineID is required.")]
	public short dmlDmrClaimLineID { get; set; }

	[JsonProperty("dmlShippedDate", Order = 48)]
	public DateTime? dmlShippedDate { get; set; }

	[JsonProperty("dmlShippingMethodID", Order = 49)]
	[MaxLength(5)]
	public string dmlShippingMethodID { get; set; }

	[JsonProperty("dmlSupplierAuthorizationNumber", Order = 50)]
	[MaxLength(20)]
	public string dmlSupplierAuthorizationNumber { get; set; }

	[JsonProperty("dmlTrackingNumber", Order = 51)]
	[MaxLength(30)]
	public string dmlTrackingNumber { get; set; }

	[JsonProperty("dmlUnitCost", Order = 52)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmlUnitCost { get; set; }

	[JsonProperty("dmlUnitCostForeign", Order = 53)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal dmlUnitCostForeign { get; set; }

	[JsonProperty("dmlUnitOfMeasure", Order = 54)]
	[MaxLength(2)]
	public string dmlUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 55)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
