using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPProductionCalendarDayModel : ERPBaseModel, IERPProductionCalendarDayModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllProductionCalendarDays(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProductionCalendarDayRepository iERPProductionCalendarDayRepository = (base.ERPProductionCalendarDayRepository = new ERPProductionCalendarDayRepository(base.ApiClientContext));
		using (iERPProductionCalendarDayRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPProductionCalendarDayRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPProductionCalendarDayRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPProductionCalendarDayRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPProductionCalendarDayRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetProductionCalendarDay(Guid productionCalendarDayId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProductionCalendarDayRepository iERPProductionCalendarDayRepository = (base.ERPProductionCalendarDayRepository = new ERPProductionCalendarDayRepository(base.ApiClientContext));
		using (iERPProductionCalendarDayRepository)
		{
			if (!(await base.ERPProductionCalendarDayRepository.DoesProductionCalendarDayExist(productionCalendarDayId)))
			{
				errorsList.Add($"ProductionCalendarDay [{productionCalendarDayId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPProductionCalendarDayDto>>> Process_GetAllProductionCalendarDays(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPProductionCalendarDayDto> allProductionCalendarDaysDto = new List<ERPProductionCalendarDayDto>();
		ERPResponseMessageDto<IList<ERPProductionCalendarDayDto>> result;
		try
		{
			IERPProductionCalendarDayRepository iERPProductionCalendarDayRepository = (base.ERPProductionCalendarDayRepository = new ERPProductionCalendarDayRepository(base.ApiClientContext));
			using (iERPProductionCalendarDayRepository)
			{
				foreach (ERPProductionCalendarDayInformationDto item2 in await base.ERPProductionCalendarDayRepository.GetAllProductionCalendarDays(pageSize, pageNumber, filter, orderBy))
				{
					ERPProductionCalendarDayDto item = new ERPProductionCalendarDayDto
					{
						jmyDayOfWeek = item2.jmyDayOfWeek,
						jmyDayStartTime = item2.jmyDayStartTime,
						jmyHours = item2.jmyHours,
						jmyHoliday = item2.jmyHoliday,
						jmyPlantID = item2.jmyPlantID,
						jmyProductionCalendarDay = item2.jmyProductionCalendarDay,
						jmyProductionCalendarMonth = item2.jmyProductionCalendarMonth,
						jmyProductionCalendarYearID = item2.jmyProductionCalendarYearID,
						jmyRowVersion = item2.jmyRowVersion,
						jmyWorkCenterID = item2.jmyWorkCenterID,
						CustomFields = item2.CustomFields
					};
					allProductionCalendarDaysDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ProductionCalendarDays]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPProductionCalendarDayDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allProductionCalendarDaysDto,
				RecordCount = allProductionCalendarDaysDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProductionCalendarDayDto>> Process_GetProductionCalendarDay(Guid productionCalendarDayId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPProductionCalendarDayDto productionCalendarDayDto = null;
		ERPResponseMessageDto<ERPProductionCalendarDayDto> result;
		try
		{
			IERPProductionCalendarDayRepository iERPProductionCalendarDayRepository = (base.ERPProductionCalendarDayRepository = new ERPProductionCalendarDayRepository(base.ApiClientContext));
			using (iERPProductionCalendarDayRepository)
			{
				ERPProductionCalendarDayInformationDto eRPProductionCalendarDayInformationDto = await base.ERPProductionCalendarDayRepository.GetProductionCalendarDay(productionCalendarDayId);
				productionCalendarDayDto = new ERPProductionCalendarDayDto
				{
					jmyDayOfWeek = eRPProductionCalendarDayInformationDto.jmyDayOfWeek,
					jmyDayStartTime = eRPProductionCalendarDayInformationDto.jmyDayStartTime,
					jmyHours = eRPProductionCalendarDayInformationDto.jmyHours,
					jmyHoliday = eRPProductionCalendarDayInformationDto.jmyHoliday,
					jmyPlantID = eRPProductionCalendarDayInformationDto.jmyPlantID,
					jmyProductionCalendarDay = eRPProductionCalendarDayInformationDto.jmyProductionCalendarDay,
					jmyProductionCalendarMonth = eRPProductionCalendarDayInformationDto.jmyProductionCalendarMonth,
					jmyProductionCalendarYearID = eRPProductionCalendarDayInformationDto.jmyProductionCalendarYearID,
					jmyRowVersion = eRPProductionCalendarDayInformationDto.jmyRowVersion,
					jmyWorkCenterID = eRPProductionCalendarDayInformationDto.jmyWorkCenterID,
					CustomFields = eRPProductionCalendarDayInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ProductionCalendarDays []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProductionCalendarDayDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = productionCalendarDayDto
			};
		}
		return result;
	}
}
