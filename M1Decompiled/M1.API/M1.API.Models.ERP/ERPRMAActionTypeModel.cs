using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPRMAActionTypeModel : ERPBaseModel, IERPRMAActionTypeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllRMAActionTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPRMAActionTypeRepository iERPRMAActionTypeRepository = (base.ERPRMAActionTypeRepository = new ERPRMAActionTypeRepository(base.ApiClientContext));
		using (iERPRMAActionTypeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPRMAActionTypeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPRMAActionTypeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPRMAActionTypeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPRMAActionTypeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetRMAActionType(Guid rMAActionTypeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAActionTypeRepository iERPRMAActionTypeRepository = (base.ERPRMAActionTypeRepository = new ERPRMAActionTypeRepository(base.ApiClientContext));
		using (iERPRMAActionTypeRepository)
		{
			if (!(await base.ERPRMAActionTypeRepository.DoesRMAActionTypeExist(rMAActionTypeId)))
			{
				errorsList.Add($"RMAActionType [{rMAActionTypeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPRMAActionTypeDto>>> Process_GetAllRMAActionTypes(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPRMAActionTypeDto> allRMAActionTypesDto = new List<ERPRMAActionTypeDto>();
		ERPResponseMessageDto<IList<ERPRMAActionTypeDto>> result;
		try
		{
			IERPRMAActionTypeRepository iERPRMAActionTypeRepository = (base.ERPRMAActionTypeRepository = new ERPRMAActionTypeRepository(base.ApiClientContext));
			using (iERPRMAActionTypeRepository)
			{
				foreach (ERPRMAActionTypeInformationDto item2 in await base.ERPRMAActionTypeRepository.GetAllRMAActionTypes(pageSize, pageNumber, filter, orderBy))
				{
					ERPRMAActionTypeDto item = new ERPRMAActionTypeDto
					{
						ratRmaActionTypeID = item2.ratRmaActionTypeID,
						ratDescription = item2.ratDescription,
						ratUniqueID = item2.ratUniqueID,
						ratRowVersion = item2.ratRowVersion,
						CustomFields = item2.CustomFields
					};
					allRMAActionTypesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all RMAActionTypes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPRMAActionTypeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allRMAActionTypesDto,
				RecordCount = allRMAActionTypesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRMAActionTypeDto>> Process_GetRMAActionType(Guid rMAActionTypeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPRMAActionTypeDto rMAActionTypeDto = null;
		ERPResponseMessageDto<ERPRMAActionTypeDto> result;
		try
		{
			IERPRMAActionTypeRepository iERPRMAActionTypeRepository = (base.ERPRMAActionTypeRepository = new ERPRMAActionTypeRepository(base.ApiClientContext));
			using (iERPRMAActionTypeRepository)
			{
				ERPRMAActionTypeInformationDto eRPRMAActionTypeInformationDto = await base.ERPRMAActionTypeRepository.GetRMAActionType(rMAActionTypeId);
				rMAActionTypeDto = new ERPRMAActionTypeDto
				{
					ratRmaActionTypeID = eRPRMAActionTypeInformationDto.ratRmaActionTypeID,
					ratDescription = eRPRMAActionTypeInformationDto.ratDescription,
					ratUniqueID = eRPRMAActionTypeInformationDto.ratUniqueID,
					ratRowVersion = eRPRMAActionTypeInformationDto.ratRowVersion,
					CustomFields = eRPRMAActionTypeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the RMAActionTypes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAActionTypeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = rMAActionTypeDto
			};
		}
		return result;
	}
}
