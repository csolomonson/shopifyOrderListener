using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPShiftRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Shift with the specified Unique Id exists.
	/// </summary>
	/// <param name="shiftId">The Unique Id of the Shift to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Shift exists or not.</returns>
	Task<bool> DoesShiftExist(Guid shiftId);

	/// <summary>
	/// Retrieves all Shifts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Shifts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Shifts DTOs.</returns>
	Task<ICollection<ERPShiftInformationDto>> GetAllShifts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Shift.
	/// </summary>
	/// <param name="shiftId">The Unique Id of the Shift to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Shift DTO.</returns>
	Task<ERPShiftInformationDto> GetShift(Guid shiftId);
}
