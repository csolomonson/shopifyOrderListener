using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPQuoteSalesPersonModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all QuoteSalesPeople with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteSalesPeople to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving QuoteSalesPerson information based on the specified QuoteSalesPerson Unique Id.
	/// </summary>
	/// <param name="quoteSalesPersonId">The Unique Id of the QuoteSalesPerson.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuoteSalesPerson(Guid quoteSalesPersonId);

	/// <summary>
	/// Validates the PUT request for creating or updating QuoteSalesPerson information based on the specified QuoteSalesPerson.
	/// </summary>
	/// <param name="quoteSalesPerson">The QuoteSalesPerson details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutQuoteSalesPerson(ERPQuoteSalesPersonDto quoteSalesPerson);

	/// <summary>
	/// Processes the request to retrieve all QuoteSalesPeople with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteSalesPeople to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuoteSalesPeople DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPQuoteSalesPersonDto>>> Process_GetAllQuoteSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific QuoteSalesPerson.
	/// </summary>
	/// <param name="quoteSalesPersonId">The Unique Id of the QuoteSalesPerson to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the QuoteSalesPerson DTO.</returns>
	Task<ERPResponseMessageDto<ERPQuoteSalesPersonDto>> Process_GetQuoteSalesPerson(Guid quoteSalesPersonId);

	/// <summary>
	/// Processes the creating or updating of a QuoteSalesPerson record.
	/// </summary>
	/// <param name="quoteSalesPerson">The QuoteSalesPerson data transfer object (DTO) containing the details of the QuoteSalesPerson to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the QuoteSalesPerson details.</returns>
	Task<ERPResponseMessageDto<ERPQuoteSalesPersonDto>> Process_PutQuoteSalesPerson(ERPQuoteSalesPersonDto quoteSalesPerson);

	/// <summary>
	/// Validates the request for deleting a QuoteSalesPerson record.
	/// </summary>
	/// <param name="quoteSalesPersonId">The Unique Id of the QuoteSalesPerson.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteSalesPerson(Guid quoteSalesPersonId);

	/// <summary>
	/// Processes the request to delete a QuoteSalesPerson record.
	/// </summary>
	/// <param name="quoteSalesPersonId">The Unique Id of the QuoteSalesPerson.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPQuoteSalesPersonDto>> Process_DeleteQuoteSalesPerson(Guid quoteSalesPersonId);
}
