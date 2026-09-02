using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp.JobSchedule.ShopLoadExplorer;

public class ShopLoadExplorer
{
	private double timeCardSetupMinutes;

	private double timeCardProductionMinutes;

	private double oprSetupMinutes;

	private double oprEstimatedProductionMinutes;

	private const string QueryGetJobOperations = "SELECT DISTINCT sxkScheduleTreeID, sxkScheduleBranchID, sxkScheduleTaskID, sxkPlantID, sxkPlantDepartmentID, \r\n                               sxkProcessID, sxkStartActualDateTime, sxkEndActualDateTime, sxkUniqueID\r\n                FROM ScheduleTrees \r\n                INNER JOIN ScheduleTasks  ON sxkScheduleTreeID = sxtScheduleTreeID \r\n                INNER JOIN ScheduleAllocations ON sxdScheduleTreeID = sxkScheduleTreeID AND \r\n\t\t        sxdScheduleBranchID = sxkScheduleBranchID AND sxdScheduleTaskID = sxkScheduleTaskID ";

	private const string QueryGetAllJobOperations = "SELECT DISTINCT sxkScheduleTreeID, sxkScheduleBranchID, sxkScheduleTaskID, sxkPlantID, sxkPlantDepartmentID, \r\n                               sxkProcessID, sxkStartActualDateTime, sxkEndActualDateTime, sxkUniqueID\r\n                FROM ScheduleTrees \r\n                INNER JOIN ScheduleTasks  ON sxkScheduleTreeID = sxtScheduleTreeID \r\n                INNER JOIN ScheduleAllocations ON sxdScheduleTreeID = sxkScheduleTreeID AND \r\n\t\t        sxdScheduleBranchID = sxkScheduleBranchID AND sxdScheduleTaskID = sxkScheduleTaskID  WHERE CONVERT(DATE, sxdStartActualDateTime) < {0} \r\n                                          AND {1} <= CONVERT(DATE, sxdEndActualDateTime)\r\n                                          AND sxdMinutes > 0 and sxtType = 1 {2}\r\n                                          ORDER BY sxkStartActualDateTime asc";

	private const string QueryGetAllPastAndFutureJobOperations = "SELECT DISTINCT sxkScheduleTreeID, sxkScheduleBranchID, sxkScheduleTaskID, sxkPlantID, sxkPlantDepartmentID, \r\n                               sxkProcessID, sxkStartActualDateTime, sxkEndActualDateTime, sxkUniqueID\r\n                FROM ScheduleTrees \r\n                INNER JOIN ScheduleTasks  ON sxkScheduleTreeID = sxtScheduleTreeID \r\n                INNER JOIN ScheduleAllocations ON sxdScheduleTreeID = sxkScheduleTreeID AND \r\n\t\t        sxdScheduleBranchID = sxkScheduleBranchID AND sxdScheduleTaskID = sxkScheduleTaskID  WHERE (CONVERT(DATE, sxdStartActualDateTime) < {1} \r\n                                                    OR {0} < CONVERT(DATE, sxdEndActualDateTime))\r\n                                           AND sxdMinutes > 0 and sxtType = 1 {2}\r\n                                           ORDER BY sxkStartActualDateTime asc";

	private const string QueryGetAllPastJobOperations = "SELECT DISTINCT sxkScheduleTreeID, sxkScheduleBranchID, sxkScheduleTaskID, sxkPlantID, sxkPlantDepartmentID, \r\n                               sxkProcessID, sxkStartActualDateTime, sxkEndActualDateTime, sxkUniqueID\r\n                FROM ScheduleTrees \r\n                INNER JOIN ScheduleTasks  ON sxkScheduleTreeID = sxtScheduleTreeID \r\n                INNER JOIN ScheduleAllocations ON sxdScheduleTreeID = sxkScheduleTreeID AND \r\n\t\t        sxdScheduleBranchID = sxkScheduleBranchID AND sxdScheduleTaskID = sxkScheduleTaskID  WHERE CONVERT(DATE, sxdStartActualDateTime) < {0}\r\n                                           AND sxdMinutes > 0 and sxtType = 1 {1} \r\n                                           ORDER BY sxkStartActualDateTime asc";

	private const string QueryGetAllFutureJobOperations = "SELECT DISTINCT sxkScheduleTreeID, sxkScheduleBranchID, sxkScheduleTaskID, sxkPlantID, sxkPlantDepartmentID, \r\n                               sxkProcessID, sxkStartActualDateTime, sxkEndActualDateTime, sxkUniqueID\r\n                FROM ScheduleTrees \r\n                INNER JOIN ScheduleTasks  ON sxkScheduleTreeID = sxtScheduleTreeID \r\n                INNER JOIN ScheduleAllocations ON sxdScheduleTreeID = sxkScheduleTreeID AND \r\n\t\t        sxdScheduleBranchID = sxkScheduleBranchID AND sxdScheduleTaskID = sxkScheduleTaskID  WHERE {0} < CONVERT(DATE, sxdEndActualDateTime) \r\n                                           AND sxdMinutes > 0 and sxtType = 1 {1}\r\n                                           ORDER BY sxkStartActualDateTime asc";

	private const string QueryCapacityCalendar = "SELECT jmyHours, jmydayOfWeek, jmyDayStartTime, xawWorkCenterID, calendarDate.CurrentDate\r\n              FROM WorkCenters \r\n              LEFT OUTER JOIN ProductionCalendarDays ON jmyPlantID = jmyPlantID AND jmyWorkCenterID = ''\r\n              CROSS APPLY (SELECT CurrentDate = CONVERT(Date, CAST(isnull(jmyProductionCalendarYearID, 1900) as varchar) + '-' + \r\n                                                              CAST(isnull(jmyProductionCalendarMonth, 1) as varchar) + '-' + \r\n                                                              CAST(isnull(jmyProductionCalendarDay, 1) as varchar))) calendarDate\r\n              WHERE calendarDate.CurrentDate >= {0} AND calendarDate.CurrentDate <= {1} {2} ";

	public const double MaxUtilizationAllowed = 999.99;

	private object[,] loadFields;

	public List<WorkCenterLoad> WorkCentersLoad;

	public List<WorkCenterPastLoad> workCentersPastLoad;

	private const double TimeConversionFactorToHours = 60.0;

	private string _bucketDateFormat = "MMM d";

	private M1Database DataSetDb { get; }

	public DateTime FromDate { get; set; }

	public int NumberOfBuckets { get; set; }

	public string BucketType { get; set; }

	public int PerBucket { get; set; }

	public bool ExcludeSetupLoad { get; set; }

