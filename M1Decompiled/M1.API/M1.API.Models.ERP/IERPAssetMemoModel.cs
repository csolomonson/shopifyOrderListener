using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAssetMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all AssetMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAssetMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving AssetMemo information based on the specified AssetMemo Unique Id.
	/// </summary>
	/// <param name="assetMemoId">The Unique Id of the AssetMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAssetMemo(Guid assetMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating AssetMemo information based on the specified AssetMemo.
	/// </summary>
	/// <param name="assetMemo">The AssetMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAssetMemo(ERPAssetMemoDto assetMemo);

	/// <summary>
	/// Processes the request to retrieve all AssetMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAssetMemoDto>>> Process_GetAllAssetMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific AssetMemo.
	/// </summary>
	/// <param name="assetMemoId">The Unique Id of the AssetMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the AssetMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPAssetMemoDto>> Process_GetAssetMemo(Guid assetMemoId);

	/// <summary>
	/// Processes the creating or updating of a AssetMemo record.
	/// </summary>
	/// <param name="assetMemo">The AssetMemo data transfer object (DTO) containing the details of the AssetMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the AssetMemo details.</returns>
	Task<ERPResponseMessageDto<ERPAssetMemoDto>> Process_PutAssetMemo(ERPAssetMemoDto assetMemo);

	/// <summary>
	/// Validates the request for deleting a AssetMemo record.
	/// </summary>
	/// <param name="assetMemoId">The Unique Id of the AssetMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAssetMemo(Guid assetMemoId);

	/// <summary>
	/// Processes the request to delete a AssetMemo record.
	/// </summary>
	/// <param name="assetMemoId">The Unique Id of the AssetMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAssetMemoDto>> Process_DeleteAssetMemo(Guid assetMemoId);
}
