using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPLotNumberTransactionModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all LotNumberTransactions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LotNumberTransactions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllLotNumberTransactions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving LotNumberTransaction information based on the specified LotNumberTransaction Unique Id.
	/// </summary>
	/// <param name="lotNumberTransactionId">The Unique Id of the LotNumberTransaction.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetLotNumberTransaction(Guid lotNumberTransactionId);

	/// <summary>
	/// Validates the PUT request for creating or updating LotNumberTransaction information based on the specified LotNumberTransaction.
	/// </summary>
	/// <param name="lotNumberTransaction">The LotNumberTransaction details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutLotNumberTransaction(ERPLotNumberTransactionDto lotNumberTransaction);

	/// <summary>
	/// Processes the request to retrieve all LotNumberTransactions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LotNumberTransactions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LotNumberTransactions DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPLotNumberTransactionDto>>> Process_GetAllLotNumberTransactions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific LotNumberTransaction.
	/// </summary>
	/// <param name="lotNumberTransactionId">The Unique Id of the LotNumberTransaction to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the LotNumberTransaction DTO.</returns>
	Task<ERPResponseMessageDto<ERPLotNumberTransactionDto>> Process_GetLotNumberTransaction(Guid lotNumberTransactionId);

	/// <summary>
	/// Processes the creating or updating of a LotNumberTransaction record.
	/// </summary>
	/// <param name="lotNumberTransaction">The LotNumberTransaction data transfer object (DTO) containing the details of the LotNumberTransaction to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the LotNumberTransaction details.</returns>
	Task<ERPResponseMessageDto<ERPLotNumberTransactionDto>> Process_PutLotNumberTransaction(ERPLotNumberTransactionDto lotNumberTransaction);

	/// <summary>
	/// Validates the request for deleting a LotNumberTransaction record.
	/// </summary>
	/// <param name="lotNumberTransactionId">The Unique Id of the LotNumberTransaction.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteLotNumberTransaction(Guid lotNumberTransactionId);

	/// <summary>
	/// Processes the request to delete a LotNumberTransaction record.
	/// </summary>
	/// <param name="lotNumberTransactionId">The Unique Id of the LotNumberTransaction.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPLotNumberTransactionDto>> Process_DeleteLotNumberTransaction(Guid lotNumberTransactionId);
}
