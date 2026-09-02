using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPNonConformanceCodeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all NonConformanceCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of NonConformanceCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllNonConformanceCodes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving NonConformanceCode information based on the specified NonConformanceCode Unique Id.
	/// </summary>
	/// <param name="nonConformanceCodeId">The Unique Id of the NonConformanceCode.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetNonConformanceCode(Guid nonConformanceCodeId);

	/// <summary>
	/// Validates the PUT request for creating or updating NonConformanceCode information based on the specified NonConformanceCode.
	/// </summary>
	/// <param name="nonConformanceCode">The NonConformanceCode details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutNonConformanceCode(ERPNonConformanceCodeDto nonConformanceCode);

	/// <summary>
	/// Processes the request to retrieve all NonConformanceCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of NonConformanceCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of NonConformanceCodes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPNonConformanceCodeDto>>> Process_GetAllNonConformanceCodes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific NonConformanceCode.
	/// </summary>
	/// <param name="nonConformanceCodeId">The Unique Id of the NonConformanceCode to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the NonConformanceCode DTO.</returns>
	Task<ERPResponseMessageDto<ERPNonConformanceCodeDto>> Process_GetNonConformanceCode(Guid nonConformanceCodeId);

	/// <summary>
	/// Processes the creating or updating of a NonConformanceCode record.
	/// </summary>
	/// <param name="nonConformanceCode">The NonConformanceCode data transfer object (DTO) containing the details of the NonConformanceCode to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the NonConformanceCode details.</returns>
	Task<ERPResponseMessageDto<ERPNonConformanceCodeDto>> Process_PutNonConformanceCode(ERPNonConformanceCodeDto nonConformanceCode);

	/// <summary>
	/// Validates the request for deleting a NonConformanceCode record.
	/// </summary>
	/// <param name="nonConformanceCodeId">The Unique Id of the NonConformanceCode.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteNonConformanceCode(Guid nonConformanceCodeId);

	/// <summary>
	/// Processes the request to delete a NonConformanceCode record.
	/// </summary>
	/// <param name="nonConformanceCodeId">The Unique Id of the NonConformanceCode.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPNonConformanceCodeDto>> Process_DeleteNonConformanceCode(Guid nonConformanceCodeId);
}
