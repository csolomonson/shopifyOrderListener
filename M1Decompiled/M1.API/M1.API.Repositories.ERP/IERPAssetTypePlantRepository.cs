using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAssetTypePlantRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a AssetTypePlant with the specified Unique Id exists.
	/// </summary>
	/// <param name="assetTypePlantId">The Unique Id of the AssetTypePlant to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the AssetTypePlant exists or not.</returns>
	Task<bool> DoesAssetTypePlantExist(Guid assetTypePlantId);

	/// <summary>
	/// Retrieves all AssetTypePlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetTypePlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetTypePlants DTOs.</returns>
	Task<ICollection<ERPAssetTypePlantInformationDto>> GetAllAssetTypePlants(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific AssetTypePlant.
	/// </summary>
	/// <param name="assetTypePlantId">The Unique Id of the AssetTypePlant to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the AssetTypePlant DTO.</returns>
	Task<ERPAssetTypePlantInformationDto> GetAssetTypePlant(Guid assetTypePlantId);
}
