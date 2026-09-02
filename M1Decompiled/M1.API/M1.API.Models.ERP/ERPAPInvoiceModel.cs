using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAPInvoiceModel : ERPBaseModel, IERPAPInvoiceModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAPInvoices(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAPInvoiceRepository iERPAPInvoiceRepository = (base.ERPAPInvoiceRepository = new ERPAPInvoiceRepository(base.ApiClientContext));
		using (iERPAPInvoiceRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAPInvoiceRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAPInvoiceRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAPInvoiceRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAPInvoiceRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAPInvoice(Guid aPInvoiceId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPInvoiceRepository iERPAPInvoiceRepository = (base.ERPAPInvoiceRepository = new ERPAPInvoiceRepository(base.ApiClientContext));
		using (iERPAPInvoiceRepository)
		{
			if (!(await base.ERPAPInvoiceRepository.DoesAPInvoiceExist(aPInvoiceId)))
			{
				errorsList.Add($"APInvoice [{aPInvoiceId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAPInvoice(ERPAPInvoiceDto aPInvoice)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPInvoiceRepository iERPAPInvoiceRepository = (base.ERPAPInvoiceRepository = new ERPAPInvoiceRepository(base.ApiClientContext));
		using (iERPAPInvoiceRepository)
		{
			if (!string.IsNullOrWhiteSpace(aPInvoice.appPlantDepartmentID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { aPInvoice.appPlantID, aPInvoice.appPlantDepartmentID })))
			{
				errorsList.Add("appPlantDepartmentID [" + aPInvoice.appPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoice.appPlantID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { aPInvoice.appPlantID })))
			{
				errorsList.Add("appPlantID [" + aPInvoice.appPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoice.appSupplierOrganizationID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { aPInvoice.appSupplierOrganizationID })))
			{
				errorsList.Add("appSupplierOrganizationID [" + aPInvoice.appSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoice.appApInvoiceLocationID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { aPInvoice.appSupplierOrganizationID, aPInvoice.appApInvoiceLocationID })))
			{
				errorsList.Add("appApInvoiceLocationID [" + aPInvoice.appApInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoice.appApInvoiceContactID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { aPInvoice.appSupplierOrganizationID, aPInvoice.appApInvoiceLocationID, aPInvoice.appApInvoiceContactID })))
			{
				errorsList.Add("appApInvoiceContactID [" + aPInvoice.appApInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoice.appCreditReasonID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { aPInvoice.appCreditReasonID })))
			{
				errorsList.Add("appCreditReasonID [" + aPInvoice.appCreditReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoice.appCreditApInvoiceID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { aPInvoice.appCreditApInvoiceID })))
			{
				errorsList.Add("appCreditApInvoiceID [" + aPInvoice.appCreditApInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoice.appPaymentTermID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("PaymentTerms", new object[1] { "XATPAYMENTTERMID" }, new object[1] { aPInvoice.appPaymentTermID })))
			{
				errorsList.Add("appPaymentTermID [" + aPInvoice.appPaymentTermID + "] not found.");
			}
			if (aPInvoice.appGlFiscalYearID > 0 && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { aPInvoice.appGlFiscalYearID })))
			{
				errorsList.Add($"appGlFiscalYearID [{aPInvoice.appGlFiscalYearID}] not found.");
			}
			if (aPInvoice.appGlFiscalYearPeriodID > 0 && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearPeriods", new object[2] { "GLFGLFISCALYEARID", "GLFGLFISCALYEARPERIODID" }, new object[2] { aPInvoice.appGlFiscalYearID, aPInvoice.appGlFiscalYearPeriodID })))
			{
				errorsList.Add($"appGlFiscalYearPeriodID [{aPInvoice.appGlFiscalYearPeriodID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoice.appCurrencyRateID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { aPInvoice.appCurrencyRateID })))
			{
				errorsList.Add("appCurrencyRateID [" + aPInvoice.appCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoice.appFreightTaxCodeID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aPInvoice.appFreightTaxCodeID })))
			{
				errorsList.Add("appFreightTaxCodeID [" + aPInvoice.appFreightTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoice.appSecondFreightTaxCodeID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aPInvoice.appSecondFreightTaxCodeID })))
			{
				errorsList.Add("appSecondFreightTaxCodeID [" + aPInvoice.appSecondFreightTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoice.appApGlAccountID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aPInvoice.appApGlAccountID })))
			{
				errorsList.Add("appApGlAccountID [" + aPInvoice.appApGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoice.appFreightGlAccountID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aPInvoice.appFreightGlAccountID })))
			{
				errorsList.Add("appFreightGlAccountID [" + aPInvoice.appFreightGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPInvoice.appProjectID) && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { aPInvoice.appProjectID })))
			{
				errorsList.Add("appProjectID [" + aPInvoice.appProjectID + "] not found.");
			}
			if (aPInvoice.appOverPaymentSessionID > 0 && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("APPaymentSessions", new object[1] { "APSAPPAYMENTSESSIONID" }, new object[1] { aPInvoice.appOverPaymentSessionID })))
			{
				errorsList.Add($"appOverPaymentSessionID [{aPInvoice.appOverPaymentSessionID}] not found.");
			}
			if (aPInvoice.appOverPaymentHeaderID > 0 && !(await base.ERPAPInvoiceRepository.DoesRecordExistInTableUsingKeys("APPaymentHeaders", new object[2] { "APTAPPAYMENTSESSIONID", "APTAPPAYMENTHEADERID" }, new object[2] { aPInvoice.appOverPaymentSessionID, aPInvoice.appOverPaymentHeaderID })))
			{
				errorsList.Add($"appOverPaymentHeaderID [{aPInvoice.appOverPaymentHeaderID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAPInvoiceDto>>> Process_GetAllAPInvoices(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAPInvoiceDto> allAPInvoicesDto = new List<ERPAPInvoiceDto>();
		ERPResponseMessageDto<IList<ERPAPInvoiceDto>> result;
		try
		{
			IERPAPInvoiceRepository iERPAPInvoiceRepository = (base.ERPAPInvoiceRepository = new ERPAPInvoiceRepository(base.ApiClientContext));
			using (iERPAPInvoiceRepository)
			{
				foreach (ERPAPInvoiceInformationDto item2 in await base.ERPAPInvoiceRepository.GetAllAPInvoices(pageSize, pageNumber, filter, orderBy))
				{
					ERPAPInvoiceDto item = new ERPAPInvoiceDto
					{
						appApGlAccountID = item2.appApGlAccountID,
						appApInvoiceContactID = item2.appApInvoiceContactID,
						appApInvoiceLocationID = item2.appApInvoiceLocationID,
						appApInvoiceID = item2.appApInvoiceID,
						appCreatedBy = item2.appCreatedBy,
						appCreatedDate = item2.appCreatedDate,
						appCreditApInvoiceID = item2.appCreditApInvoiceID,
						appCreditDate = item2.appCreditDate,
						appCreditReasonID = item2.appCreditReasonID,
						appCurrencyRateID = item2.appCurrencyRateID,
						appDiscountAmountBase = item2.appDiscountAmountBase,
						appDiscountAmountForeign = item2.appDiscountAmountForeign,
						appDiscountDueDate = item2.appDiscountDueDate,
						appDueDate = item2.appDueDate,
						appUniqueID = item2.appUniqueID,
						appExchangeRate = item2.appExchangeRate,
						appFreightAmountBase = item2.appFreightAmountBase,
						appFreightAmountForeign = item2.appFreightAmountForeign,
						appFreightGlAccountID = item2.appFreightGlAccountID,
						appFreightTaxAmountBase = item2.appFreightTaxAmountBase,
						appFreightTaxAmountForeign = item2.appFreightTaxAmountForeign,
						appFreightTaxCodeID = item2.appFreightTaxCodeID,
						appGlFiscalYearID = item2.appGlFiscalYearID,
						appGlFiscalYearPeriodID = item2.appGlFiscalYearPeriodID,
						appInvoiceBalanceBase = item2.appInvoiceBalanceBase,
						appInvoiceBalanceForeign = item2.appInvoiceBalanceForeign,
						appInvoiceCommentsRTF = item2.appInvoiceCommentsRTF,
						appInvoiceCommentsText = item2.appInvoiceCommentsText,
						appInvoiceDate = item2.appInvoiceDate,
						appInvoiceDescription = item2.appInvoiceDescription,
						appInvoiceSubtotalBase = item2.appInvoiceSubtotalBase,
						appInvoiceSubtotalForeign = item2.appInvoiceSubtotalForeign,
						appInvoiceTaxAmountBase = item2.appInvoiceTaxAmountBase,
						appInvoiceTaxAmountForeign = item2.appInvoiceTaxAmountForeign,
						appInvoiceTotalBase = item2.appInvoiceTotalBase,
						appInvoiceTotalForeign = item2.appInvoiceTotalForeign,
						appInvoiceType = item2.appInvoiceType,
						appCustomRate = item2.appCustomRate,
						appOnHold = item2.appOnHold,
						appOpenInvoiceLoad = item2.appOpenInvoiceLoad,
						appOverpayment = item2.appOverpayment,
						appPaidComplete = item2.appPaidComplete,
						appPostedToGl = item2.appPostedToGl,
						appTaxReportable = item2.appTaxReportable,
						appOriginalExchangeRate = item2.appOriginalExchangeRate,
						appOverPaymentHeaderID = item2.appOverPaymentHeaderID,
						appOverPaymentSessionID = item2.appOverPaymentSessionID,
						appPaidDate = item2.appPaidDate,
						appPaymentTermID = item2.appPaymentTermID,
						appPlantDepartmentID = item2.appPlantDepartmentID,
						appPlantID = item2.appPlantID,
						appPostedDate = item2.appPostedDate,
						appProjectID = item2.appProjectID,
						appRetentionBalanceBase = item2.appRetentionBalanceBase,
						appRetentionBalanceForeign = item2.appRetentionBalanceForeign,
						appRetentionTotalBase = item2.appRetentionTotalBase,
						appRetentionTotalForeign = item2.appRetentionTotalForeign,
						appRowVersion = item2.appRowVersion,
						appSecondFreightTaxAmtBase = item2.appSecondFreightTaxAmtBase,
						appSecondFreightTaxAmtForeign = item2.appSecondFreightTaxAmtForeign,
						appSecondFreightTaxCodeID = item2.appSecondFreightTaxCodeID,
						appSupplierInvoiceNumber = item2.appSupplierInvoiceNumber,
						appSupplierOrganizationID = item2.appSupplierOrganizationID,
						CustomFields = item2.CustomFields
					};
					allAPInvoicesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all APInvoices]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAPInvoiceDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAPInvoicesDto,
				RecordCount = allAPInvoicesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPInvoiceDto>> Process_GetAPInvoice(Guid aPInvoiceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAPInvoiceDto aPInvoiceDto = null;
		ERPResponseMessageDto<ERPAPInvoiceDto> result;
		try
		{
			IERPAPInvoiceRepository iERPAPInvoiceRepository = (base.ERPAPInvoiceRepository = new ERPAPInvoiceRepository(base.ApiClientContext));
			using (iERPAPInvoiceRepository)
			{
				ERPAPInvoiceInformationDto eRPAPInvoiceInformationDto = await base.ERPAPInvoiceRepository.GetAPInvoice(aPInvoiceId);
				aPInvoiceDto = new ERPAPInvoiceDto
				{
					appApGlAccountID = eRPAPInvoiceInformationDto.appApGlAccountID,
					appApInvoiceContactID = eRPAPInvoiceInformationDto.appApInvoiceContactID,
					appApInvoiceLocationID = eRPAPInvoiceInformationDto.appApInvoiceLocationID,
					appApInvoiceID = eRPAPInvoiceInformationDto.appApInvoiceID,
					appCreatedBy = eRPAPInvoiceInformationDto.appCreatedBy,
					appCreatedDate = eRPAPInvoiceInformationDto.appCreatedDate,
					appCreditApInvoiceID = eRPAPInvoiceInformationDto.appCreditApInvoiceID,
					appCreditDate = eRPAPInvoiceInformationDto.appCreditDate,
					appCreditReasonID = eRPAPInvoiceInformationDto.appCreditReasonID,
					appCurrencyRateID = eRPAPInvoiceInformationDto.appCurrencyRateID,
					appDiscountAmountBase = eRPAPInvoiceInformationDto.appDiscountAmountBase,
					appDiscountAmountForeign = eRPAPInvoiceInformationDto.appDiscountAmountForeign,
					appDiscountDueDate = eRPAPInvoiceInformationDto.appDiscountDueDate,
					appDueDate = eRPAPInvoiceInformationDto.appDueDate,
					appUniqueID = eRPAPInvoiceInformationDto.appUniqueID,
					appExchangeRate = eRPAPInvoiceInformationDto.appExchangeRate,
					appFreightAmountBase = eRPAPInvoiceInformationDto.appFreightAmountBase,
					appFreightAmountForeign = eRPAPInvoiceInformationDto.appFreightAmountForeign,
					appFreightGlAccountID = eRPAPInvoiceInformationDto.appFreightGlAccountID,
					appFreightTaxAmountBase = eRPAPInvoiceInformationDto.appFreightTaxAmountBase,
					appFreightTaxAmountForeign = eRPAPInvoiceInformationDto.appFreightTaxAmountForeign,
					appFreightTaxCodeID = eRPAPInvoiceInformationDto.appFreightTaxCodeID,
					appGlFiscalYearID = eRPAPInvoiceInformationDto.appGlFiscalYearID,
					appGlFiscalYearPeriodID = eRPAPInvoiceInformationDto.appGlFiscalYearPeriodID,
					appInvoiceBalanceBase = eRPAPInvoiceInformationDto.appInvoiceBalanceBase,
					appInvoiceBalanceForeign = eRPAPInvoiceInformationDto.appInvoiceBalanceForeign,
					appInvoiceCommentsRTF = eRPAPInvoiceInformationDto.appInvoiceCommentsRTF,
					appInvoiceCommentsText = eRPAPInvoiceInformationDto.appInvoiceCommentsText,
					appInvoiceDate = eRPAPInvoiceInformationDto.appInvoiceDate,
					appInvoiceDescription = eRPAPInvoiceInformationDto.appInvoiceDescription,
					appInvoiceSubtotalBase = eRPAPInvoiceInformationDto.appInvoiceSubtotalBase,
					appInvoiceSubtotalForeign = eRPAPInvoiceInformationDto.appInvoiceSubtotalForeign,
					appInvoiceTaxAmountBase = eRPAPInvoiceInformationDto.appInvoiceTaxAmountBase,
					appInvoiceTaxAmountForeign = eRPAPInvoiceInformationDto.appInvoiceTaxAmountForeign,
					appInvoiceTotalBase = eRPAPInvoiceInformationDto.appInvoiceTotalBase,
					appInvoiceTotalForeign = eRPAPInvoiceInformationDto.appInvoiceTotalForeign,
					appInvoiceType = eRPAPInvoiceInformationDto.appInvoiceType,
					appCustomRate = eRPAPInvoiceInformationDto.appCustomRate,
					appOnHold = eRPAPInvoiceInformationDto.appOnHold,
					appOpenInvoiceLoad = eRPAPInvoiceInformationDto.appOpenInvoiceLoad,
					appOverpayment = eRPAPInvoiceInformationDto.appOverpayment,
					appPaidComplete = eRPAPInvoiceInformationDto.appPaidComplete,
					appPostedToGl = eRPAPInvoiceInformationDto.appPostedToGl,
					appTaxReportable = eRPAPInvoiceInformationDto.appTaxReportable,
					appOriginalExchangeRate = eRPAPInvoiceInformationDto.appOriginalExchangeRate,
					appOverPaymentHeaderID = eRPAPInvoiceInformationDto.appOverPaymentHeaderID,
					appOverPaymentSessionID = eRPAPInvoiceInformationDto.appOverPaymentSessionID,
					appPaidDate = eRPAPInvoiceInformationDto.appPaidDate,
					appPaymentTermID = eRPAPInvoiceInformationDto.appPaymentTermID,
					appPlantDepartmentID = eRPAPInvoiceInformationDto.appPlantDepartmentID,
					appPlantID = eRPAPInvoiceInformationDto.appPlantID,
					appPostedDate = eRPAPInvoiceInformationDto.appPostedDate,
					appProjectID = eRPAPInvoiceInformationDto.appProjectID,
					appRetentionBalanceBase = eRPAPInvoiceInformationDto.appRetentionBalanceBase,
					appRetentionBalanceForeign = eRPAPInvoiceInformationDto.appRetentionBalanceForeign,
					appRetentionTotalBase = eRPAPInvoiceInformationDto.appRetentionTotalBase,
					appRetentionTotalForeign = eRPAPInvoiceInformationDto.appRetentionTotalForeign,
					appRowVersion = eRPAPInvoiceInformationDto.appRowVersion,
					appSecondFreightTaxAmtBase = eRPAPInvoiceInformationDto.appSecondFreightTaxAmtBase,
					appSecondFreightTaxAmtForeign = eRPAPInvoiceInformationDto.appSecondFreightTaxAmtForeign,
					appSecondFreightTaxCodeID = eRPAPInvoiceInformationDto.appSecondFreightTaxCodeID,
					appSupplierInvoiceNumber = eRPAPInvoiceInformationDto.appSupplierInvoiceNumber,
					appSupplierOrganizationID = eRPAPInvoiceInformationDto.appSupplierOrganizationID,
					CustomFields = eRPAPInvoiceInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the APInvoices []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPInvoiceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aPInvoiceDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPInvoiceDto>> Process_PutAPInvoice(ERPAPInvoiceDto aPInvoice)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAPInvoiceDto createdObject = null;
		ERPResponseMessageDto<ERPAPInvoiceDto> result;
		try
		{
			IERPAPInvoiceRepository iERPAPInvoiceRepository = (base.ERPAPInvoiceRepository = new ERPAPInvoiceRepository(base.ApiClientContext));
			using (iERPAPInvoiceRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAPInvoiceRepository.SaveAPInvoice(aPInvoice);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAPInvoiceInformationDto eRPAPInvoiceInformationDto = await base.ERPAPInvoiceRepository.GetAPInvoice(aPInvoice.appUniqueID);
					createdObject = new ERPAPInvoiceDto
					{
						appApGlAccountID = eRPAPInvoiceInformationDto.appApGlAccountID,
						appApInvoiceContactID = eRPAPInvoiceInformationDto.appApInvoiceContactID,
						appApInvoiceLocationID = eRPAPInvoiceInformationDto.appApInvoiceLocationID,
						appApInvoiceID = eRPAPInvoiceInformationDto.appApInvoiceID,
						appCreatedBy = eRPAPInvoiceInformationDto.appCreatedBy,
						appCreatedDate = eRPAPInvoiceInformationDto.appCreatedDate,
						appCreditApInvoiceID = eRPAPInvoiceInformationDto.appCreditApInvoiceID,
						appCreditDate = eRPAPInvoiceInformationDto.appCreditDate,
						appCreditReasonID = eRPAPInvoiceInformationDto.appCreditReasonID,
						appCurrencyRateID = eRPAPInvoiceInformationDto.appCurrencyRateID,
						appDiscountAmountBase = eRPAPInvoiceInformationDto.appDiscountAmountBase,
						appDiscountAmountForeign = eRPAPInvoiceInformationDto.appDiscountAmountForeign,
						appDiscountDueDate = eRPAPInvoiceInformationDto.appDiscountDueDate,
						appDueDate = eRPAPInvoiceInformationDto.appDueDate,
						appUniqueID = eRPAPInvoiceInformationDto.appUniqueID,
						appExchangeRate = eRPAPInvoiceInformationDto.appExchangeRate,
						appFreightAmountBase = eRPAPInvoiceInformationDto.appFreightAmountBase,
						appFreightAmountForeign = eRPAPInvoiceInformationDto.appFreightAmountForeign,
						appFreightGlAccountID = eRPAPInvoiceInformationDto.appFreightGlAccountID,
						appFreightTaxAmountBase = eRPAPInvoiceInformationDto.appFreightTaxAmountBase,
						appFreightTaxAmountForeign = eRPAPInvoiceInformationDto.appFreightTaxAmountForeign,
						appFreightTaxCodeID = eRPAPInvoiceInformationDto.appFreightTaxCodeID,
						appGlFiscalYearID = eRPAPInvoiceInformationDto.appGlFiscalYearID,
						appGlFiscalYearPeriodID = eRPAPInvoiceInformationDto.appGlFiscalYearPeriodID,
						appInvoiceBalanceBase = eRPAPInvoiceInformationDto.appInvoiceBalanceBase,
						appInvoiceBalanceForeign = eRPAPInvoiceInformationDto.appInvoiceBalanceForeign,
						appInvoiceCommentsRTF = eRPAPInvoiceInformationDto.appInvoiceCommentsRTF,
						appInvoiceCommentsText = eRPAPInvoiceInformationDto.appInvoiceCommentsText,
						appInvoiceDate = eRPAPInvoiceInformationDto.appInvoiceDate,
						appInvoiceDescription = eRPAPInvoiceInformationDto.appInvoiceDescription,
						appInvoiceSubtotalBase = eRPAPInvoiceInformationDto.appInvoiceSubtotalBase,
						appInvoiceSubtotalForeign = eRPAPInvoiceInformationDto.appInvoiceSubtotalForeign,
						appInvoiceTaxAmountBase = eRPAPInvoiceInformationDto.appInvoiceTaxAmountBase,
						appInvoiceTaxAmountForeign = eRPAPInvoiceInformationDto.appInvoiceTaxAmountForeign,
						appInvoiceTotalBase = eRPAPInvoiceInformationDto.appInvoiceTotalBase,
						appInvoiceTotalForeign = eRPAPInvoiceInformationDto.appInvoiceTotalForeign,
						appInvoiceType = eRPAPInvoiceInformationDto.appInvoiceType,
						appCustomRate = eRPAPInvoiceInformationDto.appCustomRate,
						appOnHold = eRPAPInvoiceInformationDto.appOnHold,
						appOpenInvoiceLoad = eRPAPInvoiceInformationDto.appOpenInvoiceLoad,
						appOverpayment = eRPAPInvoiceInformationDto.appOverpayment,
						appPaidComplete = eRPAPInvoiceInformationDto.appPaidComplete,
						appPostedToGl = eRPAPInvoiceInformationDto.appPostedToGl,
						appTaxReportable = eRPAPInvoiceInformationDto.appTaxReportable,
						appOriginalExchangeRate = eRPAPInvoiceInformationDto.appOriginalExchangeRate,
						appOverPaymentHeaderID = eRPAPInvoiceInformationDto.appOverPaymentHeaderID,
						appOverPaymentSessionID = eRPAPInvoiceInformationDto.appOverPaymentSessionID,
						appPaidDate = eRPAPInvoiceInformationDto.appPaidDate,
						appPaymentTermID = eRPAPInvoiceInformationDto.appPaymentTermID,
						appPlantDepartmentID = eRPAPInvoiceInformationDto.appPlantDepartmentID,
						appPlantID = eRPAPInvoiceInformationDto.appPlantID,
						appPostedDate = eRPAPInvoiceInformationDto.appPostedDate,
						appProjectID = eRPAPInvoiceInformationDto.appProjectID,
						appRetentionBalanceBase = eRPAPInvoiceInformationDto.appRetentionBalanceBase,
						appRetentionBalanceForeign = eRPAPInvoiceInformationDto.appRetentionBalanceForeign,
						appRetentionTotalBase = eRPAPInvoiceInformationDto.appRetentionTotalBase,
						appRetentionTotalForeign = eRPAPInvoiceInformationDto.appRetentionTotalForeign,
						appRowVersion = eRPAPInvoiceInformationDto.appRowVersion,
						appSecondFreightTaxAmtBase = eRPAPInvoiceInformationDto.appSecondFreightTaxAmtBase,
						appSecondFreightTaxAmtForeign = eRPAPInvoiceInformationDto.appSecondFreightTaxAmtForeign,
						appSecondFreightTaxCodeID = eRPAPInvoiceInformationDto.appSecondFreightTaxCodeID,
						appSupplierInvoiceNumber = eRPAPInvoiceInformationDto.appSupplierInvoiceNumber,
						appSupplierOrganizationID = eRPAPInvoiceInformationDto.appSupplierOrganizationID,
						CustomFields = eRPAPInvoiceInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing APInvoice [{aPInvoice.appUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPInvoiceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAPInvoice(Guid aPInvoiceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPInvoiceRepository iERPAPInvoiceRepository = (base.ERPAPInvoiceRepository = new ERPAPInvoiceRepository(base.ApiClientContext));
		using (iERPAPInvoiceRepository)
		{
			if (!(await base.ERPAPInvoiceRepository.DoesAPInvoiceExist(aPInvoiceId)))
			{
				base.ErrorsList.Add($"APInvoice [{aPInvoiceId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAPInvoiceInformationDto eRPAPInvoiceInformationDto = await base.ERPAPInvoiceRepository.GetAPInvoice(aPInvoiceId);
				string text = await base.ERPAPInvoiceRepository.WhereUsed("APInvoices", new object[1] { eRPAPInvoiceInformationDto.appApInvoiceID }, new object[1] { "appApInvoiceID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("APInvoice cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAPInvoiceDto>> Process_DeleteAPInvoice(Guid aPInvoiceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAPInvoiceDto> result;
		try
		{
			IERPAPInvoiceRepository iERPAPInvoiceRepository = (base.ERPAPInvoiceRepository = new ERPAPInvoiceRepository(base.ApiClientContext));
			using (iERPAPInvoiceRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAPInvoiceRepository.DeleteRowFromTable("APInvoices", "app", aPInvoiceId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of APInvoice [{aPInvoiceId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPInvoiceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAPInvoiceDto()
			};
		}
		return result;
	}
}
