using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPReceiptLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ReceiptLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ReceiptLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllReceiptLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ReceiptLine information based on the specified ReceiptLine Unique Id.
	/// </summary>
	/// <param name="receiptLineId">The Unique Id of the ReceiptLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetReceiptLine(Guid receiptLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating ReceiptLine information based on the specified ReceiptLine.
	/// </summary>
	/// <param name="receiptLine">The ReceiptLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutReceiptLine(ERPReceiptLineDto receiptLine);

	/// <summary>
	/// Processes the request to retrieve all ReceiptLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ReceiptLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ReceiptLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPReceiptLineDto>>> Process_GetAllReceiptLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ReceiptLine.
	/// </summary>
	/// <param name="receiptLineId">The Unique Id of the ReceiptLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ReceiptLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPReceiptLineDto>> Process_GetReceiptLine(Guid receiptLineId);

	/// <summary>
	/// Processes the creating or updating of a ReceiptLine record.
	/// </summary>
	/// <param name="receiptLine">The ReceiptLine data transfer object (DTO) containing the details of the ReceiptLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ReceiptLine details.</returns>
	Task<ERPResponseMessageDto<ERPReceiptLineDto>> Process_PutReceiptLine(ERPReceiptLineDto receiptLine);

	/// <summary>
	/// Validates the request for deleting a ReceiptLine record.
	/// </summary>
	/// <param name="receiptLineId">The Unique Id of the ReceiptLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteReceiptLine(Guid receiptLineId);

	/// <summary>
	/// Processes the request to delete a ReceiptLine record.
	/// </summary>
	/// <param name="receiptLineId">The Unique Id of the ReceiptLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPReceiptLineDto>> Process_DeleteReceiptLine(Guid receiptLineId);
}
