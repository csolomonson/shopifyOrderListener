using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Repositories.Core;
using M1.API.Repositories.Core.Job;

namespace M1.API.Models.BOM.Job;

public class BOMJobModel : BOMBaseModel, IBOMJobModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public IDictionary<string, object> JobKeyDictionary { get; set; }

	public BOMJobModel()
	{
		JobKeyDictionary = new Dictionary<string, object>();
	}

	public Task<APIValidationInfoDto> ValidateRequest_GetJobGUIDs(string jobId, string partId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		APIValidationInfoDto aPIValidationInfoDto = null;
		try
		{
			if (string.IsNullOrWhiteSpace(jobId) && string.IsNullOrWhiteSpace(partId))
			{
				base.ErrorsList.Add("Both job id and part id cannot be empty");
			}
			IList<string> errorsList = base.ErrorsList;
			if (errorsList != null && errorsList.Count > 0)
			{
				httpValidationStatusCode = HttpStatusCode.BadRequest;
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the parameters]");
		}
		finally
		{
			aPIValidationInfoDto = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(aPIValidationInfoDto);
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetJob(string jobId)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		using (JobRepository jobRepository = new JobRepository(base.ApiClientContext))
		{
			if (!jobRepository.DoesJobExists(jobId).Result)
			{
				list.Add("Job [jobId] is invalid");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PostJob(CTMJobDto job)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		new APIValidationInfoDto(errorsList, warningsList, httpStatus);
		APIValidationInfoDto result;
		try
		{
			using (JobRepository jobRepository = new JobRepository(base.ApiClientContext))
			{
				if (jobRepository.DoesJobExists(job.JobID).Result)
				{
					base.ErrorsList.Add("Job [" + job.JobID + "] already exists.");
				}
			}
			using (PartRepository partRepository = new PartRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(job.PartID) || !string.IsNullOrWhiteSpace(job.PartRevisionID))
				{
					if (await partRepository.DoesRequirePartsToExistInventory() && !partRepository.DoesPartRevisionExists(job.PartID ?? string.Empty, job.PartRevisionID ?? string.Empty).Result)
					{
						base.ErrorsList.Add("Part [" + job.PartID + "] or part revision [" + job.PartRevisionID + "] is invalid.");
					}
					PartRevisionInformationDto partRevisionInformationDto = await partRepository.GetPartRevisionInfo(job.PartID, job.PartRevisionID);
					if (!partRevisionInformationDto.EffectiveEndDate.HasValue || partRevisionInformationDto.EffectiveEndDate > DateTime.Today)
					{
						base.ErrorsList.Add("The effective end date for part [" + job.PartID + "] Rev: [" + job.PartRevisionID + "] is either null or set to a date beyond today.");
					}
				}
				if ((!string.IsNullOrWhiteSpace(job.PartID) || !string.IsNullOrWhiteSpace(job.PartRevisionID) || !string.IsNullOrWhiteSpace(job.PartWareHouseLocationID)) && !(await partRepository.DoesPartWarehouseLocationExists(job.PartID, job.PartRevisionID, job.PartWareHouseLocationID)))
				{
					base.ErrorsList.Add("Part Warehouse Location [" + job.PartWareHouseLocationID + "] is invalid.");
				}
				if ((!string.IsNullOrWhiteSpace(job.PartID) || !string.IsNullOrWhiteSpace(job.PartRevisionID) || !string.IsNullOrWhiteSpace(job.PartWareHouseLocationID) || !string.IsNullOrEmpty(job.PartBinID)) && !(await partRepository.DoesPartBinExists(job.PartID, job.PartRevisionID, job.PartWareHouseLocationID, job.PartBinID)))
				{
					base.ErrorsList.Add("Part Bin [" + job.PartBinID + "] is invalid.");
				}
			}
			using (OrganizationRepository organizationRepository = new OrganizationRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(job.CustomerOrganizationID) && !(await organizationRepository.DoesOrganizationExists(job.CustomerOrganizationID)))
				{
					base.ErrorsList.Add("Customer Organization [" + job.CustomerOrganizationID + "] is invalid.");
				}
			}
			IList<string> errorsList2 = base.ErrorsList;
			if (errorsList2 != null && errorsList2.Count > 0)
			{
				httpStatus = HttpStatusCode.BadRequest;
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the job [" + job.JobID + "]");
		}
		finally
		{
			result = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return await Task.FromResult(result);
	}

	public async Task<BOMResponseMessageDto<BOMJobGuidsDto>> Process_GetJobGUIDs(string jobId, string partId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMJobGuidsDto jobGuidsDto = new BOMJobGuidsDto();
		BOMResponseMessageDto<BOMJobGuidsDto> result;
		try
		{
			IJobRepository jobRepository = (base.JobRepository = new JobRepository(base.ApiClientContext));
			using (jobRepository)
			{
				jobGuidsDto = await base.JobRepository.GetJobGuidsInfo(jobId, partId);
			}
			if (jobGuidsDto.JobGuids.Count == 0)
			{
				base.ErrorsList.Add("No records to display");
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the request");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMJobGuidsDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobGuidsDto
			};
		}
		return result;
	}

	public Task<APIValidationInfoDto> ValidateRequest_GetJobMethod(string jobIdString)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		APIValidationInfoDto aPIValidationInfoDto = null;
		try
		{
			string result = GetM1JobIdFromGuid(jobIdString).Result;
			if (string.IsNullOrWhiteSpace(result))
			{
				base.ErrorsList.Add("Invalid job Guid/Id");
			}
			if (base.ErrorsList.Count > 0)
			{
				httpValidationStatusCode = HttpStatusCode.BadRequest;
			}
			else
			{
				JobKeyDictionary.Add("jmpJobID", result);
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the parameters]");
			throw;
		}
		finally
		{
			aPIValidationInfoDto = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(aPIValidationInfoDto);
	}

	public async Task<BOMResponseMessageDto<CTMBOMJobMethodDto>> Process_GetJobMethod(string jobId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatusCode = HttpStatusCode.OK;
		CTMBOMJobMethodDto jobMethodDto = new CTMBOMJobMethodDto();
		BOMResponseMessageDto<CTMBOMJobMethodDto> result;
		try
		{
			IJobRepository jobRepository = (base.JobRepository = new JobRepository(base.ApiClientContext));
			using (jobRepository)
			{
				using DataTable dataTable = await base.JobRepository.GetJobMethodAsDataTable(jobId);
				jobMethodDto.JobHeader = GetJobHeaderInfo(dataTable?.AsEnumerable().FirstOrDefault());
				jobMethodDto.JobMethodAssemblies = new List<CTMBOMJobMethodAssemblyDto>(GetJobAssembliesInfo(dataTable));
			}
		}
		catch (Exception ex)
		{
			httpStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the request");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatusCode);
			result = new BOMResponseMessageDto<CTMBOMJobMethodDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobMethodDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<IList<BOMJobDto>>> Process_GetAllJobs(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMJobDto> allJobsDto = new List<BOMJobDto>();
		BOMResponseMessageDto<IList<BOMJobDto>> result;
		try
		{
			using JobRepository jobRepository = new JobRepository(base.ApiClientContext);
			foreach (BOMJobDto item2 in await jobRepository.GetAllJobs(pageSize, pageNumber))
			{
				BOMJobDto item = new BOMJobDto
				{
					JobID = item2.JobID,
					CustomerOrganizationID = item2.CustomerOrganizationID,
					InventoryQuantity = item2.InventoryQuantity,
					Closed = item2.Closed,
					Firm = item2.Firm,
					PlanningComplete = item2.PlanningComplete,
					ProductionComplete = item2.ProductionComplete,
					ReleasedToFloor = item2.ReleasedToFloor,
					ScheduleComplete = item2.ScheduleComplete,
					OrderQuantity = item2.OrderQuantity,
					PartBinID = item2.PartBinID,
					PartForecastPeriodID = item2.PartForecastPeriodID,
					PartForecastYearID = item2.PartForecastYearID,
					PartID = item2.PartID,
					PartRevisionID = item2.PartRevisionID,
					PartWareHouseLocationID = item2.PartWareHouseLocationID,
					PlantID = item2.PlantID,
					ProductionDueDate = item2.ProductionDueDate,
					ProductionQuantity = item2.ProductionQuantity,
					ProjectAreaID = item2.ProjectAreaID,
					ProjectID = item2.ProjectID,
					JobPriorityID = item2.JobPriorityID,
					QuantityReceivedToInventory = item2.QuantityReceivedToInventory,
					QuantityShipped = item2.QuantityShipped,
					ReworkDate = item2.ReworkDate,
					ReworkQuantity = item2.ReworkQuantity,
					ScrapQuantity = item2.ScrapQuantity,
					ScrapQuantityCompleted = item2.ScrapQuantityCompleted,
					UnitOfMeasure = item2.UnitOfMeasure,
					NestlinkProcessed = item2.NestlinkProcessed
				};
				allJobsDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Jobs]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMJobDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allJobsDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMJobDto>> Process_GetJob(string jobId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMJobDto jobDto = null;
		BOMResponseMessageDto<BOMJobDto> result;
		try
		{
			using JobRepository jobRepository = new JobRepository(base.ApiClientContext);
			BOMJobDto bOMJobDto = await jobRepository.GetJob(jobId);
			jobDto = new BOMJobDto
			{
				JobID = bOMJobDto.JobID,
				CustomerOrganizationID = bOMJobDto.CustomerOrganizationID,
				InventoryQuantity = bOMJobDto.InventoryQuantity,
				Closed = bOMJobDto.Closed,
				Firm = bOMJobDto.Firm,
				PlanningComplete = bOMJobDto.PlanningComplete,
				ProductionComplete = bOMJobDto.ProductionComplete,
				ReleasedToFloor = bOMJobDto.ReleasedToFloor,
				ScheduleComplete = bOMJobDto.ScheduleComplete,
				OrderQuantity = bOMJobDto.OrderQuantity,
				PartBinID = bOMJobDto.PartBinID,
				PartForecastPeriodID = bOMJobDto.PartForecastPeriodID,
				PartForecastYearID = bOMJobDto.PartForecastYearID,
				PartID = bOMJobDto.PartID,
				PartRevisionID = bOMJobDto.PartRevisionID,
				PartWareHouseLocationID = bOMJobDto.PartWareHouseLocationID,
				PlantID = bOMJobDto.PlantID,
				ProductionDueDate = bOMJobDto.ProductionDueDate,
				ProductionQuantity = bOMJobDto.ProductionQuantity,
				ProjectAreaID = bOMJobDto.ProjectAreaID,
				ProjectID = bOMJobDto.ProjectID,
				JobPriorityID = bOMJobDto.JobPriorityID,
				QuantityReceivedToInventory = bOMJobDto.QuantityReceivedToInventory,
				QuantityShipped = bOMJobDto.QuantityShipped,
				ReworkDate = bOMJobDto.ReworkDate,
				ReworkQuantity = bOMJobDto.ReworkQuantity,
				ScrapQuantity = bOMJobDto.ScrapQuantity,
				ScrapQuantityCompleted = bOMJobDto.ScrapQuantityCompleted,
				UnitOfMeasure = bOMJobDto.UnitOfMeasure,
				NestlinkProcessed = bOMJobDto.NestlinkProcessed
			};
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Jobs []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMJobDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<CTMJobDto>> Process_PostJob(CTMJobDto job)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		BOMResponseMessageDto<CTMJobDto> result;
		try
		{
			using JobRepository jobRepository = new JobRepository(base.ApiClientContext);
			APIValidationInfoDto aPIValidationInfoDto = await jobRepository.SaveJob(job);
			((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
			((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the job [" + job.JobID + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<CTMJobDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = job
			};
		}
		return result;
	}

	private async Task<string> GetM1JobIdFromGuid(string jobIdString)
	{
		IJobRepository jobRepository = (base.JobRepository = new JobRepository(base.ApiClientContext));
		using (jobRepository)
		{
			if (Guid.TryParse(jobIdString, out var _))
			{
				return await base.JobRepository.GetJobIdFromGuid(jobIdString);
			}
			if (base.JobRepository.DoesJobExists(jobIdString).Result)
			{
				return jobIdString;
			}
		}
		return string.Empty;
	}

	private BOMJobAssemblyDto CreateMethodAsmblyDto(DataTable dataTable, int asmId)
	{
		return (from row in dataTable.AsEnumerable()
			where row.Field<int?>("jmaJobAssemblyID") == asmId
			select new BOMJobAssemblyDto
			{
				JobID = row["jmaJobID"].ToString().Trim(),
				JobAssemblyID = Convert.ToInt32(row["jmaJobAssemblyID"]),
				Level = Convert.ToInt16(row["jmaLevel"]),
				ParentAssemblyID = Convert.ToInt32(row["jmaParentAssemblyID"]),
				SourceMethodID = row["jmaSourceMethodID"].ToString().Trim(),
				SourceRevisionID = row["jmaSourceRevisionID"].ToString().Trim(),
				PartID = row["jmaPartID"].ToString().Trim(),
				PartRevisionID = row["jmaPartRevisionID"].ToString().Trim(),
				PartShortDescription = row["jmaPartShortDescription"].ToString().Trim(),
				UnitOfMeasure = row["jmaUnitOfMeasure"].ToString().Trim(),
				QuantityPerParent = Convert.ToDecimal(row["jmaQuantityPerParent"]),
				OrderQuantity = Convert.ToDecimal(row["jmaOrderQuantity"]),
				ProductionQuantity = Convert.ToDecimal(row["jmaProductionQuantity"]),
				QuantityToMake = Convert.ToDecimal(row["jmaQuantityToMake"]),
				EstimatedUnitCost = Convert.ToDecimal(row["jmaEstimatedUnitCost"]),
				OverlapOperationID = Convert.ToInt32(row["jmaOverlapOperationID"]),
				OverlapType = Convert.ToByte(row["jmaOverlapType"]),
				DueDate = ((row["jmaScheduledDueDate"] == DBNull.Value) ? ((DateTime?)null) : new DateTime?(Convert.ToDateTime(row["jmaScheduledDueDate"])))
			}).FirstOrDefault();
	}

	private IList<BOMJobMaterialDto> CreateMethodAsmMaterialDtos(DataTable dataTable, int asmId, IEnumerable<int?> distinctAsmMatlIds)
	{
		List<BOMJobMaterialDto> list = new List<BOMJobMaterialDto>();
		foreach (int? asmMatlId in distinctAsmMatlIds)
		{
			BOMJobMaterialDto item = (from row in dataTable.AsEnumerable()
				where row.Field<int?>("jmmJobAssemblyID") == asmId && row.Field<int?>("jmmJobMaterialID") == asmMatlId
				select new BOMJobMaterialDto
				{
					JobID = row["jmmJobID"].ToString().Trim(),
					JobAssemblyID = Convert.ToInt32(row["jmmJobAssemblyID"]),
					JobMaterialID = Convert.ToInt32(row["jmmJobMaterialID"]),
					PartID = row["jmmPartID"].ToString().Trim(),
					PartRevisionID = row["jmmPartRevisionID"].ToString().Trim(),
					UnitOfMeasure = row["jmmUnitOfMeasure"].ToString().Trim(),
					PartShortDescription = row["jmmPartShortDescription"].ToString().Trim(),
					RelatedJobOperationID = Convert.ToInt32(row["jmmRelatedJobOperationID"]),
					QuantityPerAssembly = Convert.ToDecimal(row["jmmQuantityPerAssembly"]),
					EstimatedQuantity = Convert.ToDecimal(row["jmmEstimatedQuantity"]),
					EstimatedUnitCost = Convert.ToDecimal(row["jmmEstimatedUnitCost"]),
					CalculatedUnitCost = Convert.ToDecimal(row["jmmCalculatedUnitCost"]),
					Firm = Convert.ToBoolean(row["jmmFirm"]),
					SupplierOrganizationID = row["jmmSupplierOrganizationID"].ToString().Trim(),
					PurchaseLocationID = row["jmmPurchaseLocationID"].ToString().Trim(),
					PurchaseOrderID = row["jmmPurchaseOrderID"].ToString().Trim(),
					LeadTime = Convert.ToInt16(row["jmmLeadTime"]),
					MinimumCharge = Convert.ToDecimal(row["jmmMinimumCharge"]),
					DueInDate = ((row["jmmDueInDate"] == DBNull.Value) ? ((DateTime?)null) : new DateTime?(Convert.ToDateTime(row["jmmDueInDate"]))),
					RequiredDate = ((row["jmmRequiredDate"] == DBNull.Value) ? ((DateTime?)null) : new DateTime?(Convert.ToDateTime(row["jmmRequiredDate"]))),
					QuantityAllocated = Convert.ToDecimal(row["jmmQuantityAllocated"]),
					QuantityReceived = Convert.ToDecimal(row["jmmQuantityReceived"]),
					ScrapQuantityReceived = Convert.ToDecimal(row["jmmScrapQuantityReceived"]),
					QuantityToInspect = Convert.ToDecimal(row["jmmQuantityToInspect"]),
					QuantityToReturn = Convert.ToDecimal(row["jmmQuantityToReturn"]),
					ReceivedComplete = Convert.ToBoolean(row["jmmReceivedComplete"]),
					PurchaseToJobQuantity = Convert.ToDecimal(row["jmmPurchaseToJobQuantity"]),
					PullAllFromStock = Convert.ToBoolean(row["jmmPullAllFromStock"]),
					PullFromStockQuantity = Convert.ToDecimal(row["jmmPullFromStockQuantity"]),
					Closed = Convert.ToBoolean(row["jmmClosed"]),
					UniqueID = row.Field<Guid>("jmmUniqueID"),
					RowVersion = row.Field<byte[]>("jmmRowVersion")
				}).FirstOrDefault();
			list.Add(item);
		}
		return list;
	}

	private IList<BOMJobOperationDto> CreateMethodAsmOperationDtos(DataTable dataTable, int asmId, IEnumerable<int?> distinctAsmOperIds)
	{
		List<BOMJobOperationDto> list = new List<BOMJobOperationDto>();
		foreach (int? asmOprId in distinctAsmOperIds)
		{
			BOMJobOperationDto item = (from row in dataTable.AsEnumerable()
				where row.Field<int?>("jmoJobAssemblyID") == asmId && row.Field<int?>("jmoJobOperationID") == asmOprId
				select new BOMJobOperationDto
				{
					JobID = row["jmoJobID"].ToString().Trim(),
					JobAssemblyID = Convert.ToInt32(row["jmoJobAssemblyID"]),
					OperationType = Convert.ToByte(row["jmoOperationType"]),
					JobOperationID = Convert.ToInt32(row["jmoJobOperationID"]),
					WorkCenterID = row["jmoWorkCenterID"].ToString().Trim(),
					ProcessID = row["jmoProcessID"].ToString().Trim(),
					PartID = row["jmoPartID"].ToString().Trim(),
					PartRevisionID = row["jmoPartRevisionID"].ToString().Trim(),
					ProcessShortDescription = row["jmoProcessShortDescription"].ToString().Trim(),
					UnitOfMeasure = row["jmoUnitOfMeasure"].ToString().Trim(),
					ProductionStandard = Convert.ToDecimal(row["jmoProductionStandard"]),
					StandardFactor = row["jmoStandardFactor"].ToString().Trim(),
					MachinesToSchedule = Convert.ToInt16(row["jmoMachinesToSchedule"]),
					QuantityPerAssembly = Convert.ToDecimal(row["jmoQuantityPerAssembly"]),
					SetupRate = Convert.ToDecimal(row["jmoSetupRate"]),
					ProductionRate = Convert.ToDecimal(row["jmoProductionRate"]),
					OverheadRate = Convert.ToDecimal(row["jmoOverheadRate"]),
					MachineType = Convert.ToByte(row["jmoMachineType"]),
					DueDate = ((row["jmoDueDate"] == DBNull.Value) ? ((DateTime?)null) : new DateTime?(Convert.ToDateTime(row["jmoDueDate"])))
				}).FirstOrDefault();
			list.Add(item);
		}
		return list;
	}

	private BOMJobDto GetJobHeaderInfo(DataRow dataRow)
	{
		if (dataRow != null)
		{
			return new BOMJobDto
			{
				JobID = dataRow.Field<string>("jmpJobID"),
				PlantID = dataRow.Field<string>("jmpPlantID"),
				ProductionDueDate = dataRow.Field<DateTime?>("jmpProductionDueDate"),
				CustomerOrganizationID = dataRow.Field<string>("jmpCustomerOrganizationID"),
				PartID = dataRow.Field<string>("jmpPartID"),
				PartRevisionID = dataRow.Field<string>("jmpPartRevisionID"),
				PartWareHouseLocationID = dataRow.Field<string>("jmpPartWareHouseLocationID"),
				PartBinID = dataRow.Field<string>("jmpPartBinID"),
				UnitOfMeasure = dataRow.Field<string>("jmpUnitOfMeasure"),
				OrderQuantity = dataRow.Field<decimal>("jmpOrderQuantity"),
				InventoryQuantity = dataRow.Field<decimal>("jmpInventoryQuantity"),
				ScrapQuantity = dataRow.Field<decimal>("jmpScrapQuantity"),
				ReworkQuantity = dataRow.Field<decimal>("jmpReworkQuantity"),
				ProductionQuantity = dataRow.Field<decimal>("jmpProductionQuantity"),
				PlanningComplete = dataRow.Field<bool>("jmpPlanningComplete"),
				ScheduleComplete = dataRow.Field<bool>("jmpScheduleComplete"),
				ReleasedToFloor = dataRow.Field<bool>("jmpReleasedToFloor"),
				ProductionComplete = dataRow.Field<bool>("jmpProductionComplete"),
				ScrapQuantityCompleted = dataRow.Field<decimal>("jmpScrapQuantityCompleted"),
				ReworkDate = dataRow.Field<DateTime?>("jmpReworkDate"),
				Closed = dataRow.Field<bool>("jmpClosed"),
				Firm = dataRow.Field<bool>("jmpFirm"),
				DueDate = ((!dataRow.Field<DateTime?>("jmpScheduledDueDate").HasValue) ? ((DateTime?)null) : dataRow.Field<DateTime?>("jmpScheduledDueDate"))
			};
		}
		return new BOMJobDto();
	}

	private List<CTMBOMJobMethodAssemblyDto> GetJobAssembliesInfo(DataTable dataTable)
	{
		CTMBOMJobMethodAssemblyDto cTMBOMJobMethodAssemblyDto = new CTMBOMJobMethodAssemblyDto();
		List<CTMBOMJobMethodAssemblyDto> list = new List<CTMBOMJobMethodAssemblyDto>();
		IEnumerable<int> enumerable = (from x in dataTable.AsEnumerable()
			select x.Field<int>("jmaJobAssemblyID")).Distinct();
		if (enumerable.Count() > 0)
		{
			foreach (int asmId in enumerable)
			{
				cTMBOMJobMethodAssemblyDto = new CTMBOMJobMethodAssemblyDto();
				cTMBOMJobMethodAssemblyDto.JobAssembly = CreateMethodAsmblyDto(dataTable, asmId);
				IEnumerable<int?> enumerable2 = (from x in dataTable.AsEnumerable()
					where x.Field<int?>("jmoJobAssemblyID") == asmId
					select x.Field<int?>("jmoJobOperationID")).Distinct();
				if (enumerable2 != null && enumerable2.Count() > 0)
				{
					cTMBOMJobMethodAssemblyDto.JobOperations = new List<BOMJobOperationDto>(CreateMethodAsmOperationDtos(dataTable, asmId, enumerable2));
				}
				IEnumerable<int?> enumerable3 = (from x in dataTable.AsEnumerable()
					where x.Field<int?>("jmmJobAssemblyID") == asmId
					select x.Field<int?>("jmmJobMaterialID")).Distinct();
				if (enumerable3 != null && enumerable3.Count() > 0)
				{
					cTMBOMJobMethodAssemblyDto.JobMaterials = new List<BOMJobMaterialDto>(CreateMethodAsmMaterialDtos(dataTable, asmId, enumerable3));
				}
				list.Add(cTMBOMJobMethodAssemblyDto);
			}
		}
		return list;
	}
}
