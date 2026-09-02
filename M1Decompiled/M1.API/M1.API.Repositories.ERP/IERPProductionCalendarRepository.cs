using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPProductionCalendarRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ProductionCalendar with the specified Unique Id exists.
	/// </summary>
	/// <param name="productionCalendarId">The Unique Id of the ProductionCalendar to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ProductionCalendar exists or not.</returns>
	Task<bool> DoesProductionCalendarExist(Guid productionCalendarId);

	/// <summary>
	/// Retrieves all ProductionCalendars with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionCalendars to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductionCalendars DTOs.</returns>
	Task<ICollection<ERPProductionCalendarInformationDto>> GetAllProductionCalendars(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ProductionCalendar.
	/// </summary>
	/// <param name="productionCalendarId">The Unique Id of the ProductionCalendar to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ProductionCalendar DTO.</returns>
	Task<ERPProductionCalendarInformationDto> GetProductionCalendar(Guid productionCalendarId);
}
