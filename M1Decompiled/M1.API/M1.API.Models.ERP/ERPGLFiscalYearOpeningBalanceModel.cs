using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLFiscalYearOpeningBalanceModel : ERPBaseModel, IERPGLFiscalYearOpeningBalanceModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYearOpeningBalances(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLFiscalYearOpeningBalanceRepository iERPGLFiscalYearOpeningBalanceRepository = (base.ERPGLFiscalYearOpeningBalanceRepository = new ERPGLFiscalYearOpeningBalanceRepository(base.ApiClientContext));
		using (iERPGLFiscalYearOpeningBalanceRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLFiscalYearOpeningBalanceRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLFiscalYearOpeningBalanceRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLFiscalYearOpeningBalanceRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLFiscalYearOpeningBalanceRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYearOpeningBalance(Guid gLFiscalYearOpeningBalanceId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearOpeningBalanceRepository iERPGLFiscalYearOpeningBalanceRepository = (base.ERPGLFiscalYearOpeningBalanceRepository = new ERPGLFiscalYearOpeningBalanceRepository(base.ApiClientContext));
		using (iERPGLFiscalYearOpeningBalanceRepository)
		{
			if (!(await base.ERPGLFiscalYearOpeningBalanceRepository.DoesGLFiscalYearOpeningBalanceExist(gLFiscalYearOpeningBalanceId)))
			{
				errorsList.Add($"GLFiscalYearOpeningBalance [{gLFiscalYearOpeningBalanceId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLFiscalYearOpeningBalanceDto>>> Process_GetAllGLFiscalYearOpeningBalances(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLFiscalYearOpeningBalanceDto> allGLFiscalYearOpeningBalancesDto = new List<ERPGLFiscalYearOpeningBalanceDto>();
		ERPResponseMessageDto<IList<ERPGLFiscalYearOpeningBalanceDto>> result;
		try
		{
			IERPGLFiscalYearOpeningBalanceRepository iERPGLFiscalYearOpeningBalanceRepository = (base.ERPGLFiscalYearOpeningBalanceRepository = new ERPGLFiscalYearOpeningBalanceRepository(base.ApiClientContext));
			using (iERPGLFiscalYearOpeningBalanceRepository)
			{
				foreach (ERPGLFiscalYearOpeningBalanceInformationDto item2 in await base.ERPGLFiscalYearOpeningBalanceRepository.GetAllGLFiscalYearOpeningBalances(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLFiscalYearOpeningBalanceDto item = new ERPGLFiscalYearOpeningBalanceDto
					{
						glyCreatedBy = item2.glyCreatedBy,
						glyCreatedDate = item2.glyCreatedDate,
						glyUniqueID = item2.glyUniqueID,
						glyGlAccountID = item2.glyGlAccountID,
						glyGlFiscalYearID = item2.glyGlFiscalYearID,
						glyRowVersion = item2.glyRowVersion,
						glyYearOpeningBalance = item2.glyYearOpeningBalance,
						CustomFields = item2.CustomFields
					};
					allGLFiscalYearOpeningBalancesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLFiscalYearOpeningBalances]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLFiscalYearOpeningBalanceDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLFiscalYearOpeningBalancesDto,
				RecordCount = allGLFiscalYearOpeningBalancesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearOpeningBalanceDto>> Process_GetGLFiscalYearOpeningBalance(Guid gLFiscalYearOpeningBalanceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLFiscalYearOpeningBalanceDto gLFiscalYearOpeningBalanceDto = null;
		ERPResponseMessageDto<ERPGLFiscalYearOpeningBalanceDto> result;
		try
		{
			IERPGLFiscalYearOpeningBalanceRepository iERPGLFiscalYearOpeningBalanceRepository = (base.ERPGLFiscalYearOpeningBalanceRepository = new ERPGLFiscalYearOpeningBalanceRepository(base.ApiClientContext));
			using (iERPGLFiscalYearOpeningBalanceRepository)
			{
				ERPGLFiscalYearOpeningBalanceInformationDto eRPGLFiscalYearOpeningBalanceInformationDto = await base.ERPGLFiscalYearOpeningBalanceRepository.GetGLFiscalYearOpeningBalance(gLFiscalYearOpeningBalanceId);
				gLFiscalYearOpeningBalanceDto = new ERPGLFiscalYearOpeningBalanceDto
				{
					glyCreatedBy = eRPGLFiscalYearOpeningBalanceInformationDto.glyCreatedBy,
					glyCreatedDate = eRPGLFiscalYearOpeningBalanceInformationDto.glyCreatedDate,
					glyUniqueID = eRPGLFiscalYearOpeningBalanceInformationDto.glyUniqueID,
					glyGlAccountID = eRPGLFiscalYearOpeningBalanceInformationDto.glyGlAccountID,
					glyGlFiscalYearID = eRPGLFiscalYearOpeningBalanceInformationDto.glyGlFiscalYearID,
					glyRowVersion = eRPGLFiscalYearOpeningBalanceInformationDto.glyRowVersion,
					glyYearOpeningBalance = eRPGLFiscalYearOpeningBalanceInformationDto.glyYearOpeningBalance,
					CustomFields = eRPGLFiscalYearOpeningBalanceInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLFiscalYearOpeningBalances []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearOpeningBalanceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLFiscalYearOpeningBalanceDto
			};
		}
		return result;
	}
}
