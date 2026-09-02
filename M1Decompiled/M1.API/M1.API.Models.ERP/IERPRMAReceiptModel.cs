using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPRMAReceiptModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all RMAReceipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAReceipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllRMAReceipts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving RMAReceipt information based on the specified RMAReceipt Unique Id.
	/// </summary>
	/// <param name="rMAReceiptId">The Unique Id of the RMAReceipt.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetRMAReceipt(Guid rMAReceiptId);

	/// <summary>
	/// Validates the PUT request for creating or updating RMAReceipt information based on the specified RMAReceipt.
	/// </summary>
	/// <param name="rMAReceipt">The RMAReceipt details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutRMAReceipt(ERPRMAReceiptDto rMAReceipt);

	/// <summary>
	/// Processes the request to retrieve all RMAReceipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RMAReceipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RMAReceipts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPRMAReceiptDto>>> Process_GetAllRMAReceipts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific RMAReceipt.
	/// </summary>
	/// <param name="rMAReceiptId">The Unique Id of the RMAReceipt to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the RMAReceipt DTO.</returns>
	Task<ERPResponseMessageDto<ERPRMAReceiptDto>> Process_GetRMAReceipt(Guid rMAReceiptId);

	/// <summary>
	/// Processes the creating or updating of a RMAReceipt record.
	/// </summary>
	/// <param name="rMAReceipt">The RMAReceipt data transfer object (DTO) containing the details of the RMAReceipt to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the RMAReceipt details.</returns>
	Task<ERPResponseMessageDto<ERPRMAReceiptDto>> Process_PutRMAReceipt(ERPRMAReceiptDto rMAReceipt);

	/// <summary>
	/// Validates the request for deleting a RMAReceipt record.
	/// </summary>
	/// <param name="rMAReceiptId">The Unique Id of the RMAReceipt.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteRMAReceipt(Guid rMAReceiptId);

	/// <summary>
	/// Processes the request to delete a RMAReceipt record.
	/// </summary>
	/// <param name="rMAReceiptId">The Unique Id of the RMAReceipt.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPRMAReceiptDto>> Process_DeleteRMAReceipt(Guid rMAReceiptId);
}
