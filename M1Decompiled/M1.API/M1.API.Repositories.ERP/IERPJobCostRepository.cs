using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPJobCostRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a JobCost with the specified Unique Id exists.
	/// </summary>
	/// <param name="jobCostId">The Unique Id of the JobCost to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the JobCost exists or not.</returns>
	Task<bool> DoesJobCostExist(Guid jobCostId);

	/// <summary>
	/// Retrieves all JobCosts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobCosts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobCosts DTOs.</returns>
	Task<ICollection<ERPJobCostInformationDto>> GetAllJobCosts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific JobCost.
	/// </summary>
	/// <param name="jobCostId">The Unique Id of the JobCost to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the JobCost DTO.</returns>
	Task<ERPJobCostInformationDto> GetJobCost(Guid jobCostId);

	/// <summary>
	/// Saves the provided ERP jobCost.
	/// </summary>
	/// <param name="jobCost">The ERP jobCost to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveJobCost(ERPJobCostDto jobCost);
}
