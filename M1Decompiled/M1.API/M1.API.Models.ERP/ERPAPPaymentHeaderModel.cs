using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAPPaymentHeaderModel : ERPBaseModel, IERPAPPaymentHeaderModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAPPaymentHeaders(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAPPaymentHeaderRepository iERPAPPaymentHeaderRepository = (base.ERPAPPaymentHeaderRepository = new ERPAPPaymentHeaderRepository(base.ApiClientContext));
		using (iERPAPPaymentHeaderRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAPPaymentHeaderRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAPPaymentHeaderRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAPPaymentHeaderRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAPPaymentHeaderRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAPPaymentHeader(Guid aPPaymentHeaderId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPPaymentHeaderRepository iERPAPPaymentHeaderRepository = (base.ERPAPPaymentHeaderRepository = new ERPAPPaymentHeaderRepository(base.ApiClientContext));
		using (iERPAPPaymentHeaderRepository)
		{
			if (!(await base.ERPAPPaymentHeaderRepository.DoesAPPaymentHeaderExist(aPPaymentHeaderId)))
			{
				errorsList.Add($"APPaymentHeader [{aPPaymentHeaderId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAPPaymentHeader(ERPAPPaymentHeaderDto aPPaymentHeader)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPPaymentHeaderRepository iERPAPPaymentHeaderRepository = (base.ERPAPPaymentHeaderRepository = new ERPAPPaymentHeaderRepository(base.ApiClientContext));
		using (iERPAPPaymentHeaderRepository)
		{
			if (aPPaymentHeader.aptApPaymentSessionID > 0 && !(await base.ERPAPPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("APPaymentSessions", new object[1] { "APSAPPAYMENTSESSIONID" }, new object[1] { aPPaymentHeader.aptApPaymentSessionID })))
			{
				errorsList.Add($"aptApPaymentSessionID [{aPPaymentHeader.aptApPaymentSessionID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentHeader.aptSupplierOrganizationID) && !(await base.ERPAPPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { aPPaymentHeader.aptSupplierOrganizationID })))
			{
				errorsList.Add("aptSupplierOrganizationID [" + aPPaymentHeader.aptSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentHeader.aptApInvoiceLocationID) && !(await base.ERPAPPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { aPPaymentHeader.aptSupplierOrganizationID, aPPaymentHeader.aptApInvoiceLocationID })))
			{
				errorsList.Add("aptApInvoiceLocationID [" + aPPaymentHeader.aptApInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentHeader.aptApInvoiceContactID) && !(await base.ERPAPPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { aPPaymentHeader.aptSupplierOrganizationID, aPPaymentHeader.aptApInvoiceLocationID, aPPaymentHeader.aptApInvoiceContactID })))
			{
				errorsList.Add("aptApInvoiceContactID [" + aPPaymentHeader.aptApInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentHeader.aptCashGlAccountID) && !(await base.ERPAPPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aPPaymentHeader.aptCashGlAccountID })))
			{
				errorsList.Add("aptCashGlAccountID [" + aPPaymentHeader.aptCashGlAccountID + "] not found.");
			}
			if (aPPaymentHeader.aptGlFiscalYearID > 0 && !(await base.ERPAPPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { aPPaymentHeader.aptGlFiscalYearID })))
			{
				errorsList.Add($"aptGlFiscalYearID [{aPPaymentHeader.aptGlFiscalYearID}] not found.");
			}
			if (aPPaymentHeader.aptGlFiscalYearPeriodID > 0 && !(await base.ERPAPPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearPeriods", new object[2] { "GLFGLFISCALYEARID", "GLFGLFISCALYEARPERIODID" }, new object[2] { aPPaymentHeader.aptGlFiscalYearID, aPPaymentHeader.aptGlFiscalYearPeriodID })))
			{
				errorsList.Add($"aptGlFiscalYearPeriodID [{aPPaymentHeader.aptGlFiscalYearPeriodID}] not found.");
			}
			if (aPPaymentHeader.aptVoidApPaymentSessionID > 0 && !(await base.ERPAPPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("APPaymentSessions", new object[1] { "APSAPPAYMENTSESSIONID" }, new object[1] { aPPaymentHeader.aptVoidApPaymentSessionID })))
			{
				errorsList.Add($"aptVoidApPaymentSessionID [{aPPaymentHeader.aptVoidApPaymentSessionID}] not found.");
			}
			if (aPPaymentHeader.aptVoidApPaymentHeaderID > 0 && !(await base.ERPAPPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("APPaymentHeaders", new object[2] { "APTAPPAYMENTSESSIONID", "APTAPPAYMENTHEADERID" }, new object[2] { aPPaymentHeader.aptVoidApPaymentSessionID, aPPaymentHeader.aptVoidApPaymentHeaderID })))
			{
				errorsList.Add($"aptVoidApPaymentHeaderID [{aPPaymentHeader.aptVoidApPaymentHeaderID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentHeader.aptCreditApInvoiceID) && !(await base.ERPAPPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { aPPaymentHeader.aptCreditApInvoiceID })))
			{
				errorsList.Add("aptCreditApInvoiceID [" + aPPaymentHeader.aptCreditApInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentHeader.aptCreatedCreditApInvoiceID) && !(await base.ERPAPPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { aPPaymentHeader.aptCreatedCreditApInvoiceID })))
			{
				errorsList.Add("aptCreatedCreditApInvoiceID [" + aPPaymentHeader.aptCreatedCreditApInvoiceID + "] not found.");
			}
			if (aPPaymentHeader.aptRecurringPaymentID > 0 && !(await base.ERPAPPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("APRecurringPayments", new object[1] { "APRRECURRINGPAYMENTID" }, new object[1] { aPPaymentHeader.aptRecurringPaymentID })))
			{
				errorsList.Add($"aptRecurringPaymentID [{aPPaymentHeader.aptRecurringPaymentID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentHeader.aptExchangeGlAccountID) && !(await base.ERPAPPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aPPaymentHeader.aptExchangeGlAccountID })))
			{
				errorsList.Add("aptExchangeGlAccountID [" + aPPaymentHeader.aptExchangeGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAPPaymentHeaderDto>>> Process_GetAllAPPaymentHeaders(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAPPaymentHeaderDto> allAPPaymentHeadersDto = new List<ERPAPPaymentHeaderDto>();
		ERPResponseMessageDto<IList<ERPAPPaymentHeaderDto>> result;
		try
		{
			IERPAPPaymentHeaderRepository iERPAPPaymentHeaderRepository = (base.ERPAPPaymentHeaderRepository = new ERPAPPaymentHeaderRepository(base.ApiClientContext));
			using (iERPAPPaymentHeaderRepository)
			{
				foreach (ERPAPPaymentHeaderInformationDto item2 in await base.ERPAPPaymentHeaderRepository.GetAllAPPaymentHeaders(pageSize, pageNumber, filter, orderBy))
				{
					ERPAPPaymentHeaderDto item = new ERPAPPaymentHeaderDto
					{
						aptApInvoiceContactID = item2.aptApInvoiceContactID,
						aptApInvoiceLocationID = item2.aptApInvoiceLocationID,
						aptApPaymentSessionID = item2.aptApPaymentSessionID,
						aptBankAccountName = item2.aptBankAccountName,
						aptBankAccountNumber = item2.aptBankAccountNumber,
						aptBankAccountType = item2.aptBankAccountType,
						aptBankInitials = item2.aptBankInitials,
						aptBic = item2.aptBic,
						aptBsbNumber = item2.aptBsbNumber,
						aptCashGlAccountID = item2.aptCashGlAccountID,
						aptCreatedBy = item2.aptCreatedBy,
						aptCreatedCreditApInvoiceID = item2.aptCreatedCreditApInvoiceID,
						aptCreatedDate = item2.aptCreatedDate,
						aptCreditApInvoiceID = item2.aptCreditApInvoiceID,
						aptEftCode = item2.aptEftCode,
						aptEftDescription = item2.aptEftDescription,
						aptEftNumber = item2.aptEftNumber,
						aptEftParticulars = item2.aptEftParticulars,
						aptUniqueID = item2.aptUniqueID,
						aptExchangeAmount = item2.aptExchangeAmount,
						aptExchangeGlAccountID = item2.aptExchangeGlAccountID,
						aptForm1099Box = item2.aptForm1099Box,
						aptGlFiscalYearID = item2.aptGlFiscalYearID,
						aptGlFiscalYearPeriodID = item2.aptGlFiscalYearPeriodID,
						aptIban = item2.aptIban,
						aptCompleted = item2.aptCompleted,
						aptManualPayment = item2.aptManualPayment,
						aptOpenPaymentLoad = item2.aptOpenPaymentLoad,
						aptOverpayment = item2.aptOverpayment,
						aptPostedToGl = item2.aptPostedToGl,
						aptSuppressVoid = item2.aptSuppressVoid,
						aptTaxReportable = item2.aptTaxReportable,
						aptVoidedPayment = item2.aptVoidedPayment,
						aptLongDescriptionRtf = item2.aptLongDescriptionRtf,
						aptLongDescriptionText = item2.aptLongDescriptionText,
						aptPaymentAmount = item2.aptPaymentAmount,
						aptPaymentAmountForeign = item2.aptPaymentAmountForeign,
						aptPaymentDate = item2.aptPaymentDate,
						aptPaymentMemo = item2.aptPaymentMemo,
						aptPaymentNumber = item2.aptPaymentNumber,
						aptPaymentType = item2.aptPaymentType,
						aptRecurringPaymentID = item2.aptRecurringPaymentID,
						aptRowVersion = item2.aptRowVersion,
						aptApPaymentHeaderID = item2.aptApPaymentHeaderID,
						aptShowAllInvoices = item2.aptShowAllInvoices,
						aptSupplierOrganizationID = item2.aptSupplierOrganizationID,
						aptVoidApPaymentHeaderID = item2.aptVoidApPaymentHeaderID,
						aptVoidApPaymentSessionID = item2.aptVoidApPaymentSessionID,
						CustomFields = item2.CustomFields
					};
					allAPPaymentHeadersDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all APPaymentHeaders]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAPPaymentHeaderDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAPPaymentHeadersDto,
				RecordCount = allAPPaymentHeadersDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPPaymentHeaderDto>> Process_GetAPPaymentHeader(Guid aPPaymentHeaderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAPPaymentHeaderDto aPPaymentHeaderDto = null;
		ERPResponseMessageDto<ERPAPPaymentHeaderDto> result;
		try
		{
			IERPAPPaymentHeaderRepository iERPAPPaymentHeaderRepository = (base.ERPAPPaymentHeaderRepository = new ERPAPPaymentHeaderRepository(base.ApiClientContext));
			using (iERPAPPaymentHeaderRepository)
			{
				ERPAPPaymentHeaderInformationDto eRPAPPaymentHeaderInformationDto = await base.ERPAPPaymentHeaderRepository.GetAPPaymentHeader(aPPaymentHeaderId);
				aPPaymentHeaderDto = new ERPAPPaymentHeaderDto
				{
					aptApInvoiceContactID = eRPAPPaymentHeaderInformationDto.aptApInvoiceContactID,
					aptApInvoiceLocationID = eRPAPPaymentHeaderInformationDto.aptApInvoiceLocationID,
					aptApPaymentSessionID = eRPAPPaymentHeaderInformationDto.aptApPaymentSessionID,
					aptBankAccountName = eRPAPPaymentHeaderInformationDto.aptBankAccountName,
					aptBankAccountNumber = eRPAPPaymentHeaderInformationDto.aptBankAccountNumber,
					aptBankAccountType = eRPAPPaymentHeaderInformationDto.aptBankAccountType,
					aptBankInitials = eRPAPPaymentHeaderInformationDto.aptBankInitials,
					aptBic = eRPAPPaymentHeaderInformationDto.aptBic,
					aptBsbNumber = eRPAPPaymentHeaderInformationDto.aptBsbNumber,
					aptCashGlAccountID = eRPAPPaymentHeaderInformationDto.aptCashGlAccountID,
					aptCreatedBy = eRPAPPaymentHeaderInformationDto.aptCreatedBy,
					aptCreatedCreditApInvoiceID = eRPAPPaymentHeaderInformationDto.aptCreatedCreditApInvoiceID,
					aptCreatedDate = eRPAPPaymentHeaderInformationDto.aptCreatedDate,
					aptCreditApInvoiceID = eRPAPPaymentHeaderInformationDto.aptCreditApInvoiceID,
					aptEftCode = eRPAPPaymentHeaderInformationDto.aptEftCode,
					aptEftDescription = eRPAPPaymentHeaderInformationDto.aptEftDescription,
					aptEftNumber = eRPAPPaymentHeaderInformationDto.aptEftNumber,
					aptEftParticulars = eRPAPPaymentHeaderInformationDto.aptEftParticulars,
					aptUniqueID = eRPAPPaymentHeaderInformationDto.aptUniqueID,
					aptExchangeAmount = eRPAPPaymentHeaderInformationDto.aptExchangeAmount,
					aptExchangeGlAccountID = eRPAPPaymentHeaderInformationDto.aptExchangeGlAccountID,
					aptForm1099Box = eRPAPPaymentHeaderInformationDto.aptForm1099Box,
					aptGlFiscalYearID = eRPAPPaymentHeaderInformationDto.aptGlFiscalYearID,
					aptGlFiscalYearPeriodID = eRPAPPaymentHeaderInformationDto.aptGlFiscalYearPeriodID,
					aptIban = eRPAPPaymentHeaderInformationDto.aptIban,
					aptCompleted = eRPAPPaymentHeaderInformationDto.aptCompleted,
					aptManualPayment = eRPAPPaymentHeaderInformationDto.aptManualPayment,
					aptOpenPaymentLoad = eRPAPPaymentHeaderInformationDto.aptOpenPaymentLoad,
					aptOverpayment = eRPAPPaymentHeaderInformationDto.aptOverpayment,
					aptPostedToGl = eRPAPPaymentHeaderInformationDto.aptPostedToGl,
					aptSuppressVoid = eRPAPPaymentHeaderInformationDto.aptSuppressVoid,
					aptTaxReportable = eRPAPPaymentHeaderInformationDto.aptTaxReportable,
					aptVoidedPayment = eRPAPPaymentHeaderInformationDto.aptVoidedPayment,
					aptLongDescriptionRtf = eRPAPPaymentHeaderInformationDto.aptLongDescriptionRtf,
					aptLongDescriptionText = eRPAPPaymentHeaderInformationDto.aptLongDescriptionText,
					aptPaymentAmount = eRPAPPaymentHeaderInformationDto.aptPaymentAmount,
					aptPaymentAmountForeign = eRPAPPaymentHeaderInformationDto.aptPaymentAmountForeign,
					aptPaymentDate = eRPAPPaymentHeaderInformationDto.aptPaymentDate,
					aptPaymentMemo = eRPAPPaymentHeaderInformationDto.aptPaymentMemo,
					aptPaymentNumber = eRPAPPaymentHeaderInformationDto.aptPaymentNumber,
					aptPaymentType = eRPAPPaymentHeaderInformationDto.aptPaymentType,
					aptRecurringPaymentID = eRPAPPaymentHeaderInformationDto.aptRecurringPaymentID,
					aptRowVersion = eRPAPPaymentHeaderInformationDto.aptRowVersion,
					aptApPaymentHeaderID = eRPAPPaymentHeaderInformationDto.aptApPaymentHeaderID,
					aptShowAllInvoices = eRPAPPaymentHeaderInformationDto.aptShowAllInvoices,
					aptSupplierOrganizationID = eRPAPPaymentHeaderInformationDto.aptSupplierOrganizationID,
					aptVoidApPaymentHeaderID = eRPAPPaymentHeaderInformationDto.aptVoidApPaymentHeaderID,
					aptVoidApPaymentSessionID = eRPAPPaymentHeaderInformationDto.aptVoidApPaymentSessionID,
					CustomFields = eRPAPPaymentHeaderInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the APPaymentHeaders []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPPaymentHeaderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aPPaymentHeaderDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPPaymentHeaderDto>> Process_PutAPPaymentHeader(ERPAPPaymentHeaderDto aPPaymentHeader)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAPPaymentHeaderDto createdObject = null;
		ERPResponseMessageDto<ERPAPPaymentHeaderDto> result;
		try
		{
			IERPAPPaymentHeaderRepository iERPAPPaymentHeaderRepository = (base.ERPAPPaymentHeaderRepository = new ERPAPPaymentHeaderRepository(base.ApiClientContext));
			using (iERPAPPaymentHeaderRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAPPaymentHeaderRepository.SaveAPPaymentHeader(aPPaymentHeader);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAPPaymentHeaderInformationDto eRPAPPaymentHeaderInformationDto = await base.ERPAPPaymentHeaderRepository.GetAPPaymentHeader(aPPaymentHeader.aptUniqueID);
					createdObject = new ERPAPPaymentHeaderDto
					{
						aptApInvoiceContactID = eRPAPPaymentHeaderInformationDto.aptApInvoiceContactID,
						aptApInvoiceLocationID = eRPAPPaymentHeaderInformationDto.aptApInvoiceLocationID,
						aptApPaymentSessionID = eRPAPPaymentHeaderInformationDto.aptApPaymentSessionID,
						aptBankAccountName = eRPAPPaymentHeaderInformationDto.aptBankAccountName,
						aptBankAccountNumber = eRPAPPaymentHeaderInformationDto.aptBankAccountNumber,
						aptBankAccountType = eRPAPPaymentHeaderInformationDto.aptBankAccountType,
						aptBankInitials = eRPAPPaymentHeaderInformationDto.aptBankInitials,
						aptBic = eRPAPPaymentHeaderInformationDto.aptBic,
						aptBsbNumber = eRPAPPaymentHeaderInformationDto.aptBsbNumber,
						aptCashGlAccountID = eRPAPPaymentHeaderInformationDto.aptCashGlAccountID,
						aptCreatedBy = eRPAPPaymentHeaderInformationDto.aptCreatedBy,
						aptCreatedCreditApInvoiceID = eRPAPPaymentHeaderInformationDto.aptCreatedCreditApInvoiceID,
						aptCreatedDate = eRPAPPaymentHeaderInformationDto.aptCreatedDate,
						aptCreditApInvoiceID = eRPAPPaymentHeaderInformationDto.aptCreditApInvoiceID,
						aptEftCode = eRPAPPaymentHeaderInformationDto.aptEftCode,
						aptEftDescription = eRPAPPaymentHeaderInformationDto.aptEftDescription,
						aptEftNumber = eRPAPPaymentHeaderInformationDto.aptEftNumber,
						aptEftParticulars = eRPAPPaymentHeaderInformationDto.aptEftParticulars,
						aptUniqueID = eRPAPPaymentHeaderInformationDto.aptUniqueID,
						aptExchangeAmount = eRPAPPaymentHeaderInformationDto.aptExchangeAmount,
						aptExchangeGlAccountID = eRPAPPaymentHeaderInformationDto.aptExchangeGlAccountID,
						aptForm1099Box = eRPAPPaymentHeaderInformationDto.aptForm1099Box,
						aptGlFiscalYearID = eRPAPPaymentHeaderInformationDto.aptGlFiscalYearID,
						aptGlFiscalYearPeriodID = eRPAPPaymentHeaderInformationDto.aptGlFiscalYearPeriodID,
						aptIban = eRPAPPaymentHeaderInformationDto.aptIban,
						aptCompleted = eRPAPPaymentHeaderInformationDto.aptCompleted,
						aptManualPayment = eRPAPPaymentHeaderInformationDto.aptManualPayment,
						aptOpenPaymentLoad = eRPAPPaymentHeaderInformationDto.aptOpenPaymentLoad,
						aptOverpayment = eRPAPPaymentHeaderInformationDto.aptOverpayment,
						aptPostedToGl = eRPAPPaymentHeaderInformationDto.aptPostedToGl,
						aptSuppressVoid = eRPAPPaymentHeaderInformationDto.aptSuppressVoid,
						aptTaxReportable = eRPAPPaymentHeaderInformationDto.aptTaxReportable,
						aptVoidedPayment = eRPAPPaymentHeaderInformationDto.aptVoidedPayment,
						aptLongDescriptionRtf = eRPAPPaymentHeaderInformationDto.aptLongDescriptionRtf,
						aptLongDescriptionText = eRPAPPaymentHeaderInformationDto.aptLongDescriptionText,
						aptPaymentAmount = eRPAPPaymentHeaderInformationDto.aptPaymentAmount,
						aptPaymentAmountForeign = eRPAPPaymentHeaderInformationDto.aptPaymentAmountForeign,
						aptPaymentDate = eRPAPPaymentHeaderInformationDto.aptPaymentDate,
						aptPaymentMemo = eRPAPPaymentHeaderInformationDto.aptPaymentMemo,
						aptPaymentNumber = eRPAPPaymentHeaderInformationDto.aptPaymentNumber,
						aptPaymentType = eRPAPPaymentHeaderInformationDto.aptPaymentType,
						aptRecurringPaymentID = eRPAPPaymentHeaderInformationDto.aptRecurringPaymentID,
						aptRowVersion = eRPAPPaymentHeaderInformationDto.aptRowVersion,
						aptApPaymentHeaderID = eRPAPPaymentHeaderInformationDto.aptApPaymentHeaderID,
						aptShowAllInvoices = eRPAPPaymentHeaderInformationDto.aptShowAllInvoices,
						aptSupplierOrganizationID = eRPAPPaymentHeaderInformationDto.aptSupplierOrganizationID,
						aptVoidApPaymentHeaderID = eRPAPPaymentHeaderInformationDto.aptVoidApPaymentHeaderID,
						aptVoidApPaymentSessionID = eRPAPPaymentHeaderInformationDto.aptVoidApPaymentSessionID,
						CustomFields = eRPAPPaymentHeaderInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing APPaymentHeader [{aPPaymentHeader.aptUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPPaymentHeaderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAPPaymentHeader(Guid aPPaymentHeaderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPPaymentHeaderRepository iERPAPPaymentHeaderRepository = (base.ERPAPPaymentHeaderRepository = new ERPAPPaymentHeaderRepository(base.ApiClientContext));
		using (iERPAPPaymentHeaderRepository)
		{
			if (!(await base.ERPAPPaymentHeaderRepository.DoesAPPaymentHeaderExist(aPPaymentHeaderId)))
			{
				base.ErrorsList.Add($"APPaymentHeader [{aPPaymentHeaderId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAPPaymentHeaderInformationDto eRPAPPaymentHeaderInformationDto = await base.ERPAPPaymentHeaderRepository.GetAPPaymentHeader(aPPaymentHeaderId);
				string text = await base.ERPAPPaymentHeaderRepository.WhereUsed("APPaymentHeaders", new object[2] { eRPAPPaymentHeaderInformationDto.aptApPaymentSessionID, eRPAPPaymentHeaderInformationDto.aptApPaymentHeaderID }, new object[2] { "aptApPaymentSessionID", "aptApPaymentHeaderID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("APPaymentHeader cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAPPaymentHeaderDto>> Process_DeleteAPPaymentHeader(Guid aPPaymentHeaderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAPPaymentHeaderDto> result;
		try
		{
			IERPAPPaymentHeaderRepository iERPAPPaymentHeaderRepository = (base.ERPAPPaymentHeaderRepository = new ERPAPPaymentHeaderRepository(base.ApiClientContext));
			using (iERPAPPaymentHeaderRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAPPaymentHeaderRepository.DeleteRowFromTable("APPaymentHeaders", "apt", aPPaymentHeaderId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of APPaymentHeader [{aPPaymentHeaderId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPPaymentHeaderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAPPaymentHeaderDto()
			};
		}
		return result;
	}
}
