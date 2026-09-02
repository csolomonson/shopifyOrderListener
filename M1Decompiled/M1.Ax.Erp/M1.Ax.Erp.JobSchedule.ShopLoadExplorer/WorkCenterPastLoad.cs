using System;
using System.Collections.Generic;

namespace M1.Ax.Erp.JobSchedule.ShopLoadExplorer;

public class WorkCenterPastLoad
{
	public string PlantId { get; set; }

	public string ProductionDepartmentId { get; set; }

	public string WorkCenterId { get; set; }

	public double PastLoad { get; set; }

	public double PastSetupLoad { get; set; }

	public int NumberOfMachines { get; set; }

	public List<BucketCell> PastBuckets { get; set; }

	public WorkCenterPastLoad(string workCenterId)
	{
		PlantId = string.Empty;
		ProductionDepartmentId = string.Empty;
		WorkCenterId = workCenterId;
		PastLoad = 0.0;
		PastSetupLoad = 0.0;
		NumberOfMachines = 1;
		PastBuckets = new List<BucketCell>();
	}

	public void LoadPastValues(DateTime[,] bucketDates)
	{
		for (int i = 0; i < bucketDates.GetLength(0); i++)
		{
			BucketCell item = new BucketCell(bucketDates[i, 0], bucketDates[i, 1]);
			PastBuckets.Add(item);
		}
	}
}
