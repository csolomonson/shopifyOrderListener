using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M1.API.DTOs.ERP;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Include)]
public class ERPInspectionLineDto
{
	[JsonProperty("qalActionType", Order = 1)]
	public byte qalActionType { get; set; }

	[JsonProperty("qalApprovalDecisionDate", Order = 2)]
	public DateTime? qalApprovalDecisionDate { get; set; }

	[JsonProperty("qalApprovalRequestDate", Order = 3)]
	public DateTime? qalApprovalRequestDate { get; set; }

	[JsonProperty("qalApprovalStatus", Order = 4)]
	[Required(ErrorMessage = "qalApprovalStatus is required.")]
	public byte qalApprovalStatus { get; set; }

	[JsonProperty("qalClosedDate", Order = 5)]
	public DateTime? qalClosedDate { get; set; }

	[JsonProperty("qalCreatedBy", Order = 6)]
	[MaxLength(20)]
	public string qalCreatedBy { get; set; }

	[JsonProperty("qalCreatedDate", Order = 7)]
	public DateTime? qalCreatedDate { get; set; }

	[JsonProperty("qalUniqueID", Order = 8)]
	public Guid qalUniqueID { get; set; }

	[JsonProperty("qalInspectionDate", Order = 9)]
	public DateTime? qalInspectionDate { get; set; }

	[JsonProperty("qalInspectionID", Order = 10)]
	[Required(ErrorMessage = "qalInspectionID is required.")]
	[MaxLength(10)]
	public string qalInspectionID { get; set; }

	[JsonProperty("qalInspectionNotesRTF", Order = 11)]
	[MaxLength(50)]
	public string qalInspectionNotesRTF { get; set; }

	[JsonProperty("qalInspectionNotesText", Order = 12)]
	[MaxLength(50)]
	public string qalInspectionNotesText { get; set; }

	[JsonProperty("qalInspectionType", Order = 13)]
	[Required(ErrorMessage = "qalInspectionType is required.")]
	public byte qalInspectionType { get; set; }

	[JsonProperty("qalInspectorEmployeeID", Order = 14)]
	[MaxLength(10)]
	public string qalInspectorEmployeeID { get; set; }

