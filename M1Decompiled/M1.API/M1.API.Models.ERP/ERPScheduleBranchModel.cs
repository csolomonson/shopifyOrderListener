using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPScheduleBranchModel : ERPBaseModel, IERPScheduleBranchModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllScheduleBranches(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPScheduleBranchRepository iERPScheduleBranchRepository = (base.ERPScheduleBranchRepository = new ERPScheduleBranchRepository(base.ApiClientContext));
		using (iERPScheduleBranchRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPScheduleBranchRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPScheduleBranchRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPScheduleBranchRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPScheduleBranchRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetScheduleBranch(Guid scheduleBranchId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPScheduleBranchRepository iERPScheduleBranchRepository = (base.ERPScheduleBranchRepository = new ERPScheduleBranchRepository(base.ApiClientContext));
		using (iERPScheduleBranchRepository)
		{
			if (!(await base.ERPScheduleBranchRepository.DoesScheduleBranchExist(scheduleBranchId)))
			{
				errorsList.Add($"ScheduleBranch [{scheduleBranchId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPScheduleBranchDto>>> Process_GetAllScheduleBranches(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPScheduleBranchDto> allScheduleBranchesDto = new List<ERPScheduleBranchDto>();
		ERPResponseMessageDto<IList<ERPScheduleBranchDto>> result;
		try
		{
			IERPScheduleBranchRepository iERPScheduleBranchRepository = (base.ERPScheduleBranchRepository = new ERPScheduleBranchRepository(base.ApiClientContext));
			using (iERPScheduleBranchRepository)
			{
				foreach (ERPScheduleBranchInformationDto item2 in await base.ERPScheduleBranchRepository.GetAllScheduleBranches(pageSize, pageNumber, filter, orderBy))
				{
					ERPScheduleBranchDto item = new ERPScheduleBranchDto
					{
						sxbCreatedBy = item2.sxbCreatedBy,
						sxbCreatedDate = item2.sxbCreatedDate,
						sxbCurrentLinkedTaskDateType = item2.sxbCurrentLinkedTaskDateType,
						sxbCurrentLinkedTaskID = item2.sxbCurrentLinkedTaskID,
						sxbUniqueID = item2.sxbUniqueID,
						sxbOffsetMinutes = item2.sxbOffsetMinutes,
						sxbParentLinkedTaskDateType = item2.sxbParentLinkedTaskDateType,
						sxbParentLinkedTaskID = item2.sxbParentLinkedTaskID,
						sxbParentScheduleBranchID = item2.sxbParentScheduleBranchID,
						sxbRowVersion = item2.sxbRowVersion,
						sxbScheduleTreeID = item2.sxbScheduleTreeID,
						sxbScheduleBranchID = item2.sxbScheduleBranchID,
						sxbSiblingBranchLink = item2.sxbSiblingBranchLink,
						CustomFields = item2.CustomFields
					};
					allScheduleBranchesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ScheduleBranches]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPScheduleBranchDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allScheduleBranchesDto,
				RecordCount = allScheduleBranchesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPScheduleBranchDto>> Process_GetScheduleBranch(Guid scheduleBranchId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPScheduleBranchDto scheduleBranchDto = null;
		ERPResponseMessageDto<ERPScheduleBranchDto> result;
		try
		{
			IERPScheduleBranchRepository iERPScheduleBranchRepository = (base.ERPScheduleBranchRepository = new ERPScheduleBranchRepository(base.ApiClientContext));
			using (iERPScheduleBranchRepository)
			{
				ERPScheduleBranchInformationDto eRPScheduleBranchInformationDto = await base.ERPScheduleBranchRepository.GetScheduleBranch(scheduleBranchId);
				scheduleBranchDto = new ERPScheduleBranchDto
				{
					sxbCreatedBy = eRPScheduleBranchInformationDto.sxbCreatedBy,
					sxbCreatedDate = eRPScheduleBranchInformationDto.sxbCreatedDate,
					sxbCurrentLinkedTaskDateType = eRPScheduleBranchInformationDto.sxbCurrentLinkedTaskDateType,
					sxbCurrentLinkedTaskID = eRPScheduleBranchInformationDto.sxbCurrentLinkedTaskID,
					sxbUniqueID = eRPScheduleBranchInformationDto.sxbUniqueID,
					sxbOffsetMinutes = eRPScheduleBranchInformationDto.sxbOffsetMinutes,
					sxbParentLinkedTaskDateType = eRPScheduleBranchInformationDto.sxbParentLinkedTaskDateType,
					sxbParentLinkedTaskID = eRPScheduleBranchInformationDto.sxbParentLinkedTaskID,
					sxbParentScheduleBranchID = eRPScheduleBranchInformationDto.sxbParentScheduleBranchID,
					sxbRowVersion = eRPScheduleBranchInformationDto.sxbRowVersion,
					sxbScheduleTreeID = eRPScheduleBranchInformationDto.sxbScheduleTreeID,
					sxbScheduleBranchID = eRPScheduleBranchInformationDto.sxbScheduleBranchID,
					sxbSiblingBranchLink = eRPScheduleBranchInformationDto.sxbSiblingBranchLink,
					CustomFields = eRPScheduleBranchInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ScheduleBranches []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPScheduleBranchDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = scheduleBranchDto
			};
		}
		return result;
	}
}
