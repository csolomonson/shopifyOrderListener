using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAssetAdjustmentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all AssetAdjustments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetAdjustments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAssetAdjustments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving AssetAdjustment information based on the specified AssetAdjustment Unique Id.
	/// </summary>
	/// <param name="assetAdjustmentId">The Unique Id of the AssetAdjustment.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAssetAdjustment(Guid assetAdjustmentId);

	/// <summary>
	/// Validates the PUT request for creating or updating AssetAdjustment information based on the specified AssetAdjustment.
	/// </summary>
	/// <param name="assetAdjustment">The AssetAdjustment details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAssetAdjustment(ERPAssetAdjustmentDto assetAdjustment);

	/// <summary>
	/// Processes the request to retrieve all AssetAdjustments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetAdjustments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetAdjustments DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAssetAdjustmentDto>>> Process_GetAllAssetAdjustments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific AssetAdjustment.
	/// </summary>
	/// <param name="assetAdjustmentId">The Unique Id of the AssetAdjustment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the AssetAdjustment DTO.</returns>
	Task<ERPResponseMessageDto<ERPAssetAdjustmentDto>> Process_GetAssetAdjustment(Guid assetAdjustmentId);

	/// <summary>
	/// Processes the creating or updating of a AssetAdjustment record.
	/// </summary>
	/// <param name="assetAdjustment">The AssetAdjustment data transfer object (DTO) containing the details of the AssetAdjustment to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the AssetAdjustment details.</returns>
	Task<ERPResponseMessageDto<ERPAssetAdjustmentDto>> Process_PutAssetAdjustment(ERPAssetAdjustmentDto assetAdjustment);

	/// <summary>
	/// Validates the request for deleting a AssetAdjustment record.
	/// </summary>
	/// <param name="assetAdjustmentId">The Unique Id of the AssetAdjustment.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAssetAdjustment(Guid assetAdjustmentId);

	/// <summary>
	/// Processes the request to delete a AssetAdjustment record.
	/// </summary>
	/// <param name="assetAdjustmentId">The Unique Id of the AssetAdjustment.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAssetAdjustmentDto>> Process_DeleteAssetAdjustment(Guid assetAdjustmentId);
}
