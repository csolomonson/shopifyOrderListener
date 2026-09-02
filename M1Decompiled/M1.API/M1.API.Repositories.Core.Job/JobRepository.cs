using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Utilities;
using M1.Core;
using M1.Extensions;

namespace M1.API.Repositories.Core.Job;

public class JobRepository : APIBaseRepository, IJobRepository, IAPIBaseRepository, IDisposable
{
	private readonly IJobAssemblyRepository _jobAssemblyRepository;

	private readonly string[] jobFields = new string[30]
	{
		"jmpJobID", "jmpCustomerOrganizationID", "jmpInventoryQuantity", "jmpClosed", "jmpFirm", "jmpPartID", "jmpPlanningComplete", "jmpProductionComplete", "jmpReleasedToFloor", "jmpScheduleComplete",
		"jmpOrderQuantity", "jmpPartBinID", "jmpPartForecastPeriodID", "jmpPartForecastYearID", "jmpNestlinkProcessed", "jmpPartRevisionID", "jmpPartWareHouseLocationID", "jmpPlantID", "jmpJobPriorityID", "jmpProductionDueDate",
		"jmpProductionQuantity", "jmpProjectAreaID", "jmpProjectID", "jmpQuantityReceivedToInventory", "jmpQuantityShipped", "jmpReworkDate", "jmpReworkQuantity", "jmpScrapQuantity", "jmpScrapQuantityCompleted", "jmpUnitOfMeasure"
	};

	private readonly string SELECT_JOBMETHOD = "SELECT  Jobs.jmpJobID, Jobs.jmpPlantID, Jobs.jmpProductionDueDate, Jobs.jmpCustomerOrganizationID, Jobs.jmpPartID, \r\n                                                    Jobs.jmpPartRevisionID, Jobs.jmpPartWareHouseLocationID, Jobs.jmpPartBinID, Jobs.jmpUnitOfMeasure, Jobs.jmpOrderQuantity, \r\n                                                    Jobs.jmpInventoryQuantity, Jobs.jmpScrapQuantity, Jobs.jmpReworkQuantity, Jobs.jmpProductionQuantity, Jobs.jmpPlanningComplete, \r\n                                                    Jobs.jmpScheduleComplete, Jobs.jmpReleasedToFloor, Jobs.jmpProductionComplete, Jobs.jmpScrapQuantityCompleted, Jobs.jmpReworkDate, \r\n                                                    Jobs.jmpClosed, Jobs.jmpFirm, Jobs.jmpScheduledDueDate, JobAssemblies.jmaJobID, JobAssemblies.jmaJobAssemblyID, JobAssemblies.jmaLevel, JobAssemblies.jmaSourceMethodID, \r\n                                                    JobAssemblies.jmaSourceRevisionID, JobAssemblies.jmaPartID, JobAssemblies.jmaPartRevisionID, JobAssemblies.jmaUnitOfMeasure, \r\n                                                    JobAssemblies.jmaPartShortDescription, JobAssemblies.jmaQuantityPerParent, JobAssemblies.jmaOrderQuantity, \r\n                                                    JobAssemblies.jmaProductionQuantity, JobAssemblies.jmaQuantityToMake, JobAssemblies.jmaEstimatedUnitCost, JobAssemblies.jmaScheduledDueDate,\r\n                                                    JobAssemblies.jmaOverlapOperationID, JobAssemblies.jmaOverlapType, JobAssemblies.jmaParentAssemblyID, JobOperations.jmoJobID, \r\n                                                    JobOperations.jmoJobAssemblyID, JobOperations.jmoOperationType, JobOperations.jmoJobOperationID, JobOperations.jmoWorkCenterID, \r\n                                                    JobOperations.jmoProcessID, JobOperations.jmoProcessShortDescription, JobOperations.jmoProductionStandard, \r\n                                                    JobOperations.jmoStandardFactor, JobOperations.jmoMachinesToSchedule, JobOperations.jmoMachineType, JobOperations.jmoDueDate,\r\n                                                    JobOperations.jmoQuantityPerAssembly, JobOperations.jmoSetupRate, JobOperations.jmoProductionRate, JobOperations.jmoOverheadRate, \r\n                                                    JobOperations.jmoPartID, JobOperations.jmoPartRevisionID, JobOperations.jmoUnitOfMeasure, JobMaterials.jmmJobID, \r\n                                                    JobMaterials.jmmJobAssemblyID, JobMaterials.jmmJobMaterialID, JobMaterials.jmmPartID, JobMaterials.jmmPartRevisionID, \r\n                                                    JobMaterials.jmmUnitOfMeasure, JobMaterials.jmmPartShortDescription, JobMaterials.jmmRelatedJobOperationID, \r\n                                                    JobMaterials.jmmQuantityPerAssembly, JobMaterials.jmmEstimatedQuantity, JobMaterials.jmmEstimatedUnitCost, \r\n                                                    JobMaterials.jmmCalculatedUnitCost, JobMaterials.jmmFirm, JobMaterials.jmmSupplierOrganizationID, JobMaterials.jmmPurchaseLocationId,\r\n                                                    JobMaterials.jmmPurchaseOrderID, JobMaterials.jmmLeadTime, JobMaterials.jmmMinimumCharge, JobMaterials.jmmDueInDate, JobMaterials.jmmRequiredDate,\r\n                                                    JobMaterials.jmmQuantityAllocated, JobMaterials.jmmQuantityReceived, JobMaterials.jmmScrapQuantityReceived, JobMaterials.jmmQuantityToInspect,\r\n                                                    JobMaterials.jmmQuantityToReturn, JobMaterials.jmmReceivedComplete, JobMaterials.jmmPurchaseToJobQuantity, JobMaterials.jmmPullAllFromStock,\r\n                                                    JobMaterials.jmmPullFromStockQuantity, JobMaterials.jmmClosed, JobMaterials.jmmUniqueID, JobMaterials.jmmRowVersion \r\n                                            FROM    JobAssemblies INNER JOIN\r\n                                                    Jobs ON JobAssemblies.jmaJobID = Jobs.jmpJobID LEFT OUTER JOIN\r\n                                                    JobMaterials ON JobAssemblies.jmaJobID = JobMaterials.jmmJobID AND \r\n                                                    JobAssemblies.jmaJobAssemblyID = JobMaterials.jmmJobAssemblyID LEFT OUTER JOIN\r\n                                                    JobOperations ON JobAssemblies.jmaJobID = JobOperations.jmoJobID AND \r\n                                                    JobAssemblies.jmaJobAssemblyID = JobOperations.jmoJobAssemblyID\r\n                                            WHERE   (Jobs.jmpJobID = @JobID)";

