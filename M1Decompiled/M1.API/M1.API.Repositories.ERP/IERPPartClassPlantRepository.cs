using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartClassPlantRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartClassPlant with the specified Unique Id exists.
	/// </summary>
	/// <param name="partClassPlantId">The Unique Id of the PartClassPlant to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartClassPlant exists or not.</returns>
	Task<bool> DoesPartClassPlantExist(Guid partClassPlantId);

	/// <summary>
	/// Retrieves all PartClassPlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartClassPlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartClassPlants DTOs.</returns>
	Task<ICollection<ERPPartClassPlantInformationDto>> GetAllPartClassPlants(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartClassPlant.
	/// </summary>
	/// <param name="partClassPlantId">The Unique Id of the PartClassPlant to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartClassPlant DTO.</returns>
	Task<ERPPartClassPlantInformationDto> GetPartClassPlant(Guid partClassPlantId);
}
