using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPARInvoiceSalesPersonModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ARInvoiceSalesPeople with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARInvoiceSalesPeople to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllARInvoiceSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ARInvoiceSalesPerson information based on the specified ARInvoiceSalesPerson Unique Id.
	/// </summary>
	/// <param name="aRInvoiceSalesPersonId">The Unique Id of the ARInvoiceSalesPerson.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetARInvoiceSalesPerson(Guid aRInvoiceSalesPersonId);

	/// <summary>
	/// Validates the PUT request for creating or updating ARInvoiceSalesPerson information based on the specified ARInvoiceSalesPerson.
	/// </summary>
	/// <param name="aRInvoiceSalesPerson">The ARInvoiceSalesPerson details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutARInvoiceSalesPerson(ERPARInvoiceSalesPersonDto aRInvoiceSalesPerson);

	/// <summary>
	/// Processes the request to retrieve all ARInvoiceSalesPeople with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ARInvoiceSalesPeople to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ARInvoiceSalesPeople DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPARInvoiceSalesPersonDto>>> Process_GetAllARInvoiceSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ARInvoiceSalesPerson.
	/// </summary>
	/// <param name="aRInvoiceSalesPersonId">The Unique Id of the ARInvoiceSalesPerson to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ARInvoiceSalesPerson DTO.</returns>
	Task<ERPResponseMessageDto<ERPARInvoiceSalesPersonDto>> Process_GetARInvoiceSalesPerson(Guid aRInvoiceSalesPersonId);

	/// <summary>
	/// Processes the creating or updating of a ARInvoiceSalesPerson record.
	/// </summary>
	/// <param name="aRInvoiceSalesPerson">The ARInvoiceSalesPerson data transfer object (DTO) containing the details of the ARInvoiceSalesPerson to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ARInvoiceSalesPerson details.</returns>
	Task<ERPResponseMessageDto<ERPARInvoiceSalesPersonDto>> Process_PutARInvoiceSalesPerson(ERPARInvoiceSalesPersonDto aRInvoiceSalesPerson);

	/// <summary>
	/// Validates the request for deleting a ARInvoiceSalesPerson record.
	/// </summary>
	/// <param name="aRInvoiceSalesPersonId">The Unique Id of the ARInvoiceSalesPerson.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteARInvoiceSalesPerson(Guid aRInvoiceSalesPersonId);

	/// <summary>
	/// Processes the request to delete a ARInvoiceSalesPerson record.
	/// </summary>
	/// <param name="aRInvoiceSalesPersonId">The Unique Id of the ARInvoiceSalesPerson.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPARInvoiceSalesPersonDto>> Process_DeleteARInvoiceSalesPerson(Guid aRInvoiceSalesPersonId);
}
