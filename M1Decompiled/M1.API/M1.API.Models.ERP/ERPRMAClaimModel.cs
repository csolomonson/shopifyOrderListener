using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPRMAClaimModel : ERPBaseModel, IERPRMAClaimModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllRMAClaims(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPRMAClaimRepository iERPRMAClaimRepository = (base.ERPRMAClaimRepository = new ERPRMAClaimRepository(base.ApiClientContext));
		using (iERPRMAClaimRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPRMAClaimRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPRMAClaimRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPRMAClaimRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPRMAClaimRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetRMAClaim(Guid rMAClaimId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAClaimRepository iERPRMAClaimRepository = (base.ERPRMAClaimRepository = new ERPRMAClaimRepository(base.ApiClientContext));
		using (iERPRMAClaimRepository)
		{
			if (!(await base.ERPRMAClaimRepository.DoesRMAClaimExist(rMAClaimId)))
			{
				errorsList.Add($"RMAClaim [{rMAClaimId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutRMAClaim(ERPRMAClaimDto rMAClaim)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAClaimRepository iERPRMAClaimRepository = (base.ERPRMAClaimRepository = new ERPRMAClaimRepository(base.ApiClientContext));
		using (iERPRMAClaimRepository)
		{
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapPlantDepartmentID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { rMAClaim.rapPlantID, rMAClaim.rapPlantDepartmentID })))
			{
				errorsList.Add("rapPlantDepartmentID [" + rMAClaim.rapPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapPlantID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { rMAClaim.rapPlantID })))
			{
				errorsList.Add("rapPlantID [" + rMAClaim.rapPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapProjectID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { rMAClaim.rapProjectID })))
			{
				errorsList.Add("rapProjectID [" + rMAClaim.rapProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapCustomerOrganizationID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { rMAClaim.rapCustomerOrganizationID })))
			{
				errorsList.Add("rapCustomerOrganizationID [" + rMAClaim.rapCustomerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapArInvoiceLocationID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { rMAClaim.rapCustomerOrganizationID, rMAClaim.rapArInvoiceLocationID })))
			{
				errorsList.Add("rapArInvoiceLocationID [" + rMAClaim.rapArInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapArInvoiceContactID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { rMAClaim.rapCustomerOrganizationID, rMAClaim.rapArInvoiceLocationID, rMAClaim.rapArInvoiceContactID })))
			{
				errorsList.Add("rapArInvoiceContactID [" + rMAClaim.rapArInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapShipOrganizationID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { rMAClaim.rapShipOrganizationID })))
			{
				errorsList.Add("rapShipOrganizationID [" + rMAClaim.rapShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapShipLocationID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { rMAClaim.rapShipOrganizationID, rMAClaim.rapShipLocationID })))
			{
				errorsList.Add("rapShipLocationID [" + rMAClaim.rapShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapShipContactID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { rMAClaim.rapShipOrganizationID, rMAClaim.rapShipLocationID, rMAClaim.rapShipContactID })))
			{
				errorsList.Add("rapShipContactID [" + rMAClaim.rapShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapResellerOrganizationID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { rMAClaim.rapResellerOrganizationID })))
			{
				errorsList.Add("rapResellerOrganizationID [" + rMAClaim.rapResellerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapResellerLocationID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { rMAClaim.rapResellerOrganizationID, rMAClaim.rapResellerLocationID })))
			{
				errorsList.Add("rapResellerLocationID [" + rMAClaim.rapResellerLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapResellerContactID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { rMAClaim.rapResellerOrganizationID, rMAClaim.rapResellerLocationID, rMAClaim.rapResellerContactID })))
			{
				errorsList.Add("rapResellerContactID [" + rMAClaim.rapResellerContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapPartID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { rMAClaim.rapPartID })))
			{
				errorsList.Add("rapPartID [" + rMAClaim.rapPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapPartRevisionID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { rMAClaim.rapPartID, rMAClaim.rapPartRevisionID })))
			{
				errorsList.Add("rapPartRevisionID [" + rMAClaim.rapPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapSerialNumberID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("SerialNumbers", new object[3] { "IMSPARTID", "IMSPARTREVISIONID", "IMSSERIALNUMBERID" }, new object[3] { rMAClaim.rapPartID, rMAClaim.rapPartRevisionID, rMAClaim.rapSerialNumberID })))
			{
				errorsList.Add("rapSerialNumberID [" + rMAClaim.rapSerialNumberID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapAuthorizedByEmployeeID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { rMAClaim.rapAuthorizedByEmployeeID })))
			{
				errorsList.Add("rapAuthorizedByEmployeeID [" + rMAClaim.rapAuthorizedByEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapClosedReasonID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { rMAClaim.rapClosedReasonID })))
			{
				errorsList.Add("rapClosedReasonID [" + rMAClaim.rapClosedReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapProcessedByEmployeeID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { rMAClaim.rapProcessedByEmployeeID })))
			{
				errorsList.Add("rapProcessedByEmployeeID [" + rMAClaim.rapProcessedByEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAClaim.rapCurrencyRateID) && !(await base.ERPRMAClaimRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { rMAClaim.rapCurrencyRateID })))
			{
				errorsList.Add("rapCurrencyRateID [" + rMAClaim.rapCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPRMAClaimDto>>> Process_GetAllRMAClaims(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPRMAClaimDto> allRMAClaimsDto = new List<ERPRMAClaimDto>();
		ERPResponseMessageDto<IList<ERPRMAClaimDto>> result;
		try
		{
			IERPRMAClaimRepository iERPRMAClaimRepository = (base.ERPRMAClaimRepository = new ERPRMAClaimRepository(base.ApiClientContext));
			using (iERPRMAClaimRepository)
			{
				foreach (ERPRMAClaimInformationDto item2 in await base.ERPRMAClaimRepository.GetAllRMAClaims(pageSize, pageNumber, filter, orderBy))
				{
					ERPRMAClaimDto item = new ERPRMAClaimDto
					{
						rapActualHoursTotal = item2.rapActualHoursTotal,
						rapArInvoiceContactID = item2.rapArInvoiceContactID,
						rapArInvoiceLocationID = item2.rapArInvoiceLocationID,
						rapAuthorizationDate = item2.rapAuthorizationDate,
						rapAuthorizationNumber = item2.rapAuthorizationNumber,
						rapAuthorizedByEmployeeID = item2.rapAuthorizedByEmployeeID,
						rapClaimDate = item2.rapClaimDate,
						rapClaimTotal = item2.rapClaimTotal,
						rapClaimTotalForeign = item2.rapClaimTotalForeign,
						rapClosedDate = item2.rapClosedDate,
						rapClosedReasonID = item2.rapClosedReasonID,
						rapRmaClaimID = item2.rapRmaClaimID,
						rapCreatedBy = item2.rapCreatedBy,
						rapCreatedDate = item2.rapCreatedDate,
						rapCurrencyRateID = item2.rapCurrencyRateID,
						rapCustomerOrganizationID = item2.rapCustomerOrganizationID,
						rapDiscountAmount = item2.rapDiscountAmount,
						rapDiscountAmountForeign = item2.rapDiscountAmountForeign,
						rapUniqueID = item2.rapUniqueID,
						rapExchangeRate = item2.rapExchangeRate,
						rapFreightAmount = item2.rapFreightAmount,
						rapFreightAmountForeign = item2.rapFreightAmountForeign,
						rapCustomRate = item2.rapCustomRate,
						rapLaborRate = item2.rapLaborRate,
						rapLaborRateForeign = item2.rapLaborRateForeign,
						rapLaborTotal = item2.rapLaborTotal,
						rapLaborTotalForeign = item2.rapLaborTotalForeign,
						rapLongDescriptionRtf = item2.rapLongDescriptionRtf,
						rapLongDescriptionText = item2.rapLongDescriptionText,
						rapPartID = item2.rapPartID,
						rapPartRevisionID = item2.rapPartRevisionID,
						rapPartShortDescription = item2.rapPartShortDescription,
						rapPartsTotal = item2.rapPartsTotal,
						rapPartsTotalForeign = item2.rapPartsTotalForeign,
						rapPayTo = item2.rapPayTo,
						rapPlantDepartmentID = item2.rapPlantDepartmentID,
						rapPlantID = item2.rapPlantID,
						rapProcessedByEmployeeID = item2.rapProcessedByEmployeeID,
						rapProjectID = item2.rapProjectID,
						rapReference = item2.rapReference,
						rapRequestedDate = item2.rapRequestedDate,
						rapResellerContactID = item2.rapResellerContactID,
						rapResellerLocationID = item2.rapResellerLocationID,
						rapResellerOrganizationID = item2.rapResellerOrganizationID,
						rapRowVersion = item2.rapRowVersion,
						rapSerialNumberID = item2.rapSerialNumberID,
						rapShipContactID = item2.rapShipContactID,
						rapShipLocationID = item2.rapShipLocationID,
						rapShipOrganizationID = item2.rapShipOrganizationID,
						rapStatus = item2.rapStatus,
						rapSubcontractTotal = item2.rapSubcontractTotal,
						rapSubcontractTotalForeign = item2.rapSubcontractTotalForeign,
						CustomFields = item2.CustomFields
					};
					allRMAClaimsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all RMAClaims]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPRMAClaimDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allRMAClaimsDto,
				RecordCount = allRMAClaimsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRMAClaimDto>> Process_GetRMAClaim(Guid rMAClaimId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPRMAClaimDto rMAClaimDto = null;
		ERPResponseMessageDto<ERPRMAClaimDto> result;
		try
		{
			IERPRMAClaimRepository iERPRMAClaimRepository = (base.ERPRMAClaimRepository = new ERPRMAClaimRepository(base.ApiClientContext));
			using (iERPRMAClaimRepository)
			{
				ERPRMAClaimInformationDto eRPRMAClaimInformationDto = await base.ERPRMAClaimRepository.GetRMAClaim(rMAClaimId);
				rMAClaimDto = new ERPRMAClaimDto
				{
					rapActualHoursTotal = eRPRMAClaimInformationDto.rapActualHoursTotal,
					rapArInvoiceContactID = eRPRMAClaimInformationDto.rapArInvoiceContactID,
					rapArInvoiceLocationID = eRPRMAClaimInformationDto.rapArInvoiceLocationID,
					rapAuthorizationDate = eRPRMAClaimInformationDto.rapAuthorizationDate,
					rapAuthorizationNumber = eRPRMAClaimInformationDto.rapAuthorizationNumber,
					rapAuthorizedByEmployeeID = eRPRMAClaimInformationDto.rapAuthorizedByEmployeeID,
					rapClaimDate = eRPRMAClaimInformationDto.rapClaimDate,
					rapClaimTotal = eRPRMAClaimInformationDto.rapClaimTotal,
					rapClaimTotalForeign = eRPRMAClaimInformationDto.rapClaimTotalForeign,
					rapClosedDate = eRPRMAClaimInformationDto.rapClosedDate,
					rapClosedReasonID = eRPRMAClaimInformationDto.rapClosedReasonID,
					rapRmaClaimID = eRPRMAClaimInformationDto.rapRmaClaimID,
					rapCreatedBy = eRPRMAClaimInformationDto.rapCreatedBy,
					rapCreatedDate = eRPRMAClaimInformationDto.rapCreatedDate,
					rapCurrencyRateID = eRPRMAClaimInformationDto.rapCurrencyRateID,
					rapCustomerOrganizationID = eRPRMAClaimInformationDto.rapCustomerOrganizationID,
					rapDiscountAmount = eRPRMAClaimInformationDto.rapDiscountAmount,
					rapDiscountAmountForeign = eRPRMAClaimInformationDto.rapDiscountAmountForeign,
					rapUniqueID = eRPRMAClaimInformationDto.rapUniqueID,
					rapExchangeRate = eRPRMAClaimInformationDto.rapExchangeRate,
					rapFreightAmount = eRPRMAClaimInformationDto.rapFreightAmount,
					rapFreightAmountForeign = eRPRMAClaimInformationDto.rapFreightAmountForeign,
					rapCustomRate = eRPRMAClaimInformationDto.rapCustomRate,
					rapLaborRate = eRPRMAClaimInformationDto.rapLaborRate,
					rapLaborRateForeign = eRPRMAClaimInformationDto.rapLaborRateForeign,
					rapLaborTotal = eRPRMAClaimInformationDto.rapLaborTotal,
					rapLaborTotalForeign = eRPRMAClaimInformationDto.rapLaborTotalForeign,
					rapLongDescriptionRtf = eRPRMAClaimInformationDto.rapLongDescriptionRtf,
					rapLongDescriptionText = eRPRMAClaimInformationDto.rapLongDescriptionText,
					rapPartID = eRPRMAClaimInformationDto.rapPartID,
					rapPartRevisionID = eRPRMAClaimInformationDto.rapPartRevisionID,
					rapPartShortDescription = eRPRMAClaimInformationDto.rapPartShortDescription,
					rapPartsTotal = eRPRMAClaimInformationDto.rapPartsTotal,
					rapPartsTotalForeign = eRPRMAClaimInformationDto.rapPartsTotalForeign,
					rapPayTo = eRPRMAClaimInformationDto.rapPayTo,
					rapPlantDepartmentID = eRPRMAClaimInformationDto.rapPlantDepartmentID,
					rapPlantID = eRPRMAClaimInformationDto.rapPlantID,
					rapProcessedByEmployeeID = eRPRMAClaimInformationDto.rapProcessedByEmployeeID,
					rapProjectID = eRPRMAClaimInformationDto.rapProjectID,
					rapReference = eRPRMAClaimInformationDto.rapReference,
					rapRequestedDate = eRPRMAClaimInformationDto.rapRequestedDate,
					rapResellerContactID = eRPRMAClaimInformationDto.rapResellerContactID,
					rapResellerLocationID = eRPRMAClaimInformationDto.rapResellerLocationID,
					rapResellerOrganizationID = eRPRMAClaimInformationDto.rapResellerOrganizationID,
					rapRowVersion = eRPRMAClaimInformationDto.rapRowVersion,
					rapSerialNumberID = eRPRMAClaimInformationDto.rapSerialNumberID,
					rapShipContactID = eRPRMAClaimInformationDto.rapShipContactID,
					rapShipLocationID = eRPRMAClaimInformationDto.rapShipLocationID,
					rapShipOrganizationID = eRPRMAClaimInformationDto.rapShipOrganizationID,
					rapStatus = eRPRMAClaimInformationDto.rapStatus,
					rapSubcontractTotal = eRPRMAClaimInformationDto.rapSubcontractTotal,
					rapSubcontractTotalForeign = eRPRMAClaimInformationDto.rapSubcontractTotalForeign,
					CustomFields = eRPRMAClaimInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the RMAClaims []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAClaimDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = rMAClaimDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRMAClaimDto>> Process_PutRMAClaim(ERPRMAClaimDto rMAClaim)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPRMAClaimDto createdObject = null;
		ERPResponseMessageDto<ERPRMAClaimDto> result;
		try
		{
			IERPRMAClaimRepository iERPRMAClaimRepository = (base.ERPRMAClaimRepository = new ERPRMAClaimRepository(base.ApiClientContext));
			using (iERPRMAClaimRepository)
			{
				APIValidationInfoDto postResult = await base.ERPRMAClaimRepository.SaveRMAClaim(rMAClaim);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPRMAClaimInformationDto eRPRMAClaimInformationDto = await base.ERPRMAClaimRepository.GetRMAClaim(rMAClaim.rapUniqueID);
					createdObject = new ERPRMAClaimDto
					{
						rapActualHoursTotal = eRPRMAClaimInformationDto.rapActualHoursTotal,
						rapArInvoiceContactID = eRPRMAClaimInformationDto.rapArInvoiceContactID,
						rapArInvoiceLocationID = eRPRMAClaimInformationDto.rapArInvoiceLocationID,
						rapAuthorizationDate = eRPRMAClaimInformationDto.rapAuthorizationDate,
						rapAuthorizationNumber = eRPRMAClaimInformationDto.rapAuthorizationNumber,
						rapAuthorizedByEmployeeID = eRPRMAClaimInformationDto.rapAuthorizedByEmployeeID,
						rapClaimDate = eRPRMAClaimInformationDto.rapClaimDate,
						rapClaimTotal = eRPRMAClaimInformationDto.rapClaimTotal,
						rapClaimTotalForeign = eRPRMAClaimInformationDto.rapClaimTotalForeign,
						rapClosedDate = eRPRMAClaimInformationDto.rapClosedDate,
						rapClosedReasonID = eRPRMAClaimInformationDto.rapClosedReasonID,
						rapRmaClaimID = eRPRMAClaimInformationDto.rapRmaClaimID,
						rapCreatedBy = eRPRMAClaimInformationDto.rapCreatedBy,
						rapCreatedDate = eRPRMAClaimInformationDto.rapCreatedDate,
						rapCurrencyRateID = eRPRMAClaimInformationDto.rapCurrencyRateID,
						rapCustomerOrganizationID = eRPRMAClaimInformationDto.rapCustomerOrganizationID,
						rapDiscountAmount = eRPRMAClaimInformationDto.rapDiscountAmount,
						rapDiscountAmountForeign = eRPRMAClaimInformationDto.rapDiscountAmountForeign,
						rapUniqueID = eRPRMAClaimInformationDto.rapUniqueID,
						rapExchangeRate = eRPRMAClaimInformationDto.rapExchangeRate,
						rapFreightAmount = eRPRMAClaimInformationDto.rapFreightAmount,
						rapFreightAmountForeign = eRPRMAClaimInformationDto.rapFreightAmountForeign,
						rapCustomRate = eRPRMAClaimInformationDto.rapCustomRate,
						rapLaborRate = eRPRMAClaimInformationDto.rapLaborRate,
						rapLaborRateForeign = eRPRMAClaimInformationDto.rapLaborRateForeign,
						rapLaborTotal = eRPRMAClaimInformationDto.rapLaborTotal,
						rapLaborTotalForeign = eRPRMAClaimInformationDto.rapLaborTotalForeign,
						rapLongDescriptionRtf = eRPRMAClaimInformationDto.rapLongDescriptionRtf,
						rapLongDescriptionText = eRPRMAClaimInformationDto.rapLongDescriptionText,
						rapPartID = eRPRMAClaimInformationDto.rapPartID,
						rapPartRevisionID = eRPRMAClaimInformationDto.rapPartRevisionID,
						rapPartShortDescription = eRPRMAClaimInformationDto.rapPartShortDescription,
						rapPartsTotal = eRPRMAClaimInformationDto.rapPartsTotal,
						rapPartsTotalForeign = eRPRMAClaimInformationDto.rapPartsTotalForeign,
						rapPayTo = eRPRMAClaimInformationDto.rapPayTo,
						rapPlantDepartmentID = eRPRMAClaimInformationDto.rapPlantDepartmentID,
						rapPlantID = eRPRMAClaimInformationDto.rapPlantID,
						rapProcessedByEmployeeID = eRPRMAClaimInformationDto.rapProcessedByEmployeeID,
						rapProjectID = eRPRMAClaimInformationDto.rapProjectID,
						rapReference = eRPRMAClaimInformationDto.rapReference,
						rapRequestedDate = eRPRMAClaimInformationDto.rapRequestedDate,
						rapResellerContactID = eRPRMAClaimInformationDto.rapResellerContactID,
						rapResellerLocationID = eRPRMAClaimInformationDto.rapResellerLocationID,
						rapResellerOrganizationID = eRPRMAClaimInformationDto.rapResellerOrganizationID,
						rapRowVersion = eRPRMAClaimInformationDto.rapRowVersion,
						rapSerialNumberID = eRPRMAClaimInformationDto.rapSerialNumberID,
						rapShipContactID = eRPRMAClaimInformationDto.rapShipContactID,
						rapShipLocationID = eRPRMAClaimInformationDto.rapShipLocationID,
						rapShipOrganizationID = eRPRMAClaimInformationDto.rapShipOrganizationID,
						rapStatus = eRPRMAClaimInformationDto.rapStatus,
						rapSubcontractTotal = eRPRMAClaimInformationDto.rapSubcontractTotal,
						rapSubcontractTotalForeign = eRPRMAClaimInformationDto.rapSubcontractTotalForeign,
						CustomFields = eRPRMAClaimInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing RMAClaim [{rMAClaim.rapUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAClaimDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteRMAClaim(Guid rMAClaimId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAClaimRepository iERPRMAClaimRepository = (base.ERPRMAClaimRepository = new ERPRMAClaimRepository(base.ApiClientContext));
		using (iERPRMAClaimRepository)
		{
			if (!(await base.ERPRMAClaimRepository.DoesRMAClaimExist(rMAClaimId)))
			{
				base.ErrorsList.Add($"RMAClaim [{rMAClaimId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPRMAClaimInformationDto eRPRMAClaimInformationDto = await base.ERPRMAClaimRepository.GetRMAClaim(rMAClaimId);
				string text = await base.ERPRMAClaimRepository.WhereUsed("RMAClaims", new object[1] { eRPRMAClaimInformationDto.rapRmaClaimID }, new object[1] { "rapRmaClaimID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("RMAClaim cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPRMAClaimDto>> Process_DeleteRMAClaim(Guid rMAClaimId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPRMAClaimDto> result;
		try
		{
			IERPRMAClaimRepository iERPRMAClaimRepository = (base.ERPRMAClaimRepository = new ERPRMAClaimRepository(base.ApiClientContext));
			using (iERPRMAClaimRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPRMAClaimRepository.DeleteRowFromTable("RMAClaims", "rap", rMAClaimId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of RMAClaim [{rMAClaimId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAClaimDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPRMAClaimDto()
			};
		}
		return result;
	}
}
