using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPProductionCalendarWorkCenterRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ProductionCalendarWorkCenter with the specified Unique Id exists.
	/// </summary>
	/// <param name="productionCalendarWorkCenterId">The Unique Id of the ProductionCalendarWorkCenter to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ProductionCalendarWorkCenter exists or not.</returns>
	Task<bool> DoesProductionCalendarWorkCenterExist(Guid productionCalendarWorkCenterId);

	/// <summary>
	/// Retrieves all ProductionCalendarWorkCenters with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionCalendarWorkCenters to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductionCalendarWorkCenters DTOs.</returns>
	Task<ICollection<ERPProductionCalendarWorkCenterInformationDto>> GetAllProductionCalendarWorkCenters(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ProductionCalendarWorkCenter.
	/// </summary>
	/// <param name="productionCalendarWorkCenterId">The Unique Id of the ProductionCalendarWorkCenter to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ProductionCalendarWorkCenter DTO.</returns>
	Task<ERPProductionCalendarWorkCenterInformationDto> GetProductionCalendarWorkCenter(Guid productionCalendarWorkCenterId);
}
