using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPOrganizationIndustryTypeLinkModel : ERPBaseModel, IERPOrganizationIndustryTypeLinkModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizationIndustryTypeLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPOrganizationIndustryTypeLinkRepository iERPOrganizationIndustryTypeLinkRepository = (base.ERPOrganizationIndustryTypeLinkRepository = new ERPOrganizationIndustryTypeLinkRepository(base.ApiClientContext));
		using (iERPOrganizationIndustryTypeLinkRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPOrganizationIndustryTypeLinkRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPOrganizationIndustryTypeLinkRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPOrganizationIndustryTypeLinkRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPOrganizationIndustryTypeLinkRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetOrganizationIndustryTypeLink(Guid organizationIndustryTypeLinkId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationIndustryTypeLinkRepository iERPOrganizationIndustryTypeLinkRepository = (base.ERPOrganizationIndustryTypeLinkRepository = new ERPOrganizationIndustryTypeLinkRepository(base.ApiClientContext));
		using (iERPOrganizationIndustryTypeLinkRepository)
		{
			if (!(await base.ERPOrganizationIndustryTypeLinkRepository.DoesOrganizationIndustryTypeLinkExist(organizationIndustryTypeLinkId)))
			{
				errorsList.Add($"OrganizationIndustryTypeLink [{organizationIndustryTypeLinkId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutOrganizationIndustryTypeLink(ERPOrganizationIndustryTypeLinkDto organizationIndustryTypeLink)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationIndustryTypeLinkRepository iERPOrganizationIndustryTypeLinkRepository = (base.ERPOrganizationIndustryTypeLinkRepository = new ERPOrganizationIndustryTypeLinkRepository(base.ApiClientContext));
		using (iERPOrganizationIndustryTypeLinkRepository)
		{
			if (!string.IsNullOrWhiteSpace(organizationIndustryTypeLink.cmdOrganizationID) && !(await base.ERPOrganizationIndustryTypeLinkRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { organizationIndustryTypeLink.cmdOrganizationID })))
			{
				errorsList.Add("cmdOrganizationID [" + organizationIndustryTypeLink.cmdOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationIndustryTypeLink.cmdIndustryTypeID) && !(await base.ERPOrganizationIndustryTypeLinkRepository.DoesRecordExistInTableUsingKeys("IndustryTypes", new object[1] { "CMIINDUSTRYTYPEID" }, new object[1] { organizationIndustryTypeLink.cmdIndustryTypeID })))
			{
				errorsList.Add("cmdIndustryTypeID [" + organizationIndustryTypeLink.cmdIndustryTypeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPOrganizationIndustryTypeLinkDto>>> Process_GetAllOrganizationIndustryTypeLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPOrganizationIndustryTypeLinkDto> allOrganizationIndustryTypeLinksDto = new List<ERPOrganizationIndustryTypeLinkDto>();
		ERPResponseMessageDto<IList<ERPOrganizationIndustryTypeLinkDto>> result;
		try
		{
			IERPOrganizationIndustryTypeLinkRepository iERPOrganizationIndustryTypeLinkRepository = (base.ERPOrganizationIndustryTypeLinkRepository = new ERPOrganizationIndustryTypeLinkRepository(base.ApiClientContext));
			using (iERPOrganizationIndustryTypeLinkRepository)
			{
				foreach (ERPOrganizationIndustryTypeLinkInformationDto item2 in await base.ERPOrganizationIndustryTypeLinkRepository.GetAllOrganizationIndustryTypeLinks(pageSize, pageNumber, filter, orderBy))
				{
					ERPOrganizationIndustryTypeLinkDto item = new ERPOrganizationIndustryTypeLinkDto
					{
						cmdCreatedBy = item2.cmdCreatedBy,
						cmdCreatedDate = item2.cmdCreatedDate,
						cmdUniqueID = item2.cmdUniqueID,
						cmdIndustryTypeID = item2.cmdIndustryTypeID,
						cmdIndustryTypeLinkID = item2.cmdIndustryTypeLinkID,
						cmdOrganizationID = item2.cmdOrganizationID,
						cmdRowVersion = item2.cmdRowVersion,
						CustomFields = item2.CustomFields
					};
					allOrganizationIndustryTypeLinksDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all OrganizationIndustryTypeLinks]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPOrganizationIndustryTypeLinkDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allOrganizationIndustryTypeLinksDto,
				RecordCount = allOrganizationIndustryTypeLinksDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationIndustryTypeLinkDto>> Process_GetOrganizationIndustryTypeLink(Guid organizationIndustryTypeLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPOrganizationIndustryTypeLinkDto organizationIndustryTypeLinkDto = null;
		ERPResponseMessageDto<ERPOrganizationIndustryTypeLinkDto> result;
		try
		{
			IERPOrganizationIndustryTypeLinkRepository iERPOrganizationIndustryTypeLinkRepository = (base.ERPOrganizationIndustryTypeLinkRepository = new ERPOrganizationIndustryTypeLinkRepository(base.ApiClientContext));
			using (iERPOrganizationIndustryTypeLinkRepository)
			{
				ERPOrganizationIndustryTypeLinkInformationDto eRPOrganizationIndustryTypeLinkInformationDto = await base.ERPOrganizationIndustryTypeLinkRepository.GetOrganizationIndustryTypeLink(organizationIndustryTypeLinkId);
				organizationIndustryTypeLinkDto = new ERPOrganizationIndustryTypeLinkDto
				{
					cmdCreatedBy = eRPOrganizationIndustryTypeLinkInformationDto.cmdCreatedBy,
					cmdCreatedDate = eRPOrganizationIndustryTypeLinkInformationDto.cmdCreatedDate,
					cmdUniqueID = eRPOrganizationIndustryTypeLinkInformationDto.cmdUniqueID,
					cmdIndustryTypeID = eRPOrganizationIndustryTypeLinkInformationDto.cmdIndustryTypeID,
					cmdIndustryTypeLinkID = eRPOrganizationIndustryTypeLinkInformationDto.cmdIndustryTypeLinkID,
					cmdOrganizationID = eRPOrganizationIndustryTypeLinkInformationDto.cmdOrganizationID,
					cmdRowVersion = eRPOrganizationIndustryTypeLinkInformationDto.cmdRowVersion,
					CustomFields = eRPOrganizationIndustryTypeLinkInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the OrganizationIndustryTypeLinks []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationIndustryTypeLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = organizationIndustryTypeLinkDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationIndustryTypeLinkDto>> Process_PutOrganizationIndustryTypeLink(ERPOrganizationIndustryTypeLinkDto organizationIndustryTypeLink)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPOrganizationIndustryTypeLinkDto createdObject = null;
		ERPResponseMessageDto<ERPOrganizationIndustryTypeLinkDto> result;
		try
		{
			IERPOrganizationIndustryTypeLinkRepository iERPOrganizationIndustryTypeLinkRepository = (base.ERPOrganizationIndustryTypeLinkRepository = new ERPOrganizationIndustryTypeLinkRepository(base.ApiClientContext));
			using (iERPOrganizationIndustryTypeLinkRepository)
			{
				APIValidationInfoDto postResult = await base.ERPOrganizationIndustryTypeLinkRepository.SaveOrganizationIndustryTypeLink(organizationIndustryTypeLink);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPOrganizationIndustryTypeLinkInformationDto eRPOrganizationIndustryTypeLinkInformationDto = await base.ERPOrganizationIndustryTypeLinkRepository.GetOrganizationIndustryTypeLink(organizationIndustryTypeLink.cmdUniqueID);
					createdObject = new ERPOrganizationIndustryTypeLinkDto
					{
						cmdCreatedBy = eRPOrganizationIndustryTypeLinkInformationDto.cmdCreatedBy,
						cmdCreatedDate = eRPOrganizationIndustryTypeLinkInformationDto.cmdCreatedDate,
						cmdUniqueID = eRPOrganizationIndustryTypeLinkInformationDto.cmdUniqueID,
						cmdIndustryTypeID = eRPOrganizationIndustryTypeLinkInformationDto.cmdIndustryTypeID,
						cmdIndustryTypeLinkID = eRPOrganizationIndustryTypeLinkInformationDto.cmdIndustryTypeLinkID,
						cmdOrganizationID = eRPOrganizationIndustryTypeLinkInformationDto.cmdOrganizationID,
						cmdRowVersion = eRPOrganizationIndustryTypeLinkInformationDto.cmdRowVersion,
						CustomFields = eRPOrganizationIndustryTypeLinkInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing OrganizationIndustryTypeLink [{organizationIndustryTypeLink.cmdUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationIndustryTypeLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteOrganizationIndustryTypeLink(Guid organizationIndustryTypeLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationIndustryTypeLinkRepository iERPOrganizationIndustryTypeLinkRepository = (base.ERPOrganizationIndustryTypeLinkRepository = new ERPOrganizationIndustryTypeLinkRepository(base.ApiClientContext));
		using (iERPOrganizationIndustryTypeLinkRepository)
		{
			if (!(await base.ERPOrganizationIndustryTypeLinkRepository.DoesOrganizationIndustryTypeLinkExist(organizationIndustryTypeLinkId)))
			{
				base.ErrorsList.Add($"OrganizationIndustryTypeLink [{organizationIndustryTypeLinkId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPOrganizationIndustryTypeLinkInformationDto eRPOrganizationIndustryTypeLinkInformationDto = await base.ERPOrganizationIndustryTypeLinkRepository.GetOrganizationIndustryTypeLink(organizationIndustryTypeLinkId);
				string text = await base.ERPOrganizationIndustryTypeLinkRepository.WhereUsed("OrganizationIndustryTypeLinks", new object[2] { eRPOrganizationIndustryTypeLinkInformationDto.cmdOrganizationID, eRPOrganizationIndustryTypeLinkInformationDto.cmdIndustryTypeLinkID }, new object[2] { "cmdOrganizationID", "cmdIndustryTypeLinkID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("OrganizationIndustryTypeLink cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationIndustryTypeLinkDto>> Process_DeleteOrganizationIndustryTypeLink(Guid organizationIndustryTypeLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPOrganizationIndustryTypeLinkDto> result;
		try
		{
			IERPOrganizationIndustryTypeLinkRepository iERPOrganizationIndustryTypeLinkRepository = (base.ERPOrganizationIndustryTypeLinkRepository = new ERPOrganizationIndustryTypeLinkRepository(base.ApiClientContext));
			using (iERPOrganizationIndustryTypeLinkRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPOrganizationIndustryTypeLinkRepository.DeleteRowFromTable("OrganizationIndustryTypeLinks", "cmd", organizationIndustryTypeLinkId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of OrganizationIndustryTypeLink [{organizationIndustryTypeLinkId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationIndustryTypeLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPOrganizationIndustryTypeLinkDto()
			};
		}
		return result;
	}
}
