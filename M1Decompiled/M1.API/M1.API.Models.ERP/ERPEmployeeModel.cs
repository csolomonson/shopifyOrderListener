using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPEmployeeModel : ERPBaseModel, IERPEmployeeModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllEmployees(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPEmployeeRepository iERPEmployeeRepository = (base.ERPEmployeeRepository = new ERPEmployeeRepository(base.ApiClientContext));
		using (iERPEmployeeRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPEmployeeRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPEmployeeRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPEmployeeRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPEmployeeRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetEmployee(Guid employeeId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeRepository iERPEmployeeRepository = (base.ERPEmployeeRepository = new ERPEmployeeRepository(base.ApiClientContext));
		using (iERPEmployeeRepository)
		{
			if (!(await base.ERPEmployeeRepository.DoesEmployeeExist(employeeId)))
			{
				errorsList.Add($"Employee [{employeeId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutEmployee(ERPEmployeeDto employee)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeRepository iERPEmployeeRepository = (base.ERPEmployeeRepository = new ERPEmployeeRepository(base.ApiClientContext));
		using (iERPEmployeeRepository)
		{
			if (!string.IsNullOrWhiteSpace(employee.lmeContactTitleID) && !(await base.ERPEmployeeRepository.DoesRecordExistInTableUsingKeys("ContactTitles", new object[1] { "CMECONTACTTITLEID" }, new object[1] { employee.lmeContactTitleID })))
			{
				errorsList.Add("lmeContactTitleID [" + employee.lmeContactTitleID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employee.lmeTerminationReasonID) && !(await base.ERPEmployeeRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { employee.lmeTerminationReasonID })))
			{
				errorsList.Add("lmeTerminationReasonID [" + employee.lmeTerminationReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employee.lmePlantDepartmentID) && !(await base.ERPEmployeeRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { employee.lmePlantID, employee.lmePlantDepartmentID })))
			{
				errorsList.Add("lmePlantDepartmentID [" + employee.lmePlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employee.lmePlantID) && !(await base.ERPEmployeeRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { employee.lmePlantID })))
			{
				errorsList.Add("lmePlantID [" + employee.lmePlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employee.lmeHomeProductionDepartmentID) && !(await base.ERPEmployeeRepository.DoesRecordExistInTableUsingKeys("ProductionDepartments", new object[1] { "XAEPRODUCTIONDEPARTMENTID" }, new object[1] { employee.lmeHomeProductionDepartmentID })))
			{
				errorsList.Add("lmeHomeProductionDepartmentID [" + employee.lmeHomeProductionDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employee.lmeDefaultWorkCenterID) && !(await base.ERPEmployeeRepository.DoesRecordExistInTableUsingKeys("WorkCenters", new object[1] { "XAWWORKCENTERID" }, new object[1] { employee.lmeDefaultWorkCenterID })))
			{
				errorsList.Add("lmeDefaultWorkCenterID [" + employee.lmeDefaultWorkCenterID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employee.lmeDirectExpenseID) && !(await base.ERPEmployeeRepository.DoesRecordExistInTableUsingKeys("Expenses", new object[1] { "LMXEXPENSEID" }, new object[1] { employee.lmeDirectExpenseID })))
			{
				errorsList.Add("lmeDirectExpenseID [" + employee.lmeDirectExpenseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employee.lmeIndirectExpenseID) && !(await base.ERPEmployeeRepository.DoesRecordExistInTableUsingKeys("Expenses", new object[1] { "LMXEXPENSEID" }, new object[1] { employee.lmeIndirectExpenseID })))
			{
				errorsList.Add("lmeIndirectExpenseID [" + employee.lmeIndirectExpenseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employee.lmeCallTypeID) && !(await base.ERPEmployeeRepository.DoesRecordExistInTableUsingKeys("CallTypes", new object[1] { "KBTCALLTYPEID" }, new object[1] { employee.lmeCallTypeID })))
			{
				errorsList.Add("lmeCallTypeID [" + employee.lmeCallTypeID + "] not found.");
			}
			if (employee.lmeDefaultShiftID > 0 && !(await base.ERPEmployeeRepository.DoesRecordExistInTableUsingKeys("Shifts", new object[1] { "LMSSHIFTID" }, new object[1] { employee.lmeDefaultShiftID })))
			{
				errorsList.Add($"lmeDefaultShiftID [{employee.lmeDefaultShiftID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employee.lmeCountyCodeID) && !(await base.ERPEmployeeRepository.DoesRecordExistInTableUsingKeys("COUNTYCODES", new object[1] { "XCCCOUNTYCODEID" }, new object[1] { employee.lmeCountyCodeID })))
			{
				errorsList.Add("lmeCountyCodeID [" + employee.lmeCountyCodeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPEmployeeDto>>> Process_GetAllEmployees(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPEmployeeDto> allEmployeesDto = new List<ERPEmployeeDto>();
		ERPResponseMessageDto<IList<ERPEmployeeDto>> result;
		try
		{
			IERPEmployeeRepository iERPEmployeeRepository = (base.ERPEmployeeRepository = new ERPEmployeeRepository(base.ApiClientContext));
			using (iERPEmployeeRepository)
			{
				foreach (ERPEmployeeInformationDto item2 in await base.ERPEmployeeRepository.GetAllEmployees(pageSize, pageNumber, filter, orderBy))
				{
					ERPEmployeeDto item = new ERPEmployeeDto
					{
						lmeCallTypeID = item2.lmeCallTypeID,
						lmeCessationType = item2.lmeCessationType,
						lmeEmployeeID = item2.lmeEmployeeID,
						lmeCommissionRate = item2.lmeCommissionRate,
						lmeContactTitleID = item2.lmeContactTitleID,
						lmeCountyCodeID = item2.lmeCountyCodeID,
						lmeCreatedBy = item2.lmeCreatedBy,
						lmeCreatedDate = item2.lmeCreatedDate,
						lmeDefaultShiftID = item2.lmeDefaultShiftID,
						lmeDefaultWorkCenterID = item2.lmeDefaultWorkCenterID,
						lmeDirectExpenseID = item2.lmeDirectExpenseID,
						lmeEarningType = item2.lmeEarningType,
						lmeEmployeeName = item2.lmeEmployeeName,
						lmeUniqueID = item2.lmeUniqueID,
						lmeHireDate = item2.lmeHireDate,
						lmeHomeProductionDepartmentID = item2.lmeHomeProductionDepartmentID,
						lmeIndirectExpenseID = item2.lmeIndirectExpenseID,
						lmeBuyerEmployee = item2.lmeBuyerEmployee,
						lmeEngineerEmployee = item2.lmeEngineerEmployee,
						lmeInspectorEmployee = item2.lmeInspectorEmployee,
						lmeLockShift = item2.lmeLockShift,
						lmePayrollEmployee = item2.lmePayrollEmployee,
						lmePlannerEmployee = item2.lmePlannerEmployee,
						lmeProjectManagerEmployee = item2.lmeProjectManagerEmployee,
						lmeQuoterEmployee = item2.lmeQuoterEmployee,
						lmeSalesEmployee = item2.lmeSalesEmployee,
						lmeShopEmployee = item2.lmeShopEmployee,
						lmeSortSfebyWorkcenter = item2.lmeSortSfebyWorkcenter,
						lmeSupportEmployee = item2.lmeSupportEmployee,
						lmeLanguage = item2.lmeLanguage,
						lmePassword = item2.lmePassword,
						lmePlantDepartmentID = item2.lmePlantDepartmentID,
						lmePlantID = item2.lmePlantID,
						lmePoApprovalAmount = item2.lmePoApprovalAmount,
						lmePreviousEmployeeID = item2.lmePreviousEmployeeID,
						lmeRowVersion = item2.lmeRowVersion,
						lmeSOApprovalAmount = item2.lmeSOApprovalAmount,
						lmeTerminationDate = item2.lmeTerminationDate,
						lmeTerminationReasonID = item2.lmeTerminationReasonID,
						lmeUseEmail = item2.lmeUseEmail,
						lmeUseEmailPayslips = item2.lmeUseEmailPayslips,
						lmeUserID = item2.lmeUserID,
						lmeWorkEmailAddress = item2.lmeWorkEmailAddress,
						CustomFields = item2.CustomFields
					};
					allEmployeesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Employees]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPEmployeeDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allEmployeesDto,
				RecordCount = allEmployeesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeDto>> Process_GetEmployee(Guid employeeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPEmployeeDto employeeDto = null;
		ERPResponseMessageDto<ERPEmployeeDto> result;
		try
		{
			IERPEmployeeRepository iERPEmployeeRepository = (base.ERPEmployeeRepository = new ERPEmployeeRepository(base.ApiClientContext));
			using (iERPEmployeeRepository)
			{
				ERPEmployeeInformationDto eRPEmployeeInformationDto = await base.ERPEmployeeRepository.GetEmployee(employeeId);
				employeeDto = new ERPEmployeeDto
				{
					lmeCallTypeID = eRPEmployeeInformationDto.lmeCallTypeID,
					lmeCessationType = eRPEmployeeInformationDto.lmeCessationType,
					lmeEmployeeID = eRPEmployeeInformationDto.lmeEmployeeID,
					lmeCommissionRate = eRPEmployeeInformationDto.lmeCommissionRate,
					lmeContactTitleID = eRPEmployeeInformationDto.lmeContactTitleID,
					lmeCountyCodeID = eRPEmployeeInformationDto.lmeCountyCodeID,
					lmeCreatedBy = eRPEmployeeInformationDto.lmeCreatedBy,
					lmeCreatedDate = eRPEmployeeInformationDto.lmeCreatedDate,
					lmeDefaultShiftID = eRPEmployeeInformationDto.lmeDefaultShiftID,
					lmeDefaultWorkCenterID = eRPEmployeeInformationDto.lmeDefaultWorkCenterID,
					lmeDirectExpenseID = eRPEmployeeInformationDto.lmeDirectExpenseID,
					lmeEarningType = eRPEmployeeInformationDto.lmeEarningType,
					lmeEmployeeName = eRPEmployeeInformationDto.lmeEmployeeName,
					lmeUniqueID = eRPEmployeeInformationDto.lmeUniqueID,
					lmeHireDate = eRPEmployeeInformationDto.lmeHireDate,
					lmeHomeProductionDepartmentID = eRPEmployeeInformationDto.lmeHomeProductionDepartmentID,
					lmeIndirectExpenseID = eRPEmployeeInformationDto.lmeIndirectExpenseID,
					lmeBuyerEmployee = eRPEmployeeInformationDto.lmeBuyerEmployee,
					lmeEngineerEmployee = eRPEmployeeInformationDto.lmeEngineerEmployee,
					lmeInspectorEmployee = eRPEmployeeInformationDto.lmeInspectorEmployee,
					lmeLockShift = eRPEmployeeInformationDto.lmeLockShift,
					lmePayrollEmployee = eRPEmployeeInformationDto.lmePayrollEmployee,
					lmePlannerEmployee = eRPEmployeeInformationDto.lmePlannerEmployee,
					lmeProjectManagerEmployee = eRPEmployeeInformationDto.lmeProjectManagerEmployee,
					lmeQuoterEmployee = eRPEmployeeInformationDto.lmeQuoterEmployee,
					lmeSalesEmployee = eRPEmployeeInformationDto.lmeSalesEmployee,
					lmeShopEmployee = eRPEmployeeInformationDto.lmeShopEmployee,
					lmeSortSfebyWorkcenter = eRPEmployeeInformationDto.lmeSortSfebyWorkcenter,
					lmeSupportEmployee = eRPEmployeeInformationDto.lmeSupportEmployee,
					lmeLanguage = eRPEmployeeInformationDto.lmeLanguage,
					lmePassword = eRPEmployeeInformationDto.lmePassword,
					lmePlantDepartmentID = eRPEmployeeInformationDto.lmePlantDepartmentID,
					lmePlantID = eRPEmployeeInformationDto.lmePlantID,
					lmePoApprovalAmount = eRPEmployeeInformationDto.lmePoApprovalAmount,
					lmePreviousEmployeeID = eRPEmployeeInformationDto.lmePreviousEmployeeID,
					lmeRowVersion = eRPEmployeeInformationDto.lmeRowVersion,
					lmeSOApprovalAmount = eRPEmployeeInformationDto.lmeSOApprovalAmount,
					lmeTerminationDate = eRPEmployeeInformationDto.lmeTerminationDate,
					lmeTerminationReasonID = eRPEmployeeInformationDto.lmeTerminationReasonID,
					lmeUseEmail = eRPEmployeeInformationDto.lmeUseEmail,
					lmeUseEmailPayslips = eRPEmployeeInformationDto.lmeUseEmailPayslips,
					lmeUserID = eRPEmployeeInformationDto.lmeUserID,
					lmeWorkEmailAddress = eRPEmployeeInformationDto.lmeWorkEmailAddress,
					CustomFields = eRPEmployeeInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Employees []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = employeeDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeDto>> Process_PutEmployee(ERPEmployeeDto employee)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPEmployeeDto createdObject = null;
		ERPResponseMessageDto<ERPEmployeeDto> result;
		try
		{
			IERPEmployeeRepository iERPEmployeeRepository = (base.ERPEmployeeRepository = new ERPEmployeeRepository(base.ApiClientContext));
			using (iERPEmployeeRepository)
			{
				APIValidationInfoDto postResult = await base.ERPEmployeeRepository.SaveEmployee(employee);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPEmployeeInformationDto eRPEmployeeInformationDto = await base.ERPEmployeeRepository.GetEmployee(employee.lmeUniqueID);
					createdObject = new ERPEmployeeDto
					{
						lmeCallTypeID = eRPEmployeeInformationDto.lmeCallTypeID,
						lmeCessationType = eRPEmployeeInformationDto.lmeCessationType,
						lmeEmployeeID = eRPEmployeeInformationDto.lmeEmployeeID,
						lmeCommissionRate = eRPEmployeeInformationDto.lmeCommissionRate,
						lmeContactTitleID = eRPEmployeeInformationDto.lmeContactTitleID,
						lmeCountyCodeID = eRPEmployeeInformationDto.lmeCountyCodeID,
						lmeCreatedBy = eRPEmployeeInformationDto.lmeCreatedBy,
						lmeCreatedDate = eRPEmployeeInformationDto.lmeCreatedDate,
						lmeDefaultShiftID = eRPEmployeeInformationDto.lmeDefaultShiftID,
						lmeDefaultWorkCenterID = eRPEmployeeInformationDto.lmeDefaultWorkCenterID,
						lmeDirectExpenseID = eRPEmployeeInformationDto.lmeDirectExpenseID,
						lmeEarningType = eRPEmployeeInformationDto.lmeEarningType,
						lmeEmployeeName = eRPEmployeeInformationDto.lmeEmployeeName,
						lmeUniqueID = eRPEmployeeInformationDto.lmeUniqueID,
						lmeHireDate = eRPEmployeeInformationDto.lmeHireDate,
						lmeHomeProductionDepartmentID = eRPEmployeeInformationDto.lmeHomeProductionDepartmentID,
						lmeIndirectExpenseID = eRPEmployeeInformationDto.lmeIndirectExpenseID,
						lmeBuyerEmployee = eRPEmployeeInformationDto.lmeBuyerEmployee,
						lmeEngineerEmployee = eRPEmployeeInformationDto.lmeEngineerEmployee,
						lmeInspectorEmployee = eRPEmployeeInformationDto.lmeInspectorEmployee,
						lmeLockShift = eRPEmployeeInformationDto.lmeLockShift,
						lmePayrollEmployee = eRPEmployeeInformationDto.lmePayrollEmployee,
						lmePlannerEmployee = eRPEmployeeInformationDto.lmePlannerEmployee,
						lmeProjectManagerEmployee = eRPEmployeeInformationDto.lmeProjectManagerEmployee,
						lmeQuoterEmployee = eRPEmployeeInformationDto.lmeQuoterEmployee,
						lmeSalesEmployee = eRPEmployeeInformationDto.lmeSalesEmployee,
						lmeShopEmployee = eRPEmployeeInformationDto.lmeShopEmployee,
						lmeSortSfebyWorkcenter = eRPEmployeeInformationDto.lmeSortSfebyWorkcenter,
						lmeSupportEmployee = eRPEmployeeInformationDto.lmeSupportEmployee,
						lmeLanguage = eRPEmployeeInformationDto.lmeLanguage,
						lmePassword = eRPEmployeeInformationDto.lmePassword,
						lmePlantDepartmentID = eRPEmployeeInformationDto.lmePlantDepartmentID,
						lmePlantID = eRPEmployeeInformationDto.lmePlantID,
						lmePoApprovalAmount = eRPEmployeeInformationDto.lmePoApprovalAmount,
						lmePreviousEmployeeID = eRPEmployeeInformationDto.lmePreviousEmployeeID,
						lmeRowVersion = eRPEmployeeInformationDto.lmeRowVersion,
						lmeSOApprovalAmount = eRPEmployeeInformationDto.lmeSOApprovalAmount,
						lmeTerminationDate = eRPEmployeeInformationDto.lmeTerminationDate,
						lmeTerminationReasonID = eRPEmployeeInformationDto.lmeTerminationReasonID,
						lmeUseEmail = eRPEmployeeInformationDto.lmeUseEmail,
						lmeUseEmailPayslips = eRPEmployeeInformationDto.lmeUseEmailPayslips,
						lmeUserID = eRPEmployeeInformationDto.lmeUserID,
						lmeWorkEmailAddress = eRPEmployeeInformationDto.lmeWorkEmailAddress,
						CustomFields = eRPEmployeeInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Employee [{employee.lmeUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteEmployee(Guid employeeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeRepository iERPEmployeeRepository = (base.ERPEmployeeRepository = new ERPEmployeeRepository(base.ApiClientContext));
		using (iERPEmployeeRepository)
		{
			if (!(await base.ERPEmployeeRepository.DoesEmployeeExist(employeeId)))
			{
				base.ErrorsList.Add($"Employee [{employeeId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPEmployeeInformationDto eRPEmployeeInformationDto = await base.ERPEmployeeRepository.GetEmployee(employeeId);
				string text = await base.ERPEmployeeRepository.WhereUsed("Employees", new object[1] { eRPEmployeeInformationDto.lmeEmployeeID }, new object[1] { "lmeEmployeeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Employee cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeDto>> Process_DeleteEmployee(Guid employeeId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPEmployeeDto> result;
		try
		{
			IERPEmployeeRepository iERPEmployeeRepository = (base.ERPEmployeeRepository = new ERPEmployeeRepository(base.ApiClientContext));
			using (iERPEmployeeRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPEmployeeRepository.DeleteRowFromTable("Employees", "lme", employeeId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Employee [{employeeId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPEmployeeDto()
			};
		}
		return result;
	}
}
