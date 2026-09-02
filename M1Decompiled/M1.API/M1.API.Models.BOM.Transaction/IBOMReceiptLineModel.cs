using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Transaction;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom.Transaction;

namespace M1.API.Models.BOM.Transaction;

/// <summary>
/// Interface for BOM receipt line model.
/// </summary>
public interface IBOMReceiptLineModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Processes and retrieves all receipt lines with pagination.
	/// </summary>
	/// <param name="pageSize">The size of each page.</param>
	/// <param name="pageNumber">The page number to retrieve.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a response message with a list of receipt line DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMReceiptLineDto>>> Process_GetAllReceiptLines(int pageSize, int pageNumber);

	/// <summary>
	/// Retrieves all receipt lines of a receipt by receipt ID.
	/// </summary>
	/// <param name="receiptId">The ID of the receipt to retrieve.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a response message with the receipt line DTO.</returns>
	Task<BOMResponseMessageDto<CTMBOMReceiptLineDto>> Process_GetReceiptLine(string receiptId);

	/// <summary>
	/// Validates the request to retrieve the receipt lines of a receipt by receipt ID.
	/// </summary>
	/// <param name="receiptId">The ID of the receipt to validate.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains validation information.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetReceipt(string receiptId);
}
