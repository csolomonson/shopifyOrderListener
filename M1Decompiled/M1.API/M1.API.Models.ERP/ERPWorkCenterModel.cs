using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPWorkCenterModel : ERPBaseModel, IERPWorkCenterModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllWorkCenters(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPWorkCenterRepository iERPWorkCenterRepository = (base.ERPWorkCenterRepository = new ERPWorkCenterRepository(base.ApiClientContext));
		using (iERPWorkCenterRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPWorkCenterRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPWorkCenterRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPWorkCenterRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPWorkCenterRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetWorkCenter(Guid workCenterId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWorkCenterRepository iERPWorkCenterRepository = (base.ERPWorkCenterRepository = new ERPWorkCenterRepository(base.ApiClientContext));
		using (iERPWorkCenterRepository)
		{
			if (!(await base.ERPWorkCenterRepository.DoesWorkCenterExist(workCenterId)))
			{
				errorsList.Add($"WorkCenter [{workCenterId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutWorkCenter(ERPWorkCenterDto workCenter)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWorkCenterRepository iERPWorkCenterRepository = (base.ERPWorkCenterRepository = new ERPWorkCenterRepository(base.ApiClientContext));
		using (iERPWorkCenterRepository)
		{
			if (!string.IsNullOrWhiteSpace(workCenter.xawPlantID) && !(await base.ERPWorkCenterRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { workCenter.xawPlantID })))
			{
				errorsList.Add("xawPlantID [" + workCenter.xawPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(workCenter.xawProductionDepartmentID) && !(await base.ERPWorkCenterRepository.DoesRecordExistInTableUsingKeys("ProductionDepartments", new object[1] { "XAEPRODUCTIONDEPARTMENTID" }, new object[1] { workCenter.xawProductionDepartmentID })))
			{
				errorsList.Add("xawProductionDepartmentID [" + workCenter.xawProductionDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(workCenter.xawProcessID) && !(await base.ERPWorkCenterRepository.DoesRecordExistInTableUsingKeys("Processes", new object[1] { "XACPROCESSID" }, new object[1] { workCenter.xawProcessID })))
			{
				errorsList.Add("xawProcessID [" + workCenter.xawProcessID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPWorkCenterDto>>> Process_GetAllWorkCenters(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPWorkCenterDto> allWorkCentersDto = new List<ERPWorkCenterDto>();
		ERPResponseMessageDto<IList<ERPWorkCenterDto>> result;
		try
		{
			IERPWorkCenterRepository iERPWorkCenterRepository = (base.ERPWorkCenterRepository = new ERPWorkCenterRepository(base.ApiClientContext));
			using (iERPWorkCenterRepository)
			{
				foreach (ERPWorkCenterInformationDto item2 in await base.ERPWorkCenterRepository.GetAllWorkCenters(pageSize, pageNumber, filter, orderBy))
				{
					ERPWorkCenterDto item = new ERPWorkCenterDto
					{
						xawCalendarColor = item2.xawCalendarColor,
						xawCalendarLocation = item2.xawCalendarLocation,
						xawWorkCenterID = item2.xawWorkCenterID,
						xawCreatedBy = item2.xawCreatedBy,
						xawCreatedDate = item2.xawCreatedDate,
						xawDayStartTimeFri = item2.xawDayStartTimeFri,
						xawDayStartTimeMon = item2.xawDayStartTimeMon,
						xawDayStartTimeSat = item2.xawDayStartTimeSat,
						xawDayStartTimeSun = item2.xawDayStartTimeSun,
						xawDayStartTimeThu = item2.xawDayStartTimeThu,
						xawDayStartTimeTue = item2.xawDayStartTimeTue,
						xawDayStartTimeWed = item2.xawDayStartTimeWed,
						xawDescription = item2.xawDescription,
						xawUniqueID = item2.xawUniqueID,
						xawFiniteTolerance = item2.xawFiniteTolerance,
						xawHoursFri = item2.xawHoursFri,
						xawHoursMon = item2.xawHoursMon,
						xawHoursSat = item2.xawHoursSat,
						xawHoursSun = item2.xawHoursSun,
						xawHoursThu = item2.xawHoursThu,
						xawHoursTue = item2.xawHoursTue,
						xawHoursWed = item2.xawHoursWed,
						xawInactiveDate = item2.xawInactiveDate,
						xawInactive = item2.xawInactive,
						xawEnableCalendar = item2.xawEnableCalendar,
						xawExcludeFromShopLoad = item2.xawExcludeFromShopLoad,
						xawExportToCalendar = item2.xawExportToCalendar,
						xawInfiniteCapacity = item2.xawInfiniteCapacity,
						xawOutsideProcessing = item2.xawOutsideProcessing,
						xawSetMachineToLaborHours = item2.xawSetMachineToLaborHours,
						xawSplitMachineHours = item2.xawSplitMachineHours,
						xawMoveTime = item2.xawMoveTime,
						xawNumberOfMachines = item2.xawNumberOfMachines,
						xawOverheadCalculationType = item2.xawOverheadCalculationType,
						xawOverheadRate = item2.xawOverheadRate,
						xawPeoplePerMachineProd = item2.xawPeoplePerMachineProd,
						xawPeoplePerMachineSetup = item2.xawPeoplePerMachineSetup,
						xawPlantID = item2.xawPlantID,
						xawProcessID = item2.xawProcessID,
						xawProductionDepartmentID = item2.xawProductionDepartmentID,
						xawProductionStandard = item2.xawProductionStandard,
						xawQueueTime = item2.xawQueueTime,
						xawQuotingRate = item2.xawQuotingRate,
						xawRowVersion = item2.xawRowVersion,
						xawSetupHours = item2.xawSetupHours,
						xawStandardFactor = item2.xawStandardFactor,
						xawStartHour = item2.xawStartHour,
						CustomFields = item2.CustomFields
					};
					allWorkCentersDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all WorkCenters]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPWorkCenterDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allWorkCentersDto,
				RecordCount = allWorkCentersDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWorkCenterDto>> Process_GetWorkCenter(Guid workCenterId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPWorkCenterDto workCenterDto = null;
		ERPResponseMessageDto<ERPWorkCenterDto> result;
		try
		{
			IERPWorkCenterRepository iERPWorkCenterRepository = (base.ERPWorkCenterRepository = new ERPWorkCenterRepository(base.ApiClientContext));
			using (iERPWorkCenterRepository)
			{
				ERPWorkCenterInformationDto eRPWorkCenterInformationDto = await base.ERPWorkCenterRepository.GetWorkCenter(workCenterId);
				workCenterDto = new ERPWorkCenterDto
				{
					xawCalendarColor = eRPWorkCenterInformationDto.xawCalendarColor,
					xawCalendarLocation = eRPWorkCenterInformationDto.xawCalendarLocation,
					xawWorkCenterID = eRPWorkCenterInformationDto.xawWorkCenterID,
					xawCreatedBy = eRPWorkCenterInformationDto.xawCreatedBy,
					xawCreatedDate = eRPWorkCenterInformationDto.xawCreatedDate,
					xawDayStartTimeFri = eRPWorkCenterInformationDto.xawDayStartTimeFri,
					xawDayStartTimeMon = eRPWorkCenterInformationDto.xawDayStartTimeMon,
					xawDayStartTimeSat = eRPWorkCenterInformationDto.xawDayStartTimeSat,
					xawDayStartTimeSun = eRPWorkCenterInformationDto.xawDayStartTimeSun,
					xawDayStartTimeThu = eRPWorkCenterInformationDto.xawDayStartTimeThu,
					xawDayStartTimeTue = eRPWorkCenterInformationDto.xawDayStartTimeTue,
					xawDayStartTimeWed = eRPWorkCenterInformationDto.xawDayStartTimeWed,
					xawDescription = eRPWorkCenterInformationDto.xawDescription,
					xawUniqueID = eRPWorkCenterInformationDto.xawUniqueID,
					xawFiniteTolerance = eRPWorkCenterInformationDto.xawFiniteTolerance,
					xawHoursFri = eRPWorkCenterInformationDto.xawHoursFri,
					xawHoursMon = eRPWorkCenterInformationDto.xawHoursMon,
					xawHoursSat = eRPWorkCenterInformationDto.xawHoursSat,
					xawHoursSun = eRPWorkCenterInformationDto.xawHoursSun,
					xawHoursThu = eRPWorkCenterInformationDto.xawHoursThu,
					xawHoursTue = eRPWorkCenterInformationDto.xawHoursTue,
					xawHoursWed = eRPWorkCenterInformationDto.xawHoursWed,
					xawInactiveDate = eRPWorkCenterInformationDto.xawInactiveDate,
					xawInactive = eRPWorkCenterInformationDto.xawInactive,
					xawEnableCalendar = eRPWorkCenterInformationDto.xawEnableCalendar,
					xawExcludeFromShopLoad = eRPWorkCenterInformationDto.xawExcludeFromShopLoad,
					xawExportToCalendar = eRPWorkCenterInformationDto.xawExportToCalendar,
					xawInfiniteCapacity = eRPWorkCenterInformationDto.xawInfiniteCapacity,
					xawOutsideProcessing = eRPWorkCenterInformationDto.xawOutsideProcessing,
					xawSetMachineToLaborHours = eRPWorkCenterInformationDto.xawSetMachineToLaborHours,
					xawSplitMachineHours = eRPWorkCenterInformationDto.xawSplitMachineHours,
					xawMoveTime = eRPWorkCenterInformationDto.xawMoveTime,
					xawNumberOfMachines = eRPWorkCenterInformationDto.xawNumberOfMachines,
					xawOverheadCalculationType = eRPWorkCenterInformationDto.xawOverheadCalculationType,
					xawOverheadRate = eRPWorkCenterInformationDto.xawOverheadRate,
					xawPeoplePerMachineProd = eRPWorkCenterInformationDto.xawPeoplePerMachineProd,
					xawPeoplePerMachineSetup = eRPWorkCenterInformationDto.xawPeoplePerMachineSetup,
					xawPlantID = eRPWorkCenterInformationDto.xawPlantID,
					xawProcessID = eRPWorkCenterInformationDto.xawProcessID,
					xawProductionDepartmentID = eRPWorkCenterInformationDto.xawProductionDepartmentID,
					xawProductionStandard = eRPWorkCenterInformationDto.xawProductionStandard,
					xawQueueTime = eRPWorkCenterInformationDto.xawQueueTime,
					xawQuotingRate = eRPWorkCenterInformationDto.xawQuotingRate,
					xawRowVersion = eRPWorkCenterInformationDto.xawRowVersion,
					xawSetupHours = eRPWorkCenterInformationDto.xawSetupHours,
					xawStandardFactor = eRPWorkCenterInformationDto.xawStandardFactor,
					xawStartHour = eRPWorkCenterInformationDto.xawStartHour,
					CustomFields = eRPWorkCenterInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the WorkCenters []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWorkCenterDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = workCenterDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPWorkCenterDto>> Process_PutWorkCenter(ERPWorkCenterDto workCenter)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPWorkCenterDto createdObject = null;
		ERPResponseMessageDto<ERPWorkCenterDto> result;
		try
		{
			IERPWorkCenterRepository iERPWorkCenterRepository = (base.ERPWorkCenterRepository = new ERPWorkCenterRepository(base.ApiClientContext));
			using (iERPWorkCenterRepository)
			{
				APIValidationInfoDto postResult = await base.ERPWorkCenterRepository.SaveWorkCenter(workCenter);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPWorkCenterInformationDto eRPWorkCenterInformationDto = await base.ERPWorkCenterRepository.GetWorkCenter(workCenter.xawUniqueID);
					createdObject = new ERPWorkCenterDto
					{
						xawCalendarColor = eRPWorkCenterInformationDto.xawCalendarColor,
						xawCalendarLocation = eRPWorkCenterInformationDto.xawCalendarLocation,
						xawWorkCenterID = eRPWorkCenterInformationDto.xawWorkCenterID,
						xawCreatedBy = eRPWorkCenterInformationDto.xawCreatedBy,
						xawCreatedDate = eRPWorkCenterInformationDto.xawCreatedDate,
						xawDayStartTimeFri = eRPWorkCenterInformationDto.xawDayStartTimeFri,
						xawDayStartTimeMon = eRPWorkCenterInformationDto.xawDayStartTimeMon,
						xawDayStartTimeSat = eRPWorkCenterInformationDto.xawDayStartTimeSat,
						xawDayStartTimeSun = eRPWorkCenterInformationDto.xawDayStartTimeSun,
						xawDayStartTimeThu = eRPWorkCenterInformationDto.xawDayStartTimeThu,
						xawDayStartTimeTue = eRPWorkCenterInformationDto.xawDayStartTimeTue,
						xawDayStartTimeWed = eRPWorkCenterInformationDto.xawDayStartTimeWed,
						xawDescription = eRPWorkCenterInformationDto.xawDescription,
						xawUniqueID = eRPWorkCenterInformationDto.xawUniqueID,
						xawFiniteTolerance = eRPWorkCenterInformationDto.xawFiniteTolerance,
						xawHoursFri = eRPWorkCenterInformationDto.xawHoursFri,
						xawHoursMon = eRPWorkCenterInformationDto.xawHoursMon,
						xawHoursSat = eRPWorkCenterInformationDto.xawHoursSat,
						xawHoursSun = eRPWorkCenterInformationDto.xawHoursSun,
						xawHoursThu = eRPWorkCenterInformationDto.xawHoursThu,
						xawHoursTue = eRPWorkCenterInformationDto.xawHoursTue,
						xawHoursWed = eRPWorkCenterInformationDto.xawHoursWed,
						xawInactiveDate = eRPWorkCenterInformationDto.xawInactiveDate,
						xawInactive = eRPWorkCenterInformationDto.xawInactive,
						xawEnableCalendar = eRPWorkCenterInformationDto.xawEnableCalendar,
						xawExcludeFromShopLoad = eRPWorkCenterInformationDto.xawExcludeFromShopLoad,
						xawExportToCalendar = eRPWorkCenterInformationDto.xawExportToCalendar,
						xawInfiniteCapacity = eRPWorkCenterInformationDto.xawInfiniteCapacity,
						xawOutsideProcessing = eRPWorkCenterInformationDto.xawOutsideProcessing,
						xawSetMachineToLaborHours = eRPWorkCenterInformationDto.xawSetMachineToLaborHours,
						xawSplitMachineHours = eRPWorkCenterInformationDto.xawSplitMachineHours,
						xawMoveTime = eRPWorkCenterInformationDto.xawMoveTime,
						xawNumberOfMachines = eRPWorkCenterInformationDto.xawNumberOfMachines,
						xawOverheadCalculationType = eRPWorkCenterInformationDto.xawOverheadCalculationType,
						xawOverheadRate = eRPWorkCenterInformationDto.xawOverheadRate,
						xawPeoplePerMachineProd = eRPWorkCenterInformationDto.xawPeoplePerMachineProd,
						xawPeoplePerMachineSetup = eRPWorkCenterInformationDto.xawPeoplePerMachineSetup,
						xawPlantID = eRPWorkCenterInformationDto.xawPlantID,
						xawProcessID = eRPWorkCenterInformationDto.xawProcessID,
						xawProductionDepartmentID = eRPWorkCenterInformationDto.xawProductionDepartmentID,
						xawProductionStandard = eRPWorkCenterInformationDto.xawProductionStandard,
						xawQueueTime = eRPWorkCenterInformationDto.xawQueueTime,
						xawQuotingRate = eRPWorkCenterInformationDto.xawQuotingRate,
						xawRowVersion = eRPWorkCenterInformationDto.xawRowVersion,
						xawSetupHours = eRPWorkCenterInformationDto.xawSetupHours,
						xawStandardFactor = eRPWorkCenterInformationDto.xawStandardFactor,
						xawStartHour = eRPWorkCenterInformationDto.xawStartHour,
						CustomFields = eRPWorkCenterInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing WorkCenter [{workCenter.xawUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWorkCenterDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteWorkCenter(Guid workCenterId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPWorkCenterRepository iERPWorkCenterRepository = (base.ERPWorkCenterRepository = new ERPWorkCenterRepository(base.ApiClientContext));
		using (iERPWorkCenterRepository)
		{
			if (!(await base.ERPWorkCenterRepository.DoesWorkCenterExist(workCenterId)))
			{
				base.ErrorsList.Add($"WorkCenter [{workCenterId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPWorkCenterInformationDto eRPWorkCenterInformationDto = await base.ERPWorkCenterRepository.GetWorkCenter(workCenterId);
				string text = await base.ERPWorkCenterRepository.WhereUsed("WorkCenters", new object[1] { eRPWorkCenterInformationDto.xawWorkCenterID }, new object[1] { "xawWorkCenterID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("WorkCenter cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPWorkCenterDto>> Process_DeleteWorkCenter(Guid workCenterId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPWorkCenterDto> result;
		try
		{
			IERPWorkCenterRepository iERPWorkCenterRepository = (base.ERPWorkCenterRepository = new ERPWorkCenterRepository(base.ApiClientContext));
			using (iERPWorkCenterRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPWorkCenterRepository.DeleteRowFromTable("WorkCenters", "xaw", workCenterId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of WorkCenter [{workCenterId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPWorkCenterDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPWorkCenterDto()
			};
		}
		return result;
	}
}
