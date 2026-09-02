using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPFollowupRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Followup with the specified Unique Id exists.
	/// </summary>
	/// <param name="followupId">The Unique Id of the Followup to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Followup exists or not.</returns>
	Task<bool> DoesFollowupExist(Guid followupId);

	/// <summary>
	/// Retrieves all Followups with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Followups to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Followups DTOs.</returns>
	Task<ICollection<ERPFollowupInformationDto>> GetAllFollowups(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Followup.
	/// </summary>
	/// <param name="followupId">The Unique Id of the Followup to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Followup DTO.</returns>
	Task<ERPFollowupInformationDto> GetFollowup(Guid followupId);

	/// <summary>
	/// Saves the provided ERP followup.
	/// </summary>
	/// <param name="followup">The ERP followup to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveFollowup(ERPFollowupDto followup);
}
