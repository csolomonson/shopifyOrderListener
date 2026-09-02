using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPChangeLogModel : ERPBaseModel, IERPChangeLogModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllChangeLog(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPChangeLogRepository iERPChangeLogRepository = (base.ERPChangeLogRepository = new ERPChangeLogRepository(base.ApiClientContext));
		using (iERPChangeLogRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPChangeLogRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPChangeLogRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPChangeLogRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPChangeLogRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetChangeLog(Guid changeLogId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPChangeLogRepository iERPChangeLogRepository = (base.ERPChangeLogRepository = new ERPChangeLogRepository(base.ApiClientContext));
		using (iERPChangeLogRepository)
		{
			if (!(await base.ERPChangeLogRepository.DoesChangeLogExist(changeLogId)))
			{
				errorsList.Add($"ChangeLog [{changeLogId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPChangeLogDto>>> Process_GetAllChangeLog(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPChangeLogDto> allChangeLogDto = new List<ERPChangeLogDto>();
		ERPResponseMessageDto<IList<ERPChangeLogDto>> result;
		try
		{
			IERPChangeLogRepository iERPChangeLogRepository = (base.ERPChangeLogRepository = new ERPChangeLogRepository(base.ApiClientContext));
			using (iERPChangeLogRepository)
			{
				foreach (ERPChangeLogInformationDto item2 in await base.ERPChangeLogRepository.GetAllChangeLog(pageSize, pageNumber, filter, orderBy))
				{
					ERPChangeLogDto item = new ERPChangeLogDto
					{
						xagChangeDate = item2.xagChangeDate,
						xagChangeType = item2.xagChangeType,
						xagChangeUserID = item2.xagChangeUserID,
						xagRowVersion = item2.xagRowVersion,
						xagChangeLogID = item2.xagChangeLogID,
						xagTableKeyValues = item2.xagTableKeyValues,
						xagTableName = item2.xagTableName,
						xagTableNewValues = item2.xagTableNewValues,
						xagTableOldValues = item2.xagTableOldValues,
						xagTableUniqueID = item2.xagTableUniqueID,
						CustomFields = item2.CustomFields
					};
					allChangeLogDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ChangeLog]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPChangeLogDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allChangeLogDto,
				RecordCount = allChangeLogDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPChangeLogDto>> Process_GetChangeLog(Guid changeLogId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPChangeLogDto changeLogDto = null;
		ERPResponseMessageDto<ERPChangeLogDto> result;
		try
		{
			IERPChangeLogRepository iERPChangeLogRepository = (base.ERPChangeLogRepository = new ERPChangeLogRepository(base.ApiClientContext));
			using (iERPChangeLogRepository)
			{
				ERPChangeLogInformationDto eRPChangeLogInformationDto = await base.ERPChangeLogRepository.GetChangeLog(changeLogId);
				changeLogDto = new ERPChangeLogDto
				{
					xagChangeDate = eRPChangeLogInformationDto.xagChangeDate,
					xagChangeType = eRPChangeLogInformationDto.xagChangeType,
					xagChangeUserID = eRPChangeLogInformationDto.xagChangeUserID,
					xagRowVersion = eRPChangeLogInformationDto.xagRowVersion,
					xagChangeLogID = eRPChangeLogInformationDto.xagChangeLogID,
					xagTableKeyValues = eRPChangeLogInformationDto.xagTableKeyValues,
					xagTableName = eRPChangeLogInformationDto.xagTableName,
					xagTableNewValues = eRPChangeLogInformationDto.xagTableNewValues,
					xagTableOldValues = eRPChangeLogInformationDto.xagTableOldValues,
					xagTableUniqueID = eRPChangeLogInformationDto.xagTableUniqueID,
					CustomFields = eRPChangeLogInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ChangeLog []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPChangeLogDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = changeLogDto
			};
		}
		return result;
	}
}
