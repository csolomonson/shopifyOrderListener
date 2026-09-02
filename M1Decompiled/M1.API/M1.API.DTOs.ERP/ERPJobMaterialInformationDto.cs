using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPJobMaterialInformationDto
{
	public decimal jmmCalculatedUnitCost { get; set; }

	public string jmmCreatedBy { get; set; }

	public DateTime? jmmCreatedDate { get; set; }

	public string jmmDocuments { get; set; }

	public DateTime? jmmDueInDate { get; set; }

	public Guid jmmUniqueID { get; set; }

	public decimal jmmEstimatedQuantity { get; set; }

	public decimal jmmEstimatedUnitCost { get; set; }

	public bool jmmBackflush { get; set; }

	public bool jmmClosed { get; set; }

	public bool jmmCostOverride { get; set; }

	public bool jmmFirm { get; set; }

	public bool jmmKitPart { get; set; }

	public bool jmmPullAllFromStock { get; set; }

	public bool jmmReceivedComplete { get; set; }

	public int jmmJobAssemblyID { get; set; }

	public string jmmJobID { get; set; }

	public short jmmLeadTime { get; set; }

	public short jmmLeadTime1 { get; set; }

	public short jmmLeadTime2 { get; set; }

	public short jmmLeadTime3 { get; set; }

	public short jmmLeadTime4 { get; set; }

	public short jmmLeadTime5 { get; set; }

	public short jmmLeadTime6 { get; set; }

	public short jmmLeadTime7 { get; set; }

	public short jmmLeadTime8 { get; set; }

	public short jmmLeadTime9 { get; set; }

	public decimal jmmMinimumCharge { get; set; }

	public DateTime? jmmOrderByDate { get; set; }

	public string jmmPartBinID { get; set; }

	public string jmmPartID { get; set; }

	public string jmmPartLongDescriptionRtf { get; set; }

	public string jmmPartLongDescriptionText { get; set; }

	public string jmmPartRevisionID { get; set; }

	public string jmmPartShortDescription { get; set; }

	public string jmmPartWarehouseLocationID { get; set; }

	public decimal jmmPullFromStockQuantity { get; set; }

	public string jmmPurchaseLocationID { get; set; }

	public string jmmPurchaseOrderID { get; set; }

	public decimal jmmPurchaseToJobQuantity { get; set; }

	public decimal jmmQuantityAllocated { get; set; }

	public decimal jmmQuantityBreak1 { get; set; }

	public decimal jmmQuantityBreak2 { get; set; }

	public decimal jmmQuantityBreak3 { get; set; }

	public decimal jmmQuantityBreak4 { get; set; }

	public decimal jmmQuantityBreak5 { get; set; }

	public decimal jmmQuantityBreak6 { get; set; }

	public decimal jmmQuantityBreak7 { get; set; }

	public decimal jmmQuantityBreak8 { get; set; }

	public decimal jmmQuantityBreak9 { get; set; }

	public decimal jmmQuantityPerAssembly { get; set; }

	public decimal jmmQuantityReceived { get; set; }

	public decimal jmmQuantityToInspect { get; set; }

	public decimal jmmQuantityToReturn { get; set; }

	public int jmmRelatedJobOperationID { get; set; }

	public DateTime? jmmRequiredDate { get; set; }

	public string jmmRfqID { get; set; }

	public byte[] jmmRowVersion { get; set; }

	public decimal jmmScrapPercent { get; set; }

	public decimal jmmScrapQuantity { get; set; }

	public decimal jmmScrapQuantityReceived { get; set; }

	public int jmmJobMaterialID { get; set; }

	public string jmmSupplierOrganizationID { get; set; }

	public decimal jmmUnitCost1 { get; set; }

	public decimal jmmUnitCost2 { get; set; }

	public decimal jmmUnitCost3 { get; set; }

	public decimal jmmUnitCost4 { get; set; }

	public decimal jmmUnitCost5 { get; set; }

	public decimal jmmUnitCost6 { get; set; }

	public decimal jmmUnitCost7 { get; set; }

	public decimal jmmUnitCost8 { get; set; }

	public decimal jmmUnitCost9 { get; set; }

	public string jmmUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
