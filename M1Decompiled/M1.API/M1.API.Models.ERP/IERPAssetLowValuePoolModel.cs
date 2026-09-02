using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAssetLowValuePoolModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all AssetLowValuePool with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetLowValuePool to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAssetLowValuePool(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving AssetLowValuePool information based on the specified AssetLowValuePool Unique Id.
	/// </summary>
	/// <param name="assetLowValuePoolId">The Unique Id of the AssetLowValuePool.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAssetLowValuePool(Guid assetLowValuePoolId);

	/// <summary>
	/// Validates the PUT request for creating or updating AssetLowValuePool information based on the specified AssetLowValuePool.
	/// </summary>
	/// <param name="assetLowValuePool">The AssetLowValuePool details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAssetLowValuePool(ERPAssetLowValuePoolDto assetLowValuePool);

	/// <summary>
	/// Processes the request to retrieve all AssetLowValuePool with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetLowValuePool to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetLowValuePool DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAssetLowValuePoolDto>>> Process_GetAllAssetLowValuePool(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific AssetLowValuePool.
	/// </summary>
	/// <param name="assetLowValuePoolId">The Unique Id of the AssetLowValuePool to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the AssetLowValuePool DTO.</returns>
	Task<ERPResponseMessageDto<ERPAssetLowValuePoolDto>> Process_GetAssetLowValuePool(Guid assetLowValuePoolId);

	/// <summary>
	/// Processes the creating or updating of a AssetLowValuePool record.
	/// </summary>
	/// <param name="assetLowValuePool">The AssetLowValuePool data transfer object (DTO) containing the details of the AssetLowValuePool to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the AssetLowValuePool details.</returns>
	Task<ERPResponseMessageDto<ERPAssetLowValuePoolDto>> Process_PutAssetLowValuePool(ERPAssetLowValuePoolDto assetLowValuePool);

	/// <summary>
	/// Validates the request for deleting a AssetLowValuePool record.
	/// </summary>
	/// <param name="assetLowValuePoolId">The Unique Id of the AssetLowValuePool.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAssetLowValuePool(Guid assetLowValuePoolId);

	/// <summary>
	/// Processes the request to delete a AssetLowValuePool record.
	/// </summary>
	/// <param name="assetLowValuePoolId">The Unique Id of the AssetLowValuePool.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAssetLowValuePoolDto>> Process_DeleteAssetLowValuePool(Guid assetLowValuePoolId);
}
