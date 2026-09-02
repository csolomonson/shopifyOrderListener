using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartGroupPlantRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartGroupPlant with the specified Unique Id exists.
	/// </summary>
	/// <param name="partGroupPlantId">The Unique Id of the PartGroupPlant to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartGroupPlant exists or not.</returns>
	Task<bool> DoesPartGroupPlantExist(Guid partGroupPlantId);

	/// <summary>
	/// Retrieves all PartGroupPlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartGroupPlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartGroupPlants DTOs.</returns>
	Task<ICollection<ERPPartGroupPlantInformationDto>> GetAllPartGroupPlants(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartGroupPlant.
	/// </summary>
	/// <param name="partGroupPlantId">The Unique Id of the PartGroupPlant to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartGroupPlant DTO.</returns>
	Task<ERPPartGroupPlantInformationDto> GetPartGroupPlant(Guid partGroupPlantId);
}
