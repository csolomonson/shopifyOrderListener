using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core.Job;

namespace M1.API.Models.BOM.Job;

public class BOMTimecardModel : BOMBaseModel, IBOMTimecardModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetTimecard(string timecardId)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		using (TimecardRepository timecardRepository = new TimecardRepository(base.ApiClientContext))
		{
			if (!timecardRepository.DoesTimecardExistsAsync(timecardId).Result)
			{
				list.Add("Timecard [" + timecardId + "] is invalid");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetTimecard(string timecardId, string employeeId)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		using (TimecardRepository timecardRepository = new TimecardRepository(base.ApiClientContext))
		{
			if (!timecardRepository.DoesTimecardExistsAsync(timecardId, employeeId).Result)
			{
				list.Add("Timecard [" + timecardId + "] for [" + employeeId + "] is invalid");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<BOMResponseMessageDto<IList<BOMTimecardDto>>> Process_GetAllTimecards(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMTimecardDto> allTimecardsDto = new List<BOMTimecardDto>();
		BOMResponseMessageDto<IList<BOMTimecardDto>> result;
		try
		{
			using TimecardRepository timecardRepository = new TimecardRepository(base.ApiClientContext);
			foreach (BOMTimecardDto item2 in await timecardRepository.GetAllTimecards(pageSize, pageNumber))
			{
				BOMTimecardDto item = new BOMTimecardDto
				{
					EmployeeID = item2.EmployeeID,
					ShiftID = item2.ShiftID,
					TimecardDate = item2.TimecardDate,
					ActualStartTime = item2.ActualStartTime,
					ActualEndTime = item2.ActualEndTime,
					LastEndTime = item2.LastEndTime,
					PlantID = item2.PlantID,
					PlantDepartmentID = item2.PlantDepartmentID,
					PostedDate = item2.PostedDate,
					UniqueID = item2.UniqueID,
					RowVersion = item2.RowVersion
				};
				allTimecardsDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Timecards]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMTimecardDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allTimecardsDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMTimecardDto>> Process_GetTimecard(string timecardId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMTimecardDto timecardDto = null;
		BOMResponseMessageDto<BOMTimecardDto> result;
		try
		{
			using TimecardRepository timecardRepository = new TimecardRepository(base.ApiClientContext);
			BOMTimecardDto bOMTimecardDto = await timecardRepository.GetTimecard(timecardId);
			timecardDto = new BOMTimecardDto
			{
				EmployeeID = bOMTimecardDto.EmployeeID,
				ShiftID = bOMTimecardDto.ShiftID,
				TimecardDate = bOMTimecardDto.TimecardDate,
				ActualStartTime = bOMTimecardDto.ActualStartTime,
				ActualEndTime = bOMTimecardDto.ActualEndTime,
				LastEndTime = bOMTimecardDto.LastEndTime,
				PlantID = bOMTimecardDto.PlantID,
				PlantDepartmentID = bOMTimecardDto.PlantDepartmentID,
				PostedDate = bOMTimecardDto.PostedDate,
				UniqueID = bOMTimecardDto.UniqueID,
				RowVersion = bOMTimecardDto.RowVersion
			};
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Timecards []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMTimecardDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = timecardDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMTimecardDto>> Process_GetTimecard(string timecardId, string employeeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMTimecardDto timecardDto = null;
		BOMResponseMessageDto<BOMTimecardDto> result;
		try
		{
			using TimecardRepository timecardRepository = new TimecardRepository(base.ApiClientContext);
			BOMTimecardDto bOMTimecardDto = await timecardRepository.GetTimecard(timecardId, employeeId);
			timecardDto = new BOMTimecardDto
			{
				EmployeeID = bOMTimecardDto.EmployeeID,
				ShiftID = bOMTimecardDto.ShiftID,
				TimecardDate = bOMTimecardDto.TimecardDate,
				ActualStartTime = bOMTimecardDto.ActualStartTime,
				ActualEndTime = bOMTimecardDto.ActualEndTime,
				LastEndTime = bOMTimecardDto.LastEndTime,
				PlantID = bOMTimecardDto.PlantID,
				PlantDepartmentID = bOMTimecardDto.PlantDepartmentID,
				PostedDate = bOMTimecardDto.PostedDate,
				UniqueID = bOMTimecardDto.UniqueID,
				RowVersion = bOMTimecardDto.RowVersion
			};
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Timecards []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMTimecardDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = timecardDto
			};
		}
		return result;
	}
}
