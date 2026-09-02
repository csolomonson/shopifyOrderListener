using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPOrganizationContactModel : ERPBaseModel, IERPOrganizationContactModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizationContacts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPOrganizationContactRepository iERPOrganizationContactRepository = (base.ERPOrganizationContactRepository = new ERPOrganizationContactRepository(base.ApiClientContext));
		using (iERPOrganizationContactRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPOrganizationContactRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPOrganizationContactRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPOrganizationContactRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPOrganizationContactRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetOrganizationContact(Guid organizationContactId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationContactRepository iERPOrganizationContactRepository = (base.ERPOrganizationContactRepository = new ERPOrganizationContactRepository(base.ApiClientContext));
		using (iERPOrganizationContactRepository)
		{
			if (!(await base.ERPOrganizationContactRepository.DoesOrganizationContactExist(organizationContactId)))
			{
				errorsList.Add($"OrganizationContact [{organizationContactId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutOrganizationContact(ERPOrganizationContactDto organizationContact)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationContactRepository iERPOrganizationContactRepository = (base.ERPOrganizationContactRepository = new ERPOrganizationContactRepository(base.ApiClientContext));
		using (iERPOrganizationContactRepository)
		{
			if (!string.IsNullOrWhiteSpace(organizationContact.cmcOrganizationID) && !(await base.ERPOrganizationContactRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { organizationContact.cmcOrganizationID })))
			{
				errorsList.Add("cmcOrganizationID [" + organizationContact.cmcOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationContact.cmcLocationID) && !(await base.ERPOrganizationContactRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organizationContact.cmcOrganizationID, organizationContact.cmcLocationID })))
			{
				errorsList.Add("cmcLocationID [" + organizationContact.cmcLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationContact.cmcContactTitleID) && !(await base.ERPOrganizationContactRepository.DoesRecordExistInTableUsingKeys("ContactTitles", new object[1] { "CMECONTACTTITLEID" }, new object[1] { organizationContact.cmcContactTitleID })))
			{
				errorsList.Add("cmcContactTitleID [" + organizationContact.cmcContactTitleID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPOrganizationContactDto>>> Process_GetAllOrganizationContacts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPOrganizationContactDto> allOrganizationContactsDto = new List<ERPOrganizationContactDto>();
		ERPResponseMessageDto<IList<ERPOrganizationContactDto>> result;
		try
		{
			IERPOrganizationContactRepository iERPOrganizationContactRepository = (base.ERPOrganizationContactRepository = new ERPOrganizationContactRepository(base.ApiClientContext));
			using (iERPOrganizationContactRepository)
			{
				foreach (ERPOrganizationContactInformationDto item2 in await base.ERPOrganizationContactRepository.GetAllOrganizationContacts(pageSize, pageNumber, filter, orderBy))
				{
					ERPOrganizationContactDto item = new ERPOrganizationContactDto
					{
						cmcAlternatePhoneNumber = item2.cmcAlternatePhoneNumber,
						cmcContactID = item2.cmcContactID,
						cmcContactTitleID = item2.cmcContactTitleID,
						cmcCorrespondenceMethod = item2.cmcCorrespondenceMethod,
						cmcCreatedBy = item2.cmcCreatedBy,
						cmcCreatedDate = item2.cmcCreatedDate,
						cmcEmailAddress = item2.cmcEmailAddress,
						cmcUniqueID = item2.cmcUniqueID,
						cmcFaxNumber = item2.cmcFaxNumber,
						cmcInactiveDate = item2.cmcInactiveDate,
						cmcInactive = item2.cmcInactive,
						cmcCreatedFromMobile = item2.cmcCreatedFromMobile,
						cmcNoMailings = item2.cmcNoMailings,
						cmcLocationID = item2.cmcLocationID,
						cmcMobileNumber = item2.cmcMobileNumber,
						cmcName = item2.cmcName,
						cmcNoteRtf = item2.cmcNoteRtf,
						cmcNoteText = item2.cmcNoteText,
						cmcOrganizationID = item2.cmcOrganizationID,
						cmcPhoneNumber = item2.cmcPhoneNumber,
						cmcRowVersion = item2.cmcRowVersion,
						CustomFields = item2.CustomFields
					};
					allOrganizationContactsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all OrganizationContacts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPOrganizationContactDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allOrganizationContactsDto,
				RecordCount = allOrganizationContactsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationContactDto>> Process_GetOrganizationContact(Guid organizationContactId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPOrganizationContactDto organizationContactDto = null;
		ERPResponseMessageDto<ERPOrganizationContactDto> result;
		try
		{
			IERPOrganizationContactRepository iERPOrganizationContactRepository = (base.ERPOrganizationContactRepository = new ERPOrganizationContactRepository(base.ApiClientContext));
			using (iERPOrganizationContactRepository)
			{
				ERPOrganizationContactInformationDto eRPOrganizationContactInformationDto = await base.ERPOrganizationContactRepository.GetOrganizationContact(organizationContactId);
				organizationContactDto = new ERPOrganizationContactDto
				{
					cmcAlternatePhoneNumber = eRPOrganizationContactInformationDto.cmcAlternatePhoneNumber,
					cmcContactID = eRPOrganizationContactInformationDto.cmcContactID,
					cmcContactTitleID = eRPOrganizationContactInformationDto.cmcContactTitleID,
					cmcCorrespondenceMethod = eRPOrganizationContactInformationDto.cmcCorrespondenceMethod,
					cmcCreatedBy = eRPOrganizationContactInformationDto.cmcCreatedBy,
					cmcCreatedDate = eRPOrganizationContactInformationDto.cmcCreatedDate,
					cmcEmailAddress = eRPOrganizationContactInformationDto.cmcEmailAddress,
					cmcUniqueID = eRPOrganizationContactInformationDto.cmcUniqueID,
					cmcFaxNumber = eRPOrganizationContactInformationDto.cmcFaxNumber,
					cmcInactiveDate = eRPOrganizationContactInformationDto.cmcInactiveDate,
					cmcInactive = eRPOrganizationContactInformationDto.cmcInactive,
					cmcCreatedFromMobile = eRPOrganizationContactInformationDto.cmcCreatedFromMobile,
					cmcNoMailings = eRPOrganizationContactInformationDto.cmcNoMailings,
					cmcLocationID = eRPOrganizationContactInformationDto.cmcLocationID,
					cmcMobileNumber = eRPOrganizationContactInformationDto.cmcMobileNumber,
					cmcName = eRPOrganizationContactInformationDto.cmcName,
					cmcNoteRtf = eRPOrganizationContactInformationDto.cmcNoteRtf,
					cmcNoteText = eRPOrganizationContactInformationDto.cmcNoteText,
					cmcOrganizationID = eRPOrganizationContactInformationDto.cmcOrganizationID,
					cmcPhoneNumber = eRPOrganizationContactInformationDto.cmcPhoneNumber,
					cmcRowVersion = eRPOrganizationContactInformationDto.cmcRowVersion,
					CustomFields = eRPOrganizationContactInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the OrganizationContacts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationContactDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = organizationContactDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationContactDto>> Process_PutOrganizationContact(ERPOrganizationContactDto organizationContact)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPOrganizationContactDto createdObject = null;
		ERPResponseMessageDto<ERPOrganizationContactDto> result;
		try
		{
			IERPOrganizationContactRepository iERPOrganizationContactRepository = (base.ERPOrganizationContactRepository = new ERPOrganizationContactRepository(base.ApiClientContext));
			using (iERPOrganizationContactRepository)
			{
				APIValidationInfoDto postResult = await base.ERPOrganizationContactRepository.SaveOrganizationContact(organizationContact);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPOrganizationContactInformationDto eRPOrganizationContactInformationDto = await base.ERPOrganizationContactRepository.GetOrganizationContact(organizationContact.cmcUniqueID);
					createdObject = new ERPOrganizationContactDto
					{
						cmcAlternatePhoneNumber = eRPOrganizationContactInformationDto.cmcAlternatePhoneNumber,
						cmcContactID = eRPOrganizationContactInformationDto.cmcContactID,
						cmcContactTitleID = eRPOrganizationContactInformationDto.cmcContactTitleID,
						cmcCorrespondenceMethod = eRPOrganizationContactInformationDto.cmcCorrespondenceMethod,
						cmcCreatedBy = eRPOrganizationContactInformationDto.cmcCreatedBy,
						cmcCreatedDate = eRPOrganizationContactInformationDto.cmcCreatedDate,
						cmcEmailAddress = eRPOrganizationContactInformationDto.cmcEmailAddress,
						cmcUniqueID = eRPOrganizationContactInformationDto.cmcUniqueID,
						cmcFaxNumber = eRPOrganizationContactInformationDto.cmcFaxNumber,
						cmcInactiveDate = eRPOrganizationContactInformationDto.cmcInactiveDate,
						cmcInactive = eRPOrganizationContactInformationDto.cmcInactive,
						cmcCreatedFromMobile = eRPOrganizationContactInformationDto.cmcCreatedFromMobile,
						cmcNoMailings = eRPOrganizationContactInformationDto.cmcNoMailings,
						cmcLocationID = eRPOrganizationContactInformationDto.cmcLocationID,
						cmcMobileNumber = eRPOrganizationContactInformationDto.cmcMobileNumber,
						cmcName = eRPOrganizationContactInformationDto.cmcName,
						cmcNoteRtf = eRPOrganizationContactInformationDto.cmcNoteRtf,
						cmcNoteText = eRPOrganizationContactInformationDto.cmcNoteText,
						cmcOrganizationID = eRPOrganizationContactInformationDto.cmcOrganizationID,
						cmcPhoneNumber = eRPOrganizationContactInformationDto.cmcPhoneNumber,
						cmcRowVersion = eRPOrganizationContactInformationDto.cmcRowVersion,
						CustomFields = eRPOrganizationContactInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing OrganizationContact [{organizationContact.cmcUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationContactDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteOrganizationContact(Guid organizationContactId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationContactRepository iERPOrganizationContactRepository = (base.ERPOrganizationContactRepository = new ERPOrganizationContactRepository(base.ApiClientContext));
		using (iERPOrganizationContactRepository)
		{
			if (!(await base.ERPOrganizationContactRepository.DoesOrganizationContactExist(organizationContactId)))
			{
				base.ErrorsList.Add($"OrganizationContact [{organizationContactId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPOrganizationContactInformationDto eRPOrganizationContactInformationDto = await base.ERPOrganizationContactRepository.GetOrganizationContact(organizationContactId);
				string text = await base.ERPOrganizationContactRepository.WhereUsed("OrganizationContacts", new object[3] { eRPOrganizationContactInformationDto.cmcOrganizationID, eRPOrganizationContactInformationDto.cmcLocationID, eRPOrganizationContactInformationDto.cmcContactID }, new object[3] { "cmcOrganizationID", "cmcLocationID", "cmcContactID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("OrganizationContact cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationContactDto>> Process_DeleteOrganizationContact(Guid organizationContactId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPOrganizationContactDto> result;
		try
		{
			IERPOrganizationContactRepository iERPOrganizationContactRepository = (base.ERPOrganizationContactRepository = new ERPOrganizationContactRepository(base.ApiClientContext));
			using (iERPOrganizationContactRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPOrganizationContactRepository.DeleteRowFromTable("OrganizationContacts", "cmc", organizationContactId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of OrganizationContact [{organizationContactId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationContactDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPOrganizationContactDto()
			};
		}
		return result;
	}
}
