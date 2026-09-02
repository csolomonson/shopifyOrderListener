using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPQuoteLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all QuoteLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving QuoteLine information based on the specified QuoteLine Unique Id.
	/// </summary>
	/// <param name="quoteLineId">The Unique Id of the QuoteLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetQuoteLine(Guid quoteLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating QuoteLine information based on the specified QuoteLine.
	/// </summary>
	/// <param name="quoteLine">The QuoteLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutQuoteLine(ERPQuoteLineDto quoteLine);

	/// <summary>
	/// Processes the request to retrieve all QuoteLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuoteLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPQuoteLineDto>>> Process_GetAllQuoteLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific QuoteLine.
	/// </summary>
	/// <param name="quoteLineId">The Unique Id of the QuoteLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the QuoteLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPQuoteLineDto>> Process_GetQuoteLine(Guid quoteLineId);

	/// <summary>
	/// Processes the creating or updating of a QuoteLine record.
	/// </summary>
	/// <param name="quoteLine">The QuoteLine data transfer object (DTO) containing the details of the QuoteLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the QuoteLine details.</returns>
	Task<ERPResponseMessageDto<ERPQuoteLineDto>> Process_PutQuoteLine(ERPQuoteLineDto quoteLine);

	/// <summary>
	/// Validates the request for deleting a QuoteLine record.
	/// </summary>
	/// <param name="quoteLineId">The Unique Id of the QuoteLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteLine(Guid quoteLineId);

	/// <summary>
	/// Processes the request to delete a QuoteLine record.
	/// </summary>
	/// <param name="quoteLineId">The Unique Id of the QuoteLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPQuoteLineDto>> Process_DeleteQuoteLine(Guid quoteLineId);
}
