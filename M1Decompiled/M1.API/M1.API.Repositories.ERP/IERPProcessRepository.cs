using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPProcessRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Process with the specified Unique Id exists.
	/// </summary>
	/// <param name="processId">The Unique Id of the Process to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Process exists or not.</returns>
	Task<bool> DoesProcessExist(Guid processId);

	/// <summary>
	/// Retrieves all Processes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Processes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Processes DTOs.</returns>
	Task<ICollection<ERPProcessInformationDto>> GetAllProcesses(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Process.
	/// </summary>
	/// <param name="processId">The Unique Id of the Process to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Process DTO.</returns>
	Task<ERPProcessInformationDto> GetProcess(Guid processId);

	/// <summary>
	/// Saves the provided ERP process.
	/// </summary>
	/// <param name="process">The ERP process to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveProcess(ERPProcessDto process);
}
