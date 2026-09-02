using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Repositories.Core;
using M1.API.Repositories.Core.Job;
using M1.Ax.Erp;

namespace M1.API.Models.BOM;

public class BOMJobAssemblyModel : BOMBaseModel, IBOMJobAssemblyModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetJobId(string jobId)
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

	public Task<APIValidationInfoDto> ValidateRequest_GetJobAssembly(string jobId, int jobAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		using (JobRepository jobRepository = new JobRepository(base.ApiClientContext))
		{
			if (!jobRepository.DoesJobAssemblyExists(jobId, jobAssemblyId).Result)
			{
				base.ErrorsList.Add($"Job [{jobId}] or job assembly [{jobAssemblyId}] is invalid");
			}
		}
		IList<string> errorsList = base.ErrorsList;
		if (errorsList != null && errorsList.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return Task.FromResult(new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PostJobAssembly(BOMJobAssemblyDto jobAssembly)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		APIValidationInfoDto result;
		try
		{
			if (jobAssembly.QuantityPerParent <= 0m)
			{
				base.ErrorsList.Add($"Quantity per parent [{jobAssembly.QuantityPerParent}] is invalid");
			}
			if (jobAssembly.JobAssemblyID == 0)
			{
				if (jobAssembly.Level != 1)
				{
					base.ErrorsList.Add($"Level [{jobAssembly.Level}] is invalid. It should be 1 for the assembly 0");
				}
				if (jobAssembly.ParentAssemblyID != 0)
				{
					base.ErrorsList.Add($"Parent assembly [{jobAssembly.ParentAssemblyID}] is invalid. It should be 0 for the assembly 0");
				}
			}
			using (JobRepository jobRepository = new JobRepository(base.ApiClientContext))
			{
				if (!jobRepository.DoesJobExists(jobAssembly.JobID).Result)
				{
					base.ErrorsList.Add("Job [" + jobAssembly.JobID + "] is invalid");
				}
				if (!string.IsNullOrWhiteSpace(jobAssembly.JobID) && !jobRepository.DoesJobAssemblyExists(jobAssembly.JobID, jobAssembly.ParentAssemblyID).Result)
				{
					base.ErrorsList.Add($"Job [{jobAssembly.JobID}] or parent assembly [{jobAssembly.ParentAssemblyID}] is invalid");
				}
				if (jobAssembly.OverlapOperationID > 0 && !jobRepository.DoesJobOperationExists(jobAssembly.JobID, jobAssembly.ParentAssemblyID, jobAssembly.OverlapOperationID).Result)
				{
					base.ErrorsList.Add($"Job [{jobAssembly.JobID}] or parent assembly [{jobAssembly.ParentAssemblyID}] or overlap operation [{jobAssembly.OverlapOperationID}] is invalid");
				}
			}
			bool num = !string.IsNullOrWhiteSpace(jobAssembly.PartWareHouseLocationID);
			bool flag = !string.IsNullOrWhiteSpace(jobAssembly.PartBinID);
			if (num != flag)
			{
				base.ErrorsList.Add("Invalid entry, warehouse ID " + jobAssembly.PartWareHouseLocationID + " or bin ID " + jobAssembly.PartBinID + " is empty and both should be provided.");
			}
			using (CoreRepository coreRepository = new CoreRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(jobAssembly.PartWareHouseLocationID) && !string.IsNullOrWhiteSpace(jobAssembly.PartBinID) && !(await coreRepository.DoesBinExistAsync(jobAssembly.PartID, jobAssembly.PartRevisionID, jobAssembly.PartWareHouseLocationID, jobAssembly.PartBinID)))
				{
					base.ErrorsList.Add("Warehouse [" + jobAssembly.PartWareHouseLocationID + "] or bin [" + jobAssembly.PartBinID + "] is invalid or inactive.");
				}
			}
			using (PartRepository partRepository = new PartRepository(base.ApiClientContext))
			{
				if ((!string.IsNullOrWhiteSpace(jobAssembly.PartID) || !string.IsNullOrWhiteSpace(jobAssembly.PartRevisionID)) && partRepository.DoesRequirePartsToExistInventory().Result && !partRepository.DoesPartRevisionExists(jobAssembly.PartID ?? string.Empty, jobAssembly.PartRevisionID ?? string.Empty).Result)
				{
					base.ErrorsList.Add("Part [" + jobAssembly.PartID + "] or part revision [" + jobAssembly.PartRevisionID + "] is invalid");
				}
				if ((!string.IsNullOrWhiteSpace(jobAssembly.SourceMethodID) || !string.IsNullOrWhiteSpace(jobAssembly.SourceRevisionID)) && !partRepository.DoesPartRevisionExists(jobAssembly.SourceMethodID ?? string.Empty, jobAssembly.SourceRevisionID ?? string.Empty).Result)
				{
					base.ErrorsList.Add("Source method [" + jobAssembly.SourceMethodID + "] or source revision [" + jobAssembly.SourceRevisionID + "] is invalid");
				}
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the job [" + jobAssembly.JobID + "]");
		}
		finally
		{
			result = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return await Task.FromResult(result);
	}

	public Task<APIValidationInfoDto> ValidateRequest_DeleteJobAssembly(string jobId, int jobAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		using (JobRepository jobRepository = new JobRepository(base.ApiClientContext))
		{
			if (!jobRepository.DoesJobAssemblyExists(jobId, jobAssemblyId).Result)
			{
				base.ErrorsList.Add($"Job [{jobId}] or job assembly [{jobAssemblyId}] is invalid");
			}
			else
			{
				string result = jobRepository.WhereUsed("JobAssemblies", new object[2] { jobId, jobAssemblyId }, new object[2] { "jmaJobID", "jmaJobAssemblyID" }, onlyIncludeForeignRelations: true).Result;
				if (result.Length > 0)
				{
					base.ErrorsList.Add("Job assembly cannot be deleted because it is used in following places.\n [" + result.ToString().Trim() + "]");
				}
			}
			IList<string> errorsList = base.ErrorsList;
			if (errorsList != null && errorsList.Count > 0)
			{
				httpValidationStatusCode = HttpStatusCode.BadRequest;
			}
		}
		return Task.FromResult(new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode));
	}

	public async Task<BOMResponseMessageDto<CTMBOMJobAssemblyDto>> Process_GetJobAssembly(string jobId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		CTMBOMJobAssemblyDto jobAssemblyDto = new CTMBOMJobAssemblyDto();
		BOMResponseMessageDto<CTMBOMJobAssemblyDto> result2;
		try
		{
			using (JobAssemblyRepository jobAssemblyRepository = new JobAssemblyRepository(base.ApiClientContext))
			{
				using JobRepository jobRepository = new JobRepository(base.ApiClientContext);
				BOMJobDto bOMJobDto = await jobRepository.GetJob(jobId);
				IList<BOMJobAssemblyDto> result = jobAssemblyRepository.GetJobAssembliesInfo(jobId).Result;
				jobAssemblyDto.Job = new JobInformationDto
				{
					JobID = bOMJobDto.JobID,
					CustomerOrganizationID = bOMJobDto.CustomerOrganizationID,
					PartID = bOMJobDto.PartID,
					PartRevisionID = bOMJobDto.PartRevisionID,
					PartWareHouseLocationID = bOMJobDto.PartWareHouseLocationID,
					PartBinID = bOMJobDto.PartBinID,
					OrderQuantity = bOMJobDto.OrderQuantity
				};
				foreach (BOMJobAssemblyDto item in result)
				{
					jobAssemblyDto.JobAssemblies.Add(new BOMJobAssemblyDto
					{
						EstimatedUnitCost = item.EstimatedUnitCost,
						Closed = item.Closed,
						JobID = item.JobID,
						Level = item.Level,
						OrderQuantity = item.OrderQuantity,
						OverlapOperationID = item.OverlapOperationID,
						OverlapType = item.OverlapType,
						ParentAssemblyID = item.ParentAssemblyID,
						PartBinID = item.PartBinID,
						PartID = item.PartID,
						PartRevisionID = item.PartRevisionID,
						PartShortDescription = item.PartShortDescription,
						PartWareHouseLocationID = item.PartWareHouseLocationID,
						ProductionQuantity = item.ProductionQuantity,
						QuantityPerParent = item.QuantityPerParent,
						QuantityToMake = item.QuantityToMake,
						QuantityToReturn = item.QuantityToReturn,
						JobAssemblyID = item.JobAssemblyID,
						SourceMethodID = item.SourceMethodID,
						SourceRevisionID = item.SourceRevisionID,
						UnitOfMeasure = item.UnitOfMeasure
					});
				}
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the JobAssemblies []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result2 = new BOMResponseMessageDto<CTMBOMJobAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobAssemblyDto
			};
		}
		return result2;
	}

	public async Task<BOMResponseMessageDto<BOMJobAssemblyDto>> Process_GetJobAssembly(string jobId, int jobAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMJobAssemblyDto jobAssemblyDto = new BOMJobAssemblyDto();
		BOMResponseMessageDto<BOMJobAssemblyDto> result;
		try
		{
			using JobAssemblyRepository jobAssemblyRepository = new JobAssemblyRepository(base.ApiClientContext);
			jobAssemblyDto = await jobAssemblyRepository.GetJobAssemblyInfo(jobId, jobAssemblyId);
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the job [{jobId}] assembly [{jobAssemblyId}] ");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMJobAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobAssemblyDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<IList<BOMJobAssemblyDto>>> Process_GetAllJobAssemblies(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMJobAssemblyDto> allJobAssembliesDto = new List<BOMJobAssemblyDto>();
		BOMResponseMessageDto<IList<BOMJobAssemblyDto>> result;
		try
		{
			using JobAssemblyRepository jobAssemblyRepository = new JobAssemblyRepository(base.ApiClientContext);
			foreach (BOMJobAssemblyDto item2 in await jobAssemblyRepository.GetAllJobAssemblies(pageSize, pageNumber))
			{
				BOMJobAssemblyDto item = new BOMJobAssemblyDto
				{
					EstimatedUnitCost = item2.EstimatedUnitCost,
					Closed = item2.Closed,
					JobID = item2.JobID,
					Level = item2.Level,
					OrderQuantity = item2.OrderQuantity,
					OverlapOperationID = item2.OverlapOperationID,
					OverlapType = item2.OverlapType,
					ParentAssemblyID = item2.ParentAssemblyID,
					PartBinID = item2.PartBinID,
					PartID = item2.PartID,
					PartRevisionID = item2.PartRevisionID,
					PartShortDescription = item2.PartShortDescription,
					PartWareHouseLocationID = item2.PartWareHouseLocationID,
					ProductionQuantity = item2.ProductionQuantity,
					QuantityPerParent = item2.QuantityPerParent,
					QuantityToMake = item2.QuantityToMake,
					QuantityToReturn = item2.QuantityToReturn,
					JobAssemblyID = item2.JobAssemblyID,
					SourceMethodID = item2.SourceMethodID,
					SourceRevisionID = item2.SourceRevisionID,
					UnitOfMeasure = item2.UnitOfMeasure
				};
				allJobAssembliesDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all JobAssemblies]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMJobAssemblyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allJobAssembliesDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMJobAssemblyDto>> Process_PostJobAssembly(BOMJobAssemblyDto jobAssembly)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMResponseMessageDto<BOMJobAssemblyDto> result;
		try
		{
			using (JobAssemblyRepository jobAssemblyRepository = new JobAssemblyRepository(base.ApiClientContext))
			{
				APIValidationInfoDto aPIValidationInfoDto = await jobAssemblyRepository.SaveJobAssembly(jobAssembly);
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the job [" + jobAssembly.JobID + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMJobAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobAssembly
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMJobAssemblyDto>> Process_DeleteJobAssembly(string jobId, int jobAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		BOMResponseMessageDto<BOMJobAssemblyDto> result;
		try
		{
			new M1.Ax.Erp.Job().DeleteJobAssembly(base.ApiClientContext.Database, null, jobId, jobAssemblyId, deleteAsmInJob: true);
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the job [{jobId}] assembly [{jobAssemblyId}] ");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<BOMJobAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new BOMJobAssemblyDto()
			};
		}
		return result;
	}
}