	public ShopLoadExplorer(M1Database database, DateTime fromDate, int numberOfBuckets, string bucketType, int perBucket, bool isFromShopLoadReport = false, bool loadData = true, bool showPastLoad = false, bool showFutureLoad = false)
	{
		DataSetDb = database;
		ReloadData(fromDate, numberOfBuckets, bucketType, perBucket, string.Empty, string.Empty, null, isFromShopLoadReport, excludeSetupLoad: false, loadData, showPastLoad, showFutureLoad);
	}

	public ShopLoadExplorer(M1Database database, DateTime fromDate, int numberOfBuckets, string bucketType, int perBucket, string plantId, string departmentId = "", string[] workCenters = null, bool isFromShopLoadReport = false, bool excludeSetupLoad = false, bool showPastLoad = false, bool showFutureLoad = false)
	{
		DataSetDb = database;
		ReloadData(fromDate, numberOfBuckets, bucketType, perBucket, plantId, departmentId, workCenters, isFromShopLoadReport, excludeSetupLoad, loaddata: true, showPastLoad, showFutureLoad);
	}

	public void ReloadData(DateTime fromDate, int numberOfBuckets, string bucketType, int perBucket, string plantId, string productionDepartmentId, string[] workCenters, bool isFromShopLoadReport, bool excludeSetupLoad, bool loaddata, bool showPastLoad, bool showFutureLoad)
	{
		FromDate = fromDate;
		NumberOfBuckets = numberOfBuckets;
		BucketType = bucketType;
		PerBucket = perBucket;
		ExcludeSetupLoad = excludeSetupLoad;
		WorkCentersLoad = new List<WorkCenterLoad>();
		workCentersPastLoad = new List<WorkCenterPastLoad>();
		if (isFromShopLoadReport)
		{
			LoadShopLoadReport(plantId, productionDepartmentId, workCenters, excludeSetupLoad, showPastLoad, showFutureLoad);
		}
		else if (loaddata)
		{
			LoadShopLoadExplorer(plantId, productionDepartmentId, workCenters, excludeSetupLoad, showPastLoad, showFutureLoad);
		}
		loadFields = GetLoadBucketFields(fromDate, numberOfBuckets, bucketType, perBucket);
	}

	private static object[,] GetLoadBucketFields(DateTime fromDate, int numberOfBuckets, string bucketType, int perBucket)
	{
		object[,] array = new object[numberOfBuckets, 8];
		for (int i = 1; i <= numberOfBuckets; i++)
		{
			DateTime dateTime = CalculateBucketDate(fromDate, bucketType, perBucket, i - 1);
			DateTime dateTime2 = CalculateBucketDate(fromDate, bucketType, perBucket, i);
			array[i - 1, 0] = dateTime;
			array[i - 1, 1] = dateTime2.AddDays(-1.0);
			array[i - 1, 2] = "bucket" + i;
			array[i - 1, 3] = "nonworkingday" + i;
			array[i - 1, 4] = "loadBucket" + i;
			array[i - 1, 5] = "loadPercentBucket" + i;
			array[i - 1, 6] = "startDate" + i;
			array[i - 1, 7] = "setupLoadBucket" + i;
		}
		return array;
	}

	private static DateTime CalculateBucketDate(DateTime startDate, string timeSpanUnit, int timesPerSpanUnit, int startPosition)
	{
		return timeSpanUnit.ToLower() switch
		{
			"d" => startDate.AddDays(startPosition * timesPerSpanUnit), 
			"ww" => startDate.AddDays(startPosition * timesPerSpanUnit * 7), 
			"m" => startDate.AddMonths(startPosition * timesPerSpanUnit), 
			"q" => startDate.AddMonths(startPosition * timesPerSpanUnit * 3), 
			_ => startDate, 
		};
	}

	public DataTable CreateEmptyDataTable()
	{
		DataTable dataTable = DataSetDb.GetDataTable(new SqlCommand("Select xawWorkCenterID from WorkCenters where 0=1"));
		dataTable.Columns.Clear();
		dataTable.Columns.Add("DepartmentDescription", typeof(string));
		dataTable.Columns.Add("JobID", typeof(string));
		dataTable.Columns.Add("PlantID", typeof(string));
		dataTable.Columns.Add("StartDate", typeof(DateTime));
		dataTable.Columns.Add("EndDate", typeof(DateTime));
		dataTable.Columns.Add("JobAssemblyID", typeof(int));
		dataTable.Columns.Add("JobOperationID", typeof(int));
		dataTable.Columns.Add("PartID", typeof(string));
		dataTable.Columns.Add("PartRevisionID", typeof(string));
		dataTable.Columns.Add("PartShortDescription", typeof(string));
		dataTable.Columns.Add("CustomerName", typeof(string));
		dataTable.Columns.Add("IsFromShopLoad", typeof(bool));
		dataTable.Columns.Add("LoadStartDate", typeof(DateTime));
		dataTable.Columns.Add("BucketType", typeof(string));
		dataTable.Columns.Add("BucketDuration", typeof(int));
		dataTable.Columns.Add("xawWorkCenterID", typeof(string));
		dataTable.Columns.Add("DepartmentID", typeof(string));
		dataTable.Columns["DepartmentID"].Caption = "Department";
		dataTable.Columns.Add("WorkCenterID", typeof(string));
		dataTable.Columns["WorkCenterID"].Caption = "Work Center";
		dataTable.Columns.Add("WorkCenterDescription", typeof(string));
		dataTable.Columns["WorkCenterDescription"].Caption = "Description";
		dataTable.Columns.Add("Capacity", typeof(double));
		dataTable.Columns["Capacity"].Caption = "Capacity";
		dataTable.Columns.Add("PastLoad", typeof(double));
		dataTable.Columns["PastLoad"].Caption = "Past Load";
		dataTable.Columns.Add("CurrentLoad", typeof(double));
		dataTable.Columns["CurrentLoad"].Caption = "Current Load";
		dataTable.Columns.Add("FutureLoad", typeof(double));
		dataTable.Columns["FutureLoad"].Caption = "Future Load";
		dataTable.Columns.Add("Utilization", typeof(double));
		dataTable.Columns["Utilization"].Caption = "Util %";
		for (int i = 0; i < loadFields.GetLength(0); i++)
		{
			string caption = Convert.ToDateTime(loadFields[i, 0]).ToString(_bucketDateFormat);
			dataTable.Columns.Add(loadFields[i, 4].ToString(), typeof(double));
			dataTable.Columns[loadFields[i, 4].ToString()].Caption = caption;
			dataTable.Columns.Add(loadFields[i, 5].ToString(), typeof(double));
			dataTable.Columns[loadFields[i, 5].ToString()].Caption = caption;
			dataTable.Columns.Add(loadFields[i, 6].ToString(), typeof(string));
			dataTable.Columns.Add(loadFields[i, 7].ToString(), typeof(double));
			dataTable.Columns.Add("bucketCellCapacity" + (i + 1), typeof(double));
		}
		return dataTable;
	}

