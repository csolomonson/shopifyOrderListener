using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPInspectionLineInformationDto
{
	public byte qalActionType { get; set; }

	public DateTime? qalApprovalDecisionDate { get; set; }

	public DateTime? qalApprovalRequestDate { get; set; }

	public byte qalApprovalStatus { get; set; }

	public DateTime? qalClosedDate { get; set; }

	public string qalCreatedBy { get; set; }

	public DateTime? qalCreatedDate { get; set; }

	public Guid qalUniqueID { get; set; }

	public DateTime? qalInspectionDate { get; set; }

	public string qalInspectionID { get; set; }

	public string qalInspectionNotesRTF { get; set; }

	public string qalInspectionNotesText { get; set; }

	public byte qalInspectionType { get; set; }

	public string qalInspectorEmployeeID { get; set; }

	public decimal qalInvQuantityAccepted { get; set; }

	public decimal qalInvQuantityToReturn { get; set; }

	public decimal qalInvQuantityToScrap { get; set; }

	public bool qalFirstOffInspection { get; set; }

	public bool qalInspectionComplete { get; set; }

	public bool qalKitPart { get; set; }

	public bool qalManualInspectionFinalized { get; set; }

	public bool qalPosted { get; set; }

	public bool qalReturnToSupplier { get; set; }

	public bool qalReversed { get; set; }

	public bool qalTransferredToDmr { get; set; }

	public int qalJobAssemblyID { get; set; }

	public string qalJobID { get; set; }

	public int qalJobMaterialID { get; set; }

	public decimal qalJobMatQuantityAccepted { get; set; }

	public decimal qalJobMatQuantityRejected { get; set; }

	public decimal qalJobMatQuantityToReturn { get; set; }

	public decimal qalJobMatQuantityToScrap { get; set; }

	public int qalJobOperationID { get; set; }

	public decimal qalJobOprQuantityAccepted { get; set; }

	public decimal qalJobOprQuantityRejected { get; set; }

	public decimal qalJobOprQuantityToReturn { get; set; }

	public decimal qalJobOprQuantityToScrap { get; set; }

	public byte qalJobType { get; set; }

	public decimal qalMfgReceiptQuantityAccepted { get; set; }

	public decimal qalMfgReceiptQuantityToReturn { get; set; }

	public decimal qalMfgReceiptQuantityToScrap { get; set; }

	public string qalNextApprovalEmployeeID { get; set; }

	public string qalPartBinID { get; set; }

	public string qalPartID { get; set; }

	public string qalPartLongDescriptionRtf { get; set; }

	public string qalPartLongDescriptionText { get; set; }

	public string qalPartRevisionID { get; set; }

	public string qalPartShortDescription { get; set; }

	public int qalPartTransactionID { get; set; }

	public string qalPartWarehouseLocationID { get; set; }

	public string qalProjectAreaID { get; set; }

	public string qalProjectID { get; set; }

	public string qalPurchaseLocationID { get; set; }

	public decimal qalQuantityRejected { get; set; }

	public decimal qalQuantityToInspect { get; set; }

	public string qalReverseInspectionID { get; set; }

	public short qalReverseInspectionLineID { get; set; }

	public string qalScrapReasonID { get; set; }

	public short qalInspectionLineID { get; set; }

	public string qalSourceTableName { get; set; }

	public Guid qalSourceTableUniqueID { get; set; }

	public string qalStatus { get; set; }

	public string qalSupplierOrganizationID { get; set; }

	public decimal qalUnitCost { get; set; }

	public string qalUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
