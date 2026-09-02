using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPProductionPropertyRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ProductionProperty with the specified Unique Id exists.
	/// </summary>
	/// <param name="productionPropertyId">The Unique Id of the ProductionProperty to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ProductionProperty exists or not.</returns>
	Task<bool> DoesProductionPropertyExist(Guid productionPropertyId);

	/// <summary>
	/// Retrieves all ProductionProperties with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductionProperties to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductionProperties DTOs.</returns>
	Task<ICollection<ERPProductionPropertyInformationDto>> GetAllProductionProperties(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ProductionProperty.
	/// </summary>
	/// <param name="productionPropertyId">The Unique Id of the ProductionProperty to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ProductionProperty DTO.</returns>
	Task<ERPProductionPropertyInformationDto> GetProductionProperty(Guid productionPropertyId);
}
