using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAPInvoiceLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all APInvoiceLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APInvoiceLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAPInvoiceLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving APInvoiceLine information based on the specified APInvoiceLine Unique Id.
	/// </summary>
	/// <param name="aPInvoiceLineId">The Unique Id of the APInvoiceLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAPInvoiceLine(Guid aPInvoiceLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating APInvoiceLine information based on the specified APInvoiceLine.
	/// </summary>
	/// <param name="aPInvoiceLine">The APInvoiceLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAPInvoiceLine(ERPAPInvoiceLineDto aPInvoiceLine);

	/// <summary>
	/// Processes the request to retrieve all APInvoiceLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APInvoiceLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of APInvoiceLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAPInvoiceLineDto>>> Process_GetAllAPInvoiceLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific APInvoiceLine.
	/// </summary>
	/// <param name="aPInvoiceLineId">The Unique Id of the APInvoiceLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the APInvoiceLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPAPInvoiceLineDto>> Process_GetAPInvoiceLine(Guid aPInvoiceLineId);

	/// <summary>
	/// Processes the creating or updating of a APInvoiceLine record.
	/// </summary>
	/// <param name="aPInvoiceLine">The APInvoiceLine data transfer object (DTO) containing the details of the APInvoiceLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the APInvoiceLine details.</returns>
	Task<ERPResponseMessageDto<ERPAPInvoiceLineDto>> Process_PutAPInvoiceLine(ERPAPInvoiceLineDto aPInvoiceLine);

	/// <summary>
	/// Validates the request for deleting a APInvoiceLine record.
	/// </summary>
	/// <param name="aPInvoiceLineId">The Unique Id of the APInvoiceLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAPInvoiceLine(Guid aPInvoiceLineId);

	/// <summary>
	/// Processes the request to delete a APInvoiceLine record.
	/// </summary>
	/// <param name="aPInvoiceLineId">The Unique Id of the APInvoiceLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAPInvoiceLineDto>> Process_DeleteAPInvoiceLine(Guid aPInvoiceLineId);
}
