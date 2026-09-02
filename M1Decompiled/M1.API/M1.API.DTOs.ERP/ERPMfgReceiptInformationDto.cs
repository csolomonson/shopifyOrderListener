using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPMfgReceiptInformationDto
{
	public string rmmMfgReceiptID { get; set; }

	public string rmmCreatedBy { get; set; }

	public DateTime? rmmCreatedDate { get; set; }

	public Guid rmmUniqueID { get; set; }

	public decimal rmmEstimatedQuantity { get; set; }

	public decimal rmmExtendedCostBase { get; set; }

	public string rmmHeatLot { get; set; }

	public byte rmmImCostingMethod { get; set; }

	public decimal rmmInventoryQuantity { get; set; }

	public decimal rmmInventoryQuantityReceived { get; set; }

	public string rmmInventoryUnitOfMeasure { get; set; }

	public bool rmmCreateJobSeq { get; set; }

	public bool rmmInInspection { get; set; }

	public bool rmmInspectionComplete { get; set; }

	public bool rmmKitPart { get; set; }

	public bool rmmNotUpdateJobQtyComplete { get; set; }

	public bool rmmPoLineReceivedComplete { get; set; }

	public bool rmmPosted { get; set; }

	public bool rmmProductionComplete { get; set; }

	public bool rmmReceivedComplete { get; set; }

	public bool rmmRequiresInspection { get; set; }

	public bool rmmReversalEntry { get; set; }

	public bool rmmReversed { get; set; }

	public decimal rmmJobAsmQuantityReceived { get; set; }

	public int rmmJobAssemblyID { get; set; }

	public string rmmJobID { get; set; }

	public int rmmJobMaterialID { get; set; }

	public decimal rmmJobMatQuantityReceived { get; set; }

	public decimal rmmJobOpenQuantity { get; set; }

	public int rmmJobOperationID { get; set; }

	public decimal rmmJobOprQuantityReceived { get; set; }

	public decimal rmmJobScrapQuantity { get; set; }

	public byte rmmJobType { get; set; }

	public string rmmLongDescriptionRtf { get; set; }

	public string rmmLongDescriptionText { get; set; }

	public byte rmmMfgCostType { get; set; }

	public decimal rmmMiscInvQuantityReceived { get; set; }

	public string rmmPartBinID { get; set; }

	public string rmmPartID { get; set; }

	public string rmmPartRevisionID { get; set; }

	public string rmmPartWarehouseLocationID { get; set; }

	public string rmmPlantDepartmentID { get; set; }

	public string rmmPlantID { get; set; }

	public decimal rmmPoOpenQuantity { get; set; }

	public DateTime? rmmPostedDate { get; set; }

	public decimal rmmProductionQuantity { get; set; }

	public string rmmProjectAreaID { get; set; }

	public string rmmProjectID { get; set; }

	public string rmmPurchaseLocationID { get; set; }

	public string rmmPurchaseOrderID { get; set; }

	public short rmmPurchaseOrderLineID { get; set; }

	public decimal rmmPurchaseQuantity { get; set; }

	public decimal rmmPurchaseQuantityReceived { get; set; }

	public decimal rmmPurchaseUnitCost { get; set; }

	public string rmmPurchaseUnitOfMeasure { get; set; }

	public decimal rmmQuantityCompleted { get; set; }

	public decimal rmmQuantityOnHand { get; set; }

	public decimal rmmQuantityReceivedToInventory { get; set; }

	public decimal rmmQuantityToInspect { get; set; }

	public DateTime? rmmReceiptDate { get; set; }

	public byte rmmReceiptType { get; set; }

	public string rmmReference { get; set; }

	public string rmmReverseMfgReceiptID { get; set; }

	public byte[] rmmRowVersion { get; set; }

	public decimal rmmScrapQuantity { get; set; }

	public decimal rmmSetupCharge { get; set; }

	public string rmmSupplierOrganizationID { get; set; }

	public decimal rmmTotalComponentCosts { get; set; }

	public decimal rmmTotalUnitCost { get; set; }

	public decimal rmmUnitLaborCost { get; set; }

	public decimal rmmUnitMaterialCost { get; set; }

	public decimal rmmUnitOverheadCost { get; set; }

	public decimal rmmUnitSubcontractCost { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
