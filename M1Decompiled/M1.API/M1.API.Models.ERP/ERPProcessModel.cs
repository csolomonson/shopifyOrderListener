using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPProcessModel : ERPBaseModel, IERPProcessModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllProcesses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProcessRepository iERPProcessRepository = (base.ERPProcessRepository = new ERPProcessRepository(base.ApiClientContext));
		using (iERPProcessRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPProcessRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPProcessRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPProcessRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPProcessRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetProcess(Guid processId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProcessRepository iERPProcessRepository = (base.ERPProcessRepository = new ERPProcessRepository(base.ApiClientContext));
		using (iERPProcessRepository)
		{
			if (!(await base.ERPProcessRepository.DoesProcessExist(processId)))
			{
				errorsList.Add($"Process [{processId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutProcess(ERPProcessDto process)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPProcessRepository iERPProcessRepository = (base.ERPProcessRepository = new ERPProcessRepository(base.ApiClientContext));
		using (iERPProcessRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPProcessDto>>> Process_GetAllProcesses(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPProcessDto> allProcessesDto = new List<ERPProcessDto>();
		ERPResponseMessageDto<IList<ERPProcessDto>> result;
		try
		{
			IERPProcessRepository iERPProcessRepository = (base.ERPProcessRepository = new ERPProcessRepository(base.ApiClientContext));
			using (iERPProcessRepository)
			{
				foreach (ERPProcessInformationDto item2 in await base.ERPProcessRepository.GetAllProcesses(pageSize, pageNumber, filter, orderBy))
				{
					ERPProcessDto item = new ERPProcessDto
					{
						xacProcessID = item2.xacProcessID,
						xacCreatedBy = item2.xacCreatedBy,
						xacCreatedDate = item2.xacCreatedDate,
						xacUniqueID = item2.xacUniqueID,
						xacInactiveDate = item2.xacInactiveDate,
						xacInspectionType = item2.xacInspectionType,
						xacInactive = item2.xacInactive,
						xacExcludeFromTMJobs = item2.xacExcludeFromTMJobs,
						xacIgnoreCalendarMove = item2.xacIgnoreCalendarMove,
						xacIgnoreCalendarQueue = item2.xacIgnoreCalendarQueue,
						xacPrintInspectionLine = item2.xacPrintInspectionLine,
						xacLongDescriptionRtf = item2.xacLongDescriptionRtf,
						xacLongDescriptionText = item2.xacLongDescriptionText,
						xacProductionStandard = item2.xacProductionStandard,
						xacProjectedProductionRate = item2.xacProjectedProductionRate,
						xacProjectedSetupRate = item2.xacProjectedSetupRate,
						xacRowVersion = item2.xacRowVersion,
						xacSetupHours = item2.xacSetupHours,
						xacShortDescription = item2.xacShortDescription,
						xacStandardFactor = item2.xacStandardFactor,
						CustomFields = item2.CustomFields
					};
					allProcessesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Processes]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPProcessDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allProcessesDto,
				RecordCount = allProcessesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProcessDto>> Process_GetProcess(Guid processId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPProcessDto processDto = null;
		ERPResponseMessageDto<ERPProcessDto> result;
		try
		{
			IERPProcessRepository iERPProcessRepository = (base.ERPProcessRepository = new ERPProcessRepository(base.ApiClientContext));
			using (iERPProcessRepository)
			{
				ERPProcessInformationDto eRPProcessInformationDto = await base.ERPProcessRepository.GetProcess(processId);
				processDto = new ERPProcessDto
				{
					xacProcessID = eRPProcessInformationDto.xacProcessID,
					xacCreatedBy = eRPProcessInformationDto.xacCreatedBy,
					xacCreatedDate = eRPProcessInformationDto.xacCreatedDate,
					xacUniqueID = eRPProcessInformationDto.xacUniqueID,
					xacInactiveDate = eRPProcessInformationDto.xacInactiveDate,
					xacInspectionType = eRPProcessInformationDto.xacInspectionType,
					xacInactive = eRPProcessInformationDto.xacInactive,
					xacExcludeFromTMJobs = eRPProcessInformationDto.xacExcludeFromTMJobs,
					xacIgnoreCalendarMove = eRPProcessInformationDto.xacIgnoreCalendarMove,
					xacIgnoreCalendarQueue = eRPProcessInformationDto.xacIgnoreCalendarQueue,
					xacPrintInspectionLine = eRPProcessInformationDto.xacPrintInspectionLine,
					xacLongDescriptionRtf = eRPProcessInformationDto.xacLongDescriptionRtf,
					xacLongDescriptionText = eRPProcessInformationDto.xacLongDescriptionText,
					xacProductionStandard = eRPProcessInformationDto.xacProductionStandard,
					xacProjectedProductionRate = eRPProcessInformationDto.xacProjectedProductionRate,
					xacProjectedSetupRate = eRPProcessInformationDto.xacProjectedSetupRate,
					xacRowVersion = eRPProcessInformationDto.xacRowVersion,
					xacSetupHours = eRPProcessInformationDto.xacSetupHours,
					xacShortDescription = eRPProcessInformationDto.xacShortDescription,
					xacStandardFactor = eRPProcessInformationDto.xacStandardFactor,
					CustomFields = eRPProcessInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Processes []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProcessDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = processDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPProcessDto>> Process_PutProcess(ERPProcessDto process)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPProcessDto createdObject = null;
		ERPResponseMessageDto<ERPProcessDto> result;
		try
		{
			IERPProcessRepository iERPProcessRepository = (base.ERPProcessRepository = new ERPProcessRepository(base.ApiClientContext));
			using (iERPProcessRepository)
			{
				APIValidationInfoDto postResult = await base.ERPProcessRepository.SaveProcess(process);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPProcessInformationDto eRPProcessInformationDto = await base.ERPProcessRepository.GetProcess(process.xacUniqueID);
					createdObject = new ERPProcessDto
					{
						xacProcessID = eRPProcessInformationDto.xacProcessID,
						xacCreatedBy = eRPProcessInformationDto.xacCreatedBy,
						xacCreatedDate = eRPProcessInformationDto.xacCreatedDate,
						xacUniqueID = eRPProcessInformationDto.xacUniqueID,
						xacInactiveDate = eRPProcessInformationDto.xacInactiveDate,
						xacInspectionType = eRPProcessInformationDto.xacInspectionType,
						xacInactive = eRPProcessInformationDto.xacInactive,
						xacExcludeFromTMJobs = eRPProcessInformationDto.xacExcludeFromTMJobs,
						xacIgnoreCalendarMove = eRPProcessInformationDto.xacIgnoreCalendarMove,
						xacIgnoreCalendarQueue = eRPProcessInformationDto.xacIgnoreCalendarQueue,
						xacPrintInspectionLine = eRPProcessInformationDto.xacPrintInspectionLine,
						xacLongDescriptionRtf = eRPProcessInformationDto.xacLongDescriptionRtf,
						xacLongDescriptionText = eRPProcessInformationDto.xacLongDescriptionText,
						xacProductionStandard = eRPProcessInformationDto.xacProductionStandard,
						xacProjectedProductionRate = eRPProcessInformationDto.xacProjectedProductionRate,
						xacProjectedSetupRate = eRPProcessInformationDto.xacProjectedSetupRate,
						xacRowVersion = eRPProcessInformationDto.xacRowVersion,
						xacSetupHours = eRPProcessInformationDto.xacSetupHours,
						xacShortDescription = eRPProcessInformationDto.xacShortDescription,
						xacStandardFactor = eRPProcessInformationDto.xacStandardFactor,
						CustomFields = eRPProcessInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Process [{process.xacUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProcessDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteProcess(Guid processId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPProcessRepository iERPProcessRepository = (base.ERPProcessRepository = new ERPProcessRepository(base.ApiClientContext));
		using (iERPProcessRepository)
		{
			if (!(await base.ERPProcessRepository.DoesProcessExist(processId)))
			{
				base.ErrorsList.Add($"Process [{processId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPProcessInformationDto eRPProcessInformationDto = await base.ERPProcessRepository.GetProcess(processId);
				string text = await base.ERPProcessRepository.WhereUsed("Processes", new object[1] { eRPProcessInformationDto.xacProcessID }, new object[1] { "xacProcessID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Process cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPProcessDto>> Process_DeleteProcess(Guid processId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPProcessDto> result;
		try
		{
			IERPProcessRepository iERPProcessRepository = (base.ERPProcessRepository = new ERPProcessRepository(base.ApiClientContext));
			using (iERPProcessRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPProcessRepository.DeleteRowFromTable("Processes", "xac", processId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Process [{processId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPProcessDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPProcessDto()
			};
		}
		return result;
	}
}
