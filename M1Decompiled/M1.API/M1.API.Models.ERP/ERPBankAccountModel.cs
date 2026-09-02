using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPBankAccountModel : ERPBaseModel, IERPBankAccountModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllBankAccounts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPBankAccountRepository iERPBankAccountRepository = (base.ERPBankAccountRepository = new ERPBankAccountRepository(base.ApiClientContext));
		using (iERPBankAccountRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPBankAccountRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPBankAccountRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPBankAccountRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPBankAccountRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetBankAccount(Guid bankAccountId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPBankAccountRepository iERPBankAccountRepository = (base.ERPBankAccountRepository = new ERPBankAccountRepository(base.ApiClientContext));
		using (iERPBankAccountRepository)
		{
			if (!(await base.ERPBankAccountRepository.DoesBankAccountExist(bankAccountId)))
			{
				errorsList.Add($"BankAccount [{bankAccountId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutBankAccount(ERPBankAccountDto bankAccount)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPBankAccountRepository iERPBankAccountRepository = (base.ERPBankAccountRepository = new ERPBankAccountRepository(base.ApiClientContext));
		using (iERPBankAccountRepository)
		{
			if (!string.IsNullOrWhiteSpace(bankAccount.glnOrganizationID) && !(await base.ERPBankAccountRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { bankAccount.glnOrganizationID })))
			{
				errorsList.Add("glnOrganizationID [" + bankAccount.glnOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(bankAccount.glnCashGlAccountID) && !(await base.ERPBankAccountRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { bankAccount.glnCashGlAccountID })))
			{
				errorsList.Add("glnCashGlAccountID [" + bankAccount.glnCashGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(bankAccount.glnCurrencyRateID) && !(await base.ERPBankAccountRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { bankAccount.glnCurrencyRateID })))
			{
				errorsList.Add("glnCurrencyRateID [" + bankAccount.glnCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPBankAccountDto>>> Process_GetAllBankAccounts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPBankAccountDto> allBankAccountsDto = new List<ERPBankAccountDto>();
		ERPResponseMessageDto<IList<ERPBankAccountDto>> result;
		try
		{
			IERPBankAccountRepository iERPBankAccountRepository = (base.ERPBankAccountRepository = new ERPBankAccountRepository(base.ApiClientContext));
			using (iERPBankAccountRepository)
			{
				foreach (ERPBankAccountInformationDto item2 in await base.ERPBankAccountRepository.GetAllBankAccounts(pageSize, pageNumber, filter, orderBy))
				{
					ERPBankAccountDto item = new ERPBankAccountDto
					{
						glnBankAccountName = item2.glnBankAccountName,
						glnBankAccountNumber = item2.glnBankAccountNumber,
						glnBankInitials = item2.glnBankInitials,
						glnBankName = item2.glnBankName,
						glnBic = item2.glnBic,
						glnBsbNumber = item2.glnBsbNumber,
						glnCanadianEftType = item2.glnCanadianEftType,
						glnCashGlAccountID = item2.glnCashGlAccountID,
						glnBankAccountID = item2.glnBankAccountID,
						glnCreatedBy = item2.glnCreatedBy,
						glnCreatedDate = item2.glnCreatedDate,
						glnCurrencyRateID = item2.glnCurrencyRateID,
						glnDataCenterCode = item2.glnDataCenterCode,
						glnDescription = item2.glnDescription,
						glnDirectEntryUserID = item2.glnDirectEntryUserID,
						glnDirectEntryUserName = item2.glnDirectEntryUserName,
						glnEftApDescription = item2.glnEftApDescription,
						glnEftCompanyID = item2.glnEftCompanyID,
						glnEftCompanyName = item2.glnEftCompanyName,
						glnEftDiscretionaryData = item2.glnEftDiscretionaryData,
						glnEftFileID = item2.glnEftFileID,
						glnEftFileIDModifier = item2.glnEftFileIDModifier,
						glnEftFileLocation = item2.glnEftFileLocation,
						glnEftPayrollDescription = item2.glnEftPayrollDescription,
						glnEftReferenceCode = item2.glnEftReferenceCode,
						glnUniqueID = item2.glnUniqueID,
						glnFileCreationNumber = item2.glnFileCreationNumber,
						glnIban = item2.glnIban,
						glnInactiveDate = item2.glnInactiveDate,
						glnAChFormat = item2.glnAChFormat,
						glnInactive = item2.glnInactive,
						glnEftCreateOffsettingDebit = item2.glnEftCreateOffsettingDebit,
						glnPayrollOnly = item2.glnPayrollOnly,
						glnLanguageCode = item2.glnLanguageCode,
						glnNextEftNumber = item2.glnNextEftNumber,
						glnNextPaymentNumber = item2.glnNextPaymentNumber,
						glnNZEftType = item2.glnNZEftType,
						glnOrganizationID = item2.glnOrganizationID,
						glnRowVersion = item2.glnRowVersion,
						CustomFields = item2.CustomFields
					};
					allBankAccountsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all BankAccounts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPBankAccountDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allBankAccountsDto,
				RecordCount = allBankAccountsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPBankAccountDto>> Process_GetBankAccount(Guid bankAccountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPBankAccountDto bankAccountDto = null;
		ERPResponseMessageDto<ERPBankAccountDto> result;
		try
		{
			IERPBankAccountRepository iERPBankAccountRepository = (base.ERPBankAccountRepository = new ERPBankAccountRepository(base.ApiClientContext));
			using (iERPBankAccountRepository)
			{
				ERPBankAccountInformationDto eRPBankAccountInformationDto = await base.ERPBankAccountRepository.GetBankAccount(bankAccountId);
				bankAccountDto = new ERPBankAccountDto
				{
					glnBankAccountName = eRPBankAccountInformationDto.glnBankAccountName,
					glnBankAccountNumber = eRPBankAccountInformationDto.glnBankAccountNumber,
					glnBankInitials = eRPBankAccountInformationDto.glnBankInitials,
					glnBankName = eRPBankAccountInformationDto.glnBankName,
					glnBic = eRPBankAccountInformationDto.glnBic,
					glnBsbNumber = eRPBankAccountInformationDto.glnBsbNumber,
					glnCanadianEftType = eRPBankAccountInformationDto.glnCanadianEftType,
					glnCashGlAccountID = eRPBankAccountInformationDto.glnCashGlAccountID,
					glnBankAccountID = eRPBankAccountInformationDto.glnBankAccountID,
					glnCreatedBy = eRPBankAccountInformationDto.glnCreatedBy,
					glnCreatedDate = eRPBankAccountInformationDto.glnCreatedDate,
					glnCurrencyRateID = eRPBankAccountInformationDto.glnCurrencyRateID,
					glnDataCenterCode = eRPBankAccountInformationDto.glnDataCenterCode,
					glnDescription = eRPBankAccountInformationDto.glnDescription,
					glnDirectEntryUserID = eRPBankAccountInformationDto.glnDirectEntryUserID,
					glnDirectEntryUserName = eRPBankAccountInformationDto.glnDirectEntryUserName,
					glnEftApDescription = eRPBankAccountInformationDto.glnEftApDescription,
					glnEftCompanyID = eRPBankAccountInformationDto.glnEftCompanyID,
					glnEftCompanyName = eRPBankAccountInformationDto.glnEftCompanyName,
					glnEftDiscretionaryData = eRPBankAccountInformationDto.glnEftDiscretionaryData,
					glnEftFileID = eRPBankAccountInformationDto.glnEftFileID,
					glnEftFileIDModifier = eRPBankAccountInformationDto.glnEftFileIDModifier,
					glnEftFileLocation = eRPBankAccountInformationDto.glnEftFileLocation,
					glnEftPayrollDescription = eRPBankAccountInformationDto.glnEftPayrollDescription,
					glnEftReferenceCode = eRPBankAccountInformationDto.glnEftReferenceCode,
					glnUniqueID = eRPBankAccountInformationDto.glnUniqueID,
					glnFileCreationNumber = eRPBankAccountInformationDto.glnFileCreationNumber,
					glnIban = eRPBankAccountInformationDto.glnIban,
					glnInactiveDate = eRPBankAccountInformationDto.glnInactiveDate,
					glnAChFormat = eRPBankAccountInformationDto.glnAChFormat,
					glnInactive = eRPBankAccountInformationDto.glnInactive,
					glnEftCreateOffsettingDebit = eRPBankAccountInformationDto.glnEftCreateOffsettingDebit,
					glnPayrollOnly = eRPBankAccountInformationDto.glnPayrollOnly,
					glnLanguageCode = eRPBankAccountInformationDto.glnLanguageCode,
					glnNextEftNumber = eRPBankAccountInformationDto.glnNextEftNumber,
					glnNextPaymentNumber = eRPBankAccountInformationDto.glnNextPaymentNumber,
					glnNZEftType = eRPBankAccountInformationDto.glnNZEftType,
					glnOrganizationID = eRPBankAccountInformationDto.glnOrganizationID,
					glnRowVersion = eRPBankAccountInformationDto.glnRowVersion,
					CustomFields = eRPBankAccountInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the BankAccounts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPBankAccountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = bankAccountDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPBankAccountDto>> Process_PutBankAccount(ERPBankAccountDto bankAccount)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPBankAccountDto createdObject = null;
		ERPResponseMessageDto<ERPBankAccountDto> result;
		try
		{
			IERPBankAccountRepository iERPBankAccountRepository = (base.ERPBankAccountRepository = new ERPBankAccountRepository(base.ApiClientContext));
			using (iERPBankAccountRepository)
			{
				APIValidationInfoDto postResult = await base.ERPBankAccountRepository.SaveBankAccount(bankAccount);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPBankAccountInformationDto eRPBankAccountInformationDto = await base.ERPBankAccountRepository.GetBankAccount(bankAccount.glnUniqueID);
					createdObject = new ERPBankAccountDto
					{
						glnBankAccountName = eRPBankAccountInformationDto.glnBankAccountName,
						glnBankAccountNumber = eRPBankAccountInformationDto.glnBankAccountNumber,
						glnBankInitials = eRPBankAccountInformationDto.glnBankInitials,
						glnBankName = eRPBankAccountInformationDto.glnBankName,
						glnBic = eRPBankAccountInformationDto.glnBic,
						glnBsbNumber = eRPBankAccountInformationDto.glnBsbNumber,
						glnCanadianEftType = eRPBankAccountInformationDto.glnCanadianEftType,
						glnCashGlAccountID = eRPBankAccountInformationDto.glnCashGlAccountID,
						glnBankAccountID = eRPBankAccountInformationDto.glnBankAccountID,
						glnCreatedBy = eRPBankAccountInformationDto.glnCreatedBy,
						glnCreatedDate = eRPBankAccountInformationDto.glnCreatedDate,
						glnCurrencyRateID = eRPBankAccountInformationDto.glnCurrencyRateID,
						glnDataCenterCode = eRPBankAccountInformationDto.glnDataCenterCode,
						glnDescription = eRPBankAccountInformationDto.glnDescription,
						glnDirectEntryUserID = eRPBankAccountInformationDto.glnDirectEntryUserID,
						glnDirectEntryUserName = eRPBankAccountInformationDto.glnDirectEntryUserName,
						glnEftApDescription = eRPBankAccountInformationDto.glnEftApDescription,
						glnEftCompanyID = eRPBankAccountInformationDto.glnEftCompanyID,
						glnEftCompanyName = eRPBankAccountInformationDto.glnEftCompanyName,
						glnEftDiscretionaryData = eRPBankAccountInformationDto.glnEftDiscretionaryData,
						glnEftFileID = eRPBankAccountInformationDto.glnEftFileID,
						glnEftFileIDModifier = eRPBankAccountInformationDto.glnEftFileIDModifier,
						glnEftFileLocation = eRPBankAccountInformationDto.glnEftFileLocation,
						glnEftPayrollDescription = eRPBankAccountInformationDto.glnEftPayrollDescription,
						glnEftReferenceCode = eRPBankAccountInformationDto.glnEftReferenceCode,
						glnUniqueID = eRPBankAccountInformationDto.glnUniqueID,
						glnFileCreationNumber = eRPBankAccountInformationDto.glnFileCreationNumber,
						glnIban = eRPBankAccountInformationDto.glnIban,
						glnInactiveDate = eRPBankAccountInformationDto.glnInactiveDate,
						glnAChFormat = eRPBankAccountInformationDto.glnAChFormat,
						glnInactive = eRPBankAccountInformationDto.glnInactive,
						glnEftCreateOffsettingDebit = eRPBankAccountInformationDto.glnEftCreateOffsettingDebit,
						glnPayrollOnly = eRPBankAccountInformationDto.glnPayrollOnly,
						glnLanguageCode = eRPBankAccountInformationDto.glnLanguageCode,
						glnNextEftNumber = eRPBankAccountInformationDto.glnNextEftNumber,
						glnNextPaymentNumber = eRPBankAccountInformationDto.glnNextPaymentNumber,
						glnNZEftType = eRPBankAccountInformationDto.glnNZEftType,
						glnOrganizationID = eRPBankAccountInformationDto.glnOrganizationID,
						glnRowVersion = eRPBankAccountInformationDto.glnRowVersion,
						CustomFields = eRPBankAccountInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing BankAccount [{bankAccount.glnUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPBankAccountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteBankAccount(Guid bankAccountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPBankAccountRepository iERPBankAccountRepository = (base.ERPBankAccountRepository = new ERPBankAccountRepository(base.ApiClientContext));
		using (iERPBankAccountRepository)
		{
			if (!(await base.ERPBankAccountRepository.DoesBankAccountExist(bankAccountId)))
			{
				base.ErrorsList.Add($"BankAccount [{bankAccountId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPBankAccountInformationDto eRPBankAccountInformationDto = await base.ERPBankAccountRepository.GetBankAccount(bankAccountId);
				string text = await base.ERPBankAccountRepository.WhereUsed("BankAccounts", new object[1] { eRPBankAccountInformationDto.glnBankAccountID }, new object[1] { "glnBankAccountID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("BankAccount cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPBankAccountDto>> Process_DeleteBankAccount(Guid bankAccountId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPBankAccountDto> result;
		try
		{
			IERPBankAccountRepository iERPBankAccountRepository = (base.ERPBankAccountRepository = new ERPBankAccountRepository(base.ApiClientContext));
			using (iERPBankAccountRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPBankAccountRepository.DeleteRowFromTable("BankAccounts", "gln", bankAccountId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of BankAccount [{bankAccountId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPBankAccountDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPBankAccountDto()
			};
		}
		return result;
	}
}
