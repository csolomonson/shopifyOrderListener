using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartOperationInformationDto
{
	public string imoCreatedBy { get; set; }

	public DateTime? imoCreatedDate { get; set; }

	public string imoDocuments { get; set; }

	public Guid imoUniqueID { get; set; }

	public decimal imoEstimatedUnitCost { get; set; }

	public byte imoInspectionType { get; set; }

	public short imoMachinesToSchedule { get; set; }

	public byte imoMachineType { get; set; }

	public int imoMethodAssemblyID { get; set; }

	public string imoMethodID { get; set; }

	public int imoMethodOperationID { get; set; }

	public string imoMethodRevisionID { get; set; }

	public decimal imoMinimumCharge { get; set; }

	public decimal imoMoveTime { get; set; }

	public byte imoOperationType { get; set; }

	public byte imoOverlap { get; set; }

	public byte imoOverlapDestinationLink { get; set; }

	public decimal imoOverlapOffsetTime { get; set; }

	public int imoOverlapOperationID { get; set; }

	public byte imoOverlapSourceLink { get; set; }

	public string imoPartID { get; set; }

	public string imoPartRevisionID { get; set; }

	public string imoPlantDepartmentID { get; set; }

	public string imoPlantID { get; set; }

	public string imoProcessID { get; set; }

	public string imoProcessLongDescriptionRtf { get; set; }

	public string imoProcessLongDescriptionText { get; set; }

	public string imoProcessShortDescription { get; set; }

	public decimal imoProductionStandard { get; set; }

	public string imoPurchaseLocationID { get; set; }

	public decimal imoQuantityBreak1 { get; set; }

	public decimal imoQuantityBreak2 { get; set; }

	public decimal imoQuantityBreak3 { get; set; }

	public decimal imoQuantityBreak4 { get; set; }

	public decimal imoQuantityBreak5 { get; set; }

	public decimal imoQuantityBreak6 { get; set; }

	public decimal imoQuantityBreak7 { get; set; }

	public decimal imoQuantityBreak8 { get; set; }

	public decimal imoQuantityBreak9 { get; set; }

	public decimal imoQuantityPerAssembly { get; set; }

	public decimal imoQueueTime { get; set; }

	public byte[] imoRowVersion { get; set; }

	public decimal imoSetupCharge { get; set; }

	public decimal imoSetupHours { get; set; }

	public string imoSfeMessageRTF { get; set; }

	public string imoSfeMessageText { get; set; }

	public string imoStandardFactor { get; set; }

	public string imoSupplierOrganizationID { get; set; }

	public decimal imoUnitCost1 { get; set; }

	public decimal imoUnitCost2 { get; set; }

	public decimal imoUnitCost3 { get; set; }

	public decimal imoUnitCost4 { get; set; }

	public decimal imoUnitCost5 { get; set; }

	public decimal imoUnitCost6 { get; set; }

	public decimal imoUnitCost7 { get; set; }

	public decimal imoUnitCost8 { get; set; }

	public decimal imoUnitCost9 { get; set; }

	public string imoUnitOfMeasure { get; set; }

	public string imoWorkCenterID { get; set; }

	public short imoWorkCenterMachineID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
