using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPBankEntryModel : ERPBaseModel, IERPBankEntryModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllBankEntries(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPBankEntryRepository iERPBankEntryRepository = (base.ERPBankEntryRepository = new ERPBankEntryRepository(base.ApiClientContext));
		using (iERPBankEntryRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPBankEntryRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPBankEntryRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPBankEntryRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPBankEntryRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetBankEntry(Guid bankEntryId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPBankEntryRepository iERPBankEntryRepository = (base.ERPBankEntryRepository = new ERPBankEntryRepository(base.ApiClientContext));
		using (iERPBankEntryRepository)
		{
			if (!(await base.ERPBankEntryRepository.DoesBankEntryExist(bankEntryId)))
			{
				errorsList.Add($"BankEntry [{bankEntryId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutBankEntry(ERPBankEntryDto bankEntry)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPBankEntryRepository iERPBankEntryRepository = (base.ERPBankEntryRepository = new ERPBankEntryRepository(base.ApiClientContext));
		using (iERPBankEntryRepository)
		{
			if (bankEntry.gleArPaymentSessionID > 0 && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("ARPaymentSessions", new object[1] { "ARSARPAYMENTSESSIONID" }, new object[1] { bankEntry.gleArPaymentSessionID })))
			{
				errorsList.Add($"gleArPaymentSessionID [{bankEntry.gleArPaymentSessionID}] not found.");
			}
			if (bankEntry.gleArPaymentHeaderID > 0 && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("ARPaymentHeaders", new object[2] { "ARTARPAYMENTSESSIONID", "ARTARPAYMENTHEADERID" }, new object[2] { bankEntry.gleArPaymentSessionID, bankEntry.gleArPaymentHeaderID })))
			{
				errorsList.Add($"gleArPaymentHeaderID [{bankEntry.gleArPaymentHeaderID}] not found.");
			}
			if (bankEntry.gleApPaymentSessionID > 0 && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("APPaymentSessions", new object[1] { "APSAPPAYMENTSESSIONID" }, new object[1] { bankEntry.gleApPaymentSessionID })))
			{
				errorsList.Add($"gleApPaymentSessionID [{bankEntry.gleApPaymentSessionID}] not found.");
			}
			if (bankEntry.gleApPaymentHeaderID > 0 && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("APPaymentHeaders", new object[2] { "APTAPPAYMENTSESSIONID", "APTAPPAYMENTHEADERID" }, new object[2] { bankEntry.gleApPaymentSessionID, bankEntry.gleApPaymentHeaderID })))
			{
				errorsList.Add($"gleApPaymentHeaderID [{bankEntry.gleApPaymentHeaderID}] not found.");
			}
			if (bankEntry.glePayrollSessionID > 0 && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("PayrollSessions", new object[1] { "PASPAYROLLSESSIONID" }, new object[1] { bankEntry.glePayrollSessionID })))
			{
				errorsList.Add($"glePayrollSessionID [{bankEntry.glePayrollSessionID}] not found.");
			}
			if (bankEntry.glePayrollHeaderID > 0 && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("PayrollHeaders", new object[2] { "PATPAYROLLSESSIONID", "PATPAYROLLHEADERID" }, new object[2] { bankEntry.glePayrollSessionID, bankEntry.glePayrollHeaderID })))
			{
				errorsList.Add($"glePayrollHeaderID [{bankEntry.glePayrollHeaderID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(bankEntry.gleTaxCodeID) && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { bankEntry.gleTaxCodeID })))
			{
				errorsList.Add("gleTaxCodeID [" + bankEntry.gleTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(bankEntry.gleNonTaxReasonID) && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { bankEntry.gleNonTaxReasonID })))
			{
				errorsList.Add("gleNonTaxReasonID [" + bankEntry.gleNonTaxReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(bankEntry.gleGlAccountID) && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { bankEntry.gleGlAccountID })))
			{
				errorsList.Add("gleGlAccountID [" + bankEntry.gleGlAccountID + "] not found.");
			}
			if (bankEntry.gleBankStatementID > 0 && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("BankStatements", new object[1] { "GLSBANKSTATEMENTID" }, new object[1] { bankEntry.gleBankStatementID })))
			{
				errorsList.Add($"gleBankStatementID [{bankEntry.gleBankStatementID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(bankEntry.gleCashGlAccountID) && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { bankEntry.gleCashGlAccountID })))
			{
				errorsList.Add("gleCashGlAccountID [" + bankEntry.gleCashGlAccountID + "] not found.");
			}
			if (bankEntry.gleGlFiscalYearID > 0 && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { bankEntry.gleGlFiscalYearID })))
			{
				errorsList.Add($"gleGlFiscalYearID [{bankEntry.gleGlFiscalYearID}] not found.");
			}
			if (bankEntry.gleGlFiscalYearPeriodID > 0 && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearPeriods", new object[2] { "GLFGLFISCALYEARID", "GLFGLFISCALYEARPERIODID" }, new object[2] { bankEntry.gleGlFiscalYearID, bankEntry.gleGlFiscalYearPeriodID })))
			{
				errorsList.Add($"gleGlFiscalYearPeriodID [{bankEntry.gleGlFiscalYearPeriodID}] not found.");
			}
			if (bankEntry.gleGlJournalID > 0 && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("GLJournals", new object[1] { "GLPGLJOURNALID" }, new object[1] { bankEntry.gleGlJournalID })))
			{
				errorsList.Add($"gleGlJournalID [{bankEntry.gleGlJournalID}] not found.");
			}
			if (bankEntry.gleGlJournalLineID > 0 && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("GLJournalLines", new object[2] { "GLLGLJOURNALID", "GLLGLJOURNALLINEID" }, new object[2] { bankEntry.gleGlJournalID, bankEntry.gleGlJournalLineID })))
			{
				errorsList.Add($"gleGlJournalLineID [{bankEntry.gleGlJournalLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(bankEntry.gleCurrencyRateID) && !(await base.ERPBankEntryRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { bankEntry.gleCurrencyRateID })))
			{
				errorsList.Add("gleCurrencyRateID [" + bankEntry.gleCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPBankEntryDto>>> Process_GetAllBankEntries(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPBankEntryDto> allBankEntriesDto = new List<ERPBankEntryDto>();
		ERPResponseMessageDto<IList<ERPBankEntryDto>> result;
		try
		{
			IERPBankEntryRepository iERPBankEntryRepository = (base.ERPBankEntryRepository = new ERPBankEntryRepository(base.ApiClientContext));
			using (iERPBankEntryRepository)
			{
				foreach (ERPBankEntryInformationDto item2 in await base.ERPBankEntryRepository.GetAllBankEntries(pageSize, pageNumber, filter, orderBy))
				{
					ERPBankEntryDto item = new ERPBankEntryDto
					{
						gleApPaymentHeaderID = item2.gleApPaymentHeaderID,
						gleApPaymentSessionID = item2.gleApPaymentSessionID,
						gleArPaymentHeaderID = item2.gleArPaymentHeaderID,
						gleArPaymentSessionID = item2.gleArPaymentSessionID,
						gleBankStatementID = item2.gleBankStatementID,
						gleCashGlAccountID = item2.gleCashGlAccountID,
						gleCreatedBy = item2.gleCreatedBy,
						gleCreatedDate = item2.gleCreatedDate,
						gleCurrencyRateID = item2.gleCurrencyRateID,
						gleDescription = item2.gleDescription,
						gleEftReferenceNumber = item2.gleEftReferenceNumber,
						gleEntryType = item2.gleEntryType,
						gleUniqueID = item2.gleUniqueID,
						gleExchangeRate = item2.gleExchangeRate,
						gleGlAccountID = item2.gleGlAccountID,
						gleGlFiscalYearID = item2.gleGlFiscalYearID,
						gleGlFiscalYearPeriodID = item2.gleGlFiscalYearPeriodID,
						gleGlJournalID = item2.gleGlJournalID,
						gleGlJournalLineID = item2.gleGlJournalLineID,
						gleCleared = item2.gleCleared,
						gleCustomRate = item2.gleCustomRate,
						gleDoNotUpdateGl = item2.gleDoNotUpdateGl,
						glePostedToGl = item2.glePostedToGl,
						gleUnpresentedPayment = item2.gleUnpresentedPayment,
						gleNonTaxReasonID = item2.gleNonTaxReasonID,
						gleOrganizationID = item2.gleOrganizationID,
						gleOriginalAmount = item2.gleOriginalAmount,
						gleOriginalAmountForeign = item2.gleOriginalAmountForeign,
						glePaymentAmount = item2.glePaymentAmount,
						glePaymentAmountForeign = item2.glePaymentAmountForeign,
						glePaymentDate = item2.glePaymentDate,
						glePaymentNumber = item2.glePaymentNumber,
						glePayrollHeaderID = item2.glePayrollHeaderID,
						glePayrollSessionID = item2.glePayrollSessionID,
						glePayType = item2.glePayType,
						glePresentedDate = item2.glePresentedDate,
						gleRowVersion = item2.gleRowVersion,
						gleBankEntryID = item2.gleBankEntryID,
						gleSource = item2.gleSource,
						gleTaxAmount = item2.gleTaxAmount,
						gleTaxAmountForeign = item2.gleTaxAmountForeign,
						gleTaxCodeID = item2.gleTaxCodeID,
						gleTransactionDate = item2.gleTransactionDate,
						gleVarianceAmount = item2.gleVarianceAmount,
						gleVarianceAmountForeign = item2.gleVarianceAmountForeign,
						CustomFields = item2.CustomFields
					};
					allBankEntriesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all BankEntries]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPBankEntryDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allBankEntriesDto,
				RecordCount = allBankEntriesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPBankEntryDto>> Process_GetBankEntry(Guid bankEntryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPBankEntryDto bankEntryDto = null;
		ERPResponseMessageDto<ERPBankEntryDto> result;
		try
		{
			IERPBankEntryRepository iERPBankEntryRepository = (base.ERPBankEntryRepository = new ERPBankEntryRepository(base.ApiClientContext));
			using (iERPBankEntryRepository)
			{
				ERPBankEntryInformationDto eRPBankEntryInformationDto = await base.ERPBankEntryRepository.GetBankEntry(bankEntryId);
				bankEntryDto = new ERPBankEntryDto
				{
					gleApPaymentHeaderID = eRPBankEntryInformationDto.gleApPaymentHeaderID,
					gleApPaymentSessionID = eRPBankEntryInformationDto.gleApPaymentSessionID,
					gleArPaymentHeaderID = eRPBankEntryInformationDto.gleArPaymentHeaderID,
					gleArPaymentSessionID = eRPBankEntryInformationDto.gleArPaymentSessionID,
					gleBankStatementID = eRPBankEntryInformationDto.gleBankStatementID,
					gleCashGlAccountID = eRPBankEntryInformationDto.gleCashGlAccountID,
					gleCreatedBy = eRPBankEntryInformationDto.gleCreatedBy,
					gleCreatedDate = eRPBankEntryInformationDto.gleCreatedDate,
					gleCurrencyRateID = eRPBankEntryInformationDto.gleCurrencyRateID,
					gleDescription = eRPBankEntryInformationDto.gleDescription,
					gleEftReferenceNumber = eRPBankEntryInformationDto.gleEftReferenceNumber,
					gleEntryType = eRPBankEntryInformationDto.gleEntryType,
					gleUniqueID = eRPBankEntryInformationDto.gleUniqueID,
					gleExchangeRate = eRPBankEntryInformationDto.gleExchangeRate,
					gleGlAccountID = eRPBankEntryInformationDto.gleGlAccountID,
					gleGlFiscalYearID = eRPBankEntryInformationDto.gleGlFiscalYearID,
					gleGlFiscalYearPeriodID = eRPBankEntryInformationDto.gleGlFiscalYearPeriodID,
					gleGlJournalID = eRPBankEntryInformationDto.gleGlJournalID,
					gleGlJournalLineID = eRPBankEntryInformationDto.gleGlJournalLineID,
					gleCleared = eRPBankEntryInformationDto.gleCleared,
					gleCustomRate = eRPBankEntryInformationDto.gleCustomRate,
					gleDoNotUpdateGl = eRPBankEntryInformationDto.gleDoNotUpdateGl,
					glePostedToGl = eRPBankEntryInformationDto.glePostedToGl,
					gleUnpresentedPayment = eRPBankEntryInformationDto.gleUnpresentedPayment,
					gleNonTaxReasonID = eRPBankEntryInformationDto.gleNonTaxReasonID,
					gleOrganizationID = eRPBankEntryInformationDto.gleOrganizationID,
					gleOriginalAmount = eRPBankEntryInformationDto.gleOriginalAmount,
					gleOriginalAmountForeign = eRPBankEntryInformationDto.gleOriginalAmountForeign,
					glePaymentAmount = eRPBankEntryInformationDto.glePaymentAmount,
					glePaymentAmountForeign = eRPBankEntryInformationDto.glePaymentAmountForeign,
					glePaymentDate = eRPBankEntryInformationDto.glePaymentDate,
					glePaymentNumber = eRPBankEntryInformationDto.glePaymentNumber,
					glePayrollHeaderID = eRPBankEntryInformationDto.glePayrollHeaderID,
					glePayrollSessionID = eRPBankEntryInformationDto.glePayrollSessionID,
					glePayType = eRPBankEntryInformationDto.glePayType,
					glePresentedDate = eRPBankEntryInformationDto.glePresentedDate,
					gleRowVersion = eRPBankEntryInformationDto.gleRowVersion,
					gleBankEntryID = eRPBankEntryInformationDto.gleBankEntryID,
					gleSource = eRPBankEntryInformationDto.gleSource,
					gleTaxAmount = eRPBankEntryInformationDto.gleTaxAmount,
					gleTaxAmountForeign = eRPBankEntryInformationDto.gleTaxAmountForeign,
					gleTaxCodeID = eRPBankEntryInformationDto.gleTaxCodeID,
					gleTransactionDate = eRPBankEntryInformationDto.gleTransactionDate,
					gleVarianceAmount = eRPBankEntryInformationDto.gleVarianceAmount,
					gleVarianceAmountForeign = eRPBankEntryInformationDto.gleVarianceAmountForeign,
					CustomFields = eRPBankEntryInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the BankEntries []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPBankEntryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = bankEntryDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPBankEntryDto>> Process_PutBankEntry(ERPBankEntryDto bankEntry)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPBankEntryDto createdObject = null;
		ERPResponseMessageDto<ERPBankEntryDto> result;
		try
		{
			IERPBankEntryRepository iERPBankEntryRepository = (base.ERPBankEntryRepository = new ERPBankEntryRepository(base.ApiClientContext));
			using (iERPBankEntryRepository)
			{
				APIValidationInfoDto postResult = await base.ERPBankEntryRepository.SaveBankEntry(bankEntry);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPBankEntryInformationDto eRPBankEntryInformationDto = await base.ERPBankEntryRepository.GetBankEntry(bankEntry.gleUniqueID);
					createdObject = new ERPBankEntryDto
					{
						gleApPaymentHeaderID = eRPBankEntryInformationDto.gleApPaymentHeaderID,
						gleApPaymentSessionID = eRPBankEntryInformationDto.gleApPaymentSessionID,
						gleArPaymentHeaderID = eRPBankEntryInformationDto.gleArPaymentHeaderID,
						gleArPaymentSessionID = eRPBankEntryInformationDto.gleArPaymentSessionID,
						gleBankStatementID = eRPBankEntryInformationDto.gleBankStatementID,
						gleCashGlAccountID = eRPBankEntryInformationDto.gleCashGlAccountID,
						gleCreatedBy = eRPBankEntryInformationDto.gleCreatedBy,
						gleCreatedDate = eRPBankEntryInformationDto.gleCreatedDate,
						gleCurrencyRateID = eRPBankEntryInformationDto.gleCurrencyRateID,
						gleDescription = eRPBankEntryInformationDto.gleDescription,
						gleEftReferenceNumber = eRPBankEntryInformationDto.gleEftReferenceNumber,
						gleEntryType = eRPBankEntryInformationDto.gleEntryType,
						gleUniqueID = eRPBankEntryInformationDto.gleUniqueID,
						gleExchangeRate = eRPBankEntryInformationDto.gleExchangeRate,
						gleGlAccountID = eRPBankEntryInformationDto.gleGlAccountID,
						gleGlFiscalYearID = eRPBankEntryInformationDto.gleGlFiscalYearID,
						gleGlFiscalYearPeriodID = eRPBankEntryInformationDto.gleGlFiscalYearPeriodID,
						gleGlJournalID = eRPBankEntryInformationDto.gleGlJournalID,
						gleGlJournalLineID = eRPBankEntryInformationDto.gleGlJournalLineID,
						gleCleared = eRPBankEntryInformationDto.gleCleared,
						gleCustomRate = eRPBankEntryInformationDto.gleCustomRate,
						gleDoNotUpdateGl = eRPBankEntryInformationDto.gleDoNotUpdateGl,
						glePostedToGl = eRPBankEntryInformationDto.glePostedToGl,
						gleUnpresentedPayment = eRPBankEntryInformationDto.gleUnpresentedPayment,
						gleNonTaxReasonID = eRPBankEntryInformationDto.gleNonTaxReasonID,
						gleOrganizationID = eRPBankEntryInformationDto.gleOrganizationID,
						gleOriginalAmount = eRPBankEntryInformationDto.gleOriginalAmount,
						gleOriginalAmountForeign = eRPBankEntryInformationDto.gleOriginalAmountForeign,
						glePaymentAmount = eRPBankEntryInformationDto.glePaymentAmount,
						glePaymentAmountForeign = eRPBankEntryInformationDto.glePaymentAmountForeign,
						glePaymentDate = eRPBankEntryInformationDto.glePaymentDate,
						glePaymentNumber = eRPBankEntryInformationDto.glePaymentNumber,
						glePayrollHeaderID = eRPBankEntryInformationDto.glePayrollHeaderID,
						glePayrollSessionID = eRPBankEntryInformationDto.glePayrollSessionID,
						glePayType = eRPBankEntryInformationDto.glePayType,
						glePresentedDate = eRPBankEntryInformationDto.glePresentedDate,
						gleRowVersion = eRPBankEntryInformationDto.gleRowVersion,
						gleBankEntryID = eRPBankEntryInformationDto.gleBankEntryID,
						gleSource = eRPBankEntryInformationDto.gleSource,
						gleTaxAmount = eRPBankEntryInformationDto.gleTaxAmount,
						gleTaxAmountForeign = eRPBankEntryInformationDto.gleTaxAmountForeign,
						gleTaxCodeID = eRPBankEntryInformationDto.gleTaxCodeID,
						gleTransactionDate = eRPBankEntryInformationDto.gleTransactionDate,
						gleVarianceAmount = eRPBankEntryInformationDto.gleVarianceAmount,
						gleVarianceAmountForeign = eRPBankEntryInformationDto.gleVarianceAmountForeign,
						CustomFields = eRPBankEntryInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing BankEntry [{bankEntry.gleUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPBankEntryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteBankEntry(Guid bankEntryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPBankEntryRepository iERPBankEntryRepository = (base.ERPBankEntryRepository = new ERPBankEntryRepository(base.ApiClientContext));
		using (iERPBankEntryRepository)
		{
			if (!(await base.ERPBankEntryRepository.DoesBankEntryExist(bankEntryId)))
			{
				base.ErrorsList.Add($"BankEntry [{bankEntryId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPBankEntryInformationDto eRPBankEntryInformationDto = await base.ERPBankEntryRepository.GetBankEntry(bankEntryId);
				string text = await base.ERPBankEntryRepository.WhereUsed("BankEntries", new object[1] { eRPBankEntryInformationDto.gleBankEntryID }, new object[1] { "gleBankEntryID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("BankEntry cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPBankEntryDto>> Process_DeleteBankEntry(Guid bankEntryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPBankEntryDto> result;
		try
		{
			IERPBankEntryRepository iERPBankEntryRepository = (base.ERPBankEntryRepository = new ERPBankEntryRepository(base.ApiClientContext));
			using (iERPBankEntryRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPBankEntryRepository.DeleteRowFromTable("BankEntries", "gle", bankEntryId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of BankEntry [{bankEntryId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPBankEntryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPBankEntryDto()
			};
		}
		return result;
	}
}
