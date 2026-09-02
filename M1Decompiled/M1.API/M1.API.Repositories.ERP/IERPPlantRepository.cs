using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPlantRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Plant with the specified Unique Id exists.
	/// </summary>
	/// <param name="plantId">The Unique Id of the Plant to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Plant exists or not.</returns>
	Task<bool> DoesPlantExist(Guid plantId);

	/// <summary>
	/// Retrieves all Plants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Plants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Plants DTOs.</returns>
	Task<ICollection<ERPPlantInformationDto>> GetAllPlants(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Plant.
	/// </summary>
	/// <param name="plantId">The Unique Id of the Plant to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Plant DTO.</returns>
	Task<ERPPlantInformationDto> GetPlant(Guid plantId);

	/// <summary>
	/// Saves the provided ERP plant.
	/// </summary>
	/// <param name="plant">The ERP plant to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePlant(ERPPlantDto plant);
}
