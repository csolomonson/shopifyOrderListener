using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPEmployeeSalesBudgetRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a EmployeeSalesBudget with the specified Unique Id exists.
	/// </summary>
	/// <param name="employeeSalesBudgetId">The Unique Id of the EmployeeSalesBudget to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the EmployeeSalesBudget exists or not.</returns>
	Task<bool> DoesEmployeeSalesBudgetExist(Guid employeeSalesBudgetId);

	/// <summary>
	/// Retrieves all EmployeeSalesBudgets with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeSalesBudgets to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeSalesBudgets DTOs.</returns>
	Task<ICollection<ERPEmployeeSalesBudgetInformationDto>> GetAllEmployeeSalesBudgets(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific EmployeeSalesBudget.
	/// </summary>
	/// <param name="employeeSalesBudgetId">The Unique Id of the EmployeeSalesBudget to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the EmployeeSalesBudget DTO.</returns>
	Task<ERPEmployeeSalesBudgetInformationDto> GetEmployeeSalesBudget(Guid employeeSalesBudgetId);
}
