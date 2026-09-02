using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAssetLowValuePoolRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a AssetLowValuePool with the specified Unique Id exists.
	/// </summary>
	/// <param name="assetLowValuePoolId">The Unique Id of the AssetLowValuePool to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the AssetLowValuePool exists or not.</returns>
	Task<bool> DoesAssetLowValuePoolExist(Guid assetLowValuePoolId);

	/// <summary>
	/// Retrieves all AssetLowValuePool with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetLowValuePool to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetLowValuePool DTOs.</returns>
	Task<ICollection<ERPAssetLowValuePoolInformationDto>> GetAllAssetLowValuePool(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific AssetLowValuePool.
	/// </summary>
	/// <param name="assetLowValuePoolId">The Unique Id of the AssetLowValuePool to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the AssetLowValuePool DTO.</returns>
	Task<ERPAssetLowValuePoolInformationDto> GetAssetLowValuePool(Guid assetLowValuePoolId);

	/// <summary>
	/// Saves the provided ERP assetLowValuePool.
	/// </summary>
	/// <param name="assetLowValuePool">The ERP assetLowValuePool to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAssetLowValuePool(ERPAssetLowValuePoolDto assetLowValuePool);
}
