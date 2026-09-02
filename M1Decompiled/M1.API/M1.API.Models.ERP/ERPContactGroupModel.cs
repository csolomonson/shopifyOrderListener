using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPContactGroupModel : ERPBaseModel, IERPContactGroupModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllContactGroups(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPContactGroupRepository iERPContactGroupRepository = (base.ERPContactGroupRepository = new ERPContactGroupRepository(base.ApiClientContext));
		using (iERPContactGroupRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPContactGroupRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPContactGroupRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPContactGroupRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPContactGroupRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetContactGroup(Guid contactGroupId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPContactGroupRepository iERPContactGroupRepository = (base.ERPContactGroupRepository = new ERPContactGroupRepository(base.ApiClientContext));
		using (iERPContactGroupRepository)
		{
			if (!(await base.ERPContactGroupRepository.DoesContactGroupExist(contactGroupId)))
			{
				errorsList.Add($"ContactGroup [{contactGroupId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutContactGroup(ERPContactGroupDto contactGroup)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPContactGroupRepository iERPContactGroupRepository = (base.ERPContactGroupRepository = new ERPContactGroupRepository(base.ApiClientContext));
		using (iERPContactGroupRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPContactGroupDto>>> Process_GetAllContactGroups(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPContactGroupDto> allContactGroupsDto = new List<ERPContactGroupDto>();
		ERPResponseMessageDto<IList<ERPContactGroupDto>> result;
		try
		{
			IERPContactGroupRepository iERPContactGroupRepository = (base.ERPContactGroupRepository = new ERPContactGroupRepository(base.ApiClientContext));
			using (iERPContactGroupRepository)
			{
				foreach (ERPContactGroupInformationDto item2 in await base.ERPContactGroupRepository.GetAllContactGroups(pageSize, pageNumber, filter, orderBy))
				{
					ERPContactGroupDto item = new ERPContactGroupDto
					{
						cmgContactGroupID = item2.cmgContactGroupID,
						cmgCreatedBy = item2.cmgCreatedBy,
						cmgCreatedDate = item2.cmgCreatedDate,
						cmgDescription = item2.cmgDescription,
						cmgUniqueID = item2.cmgUniqueID,
						cmgRowVersion = item2.cmgRowVersion,
						CustomFields = item2.CustomFields
					};
					allContactGroupsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ContactGroups]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPContactGroupDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allContactGroupsDto,
				RecordCount = allContactGroupsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPContactGroupDto>> Process_GetContactGroup(Guid contactGroupId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPContactGroupDto contactGroupDto = null;
		ERPResponseMessageDto<ERPContactGroupDto> result;
		try
		{
			IERPContactGroupRepository iERPContactGroupRepository = (base.ERPContactGroupRepository = new ERPContactGroupRepository(base.ApiClientContext));
			using (iERPContactGroupRepository)
			{
				ERPContactGroupInformationDto eRPContactGroupInformationDto = await base.ERPContactGroupRepository.GetContactGroup(contactGroupId);
				contactGroupDto = new ERPContactGroupDto
				{
					cmgContactGroupID = eRPContactGroupInformationDto.cmgContactGroupID,
					cmgCreatedBy = eRPContactGroupInformationDto.cmgCreatedBy,
					cmgCreatedDate = eRPContactGroupInformationDto.cmgCreatedDate,
					cmgDescription = eRPContactGroupInformationDto.cmgDescription,
					cmgUniqueID = eRPContactGroupInformationDto.cmgUniqueID,
					cmgRowVersion = eRPContactGroupInformationDto.cmgRowVersion,
					CustomFields = eRPContactGroupInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ContactGroups []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPContactGroupDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = contactGroupDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPContactGroupDto>> Process_PutContactGroup(ERPContactGroupDto contactGroup)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPContactGroupDto createdObject = null;
		ERPResponseMessageDto<ERPContactGroupDto> result;
		try
		{
			IERPContactGroupRepository iERPContactGroupRepository = (base.ERPContactGroupRepository = new ERPContactGroupRepository(base.ApiClientContext));
			using (iERPContactGroupRepository)
			{
				APIValidationInfoDto postResult = await base.ERPContactGroupRepository.SaveContactGroup(contactGroup);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPContactGroupInformationDto eRPContactGroupInformationDto = await base.ERPContactGroupRepository.GetContactGroup(contactGroup.cmgUniqueID);
					createdObject = new ERPContactGroupDto
					{
						cmgContactGroupID = eRPContactGroupInformationDto.cmgContactGroupID,
						cmgCreatedBy = eRPContactGroupInformationDto.cmgCreatedBy,
						cmgCreatedDate = eRPContactGroupInformationDto.cmgCreatedDate,
						cmgDescription = eRPContactGroupInformationDto.cmgDescription,
						cmgUniqueID = eRPContactGroupInformationDto.cmgUniqueID,
						cmgRowVersion = eRPContactGroupInformationDto.cmgRowVersion,
						CustomFields = eRPContactGroupInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ContactGroup [{contactGroup.cmgUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPContactGroupDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteContactGroup(Guid contactGroupId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPContactGroupRepository iERPContactGroupRepository = (base.ERPContactGroupRepository = new ERPContactGroupRepository(base.ApiClientContext));
		using (iERPContactGroupRepository)
		{
			if (!(await base.ERPContactGroupRepository.DoesContactGroupExist(contactGroupId)))
			{
				base.ErrorsList.Add($"ContactGroup [{contactGroupId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPContactGroupInformationDto eRPContactGroupInformationDto = await base.ERPContactGroupRepository.GetContactGroup(contactGroupId);
				string text = await base.ERPContactGroupRepository.WhereUsed("ContactGroups", new object[1] { eRPContactGroupInformationDto.cmgContactGroupID }, new object[1] { "cmgContactGroupID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ContactGroup cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPContactGroupDto>> Process_DeleteContactGroup(Guid contactGroupId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPContactGroupDto> result;
		try
		{
			IERPContactGroupRepository iERPContactGroupRepository = (base.ERPContactGroupRepository = new ERPContactGroupRepository(base.ApiClientContext));
			using (iERPContactGroupRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPContactGroupRepository.DeleteRowFromTable("ContactGroups", "cmg", contactGroupId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ContactGroup [{contactGroupId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPContactGroupDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPContactGroupDto()
			};
		}
		return result;
	}
}
