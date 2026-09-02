using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
public class ProjectCosts
{
	public decimal EstJobMaterialCost;

	public decimal EstQuoteMaterialCost;

	public decimal ActMaterialCost;

	public decimal EstJobLaborCost;

	public decimal EstQuoteLaborCost;

	public decimal ActLaborCost;

	public decimal EstJobSubCCost;

	public decimal EstQuoteSubCCost;

	public decimal ActSubCCost;

	public decimal EstJobOverheadCost;

	public decimal EstQuoteOverheadCost;

	public decimal ActOverheadCost;

	public decimal EstJobSetupHours;

	public decimal EstQuoteSetupHours;

	public decimal ActSetupHours;

	public decimal EstJobProdHours;

	public decimal EstQuoteProdHours;

	public decimal ActProdHours;

	public decimal ActReworkHours;

	public decimal EstPurchaseToOrder;

	public decimal JobPurchaseToOrder;

	public decimal ActPurchaseToOrder;
}
