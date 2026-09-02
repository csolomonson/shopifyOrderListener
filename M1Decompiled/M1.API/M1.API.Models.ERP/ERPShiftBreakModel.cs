using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPShiftBreakModel : ERPBaseModel, IERPShiftBreakModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllShiftBreaks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPShiftBreakRepository iERPShiftBreakRepository = (base.ERPShiftBreakRepository = new ERPShiftBreakRepository(base.ApiClientContext));
		using (iERPShiftBreakRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPShiftBreakRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPShiftBreakRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPShiftBreakRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPShiftBreakRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetShiftBreak(Guid shiftBreakId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShiftBreakRepository iERPShiftBreakRepository = (base.ERPShiftBreakRepository = new ERPShiftBreakRepository(base.ApiClientContext));
		using (iERPShiftBreakRepository)
		{
			if (!(await base.ERPShiftBreakRepository.DoesShiftBreakExist(shiftBreakId)))
			{
				errorsList.Add($"ShiftBreak [{shiftBreakId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPShiftBreakDto>>> Process_GetAllShiftBreaks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPShiftBreakDto> allShiftBreaksDto = new List<ERPShiftBreakDto>();
		ERPResponseMessageDto<IList<ERPShiftBreakDto>> result;
		try
		{
			IERPShiftBreakRepository iERPShiftBreakRepository = (base.ERPShiftBreakRepository = new ERPShiftBreakRepository(base.ApiClientContext));
			using (iERPShiftBreakRepository)
			{
				foreach (ERPShiftBreakInformationDto item2 in await base.ERPShiftBreakRepository.GetAllShiftBreaks(pageSize, pageNumber, filter, orderBy))
				{
					ERPShiftBreakDto item = new ERPShiftBreakDto
					{
						lmtBreak1EndTime = item2.lmtBreak1EndTime,
						lmtBreak1StartTime = item2.lmtBreak1StartTime,
						lmtBreak2EndTime = item2.lmtBreak2EndTime,
						lmtBreak2StartTime = item2.lmtBreak2StartTime,
						lmtBreak3EndTime = item2.lmtBreak3EndTime,
						lmtBreak3StartTime = item2.lmtBreak3StartTime,
						lmtCreatedBy = item2.lmtCreatedBy,
						lmtCreatedDate = item2.lmtCreatedDate,
						lmtDay = item2.lmtDay,
						lmtEndTime = item2.lmtEndTime,
						lmtUniqueID = item2.lmtUniqueID,
						lmtBreak1Paid = item2.lmtBreak1Paid,
						lmtBreak2Paid = item2.lmtBreak2Paid,
						lmtBreak3Paid = item2.lmtBreak3Paid,
						lmtRowVersion = item2.lmtRowVersion,
						lmtShiftID = item2.lmtShiftID,
						lmtStartTime = item2.lmtStartTime,
						CustomFields = item2.CustomFields
					};
					allShiftBreaksDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ShiftBreaks]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPShiftBreakDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allShiftBreaksDto,
				RecordCount = allShiftBreaksDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShiftBreakDto>> Process_GetShiftBreak(Guid shiftBreakId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPShiftBreakDto shiftBreakDto = null;
		ERPResponseMessageDto<ERPShiftBreakDto> result;
		try
		{
			IERPShiftBreakRepository iERPShiftBreakRepository = (base.ERPShiftBreakRepository = new ERPShiftBreakRepository(base.ApiClientContext));
			using (iERPShiftBreakRepository)
			{
				ERPShiftBreakInformationDto eRPShiftBreakInformationDto = await base.ERPShiftBreakRepository.GetShiftBreak(shiftBreakId);
				shiftBreakDto = new ERPShiftBreakDto
				{
					lmtBreak1EndTime = eRPShiftBreakInformationDto.lmtBreak1EndTime,
					lmtBreak1StartTime = eRPShiftBreakInformationDto.lmtBreak1StartTime,
					lmtBreak2EndTime = eRPShiftBreakInformationDto.lmtBreak2EndTime,
					lmtBreak2StartTime = eRPShiftBreakInformationDto.lmtBreak2StartTime,
					lmtBreak3EndTime = eRPShiftBreakInformationDto.lmtBreak3EndTime,
					lmtBreak3StartTime = eRPShiftBreakInformationDto.lmtBreak3StartTime,
					lmtCreatedBy = eRPShiftBreakInformationDto.lmtCreatedBy,
					lmtCreatedDate = eRPShiftBreakInformationDto.lmtCreatedDate,
					lmtDay = eRPShiftBreakInformationDto.lmtDay,
					lmtEndTime = eRPShiftBreakInformationDto.lmtEndTime,
					lmtUniqueID = eRPShiftBreakInformationDto.lmtUniqueID,
					lmtBreak1Paid = eRPShiftBreakInformationDto.lmtBreak1Paid,
					lmtBreak2Paid = eRPShiftBreakInformationDto.lmtBreak2Paid,
					lmtBreak3Paid = eRPShiftBreakInformationDto.lmtBreak3Paid,
					lmtRowVersion = eRPShiftBreakInformationDto.lmtRowVersion,
					lmtShiftID = eRPShiftBreakInformationDto.lmtShiftID,
					lmtStartTime = eRPShiftBreakInformationDto.lmtStartTime,
					CustomFields = eRPShiftBreakInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ShiftBreaks []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShiftBreakDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = shiftBreakDto
			};
		}
		return result;
	}
}
