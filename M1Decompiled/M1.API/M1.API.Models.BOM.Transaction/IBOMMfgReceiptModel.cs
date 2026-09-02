using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Transaction;
using M1.API.DTOs.Core;

namespace M1.API.Models.BOM.Transaction;

public interface IBOMMfgReceiptModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving manufacturing receipt information based on the specified manufacturing receipt ID.
	/// </summary>
	/// <param name="mfgReceiptId">The ID of the manufacturing receipt.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetMfgReceipt(string mfgReceiptId);

	/// <summary>
	/// Processes the request to retrieve all manufacturing receipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of manufacturing receipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of manufacturing receipt DTOs.</returns>
	Task<BOMResponseMessageDto<IList<BOMMfgReceiptDto>>> Process_GetAllMfgReceipts(int pageSize, int pageNumber);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific manufacturing receipt.
	/// </summary>
	/// <param name="mfgReceiptId">The ID of the manufacturing receipt to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with the manufacturing receipt DTO.</returns>
	Task<BOMResponseMessageDto<BOMMfgReceiptDto>> Process_GetMfgReceipt(string mfgReceiptId);
}
