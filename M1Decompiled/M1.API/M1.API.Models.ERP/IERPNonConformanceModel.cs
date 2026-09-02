using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPNonConformanceModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all NonConformances with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of NonConformances to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllNonConformances(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving NonConformance information based on the specified NonConformance Unique Id.
	/// </summary>
	/// <param name="nonConformanceId">The Unique Id of the NonConformance.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetNonConformance(Guid nonConformanceId);

	/// <summary>
	/// Validates the PUT request for creating or updating NonConformance information based on the specified NonConformance.
	/// </summary>
	/// <param name="nonConformance">The NonConformance details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutNonConformance(ERPNonConformanceDto nonConformance);

	/// <summary>
	/// Processes the request to retrieve all NonConformances with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of NonConformances to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of NonConformances DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPNonConformanceDto>>> Process_GetAllNonConformances(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific NonConformance.
	/// </summary>
	/// <param name="nonConformanceId">The Unique Id of the NonConformance to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the NonConformance DTO.</returns>
	Task<ERPResponseMessageDto<ERPNonConformanceDto>> Process_GetNonConformance(Guid nonConformanceId);

	/// <summary>
	/// Processes the creating or updating of a NonConformance record.
	/// </summary>
	/// <param name="nonConformance">The NonConformance data transfer object (DTO) containing the details of the NonConformance to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the NonConformance details.</returns>
	Task<ERPResponseMessageDto<ERPNonConformanceDto>> Process_PutNonConformance(ERPNonConformanceDto nonConformance);

	/// <summary>
	/// Validates the request for deleting a NonConformance record.
	/// </summary>
	/// <param name="nonConformanceId">The Unique Id of the NonConformance.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteNonConformance(Guid nonConformanceId);

	/// <summary>
	/// Processes the request to delete a NonConformance record.
	/// </summary>
	/// <param name="nonConformanceId">The Unique Id of the NonConformance.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPNonConformanceDto>> Process_DeleteNonConformance(Guid nonConformanceId);
}
