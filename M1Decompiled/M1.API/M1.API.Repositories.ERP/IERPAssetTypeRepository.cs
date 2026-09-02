using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAssetTypeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a AssetType with the specified Unique Id exists.
	/// </summary>
	/// <param name="assetTypeId">The Unique Id of the AssetType to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the AssetType exists or not.</returns>
	Task<bool> DoesAssetTypeExist(Guid assetTypeId);

	/// <summary>
	/// Retrieves all AssetTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetTypes DTOs.</returns>
	Task<ICollection<ERPAssetTypeInformationDto>> GetAllAssetTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific AssetType.
	/// </summary>
	/// <param name="assetTypeId">The Unique Id of the AssetType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the AssetType DTO.</returns>
	Task<ERPAssetTypeInformationDto> GetAssetType(Guid assetTypeId);
}
