using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Repositories.Core;

namespace M1.API.Models.BOM;

public class BOMOrganizationContactModel : BOMBaseModel, IBOMOrganizationContactModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetOrganizationContact(string organizationId, string locationId, string contactId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		using (OrganizationContactRepository organizationContactRepository = new OrganizationContactRepository(base.ApiClientContext))
		{
			if (!(await organizationContactRepository.DoesOrganizationContactExists(organizationId, locationId, contactId)))
			{
				errorsList.Add("Organization [" + organizationId + "], containing Location [" + locationId + "] and Contact [" + contactId + "] is invalid");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PostOrganizationContact(BOMOrganizationContactDto organizationContact)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		APIValidationInfoDto result;
		try
		{
			using OrganizationLocationRepository organizationLocationRepository = new OrganizationLocationRepository(base.ApiClientContext);
			if (!string.IsNullOrWhiteSpace(organizationContact.OrganizationID) && !(await organizationLocationRepository.DoesOrganizationLocationExists(organizationContact.OrganizationID, organizationContact.LocationID)))
			{
				base.ErrorsList.Add("Organization [" + organizationContact.OrganizationID + "] or Organization Location [" + organizationContact.LocationID + "] is not valid.");
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while validating the organization contact [" + organizationContact.ContactID + "]");
		}
		finally
		{
			result = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			base.ErrorsList.Clear();
			base.WarningsList.Clear();
		}
		return await Task.FromResult(result);
	}

	public async Task<BOMResponseMessageDto<IList<CTMOrganizationContactDto>>> Process_GetAllOrganizationContacts(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<CTMOrganizationContactDto> allOrganizationContactsDto = new List<CTMOrganizationContactDto>();
		BOMResponseMessageDto<IList<CTMOrganizationContactDto>> result;
		try
		{
			using OrganizationContactRepository organizationContactRepository = new OrganizationContactRepository(base.ApiClientContext);
			foreach (OrganizationContactInformationDto item2 in await organizationContactRepository.GetAllOrganizationContacts(pageSize, pageNumber))
			{
				CTMOrganizationContactDto item = new CTMOrganizationContactDto
				{
					OrganizationID = item2.OrganizationID,
					LocationID = item2.LocationID,
					ContactID = item2.ContactID,
					Name = item2.Name,
					PhoneNumber = item2.PhoneNumber,
					MobileNumber = item2.MobileNumber,
					EmailAddress = item2.EmailAddress,
					CorrespondenceMethod = item2.CorrespondenceMethod,
					Inactive = item2.Inactive,
					InactiveDate = item2.InactiveDate,
					EasyOrderEnabled = item2.EasyOrderEnabled,
					CreatedByEasyOrder = item2.CreatedByEasyOrder,
					EOFirstName = item2.EOFirstName,
					EOInitials = item2.EOInitials,
					EOPrefix = item2.EOPrefix,
					EOSurname = item2.EOSurname,
					EOPassword = item2.EOPassword,
					EOUserRole = item2.EOUserRole,
					EODefSupervisor = item2.EODefSupervisor,
					EOSubSupervisor = item2.EOSubSupervisor,
					EOCustomerGroup = item2.EOCustomerGroup,
					EOMultiShipAddress = item2.EOMultiShipAddress,
					EOReceiveOrderConfirmation = item2.EOReceiveOrderConfirmation,
					EOEditShippingAddress = item2.EOEditShippingAddress,
					EOReceiveEMails = item2.EOReceiveEMails,
					EOHTMLMail = item2.EOHTMLMail,
					EOReminderOfOpenOrders = item2.EOReminderOfOpenOrders,
					EOOrderAuthorisationMessage = item2.EOOrderAuthorisationMessage,
					EOAuthorisationRequest = item2.EOAuthorisationRequest,
					EOMayNotCreOrdTemp = item2.EOMayNotCreOrdTemp,
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					RowVersion = item2.RowVersion
				};
				allOrganizationContactsDto.Add(item);
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
			result = new BOMResponseMessageDto<IList<CTMOrganizationContactDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allOrganizationContactsDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<CTMOrganizationContactDto>> Process_GetOrganizationContact(string organizationId, string locationId, string contactId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		CTMOrganizationContactDto organizationContactDto = null;
		BOMResponseMessageDto<CTMOrganizationContactDto> result;
		try
		{
			using OrganizationContactRepository organizationContactRepository = new OrganizationContactRepository(base.ApiClientContext);
			OrganizationContactInformationDto organizationContactInformationDto = await organizationContactRepository.GetOrganizationContact(organizationId, locationId, contactId);
			organizationContactDto = new CTMOrganizationContactDto
			{
				OrganizationID = organizationContactInformationDto.OrganizationID,
				LocationID = organizationContactInformationDto.LocationID,
				ContactID = organizationContactInformationDto.ContactID,
				Name = organizationContactInformationDto.Name,
				PhoneNumber = organizationContactInformationDto.PhoneNumber,
				MobileNumber = organizationContactInformationDto.MobileNumber,
				EmailAddress = organizationContactInformationDto.EmailAddress,
				CorrespondenceMethod = organizationContactInformationDto.CorrespondenceMethod,
				Inactive = organizationContactInformationDto.Inactive,
				InactiveDate = organizationContactInformationDto.InactiveDate,
				EasyOrderEnabled = organizationContactInformationDto.EasyOrderEnabled,
				CreatedByEasyOrder = organizationContactInformationDto.CreatedByEasyOrder,
				EOFirstName = organizationContactInformationDto.EOFirstName,
				EOInitials = organizationContactInformationDto.EOInitials,
				EOPrefix = organizationContactInformationDto.EOPrefix,
				EOSurname = organizationContactInformationDto.EOSurname,
				EOPassword = organizationContactInformationDto.EOPassword,
				EOUserRole = organizationContactInformationDto.EOUserRole,
				EODefSupervisor = organizationContactInformationDto.EODefSupervisor,
				EOSubSupervisor = organizationContactInformationDto.EOSubSupervisor,
				EOCustomerGroup = organizationContactInformationDto.EOCustomerGroup,
				EOMultiShipAddress = organizationContactInformationDto.EOMultiShipAddress,
				EOReceiveOrderConfirmation = organizationContactInformationDto.EOReceiveOrderConfirmation,
				EOEditShippingAddress = organizationContactInformationDto.EOEditShippingAddress,
				EOReceiveEMails = organizationContactInformationDto.EOReceiveEMails,
				EOHTMLMail = organizationContactInformationDto.EOHTMLMail,
				EOReminderOfOpenOrders = organizationContactInformationDto.EOReminderOfOpenOrders,
				EOOrderAuthorisationMessage = organizationContactInformationDto.EOOrderAuthorisationMessage,
				EOAuthorisationRequest = organizationContactInformationDto.EOAuthorisationRequest,
				EOMayNotCreOrdTemp = organizationContactInformationDto.EOMayNotCreOrdTemp,
				CreatedBy = organizationContactInformationDto.CreatedBy,
				CreatedDate = organizationContactInformationDto.CreatedDate,
				UniqueID = organizationContactInformationDto.UniqueID,
				RowVersion = organizationContactInformationDto.RowVersion
			};
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the OrganizationContacts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<CTMOrganizationContactDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = organizationContactDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMOrganizationContactDto>> Process_PostOrganizationContact(BOMOrganizationContactDto organizationContact)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		BOMResponseMessageDto<BOMOrganizationContactDto> result;
		try
		{
			using OrganizationContactRepository organizationContactRepository = new OrganizationContactRepository(base.ApiClientContext);
			APIValidationInfoDto aPIValidationInfoDto = await organizationContactRepository.SaveOrganizationContact(organizationContact);
			((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
			((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing OrganizationContact [" + organizationContact.ContactID + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMOrganizationContactDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = organizationContact
			};
		}
		return result;
	}
}
