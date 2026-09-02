using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPBankStatementModel : ERPBaseModel, IERPBankStatementModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllBankStatements(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPBankStatementRepository iERPBankStatementRepository = (base.ERPBankStatementRepository = new ERPBankStatementRepository(base.ApiClientContext));
		using (iERPBankStatementRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPBankStatementRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPBankStatementRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPBankStatementRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPBankStatementRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetBankStatement(Guid bankStatementId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPBankStatementRepository iERPBankStatementRepository = (base.ERPBankStatementRepository = new ERPBankStatementRepository(base.ApiClientContext));
		using (iERPBankStatementRepository)
		{
			if (!(await base.ERPBankStatementRepository.DoesBankStatementExist(bankStatementId)))
			{
				errorsList.Add($"BankStatement [{bankStatementId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutBankStatement(ERPBankStatementDto bankStatement)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPBankStatementRepository iERPBankStatementRepository = (base.ERPBankStatementRepository = new ERPBankStatementRepository(base.ApiClientContext));
		using (iERPBankStatementRepository)
		{
			if (!string.IsNullOrWhiteSpace(bankStatement.glsCashGlAccountID) && !(await base.ERPBankStatementRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { bankStatement.glsCashGlAccountID })))
			{
				errorsList.Add("glsCashGlAccountID [" + bankStatement.glsCashGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(bankStatement.glsBankAccountID) && !(await base.ERPBankStatementRepository.DoesRecordExistInTableUsingKeys("BankAccounts", new object[1] { "GLNBANKACCOUNTID" }, new object[1] { bankStatement.glsBankAccountID })))
			{
				errorsList.Add("glsBankAccountID [" + bankStatement.glsBankAccountID + "] not found.");
			}
			if (bankStatement.glsGlFiscalYearID > 0 && !(await base.ERPBankStatementRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { bankStatement.glsGlFiscalYearID })))
			{
				errorsList.Add($"glsGlFiscalYearID [{bankStatement.glsGlFiscalYearID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(bankStatement.glsCurrencyRateID) && !(await base.ERPBankStatementRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { bankStatement.glsCurrencyRateID })))
			{
				errorsList.Add("glsCurrencyRateID [" + bankStatement.glsCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(bankStatement.glsExchangeGlAccountID) && !(await base.ERPBankStatementRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { bankStatement.glsExchangeGlAccountID })))
			{
				errorsList.Add("glsExchangeGlAccountID [" + bankStatement.glsExchangeGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPBankStatementDto>>> Process_GetAllBankStatements(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPBankStatementDto> allBankStatementsDto = new List<ERPBankStatementDto>();
		ERPResponseMessageDto<IList<ERPBankStatementDto>> result;
		try
		{
			IERPBankStatementRepository iERPBankStatementRepository = (base.ERPBankStatementRepository = new ERPBankStatementRepository(base.ApiClientContext));
			using (iERPBankStatementRepository)
			{
				foreach (ERPBankStatementInformationDto item2 in await base.ERPBankStatementRepository.GetAllBankStatements(pageSize, pageNumber, filter, orderBy))
				{
					ERPBankStatementDto item = new ERPBankStatementDto
					{
						glsBankAccountID = item2.glsBankAccountID,
						glsBankStatementReference = item2.glsBankStatementReference,
						glsCashGlAccountID = item2.glsCashGlAccountID,
						glsCreatedBy = item2.glsCreatedBy,
						glsCreatedDate = item2.glsCreatedDate,
						glsCurrencyRateID = item2.glsCurrencyRateID,
						glsEndingBalance = item2.glsEndingBalance,
						glsEndingBalanceForeign = item2.glsEndingBalanceForeign,
						glsEndingDate = item2.glsEndingDate,
						glsUniqueID = item2.glsUniqueID,
						glsExchangeAmount = item2.glsExchangeAmount,
						glsExchangeGlAccountID = item2.glsExchangeGlAccountID,
						glsExchangeRate = item2.glsExchangeRate,
						glsGlFiscalYearID = item2.glsGlFiscalYearID,
						glsCustomRate = item2.glsCustomRate,
						glsPostedToGl = item2.glsPostedToGl,
						glsOpeningBalance = item2.glsOpeningBalance,
						glsOpeningBalanceForeign = item2.glsOpeningBalanceForeign,
						glsOpeningDate = item2.glsOpeningDate,
						glsPostedDate = item2.glsPostedDate,
						glsRowVersion = item2.glsRowVersion,
						glsBankStatementID = item2.glsBankStatementID,
						glsShowTransactions = item2.glsShowTransactions,
						CustomFields = item2.CustomFields
					};
					allBankStatementsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all BankStatements]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPBankStatementDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allBankStatementsDto,
				RecordCount = allBankStatementsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPBankStatementDto>> Process_GetBankStatement(Guid bankStatementId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPBankStatementDto bankStatementDto = null;
		ERPResponseMessageDto<ERPBankStatementDto> result;
		try
		{
			IERPBankStatementRepository iERPBankStatementRepository = (base.ERPBankStatementRepository = new ERPBankStatementRepository(base.ApiClientContext));
			using (iERPBankStatementRepository)
			{
				ERPBankStatementInformationDto eRPBankStatementInformationDto = await base.ERPBankStatementRepository.GetBankStatement(bankStatementId);
				bankStatementDto = new ERPBankStatementDto
				{
					glsBankAccountID = eRPBankStatementInformationDto.glsBankAccountID,
					glsBankStatementReference = eRPBankStatementInformationDto.glsBankStatementReference,
					glsCashGlAccountID = eRPBankStatementInformationDto.glsCashGlAccountID,
					glsCreatedBy = eRPBankStatementInformationDto.glsCreatedBy,
					glsCreatedDate = eRPBankStatementInformationDto.glsCreatedDate,
					glsCurrencyRateID = eRPBankStatementInformationDto.glsCurrencyRateID,
					glsEndingBalance = eRPBankStatementInformationDto.glsEndingBalance,
					glsEndingBalanceForeign = eRPBankStatementInformationDto.glsEndingBalanceForeign,
					glsEndingDate = eRPBankStatementInformationDto.glsEndingDate,
					glsUniqueID = eRPBankStatementInformationDto.glsUniqueID,
					glsExchangeAmount = eRPBankStatementInformationDto.glsExchangeAmount,
					glsExchangeGlAccountID = eRPBankStatementInformationDto.glsExchangeGlAccountID,
					glsExchangeRate = eRPBankStatementInformationDto.glsExchangeRate,
					glsGlFiscalYearID = eRPBankStatementInformationDto.glsGlFiscalYearID,
					glsCustomRate = eRPBankStatementInformationDto.glsCustomRate,
					glsPostedToGl = eRPBankStatementInformationDto.glsPostedToGl,
					glsOpeningBalance = eRPBankStatementInformationDto.glsOpeningBalance,
					glsOpeningBalanceForeign = eRPBankStatementInformationDto.glsOpeningBalanceForeign,
					glsOpeningDate = eRPBankStatementInformationDto.glsOpeningDate,
					glsPostedDate = eRPBankStatementInformationDto.glsPostedDate,
					glsRowVersion = eRPBankStatementInformationDto.glsRowVersion,
					glsBankStatementID = eRPBankStatementInformationDto.glsBankStatementID,
					glsShowTransactions = eRPBankStatementInformationDto.glsShowTransactions,
					CustomFields = eRPBankStatementInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the BankStatements []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPBankStatementDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = bankStatementDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPBankStatementDto>> Process_PutBankStatement(ERPBankStatementDto bankStatement)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPBankStatementDto createdObject = null;
		ERPResponseMessageDto<ERPBankStatementDto> result;
		try
		{
			IERPBankStatementRepository iERPBankStatementRepository = (base.ERPBankStatementRepository = new ERPBankStatementRepository(base.ApiClientContext));
			using (iERPBankStatementRepository)
			{
				APIValidationInfoDto postResult = await base.ERPBankStatementRepository.SaveBankStatement(bankStatement);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPBankStatementInformationDto eRPBankStatementInformationDto = await base.ERPBankStatementRepository.GetBankStatement(bankStatement.glsUniqueID);
					createdObject = new ERPBankStatementDto
					{
						glsBankAccountID = eRPBankStatementInformationDto.glsBankAccountID,
						glsBankStatementReference = eRPBankStatementInformationDto.glsBankStatementReference,
						glsCashGlAccountID = eRPBankStatementInformationDto.glsCashGlAccountID,
						glsCreatedBy = eRPBankStatementInformationDto.glsCreatedBy,
						glsCreatedDate = eRPBankStatementInformationDto.glsCreatedDate,
						glsCurrencyRateID = eRPBankStatementInformationDto.glsCurrencyRateID,
						glsEndingBalance = eRPBankStatementInformationDto.glsEndingBalance,
						glsEndingBalanceForeign = eRPBankStatementInformationDto.glsEndingBalanceForeign,
						glsEndingDate = eRPBankStatementInformationDto.glsEndingDate,
						glsUniqueID = eRPBankStatementInformationDto.glsUniqueID,
						glsExchangeAmount = eRPBankStatementInformationDto.glsExchangeAmount,
						glsExchangeGlAccountID = eRPBankStatementInformationDto.glsExchangeGlAccountID,
						glsExchangeRate = eRPBankStatementInformationDto.glsExchangeRate,
						glsGlFiscalYearID = eRPBankStatementInformationDto.glsGlFiscalYearID,
						glsCustomRate = eRPBankStatementInformationDto.glsCustomRate,
						glsPostedToGl = eRPBankStatementInformationDto.glsPostedToGl,
						glsOpeningBalance = eRPBankStatementInformationDto.glsOpeningBalance,
						glsOpeningBalanceForeign = eRPBankStatementInformationDto.glsOpeningBalanceForeign,
						glsOpeningDate = eRPBankStatementInformationDto.glsOpeningDate,
						glsPostedDate = eRPBankStatementInformationDto.glsPostedDate,
						glsRowVersion = eRPBankStatementInformationDto.glsRowVersion,
						glsBankStatementID = eRPBankStatementInformationDto.glsBankStatementID,
						glsShowTransactions = eRPBankStatementInformationDto.glsShowTransactions,
						CustomFields = eRPBankStatementInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing BankStatement [{bankStatement.glsUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPBankStatementDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteBankStatement(Guid bankStatementId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPBankStatementRepository iERPBankStatementRepository = (base.ERPBankStatementRepository = new ERPBankStatementRepository(base.ApiClientContext));
		using (iERPBankStatementRepository)
		{
			if (!(await base.ERPBankStatementRepository.DoesBankStatementExist(bankStatementId)))
			{
				base.ErrorsList.Add($"BankStatement [{bankStatementId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPBankStatementInformationDto eRPBankStatementInformationDto = await base.ERPBankStatementRepository.GetBankStatement(bankStatementId);
				string text = await base.ERPBankStatementRepository.WhereUsed("BankStatements", new object[1] { eRPBankStatementInformationDto.glsBankStatementID }, new object[1] { "glsBankStatementID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("BankStatement cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPBankStatementDto>> Process_DeleteBankStatement(Guid bankStatementId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPBankStatementDto> result;
		try
		{
			IERPBankStatementRepository iERPBankStatementRepository = (base.ERPBankStatementRepository = new ERPBankStatementRepository(base.ApiClientContext));
			using (iERPBankStatementRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPBankStatementRepository.DeleteRowFromTable("BankStatements", "gls", bankStatementId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of BankStatement [{bankStatementId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPBankStatementDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPBankStatementDto()
			};
		}
		return result;
	}
}
