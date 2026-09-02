using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPShiftBreakRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ShiftBreak with the specified Unique Id exists.
	/// </summary>
	/// <param name="shiftBreakId">The Unique Id of the ShiftBreak to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ShiftBreak exists or not.</returns>
	Task<bool> DoesShiftBreakExist(Guid shiftBreakId);

	/// <summary>
	/// Retrieves all ShiftBreaks with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShiftBreaks to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShiftBreaks DTOs.</returns>
	Task<ICollection<ERPShiftBreakInformationDto>> GetAllShiftBreaks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ShiftBreak.
	/// </summary>
	/// <param name="shiftBreakId">The Unique Id of the ShiftBreak to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ShiftBreak DTO.</returns>
	Task<ERPShiftBreakInformationDto> GetShiftBreak(Guid shiftBreakId);
}
