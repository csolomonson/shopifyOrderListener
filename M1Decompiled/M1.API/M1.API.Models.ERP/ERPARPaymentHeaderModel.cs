using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPARPaymentHeaderModel : ERPBaseModel, IERPARPaymentHeaderModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllARPaymentHeaders(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPARPaymentHeaderRepository iERPARPaymentHeaderRepository = (base.ERPARPaymentHeaderRepository = new ERPARPaymentHeaderRepository(base.ApiClientContext));
		using (iERPARPaymentHeaderRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPARPaymentHeaderRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPARPaymentHeaderRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPARPaymentHeaderRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPARPaymentHeaderRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetARPaymentHeader(Guid aRPaymentHeaderId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARPaymentHeaderRepository iERPARPaymentHeaderRepository = (base.ERPARPaymentHeaderRepository = new ERPARPaymentHeaderRepository(base.ApiClientContext));
		using (iERPARPaymentHeaderRepository)
		{
			if (!(await base.ERPARPaymentHeaderRepository.DoesARPaymentHeaderExist(aRPaymentHeaderId)))
			{
				errorsList.Add($"ARPaymentHeader [{aRPaymentHeaderId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutARPaymentHeader(ERPARPaymentHeaderDto aRPaymentHeader)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARPaymentHeaderRepository iERPARPaymentHeaderRepository = (base.ERPARPaymentHeaderRepository = new ERPARPaymentHeaderRepository(base.ApiClientContext));
		using (iERPARPaymentHeaderRepository)
		{
			if (aRPaymentHeader.artArPaymentSessionID > 0 && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("ARPaymentSessions", new object[1] { "ARSARPAYMENTSESSIONID" }, new object[1] { aRPaymentHeader.artArPaymentSessionID })))
			{
				errorsList.Add($"artArPaymentSessionID [{aRPaymentHeader.artArPaymentSessionID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentHeader.artCustomerOrganizationID) && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { aRPaymentHeader.artCustomerOrganizationID })))
			{
				errorsList.Add("artCustomerOrganizationID [" + aRPaymentHeader.artCustomerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentHeader.artArInvoiceLocationID) && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { aRPaymentHeader.artCustomerOrganizationID, aRPaymentHeader.artArInvoiceLocationID })))
			{
				errorsList.Add("artArInvoiceLocationID [" + aRPaymentHeader.artArInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentHeader.artArInvoiceContactID) && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { aRPaymentHeader.artCustomerOrganizationID, aRPaymentHeader.artArInvoiceLocationID, aRPaymentHeader.artArInvoiceContactID })))
			{
				errorsList.Add("artArInvoiceContactID [" + aRPaymentHeader.artArInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentHeader.artGlAccountID) && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRPaymentHeader.artGlAccountID })))
			{
				errorsList.Add("artGlAccountID [" + aRPaymentHeader.artGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentHeader.artTaxCodeID) && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aRPaymentHeader.artTaxCodeID })))
			{
				errorsList.Add("artTaxCodeID [" + aRPaymentHeader.artTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentHeader.artNonTaxReasonID) && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { aRPaymentHeader.artNonTaxReasonID })))
			{
				errorsList.Add("artNonTaxReasonID [" + aRPaymentHeader.artNonTaxReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentHeader.artSecondTaxCodeID) && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { aRPaymentHeader.artSecondTaxCodeID })))
			{
				errorsList.Add("artSecondTaxCodeID [" + aRPaymentHeader.artSecondTaxCodeID + "] not found.");
			}
			if (aRPaymentHeader.artGlFiscalYearID > 0 && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { aRPaymentHeader.artGlFiscalYearID })))
			{
				errorsList.Add($"artGlFiscalYearID [{aRPaymentHeader.artGlFiscalYearID}] not found.");
			}
			if (aRPaymentHeader.artGlFiscalYearPeriodID > 0 && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearPeriods", new object[2] { "GLFGLFISCALYEARID", "GLFGLFISCALYEARPERIODID" }, new object[2] { aRPaymentHeader.artGlFiscalYearID, aRPaymentHeader.artGlFiscalYearPeriodID })))
			{
				errorsList.Add($"artGlFiscalYearPeriodID [{aRPaymentHeader.artGlFiscalYearPeriodID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentHeader.artCreditArInvoiceID) && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { aRPaymentHeader.artCreditArInvoiceID })))
			{
				errorsList.Add("artCreditArInvoiceID [" + aRPaymentHeader.artCreditArInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentHeader.artCashGlAccountID) && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRPaymentHeader.artCashGlAccountID })))
			{
				errorsList.Add("artCashGlAccountID [" + aRPaymentHeader.artCashGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentHeader.artArGlAccountID) && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRPaymentHeader.artArGlAccountID })))
			{
				errorsList.Add("artArGlAccountID [" + aRPaymentHeader.artArGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentHeader.artCreatedCreditArInvoiceID) && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { aRPaymentHeader.artCreatedCreditArInvoiceID })))
			{
				errorsList.Add("artCreatedCreditArInvoiceID [" + aRPaymentHeader.artCreatedCreditArInvoiceID + "] not found.");
			}
			if (aRPaymentHeader.artVoidArPaymentSessionID > 0 && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("ARPaymentSessions", new object[1] { "ARSARPAYMENTSESSIONID" }, new object[1] { aRPaymentHeader.artVoidArPaymentSessionID })))
			{
				errorsList.Add($"artVoidArPaymentSessionID [{aRPaymentHeader.artVoidArPaymentSessionID}] not found.");
			}
			if (aRPaymentHeader.artVoidArPaymentHeaderId > 0 && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("ARPaymentHeaders", new object[2] { "ARTARPAYMENTSESSIONID", "ARTARPAYMENTHEADERID" }, new object[2] { aRPaymentHeader.artVoidArPaymentSessionID, aRPaymentHeader.artVoidArPaymentHeaderId })))
			{
				errorsList.Add($"artVoidArPaymentHeaderId [{aRPaymentHeader.artVoidArPaymentHeaderId}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRPaymentHeader.artExchangeGlAccountID) && !(await base.ERPARPaymentHeaderRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { aRPaymentHeader.artExchangeGlAccountID })))
			{
				errorsList.Add("artExchangeGlAccountID [" + aRPaymentHeader.artExchangeGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPARPaymentHeaderDto>>> Process_GetAllARPaymentHeaders(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPARPaymentHeaderDto> allARPaymentHeadersDto = new List<ERPARPaymentHeaderDto>();
		ERPResponseMessageDto<IList<ERPARPaymentHeaderDto>> result;
		try
		{
			IERPARPaymentHeaderRepository iERPARPaymentHeaderRepository = (base.ERPARPaymentHeaderRepository = new ERPARPaymentHeaderRepository(base.ApiClientContext));
			using (iERPARPaymentHeaderRepository)
			{
				foreach (ERPARPaymentHeaderInformationDto item2 in await base.ERPARPaymentHeaderRepository.GetAllARPaymentHeaders(pageSize, pageNumber, filter, orderBy))
				{
					ERPARPaymentHeaderDto item = new ERPARPaymentHeaderDto
					{
						artArGlAccountID = item2.artArGlAccountID,
						artArInvoiceContactID = item2.artArInvoiceContactID,
						artArInvoiceLocationID = item2.artArInvoiceLocationID,
						artArPaymentSessionID = item2.artArPaymentSessionID,
						artBankAccountName = item2.artBankAccountName,
						artBankAccountNumber = item2.artBankAccountNumber,
						artBankInitials = item2.artBankInitials,
						artBsbNumber = item2.artBsbNumber,
						artCashGlAccountID = item2.artCashGlAccountID,
						artCreatedBy = item2.artCreatedBy,
						artCreatedCreditArInvoiceID = item2.artCreatedCreditArInvoiceID,
						artCreatedDate = item2.artCreatedDate,
						artCreditArInvoiceID = item2.artCreditArInvoiceID,
						artCustomerOrganizationID = item2.artCustomerOrganizationID,
						artCustomerPaymentNumber = item2.artCustomerPaymentNumber,
						artDescription = item2.artDescription,
						artUniqueID = item2.artUniqueID,
						artExchangeAmount = item2.artExchangeAmount,
						artExchangeGlAccountID = item2.artExchangeGlAccountID,
						artGlAccountID = item2.artGlAccountID,
						artGlFiscalYearID = item2.artGlFiscalYearID,
						artGlFiscalYearPeriodID = item2.artGlFiscalYearPeriodID,
						artAvalaraTaxCalculated = item2.artAvalaraTaxCalculated,
						artNet1PaymentProcessed = item2.artNet1PaymentProcessed,
						artOpenPaymentLoad = item2.artOpenPaymentLoad,
						artPostedToGl = item2.artPostedToGl,
						artVoidedPayment = item2.artVoidedPayment,
						artLongDescriptionRtf = item2.artLongDescriptionRtf,
						artLongDescriptionText = item2.artLongDescriptionText,
						artNonTaxReasonID = item2.artNonTaxReasonID,
						artPaymentMethod = item2.artPaymentMethod,
						artReceiptAmount = item2.artReceiptAmount,
						artReceiptAmountForeign = item2.artReceiptAmountForeign,
						artReceiptDate = item2.artReceiptDate,
						artReceiptType = item2.artReceiptType,
						artRowVersion = item2.artRowVersion,
						artSecondTaxAmount = item2.artSecondTaxAmount,
						artSecondTaxAmountForeign = item2.artSecondTaxAmountForeign,
						artSecondTaxCodeID = item2.artSecondTaxCodeID,
						artArPaymentHeaderID = item2.artArPaymentHeaderID,
						artShowAllInvoices = item2.artShowAllInvoices,
						artTaxAmount = item2.artTaxAmount,
						artTaxAmountForeign = item2.artTaxAmountForeign,
						artTaxCodeID = item2.artTaxCodeID,
						artVoidArPaymentHeaderId = item2.artVoidArPaymentHeaderId,
						artVoidArPaymentSessionID = item2.artVoidArPaymentSessionID,
						CustomFields = item2.CustomFields
					};
					allARPaymentHeadersDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ARPaymentHeaders]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPARPaymentHeaderDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allARPaymentHeadersDto,
				RecordCount = allARPaymentHeadersDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARPaymentHeaderDto>> Process_GetARPaymentHeader(Guid aRPaymentHeaderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPARPaymentHeaderDto aRPaymentHeaderDto = null;
		ERPResponseMessageDto<ERPARPaymentHeaderDto> result;
		try
		{
			IERPARPaymentHeaderRepository iERPARPaymentHeaderRepository = (base.ERPARPaymentHeaderRepository = new ERPARPaymentHeaderRepository(base.ApiClientContext));
			using (iERPARPaymentHeaderRepository)
			{
				ERPARPaymentHeaderInformationDto eRPARPaymentHeaderInformationDto = await base.ERPARPaymentHeaderRepository.GetARPaymentHeader(aRPaymentHeaderId);
				aRPaymentHeaderDto = new ERPARPaymentHeaderDto
				{
					artArGlAccountID = eRPARPaymentHeaderInformationDto.artArGlAccountID,
					artArInvoiceContactID = eRPARPaymentHeaderInformationDto.artArInvoiceContactID,
					artArInvoiceLocationID = eRPARPaymentHeaderInformationDto.artArInvoiceLocationID,
					artArPaymentSessionID = eRPARPaymentHeaderInformationDto.artArPaymentSessionID,
					artBankAccountName = eRPARPaymentHeaderInformationDto.artBankAccountName,
					artBankAccountNumber = eRPARPaymentHeaderInformationDto.artBankAccountNumber,
					artBankInitials = eRPARPaymentHeaderInformationDto.artBankInitials,
					artBsbNumber = eRPARPaymentHeaderInformationDto.artBsbNumber,
					artCashGlAccountID = eRPARPaymentHeaderInformationDto.artCashGlAccountID,
					artCreatedBy = eRPARPaymentHeaderInformationDto.artCreatedBy,
					artCreatedCreditArInvoiceID = eRPARPaymentHeaderInformationDto.artCreatedCreditArInvoiceID,
					artCreatedDate = eRPARPaymentHeaderInformationDto.artCreatedDate,
					artCreditArInvoiceID = eRPARPaymentHeaderInformationDto.artCreditArInvoiceID,
					artCustomerOrganizationID = eRPARPaymentHeaderInformationDto.artCustomerOrganizationID,
					artCustomerPaymentNumber = eRPARPaymentHeaderInformationDto.artCustomerPaymentNumber,
					artDescription = eRPARPaymentHeaderInformationDto.artDescription,
					artUniqueID = eRPARPaymentHeaderInformationDto.artUniqueID,
					artExchangeAmount = eRPARPaymentHeaderInformationDto.artExchangeAmount,
					artExchangeGlAccountID = eRPARPaymentHeaderInformationDto.artExchangeGlAccountID,
					artGlAccountID = eRPARPaymentHeaderInformationDto.artGlAccountID,
					artGlFiscalYearID = eRPARPaymentHeaderInformationDto.artGlFiscalYearID,
					artGlFiscalYearPeriodID = eRPARPaymentHeaderInformationDto.artGlFiscalYearPeriodID,
					artAvalaraTaxCalculated = eRPARPaymentHeaderInformationDto.artAvalaraTaxCalculated,
					artNet1PaymentProcessed = eRPARPaymentHeaderInformationDto.artNet1PaymentProcessed,
					artOpenPaymentLoad = eRPARPaymentHeaderInformationDto.artOpenPaymentLoad,
					artPostedToGl = eRPARPaymentHeaderInformationDto.artPostedToGl,
					artVoidedPayment = eRPARPaymentHeaderInformationDto.artVoidedPayment,
					artLongDescriptionRtf = eRPARPaymentHeaderInformationDto.artLongDescriptionRtf,
					artLongDescriptionText = eRPARPaymentHeaderInformationDto.artLongDescriptionText,
					artNonTaxReasonID = eRPARPaymentHeaderInformationDto.artNonTaxReasonID,
					artPaymentMethod = eRPARPaymentHeaderInformationDto.artPaymentMethod,
					artReceiptAmount = eRPARPaymentHeaderInformationDto.artReceiptAmount,
					artReceiptAmountForeign = eRPARPaymentHeaderInformationDto.artReceiptAmountForeign,
					artReceiptDate = eRPARPaymentHeaderInformationDto.artReceiptDate,
					artReceiptType = eRPARPaymentHeaderInformationDto.artReceiptType,
					artRowVersion = eRPARPaymentHeaderInformationDto.artRowVersion,
					artSecondTaxAmount = eRPARPaymentHeaderInformationDto.artSecondTaxAmount,
					artSecondTaxAmountForeign = eRPARPaymentHeaderInformationDto.artSecondTaxAmountForeign,
					artSecondTaxCodeID = eRPARPaymentHeaderInformationDto.artSecondTaxCodeID,
					artArPaymentHeaderID = eRPARPaymentHeaderInformationDto.artArPaymentHeaderID,
					artShowAllInvoices = eRPARPaymentHeaderInformationDto.artShowAllInvoices,
					artTaxAmount = eRPARPaymentHeaderInformationDto.artTaxAmount,
					artTaxAmountForeign = eRPARPaymentHeaderInformationDto.artTaxAmountForeign,
					artTaxCodeID = eRPARPaymentHeaderInformationDto.artTaxCodeID,
					artVoidArPaymentHeaderId = eRPARPaymentHeaderInformationDto.artVoidArPaymentHeaderId,
					artVoidArPaymentSessionID = eRPARPaymentHeaderInformationDto.artVoidArPaymentSessionID,
					CustomFields = eRPARPaymentHeaderInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ARPaymentHeaders []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARPaymentHeaderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aRPaymentHeaderDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARPaymentHeaderDto>> Process_PutARPaymentHeader(ERPARPaymentHeaderDto aRPaymentHeader)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPARPaymentHeaderDto createdObject = null;
		ERPResponseMessageDto<ERPARPaymentHeaderDto> result;
		try
		{
			IERPARPaymentHeaderRepository iERPARPaymentHeaderRepository = (base.ERPARPaymentHeaderRepository = new ERPARPaymentHeaderRepository(base.ApiClientContext));
			using (iERPARPaymentHeaderRepository)
			{
				APIValidationInfoDto postResult = await base.ERPARPaymentHeaderRepository.SaveARPaymentHeader(aRPaymentHeader);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPARPaymentHeaderInformationDto eRPARPaymentHeaderInformationDto = await base.ERPARPaymentHeaderRepository.GetARPaymentHeader(aRPaymentHeader.artUniqueID);
					createdObject = new ERPARPaymentHeaderDto
					{
						artArGlAccountID = eRPARPaymentHeaderInformationDto.artArGlAccountID,
						artArInvoiceContactID = eRPARPaymentHeaderInformationDto.artArInvoiceContactID,
						artArInvoiceLocationID = eRPARPaymentHeaderInformationDto.artArInvoiceLocationID,
						artArPaymentSessionID = eRPARPaymentHeaderInformationDto.artArPaymentSessionID,
						artBankAccountName = eRPARPaymentHeaderInformationDto.artBankAccountName,
						artBankAccountNumber = eRPARPaymentHeaderInformationDto.artBankAccountNumber,
						artBankInitials = eRPARPaymentHeaderInformationDto.artBankInitials,
						artBsbNumber = eRPARPaymentHeaderInformationDto.artBsbNumber,
						artCashGlAccountID = eRPARPaymentHeaderInformationDto.artCashGlAccountID,
						artCreatedBy = eRPARPaymentHeaderInformationDto.artCreatedBy,
						artCreatedCreditArInvoiceID = eRPARPaymentHeaderInformationDto.artCreatedCreditArInvoiceID,
						artCreatedDate = eRPARPaymentHeaderInformationDto.artCreatedDate,
						artCreditArInvoiceID = eRPARPaymentHeaderInformationDto.artCreditArInvoiceID,
						artCustomerOrganizationID = eRPARPaymentHeaderInformationDto.artCustomerOrganizationID,
						artCustomerPaymentNumber = eRPARPaymentHeaderInformationDto.artCustomerPaymentNumber,
						artDescription = eRPARPaymentHeaderInformationDto.artDescription,
						artUniqueID = eRPARPaymentHeaderInformationDto.artUniqueID,
						artExchangeAmount = eRPARPaymentHeaderInformationDto.artExchangeAmount,
						artExchangeGlAccountID = eRPARPaymentHeaderInformationDto.artExchangeGlAccountID,
						artGlAccountID = eRPARPaymentHeaderInformationDto.artGlAccountID,
						artGlFiscalYearID = eRPARPaymentHeaderInformationDto.artGlFiscalYearID,
						artGlFiscalYearPeriodID = eRPARPaymentHeaderInformationDto.artGlFiscalYearPeriodID,
						artAvalaraTaxCalculated = eRPARPaymentHeaderInformationDto.artAvalaraTaxCalculated,
						artNet1PaymentProcessed = eRPARPaymentHeaderInformationDto.artNet1PaymentProcessed,
						artOpenPaymentLoad = eRPARPaymentHeaderInformationDto.artOpenPaymentLoad,
						artPostedToGl = eRPARPaymentHeaderInformationDto.artPostedToGl,
						artVoidedPayment = eRPARPaymentHeaderInformationDto.artVoidedPayment,
						artLongDescriptionRtf = eRPARPaymentHeaderInformationDto.artLongDescriptionRtf,
						artLongDescriptionText = eRPARPaymentHeaderInformationDto.artLongDescriptionText,
						artNonTaxReasonID = eRPARPaymentHeaderInformationDto.artNonTaxReasonID,
						artPaymentMethod = eRPARPaymentHeaderInformationDto.artPaymentMethod,
						artReceiptAmount = eRPARPaymentHeaderInformationDto.artReceiptAmount,
						artReceiptAmountForeign = eRPARPaymentHeaderInformationDto.artReceiptAmountForeign,
						artReceiptDate = eRPARPaymentHeaderInformationDto.artReceiptDate,
						artReceiptType = eRPARPaymentHeaderInformationDto.artReceiptType,
						artRowVersion = eRPARPaymentHeaderInformationDto.artRowVersion,
						artSecondTaxAmount = eRPARPaymentHeaderInformationDto.artSecondTaxAmount,
						artSecondTaxAmountForeign = eRPARPaymentHeaderInformationDto.artSecondTaxAmountForeign,
						artSecondTaxCodeID = eRPARPaymentHeaderInformationDto.artSecondTaxCodeID,
						artArPaymentHeaderID = eRPARPaymentHeaderInformationDto.artArPaymentHeaderID,
						artShowAllInvoices = eRPARPaymentHeaderInformationDto.artShowAllInvoices,
						artTaxAmount = eRPARPaymentHeaderInformationDto.artTaxAmount,
						artTaxAmountForeign = eRPARPaymentHeaderInformationDto.artTaxAmountForeign,
						artTaxCodeID = eRPARPaymentHeaderInformationDto.artTaxCodeID,
						artVoidArPaymentHeaderId = eRPARPaymentHeaderInformationDto.artVoidArPaymentHeaderId,
						artVoidArPaymentSessionID = eRPARPaymentHeaderInformationDto.artVoidArPaymentSessionID,
						CustomFields = eRPARPaymentHeaderInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ARPaymentHeader [{aRPaymentHeader.artUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARPaymentHeaderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteARPaymentHeader(Guid aRPaymentHeaderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARPaymentHeaderRepository iERPARPaymentHeaderRepository = (base.ERPARPaymentHeaderRepository = new ERPARPaymentHeaderRepository(base.ApiClientContext));
		using (iERPARPaymentHeaderRepository)
		{
			if (!(await base.ERPARPaymentHeaderRepository.DoesARPaymentHeaderExist(aRPaymentHeaderId)))
			{
				base.ErrorsList.Add($"ARPaymentHeader [{aRPaymentHeaderId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPARPaymentHeaderInformationDto eRPARPaymentHeaderInformationDto = await base.ERPARPaymentHeaderRepository.GetARPaymentHeader(aRPaymentHeaderId);
				string text = await base.ERPARPaymentHeaderRepository.WhereUsed("ARPaymentHeaders", new object[2] { eRPARPaymentHeaderInformationDto.artArPaymentSessionID, eRPARPaymentHeaderInformationDto.artArPaymentHeaderID }, new object[2] { "artArPaymentSessionID", "artArPaymentHeaderID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ARPaymentHeader cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPARPaymentHeaderDto>> Process_DeleteARPaymentHeader(Guid aRPaymentHeaderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPARPaymentHeaderDto> result;
		try
		{
			IERPARPaymentHeaderRepository iERPARPaymentHeaderRepository = (base.ERPARPaymentHeaderRepository = new ERPARPaymentHeaderRepository(base.ApiClientContext));
			using (iERPARPaymentHeaderRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPARPaymentHeaderRepository.DeleteRowFromTable("ARPaymentHeaders", "art", aRPaymentHeaderId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ARPaymentHeader [{aRPaymentHeaderId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARPaymentHeaderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPARPaymentHeaderDto()
			};
		}
		return result;
	}
}
