using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPARPaymentLineModel : ERPBaseModel, IERPARPaymentLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllARPaymentLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPARPaymentLineRepository iERPARPaymentLineRepository = (base.ERPARPaymentLineRepository = new ERPARPaymentLineRepository(base.ApiClientContext));
		using (iERPARPaymentLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPARPaymentLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPARPaymentLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPARPaymentLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPARPaymentLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetARPaymentLine(Guid aRPaymentLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARPaymentLineRepository iERPARPaymentLineRepository = (base.ERPARPaymentLineRepository = new ERPARPaymentLineRepository(base.ApiClientContext));
		using (iERPARPaymentLineRepository)
		{
			if (!(await base.ERPARPaymentLineRepository.DoesARPaymentLineExist(aRPaymentLineId)))
			{
				errorsList.Add($"ARPaymentLine [{aRPaymentLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutARPaymentLine(ERPARPaymentLineDto aRPaymentLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARPaymentLineRepository iERPARPaymentLineRepository = (base.ERPARPaymentLineRepository = new ERPARPaymentLineRepository(base.ApiClientContext));
		using (iERPARPaymentLineRepository)
		{
			if (aRPaymentLine.arnArPaymentSessionID > 0 && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("ARPaymentSessions", new object[1] { "ARSARPAYMENTSESSIONID" }, new object[1] { aRPaymentLine.arnArPaymentSessionID })))
			{
				errorsList.Add($"arnArPaymentSessionID [{aRPaymentLine.arnArPaymentSessionID}] not found.");
			}
			if (aRPaymentLine.arnArPaymentHeaderID > 0 && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("ARPaymentHeaders", new object[2] { "ARTARPAYMENTSESSIONID", "ARTARPAYMENTHEADERID" }, new object[2] { aRPaymentLine.arnArPaymentSessionID, aRPaymentLine.arnArPaymentHeaderID })))
			{
				errorsList.Add($"arnArPaymentHeaderID [{aRPaymentLine.arnArPaymentHeaderID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentLine.arnArInvoiceID) && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { aRPaymentLine.arnArInvoiceID })))
			{
				errorsList.Add("arnArInvoiceID [" + aRPaymentLine.arnArInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentLine.arnDiscountGlAccountID) && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRPaymentLine.arnDiscountGlAccountID })))
			{
				errorsList.Add("arnDiscountGlAccountID [" + aRPaymentLine.arnDiscountGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentLine.arnDiscountTaxCodeID) && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aRPaymentLine.arnDiscountTaxCodeID })))
			{
				errorsList.Add("arnDiscountTaxCodeID [" + aRPaymentLine.arnDiscountTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentLine.arnSecondDiscountTaxCodeID) && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aRPaymentLine.arnSecondDiscountTaxCodeID })))
			{
				errorsList.Add("arnSecondDiscountTaxCodeID [" + aRPaymentLine.arnSecondDiscountTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentLine.arnAdjustmentGlAccountID) && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRPaymentLine.arnAdjustmentGlAccountID })))
			{
				errorsList.Add("arnAdjustmentGlAccountID [" + aRPaymentLine.arnAdjustmentGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentLine.arnTaxCodeID) && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aRPaymentLine.arnTaxCodeID })))
			{
				errorsList.Add("arnTaxCodeID [" + aRPaymentLine.arnTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentLine.arnNonTaxReasonID) && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { aRPaymentLine.arnNonTaxReasonID })))
			{
				errorsList.Add("arnNonTaxReasonID [" + aRPaymentLine.arnNonTaxReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentLine.arnSecondTaxCodeID) && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aRPaymentLine.arnSecondTaxCodeID })))
			{
				errorsList.Add("arnSecondTaxCodeID [" + aRPaymentLine.arnSecondTaxCodeID + "] not found.");
			}
			if (aRPaymentLine.arnArPaymentEPayID > 0 && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("ARPaymentEPays", new object[1] { "AREARPAYMENTEPAYID" }, new object[1] { aRPaymentLine.arnArPaymentEPayID })))
			{
				errorsList.Add($"arnArPaymentEPayID [{aRPaymentLine.arnArPaymentEPayID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentLine.arnApInvoiceID) && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { aRPaymentLine.arnApInvoiceID })))
			{
				errorsList.Add("arnApInvoiceID [" + aRPaymentLine.arnApInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentLine.arnExchangeGlAccountID) && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRPaymentLine.arnExchangeGlAccountID })))
			{
				errorsList.Add("arnExchangeGlAccountID [" + aRPaymentLine.arnExchangeGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentLine.arnUnrealisedExGlAccountID) && !(await base.ERPARPaymentLineRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRPaymentLine.arnUnrealisedExGlAccountID })))
			{
				errorsList.Add("arnUnrealisedExGlAccountID [" + aRPaymentLine.arnUnrealisedExGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPARPaymentLineDto>>> Process_GetAllARPaymentLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPARPaymentLineDto> allARPaymentLinesDto = new List<ERPARPaymentLineDto>();
		ERPResponseMessageDto<IList<ERPARPaymentLineDto>> result;
		try
		{
			IERPARPaymentLineRepository iERPARPaymentLineRepository = (base.ERPARPaymentLineRepository = new ERPARPaymentLineRepository(base.ApiClientContext));
			using (iERPARPaymentLineRepository)
			{
				foreach (ERPARPaymentLineInformationDto item2 in await base.ERPARPaymentLineRepository.GetAllARPaymentLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPARPaymentLineDto item = new ERPARPaymentLineDto
					{
						arnAdjustmentAmount = item2.arnAdjustmentAmount,
						arnAdjustmentAmountForeign = item2.arnAdjustmentAmountForeign,
						arnAdjustmentGlAccountID = item2.arnAdjustmentGlAccountID,
						arnApInvoiceID = item2.arnApInvoiceID,
						arnArInvoiceID = item2.arnArInvoiceID,
						arnArPaymentEPayID = item2.arnArPaymentEPayID,
						arnArPaymentHeaderID = item2.arnArPaymentHeaderID,
						arnArPaymentSessionID = item2.arnArPaymentSessionID,
						arnCreatedBy = item2.arnCreatedBy,
						arnCreatedDate = item2.arnCreatedDate,
						arnDiscountAmount = item2.arnDiscountAmount,
						arnDiscountAmountForeign = item2.arnDiscountAmountForeign,
						arnDiscountGlAccountID = item2.arnDiscountGlAccountID,
						arnDiscountTaxAmount = item2.arnDiscountTaxAmount,
						arnDiscountTaxAmountForeign = item2.arnDiscountTaxAmountForeign,
						arnDiscountTaxCodeID = item2.arnDiscountTaxCodeID,
						arnUniqueID = item2.arnUniqueID,
						arnExchangeAmount = item2.arnExchangeAmount,
						arnExchangeGlAccountID = item2.arnExchangeGlAccountID,
						arnAvalaraTaxCalculated = item2.arnAvalaraTaxCalculated,
						arnOverpayment = item2.arnOverpayment,
						arnPostedToGl = item2.arnPostedToGl,
						arnNonTaxReasonID = item2.arnNonTaxReasonID,
						arnOriginalInvBalanceForeign = item2.arnOriginalInvBalanceForeign,
						arnOriginalInvoiceBalance = item2.arnOriginalInvoiceBalance,
						arnPaymentAmount = item2.arnPaymentAmount,
						arnPaymentAmountForeign = item2.arnPaymentAmountForeign,
						arnRetentionPayAmtForeign = item2.arnRetentionPayAmtForeign,
						arnRetentionPaymentAmount = item2.arnRetentionPaymentAmount,
						arnRowVersion = item2.arnRowVersion,
						arnSecondDiscountTaxAmount = item2.arnSecondDiscountTaxAmount,
						arnSecondDiscountTaxCodeID = item2.arnSecondDiscountTaxCodeID,
						arnSecondDisTaxAmtForeign = item2.arnSecondDisTaxAmtForeign,
						arnSecondTaxAmount = item2.arnSecondTaxAmount,
						arnSecondTaxAmountForeign = item2.arnSecondTaxAmountForeign,
						arnSecondTaxCodeID = item2.arnSecondTaxCodeID,
						arnArPaymentLineID = item2.arnArPaymentLineID,
						arnTaxAmount = item2.arnTaxAmount,
						arnTaxAmountForeign = item2.arnTaxAmountForeign,
						arnTaxCodeID = item2.arnTaxCodeID,
						arnTotalDiscountAmount = item2.arnTotalDiscountAmount,
						arnTotalDiscountAmtForeign = item2.arnTotalDiscountAmtForeign,
						arnUnrealisedExchangeAmt = item2.arnUnrealisedExchangeAmt,
						arnUnrealisedExGlAccountID = item2.arnUnrealisedExGlAccountID,
						CustomFields = item2.CustomFields
					};
					allARPaymentLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ARPaymentLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPARPaymentLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allARPaymentLinesDto,
				RecordCount = allARPaymentLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARPaymentLineDto>> Process_GetARPaymentLine(Guid aRPaymentLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPARPaymentLineDto aRPaymentLineDto = null;
		ERPResponseMessageDto<ERPARPaymentLineDto> result;
		try
		{
			IERPARPaymentLineRepository iERPARPaymentLineRepository = (base.ERPARPaymentLineRepository = new ERPARPaymentLineRepository(base.ApiClientContext));
			using (iERPARPaymentLineRepository)
			{
				ERPARPaymentLineInformationDto eRPARPaymentLineInformationDto = await base.ERPARPaymentLineRepository.GetARPaymentLine(aRPaymentLineId);
				aRPaymentLineDto = new ERPARPaymentLineDto
				{
					arnAdjustmentAmount = eRPARPaymentLineInformationDto.arnAdjustmentAmount,
					arnAdjustmentAmountForeign = eRPARPaymentLineInformationDto.arnAdjustmentAmountForeign,
					arnAdjustmentGlAccountID = eRPARPaymentLineInformationDto.arnAdjustmentGlAccountID,
					arnApInvoiceID = eRPARPaymentLineInformationDto.arnApInvoiceID,
					arnArInvoiceID = eRPARPaymentLineInformationDto.arnArInvoiceID,
					arnArPaymentEPayID = eRPARPaymentLineInformationDto.arnArPaymentEPayID,
					arnArPaymentHeaderID = eRPARPaymentLineInformationDto.arnArPaymentHeaderID,
					arnArPaymentSessionID = eRPARPaymentLineInformationDto.arnArPaymentSessionID,
					arnCreatedBy = eRPARPaymentLineInformationDto.arnCreatedBy,
					arnCreatedDate = eRPARPaymentLineInformationDto.arnCreatedDate,
					arnDiscountAmount = eRPARPaymentLineInformationDto.arnDiscountAmount,
					arnDiscountAmountForeign = eRPARPaymentLineInformationDto.arnDiscountAmountForeign,
					arnDiscountGlAccountID = eRPARPaymentLineInformationDto.arnDiscountGlAccountID,
					arnDiscountTaxAmount = eRPARPaymentLineInformationDto.arnDiscountTaxAmount,
					arnDiscountTaxAmountForeign = eRPARPaymentLineInformationDto.arnDiscountTaxAmountForeign,
					arnDiscountTaxCodeID = eRPARPaymentLineInformationDto.arnDiscountTaxCodeID,
					arnUniqueID = eRPARPaymentLineInformationDto.arnUniqueID,
					arnExchangeAmount = eRPARPaymentLineInformationDto.arnExchangeAmount,
					arnExchangeGlAccountID = eRPARPaymentLineInformationDto.arnExchangeGlAccountID,
					arnAvalaraTaxCalculated = eRPARPaymentLineInformationDto.arnAvalaraTaxCalculated,
					arnOverpayment = eRPARPaymentLineInformationDto.arnOverpayment,
					arnPostedToGl = eRPARPaymentLineInformationDto.arnPostedToGl,
					arnNonTaxReasonID = eRPARPaymentLineInformationDto.arnNonTaxReasonID,
					arnOriginalInvBalanceForeign = eRPARPaymentLineInformationDto.arnOriginalInvBalanceForeign,
					arnOriginalInvoiceBalance = eRPARPaymentLineInformationDto.arnOriginalInvoiceBalance,
					arnPaymentAmount = eRPARPaymentLineInformationDto.arnPaymentAmount,
					arnPaymentAmountForeign = eRPARPaymentLineInformationDto.arnPaymentAmountForeign,
					arnRetentionPayAmtForeign = eRPARPaymentLineInformationDto.arnRetentionPayAmtForeign,
					arnRetentionPaymentAmount = eRPARPaymentLineInformationDto.arnRetentionPaymentAmount,
					arnRowVersion = eRPARPaymentLineInformationDto.arnRowVersion,
					arnSecondDiscountTaxAmount = eRPARPaymentLineInformationDto.arnSecondDiscountTaxAmount,
					arnSecondDiscountTaxCodeID = eRPARPaymentLineInformationDto.arnSecondDiscountTaxCodeID,
					arnSecondDisTaxAmtForeign = eRPARPaymentLineInformationDto.arnSecondDisTaxAmtForeign,
					arnSecondTaxAmount = eRPARPaymentLineInformationDto.arnSecondTaxAmount,
					arnSecondTaxAmountForeign = eRPARPaymentLineInformationDto.arnSecondTaxAmountForeign,
					arnSecondTaxCodeID = eRPARPaymentLineInformationDto.arnSecondTaxCodeID,
					arnArPaymentLineID = eRPARPaymentLineInformationDto.arnArPaymentLineID,
					arnTaxAmount = eRPARPaymentLineInformationDto.arnTaxAmount,
					arnTaxAmountForeign = eRPARPaymentLineInformationDto.arnTaxAmountForeign,
					arnTaxCodeID = eRPARPaymentLineInformationDto.arnTaxCodeID,
					arnTotalDiscountAmount = eRPARPaymentLineInformationDto.arnTotalDiscountAmount,
					arnTotalDiscountAmtForeign = eRPARPaymentLineInformationDto.arnTotalDiscountAmtForeign,
					arnUnrealisedExchangeAmt = eRPARPaymentLineInformationDto.arnUnrealisedExchangeAmt,
					arnUnrealisedExGlAccountID = eRPARPaymentLineInformationDto.arnUnrealisedExGlAccountID,
					CustomFields = eRPARPaymentLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ARPaymentLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARPaymentLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aRPaymentLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARPaymentLineDto>> Process_PutARPaymentLine(ERPARPaymentLineDto aRPaymentLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPARPaymentLineDto createdObject = null;
		ERPResponseMessageDto<ERPARPaymentLineDto> result;
		try
		{
			IERPARPaymentLineRepository iERPARPaymentLineRepository = (base.ERPARPaymentLineRepository = new ERPARPaymentLineRepository(base.ApiClientContext));
			using (iERPARPaymentLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPARPaymentLineRepository.SaveARPaymentLine(aRPaymentLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPARPaymentLineInformationDto eRPARPaymentLineInformationDto = await base.ERPARPaymentLineRepository.GetARPaymentLine(aRPaymentLine.arnUniqueID);
					createdObject = new ERPARPaymentLineDto
					{
						arnAdjustmentAmount = eRPARPaymentLineInformationDto.arnAdjustmentAmount,
						arnAdjustmentAmountForeign = eRPARPaymentLineInformationDto.arnAdjustmentAmountForeign,
						arnAdjustmentGlAccountID = eRPARPaymentLineInformationDto.arnAdjustmentGlAccountID,
						arnApInvoiceID = eRPARPaymentLineInformationDto.arnApInvoiceID,
						arnArInvoiceID = eRPARPaymentLineInformationDto.arnArInvoiceID,
						arnArPaymentEPayID = eRPARPaymentLineInformationDto.arnArPaymentEPayID,
						arnArPaymentHeaderID = eRPARPaymentLineInformationDto.arnArPaymentHeaderID,
						arnArPaymentSessionID = eRPARPaymentLineInformationDto.arnArPaymentSessionID,
						arnCreatedBy = eRPARPaymentLineInformationDto.arnCreatedBy,
						arnCreatedDate = eRPARPaymentLineInformationDto.arnCreatedDate,
						arnDiscountAmount = eRPARPaymentLineInformationDto.arnDiscountAmount,
						arnDiscountAmountForeign = eRPARPaymentLineInformationDto.arnDiscountAmountForeign,
						arnDiscountGlAccountID = eRPARPaymentLineInformationDto.arnDiscountGlAccountID,
						arnDiscountTaxAmount = eRPARPaymentLineInformationDto.arnDiscountTaxAmount,
						arnDiscountTaxAmountForeign = eRPARPaymentLineInformationDto.arnDiscountTaxAmountForeign,
						arnDiscountTaxCodeID = eRPARPaymentLineInformationDto.arnDiscountTaxCodeID,
						arnUniqueID = eRPARPaymentLineInformationDto.arnUniqueID,
						arnExchangeAmount = eRPARPaymentLineInformationDto.arnExchangeAmount,
						arnExchangeGlAccountID = eRPARPaymentLineInformationDto.arnExchangeGlAccountID,
						arnAvalaraTaxCalculated = eRPARPaymentLineInformationDto.arnAvalaraTaxCalculated,
						arnOverpayment = eRPARPaymentLineInformationDto.arnOverpayment,
						arnPostedToGl = eRPARPaymentLineInformationDto.arnPostedToGl,
						arnNonTaxReasonID = eRPARPaymentLineInformationDto.arnNonTaxReasonID,
						arnOriginalInvBalanceForeign = eRPARPaymentLineInformationDto.arnOriginalInvBalanceForeign,
						arnOriginalInvoiceBalance = eRPARPaymentLineInformationDto.arnOriginalInvoiceBalance,
						arnPaymentAmount = eRPARPaymentLineInformationDto.arnPaymentAmount,
						arnPaymentAmountForeign = eRPARPaymentLineInformationDto.arnPaymentAmountForeign,
						arnRetentionPayAmtForeign = eRPARPaymentLineInformationDto.arnRetentionPayAmtForeign,
						arnRetentionPaymentAmount = eRPARPaymentLineInformationDto.arnRetentionPaymentAmount,
						arnRowVersion = eRPARPaymentLineInformationDto.arnRowVersion,
						arnSecondDiscountTaxAmount = eRPARPaymentLineInformationDto.arnSecondDiscountTaxAmount,
						arnSecondDiscountTaxCodeID = eRPARPaymentLineInformationDto.arnSecondDiscountTaxCodeID,
						arnSecondDisTaxAmtForeign = eRPARPaymentLineInformationDto.arnSecondDisTaxAmtForeign,
						arnSecondTaxAmount = eRPARPaymentLineInformationDto.arnSecondTaxAmount,
						arnSecondTaxAmountForeign = eRPARPaymentLineInformationDto.arnSecondTaxAmountForeign,
						arnSecondTaxCodeID = eRPARPaymentLineInformationDto.arnSecondTaxCodeID,
						arnArPaymentLineID = eRPARPaymentLineInformationDto.arnArPaymentLineID,
						arnTaxAmount = eRPARPaymentLineInformationDto.arnTaxAmount,
						arnTaxAmountForeign = eRPARPaymentLineInformationDto.arnTaxAmountForeign,
						arnTaxCodeID = eRPARPaymentLineInformationDto.arnTaxCodeID,
						arnTotalDiscountAmount = eRPARPaymentLineInformationDto.arnTotalDiscountAmount,
						arnTotalDiscountAmtForeign = eRPARPaymentLineInformationDto.arnTotalDiscountAmtForeign,
						arnUnrealisedExchangeAmt = eRPARPaymentLineInformationDto.arnUnrealisedExchangeAmt,
						arnUnrealisedExGlAccountID = eRPARPaymentLineInformationDto.arnUnrealisedExGlAccountID,
						CustomFields = eRPARPaymentLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ARPaymentLine [{aRPaymentLine.arnUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARPaymentLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteARPaymentLine(Guid aRPaymentLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARPaymentLineRepository iERPARPaymentLineRepository = (base.ERPARPaymentLineRepository = new ERPARPaymentLineRepository(base.ApiClientContext));
		using (iERPARPaymentLineRepository)
		{
			if (!(await base.ERPARPaymentLineRepository.DoesARPaymentLineExist(aRPaymentLineId)))
			{
				base.ErrorsList.Add($"ARPaymentLine [{aRPaymentLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPARPaymentLineInformationDto eRPARPaymentLineInformationDto = await base.ERPARPaymentLineRepository.GetARPaymentLine(aRPaymentLineId);
				string text = await base.ERPARPaymentLineRepository.WhereUsed("ARPaymentLines", new object[3] { eRPARPaymentLineInformationDto.arnArPaymentSessionID, eRPARPaymentLineInformationDto.arnArPaymentHeaderID, eRPARPaymentLineInformationDto.arnArPaymentLineID }, new object[3] { "arnArPaymentSessionID", "arnArPaymentHeaderID", "arnArPaymentLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ARPaymentLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPARPaymentLineDto>> Process_DeleteARPaymentLine(Guid aRPaymentLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPARPaymentLineDto> result;
		try
		{
			IERPARPaymentLineRepository iERPARPaymentLineRepository = (base.ERPARPaymentLineRepository = new ERPARPaymentLineRepository(base.ApiClientContext));
			using (iERPARPaymentLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPARPaymentLineRepository.DeleteRowFromTable("ARPaymentLines", "arn", aRPaymentLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ARPaymentLine [{aRPaymentLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARPaymentLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPARPaymentLineDto()
			};
		}
		return result;
	}
}
