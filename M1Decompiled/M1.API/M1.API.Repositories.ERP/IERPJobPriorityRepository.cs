using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPJobPriorityRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a JobPriority with the specified Unique Id exists.
	/// </summary>
	/// <param name="jobPriorityId">The Unique Id of the JobPriority to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the JobPriority exists or not.</returns>
	Task<bool> DoesJobPriorityExist(Guid jobPriorityId);

	/// <summary>
	/// Retrieves all JobPriorities with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobPriorities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobPriorities DTOs.</returns>
	Task<ICollection<ERPJobPriorityInformationDto>> GetAllJobPriorities(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific JobPriority.
	/// </summary>
	/// <param name="jobPriorityId">The Unique Id of the JobPriority to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the JobPriority DTO.</returns>
	Task<ERPJobPriorityInformationDto> GetJobPriority(Guid jobPriorityId);

	/// <summary>
	/// Saves the provided ERP jobPriority.
	/// </summary>
	/// <param name="jobPriority">The ERP jobPriority to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveJobPriority(ERPJobPriorityDto jobPriority);
}
