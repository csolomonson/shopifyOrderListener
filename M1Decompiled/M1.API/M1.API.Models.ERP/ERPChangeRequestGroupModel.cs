using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPChangeRequestGroupModel : ERPBaseModel, IERPChangeRequestGroupModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllChangeRequestGroups(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPChangeRequestGroupRepository iERPChangeRequestGroupRepository = (base.ERPChangeRequestGroupRepository = new ERPChangeRequestGroupRepository(base.ApiClientContext));
		using (iERPChangeRequestGroupRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPChangeRequestGroupRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPChangeRequestGroupRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPChangeRequestGroupRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPChangeRequestGroupRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetChangeRequestGroup(Guid changeRequestGroupId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPChangeRequestGroupRepository iERPChangeRequestGroupRepository = (base.ERPChangeRequestGroupRepository = new ERPChangeRequestGroupRepository(base.ApiClientContext));
		using (iERPChangeRequestGroupRepository)
		{
			if (!(await base.ERPChangeRequestGroupRepository.DoesChangeRequestGroupExist(changeRequestGroupId)))
			{
				errorsList.Add($"ChangeRequestGroup [{changeRequestGroupId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPChangeRequestGroupDto>>> Process_GetAllChangeRequestGroups(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPChangeRequestGroupDto> allChangeRequestGroupsDto = new List<ERPChangeRequestGroupDto>();
		ERPResponseMessageDto<IList<ERPChangeRequestGroupDto>> result;
		try
		{
			IERPChangeRequestGroupRepository iERPChangeRequestGroupRepository = (base.ERPChangeRequestGroupRepository = new ERPChangeRequestGroupRepository(base.ApiClientContext));
			using (iERPChangeRequestGroupRepository)
			{
				foreach (ERPChangeRequestGroupInformationDto item2 in await base.ERPChangeRequestGroupRepository.GetAllChangeRequestGroups(pageSize, pageNumber, filter, orderBy))
				{
					ERPChangeRequestGroupDto item = new ERPChangeRequestGroupDto
					{
						chgChangeRequestGroupID = item2.chgChangeRequestGroupID,
						chgCreatedBy = item2.chgCreatedBy,
						chgCreatedDate = item2.chgCreatedDate,
						chgDescription = item2.chgDescription,
						chgUniqueID = item2.chgUniqueID,
						chgInactiveDate = item2.chgInactiveDate,
						chgInactive = item2.chgInactive,
						chgRowVersion = item2.chgRowVersion,
						CustomFields = item2.CustomFields
					};
					allChangeRequestGroupsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ChangeRequestGroups]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPChangeRequestGroupDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allChangeRequestGroupsDto,
				RecordCount = allChangeRequestGroupsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPChangeRequestGroupDto>> Process_GetChangeRequestGroup(Guid changeRequestGroupId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPChangeRequestGroupDto changeRequestGroupDto = null;
		ERPResponseMessageDto<ERPChangeRequestGroupDto> result;
		try
		{
			IERPChangeRequestGroupRepository iERPChangeRequestGroupRepository = (base.ERPChangeRequestGroupRepository = new ERPChangeRequestGroupRepository(base.ApiClientContext));
			using (iERPChangeRequestGroupRepository)
			{
				ERPChangeRequestGroupInformationDto eRPChangeRequestGroupInformationDto = await base.ERPChangeRequestGroupRepository.GetChangeRequestGroup(changeRequestGroupId);
				changeRequestGroupDto = new ERPChangeRequestGroupDto
				{
					chgChangeRequestGroupID = eRPChangeRequestGroupInformationDto.chgChangeRequestGroupID,
					chgCreatedBy = eRPChangeRequestGroupInformationDto.chgCreatedBy,
					chgCreatedDate = eRPChangeRequestGroupInformationDto.chgCreatedDate,
					chgDescription = eRPChangeRequestGroupInformationDto.chgDescription,
					chgUniqueID = eRPChangeRequestGroupInformationDto.chgUniqueID,
					chgInactiveDate = eRPChangeRequestGroupInformationDto.chgInactiveDate,
					chgInactive = eRPChangeRequestGroupInformationDto.chgInactive,
					chgRowVersion = eRPChangeRequestGroupInformationDto.chgRowVersion,
					CustomFields = eRPChangeRequestGroupInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ChangeRequestGroups []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPChangeRequestGroupDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = changeRequestGroupDto
			};
		}
		return result;
	}
}
