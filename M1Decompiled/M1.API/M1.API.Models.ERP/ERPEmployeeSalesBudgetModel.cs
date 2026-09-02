using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPEmployeeSalesBudgetModel : ERPBaseModel, IERPEmployeeSalesBudgetModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeSalesBudgets(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPEmployeeSalesBudgetRepository iERPEmployeeSalesBudgetRepository = (base.ERPEmployeeSalesBudgetRepository = new ERPEmployeeSalesBudgetRepository(base.ApiClientContext));
		using (iERPEmployeeSalesBudgetRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPEmployeeSalesBudgetRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPEmployeeSalesBudgetRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPEmployeeSalesBudgetRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPEmployeeSalesBudgetRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetEmployeeSalesBudget(Guid employeeSalesBudgetId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeSalesBudgetRepository iERPEmployeeSalesBudgetRepository = (base.ERPEmployeeSalesBudgetRepository = new ERPEmployeeSalesBudgetRepository(base.ApiClientContext));
		using (iERPEmployeeSalesBudgetRepository)
		{
			if (!(await base.ERPEmployeeSalesBudgetRepository.DoesEmployeeSalesBudgetExist(employeeSalesBudgetId)))
			{
				errorsList.Add($"EmployeeSalesBudget [{employeeSalesBudgetId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPEmployeeSalesBudgetDto>>> Process_GetAllEmployeeSalesBudgets(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPEmployeeSalesBudgetDto> allEmployeeSalesBudgetsDto = new List<ERPEmployeeSalesBudgetDto>();
		ERPResponseMessageDto<IList<ERPEmployeeSalesBudgetDto>> result;
		try
		{
			IERPEmployeeSalesBudgetRepository iERPEmployeeSalesBudgetRepository = (base.ERPEmployeeSalesBudgetRepository = new ERPEmployeeSalesBudgetRepository(base.ApiClientContext));
			using (iERPEmployeeSalesBudgetRepository)
			{
				foreach (ERPEmployeeSalesBudgetInformationDto item2 in await base.ERPEmployeeSalesBudgetRepository.GetAllEmployeeSalesBudgets(pageSize, pageNumber, filter, orderBy))
				{
					ERPEmployeeSalesBudgetDto item = new ERPEmployeeSalesBudgetDto
					{
						lnsAnnualAmount = item2.lnsAnnualAmount,
						lnsEmployeeID = item2.lnsEmployeeID,
						lnsEndDate = item2.lnsEndDate,
						lnsUniqueID = item2.lnsUniqueID,
						lnsRowVersion = item2.lnsRowVersion,
						lnsSalesBudgetYearID = item2.lnsSalesBudgetYearID,
						lnsStartDate = item2.lnsStartDate,
						CustomFields = item2.CustomFields
					};
					allEmployeeSalesBudgetsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all EmployeeSalesBudgets]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPEmployeeSalesBudgetDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allEmployeeSalesBudgetsDto,
				RecordCount = allEmployeeSalesBudgetsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeSalesBudgetDto>> Process_GetEmployeeSalesBudget(Guid employeeSalesBudgetId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPEmployeeSalesBudgetDto employeeSalesBudgetDto = null;
		ERPResponseMessageDto<ERPEmployeeSalesBudgetDto> result;
		try
		{
			IERPEmployeeSalesBudgetRepository iERPEmployeeSalesBudgetRepository = (base.ERPEmployeeSalesBudgetRepository = new ERPEmployeeSalesBudgetRepository(base.ApiClientContext));
			using (iERPEmployeeSalesBudgetRepository)
			{
				ERPEmployeeSalesBudgetInformationDto eRPEmployeeSalesBudgetInformationDto = await base.ERPEmployeeSalesBudgetRepository.GetEmployeeSalesBudget(employeeSalesBudgetId);
				employeeSalesBudgetDto = new ERPEmployeeSalesBudgetDto
				{
					lnsAnnualAmount = eRPEmployeeSalesBudgetInformationDto.lnsAnnualAmount,
					lnsEmployeeID = eRPEmployeeSalesBudgetInformationDto.lnsEmployeeID,
					lnsEndDate = eRPEmployeeSalesBudgetInformationDto.lnsEndDate,
					lnsUniqueID = eRPEmployeeSalesBudgetInformationDto.lnsUniqueID,
					lnsRowVersion = eRPEmployeeSalesBudgetInformationDto.lnsRowVersion,
					lnsSalesBudgetYearID = eRPEmployeeSalesBudgetInformationDto.lnsSalesBudgetYearID,
					lnsStartDate = eRPEmployeeSalesBudgetInformationDto.lnsStartDate,
					CustomFields = eRPEmployeeSalesBudgetInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the EmployeeSalesBudgets []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeSalesBudgetDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = employeeSalesBudgetDto
			};
		}
		return result;
	}
}
