namespace M1.Ax.Erp.Methods;

public class PriceBreak
{
	public decimal QuantityBreak;

	public decimal UnitCost;

	public short LeadTime;

	public override string ToString()
	{
		return $"Qty = {QuantityBreak}, Cost = {UnitCost}, LeadTime = {LeadTime}";
	}
}
