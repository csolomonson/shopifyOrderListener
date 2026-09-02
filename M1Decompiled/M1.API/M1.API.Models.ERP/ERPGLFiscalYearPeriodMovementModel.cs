using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLFiscalYearPeriodMovementModel : ERPBaseModel, IERPGLFiscalYearPeriodMovementModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLFiscalYearPeriodMovements(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLFiscalYearPeriodMovementRepository iERPGLFiscalYearPeriodMovementRepository = (base.ERPGLFiscalYearPeriodMovementRepository = new ERPGLFiscalYearPeriodMovementRepository(base.ApiClientContext));
		using (iERPGLFiscalYearPeriodMovementRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLFiscalYearPeriodMovementRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLFiscalYearPeriodMovementRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLFiscalYearPeriodMovementRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLFiscalYearPeriodMovementRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLFiscalYearPeriodMovement(Guid gLFiscalYearPeriodMovementId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLFiscalYearPeriodMovementRepository iERPGLFiscalYearPeriodMovementRepository = (base.ERPGLFiscalYearPeriodMovementRepository = new ERPGLFiscalYearPeriodMovementRepository(base.ApiClientContext));
		using (iERPGLFiscalYearPeriodMovementRepository)
		{
			if (!(await base.ERPGLFiscalYearPeriodMovementRepository.DoesGLFiscalYearPeriodMovementExist(gLFiscalYearPeriodMovementId)))
			{
				errorsList.Add($"GLFiscalYearPeriodMovement [{gLFiscalYearPeriodMovementId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLFiscalYearPeriodMovementDto>>> Process_GetAllGLFiscalYearPeriodMovements(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLFiscalYearPeriodMovementDto> allGLFiscalYearPeriodMovementsDto = new List<ERPGLFiscalYearPeriodMovementDto>();
		ERPResponseMessageDto<IList<ERPGLFiscalYearPeriodMovementDto>> result;
		try
		{
			IERPGLFiscalYearPeriodMovementRepository iERPGLFiscalYearPeriodMovementRepository = (base.ERPGLFiscalYearPeriodMovementRepository = new ERPGLFiscalYearPeriodMovementRepository(base.ApiClientContext));
			using (iERPGLFiscalYearPeriodMovementRepository)
			{
				foreach (ERPGLFiscalYearPeriodMovementInformationDto item2 in await base.ERPGLFiscalYearPeriodMovementRepository.GetAllGLFiscalYearPeriodMovements(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLFiscalYearPeriodMovementDto item = new ERPGLFiscalYearPeriodMovementDto
					{
						gliCreatedBy = item2.gliCreatedBy,
						gliCreatedDate = item2.gliCreatedDate,
						gliUniqueID = item2.gliUniqueID,
						gliGlAccountID = item2.gliGlAccountID,
						gliGlFiscalYearID = item2.gliGlFiscalYearID,
						gliGlFiscalYearPeriodID = item2.gliGlFiscalYearPeriodID,
						gliRowVersion = item2.gliRowVersion,
						gliTotalCredits = item2.gliTotalCredits,
						gliTotalDebits = item2.gliTotalDebits,
						CustomFields = item2.CustomFields
					};
					allGLFiscalYearPeriodMovementsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLFiscalYearPeriodMovements]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLFiscalYearPeriodMovementDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLFiscalYearPeriodMovementsDto,
				RecordCount = allGLFiscalYearPeriodMovementsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLFiscalYearPeriodMovementDto>> Process_GetGLFiscalYearPeriodMovement(Guid gLFiscalYearPeriodMovementId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLFiscalYearPeriodMovementDto gLFiscalYearPeriodMovementDto = null;
		ERPResponseMessageDto<ERPGLFiscalYearPeriodMovementDto> result;
		try
		{
			IERPGLFiscalYearPeriodMovementRepository iERPGLFiscalYearPeriodMovementRepository = (base.ERPGLFiscalYearPeriodMovementRepository = new ERPGLFiscalYearPeriodMovementRepository(base.ApiClientContext));
			using (iERPGLFiscalYearPeriodMovementRepository)
			{
				ERPGLFiscalYearPeriodMovementInformationDto eRPGLFiscalYearPeriodMovementInformationDto = await base.ERPGLFiscalYearPeriodMovementRepository.GetGLFiscalYearPeriodMovement(gLFiscalYearPeriodMovementId);
				gLFiscalYearPeriodMovementDto = new ERPGLFiscalYearPeriodMovementDto
				{
					gliCreatedBy = eRPGLFiscalYearPeriodMovementInformationDto.gliCreatedBy,
					gliCreatedDate = eRPGLFiscalYearPeriodMovementInformationDto.gliCreatedDate,
					gliUniqueID = eRPGLFiscalYearPeriodMovementInformationDto.gliUniqueID,
					gliGlAccountID = eRPGLFiscalYearPeriodMovementInformationDto.gliGlAccountID,
					gliGlFiscalYearID = eRPGLFiscalYearPeriodMovementInformationDto.gliGlFiscalYearID,
					gliGlFiscalYearPeriodID = eRPGLFiscalYearPeriodMovementInformationDto.gliGlFiscalYearPeriodID,
					gliRowVersion = eRPGLFiscalYearPeriodMovementInformationDto.gliRowVersion,
					gliTotalCredits = eRPGLFiscalYearPeriodMovementInformationDto.gliTotalCredits,
					gliTotalDebits = eRPGLFiscalYearPeriodMovementInformationDto.gliTotalDebits,
					CustomFields = eRPGLFiscalYearPeriodMovementInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLFiscalYearPeriodMovements []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLFiscalYearPeriodMovementDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLFiscalYearPeriodMovementDto
			};
		}
		return result;
	}
}
