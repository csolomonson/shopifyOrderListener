using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPMilestoneRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Milestone with the specified Unique Id exists.
	/// </summary>
	/// <param name="milestoneId">The Unique Id of the Milestone to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Milestone exists or not.</returns>
	Task<bool> DoesMilestoneExist(Guid milestoneId);

	/// <summary>
	/// Retrieves all Milestones with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Milestones to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Milestones DTOs.</returns>
	Task<ICollection<ERPMilestoneInformationDto>> GetAllMilestones(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Milestone.
	/// </summary>
	/// <param name="milestoneId">The Unique Id of the Milestone to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Milestone DTO.</returns>
	Task<ERPMilestoneInformationDto> GetMilestone(Guid milestoneId);

	/// <summary>
	/// Saves the provided ERP milestone.
	/// </summary>
	/// <param name="milestone">The ERP milestone to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveMilestone(ERPMilestoneDto milestone);
}
