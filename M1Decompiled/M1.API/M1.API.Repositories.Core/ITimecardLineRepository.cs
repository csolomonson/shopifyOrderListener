using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;

namespace M1.API.Repositories.Core;

public interface ITimecardLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a TimecardLine with the specified ID exists.
	/// </summary>
	/// <param name="timecardId">The ID of the Timecard to check.</param>
	/// <param name="timecardLineId">The ID of the TimecardLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the TimecardLine exists or not.</returns>
	Task<bool> DoesTimecardLineExists(string timecardId, string timecardLineId);

	/// <summary>
	/// Retrieves all TimecardLine with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TimecardLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of TimecardLines DTOs.</returns>
	Task<ICollection<BOMTimecardLineDto>> GetAllTimecardLines(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed information about a specific TimecardLine.
	/// </summary>
	/// <param name="timecardId">The ID of the Timecard to retrieve information for.</param>
	/// <param name="timecardLineId">The ID of the TimecardLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the TimecardLine DTO.</returns>
	Task<BOMTimecardLineDto> GetTimecardLine(string timecardId, string timecardLineId);
}
