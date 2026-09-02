using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAssetTypeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all AssetTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAssetTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving AssetType information based on the specified AssetType Unique Id.
	/// </summary>
	/// <param name="assetTypeId">The Unique Id of the AssetType.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAssetType(Guid assetTypeId);

	/// <summary>
	/// Processes the request to retrieve all AssetTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetTypes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAssetTypeDto>>> Process_GetAllAssetTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific AssetType.
	/// </summary>
	/// <param name="assetTypeId">The Unique Id of the AssetType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the AssetType DTO.</returns>
	Task<ERPResponseMessageDto<ERPAssetTypeDto>> Process_GetAssetType(Guid assetTypeId);
}
