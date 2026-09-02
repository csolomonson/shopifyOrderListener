using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPProjectContactModel : ERPBaseModel, IERPProjectContactModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllProjectContacts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProjectContactRepository iERPProjectContactRepository = (base.ERPProjectContactRepository = new ERPProjectContactRepository(base.ApiClientContext));
		using (iERPProjectContactRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPProjectContactRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPProjectContactRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPProjectContactRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPProjectContactRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetProjectContact(Guid projectContactId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectContactRepository iERPProjectContactRepository = (base.ERPProjectContactRepository = new ERPProjectContactRepository(base.ApiClientContext));
		using (iERPProjectContactRepository)
		{
			if (!(await base.ERPProjectContactRepository.DoesProjectContactExist(projectContactId)))
			{
				errorsList.Add($"ProjectContact [{projectContactId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutProjectContact(ERPProjectContactDto projectContact)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectContactRepository iERPProjectContactRepository = (base.ERPProjectContactRepository = new ERPProjectContactRepository(base.ApiClientContext));
		using (iERPProjectContactRepository)
		{
			if (!string.IsNullOrWhiteSpace(projectContact.prcProjectID) && !(await base.ERPProjectContactRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { projectContact.prcProjectID })))
			{
				errorsList.Add("prcProjectID [" + projectContact.prcProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(projectContact.prcOrganizationID) && !(await base.ERPProjectContactRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { projectContact.prcOrganizationID })))
			{
				errorsList.Add("prcOrganizationID [" + projectContact.prcOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(projectContact.prcLocationID) && !(await base.ERPProjectContactRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { projectContact.prcOrganizationID, projectContact.prcLocationID })))
			{
				errorsList.Add("prcLocationID [" + projectContact.prcLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(projectContact.prcContactID) && !(await base.ERPProjectContactRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { projectContact.prcOrganizationID, projectContact.prcLocationID, projectContact.prcContactID })))
			{
				errorsList.Add("prcContactID [" + projectContact.prcContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(projectContact.prcContactTitleID) && !(await base.ERPProjectContactRepository.DoesRecordExistInTableUsingKeys("ContactTitles", new object[1] { "CMECONTACTTITLEID" }, new object[1] { projectContact.prcContactTitleID })))
			{
				errorsList.Add("prcContactTitleID [" + projectContact.prcContactTitleID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPProjectContactDto>>> Process_GetAllProjectContacts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPProjectContactDto> allProjectContactsDto = new List<ERPProjectContactDto>();
		ERPResponseMessageDto<IList<ERPProjectContactDto>> result;
		try
		{
			IERPProjectContactRepository iERPProjectContactRepository = (base.ERPProjectContactRepository = new ERPProjectContactRepository(base.ApiClientContext));
			using (iERPProjectContactRepository)
			{
				foreach (ERPProjectContactInformationDto item2 in await base.ERPProjectContactRepository.GetAllProjectContacts(pageSize, pageNumber, filter, orderBy))
				{
					ERPProjectContactDto item = new ERPProjectContactDto
					{
						prcContactID = item2.prcContactID,
						prcContactTitleID = item2.prcContactTitleID,
						prcCreatedBy = item2.prcCreatedBy,
						prcCreatedDate = item2.prcCreatedDate,
						prcUniqueID = item2.prcUniqueID,
						prcLocationID = item2.prcLocationID,
						prcNotesRTF = item2.prcNotesRTF,
						prcNotesText = item2.prcNotesText,
						prcOrganizationID = item2.prcOrganizationID,
						prcProjectID = item2.prcProjectID,
						prcRowVersion = item2.prcRowVersion,
						prcProjectContactID = item2.prcProjectContactID,
						CustomFields = item2.CustomFields
					};
					allProjectContactsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ProjectContacts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPProjectContactDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allProjectContactsDto,
				RecordCount = allProjectContactsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProjectContactDto>> Process_GetProjectContact(Guid projectContactId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPProjectContactDto projectContactDto = null;
		ERPResponseMessageDto<ERPProjectContactDto> result;
		try
		{
			IERPProjectContactRepository iERPProjectContactRepository = (base.ERPProjectContactRepository = new ERPProjectContactRepository(base.ApiClientContext));
			using (iERPProjectContactRepository)
			{
				ERPProjectContactInformationDto eRPProjectContactInformationDto = await base.ERPProjectContactRepository.GetProjectContact(projectContactId);
				projectContactDto = new ERPProjectContactDto
				{
					prcContactID = eRPProjectContactInformationDto.prcContactID,
					prcContactTitleID = eRPProjectContactInformationDto.prcContactTitleID,
					prcCreatedBy = eRPProjectContactInformationDto.prcCreatedBy,
					prcCreatedDate = eRPProjectContactInformationDto.prcCreatedDate,
					prcUniqueID = eRPProjectContactInformationDto.prcUniqueID,
					prcLocationID = eRPProjectContactInformationDto.prcLocationID,
					prcNotesRTF = eRPProjectContactInformationDto.prcNotesRTF,
					prcNotesText = eRPProjectContactInformationDto.prcNotesText,
					prcOrganizationID = eRPProjectContactInformationDto.prcOrganizationID,
					prcProjectID = eRPProjectContactInformationDto.prcProjectID,
					prcRowVersion = eRPProjectContactInformationDto.prcRowVersion,
					prcProjectContactID = eRPProjectContactInformationDto.prcProjectContactID,
					CustomFields = eRPProjectContactInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ProjectContacts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectContactDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = projectContactDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProjectContactDto>> Process_PutProjectContact(ERPProjectContactDto projectContact)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPProjectContactDto createdObject = null;
		ERPResponseMessageDto<ERPProjectContactDto> result;
		try
		{
			IERPProjectContactRepository iERPProjectContactRepository = (base.ERPProjectContactRepository = new ERPProjectContactRepository(base.ApiClientContext));
			using (iERPProjectContactRepository)
			{
				APIValidationInfoDto postResult = await base.ERPProjectContactRepository.SaveProjectContact(projectContact);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPProjectContactInformationDto eRPProjectContactInformationDto = await base.ERPProjectContactRepository.GetProjectContact(projectContact.prcUniqueID);
					createdObject = new ERPProjectContactDto
					{
						prcContactID = eRPProjectContactInformationDto.prcContactID,
						prcContactTitleID = eRPProjectContactInformationDto.prcContactTitleID,
						prcCreatedBy = eRPProjectContactInformationDto.prcCreatedBy,
						prcCreatedDate = eRPProjectContactInformationDto.prcCreatedDate,
						prcUniqueID = eRPProjectContactInformationDto.prcUniqueID,
						prcLocationID = eRPProjectContactInformationDto.prcLocationID,
						prcNotesRTF = eRPProjectContactInformationDto.prcNotesRTF,
						prcNotesText = eRPProjectContactInformationDto.prcNotesText,
						prcOrganizationID = eRPProjectContactInformationDto.prcOrganizationID,
						prcProjectID = eRPProjectContactInformationDto.prcProjectID,
						prcRowVersion = eRPProjectContactInformationDto.prcRowVersion,
						prcProjectContactID = eRPProjectContactInformationDto.prcProjectContactID,
						CustomFields = eRPProjectContactInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ProjectContact [{projectContact.prcUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectContactDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteProjectContact(Guid projectContactId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProjectContactRepository iERPProjectContactRepository = (base.ERPProjectContactRepository = new ERPProjectContactRepository(base.ApiClientContext));
		using (iERPProjectContactRepository)
		{
			if (!(await base.ERPProjectContactRepository.DoesProjectContactExist(projectContactId)))
			{
				base.ErrorsList.Add($"ProjectContact [{projectContactId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPProjectContactInformationDto eRPProjectContactInformationDto = await base.ERPProjectContactRepository.GetProjectContact(projectContactId);
				string text = await base.ERPProjectContactRepository.WhereUsed("ProjectContacts", new object[2] { eRPProjectContactInformationDto.prcProjectID, eRPProjectContactInformationDto.prcProjectContactID }, new object[2] { "prcProjectID", "prcProjectContactID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ProjectContact cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPProjectContactDto>> Process_DeleteProjectContact(Guid projectContactId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPProjectContactDto> result;
		try
		{
			IERPProjectContactRepository iERPProjectContactRepository = (base.ERPProjectContactRepository = new ERPProjectContactRepository(base.ApiClientContext));
			using (iERPProjectContactRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPProjectContactRepository.DeleteRowFromTable("ProjectContacts", "prc", projectContactId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ProjectContact [{projectContactId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProjectContactDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPProjectContactDto()
			};
		}
		return result;
	}
}
