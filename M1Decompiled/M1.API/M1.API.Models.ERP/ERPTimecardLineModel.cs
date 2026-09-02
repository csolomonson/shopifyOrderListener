using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPTimecardLineModel : ERPBaseModel, IERPTimecardLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllTimecardLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPTimecardLineRepository iERPTimecardLineRepository = (base.ERPTimecardLineRepository = new ERPTimecardLineRepository(base.ApiClientContext));
		using (iERPTimecardLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPTimecardLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPTimecardLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPTimecardLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPTimecardLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetTimecardLine(Guid timecardLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTimecardLineRepository iERPTimecardLineRepository = (base.ERPTimecardLineRepository = new ERPTimecardLineRepository(base.ApiClientContext));
		using (iERPTimecardLineRepository)
		{
			if (!(await base.ERPTimecardLineRepository.DoesTimecardLineExist(timecardLineId)))
			{
				errorsList.Add($"TimecardLine [{timecardLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutTimecardLine(ERPTimecardLineDto timecardLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTimecardLineRepository iERPTimecardLineRepository = (base.ERPTimecardLineRepository = new ERPTimecardLineRepository(base.ApiClientContext));
		using (iERPTimecardLineRepository)
		{
			if (timecardLine.lmlTimecardID > 0 && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("Timecards", new object[1] { "LMPTIMECARDID" }, new object[1] { timecardLine.lmlTimecardID })))
			{
				errorsList.Add($"lmlTimecardID [{timecardLine.lmlTimecardID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecardLine.lmlIndirectLaborID) && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("IndirectLaborCodes", new object[1] { "LMIINDIRECTLABORID" }, new object[1] { timecardLine.lmlIndirectLaborID })))
			{
				errorsList.Add("lmlIndirectLaborID [" + timecardLine.lmlIndirectLaborID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecardLine.lmlJobID) && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { timecardLine.lmlJobID })))
			{
				errorsList.Add("lmlJobID [" + timecardLine.lmlJobID + "] not found.");
			}
			if (timecardLine.lmlJobAssemblyID > 0 && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { timecardLine.lmlJobID, timecardLine.lmlJobAssemblyID })))
			{
				errorsList.Add($"lmlJobAssemblyID [{timecardLine.lmlJobAssemblyID}] not found.");
			}
			if (timecardLine.lmlJobOperationID > 0 && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { timecardLine.lmlJobID, timecardLine.lmlJobAssemblyID, timecardLine.lmlJobOperationID })))
			{
				errorsList.Add($"lmlJobOperationID [{timecardLine.lmlJobOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecardLine.lmlWorkCenterID) && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("WorkCenters", new object[1] { "XAWWORKCENTERID" }, new object[1] { timecardLine.lmlWorkCenterID })))
			{
				errorsList.Add("lmlWorkCenterID [" + timecardLine.lmlWorkCenterID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecardLine.lmlProcessID) && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("Processes", new object[1] { "XACPROCESSID" }, new object[1] { timecardLine.lmlProcessID })))
			{
				errorsList.Add("lmlProcessID [" + timecardLine.lmlProcessID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecardLine.lmlEmployeeID) && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { timecardLine.lmlEmployeeID })))
			{
				errorsList.Add("lmlEmployeeID [" + timecardLine.lmlEmployeeID + "] not found.");
			}
			if (timecardLine.lmlShiftID > 0 && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("Shifts", new object[1] { "LMSSHIFTID" }, new object[1] { timecardLine.lmlShiftID })))
			{
				errorsList.Add($"lmlShiftID [{timecardLine.lmlShiftID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecardLine.lmlScrapReasonID) && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { timecardLine.lmlScrapReasonID })))
			{
				errorsList.Add("lmlScrapReasonID [" + timecardLine.lmlScrapReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecardLine.lmlReworkReasonID) && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { timecardLine.lmlReworkReasonID })))
			{
				errorsList.Add("lmlReworkReasonID [" + timecardLine.lmlReworkReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecardLine.lmlExpenseID) && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("Expenses", new object[1] { "LMXEXPENSEID" }, new object[1] { timecardLine.lmlExpenseID })))
			{
				errorsList.Add("lmlExpenseID [" + timecardLine.lmlExpenseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecardLine.lmlProjectID) && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { timecardLine.lmlProjectID })))
			{
				errorsList.Add("lmlProjectID [" + timecardLine.lmlProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecardLine.lmlProjectAreaID) && !(await base.ERPTimecardLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { timecardLine.lmlProjectID, timecardLine.lmlProjectAreaID })))
			{
				errorsList.Add("lmlProjectAreaID [" + timecardLine.lmlProjectAreaID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPTimecardLineDto>>> Process_GetAllTimecardLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPTimecardLineDto> allTimecardLinesDto = new List<ERPTimecardLineDto>();
		ERPResponseMessageDto<IList<ERPTimecardLineDto>> result;
		try
		{
			IERPTimecardLineRepository iERPTimecardLineRepository = (base.ERPTimecardLineRepository = new ERPTimecardLineRepository(base.ApiClientContext));
			using (iERPTimecardLineRepository)
			{
				foreach (ERPTimecardLineInformationDto item2 in await base.ERPTimecardLineRepository.GetAllTimecardLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPTimecardLineDto item = new ERPTimecardLineDto
					{
						lmlActualEndTime = item2.lmlActualEndTime,
						lmlActualStartTime = item2.lmlActualStartTime,
						lmlCompletionType = item2.lmlCompletionType,
						lmlCreatedBy = item2.lmlCreatedBy,
						lmlCreatedDate = item2.lmlCreatedDate,
						lmlEmployeeID = item2.lmlEmployeeID,
						lmlUniqueID = item2.lmlUniqueID,
						lmlExpenseID = item2.lmlExpenseID,
						lmlGoodQuantity = item2.lmlGoodQuantity,
						lmlIndirectLaborID = item2.lmlIndirectLaborID,
						lmlActive = item2.lmlActive,
						lmlCreatedFromPayrollSession = item2.lmlCreatedFromPayrollSession,
						lmlLaborHoursCalculated = item2.lmlLaborHoursCalculated,
						lmlMachineHoursCalculated = item2.lmlMachineHoursCalculated,
						lmlPostedToWip = item2.lmlPostedToWip,
						lmlSuspended = item2.lmlSuspended,
						lmlTransferredToPayroll = item2.lmlTransferredToPayroll,
						lmlJobAssemblyID = item2.lmlJobAssemblyID,
						lmlJobID = item2.lmlJobID,
						lmlJobOperationID = item2.lmlJobOperationID,
						lmlLaborCost = item2.lmlLaborCost,
						lmlLaborDescriptionRtf = item2.lmlLaborDescriptionRtf,
						lmlLaborDescriptionText = item2.lmlLaborDescriptionText,
						lmlLaborHours = item2.lmlLaborHours,
						lmlMachineHours = item2.lmlMachineHours,
						lmlOverheadCost = item2.lmlOverheadCost,
						lmlProcessID = item2.lmlProcessID,
						lmlProjectAreaID = item2.lmlProjectAreaID,
						lmlProjectID = item2.lmlProjectID,
						lmlReworkQuantity = item2.lmlReworkQuantity,
						lmlReworkReasonID = item2.lmlReworkReasonID,
						lmlRoundedEndTime = item2.lmlRoundedEndTime,
						lmlRoundedStartTime = item2.lmlRoundedStartTime,
						lmlRowVersion = item2.lmlRowVersion,
						lmlScrapQuantity = item2.lmlScrapQuantity,
						lmlScrapReasonID = item2.lmlScrapReasonID,
						lmlTimecardLineID = item2.lmlTimecardLineID,
						lmlSetupPercentCompleted = item2.lmlSetupPercentCompleted,
						lmlShiftID = item2.lmlShiftID,
						lmlSource = item2.lmlSource,
						lmlTimecardID = item2.lmlTimecardID,
						lmlTimecardType = item2.lmlTimecardType,
						lmlWorkCenterID = item2.lmlWorkCenterID,
						lmlWorkType = item2.lmlWorkType,
						CustomFields = item2.CustomFields
					};
					allTimecardLinesDto.Add(item);
				}
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
			result = new ERPResponseMessageDto<IList<ERPTimecardLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allTimecardLinesDto,
				RecordCount = allTimecardLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPTimecardLineDto>> Process_GetTimecardLine(Guid timecardLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPTimecardLineDto timecardLineDto = null;
		ERPResponseMessageDto<ERPTimecardLineDto> result;
		try
		{
			IERPTimecardLineRepository iERPTimecardLineRepository = (base.ERPTimecardLineRepository = new ERPTimecardLineRepository(base.ApiClientContext));
			using (iERPTimecardLineRepository)
			{
				ERPTimecardLineInformationDto eRPTimecardLineInformationDto = await base.ERPTimecardLineRepository.GetTimecardLine(timecardLineId);
				timecardLineDto = new ERPTimecardLineDto
				{
					lmlActualEndTime = eRPTimecardLineInformationDto.lmlActualEndTime,
					lmlActualStartTime = eRPTimecardLineInformationDto.lmlActualStartTime,
					lmlCompletionType = eRPTimecardLineInformationDto.lmlCompletionType,
					lmlCreatedBy = eRPTimecardLineInformationDto.lmlCreatedBy,
					lmlCreatedDate = eRPTimecardLineInformationDto.lmlCreatedDate,
					lmlEmployeeID = eRPTimecardLineInformationDto.lmlEmployeeID,
					lmlUniqueID = eRPTimecardLineInformationDto.lmlUniqueID,
					lmlExpenseID = eRPTimecardLineInformationDto.lmlExpenseID,
					lmlGoodQuantity = eRPTimecardLineInformationDto.lmlGoodQuantity,
					lmlIndirectLaborID = eRPTimecardLineInformationDto.lmlIndirectLaborID,
					lmlActive = eRPTimecardLineInformationDto.lmlActive,
					lmlCreatedFromPayrollSession = eRPTimecardLineInformationDto.lmlCreatedFromPayrollSession,
					lmlLaborHoursCalculated = eRPTimecardLineInformationDto.lmlLaborHoursCalculated,
					lmlMachineHoursCalculated = eRPTimecardLineInformationDto.lmlMachineHoursCalculated,
					lmlPostedToWip = eRPTimecardLineInformationDto.lmlPostedToWip,
					lmlSuspended = eRPTimecardLineInformationDto.lmlSuspended,
					lmlTransferredToPayroll = eRPTimecardLineInformationDto.lmlTransferredToPayroll,
					lmlJobAssemblyID = eRPTimecardLineInformationDto.lmlJobAssemblyID,
					lmlJobID = eRPTimecardLineInformationDto.lmlJobID,
					lmlJobOperationID = eRPTimecardLineInformationDto.lmlJobOperationID,
					lmlLaborCost = eRPTimecardLineInformationDto.lmlLaborCost,
					lmlLaborDescriptionRtf = eRPTimecardLineInformationDto.lmlLaborDescriptionRtf,
					lmlLaborDescriptionText = eRPTimecardLineInformationDto.lmlLaborDescriptionText,
					lmlLaborHours = eRPTimecardLineInformationDto.lmlLaborHours,
					lmlMachineHours = eRPTimecardLineInformationDto.lmlMachineHours,
					lmlOverheadCost = eRPTimecardLineInformationDto.lmlOverheadCost,
					lmlProcessID = eRPTimecardLineInformationDto.lmlProcessID,
					lmlProjectAreaID = eRPTimecardLineInformationDto.lmlProjectAreaID,
					lmlProjectID = eRPTimecardLineInformationDto.lmlProjectID,
					lmlReworkQuantity = eRPTimecardLineInformationDto.lmlReworkQuantity,
					lmlReworkReasonID = eRPTimecardLineInformationDto.lmlReworkReasonID,
					lmlRoundedEndTime = eRPTimecardLineInformationDto.lmlRoundedEndTime,
					lmlRoundedStartTime = eRPTimecardLineInformationDto.lmlRoundedStartTime,
					lmlRowVersion = eRPTimecardLineInformationDto.lmlRowVersion,
					lmlScrapQuantity = eRPTimecardLineInformationDto.lmlScrapQuantity,
					lmlScrapReasonID = eRPTimecardLineInformationDto.lmlScrapReasonID,
					lmlTimecardLineID = eRPTimecardLineInformationDto.lmlTimecardLineID,
					lmlSetupPercentCompleted = eRPTimecardLineInformationDto.lmlSetupPercentCompleted,
					lmlShiftID = eRPTimecardLineInformationDto.lmlShiftID,
					lmlSource = eRPTimecardLineInformationDto.lmlSource,
					lmlTimecardID = eRPTimecardLineInformationDto.lmlTimecardID,
					lmlTimecardType = eRPTimecardLineInformationDto.lmlTimecardType,
					lmlWorkCenterID = eRPTimecardLineInformationDto.lmlWorkCenterID,
					lmlWorkType = eRPTimecardLineInformationDto.lmlWorkType,
					CustomFields = eRPTimecardLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the TimecardLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTimecardLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = timecardLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPTimecardLineDto>> Process_PutTimecardLine(ERPTimecardLineDto timecardLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPTimecardLineDto createdObject = null;
		ERPResponseMessageDto<ERPTimecardLineDto> result;
		try
		{
			IERPTimecardLineRepository iERPTimecardLineRepository = (base.ERPTimecardLineRepository = new ERPTimecardLineRepository(base.ApiClientContext));
			using (iERPTimecardLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPTimecardLineRepository.SaveTimecardLine(timecardLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPTimecardLineInformationDto eRPTimecardLineInformationDto = await base.ERPTimecardLineRepository.GetTimecardLine(timecardLine.lmlUniqueID);
					createdObject = new ERPTimecardLineDto
					{
						lmlActualEndTime = eRPTimecardLineInformationDto.lmlActualEndTime,
						lmlActualStartTime = eRPTimecardLineInformationDto.lmlActualStartTime,
						lmlCompletionType = eRPTimecardLineInformationDto.lmlCompletionType,
						lmlCreatedBy = eRPTimecardLineInformationDto.lmlCreatedBy,
						lmlCreatedDate = eRPTimecardLineInformationDto.lmlCreatedDate,
						lmlEmployeeID = eRPTimecardLineInformationDto.lmlEmployeeID,
						lmlUniqueID = eRPTimecardLineInformationDto.lmlUniqueID,
						lmlExpenseID = eRPTimecardLineInformationDto.lmlExpenseID,
						lmlGoodQuantity = eRPTimecardLineInformationDto.lmlGoodQuantity,
						lmlIndirectLaborID = eRPTimecardLineInformationDto.lmlIndirectLaborID,
						lmlActive = eRPTimecardLineInformationDto.lmlActive,
						lmlCreatedFromPayrollSession = eRPTimecardLineInformationDto.lmlCreatedFromPayrollSession,
						lmlLaborHoursCalculated = eRPTimecardLineInformationDto.lmlLaborHoursCalculated,
						lmlMachineHoursCalculated = eRPTimecardLineInformationDto.lmlMachineHoursCalculated,
						lmlPostedToWip = eRPTimecardLineInformationDto.lmlPostedToWip,
						lmlSuspended = eRPTimecardLineInformationDto.lmlSuspended,
						lmlTransferredToPayroll = eRPTimecardLineInformationDto.lmlTransferredToPayroll,
						lmlJobAssemblyID = eRPTimecardLineInformationDto.lmlJobAssemblyID,
						lmlJobID = eRPTimecardLineInformationDto.lmlJobID,
						lmlJobOperationID = eRPTimecardLineInformationDto.lmlJobOperationID,
						lmlLaborCost = eRPTimecardLineInformationDto.lmlLaborCost,
						lmlLaborDescriptionRtf = eRPTimecardLineInformationDto.lmlLaborDescriptionRtf,
						lmlLaborDescriptionText = eRPTimecardLineInformationDto.lmlLaborDescriptionText,
						lmlLaborHours = eRPTimecardLineInformationDto.lmlLaborHours,
						lmlMachineHours = eRPTimecardLineInformationDto.lmlMachineHours,
						lmlOverheadCost = eRPTimecardLineInformationDto.lmlOverheadCost,
						lmlProcessID = eRPTimecardLineInformationDto.lmlProcessID,
						lmlProjectAreaID = eRPTimecardLineInformationDto.lmlProjectAreaID,
						lmlProjectID = eRPTimecardLineInformationDto.lmlProjectID,
						lmlReworkQuantity = eRPTimecardLineInformationDto.lmlReworkQuantity,
						lmlReworkReasonID = eRPTimecardLineInformationDto.lmlReworkReasonID,
						lmlRoundedEndTime = eRPTimecardLineInformationDto.lmlRoundedEndTime,
						lmlRoundedStartTime = eRPTimecardLineInformationDto.lmlRoundedStartTime,
						lmlRowVersion = eRPTimecardLineInformationDto.lmlRowVersion,
						lmlScrapQuantity = eRPTimecardLineInformationDto.lmlScrapQuantity,
						lmlScrapReasonID = eRPTimecardLineInformationDto.lmlScrapReasonID,
						lmlTimecardLineID = eRPTimecardLineInformationDto.lmlTimecardLineID,
						lmlSetupPercentCompleted = eRPTimecardLineInformationDto.lmlSetupPercentCompleted,
						lmlShiftID = eRPTimecardLineInformationDto.lmlShiftID,
						lmlSource = eRPTimecardLineInformationDto.lmlSource,
						lmlTimecardID = eRPTimecardLineInformationDto.lmlTimecardID,
						lmlTimecardType = eRPTimecardLineInformationDto.lmlTimecardType,
						lmlWorkCenterID = eRPTimecardLineInformationDto.lmlWorkCenterID,
						lmlWorkType = eRPTimecardLineInformationDto.lmlWorkType,
						CustomFields = eRPTimecardLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing TimecardLine [{timecardLine.lmlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTimecardLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteTimecardLine(Guid timecardLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTimecardLineRepository iERPTimecardLineRepository = (base.ERPTimecardLineRepository = new ERPTimecardLineRepository(base.ApiClientContext));
		using (iERPTimecardLineRepository)
		{
			if (!(await base.ERPTimecardLineRepository.DoesTimecardLineExist(timecardLineId)))
			{
				base.ErrorsList.Add($"TimecardLine [{timecardLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPTimecardLineInformationDto eRPTimecardLineInformationDto = await base.ERPTimecardLineRepository.GetTimecardLine(timecardLineId);
				string text = await base.ERPTimecardLineRepository.WhereUsed("TimecardLines", new object[2] { eRPTimecardLineInformationDto.lmlTimecardID, eRPTimecardLineInformationDto.lmlTimecardLineID }, new object[2] { "lmlTimecardID", "lmlTimecardLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("TimecardLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPTimecardLineDto>> Process_DeleteTimecardLine(Guid timecardLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPTimecardLineDto> result;
		try
		{
			IERPTimecardLineRepository iERPTimecardLineRepository = (base.ERPTimecardLineRepository = new ERPTimecardLineRepository(base.ApiClientContext));
			using (iERPTimecardLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPTimecardLineRepository.DeleteRowFromTable("TimecardLines", "lml", timecardLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of TimecardLine [{timecardLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTimecardLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPTimecardLineDto()
			};
		}
		return result;
	}
}
