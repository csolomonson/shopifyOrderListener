using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPunchCalculatorModel : ERPBaseModel, IERPPunchCalculatorModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPunchCalculators(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPunchCalculatorRepository iERPPunchCalculatorRepository = (base.ERPPunchCalculatorRepository = new ERPPunchCalculatorRepository(base.ApiClientContext));
		using (iERPPunchCalculatorRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPunchCalculatorRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPunchCalculatorRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPunchCalculatorRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPunchCalculatorRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPunchCalculator(Guid punchCalculatorId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPunchCalculatorRepository iERPPunchCalculatorRepository = (base.ERPPunchCalculatorRepository = new ERPPunchCalculatorRepository(base.ApiClientContext));
		using (iERPPunchCalculatorRepository)
		{
			if (!(await base.ERPPunchCalculatorRepository.DoesPunchCalculatorExist(punchCalculatorId)))
			{
				errorsList.Add($"PunchCalculator [{punchCalculatorId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPunchCalculator(ERPPunchCalculatorDto punchCalculator)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPunchCalculatorRepository iERPPunchCalculatorRepository = (base.ERPPunchCalculatorRepository = new ERPPunchCalculatorRepository(base.ApiClientContext));
		using (iERPPunchCalculatorRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPunchCalculatorDto>>> Process_GetAllPunchCalculators(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPunchCalculatorDto> allPunchCalculatorsDto = new List<ERPPunchCalculatorDto>();
		ERPResponseMessageDto<IList<ERPPunchCalculatorDto>> result;
		try
		{
			IERPPunchCalculatorRepository iERPPunchCalculatorRepository = (base.ERPPunchCalculatorRepository = new ERPPunchCalculatorRepository(base.ApiClientContext));
			using (iERPPunchCalculatorRepository)
			{
				foreach (ERPPunchCalculatorInformationDto item2 in await base.ERPPunchCalculatorRepository.GetAllPunchCalculators(pageSize, pageNumber, filter, orderBy))
				{
					ERPPunchCalculatorDto item = new ERPPunchCalculatorDto
					{
						ccuPunchCalculatorId = item2.ccuPunchCalculatorId,
						ccuCreatedBy = item2.ccuCreatedBy,
						ccuCreatedDate = item2.ccuCreatedDate,
						ccuUniqueID = item2.ccuUniqueID,
						ccuHitRate = item2.ccuHitRate,
						ccuHitsPerPart = item2.ccuHitsPerPart,
						ccuPartsPerHour = item2.ccuPartsPerHour,
						ccuPartsPerSheet = item2.ccuPartsPerSheet,
						ccuRepositions = item2.ccuRepositions,
						ccuRepositionTime = item2.ccuRepositionTime,
						ccuRepositionTimeSec = item2.ccuRepositionTimeSec,
						ccuRowVersion = item2.ccuRowVersion,
						ccuSheetLoadTime = item2.ccuSheetLoadTime,
						ccuSheetLoadTimeSec = item2.ccuSheetLoadTimeSec,
						ccuSheetsPerHour = item2.ccuSheetsPerHour,
						ccuTimeToPiece = item2.ccuTimeToPiece,
						ccuToolChangeTimeSec = item2.ccuToolChangeTimeSec,
						ccuToolChangeTimeTotal = item2.ccuToolChangeTimeTotal,
						ccuTools = item2.ccuTools,
						ccuTotalTimeMinutes = item2.ccuTotalTimeMinutes,
						ccuTotalTimeSeconds = item2.ccuTotalTimeSeconds,
						ccuTurns = item2.ccuTurns,
						CustomFields = item2.CustomFields
					};
					allPunchCalculatorsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PunchCalculators]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPunchCalculatorDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPunchCalculatorsDto,
				RecordCount = allPunchCalculatorsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPunchCalculatorDto>> Process_GetPunchCalculator(Guid punchCalculatorId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPunchCalculatorDto punchCalculatorDto = null;
		ERPResponseMessageDto<ERPPunchCalculatorDto> result;
		try
		{
			IERPPunchCalculatorRepository iERPPunchCalculatorRepository = (base.ERPPunchCalculatorRepository = new ERPPunchCalculatorRepository(base.ApiClientContext));
			using (iERPPunchCalculatorRepository)
			{
				ERPPunchCalculatorInformationDto eRPPunchCalculatorInformationDto = await base.ERPPunchCalculatorRepository.GetPunchCalculator(punchCalculatorId);
				punchCalculatorDto = new ERPPunchCalculatorDto
				{
					ccuPunchCalculatorId = eRPPunchCalculatorInformationDto.ccuPunchCalculatorId,
					ccuCreatedBy = eRPPunchCalculatorInformationDto.ccuCreatedBy,
					ccuCreatedDate = eRPPunchCalculatorInformationDto.ccuCreatedDate,
					ccuUniqueID = eRPPunchCalculatorInformationDto.ccuUniqueID,
					ccuHitRate = eRPPunchCalculatorInformationDto.ccuHitRate,
					ccuHitsPerPart = eRPPunchCalculatorInformationDto.ccuHitsPerPart,
					ccuPartsPerHour = eRPPunchCalculatorInformationDto.ccuPartsPerHour,
					ccuPartsPerSheet = eRPPunchCalculatorInformationDto.ccuPartsPerSheet,
					ccuRepositions = eRPPunchCalculatorInformationDto.ccuRepositions,
					ccuRepositionTime = eRPPunchCalculatorInformationDto.ccuRepositionTime,
					ccuRepositionTimeSec = eRPPunchCalculatorInformationDto.ccuRepositionTimeSec,
					ccuRowVersion = eRPPunchCalculatorInformationDto.ccuRowVersion,
					ccuSheetLoadTime = eRPPunchCalculatorInformationDto.ccuSheetLoadTime,
					ccuSheetLoadTimeSec = eRPPunchCalculatorInformationDto.ccuSheetLoadTimeSec,
					ccuSheetsPerHour = eRPPunchCalculatorInformationDto.ccuSheetsPerHour,
					ccuTimeToPiece = eRPPunchCalculatorInformationDto.ccuTimeToPiece,
					ccuToolChangeTimeSec = eRPPunchCalculatorInformationDto.ccuToolChangeTimeSec,
					ccuToolChangeTimeTotal = eRPPunchCalculatorInformationDto.ccuToolChangeTimeTotal,
					ccuTools = eRPPunchCalculatorInformationDto.ccuTools,
					ccuTotalTimeMinutes = eRPPunchCalculatorInformationDto.ccuTotalTimeMinutes,
					ccuTotalTimeSeconds = eRPPunchCalculatorInformationDto.ccuTotalTimeSeconds,
					ccuTurns = eRPPunchCalculatorInformationDto.ccuTurns,
					CustomFields = eRPPunchCalculatorInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PunchCalculators []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPunchCalculatorDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = punchCalculatorDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPunchCalculatorDto>> Process_PutPunchCalculator(ERPPunchCalculatorDto punchCalculator)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPunchCalculatorDto createdObject = null;
		ERPResponseMessageDto<ERPPunchCalculatorDto> result;
		try
		{
			IERPPunchCalculatorRepository iERPPunchCalculatorRepository = (base.ERPPunchCalculatorRepository = new ERPPunchCalculatorRepository(base.ApiClientContext));
			using (iERPPunchCalculatorRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPunchCalculatorRepository.SavePunchCalculator(punchCalculator);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPunchCalculatorInformationDto eRPPunchCalculatorInformationDto = await base.ERPPunchCalculatorRepository.GetPunchCalculator(punchCalculator.ccuUniqueID);
					createdObject = new ERPPunchCalculatorDto
					{
						ccuPunchCalculatorId = eRPPunchCalculatorInformationDto.ccuPunchCalculatorId,
						ccuCreatedBy = eRPPunchCalculatorInformationDto.ccuCreatedBy,
						ccuCreatedDate = eRPPunchCalculatorInformationDto.ccuCreatedDate,
						ccuUniqueID = eRPPunchCalculatorInformationDto.ccuUniqueID,
						ccuHitRate = eRPPunchCalculatorInformationDto.ccuHitRate,
						ccuHitsPerPart = eRPPunchCalculatorInformationDto.ccuHitsPerPart,
						ccuPartsPerHour = eRPPunchCalculatorInformationDto.ccuPartsPerHour,
						ccuPartsPerSheet = eRPPunchCalculatorInformationDto.ccuPartsPerSheet,
						ccuRepositions = eRPPunchCalculatorInformationDto.ccuRepositions,
						ccuRepositionTime = eRPPunchCalculatorInformationDto.ccuRepositionTime,
						ccuRepositionTimeSec = eRPPunchCalculatorInformationDto.ccuRepositionTimeSec,
						ccuRowVersion = eRPPunchCalculatorInformationDto.ccuRowVersion,
						ccuSheetLoadTime = eRPPunchCalculatorInformationDto.ccuSheetLoadTime,
						ccuSheetLoadTimeSec = eRPPunchCalculatorInformationDto.ccuSheetLoadTimeSec,
						ccuSheetsPerHour = eRPPunchCalculatorInformationDto.ccuSheetsPerHour,
						ccuTimeToPiece = eRPPunchCalculatorInformationDto.ccuTimeToPiece,
						ccuToolChangeTimeSec = eRPPunchCalculatorInformationDto.ccuToolChangeTimeSec,
						ccuToolChangeTimeTotal = eRPPunchCalculatorInformationDto.ccuToolChangeTimeTotal,
						ccuTools = eRPPunchCalculatorInformationDto.ccuTools,
						ccuTotalTimeMinutes = eRPPunchCalculatorInformationDto.ccuTotalTimeMinutes,
						ccuTotalTimeSeconds = eRPPunchCalculatorInformationDto.ccuTotalTimeSeconds,
						ccuTurns = eRPPunchCalculatorInformationDto.ccuTurns,
						CustomFields = eRPPunchCalculatorInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PunchCalculator [{punchCalculator.ccuUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPunchCalculatorDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePunchCalculator(Guid punchCalculatorId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPunchCalculatorRepository iERPPunchCalculatorRepository = (base.ERPPunchCalculatorRepository = new ERPPunchCalculatorRepository(base.ApiClientContext));
		using (iERPPunchCalculatorRepository)
		{
			if (!(await base.ERPPunchCalculatorRepository.DoesPunchCalculatorExist(punchCalculatorId)))
			{
				base.ErrorsList.Add($"PunchCalculator [{punchCalculatorId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPunchCalculatorInformationDto eRPPunchCalculatorInformationDto = await base.ERPPunchCalculatorRepository.GetPunchCalculator(punchCalculatorId);
				string text = await base.ERPPunchCalculatorRepository.WhereUsed("PunchCalculators", new object[1] { eRPPunchCalculatorInformationDto.ccuPunchCalculatorId }, new object[1] { "ccuPunchCalculatorId" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PunchCalculator cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPunchCalculatorDto>> Process_DeletePunchCalculator(Guid punchCalculatorId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPunchCalculatorDto> result;
		try
		{
			IERPPunchCalculatorRepository iERPPunchCalculatorRepository = (base.ERPPunchCalculatorRepository = new ERPPunchCalculatorRepository(base.ApiClientContext));
			using (iERPPunchCalculatorRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPunchCalculatorRepository.DeleteRowFromTable("PunchCalculators", "ccu", punchCalculatorId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PunchCalculator [{punchCalculatorId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPunchCalculatorDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPunchCalculatorDto()
			};
		}
		return result;
	}
}
