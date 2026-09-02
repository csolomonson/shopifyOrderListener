using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPOrganizationContactGroupLinkModel : ERPBaseModel, IERPOrganizationContactGroupLinkModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizationContactGroupLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPOrganizationContactGroupLinkRepository iERPOrganizationContactGroupLinkRepository = (base.ERPOrganizationContactGroupLinkRepository = new ERPOrganizationContactGroupLinkRepository(base.ApiClientContext));
		using (iERPOrganizationContactGroupLinkRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPOrganizationContactGroupLinkRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPOrganizationContactGroupLinkRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPOrganizationContactGroupLinkRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPOrganizationContactGroupLinkRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetOrganizationContactGroupLink(Guid organizationContactGroupLinkId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationContactGroupLinkRepository iERPOrganizationContactGroupLinkRepository = (base.ERPOrganizationContactGroupLinkRepository = new ERPOrganizationContactGroupLinkRepository(base.ApiClientContext));
		using (iERPOrganizationContactGroupLinkRepository)
		{
			if (!(await base.ERPOrganizationContactGroupLinkRepository.DoesOrganizationContactGroupLinkExist(organizationContactGroupLinkId)))
			{
				errorsList.Add($"OrganizationContactGroupLink [{organizationContactGroupLinkId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutOrganizationContactGroupLink(ERPOrganizationContactGroupLinkDto organizationContactGroupLink)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationContactGroupLinkRepository iERPOrganizationContactGroupLinkRepository = (base.ERPOrganizationContactGroupLinkRepository = new ERPOrganizationContactGroupLinkRepository(base.ApiClientContext));
		using (iERPOrganizationContactGroupLinkRepository)
		{
			if (!string.IsNullOrWhiteSpace(organizationContactGroupLink.cmrOrganizationID) && !(await base.ERPOrganizationContactGroupLinkRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { organizationContactGroupLink.cmrOrganizationID })))
			{
				errorsList.Add("cmrOrganizationID [" + organizationContactGroupLink.cmrOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationContactGroupLink.cmrLocationID) && !(await base.ERPOrganizationContactGroupLinkRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organizationContactGroupLink.cmrOrganizationID, organizationContactGroupLink.cmrLocationID })))
			{
				errorsList.Add("cmrLocationID [" + organizationContactGroupLink.cmrLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationContactGroupLink.cmrContactID) && !(await base.ERPOrganizationContactGroupLinkRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { organizationContactGroupLink.cmrOrganizationID, organizationContactGroupLink.cmrLocationID, organizationContactGroupLink.cmrContactID })))
			{
				errorsList.Add("cmrContactID [" + organizationContactGroupLink.cmrContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationContactGroupLink.cmrContactGroupID) && !(await base.ERPOrganizationContactGroupLinkRepository.DoesRecordExistInTableUsingKeys("ContactGroups", new object[1] { "CMGCONTACTGROUPID" }, new object[1] { organizationContactGroupLink.cmrContactGroupID })))
			{
				errorsList.Add("cmrContactGroupID [" + organizationContactGroupLink.cmrContactGroupID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPOrganizationContactGroupLinkDto>>> Process_GetAllOrganizationContactGroupLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPOrganizationContactGroupLinkDto> allOrganizationContactGroupLinksDto = new List<ERPOrganizationContactGroupLinkDto>();
		ERPResponseMessageDto<IList<ERPOrganizationContactGroupLinkDto>> result;
		try
		{
			IERPOrganizationContactGroupLinkRepository iERPOrganizationContactGroupLinkRepository = (base.ERPOrganizationContactGroupLinkRepository = new ERPOrganizationContactGroupLinkRepository(base.ApiClientContext));
			using (iERPOrganizationContactGroupLinkRepository)
			{
				foreach (ERPOrganizationContactGroupLinkInformationDto item2 in await base.ERPOrganizationContactGroupLinkRepository.GetAllOrganizationContactGroupLinks(pageSize, pageNumber, filter, orderBy))
				{
					ERPOrganizationContactGroupLinkDto item = new ERPOrganizationContactGroupLinkDto
					{
						cmrContactGroupID = item2.cmrContactGroupID,
						cmrContactGroupLinkID = item2.cmrContactGroupLinkID,
						cmrContactID = item2.cmrContactID,
						cmrCreatedBy = item2.cmrCreatedBy,
						cmrCreatedDate = item2.cmrCreatedDate,
						cmrUniqueID = item2.cmrUniqueID,
						cmrLocationID = item2.cmrLocationID,
						cmrOrganizationID = item2.cmrOrganizationID,
						cmrRowVersion = item2.cmrRowVersion,
						CustomFields = item2.CustomFields
					};
					allOrganizationContactGroupLinksDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all OrganizationContactGroupLinks]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPOrganizationContactGroupLinkDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allOrganizationContactGroupLinksDto,
				RecordCount = allOrganizationContactGroupLinksDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationContactGroupLinkDto>> Process_GetOrganizationContactGroupLink(Guid organizationContactGroupLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPOrganizationContactGroupLinkDto organizationContactGroupLinkDto = null;
		ERPResponseMessageDto<ERPOrganizationContactGroupLinkDto> result;
		try
		{
			IERPOrganizationContactGroupLinkRepository iERPOrganizationContactGroupLinkRepository = (base.ERPOrganizationContactGroupLinkRepository = new ERPOrganizationContactGroupLinkRepository(base.ApiClientContext));
			using (iERPOrganizationContactGroupLinkRepository)
			{
				ERPOrganizationContactGroupLinkInformationDto eRPOrganizationContactGroupLinkInformationDto = await base.ERPOrganizationContactGroupLinkRepository.GetOrganizationContactGroupLink(organizationContactGroupLinkId);
				organizationContactGroupLinkDto = new ERPOrganizationContactGroupLinkDto
				{
					cmrContactGroupID = eRPOrganizationContactGroupLinkInformationDto.cmrContactGroupID,
					cmrContactGroupLinkID = eRPOrganizationContactGroupLinkInformationDto.cmrContactGroupLinkID,
					cmrContactID = eRPOrganizationContactGroupLinkInformationDto.cmrContactID,
					cmrCreatedBy = eRPOrganizationContactGroupLinkInformationDto.cmrCreatedBy,
					cmrCreatedDate = eRPOrganizationContactGroupLinkInformationDto.cmrCreatedDate,
					cmrUniqueID = eRPOrganizationContactGroupLinkInformationDto.cmrUniqueID,
					cmrLocationID = eRPOrganizationContactGroupLinkInformationDto.cmrLocationID,
					cmrOrganizationID = eRPOrganizationContactGroupLinkInformationDto.cmrOrganizationID,
					cmrRowVersion = eRPOrganizationContactGroupLinkInformationDto.cmrRowVersion,
					CustomFields = eRPOrganizationContactGroupLinkInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the OrganizationContactGroupLinks []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationContactGroupLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = organizationContactGroupLinkDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationContactGroupLinkDto>> Process_PutOrganizationContactGroupLink(ERPOrganizationContactGroupLinkDto organizationContactGroupLink)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPOrganizationContactGroupLinkDto createdObject = null;
		ERPResponseMessageDto<ERPOrganizationContactGroupLinkDto> result;
		try
		{
			IERPOrganizationContactGroupLinkRepository iERPOrganizationContactGroupLinkRepository = (base.ERPOrganizationContactGroupLinkRepository = new ERPOrganizationContactGroupLinkRepository(base.ApiClientContext));
			using (iERPOrganizationContactGroupLinkRepository)
			{
				APIValidationInfoDto postResult = await base.ERPOrganizationContactGroupLinkRepository.SaveOrganizationContactGroupLink(organizationContactGroupLink);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPOrganizationContactGroupLinkInformationDto eRPOrganizationContactGroupLinkInformationDto = await base.ERPOrganizationContactGroupLinkRepository.GetOrganizationContactGroupLink(organizationContactGroupLink.cmrUniqueID);
					createdObject = new ERPOrganizationContactGroupLinkDto
					{
						cmrContactGroupID = eRPOrganizationContactGroupLinkInformationDto.cmrContactGroupID,
						cmrContactGroupLinkID = eRPOrganizationContactGroupLinkInformationDto.cmrContactGroupLinkID,
						cmrContactID = eRPOrganizationContactGroupLinkInformationDto.cmrContactID,
						cmrCreatedBy = eRPOrganizationContactGroupLinkInformationDto.cmrCreatedBy,
						cmrCreatedDate = eRPOrganizationContactGroupLinkInformationDto.cmrCreatedDate,
						cmrUniqueID = eRPOrganizationContactGroupLinkInformationDto.cmrUniqueID,
						cmrLocationID = eRPOrganizationContactGroupLinkInformationDto.cmrLocationID,
						cmrOrganizationID = eRPOrganizationContactGroupLinkInformationDto.cmrOrganizationID,
						cmrRowVersion = eRPOrganizationContactGroupLinkInformationDto.cmrRowVersion,
						CustomFields = eRPOrganizationContactGroupLinkInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing OrganizationContactGroupLink [{organizationContactGroupLink.cmrUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationContactGroupLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteOrganizationContactGroupLink(Guid organizationContactGroupLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationContactGroupLinkRepository iERPOrganizationContactGroupLinkRepository = (base.ERPOrganizationContactGroupLinkRepository = new ERPOrganizationContactGroupLinkRepository(base.ApiClientContext));
		using (iERPOrganizationContactGroupLinkRepository)
		{
			if (!(await base.ERPOrganizationContactGroupLinkRepository.DoesOrganizationContactGroupLinkExist(organizationContactGroupLinkId)))
			{
				base.ErrorsList.Add($"OrganizationContactGroupLink [{organizationContactGroupLinkId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPOrganizationContactGroupLinkInformationDto eRPOrganizationContactGroupLinkInformationDto = await base.ERPOrganizationContactGroupLinkRepository.GetOrganizationContactGroupLink(organizationContactGroupLinkId);
				string text = await base.ERPOrganizationContactGroupLinkRepository.WhereUsed("OrganizationContactGroupLinks", new object[4] { eRPOrganizationContactGroupLinkInformationDto.cmrOrganizationID, eRPOrganizationContactGroupLinkInformationDto.cmrLocationID, eRPOrganizationContactGroupLinkInformationDto.cmrContactID, eRPOrganizationContactGroupLinkInformationDto.cmrContactGroupLinkID }, new object[4] { "cmrOrganizationID", "cmrLocationID", "cmrContactID", "cmrContactGroupLinkID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("OrganizationContactGroupLink cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationContactGroupLinkDto>> Process_DeleteOrganizationContactGroupLink(Guid organizationContactGroupLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPOrganizationContactGroupLinkDto> result;
		try
		{
			IERPOrganizationContactGroupLinkRepository iERPOrganizationContactGroupLinkRepository = (base.ERPOrganizationContactGroupLinkRepository = new ERPOrganizationContactGroupLinkRepository(base.ApiClientContext));
			using (iERPOrganizationContactGroupLinkRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPOrganizationContactGroupLinkRepository.DeleteRowFromTable("OrganizationContactGroupLinks", "cmr", organizationContactGroupLinkId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of OrganizationContactGroupLink [{organizationContactGroupLinkId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationContactGroupLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPOrganizationContactGroupLinkDto()
			};
		}
		return result;
	}
}
