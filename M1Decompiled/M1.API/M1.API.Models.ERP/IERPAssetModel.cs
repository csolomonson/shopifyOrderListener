using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAssetModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Assets with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Assets to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAssets(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Asset information based on the specified Asset Unique Id.
	/// </summary>
	/// <param name="assetId">The Unique Id of the Asset.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAsset(Guid assetId);

	/// <summary>
	/// Validates the PUT request for creating or updating Asset information based on the specified Asset.
	/// </summary>
	/// <param name="asset">The Asset details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAsset(ERPAssetDto asset);

	/// <summary>
	/// Processes the request to retrieve all Assets with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Assets to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Assets DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAssetDto>>> Process_GetAllAssets(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Asset.
	/// </summary>
	/// <param name="assetId">The Unique Id of the Asset to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Asset DTO.</returns>
	Task<ERPResponseMessageDto<ERPAssetDto>> Process_GetAsset(Guid assetId);

	/// <summary>
	/// Processes the creating or updating of a Asset record.
	/// </summary>
	/// <param name="asset">The Asset data transfer object (DTO) containing the details of the Asset to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Asset details.</returns>
	Task<ERPResponseMessageDto<ERPAssetDto>> Process_PutAsset(ERPAssetDto asset);

	/// <summary>
	/// Validates the request for deleting a Asset record.
	/// </summary>
	/// <param name="assetId">The Unique Id of the Asset.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAsset(Guid assetId);

	/// <summary>
	/// Processes the request to delete a Asset record.
	/// </summary>
	/// <param name="assetId">The Unique Id of the Asset.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAssetDto>> Process_DeleteAsset(Guid assetId);
}
