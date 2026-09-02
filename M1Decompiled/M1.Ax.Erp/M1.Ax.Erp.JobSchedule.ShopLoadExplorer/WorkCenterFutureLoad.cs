using System;
using System.Collections.Generic;

namespace M1.Ax.Erp.JobSchedule.ShopLoadExplorer;

public class WorkCenterFutureLoad
{
	public string PlantId { get; set; }

	public string ProductionDepartmentId { get; set; }

	public string WorkCenterId { get; set; }

	public double FutureLoad { get; set; }

	public double FutureSetupLoad { get; set; }

	public int NumberOfMachines { get; set; }

	public List<BucketCell> FutureBuckets { get; set; }

	public WorkCenterFutureLoad(string workCenterId)
	{
		PlantId = string.Empty;
		ProductionDepartmentId = string.Empty;
		WorkCenterId = workCenterId;
		FutureLoad = 0.0;
		FutureSetupLoad = 0.0;
		NumberOfMachines = 1;
		FutureBuckets = new List<BucketCell>();
	}

	public void loadFutureValues(DateTime[,] bucketDates)
	{
		for (int i = 0; i < bucketDates.GetLength(0); i++)
		{
			BucketCell item = new BucketCell(bucketDates[i, 0], bucketDates[i, 1]);
			FutureBuckets.Add(item);
		}
	}
}
