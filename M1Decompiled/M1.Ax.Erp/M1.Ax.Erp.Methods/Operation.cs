using System;
using System.Collections.Generic;

namespace M1.Ax.Erp.Methods;

public class Operation
{
	public Dictionary<string, object> CustomFields = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);

	public string MethodID = string.Empty;

	public string MethodRevisionID = string.Empty;

	public int AssemblyID;

	public int OperationID;

	public byte OperationType;

	public string PlantID = string.Empty;

	public string PlantDepartmentID = string.Empty;

	public string WorkCenterID = string.Empty;

	public string ProcessID = string.Empty;

	public string ProcessShortDescription = string.Empty;

	public string ProcessLongDescriptionRTF = string.Empty;

	public string ProcessLongDescriptionText = string.Empty;

	public decimal QuantityPerAssembly;

	public decimal OverheadRate;

	public decimal SetupRate;

	public decimal ProductionRate;

	public decimal QueueTime;

	public decimal MoveTime;

	public decimal SetupHours;

	public decimal ProductionStandard;

	public string StandardFactor = string.Empty;

	public decimal OperationQuantity;

	public decimal EstimatedProductionHours;

	public decimal CalculatedUnitCost;

	public int OverlapOperationID;

	public byte OverlapSourceLink;

	public byte OverlapDestinationLink;

	public decimal OverlapOffsetTime;

	public string PartID = string.Empty;

	public string PartRevisionID = string.Empty;

	public string UnitOfMeasure = string.Empty;

	public decimal EstimatedUnitCost;

	public decimal MinimumCharge;

	public decimal SetupCharge;

	public string SupplierOrganizationID = string.Empty;

	public string PurchaseLocationID = string.Empty;

	public string Documents = string.Empty;

	public string SFEMessageText = string.Empty;

	public string SFEMessageRTF = string.Empty;

	public byte InspectionType;

	public byte MachineType;

	public short WorkCenterMachineID;

	public short MachinesToSchedule;

	public PriceBreak PriceBreak1 = new PriceBreak();

	public PriceBreak PriceBreak2 = new PriceBreak();

	public PriceBreak PriceBreak3 = new PriceBreak();

	public PriceBreak PriceBreak4 = new PriceBreak();

	public PriceBreak PriceBreak5 = new PriceBreak();

	public PriceBreak PriceBreak6 = new PriceBreak();

	public PriceBreak PriceBreak7 = new PriceBreak();

	public PriceBreak PriceBreak8 = new PriceBreak();

	public PriceBreak PriceBreak9 = new PriceBreak();

	public override string ToString()
	{
		return $"{AssemblyID} - {OperationID}, \"{ProcessID}\", Qty = {OperationQuantity}";
	}
}
