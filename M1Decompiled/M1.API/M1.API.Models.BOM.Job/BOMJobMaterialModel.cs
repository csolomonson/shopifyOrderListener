using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core;
using M1.API.Repositories.Core.Job;

namespace M1.API.Models.BOM.Job;

public class BOMJobMaterialModel : BOMBaseModel, IBOMJobMaterialModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public Task<APIValidationInfoDto> ValidateRequest_GetJobMaterial(string jobId, int jobAssemblyId, int jobMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		APIValidationInfoDto result;
		try
		{
			using (JobMaterialRepository jobMaterialRepository = new JobMaterialRepository(base.ApiClientContext))
			{
				if (!jobMaterialRepository.DoesJobMaterialExists(jobId, jobAssemblyId, jobMaterialId).Result)
				{
					base.ErrorsList.Add($"Job [{jobId}] or job assembly [{jobAssemblyId}] or job operation [{jobMaterialId}] is invalid");
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

	public async Task<APIValidationInfoDto> ValidateRequest_PostJobMaterial(BOMJobMaterialDto jobMaterial)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		APIValidationInfoDto result;
		try
		{
			using (JobRepository jobRepository = new JobRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(jobMaterial.JobID) && !jobRepository.DoesJobAssemblyExists(jobMaterial.JobID, jobMaterial.JobAssemblyID).Result)
				{
					base.ErrorsList.Add($"Job [{jobMaterial.JobID}] or assembly [{jobMaterial.JobAssemblyID}] is invalid");
				}
				if (jobMaterial.RelatedJobOperationID != 0 && !jobRepository.DoesJobOperationExists(jobMaterial.JobID, jobMaterial.JobAssemblyID, jobMaterial.RelatedJobOperationID).Result)
				{
					base.ErrorsList.Add($"Job [{jobMaterial.JobID}] or assembly [{jobMaterial.JobAssemblyID}] or related operation [{jobMaterial.RelatedJobOperationID}] is invalid");
				}
			}
			using (PartRepository partRepository = new PartRepository(base.ApiClientContext))
			{
				if ((!string.IsNullOrWhiteSpace(jobMaterial.PartID) || !string.IsNullOrWhiteSpace(jobMaterial.PartRevisionID)) && partRepository.DoesRequirePartsToExistInventory().Result && !partRepository.DoesPartRevisionExists(jobMaterial.PartID ?? string.Empty, jobMaterial.PartRevisionID ?? string.Empty).Result)
				{
					base.ErrorsList.Add("Part [" + jobMaterial.PartID + "] or part revision [" + jobMaterial.PartRevisionID + "] is invalid");
				}
			}
			bool num = !string.IsNullOrWhiteSpace(jobMaterial.PartWarehouseLocationID);
			bool flag = !string.IsNullOrWhiteSpace(jobMaterial.PartBinID);
			if (num != flag)
			{
				base.ErrorsList.Add("Invalid entry, warehouse ID " + jobMaterial.PartWarehouseLocationID + " or bin ID " + jobMaterial.PartBinID + " is empty and both should be provided.");
			}
			using (CoreRepository coreRepository = new CoreRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(jobMaterial.PartWarehouseLocationID) && !string.IsNullOrWhiteSpace(jobMaterial.PartBinID) && !(await coreRepository.DoesBinExistAsync(jobMaterial.PartID, jobMaterial.PartRevisionID, jobMaterial.PartWarehouseLocationID, jobMaterial.PartBinID)))
				{
					base.ErrorsList.Add("Warehouse [" + jobMaterial.PartWarehouseLocationID + "] or bin [" + jobMaterial.PartBinID + "] is invalid or inactive.");
				}
			}
			using (PurchaseOrderRepository purchaseOrderRepository = new PurchaseOrderRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(jobMaterial.PurchaseOrderID) && !(await purchaseOrderRepository.DoesPurchaseOrderExists(jobMaterial.PurchaseOrderID)))
				{
					base.ErrorsList.Add("Purchase Order ID [" + jobMaterial.PurchaseOrderID + "] is invalid.");
				}
			}
			using (OrganizationRepository organizationRepository = new OrganizationRepository(base.ApiClientContext))
			{
				if (!string.IsNullOrWhiteSpace(jobMaterial.PurchaseLocationID) && !(await organizationRepository.DoesSupplierPurchaseLocationExists(jobMaterial.SupplierOrganizationID, jobMaterial.PurchaseLocationID)))
				{
					base.ErrorsList.Add("Supplier Organization ID [" + jobMaterial.SupplierOrganizationID + "] or Purchase Location ID [" + jobMaterial.PurchaseLocationID + "] is invalid.");
				}
			}
			if (jobMaterial.QuantityPerAssembly <= 0m)
			{
				base.ErrorsList.Add($"Quantity per assembly [{jobMaterial.QuantityPerAssembly}] is invalid");
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the job [" + jobMaterial.JobID + "]");
		}
		finally
		{
			result = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return await Task.FromResult(result);
	}

	public Task<APIValidationInfoDto> ValidateRequest_DeleteJobMaterial(string jobId, int jobAssemblyId, int jobMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		APIValidationInfoDto result2;
		try
		{
			using (JobMaterialRepository jobMaterialRepository = new JobMaterialRepository(base.ApiClientContext))
			{
				if (!jobMaterialRepository.DoesJobMaterialExists(jobId, jobAssemblyId, jobMaterialId).Result)
				{
					base.ErrorsList.Add($"Job [{jobId}] or job assembly [{jobAssemblyId}] or job material [{jobMaterialId}] is invalid");
				}
				else
				{
					string result = jobMaterialRepository.WhereUsed("JobMaterials", new object[3] { jobId, jobAssemblyId, jobMaterialId }, new object[3] { "jmmJobID", "jmmJobAssemblyID", "jmmJobMaterialID" }, onlyIncludeForeignRelations: true).Result;
					if (result.Length > 0)
					{
						base.ErrorsList.Add("Job material cannot be deleted because it is used in following places.\n [" + result.ToString().Trim() + "]");
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

	public async Task<BOMResponseMessageDto<IList<BOMJobMaterialDto>>> Process_GetAllJobMaterials(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMJobMaterialDto> allJobMaterialsDto = new List<BOMJobMaterialDto>();
		BOMResponseMessageDto<IList<BOMJobMaterialDto>> result;
		try
		{
			using JobMaterialRepository jobMaterialRepository = new JobMaterialRepository(base.ApiClientContext);
			foreach (BOMJobMaterialDto item2 in await jobMaterialRepository.GetAllJobMaterials(pageSize, pageNumber))
			{
				BOMJobMaterialDto item = new BOMJobMaterialDto
				{
					JobID = item2.JobID,
					JobAssemblyID = item2.JobAssemblyID,
					JobMaterialID = item2.JobMaterialID,
					PartID = item2.PartID,
					PartRevisionID = item2.PartRevisionID,
					PartWarehouseLocationID = item2.PartWarehouseLocationID,
					PartBinID = item2.PartBinID,
					UnitOfMeasure = item2.UnitOfMeasure,
					PartShortDescription = item2.PartShortDescription,
					QuantityPerAssembly = item2.QuantityPerAssembly,
					ScrapPercent = item2.ScrapPercent,
					ScrapQuantity = item2.ScrapQuantity,
					EstimatedQuantity = item2.EstimatedQuantity,
					EstimatedUnitCost = item2.EstimatedUnitCost,
					CalculatedUnitCost = item2.CalculatedUnitCost,
					Firm = item2.Firm,
					SupplierOrganizationID = item2.SupplierOrganizationID,
					PurchaseLocationID = item2.PurchaseLocationID,
					PurchaseOrderID = item2.PurchaseOrderID,
					LeadTime = item2.LeadTime,
					MinimumCharge = item2.MinimumCharge,
					DueInDate = item2.DueInDate,
					RequiredDate = item2.RequiredDate,
					QuantityAllocated = item2.QuantityAllocated,
					QuantityReceived = item2.QuantityReceived,
					ScrapQuantityReceived = item2.ScrapQuantityReceived,
					QuantityToInspect = item2.QuantityToInspect,
					QuantityToReturn = item2.QuantityToReturn,
					ReceivedComplete = item2.ReceivedComplete,
					PurchaseToJobQuantity = item2.PurchaseToJobQuantity,
					PullAllFromStock = item2.PullAllFromStock,
					PullFromStockQuantity = item2.PullFromStockQuantity,
					Closed = item2.Closed,
					UniqueID = item2.UniqueID,
					RowVersion = item2.RowVersion,
					RelatedJobOperationID = item2.RelatedJobOperationID
				};
				allJobMaterialsDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all JobMaterials]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMJobMaterialDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allJobMaterialsDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMJobMaterialDto>> Process_GetJobMaterial(string jobId, int jobAssemblyId, int jobMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMJobMaterialDto jobMaterialDto = new BOMJobMaterialDto();
		BOMResponseMessageDto<BOMJobMaterialDto> result;
		try
		{
			using JobMaterialRepository jobMaterialRepository = new JobMaterialRepository(base.ApiClientContext);
			jobMaterialDto = await jobMaterialRepository.GetJobMaterialInfo(jobId, jobAssemblyId, jobMaterialId);
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the job [{jobId}] assembly [{jobAssemblyId}] operation [{jobMaterialId}] ");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMJobMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobMaterialDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMJobMaterialDto>> Process_PostJobMaterial(BOMJobMaterialDto jobMaterial)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		BOMResponseMessageDto<BOMJobMaterialDto> result;
		try
		{
			using JobMaterialRepository jobMaterialRepository = new JobMaterialRepository(base.ApiClientContext);
			APIValidationInfoDto aPIValidationInfoDto = await jobMaterialRepository.SaveJobMaterial(jobMaterial);
			((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
			((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing JobMaterial [{jobMaterial.JobMaterialID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMJobMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobMaterial
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMJobMaterialDto>> Process_DeleteJobMaterial(string jobId, int jobAssemblyId, int jobMaterialId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMResponseMessageDto<BOMJobMaterialDto> result;
		try
		{
			using (JobMaterialRepository jobMaterialRepository = new JobMaterialRepository(base.ApiClientContext))
			{
				APIValidationInfoDto aPIValidationInfoDto = await jobMaterialRepository.DeleteJobMaterial(jobId, jobAssemblyId, jobMaterialId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the job [{jobId}] assembly [{jobAssemblyId}] material [{jobMaterialId}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMJobMaterialDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new BOMJobMaterialDto()
			};
		}
		return result;
	}
}