	public DataTable FillDataTable(bool isFromShopLoadReport = false)
	{
		DataTable dataTable = CreateEmptyDataTable();
		foreach (WorkCenterLoad item in WorkCentersLoad)
		{
			DataRow dataRow = dataTable.NewRow();
			if (isFromShopLoadReport)
			{
				dataRow["DepartmentDescription"] = item.ProductionDepartDescription;
				dataRow["JobID"] = item.JobId;
				dataRow["JobAssemblyID"] = item.JobAssemblyId;
				dataRow["JobOperationID"] = item.JobOperationId;
				dataRow["PartID"] = item.PartId;
				dataRow["PartRevisionID"] = "";
				dataRow["PartShortDescription"] = item.PartShortDescription;
				dataRow["CustomerName"] = item.CustomerName;
				dataRow["StartDate"] = item.StartDate;
				dataRow["EndDate"] = item.EndDate;
			}
			dataRow["IsFromShopLoad"] = true;
			dataRow["LoadStartDate"] = FromDate;
			dataRow["BucketType"] = BucketType;
			dataRow["BucketDuration"] = PerBucket;
			dataRow["xawWorkCenterID"] = item.WorkCenterId;
			dataRow["PlantID"] = item.PlantId;
			dataRow["DepartmentID"] = item.ProductionDepartmentId;
			dataRow["WorkCenterID"] = item.WorkCenterId;
			dataRow["WorkCenterDescription"] = item.Description;
			dataRow["Capacity"] = item.Capacity;
			dataRow["PastLoad"] = item.PastLoad;
			dataRow["CurrentLoad"] = item.Load;
			dataRow["FutureLoad"] = item.FutureLoad;
			double num = ((item.Capacity == 0.0) ? 0.0 : (item.Load / item.Capacity * 100.0));
			dataRow["Utilization"] = ((num > 999.99) ? 999.99 : num);
			int num2 = 1;
			foreach (BucketCell bucket in item.Buckets)
			{
				dataRow["loadBucket" + num2] = Math.Round(bucket.Load, 2);
				dataRow["bucketCellCapacity" + num2] = Math.Round(bucket.Capacity, 2);
				double num3 = bucket.Load / bucket.Capacity;
				dataRow["loadPercentBucket" + num2] = ((double.IsNaN(num3) || double.IsInfinity(num3)) ? 0.0 : num3);
				dataRow["startDate" + num2] = Convert.ToDateTime(bucket.StartDate).ToString(_bucketDateFormat);
				dataRow["setupLoadBucket" + num2] = Math.Round(bucket.SetupLoad, 2);
				num2++;
			}
			dataTable.Rows.Add(dataRow);
		}
		return dataTable;
	}

	public void LoadShopLoadExplorer(string plantId = "", string departmentId = "", string[] workCenters = null, bool excludeSetupLoad = false, bool showPastLoad = false, bool showFutureLoad = false)
	{
		DateTime[,] bucketDates = GetBucketDates(FromDate, NumberOfBuckets, BucketType, PerBucket);
		DateTime dateTime = bucketDates[0, 0];
		DateTime dateTime2 = bucketDates[NumberOfBuckets - 1, 1];
		using ScheduleCache scheduleCache = ScheduleProcess.LoadCache(DataSetDb);
		_bucketDateFormat = ((dateTime.Year != dateTime2.Year) ? "MMM d,yy" : "MMM d");
		DataTable allWorkCenters = GetAllWorkCenters(plantId, departmentId);
		DateTime endDate = dateTime2.AddDays(1.0);
		DataTable workCenterPastLoadData = new DataTable();
		DataTable workCenterFutureLoadData = new DataTable();
		if (showPastLoad)
		{
			workCenterPastLoadData = GetAllWCLoadPast(dateTime, plantId, departmentId);
		}
		if (showFutureLoad)
		{
			workCenterFutureLoadData = GetAllWCLoadFuture(endDate, plantId, departmentId);
		}
		foreach (DataRow row in allWorkCenters.Rows)
		{
			string plantId2 = (string.IsNullOrEmpty(plantId) ? row.Field<string>("xawPlantID") : plantId);
			string workCenterId = row.Field<string>("xawWorkCenterID");
			short num = row.Field<short>("xawNumberOfMachines");
			int operationId = 0;
			WorkCenterLoad workCenterLoad = new WorkCenterLoad(workCenterId)
			{
				NumberOfMachines = num
			};
			if (showPastLoad)
			{
				workCenterLoad.PastLoad = GetPastLoad(dateTime, dateTime2, workCenterId, num, departmentId, plantId2, string.Empty, 0, scheduleCache, workCenterPastLoadData, excludeSetupLoad, operationId);
			}
			GetWorkCenterCurrentLoad(dateTime, dateTime2, workCenterLoad, plantId2, bucketDates, scheduleCache, row);
			if (showFutureLoad)
			{
				workCenterLoad.FutureLoad = GetFutureLoad(dateTime, dateTime2, workCenterId, num, departmentId, plantId2, string.Empty, 0, scheduleCache, workCenterFutureLoadData, excludeSetupLoad, operationId);
			}
			WorkCentersLoad.Add(workCenterLoad);
		}
	}

