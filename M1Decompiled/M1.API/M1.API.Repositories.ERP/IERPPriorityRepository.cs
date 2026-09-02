using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPriorityRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Priority with the specified Unique Id exists.
	/// </summary>
	/// <param name="priorityId">The Unique Id of the Priority to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Priority exists or not.</returns>
	Task<bool> DoesPriorityExist(Guid priorityId);

	/// <summary>
	/// Retrieves all Priorities with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Priorities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Priorities DTOs.</returns>
	Task<ICollection<ERPPriorityInformationDto>> GetAllPriorities(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Priority.
	/// </summary>
	/// <param name="priorityId">The Unique Id of the Priority to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Priority DTO.</returns>
	Task<ERPPriorityInformationDto> GetPriority(Guid priorityId);

	/// <summary>
	/// Saves the provided ERP priority.
	/// </summary>
	/// <param name="priority">The ERP priority to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePriority(ERPPriorityDto priority);
}
