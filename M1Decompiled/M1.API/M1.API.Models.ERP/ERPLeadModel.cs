using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLeadModel : ERPBaseModel, IERPLeadModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLeads(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLeadRepository iERPLeadRepository = (base.ERPLeadRepository = new ERPLeadRepository(base.ApiClientContext));
		using (iERPLeadRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLeadRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLeadRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLeadRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLeadRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLead(Guid leadId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadRepository iERPLeadRepository = (base.ERPLeadRepository = new ERPLeadRepository(base.ApiClientContext));
		using (iERPLeadRepository)
		{
			if (!(await base.ERPLeadRepository.DoesLeadExist(leadId)))
			{
				errorsList.Add($"Lead [{leadId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutLead(ERPLeadDto lead)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadRepository iERPLeadRepository = (base.ERPLeadRepository = new ERPLeadRepository(base.ApiClientContext));
		using (iERPLeadRepository)
		{
			if (!string.IsNullOrWhiteSpace(lead.lopPlantDepartmentID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { lead.lopPlantID, lead.lopPlantDepartmentID })))
			{
				errorsList.Add("lopPlantDepartmentID [" + lead.lopPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopPlantID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { lead.lopPlantID })))
			{
				errorsList.Add("lopPlantID [" + lead.lopPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopCustomerOrganizationID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { lead.lopCustomerOrganizationID })))
			{
				errorsList.Add("lopCustomerOrganizationID [" + lead.lopCustomerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopLocationID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { lead.lopCustomerOrganizationID, lead.lopLocationID })))
			{
				errorsList.Add("lopLocationID [" + lead.lopLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopContactID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { lead.lopCustomerOrganizationID, lead.lopLocationID, lead.lopContactID })))
			{
				errorsList.Add("lopContactID [" + lead.lopContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopMilestoneID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("Milestones", new object[1] { "LOSMILESTONEID" }, new object[1] { lead.lopMilestoneID })))
			{
				errorsList.Add("lopMilestoneID [" + lead.lopMilestoneID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopResponseMethodID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("ContactMethods", new object[1] { "KBCCONTACTMETHODID" }, new object[1] { lead.lopResponseMethodID })))
			{
				errorsList.Add("lopResponseMethodID [" + lead.lopResponseMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopMarketingProgramID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("MarketingPrograms", new object[1] { "LOOMARKETINGPROGRAMID" }, new object[1] { lead.lopMarketingProgramID })))
			{
				errorsList.Add("lopMarketingProgramID [" + lead.lopMarketingProgramID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopQuoterEmployeeID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { lead.lopQuoterEmployeeID })))
			{
				errorsList.Add("lopQuoterEmployeeID [" + lead.lopQuoterEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopClosedReasonID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { lead.lopClosedReasonID })))
			{
				errorsList.Add("lopClosedReasonID [" + lead.lopClosedReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopClosedByEmployeeID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { lead.lopClosedByEmployeeID })))
			{
				errorsList.Add("lopClosedByEmployeeID [" + lead.lopClosedByEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopProjectID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { lead.lopProjectID })))
			{
				errorsList.Add("lopProjectID [" + lead.lopProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopProjectAreaID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { lead.lopProjectID, lead.lopProjectAreaID })))
			{
				errorsList.Add("lopProjectAreaID [" + lead.lopProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopQuoteLocationID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { lead.lopCustomerOrganizationID, lead.lopQuoteLocationID })))
			{
				errorsList.Add("lopQuoteLocationID [" + lead.lopQuoteLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopShipLocationID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { lead.lopShipOrganizationID, lead.lopShipLocationID })))
			{
				errorsList.Add("lopShipLocationID [" + lead.lopShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopShipOrganizationID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { lead.lopShipOrganizationID })))
			{
				errorsList.Add("lopShipOrganizationID [" + lead.lopShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopQuoteContactID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { lead.lopCustomerOrganizationID, lead.lopQuoteLocationID, lead.lopQuoteContactID })))
			{
				errorsList.Add("lopQuoteContactID [" + lead.lopQuoteContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopShipContactID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { lead.lopShipOrganizationID, lead.lopShipLocationID, lead.lopShipContactID })))
			{
				errorsList.Add("lopShipContactID [" + lead.lopShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(lead.lopCurrencyRateID) && !(await base.ERPLeadRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { lead.lopCurrencyRateID })))
			{
				errorsList.Add("lopCurrencyRateID [" + lead.lopCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLeadDto>>> Process_GetAllLeads(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLeadDto> allLeadsDto = new List<ERPLeadDto>();
		ERPResponseMessageDto<IList<ERPLeadDto>> result;
		try
		{
			IERPLeadRepository iERPLeadRepository = (base.ERPLeadRepository = new ERPLeadRepository(base.ApiClientContext));
			using (iERPLeadRepository)
			{
				foreach (ERPLeadInformationDto item2 in await base.ERPLeadRepository.GetAllLeads(pageSize, pageNumber, filter, orderBy))
				{
					ERPLeadDto item = new ERPLeadDto
					{
						lopClosedByEmployeeID = item2.lopClosedByEmployeeID,
						lopClosedDate = item2.lopClosedDate,
						lopClosedReasonID = item2.lopClosedReasonID,
						lopLeadID = item2.lopLeadID,
						lopContactID = item2.lopContactID,
						lopCreatedBy = item2.lopCreatedBy,
						lopCreatedDate = item2.lopCreatedDate,
						lopCurrencyRateID = item2.lopCurrencyRateID,
						lopCustomerOrganizationID = item2.lopCustomerOrganizationID,
						lopUniqueID = item2.lopUniqueID,
						lopExchangeRate = item2.lopExchangeRate,
						lopExpectedCloseDate = item2.lopExpectedCloseDate,
						lopExpirationDate = item2.lopExpirationDate,
						lopCreatedFromMobile = item2.lopCreatedFromMobile,
						lopCustomRate = item2.lopCustomRate,
						lopLeadDate = item2.lopLeadDate,
						lopLeadTotal = item2.lopLeadTotal,
						lopLeadTotalForeign = item2.lopLeadTotalForeign,
						lopLocationID = item2.lopLocationID,
						lopLongDescriptionRtf = item2.lopLongDescriptionRtf,
						lopLongDescriptionText = item2.lopLongDescriptionText,
						lopMarketingProgramID = item2.lopMarketingProgramID,
						lopMilestoneDate = item2.lopMilestoneDate,
						lopMilestoneID = item2.lopMilestoneID,
						lopPlantDepartmentID = item2.lopPlantDepartmentID,
						lopPlantID = item2.lopPlantID,
						lopProjectAreaID = item2.lopProjectAreaID,
						lopProjectID = item2.lopProjectID,
						lopQuoteContactID = item2.lopQuoteContactID,
						lopQuoteLocationID = item2.lopQuoteLocationID,
						lopQuoterEmployeeID = item2.lopQuoterEmployeeID,
						lopReferredBy = item2.lopReferredBy,
						lopResponseMethodID = item2.lopResponseMethodID,
						lopRowVersion = item2.lopRowVersion,
						lopShipContactID = item2.lopShipContactID,
						lopShipLocationID = item2.lopShipLocationID,
						lopShipOrganizationID = item2.lopShipOrganizationID,
						lopShortDescription = item2.lopShortDescription,
						lopSplitPercentTotal = item2.lopSplitPercentTotal,
						lopStatus = item2.lopStatus,
						CustomFields = item2.CustomFields
					};
					allLeadsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Leads]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLeadDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLeadsDto,
				RecordCount = allLeadsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLeadDto>> Process_GetLead(Guid leadId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLeadDto leadDto = null;
		ERPResponseMessageDto<ERPLeadDto> result;
		try
		{
			IERPLeadRepository iERPLeadRepository = (base.ERPLeadRepository = new ERPLeadRepository(base.ApiClientContext));
			using (iERPLeadRepository)
			{
				ERPLeadInformationDto eRPLeadInformationDto = await base.ERPLeadRepository.GetLead(leadId);
				leadDto = new ERPLeadDto
				{
					lopClosedByEmployeeID = eRPLeadInformationDto.lopClosedByEmployeeID,
					lopClosedDate = eRPLeadInformationDto.lopClosedDate,
					lopClosedReasonID = eRPLeadInformationDto.lopClosedReasonID,
					lopLeadID = eRPLeadInformationDto.lopLeadID,
					lopContactID = eRPLeadInformationDto.lopContactID,
					lopCreatedBy = eRPLeadInformationDto.lopCreatedBy,
					lopCreatedDate = eRPLeadInformationDto.lopCreatedDate,
					lopCurrencyRateID = eRPLeadInformationDto.lopCurrencyRateID,
					lopCustomerOrganizationID = eRPLeadInformationDto.lopCustomerOrganizationID,
					lopUniqueID = eRPLeadInformationDto.lopUniqueID,
					lopExchangeRate = eRPLeadInformationDto.lopExchangeRate,
					lopExpectedCloseDate = eRPLeadInformationDto.lopExpectedCloseDate,
					lopExpirationDate = eRPLeadInformationDto.lopExpirationDate,
					lopCreatedFromMobile = eRPLeadInformationDto.lopCreatedFromMobile,
					lopCustomRate = eRPLeadInformationDto.lopCustomRate,
					lopLeadDate = eRPLeadInformationDto.lopLeadDate,
					lopLeadTotal = eRPLeadInformationDto.lopLeadTotal,
					lopLeadTotalForeign = eRPLeadInformationDto.lopLeadTotalForeign,
					lopLocationID = eRPLeadInformationDto.lopLocationID,
					lopLongDescriptionRtf = eRPLeadInformationDto.lopLongDescriptionRtf,
					lopLongDescriptionText = eRPLeadInformationDto.lopLongDescriptionText,
					lopMarketingProgramID = eRPLeadInformationDto.lopMarketingProgramID,
					lopMilestoneDate = eRPLeadInformationDto.lopMilestoneDate,
					lopMilestoneID = eRPLeadInformationDto.lopMilestoneID,
					lopPlantDepartmentID = eRPLeadInformationDto.lopPlantDepartmentID,
					lopPlantID = eRPLeadInformationDto.lopPlantID,
					lopProjectAreaID = eRPLeadInformationDto.lopProjectAreaID,
					lopProjectID = eRPLeadInformationDto.lopProjectID,
					lopQuoteContactID = eRPLeadInformationDto.lopQuoteContactID,
					lopQuoteLocationID = eRPLeadInformationDto.lopQuoteLocationID,
					lopQuoterEmployeeID = eRPLeadInformationDto.lopQuoterEmployeeID,
					lopReferredBy = eRPLeadInformationDto.lopReferredBy,
					lopResponseMethodID = eRPLeadInformationDto.lopResponseMethodID,
					lopRowVersion = eRPLeadInformationDto.lopRowVersion,
					lopShipContactID = eRPLeadInformationDto.lopShipContactID,
					lopShipLocationID = eRPLeadInformationDto.lopShipLocationID,
					lopShipOrganizationID = eRPLeadInformationDto.lopShipOrganizationID,
					lopShortDescription = eRPLeadInformationDto.lopShortDescription,
					lopSplitPercentTotal = eRPLeadInformationDto.lopSplitPercentTotal,
					lopStatus = eRPLeadInformationDto.lopStatus,
					CustomFields = eRPLeadInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Leads []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = leadDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLeadDto>> Process_PutLead(ERPLeadDto lead)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPLeadDto createdObject = null;
		ERPResponseMessageDto<ERPLeadDto> result;
		try
		{
			IERPLeadRepository iERPLeadRepository = (base.ERPLeadRepository = new ERPLeadRepository(base.ApiClientContext));
			using (iERPLeadRepository)
			{
				APIValidationInfoDto postResult = await base.ERPLeadRepository.SaveLead(lead);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPLeadInformationDto eRPLeadInformationDto = await base.ERPLeadRepository.GetLead(lead.lopUniqueID);
					createdObject = new ERPLeadDto
					{
						lopClosedByEmployeeID = eRPLeadInformationDto.lopClosedByEmployeeID,
						lopClosedDate = eRPLeadInformationDto.lopClosedDate,
						lopClosedReasonID = eRPLeadInformationDto.lopClosedReasonID,
						lopLeadID = eRPLeadInformationDto.lopLeadID,
						lopContactID = eRPLeadInformationDto.lopContactID,
						lopCreatedBy = eRPLeadInformationDto.lopCreatedBy,
						lopCreatedDate = eRPLeadInformationDto.lopCreatedDate,
						lopCurrencyRateID = eRPLeadInformationDto.lopCurrencyRateID,
						lopCustomerOrganizationID = eRPLeadInformationDto.lopCustomerOrganizationID,
						lopUniqueID = eRPLeadInformationDto.lopUniqueID,
						lopExchangeRate = eRPLeadInformationDto.lopExchangeRate,
						lopExpectedCloseDate = eRPLeadInformationDto.lopExpectedCloseDate,
						lopExpirationDate = eRPLeadInformationDto.lopExpirationDate,
						lopCreatedFromMobile = eRPLeadInformationDto.lopCreatedFromMobile,
						lopCustomRate = eRPLeadInformationDto.lopCustomRate,
						lopLeadDate = eRPLeadInformationDto.lopLeadDate,
						lopLeadTotal = eRPLeadInformationDto.lopLeadTotal,
						lopLeadTotalForeign = eRPLeadInformationDto.lopLeadTotalForeign,
						lopLocationID = eRPLeadInformationDto.lopLocationID,
						lopLongDescriptionRtf = eRPLeadInformationDto.lopLongDescriptionRtf,
						lopLongDescriptionText = eRPLeadInformationDto.lopLongDescriptionText,
						lopMarketingProgramID = eRPLeadInformationDto.lopMarketingProgramID,
						lopMilestoneDate = eRPLeadInformationDto.lopMilestoneDate,
						lopMilestoneID = eRPLeadInformationDto.lopMilestoneID,
						lopPlantDepartmentID = eRPLeadInformationDto.lopPlantDepartmentID,
						lopPlantID = eRPLeadInformationDto.lopPlantID,
						lopProjectAreaID = eRPLeadInformationDto.lopProjectAreaID,
						lopProjectID = eRPLeadInformationDto.lopProjectID,
						lopQuoteContactID = eRPLeadInformationDto.lopQuoteContactID,
						lopQuoteLocationID = eRPLeadInformationDto.lopQuoteLocationID,
						lopQuoterEmployeeID = eRPLeadInformationDto.lopQuoterEmployeeID,
						lopReferredBy = eRPLeadInformationDto.lopReferredBy,
						lopResponseMethodID = eRPLeadInformationDto.lopResponseMethodID,
						lopRowVersion = eRPLeadInformationDto.lopRowVersion,
						lopShipContactID = eRPLeadInformationDto.lopShipContactID,
						lopShipLocationID = eRPLeadInformationDto.lopShipLocationID,
						lopShipOrganizationID = eRPLeadInformationDto.lopShipOrganizationID,
						lopShortDescription = eRPLeadInformationDto.lopShortDescription,
						lopSplitPercentTotal = eRPLeadInformationDto.lopSplitPercentTotal,
						lopStatus = eRPLeadInformationDto.lopStatus,
						CustomFields = eRPLeadInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Lead [{lead.lopUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteLead(Guid leadId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadRepository iERPLeadRepository = (base.ERPLeadRepository = new ERPLeadRepository(base.ApiClientContext));
		using (iERPLeadRepository)
		{
			if (!(await base.ERPLeadRepository.DoesLeadExist(leadId)))
			{
				base.ErrorsList.Add($"Lead [{leadId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPLeadInformationDto eRPLeadInformationDto = await base.ERPLeadRepository.GetLead(leadId);
				string text = await base.ERPLeadRepository.WhereUsed("Leads", new object[1] { eRPLeadInformationDto.lopLeadID }, new object[1] { "lopLeadID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Lead cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPLeadDto>> Process_DeleteLead(Guid leadId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPLeadDto> result;
		try
		{
			IERPLeadRepository iERPLeadRepository = (base.ERPLeadRepository = new ERPLeadRepository(base.ApiClientContext));
			using (iERPLeadRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPLeadRepository.DeleteRowFromTable("Leads", "lop", leadId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Lead [{leadId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPLeadDto()
			};
		}
		return result;
	}
}
