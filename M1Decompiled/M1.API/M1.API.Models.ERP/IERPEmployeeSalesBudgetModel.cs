using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPEmployeeSalesBudgetModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all EmployeeSalesBudgets with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeSalesBudgets to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeeSalesBudgets(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving EmployeeSalesBudget information based on the specified EmployeeSalesBudget Unique Id.
	/// </summary>
	/// <param name="employeeSalesBudgetId">The Unique Id of the EmployeeSalesBudget.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetEmployeeSalesBudget(Guid employeeSalesBudgetId);

	/// <summary>
	/// Processes the request to retrieve all EmployeeSalesBudgets with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeSalesBudgets to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeSalesBudgets DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPEmployeeSalesBudgetDto>>> Process_GetAllEmployeeSalesBudgets(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific EmployeeSalesBudget.
	/// </summary>
	/// <param name="employeeSalesBudgetId">The Unique Id of the EmployeeSalesBudget to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the EmployeeSalesBudget DTO.</returns>
	Task<ERPResponseMessageDto<ERPEmployeeSalesBudgetDto>> Process_GetEmployeeSalesBudget(Guid employeeSalesBudgetId);
}