	private IList<BucketCell> CreateBucketCellLoadInfoList(ScheduleCache cache, string wcId, IEnumerable<DataRow> dataRows, IList<BucketCell> wcBucketCells, bool setupLoad = false)
	{
		if (dataRows != null && dataRows.Any())
		{
			foreach (DataRow dataRow in dataRows)
			{
				if (dataRow.Field<byte>("sxdDateType") == 3)
				{
					timeCardProductionMinutes = (double)dataRow.Field<decimal>("actualMinutes");
					oprEstimatedProductionMinutes = (double)dataRow.Field<decimal>("jmoEstimatedProductionHours") * 60.0;
				}
				else
				{
					timeCardSetupMinutes = (double)dataRow.Field<decimal>("actualMinutes");
					oprSetupMinutes = (double)dataRow.Field<decimal>("jmoSetupHours") * 60.0;
				}
				DateTime dateTime = dataRow.Field<DateTime>("sxdStartActualDateTime");
				DateTime dateTime2 = dataRow.Field<DateTime>("sxdEndActualDateTime");
				ResourceCalendarDefinition calendar = cache.ResourceGroups[ScheduleProcess.ResourceTypes.WorkCenters].Values.FirstOrDefault((IResourceGroup x) => x.DisplayID.ToString() == wcId)?.Calendar;
				Dictionary<DateTime, StartTimeAndHours> workingDaysInRange = ScheduleProcess.GetWorkingDaysInRange(DataSetDb, cache, calendar, dateTime.Date, dateTime2.Date);
				DateTime dateTime3 = dateTime2;
				DateTime operationTime = dateTime.Date;
				while (operationTime <= dateTime3.Date)
				{
					workingDaysInRange.TryGetValue(operationTime, out var value);
					if (value != null && value.Hours > default(decimal))
					{
						DateTime dateTime4 = operationTime.AddMinutes(Convert.ToDouble(value.StartTime));
						DateTime dateTime5 = dateTime4.AddHours(Convert.ToDouble(value.Hours));
						double num;
						if (operationTime.Subtract(dateTime.Date).TotalMinutes != 0.0 || dateTime.Subtract(dateTime4).TotalMinutes == 0.0)
						{
							num = ((dateTime2.Date.Subtract(operationTime.Date).Days != 0) ? (Convert.ToDouble(value.Hours) * 60.0) : new DateTime[2] { dateTime2, dateTime5 }.Min().Subtract(dateTime4).TotalMinutes);
						}
						else
						{
							DateTime value2 = new DateTime[2] { dateTime, dateTime4 }.Max();
							num = ((dateTime2.Date.Subtract(dateTime.Date).Days == 0) ? dateTime2.Subtract(value2).TotalMinutes : dateTime5.Subtract(value2).TotalMinutes);
						}
						if (dataRow.Field<byte>("sxdDateType") == 3)
						{
							if (timeCardProductionMinutes <= num && timeCardProductionMinutes > 0.0)
							{
								num -= timeCardProductionMinutes;
								timeCardProductionMinutes = 0.0;
							}
							else
							{
								timeCardProductionMinutes = ((timeCardProductionMinutes > 0.0) ? (timeCardProductionMinutes - num) : 0.0);
								num = ((timeCardProductionMinutes > 0.0) ? 0.0 : num);
							}
						}
						if (wcBucketCells.Last().EndDate.Subtract(operationTime).TotalMinutes >= 0.0)
						{
							BucketCell bucketCell = wcBucketCells.FirstOrDefault((BucketCell b) => b.StartDate <= operationTime.Date && b.EndDate >= operationTime.Date);
							if (bucketCell != null)
							{
								if (setupLoad)
								{
									if (dataRow.Field<bool>("setupComplete"))
									{
										bucketCell.SetupLoad += ((num < 0.0) ? 0.0 : num);
									}
									else if (timeCardSetupMinutes < num && timeCardSetupMinutes > 0.0)
									{
										bucketCell.SetupLoad += timeCardSetupMinutes;
										timeCardSetupMinutes = 0.0;
									}
									else
									{
										bucketCell.SetupLoad = ((timeCardSetupMinutes > 0.0) ? num : 0.0);
										timeCardSetupMinutes -= ((timeCardSetupMinutes > 0.0) ? num : 0.0);
									}
								}
								else
								{
									bucketCell.Load += ((num < 0.0) ? 0.0 : num);
								}
							}
						}
					}
					operationTime = operationTime.AddDays(1.0);
				}
			}
		}
		return wcBucketCells;
	}

