using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPChangeRequestTypeModel : ERPBaseModel, IERPChangeRequestTypeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllChangeRequestTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPChangeRequestTypeRepository iERPChangeRequestTypeRepository = (base.ERPChangeRequestTypeRepository = new ERPChangeRequestTypeRepository(base.ApiClientContext));
		using (iERPChangeRequestTypeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPChangeRequestTypeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPChangeRequestTypeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPChangeRequestTypeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPChangeRequestTypeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetChangeRequestType(Guid changeRequestTypeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPChangeRequestTypeRepository iERPChangeRequestTypeRepository = (base.ERPChangeRequestTypeRepository = new ERPChangeRequestTypeRepository(base.ApiClientContext));
		using (iERPChangeRequestTypeRepository)
		{
			if (!(await base.ERPChangeRequestTypeRepository.DoesChangeRequestTypeExist(changeRequestTypeId)))
			{
				errorsList.Add($"ChangeRequestType [{changeRequestTypeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutChangeRequestType(ERPChangeRequestTypeDto changeRequestType)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPChangeRequestTypeRepository iERPChangeRequestTypeRepository = (base.ERPChangeRequestTypeRepository = new ERPChangeRequestTypeRepository(base.ApiClientContext));
		using (iERPChangeRequestTypeRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPChangeRequestTypeDto>>> Process_GetAllChangeRequestTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPChangeRequestTypeDto> allChangeRequestTypesDto = new List<ERPChangeRequestTypeDto>();
		ERPResponseMessageDto<IList<ERPChangeRequestTypeDto>> result;
		try
		{
			IERPChangeRequestTypeRepository iERPChangeRequestTypeRepository = (base.ERPChangeRequestTypeRepository = new ERPChangeRequestTypeRepository(base.ApiClientContext));
			using (iERPChangeRequestTypeRepository)
			{
				foreach (ERPChangeRequestTypeInformationDto item2 in await base.ERPChangeRequestTypeRepository.GetAllChangeRequestTypes(pageSize, pageNumber, filter, orderBy))
				{
					ERPChangeRequestTypeDto item = new ERPChangeRequestTypeDto
					{
						chtChangeRequestTypeID = item2.chtChangeRequestTypeID,
						chtCreatedBy = item2.chtCreatedBy,
						chtCreatedDate = item2.chtCreatedDate,
						chtDescription = item2.chtDescription,
						chtUniqueID = item2.chtUniqueID,
						chtInactiveDate = item2.chtInactiveDate,
						chtInactive = item2.chtInactive,
						chtRowVersion = item2.chtRowVersion,
						CustomFields = item2.CustomFields
					};
					allChangeRequestTypesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ChangeRequestTypes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPChangeRequestTypeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allChangeRequestTypesDto,
				RecordCount = allChangeRequestTypesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPChangeRequestTypeDto>> Process_GetChangeRequestType(Guid changeRequestTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPChangeRequestTypeDto changeRequestTypeDto = null;
		ERPResponseMessageDto<ERPChangeRequestTypeDto> result;
		try
		{
			IERPChangeRequestTypeRepository iERPChangeRequestTypeRepository = (base.ERPChangeRequestTypeRepository = new ERPChangeRequestTypeRepository(base.ApiClientContext));
			using (iERPChangeRequestTypeRepository)
			{
				ERPChangeRequestTypeInformationDto eRPChangeRequestTypeInformationDto = await base.ERPChangeRequestTypeRepository.GetChangeRequestType(changeRequestTypeId);
				changeRequestTypeDto = new ERPChangeRequestTypeDto
				{
					chtChangeRequestTypeID = eRPChangeRequestTypeInformationDto.chtChangeRequestTypeID,
					chtCreatedBy = eRPChangeRequestTypeInformationDto.chtCreatedBy,
					chtCreatedDate = eRPChangeRequestTypeInformationDto.chtCreatedDate,
					chtDescription = eRPChangeRequestTypeInformationDto.chtDescription,
					chtUniqueID = eRPChangeRequestTypeInformationDto.chtUniqueID,
					chtInactiveDate = eRPChangeRequestTypeInformationDto.chtInactiveDate,
					chtInactive = eRPChangeRequestTypeInformationDto.chtInactive,
					chtRowVersion = eRPChangeRequestTypeInformationDto.chtRowVersion,
					CustomFields = eRPChangeRequestTypeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ChangeRequestTypes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPChangeRequestTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = changeRequestTypeDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPChangeRequestTypeDto>> Process_PutChangeRequestType(ERPChangeRequestTypeDto changeRequestType)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPChangeRequestTypeDto createdObject = null;
		ERPResponseMessageDto<ERPChangeRequestTypeDto> result;
		try
		{
			IERPChangeRequestTypeRepository iERPChangeRequestTypeRepository = (base.ERPChangeRequestTypeRepository = new ERPChangeRequestTypeRepository(base.ApiClientContext));
			using (iERPChangeRequestTypeRepository)
			{
				APIValidationInfoDto postResult = await base.ERPChangeRequestTypeRepository.SaveChangeRequestType(changeRequestType);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPChangeRequestTypeInformationDto eRPChangeRequestTypeInformationDto = await base.ERPChangeRequestTypeRepository.GetChangeRequestType(changeRequestType.chtUniqueID);
					createdObject = new ERPChangeRequestTypeDto
					{
						chtChangeRequestTypeID = eRPChangeRequestTypeInformationDto.chtChangeRequestTypeID,
						chtCreatedBy = eRPChangeRequestTypeInformationDto.chtCreatedBy,
						chtCreatedDate = eRPChangeRequestTypeInformationDto.chtCreatedDate,
						chtDescription = eRPChangeRequestTypeInformationDto.chtDescription,
						chtUniqueID = eRPChangeRequestTypeInformationDto.chtUniqueID,
						chtInactiveDate = eRPChangeRequestTypeInformationDto.chtInactiveDate,
						chtInactive = eRPChangeRequestTypeInformationDto.chtInactive,
						chtRowVersion = eRPChangeRequestTypeInformationDto.chtRowVersion,
						CustomFields = eRPChangeRequestTypeInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ChangeRequestType [{changeRequestType.chtUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPChangeRequestTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteChangeRequestType(Guid changeRequestTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPChangeRequestTypeRepository iERPChangeRequestTypeRepository = (base.ERPChangeRequestTypeRepository = new ERPChangeRequestTypeRepository(base.ApiClientContext));
		using (iERPChangeRequestTypeRepository)
		{
			if (!(await base.ERPChangeRequestTypeRepository.DoesChangeRequestTypeExist(changeRequestTypeId)))
			{
				base.ErrorsList.Add($"ChangeRequestType [{changeRequestTypeId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPChangeRequestTypeInformationDto eRPChangeRequestTypeInformationDto = await base.ERPChangeRequestTypeRepository.GetChangeRequestType(changeRequestTypeId);
				string text = await base.ERPChangeRequestTypeRepository.WhereUsed("ChangeRequestTypes", new object[1] { eRPChangeRequestTypeInformationDto.chtChangeRequestTypeID }, new object[1] { "chtChangeRequestTypeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ChangeRequestType cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPChangeRequestTypeDto>> Process_DeleteChangeRequestType(Guid changeRequestTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPChangeRequestTypeDto> result;
		try
		{
			IERPChangeRequestTypeRepository iERPChangeRequestTypeRepository = (base.ERPChangeRequestTypeRepository = new ERPChangeRequestTypeRepository(base.ApiClientContext));
			using (iERPChangeRequestTypeRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPChangeRequestTypeRepository.DeleteRowFromTable("ChangeRequestTypes", "cht", changeRequestTypeId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ChangeRequestType [{changeRequestTypeId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPChangeRequestTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPChangeRequestTypeDto()
			};
		}
		return result;
	}
}
