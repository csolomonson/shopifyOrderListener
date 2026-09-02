using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAPPaymentLineModel : ERPBaseModel, IERPAPPaymentLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAPPaymentLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAPPaymentLineRepository iERPAPPaymentLineRepository = (base.ERPAPPaymentLineRepository = new ERPAPPaymentLineRepository(base.ApiClientContext));
		using (iERPAPPaymentLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAPPaymentLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAPPaymentLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAPPaymentLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAPPaymentLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAPPaymentLine(Guid aPPaymentLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPPaymentLineRepository iERPAPPaymentLineRepository = (base.ERPAPPaymentLineRepository = new ERPAPPaymentLineRepository(base.ApiClientContext));
		using (iERPAPPaymentLineRepository)
		{
			if (!(await base.ERPAPPaymentLineRepository.DoesAPPaymentLineExist(aPPaymentLineId)))
			{
				errorsList.Add($"APPaymentLine [{aPPaymentLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAPPaymentLine(ERPAPPaymentLineDto aPPaymentLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPPaymentLineRepository iERPAPPaymentLineRepository = (base.ERPAPPaymentLineRepository = new ERPAPPaymentLineRepository(base.ApiClientContext));
		using (iERPAPPaymentLineRepository)
		{
			if (aPPaymentLine.apnApPaymentSessionID > 0 && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("APPaymentSessions", new object[1] { "APSAPPAYMENTSESSIONID" }, new object[1] { aPPaymentLine.apnApPaymentSessionID })))
			{
				errorsList.Add($"apnApPaymentSessionID [{aPPaymentLine.apnApPaymentSessionID}] not found.");
			}
			if (aPPaymentLine.apnApPaymentHeaderID > 0 && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("APPaymentHeaders", new object[2] { "APTAPPAYMENTSESSIONID", "APTAPPAYMENTHEADERID" }, new object[2] { aPPaymentLine.apnApPaymentSessionID, aPPaymentLine.apnApPaymentHeaderID })))
			{
				errorsList.Add($"apnApPaymentHeaderID [{aPPaymentLine.apnApPaymentHeaderID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnApInvoiceID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { aPPaymentLine.apnApInvoiceID })))
			{
				errorsList.Add("apnApInvoiceID [" + aPPaymentLine.apnApInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnExpenseGlAccountID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aPPaymentLine.apnExpenseGlAccountID })))
			{
				errorsList.Add("apnExpenseGlAccountID [" + aPPaymentLine.apnExpenseGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnDiscountGlAccountID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aPPaymentLine.apnDiscountGlAccountID })))
			{
				errorsList.Add("apnDiscountGlAccountID [" + aPPaymentLine.apnDiscountGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnDiscountTaxCodeID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aPPaymentLine.apnDiscountTaxCodeID })))
			{
				errorsList.Add("apnDiscountTaxCodeID [" + aPPaymentLine.apnDiscountTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnSecondDiscountTaxCodeID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aPPaymentLine.apnSecondDiscountTaxCodeID })))
			{
				errorsList.Add("apnSecondDiscountTaxCodeID [" + aPPaymentLine.apnSecondDiscountTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnTaxCodeID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aPPaymentLine.apnTaxCodeID })))
			{
				errorsList.Add("apnTaxCodeID [" + aPPaymentLine.apnTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnNonTaxReasonID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { aPPaymentLine.apnNonTaxReasonID })))
			{
				errorsList.Add("apnNonTaxReasonID [" + aPPaymentLine.apnNonTaxReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnSecondTaxCodeID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aPPaymentLine.apnSecondTaxCodeID })))
			{
				errorsList.Add("apnSecondTaxCodeID [" + aPPaymentLine.apnSecondTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnAdjustmentGlAccountID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aPPaymentLine.apnAdjustmentGlAccountID })))
			{
				errorsList.Add("apnAdjustmentGlAccountID [" + aPPaymentLine.apnAdjustmentGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnBankAccountID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("BankAccounts", new object[1] { "GLNBANKACCOUNTID" }, new object[1] { aPPaymentLine.apnBankAccountID })))
			{
				errorsList.Add("apnBankAccountID [" + aPPaymentLine.apnBankAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnArInvoiceID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { aPPaymentLine.apnArInvoiceID })))
			{
				errorsList.Add("apnArInvoiceID [" + aPPaymentLine.apnArInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnExchangeGlAccountID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aPPaymentLine.apnExchangeGlAccountID })))
			{
				errorsList.Add("apnExchangeGlAccountID [" + aPPaymentLine.apnExchangeGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnCurrencyRateID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { aPPaymentLine.apnCurrencyRateID })))
			{
				errorsList.Add("apnCurrencyRateID [" + aPPaymentLine.apnCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aPPaymentLine.apnUnrealisedExGlAccountID) && !(await base.ERPAPPaymentLineRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aPPaymentLine.apnUnrealisedExGlAccountID })))
			{
				errorsList.Add("apnUnrealisedExGlAccountID [" + aPPaymentLine.apnUnrealisedExGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAPPaymentLineDto>>> Process_GetAllAPPaymentLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAPPaymentLineDto> allAPPaymentLinesDto = new List<ERPAPPaymentLineDto>();
		ERPResponseMessageDto<IList<ERPAPPaymentLineDto>> result;
		try
		{
			IERPAPPaymentLineRepository iERPAPPaymentLineRepository = (base.ERPAPPaymentLineRepository = new ERPAPPaymentLineRepository(base.ApiClientContext));
			using (iERPAPPaymentLineRepository)
			{
				foreach (ERPAPPaymentLineInformationDto item2 in await base.ERPAPPaymentLineRepository.GetAllAPPaymentLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPAPPaymentLineDto item = new ERPAPPaymentLineDto
					{
						apnAdjustmentAmount = item2.apnAdjustmentAmount,
						apnAdjustmentAmountForeign = item2.apnAdjustmentAmountForeign,
						apnAdjustmentGlAccountID = item2.apnAdjustmentGlAccountID,
						apnApInvoiceID = item2.apnApInvoiceID,
						apnApPaymentHeaderID = item2.apnApPaymentHeaderID,
						apnApPaymentSessionID = item2.apnApPaymentSessionID,
						apnArInvoiceID = item2.apnArInvoiceID,
						apnBankAccountID = item2.apnBankAccountID,
						apnCreatedBy = item2.apnCreatedBy,
						apnCreatedDate = item2.apnCreatedDate,
						apnCurrencyRateID = item2.apnCurrencyRateID,
						apnDescription = item2.apnDescription,
						apnDiscountAmount = item2.apnDiscountAmount,
						apnDiscountAmountForeign = item2.apnDiscountAmountForeign,
						apnDiscountGlAccountID = item2.apnDiscountGlAccountID,
						apnDiscountTaxAmount = item2.apnDiscountTaxAmount,
						apnDiscountTaxAmountForeign = item2.apnDiscountTaxAmountForeign,
						apnDiscountTaxCodeID = item2.apnDiscountTaxCodeID,
						apnUniqueID = item2.apnUniqueID,
						apnExchangeAmount = item2.apnExchangeAmount,
						apnExchangeGlAccountID = item2.apnExchangeGlAccountID,
						apnExchangeRate = item2.apnExchangeRate,
						apnExpenseGlAccountID = item2.apnExpenseGlAccountID,
						apnCompleted = item2.apnCompleted,
						apnCustomRate = item2.apnCustomRate,
						apnOverpayment = item2.apnOverpayment,
						apnPostedToGl = item2.apnPostedToGl,
						apnNonTaxReasonID = item2.apnNonTaxReasonID,
						apnOriginalInvBalanceForeign = item2.apnOriginalInvBalanceForeign,
						apnOriginalInvoiceBalance = item2.apnOriginalInvoiceBalance,
						apnPaymentAmount = item2.apnPaymentAmount,
						apnPaymentAmountForeign = item2.apnPaymentAmountForeign,
						apnRetentionPayAmtForeign = item2.apnRetentionPayAmtForeign,
						apnRetentionPaymentAmount = item2.apnRetentionPaymentAmount,
						apnRowVersion = item2.apnRowVersion,
						apnSecondDiscountTaxAmount = item2.apnSecondDiscountTaxAmount,
						apnSecondDiscountTaxCodeID = item2.apnSecondDiscountTaxCodeID,
						apnSecondDisTaxAmtForeign = item2.apnSecondDisTaxAmtForeign,
						apnSecondTaxAmount = item2.apnSecondTaxAmount,
						apnSecondTaxAmountForeign = item2.apnSecondTaxAmountForeign,
						apnSecondTaxCodeID = item2.apnSecondTaxCodeID,
						apnApPaymentLineID = item2.apnApPaymentLineID,
						apnTaxAmount = item2.apnTaxAmount,
						apnTaxAmountForeign = item2.apnTaxAmountForeign,
						apnTaxCodeID = item2.apnTaxCodeID,
						apnTotalDiscountAmount = item2.apnTotalDiscountAmount,
						apnTotalDiscountAmtForeign = item2.apnTotalDiscountAmtForeign,
						apnUnrealisedExchangeAmt = item2.apnUnrealisedExchangeAmt,
						apnUnrealisedExGlAccountID = item2.apnUnrealisedExGlAccountID,
						CustomFields = item2.CustomFields
					};
					allAPPaymentLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all APPaymentLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAPPaymentLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAPPaymentLinesDto,
				RecordCount = allAPPaymentLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPPaymentLineDto>> Process_GetAPPaymentLine(Guid aPPaymentLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAPPaymentLineDto aPPaymentLineDto = null;
		ERPResponseMessageDto<ERPAPPaymentLineDto> result;
		try
		{
			IERPAPPaymentLineRepository iERPAPPaymentLineRepository = (base.ERPAPPaymentLineRepository = new ERPAPPaymentLineRepository(base.ApiClientContext));
			using (iERPAPPaymentLineRepository)
			{
				ERPAPPaymentLineInformationDto eRPAPPaymentLineInformationDto = await base.ERPAPPaymentLineRepository.GetAPPaymentLine(aPPaymentLineId);
				aPPaymentLineDto = new ERPAPPaymentLineDto
				{
					apnAdjustmentAmount = eRPAPPaymentLineInformationDto.apnAdjustmentAmount,
					apnAdjustmentAmountForeign = eRPAPPaymentLineInformationDto.apnAdjustmentAmountForeign,
					apnAdjustmentGlAccountID = eRPAPPaymentLineInformationDto.apnAdjustmentGlAccountID,
					apnApInvoiceID = eRPAPPaymentLineInformationDto.apnApInvoiceID,
					apnApPaymentHeaderID = eRPAPPaymentLineInformationDto.apnApPaymentHeaderID,
					apnApPaymentSessionID = eRPAPPaymentLineInformationDto.apnApPaymentSessionID,
					apnArInvoiceID = eRPAPPaymentLineInformationDto.apnArInvoiceID,
					apnBankAccountID = eRPAPPaymentLineInformationDto.apnBankAccountID,
					apnCreatedBy = eRPAPPaymentLineInformationDto.apnCreatedBy,
					apnCreatedDate = eRPAPPaymentLineInformationDto.apnCreatedDate,
					apnCurrencyRateID = eRPAPPaymentLineInformationDto.apnCurrencyRateID,
					apnDescription = eRPAPPaymentLineInformationDto.apnDescription,
					apnDiscountAmount = eRPAPPaymentLineInformationDto.apnDiscountAmount,
					apnDiscountAmountForeign = eRPAPPaymentLineInformationDto.apnDiscountAmountForeign,
					apnDiscountGlAccountID = eRPAPPaymentLineInformationDto.apnDiscountGlAccountID,
					apnDiscountTaxAmount = eRPAPPaymentLineInformationDto.apnDiscountTaxAmount,
					apnDiscountTaxAmountForeign = eRPAPPaymentLineInformationDto.apnDiscountTaxAmountForeign,
					apnDiscountTaxCodeID = eRPAPPaymentLineInformationDto.apnDiscountTaxCodeID,
					apnUniqueID = eRPAPPaymentLineInformationDto.apnUniqueID,
					apnExchangeAmount = eRPAPPaymentLineInformationDto.apnExchangeAmount,
					apnExchangeGlAccountID = eRPAPPaymentLineInformationDto.apnExchangeGlAccountID,
					apnExchangeRate = eRPAPPaymentLineInformationDto.apnExchangeRate,
					apnExpenseGlAccountID = eRPAPPaymentLineInformationDto.apnExpenseGlAccountID,
					apnCompleted = eRPAPPaymentLineInformationDto.apnCompleted,
					apnCustomRate = eRPAPPaymentLineInformationDto.apnCustomRate,
					apnOverpayment = eRPAPPaymentLineInformationDto.apnOverpayment,
					apnPostedToGl = eRPAPPaymentLineInformationDto.apnPostedToGl,
					apnNonTaxReasonID = eRPAPPaymentLineInformationDto.apnNonTaxReasonID,
					apnOriginalInvBalanceForeign = eRPAPPaymentLineInformationDto.apnOriginalInvBalanceForeign,
					apnOriginalInvoiceBalance = eRPAPPaymentLineInformationDto.apnOriginalInvoiceBalance,
					apnPaymentAmount = eRPAPPaymentLineInformationDto.apnPaymentAmount,
					apnPaymentAmountForeign = eRPAPPaymentLineInformationDto.apnPaymentAmountForeign,
					apnRetentionPayAmtForeign = eRPAPPaymentLineInformationDto.apnRetentionPayAmtForeign,
					apnRetentionPaymentAmount = eRPAPPaymentLineInformationDto.apnRetentionPaymentAmount,
					apnRowVersion = eRPAPPaymentLineInformationDto.apnRowVersion,
					apnSecondDiscountTaxAmount = eRPAPPaymentLineInformationDto.apnSecondDiscountTaxAmount,
					apnSecondDiscountTaxCodeID = eRPAPPaymentLineInformationDto.apnSecondDiscountTaxCodeID,
					apnSecondDisTaxAmtForeign = eRPAPPaymentLineInformationDto.apnSecondDisTaxAmtForeign,
					apnSecondTaxAmount = eRPAPPaymentLineInformationDto.apnSecondTaxAmount,
					apnSecondTaxAmountForeign = eRPAPPaymentLineInformationDto.apnSecondTaxAmountForeign,
					apnSecondTaxCodeID = eRPAPPaymentLineInformationDto.apnSecondTaxCodeID,
					apnApPaymentLineID = eRPAPPaymentLineInformationDto.apnApPaymentLineID,
					apnTaxAmount = eRPAPPaymentLineInformationDto.apnTaxAmount,
					apnTaxAmountForeign = eRPAPPaymentLineInformationDto.apnTaxAmountForeign,
					apnTaxCodeID = eRPAPPaymentLineInformationDto.apnTaxCodeID,
					apnTotalDiscountAmount = eRPAPPaymentLineInformationDto.apnTotalDiscountAmount,
					apnTotalDiscountAmtForeign = eRPAPPaymentLineInformationDto.apnTotalDiscountAmtForeign,
					apnUnrealisedExchangeAmt = eRPAPPaymentLineInformationDto.apnUnrealisedExchangeAmt,
					apnUnrealisedExGlAccountID = eRPAPPaymentLineInformationDto.apnUnrealisedExGlAccountID,
					CustomFields = eRPAPPaymentLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the APPaymentLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPPaymentLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aPPaymentLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPPaymentLineDto>> Process_PutAPPaymentLine(ERPAPPaymentLineDto aPPaymentLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAPPaymentLineDto createdObject = null;
		ERPResponseMessageDto<ERPAPPaymentLineDto> result;
		try
		{
			IERPAPPaymentLineRepository iERPAPPaymentLineRepository = (base.ERPAPPaymentLineRepository = new ERPAPPaymentLineRepository(base.ApiClientContext));
			using (iERPAPPaymentLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAPPaymentLineRepository.SaveAPPaymentLine(aPPaymentLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAPPaymentLineInformationDto eRPAPPaymentLineInformationDto = await base.ERPAPPaymentLineRepository.GetAPPaymentLine(aPPaymentLine.apnUniqueID);
					createdObject = new ERPAPPaymentLineDto
					{
						apnAdjustmentAmount = eRPAPPaymentLineInformationDto.apnAdjustmentAmount,
						apnAdjustmentAmountForeign = eRPAPPaymentLineInformationDto.apnAdjustmentAmountForeign,
						apnAdjustmentGlAccountID = eRPAPPaymentLineInformationDto.apnAdjustmentGlAccountID,
						apnApInvoiceID = eRPAPPaymentLineInformationDto.apnApInvoiceID,
						apnApPaymentHeaderID = eRPAPPaymentLineInformationDto.apnApPaymentHeaderID,
						apnApPaymentSessionID = eRPAPPaymentLineInformationDto.apnApPaymentSessionID,
						apnArInvoiceID = eRPAPPaymentLineInformationDto.apnArInvoiceID,
						apnBankAccountID = eRPAPPaymentLineInformationDto.apnBankAccountID,
						apnCreatedBy = eRPAPPaymentLineInformationDto.apnCreatedBy,
						apnCreatedDate = eRPAPPaymentLineInformationDto.apnCreatedDate,
						apnCurrencyRateID = eRPAPPaymentLineInformationDto.apnCurrencyRateID,
						apnDescription = eRPAPPaymentLineInformationDto.apnDescription,
						apnDiscountAmount = eRPAPPaymentLineInformationDto.apnDiscountAmount,
						apnDiscountAmountForeign = eRPAPPaymentLineInformationDto.apnDiscountAmountForeign,
						apnDiscountGlAccountID = eRPAPPaymentLineInformationDto.apnDiscountGlAccountID,
						apnDiscountTaxAmount = eRPAPPaymentLineInformationDto.apnDiscountTaxAmount,
						apnDiscountTaxAmountForeign = eRPAPPaymentLineInformationDto.apnDiscountTaxAmountForeign,
						apnDiscountTaxCodeID = eRPAPPaymentLineInformationDto.apnDiscountTaxCodeID,
						apnUniqueID = eRPAPPaymentLineInformationDto.apnUniqueID,
						apnExchangeAmount = eRPAPPaymentLineInformationDto.apnExchangeAmount,
						apnExchangeGlAccountID = eRPAPPaymentLineInformationDto.apnExchangeGlAccountID,
						apnExchangeRate = eRPAPPaymentLineInformationDto.apnExchangeRate,
						apnExpenseGlAccountID = eRPAPPaymentLineInformationDto.apnExpenseGlAccountID,
						apnCompleted = eRPAPPaymentLineInformationDto.apnCompleted,
						apnCustomRate = eRPAPPaymentLineInformationDto.apnCustomRate,
						apnOverpayment = eRPAPPaymentLineInformationDto.apnOverpayment,
						apnPostedToGl = eRPAPPaymentLineInformationDto.apnPostedToGl,
						apnNonTaxReasonID = eRPAPPaymentLineInformationDto.apnNonTaxReasonID,
						apnOriginalInvBalanceForeign = eRPAPPaymentLineInformationDto.apnOriginalInvBalanceForeign,
						apnOriginalInvoiceBalance = eRPAPPaymentLineInformationDto.apnOriginalInvoiceBalance,
						apnPaymentAmount = eRPAPPaymentLineInformationDto.apnPaymentAmount,
						apnPaymentAmountForeign = eRPAPPaymentLineInformationDto.apnPaymentAmountForeign,
						apnRetentionPayAmtForeign = eRPAPPaymentLineInformationDto.apnRetentionPayAmtForeign,
						apnRetentionPaymentAmount = eRPAPPaymentLineInformationDto.apnRetentionPaymentAmount,
						apnRowVersion = eRPAPPaymentLineInformationDto.apnRowVersion,
						apnSecondDiscountTaxAmount = eRPAPPaymentLineInformationDto.apnSecondDiscountTaxAmount,
						apnSecondDiscountTaxCodeID = eRPAPPaymentLineInformationDto.apnSecondDiscountTaxCodeID,
						apnSecondDisTaxAmtForeign = eRPAPPaymentLineInformationDto.apnSecondDisTaxAmtForeign,
						apnSecondTaxAmount = eRPAPPaymentLineInformationDto.apnSecondTaxAmount,
						apnSecondTaxAmountForeign = eRPAPPaymentLineInformationDto.apnSecondTaxAmountForeign,
						apnSecondTaxCodeID = eRPAPPaymentLineInformationDto.apnSecondTaxCodeID,
						apnApPaymentLineID = eRPAPPaymentLineInformationDto.apnApPaymentLineID,
						apnTaxAmount = eRPAPPaymentLineInformationDto.apnTaxAmount,
						apnTaxAmountForeign = eRPAPPaymentLineInformationDto.apnTaxAmountForeign,
						apnTaxCodeID = eRPAPPaymentLineInformationDto.apnTaxCodeID,
						apnTotalDiscountAmount = eRPAPPaymentLineInformationDto.apnTotalDiscountAmount,
						apnTotalDiscountAmtForeign = eRPAPPaymentLineInformationDto.apnTotalDiscountAmtForeign,
						apnUnrealisedExchangeAmt = eRPAPPaymentLineInformationDto.apnUnrealisedExchangeAmt,
						apnUnrealisedExGlAccountID = eRPAPPaymentLineInformationDto.apnUnrealisedExGlAccountID,
						CustomFields = eRPAPPaymentLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing APPaymentLine [{aPPaymentLine.apnUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPPaymentLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAPPaymentLine(Guid aPPaymentLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPPaymentLineRepository iERPAPPaymentLineRepository = (base.ERPAPPaymentLineRepository = new ERPAPPaymentLineRepository(base.ApiClientContext));
		using (iERPAPPaymentLineRepository)
		{
			if (!(await base.ERPAPPaymentLineRepository.DoesAPPaymentLineExist(aPPaymentLineId)))
			{
				base.ErrorsList.Add($"APPaymentLine [{aPPaymentLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAPPaymentLineInformationDto eRPAPPaymentLineInformationDto = await base.ERPAPPaymentLineRepository.GetAPPaymentLine(aPPaymentLineId);
				string text = await base.ERPAPPaymentLineRepository.WhereUsed("APPaymentLines", new object[3] { eRPAPPaymentLineInformationDto.apnApPaymentSessionID, eRPAPPaymentLineInformationDto.apnApPaymentHeaderID, eRPAPPaymentLineInformationDto.apnApPaymentLineID }, new object[3] { "apnApPaymentSessionID", "apnApPaymentHeaderID", "apnApPaymentLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("APPaymentLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAPPaymentLineDto>> Process_DeleteAPPaymentLine(Guid aPPaymentLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAPPaymentLineDto> result;
		try
		{
			IERPAPPaymentLineRepository iERPAPPaymentLineRepository = (base.ERPAPPaymentLineRepository = new ERPAPPaymentLineRepository(base.ApiClientContext));
			using (iERPAPPaymentLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAPPaymentLineRepository.DeleteRowFromTable("APPaymentLines", "apn", aPPaymentLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of APPaymentLine [{aPPaymentLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPPaymentLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAPPaymentLineDto()
			};
		}
		return result;
	}
}