	public void LoadShopLoadReport(string plantId = "", string departmentId = "", string[] workCenters = null, bool excludeSetupLoad = false, bool showPastLoad = false, bool showFutureLoad = false)
	{
		using ScheduleCache scheduleCache = ScheduleProcess.LoadCache(DataSetDb);
		DateTime[,] bucketDates = GetBucketDates(FromDate, NumberOfBuckets, BucketType, PerBucket);
		DateTime startDate = bucketDates[0, 0];
		DateTime endDate = bucketDates[NumberOfBuckets - 1, 1];
		_bucketDateFormat = ((startDate.Year != endDate.Year) ? "MMM d,yy" : "MMM d");
		DataTable allWCLoadWorkTime = GetAllWCLoadWorkTime(startDate, endDate);
		allWCLoadWorkTime.PrimaryKey = new DataColumn[1] { allWCLoadWorkTime.Columns["sxdUniqueID"] };
		DateTime endDate2 = endDate.AddDays(1.0);
		DataTable allJobOperations = GetAllJobOperations(startDate, endDate2, plantId);
		allJobOperations.PrimaryKey = new DataColumn[1] { allJobOperations.Columns["sxkUniqueID"] };
		DataTable dataTable = new DataTable();
		DataTable dataTable2 = new DataTable();
		if (showPastLoad)
		{
			dataTable = GetAllWCLoadPast(startDate, plantId, departmentId);
			dataTable.PrimaryKey = new DataColumn[1] { dataTable.Columns["sxdUniqueID"] };
		}
		if (showFutureLoad)
		{
			dataTable2 = GetAllWCLoadFuture(endDate2, plantId, departmentId);
			dataTable2.PrimaryKey = new DataColumn[1] { dataTable2.Columns["sxdUniqueID"] };
		}
		if (showPastLoad && showFutureLoad)
		{
			allJobOperations.Merge(GetAllPastAndFutureJobOperations(startDate, endDate2, plantId));
			allWCLoadWorkTime.Merge(dataTable);
			allWCLoadWorkTime.Merge(dataTable2);
		}
		else
		{
			if (showPastLoad)
			{
				allJobOperations.Merge(GetAllPastJobOperations(startDate, plantId));
				allWCLoadWorkTime.Merge(dataTable);
			}
			if (showFutureLoad)
			{
				allJobOperations.Merge(GetAllFutureJobOperations(endDate2, plantId));
				allWCLoadWorkTime.Merge(dataTable2);
			}
		}
		DataTable productionCalendarCapacityDT = GetProductionCalendarCapacityDT(bucketDates[0, 0], bucketDates[NumberOfBuckets - 1, 1], plantId);
		foreach (DataRow row3 in allJobOperations.Rows)
		{
			int num = row3.Field<int>("sxkScheduleTreeID");
			int num2 = row3.Field<int>("sxkScheduleBranchID");
			int num3 = row3.Field<int>("sxkScheduleTaskID");
			string text = "sxdScheduleTreeID = " + M1Util.ConvertToLinq(num) + " AND sxdScheduleBranchID = " + M1Util.ConvertToLinq(num2) + " AND sxdScheduleTaskID = " + M1Util.ConvertToLinq(num3) + " ";
			if (workCenters != null && workCenters.Any())
			{
				string text2 = string.Join(",", workCenters.Select((string workCenter) => "'" + workCenter + "'"));
				text = string.Concat(text, "AND xaqWorkCenterID IN (" + text2 + ") ");
			}
			DataRow[] array = allWCLoadWorkTime.Select(text);
			DataTable dataTable3 = new DataTable();
			if (array.Length != 0)
			{
				dataTable3 = array.CopyToDataTable();
			}
			if (dataTable3.Rows.Count <= 0)
			{
				continue;
			}
			DataRow row2 = dataTable3.Rows[0];
			string plantId2 = (string.IsNullOrEmpty(plantId) ? row2.Field<string>("xawPlantID") : plantId);
			string jobId = row2.Field<string>("jmpJobID");
			int assemblyId = row2.Field<int>("jmaJobAssemblyID");
			int operationId = row2.Field<int>("jmoJobOperationID");
			string text3 = row2.Field<string>("xaqWorkCenterID");
			string text4 = "xaqWorkCenterID = " + M1Util.ConvertToLinq(text3) + " ";
			_ = $"sxdDateType = {3}";
			string text5 = $"sxdDateType = {2}";
			WorkCenterLoad workCenterLoad = new WorkCenterLoad(text3);
			if (showPastLoad)
			{
				workCenterLoad.PastLoad = GetPastLoad(startDate, endDate, text3, row2.Field<short>("xawNumberOfMachines"), departmentId, plantId2, jobId, assemblyId, scheduleCache, dataTable, excludeSetupLoad, operationId);
			}
			if (showFutureLoad)
			{
				workCenterLoad.FutureLoad = GetFutureLoad(startDate, endDate, text3, row2.Field<short>("xawNumberOfMachines"), departmentId, plantId2, jobId, assemblyId, scheduleCache, dataTable2, excludeSetupLoad, operationId);
			}
			workCenterLoad.LoadValues(bucketDates);
			workCenterLoad.ProductionDepartmentId = row2.Field<string>("xaeProductionDepartmentID");
			workCenterLoad.Description = row2.Field<string>("xawDescription");
			workCenterLoad.PlantId = row2.Field<string>("xawPlantID");
			workCenterLoad.ProductionDepartDescription = row2.Field<string>("xaeDescription");
			workCenterLoad.JobId = row2.Field<string>("jmpJobID");
			workCenterLoad.JobAssemblyId = row2.Field<int>("sxdScheduleBranchID");
			workCenterLoad.JobOperationId = row2.Field<int>("jmoJobOperationId");
			workCenterLoad.PartId = row2.Field<string>("jmpPartID");
			workCenterLoad.PartShortDescription = row2.Field<string>("jmpPartShortDescription");
			workCenterLoad.CustomerName = row2.Field<string>("cmoName");
			workCenterLoad.NumberOfMachines = row2.Field<short>("xawNumberOfMachines");
			workCenterLoad.StartDate = row2.Field<DateTime>("jmoStartDate");
			workCenterLoad.EndDate = row2.Field<DateTime>("jmoDueDate");
			workCenterLoad.Capacity = GetCapacityPerWorkCenter(text3, bucketDates[0, 0], bucketDates[NumberOfBuckets - 1, 1], plantId2, productionCalendarCapacityDT, scheduleCache) * (double)workCenterLoad.NumberOfMachines;
			DataRow[] dataRows = dataTable3.Select(text4 + " ");
			IList<BucketCell> source = CreateBucketCellLoadInfoList(scheduleCache, workCenterLoad.WorkCenterId, dataRows, workCenterLoad.Buckets);
			DataRow[] dataRows2 = dataTable3.Select(text4 + " And " + text5);
			IList<BucketCell> source2 = CreateBucketCellLoadInfoList(scheduleCache, workCenterLoad.WorkCenterId, dataRows2, workCenterLoad.Buckets, setupLoad: true);
			foreach (BucketCell bucketCell in workCenterLoad.Buckets)
			{
				double load = source.ToList().Find((BucketCell x) => x == bucketCell).Load;
				bucketCell.Load = load / 60.0;
				double setupLoad = source2.ToList().Find((BucketCell x) => x == bucketCell).SetupLoad;
				bucketCell.SetupLoad = setupLoad / 60.0;
				if (excludeSetupLoad)
				{
					bucketCell.Load = (load - setupLoad) / 60.0;
				}
				workCenterLoad.Load += Math.Round(bucketCell.Load, 2);
				workCenterLoad.SetupLoad += bucketCell.SetupLoad;
				bucketCell.Capacity = GetCapacityPerWorkCenter(text3, bucketCell.StartDate, bucketCell.EndDate, plantId2, productionCalendarCapacityDT, scheduleCache) * (double)workCenterLoad.NumberOfMachines;
			}
			WorkCentersLoad.Add(workCenterLoad);
		}
	}

	public double GetPastLoad(DateTime startDate, DateTime endDate, string workCenterId, int nMachines, string departmentId, string plantId, string jobId, int assemblyId, ScheduleCache scheduleCache, DataTable workCenterPastLoadData, bool excludeSetupLoad, int operationId)
	{
		double num = 0.0;
		double num2 = 0.0;
		string filterExpression = (string.IsNullOrWhiteSpace(jobId) ? ("xaqWorkCenterID = " + M1Util.ConvertToLinq(workCenterId)) : ("xaqWorkCenterID = " + M1Util.ConvertToLinq(workCenterId) + " AND jmpJobID = " + M1Util.ConvertToLinq(jobId) + " AND jmaJobAssemblyID = " + M1Util.ConvertToLinq(assemblyId) + " AND jmoJobOperationID = " + M1Util.ConvertToLinq(operationId)));
		DataRow[] array = workCenterPastLoadData.Select(filterExpression);
		if (array.Any())
		{
			DateTime dateTime = array.Select((DataRow row) => row.Field<DateTime>("sxdStartActualDateTime").Date).Min();
			endDate = array.Select((DataRow row) => row.Field<DateTime>("sxdEndActualDateTime").Date).Max().AddDays(1.0);
			DateTime dateTime2 = ((endDate > startDate) ? startDate : endDate);
			int numberOfBuckets = (((dateTime2 - dateTime).Days == 0) ? 1 : (dateTime2 - dateTime).Days);
			DateTime[,] bucketDates = GetBucketDates(dateTime, numberOfBuckets, "d", 1);
			WorkCenterPastLoad workCenterPastLoad = new WorkCenterPastLoad(workCenterId)
			{
				NumberOfMachines = nMachines
			};
			workCenterPastLoad.LoadPastValues(bucketDates);
			workCenterPastLoad.ProductionDepartmentId = workCenterId;
			workCenterPastLoad.PlantId = plantId;
			workCenterPastLoad.WorkCenterId = workCenterId;
			IEnumerable<DataRow> dataRows = from row in workCenterPastLoadData.Select(filterExpression)
				where row.Field<byte>("sxdDateType") == 2
				select row;
			CreateBucketCellLoadInfoList(scheduleCache, workCenterPastLoad.WorkCenterId, array, workCenterPastLoad.PastBuckets);
			CreateBucketCellLoadInfoList(scheduleCache, workCenterPastLoad.WorkCenterId, dataRows, workCenterPastLoad.PastBuckets, setupLoad: true);
			foreach (BucketCell item in workCenterPastLoad.PastBuckets.Where((BucketCell bucket) => bucket.Load > 0.0 || bucket.SetupLoad > 0.0))
			{
				double load = item.Load;
				double setupLoad = item.SetupLoad;
				item.Load = load / 60.0;
				item.SetupLoad = setupLoad / 60.0;
				if (excludeSetupLoad || setupLoad > 0.0)
				{
					item.Load = (load - setupLoad) / 60.0;
				}
				num += item.Load;
				num2 += item.SetupLoad;
			}
			workCenterPastLoad.PastLoad = Math.Round(num, 2);
			workCenterPastLoad.PastSetupLoad = Math.Round(num2, 2);
		}
		return Math.Round(num, 2);
	}

