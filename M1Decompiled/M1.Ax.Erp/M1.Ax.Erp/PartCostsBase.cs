using System;
using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
public class PartCostsBase
{
	public PartTransactionDefinition.CostType CostType;

	public decimal Quantity;

	public decimal LaborCost;

	public decimal OverheadCost;

	public decimal MaterialCost;

	public decimal SubcontractCost;

	public decimal DutyCost;

	public decimal FreightCost;

	public decimal MiscCost;

	public decimal ActualUnitLaborCost;

	public decimal ActualUnitOverheadCost;

	public decimal ActualUnitMaterialCost;

	public decimal ActualUnitSubcontractCost;

	public decimal ActualUnitDutyCost;

	public decimal ActualUnitFreightCost;

	public decimal ActualUnitMiscCost;

	public Guid? SourcePartBinDetailID;
}
