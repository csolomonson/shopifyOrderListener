using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPScheduleTreeModel : ERPBaseModel, IERPScheduleTreeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllScheduleTrees(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPScheduleTreeRepository iERPScheduleTreeRepository = (base.ERPScheduleTreeRepository = new ERPScheduleTreeRepository(base.ApiClientContext));
		using (iERPScheduleTreeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPScheduleTreeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPScheduleTreeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPScheduleTreeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPScheduleTreeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetScheduleTree(Guid scheduleTreeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPScheduleTreeRepository iERPScheduleTreeRepository = (base.ERPScheduleTreeRepository = new ERPScheduleTreeRepository(base.ApiClientContext));
		using (iERPScheduleTreeRepository)
		{
			if (!(await base.ERPScheduleTreeRepository.DoesScheduleTreeExist(scheduleTreeId)))
			{
				errorsList.Add($"ScheduleTree [{scheduleTreeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPScheduleTreeDto>>> Process_GetAllScheduleTrees(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPScheduleTreeDto> allScheduleTreesDto = new List<ERPScheduleTreeDto>();
		ERPResponseMessageDto<IList<ERPScheduleTreeDto>> result;
		try
		{
			IERPScheduleTreeRepository iERPScheduleTreeRepository = (base.ERPScheduleTreeRepository = new ERPScheduleTreeRepository(base.ApiClientContext));
			using (iERPScheduleTreeRepository)
			{
				foreach (ERPScheduleTreeInformationDto item2 in await base.ERPScheduleTreeRepository.GetAllScheduleTrees(pageSize, pageNumber, filter, orderBy))
				{
					ERPScheduleTreeDto item = new ERPScheduleTreeDto
					{
						sxtCreatedBy = item2.sxtCreatedBy,
						sxtCreatedDate = item2.sxtCreatedDate,
						sxtDescription = item2.sxtDescription,
						sxtUniqueID = item2.sxtUniqueID,
						sxtGroupUniqueID = item2.sxtGroupUniqueID,
						sxtJobScenarioID = item2.sxtJobScenarioID,
						sxtRowVersion = item2.sxtRowVersion,
						sxtScheduleTreeID = item2.sxtScheduleTreeID,
						sxtSourceTable = item2.sxtSourceTable,
						sxtSourceUniqueID = item2.sxtSourceUniqueID,
						sxtType = item2.sxtType,
						CustomFields = item2.CustomFields
					};
					allScheduleTreesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ScheduleTrees]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPScheduleTreeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allScheduleTreesDto,
				RecordCount = allScheduleTreesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPScheduleTreeDto>> Process_GetScheduleTree(Guid scheduleTreeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPScheduleTreeDto scheduleTreeDto = null;
		ERPResponseMessageDto<ERPScheduleTreeDto> result;
		try
		{
			IERPScheduleTreeRepository iERPScheduleTreeRepository = (base.ERPScheduleTreeRepository = new ERPScheduleTreeRepository(base.ApiClientContext));
			using (iERPScheduleTreeRepository)
			{
				ERPScheduleTreeInformationDto eRPScheduleTreeInformationDto = await base.ERPScheduleTreeRepository.GetScheduleTree(scheduleTreeId);
				scheduleTreeDto = new ERPScheduleTreeDto
				{
					sxtCreatedBy = eRPScheduleTreeInformationDto.sxtCreatedBy,
					sxtCreatedDate = eRPScheduleTreeInformationDto.sxtCreatedDate,
					sxtDescription = eRPScheduleTreeInformationDto.sxtDescription,
					sxtUniqueID = eRPScheduleTreeInformationDto.sxtUniqueID,
					sxtGroupUniqueID = eRPScheduleTreeInformationDto.sxtGroupUniqueID,
					sxtJobScenarioID = eRPScheduleTreeInformationDto.sxtJobScenarioID,
					sxtRowVersion = eRPScheduleTreeInformationDto.sxtRowVersion,
					sxtScheduleTreeID = eRPScheduleTreeInformationDto.sxtScheduleTreeID,
					sxtSourceTable = eRPScheduleTreeInformationDto.sxtSourceTable,
					sxtSourceUniqueID = eRPScheduleTreeInformationDto.sxtSourceUniqueID,
					sxtType = eRPScheduleTreeInformationDto.sxtType,
					CustomFields = eRPScheduleTreeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ScheduleTrees []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPScheduleTreeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = scheduleTreeDto
			};
		}
		return result;
	}
}
