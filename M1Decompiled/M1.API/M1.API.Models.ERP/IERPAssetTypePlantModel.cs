using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAssetTypePlantModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all AssetTypePlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetTypePlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAssetTypePlants(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving AssetTypePlant information based on the specified AssetTypePlant Unique Id.
	/// </summary>
	/// <param name="assetTypePlantId">The Unique Id of the AssetTypePlant.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAssetTypePlant(Guid assetTypePlantId);

	/// <summary>
	/// Processes the request to retrieve all AssetTypePlants with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetTypePlants to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetTypePlants DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAssetTypePlantDto>>> Process_GetAllAssetTypePlants(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific AssetTypePlant.
	/// </summary>
	/// <param name="assetTypePlantId">The Unique Id of the AssetTypePlant to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the AssetTypePlant DTO.</returns>
	Task<ERPResponseMessageDto<ERPAssetTypePlantDto>> Process_GetAssetTypePlant(Guid assetTypePlantId);
}
