using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPEmployeeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Employee with the specified Unique Id exists.
	/// </summary>
	/// <param name="employeeId">The Unique Id of the Employee to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Employee exists or not.</returns>
	Task<bool> DoesEmployeeExist(Guid employeeId);

	/// <summary>
	/// Retrieves all Employees with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Employees to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Employees DTOs.</returns>
	Task<ICollection<ERPEmployeeInformationDto>> GetAllEmployees(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Employee.
	/// </summary>
	/// <param name="employeeId">The Unique Id of the Employee to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Employee DTO.</returns>
	Task<ERPEmployeeInformationDto> GetEmployee(Guid employeeId);

	/// <summary>
	/// Saves the provided ERP employee.
	/// </summary>
	/// <param name="employee">The ERP employee to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveEmployee(ERPEmployeeDto employee);
}
