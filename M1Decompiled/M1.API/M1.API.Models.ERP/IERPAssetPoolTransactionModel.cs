using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAssetPoolTransactionModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all AssetPoolTransactions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetPoolTransactions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAssetPoolTransactions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving AssetPoolTransaction information based on the specified AssetPoolTransaction Unique Id.
	/// </summary>
	/// <param name="assetPoolTransactionId">The Unique Id of the AssetPoolTransaction.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAssetPoolTransaction(Guid assetPoolTransactionId);

	/// <summary>
	/// Validates the PUT request for creating or updating AssetPoolTransaction information based on the specified AssetPoolTransaction.
	/// </summary>
	/// <param name="assetPoolTransaction">The AssetPoolTransaction details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAssetPoolTransaction(ERPAssetPoolTransactionDto assetPoolTransaction);

	/// <summary>
	/// Processes the request to retrieve all AssetPoolTransactions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of AssetPoolTransactions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of AssetPoolTransactions DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAssetPoolTransactionDto>>> Process_GetAllAssetPoolTransactions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific AssetPoolTransaction.
	/// </summary>
	/// <param name="assetPoolTransactionId">The Unique Id of the AssetPoolTransaction to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the AssetPoolTransaction DTO.</returns>
	Task<ERPResponseMessageDto<ERPAssetPoolTransactionDto>> Process_GetAssetPoolTransaction(Guid assetPoolTransactionId);

	/// <summary>
	/// Processes the creating or updating of a AssetPoolTransaction record.
	/// </summary>
	/// <param name="assetPoolTransaction">The AssetPoolTransaction data transfer object (DTO) containing the details of the AssetPoolTransaction to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the AssetPoolTransaction details.</returns>
	Task<ERPResponseMessageDto<ERPAssetPoolTransactionDto>> Process_PutAssetPoolTransaction(ERPAssetPoolTransactionDto assetPoolTransaction);

	/// <summary>
	/// Validates the request for deleting a AssetPoolTransaction record.
	/// </summary>
	/// <param name="assetPoolTransactionId">The Unique Id of the AssetPoolTransaction.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAssetPoolTransaction(Guid assetPoolTransactionId);

	/// <summary>
	/// Processes the request to delete a AssetPoolTransaction record.
	/// </summary>
	/// <param name="assetPoolTransactionId">The Unique Id of the AssetPoolTransaction.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAssetPoolTransactionDto>> Process_DeleteAssetPoolTransaction(Guid assetPoolTransactionId);
}
