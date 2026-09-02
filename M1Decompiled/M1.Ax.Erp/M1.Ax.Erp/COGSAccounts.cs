using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
public class COGSAccounts
{
	public string TableSource = string.Empty;

	public bool Success;

	public string PartID = string.Empty;

	public string PartGroupID = string.Empty;

	public string PartClassID = string.Empty;

	public string PlantID = string.Empty;

	public string InventoryGLAccountID = string.Empty;

	public string COGSLaborGLAccountID = string.Empty;

	public string COGSMaterialGLAccountID = string.Empty;

	public string COGSSubcontractGLAccountID = string.Empty;

	public string COGSOverheadGLAccountID = string.Empty;

	public string SVarLaborGLAccountID = string.Empty;

	public string SVarMaterialGLAccountID = string.Empty;

	public string SVarSubcontractGLAccountID = string.Empty;

	public string SVarOverheadGLAccountID = string.Empty;

	public string PurchaseVarianceGLAccountID = string.Empty;

	public string WIPLaborGLAccountID = string.Empty;

	public string WIPMaterialGLAccountID = string.Empty;

	public string WIPSubcontractGLAccountID = string.Empty;

	public string WIPOverheadGLAccountID = string.Empty;

	public string AccruedCreditorsGLAccountID = string.Empty;

	public string LaborClearingGLAccountID = string.Empty;

	public string OverheadClearingGLAccountID = string.Empty;

	public string StockRevaluationGLAccountID = string.Empty;

	public string ShipAwaitInvoiceGLAccountID = string.Empty;

	public string StockInTransitGLAccountID = string.Empty;

	public string ReasonGLAccountID = string.Empty;

	public string ScrapGLAccountID = string.Empty;

	public string InventoryToReturnGLAccountID = string.Empty;

	public string InventoryInInspectionGLAccountID = string.Empty;

	public string InventoryInTransferGLAccountID = string.Empty;
}
