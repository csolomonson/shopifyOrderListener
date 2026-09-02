using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPSerialNumberTransactionModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all SerialNumberTransactions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SerialNumberTransactions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllSerialNumberTransactions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving SerialNumberTransaction information based on the specified SerialNumberTransaction Unique Id.
	/// </summary>
	/// <param name="serialNumberTransactionId">The Unique Id of the SerialNumberTransaction.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetSerialNumberTransaction(Guid serialNumberTransactionId);

	/// <summary>
	/// Validates the PUT request for creating or updating SerialNumberTransaction information based on the specified SerialNumberTransaction.
	/// </summary>
	/// <param name="serialNumberTransaction">The SerialNumberTransaction details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutSerialNumberTransaction(ERPSerialNumberTransactionDto serialNumberTransaction);

	/// <summary>
	/// Processes the request to retrieve all SerialNumberTransactions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SerialNumberTransactions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SerialNumberTransactions DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPSerialNumberTransactionDto>>> Process_GetAllSerialNumberTransactions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific SerialNumberTransaction.
	/// </summary>
	/// <param name="serialNumberTransactionId">The Unique Id of the SerialNumberTransaction to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the SerialNumberTransaction DTO.</returns>
	Task<ERPResponseMessageDto<ERPSerialNumberTransactionDto>> Process_GetSerialNumberTransaction(Guid serialNumberTransactionId);

	/// <summary>
	/// Processes the creating or updating of a SerialNumberTransaction record.
	/// </summary>
	/// <param name="serialNumberTransaction">The SerialNumberTransaction data transfer object (DTO) containing the details of the SerialNumberTransaction to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the SerialNumberTransaction details.</returns>
	Task<ERPResponseMessageDto<ERPSerialNumberTransactionDto>> Process_PutSerialNumberTransaction(ERPSerialNumberTransactionDto serialNumberTransaction);

	/// <summary>
	/// Validates the request for deleting a SerialNumberTransaction record.
	/// </summary>
	/// <param name="serialNumberTransactionId">The Unique Id of the SerialNumberTransaction.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteSerialNumberTransaction(Guid serialNumberTransactionId);

	/// <summary>
	/// Processes the request to delete a SerialNumberTransaction record.
	/// </summary>
	/// <param name="serialNumberTransactionId">The Unique Id of the SerialNumberTransaction.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPSerialNumberTransactionDto>> Process_DeleteSerialNumberTransaction(Guid serialNumberTransactionId);
}
