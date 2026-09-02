using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPEmployeeMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a EmployeeMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="employeeMemoId">The Unique Id of the EmployeeMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the EmployeeMemo exists or not.</returns>
	Task<bool> DoesEmployeeMemoExist(Guid employeeMemoId);

	/// <summary>
	/// Retrieves all EmployeeMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of EmployeeMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of EmployeeMemos DTOs.</returns>
	Task<ICollection<ERPEmployeeMemoInformationDto>> GetAllEmployeeMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific EmployeeMemo.
	/// </summary>
	/// <param name="employeeMemoId">The Unique Id of the EmployeeMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the EmployeeMemo DTO.</returns>
	Task<ERPEmployeeMemoInformationDto> GetEmployeeMemo(Guid employeeMemoId);

	/// <summary>
	/// Saves the provided ERP employeeMemo.
	/// </summary>
	/// <param name="employeeMemo">The ERP employeeMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveEmployeeMemo(ERPEmployeeMemoDto employeeMemo);
}
