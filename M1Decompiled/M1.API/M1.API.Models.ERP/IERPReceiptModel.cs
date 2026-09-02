using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPReceiptModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Receipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Receipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllReceipts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Receipt information based on the specified Receipt Unique Id.
	/// </summary>
	/// <param name="receiptId">The Unique Id of the Receipt.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetReceipt(Guid receiptId);

	/// <summary>
	/// Validates the PUT request for creating or updating Receipt information based on the specified Receipt.
	/// </summary>
	/// <param name="receipt">The Receipt details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutReceipt(ERPReceiptDto receipt);

	/// <summary>
	/// Processes the request to retrieve all Receipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Receipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Receipts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPReceiptDto>>> Process_GetAllReceipts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Receipt.
	/// </summary>
	/// <param name="receiptId">The Unique Id of the Receipt to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Receipt DTO.</returns>
	Task<ERPResponseMessageDto<ERPReceiptDto>> Process_GetReceipt(Guid receiptId);

	/// <summary>
	/// Processes the creating or updating of a Receipt record.
	/// </summary>
	/// <param name="receipt">The Receipt data transfer object (DTO) containing the details of the Receipt to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Receipt details.</returns>
	Task<ERPResponseMessageDto<ERPReceiptDto>> Process_PutReceipt(ERPReceiptDto receipt);

	/// <summary>
	/// Validates the request for deleting a Receipt record.
	/// </summary>
	/// <param name="receiptId">The Unique Id of the Receipt.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteReceipt(Guid receiptId);

	/// <summary>
	/// Processes the request to delete a Receipt record.
	/// </summary>
	/// <param name="receiptId">The Unique Id of the Receipt.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPReceiptDto>> Process_DeleteReceipt(Guid receiptId);
}
