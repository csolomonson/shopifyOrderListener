using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPCurrencyRateLineModel : ERPBaseModel, IERPCurrencyRateLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllCurrencyRateLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCurrencyRateLineRepository iERPCurrencyRateLineRepository = (base.ERPCurrencyRateLineRepository = new ERPCurrencyRateLineRepository(base.ApiClientContext));
		using (iERPCurrencyRateLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPCurrencyRateLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPCurrencyRateLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPCurrencyRateLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPCurrencyRateLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetCurrencyRateLine(Guid currencyRateLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCurrencyRateLineRepository iERPCurrencyRateLineRepository = (base.ERPCurrencyRateLineRepository = new ERPCurrencyRateLineRepository(base.ApiClientContext));
		using (iERPCurrencyRateLineRepository)
		{
			if (!(await base.ERPCurrencyRateLineRepository.DoesCurrencyRateLineExist(currencyRateLineId)))
			{
				errorsList.Add($"CurrencyRateLine [{currencyRateLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutCurrencyRateLine(ERPCurrencyRateLineDto currencyRateLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCurrencyRateLineRepository iERPCurrencyRateLineRepository = (base.ERPCurrencyRateLineRepository = new ERPCurrencyRateLineRepository(base.ApiClientContext));
		using (iERPCurrencyRateLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(currencyRateLine.mclCurrencyRateID) && !(await base.ERPCurrencyRateLineRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { currencyRateLine.mclCurrencyRateID })))
			{
				errorsList.Add("mclCurrencyRateID [" + currencyRateLine.mclCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPCurrencyRateLineDto>>> Process_GetAllCurrencyRateLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPCurrencyRateLineDto> allCurrencyRateLinesDto = new List<ERPCurrencyRateLineDto>();
		ERPResponseMessageDto<IList<ERPCurrencyRateLineDto>> result;
		try
		{
			IERPCurrencyRateLineRepository iERPCurrencyRateLineRepository = (base.ERPCurrencyRateLineRepository = new ERPCurrencyRateLineRepository(base.ApiClientContext));
			using (iERPCurrencyRateLineRepository)
			{
				foreach (ERPCurrencyRateLineInformationDto item2 in await base.ERPCurrencyRateLineRepository.GetAllCurrencyRateLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPCurrencyRateLineDto item = new ERPCurrencyRateLineDto
					{
						mclCreatedBy = item2.mclCreatedBy,
						mclCreatedDate = item2.mclCreatedDate,
						mclCurrencyRateID = item2.mclCurrencyRateID,
						mclEffectiveDate = item2.mclEffectiveDate,
						mclUniqueID = item2.mclUniqueID,
						mclExchangeRate = item2.mclExchangeRate,
						mclReference = item2.mclReference,
						mclRowVersion = item2.mclRowVersion,
						mclCurrencyRateLineID = item2.mclCurrencyRateLineID,
						CustomFields = item2.CustomFields
					};
					allCurrencyRateLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all CurrencyRateLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPCurrencyRateLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allCurrencyRateLinesDto,
				RecordCount = allCurrencyRateLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCurrencyRateLineDto>> Process_GetCurrencyRateLine(Guid currencyRateLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPCurrencyRateLineDto currencyRateLineDto = null;
		ERPResponseMessageDto<ERPCurrencyRateLineDto> result;
		try
		{
			IERPCurrencyRateLineRepository iERPCurrencyRateLineRepository = (base.ERPCurrencyRateLineRepository = new ERPCurrencyRateLineRepository(base.ApiClientContext));
			using (iERPCurrencyRateLineRepository)
			{
				ERPCurrencyRateLineInformationDto eRPCurrencyRateLineInformationDto = await base.ERPCurrencyRateLineRepository.GetCurrencyRateLine(currencyRateLineId);
				currencyRateLineDto = new ERPCurrencyRateLineDto
				{
					mclCreatedBy = eRPCurrencyRateLineInformationDto.mclCreatedBy,
					mclCreatedDate = eRPCurrencyRateLineInformationDto.mclCreatedDate,
					mclCurrencyRateID = eRPCurrencyRateLineInformationDto.mclCurrencyRateID,
					mclEffectiveDate = eRPCurrencyRateLineInformationDto.mclEffectiveDate,
					mclUniqueID = eRPCurrencyRateLineInformationDto.mclUniqueID,
					mclExchangeRate = eRPCurrencyRateLineInformationDto.mclExchangeRate,
					mclReference = eRPCurrencyRateLineInformationDto.mclReference,
					mclRowVersion = eRPCurrencyRateLineInformationDto.mclRowVersion,
					mclCurrencyRateLineID = eRPCurrencyRateLineInformationDto.mclCurrencyRateLineID,
					CustomFields = eRPCurrencyRateLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the CurrencyRateLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCurrencyRateLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = currencyRateLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCurrencyRateLineDto>> Process_PutCurrencyRateLine(ERPCurrencyRateLineDto currencyRateLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPCurrencyRateLineDto createdObject = null;
		ERPResponseMessageDto<ERPCurrencyRateLineDto> result;
		try
		{
			IERPCurrencyRateLineRepository iERPCurrencyRateLineRepository = (base.ERPCurrencyRateLineRepository = new ERPCurrencyRateLineRepository(base.ApiClientContext));
			using (iERPCurrencyRateLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPCurrencyRateLineRepository.SaveCurrencyRateLine(currencyRateLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPCurrencyRateLineInformationDto eRPCurrencyRateLineInformationDto = await base.ERPCurrencyRateLineRepository.GetCurrencyRateLine(currencyRateLine.mclUniqueID);
					createdObject = new ERPCurrencyRateLineDto
					{
						mclCreatedBy = eRPCurrencyRateLineInformationDto.mclCreatedBy,
						mclCreatedDate = eRPCurrencyRateLineInformationDto.mclCreatedDate,
						mclCurrencyRateID = eRPCurrencyRateLineInformationDto.mclCurrencyRateID,
						mclEffectiveDate = eRPCurrencyRateLineInformationDto.mclEffectiveDate,
						mclUniqueID = eRPCurrencyRateLineInformationDto.mclUniqueID,
						mclExchangeRate = eRPCurrencyRateLineInformationDto.mclExchangeRate,
						mclReference = eRPCurrencyRateLineInformationDto.mclReference,
						mclRowVersion = eRPCurrencyRateLineInformationDto.mclRowVersion,
						mclCurrencyRateLineID = eRPCurrencyRateLineInformationDto.mclCurrencyRateLineID,
						CustomFields = eRPCurrencyRateLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing CurrencyRateLine [{currencyRateLine.mclUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCurrencyRateLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteCurrencyRateLine(Guid currencyRateLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCurrencyRateLineRepository iERPCurrencyRateLineRepository = (base.ERPCurrencyRateLineRepository = new ERPCurrencyRateLineRepository(base.ApiClientContext));
		using (iERPCurrencyRateLineRepository)
		{
			if (!(await base.ERPCurrencyRateLineRepository.DoesCurrencyRateLineExist(currencyRateLineId)))
			{
				base.ErrorsList.Add($"CurrencyRateLine [{currencyRateLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPCurrencyRateLineInformationDto eRPCurrencyRateLineInformationDto = await base.ERPCurrencyRateLineRepository.GetCurrencyRateLine(currencyRateLineId);
				string text = await base.ERPCurrencyRateLineRepository.WhereUsed("CurrencyRateLines", new object[2] { eRPCurrencyRateLineInformationDto.mclCurrencyRateID, eRPCurrencyRateLineInformationDto.mclCurrencyRateLineID }, new object[2] { "mclCurrencyRateID", "mclCurrencyRateLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("CurrencyRateLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPCurrencyRateLineDto>> Process_DeleteCurrencyRateLine(Guid currencyRateLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPCurrencyRateLineDto> result;
		try
		{
			IERPCurrencyRateLineRepository iERPCurrencyRateLineRepository = (base.ERPCurrencyRateLineRepository = new ERPCurrencyRateLineRepository(base.ApiClientContext));
			using (iERPCurrencyRateLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPCurrencyRateLineRepository.DeleteRowFromTable("CurrencyRateLines", "mcl", currencyRateLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of CurrencyRateLine [{currencyRateLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCurrencyRateLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPCurrencyRateLineDto()
			};
		}
		return result;
	}
}
