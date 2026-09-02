using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPARInvoiceModel : ERPBaseModel, IERPARInvoiceModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllARInvoices(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPARInvoiceRepository iERPARInvoiceRepository = (base.ERPARInvoiceRepository = new ERPARInvoiceRepository(base.ApiClientContext));
		using (iERPARInvoiceRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPARInvoiceRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPARInvoiceRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPARInvoiceRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPARInvoiceRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetARInvoice(Guid aRInvoiceId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARInvoiceRepository iERPARInvoiceRepository = (base.ERPARInvoiceRepository = new ERPARInvoiceRepository(base.ApiClientContext));
		using (iERPARInvoiceRepository)
		{
			if (!(await base.ERPARInvoiceRepository.DoesARInvoiceExist(aRInvoiceId)))
			{
				errorsList.Add($"ARInvoice [{aRInvoiceId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutARInvoice(ERPARInvoiceDto aRInvoice)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARInvoiceRepository iERPARInvoiceRepository = (base.ERPARInvoiceRepository = new ERPARInvoiceRepository(base.ApiClientContext));
		using (iERPARInvoiceRepository)
		{
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpPlantDepartmentID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { aRInvoice.arpPlantID, aRInvoice.arpPlantDepartmentID })))
			{
				errorsList.Add("arpPlantDepartmentID [" + aRInvoice.arpPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpPlantID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { aRInvoice.arpPlantID })))
			{
				errorsList.Add("arpPlantID [" + aRInvoice.arpPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpCreditArInvoiceID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { aRInvoice.arpCreditArInvoiceID })))
			{
				errorsList.Add("arpCreditArInvoiceID [" + aRInvoice.arpCreditArInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpCreditReasonID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { aRInvoice.arpCreditReasonID })))
			{
				errorsList.Add("arpCreditReasonID [" + aRInvoice.arpCreditReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpCustomerOrganizationID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { aRInvoice.arpCustomerOrganizationID })))
			{
				errorsList.Add("arpCustomerOrganizationID [" + aRInvoice.arpCustomerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpArInvoiceLocationID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { aRInvoice.arpCustomerOrganizationID, aRInvoice.arpArInvoiceLocationID })))
			{
				errorsList.Add("arpArInvoiceLocationID [" + aRInvoice.arpArInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpArInvoiceContactID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { aRInvoice.arpCustomerOrganizationID, aRInvoice.arpArInvoiceLocationID, aRInvoice.arpArInvoiceContactID })))
			{
				errorsList.Add("arpArInvoiceContactID [" + aRInvoice.arpArInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpShipOrganizationID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { aRInvoice.arpShipOrganizationID })))
			{
				errorsList.Add("arpShipOrganizationID [" + aRInvoice.arpShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpShipLocationID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { aRInvoice.arpShipOrganizationID, aRInvoice.arpShipLocationID })))
			{
				errorsList.Add("arpShipLocationID [" + aRInvoice.arpShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpShipContactID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { aRInvoice.arpShipOrganizationID, aRInvoice.arpShipLocationID, aRInvoice.arpShipContactID })))
			{
				errorsList.Add("arpShipContactID [" + aRInvoice.arpShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpPaymentTermID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("PaymentTerms", new object[1] { "XATPAYMENTTERMID" }, new object[1] { aRInvoice.arpPaymentTermID })))
			{
				errorsList.Add("arpPaymentTermID [" + aRInvoice.arpPaymentTermID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpStandardMessageID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("StandardMessages", new object[1] { "XAMSTANDARDMESSAGEID" }, new object[1] { aRInvoice.arpStandardMessageID })))
			{
				errorsList.Add("arpStandardMessageID [" + aRInvoice.arpStandardMessageID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpShippingMethodID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { aRInvoice.arpShippingMethodID })))
			{
				errorsList.Add("arpShippingMethodID [" + aRInvoice.arpShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpShippingPaymentTypeID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("ShippingPaymentTypes", new object[1] { "XAYSHIPPINGPAYMENTTYPEID" }, new object[1] { aRInvoice.arpShippingPaymentTypeID })))
			{
				errorsList.Add("arpShippingPaymentTypeID [" + aRInvoice.arpShippingPaymentTypeID + "] not found.");
			}
			if (aRInvoice.arpGlFiscalYearID > 0 && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { aRInvoice.arpGlFiscalYearID })))
			{
				errorsList.Add($"arpGlFiscalYearID [{aRInvoice.arpGlFiscalYearID}] not found.");
			}
			if (aRInvoice.arpGlFiscalYearPeriodID > 0 && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearPeriods", new object[2] { "GLFGLFISCALYEARID", "GLFGLFISCALYEARPERIODID" }, new object[2] { aRInvoice.arpGlFiscalYearID, aRInvoice.arpGlFiscalYearPeriodID })))
			{
				errorsList.Add($"arpGlFiscalYearPeriodID [{aRInvoice.arpGlFiscalYearPeriodID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpCurrencyRateID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { aRInvoice.arpCurrencyRateID })))
			{
				errorsList.Add("arpCurrencyRateID [" + aRInvoice.arpCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpFreightTaxCodeID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aRInvoice.arpFreightTaxCodeID })))
			{
				errorsList.Add("arpFreightTaxCodeID [" + aRInvoice.arpFreightTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpSecondFreightTaxCodeID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aRInvoice.arpSecondFreightTaxCodeID })))
			{
				errorsList.Add("arpSecondFreightTaxCodeID [" + aRInvoice.arpSecondFreightTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpResellerOrganizationID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { aRInvoice.arpResellerOrganizationID })))
			{
				errorsList.Add("arpResellerOrganizationID [" + aRInvoice.arpResellerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpResellerLocationID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { aRInvoice.arpResellerOrganizationID, aRInvoice.arpResellerLocationID })))
			{
				errorsList.Add("arpResellerLocationID [" + aRInvoice.arpResellerLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpResellerContactID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { aRInvoice.arpResellerOrganizationID, aRInvoice.arpResellerLocationID, aRInvoice.arpResellerContactID })))
			{
				errorsList.Add("arpResellerContactID [" + aRInvoice.arpResellerContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpArGlAccountID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRInvoice.arpArGlAccountID })))
			{
				errorsList.Add("arpArGlAccountID [" + aRInvoice.arpArGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpSalesGlAccountID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRInvoice.arpSalesGlAccountID })))
			{
				errorsList.Add("arpSalesGlAccountID [" + aRInvoice.arpSalesGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpFreightGlAccountID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRInvoice.arpFreightGlAccountID })))
			{
				errorsList.Add("arpFreightGlAccountID [" + aRInvoice.arpFreightGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpDiscountGlAccountID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRInvoice.arpDiscountGlAccountID })))
			{
				errorsList.Add("arpDiscountGlAccountID [" + aRInvoice.arpDiscountGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpProjectID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { aRInvoice.arpProjectID })))
			{
				errorsList.Add("arpProjectID [" + aRInvoice.arpProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoice.arpDepositGlAccountID) && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRInvoice.arpDepositGlAccountID })))
			{
				errorsList.Add("arpDepositGlAccountID [" + aRInvoice.arpDepositGlAccountID + "] not found.");
			}
			if (aRInvoice.arpOverPaymentSessionID > 0 && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("ARPaymentSessions", new object[1] { "ARSARPAYMENTSESSIONID" }, new object[1] { aRInvoice.arpOverPaymentSessionID })))
			{
				errorsList.Add($"arpOverPaymentSessionID [{aRInvoice.arpOverPaymentSessionID}] not found.");
			}
			if (aRInvoice.arpOverPaymentHeaderID > 0 && !(await base.ERPARInvoiceRepository.DoesRecordExistInTableUsingKeys("ARPaymentHeaders", new object[2] { "ARTARPAYMENTSESSIONID", "ARTARPAYMENTHEADERID" }, new object[2] { aRInvoice.arpOverPaymentSessionID, aRInvoice.arpOverPaymentHeaderID })))
			{
				errorsList.Add($"arpOverPaymentHeaderID [{aRInvoice.arpOverPaymentHeaderID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPARInvoiceDto>>> Process_GetAllARInvoices(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPARInvoiceDto> allARInvoicesDto = new List<ERPARInvoiceDto>();
		ERPResponseMessageDto<IList<ERPARInvoiceDto>> result;
		try
		{
			IERPARInvoiceRepository iERPARInvoiceRepository = (base.ERPARInvoiceRepository = new ERPARInvoiceRepository(base.ApiClientContext));
			using (iERPARInvoiceRepository)
			{
				foreach (ERPARInvoiceInformationDto item2 in await base.ERPARInvoiceRepository.GetAllARInvoices(pageSize, pageNumber, filter, orderBy))
				{
					ERPARInvoiceDto item = new ERPARInvoiceDto
					{
						arpArGlAccountID = item2.arpArGlAccountID,
						arpArInvoiceContactID = item2.arpArInvoiceContactID,
						arpArInvoiceLocationID = item2.arpArInvoiceLocationID,
						arpArInvoiceID = item2.arpArInvoiceID,
						arpCommissionAmountBase = item2.arpCommissionAmountBase,
						arpCreatedBy = item2.arpCreatedBy,
						arpCreatedDate = item2.arpCreatedDate,
						arpCreditArInvoiceID = item2.arpCreditArInvoiceID,
						arpCreditDate = item2.arpCreditDate,
						arpCreditReasonID = item2.arpCreditReasonID,
						arpCurrencyRateID = item2.arpCurrencyRateID,
						arpCustomerOrganizationID = item2.arpCustomerOrganizationID,
						arpDepositAppliedBase = item2.arpDepositAppliedBase,
						arpDepositAppliedForeign = item2.arpDepositAppliedForeign,
						arpDepositBalanceBase = item2.arpDepositBalanceBase,
						arpDepositBalanceForeign = item2.arpDepositBalanceForeign,
						arpDepositGlAccountID = item2.arpDepositGlAccountID,
						arpDepositTransferredBase = item2.arpDepositTransferredBase,
						arpDepositTransferredForeign = item2.arpDepositTransferredForeign,
						arpDiscountDueDate = item2.arpDiscountDueDate,
						arpDiscountGlAccountID = item2.arpDiscountGlAccountID,
						arpDiscountTotalBase = item2.arpDiscountTotalBase,
						arpDiscountTotalForeign = item2.arpDiscountTotalForeign,
						arpDueDate = item2.arpDueDate,
						arpEdiTransferredDate = item2.arpEdiTransferredDate,
						arpUniqueID = item2.arpUniqueID,
						arpExchangeRate = item2.arpExchangeRate,
						arpFreeOnBoardDescription = item2.arpFreeOnBoardDescription,
						arpFreightAmountBase = item2.arpFreightAmountBase,
						arpFreightAmountForeign = item2.arpFreightAmountForeign,
						arpFreightGlAccountID = item2.arpFreightGlAccountID,
						arpFreightSubtotalBase = item2.arpFreightSubtotalBase,
						arpFreightSubtotalForeign = item2.arpFreightSubtotalForeign,
						arpFreightTaxAmountBase = item2.arpFreightTaxAmountBase,
						arpFreightTaxAmountForeign = item2.arpFreightTaxAmountForeign,
						arpFreightTaxCodeID = item2.arpFreightTaxCodeID,
						arpFreightTotalBase = item2.arpFreightTotalBase,
						arpFreightTotalForeign = item2.arpFreightTotalForeign,
						arpFullInvoiceSubtotalBase = item2.arpFullInvoiceSubtotalBase,
						arpFullInvoiceSubtotalForeign = item2.arpFullInvoiceSubtotalForeign,
						arpGlFiscalYearID = item2.arpGlFiscalYearID,
						arpGlFiscalYearPeriodID = item2.arpGlFiscalYearPeriodID,
						arpIntraCompanyPostedDate = item2.arpIntraCompanyPostedDate,
						arpInvoiceBalanceBase = item2.arpInvoiceBalanceBase,
						arpInvoiceBalanceForeign = item2.arpInvoiceBalanceForeign,
						arpInvoiceCommentsRTF = item2.arpInvoiceCommentsRTF,
						arpInvoiceCommentsText = item2.arpInvoiceCommentsText,
						arpInvoiceDate = item2.arpInvoiceDate,
						arpInvoicePaidBase = item2.arpInvoicePaidBase,
						arpInvoicePaidForeign = item2.arpInvoicePaidForeign,
						arpInvoiceSubtotalBase = item2.arpInvoiceSubtotalBase,
						arpInvoiceSubtotalForeign = item2.arpInvoiceSubtotalForeign,
						arpInvoiceTaxAmountBase = item2.arpInvoiceTaxAmountBase,
						arpInvoiceTaxAmountForeign = item2.arpInvoiceTaxAmountForeign,
						arpInvoiceTotalBase = item2.arpInvoiceTotalBase,
						arpInvoiceTotalForeign = item2.arpInvoiceTotalForeign,
						arpInvoiceType = item2.arpInvoiceType,
						arpAvalaraOverrideTax = item2.arpAvalaraOverrideTax,
						arpAvalaraTaxCalculated = item2.arpAvalaraTaxCalculated,
						arpCustomRate = item2.arpCustomRate,
						arpDepositCredit = item2.arpDepositCredit,
						arpEdiTransferred = item2.arpEdiTransferred,
						arpIncludeFreightInPrice = item2.arpIncludeFreightInPrice,
						arpIncludeTaxInRetention = item2.arpIncludeTaxInRetention,
						arpIntraCompany = item2.arpIntraCompany,
						arpIntraCompanyPosted = item2.arpIntraCompanyPosted,
						arpOnHold = item2.arpOnHold,
						arpOpenInvoiceLoad = item2.arpOpenInvoiceLoad,
						arpOverpayment = item2.arpOverpayment,
						arpPaidComplete = item2.arpPaidComplete,
						arpPostedToGl = item2.arpPostedToGl,
						arpReadyToPrint = item2.arpReadyToPrint,
						arpRecurringInvoice = item2.arpRecurringInvoice,
						arpRefundCheckRequired = item2.arpRefundCheckRequired,
						arpLineCommissionTotal = item2.arpLineCommissionTotal,
						arpOrderDate = item2.arpOrderDate,
						arpOriginalExchangeRate = item2.arpOriginalExchangeRate,
						arpOverPaymentHeaderID = item2.arpOverPaymentHeaderID,
						arpOverPaymentSessionID = item2.arpOverPaymentSessionID,
						arpPaidDate = item2.arpPaidDate,
						arpPaymentTermID = item2.arpPaymentTermID,
						arpPlantDepartmentID = item2.arpPlantDepartmentID,
						arpPlantID = item2.arpPlantID,
						arpPointOfSaleTerminalID = item2.arpPointOfSaleTerminalID,
						arpPostedDate = item2.arpPostedDate,
						arpProjectID = item2.arpProjectID,
						arpResellerCommissionAmount = item2.arpResellerCommissionAmount,
						arpResellerCommissionRate = item2.arpResellerCommissionRate,
						arpResellerContactID = item2.arpResellerContactID,
						arpResellerLocationID = item2.arpResellerLocationID,
						arpResellerOrganizationID = item2.arpResellerOrganizationID,
						arpRetentionBalanceBase = item2.arpRetentionBalanceBase,
						arpRetentionBalanceForeign = item2.arpRetentionBalanceForeign,
						arpRetentionPaidBase = item2.arpRetentionPaidBase,
						arpRetentionPaidForeign = item2.arpRetentionPaidForeign,
						arpRetentionTotalBase = item2.arpRetentionTotalBase,
						arpRetentionTotalForeign = item2.arpRetentionTotalForeign,
						arpRowVersion = item2.arpRowVersion,
						arpSalesCommissionTotal = item2.arpSalesCommissionTotal,
						arpSalesGlAccountID = item2.arpSalesGlAccountID,
						arpSecondFreightTaxAmtBase = item2.arpSecondFreightTaxAmtBase,
						arpSecondFreightTaxAmtForeign = item2.arpSecondFreightTaxAmtForeign,
						arpSecondFreightTaxCodeID = item2.arpSecondFreightTaxCodeID,
						arpShipContactID = item2.arpShipContactID,
						arpShipLocationID = item2.arpShipLocationID,
						arpShipOrganizationID = item2.arpShipOrganizationID,
						arpShippingMethodID = item2.arpShippingMethodID,
						arpShippingPaymentTypeID = item2.arpShippingPaymentTypeID,
						arpSplitPercentTotal = item2.arpSplitPercentTotal,
						arpStandardMessageID = item2.arpStandardMessageID,
						arpTaxDate = item2.arpTaxDate,
						arpTaxSubtotalBase = item2.arpTaxSubtotalBase,
						arpTaxSubtotalForeign = item2.arpTaxSubtotalForeign,
						arpTotalForResellerCommission = item2.arpTotalForResellerCommission,
						arpTotalForSalesCommission = item2.arpTotalForSalesCommission,
						CustomFields = item2.CustomFields
					};
					allARInvoicesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ARInvoices]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPARInvoiceDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allARInvoicesDto,
				RecordCount = allARInvoicesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARInvoiceDto>> Process_GetARInvoice(Guid aRInvoiceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPARInvoiceDto aRInvoiceDto = null;
		ERPResponseMessageDto<ERPARInvoiceDto> result;
		try
		{
			IERPARInvoiceRepository iERPARInvoiceRepository = (base.ERPARInvoiceRepository = new ERPARInvoiceRepository(base.ApiClientContext));
			using (iERPARInvoiceRepository)
			{
				ERPARInvoiceInformationDto eRPARInvoiceInformationDto = await base.ERPARInvoiceRepository.GetARInvoice(aRInvoiceId);
				aRInvoiceDto = new ERPARInvoiceDto
				{
					arpArGlAccountID = eRPARInvoiceInformationDto.arpArGlAccountID,
					arpArInvoiceContactID = eRPARInvoiceInformationDto.arpArInvoiceContactID,
					arpArInvoiceLocationID = eRPARInvoiceInformationDto.arpArInvoiceLocationID,
					arpArInvoiceID = eRPARInvoiceInformationDto.arpArInvoiceID,
					arpCommissionAmountBase = eRPARInvoiceInformationDto.arpCommissionAmountBase,
					arpCreatedBy = eRPARInvoiceInformationDto.arpCreatedBy,
					arpCreatedDate = eRPARInvoiceInformationDto.arpCreatedDate,
					arpCreditArInvoiceID = eRPARInvoiceInformationDto.arpCreditArInvoiceID,
					arpCreditDate = eRPARInvoiceInformationDto.arpCreditDate,
					arpCreditReasonID = eRPARInvoiceInformationDto.arpCreditReasonID,
					arpCurrencyRateID = eRPARInvoiceInformationDto.arpCurrencyRateID,
					arpCustomerOrganizationID = eRPARInvoiceInformationDto.arpCustomerOrganizationID,
					arpDepositAppliedBase = eRPARInvoiceInformationDto.arpDepositAppliedBase,
					arpDepositAppliedForeign = eRPARInvoiceInformationDto.arpDepositAppliedForeign,
					arpDepositBalanceBase = eRPARInvoiceInformationDto.arpDepositBalanceBase,
					arpDepositBalanceForeign = eRPARInvoiceInformationDto.arpDepositBalanceForeign,
					arpDepositGlAccountID = eRPARInvoiceInformationDto.arpDepositGlAccountID,
					arpDepositTransferredBase = eRPARInvoiceInformationDto.arpDepositTransferredBase,
					arpDepositTransferredForeign = eRPARInvoiceInformationDto.arpDepositTransferredForeign,
					arpDiscountDueDate = eRPARInvoiceInformationDto.arpDiscountDueDate,
					arpDiscountGlAccountID = eRPARInvoiceInformationDto.arpDiscountGlAccountID,
					arpDiscountTotalBase = eRPARInvoiceInformationDto.arpDiscountTotalBase,
					arpDiscountTotalForeign = eRPARInvoiceInformationDto.arpDiscountTotalForeign,
					arpDueDate = eRPARInvoiceInformationDto.arpDueDate,
					arpEdiTransferredDate = eRPARInvoiceInformationDto.arpEdiTransferredDate,
					arpUniqueID = eRPARInvoiceInformationDto.arpUniqueID,
					arpExchangeRate = eRPARInvoiceInformationDto.arpExchangeRate,
					arpFreeOnBoardDescription = eRPARInvoiceInformationDto.arpFreeOnBoardDescription,
					arpFreightAmountBase = eRPARInvoiceInformationDto.arpFreightAmountBase,
					arpFreightAmountForeign = eRPARInvoiceInformationDto.arpFreightAmountForeign,
					arpFreightGlAccountID = eRPARInvoiceInformationDto.arpFreightGlAccountID,
					arpFreightSubtotalBase = eRPARInvoiceInformationDto.arpFreightSubtotalBase,
					arpFreightSubtotalForeign = eRPARInvoiceInformationDto.arpFreightSubtotalForeign,
					arpFreightTaxAmountBase = eRPARInvoiceInformationDto.arpFreightTaxAmountBase,
					arpFreightTaxAmountForeign = eRPARInvoiceInformationDto.arpFreightTaxAmountForeign,
					arpFreightTaxCodeID = eRPARInvoiceInformationDto.arpFreightTaxCodeID,
					arpFreightTotalBase = eRPARInvoiceInformationDto.arpFreightTotalBase,
					arpFreightTotalForeign = eRPARInvoiceInformationDto.arpFreightTotalForeign,
					arpFullInvoiceSubtotalBase = eRPARInvoiceInformationDto.arpFullInvoiceSubtotalBase,
					arpFullInvoiceSubtotalForeign = eRPARInvoiceInformationDto.arpFullInvoiceSubtotalForeign,
					arpGlFiscalYearID = eRPARInvoiceInformationDto.arpGlFiscalYearID,
					arpGlFiscalYearPeriodID = eRPARInvoiceInformationDto.arpGlFiscalYearPeriodID,
					arpIntraCompanyPostedDate = eRPARInvoiceInformationDto.arpIntraCompanyPostedDate,
					arpInvoiceBalanceBase = eRPARInvoiceInformationDto.arpInvoiceBalanceBase,
					arpInvoiceBalanceForeign = eRPARInvoiceInformationDto.arpInvoiceBalanceForeign,
					arpInvoiceCommentsRTF = eRPARInvoiceInformationDto.arpInvoiceCommentsRTF,
					arpInvoiceCommentsText = eRPARInvoiceInformationDto.arpInvoiceCommentsText,
					arpInvoiceDate = eRPARInvoiceInformationDto.arpInvoiceDate,
					arpInvoicePaidBase = eRPARInvoiceInformationDto.arpInvoicePaidBase,
					arpInvoicePaidForeign = eRPARInvoiceInformationDto.arpInvoicePaidForeign,
					arpInvoiceSubtotalBase = eRPARInvoiceInformationDto.arpInvoiceSubtotalBase,
					arpInvoiceSubtotalForeign = eRPARInvoiceInformationDto.arpInvoiceSubtotalForeign,
					arpInvoiceTaxAmountBase = eRPARInvoiceInformationDto.arpInvoiceTaxAmountBase,
					arpInvoiceTaxAmountForeign = eRPARInvoiceInformationDto.arpInvoiceTaxAmountForeign,
					arpInvoiceTotalBase = eRPARInvoiceInformationDto.arpInvoiceTotalBase,
					arpInvoiceTotalForeign = eRPARInvoiceInformationDto.arpInvoiceTotalForeign,
					arpInvoiceType = eRPARInvoiceInformationDto.arpInvoiceType,
					arpAvalaraOverrideTax = eRPARInvoiceInformationDto.arpAvalaraOverrideTax,
					arpAvalaraTaxCalculated = eRPARInvoiceInformationDto.arpAvalaraTaxCalculated,
					arpCustomRate = eRPARInvoiceInformationDto.arpCustomRate,
					arpDepositCredit = eRPARInvoiceInformationDto.arpDepositCredit,
					arpEdiTransferred = eRPARInvoiceInformationDto.arpEdiTransferred,
					arpIncludeFreightInPrice = eRPARInvoiceInformationDto.arpIncludeFreightInPrice,
					arpIncludeTaxInRetention = eRPARInvoiceInformationDto.arpIncludeTaxInRetention,
					arpIntraCompany = eRPARInvoiceInformationDto.arpIntraCompany,
					arpIntraCompanyPosted = eRPARInvoiceInformationDto.arpIntraCompanyPosted,
					arpOnHold = eRPARInvoiceInformationDto.arpOnHold,
					arpOpenInvoiceLoad = eRPARInvoiceInformationDto.arpOpenInvoiceLoad,
					arpOverpayment = eRPARInvoiceInformationDto.arpOverpayment,
					arpPaidComplete = eRPARInvoiceInformationDto.arpPaidComplete,
					arpPostedToGl = eRPARInvoiceInformationDto.arpPostedToGl,
					arpReadyToPrint = eRPARInvoiceInformationDto.arpReadyToPrint,
					arpRecurringInvoice = eRPARInvoiceInformationDto.arpRecurringInvoice,
					arpRefundCheckRequired = eRPARInvoiceInformationDto.arpRefundCheckRequired,
					arpLineCommissionTotal = eRPARInvoiceInformationDto.arpLineCommissionTotal,
					arpOrderDate = eRPARInvoiceInformationDto.arpOrderDate,
					arpOriginalExchangeRate = eRPARInvoiceInformationDto.arpOriginalExchangeRate,
					arpOverPaymentHeaderID = eRPARInvoiceInformationDto.arpOverPaymentHeaderID,
					arpOverPaymentSessionID = eRPARInvoiceInformationDto.arpOverPaymentSessionID,
					arpPaidDate = eRPARInvoiceInformationDto.arpPaidDate,
					arpPaymentTermID = eRPARInvoiceInformationDto.arpPaymentTermID,
					arpPlantDepartmentID = eRPARInvoiceInformationDto.arpPlantDepartmentID,
					arpPlantID = eRPARInvoiceInformationDto.arpPlantID,
					arpPointOfSaleTerminalID = eRPARInvoiceInformationDto.arpPointOfSaleTerminalID,
					arpPostedDate = eRPARInvoiceInformationDto.arpPostedDate,
					arpProjectID = eRPARInvoiceInformationDto.arpProjectID,
					arpResellerCommissionAmount = eRPARInvoiceInformationDto.arpResellerCommissionAmount,
					arpResellerCommissionRate = eRPARInvoiceInformationDto.arpResellerCommissionRate,
					arpResellerContactID = eRPARInvoiceInformationDto.arpResellerContactID,
					arpResellerLocationID = eRPARInvoiceInformationDto.arpResellerLocationID,
					arpResellerOrganizationID = eRPARInvoiceInformationDto.arpResellerOrganizationID,
					arpRetentionBalanceBase = eRPARInvoiceInformationDto.arpRetentionBalanceBase,
					arpRetentionBalanceForeign = eRPARInvoiceInformationDto.arpRetentionBalanceForeign,
					arpRetentionPaidBase = eRPARInvoiceInformationDto.arpRetentionPaidBase,
					arpRetentionPaidForeign = eRPARInvoiceInformationDto.arpRetentionPaidForeign,
					arpRetentionTotalBase = eRPARInvoiceInformationDto.arpRetentionTotalBase,
					arpRetentionTotalForeign = eRPARInvoiceInformationDto.arpRetentionTotalForeign,
					arpRowVersion = eRPARInvoiceInformationDto.arpRowVersion,
					arpSalesCommissionTotal = eRPARInvoiceInformationDto.arpSalesCommissionTotal,
					arpSalesGlAccountID = eRPARInvoiceInformationDto.arpSalesGlAccountID,
					arpSecondFreightTaxAmtBase = eRPARInvoiceInformationDto.arpSecondFreightTaxAmtBase,
					arpSecondFreightTaxAmtForeign = eRPARInvoiceInformationDto.arpSecondFreightTaxAmtForeign,
					arpSecondFreightTaxCodeID = eRPARInvoiceInformationDto.arpSecondFreightTaxCodeID,
					arpShipContactID = eRPARInvoiceInformationDto.arpShipContactID,
					arpShipLocationID = eRPARInvoiceInformationDto.arpShipLocationID,
					arpShipOrganizationID = eRPARInvoiceInformationDto.arpShipOrganizationID,
					arpShippingMethodID = eRPARInvoiceInformationDto.arpShippingMethodID,
					arpShippingPaymentTypeID = eRPARInvoiceInformationDto.arpShippingPaymentTypeID,
					arpSplitPercentTotal = eRPARInvoiceInformationDto.arpSplitPercentTotal,
					arpStandardMessageID = eRPARInvoiceInformationDto.arpStandardMessageID,
					arpTaxDate = eRPARInvoiceInformationDto.arpTaxDate,
					arpTaxSubtotalBase = eRPARInvoiceInformationDto.arpTaxSubtotalBase,
					arpTaxSubtotalForeign = eRPARInvoiceInformationDto.arpTaxSubtotalForeign,
					arpTotalForResellerCommission = eRPARInvoiceInformationDto.arpTotalForResellerCommission,
					arpTotalForSalesCommission = eRPARInvoiceInformationDto.arpTotalForSalesCommission,
					CustomFields = eRPARInvoiceInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ARInvoices []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARInvoiceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aRInvoiceDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARInvoiceDto>> Process_PutARInvoice(ERPARInvoiceDto aRInvoice)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPARInvoiceDto createdObject = null;
		ERPResponseMessageDto<ERPARInvoiceDto> result;
		try
		{
			IERPARInvoiceRepository iERPARInvoiceRepository = (base.ERPARInvoiceRepository = new ERPARInvoiceRepository(base.ApiClientContext));
			using (iERPARInvoiceRepository)
			{
				APIValidationInfoDto postResult = await base.ERPARInvoiceRepository.SaveARInvoice(aRInvoice);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPARInvoiceInformationDto eRPARInvoiceInformationDto = await base.ERPARInvoiceRepository.GetARInvoice(aRInvoice.arpUniqueID);
					createdObject = new ERPARInvoiceDto
					{
						arpArGlAccountID = eRPARInvoiceInformationDto.arpArGlAccountID,
						arpArInvoiceContactID = eRPARInvoiceInformationDto.arpArInvoiceContactID,
						arpArInvoiceLocationID = eRPARInvoiceInformationDto.arpArInvoiceLocationID,
						arpArInvoiceID = eRPARInvoiceInformationDto.arpArInvoiceID,
						arpCommissionAmountBase = eRPARInvoiceInformationDto.arpCommissionAmountBase,
						arpCreatedBy = eRPARInvoiceInformationDto.arpCreatedBy,
						arpCreatedDate = eRPARInvoiceInformationDto.arpCreatedDate,
						arpCreditArInvoiceID = eRPARInvoiceInformationDto.arpCreditArInvoiceID,
						arpCreditDate = eRPARInvoiceInformationDto.arpCreditDate,
						arpCreditReasonID = eRPARInvoiceInformationDto.arpCreditReasonID,
						arpCurrencyRateID = eRPARInvoiceInformationDto.arpCurrencyRateID,
						arpCustomerOrganizationID = eRPARInvoiceInformationDto.arpCustomerOrganizationID,
						arpDepositAppliedBase = eRPARInvoiceInformationDto.arpDepositAppliedBase,
						arpDepositAppliedForeign = eRPARInvoiceInformationDto.arpDepositAppliedForeign,
						arpDepositBalanceBase = eRPARInvoiceInformationDto.arpDepositBalanceBase,
						arpDepositBalanceForeign = eRPARInvoiceInformationDto.arpDepositBalanceForeign,
						arpDepositGlAccountID = eRPARInvoiceInformationDto.arpDepositGlAccountID,
						arpDepositTransferredBase = eRPARInvoiceInformationDto.arpDepositTransferredBase,
						arpDepositTransferredForeign = eRPARInvoiceInformationDto.arpDepositTransferredForeign,
						arpDiscountDueDate = eRPARInvoiceInformationDto.arpDiscountDueDate,
						arpDiscountGlAccountID = eRPARInvoiceInformationDto.arpDiscountGlAccountID,
						arpDiscountTotalBase = eRPARInvoiceInformationDto.arpDiscountTotalBase,
						arpDiscountTotalForeign = eRPARInvoiceInformationDto.arpDiscountTotalForeign,
						arpDueDate = eRPARInvoiceInformationDto.arpDueDate,
						arpEdiTransferredDate = eRPARInvoiceInformationDto.arpEdiTransferredDate,
						arpUniqueID = eRPARInvoiceInformationDto.arpUniqueID,
						arpExchangeRate = eRPARInvoiceInformationDto.arpExchangeRate,
						arpFreeOnBoardDescription = eRPARInvoiceInformationDto.arpFreeOnBoardDescription,
						arpFreightAmountBase = eRPARInvoiceInformationDto.arpFreightAmountBase,
						arpFreightAmountForeign = eRPARInvoiceInformationDto.arpFreightAmountForeign,
						arpFreightGlAccountID = eRPARInvoiceInformationDto.arpFreightGlAccountID,
						arpFreightSubtotalBase = eRPARInvoiceInformationDto.arpFreightSubtotalBase,
						arpFreightSubtotalForeign = eRPARInvoiceInformationDto.arpFreightSubtotalForeign,
						arpFreightTaxAmountBase = eRPARInvoiceInformationDto.arpFreightTaxAmountBase,
						arpFreightTaxAmountForeign = eRPARInvoiceInformationDto.arpFreightTaxAmountForeign,
						arpFreightTaxCodeID = eRPARInvoiceInformationDto.arpFreightTaxCodeID,
						arpFreightTotalBase = eRPARInvoiceInformationDto.arpFreightTotalBase,
						arpFreightTotalForeign = eRPARInvoiceInformationDto.arpFreightTotalForeign,
						arpFullInvoiceSubtotalBase = eRPARInvoiceInformationDto.arpFullInvoiceSubtotalBase,
						arpFullInvoiceSubtotalForeign = eRPARInvoiceInformationDto.arpFullInvoiceSubtotalForeign,
						arpGlFiscalYearID = eRPARInvoiceInformationDto.arpGlFiscalYearID,
						arpGlFiscalYearPeriodID = eRPARInvoiceInformationDto.arpGlFiscalYearPeriodID,
						arpIntraCompanyPostedDate = eRPARInvoiceInformationDto.arpIntraCompanyPostedDate,
						arpInvoiceBalanceBase = eRPARInvoiceInformationDto.arpInvoiceBalanceBase,
						arpInvoiceBalanceForeign = eRPARInvoiceInformationDto.arpInvoiceBalanceForeign,
						arpInvoiceCommentsRTF = eRPARInvoiceInformationDto.arpInvoiceCommentsRTF,
						arpInvoiceCommentsText = eRPARInvoiceInformationDto.arpInvoiceCommentsText,
						arpInvoiceDate = eRPARInvoiceInformationDto.arpInvoiceDate,
						arpInvoicePaidBase = eRPARInvoiceInformationDto.arpInvoicePaidBase,
						arpInvoicePaidForeign = eRPARInvoiceInformationDto.arpInvoicePaidForeign,
						arpInvoiceSubtotalBase = eRPARInvoiceInformationDto.arpInvoiceSubtotalBase,
						arpInvoiceSubtotalForeign = eRPARInvoiceInformationDto.arpInvoiceSubtotalForeign,
						arpInvoiceTaxAmountBase = eRPARInvoiceInformationDto.arpInvoiceTaxAmountBase,
						arpInvoiceTaxAmountForeign = eRPARInvoiceInformationDto.arpInvoiceTaxAmountForeign,
						arpInvoiceTotalBase = eRPARInvoiceInformationDto.arpInvoiceTotalBase,
						arpInvoiceTotalForeign = eRPARInvoiceInformationDto.arpInvoiceTotalForeign,
						arpInvoiceType = eRPARInvoiceInformationDto.arpInvoiceType,
						arpAvalaraOverrideTax = eRPARInvoiceInformationDto.arpAvalaraOverrideTax,
						arpAvalaraTaxCalculated = eRPARInvoiceInformationDto.arpAvalaraTaxCalculated,
						arpCustomRate = eRPARInvoiceInformationDto.arpCustomRate,
						arpDepositCredit = eRPARInvoiceInformationDto.arpDepositCredit,
						arpEdiTransferred = eRPARInvoiceInformationDto.arpEdiTransferred,
						arpIncludeFreightInPrice = eRPARInvoiceInformationDto.arpIncludeFreightInPrice,
						arpIncludeTaxInRetention = eRPARInvoiceInformationDto.arpIncludeTaxInRetention,
						arpIntraCompany = eRPARInvoiceInformationDto.arpIntraCompany,
						arpIntraCompanyPosted = eRPARInvoiceInformationDto.arpIntraCompanyPosted,
						arpOnHold = eRPARInvoiceInformationDto.arpOnHold,
						arpOpenInvoiceLoad = eRPARInvoiceInformationDto.arpOpenInvoiceLoad,
						arpOverpayment = eRPARInvoiceInformationDto.arpOverpayment,
						arpPaidComplete = eRPARInvoiceInformationDto.arpPaidComplete,
						arpPostedToGl = eRPARInvoiceInformationDto.arpPostedToGl,
						arpReadyToPrint = eRPARInvoiceInformationDto.arpReadyToPrint,
						arpRecurringInvoice = eRPARInvoiceInformationDto.arpRecurringInvoice,
						arpRefundCheckRequired = eRPARInvoiceInformationDto.arpRefundCheckRequired,
						arpLineCommissionTotal = eRPARInvoiceInformationDto.arpLineCommissionTotal,
						arpOrderDate = eRPARInvoiceInformationDto.arpOrderDate,
						arpOriginalExchangeRate = eRPARInvoiceInformationDto.arpOriginalExchangeRate,
						arpOverPaymentHeaderID = eRPARInvoiceInformationDto.arpOverPaymentHeaderID,
						arpOverPaymentSessionID = eRPARInvoiceInformationDto.arpOverPaymentSessionID,
						arpPaidDate = eRPARInvoiceInformationDto.arpPaidDate,
						arpPaymentTermID = eRPARInvoiceInformationDto.arpPaymentTermID,
						arpPlantDepartmentID = eRPARInvoiceInformationDto.arpPlantDepartmentID,
						arpPlantID = eRPARInvoiceInformationDto.arpPlantID,
						arpPointOfSaleTerminalID = eRPARInvoiceInformationDto.arpPointOfSaleTerminalID,
						arpPostedDate = eRPARInvoiceInformationDto.arpPostedDate,
						arpProjectID = eRPARInvoiceInformationDto.arpProjectID,
						arpResellerCommissionAmount = eRPARInvoiceInformationDto.arpResellerCommissionAmount,
						arpResellerCommissionRate = eRPARInvoiceInformationDto.arpResellerCommissionRate,
						arpResellerContactID = eRPARInvoiceInformationDto.arpResellerContactID,
						arpResellerLocationID = eRPARInvoiceInformationDto.arpResellerLocationID,
						arpResellerOrganizationID = eRPARInvoiceInformationDto.arpResellerOrganizationID,
						arpRetentionBalanceBase = eRPARInvoiceInformationDto.arpRetentionBalanceBase,
						arpRetentionBalanceForeign = eRPARInvoiceInformationDto.arpRetentionBalanceForeign,
						arpRetentionPaidBase = eRPARInvoiceInformationDto.arpRetentionPaidBase,
						arpRetentionPaidForeign = eRPARInvoiceInformationDto.arpRetentionPaidForeign,
						arpRetentionTotalBase = eRPARInvoiceInformationDto.arpRetentionTotalBase,
						arpRetentionTotalForeign = eRPARInvoiceInformationDto.arpRetentionTotalForeign,
						arpRowVersion = eRPARInvoiceInformationDto.arpRowVersion,
						arpSalesCommissionTotal = eRPARInvoiceInformationDto.arpSalesCommissionTotal,
						arpSalesGlAccountID = eRPARInvoiceInformationDto.arpSalesGlAccountID,
						arpSecondFreightTaxAmtBase = eRPARInvoiceInformationDto.arpSecondFreightTaxAmtBase,
						arpSecondFreightTaxAmtForeign = eRPARInvoiceInformationDto.arpSecondFreightTaxAmtForeign,
						arpSecondFreightTaxCodeID = eRPARInvoiceInformationDto.arpSecondFreightTaxCodeID,
						arpShipContactID = eRPARInvoiceInformationDto.arpShipContactID,
						arpShipLocationID = eRPARInvoiceInformationDto.arpShipLocationID,
						arpShipOrganizationID = eRPARInvoiceInformationDto.arpShipOrganizationID,
						arpShippingMethodID = eRPARInvoiceInformationDto.arpShippingMethodID,
						arpShippingPaymentTypeID = eRPARInvoiceInformationDto.arpShippingPaymentTypeID,
						arpSplitPercentTotal = eRPARInvoiceInformationDto.arpSplitPercentTotal,
						arpStandardMessageID = eRPARInvoiceInformationDto.arpStandardMessageID,
						arpTaxDate = eRPARInvoiceInformationDto.arpTaxDate,
						arpTaxSubtotalBase = eRPARInvoiceInformationDto.arpTaxSubtotalBase,
						arpTaxSubtotalForeign = eRPARInvoiceInformationDto.arpTaxSubtotalForeign,
						arpTotalForResellerCommission = eRPARInvoiceInformationDto.arpTotalForResellerCommission,
						arpTotalForSalesCommission = eRPARInvoiceInformationDto.arpTotalForSalesCommission,
						CustomFields = eRPARInvoiceInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ARInvoice [{aRInvoice.arpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARInvoiceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteARInvoice(Guid aRInvoiceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARInvoiceRepository iERPARInvoiceRepository = (base.ERPARInvoiceRepository = new ERPARInvoiceRepository(base.ApiClientContext));
		using (iERPARInvoiceRepository)
		{
			if (!(await base.ERPARInvoiceRepository.DoesARInvoiceExist(aRInvoiceId)))
			{
				base.ErrorsList.Add($"ARInvoice [{aRInvoiceId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPARInvoiceInformationDto eRPARInvoiceInformationDto = await base.ERPARInvoiceRepository.GetARInvoice(aRInvoiceId);
				string text = await base.ERPARInvoiceRepository.WhereUsed("ARInvoices", new object[1] { eRPARInvoiceInformationDto.arpArInvoiceID }, new object[1] { "arpArInvoiceID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ARInvoice cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPARInvoiceDto>> Process_DeleteARInvoice(Guid aRInvoiceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPARInvoiceDto> result;
		try
		{
			IERPARInvoiceRepository iERPARInvoiceRepository = (base.ERPARInvoiceRepository = new ERPARInvoiceRepository(base.ApiClientContext));
			using (iERPARInvoiceRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPARInvoiceRepository.DeleteRowFromTable("ARInvoices", "arp", aRInvoiceId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ARInvoice [{aRInvoiceId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARInvoiceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPARInvoiceDto()
			};
		}
		return result;
	}
}
