using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPReasonPlantRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ReasonPlant with the specified Unique Id exists.
	/// </summary>
	/// <param name="reasonPlantId">The Unique Id of the ReasonPlant to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ReasonPlant exists or not.</returns>
	Task<bool> DoesReasonPlantExist(Guid reasonPlantId);

	/// <summary>
	/// Retrieves all ReasonPlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ReasonPlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ReasonPlants DTOs.</returns>
	Task<ICollection<ERPReasonPlantInformationDto>> GetAllReasonPlants(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ReasonPlant.
	/// </summary>
	/// <param name="reasonPlantId">The Unique Id of the ReasonPlant to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ReasonPlant DTO.</returns>
	Task<ERPReasonPlantInformationDto> GetReasonPlant(Guid reasonPlantId);
}
