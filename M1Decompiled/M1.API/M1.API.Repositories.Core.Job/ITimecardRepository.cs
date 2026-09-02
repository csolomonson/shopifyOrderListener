using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM.Job;

namespace M1.API.Repositories.Core.Job;

public interface ITimecardRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Timecard with the specified ID exists.
	/// </summary>
	/// <param name="timecardId">The ID of the Timecard to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Timecard exists or not.</returns>
	Task<bool> DoesTimecardExistsAsync(string timecardId);

	/// <summary>
	/// Checks if a Timecard with the specified ID exists.
	/// </summary>
	/// <param name="timecardId">The ID of the Timecard to check.</param>
	/// <param name="employeeId">The ID of Employee to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Timecard exists or not.</returns>
	Task<bool> DoesTimecardExistsAsync(string timecardId, string employeeId);

	/// <summary>
	/// Retrieves all Timecard with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Timecards to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of Timecards DTOs.</returns>
	Task<ICollection<BOMTimecardDto>> GetAllTimecards(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed information about a specific Timecard.
	/// </summary>
	/// <param name="timecardId">The ID of the Timecard to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Timecard DTO.</returns>
	Task<BOMTimecardDto> GetTimecard(string timecardId);

	/// <summary>
	/// Retrieves detailed information about a specific Timecard.
	/// </summary>
	/// <param name="timecardId">The ID of the Timecard to retrieve information for.</param>
	/// <param name="employeeId">The ID of Employee to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Timecard DTO.</returns>
	Task<BOMTimecardDto> GetTimecard(string timecardId, string employeeId);
}
