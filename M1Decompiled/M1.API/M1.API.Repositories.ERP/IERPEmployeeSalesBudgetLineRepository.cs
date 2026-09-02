using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPEmployeeSalesBudgetLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a EmployeeSalesBudgetLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="employeeSalesBudgetLineId">The Unique Id of the EmployeeSalesBudgetLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the EmployeeSalesBudgetLine exists or not.</returns>
	Task<bool> DoesEmployeeSalesBudgetLineExist(Guid employeeSalesBudgetLineId);

	/// <summary>
	/// Retrieves all EmployeeSalesBudgetLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeSalesBudgetLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeSalesBudgetLines DTOs.</returns>
	Task<ICollection<ERPEmployeeSalesBudgetLineInformationDto>> GetAllEmployeeSalesBudgetLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific EmployeeSalesBudgetLine.
	/// </summary>
	/// <param name="employeeSalesBudgetLineId">The Unique Id of the EmployeeSalesBudgetLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the EmployeeSalesBudgetLine DTO.</returns>
	Task<ERPEmployeeSalesBudgetLineInformationDto> GetEmployeeSalesBudgetLine(Guid employeeSalesBudgetLineId);
}
