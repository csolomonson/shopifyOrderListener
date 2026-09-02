using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPAssetAdjustmentRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a AssetAdjustment with the specified Unique Id exists.
	/// </summary>
	/// <param name="assetAdjustmentId">The Unique Id of the AssetAdjustment to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the AssetAdjustment exists or not.</returns>
	Task<bool> DoesAssetAdjustmentExist(Guid assetAdjustmentId);

	/// <summary>
	/// Retrieves all AssetAdjustments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetAdjustments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetAdjustments DTOs.</returns>
	Task<ICollection<ERPAssetAdjustmentInformationDto>> GetAllAssetAdjustments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific AssetAdjustment.
	/// </summary>
	/// <param name="assetAdjustmentId">The Unique Id of the AssetAdjustment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the AssetAdjustment DTO.</returns>
	Task<ERPAssetAdjustmentInformationDto> GetAssetAdjustment(Guid assetAdjustmentId);

	/// <summary>
	/// Saves the provided ERP assetAdjustment.
	/// </summary>
	/// <param name="assetAdjustment">The ERP assetAdjustment to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveAssetAdjustment(ERPAssetAdjustmentDto assetAdjustment);
}
