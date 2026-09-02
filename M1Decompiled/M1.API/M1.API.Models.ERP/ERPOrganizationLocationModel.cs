using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPOrganizationLocationModel : ERPBaseModel, IERPOrganizationLocationModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizationLocations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPOrganizationLocationRepository iERPOrganizationLocationRepository = (base.ERPOrganizationLocationRepository = new ERPOrganizationLocationRepository(base.ApiClientContext));
		using (iERPOrganizationLocationRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPOrganizationLocationRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPOrganizationLocationRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPOrganizationLocationRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPOrganizationLocationRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetOrganizationLocation(Guid organizationLocationId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationLocationRepository iERPOrganizationLocationRepository = (base.ERPOrganizationLocationRepository = new ERPOrganizationLocationRepository(base.ApiClientContext));
		using (iERPOrganizationLocationRepository)
		{
			if (!(await base.ERPOrganizationLocationRepository.DoesOrganizationLocationExist(organizationLocationId)))
			{
				errorsList.Add($"OrganizationLocation [{organizationLocationId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutOrganizationLocation(ERPOrganizationLocationDto organizationLocation)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationLocationRepository iERPOrganizationLocationRepository = (base.ERPOrganizationLocationRepository = new ERPOrganizationLocationRepository(base.ApiClientContext));
		using (iERPOrganizationLocationRepository)
		{
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlOrganizationID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { organizationLocation.cmlOrganizationID })))
			{
				errorsList.Add("cmlOrganizationID [" + organizationLocation.cmlOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlFinanceOrganizationID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { organizationLocation.cmlFinanceOrganizationID })))
			{
				errorsList.Add("cmlFinanceOrganizationID [" + organizationLocation.cmlFinanceOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlQuoteContactID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { organizationLocation.cmlOrganizationID, organizationLocation.cmlLocationID, organizationLocation.cmlQuoteContactID })))
			{
				errorsList.Add("cmlQuoteContactID [" + organizationLocation.cmlQuoteContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlShipContactID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { organizationLocation.cmlOrganizationID, organizationLocation.cmlLocationID, organizationLocation.cmlShipContactID })))
			{
				errorsList.Add("cmlShipContactID [" + organizationLocation.cmlShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlArInvoiceContactID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { organizationLocation.cmlOrganizationID, organizationLocation.cmlLocationID, organizationLocation.cmlArInvoiceContactID })))
			{
				errorsList.Add("cmlArInvoiceContactID [" + organizationLocation.cmlArInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlPurchaseContactID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { organizationLocation.cmlOrganizationID, organizationLocation.cmlLocationID, organizationLocation.cmlPurchaseContactID })))
			{
				errorsList.Add("cmlPurchaseContactID [" + organizationLocation.cmlPurchaseContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlApInvoiceContactID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { organizationLocation.cmlOrganizationID, organizationLocation.cmlLocationID, organizationLocation.cmlApInvoiceContactID })))
			{
				errorsList.Add("cmlApInvoiceContactID [" + organizationLocation.cmlApInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlCustomerTaxCodeID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { organizationLocation.cmlCustomerTaxCodeID })))
			{
				errorsList.Add("cmlCustomerTaxCodeID [" + organizationLocation.cmlCustomerTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlCustomerSecondTaxCodeID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { organizationLocation.cmlCustomerSecondTaxCodeID })))
			{
				errorsList.Add("cmlCustomerSecondTaxCodeID [" + organizationLocation.cmlCustomerSecondTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlCustomerShippingMethodID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { organizationLocation.cmlCustomerShippingMethodID })))
			{
				errorsList.Add("cmlCustomerShippingMethodID [" + organizationLocation.cmlCustomerShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlCustomerShipPaymentTypeID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("ShippingPaymentTypes", new object[1] { "XAYSHIPPINGPAYMENTTYPEID" }, new object[1] { organizationLocation.cmlCustomerShipPaymentTypeID })))
			{
				errorsList.Add("cmlCustomerShipPaymentTypeID [" + organizationLocation.cmlCustomerShipPaymentTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlNonTaxReasonID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { organizationLocation.cmlNonTaxReasonID })))
			{
				errorsList.Add("cmlNonTaxReasonID [" + organizationLocation.cmlNonTaxReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlCustomerPaymentTermID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("PaymentTerms", new object[1] { "XATPAYMENTTERMID" }, new object[1] { organizationLocation.cmlCustomerPaymentTermID })))
			{
				errorsList.Add("cmlCustomerPaymentTermID [" + organizationLocation.cmlCustomerPaymentTermID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlCurrencyRateID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { organizationLocation.cmlCurrencyRateID })))
			{
				errorsList.Add("cmlCurrencyRateID [" + organizationLocation.cmlCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlSupplierPaymentTermID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("PaymentTerms", new object[1] { "XATPAYMENTTERMID" }, new object[1] { organizationLocation.cmlSupplierPaymentTermID })))
			{
				errorsList.Add("cmlSupplierPaymentTermID [" + organizationLocation.cmlSupplierPaymentTermID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlSupplierShippingMethodID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { organizationLocation.cmlSupplierShippingMethodID })))
			{
				errorsList.Add("cmlSupplierShippingMethodID [" + organizationLocation.cmlSupplierShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlUps3rdPartyOrganizationID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { organizationLocation.cmlUps3rdPartyOrganizationID })))
			{
				errorsList.Add("cmlUps3rdPartyOrganizationID [" + organizationLocation.cmlUps3rdPartyOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlUps3rdPartyLocationID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organizationLocation.cmlUps3rdPartyOrganizationID, organizationLocation.cmlUps3rdPartyLocationID })))
			{
				errorsList.Add("cmlUps3rdPartyLocationID [" + organizationLocation.cmlUps3rdPartyLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlFedEx3rdPartyOrganizationID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONS", new object[1] { "CMOORGANIZATIONID" }, new object[1] { organizationLocation.cmlFedEx3rdPartyOrganizationID })))
			{
				errorsList.Add("cmlFedEx3rdPartyOrganizationID [" + organizationLocation.cmlFedEx3rdPartyOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocation.cmlFedEx3rdPartyLocationID) && !(await base.ERPOrganizationLocationRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONLOCATIONS", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organizationLocation.cmlFedEx3rdPartyOrganizationID, organizationLocation.cmlFedEx3rdPartyLocationID })))
			{
				errorsList.Add("cmlFedEx3rdPartyLocationID [" + organizationLocation.cmlFedEx3rdPartyLocationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPOrganizationLocationDto>>> Process_GetAllOrganizationLocations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPOrganizationLocationDto> allOrganizationLocationsDto = new List<ERPOrganizationLocationDto>();
		ERPResponseMessageDto<IList<ERPOrganizationLocationDto>> result;
		try
		{
			IERPOrganizationLocationRepository iERPOrganizationLocationRepository = (base.ERPOrganizationLocationRepository = new ERPOrganizationLocationRepository(base.ApiClientContext));
			using (iERPOrganizationLocationRepository)
			{
				foreach (ERPOrganizationLocationInformationDto item2 in await base.ERPOrganizationLocationRepository.GetAllOrganizationLocations(pageSize, pageNumber, filter, orderBy))
				{
					ERPOrganizationLocationDto item = new ERPOrganizationLocationDto
					{
						cmlAddressLine1 = item2.cmlAddressLine1,
						cmlAddressLine2 = item2.cmlAddressLine2,
						cmlAddressLine3 = item2.cmlAddressLine3,
						cmlAddressValidationResult = item2.cmlAddressValidationResult,
						cmlAlternatePhoneNumber = item2.cmlAlternatePhoneNumber,
						cmlApInvoiceContactID = item2.cmlApInvoiceContactID,
						cmlArInvoiceContactID = item2.cmlArInvoiceContactID,
						cmlAvalaraUseCodes = item2.cmlAvalaraUseCodes,
						cmlBankAccountName = item2.cmlBankAccountName,
						cmlBankAccountNumber = item2.cmlBankAccountNumber,
						cmlBankAccountType = item2.cmlBankAccountType,
						cmlBankInitials = item2.cmlBankInitials,
						cmlBic = item2.cmlBic,
						cmlBsbNumber = item2.cmlBsbNumber,
						cmlCity = item2.cmlCity,
						cmlCountry = item2.cmlCountry,
						cmlCountryCode = item2.cmlCountryCode,
						cmlCounty = item2.cmlCounty,
						cmlCreatedBy = item2.cmlCreatedBy,
						cmlCreatedDate = item2.cmlCreatedDate,
						cmlCurrencyRateID = item2.cmlCurrencyRateID,
						cmlCustomerCreditLimit = item2.cmlCustomerCreditLimit,
						cmlCustomerPaymentTermID = item2.cmlCustomerPaymentTermID,
						cmlCustomerSecondTaxCodeID = item2.cmlCustomerSecondTaxCodeID,
						cmlCustomerShipPaymentTypeID = item2.cmlCustomerShipPaymentTypeID,
						cmlCustomerShippingCarrier = item2.cmlCustomerShippingCarrier,
						cmlCustomerShippingMethodID = item2.cmlCustomerShippingMethodID,
						cmlCustomerTaxCodeID = item2.cmlCustomerTaxCodeID,
						cmlEdiLocationID = item2.cmlEdiLocationID,
						cmlEftCode = item2.cmlEftCode,
						cmlEftDescription = item2.cmlEftDescription,
						cmlEftParticulars = item2.cmlEftParticulars,
						cmlEmailAddress = item2.cmlEmailAddress,
						cmlUniqueID = item2.cmlUniqueID,
						cmlFaxNumber = item2.cmlFaxNumber,
						cmlFedEx3rdPartyLocationID = item2.cmlFedEx3rdPartyLocationID,
						cmlFedEx3rdPartyOrganizationID = item2.cmlFedEx3rdPartyOrganizationID,
						cmlFedExAccountNumber = item2.cmlFedExAccountNumber,
						cmlFedExBillingOption = item2.cmlFedExBillingOption,
						cmlFinanceOrganizationID = item2.cmlFinanceOrganizationID,
						cmlFirstGivenName = item2.cmlFirstGivenName,
						cmlFreeOnBoardDescription = item2.cmlFreeOnBoardDescription,
						cmlHdAttachmentFilePath = item2.cmlHdAttachmentFilePath,
						cmlIban = item2.cmlIban,
						cmlInactiveDate = item2.cmlInactiveDate,
						cmlInactive = item2.cmlInactive,
						cmlApInvoiceLocation = item2.cmlApInvoiceLocation,
						cmlArInvoiceLocation = item2.cmlArInvoiceLocation,
						cmlArInvoicePerShipmentLine = item2.cmlArInvoicePerShipmentLine,
						cmlAvalaraAddressValidated = item2.cmlAvalaraAddressValidated,
						cmlBareCostOfDuty = item2.cmlBareCostOfDuty,
						cmlBareTransportationCost = item2.cmlBareTransportationCost,
						cmlContractor = item2.cmlContractor,
						cmlCreatedFromMobile = item2.cmlCreatedFromMobile,
						cmlCreditCheckForLocation = item2.cmlCreditCheckForLocation,
						cmlCreditHold = item2.cmlCreditHold,
						cmlCustomerTaxable = item2.cmlCustomerTaxable,
						cmlDirectPayment = item2.cmlDirectPayment,
						cmlEdiIntegrated = item2.cmlEdiIntegrated,
						cmlIgnoreAvalara = item2.cmlIgnoreAvalara,
						cmlPurchaseLocation = item2.cmlPurchaseLocation,
						cmlQuoteLocation = item2.cmlQuoteLocation,
						cmlResidentialAddress = item2.cmlResidentialAddress,
						cmlShipLocation = item2.cmlShipLocation,
						cmlTaxReportable = item2.cmlTaxReportable,
						cmlUpsValidated = item2.cmlUpsValidated,
						cmlLastName = item2.cmlLastName,
						cmlLocationID = item2.cmlLocationID,
						cmlName = item2.cmlName,
						cmlNonTaxReasonID = item2.cmlNonTaxReasonID,
						cmlOrganizationID = item2.cmlOrganizationID,
						cmlPhoneNumber = item2.cmlPhoneNumber,
						cmlPostCode = item2.cmlPostCode,
						cmlPurchaseContactID = item2.cmlPurchaseContactID,
						cmlQuoteContactID = item2.cmlQuoteContactID,
						cmlRowVersion = item2.cmlRowVersion,
						cmlSecondGivenName = item2.cmlSecondGivenName,
						cmlShipContactID = item2.cmlShipContactID,
						cmlSplitPercentTotal = item2.cmlSplitPercentTotal,
						cmlState = item2.cmlState,
						cmlSupplierPaymentTermID = item2.cmlSupplierPaymentTermID,
						cmlSupplierShippingMethodID = item2.cmlSupplierShippingMethodID,
						cmlTaxExemptNumber = item2.cmlTaxExemptNumber,
						cmlTradingName = item2.cmlTradingName,
						cmlUps3rdPartyLocationID = item2.cmlUps3rdPartyLocationID,
						cmlUps3rdPartyOrganizationID = item2.cmlUps3rdPartyOrganizationID,
						cmlUpsAcctNumber = item2.cmlUpsAcctNumber,
						cmlUpsBillingOption = item2.cmlUpsBillingOption,
						cmlUpsWsBillingOption = item2.cmlUpsWsBillingOption,
						CustomFields = item2.CustomFields
					};
					allOrganizationLocationsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all OrganizationLocations]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPOrganizationLocationDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allOrganizationLocationsDto,
				RecordCount = allOrganizationLocationsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationLocationDto>> Process_GetOrganizationLocation(Guid organizationLocationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPOrganizationLocationDto organizationLocationDto = null;
		ERPResponseMessageDto<ERPOrganizationLocationDto> result;
		try
		{
			IERPOrganizationLocationRepository iERPOrganizationLocationRepository = (base.ERPOrganizationLocationRepository = new ERPOrganizationLocationRepository(base.ApiClientContext));
			using (iERPOrganizationLocationRepository)
			{
				ERPOrganizationLocationInformationDto eRPOrganizationLocationInformationDto = await base.ERPOrganizationLocationRepository.GetOrganizationLocation(organizationLocationId);
				organizationLocationDto = new ERPOrganizationLocationDto
				{
					cmlAddressLine1 = eRPOrganizationLocationInformationDto.cmlAddressLine1,
					cmlAddressLine2 = eRPOrganizationLocationInformationDto.cmlAddressLine2,
					cmlAddressLine3 = eRPOrganizationLocationInformationDto.cmlAddressLine3,
					cmlAddressValidationResult = eRPOrganizationLocationInformationDto.cmlAddressValidationResult,
					cmlAlternatePhoneNumber = eRPOrganizationLocationInformationDto.cmlAlternatePhoneNumber,
					cmlApInvoiceContactID = eRPOrganizationLocationInformationDto.cmlApInvoiceContactID,
					cmlArInvoiceContactID = eRPOrganizationLocationInformationDto.cmlArInvoiceContactID,
					cmlAvalaraUseCodes = eRPOrganizationLocationInformationDto.cmlAvalaraUseCodes,
					cmlBankAccountName = eRPOrganizationLocationInformationDto.cmlBankAccountName,
					cmlBankAccountNumber = eRPOrganizationLocationInformationDto.cmlBankAccountNumber,
					cmlBankAccountType = eRPOrganizationLocationInformationDto.cmlBankAccountType,
					cmlBankInitials = eRPOrganizationLocationInformationDto.cmlBankInitials,
					cmlBic = eRPOrganizationLocationInformationDto.cmlBic,
					cmlBsbNumber = eRPOrganizationLocationInformationDto.cmlBsbNumber,
					cmlCity = eRPOrganizationLocationInformationDto.cmlCity,
					cmlCountry = eRPOrganizationLocationInformationDto.cmlCountry,
					cmlCountryCode = eRPOrganizationLocationInformationDto.cmlCountryCode,
					cmlCounty = eRPOrganizationLocationInformationDto.cmlCounty,
					cmlCreatedBy = eRPOrganizationLocationInformationDto.cmlCreatedBy,
					cmlCreatedDate = eRPOrganizationLocationInformationDto.cmlCreatedDate,
					cmlCurrencyRateID = eRPOrganizationLocationInformationDto.cmlCurrencyRateID,
					cmlCustomerCreditLimit = eRPOrganizationLocationInformationDto.cmlCustomerCreditLimit,
					cmlCustomerPaymentTermID = eRPOrganizationLocationInformationDto.cmlCustomerPaymentTermID,
					cmlCustomerSecondTaxCodeID = eRPOrganizationLocationInformationDto.cmlCustomerSecondTaxCodeID,
					cmlCustomerShipPaymentTypeID = eRPOrganizationLocationInformationDto.cmlCustomerShipPaymentTypeID,
					cmlCustomerShippingCarrier = eRPOrganizationLocationInformationDto.cmlCustomerShippingCarrier,
					cmlCustomerShippingMethodID = eRPOrganizationLocationInformationDto.cmlCustomerShippingMethodID,
					cmlCustomerTaxCodeID = eRPOrganizationLocationInformationDto.cmlCustomerTaxCodeID,
					cmlEdiLocationID = eRPOrganizationLocationInformationDto.cmlEdiLocationID,
					cmlEftCode = eRPOrganizationLocationInformationDto.cmlEftCode,
					cmlEftDescription = eRPOrganizationLocationInformationDto.cmlEftDescription,
					cmlEftParticulars = eRPOrganizationLocationInformationDto.cmlEftParticulars,
					cmlEmailAddress = eRPOrganizationLocationInformationDto.cmlEmailAddress,
					cmlUniqueID = eRPOrganizationLocationInformationDto.cmlUniqueID,
					cmlFaxNumber = eRPOrganizationLocationInformationDto.cmlFaxNumber,
					cmlFedEx3rdPartyLocationID = eRPOrganizationLocationInformationDto.cmlFedEx3rdPartyLocationID,
					cmlFedEx3rdPartyOrganizationID = eRPOrganizationLocationInformationDto.cmlFedEx3rdPartyOrganizationID,
					cmlFedExAccountNumber = eRPOrganizationLocationInformationDto.cmlFedExAccountNumber,
					cmlFedExBillingOption = eRPOrganizationLocationInformationDto.cmlFedExBillingOption,
					cmlFinanceOrganizationID = eRPOrganizationLocationInformationDto.cmlFinanceOrganizationID,
					cmlFirstGivenName = eRPOrganizationLocationInformationDto.cmlFirstGivenName,
					cmlFreeOnBoardDescription = eRPOrganizationLocationInformationDto.cmlFreeOnBoardDescription,
					cmlHdAttachmentFilePath = eRPOrganizationLocationInformationDto.cmlHdAttachmentFilePath,
					cmlIban = eRPOrganizationLocationInformationDto.cmlIban,
					cmlInactiveDate = eRPOrganizationLocationInformationDto.cmlInactiveDate,
					cmlInactive = eRPOrganizationLocationInformationDto.cmlInactive,
					cmlApInvoiceLocation = eRPOrganizationLocationInformationDto.cmlApInvoiceLocation,
					cmlArInvoiceLocation = eRPOrganizationLocationInformationDto.cmlArInvoiceLocation,
					cmlArInvoicePerShipmentLine = eRPOrganizationLocationInformationDto.cmlArInvoicePerShipmentLine,
					cmlAvalaraAddressValidated = eRPOrganizationLocationInformationDto.cmlAvalaraAddressValidated,
					cmlBareCostOfDuty = eRPOrganizationLocationInformationDto.cmlBareCostOfDuty,
					cmlBareTransportationCost = eRPOrganizationLocationInformationDto.cmlBareTransportationCost,
					cmlContractor = eRPOrganizationLocationInformationDto.cmlContractor,
					cmlCreatedFromMobile = eRPOrganizationLocationInformationDto.cmlCreatedFromMobile,
					cmlCreditCheckForLocation = eRPOrganizationLocationInformationDto.cmlCreditCheckForLocation,
					cmlCreditHold = eRPOrganizationLocationInformationDto.cmlCreditHold,
					cmlCustomerTaxable = eRPOrganizationLocationInformationDto.cmlCustomerTaxable,
					cmlDirectPayment = eRPOrganizationLocationInformationDto.cmlDirectPayment,
					cmlEdiIntegrated = eRPOrganizationLocationInformationDto.cmlEdiIntegrated,
					cmlIgnoreAvalara = eRPOrganizationLocationInformationDto.cmlIgnoreAvalara,
					cmlPurchaseLocation = eRPOrganizationLocationInformationDto.cmlPurchaseLocation,
					cmlQuoteLocation = eRPOrganizationLocationInformationDto.cmlQuoteLocation,
					cmlResidentialAddress = eRPOrganizationLocationInformationDto.cmlResidentialAddress,
					cmlShipLocation = eRPOrganizationLocationInformationDto.cmlShipLocation,
					cmlTaxReportable = eRPOrganizationLocationInformationDto.cmlTaxReportable,
					cmlUpsValidated = eRPOrganizationLocationInformationDto.cmlUpsValidated,
					cmlLastName = eRPOrganizationLocationInformationDto.cmlLastName,
					cmlLocationID = eRPOrganizationLocationInformationDto.cmlLocationID,
					cmlName = eRPOrganizationLocationInformationDto.cmlName,
					cmlNonTaxReasonID = eRPOrganizationLocationInformationDto.cmlNonTaxReasonID,
					cmlOrganizationID = eRPOrganizationLocationInformationDto.cmlOrganizationID,
					cmlPhoneNumber = eRPOrganizationLocationInformationDto.cmlPhoneNumber,
					cmlPostCode = eRPOrganizationLocationInformationDto.cmlPostCode,
					cmlPurchaseContactID = eRPOrganizationLocationInformationDto.cmlPurchaseContactID,
					cmlQuoteContactID = eRPOrganizationLocationInformationDto.cmlQuoteContactID,
					cmlRowVersion = eRPOrganizationLocationInformationDto.cmlRowVersion,
					cmlSecondGivenName = eRPOrganizationLocationInformationDto.cmlSecondGivenName,
					cmlShipContactID = eRPOrganizationLocationInformationDto.cmlShipContactID,
					cmlSplitPercentTotal = eRPOrganizationLocationInformationDto.cmlSplitPercentTotal,
					cmlState = eRPOrganizationLocationInformationDto.cmlState,
					cmlSupplierPaymentTermID = eRPOrganizationLocationInformationDto.cmlSupplierPaymentTermID,
					cmlSupplierShippingMethodID = eRPOrganizationLocationInformationDto.cmlSupplierShippingMethodID,
					cmlTaxExemptNumber = eRPOrganizationLocationInformationDto.cmlTaxExemptNumber,
					cmlTradingName = eRPOrganizationLocationInformationDto.cmlTradingName,
					cmlUps3rdPartyLocationID = eRPOrganizationLocationInformationDto.cmlUps3rdPartyLocationID,
					cmlUps3rdPartyOrganizationID = eRPOrganizationLocationInformationDto.cmlUps3rdPartyOrganizationID,
					cmlUpsAcctNumber = eRPOrganizationLocationInformationDto.cmlUpsAcctNumber,
					cmlUpsBillingOption = eRPOrganizationLocationInformationDto.cmlUpsBillingOption,
					cmlUpsWsBillingOption = eRPOrganizationLocationInformationDto.cmlUpsWsBillingOption,
					CustomFields = eRPOrganizationLocationInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the OrganizationLocations []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationLocationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = organizationLocationDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationLocationDto>> Process_PutOrganizationLocation(ERPOrganizationLocationDto organizationLocation)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPOrganizationLocationDto createdObject = null;
		ERPResponseMessageDto<ERPOrganizationLocationDto> result;
		try
		{
			IERPOrganizationLocationRepository iERPOrganizationLocationRepository = (base.ERPOrganizationLocationRepository = new ERPOrganizationLocationRepository(base.ApiClientContext));
			using (iERPOrganizationLocationRepository)
			{
				APIValidationInfoDto postResult = await base.ERPOrganizationLocationRepository.SaveOrganizationLocation(organizationLocation);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPOrganizationLocationInformationDto eRPOrganizationLocationInformationDto = await base.ERPOrganizationLocationRepository.GetOrganizationLocation(organizationLocation.cmlUniqueID);
					createdObject = new ERPOrganizationLocationDto
					{
						cmlAddressLine1 = eRPOrganizationLocationInformationDto.cmlAddressLine1,
						cmlAddressLine2 = eRPOrganizationLocationInformationDto.cmlAddressLine2,
						cmlAddressLine3 = eRPOrganizationLocationInformationDto.cmlAddressLine3,
						cmlAddressValidationResult = eRPOrganizationLocationInformationDto.cmlAddressValidationResult,
						cmlAlternatePhoneNumber = eRPOrganizationLocationInformationDto.cmlAlternatePhoneNumber,
						cmlApInvoiceContactID = eRPOrganizationLocationInformationDto.cmlApInvoiceContactID,
						cmlArInvoiceContactID = eRPOrganizationLocationInformationDto.cmlArInvoiceContactID,
						cmlAvalaraUseCodes = eRPOrganizationLocationInformationDto.cmlAvalaraUseCodes,
						cmlBankAccountName = eRPOrganizationLocationInformationDto.cmlBankAccountName,
						cmlBankAccountNumber = eRPOrganizationLocationInformationDto.cmlBankAccountNumber,
						cmlBankAccountType = eRPOrganizationLocationInformationDto.cmlBankAccountType,
						cmlBankInitials = eRPOrganizationLocationInformationDto.cmlBankInitials,
						cmlBic = eRPOrganizationLocationInformationDto.cmlBic,
						cmlBsbNumber = eRPOrganizationLocationInformationDto.cmlBsbNumber,
						cmlCity = eRPOrganizationLocationInformationDto.cmlCity,
						cmlCountry = eRPOrganizationLocationInformationDto.cmlCountry,
						cmlCountryCode = eRPOrganizationLocationInformationDto.cmlCountryCode,
						cmlCounty = eRPOrganizationLocationInformationDto.cmlCounty,
						cmlCreatedBy = eRPOrganizationLocationInformationDto.cmlCreatedBy,
						cmlCreatedDate = eRPOrganizationLocationInformationDto.cmlCreatedDate,
						cmlCurrencyRateID = eRPOrganizationLocationInformationDto.cmlCurrencyRateID,
						cmlCustomerCreditLimit = eRPOrganizationLocationInformationDto.cmlCustomerCreditLimit,
						cmlCustomerPaymentTermID = eRPOrganizationLocationInformationDto.cmlCustomerPaymentTermID,
						cmlCustomerSecondTaxCodeID = eRPOrganizationLocationInformationDto.cmlCustomerSecondTaxCodeID,
						cmlCustomerShipPaymentTypeID = eRPOrganizationLocationInformationDto.cmlCustomerShipPaymentTypeID,
						cmlCustomerShippingCarrier = eRPOrganizationLocationInformationDto.cmlCustomerShippingCarrier,
						cmlCustomerShippingMethodID = eRPOrganizationLocationInformationDto.cmlCustomerShippingMethodID,
						cmlCustomerTaxCodeID = eRPOrganizationLocationInformationDto.cmlCustomerTaxCodeID,
						cmlEdiLocationID = eRPOrganizationLocationInformationDto.cmlEdiLocationID,
						cmlEftCode = eRPOrganizationLocationInformationDto.cmlEftCode,
						cmlEftDescription = eRPOrganizationLocationInformationDto.cmlEftDescription,
						cmlEftParticulars = eRPOrganizationLocationInformationDto.cmlEftParticulars,
						cmlEmailAddress = eRPOrganizationLocationInformationDto.cmlEmailAddress,
						cmlUniqueID = eRPOrganizationLocationInformationDto.cmlUniqueID,
						cmlFaxNumber = eRPOrganizationLocationInformationDto.cmlFaxNumber,
						cmlFedEx3rdPartyLocationID = eRPOrganizationLocationInformationDto.cmlFedEx3rdPartyLocationID,
						cmlFedEx3rdPartyOrganizationID = eRPOrganizationLocationInformationDto.cmlFedEx3rdPartyOrganizationID,
						cmlFedExAccountNumber = eRPOrganizationLocationInformationDto.cmlFedExAccountNumber,
						cmlFedExBillingOption = eRPOrganizationLocationInformationDto.cmlFedExBillingOption,
						cmlFinanceOrganizationID = eRPOrganizationLocationInformationDto.cmlFinanceOrganizationID,
						cmlFirstGivenName = eRPOrganizationLocationInformationDto.cmlFirstGivenName,
						cmlFreeOnBoardDescription = eRPOrganizationLocationInformationDto.cmlFreeOnBoardDescription,
						cmlHdAttachmentFilePath = eRPOrganizationLocationInformationDto.cmlHdAttachmentFilePath,
						cmlIban = eRPOrganizationLocationInformationDto.cmlIban,
						cmlInactiveDate = eRPOrganizationLocationInformationDto.cmlInactiveDate,
						cmlInactive = eRPOrganizationLocationInformationDto.cmlInactive,
						cmlApInvoiceLocation = eRPOrganizationLocationInformationDto.cmlApInvoiceLocation,
						cmlArInvoiceLocation = eRPOrganizationLocationInformationDto.cmlArInvoiceLocation,
						cmlArInvoicePerShipmentLine = eRPOrganizationLocationInformationDto.cmlArInvoicePerShipmentLine,
						cmlAvalaraAddressValidated = eRPOrganizationLocationInformationDto.cmlAvalaraAddressValidated,
						cmlBareCostOfDuty = eRPOrganizationLocationInformationDto.cmlBareCostOfDuty,
						cmlBareTransportationCost = eRPOrganizationLocationInformationDto.cmlBareTransportationCost,
						cmlContractor = eRPOrganizationLocationInformationDto.cmlContractor,
						cmlCreatedFromMobile = eRPOrganizationLocationInformationDto.cmlCreatedFromMobile,
						cmlCreditCheckForLocation = eRPOrganizationLocationInformationDto.cmlCreditCheckForLocation,
						cmlCreditHold = eRPOrganizationLocationInformationDto.cmlCreditHold,
						cmlCustomerTaxable = eRPOrganizationLocationInformationDto.cmlCustomerTaxable,
						cmlDirectPayment = eRPOrganizationLocationInformationDto.cmlDirectPayment,
						cmlEdiIntegrated = eRPOrganizationLocationInformationDto.cmlEdiIntegrated,
						cmlIgnoreAvalara = eRPOrganizationLocationInformationDto.cmlIgnoreAvalara,
						cmlPurchaseLocation = eRPOrganizationLocationInformationDto.cmlPurchaseLocation,
						cmlQuoteLocation = eRPOrganizationLocationInformationDto.cmlQuoteLocation,
						cmlResidentialAddress = eRPOrganizationLocationInformationDto.cmlResidentialAddress,
						cmlShipLocation = eRPOrganizationLocationInformationDto.cmlShipLocation,
						cmlTaxReportable = eRPOrganizationLocationInformationDto.cmlTaxReportable,
						cmlUpsValidated = eRPOrganizationLocationInformationDto.cmlUpsValidated,
						cmlLastName = eRPOrganizationLocationInformationDto.cmlLastName,
						cmlLocationID = eRPOrganizationLocationInformationDto.cmlLocationID,
						cmlName = eRPOrganizationLocationInformationDto.cmlName,
						cmlNonTaxReasonID = eRPOrganizationLocationInformationDto.cmlNonTaxReasonID,
						cmlOrganizationID = eRPOrganizationLocationInformationDto.cmlOrganizationID,
						cmlPhoneNumber = eRPOrganizationLocationInformationDto.cmlPhoneNumber,
						cmlPostCode = eRPOrganizationLocationInformationDto.cmlPostCode,
						cmlPurchaseContactID = eRPOrganizationLocationInformationDto.cmlPurchaseContactID,
						cmlQuoteContactID = eRPOrganizationLocationInformationDto.cmlQuoteContactID,
						cmlRowVersion = eRPOrganizationLocationInformationDto.cmlRowVersion,
						cmlSecondGivenName = eRPOrganizationLocationInformationDto.cmlSecondGivenName,
						cmlShipContactID = eRPOrganizationLocationInformationDto.cmlShipContactID,
						cmlSplitPercentTotal = eRPOrganizationLocationInformationDto.cmlSplitPercentTotal,
						cmlState = eRPOrganizationLocationInformationDto.cmlState,
						cmlSupplierPaymentTermID = eRPOrganizationLocationInformationDto.cmlSupplierPaymentTermID,
						cmlSupplierShippingMethodID = eRPOrganizationLocationInformationDto.cmlSupplierShippingMethodID,
						cmlTaxExemptNumber = eRPOrganizationLocationInformationDto.cmlTaxExemptNumber,
						cmlTradingName = eRPOrganizationLocationInformationDto.cmlTradingName,
						cmlUps3rdPartyLocationID = eRPOrganizationLocationInformationDto.cmlUps3rdPartyLocationID,
						cmlUps3rdPartyOrganizationID = eRPOrganizationLocationInformationDto.cmlUps3rdPartyOrganizationID,
						cmlUpsAcctNumber = eRPOrganizationLocationInformationDto.cmlUpsAcctNumber,
						cmlUpsBillingOption = eRPOrganizationLocationInformationDto.cmlUpsBillingOption,
						cmlUpsWsBillingOption = eRPOrganizationLocationInformationDto.cmlUpsWsBillingOption,
						CustomFields = eRPOrganizationLocationInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing OrganizationLocation [{organizationLocation.cmlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationLocationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteOrganizationLocation(Guid organizationLocationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationLocationRepository iERPOrganizationLocationRepository = (base.ERPOrganizationLocationRepository = new ERPOrganizationLocationRepository(base.ApiClientContext));
		using (iERPOrganizationLocationRepository)
		{
			if (!(await base.ERPOrganizationLocationRepository.DoesOrganizationLocationExist(organizationLocationId)))
			{
				base.ErrorsList.Add($"OrganizationLocation [{organizationLocationId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPOrganizationLocationInformationDto eRPOrganizationLocationInformationDto = await base.ERPOrganizationLocationRepository.GetOrganizationLocation(organizationLocationId);
				string text = await base.ERPOrganizationLocationRepository.WhereUsed("OrganizationLocations", new object[2] { eRPOrganizationLocationInformationDto.cmlOrganizationID, eRPOrganizationLocationInformationDto.cmlLocationID }, new object[2] { "cmlOrganizationID", "cmlLocationID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("OrganizationLocation cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationLocationDto>> Process_DeleteOrganizationLocation(Guid organizationLocationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPOrganizationLocationDto> result;
		try
		{
			IERPOrganizationLocationRepository iERPOrganizationLocationRepository = (base.ERPOrganizationLocationRepository = new ERPOrganizationLocationRepository(base.ApiClientContext));
			using (iERPOrganizationLocationRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPOrganizationLocationRepository.DeleteRowFromTable("OrganizationLocations", "cml", organizationLocationId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of OrganizationLocation [{organizationLocationId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationLocationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPOrganizationLocationDto()
			};
		}
		return result;
	}
}
