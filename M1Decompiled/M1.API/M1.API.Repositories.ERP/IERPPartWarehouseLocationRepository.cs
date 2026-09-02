using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartWarehouseLocationRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartWarehouseLocation with the specified Unique Id exists.
	/// </summary>
	/// <param name="partWarehouseLocationId">The Unique Id of the PartWarehouseLocation to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartWarehouseLocation exists or not.</returns>
	Task<bool> DoesPartWarehouseLocationExist(Guid partWarehouseLocationId);

	/// <summary>
	/// Retrieves all PartWarehouseLocations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartWarehouseLocations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartWarehouseLocations DTOs.</returns>
	Task<ICollection<ERPPartWarehouseLocationInformationDto>> GetAllPartWarehouseLocations(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartWarehouseLocation.
	/// </summary>
	/// <param name="partWarehouseLocationId">The Unique Id of the PartWarehouseLocation to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartWarehouseLocation DTO.</returns>
	Task<ERPPartWarehouseLocationInformationDto> GetPartWarehouseLocation(Guid partWarehouseLocationId);

	/// <summary>
	/// Saves the provided ERP partWarehouseLocation.
	/// </summary>
	/// <param name="partWarehouseLocation">The ERP partWarehouseLocation to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartWarehouseLocation(ERPPartWarehouseLocationDto partWarehouseLocation);
}