	public double GetFutureLoad(DateTime startDate, DateTime endDate, string workCenterId, int nMachines, string departmentId, string plantId, string jobId, int assemblyId, ScheduleCache scheduleCache, DataTable workCenterFutureLoadData, bool excludeSetupLoad, int operationId)
	{
		double num = 0.0;
		double num2 = 0.0;
		string filterExpression = (string.IsNullOrWhiteSpace(jobId) ? ("xaqWorkCenterID = " + M1Util.ConvertToLinq(workCenterId)) : ("xaqWorkCenterID = " + M1Util.ConvertToLinq(workCenterId) + " AND jmpJobID = " + M1Util.ConvertToLinq(jobId) + " AND jmaJobAssemblyID = " + M1Util.ConvertToLinq(assemblyId) + " AND jmoJobOperationID = " + M1Util.ConvertToLinq(operationId)));
		DataRow[] array = workCenterFutureLoadData.Select(filterExpression);
		if (array.Any())
		{
			DateTime dateTime = array.Select((DataRow row) => row.Field<DateTime>("sxdStartActualDateTime").Date).Min();
			DateTime dateTime2 = ((dateTime < endDate.AddDays(1.0)) ? endDate.AddDays(1.0) : dateTime);
			DateTime dateTime3 = array.Select((DataRow row) => row.Field<DateTime>("sxdEndActualDateTime").Date).Max();
			int numberOfBuckets = (((dateTime3 - dateTime2).Days + 1 == 0) ? 1 : ((dateTime3 - dateTime2).Days + 1));
			DateTime[,] bucketDates = GetBucketDates(dateTime2, numberOfBuckets, "d", 1);
			WorkCenterFutureLoad workCenterFutureLoad = new WorkCenterFutureLoad(workCenterId)
			{
				NumberOfMachines = nMachines
			};
			workCenterFutureLoad.loadFutureValues(bucketDates);
			workCenterFutureLoad.ProductionDepartmentId = workCenterId;
			workCenterFutureLoad.PlantId = plantId;
			workCenterFutureLoad.WorkCenterId = workCenterId;
			IEnumerable<DataRow> dataRows = from row in workCenterFutureLoadData.Select(filterExpression)
				where row.Field<byte>("sxdDateType") == 2
				select row;
			CreateBucketCellLoadInfoList(scheduleCache, workCenterFutureLoad.WorkCenterId, array, workCenterFutureLoad.FutureBuckets);
			CreateBucketCellLoadInfoList(scheduleCache, workCenterFutureLoad.WorkCenterId, dataRows, workCenterFutureLoad.FutureBuckets, setupLoad: true);
			foreach (BucketCell item in workCenterFutureLoad.FutureBuckets.Where((BucketCell bucket) => bucket.Load > 0.0 || bucket.SetupLoad > 0.0))
			{
				double load = item.Load;
				double setupLoad = item.SetupLoad;
				item.Load = load / 60.0;
				item.SetupLoad = setupLoad / 60.0;
				if (excludeSetupLoad || setupLoad > 0.0)
				{
					item.Load = (load - setupLoad) / 60.0;
				}
				num += item.Load;
				num2 += item.SetupLoad;
			}
			workCenterFutureLoad.FutureLoad = Math.Round(num, 2);
			workCenterFutureLoad.FutureSetupLoad = Math.Round(num2, 2);
		}
		return Math.Round(num, 2);
	}

	private void GetWorkCenterCurrentLoad(DateTime startPeriodDate, DateTime endPeriodDate, WorkCenterLoad workCenterLoad, string plantId, DateTime[,] bucketDates, ScheduleCache scheduleCache, DataRow item)
	{
		string filterExpression = "xaqWorkCenterID = " + M1Util.ConvertToLinq(workCenterLoad.WorkCenterId);
		DataTable allWCLoadWorkTime = GetAllWCLoadWorkTime(startPeriodDate, endPeriodDate);
		DataTable productionCalendarCapacityDT = GetProductionCalendarCapacityDT(bucketDates[0, 0], bucketDates[NumberOfBuckets - 1, 1], plantId);
		workCenterLoad.Capacity = GetCapacityPerWorkCenter(workCenterLoad.WorkCenterId, bucketDates[0, 0], bucketDates[NumberOfBuckets - 1, 1], plantId, productionCalendarCapacityDT, scheduleCache) * (double)workCenterLoad.NumberOfMachines;
		workCenterLoad.LoadValues(bucketDates);
		workCenterLoad.ProductionDepartmentId = item.Field<string>("xawProductionDepartmentID");
		workCenterLoad.Description = item.Field<string>("xawDescription");
		workCenterLoad.PlantId = item.Field<string>("xawPlantID");
		IEnumerable<DataRow> dataRows = from row in allWCLoadWorkTime.Select(filterExpression)
			where row.Field<DateTime>("sxdStartActualDateTime").Date <= endPeriodDate && startPeriodDate <= row.Field<DateTime>("sxdEndActualDateTime").Date
			select row;
		IEnumerable<DataRow> dataRows2 = from row in allWCLoadWorkTime.Select(filterExpression)
			where row.Field<DateTime>("sxdStartActualDateTime").Date <= endPeriodDate && startPeriodDate <= row.Field<DateTime>("sxdEndActualDateTime").Date && row.Field<byte>("sxdDateType") == 2
			select row;
		CreateBucketCellLoadInfoList(scheduleCache, workCenterLoad.WorkCenterId, dataRows, workCenterLoad.Buckets);
		CreateBucketCellLoadInfoList(scheduleCache, workCenterLoad.WorkCenterId, dataRows2, workCenterLoad.Buckets, setupLoad: true);
		foreach (BucketCell item2 in workCenterLoad.Buckets.Where((BucketCell bucket) => bucket.Load > 0.0 || bucket.SetupLoad > 0.0))
		{
			double load = item2.Load;
			double setupLoad = item2.SetupLoad;
			item2.Load = load / 60.0;
			item2.SetupLoad = setupLoad / 60.0;
			if (setupLoad > 0.0)
			{
				item2.Load = (load - setupLoad) / 60.0;
			}
			workCenterLoad.Load += item2.Load;
			workCenterLoad.SetupLoad += item2.SetupLoad;
			item2.Capacity = GetCapacityPerWorkCenter(workCenterLoad.WorkCenterId, item2.StartDate, item2.EndDate, plantId, productionCalendarCapacityDT, scheduleCache) * (double)workCenterLoad.NumberOfMachines;
		}
	}

