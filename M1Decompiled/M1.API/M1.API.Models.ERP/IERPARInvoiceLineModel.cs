using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPARInvoiceLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ARInvoiceLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARInvoiceLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllARInvoiceLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ARInvoiceLine information based on the specified ARInvoiceLine Unique Id.
	/// </summary>
	/// <param name="aRInvoiceLineId">The Unique Id of the ARInvoiceLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetARInvoiceLine(Guid aRInvoiceLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating ARInvoiceLine information based on the specified ARInvoiceLine.
	/// </summary>
	/// <param name="aRInvoiceLine">The ARInvoiceLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutARInvoiceLine(ERPARInvoiceLineDto aRInvoiceLine);

	/// <summary>
	/// Processes the request to retrieve all ARInvoiceLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARInvoiceLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ARInvoiceLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPARInvoiceLineDto>>> Process_GetAllARInvoiceLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ARInvoiceLine.
	/// </summary>
	/// <param name="aRInvoiceLineId">The Unique Id of the ARInvoiceLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ARInvoiceLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPARInvoiceLineDto>> Process_GetARInvoiceLine(Guid aRInvoiceLineId);

	/// <summary>
	/// Processes the creating or updating of a ARInvoiceLine record.
	/// </summary>
	/// <param name="aRInvoiceLine">The ARInvoiceLine data transfer object (DTO) containing the details of the ARInvoiceLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ARInvoiceLine details.</returns>
	Task<ERPResponseMessageDto<ERPARInvoiceLineDto>> Process_PutARInvoiceLine(ERPARInvoiceLineDto aRInvoiceLine);

	/// <summary>
	/// Validates the request for deleting a ARInvoiceLine record.
	/// </summary>
	/// <param name="aRInvoiceLineId">The Unique Id of the ARInvoiceLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteARInvoiceLine(Guid aRInvoiceLineId);

	/// <summary>
	/// Processes the request to delete a ARInvoiceLine record.
	/// </summary>
	/// <param name="aRInvoiceLineId">The Unique Id of the ARInvoiceLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPARInvoiceLineDto>> Process_DeleteARInvoiceLine(Guid aRInvoiceLineId);
}
