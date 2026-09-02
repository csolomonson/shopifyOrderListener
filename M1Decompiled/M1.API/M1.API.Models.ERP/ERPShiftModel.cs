using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPShiftModel : ERPBaseModel, IERPShiftModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllShifts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPShiftRepository iERPShiftRepository = (base.ERPShiftRepository = new ERPShiftRepository(base.ApiClientContext));
		using (iERPShiftRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPShiftRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPShiftRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPShiftRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPShiftRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetShift(Guid shiftId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShiftRepository iERPShiftRepository = (base.ERPShiftRepository = new ERPShiftRepository(base.ApiClientContext));
		using (iERPShiftRepository)
		{
			if (!(await base.ERPShiftRepository.DoesShiftExist(shiftId)))
			{
				errorsList.Add($"Shift [{shiftId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPShiftDto>>> Process_GetAllShifts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPShiftDto> allShiftsDto = new List<ERPShiftDto>();
		ERPResponseMessageDto<IList<ERPShiftDto>> result;
		try
		{
			IERPShiftRepository iERPShiftRepository = (base.ERPShiftRepository = new ERPShiftRepository(base.ApiClientContext));
			using (iERPShiftRepository)
			{
				foreach (ERPShiftInformationDto item2 in await base.ERPShiftRepository.GetAllShifts(pageSize, pageNumber, filter, orderBy))
				{
					ERPShiftDto item = new ERPShiftDto
					{
						lmsAutoClockOutLastRunTime = item2.lmsAutoClockOutLastRunTime,
						lmsAutoClockOutTime = item2.lmsAutoClockOutTime,
						lmsClockInWindow = item2.lmsClockInWindow,
						lmsClockOutWindow = item2.lmsClockOutWindow,
						lmsCreatedBy = item2.lmsCreatedBy,
						lmsCreatedDate = item2.lmsCreatedDate,
						lmsDescription = item2.lmsDescription,
						lmsUniqueID = item2.lmsUniqueID,
						lmsGraceTimeIn = item2.lmsGraceTimeIn,
						lmsGraceTimeOut = item2.lmsGraceTimeOut,
						lmsIdleTimeIndirectLaborID = item2.lmsIdleTimeIndirectLaborID,
						lmsIdleTimeWorkCenterID = item2.lmsIdleTimeWorkCenterID,
						lmsInactiveDate = item2.lmsInactiveDate,
						lmsInactive = item2.lmsInactive,
						lmsRoundClockWithInShift = item2.lmsRoundClockWithInShift,
						lmsRoundJobsOutsideOfShift = item2.lmsRoundJobsOutsideOfShift,
						lmsRoundJobsWithinShift = item2.lmsRoundJobsWithinShift,
						lmsRoundOutsideOfShift = item2.lmsRoundOutsideOfShift,
						lmsPlantID = item2.lmsPlantID,
						lmsRoundClockInDirection = item2.lmsRoundClockInDirection,
						lmsRoundClockOutDirection = item2.lmsRoundClockOutDirection,
						lmsRoundTo = item2.lmsRoundTo,
						lmsRowVersion = item2.lmsRowVersion,
						lmsShiftID = item2.lmsShiftID,
						lmsShiftGroup = item2.lmsShiftGroup,
						CustomFields = item2.CustomFields
					};
					allShiftsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Shifts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPShiftDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allShiftsDto,
				RecordCount = allShiftsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShiftDto>> Process_GetShift(Guid shiftId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPShiftDto shiftDto = null;
		ERPResponseMessageDto<ERPShiftDto> result;
		try
		{
			IERPShiftRepository iERPShiftRepository = (base.ERPShiftRepository = new ERPShiftRepository(base.ApiClientContext));
			using (iERPShiftRepository)
			{
				ERPShiftInformationDto eRPShiftInformationDto = await base.ERPShiftRepository.GetShift(shiftId);
				shiftDto = new ERPShiftDto
				{
					lmsAutoClockOutLastRunTime = eRPShiftInformationDto.lmsAutoClockOutLastRunTime,
					lmsAutoClockOutTime = eRPShiftInformationDto.lmsAutoClockOutTime,
					lmsClockInWindow = eRPShiftInformationDto.lmsClockInWindow,
					lmsClockOutWindow = eRPShiftInformationDto.lmsClockOutWindow,
					lmsCreatedBy = eRPShiftInformationDto.lmsCreatedBy,
					lmsCreatedDate = eRPShiftInformationDto.lmsCreatedDate,
					lmsDescription = eRPShiftInformationDto.lmsDescription,
					lmsUniqueID = eRPShiftInformationDto.lmsUniqueID,
					lmsGraceTimeIn = eRPShiftInformationDto.lmsGraceTimeIn,
					lmsGraceTimeOut = eRPShiftInformationDto.lmsGraceTimeOut,
					lmsIdleTimeIndirectLaborID = eRPShiftInformationDto.lmsIdleTimeIndirectLaborID,
					lmsIdleTimeWorkCenterID = eRPShiftInformationDto.lmsIdleTimeWorkCenterID,
					lmsInactiveDate = eRPShiftInformationDto.lmsInactiveDate,
					lmsInactive = eRPShiftInformationDto.lmsInactive,
					lmsRoundClockWithInShift = eRPShiftInformationDto.lmsRoundClockWithInShift,
					lmsRoundJobsOutsideOfShift = eRPShiftInformationDto.lmsRoundJobsOutsideOfShift,
					lmsRoundJobsWithinShift = eRPShiftInformationDto.lmsRoundJobsWithinShift,
					lmsRoundOutsideOfShift = eRPShiftInformationDto.lmsRoundOutsideOfShift,
					lmsPlantID = eRPShiftInformationDto.lmsPlantID,
					lmsRoundClockInDirection = eRPShiftInformationDto.lmsRoundClockInDirection,
					lmsRoundClockOutDirection = eRPShiftInformationDto.lmsRoundClockOutDirection,
					lmsRoundTo = eRPShiftInformationDto.lmsRoundTo,
					lmsRowVersion = eRPShiftInformationDto.lmsRowVersion,
					lmsShiftID = eRPShiftInformationDto.lmsShiftID,
					lmsShiftGroup = eRPShiftInformationDto.lmsShiftGroup,
					CustomFields = eRPShiftInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Shifts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShiftDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = shiftDto
			};
		}
		return result;
	}
}
