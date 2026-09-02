using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPAPInvoiceModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all APInvoices with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APInvoices to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllAPInvoices(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving APInvoice information based on the specified APInvoice Unique Id.
	/// </summary>
	/// <param name="aPInvoiceId">The Unique Id of the APInvoice.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAPInvoice(Guid aPInvoiceId);

	/// <summary>
	/// Validates the PUT request for creating or updating APInvoice information based on the specified APInvoice.
	/// </summary>
	/// <param name="aPInvoice">The APInvoice details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutAPInvoice(ERPAPInvoiceDto aPInvoice);

	/// <summary>
	/// Processes the request to retrieve all APInvoices with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of APInvoices to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of APInvoices DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPAPInvoiceDto>>> Process_GetAllAPInvoices(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific APInvoice.
	/// </summary>
	/// <param name="aPInvoiceId">The Unique Id of the APInvoice to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the APInvoice DTO.</returns>
	Task<ERPResponseMessageDto<ERPAPInvoiceDto>> Process_GetAPInvoice(Guid aPInvoiceId);

	/// <summary>
	/// Processes the creating or updating of a APInvoice record.
	/// </summary>
	/// <param name="aPInvoice">The APInvoice data transfer object (DTO) containing the details of the APInvoice to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the APInvoice details.</returns>
	Task<ERPResponseMessageDto<ERPAPInvoiceDto>> Process_PutAPInvoice(ERPAPInvoiceDto aPInvoice);

	/// <summary>
	/// Validates the request for deleting a APInvoice record.
	/// </summary>
	/// <param name="aPInvoiceId">The Unique Id of the APInvoice.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteAPInvoice(Guid aPInvoiceId);

	/// <summary>
	/// Processes the request to delete a APInvoice record.
	/// </summary>
	/// <param name="aPInvoiceId">The Unique Id of the APInvoice.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPAPInvoiceDto>> Process_DeleteAPInvoice(Guid aPInvoiceId);
}
