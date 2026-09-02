using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPTimecardModel : ERPBaseModel, IERPTimecardModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllTimecards(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPTimecardRepository iERPTimecardRepository = (base.ERPTimecardRepository = new ERPTimecardRepository(base.ApiClientContext));
		using (iERPTimecardRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPTimecardRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPTimecardRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPTimecardRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPTimecardRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetTimecard(Guid timecardId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTimecardRepository iERPTimecardRepository = (base.ERPTimecardRepository = new ERPTimecardRepository(base.ApiClientContext));
		using (iERPTimecardRepository)
		{
			if (!(await base.ERPTimecardRepository.DoesTimecardExist(timecardId)))
			{
				errorsList.Add($"Timecard [{timecardId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutTimecard(ERPTimecardDto timecard)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTimecardRepository iERPTimecardRepository = (base.ERPTimecardRepository = new ERPTimecardRepository(base.ApiClientContext));
		using (iERPTimecardRepository)
		{
			if (!string.IsNullOrWhiteSpace(timecard.lmpEmployeeID) && !(await base.ERPTimecardRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { timecard.lmpEmployeeID })))
			{
				errorsList.Add("lmpEmployeeID [" + timecard.lmpEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecard.lmpPlantDepartmentID) && !(await base.ERPTimecardRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { timecard.lmpPlantID, timecard.lmpPlantDepartmentID })))
			{
				errorsList.Add("lmpPlantDepartmentID [" + timecard.lmpPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecard.lmpPlantID) && !(await base.ERPTimecardRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { timecard.lmpPlantID })))
			{
				errorsList.Add("lmpPlantID [" + timecard.lmpPlantID + "] not found.");
			}
			if (timecard.lmpShiftID > 0 && !(await base.ERPTimecardRepository.DoesRecordExistInTableUsingKeys("Shifts", new object[1] { "LMSSHIFTID" }, new object[1] { timecard.lmpShiftID })))
			{
				errorsList.Add($"lmpShiftID [{timecard.lmpShiftID}] not found.");
			}
			if (timecard.lmpShiftBreakID > 0 && !(await base.ERPTimecardRepository.DoesRecordExistInTableUsingKeys("ShiftBreaks", new object[2] { "LMTSHIFTID", "LMTDAY" }, new object[2] { timecard.lmpShiftID, timecard.lmpShiftBreakID })))
			{
				errorsList.Add($"lmpShiftBreakID [{timecard.lmpShiftBreakID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecard.lmpLeaveAccrualID) && !(await base.ERPTimecardRepository.DoesRecordExistInTableUsingKeys("LeaveAccruals", new object[1] { "PAJLEAVEACCRUALID" }, new object[1] { timecard.lmpLeaveAccrualID })))
			{
				errorsList.Add("lmpLeaveAccrualID [" + timecard.lmpLeaveAccrualID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecard.lmpOTPeriod1PayrollRateID) && !(await base.ERPTimecardRepository.DoesRecordExistInTableUsingKeys("PayrollRates", new object[1] { "PAYPAYROLLRATEID" }, new object[1] { timecard.lmpOTPeriod1PayrollRateID })))
			{
				errorsList.Add("lmpOTPeriod1PayrollRateID [" + timecard.lmpOTPeriod1PayrollRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecard.lmpOTPeriod2PayrollRateID) && !(await base.ERPTimecardRepository.DoesRecordExistInTableUsingKeys("PayrollRates", new object[1] { "PAYPAYROLLRATEID" }, new object[1] { timecard.lmpOTPeriod2PayrollRateID })))
			{
				errorsList.Add("lmpOTPeriod2PayrollRateID [" + timecard.lmpOTPeriod2PayrollRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecard.lmpOTPeriod3PayrollRateID) && !(await base.ERPTimecardRepository.DoesRecordExistInTableUsingKeys("PayrollRates", new object[1] { "PAYPAYROLLRATEID" }, new object[1] { timecard.lmpOTPeriod3PayrollRateID })))
			{
				errorsList.Add("lmpOTPeriod3PayrollRateID [" + timecard.lmpOTPeriod3PayrollRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecard.lmpOTPeriod4PayrollRateID) && !(await base.ERPTimecardRepository.DoesRecordExistInTableUsingKeys("PayrollRates", new object[1] { "PAYPAYROLLRATEID" }, new object[1] { timecard.lmpOTPeriod4PayrollRateID })))
			{
				errorsList.Add("lmpOTPeriod4PayrollRateID [" + timecard.lmpOTPeriod4PayrollRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecard.lmpStandardPayrollRateID) && !(await base.ERPTimecardRepository.DoesRecordExistInTableUsingKeys("PayrollRates", new object[1] { "PAYPAYROLLRATEID" }, new object[1] { timecard.lmpStandardPayrollRateID })))
			{
				errorsList.Add("lmpStandardPayrollRateID [" + timecard.lmpStandardPayrollRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecard.lmpOtherPayrollRateID) && !(await base.ERPTimecardRepository.DoesRecordExistInTableUsingKeys("PayrollRates", new object[1] { "PAYPAYROLLRATEID" }, new object[1] { timecard.lmpOtherPayrollRateID })))
			{
				errorsList.Add("lmpOtherPayrollRateID [" + timecard.lmpOtherPayrollRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(timecard.lmpProjectID) && !(await base.ERPTimecardRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { timecard.lmpProjectID })))
			{
				errorsList.Add("lmpProjectID [" + timecard.lmpProjectID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPTimecardDto>>> Process_GetAllTimecards(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPTimecardDto> allTimecardsDto = new List<ERPTimecardDto>();
		ERPResponseMessageDto<IList<ERPTimecardDto>> result;
		try
		{
			IERPTimecardRepository iERPTimecardRepository = (base.ERPTimecardRepository = new ERPTimecardRepository(base.ApiClientContext));
			using (iERPTimecardRepository)
			{
				foreach (ERPTimecardInformationDto item2 in await base.ERPTimecardRepository.GetAllTimecards(pageSize, pageNumber, filter, orderBy))
				{
					ERPTimecardDto item = new ERPTimecardDto
					{
						lmpActualEndTime = item2.lmpActualEndTime,
						lmpActualStartTime = item2.lmpActualStartTime,
						lmpCreatedBy = item2.lmpCreatedBy,
						lmpCreatedDate = item2.lmpCreatedDate,
						lmpEmployeeID = item2.lmpEmployeeID,
						lmpUniqueID = item2.lmpUniqueID,
						lmpExchangeID = item2.lmpExchangeID,
						lmpActive = item2.lmpActive,
						lmpAutoClockedOut = item2.lmpAutoClockedOut,
						lmpCreatedFromPayrollSession = item2.lmpCreatedFromPayrollSession,
						lmpPostedToWip = item2.lmpPostedToWip,
						lmpTransferredToPayroll = item2.lmpTransferredToPayroll,
						lmpLastEndTime = item2.lmpLastEndTime,
						lmpLeaveAccrualID = item2.lmpLeaveAccrualID,
						lmpMachineHours = item2.lmpMachineHours,
						lmpNoteRtf = item2.lmpNoteRtf,
						lmpNoteText = item2.lmpNoteText,
						lmpOtherHours = item2.lmpOtherHours,
						lmpOtherPayrollRateID = item2.lmpOtherPayrollRateID,
						lmpOTPeriod1Hours = item2.lmpOTPeriod1Hours,
						lmpOTPeriod1PayrollRateID = item2.lmpOTPeriod1PayrollRateID,
						lmpOTPeriod2Hours = item2.lmpOTPeriod2Hours,
						lmpOTPeriod2PayrollRateID = item2.lmpOTPeriod2PayrollRateID,
						lmpOTPeriod3Hours = item2.lmpOTPeriod3Hours,
						lmpOTPeriod3PayrollRateID = item2.lmpOTPeriod3PayrollRateID,
						lmpOTPeriod4Hours = item2.lmpOTPeriod4Hours,
						lmpOTPeriod4PayrollRateID = item2.lmpOTPeriod4PayrollRateID,
						lmpPaidDate = item2.lmpPaidDate,
						lmpPayrollHours = item2.lmpPayrollHours,
						lmpPlantDepartmentID = item2.lmpPlantDepartmentID,
						lmpPlantID = item2.lmpPlantID,
						lmpPostedDate = item2.lmpPostedDate,
						lmpProjectID = item2.lmpProjectID,
						lmpRoundedEndTime = item2.lmpRoundedEndTime,
						lmpRoundedStartTime = item2.lmpRoundedStartTime,
						lmpRowVersion = item2.lmpRowVersion,
						lmpTimecardID = item2.lmpTimecardID,
						lmpShiftBreakID = item2.lmpShiftBreakID,
						lmpShiftID = item2.lmpShiftID,
						lmpSource = item2.lmpSource,
						lmpStandardHours = item2.lmpStandardHours,
						lmpStandardPayrollRateID = item2.lmpStandardPayrollRateID,
						lmpTimecardDate = item2.lmpTimecardDate,
						lmpTotalPayrollHours = item2.lmpTotalPayrollHours,
						lmpTransferredDate = item2.lmpTransferredDate,
						lmpUtcOffset = item2.lmpUtcOffset,
						CustomFields = item2.CustomFields
					};
					allTimecardsDto.Add(item);
				}
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
			result = new ERPResponseMessageDto<IList<ERPTimecardDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allTimecardsDto,
				RecordCount = allTimecardsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPTimecardDto>> Process_GetTimecard(Guid timecardId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPTimecardDto timecardDto = null;
		ERPResponseMessageDto<ERPTimecardDto> result;
		try
		{
			IERPTimecardRepository iERPTimecardRepository = (base.ERPTimecardRepository = new ERPTimecardRepository(base.ApiClientContext));
			using (iERPTimecardRepository)
			{
				ERPTimecardInformationDto eRPTimecardInformationDto = await base.ERPTimecardRepository.GetTimecard(timecardId);
				timecardDto = new ERPTimecardDto
				{
					lmpActualEndTime = eRPTimecardInformationDto.lmpActualEndTime,
					lmpActualStartTime = eRPTimecardInformationDto.lmpActualStartTime,
					lmpCreatedBy = eRPTimecardInformationDto.lmpCreatedBy,
					lmpCreatedDate = eRPTimecardInformationDto.lmpCreatedDate,
					lmpEmployeeID = eRPTimecardInformationDto.lmpEmployeeID,
					lmpUniqueID = eRPTimecardInformationDto.lmpUniqueID,
					lmpExchangeID = eRPTimecardInformationDto.lmpExchangeID,
					lmpActive = eRPTimecardInformationDto.lmpActive,
					lmpAutoClockedOut = eRPTimecardInformationDto.lmpAutoClockedOut,
					lmpCreatedFromPayrollSession = eRPTimecardInformationDto.lmpCreatedFromPayrollSession,
					lmpPostedToWip = eRPTimecardInformationDto.lmpPostedToWip,
					lmpTransferredToPayroll = eRPTimecardInformationDto.lmpTransferredToPayroll,
					lmpLastEndTime = eRPTimecardInformationDto.lmpLastEndTime,
					lmpLeaveAccrualID = eRPTimecardInformationDto.lmpLeaveAccrualID,
					lmpMachineHours = eRPTimecardInformationDto.lmpMachineHours,
					lmpNoteRtf = eRPTimecardInformationDto.lmpNoteRtf,
					lmpNoteText = eRPTimecardInformationDto.lmpNoteText,
					lmpOtherHours = eRPTimecardInformationDto.lmpOtherHours,
					lmpOtherPayrollRateID = eRPTimecardInformationDto.lmpOtherPayrollRateID,
					lmpOTPeriod1Hours = eRPTimecardInformationDto.lmpOTPeriod1Hours,
					lmpOTPeriod1PayrollRateID = eRPTimecardInformationDto.lmpOTPeriod1PayrollRateID,
					lmpOTPeriod2Hours = eRPTimecardInformationDto.lmpOTPeriod2Hours,
					lmpOTPeriod2PayrollRateID = eRPTimecardInformationDto.lmpOTPeriod2PayrollRateID,
					lmpOTPeriod3Hours = eRPTimecardInformationDto.lmpOTPeriod3Hours,
					lmpOTPeriod3PayrollRateID = eRPTimecardInformationDto.lmpOTPeriod3PayrollRateID,
					lmpOTPeriod4Hours = eRPTimecardInformationDto.lmpOTPeriod4Hours,
					lmpOTPeriod4PayrollRateID = eRPTimecardInformationDto.lmpOTPeriod4PayrollRateID,
					lmpPaidDate = eRPTimecardInformationDto.lmpPaidDate,
					lmpPayrollHours = eRPTimecardInformationDto.lmpPayrollHours,
					lmpPlantDepartmentID = eRPTimecardInformationDto.lmpPlantDepartmentID,
					lmpPlantID = eRPTimecardInformationDto.lmpPlantID,
					lmpPostedDate = eRPTimecardInformationDto.lmpPostedDate,
					lmpProjectID = eRPTimecardInformationDto.lmpProjectID,
					lmpRoundedEndTime = eRPTimecardInformationDto.lmpRoundedEndTime,
					lmpRoundedStartTime = eRPTimecardInformationDto.lmpRoundedStartTime,
					lmpRowVersion = eRPTimecardInformationDto.lmpRowVersion,
					lmpTimecardID = eRPTimecardInformationDto.lmpTimecardID,
					lmpShiftBreakID = eRPTimecardInformationDto.lmpShiftBreakID,
					lmpShiftID = eRPTimecardInformationDto.lmpShiftID,
					lmpSource = eRPTimecardInformationDto.lmpSource,
					lmpStandardHours = eRPTimecardInformationDto.lmpStandardHours,
					lmpStandardPayrollRateID = eRPTimecardInformationDto.lmpStandardPayrollRateID,
					lmpTimecardDate = eRPTimecardInformationDto.lmpTimecardDate,
					lmpTotalPayrollHours = eRPTimecardInformationDto.lmpTotalPayrollHours,
					lmpTransferredDate = eRPTimecardInformationDto.lmpTransferredDate,
					lmpUtcOffset = eRPTimecardInformationDto.lmpUtcOffset,
					CustomFields = eRPTimecardInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Timecards []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTimecardDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = timecardDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPTimecardDto>> Process_PutTimecard(ERPTimecardDto timecard)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPTimecardDto createdObject = null;
		ERPResponseMessageDto<ERPTimecardDto> result;
		try
		{
			IERPTimecardRepository iERPTimecardRepository = (base.ERPTimecardRepository = new ERPTimecardRepository(base.ApiClientContext));
			using (iERPTimecardRepository)
			{
				APIValidationInfoDto postResult = await base.ERPTimecardRepository.SaveTimecard(timecard);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPTimecardInformationDto eRPTimecardInformationDto = await base.ERPTimecardRepository.GetTimecard(timecard.lmpUniqueID);
					createdObject = new ERPTimecardDto
					{
						lmpActualEndTime = eRPTimecardInformationDto.lmpActualEndTime,
						lmpActualStartTime = eRPTimecardInformationDto.lmpActualStartTime,
						lmpCreatedBy = eRPTimecardInformationDto.lmpCreatedBy,
						lmpCreatedDate = eRPTimecardInformationDto.lmpCreatedDate,
						lmpEmployeeID = eRPTimecardInformationDto.lmpEmployeeID,
						lmpUniqueID = eRPTimecardInformationDto.lmpUniqueID,
						lmpExchangeID = eRPTimecardInformationDto.lmpExchangeID,
						lmpActive = eRPTimecardInformationDto.lmpActive,
						lmpAutoClockedOut = eRPTimecardInformationDto.lmpAutoClockedOut,
						lmpCreatedFromPayrollSession = eRPTimecardInformationDto.lmpCreatedFromPayrollSession,
						lmpPostedToWip = eRPTimecardInformationDto.lmpPostedToWip,
						lmpTransferredToPayroll = eRPTimecardInformationDto.lmpTransferredToPayroll,
						lmpLastEndTime = eRPTimecardInformationDto.lmpLastEndTime,
						lmpLeaveAccrualID = eRPTimecardInformationDto.lmpLeaveAccrualID,
						lmpMachineHours = eRPTimecardInformationDto.lmpMachineHours,
						lmpNoteRtf = eRPTimecardInformationDto.lmpNoteRtf,
						lmpNoteText = eRPTimecardInformationDto.lmpNoteText,
						lmpOtherHours = eRPTimecardInformationDto.lmpOtherHours,
						lmpOtherPayrollRateID = eRPTimecardInformationDto.lmpOtherPayrollRateID,
						lmpOTPeriod1Hours = eRPTimecardInformationDto.lmpOTPeriod1Hours,
						lmpOTPeriod1PayrollRateID = eRPTimecardInformationDto.lmpOTPeriod1PayrollRateID,
						lmpOTPeriod2Hours = eRPTimecardInformationDto.lmpOTPeriod2Hours,
						lmpOTPeriod2PayrollRateID = eRPTimecardInformationDto.lmpOTPeriod2PayrollRateID,
						lmpOTPeriod3Hours = eRPTimecardInformationDto.lmpOTPeriod3Hours,
						lmpOTPeriod3PayrollRateID = eRPTimecardInformationDto.lmpOTPeriod3PayrollRateID,
						lmpOTPeriod4Hours = eRPTimecardInformationDto.lmpOTPeriod4Hours,
						lmpOTPeriod4PayrollRateID = eRPTimecardInformationDto.lmpOTPeriod4PayrollRateID,
						lmpPaidDate = eRPTimecardInformationDto.lmpPaidDate,
						lmpPayrollHours = eRPTimecardInformationDto.lmpPayrollHours,
						lmpPlantDepartmentID = eRPTimecardInformationDto.lmpPlantDepartmentID,
						lmpPlantID = eRPTimecardInformationDto.lmpPlantID,
						lmpPostedDate = eRPTimecardInformationDto.lmpPostedDate,
						lmpProjectID = eRPTimecardInformationDto.lmpProjectID,
						lmpRoundedEndTime = eRPTimecardInformationDto.lmpRoundedEndTime,
						lmpRoundedStartTime = eRPTimecardInformationDto.lmpRoundedStartTime,
						lmpRowVersion = eRPTimecardInformationDto.lmpRowVersion,
						lmpTimecardID = eRPTimecardInformationDto.lmpTimecardID,
						lmpShiftBreakID = eRPTimecardInformationDto.lmpShiftBreakID,
						lmpShiftID = eRPTimecardInformationDto.lmpShiftID,
						lmpSource = eRPTimecardInformationDto.lmpSource,
						lmpStandardHours = eRPTimecardInformationDto.lmpStandardHours,
						lmpStandardPayrollRateID = eRPTimecardInformationDto.lmpStandardPayrollRateID,
						lmpTimecardDate = eRPTimecardInformationDto.lmpTimecardDate,
						lmpTotalPayrollHours = eRPTimecardInformationDto.lmpTotalPayrollHours,
						lmpTransferredDate = eRPTimecardInformationDto.lmpTransferredDate,
						lmpUtcOffset = eRPTimecardInformationDto.lmpUtcOffset,
						CustomFields = eRPTimecardInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Timecard [{timecard.lmpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTimecardDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteTimecard(Guid timecardId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTimecardRepository iERPTimecardRepository = (base.ERPTimecardRepository = new ERPTimecardRepository(base.ApiClientContext));
		using (iERPTimecardRepository)
		{
			if (!(await base.ERPTimecardRepository.DoesTimecardExist(timecardId)))
			{
				base.ErrorsList.Add($"Timecard [{timecardId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPTimecardInformationDto eRPTimecardInformationDto = await base.ERPTimecardRepository.GetTimecard(timecardId);
				string text = await base.ERPTimecardRepository.WhereUsed("Timecards", new object[1] { eRPTimecardInformationDto.lmpTimecardID }, new object[1] { "lmpTimecardID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Timecard cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPTimecardDto>> Process_DeleteTimecard(Guid timecardId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPTimecardDto> result;
		try
		{
			IERPTimecardRepository iERPTimecardRepository = (base.ERPTimecardRepository = new ERPTimecardRepository(base.ApiClientContext));
			using (iERPTimecardRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPTimecardRepository.DeleteRowFromTable("Timecards", "lmp", timecardId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Timecard [{timecardId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTimecardDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPTimecardDto()
			};
		}
		return result;
	}
}
