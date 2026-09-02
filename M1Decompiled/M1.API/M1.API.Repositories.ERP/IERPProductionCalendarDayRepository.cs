using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPProductionCalendarDayRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ProductionCalendarDay with the specified Unique Id exists.
	/// </summary>
	/// <param name="productionCalendarDayId">The Unique Id of the ProductionCalendarDay to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ProductionCalendarDay exists or not.</returns>
	Task<bool> DoesProductionCalendarDayExist(Guid productionCalendarDayId);

	/// <summary>
	/// Retrieves all ProductionCalendarDays with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionCalendarDays to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductionCalendarDays DTOs.</returns>
	Task<ICollection<ERPProductionCalendarDayInformationDto>> GetAllProductionCalendarDays(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ProductionCalendarDay.
	/// </summary>
	/// <param name="productionCalendarDayId">The Unique Id of the ProductionCalendarDay to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ProductionCalendarDay DTO.</returns>
	Task<ERPProductionCalendarDayInformationDto> GetProductionCalendarDay(Guid productionCalendarDayId);
}
