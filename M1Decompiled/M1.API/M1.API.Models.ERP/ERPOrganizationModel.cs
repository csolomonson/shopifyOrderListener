using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPOrganizationModel : ERPBaseModel, IERPOrganizationModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPOrganizationRepository iERPOrganizationRepository = (base.ERPOrganizationRepository = new ERPOrganizationRepository(base.ApiClientContext));
		using (iERPOrganizationRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPOrganizationRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPOrganizationRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPOrganizationRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPOrganizationRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetOrganization(Guid organizationId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationRepository iERPOrganizationRepository = (base.ERPOrganizationRepository = new ERPOrganizationRepository(base.ApiClientContext));
		using (iERPOrganizationRepository)
		{
			if (!(await base.ERPOrganizationRepository.DoesOrganizationExist(organizationId)))
			{
				errorsList.Add($"Organization [{organizationId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutOrganization(ERPOrganizationDto organization)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationRepository iERPOrganizationRepository = (base.ERPOrganizationRepository = new ERPOrganizationRepository(base.ApiClientContext));
		using (iERPOrganizationRepository)
		{
			if (!string.IsNullOrWhiteSpace(organization.cmoArInvoiceContactID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[2] { organization.cmoOrganizationID, organization.cmoArInvoiceContactID })))
			{
				errorsList.Add("cmoArInvoiceContactID [" + organization.cmoArInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoQuoteContactID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[2] { organization.cmoOrganizationID, organization.cmoQuoteContactID })))
			{
				errorsList.Add("cmoQuoteContactID [" + organization.cmoQuoteContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoShipContactID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[2] { organization.cmoOrganizationID, organization.cmoShipContactID })))
			{
				errorsList.Add("cmoShipContactID [" + organization.cmoShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoPurchaseContactID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[2] { organization.cmoOrganizationID, organization.cmoPurchaseContactID })))
			{
				errorsList.Add("cmoPurchaseContactID [" + organization.cmoPurchaseContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoApInvoiceContactID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[2] { organization.cmoOrganizationID, organization.cmoApInvoiceContactID })))
			{
				errorsList.Add("cmoApInvoiceContactID [" + organization.cmoApInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoSupplierPaymentTermID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("PaymentTerms", new object[1] { "XATPAYMENTTERMID" }, new object[1] { organization.cmoSupplierPaymentTermID })))
			{
				errorsList.Add("cmoSupplierPaymentTermID [" + organization.cmoSupplierPaymentTermID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoSupplierShippingMethodID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { organization.cmoSupplierShippingMethodID })))
			{
				errorsList.Add("cmoSupplierShippingMethodID [" + organization.cmoSupplierShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoSupplierTaxCodeID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { organization.cmoSupplierTaxCodeID })))
			{
				errorsList.Add("cmoSupplierTaxCodeID [" + organization.cmoSupplierTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoSupplierSecondTaxCodeID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { organization.cmoSupplierSecondTaxCodeID })))
			{
				errorsList.Add("cmoSupplierSecondTaxCodeID [" + organization.cmoSupplierSecondTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoAccountManagerEmployeeID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { organization.cmoAccountManagerEmployeeID })))
			{
				errorsList.Add("cmoAccountManagerEmployeeID [" + organization.cmoAccountManagerEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoCustomerGroupID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("CustomerGroups", new object[1] { "CMUCUSTOMERGROUPID" }, new object[1] { organization.cmoCustomerGroupID })))
			{
				errorsList.Add("cmoCustomerGroupID [" + organization.cmoCustomerGroupID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoCustomerTaxCodeID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { organization.cmoCustomerTaxCodeID })))
			{
				errorsList.Add("cmoCustomerTaxCodeID [" + organization.cmoCustomerTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoCustomerSecondTaxCodeID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { organization.cmoCustomerSecondTaxCodeID })))
			{
				errorsList.Add("cmoCustomerSecondTaxCodeID [" + organization.cmoCustomerSecondTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoCustomerPaymentTermsID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("PaymentTerms", new object[1] { "XATPAYMENTTERMID" }, new object[1] { organization.cmoCustomerPaymentTermsID })))
			{
				errorsList.Add("cmoCustomerPaymentTermsID [" + organization.cmoCustomerPaymentTermsID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoCurrencyRateID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { organization.cmoCurrencyRateID })))
			{
				errorsList.Add("cmoCurrencyRateID [" + organization.cmoCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoDefaultQuoteLocationID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organization.cmoOrganizationID, organization.cmoDefaultQuoteLocationID })))
			{
				errorsList.Add("cmoDefaultQuoteLocationID [" + organization.cmoDefaultQuoteLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoDefaultShipLocationID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organization.cmoOrganizationID, organization.cmoDefaultShipLocationID })))
			{
				errorsList.Add("cmoDefaultShipLocationID [" + organization.cmoDefaultShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoDefaultArInvoiceLocationID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organization.cmoOrganizationID, organization.cmoDefaultArInvoiceLocationID })))
			{
				errorsList.Add("cmoDefaultArInvoiceLocationID [" + organization.cmoDefaultArInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoDefaultPurchaseLocationID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organization.cmoOrganizationID, organization.cmoDefaultPurchaseLocationID })))
			{
				errorsList.Add("cmoDefaultPurchaseLocationID [" + organization.cmoDefaultPurchaseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoDefaultApInvoiceLocationID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organization.cmoOrganizationID, organization.cmoDefaultApInvoiceLocationID })))
			{
				errorsList.Add("cmoDefaultApInvoiceLocationID [" + organization.cmoDefaultApInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoDropShipOrganizationID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { organization.cmoDropShipOrganizationID })))
			{
				errorsList.Add("cmoDropShipOrganizationID [" + organization.cmoDropShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoDropShipLocationID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organization.cmoDropShipOrganizationID, organization.cmoDropShipLocationID })))
			{
				errorsList.Add("cmoDropShipLocationID [" + organization.cmoDropShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoNonTaxReasonID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { organization.cmoNonTaxReasonID })))
			{
				errorsList.Add("cmoNonTaxReasonID [" + organization.cmoNonTaxReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoCustomerShippingMethodID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { organization.cmoCustomerShippingMethodID })))
			{
				errorsList.Add("cmoCustomerShippingMethodID [" + organization.cmoCustomerShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoCustomerShipPaymentTypeID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("ShippingPaymentTypes", new object[1] { "XAYSHIPPINGPAYMENTTYPEID" }, new object[1] { organization.cmoCustomerShipPaymentTypeID })))
			{
				errorsList.Add("cmoCustomerShipPaymentTypeID [" + organization.cmoCustomerShipPaymentTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoResellerOrganizationID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { organization.cmoResellerOrganizationID })))
			{
				errorsList.Add("cmoResellerOrganizationID [" + organization.cmoResellerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoResellerLocationID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organization.cmoResellerOrganizationID, organization.cmoResellerLocationID })))
			{
				errorsList.Add("cmoResellerLocationID [" + organization.cmoResellerLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoResellerContactID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { organization.cmoResellerOrganizationID, organization.cmoResellerLocationID, organization.cmoResellerContactID })))
			{
				errorsList.Add("cmoResellerContactID [" + organization.cmoResellerContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoSupplierRatingID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("SupplierRatings", new object[1] { "CMSSUPPLIERRATINGID" }, new object[1] { organization.cmoSupplierRatingID })))
			{
				errorsList.Add("cmoSupplierRatingID [" + organization.cmoSupplierRatingID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoUps3rdPartyOrganizationID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { organization.cmoUps3rdPartyOrganizationID })))
			{
				errorsList.Add("cmoUps3rdPartyOrganizationID [" + organization.cmoUps3rdPartyOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoUps3rdPartyLocationID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organization.cmoUps3rdPartyOrganizationID, organization.cmoUps3rdPartyLocationID })))
			{
				errorsList.Add("cmoUps3rdPartyLocationID [" + organization.cmoUps3rdPartyLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoFedEx3rdPartyOrganizationID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONS", new object[1] { "CMOORGANIZATIONID" }, new object[1] { organization.cmoFedEx3rdPartyOrganizationID })))
			{
				errorsList.Add("cmoFedEx3rdPartyOrganizationID [" + organization.cmoFedEx3rdPartyOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organization.cmoFedEx3rdPartyLocationID) && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONLOCATIONS", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organization.cmoFedEx3rdPartyOrganizationID, organization.cmoFedEx3rdPartyLocationID })))
			{
				errorsList.Add("cmoFedEx3rdPartyLocationID [" + organization.cmoFedEx3rdPartyLocationID + "] not found.");
			}
			if (organization.cmoJobPriorityID > 0 && !(await base.ERPOrganizationRepository.DoesRecordExistInTableUsingKeys("JobPriorities", new object[1] { "JMJJOBPRIORITYID" }, new object[1] { organization.cmoJobPriorityID })))
			{
				errorsList.Add($"cmoJobPriorityID [{organization.cmoJobPriorityID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPOrganizationDto>>> Process_GetAllOrganizations(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPOrganizationDto> allOrganizationsDto = new List<ERPOrganizationDto>();
		ERPResponseMessageDto<IList<ERPOrganizationDto>> result;
		try
		{
			IERPOrganizationRepository iERPOrganizationRepository = (base.ERPOrganizationRepository = new ERPOrganizationRepository(base.ApiClientContext));
			using (iERPOrganizationRepository)
			{
				foreach (ERPOrganizationInformationDto item2 in await base.ERPOrganizationRepository.GetAllOrganizations(pageSize, pageNumber, filter, orderBy))
				{
					ERPOrganizationDto item = new ERPOrganizationDto
					{
						cmoAccountManagerEmployeeID = item2.cmoAccountManagerEmployeeID,
						cmoAddressLine1 = item2.cmoAddressLine1,
						cmoAddressLine2 = item2.cmoAddressLine2,
						cmoAddressLine3 = item2.cmoAddressLine3,
						cmoAddressValidationResult = item2.cmoAddressValidationResult,
						cmoAlternatePhoneNumber = item2.cmoAlternatePhoneNumber,
						cmoApInvoiceContactID = item2.cmoApInvoiceContactID,
						cmoArInvoiceContactID = item2.cmoArInvoiceContactID,
						cmoAttachmentFileFolder = item2.cmoAttachmentFileFolder,
						cmoAvalaraUseCodes = item2.cmoAvalaraUseCodes,
						cmoBankAccountName = item2.cmoBankAccountName,
						cmoBankAccountNumber = item2.cmoBankAccountNumber,
						cmoBankAccountType = item2.cmoBankAccountType,
						cmoBankInitials = item2.cmoBankInitials,
						cmoBic = item2.cmoBic,
						cmoBsbNumber = item2.cmoBsbNumber,
						cmoCity = item2.cmoCity,
						cmoOrganizationID = item2.cmoOrganizationID,
						cmoCompanyEntryDescription = item2.cmoCompanyEntryDescription,
						cmoCountry = item2.cmoCountry,
						cmoCountryCode = item2.cmoCountryCode,
						cmoCounty = item2.cmoCounty,
						cmoCreatedBy = item2.cmoCreatedBy,
						cmoCreatedDate = item2.cmoCreatedDate,
						cmoCurrencyRateID = item2.cmoCurrencyRateID,
						cmoCustomerActiveDate = item2.cmoCustomerActiveDate,
						cmoCustomerCreditLimit = item2.cmoCustomerCreditLimit,
						cmoCustomerGroupID = item2.cmoCustomerGroupID,
						cmoCustomerInactiveDate = item2.cmoCustomerInactiveDate,
						cmoCustomerPaymentTermsID = item2.cmoCustomerPaymentTermsID,
						cmoCustomerProspectDate = item2.cmoCustomerProspectDate,
						cmoCustomerSecondTaxCodeID = item2.cmoCustomerSecondTaxCodeID,
						cmoCustomerShipPaymentTypeID = item2.cmoCustomerShipPaymentTypeID,
						cmoCustomerShippingCarrier = item2.cmoCustomerShippingCarrier,
						cmoCustomerShippingMethodID = item2.cmoCustomerShippingMethodID,
						cmoCustomerStatus = item2.cmoCustomerStatus,
						cmoCustomerTaxCodeID = item2.cmoCustomerTaxCodeID,
						cmoDefaultApInvoiceLocationID = item2.cmoDefaultApInvoiceLocationID,
						cmoDefaultArInvoiceLocationID = item2.cmoDefaultArInvoiceLocationID,
						cmoDefaultPurchaseLocationID = item2.cmoDefaultPurchaseLocationID,
						cmoDefaultQuoteLocationID = item2.cmoDefaultQuoteLocationID,
						cmoDefaultShipLocationID = item2.cmoDefaultShipLocationID,
						cmoDropShipLocationID = item2.cmoDropShipLocationID,
						cmoDropShipOrganizationID = item2.cmoDropShipOrganizationID,
						cmoEftCode = item2.cmoEftCode,
						cmoEftDescription = item2.cmoEftDescription,
						cmoEftParticulars = item2.cmoEftParticulars,
						cmoEmailAddress = item2.cmoEmailAddress,
						cmoEmployeeCount = item2.cmoEmployeeCount,
						cmoUniqueID = item2.cmoUniqueID,
						cmoEstablishedDate = item2.cmoEstablishedDate,
						cmoExpenseSplitPercentTotal = item2.cmoExpenseSplitPercentTotal,
						cmoFaxNumber = item2.cmoFaxNumber,
						cmoFederalID = item2.cmoFederalID,
						cmoFedEx3rdPartyLocationID = item2.cmoFedEx3rdPartyLocationID,
						cmoFedEx3rdPartyOrganizationID = item2.cmoFedEx3rdPartyOrganizationID,
						cmoFedExAccountNumber = item2.cmoFedExAccountNumber,
						cmoFedExBillingOption = item2.cmoFedExBillingOption,
						cmoFirstGivenName = item2.cmoFirstGivenName,
						cmoForm1099Box = item2.cmoForm1099Box,
						cmoFreeOnBoardDescription = item2.cmoFreeOnBoardDescription,
						cmoHdAttachmentFilePath = item2.cmoHdAttachmentFilePath,
						cmoIban = item2.cmoIban,
						cmoIntraCompanyDatasetID = item2.cmoIntraCompanyDatasetID,
						cmoApIncludeTaxInRetention = item2.cmoApIncludeTaxInRetention,
						cmoArIncludeTaxInRetention = item2.cmoArIncludeTaxInRetention,
						cmoArInvoicePerShipmentLine = item2.cmoArInvoicePerShipmentLine,
						cmoAvalaraAddressValidated = item2.cmoAvalaraAddressValidated,
						cmoBareCostOfDuty = item2.cmoBareCostOfDuty,
						cmoBareTransportationCost = item2.cmoBareTransportationCost,
						cmoCalculateFinanceCharges = item2.cmoCalculateFinanceCharges,
						cmoCompetitor = item2.cmoCompetitor,
						cmoContractor = item2.cmoContractor,
						cmoCreatedFromMobile = item2.cmoCreatedFromMobile,
						cmoCreditHold = item2.cmoCreditHold,
						cmoCustomerTaxable = item2.cmoCustomerTaxable,
						cmoDirectPayment = item2.cmoDirectPayment,
						cmoEdiIntegrated = item2.cmoEdiIntegrated,
						cmoFinanceCompany = item2.cmoFinanceCompany,
						cmoIgnoreAvalara = item2.cmoIgnoreAvalara,
						cmoIncludeFreightInPrice = item2.cmoIncludeFreightInPrice,
						cmoPrintStatement = item2.cmoPrintStatement,
						cmoRequires1099 = item2.cmoRequires1099,
						cmoRequiresInspection = item2.cmoRequiresInspection,
						cmoResidentialAddress = item2.cmoResidentialAddress,
						cmoSuperFund = item2.cmoSuperFund,
						cmoSupplierAccredited = item2.cmoSupplierAccredited,
						cmoSupplierTaxable = item2.cmoSupplierTaxable,
						cmoTaxReportable = item2.cmoTaxReportable,
						cmoUpsValidated = item2.cmoUpsValidated,
						cmoJobPriorityID = item2.cmoJobPriorityID,
						cmoLastName = item2.cmoLastName,
						cmoLongDescriptionRtf = item2.cmoLongDescriptionRtf,
						cmoLongDescriptionText = item2.cmoLongDescriptionText,
						cmoName = item2.cmoName,
						cmoNonTaxReasonID = item2.cmoNonTaxReasonID,
						cmoOrganizationAccountID = item2.cmoOrganizationAccountID,
						cmoPhoneNumber = item2.cmoPhoneNumber,
						cmoPostCode = item2.cmoPostCode,
						cmoPurchaseContactID = item2.cmoPurchaseContactID,
						cmoQuoteContactID = item2.cmoQuoteContactID,
						cmoResellerActiveDate = item2.cmoResellerActiveDate,
						cmoResellerCommissionRate = item2.cmoResellerCommissionRate,
						cmoResellerContactID = item2.cmoResellerContactID,
						cmoResellerInactiveDate = item2.cmoResellerInactiveDate,
						cmoResellerLocationID = item2.cmoResellerLocationID,
						cmoResellerOrganizationID = item2.cmoResellerOrganizationID,
						cmoResellerProspectDate = item2.cmoResellerProspectDate,
						cmoResellerStatus = item2.cmoResellerStatus,
						cmoRowVersion = item2.cmoRowVersion,
						cmoSecondGivenName = item2.cmoSecondGivenName,
						cmoShipContactID = item2.cmoShipContactID,
						cmoSplitPercentTotal = item2.cmoSplitPercentTotal,
						cmoState = item2.cmoState,
						cmoSuperFundEmployerID = item2.cmoSuperFundEmployerID,
						cmoSuperFundName = item2.cmoSuperFundName,
						cmoSupplierAccreditedDate = item2.cmoSupplierAccreditedDate,
						cmoSupplierActiveDate = item2.cmoSupplierActiveDate,
						cmoSupplierInactiveDate = item2.cmoSupplierInactiveDate,
						cmoSupplierPaymentTermID = item2.cmoSupplierPaymentTermID,
						cmoSupplierProspectDate = item2.cmoSupplierProspectDate,
						cmoSupplierRatingID = item2.cmoSupplierRatingID,
						cmoSupplierSecondTaxCodeID = item2.cmoSupplierSecondTaxCodeID,
						cmoSupplierShippingMethodID = item2.cmoSupplierShippingMethodID,
						cmoSupplierStatus = item2.cmoSupplierStatus,
						cmoSupplierTaxCodeID = item2.cmoSupplierTaxCodeID,
						cmoTaxExemptNumber = item2.cmoTaxExemptNumber,
						cmoTradingName = item2.cmoTradingName,
						cmoUps3rdPartyLocationID = item2.cmoUps3rdPartyLocationID,
						cmoUps3rdPartyOrganizationID = item2.cmoUps3rdPartyOrganizationID,
						cmoUpsAcctNumber = item2.cmoUpsAcctNumber,
						cmoUpsBillingOption = item2.cmoUpsBillingOption,
						cmoUpsWsBillingOption = item2.cmoUpsWsBillingOption,
						cmoUsaTransactionTypeCode = item2.cmoUsaTransactionTypeCode,
						cmoWebAddress = item2.cmoWebAddress,
						CustomFields = item2.CustomFields
					};
					allOrganizationsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Organizations]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPOrganizationDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allOrganizationsDto,
				RecordCount = allOrganizationsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationDto>> Process_GetOrganization(Guid organizationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPOrganizationDto organizationDto = null;
		ERPResponseMessageDto<ERPOrganizationDto> result;
		try
		{
			IERPOrganizationRepository iERPOrganizationRepository = (base.ERPOrganizationRepository = new ERPOrganizationRepository(base.ApiClientContext));
			using (iERPOrganizationRepository)
			{
				ERPOrganizationInformationDto eRPOrganizationInformationDto = await base.ERPOrganizationRepository.GetOrganization(organizationId);
				organizationDto = new ERPOrganizationDto
				{
					cmoAccountManagerEmployeeID = eRPOrganizationInformationDto.cmoAccountManagerEmployeeID,
					cmoAddressLine1 = eRPOrganizationInformationDto.cmoAddressLine1,
					cmoAddressLine2 = eRPOrganizationInformationDto.cmoAddressLine2,
					cmoAddressLine3 = eRPOrganizationInformationDto.cmoAddressLine3,
					cmoAddressValidationResult = eRPOrganizationInformationDto.cmoAddressValidationResult,
					cmoAlternatePhoneNumber = eRPOrganizationInformationDto.cmoAlternatePhoneNumber,
					cmoApInvoiceContactID = eRPOrganizationInformationDto.cmoApInvoiceContactID,
					cmoArInvoiceContactID = eRPOrganizationInformationDto.cmoArInvoiceContactID,
					cmoAttachmentFileFolder = eRPOrganizationInformationDto.cmoAttachmentFileFolder,
					cmoAvalaraUseCodes = eRPOrganizationInformationDto.cmoAvalaraUseCodes,
					cmoBankAccountName = eRPOrganizationInformationDto.cmoBankAccountName,
					cmoBankAccountNumber = eRPOrganizationInformationDto.cmoBankAccountNumber,
					cmoBankAccountType = eRPOrganizationInformationDto.cmoBankAccountType,
					cmoBankInitials = eRPOrganizationInformationDto.cmoBankInitials,
					cmoBic = eRPOrganizationInformationDto.cmoBic,
					cmoBsbNumber = eRPOrganizationInformationDto.cmoBsbNumber,
					cmoCity = eRPOrganizationInformationDto.cmoCity,
					cmoOrganizationID = eRPOrganizationInformationDto.cmoOrganizationID,
					cmoCompanyEntryDescription = eRPOrganizationInformationDto.cmoCompanyEntryDescription,
					cmoCountry = eRPOrganizationInformationDto.cmoCountry,
					cmoCountryCode = eRPOrganizationInformationDto.cmoCountryCode,
					cmoCounty = eRPOrganizationInformationDto.cmoCounty,
					cmoCreatedBy = eRPOrganizationInformationDto.cmoCreatedBy,
					cmoCreatedDate = eRPOrganizationInformationDto.cmoCreatedDate,
					cmoCurrencyRateID = eRPOrganizationInformationDto.cmoCurrencyRateID,
					cmoCustomerActiveDate = eRPOrganizationInformationDto.cmoCustomerActiveDate,
					cmoCustomerCreditLimit = eRPOrganizationInformationDto.cmoCustomerCreditLimit,
					cmoCustomerGroupID = eRPOrganizationInformationDto.cmoCustomerGroupID,
					cmoCustomerInactiveDate = eRPOrganizationInformationDto.cmoCustomerInactiveDate,
					cmoCustomerPaymentTermsID = eRPOrganizationInformationDto.cmoCustomerPaymentTermsID,
					cmoCustomerProspectDate = eRPOrganizationInformationDto.cmoCustomerProspectDate,
					cmoCustomerSecondTaxCodeID = eRPOrganizationInformationDto.cmoCustomerSecondTaxCodeID,
					cmoCustomerShipPaymentTypeID = eRPOrganizationInformationDto.cmoCustomerShipPaymentTypeID,
					cmoCustomerShippingCarrier = eRPOrganizationInformationDto.cmoCustomerShippingCarrier,
					cmoCustomerShippingMethodID = eRPOrganizationInformationDto.cmoCustomerShippingMethodID,
					cmoCustomerStatus = eRPOrganizationInformationDto.cmoCustomerStatus,
					cmoCustomerTaxCodeID = eRPOrganizationInformationDto.cmoCustomerTaxCodeID,
					cmoDefaultApInvoiceLocationID = eRPOrganizationInformationDto.cmoDefaultApInvoiceLocationID,
					cmoDefaultArInvoiceLocationID = eRPOrganizationInformationDto.cmoDefaultArInvoiceLocationID,
					cmoDefaultPurchaseLocationID = eRPOrganizationInformationDto.cmoDefaultPurchaseLocationID,
					cmoDefaultQuoteLocationID = eRPOrganizationInformationDto.cmoDefaultQuoteLocationID,
					cmoDefaultShipLocationID = eRPOrganizationInformationDto.cmoDefaultShipLocationID,
					cmoDropShipLocationID = eRPOrganizationInformationDto.cmoDropShipLocationID,
					cmoDropShipOrganizationID = eRPOrganizationInformationDto.cmoDropShipOrganizationID,
					cmoEftCode = eRPOrganizationInformationDto.cmoEftCode,
					cmoEftDescription = eRPOrganizationInformationDto.cmoEftDescription,
					cmoEftParticulars = eRPOrganizationInformationDto.cmoEftParticulars,
					cmoEmailAddress = eRPOrganizationInformationDto.cmoEmailAddress,
					cmoEmployeeCount = eRPOrganizationInformationDto.cmoEmployeeCount,
					cmoUniqueID = eRPOrganizationInformationDto.cmoUniqueID,
					cmoEstablishedDate = eRPOrganizationInformationDto.cmoEstablishedDate,
					cmoExpenseSplitPercentTotal = eRPOrganizationInformationDto.cmoExpenseSplitPercentTotal,
					cmoFaxNumber = eRPOrganizationInformationDto.cmoFaxNumber,
					cmoFederalID = eRPOrganizationInformationDto.cmoFederalID,
					cmoFedEx3rdPartyLocationID = eRPOrganizationInformationDto.cmoFedEx3rdPartyLocationID,
					cmoFedEx3rdPartyOrganizationID = eRPOrganizationInformationDto.cmoFedEx3rdPartyOrganizationID,
					cmoFedExAccountNumber = eRPOrganizationInformationDto.cmoFedExAccountNumber,
					cmoFedExBillingOption = eRPOrganizationInformationDto.cmoFedExBillingOption,
					cmoFirstGivenName = eRPOrganizationInformationDto.cmoFirstGivenName,
					cmoForm1099Box = eRPOrganizationInformationDto.cmoForm1099Box,
					cmoFreeOnBoardDescription = eRPOrganizationInformationDto.cmoFreeOnBoardDescription,
					cmoHdAttachmentFilePath = eRPOrganizationInformationDto.cmoHdAttachmentFilePath,
					cmoIban = eRPOrganizationInformationDto.cmoIban,
					cmoIntraCompanyDatasetID = eRPOrganizationInformationDto.cmoIntraCompanyDatasetID,
					cmoApIncludeTaxInRetention = eRPOrganizationInformationDto.cmoApIncludeTaxInRetention,
					cmoArIncludeTaxInRetention = eRPOrganizationInformationDto.cmoArIncludeTaxInRetention,
					cmoArInvoicePerShipmentLine = eRPOrganizationInformationDto.cmoArInvoicePerShipmentLine,
					cmoAvalaraAddressValidated = eRPOrganizationInformationDto.cmoAvalaraAddressValidated,
					cmoBareCostOfDuty = eRPOrganizationInformationDto.cmoBareCostOfDuty,
					cmoBareTransportationCost = eRPOrganizationInformationDto.cmoBareTransportationCost,
					cmoCalculateFinanceCharges = eRPOrganizationInformationDto.cmoCalculateFinanceCharges,
					cmoCompetitor = eRPOrganizationInformationDto.cmoCompetitor,
					cmoContractor = eRPOrganizationInformationDto.cmoContractor,
					cmoCreatedFromMobile = eRPOrganizationInformationDto.cmoCreatedFromMobile,
					cmoCreditHold = eRPOrganizationInformationDto.cmoCreditHold,
					cmoCustomerTaxable = eRPOrganizationInformationDto.cmoCustomerTaxable,
					cmoDirectPayment = eRPOrganizationInformationDto.cmoDirectPayment,
					cmoEdiIntegrated = eRPOrganizationInformationDto.cmoEdiIntegrated,
					cmoFinanceCompany = eRPOrganizationInformationDto.cmoFinanceCompany,
					cmoIgnoreAvalara = eRPOrganizationInformationDto.cmoIgnoreAvalara,
					cmoIncludeFreightInPrice = eRPOrganizationInformationDto.cmoIncludeFreightInPrice,
					cmoPrintStatement = eRPOrganizationInformationDto.cmoPrintStatement,
					cmoRequires1099 = eRPOrganizationInformationDto.cmoRequires1099,
					cmoRequiresInspection = eRPOrganizationInformationDto.cmoRequiresInspection,
					cmoResidentialAddress = eRPOrganizationInformationDto.cmoResidentialAddress,
					cmoSuperFund = eRPOrganizationInformationDto.cmoSuperFund,
					cmoSupplierAccredited = eRPOrganizationInformationDto.cmoSupplierAccredited,
					cmoSupplierTaxable = eRPOrganizationInformationDto.cmoSupplierTaxable,
					cmoTaxReportable = eRPOrganizationInformationDto.cmoTaxReportable,
					cmoUpsValidated = eRPOrganizationInformationDto.cmoUpsValidated,
					cmoJobPriorityID = eRPOrganizationInformationDto.cmoJobPriorityID,
					cmoLastName = eRPOrganizationInformationDto.cmoLastName,
					cmoLongDescriptionRtf = eRPOrganizationInformationDto.cmoLongDescriptionRtf,
					cmoLongDescriptionText = eRPOrganizationInformationDto.cmoLongDescriptionText,
					cmoName = eRPOrganizationInformationDto.cmoName,
					cmoNonTaxReasonID = eRPOrganizationInformationDto.cmoNonTaxReasonID,
					cmoOrganizationAccountID = eRPOrganizationInformationDto.cmoOrganizationAccountID,
					cmoPhoneNumber = eRPOrganizationInformationDto.cmoPhoneNumber,
					cmoPostCode = eRPOrganizationInformationDto.cmoPostCode,
					cmoPurchaseContactID = eRPOrganizationInformationDto.cmoPurchaseContactID,
					cmoQuoteContactID = eRPOrganizationInformationDto.cmoQuoteContactID,
					cmoResellerActiveDate = eRPOrganizationInformationDto.cmoResellerActiveDate,
					cmoResellerCommissionRate = eRPOrganizationInformationDto.cmoResellerCommissionRate,
					cmoResellerContactID = eRPOrganizationInformationDto.cmoResellerContactID,
					cmoResellerInactiveDate = eRPOrganizationInformationDto.cmoResellerInactiveDate,
					cmoResellerLocationID = eRPOrganizationInformationDto.cmoResellerLocationID,
					cmoResellerOrganizationID = eRPOrganizationInformationDto.cmoResellerOrganizationID,
					cmoResellerProspectDate = eRPOrganizationInformationDto.cmoResellerProspectDate,
					cmoResellerStatus = eRPOrganizationInformationDto.cmoResellerStatus,
					cmoRowVersion = eRPOrganizationInformationDto.cmoRowVersion,
					cmoSecondGivenName = eRPOrganizationInformationDto.cmoSecondGivenName,
					cmoShipContactID = eRPOrganizationInformationDto.cmoShipContactID,
					cmoSplitPercentTotal = eRPOrganizationInformationDto.cmoSplitPercentTotal,
					cmoState = eRPOrganizationInformationDto.cmoState,
					cmoSuperFundEmployerID = eRPOrganizationInformationDto.cmoSuperFundEmployerID,
					cmoSuperFundName = eRPOrganizationInformationDto.cmoSuperFundName,
					cmoSupplierAccreditedDate = eRPOrganizationInformationDto.cmoSupplierAccreditedDate,
					cmoSupplierActiveDate = eRPOrganizationInformationDto.cmoSupplierActiveDate,
					cmoSupplierInactiveDate = eRPOrganizationInformationDto.cmoSupplierInactiveDate,
					cmoSupplierPaymentTermID = eRPOrganizationInformationDto.cmoSupplierPaymentTermID,
					cmoSupplierProspectDate = eRPOrganizationInformationDto.cmoSupplierProspectDate,
					cmoSupplierRatingID = eRPOrganizationInformationDto.cmoSupplierRatingID,
					cmoSupplierSecondTaxCodeID = eRPOrganizationInformationDto.cmoSupplierSecondTaxCodeID,
					cmoSupplierShippingMethodID = eRPOrganizationInformationDto.cmoSupplierShippingMethodID,
					cmoSupplierStatus = eRPOrganizationInformationDto.cmoSupplierStatus,
					cmoSupplierTaxCodeID = eRPOrganizationInformationDto.cmoSupplierTaxCodeID,
					cmoTaxExemptNumber = eRPOrganizationInformationDto.cmoTaxExemptNumber,
					cmoTradingName = eRPOrganizationInformationDto.cmoTradingName,
					cmoUps3rdPartyLocationID = eRPOrganizationInformationDto.cmoUps3rdPartyLocationID,
					cmoUps3rdPartyOrganizationID = eRPOrganizationInformationDto.cmoUps3rdPartyOrganizationID,
					cmoUpsAcctNumber = eRPOrganizationInformationDto.cmoUpsAcctNumber,
					cmoUpsBillingOption = eRPOrganizationInformationDto.cmoUpsBillingOption,
					cmoUpsWsBillingOption = eRPOrganizationInformationDto.cmoUpsWsBillingOption,
					cmoUsaTransactionTypeCode = eRPOrganizationInformationDto.cmoUsaTransactionTypeCode,
					cmoWebAddress = eRPOrganizationInformationDto.cmoWebAddress,
					CustomFields = eRPOrganizationInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Organizations []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = organizationDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationDto>> Process_PutOrganization(ERPOrganizationDto organization)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPOrganizationDto createdObject = null;
		ERPResponseMessageDto<ERPOrganizationDto> result;
		try
		{
			IERPOrganizationRepository iERPOrganizationRepository = (base.ERPOrganizationRepository = new ERPOrganizationRepository(base.ApiClientContext));
			using (iERPOrganizationRepository)
			{
				APIValidationInfoDto postResult = await base.ERPOrganizationRepository.SaveOrganization(organization);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPOrganizationInformationDto eRPOrganizationInformationDto = await base.ERPOrganizationRepository.GetOrganization(organization.cmoUniqueID);
					createdObject = new ERPOrganizationDto
					{
						cmoAccountManagerEmployeeID = eRPOrganizationInformationDto.cmoAccountManagerEmployeeID,
						cmoAddressLine1 = eRPOrganizationInformationDto.cmoAddressLine1,
						cmoAddressLine2 = eRPOrganizationInformationDto.cmoAddressLine2,
						cmoAddressLine3 = eRPOrganizationInformationDto.cmoAddressLine3,
						cmoAddressValidationResult = eRPOrganizationInformationDto.cmoAddressValidationResult,
						cmoAlternatePhoneNumber = eRPOrganizationInformationDto.cmoAlternatePhoneNumber,
						cmoApInvoiceContactID = eRPOrganizationInformationDto.cmoApInvoiceContactID,
						cmoArInvoiceContactID = eRPOrganizationInformationDto.cmoArInvoiceContactID,
						cmoAttachmentFileFolder = eRPOrganizationInformationDto.cmoAttachmentFileFolder,
						cmoAvalaraUseCodes = eRPOrganizationInformationDto.cmoAvalaraUseCodes,
						cmoBankAccountName = eRPOrganizationInformationDto.cmoBankAccountName,
						cmoBankAccountNumber = eRPOrganizationInformationDto.cmoBankAccountNumber,
						cmoBankAccountType = eRPOrganizationInformationDto.cmoBankAccountType,
						cmoBankInitials = eRPOrganizationInformationDto.cmoBankInitials,
						cmoBic = eRPOrganizationInformationDto.cmoBic,
						cmoBsbNumber = eRPOrganizationInformationDto.cmoBsbNumber,
						cmoCity = eRPOrganizationInformationDto.cmoCity,
						cmoOrganizationID = eRPOrganizationInformationDto.cmoOrganizationID,
						cmoCompanyEntryDescription = eRPOrganizationInformationDto.cmoCompanyEntryDescription,
						cmoCountry = eRPOrganizationInformationDto.cmoCountry,
						cmoCountryCode = eRPOrganizationInformationDto.cmoCountryCode,
						cmoCounty = eRPOrganizationInformationDto.cmoCounty,
						cmoCreatedBy = eRPOrganizationInformationDto.cmoCreatedBy,
						cmoCreatedDate = eRPOrganizationInformationDto.cmoCreatedDate,
						cmoCurrencyRateID = eRPOrganizationInformationDto.cmoCurrencyRateID,
						cmoCustomerActiveDate = eRPOrganizationInformationDto.cmoCustomerActiveDate,
						cmoCustomerCreditLimit = eRPOrganizationInformationDto.cmoCustomerCreditLimit,
						cmoCustomerGroupID = eRPOrganizationInformationDto.cmoCustomerGroupID,
						cmoCustomerInactiveDate = eRPOrganizationInformationDto.cmoCustomerInactiveDate,
						cmoCustomerPaymentTermsID = eRPOrganizationInformationDto.cmoCustomerPaymentTermsID,
						cmoCustomerProspectDate = eRPOrganizationInformationDto.cmoCustomerProspectDate,
						cmoCustomerSecondTaxCodeID = eRPOrganizationInformationDto.cmoCustomerSecondTaxCodeID,
						cmoCustomerShipPaymentTypeID = eRPOrganizationInformationDto.cmoCustomerShipPaymentTypeID,
						cmoCustomerShippingCarrier = eRPOrganizationInformationDto.cmoCustomerShippingCarrier,
						cmoCustomerShippingMethodID = eRPOrganizationInformationDto.cmoCustomerShippingMethodID,
						cmoCustomerStatus = eRPOrganizationInformationDto.cmoCustomerStatus,
						cmoCustomerTaxCodeID = eRPOrganizationInformationDto.cmoCustomerTaxCodeID,
						cmoDefaultApInvoiceLocationID = eRPOrganizationInformationDto.cmoDefaultApInvoiceLocationID,
						cmoDefaultArInvoiceLocationID = eRPOrganizationInformationDto.cmoDefaultArInvoiceLocationID,
						cmoDefaultPurchaseLocationID = eRPOrganizationInformationDto.cmoDefaultPurchaseLocationID,
						cmoDefaultQuoteLocationID = eRPOrganizationInformationDto.cmoDefaultQuoteLocationID,
						cmoDefaultShipLocationID = eRPOrganizationInformationDto.cmoDefaultShipLocationID,
						cmoDropShipLocationID = eRPOrganizationInformationDto.cmoDropShipLocationID,
						cmoDropShipOrganizationID = eRPOrganizationInformationDto.cmoDropShipOrganizationID,
						cmoEftCode = eRPOrganizationInformationDto.cmoEftCode,
						cmoEftDescription = eRPOrganizationInformationDto.cmoEftDescription,
						cmoEftParticulars = eRPOrganizationInformationDto.cmoEftParticulars,
						cmoEmailAddress = eRPOrganizationInformationDto.cmoEmailAddress,
						cmoEmployeeCount = eRPOrganizationInformationDto.cmoEmployeeCount,
						cmoUniqueID = eRPOrganizationInformationDto.cmoUniqueID,
						cmoEstablishedDate = eRPOrganizationInformationDto.cmoEstablishedDate,
						cmoExpenseSplitPercentTotal = eRPOrganizationInformationDto.cmoExpenseSplitPercentTotal,
						cmoFaxNumber = eRPOrganizationInformationDto.cmoFaxNumber,
						cmoFederalID = eRPOrganizationInformationDto.cmoFederalID,
						cmoFedEx3rdPartyLocationID = eRPOrganizationInformationDto.cmoFedEx3rdPartyLocationID,
						cmoFedEx3rdPartyOrganizationID = eRPOrganizationInformationDto.cmoFedEx3rdPartyOrganizationID,
						cmoFedExAccountNumber = eRPOrganizationInformationDto.cmoFedExAccountNumber,
						cmoFedExBillingOption = eRPOrganizationInformationDto.cmoFedExBillingOption,
						cmoFirstGivenName = eRPOrganizationInformationDto.cmoFirstGivenName,
						cmoForm1099Box = eRPOrganizationInformationDto.cmoForm1099Box,
						cmoFreeOnBoardDescription = eRPOrganizationInformationDto.cmoFreeOnBoardDescription,
						cmoHdAttachmentFilePath = eRPOrganizationInformationDto.cmoHdAttachmentFilePath,
						cmoIban = eRPOrganizationInformationDto.cmoIban,
						cmoIntraCompanyDatasetID = eRPOrganizationInformationDto.cmoIntraCompanyDatasetID,
						cmoApIncludeTaxInRetention = eRPOrganizationInformationDto.cmoApIncludeTaxInRetention,
						cmoArIncludeTaxInRetention = eRPOrganizationInformationDto.cmoArIncludeTaxInRetention,
						cmoArInvoicePerShipmentLine = eRPOrganizationInformationDto.cmoArInvoicePerShipmentLine,
						cmoAvalaraAddressValidated = eRPOrganizationInformationDto.cmoAvalaraAddressValidated,
						cmoBareCostOfDuty = eRPOrganizationInformationDto.cmoBareCostOfDuty,
						cmoBareTransportationCost = eRPOrganizationInformationDto.cmoBareTransportationCost,
						cmoCalculateFinanceCharges = eRPOrganizationInformationDto.cmoCalculateFinanceCharges,
						cmoCompetitor = eRPOrganizationInformationDto.cmoCompetitor,
						cmoContractor = eRPOrganizationInformationDto.cmoContractor,
						cmoCreatedFromMobile = eRPOrganizationInformationDto.cmoCreatedFromMobile,
						cmoCreditHold = eRPOrganizationInformationDto.cmoCreditHold,
						cmoCustomerTaxable = eRPOrganizationInformationDto.cmoCustomerTaxable,
						cmoDirectPayment = eRPOrganizationInformationDto.cmoDirectPayment,
						cmoEdiIntegrated = eRPOrganizationInformationDto.cmoEdiIntegrated,
						cmoFinanceCompany = eRPOrganizationInformationDto.cmoFinanceCompany,
						cmoIgnoreAvalara = eRPOrganizationInformationDto.cmoIgnoreAvalara,
						cmoIncludeFreightInPrice = eRPOrganizationInformationDto.cmoIncludeFreightInPrice,
						cmoPrintStatement = eRPOrganizationInformationDto.cmoPrintStatement,
						cmoRequires1099 = eRPOrganizationInformationDto.cmoRequires1099,
						cmoRequiresInspection = eRPOrganizationInformationDto.cmoRequiresInspection,
						cmoResidentialAddress = eRPOrganizationInformationDto.cmoResidentialAddress,
						cmoSuperFund = eRPOrganizationInformationDto.cmoSuperFund,
						cmoSupplierAccredited = eRPOrganizationInformationDto.cmoSupplierAccredited,
						cmoSupplierTaxable = eRPOrganizationInformationDto.cmoSupplierTaxable,
						cmoTaxReportable = eRPOrganizationInformationDto.cmoTaxReportable,
						cmoUpsValidated = eRPOrganizationInformationDto.cmoUpsValidated,
						cmoJobPriorityID = eRPOrganizationInformationDto.cmoJobPriorityID,
						cmoLastName = eRPOrganizationInformationDto.cmoLastName,
						cmoLongDescriptionRtf = eRPOrganizationInformationDto.cmoLongDescriptionRtf,
						cmoLongDescriptionText = eRPOrganizationInformationDto.cmoLongDescriptionText,
						cmoName = eRPOrganizationInformationDto.cmoName,
						cmoNonTaxReasonID = eRPOrganizationInformationDto.cmoNonTaxReasonID,
						cmoOrganizationAccountID = eRPOrganizationInformationDto.cmoOrganizationAccountID,
						cmoPhoneNumber = eRPOrganizationInformationDto.cmoPhoneNumber,
						cmoPostCode = eRPOrganizationInformationDto.cmoPostCode,
						cmoPurchaseContactID = eRPOrganizationInformationDto.cmoPurchaseContactID,
						cmoQuoteContactID = eRPOrganizationInformationDto.cmoQuoteContactID,
						cmoResellerActiveDate = eRPOrganizationInformationDto.cmoResellerActiveDate,
						cmoResellerCommissionRate = eRPOrganizationInformationDto.cmoResellerCommissionRate,
						cmoResellerContactID = eRPOrganizationInformationDto.cmoResellerContactID,
						cmoResellerInactiveDate = eRPOrganizationInformationDto.cmoResellerInactiveDate,
						cmoResellerLocationID = eRPOrganizationInformationDto.cmoResellerLocationID,
						cmoResellerOrganizationID = eRPOrganizationInformationDto.cmoResellerOrganizationID,
						cmoResellerProspectDate = eRPOrganizationInformationDto.cmoResellerProspectDate,
						cmoResellerStatus = eRPOrganizationInformationDto.cmoResellerStatus,
						cmoRowVersion = eRPOrganizationInformationDto.cmoRowVersion,
						cmoSecondGivenName = eRPOrganizationInformationDto.cmoSecondGivenName,
						cmoShipContactID = eRPOrganizationInformationDto.cmoShipContactID,
						cmoSplitPercentTotal = eRPOrganizationInformationDto.cmoSplitPercentTotal,
						cmoState = eRPOrganizationInformationDto.cmoState,
						cmoSuperFundEmployerID = eRPOrganizationInformationDto.cmoSuperFundEmployerID,
						cmoSuperFundName = eRPOrganizationInformationDto.cmoSuperFundName,
						cmoSupplierAccreditedDate = eRPOrganizationInformationDto.cmoSupplierAccreditedDate,
						cmoSupplierActiveDate = eRPOrganizationInformationDto.cmoSupplierActiveDate,
						cmoSupplierInactiveDate = eRPOrganizationInformationDto.cmoSupplierInactiveDate,
						cmoSupplierPaymentTermID = eRPOrganizationInformationDto.cmoSupplierPaymentTermID,
						cmoSupplierProspectDate = eRPOrganizationInformationDto.cmoSupplierProspectDate,
						cmoSupplierRatingID = eRPOrganizationInformationDto.cmoSupplierRatingID,
						cmoSupplierSecondTaxCodeID = eRPOrganizationInformationDto.cmoSupplierSecondTaxCodeID,
						cmoSupplierShippingMethodID = eRPOrganizationInformationDto.cmoSupplierShippingMethodID,
						cmoSupplierStatus = eRPOrganizationInformationDto.cmoSupplierStatus,
						cmoSupplierTaxCodeID = eRPOrganizationInformationDto.cmoSupplierTaxCodeID,
						cmoTaxExemptNumber = eRPOrganizationInformationDto.cmoTaxExemptNumber,
						cmoTradingName = eRPOrganizationInformationDto.cmoTradingName,
						cmoUps3rdPartyLocationID = eRPOrganizationInformationDto.cmoUps3rdPartyLocationID,
						cmoUps3rdPartyOrganizationID = eRPOrganizationInformationDto.cmoUps3rdPartyOrganizationID,
						cmoUpsAcctNumber = eRPOrganizationInformationDto.cmoUpsAcctNumber,
						cmoUpsBillingOption = eRPOrganizationInformationDto.cmoUpsBillingOption,
						cmoUpsWsBillingOption = eRPOrganizationInformationDto.cmoUpsWsBillingOption,
						cmoUsaTransactionTypeCode = eRPOrganizationInformationDto.cmoUsaTransactionTypeCode,
						cmoWebAddress = eRPOrganizationInformationDto.cmoWebAddress,
						CustomFields = eRPOrganizationInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Organization [{organization.cmoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteOrganization(Guid organizationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationRepository iERPOrganizationRepository = (base.ERPOrganizationRepository = new ERPOrganizationRepository(base.ApiClientContext));
		using (iERPOrganizationRepository)
		{
			if (!(await base.ERPOrganizationRepository.DoesOrganizationExist(organizationId)))
			{
				base.ErrorsList.Add($"Organization [{organizationId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPOrganizationInformationDto eRPOrganizationInformationDto = await base.ERPOrganizationRepository.GetOrganization(organizationId);
				string text = await base.ERPOrganizationRepository.WhereUsed("Organizations", new object[1] { eRPOrganizationInformationDto.cmoOrganizationID }, new object[1] { "cmoOrganizationID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Organization cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationDto>> Process_DeleteOrganization(Guid organizationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPOrganizationDto> result;
		try
		{
			IERPOrganizationRepository iERPOrganizationRepository = (base.ERPOrganizationRepository = new ERPOrganizationRepository(base.ApiClientContext));
			using (iERPOrganizationRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPOrganizationRepository.DeleteRowFromTable("Organizations", "cmo", organizationId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Organization [{organizationId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPOrganizationDto()
			};
		}
		return result;
	}
}
