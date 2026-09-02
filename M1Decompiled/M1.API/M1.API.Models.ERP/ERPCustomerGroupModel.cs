using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPCustomerGroupModel : ERPBaseModel, IERPCustomerGroupModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllCustomerGroups(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCustomerGroupRepository iERPCustomerGroupRepository = (base.ERPCustomerGroupRepository = new ERPCustomerGroupRepository(base.ApiClientContext));
		using (iERPCustomerGroupRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPCustomerGroupRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPCustomerGroupRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPCustomerGroupRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPCustomerGroupRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetCustomerGroup(Guid customerGroupId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCustomerGroupRepository iERPCustomerGroupRepository = (base.ERPCustomerGroupRepository = new ERPCustomerGroupRepository(base.ApiClientContext));
		using (iERPCustomerGroupRepository)
		{
			if (!(await base.ERPCustomerGroupRepository.DoesCustomerGroupExist(customerGroupId)))
			{
				errorsList.Add($"CustomerGroup [{customerGroupId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutCustomerGroup(ERPCustomerGroupDto customerGroup)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCustomerGroupRepository iERPCustomerGroupRepository = (base.ERPCustomerGroupRepository = new ERPCustomerGroupRepository(base.ApiClientContext));
		using (iERPCustomerGroupRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPCustomerGroupDto>>> Process_GetAllCustomerGroups(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPCustomerGroupDto> allCustomerGroupsDto = new List<ERPCustomerGroupDto>();
		ERPResponseMessageDto<IList<ERPCustomerGroupDto>> result;
		try
		{
			IERPCustomerGroupRepository iERPCustomerGroupRepository = (base.ERPCustomerGroupRepository = new ERPCustomerGroupRepository(base.ApiClientContext));
			using (iERPCustomerGroupRepository)
			{
				foreach (ERPCustomerGroupInformationDto item2 in await base.ERPCustomerGroupRepository.GetAllCustomerGroups(pageSize, pageNumber, filter, orderBy))
				{
					ERPCustomerGroupDto item = new ERPCustomerGroupDto
					{
						cmuCustomerGroupID = item2.cmuCustomerGroupID,
						cmuCreatedBy = item2.cmuCreatedBy,
						cmuCreatedDate = item2.cmuCreatedDate,
						cmuDescription = item2.cmuDescription,
						cmuUniqueID = item2.cmuUniqueID,
						cmuRowVersion = item2.cmuRowVersion,
						CustomFields = item2.CustomFields
					};
					allCustomerGroupsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all CustomerGroups]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPCustomerGroupDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allCustomerGroupsDto,
				RecordCount = allCustomerGroupsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCustomerGroupDto>> Process_GetCustomerGroup(Guid customerGroupId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPCustomerGroupDto customerGroupDto = null;
		ERPResponseMessageDto<ERPCustomerGroupDto> result;
		try
		{
			IERPCustomerGroupRepository iERPCustomerGroupRepository = (base.ERPCustomerGroupRepository = new ERPCustomerGroupRepository(base.ApiClientContext));
			using (iERPCustomerGroupRepository)
			{
				ERPCustomerGroupInformationDto eRPCustomerGroupInformationDto = await base.ERPCustomerGroupRepository.GetCustomerGroup(customerGroupId);
				customerGroupDto = new ERPCustomerGroupDto
				{
					cmuCustomerGroupID = eRPCustomerGroupInformationDto.cmuCustomerGroupID,
					cmuCreatedBy = eRPCustomerGroupInformationDto.cmuCreatedBy,
					cmuCreatedDate = eRPCustomerGroupInformationDto.cmuCreatedDate,
					cmuDescription = eRPCustomerGroupInformationDto.cmuDescription,
					cmuUniqueID = eRPCustomerGroupInformationDto.cmuUniqueID,
					cmuRowVersion = eRPCustomerGroupInformationDto.cmuRowVersion,
					CustomFields = eRPCustomerGroupInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the CustomerGroups []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCustomerGroupDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = customerGroupDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCustomerGroupDto>> Process_PutCustomerGroup(ERPCustomerGroupDto customerGroup)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPCustomerGroupDto createdObject = null;
		ERPResponseMessageDto<ERPCustomerGroupDto> result;
		try
		{
			IERPCustomerGroupRepository iERPCustomerGroupRepository = (base.ERPCustomerGroupRepository = new ERPCustomerGroupRepository(base.ApiClientContext));
			using (iERPCustomerGroupRepository)
			{
				APIValidationInfoDto postResult = await base.ERPCustomerGroupRepository.SaveCustomerGroup(customerGroup);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPCustomerGroupInformationDto eRPCustomerGroupInformationDto = await base.ERPCustomerGroupRepository.GetCustomerGroup(customerGroup.cmuUniqueID);
					createdObject = new ERPCustomerGroupDto
					{
						cmuCustomerGroupID = eRPCustomerGroupInformationDto.cmuCustomerGroupID,
						cmuCreatedBy = eRPCustomerGroupInformationDto.cmuCreatedBy,
						cmuCreatedDate = eRPCustomerGroupInformationDto.cmuCreatedDate,
						cmuDescription = eRPCustomerGroupInformationDto.cmuDescription,
						cmuUniqueID = eRPCustomerGroupInformationDto.cmuUniqueID,
						cmuRowVersion = eRPCustomerGroupInformationDto.cmuRowVersion,
						CustomFields = eRPCustomerGroupInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing CustomerGroup [{customerGroup.cmuUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCustomerGroupDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteCustomerGroup(Guid customerGroupId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCustomerGroupRepository iERPCustomerGroupRepository = (base.ERPCustomerGroupRepository = new ERPCustomerGroupRepository(base.ApiClientContext));
		using (iERPCustomerGroupRepository)
		{
			if (!(await base.ERPCustomerGroupRepository.DoesCustomerGroupExist(customerGroupId)))
			{
				base.ErrorsList.Add($"CustomerGroup [{customerGroupId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPCustomerGroupInformationDto eRPCustomerGroupInformationDto = await base.ERPCustomerGroupRepository.GetCustomerGroup(customerGroupId);
				string text = await base.ERPCustomerGroupRepository.WhereUsed("CustomerGroups", new object[1] { eRPCustomerGroupInformationDto.cmuCustomerGroupID }, new object[1] { "cmuCustomerGroupID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("CustomerGroup cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPCustomerGroupDto>> Process_DeleteCustomerGroup(Guid customerGroupId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPCustomerGroupDto> result;
		try
		{
			IERPCustomerGroupRepository iERPCustomerGroupRepository = (base.ERPCustomerGroupRepository = new ERPCustomerGroupRepository(base.ApiClientContext));
			using (iERPCustomerGroupRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPCustomerGroupRepository.DeleteRowFromTable("CustomerGroups", "cmu", customerGroupId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of CustomerGroup [{customerGroupId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCustomerGroupDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPCustomerGroupDto()
			};
		}
		return result;
	}
}
