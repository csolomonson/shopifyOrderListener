using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPJobOperationInformationDto
{
	public decimal jmoActualProductionHours { get; set; }

	public decimal jmoActualSetupHours { get; set; }

	public decimal jmoCalculatedUnitCost { get; set; }

	public decimal jmoCompletedProductionHours { get; set; }

	public decimal jmoCompletedSetupHours { get; set; }

	public string jmoCreatedBy { get; set; }

	public DateTime? jmoCreatedDate { get; set; }

	public string jmoDocuments { get; set; }

	public DateTime? jmoDueDate { get; set; }

	public decimal jmoDueHour { get; set; }

	public Guid jmoUniqueID { get; set; }

	public decimal jmoEstimatedProductionHours { get; set; }

	public decimal jmoEstimatedUnitCost { get; set; }

	public byte jmoInspectionStatus { get; set; }

	public byte jmoInspectionType { get; set; }

	public bool jmoAddedOperation { get; set; }

	public bool jmoClosed { get; set; }

	public bool jmoFirm { get; set; }

	public bool jmoInspectionComplete { get; set; }

	public bool jmoProductionComplete { get; set; }

	public bool jmoPrototypeOperation { get; set; }

	public bool jmoSetupComplete { get; set; }

	public int jmoJobAssemblyID { get; set; }

	public string jmoJobID { get; set; }

	public short jmoMachinesToSchedule { get; set; }

	public byte jmoMachineType { get; set; }

	public decimal jmoMinimumCharge { get; set; }

	public decimal jmoMoveTime { get; set; }

	public decimal jmoOperationQuantity { get; set; }

	public byte jmoOperationType { get; set; }

	public decimal jmoOverheadRate { get; set; }

	public byte jmoOverlap { get; set; }

	public byte jmoOverlapDestinationLink { get; set; }

	public decimal jmoOverlapOffsetTime { get; set; }

	public int jmoOverlapOperationID { get; set; }

	public byte jmoOverlapSourceLink { get; set; }

	public string jmoPartBinID { get; set; }

	public string jmoPartID { get; set; }

	public string jmoPartRevisionID { get; set; }

	public string jmoPartWarehouseLocationID { get; set; }

	public string jmoPlantDepartmentID { get; set; }

	public string jmoPlantID { get; set; }

	public string jmoProcessID { get; set; }

	public string jmoProcessLongDescriptionRtf { get; set; }

	public string jmoProcessLongDescriptionText { get; set; }

	public string jmoProcessShortDescription { get; set; }

	public decimal jmoProductionRate { get; set; }

	public decimal jmoProductionStandard { get; set; }

	public string jmoPurchaseLocationID { get; set; }

	public string jmoPurchaseOrderID { get; set; }

	public decimal jmoQuantityBreak1 { get; set; }

	public decimal jmoQuantityBreak2 { get; set; }

	public decimal jmoQuantityBreak3 { get; set; }

	public decimal jmoQuantityBreak4 { get; set; }

	public decimal jmoQuantityBreak5 { get; set; }

	public decimal jmoQuantityBreak6 { get; set; }

	public decimal jmoQuantityBreak7 { get; set; }

	public decimal jmoQuantityBreak8 { get; set; }

	public decimal jmoQuantityBreak9 { get; set; }

	public decimal jmoQuantityComplete { get; set; }

	public decimal jmoQuantityPerAssembly { get; set; }

	public decimal jmoQuantityToInspect { get; set; }

	public decimal jmoQuantityToReturn { get; set; }

	public decimal jmoQueueTime { get; set; }

	public string jmoRfqID { get; set; }

	public byte[] jmoRowVersion { get; set; }

	public decimal jmoScrapQuantityReceived { get; set; }

	public int jmoJobOperationID { get; set; }

	public decimal jmoSetupCharge { get; set; }

	public decimal jmoSetupHours { get; set; }

	public short jmoSetupPercentComplete { get; set; }

	public decimal jmoSetupRate { get; set; }

	public string jmoSfeMessageRTF { get; set; }

	public string jmoSfeMessageText { get; set; }

	public string jmoStandardFactor { get; set; }

	public DateTime? jmoStartDate { get; set; }

	public decimal jmoStartHour { get; set; }

	public string jmoSupplierOrganizationID { get; set; }

	public decimal jmoUnitCost1 { get; set; }

	public decimal jmoUnitCost2 { get; set; }

	public decimal jmoUnitCost3 { get; set; }

	public decimal jmoUnitCost4 { get; set; }

	public decimal jmoUnitCost5 { get; set; }

	public decimal jmoUnitCost6 { get; set; }

	public decimal jmoUnitCost7 { get; set; }

	public decimal jmoUnitCost8 { get; set; }

	public decimal jmoUnitCost9 { get; set; }

	public string jmoUnitOfMeasure { get; set; }

	public string jmoWorkCenterID { get; set; }

	public short jmoWorkCenterMachineID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
