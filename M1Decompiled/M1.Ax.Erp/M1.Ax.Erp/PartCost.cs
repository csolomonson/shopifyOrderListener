using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
public class PartCost : PartCostsBase
{
	public decimal PrevUnitLaborCost;

	public decimal PrevUnitOverheadCost;

	public decimal PrevUnitMaterialCost;

	public decimal PrevUnitSubcontractCost;

	public decimal PrevUnitDutyCost;

	public decimal PrevUnitFreightCost;

	public decimal PrevUnitMiscCost;
}
