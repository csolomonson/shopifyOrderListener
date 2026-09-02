using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPTimecardRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Timecard with the specified Unique Id exists.
	/// </summary>
	/// <param name="timecardId">The Unique Id of the Timecard to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Timecard exists or not.</returns>
	Task<bool> DoesTimecardExist(Guid timecardId);

	/// <summary>
	/// Retrieves all Timecards with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Timecards to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Timecards DTOs.</returns>
	Task<ICollection<ERPTimecardInformationDto>> GetAllTimecards(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Timecard.
	/// </summary>
	/// <param name="timecardId">The Unique Id of the Timecard to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Timecard DTO.</returns>
	Task<ERPTimecardInformationDto> GetTimecard(Guid timecardId);

	/// <summary>
	/// Saves the provided ERP timecard.
	/// </summary>
	/// <param name="timecard">The ERP timecard to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveTimecard(ERPTimecardDto timecard);
}
