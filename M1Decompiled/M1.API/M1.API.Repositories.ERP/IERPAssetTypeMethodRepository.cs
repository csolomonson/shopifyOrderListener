using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAssetTypeMethodRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a AssetTypeMethod with the specified Unique Id exists.
	/// </summary>
	/// <param name="assetTypeMethodId">The Unique Id of the AssetTypeMethod to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the AssetTypeMethod exists or not.</returns>
	Task<bool> DoesAssetTypeMethodExist(Guid assetTypeMethodId);

	/// <summary>
	/// Retrieves all AssetTypeMethods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetTypeMethods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetTypeMethods DTOs.</returns>
	Task<ICollection<ERPAssetTypeMethodInformationDto>> GetAllAssetTypeMethods(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific AssetTypeMethod.
	/// </summary>
	/// <param name="assetTypeMethodId">The Unique Id of the AssetTypeMethod to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the AssetTypeMethod DTO.</returns>
	Task<ERPAssetTypeMethodInformationDto> GetAssetTypeMethod(Guid assetTypeMethodId);
}
