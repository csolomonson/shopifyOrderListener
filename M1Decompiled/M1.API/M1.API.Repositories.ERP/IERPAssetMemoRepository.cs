using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAssetMemoRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a AssetMemo with the specified Unique Id exists.
	/// </summary>
	/// <param name="assetMemoId">The Unique Id of the AssetMemo to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the AssetMemo exists or not.</returns>
	Task<bool> DoesAssetMemoExist(Guid assetMemoId);

	/// <summary>
	/// Retrieves all AssetMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetMemos DTOs.</returns>
	Task<ICollection<ERPAssetMemoInformationDto>> GetAllAssetMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific AssetMemo.
	/// </summary>
	/// <param name="assetMemoId">The Unique Id of the AssetMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the AssetMemo DTO.</returns>
	Task<ERPAssetMemoInformationDto> GetAssetMemo(Guid assetMemoId);

	/// <summary>
	/// Saves the provided ERP assetMemo.
	/// </summary>
	/// <param name="assetMemo">The ERP assetMemo to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAssetMemo(ERPAssetMemoDto assetMemo);
}
