using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core;

namespace M1.API.Models.BOM;

public class BOMTimecardLineModel : BOMBaseModel, IBOMTimecardLineModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetTimecardLine(string timecardId, string timecardLineId)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		using (TimecardLineRepository timecardLineRepository = new TimecardLineRepository(base.ApiClientContext))
		{
			if (!timecardLineRepository.DoesTimecardLineExists(timecardId, timecardLineId).Result)
			{
				list.Add("Timecard [" + timecardId + "], containing TimecardLine [" + timecardLineId + "], is invalid");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<BOMResponseMessageDto<IList<BOMTimecardLineDto>>> Process_GetAllTimecardLines(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMTimecardLineDto> allTimecardLinesDto = new List<BOMTimecardLineDto>();
		BOMResponseMessageDto<IList<BOMTimecardLineDto>> result;
		try
		{
			using TimecardLineRepository timecardLineRepository = new TimecardLineRepository(base.ApiClientContext);
			foreach (BOMTimecardLineDto item2 in await timecardLineRepository.GetAllTimecardLines(pageSize, pageNumber))
			{
				BOMTimecardLineDto item = new BOMTimecardLineDto
				{
					TimecardID = item2.TimecardID,
					TimecardLineID = item2.TimecardLineID,
					JobID = item2.JobID,
					JobAssemblyID = item2.JobAssemblyID,
					JobOperationID = item2.JobOperationID,
					WorkCenterID = item2.WorkCenterID,
					ProcessID = item2.ProcessID,
					CompletionType = item2.CompletionType,
					WorkType = item2.WorkType,
					GoodQuantity = item2.GoodQuantity,
					ScrapQuantity = item2.ScrapQuantity,
					ReworkQuantity = item2.ReworkQuantity,
					ActualStartTime = item2.ActualStartTime,
					ActualEndTime = item2.ActualEndTime,
					EmployeeID = item2.EmployeeID,
					MachineHours = item2.MachineHours,
					LaborHours = item2.LaborHours,
					TimecardType = item2.TimecardType,
					UniqueID = item2.UniqueID,
					RowVersion = item2.RowVersion
				};
				allTimecardLinesDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all TimecardLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMTimecardLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allTimecardLinesDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMTimecardLineDto>> Process_GetTimecardLine(string timecardId, string timecardLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMTimecardLineDto timecardLineDto = null;
		BOMResponseMessageDto<BOMTimecardLineDto> result;
		try
		{
			using TimecardLineRepository timecardLineRepository = new TimecardLineRepository(base.ApiClientContext);
			BOMTimecardLineDto bOMTimecardLineDto = await timecardLineRepository.GetTimecardLine(timecardId, timecardLineId);
			timecardLineDto = new BOMTimecardLineDto
			{
				TimecardID = bOMTimecardLineDto.TimecardID,
				TimecardLineID = bOMTimecardLineDto.TimecardLineID,
				JobID = bOMTimecardLineDto.JobID,
				JobAssemblyID = bOMTimecardLineDto.JobAssemblyID,
				JobOperationID = bOMTimecardLineDto.JobOperationID,
				WorkCenterID = bOMTimecardLineDto.WorkCenterID,
				ProcessID = bOMTimecardLineDto.ProcessID,
				CompletionType = bOMTimecardLineDto.CompletionType,
				WorkType = bOMTimecardLineDto.WorkType,
				GoodQuantity = bOMTimecardLineDto.GoodQuantity,
				ScrapQuantity = bOMTimecardLineDto.ScrapQuantity,
				ReworkQuantity = bOMTimecardLineDto.ReworkQuantity,
				ActualStartTime = bOMTimecardLineDto.ActualStartTime,
				ActualEndTime = bOMTimecardLineDto.ActualEndTime,
				EmployeeID = bOMTimecardLineDto.EmployeeID,
				MachineHours = bOMTimecardLineDto.MachineHours,
				LaborHours = bOMTimecardLineDto.LaborHours,
				TimecardType = bOMTimecardLineDto.TimecardType,
				UniqueID = bOMTimecardLineDto.UniqueID,
				RowVersion = bOMTimecardLineDto.RowVersion
			};
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the TimecardLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMTimecardLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = timecardLineDto
			};
		}
		return result;
	}
}