	public DataTable GetAllWorkCenters(string plantId, string productionDepartmentId)
	{
		string text = string.Empty;
		string format = "SELECT xawProductionDepartmentID, xawWorkCenterID, xawDescription, xawPlantID, xawNumberOfMachines\r\n                  FROM dbo.WorkCenters \r\n                  WHERE xawExcludeFromShopLoad = 0 {0} \r\n                  ORDER by xawProductionDepartmentID";
		if (!string.IsNullOrEmpty(plantId))
		{
			text = "AND xawPlantID = " + M1Util.ConvertToSql(plantId);
		}
		if (!string.IsNullOrEmpty(productionDepartmentId))
		{
			text = text + "AND xawProductionDepartmentID = " + M1Util.ConvertToSql(productionDepartmentId);
		}
		format = string.Format(format, text);
		return DataSetDb.GetDataTable(format);
	}

	public DataTable GetAllJobOperations(DateTime startDate, DateTime endDate, string plantId)
	{
		string arg = string.Empty;
		string format = "SELECT DISTINCT sxkScheduleTreeID, sxkScheduleBranchID, sxkScheduleTaskID, sxkPlantID, sxkPlantDepartmentID, \r\n                               sxkProcessID, sxkStartActualDateTime, sxkEndActualDateTime, sxkUniqueID\r\n                FROM ScheduleTrees \r\n                INNER JOIN ScheduleTasks  ON sxkScheduleTreeID = sxtScheduleTreeID \r\n                INNER JOIN ScheduleAllocations ON sxdScheduleTreeID = sxkScheduleTreeID AND \r\n\t\t        sxdScheduleBranchID = sxkScheduleBranchID AND sxdScheduleTaskID = sxkScheduleTaskID  WHERE CONVERT(DATE, sxdStartActualDateTime) < {0} \r\n                                          AND {1} <= CONVERT(DATE, sxdEndActualDateTime)\r\n                                          AND sxdMinutes > 0 and sxtType = 1 {2}\r\n                                          ORDER BY sxkStartActualDateTime asc";
		if (!string.IsNullOrEmpty(plantId))
		{
			arg = "AND sxkPlantID = " + M1Util.ConvertToSql(plantId);
		}
		format = string.Format(format, M1Util.ConvertToSql(endDate), M1Util.ConvertToSql(startDate), arg);
		return DataSetDb.GetDataTable(format);
	}

	public DataTable GetAllPastAndFutureJobOperations(DateTime startDate, DateTime endDate, string plantId)
	{
		string arg = string.Empty;
		string format = "SELECT DISTINCT sxkScheduleTreeID, sxkScheduleBranchID, sxkScheduleTaskID, sxkPlantID, sxkPlantDepartmentID, \r\n                               sxkProcessID, sxkStartActualDateTime, sxkEndActualDateTime, sxkUniqueID\r\n                FROM ScheduleTrees \r\n                INNER JOIN ScheduleTasks  ON sxkScheduleTreeID = sxtScheduleTreeID \r\n                INNER JOIN ScheduleAllocations ON sxdScheduleTreeID = sxkScheduleTreeID AND \r\n\t\t        sxdScheduleBranchID = sxkScheduleBranchID AND sxdScheduleTaskID = sxkScheduleTaskID  WHERE (CONVERT(DATE, sxdStartActualDateTime) < {1} \r\n                                                    OR {0} < CONVERT(DATE, sxdEndActualDateTime))\r\n                                           AND sxdMinutes > 0 and sxtType = 1 {2}\r\n                                           ORDER BY sxkStartActualDateTime asc";
		if (!string.IsNullOrEmpty(plantId))
		{
			arg = "AND sxkPlantID = " + M1Util.ConvertToSql(plantId);
		}
		format = string.Format(format, M1Util.ConvertToSql(endDate), M1Util.ConvertToSql(startDate), arg);
		return DataSetDb.GetDataTable(format);
	}

	public DataTable GetAllPastJobOperations(DateTime startDate, string plantId)
	{
		string arg = string.Empty;
		string format = "SELECT DISTINCT sxkScheduleTreeID, sxkScheduleBranchID, sxkScheduleTaskID, sxkPlantID, sxkPlantDepartmentID, \r\n                               sxkProcessID, sxkStartActualDateTime, sxkEndActualDateTime, sxkUniqueID\r\n                FROM ScheduleTrees \r\n                INNER JOIN ScheduleTasks  ON sxkScheduleTreeID = sxtScheduleTreeID \r\n                INNER JOIN ScheduleAllocations ON sxdScheduleTreeID = sxkScheduleTreeID AND \r\n\t\t        sxdScheduleBranchID = sxkScheduleBranchID AND sxdScheduleTaskID = sxkScheduleTaskID  WHERE CONVERT(DATE, sxdStartActualDateTime) < {0}\r\n                                           AND sxdMinutes > 0 and sxtType = 1 {1} \r\n                                           ORDER BY sxkStartActualDateTime asc";
		if (!string.IsNullOrEmpty(plantId))
		{
			arg = "AND sxkPlantID = " + M1Util.ConvertToSql(plantId);
		}
		format = string.Format(format, M1Util.ConvertToSql(startDate), arg);
		return DataSetDb.GetDataTable(format);
	}

