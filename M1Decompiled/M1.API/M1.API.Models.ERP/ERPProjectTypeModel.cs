using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPProjectTypeModel : ERPBaseModel, IERPProjectTypeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllProjectTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProjectTypeRepository iERPProjectTypeRepository = (base.ERPProjectTypeRepository = new ERPProjectTypeRepository(base.ApiClientContext));
		using (iERPProjectTypeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPProjectTypeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPProjectTypeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPProjectTypeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPProjectTypeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetProjectType(Guid projectTypeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectTypeRepository iERPProjectTypeRepository = (base.ERPProjectTypeRepository = new ERPProjectTypeRepository(base.ApiClientContext));
		using (iERPProjectTypeRepository)
		{
			if (!(await base.ERPProjectTypeRepository.DoesProjectTypeExist(projectTypeId)))
			{
				errorsList.Add($"ProjectType [{projectTypeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutProjectType(ERPProjectTypeDto projectType)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProjectTypeRepository iERPProjectTypeRepository = (base.ERPProjectTypeRepository = new ERPProjectTypeRepository(base.ApiClientContext));
		using (iERPProjectTypeRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPProjectTypeDto>>> Process_GetAllProjectTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPProjectTypeDto> allProjectTypesDto = new List<ERPProjectTypeDto>();
		ERPResponseMessageDto<IList<ERPProjectTypeDto>> result;
		try
		{
			IERPProjectTypeRepository iERPProjectTypeRepository = (base.ERPProjectTypeRepository = new ERPProjectTypeRepository(base.ApiClientContext));
			using (iERPProjectTypeRepository)
			{
				foreach (ERPProjectTypeInformationDto item2 in await base.ERPProjectTypeRepository.GetAllProjectTypes(pageSize, pageNumber, filter, orderBy))
				{
					ERPProjectTypeDto item = new ERPProjectTypeDto
					{
						prtProjectTypeID = item2.prtProjectTypeID,
						prtCreatedBy = item2.prtCreatedBy,
						prtCreatedDate = item2.prtCreatedDate,
						prtDescription = item2.prtDescription,
						prtUniqueID = item2.prtUniqueID,
						prtInactiveDate = item2.prtInactiveDate,
						prtInactive = item2.prtInactive,
						prtRowVersion = item2.prtRowVersion,
						CustomFields = item2.CustomFields
					};
					allProjectTypesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ProjectTypes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPProjectTypeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allProjectTypesDto,
				RecordCount = allProjectTypesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProjectTypeDto>> Process_GetProjectType(Guid projectTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPProjectTypeDto projectTypeDto = null;
		ERPResponseMessageDto<ERPProjectTypeDto> result;
		try
		{
			IERPProjectTypeRepository iERPProjectTypeRepository = (base.ERPProjectTypeRepository = new ERPProjectTypeRepository(base.ApiClientContext));
			using (iERPProjectTypeRepository)
			{
				ERPProjectTypeInformationDto eRPProjectTypeInformationDto = await base.ERPProjectTypeRepository.GetProjectType(projectTypeId);
				projectTypeDto = new ERPProjectTypeDto
				{
					prtProjectTypeID = eRPProjectTypeInformationDto.prtProjectTypeID,
					prtCreatedBy = eRPProjectTypeInformationDto.prtCreatedBy,
					prtCreatedDate = eRPProjectTypeInformationDto.prtCreatedDate,
					prtDescription = eRPProjectTypeInformationDto.prtDescription,
					prtUniqueID = eRPProjectTypeInformationDto.prtUniqueID,
					prtInactiveDate = eRPProjectTypeInformationDto.prtInactiveDate,
					prtInactive = eRPProjectTypeInformationDto.prtInactive,
					prtRowVersion = eRPProjectTypeInformationDto.prtRowVersion,
					CustomFields = eRPProjectTypeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ProjectTypes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = projectTypeDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProjectTypeDto>> Process_PutProjectType(ERPProjectTypeDto projectType)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPProjectTypeDto createdObject = null;
		ERPResponseMessageDto<ERPProjectTypeDto> result;
		try
		{
			IERPProjectTypeRepository iERPProjectTypeRepository = (base.ERPProjectTypeRepository = new ERPProjectTypeRepository(base.ApiClientContext));
			using (iERPProjectTypeRepository)
			{
				APIValidationInfoDto postResult = await base.ERPProjectTypeRepository.SaveProjectType(projectType);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPProjectTypeInformationDto eRPProjectTypeInformationDto = await base.ERPProjectTypeRepository.GetProjectType(projectType.prtUniqueID);
					createdObject = new ERPProjectTypeDto
					{
						prtProjectTypeID = eRPProjectTypeInformationDto.prtProjectTypeID,
						prtCreatedBy = eRPProjectTypeInformationDto.prtCreatedBy,
						prtCreatedDate = eRPProjectTypeInformationDto.prtCreatedDate,
						prtDescription = eRPProjectTypeInformationDto.prtDescription,
						prtUniqueID = eRPProjectTypeInformationDto.prtUniqueID,
						prtInactiveDate = eRPProjectTypeInformationDto.prtInactiveDate,
						prtInactive = eRPProjectTypeInformationDto.prtInactive,
						prtRowVersion = eRPProjectTypeInformationDto.prtRowVersion,
						CustomFields = eRPProjectTypeInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ProjectType [{projectType.prtUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteProjectType(Guid projectTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectTypeRepository iERPProjectTypeRepository = (base.ERPProjectTypeRepository = new ERPProjectTypeRepository(base.ApiClientContext));
		using (iERPProjectTypeRepository)
		{
			if (!(await base.ERPProjectTypeRepository.DoesProjectTypeExist(projectTypeId)))
			{
				base.ErrorsList.Add($"ProjectType [{projectTypeId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPProjectTypeInformationDto eRPProjectTypeInformationDto = await base.ERPProjectTypeRepository.GetProjectType(projectTypeId);
				string text = await base.ERPProjectTypeRepository.WhereUsed("ProjectTypes", new object[1] { eRPProjectTypeInformationDto.prtProjectTypeID }, new object[1] { "prtProjectTypeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ProjectType cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPProjectTypeDto>> Process_DeleteProjectType(Guid projectTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPProjectTypeDto> result;
		try
		{
			IERPProjectTypeRepository iERPProjectTypeRepository = (base.ERPProjectTypeRepository = new ERPProjectTypeRepository(base.ApiClientContext));
			using (iERPProjectTypeRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPProjectTypeRepository.DeleteRowFromTable("ProjectTypes", "prt", projectTypeId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ProjectType [{projectTypeId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPProjectTypeDto()
			};
		}
		return result;
	}
}
