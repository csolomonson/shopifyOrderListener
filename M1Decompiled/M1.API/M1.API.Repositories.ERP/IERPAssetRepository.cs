using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAssetRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a Asset with the specified Unique Id exists.
	/// </summary>
	/// <param name="assetId">The Unique Id of the Asset to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the Asset exists or not.</returns>
	Task<bool> DoesAssetExist(Guid assetId);

	/// <summary>
	/// Retrieves all Assets with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Assets to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Assets DTOs.</returns>
	Task<ICollection<ERPAssetInformationDto>> GetAllAssets(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific Asset.
	/// </summary>
	/// <param name="assetId">The Unique Id of the Asset to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the Asset DTO.</returns>
	Task<ERPAssetInformationDto> GetAsset(Guid assetId);

	/// <summary>
	/// Saves the provided ERP asset.
	/// </summary>
	/// <param name="asset">The ERP asset to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAsset(ERPAssetDto asset);
}
