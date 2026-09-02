using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPProductionCalendarModel : ERPBaseModel, IERPProductionCalendarModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllProductionCalendars(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProductionCalendarRepository iERPProductionCalendarRepository = (base.ERPProductionCalendarRepository = new ERPProductionCalendarRepository(base.ApiClientContext));
		using (iERPProductionCalendarRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPProductionCalendarRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPProductionCalendarRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPProductionCalendarRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPProductionCalendarRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetProductionCalendar(Guid productionCalendarId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProductionCalendarRepository iERPProductionCalendarRepository = (base.ERPProductionCalendarRepository = new ERPProductionCalendarRepository(base.ApiClientContext));
		using (iERPProductionCalendarRepository)
		{
			if (!(await base.ERPProductionCalendarRepository.DoesProductionCalendarExist(productionCalendarId)))
			{
				errorsList.Add($"ProductionCalendar [{productionCalendarId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPProductionCalendarDto>>> Process_GetAllProductionCalendars(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPProductionCalendarDto> allProductionCalendarsDto = new List<ERPProductionCalendarDto>();
		ERPResponseMessageDto<IList<ERPProductionCalendarDto>> result;
		try
		{
			IERPProductionCalendarRepository iERPProductionCalendarRepository = (base.ERPProductionCalendarRepository = new ERPProductionCalendarRepository(base.ApiClientContext));
			using (iERPProductionCalendarRepository)
			{
				foreach (ERPProductionCalendarInformationDto item2 in await base.ERPProductionCalendarRepository.GetAllProductionCalendars(pageSize, pageNumber, filter, orderBy))
				{
					ERPProductionCalendarDto item = new ERPProductionCalendarDto
					{
						jmlCreatedBy = item2.jmlCreatedBy,
						jmlCreatedDate = item2.jmlCreatedDate,
						jmlUniqueID = item2.jmlUniqueID,
						jmlPlantID = item2.jmlPlantID,
						jmlProductionCalendarYearID = item2.jmlProductionCalendarYearID,
						jmlRowVersion = item2.jmlRowVersion,
						jmlWorkCenterID = item2.jmlWorkCenterID,
						CustomFields = item2.CustomFields
					};
					allProductionCalendarsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ProductionCalendars]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPProductionCalendarDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allProductionCalendarsDto,
				RecordCount = allProductionCalendarsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProductionCalendarDto>> Process_GetProductionCalendar(Guid productionCalendarId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPProductionCalendarDto productionCalendarDto = null;
		ERPResponseMessageDto<ERPProductionCalendarDto> result;
		try
		{
			IERPProductionCalendarRepository iERPProductionCalendarRepository = (base.ERPProductionCalendarRepository = new ERPProductionCalendarRepository(base.ApiClientContext));
			using (iERPProductionCalendarRepository)
			{
				ERPProductionCalendarInformationDto eRPProductionCalendarInformationDto = await base.ERPProductionCalendarRepository.GetProductionCalendar(productionCalendarId);
				productionCalendarDto = new ERPProductionCalendarDto
				{
					jmlCreatedBy = eRPProductionCalendarInformationDto.jmlCreatedBy,
					jmlCreatedDate = eRPProductionCalendarInformationDto.jmlCreatedDate,
					jmlUniqueID = eRPProductionCalendarInformationDto.jmlUniqueID,
					jmlPlantID = eRPProductionCalendarInformationDto.jmlPlantID,
					jmlProductionCalendarYearID = eRPProductionCalendarInformationDto.jmlProductionCalendarYearID,
					jmlRowVersion = eRPProductionCalendarInformationDto.jmlRowVersion,
					jmlWorkCenterID = eRPProductionCalendarInformationDto.jmlWorkCenterID,
					CustomFields = eRPProductionCalendarInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ProductionCalendars []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProductionCalendarDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = productionCalendarDto
			};
		}
		return result;
	}
}