	public DataTable GetAllFutureJobOperations(DateTime endDate, string plantId)
	{
		string arg = string.Empty;
		string format = "SELECT DISTINCT sxkScheduleTreeID, sxkScheduleBranchID, sxkScheduleTaskID, sxkPlantID, sxkPlantDepartmentID, \r\n                               sxkProcessID, sxkStartActualDateTime, sxkEndActualDateTime, sxkUniqueID\r\n                FROM ScheduleTrees \r\n                INNER JOIN ScheduleTasks  ON sxkScheduleTreeID = sxtScheduleTreeID \r\n                INNER JOIN ScheduleAllocations ON sxdScheduleTreeID = sxkScheduleTreeID AND \r\n\t\t        sxdScheduleBranchID = sxkScheduleBranchID AND sxdScheduleTaskID = sxkScheduleTaskID  WHERE {0} < CONVERT(DATE, sxdEndActualDateTime) \r\n                                           AND sxdMinutes > 0 and sxtType = 1 {1}\r\n                                           ORDER BY sxkStartActualDateTime asc";
		if (!string.IsNullOrEmpty(plantId))
		{
			arg = "AND sxkPlantID = " + M1Util.ConvertToSql(plantId);
		}
		format = string.Format(format, M1Util.ConvertToSql(endDate), arg);
		return DataSetDb.GetDataTable(format);
	}

	public DataTable GetAllWCLoadWorkTime(DateTime startDate, DateTime endDate)
	{
		string arg = "CONVERT(DATE, sxdStartActualDateTime) <= " + M1Util.ConvertToSql(endDate) + " AND " + M1Util.ConvertToSql(startDate) + " <= CONVERT(DATE, sxdEndActualDateTime)";
		string format = new QueryBuilder().BuildShopLoadQuery();
		format = string.Format(format, arg, "xaeProductionDepartmentID, xaqWorkCenterID, jmpJobID, sxdScheduleBranchID, sxdScheduleTaskID, sxdScheduleTreeID");
		return DataSetDb.GetDataTable(format);
	}

	public DataTable GetAllWCLoadPast(DateTime startDate, string plantId, string departmentId)
	{
		string arg = "CONVERT(DATE, sxdStartActualDateTime) < " + M1Util.ConvertToSql(startDate);
		string format = new QueryBuilder().BuildShopLoadQuery();
		format = string.Format(format, arg, " sxdEndActualDateTime ASC ");
		return DataSetDb.GetDataTable(format);
	}

	public DataTable GetAllWCLoadFuture(DateTime endDate, string plantId, string departmentId)
	{
		string arg = "CONVERT(DATE, sxdEndActualDateTime) >= " + M1Util.ConvertToSql(endDate);
		string format = new QueryBuilder().BuildShopLoadQuery();
		format = string.Format(format, arg, " sxdEndActualDateTime ASC ");
		return DataSetDb.GetDataTable(format);
	}

	public double GetCapacityPerWorkCenter(string workCenterId, DateTime startDate, DateTime endDate, string plantId, DataTable calendarCapacityDT, ScheduleCache scheduleCache)
	{
		ResourceCalendarDefinition calendar = scheduleCache.ResourceGroups[ScheduleProcess.ResourceTypes.WorkCenters].Values.Where((IResourceGroup x) => x.DisplayID.ToString() == workCenterId).FirstOrDefault().Calendar;
		return Convert.ToDouble(ScheduleProcess.GetWorkingDaysInRange(DataSetDb, scheduleCache, calendar, startDate.Date, endDate.Date).Sum((KeyValuePair<DateTime, StartTimeAndHours> d) => d.Value.Hours));
	}

	private DataTable GetProductionCalendarCapacityDT(DateTime startDate, DateTime endDate, string plantId)
	{
		string arg = " AND jmyPlantID = " + M1Util.ConvertToSql(plantId);
		string queryString = $"SELECT jmyHours, jmydayOfWeek, jmyDayStartTime, xawWorkCenterID, calendarDate.CurrentDate\r\n              FROM WorkCenters \r\n              LEFT OUTER JOIN ProductionCalendarDays ON jmyPlantID = jmyPlantID AND jmyWorkCenterID = ''\r\n              CROSS APPLY (SELECT CurrentDate = CONVERT(Date, CAST(isnull(jmyProductionCalendarYearID, 1900) as varchar) + '-' + \r\n                                                              CAST(isnull(jmyProductionCalendarMonth, 1) as varchar) + '-' + \r\n                                                              CAST(isnull(jmyProductionCalendarDay, 1) as varchar))) calendarDate\r\n              WHERE calendarDate.CurrentDate >= {M1Util.ConvertToSql(startDate)} AND calendarDate.CurrentDate <= {M1Util.ConvertToSql(endDate)} {arg} ";
		DataTable dataTable = DataSetDb.GetDataTable(queryString);
		if (dataTable.Rows.Count > 0)
		{
			return dataTable;
		}
		queryString = $"SELECT jmyHours, jmydayOfWeek, jmyDayStartTime, xawWorkCenterID, calendarDate.CurrentDate\r\n              FROM WorkCenters \r\n              LEFT OUTER JOIN ProductionCalendarDays ON jmyPlantID = jmyPlantID AND jmyWorkCenterID = ''\r\n              CROSS APPLY (SELECT CurrentDate = CONVERT(Date, CAST(isnull(jmyProductionCalendarYearID, 1900) as varchar) + '-' + \r\n                                                              CAST(isnull(jmyProductionCalendarMonth, 1) as varchar) + '-' + \r\n                                                              CAST(isnull(jmyProductionCalendarDay, 1) as varchar))) calendarDate\r\n              WHERE calendarDate.CurrentDate >= {M1Util.ConvertToSql(startDate)} AND calendarDate.CurrentDate <= {M1Util.ConvertToSql(endDate)} {string.Empty} ";
		return DataSetDb.GetDataTable(queryString);
	}

	public DateTime[,] GetBucketDates(DateTime fromDate, int numberOfBuckets, string bucketType, int perBucket)
	{
		DateTime[,] array = new DateTime[numberOfBuckets, 2];
		for (int i = 1; i <= numberOfBuckets; i++)
		{
			array[i - 1, 0] = CalculateBucketDate(fromDate, bucketType, perBucket, i - 1);
			array[i - 1, 1] = CalculateBucketDate(fromDate, bucketType, perBucket, i).AddDays(-1.0);
		}
		return array;
	}
}
