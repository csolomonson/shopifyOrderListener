using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPARInvoiceModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ARInvoices with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARInvoices to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllARInvoices(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ARInvoice information based on the specified ARInvoice Unique Id.
	/// </summary>
	/// <param name="aRInvoiceId">The Unique Id of the ARInvoice.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetARInvoice(Guid aRInvoiceId);

	/// <summary>
	/// Validates the PUT request for creating or updating ARInvoice information based on the specified ARInvoice.
	/// </summary>
	/// <param name="aRInvoice">The ARInvoice details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutARInvoice(ERPARInvoiceDto aRInvoice);

	/// <summary>
	/// Processes the request to retrieve all ARInvoices with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARInvoices to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ARInvoices DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPARInvoiceDto>>> Process_GetAllARInvoices(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ARInvoice.
	/// </summary>
	/// <param name="aRInvoiceId">The Unique Id of the ARInvoice to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ARInvoice DTO.</returns>
	Task<ERPResponseMessageDto<ERPARInvoiceDto>> Process_GetARInvoice(Guid aRInvoiceId);

	/// <summary>
	/// Processes the creating or updating of a ARInvoice record.
	/// </summary>
	/// <param name="aRInvoice">The ARInvoice data transfer object (DTO) containing the details of the ARInvoice to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ARInvoice details.</returns>
	Task<ERPResponseMessageDto<ERPARInvoiceDto>> Process_PutARInvoice(ERPARInvoiceDto aRInvoice);

	/// <summary>
	/// Validates the request for deleting a ARInvoice record.
	/// </summary>
	/// <param name="aRInvoiceId">The Unique Id of the ARInvoice.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteARInvoice(Guid aRInvoiceId);

	/// <summary>
	/// Processes the request to delete a ARInvoice record.
	/// </summary>
	/// <param name="aRInvoiceId">The Unique Id of the ARInvoice.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPARInvoiceDto>> Process_DeleteARInvoice(Guid aRInvoiceId);
}
