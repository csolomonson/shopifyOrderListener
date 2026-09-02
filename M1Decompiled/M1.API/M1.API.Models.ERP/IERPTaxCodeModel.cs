using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPTaxCodeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all TaxCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TaxCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllTaxCodes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving TaxCode information based on the specified TaxCode Unique Id.
	/// </summary>
	/// <param name="taxCodeId">The Unique Id of the TaxCode.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetTaxCode(Guid taxCodeId);

	/// <summary>
	/// Validates the PUT request for creating or updating TaxCode information based on the specified TaxCode.
	/// </summary>
	/// <param name="taxCode">The TaxCode details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutTaxCode(ERPTaxCodeDto taxCode);

	/// <summary>
	/// Processes the request to retrieve all TaxCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of TaxCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of TaxCodes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPTaxCodeDto>>> Process_GetAllTaxCodes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific TaxCode.
	/// </summary>
	/// <param name="taxCodeId">The Unique Id of the TaxCode to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the TaxCode DTO.</returns>
	Task<ERPResponseMessageDto<ERPTaxCodeDto>> Process_GetTaxCode(Guid taxCodeId);

	/// <summary>
	/// Processes the creating or updating of a TaxCode record.
	/// </summary>
	/// <param name="taxCode">The TaxCode data transfer object (DTO) containing the details of the TaxCode to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the TaxCode details.</returns>
	Task<ERPResponseMessageDto<ERPTaxCodeDto>> Process_PutTaxCode(ERPTaxCodeDto taxCode);

	/// <summary>
	/// Validates the request for deleting a TaxCode record.
	/// </summary>
	/// <param name="taxCodeId">The Unique Id of the TaxCode.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteTaxCode(Guid taxCodeId);

	/// <summary>
	/// Processes the request to delete a TaxCode record.
	/// </summary>
	/// <param name="taxCodeId">The Unique Id of the TaxCode.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPTaxCodeDto>> Process_DeleteTaxCode(Guid taxCodeId);
}
