using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPCurrencyRateModel : ERPBaseModel, IERPCurrencyRateModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllCurrencyRates(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCurrencyRateRepository iERPCurrencyRateRepository = (base.ERPCurrencyRateRepository = new ERPCurrencyRateRepository(base.ApiClientContext));
		using (iERPCurrencyRateRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPCurrencyRateRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPCurrencyRateRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPCurrencyRateRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPCurrencyRateRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetCurrencyRate(Guid currencyRateId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCurrencyRateRepository iERPCurrencyRateRepository = (base.ERPCurrencyRateRepository = new ERPCurrencyRateRepository(base.ApiClientContext));
		using (iERPCurrencyRateRepository)
		{
			if (!(await base.ERPCurrencyRateRepository.DoesCurrencyRateExist(currencyRateId)))
			{
				errorsList.Add($"CurrencyRate [{currencyRateId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutCurrencyRate(ERPCurrencyRateDto currencyRate)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCurrencyRateRepository iERPCurrencyRateRepository = (base.ERPCurrencyRateRepository = new ERPCurrencyRateRepository(base.ApiClientContext));
		using (iERPCurrencyRateRepository)
		{
			if (!string.IsNullOrWhiteSpace(currencyRate.mcpExchangeGainGlAccountID) && !(await base.ERPCurrencyRateRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { currencyRate.mcpExchangeGainGlAccountID })))
			{
				errorsList.Add("mcpExchangeGainGlAccountID [" + currencyRate.mcpExchangeGainGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(currencyRate.mcpArGlAccountID) && !(await base.ERPCurrencyRateRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { currencyRate.mcpArGlAccountID })))
			{
				errorsList.Add("mcpArGlAccountID [" + currencyRate.mcpArGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(currencyRate.mcpApGlAccountID) && !(await base.ERPCurrencyRateRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { currencyRate.mcpApGlAccountID })))
			{
				errorsList.Add("mcpApGlAccountID [" + currencyRate.mcpApGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(currencyRate.mcpExchangeLossGlAccountID) && !(await base.ERPCurrencyRateRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { currencyRate.mcpExchangeLossGlAccountID })))
			{
				errorsList.Add("mcpExchangeLossGlAccountID [" + currencyRate.mcpExchangeLossGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(currencyRate.mcpUnrealisedExGainGlAccountID) && !(await base.ERPCurrencyRateRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { currencyRate.mcpUnrealisedExGainGlAccountID })))
			{
				errorsList.Add("mcpUnrealisedExGainGlAccountID [" + currencyRate.mcpUnrealisedExGainGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(currencyRate.mcpUnrealisedExLossGlAccountID) && !(await base.ERPCurrencyRateRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { currencyRate.mcpUnrealisedExLossGlAccountID })))
			{
				errorsList.Add("mcpUnrealisedExLossGlAccountID [" + currencyRate.mcpUnrealisedExLossGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPCurrencyRateDto>>> Process_GetAllCurrencyRates(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPCurrencyRateDto> allCurrencyRatesDto = new List<ERPCurrencyRateDto>();
		ERPResponseMessageDto<IList<ERPCurrencyRateDto>> result;
		try
		{
			IERPCurrencyRateRepository iERPCurrencyRateRepository = (base.ERPCurrencyRateRepository = new ERPCurrencyRateRepository(base.ApiClientContext));
			using (iERPCurrencyRateRepository)
			{
				foreach (ERPCurrencyRateInformationDto item2 in await base.ERPCurrencyRateRepository.GetAllCurrencyRates(pageSize, pageNumber, filter, orderBy))
				{
					ERPCurrencyRateDto item = new ERPCurrencyRateDto
					{
						mcpApGlAccountID = item2.mcpApGlAccountID,
						mcpArGlAccountID = item2.mcpArGlAccountID,
						mcpCurrencyRateID = item2.mcpCurrencyRateID,
						mcpCreatedBy = item2.mcpCreatedBy,
						mcpCreatedDate = item2.mcpCreatedDate,
						mcpDescription = item2.mcpDescription,
						mcpUniqueID = item2.mcpUniqueID,
						mcpExchangeGainGlAccountID = item2.mcpExchangeGainGlAccountID,
						mcpExchangeLossGlAccountID = item2.mcpExchangeLossGlAccountID,
						mcpRowVersion = item2.mcpRowVersion,
						mcpSymbol = item2.mcpSymbol,
						mcpUnrealisedExGainGlAccountID = item2.mcpUnrealisedExGainGlAccountID,
						mcpUnrealisedExLossGlAccountID = item2.mcpUnrealisedExLossGlAccountID,
						CustomFields = item2.CustomFields
					};
					allCurrencyRatesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all CurrencyRates]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPCurrencyRateDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allCurrencyRatesDto,
				RecordCount = allCurrencyRatesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCurrencyRateDto>> Process_GetCurrencyRate(Guid currencyRateId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPCurrencyRateDto currencyRateDto = null;
		ERPResponseMessageDto<ERPCurrencyRateDto> result;
		try
		{
			IERPCurrencyRateRepository iERPCurrencyRateRepository = (base.ERPCurrencyRateRepository = new ERPCurrencyRateRepository(base.ApiClientContext));
			using (iERPCurrencyRateRepository)
			{
				ERPCurrencyRateInformationDto eRPCurrencyRateInformationDto = await base.ERPCurrencyRateRepository.GetCurrencyRate(currencyRateId);
				currencyRateDto = new ERPCurrencyRateDto
				{
					mcpApGlAccountID = eRPCurrencyRateInformationDto.mcpApGlAccountID,
					mcpArGlAccountID = eRPCurrencyRateInformationDto.mcpArGlAccountID,
					mcpCurrencyRateID = eRPCurrencyRateInformationDto.mcpCurrencyRateID,
					mcpCreatedBy = eRPCurrencyRateInformationDto.mcpCreatedBy,
					mcpCreatedDate = eRPCurrencyRateInformationDto.mcpCreatedDate,
					mcpDescription = eRPCurrencyRateInformationDto.mcpDescription,
					mcpUniqueID = eRPCurrencyRateInformationDto.mcpUniqueID,
					mcpExchangeGainGlAccountID = eRPCurrencyRateInformationDto.mcpExchangeGainGlAccountID,
					mcpExchangeLossGlAccountID = eRPCurrencyRateInformationDto.mcpExchangeLossGlAccountID,
					mcpRowVersion = eRPCurrencyRateInformationDto.mcpRowVersion,
					mcpSymbol = eRPCurrencyRateInformationDto.mcpSymbol,
					mcpUnrealisedExGainGlAccountID = eRPCurrencyRateInformationDto.mcpUnrealisedExGainGlAccountID,
					mcpUnrealisedExLossGlAccountID = eRPCurrencyRateInformationDto.mcpUnrealisedExLossGlAccountID,
					CustomFields = eRPCurrencyRateInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the CurrencyRates []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCurrencyRateDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = currencyRateDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCurrencyRateDto>> Process_PutCurrencyRate(ERPCurrencyRateDto currencyRate)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPCurrencyRateDto createdObject = null;
		ERPResponseMessageDto<ERPCurrencyRateDto> result;
		try
		{
			IERPCurrencyRateRepository iERPCurrencyRateRepository = (base.ERPCurrencyRateRepository = new ERPCurrencyRateRepository(base.ApiClientContext));
			using (iERPCurrencyRateRepository)
			{
				APIValidationInfoDto postResult = await base.ERPCurrencyRateRepository.SaveCurrencyRate(currencyRate);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPCurrencyRateInformationDto eRPCurrencyRateInformationDto = await base.ERPCurrencyRateRepository.GetCurrencyRate(currencyRate.mcpUniqueID);
					createdObject = new ERPCurrencyRateDto
					{
						mcpApGlAccountID = eRPCurrencyRateInformationDto.mcpApGlAccountID,
						mcpArGlAccountID = eRPCurrencyRateInformationDto.mcpArGlAccountID,
						mcpCurrencyRateID = eRPCurrencyRateInformationDto.mcpCurrencyRateID,
						mcpCreatedBy = eRPCurrencyRateInformationDto.mcpCreatedBy,
						mcpCreatedDate = eRPCurrencyRateInformationDto.mcpCreatedDate,
						mcpDescription = eRPCurrencyRateInformationDto.mcpDescription,
						mcpUniqueID = eRPCurrencyRateInformationDto.mcpUniqueID,
						mcpExchangeGainGlAccountID = eRPCurrencyRateInformationDto.mcpExchangeGainGlAccountID,
						mcpExchangeLossGlAccountID = eRPCurrencyRateInformationDto.mcpExchangeLossGlAccountID,
						mcpRowVersion = eRPCurrencyRateInformationDto.mcpRowVersion,
						mcpSymbol = eRPCurrencyRateInformationDto.mcpSymbol,
						mcpUnrealisedExGainGlAccountID = eRPCurrencyRateInformationDto.mcpUnrealisedExGainGlAccountID,
						mcpUnrealisedExLossGlAccountID = eRPCurrencyRateInformationDto.mcpUnrealisedExLossGlAccountID,
						CustomFields = eRPCurrencyRateInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing CurrencyRate [{currencyRate.mcpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCurrencyRateDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteCurrencyRate(Guid currencyRateId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCurrencyRateRepository iERPCurrencyRateRepository = (base.ERPCurrencyRateRepository = new ERPCurrencyRateRepository(base.ApiClientContext));
		using (iERPCurrencyRateRepository)
		{
			if (!(await base.ERPCurrencyRateRepository.DoesCurrencyRateExist(currencyRateId)))
			{
				base.ErrorsList.Add($"CurrencyRate [{currencyRateId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPCurrencyRateInformationDto eRPCurrencyRateInformationDto = await base.ERPCurrencyRateRepository.GetCurrencyRate(currencyRateId);
				string text = await base.ERPCurrencyRateRepository.WhereUsed("CurrencyRates", new object[1] { eRPCurrencyRateInformationDto.mcpCurrencyRateID }, new object[1] { "mcpCurrencyRateID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("CurrencyRate cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPCurrencyRateDto>> Process_DeleteCurrencyRate(Guid currencyRateId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPCurrencyRateDto> result;
		try
		{
			IERPCurrencyRateRepository iERPCurrencyRateRepository = (base.ERPCurrencyRateRepository = new ERPCurrencyRateRepository(base.ApiClientContext));
			using (iERPCurrencyRateRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPCurrencyRateRepository.DeleteRowFromTable("CurrencyRates", "mcp", currencyRateId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of CurrencyRate [{currencyRateId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCurrencyRateDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPCurrencyRateDto()
			};
		}
		return result;
	}
}
