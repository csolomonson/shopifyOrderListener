using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLeadCompetitorModel : ERPBaseModel, IERPLeadCompetitorModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLeadCompetitors(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLeadCompetitorRepository iERPLeadCompetitorRepository = (base.ERPLeadCompetitorRepository = new ERPLeadCompetitorRepository(base.ApiClientContext));
		using (iERPLeadCompetitorRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLeadCompetitorRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLeadCompetitorRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLeadCompetitorRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLeadCompetitorRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLeadCompetitor(Guid leadCompetitorId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadCompetitorRepository iERPLeadCompetitorRepository = (base.ERPLeadCompetitorRepository = new ERPLeadCompetitorRepository(base.ApiClientContext));
		using (iERPLeadCompetitorRepository)
		{
			if (!(await base.ERPLeadCompetitorRepository.DoesLeadCompetitorExist(leadCompetitorId)))
			{
				errorsList.Add($"LeadCompetitor [{leadCompetitorId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutLeadCompetitor(ERPLeadCompetitorDto leadCompetitor)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadCompetitorRepository iERPLeadCompetitorRepository = (base.ERPLeadCompetitorRepository = new ERPLeadCompetitorRepository(base.ApiClientContext));
		using (iERPLeadCompetitorRepository)
		{
			if (!string.IsNullOrWhiteSpace(leadCompetitor.locLeadID) && !(await base.ERPLeadCompetitorRepository.DoesRecordExistInTableUsingKeys("Leads", new object[1] { "LOPLEADID" }, new object[1] { leadCompetitor.locLeadID })))
			{
				errorsList.Add("locLeadID [" + leadCompetitor.locLeadID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(leadCompetitor.locOrganizationID) && !(await base.ERPLeadCompetitorRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { leadCompetitor.locOrganizationID })))
			{
				errorsList.Add("locOrganizationID [" + leadCompetitor.locOrganizationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLeadCompetitorDto>>> Process_GetAllLeadCompetitors(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLeadCompetitorDto> allLeadCompetitorsDto = new List<ERPLeadCompetitorDto>();
		ERPResponseMessageDto<IList<ERPLeadCompetitorDto>> result;
		try
		{
			IERPLeadCompetitorRepository iERPLeadCompetitorRepository = (base.ERPLeadCompetitorRepository = new ERPLeadCompetitorRepository(base.ApiClientContext));
			using (iERPLeadCompetitorRepository)
			{
				foreach (ERPLeadCompetitorInformationDto item2 in await base.ERPLeadCompetitorRepository.GetAllLeadCompetitors(pageSize, pageNumber, filter, orderBy))
				{
					ERPLeadCompetitorDto item = new ERPLeadCompetitorDto
					{
						locCreatedBy = item2.locCreatedBy,
						locCreatedDate = item2.locCreatedDate,
						locUniqueID = item2.locUniqueID,
						locLeadID = item2.locLeadID,
						locLeadNotesRTF = item2.locLeadNotesRTF,
						locLeadNotesText = item2.locLeadNotesText,
						locOrganizationID = item2.locOrganizationID,
						locProductName = item2.locProductName,
						locRowVersion = item2.locRowVersion,
						locLeadCompetitorID = item2.locLeadCompetitorID,
						CustomFields = item2.CustomFields
					};
					allLeadCompetitorsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all LeadCompetitors]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLeadCompetitorDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLeadCompetitorsDto,
				RecordCount = allLeadCompetitorsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLeadCompetitorDto>> Process_GetLeadCompetitor(Guid leadCompetitorId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLeadCompetitorDto leadCompetitorDto = null;
		ERPResponseMessageDto<ERPLeadCompetitorDto> result;
		try
		{
			IERPLeadCompetitorRepository iERPLeadCompetitorRepository = (base.ERPLeadCompetitorRepository = new ERPLeadCompetitorRepository(base.ApiClientContext));
			using (iERPLeadCompetitorRepository)
			{
				ERPLeadCompetitorInformationDto eRPLeadCompetitorInformationDto = await base.ERPLeadCompetitorRepository.GetLeadCompetitor(leadCompetitorId);
				leadCompetitorDto = new ERPLeadCompetitorDto
				{
					locCreatedBy = eRPLeadCompetitorInformationDto.locCreatedBy,
					locCreatedDate = eRPLeadCompetitorInformationDto.locCreatedDate,
					locUniqueID = eRPLeadCompetitorInformationDto.locUniqueID,
					locLeadID = eRPLeadCompetitorInformationDto.locLeadID,
					locLeadNotesRTF = eRPLeadCompetitorInformationDto.locLeadNotesRTF,
					locLeadNotesText = eRPLeadCompetitorInformationDto.locLeadNotesText,
					locOrganizationID = eRPLeadCompetitorInformationDto.locOrganizationID,
					locProductName = eRPLeadCompetitorInformationDto.locProductName,
					locRowVersion = eRPLeadCompetitorInformationDto.locRowVersion,
					locLeadCompetitorID = eRPLeadCompetitorInformationDto.locLeadCompetitorID,
					CustomFields = eRPLeadCompetitorInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the LeadCompetitors []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadCompetitorDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = leadCompetitorDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLeadCompetitorDto>> Process_PutLeadCompetitor(ERPLeadCompetitorDto leadCompetitor)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPLeadCompetitorDto createdObject = null;
		ERPResponseMessageDto<ERPLeadCompetitorDto> result;
		try
		{
			IERPLeadCompetitorRepository iERPLeadCompetitorRepository = (base.ERPLeadCompetitorRepository = new ERPLeadCompetitorRepository(base.ApiClientContext));
			using (iERPLeadCompetitorRepository)
			{
				APIValidationInfoDto postResult = await base.ERPLeadCompetitorRepository.SaveLeadCompetitor(leadCompetitor);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPLeadCompetitorInformationDto eRPLeadCompetitorInformationDto = await base.ERPLeadCompetitorRepository.GetLeadCompetitor(leadCompetitor.locUniqueID);
					createdObject = new ERPLeadCompetitorDto
					{
						locCreatedBy = eRPLeadCompetitorInformationDto.locCreatedBy,
						locCreatedDate = eRPLeadCompetitorInformationDto.locCreatedDate,
						locUniqueID = eRPLeadCompetitorInformationDto.locUniqueID,
						locLeadID = eRPLeadCompetitorInformationDto.locLeadID,
						locLeadNotesRTF = eRPLeadCompetitorInformationDto.locLeadNotesRTF,
						locLeadNotesText = eRPLeadCompetitorInformationDto.locLeadNotesText,
						locOrganizationID = eRPLeadCompetitorInformationDto.locOrganizationID,
						locProductName = eRPLeadCompetitorInformationDto.locProductName,
						locRowVersion = eRPLeadCompetitorInformationDto.locRowVersion,
						locLeadCompetitorID = eRPLeadCompetitorInformationDto.locLeadCompetitorID,
						CustomFields = eRPLeadCompetitorInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing LeadCompetitor [{leadCompetitor.locUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadCompetitorDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteLeadCompetitor(Guid leadCompetitorId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadCompetitorRepository iERPLeadCompetitorRepository = (base.ERPLeadCompetitorRepository = new ERPLeadCompetitorRepository(base.ApiClientContext));
		using (iERPLeadCompetitorRepository)
		{
			if (!(await base.ERPLeadCompetitorRepository.DoesLeadCompetitorExist(leadCompetitorId)))
			{
				base.ErrorsList.Add($"LeadCompetitor [{leadCompetitorId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPLeadCompetitorInformationDto eRPLeadCompetitorInformationDto = await base.ERPLeadCompetitorRepository.GetLeadCompetitor(leadCompetitorId);
				string text = await base.ERPLeadCompetitorRepository.WhereUsed("LeadCompetitors", new object[2] { eRPLeadCompetitorInformationDto.locLeadID, eRPLeadCompetitorInformationDto.locLeadCompetitorID }, new object[2] { "locLeadID", "locLeadCompetitorID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("LeadCompetitor cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPLeadCompetitorDto>> Process_DeleteLeadCompetitor(Guid leadCompetitorId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPLeadCompetitorDto> result;
		try
		{
			IERPLeadCompetitorRepository iERPLeadCompetitorRepository = (base.ERPLeadCompetitorRepository = new ERPLeadCompetitorRepository(base.ApiClientContext));
			using (iERPLeadCompetitorRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPLeadCompetitorRepository.DeleteRowFromTable("LeadCompetitors", "loc", leadCompetitorId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of LeadCompetitor [{leadCompetitorId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadCompetitorDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPLeadCompetitorDto()
			};
		}
		return result;
	}
}
