using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core;
using M1.API.Repositories.Core.Job;
using M1.API.Utilities;

namespace M1.API.Models.BOM.Job;

public class BOMJobOperationModel : BOMBaseModel, IBOMJobOperationModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_PostJobOperation(BOMJobOperationDto jobOperation)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		APIValidationInfoDto result;
		try
		{
			using (JobRepository jobRepository = new JobRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(jobOperation.JobID) && !jobRepository.DoesJobAssemblyExists(jobOperation.JobID, jobOperation.JobAssemblyID).Result)
				{
					base.ErrorsList.Add($"Job [{jobOperation.JobID}] or assembly [{jobOperation.JobAssemblyID}] is invalid");
				}
				if (!Enum.IsDefined(typeof(APIEnums.OperationType), jobOperation.OperationType))
				{
					base.ErrorsList.Add($"Operation type [{jobOperation.OperationType}] is invalid");
				}
			}
			using (PartRepository partRepository = new PartRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(jobOperation.WorkCenterID) && !partRepository.DoesWorkCenterExists(jobOperation.WorkCenterID).Result)
				{
					base.ErrorsList.Add("WorkCenter Id [" + jobOperation.WorkCenterID + "] is invalid");
				}
				if (!string.IsNullOrWhiteSpace(jobOperation.ProcessID) && !partRepository.DoesProcessExists(jobOperation.ProcessID).Result)
				{
					base.ErrorsList.Add("Process Id [" + jobOperation.ProcessID + "] is invalid");
				}
				if (!Enum.IsDefined(typeof(APIEnums.MachineType), jobOperation.MachineType))
				{
					base.ErrorsList.Add($"Machine type [{jobOperation.MachineType}] is invalid");
				}
				if (jobOperation.QuantityPerAssembly <= 0m)
				{
					base.ErrorsList.Add($"Quantity per assembly [{jobOperation.QuantityPerAssembly}] is invalid");
				}
				if (!string.IsNullOrWhiteSpace(jobOperation.PartID) || !string.IsNullOrWhiteSpace(jobOperation.PartRevisionID))
				{
					if (partRepository.DoesRequirePartsToExistInventory().Result && !partRepository.DoesPartRevisionExists(jobOperation.PartID ?? string.Empty, jobOperation.PartRevisionID ?? string.Empty).Result)
					{
						base.ErrorsList.Add("Part [" + jobOperation.PartID + "] or part revision [" + jobOperation.PartRevisionID + "] is invalid");
					}
				}
				else if (jobOperation.OperationType == 2)
				{
					base.ErrorsList.Add("Part Id is required for outside operations");
				}
			}
			bool num = !string.IsNullOrWhiteSpace(jobOperation.PartWarehouseLocationID);
			bool flag = !string.IsNullOrWhiteSpace(jobOperation.PartBinID);
			if (num != flag)
			{
				base.ErrorsList.Add("Invalid entry, warehouse ID " + jobOperation.PartWarehouseLocationID + " or bin ID " + jobOperation.PartBinID + " is empty and both should be provided.");
			}
			using (CoreRepository coreRepository = new CoreRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(jobOperation.PartWarehouseLocationID) && !string.IsNullOrWhiteSpace(jobOperation.PartBinID) && !(await coreRepository.DoesBinExistAsync(jobOperation.PartID, jobOperation.PartRevisionID, jobOperation.PartWarehouseLocationID, jobOperation.PartBinID)))
				{
					base.ErrorsList.Add("PartID " + jobOperation.PartID + " PartRevision " + jobOperation.PartRevisionID + " Warehouse [" + jobOperation.PartWarehouseLocationID + "] and Bin [" + jobOperation.PartBinID + "] don't exist in the system.");
				}
			}
			if (!WebAPIConstants.M1_STANDARD_FACTORS_ARRAY.Contains(jobOperation.StandardFactor.ToUpper()))
			{
				base.ErrorsList.Add("Standard factor [" + jobOperation.StandardFactor + "] is invalid and should only be one of the following: [" + string.Join(",", WebAPIConstants.M1_STANDARD_FACTORS_ARRAY) + "]");
			}
			IList<string> errorsList = base.ErrorsList;
			if (errorsList != null && errorsList.Count > 0)
			{
				httpStatus = HttpStatusCode.BadRequest;
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the job [" + jobOperation.JobID + "]");
		}
		finally
		{
			result = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return await Task.FromResult(result);
	}

	public Task<APIValidationInfoDto> ValidateRequest_GetJobOperation(string jobId, int jobAssemblyId, int jobOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		APIValidationInfoDto result;
		try
		{
			using (JobRepository jobRepository = new JobRepository(base.ApiClientContext))
			{
				if (!jobRepository.DoesJobOperationExists(jobId, jobAssemblyId, jobOperationId).Result)
				{
					base.ErrorsList.Add($"Job [{jobId}] or job assembly [{jobAssemblyId}] or job operation [{jobOperationId}] is invalid");
				}
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the job [" + jobId + "]");
		}
		finally
		{
			result = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(result);
	}

	public Task<APIValidationInfoDto> ValidateRequest_DeleteJobOperation(string jobId, int jobAssemblyId, int jobOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		APIValidationInfoDto result2;
		try
		{
			using (JobRepository jobRepository = new JobRepository(base.ApiClientContext))
			{
				if (!jobRepository.DoesJobOperationExists(jobId, jobAssemblyId, jobOperationId).Result)
				{
					base.ErrorsList.Add($"Job [{jobId}] or job assembly [{jobAssemblyId}] or job operation [{jobOperationId}] is invalid");
				}
				else
				{
					string result = jobRepository.WhereUsed("JobOperations", new object[3] { jobId, jobAssemblyId, jobOperationId }, new object[3] { "jmoJobID", "jmoJobAssemblyID", "jmoJobOperationID" }, onlyIncludeForeignRelations: true).Result;
					if (result.Length > 0)
					{
						base.ErrorsList.Add("Job operation cannot be deleted because it is used in following places.\n [" + result.ToString().Trim() + "]");
					}
				}
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the job [" + jobId + "]");
		}
		finally
		{
			result2 = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return Task.FromResult(result2);
	}

	public async Task<BOMResponseMessageDto<IList<BOMJobOperationDto>>> Process_GetAllJobOperations(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMJobOperationDto> allJobOperationsDto = new List<BOMJobOperationDto>();
		BOMResponseMessageDto<IList<BOMJobOperationDto>> result;
		try
		{
			using JobOperationRepository jobOperationRepository = new JobOperationRepository(base.ApiClientContext);
			foreach (BOMJobOperationDto item2 in await jobOperationRepository.GetAllJobOperations(pageSize, pageNumber))
			{
				BOMJobOperationDto item = new BOMJobOperationDto
				{
					JobID = item2.JobID,
					JobAssemblyID = item2.JobAssemblyID,
					JobOperationID = item2.JobOperationID,
					OperationType = item2.OperationType,
					WorkCenterID = item2.WorkCenterID,
					ProcessID = item2.ProcessID,
					ProcessShortDescription = item2.ProcessShortDescription,
					ProductionStandard = item2.ProductionStandard,
					StandardFactor = item2.StandardFactor,
					MachinesToSchedule = item2.MachinesToSchedule,
					MachineType = item2.MachineType,
					QuantityPerAssembly = item2.QuantityPerAssembly,
					OperationQuantity = item2.OperationQuantity,
					QuantityComplete = item2.QuantityComplete,
					SetupRate = item2.SetupRate,
					ProductionRate = item2.ProductionRate,
					OverheadRate = item2.OverheadRate,
					PartID = item2.PartID,
					PartRevisionID = item2.PartRevisionID,
					PartWarehouseLocationID = item2.PartWarehouseLocationID,
					PartBinID = item2.PartBinID,
					PlantID = item2.PlantID,
					UnitOfMeasure = item2.UnitOfMeasure,
					Closed = item2.Closed,
					DueDate = item2.DueDate
				};
				allJobOperationsDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all JobOperations]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMJobOperationDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allJobOperationsDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMJobOperationDto>> Process_GetJobOperation(string jobId, int jobAssemblyId, int jobOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMJobOperationDto jobOperationDto = new BOMJobOperationDto();
		BOMResponseMessageDto<BOMJobOperationDto> result;
		try
		{
			using JobOperationRepository jobOperationRepository = new JobOperationRepository(base.ApiClientContext);
			jobOperationDto = await jobOperationRepository.GetJobOperationInfo(jobId, jobAssemblyId, jobOperationId);
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the job [{jobId}] assembly [{jobAssemblyId}] operation [{jobOperationId}] ");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMJobOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobOperationDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMJobOperationDto>> Process_PostJobOperation(BOMJobOperationDto jobOperation)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMResponseMessageDto<BOMJobOperationDto> result;
		try
		{
			using (JobOperationRepository jobOperationRepository = new JobOperationRepository(base.ApiClientContext))
			{
				APIValidationInfoDto aPIValidationInfoDto = await jobOperationRepository.SaveJobOperationAsync(jobOperation);
				((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
			}
			IList<string> errorsList = base.ErrorsList;
			if (errorsList != null && errorsList.Count > 0)
			{
				httpStatus = HttpStatusCode.BadRequest;
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the job [" + jobOperation.JobID + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMJobOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobOperation
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMJobOperationDto>> Process_DeleteJobOperation(string jobId, int jobAssemblyId, int jobOperationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMResponseMessageDto<BOMJobOperationDto> result;
		try
		{
			using (JobOperationRepository jobOperationRepository = new JobOperationRepository(base.ApiClientContext))
			{
				APIValidationInfoDto aPIValidationInfoDto = await jobOperationRepository.DeleteJobOperation(jobId, jobAssemblyId, jobOperationId);
				((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
			}
			IList<string> errorsList = base.ErrorsList;
			if (errorsList != null && errorsList.Count > 0)
			{
				httpStatus = HttpStatusCode.BadRequest;
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the job [{jobId}] assembly [{jobAssemblyId}] operation [{jobOperationId}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMJobOperationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new BOMJobOperationDto()
			};
		}
		return result;
	}
}
