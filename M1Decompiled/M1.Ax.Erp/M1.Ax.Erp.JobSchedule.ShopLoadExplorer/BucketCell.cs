using System;

namespace M1.Ax.Erp.JobSchedule.ShopLoadExplorer;

public class BucketCell
{
	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public double Load { get; set; }

	public double Capacity { get; set; }

	public double SetupLoad { get; set; }

	public BucketCell(DateTime startDate, DateTime endDate)
	{
		StartDate = startDate;
		EndDate = endDate;
	}
}
