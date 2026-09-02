using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPEmployeeSalesBudgetLineModel : ERPBaseModel, IERPEmployeeSalesBudgetLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeSalesBudgetLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPEmployeeSalesBudgetLineRepository iERPEmployeeSalesBudgetLineRepository = (base.ERPEmployeeSalesBudgetLineRepository = new ERPEmployeeSalesBudgetLineRepository(base.ApiClientContext));
		using (iERPEmployeeSalesBudgetLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPEmployeeSalesBudgetLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPEmployeeSalesBudgetLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPEmployeeSalesBudgetLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPEmployeeSalesBudgetLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetEmployeeSalesBudgetLine(Guid employeeSalesBudgetLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeeSalesBudgetLineRepository iERPEmployeeSalesBudgetLineRepository = (base.ERPEmployeeSalesBudgetLineRepository = new ERPEmployeeSalesBudgetLineRepository(base.ApiClientContext));
		using (iERPEmployeeSalesBudgetLineRepository)
		{
			if (!(await base.ERPEmployeeSalesBudgetLineRepository.DoesEmployeeSalesBudgetLineExist(employeeSalesBudgetLineId)))
			{
				errorsList.Add($"EmployeeSalesBudgetLine [{employeeSalesBudgetLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPEmployeeSalesBudgetLineDto>>> Process_GetAllEmployeeSalesBudgetLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPEmployeeSalesBudgetLineDto> allEmployeeSalesBudgetLinesDto = new List<ERPEmployeeSalesBudgetLineDto>();
		ERPResponseMessageDto<IList<ERPEmployeeSalesBudgetLineDto>> result;
		try
		{
			IERPEmployeeSalesBudgetLineRepository iERPEmployeeSalesBudgetLineRepository = (base.ERPEmployeeSalesBudgetLineRepository = new ERPEmployeeSalesBudgetLineRepository(base.ApiClientContext));
			using (iERPEmployeeSalesBudgetLineRepository)
			{
				foreach (ERPEmployeeSalesBudgetLineInformationDto item2 in await base.ERPEmployeeSalesBudgetLineRepository.GetAllEmployeeSalesBudgetLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPEmployeeSalesBudgetLineDto item = new ERPEmployeeSalesBudgetLineDto
					{
						lnlBudgetAmount = item2.lnlBudgetAmount,
						lnlEmployeeID = item2.lnlEmployeeID,
						lnlEndDate = item2.lnlEndDate,
						lnlUniqueID = item2.lnlUniqueID,
						lnlRowVersion = item2.lnlRowVersion,
						lnlSalesBudgetPeriodID = item2.lnlSalesBudgetPeriodID,
						lnlSalesBudgetYearID = item2.lnlSalesBudgetYearID,
						lnlStartDate = item2.lnlStartDate,
						CustomFields = item2.CustomFields
					};
					allEmployeeSalesBudgetLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all EmployeeSalesBudgetLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPEmployeeSalesBudgetLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allEmployeeSalesBudgetLinesDto,
				RecordCount = allEmployeeSalesBudgetLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeeSalesBudgetLineDto>> Process_GetEmployeeSalesBudgetLine(Guid employeeSalesBudgetLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPEmployeeSalesBudgetLineDto employeeSalesBudgetLineDto = null;
		ERPResponseMessageDto<ERPEmployeeSalesBudgetLineDto> result;
		try
		{
			IERPEmployeeSalesBudgetLineRepository iERPEmployeeSalesBudgetLineRepository = (base.ERPEmployeeSalesBudgetLineRepository = new ERPEmployeeSalesBudgetLineRepository(base.ApiClientContext));
			using (iERPEmployeeSalesBudgetLineRepository)
			{
				ERPEmployeeSalesBudgetLineInformationDto eRPEmployeeSalesBudgetLineInformationDto = await base.ERPEmployeeSalesBudgetLineRepository.GetEmployeeSalesBudgetLine(employeeSalesBudgetLineId);
				employeeSalesBudgetLineDto = new ERPEmployeeSalesBudgetLineDto
				{
					lnlBudgetAmount = eRPEmployeeSalesBudgetLineInformationDto.lnlBudgetAmount,
					lnlEmployeeID = eRPEmployeeSalesBudgetLineInformationDto.lnlEmployeeID,
					lnlEndDate = eRPEmployeeSalesBudgetLineInformationDto.lnlEndDate,
					lnlUniqueID = eRPEmployeeSalesBudgetLineInformationDto.lnlUniqueID,
					lnlRowVersion = eRPEmployeeSalesBudgetLineInformationDto.lnlRowVersion,
					lnlSalesBudgetPeriodID = eRPEmployeeSalesBudgetLineInformationDto.lnlSalesBudgetPeriodID,
					lnlSalesBudgetYearID = eRPEmployeeSalesBudgetLineInformationDto.lnlSalesBudgetYearID,
					lnlStartDate = eRPEmployeeSalesBudgetLineInformationDto.lnlStartDate,
					CustomFields = eRPEmployeeSalesBudgetLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the EmployeeSalesBudgetLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeeSalesBudgetLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = employeeSalesBudgetLineDto
			};
		}
		return result;
	}
}
