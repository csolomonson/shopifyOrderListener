namespace M1.Ax.Erp;

public class QuoteAssemblyTotals
{
	public decimal QuoteQuantity;

	public decimal LaborCost;

	public decimal OverheadCost;

	public decimal QuotingCost;

	public decimal SubcontractCost;

	public decimal MaterialCost;

	public decimal SetupHours;

	public decimal ProductionHours;

	public decimal CalculatedUnitPrice;

	public string PartID = string.Empty;

	public string PartRevisionID = string.Empty;

	public int AssemblyID;

	public int ParentAssemblyID;

	public decimal QuantityPerParent;

	public short Level;
}