	private readonly string SELECT_JOB_GUIDS = "SELECT J.*,P.impUniqueID FROM (SELECT jmpJobID, jmpUniqueID, jmpPartID FROM Jobs WHERE jmpClosed=0 AND {0}) AS J INNER JOIN Parts P ON J.jmpPartID=P.impPartID";

	public JobRepository(APIClientContext clientContext)
	{
		_jobAssemblyRepository = new JobAssemblyRepository(clientContext);
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
	}

	public JobRepository(M1Database database)
	{
		base.M1database = database;
	}

	public Task<bool> DoesJobAssemblyExists(string jobId, int jobAssemblyId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmaJobID|C", jobId);
		base.filterList.Add("jmaJobAssemblyID", jobAssemblyId);
		base.selectList.Add("jmaJobID");
		return Task.FromResult(GetAsObject("JobAssemblies", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesJobExists(string jobId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmpJobID|C", jobId);
		base.selectList.Add("jmpJobID");
		return Task.FromResult(GetAsObject("Jobs", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesJobOperationExists(string jobId, int jobAssemblyId, int jobOperationId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmoJobID|C", jobId);
		base.filterList.Add("jmoJobAssemblyID", jobAssemblyId);
		base.filterList.Add("jmoJobOperationID", jobOperationId);
		base.selectList.Add("jmoJobID");
		return Task.FromResult(GetAsObject("JobOperations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<BOMJobGuidsDto> GetJobGuidsInfo(string jobId, string partId)
	{
		BOMJobGuidsDto bOMJobGuidsDto = new BOMJobGuidsDto();
		string empty = string.Empty;
		SqlCommand sqlCommand = null;
		new StringBuilder(SELECT_JOB_GUIDS);
		InitializeParameterLists();
		if (string.IsNullOrWhiteSpace(partId))
		{
			empty = "jmpJobID LiKE '%'+@jobId+'%'";
			sqlCommand = new SqlCommand(string.Format(SELECT_JOB_GUIDS, empty));
			sqlCommand.Parameters.AddWithValue("@jobId", jobId);
		}
		else if (string.IsNullOrWhiteSpace(jobId))
		{
			empty = "jmpPartID LiKE '%'+@partId+'%'";
			sqlCommand = new SqlCommand(string.Format(SELECT_JOB_GUIDS, empty));
			sqlCommand.Parameters.AddWithValue("@partId", partId);
		}
		else
		{
			empty = "jmpJobID LiKE '%'+@jobId+'%' AND jmpPartID LiKE '%'+@partId+'%'";
			sqlCommand = new SqlCommand(string.Format(SELECT_JOB_GUIDS, empty));
			sqlCommand.Parameters.AddWithValue("@jobId", jobId);
			sqlCommand.Parameters.AddWithValue("@partId", partId);
		}
		using (DataTable dataTable = base.M1database.GetDataTable(sqlCommand))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				List<BOMJobGuidDto> collection = (from x in dataTable.AsEnumerable()
					select new BOMJobGuidDto
					{
						JobId = x.Field<string>("jmpJobID"),
						JobGUID = x.Field<Guid>("jmpUniqueID").ToString(),
						PartId = x.Field<string>("jmpPartID"),
						PartGUID = x.Field<Guid>("impUniqueID").ToString()
					}).ToList();
				bOMJobGuidsDto.JobGuids = new List<BOMJobGuidDto>(collection);
			}
		}
		return Task.FromResult(bOMJobGuidsDto);
	}

	public Task<string> GetJobIdFromGuid(string jobIdString)
	{
		InitializeParameterLists();
		base.filterList.Add("jmpUniqueID|C", jobIdString);
		base.selectList.Add("jmpJobID");
		return Task.FromResult(GetAsObject("Jobs", base.filterList, base.selectList, null, null)?.ToString());
	}

	public Task<BOMJobDto> GetJobHeaderInfo(string jobId)
	{
		BOMJobDto result = new BOMJobDto();
		InitializeParameterLists();
		base.selectList.AddRange(new string[23]
		{
			"jmpJobID", "jmpPlantID", "jmpProductionDueDate", "jmpCustomerOrganizationID", "jmpPartID", "jmpPartRevisionID", "jmpPartWareHouseLocationID", "jmpPartBinID", "jmpUnitOfMeasure", "jmpOrderQuantity",
			"jmpInventoryQuantity", "jmpScrapQuantity", "jmpReworkQuantity", "jmpProductionQuantity", "jmpPlanningComplete", "jmpScheduleComplete", "jmpReleasedToFloor", "jmpProductionComplete", "jmpScrapQuantityCompleted", "jmpReworkDate",
			"jmpClosed", "jmpFirm", "jmpScheduledDueDate"
		});
		base.filterList.Add("jmpJobID|C", jobId);
		using (DataTable dataTable = GetAsDataTable("Jobs", base.filterList, base.selectList, null, null))
		{
			if (dataTable.Rows.Count > 0)
			{
				result = (from row in dataTable.AsEnumerable()
					select new BOMJobDto
					{
						JobID = row.Field<string>("jmpJobID"),
						PlantID = row.Field<string>("jmpPlantID"),
						ProductionDueDate = row.Field<DateTime>("jmpProductionDueDate"),
						CustomerOrganizationID = row.Field<string>("jmpCustomerOrganizationID"),
						PartID = row.Field<string>("jmpPartID"),
						PartRevisionID = row.Field<string>("jmpPartRevisionID"),
						PartWareHouseLocationID = row.Field<string>("jmpPartWareHouseLocationID"),
						PartBinID = row.Field<string>("jmpPartBinID"),
						JobPriorityID = row.Field<short>("jmpJobPriorityID"),
						UnitOfMeasure = row.Field<string>("jmpUnitOfMeasure"),
						OrderQuantity = row.Field<decimal>("jmpOrderQuantity"),
						InventoryQuantity = row.Field<decimal>("jmpInventoryQuantity"),
						ScrapQuantity = row.Field<decimal>("jmpScrapQuantity"),
						ReworkQuantity = row.Field<decimal>("jmpReworkQuantity"),
						ProductionQuantity = row.Field<decimal>("jmpProductionQuantity"),
						PlanningComplete = row.Field<bool>("jmpPlanningComplete"),
						ScheduleComplete = row.Field<bool>("jmpScheduleComplete"),
						ReleasedToFloor = row.Field<bool>("jmpReleasedToFloor"),
						ProductionComplete = row.Field<bool>("jmpProductionComplete"),
						ScrapQuantityCompleted = row.Field<decimal>("jmpScrapQuantityCompleted"),
						ReworkDate = row.Field<DateTime>("jmpReworkDate"),
						Closed = row.Field<bool>("jmpClosed"),
						NestlinkProcessed = row.Field<bool>("jmpNestlinkProcessed"),
						Firm = row.Field<bool>("jmpFirm"),
						DueDate = row.Field<DateTime>("jmpScheduledDueDate")
					}).FirstOrDefault();
			}
		}
		return Task.FromResult(result);
	}

	public Task<DataTable> GetJobMethodAsDataTable(string jobId)
	{
		using SqlCommand sqlCommand = new SqlCommand(SELECT_JOBMETHOD);
		sqlCommand.Parameters.AddWithValue("@JobID", jobId);
		return Task.FromResult(base.M1database.GetDataTable(sqlCommand));
	}

	public Task<ICollection<BOMJobDto>> GetAllJobs(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<BOMJobDto> collection = new List<BOMJobDto>();
		InitializeParameterLists();
		base.selectList.AddRange(jobFields);
		List<string> orderbyList = new List<string> { "jmpJobID" };
		using (DataTable dataTable = GetAsDataTable("Jobs", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BOMJobDto bOMJobDto = new BOMJobDto();
				bOMJobDto.JobID = dataTable.Rows[i].Field<string>("jmpJobID");
				bOMJobDto.CustomerOrganizationID = dataTable.Rows[i].Field<string>("jmpCustomerOrganizationID");
				bOMJobDto.InventoryQuantity = dataTable.Rows[i].Field<decimal>("jmpInventoryQuantity");
				bOMJobDto.Closed = dataTable.Rows[i].Field<bool>("jmpClosed");
				bOMJobDto.Firm = dataTable.Rows[i].Field<bool>("jmpFirm");
				bOMJobDto.PlanningComplete = dataTable.Rows[i].Field<bool>("jmpPlanningComplete");
				bOMJobDto.ProductionComplete = dataTable.Rows[i].Field<bool>("jmpProductionComplete");
				bOMJobDto.ReleasedToFloor = dataTable.Rows[i].Field<bool>("jmpReleasedToFloor");
				bOMJobDto.ScheduleComplete = dataTable.Rows[i].Field<bool>("jmpScheduleComplete");
				bOMJobDto.OrderQuantity = dataTable.Rows[i].Field<decimal>("jmpOrderQuantity");
				bOMJobDto.PartBinID = dataTable.Rows[i].Field<string>("jmpPartBinID");
				bOMJobDto.PartForecastPeriodID = dataTable.Rows[i].Field<short>("jmpPartForecastPeriodID");
				bOMJobDto.PartForecastYearID = dataTable.Rows[i].Field<short>("jmpPartForecastYearID");
				bOMJobDto.PartID = dataTable.Rows[i].Field<string>("jmpPartID");
				bOMJobDto.PartRevisionID = dataTable.Rows[i].Field<string>("jmpPartRevisionID");
				bOMJobDto.PartWareHouseLocationID = dataTable.Rows[i].Field<string>("jmpPartWareHouseLocationID");
				bOMJobDto.PlantID = dataTable.Rows[i].Field<string>("jmpPlantID");
				bOMJobDto.ProductionDueDate = dataTable.Rows[i].Field<DateTime?>("jmpProductionDueDate");
				bOMJobDto.ProductionQuantity = dataTable.Rows[i].Field<decimal>("jmpProductionQuantity");
				bOMJobDto.ProjectAreaID = dataTable.Rows[i].Field<string>("jmpProjectAreaID");
				bOMJobDto.ProjectID = dataTable.Rows[i].Field<string>("jmpProjectID");
				bOMJobDto.JobPriorityID = dataTable.Rows[i].Field<short>("jmpJobPriorityID");
				bOMJobDto.QuantityReceivedToInventory = dataTable.Rows[i].Field<decimal>("jmpQuantityReceivedToInventory");
				bOMJobDto.QuantityShipped = dataTable.Rows[i].Field<decimal>("jmpQuantityShipped");
				bOMJobDto.ReworkDate = dataTable.Rows[i].Field<DateTime?>("jmpReworkDate");
				bOMJobDto.ReworkQuantity = dataTable.Rows[i].Field<decimal>("jmpReworkQuantity");
				bOMJobDto.ScrapQuantity = dataTable.Rows[i].Field<decimal>("jmpScrapQuantity");
				bOMJobDto.ScrapQuantityCompleted = dataTable.Rows[i].Field<decimal>("jmpScrapQuantityCompleted");
				bOMJobDto.UnitOfMeasure = dataTable.Rows[i].Field<string>("jmpUnitOfMeasure");
				bOMJobDto.NestlinkProcessed = dataTable.Rows[i].Field<bool>("jmpNestlinkProcessed");
				collection.Add(bOMJobDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<BOMJobDto> GetJob(string jobId)
	{
		BOMJobDto bOMJobDto = new BOMJobDto();
		InitializeParameterLists();
		base.selectList.AddRange(jobFields);
		base.filterList.Add(Guid.TryParse(jobId, out var _) ? "jmpUniqueID|C" : "jmpJobID|C", jobId);
		using (DataTable dataTable = GetAsDataTable("Jobs", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(bOMJobDto);
			}
			bOMJobDto.JobID = dataTable.Rows[0].Field<string>("jmpJobID");
			bOMJobDto.CustomerOrganizationID = dataTable.Rows[0].Field<string>("jmpCustomerOrganizationID");
			bOMJobDto.InventoryQuantity = dataTable.Rows[0].Field<decimal>("jmpInventoryQuantity");
			bOMJobDto.Closed = dataTable.Rows[0].Field<bool>("jmpClosed");
			bOMJobDto.Firm = dataTable.Rows[0].Field<bool>("jmpFirm");
			bOMJobDto.PlanningComplete = dataTable.Rows[0].Field<bool>("jmpPlanningComplete");
			bOMJobDto.ProductionComplete = dataTable.Rows[0].Field<bool>("jmpProductionComplete");
			bOMJobDto.ReleasedToFloor = dataTable.Rows[0].Field<bool>("jmpReleasedToFloor");
			bOMJobDto.ScheduleComplete = dataTable.Rows[0].Field<bool>("jmpScheduleComplete");
			bOMJobDto.OrderQuantity = dataTable.Rows[0].Field<decimal>("jmpOrderQuantity");
			bOMJobDto.PartBinID = dataTable.Rows[0].Field<string>("jmpPartBinID");
			bOMJobDto.PartForecastPeriodID = dataTable.Rows[0].Field<short>("jmpPartForecastPeriodID");
			bOMJobDto.PartForecastYearID = dataTable.Rows[0].Field<short>("jmpPartForecastYearID");
			bOMJobDto.PartID = dataTable.Rows[0].Field<string>("jmpPartID");
			bOMJobDto.PartRevisionID = dataTable.Rows[0].Field<string>("jmpPartRevisionID");
			bOMJobDto.PartWareHouseLocationID = dataTable.Rows[0].Field<string>("jmpPartWareHouseLocationID");
			bOMJobDto.PlantID = dataTable.Rows[0].Field<string>("jmpPlantID");
			bOMJobDto.ProductionDueDate = dataTable.Rows[0].Field<DateTime?>("jmpProductionDueDate");
			bOMJobDto.ProductionQuantity = dataTable.Rows[0].Field<decimal>("jmpProductionQuantity");
			bOMJobDto.ProjectAreaID = dataTable.Rows[0].Field<string>("jmpProjectAreaID");
			bOMJobDto.ProjectID = dataTable.Rows[0].Field<string>("jmpProjectID");
			bOMJobDto.JobPriorityID = dataTable.Rows[0].Field<short>("jmpJobPriorityID");
			bOMJobDto.QuantityReceivedToInventory = dataTable.Rows[0].Field<decimal>("jmpQuantityReceivedToInventory");
			bOMJobDto.QuantityShipped = dataTable.Rows[0].Field<decimal>("jmpQuantityShipped");
			bOMJobDto.ReworkDate = dataTable.Rows[0].Field<DateTime?>("jmpReworkDate");
			bOMJobDto.ReworkQuantity = dataTable.Rows[0].Field<decimal>("jmpReworkQuantity");
			bOMJobDto.ScrapQuantity = dataTable.Rows[0].Field<decimal>("jmpScrapQuantity");
			bOMJobDto.ScrapQuantityCompleted = dataTable.Rows[0].Field<decimal>("jmpScrapQuantityCompleted");
			bOMJobDto.UnitOfMeasure = dataTable.Rows[0].Field<string>("jmpUnitOfMeasure");
		}
		return Task.FromResult(bOMJobDto);
	}

	public async Task<APIValidationInfoDto> SaveJob(CTMJobDto job)
	{
		List<string> errorsList = new List<string>();
		List<string> warningList = new List<string>();
		APIValidationInfoDto apiValidationInfoDto = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		BOMJobAssemblyDto bOMJobAssemblyDto = new BOMJobAssemblyDto
		{
			JobID = job.JobID,
			JobAssemblyID = 0,
			ParentAssemblyID = 0,
			PartID = job.PartID,
			PartRevisionID = job.PartRevisionID,
			QuantityPerParent = 1m,
			OrderQuantity = job.OrderQuantity,
			EstimatedUnitCost = 0m,
			OverlapOperationID = 0,
			OverlapType = 0,
			Level = 1
		};
		try
		{
			InitializeParameterLists();
			base.selectList.AddRange(new string[2] { "imrPartID", "imrShortDescription" });
			base.filterList.Add("imrPartID|C", job.PartID);
			base.filterList.Add("imrPartRevisionID|C", job.PartRevisionID);
			using M1BindingSource jobBindingSource = new M1BindingSource(base.M1database, null);
			jobBindingSource.ClearCache();
			stringBuilder.Append("jmpJobID = " + M1Util.ConvertToLinq(job.JobID) + " ");
			jobBindingSource.DataSourceTable = "Jobs";
			jobBindingSource.NavigateTo(stringBuilder.ToString());
			DataRow dataRow;
			if (jobBindingSource.Count == 0)
			{
				dataRow = jobBindingSource.AddNew() as DataRow;
				dataRow["jmpJobID"] = job.JobID;
			}
			else
			{
				dataRow = jobBindingSource.CurrentAsDataRow;
			}
			if (job.JobAssemblyID != 0 && job.JobAssemblyLevel != 0)
			{
				bOMJobAssemblyDto.JobAssemblyID = job.JobAssemblyID;
				bOMJobAssemblyDto.ParentAssemblyID = job.ParentAssemblyID;
				bOMJobAssemblyDto.Level = job.JobAssemblyLevel;
			}
			if (!string.IsNullOrEmpty(job.PartID))
			{
				dataRow["jmpPartID"] = job.PartID;
			}
			if (!string.IsNullOrEmpty(job.PartRevisionID))
			{
				dataRow["jmpPartRevisionID"] = job.PartRevisionID;
			}
			if (!string.IsNullOrEmpty(job.PartWareHouseLocationID))
			{
				dataRow["jmpPartWareHouseLocationID"] = job.PartWareHouseLocationID;
			}
			if (!string.IsNullOrEmpty(job.PartBinID))
			{
				dataRow["jmpPartBinID"] = job.PartBinID;
			}
			if (job.InventoryQuantity != 0m)
			{
				dataRow["jmpInventoryQuantity"] = job.InventoryQuantity;
			}
			if (job.OrderQuantity != 0m)
			{
				dataRow["jmpOrderQuantity"] = job.OrderQuantity;
			}
			if (job.ScrapQuantity != 0m)
			{
				dataRow["jmpScrapQuantity"] = job.ScrapQuantity;
			}
			if (job.ReworkQuantity != 0m)
			{
				dataRow["jmpReworkQuantity"] = job.ReworkQuantity;
			}
			if (job.ProductionDueDate.HasValue)
			{
				dataRow["jmpProductionDueDate"] = job.ProductionDueDate.Value;
			}
			if (!job.NestlinkProcessed)
			{
				dataRow["jmpNestlinkProcessed"] = job.NestlinkProcessed;
			}
			jobBindingSource.SaveData();
			APIValidationInfoDto aPIValidationInfoDto = await _jobAssemblyRepository.SaveJobAssembly(bOMJobAssemblyDto);
			errorsList.AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
			warningList.AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
			if (errorsList == null || errorsList.Count <= 0)
			{
			}
		}
		catch (Exception ex)
		{
			HttpStatusCode httpValidationStatusCode = HttpStatusCode.InternalServerError;
			errorsList.Add("Error occurred [" + ex.Message + "] while processing the job [" + job.JobID + "]");
			apiValidationInfoDto = new APIValidationInfoDto(errorsList, null, httpValidationStatusCode);
		}
		return await Task.FromResult(apiValidationInfoDto);
	}
}
