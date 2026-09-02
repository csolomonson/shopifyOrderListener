namespace M1.Ax.Erp.JobSplit;

public class TargetJob
{
	public string JobId { get; set; }

	public double OrderQuantity { get; set; }

	public double InventoryQuantity { get; set; }

	public double ScrapQuantity { get; set; }

	public double ProductionQuantity { get; set; }

	public object ProductionDueDate { get; set; }
}
