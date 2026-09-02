using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPProductionCalendarWorkCenterModel : ERPBaseModel, IERPProductionCalendarWorkCenterModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllProductionCalendarWorkCenters(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProductionCalendarWorkCenterRepository iERPProductionCalendarWorkCenterRepository = (base.ERPProductionCalendarWorkCenterRepository = new ERPProductionCalendarWorkCenterRepository(base.ApiClientContext));
		using (iERPProductionCalendarWorkCenterRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPProductionCalendarWorkCenterRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPProductionCalendarWorkCenterRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPProductionCalendarWorkCenterRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPProductionCalendarWorkCenterRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetProductionCalendarWorkCenter(Guid productionCalendarWorkCenterId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProductionCalendarWorkCenterRepository iERPProductionCalendarWorkCenterRepository = (base.ERPProductionCalendarWorkCenterRepository = new ERPProductionCalendarWorkCenterRepository(base.ApiClientContext));
		using (iERPProductionCalendarWorkCenterRepository)
		{
			if (!(await base.ERPProductionCalendarWorkCenterRepository.DoesProductionCalendarWorkCenterExist(productionCalendarWorkCenterId)))
			{
				errorsList.Add($"ProductionCalendarWorkCenter [{productionCalendarWorkCenterId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPProductionCalendarWorkCenterDto>>> Process_GetAllProductionCalendarWorkCenters(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPProductionCalendarWorkCenterDto> allProductionCalendarWorkCentersDto = new List<ERPProductionCalendarWorkCenterDto>();
		ERPResponseMessageDto<IList<ERPProductionCalendarWorkCenterDto>> result;
		try
		{
			IERPProductionCalendarWorkCenterRepository iERPProductionCalendarWorkCenterRepository = (base.ERPProductionCalendarWorkCenterRepository = new ERPProductionCalendarWorkCenterRepository(base.ApiClientContext));
			using (iERPProductionCalendarWorkCenterRepository)
			{
				foreach (ERPProductionCalendarWorkCenterInformationDto item2 in await base.ERPProductionCalendarWorkCenterRepository.GetAllProductionCalendarWorkCenters(pageSize, pageNumber, filter, orderBy))
				{
					ERPProductionCalendarWorkCenterDto item = new ERPProductionCalendarWorkCenterDto
					{
						jmrCreatedBy = item2.jmrCreatedBy,
						jmrCreatedDate = item2.jmrCreatedDate,
						jmrUniqueID = item2.jmrUniqueID,
						jmrProductionCalendarLineID = item2.jmrProductionCalendarLineID,
						jmrProductionCalendarYearID = item2.jmrProductionCalendarYearID,
						jmrRowVersion = item2.jmrRowVersion,
						jmrWorkCenterID = item2.jmrWorkCenterID,
						CustomFields = item2.CustomFields
					};
					allProductionCalendarWorkCentersDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ProductionCalendarWorkCenters]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPProductionCalendarWorkCenterDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allProductionCalendarWorkCentersDto,
				RecordCount = allProductionCalendarWorkCentersDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProductionCalendarWorkCenterDto>> Process_GetProductionCalendarWorkCenter(Guid productionCalendarWorkCenterId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPProductionCalendarWorkCenterDto productionCalendarWorkCenterDto = null;
		ERPResponseMessageDto<ERPProductionCalendarWorkCenterDto> result;
		try
		{
			IERPProductionCalendarWorkCenterRepository iERPProductionCalendarWorkCenterRepository = (base.ERPProductionCalendarWorkCenterRepository = new ERPProductionCalendarWorkCenterRepository(base.ApiClientContext));
			using (iERPProductionCalendarWorkCenterRepository)
			{
				ERPProductionCalendarWorkCenterInformationDto eRPProductionCalendarWorkCenterInformationDto = await base.ERPProductionCalendarWorkCenterRepository.GetProductionCalendarWorkCenter(productionCalendarWorkCenterId);
				productionCalendarWorkCenterDto = new ERPProductionCalendarWorkCenterDto
				{
					jmrCreatedBy = eRPProductionCalendarWorkCenterInformationDto.jmrCreatedBy,
					jmrCreatedDate = eRPProductionCalendarWorkCenterInformationDto.jmrCreatedDate,
					jmrUniqueID = eRPProductionCalendarWorkCenterInformationDto.jmrUniqueID,
					jmrProductionCalendarLineID = eRPProductionCalendarWorkCenterInformationDto.jmrProductionCalendarLineID,
					jmrProductionCalendarYearID = eRPProductionCalendarWorkCenterInformationDto.jmrProductionCalendarYearID,
					jmrRowVersion = eRPProductionCalendarWorkCenterInformationDto.jmrRowVersion,
					jmrWorkCenterID = eRPProductionCalendarWorkCenterInformationDto.jmrWorkCenterID,
					CustomFields = eRPProductionCalendarWorkCenterInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ProductionCalendarWorkCenters []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProductionCalendarWorkCenterDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = productionCalendarWorkCenterDto
			};
		}
		return result;
	}
}
