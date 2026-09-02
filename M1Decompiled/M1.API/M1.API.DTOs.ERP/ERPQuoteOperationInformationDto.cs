using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPQuoteOperationInformationDto
{
	public decimal qmoAdditionalSetupHours { get; set; }

	public decimal qmoAdditionalSetupQuantity { get; set; }

	public string qmoCreatedBy { get; set; }

	public DateTime? qmoCreatedDate { get; set; }

	public string qmoDocuments { get; set; }

	public Guid qmoUniqueID { get; set; }

	public decimal qmoEstimatedUnitCost { get; set; }

	public byte qmoInspectionType { get; set; }

	public bool qmoClosed { get; set; }

	public short qmoMachinesToSchedule { get; set; }

	public byte qmoMachineType { get; set; }

	public decimal qmoMinimumCharge { get; set; }

	public decimal qmoMoveTime { get; set; }

	public byte qmoOperationType { get; set; }

	public decimal qmoOverheadRate { get; set; }

	public byte qmoOverlap { get; set; }

	public byte qmoOverlapDestinationLink { get; set; }

	public decimal qmoOverlapOffsetTime { get; set; }

	public int qmoOverlapOperationID { get; set; }

	public byte qmoOverlapSourceLink { get; set; }

	public string qmoPartID { get; set; }

	public string qmoPartRevisionID { get; set; }

	public string qmoPlantDepartmentID { get; set; }

	public string qmoPlantID { get; set; }

	public string qmoProcessID { get; set; }

	public string qmoProcessLongDescriptionRtf { get; set; }

	public string qmoProcessLongDescriptionText { get; set; }

	public string qmoProcessShortDescription { get; set; }

	public decimal qmoProductionRate { get; set; }

	public decimal qmoProductionStandard { get; set; }

	public string qmoPurchaseLocationID { get; set; }

	public decimal qmoQuantityBreak1 { get; set; }

	public decimal qmoQuantityBreak2 { get; set; }

	public decimal qmoQuantityBreak3 { get; set; }

	public decimal qmoQuantityBreak4 { get; set; }

	public decimal qmoQuantityBreak5 { get; set; }

	public decimal qmoQuantityBreak6 { get; set; }

	public decimal qmoQuantityBreak7 { get; set; }

	public decimal qmoQuantityBreak8 { get; set; }

	public decimal qmoQuantityBreak9 { get; set; }

	public decimal qmoQuantityPerAssembly { get; set; }

	public decimal qmoQueueTime { get; set; }

	public int qmoQuoteAssemblyID { get; set; }

	public string qmoQuoteID { get; set; }

	public short qmoQuoteLineID { get; set; }

	public decimal qmoQuotingRate { get; set; }

	public byte[] qmoRowVersion { get; set; }

	public int qmoQuoteOperationID { get; set; }

	public decimal qmoSetupCharge { get; set; }

	public decimal qmoSetupHours { get; set; }

	public decimal qmoSetupRate { get; set; }

	public string qmoSfeMessageRTF { get; set; }

	public string qmoSfeMessageText { get; set; }

	public string qmoStandardFactor { get; set; }

	public string qmoSupplierOrganizationID { get; set; }

	public decimal qmoUnitCost1 { get; set; }

	public decimal qmoUnitCost2 { get; set; }

	public decimal qmoUnitCost3 { get; set; }

	public decimal qmoUnitCost4 { get; set; }

	public decimal qmoUnitCost5 { get; set; }

	public decimal qmoUnitCost6 { get; set; }

	public decimal qmoUnitCost7 { get; set; }

	public decimal qmoUnitCost8 { get; set; }

	public decimal qmoUnitCost9 { get; set; }

	public string qmoUnitOfMeasure { get; set; }

	public string qmoWorkCenterID { get; set; }

	public short qmoWorkCenterMachineID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
