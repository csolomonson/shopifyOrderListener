using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAssetTypeMethodModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all AssetTypeMethods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetTypeMethods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAssetTypeMethods(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving AssetTypeMethod information based on the specified AssetTypeMethod Unique Id.
	/// </summary>
	/// <param name="assetTypeMethodId">The Unique Id of the AssetTypeMethod.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAssetTypeMethod(Guid assetTypeMethodId);

	/// <summary>
	/// Processes the request to retrieve all AssetTypeMethods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetTypeMethods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetTypeMethods DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAssetTypeMethodDto>>> Process_GetAllAssetTypeMethods(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific AssetTypeMethod.
	/// </summary>
	/// <param name="assetTypeMethodId">The Unique Id of the AssetTypeMethod to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the AssetTypeMethod DTO.</returns>
	Task<ERPResponseMessageDto<ERPAssetTypeMethodDto>> Process_GetAssetTypeMethod(Guid assetTypeMethodId);
}
