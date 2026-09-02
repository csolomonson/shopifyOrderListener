using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartTransactionModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartTransactions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartTransactions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartTransactions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartTransaction information based on the specified PartTransaction Unique Id.
	/// </summary>
	/// <param name="partTransactionId">The Unique Id of the PartTransaction.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartTransaction(Guid partTransactionId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartTransaction information based on the specified PartTransaction.
	/// </summary>
	/// <param name="partTransaction">The PartTransaction details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartTransaction(ERPPartTransactionDto partTransaction);

	/// <summary>
	/// Processes the request to retrieve all PartTransactions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartTransactions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartTransactions DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartTransactionDto>>> Process_GetAllPartTransactions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartTransaction.
	/// </summary>
	/// <param name="partTransactionId">The Unique Id of the PartTransaction to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartTransaction DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartTransactionDto>> Process_GetPartTransaction(Guid partTransactionId);

	/// <summary>
	/// Processes the creating or updating of a PartTransaction record.
	/// </summary>
	/// <param name="partTransaction">The PartTransaction data transfer object (DTO) containing the details of the PartTransaction to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartTransaction details.</returns>
	Task<ERPResponseMessageDto<ERPPartTransactionDto>> Process_PutPartTransaction(ERPPartTransactionDto partTransaction);

	/// <summary>
	/// Validates the request for deleting a PartTransaction record.
	/// </summary>
	/// <param name="partTransactionId">The Unique Id of the PartTransaction.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartTransaction(Guid partTransactionId);

	/// <summary>
	/// Processes the request to delete a PartTransaction record.
	/// </summary>
	/// <param name="partTransactionId">The Unique Id of the PartTransaction.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartTransactionDto>> Process_DeletePartTransaction(Guid partTransactionId);
}
