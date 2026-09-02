using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPJobMaterialComponentModel : ERPBaseModel, IERPJobMaterialComponentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllJobMaterialComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPJobMaterialComponentRepository iERPJobMaterialComponentRepository = (base.ERPJobMaterialComponentRepository = new ERPJobMaterialComponentRepository(base.ApiClientContext));
		using (iERPJobMaterialComponentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPJobMaterialComponentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPJobMaterialComponentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPJobMaterialComponentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPJobMaterialComponentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetJobMaterialComponent(Guid jobMaterialComponentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobMaterialComponentRepository iERPJobMaterialComponentRepository = (base.ERPJobMaterialComponentRepository = new ERPJobMaterialComponentRepository(base.ApiClientContext));
		using (iERPJobMaterialComponentRepository)
		{
			if (!(await base.ERPJobMaterialComponentRepository.DoesJobMaterialComponentExist(jobMaterialComponentId)))
			{
				errorsList.Add($"JobMaterialComponent [{jobMaterialComponentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutJobMaterialComponent(ERPJobMaterialComponentDto jobMaterialComponent)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobMaterialComponentRepository iERPJobMaterialComponentRepository = (base.ERPJobMaterialComponentRepository = new ERPJobMaterialComponentRepository(base.ApiClientContext));
		using (iERPJobMaterialComponentRepository)
		{
			if (!string.IsNullOrWhiteSpace(jobMaterialComponent.jmtJobID) && !(await base.ERPJobMaterialComponentRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { jobMaterialComponent.jmtJobID })))
			{
				errorsList.Add("jmtJobID [" + jobMaterialComponent.jmtJobID + "] not found.");
			}
			if (jobMaterialComponent.jmtJobAssemblyID > 0 && !(await base.ERPJobMaterialComponentRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { jobMaterialComponent.jmtJobID, jobMaterialComponent.jmtJobAssemblyID })))
			{
				errorsList.Add($"jmtJobAssemblyID [{jobMaterialComponent.jmtJobAssemblyID}] not found.");
			}
			if (jobMaterialComponent.jmtJobMaterialID > 0 && !(await base.ERPJobMaterialComponentRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { jobMaterialComponent.jmtJobID, jobMaterialComponent.jmtJobAssemblyID, jobMaterialComponent.jmtJobMaterialID })))
			{
				errorsList.Add($"jmtJobMaterialID [{jobMaterialComponent.jmtJobMaterialID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobMaterialComponent.jmtPartID) && !(await base.ERPJobMaterialComponentRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { jobMaterialComponent.jmtPartID })))
			{
				errorsList.Add("jmtPartID [" + jobMaterialComponent.jmtPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobMaterialComponent.jmtPartRevisionID) && !(await base.ERPJobMaterialComponentRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { jobMaterialComponent.jmtPartID, jobMaterialComponent.jmtPartRevisionID })))
			{
				errorsList.Add("jmtPartRevisionID [" + jobMaterialComponent.jmtPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobMaterialComponent.jmtPartWarehouseLocationID) && !(await base.ERPJobMaterialComponentRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { jobMaterialComponent.jmtPartID, jobMaterialComponent.jmtPartRevisionID, jobMaterialComponent.jmtPartWarehouseLocationID })))
			{
				errorsList.Add("jmtPartWarehouseLocationID [" + jobMaterialComponent.jmtPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobMaterialComponent.jmtPartBinID) && !(await base.ERPJobMaterialComponentRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { jobMaterialComponent.jmtPartID, jobMaterialComponent.jmtPartRevisionID, jobMaterialComponent.jmtPartWarehouseLocationID, jobMaterialComponent.jmtPartBinID })))
			{
				errorsList.Add("jmtPartBinID [" + jobMaterialComponent.jmtPartBinID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPJobMaterialComponentDto>>> Process_GetAllJobMaterialComponents(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPJobMaterialComponentDto> allJobMaterialComponentsDto = new List<ERPJobMaterialComponentDto>();
		ERPResponseMessageDto<IList<ERPJobMaterialComponentDto>> result;
		try
		{
			IERPJobMaterialComponentRepository iERPJobMaterialComponentRepository = (base.ERPJobMaterialComponentRepository = new ERPJobMaterialComponentRepository(base.ApiClientContext));
			using (iERPJobMaterialComponentRepository)
			{
				foreach (ERPJobMaterialComponentInformationDto item2 in await base.ERPJobMaterialComponentRepository.GetAllJobMaterialComponents(pageSize, pageNumber, filter, orderBy))
				{
					ERPJobMaterialComponentDto item = new ERPJobMaterialComponentDto
					{
						jmtAdditionalQuantity = item2.jmtAdditionalQuantity,
						jmtCreatedBy = item2.jmtCreatedBy,
						jmtCreatedDate = item2.jmtCreatedDate,
						jmtDescription = item2.jmtDescription,
						jmtUniqueID = item2.jmtUniqueID,
						jmtClosed = item2.jmtClosed,
						jmtPullAllFromStock = item2.jmtPullAllFromStock,
						jmtReceivedComplete = item2.jmtReceivedComplete,
						jmtJobAssemblyID = item2.jmtJobAssemblyID,
						jmtJobID = item2.jmtJobID,
						jmtJobMaterialID = item2.jmtJobMaterialID,
						jmtMaterialQuantity = item2.jmtMaterialQuantity,
						jmtParentQuantity = item2.jmtParentQuantity,
						jmtPartBinID = item2.jmtPartBinID,
						jmtPartID = item2.jmtPartID,
						jmtPartRevisionID = item2.jmtPartRevisionID,
						jmtPartWarehouseLocationID = item2.jmtPartWarehouseLocationID,
						jmtQuantityAllocated = item2.jmtQuantityAllocated,
						jmtQuantityPerParent = item2.jmtQuantityPerParent,
						jmtQuantityReceived = item2.jmtQuantityReceived,
						jmtQuantityToInspect = item2.jmtQuantityToInspect,
						jmtQuantityToReturn = item2.jmtQuantityToReturn,
						jmtRowVersion = item2.jmtRowVersion,
						jmtScrapQuantityReceived = item2.jmtScrapQuantityReceived,
						jmtJobMaterialComponentID = item2.jmtJobMaterialComponentID,
						jmtUnitOfMeasure = item2.jmtUnitOfMeasure,
						jmtWeight = item2.jmtWeight,
						CustomFields = item2.CustomFields
					};
					allJobMaterialComponentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all JobMaterialComponents]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPJobMaterialComponentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allJobMaterialComponentsDto,
				RecordCount = allJobMaterialComponentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobMaterialComponentDto>> Process_GetJobMaterialComponent(Guid jobMaterialComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPJobMaterialComponentDto jobMaterialComponentDto = null;
		ERPResponseMessageDto<ERPJobMaterialComponentDto> result;
		try
		{
			IERPJobMaterialComponentRepository iERPJobMaterialComponentRepository = (base.ERPJobMaterialComponentRepository = new ERPJobMaterialComponentRepository(base.ApiClientContext));
			using (iERPJobMaterialComponentRepository)
			{
				ERPJobMaterialComponentInformationDto eRPJobMaterialComponentInformationDto = await base.ERPJobMaterialComponentRepository.GetJobMaterialComponent(jobMaterialComponentId);
				jobMaterialComponentDto = new ERPJobMaterialComponentDto
				{
					jmtAdditionalQuantity = eRPJobMaterialComponentInformationDto.jmtAdditionalQuantity,
					jmtCreatedBy = eRPJobMaterialComponentInformationDto.jmtCreatedBy,
					jmtCreatedDate = eRPJobMaterialComponentInformationDto.jmtCreatedDate,
					jmtDescription = eRPJobMaterialComponentInformationDto.jmtDescription,
					jmtUniqueID = eRPJobMaterialComponentInformationDto.jmtUniqueID,
					jmtClosed = eRPJobMaterialComponentInformationDto.jmtClosed,
					jmtPullAllFromStock = eRPJobMaterialComponentInformationDto.jmtPullAllFromStock,
					jmtReceivedComplete = eRPJobMaterialComponentInformationDto.jmtReceivedComplete,
					jmtJobAssemblyID = eRPJobMaterialComponentInformationDto.jmtJobAssemblyID,
					jmtJobID = eRPJobMaterialComponentInformationDto.jmtJobID,
					jmtJobMaterialID = eRPJobMaterialComponentInformationDto.jmtJobMaterialID,
					jmtMaterialQuantity = eRPJobMaterialComponentInformationDto.jmtMaterialQuantity,
					jmtParentQuantity = eRPJobMaterialComponentInformationDto.jmtParentQuantity,
					jmtPartBinID = eRPJobMaterialComponentInformationDto.jmtPartBinID,
					jmtPartID = eRPJobMaterialComponentInformationDto.jmtPartID,
					jmtPartRevisionID = eRPJobMaterialComponentInformationDto.jmtPartRevisionID,
					jmtPartWarehouseLocationID = eRPJobMaterialComponentInformationDto.jmtPartWarehouseLocationID,
					jmtQuantityAllocated = eRPJobMaterialComponentInformationDto.jmtQuantityAllocated,
					jmtQuantityPerParent = eRPJobMaterialComponentInformationDto.jmtQuantityPerParent,
					jmtQuantityReceived = eRPJobMaterialComponentInformationDto.jmtQuantityReceived,
					jmtQuantityToInspect = eRPJobMaterialComponentInformationDto.jmtQuantityToInspect,
					jmtQuantityToReturn = eRPJobMaterialComponentInformationDto.jmtQuantityToReturn,
					jmtRowVersion = eRPJobMaterialComponentInformationDto.jmtRowVersion,
					jmtScrapQuantityReceived = eRPJobMaterialComponentInformationDto.jmtScrapQuantityReceived,
					jmtJobMaterialComponentID = eRPJobMaterialComponentInformationDto.jmtJobMaterialComponentID,
					jmtUnitOfMeasure = eRPJobMaterialComponentInformationDto.jmtUnitOfMeasure,
					jmtWeight = eRPJobMaterialComponentInformationDto.jmtWeight,
					CustomFields = eRPJobMaterialComponentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the JobMaterialComponents []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobMaterialComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobMaterialComponentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobMaterialComponentDto>> Process_PutJobMaterialComponent(ERPJobMaterialComponentDto jobMaterialComponent)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPJobMaterialComponentDto createdObject = null;
		ERPResponseMessageDto<ERPJobMaterialComponentDto> result;
		try
		{
			IERPJobMaterialComponentRepository iERPJobMaterialComponentRepository = (base.ERPJobMaterialComponentRepository = new ERPJobMaterialComponentRepository(base.ApiClientContext));
			using (iERPJobMaterialComponentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPJobMaterialComponentRepository.SaveJobMaterialComponent(jobMaterialComponent);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPJobMaterialComponentInformationDto eRPJobMaterialComponentInformationDto = await base.ERPJobMaterialComponentRepository.GetJobMaterialComponent(jobMaterialComponent.jmtUniqueID);
					createdObject = new ERPJobMaterialComponentDto
					{
						jmtAdditionalQuantity = eRPJobMaterialComponentInformationDto.jmtAdditionalQuantity,
						jmtCreatedBy = eRPJobMaterialComponentInformationDto.jmtCreatedBy,
						jmtCreatedDate = eRPJobMaterialComponentInformationDto.jmtCreatedDate,
						jmtDescription = eRPJobMaterialComponentInformationDto.jmtDescription,
						jmtUniqueID = eRPJobMaterialComponentInformationDto.jmtUniqueID,
						jmtClosed = eRPJobMaterialComponentInformationDto.jmtClosed,
						jmtPullAllFromStock = eRPJobMaterialComponentInformationDto.jmtPullAllFromStock,
						jmtReceivedComplete = eRPJobMaterialComponentInformationDto.jmtReceivedComplete,
						jmtJobAssemblyID = eRPJobMaterialComponentInformationDto.jmtJobAssemblyID,
						jmtJobID = eRPJobMaterialComponentInformationDto.jmtJobID,
						jmtJobMaterialID = eRPJobMaterialComponentInformationDto.jmtJobMaterialID,
						jmtMaterialQuantity = eRPJobMaterialComponentInformationDto.jmtMaterialQuantity,
						jmtParentQuantity = eRPJobMaterialComponentInformationDto.jmtParentQuantity,
						jmtPartBinID = eRPJobMaterialComponentInformationDto.jmtPartBinID,
						jmtPartID = eRPJobMaterialComponentInformationDto.jmtPartID,
						jmtPartRevisionID = eRPJobMaterialComponentInformationDto.jmtPartRevisionID,
						jmtPartWarehouseLocationID = eRPJobMaterialComponentInformationDto.jmtPartWarehouseLocationID,
						jmtQuantityAllocated = eRPJobMaterialComponentInformationDto.jmtQuantityAllocated,
						jmtQuantityPerParent = eRPJobMaterialComponentInformationDto.jmtQuantityPerParent,
						jmtQuantityReceived = eRPJobMaterialComponentInformationDto.jmtQuantityReceived,
						jmtQuantityToInspect = eRPJobMaterialComponentInformationDto.jmtQuantityToInspect,
						jmtQuantityToReturn = eRPJobMaterialComponentInformationDto.jmtQuantityToReturn,
						jmtRowVersion = eRPJobMaterialComponentInformationDto.jmtRowVersion,
						jmtScrapQuantityReceived = eRPJobMaterialComponentInformationDto.jmtScrapQuantityReceived,
						jmtJobMaterialComponentID = eRPJobMaterialComponentInformationDto.jmtJobMaterialComponentID,
						jmtUnitOfMeasure = eRPJobMaterialComponentInformationDto.jmtUnitOfMeasure,
						jmtWeight = eRPJobMaterialComponentInformationDto.jmtWeight,
						CustomFields = eRPJobMaterialComponentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing JobMaterialComponent [{jobMaterialComponent.jmtUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobMaterialComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteJobMaterialComponent(Guid jobMaterialComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobMaterialComponentRepository iERPJobMaterialComponentRepository = (base.ERPJobMaterialComponentRepository = new ERPJobMaterialComponentRepository(base.ApiClientContext));
		using (iERPJobMaterialComponentRepository)
		{
			if (!(await base.ERPJobMaterialComponentRepository.DoesJobMaterialComponentExist(jobMaterialComponentId)))
			{
				base.ErrorsList.Add($"JobMaterialComponent [{jobMaterialComponentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPJobMaterialComponentInformationDto eRPJobMaterialComponentInformationDto = await base.ERPJobMaterialComponentRepository.GetJobMaterialComponent(jobMaterialComponentId);
				string text = await base.ERPJobMaterialComponentRepository.WhereUsed("JobMaterialComponents", new object[4] { eRPJobMaterialComponentInformationDto.jmtJobID, eRPJobMaterialComponentInformationDto.jmtJobAssemblyID, eRPJobMaterialComponentInformationDto.jmtJobMaterialID, eRPJobMaterialComponentInformationDto.jmtJobMaterialComponentID }, new object[4] { "jmtJobID", "jmtJobAssemblyID", "jmtJobMaterialID", "jmtJobMaterialComponentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("JobMaterialComponent cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPJobMaterialComponentDto>> Process_DeleteJobMaterialComponent(Guid jobMaterialComponentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPJobMaterialComponentDto> result;
		try
		{
			IERPJobMaterialComponentRepository iERPJobMaterialComponentRepository = (base.ERPJobMaterialComponentRepository = new ERPJobMaterialComponentRepository(base.ApiClientContext));
			using (iERPJobMaterialComponentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPJobMaterialComponentRepository.DeleteRowFromTable("JobMaterialComponents", "jmt", jobMaterialComponentId);
				((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
				IList<string> errorsList = base.ErrorsList;
				if (errorsList != null && errorsList.Count > 0)
				{
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of JobMaterialComponent [{jobMaterialComponentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobMaterialComponentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPJobMaterialComponentDto()
			};
		}
		return result;
	}
}
