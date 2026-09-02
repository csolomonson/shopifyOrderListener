using System;
using System.Collections.Generic;

namespace M1.Ax.Erp.JobSchedule.ShopLoadExplorer;

public class WorkCenterLoad
{
	public string Description { get; set; }

	public string ProductionDepartmentId { get; set; }

	public string ProductionDepartDescription { get; set; }

	public string JobId { get; set; }

	public int JobAssemblyId { get; set; }

	public int JobOperationId { get; set; }

	public string PartId { get; set; }

	public string PartShortDescription { get; set; }

	public string WorkCenterId { get; set; }

	public string CustomerName { get; set; }

	public double Capacity { get; set; }

	public double Load { get; set; }

	public double PastLoad { get; set; }

	public double FutureLoad { get; set; }

	public string PlantId { get; set; }

	public int NumberOfMachines { get; set; }

	public List<BucketCell> Buckets { get; set; }

	public double SetupLoad { get; set; }

	public double SetupComplete { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public WorkCenterLoad(string workCenterId)
	{
		Capacity = 0.0;
		Load = 0.0;
		SetupLoad = 0.0;
		SetupComplete = 0.0;
		PastLoad = 0.0;
		FutureLoad = 0.0;
		NumberOfMachines = 1;
		PlantId = string.Empty;
		WorkCenterId = workCenterId;
		ProductionDepartmentId = string.Empty;
		StartDate = DateTime.Today.Date;
		EndDate = DateTime.Today.Date;
		Buckets = new List<BucketCell>();
	}

	public void LoadValues(DateTime[,] bucketDates)
	{
		for (int i = 0; i < bucketDates.GetLength(0); i++)
		{
			BucketCell item = new BucketCell(bucketDates[i, 0], bucketDates[i, 1]);
			Buckets.Add(item);
		}
	}
}
