using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPLeadCompetitorRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a LeadCompetitor with the specified Unique Id exists.
	/// </summary>
	/// <param name="leadCompetitorId">The Unique Id of the LeadCompetitor to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the LeadCompetitor exists or not.</returns>
	Task<bool> DoesLeadCompetitorExist(Guid leadCompetitorId);

	/// <summary>
	/// Retrieves all LeadCompetitors with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LeadCompetitors to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LeadCompetitors DTOs.</returns>
	Task<ICollection<ERPLeadCompetitorInformationDto>> GetAllLeadCompetitors(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific LeadCompetitor.
	/// </summary>
	/// <param name="leadCompetitorId">The Unique Id of the LeadCompetitor to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the LeadCompetitor DTO.</returns>
	Task<ERPLeadCompetitorInformationDto> GetLeadCompetitor(Guid leadCompetitorId);

	/// <summary>
	/// Saves the provided ERP leadCompetitor.
	/// </summary>
	/// <param name="leadCompetitor">The ERP leadCompetitor to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveLeadCompetitor(ERPLeadCompetitorDto leadCompetitor);
}