	[JsonProperty("qalInvQuantityAccepted", Order = 15)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalInvQuantityAccepted { get; set; }

	[JsonProperty("qalInvQuantityToReturn", Order = 16)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalInvQuantityToReturn { get; set; }

	[JsonProperty("qalInvQuantityToScrap", Order = 17)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalInvQuantityToScrap { get; set; }

	[JsonProperty("qalFirstOffInspection", Order = 18)]
	public bool qalFirstOffInspection { get; set; }

	[JsonProperty("qalInspectionComplete", Order = 19)]
	public bool qalInspectionComplete { get; set; }

	[JsonProperty("qalKitPart", Order = 20)]
	public bool qalKitPart { get; set; }

	[JsonProperty("qalManualInspectionFinalized", Order = 21)]
	public bool qalManualInspectionFinalized { get; set; }

	[JsonProperty("qalPosted", Order = 22)]
	public bool qalPosted { get; set; }

	[JsonProperty("qalReturnToSupplier", Order = 23)]
	public bool qalReturnToSupplier { get; set; }

	[JsonProperty("qalReversed", Order = 24)]
	public bool qalReversed { get; set; }

	[JsonProperty("qalTransferredToDmr", Order = 25)]
	public bool qalTransferredToDmr { get; set; }

	[JsonProperty("qalJobAssemblyID", Order = 26)]
	public int qalJobAssemblyID { get; set; }

	[JsonProperty("qalJobID", Order = 27)]
	[MaxLength(20)]
	public string qalJobID { get; set; }

	[JsonProperty("qalJobMaterialID", Order = 28)]
	public int qalJobMaterialID { get; set; }

	[JsonProperty("qalJobMatQuantityAccepted", Order = 29)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalJobMatQuantityAccepted { get; set; }

	[JsonProperty("qalJobMatQuantityRejected", Order = 30)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalJobMatQuantityRejected { get; set; }

	[JsonProperty("qalJobMatQuantityToReturn", Order = 31)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalJobMatQuantityToReturn { get; set; }

	[JsonProperty("qalJobMatQuantityToScrap", Order = 32)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalJobMatQuantityToScrap { get; set; }

	[JsonProperty("qalJobOperationID", Order = 33)]
	public int qalJobOperationID { get; set; }

	[JsonProperty("qalJobOprQuantityAccepted", Order = 34)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalJobOprQuantityAccepted { get; set; }

	[JsonProperty("qalJobOprQuantityRejected", Order = 35)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalJobOprQuantityRejected { get; set; }

	[JsonProperty("qalJobOprQuantityToReturn", Order = 36)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalJobOprQuantityToReturn { get; set; }

	[JsonProperty("qalJobOprQuantityToScrap", Order = 37)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalJobOprQuantityToScrap { get; set; }

	[JsonProperty("qalJobType", Order = 38)]
	public byte qalJobType { get; set; }

	[JsonProperty("qalMfgReceiptQuantityAccepted", Order = 39)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalMfgReceiptQuantityAccepted { get; set; }

	[JsonProperty("qalMfgReceiptQuantityToReturn", Order = 40)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalMfgReceiptQuantityToReturn { get; set; }

	[JsonProperty("qalMfgReceiptQuantityToScrap", Order = 41)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalMfgReceiptQuantityToScrap { get; set; }

	[JsonProperty("qalNextApprovalEmployeeID", Order = 42)]
	[MaxLength(10)]
	public string qalNextApprovalEmployeeID { get; set; }

	[JsonProperty("qalPartBinID", Order = 43)]
	[Required(ErrorMessage = "qalPartBinID is required.")]
	[MaxLength(15)]
	public string qalPartBinID { get; set; }

	[JsonProperty("qalPartID", Order = 44)]
	[Required(ErrorMessage = "qalPartID is required.")]
	[MaxLength(30)]
	public string qalPartID { get; set; }

	[JsonProperty("qalPartLongDescriptionRtf", Order = 45)]
	public string qalPartLongDescriptionRtf { get; set; }

	[JsonProperty("qalPartLongDescriptionText", Order = 46)]
	public string qalPartLongDescriptionText { get; set; }

	[JsonProperty("qalPartRevisionID", Order = 47)]
	[MaxLength(15)]
	public string qalPartRevisionID { get; set; }

	[JsonProperty("qalPartShortDescription", Order = 48)]
	[Required(ErrorMessage = "qalPartShortDescription is required.")]
	[MaxLength(50)]
	public string qalPartShortDescription { get; set; }

	[JsonProperty("qalPartTransactionID", Order = 49)]
	public int qalPartTransactionID { get; set; }

	[JsonProperty("qalPartWarehouseLocationID", Order = 50)]
	[Required(ErrorMessage = "qalPartWarehouseLocationID is required.")]
	[MaxLength(5)]
	public string qalPartWarehouseLocationID { get; set; }

	[JsonProperty("qalProjectAreaID", Order = 51)]
	[MaxLength(15)]
	public string qalProjectAreaID { get; set; }

	[JsonProperty("qalProjectID", Order = 52)]
	[MaxLength(10)]
	public string qalProjectID { get; set; }

	[JsonProperty("qalPurchaseLocationID", Order = 53)]
	[MaxLength(5)]
	public string qalPurchaseLocationID { get; set; }

	[JsonProperty("qalQuantityRejected", Order = 54)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalQuantityRejected { get; set; }

	[JsonProperty("qalQuantityToInspect", Order = 55)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalQuantityToInspect { get; set; }

	[JsonProperty("qalReverseInspectionID", Order = 56)]
	[MaxLength(10)]
	public string qalReverseInspectionID { get; set; }

	[JsonProperty("qalReverseInspectionLineID", Order = 57)]
	public short qalReverseInspectionLineID { get; set; }

	[JsonProperty("qalScrapReasonID", Order = 58)]
	[MaxLength(5)]
	public string qalScrapReasonID { get; set; }

	[JsonProperty("qalInspectionLineID", Order = 59)]
	[Required(ErrorMessage = "qalInspectionLineID is required.")]
	public short qalInspectionLineID { get; set; }

	[JsonProperty("qalSourceTableName", Order = 60)]
	[MaxLength(30)]
	public string qalSourceTableName { get; set; }

	[JsonProperty("qalSourceTableUniqueID", Order = 61)]
	public Guid qalSourceTableUniqueID { get; set; }

	[JsonProperty("qalStatus", Order = 62)]
	[Required(ErrorMessage = "qalStatus is required.")]
	[MaxLength(1)]
	public string qalStatus { get; set; }

	[JsonProperty("qalSupplierOrganizationID", Order = 63)]
	[MaxLength(10)]
	public string qalSupplierOrganizationID { get; set; }

	[JsonProperty("qalUnitCost", Order = 64)]
	[Range(0.0, 9999999999.99999, ErrorMessage = "{0} value must be between {1} and {2}.")]
	public decimal qalUnitCost { get; set; }

	[JsonProperty("qalUnitOfMeasure", Order = 65)]
	[MaxLength(2)]
	public string qalUnitOfMeasure { get; set; }

	[JsonProperty("customFields", Order = 66)]
	[JsonExtensionData]
	public IDictionary<string, object> CustomFields { get; set; }
}
