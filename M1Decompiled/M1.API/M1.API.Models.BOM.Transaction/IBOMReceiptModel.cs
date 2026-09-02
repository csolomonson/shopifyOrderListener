using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Transaction;
using M1.API.DTOs.Core;
using M1.API.Utilities;

namespace M1.API.Models.BOM.Transaction;

/// <summary>
/// Interface for BOM receipt model.
/// </summary>
public interface IBOMReceiptModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Processes and retrieves all receipts with pagination.
	/// </summary>
	/// <param name="pageSize">The size of each page.</param>
	/// <param name="pageNumber">The page number to retrieve.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a response message with a list of receipt DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMReceiptDto>>> Process_GetAllReceipts(int pageSize, int pageNumber);

	/// <summary>
	/// Retrieves the details of a receipt by its ID.
	/// </summary>
	/// <param name="receiptId">The ID of the receipt to retrieve.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a response message with the receipt DTO.</returns>
	Task<BOMResponseMessageDto<BOMReceiptDto>> Process_GetReceipt(string receiptId);

	/// <summary>
	/// Validates the request to retrieve the details of a receipt by its ID.
	/// </summary>
	/// <param name="receiptId">The ID of the receipt to validate.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains validation information.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetReceipt(string receiptId);

	/// <summary>
	/// Validates the request for retrieving receipt information with an api client context defined.
	/// </summary>
	/// <param name="receiptId">The ID of the receipt to validate.</param>
	/// <param name="context">The API client context.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains validation information.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetReceipt(string receiptId, APIClientContext context);
}
