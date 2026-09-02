using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPQuoteOperationModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all QuoteOperations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteOperations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteOperations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving QuoteOperation information based on the specified QuoteOperation Unique Id.
	/// </summary>
	/// <param name="quoteOperationId">The Unique Id of the QuoteOperation.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuoteOperation(Guid quoteOperationId);

	/// <summary>
	/// Validates the PUT request for creating or updating QuoteOperation information based on the specified QuoteOperation.
	/// </summary>
	/// <param name="quoteOperation">The QuoteOperation details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutQuoteOperation(ERPQuoteOperationDto quoteOperation);

	/// <summary>
	/// Processes the request to retrieve all QuoteOperations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteOperations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuoteOperations DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPQuoteOperationDto>>> Process_GetAllQuoteOperations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific QuoteOperation.
	/// </summary>
	/// <param name="quoteOperationId">The Unique Id of the QuoteOperation to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the QuoteOperation DTO.</returns>
	Task<ERPResponseMessageDto<ERPQuoteOperationDto>> Process_GetQuoteOperation(Guid quoteOperationId);

	/// <summary>
	/// Processes the creating or updating of a QuoteOperation record.
	/// </summary>
	/// <param name="quoteOperation">The QuoteOperation data transfer object (DTO) containing the details of the QuoteOperation to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the QuoteOperation details.</returns>
	Task<ERPResponseMessageDto<ERPQuoteOperationDto>> Process_PutQuoteOperation(ERPQuoteOperationDto quoteOperation);

	/// <summary>
	/// Validates the request for deleting a QuoteOperation record.
	/// </summary>
	/// <param name="quoteOperationId">The Unique Id of the QuoteOperation.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteOperation(Guid quoteOperationId);

	/// <summary>
	/// Processes the request to delete a QuoteOperation record.
	/// </summary>
	/// <param name="quoteOperationId">The Unique Id of the QuoteOperation.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPQuoteOperationDto>> Process_DeleteQuoteOperation(Guid quoteOperationId);
}
