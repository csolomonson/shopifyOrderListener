using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPTaxCodeLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all TaxCodeLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TaxCodeLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllTaxCodeLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving TaxCodeLine information based on the specified TaxCodeLine Unique Id.
	/// </summary>
	/// <param name="taxCodeLineId">The Unique Id of the TaxCodeLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetTaxCodeLine(Guid taxCodeLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating TaxCodeLine information based on the specified TaxCodeLine.
	/// </summary>
	/// <param name="taxCodeLine">The TaxCodeLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutTaxCodeLine(ERPTaxCodeLineDto taxCodeLine);

	/// <summary>
	/// Processes the request to retrieve all TaxCodeLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TaxCodeLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of TaxCodeLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPTaxCodeLineDto>>> Process_GetAllTaxCodeLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific TaxCodeLine.
	/// </summary>
	/// <param name="taxCodeLineId">The Unique Id of the TaxCodeLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the TaxCodeLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPTaxCodeLineDto>> Process_GetTaxCodeLine(Guid taxCodeLineId);

	/// <summary>
	/// Processes the creating or updating of a TaxCodeLine record.
	/// </summary>
	/// <param name="taxCodeLine">The TaxCodeLine data transfer object (DTO) containing the details of the TaxCodeLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the TaxCodeLine details.</returns>
	Task<ERPResponseMessageDto<ERPTaxCodeLineDto>> Process_PutTaxCodeLine(ERPTaxCodeLineDto taxCodeLine);

	/// <summary>
	/// Validates the request for deleting a TaxCodeLine record.
	/// </summary>
	/// <param name="taxCodeLineId">The Unique Id of the TaxCodeLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteTaxCodeLine(Guid taxCodeLineId);

	/// <summary>
	/// Processes the request to delete a TaxCodeLine record.
	/// </summary>
	/// <param name="taxCodeLineId">The Unique Id of the TaxCodeLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPTaxCodeLineDto>> Process_DeleteTaxCodeLine(Guid taxCodeLineId);
}
