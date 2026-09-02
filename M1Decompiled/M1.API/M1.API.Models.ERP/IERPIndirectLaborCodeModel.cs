using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPIndirectLaborCodeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all IndirectLaborCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of IndirectLaborCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllIndirectLaborCodes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving IndirectLaborCode information based on the specified IndirectLaborCode Unique Id.
	/// </summary>
	/// <param name="indirectLaborCodeId">The Unique Id of the IndirectLaborCode.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetIndirectLaborCode(Guid indirectLaborCodeId);

	/// <summary>
	/// Validates the PUT request for creating or updating IndirectLaborCode information based on the specified IndirectLaborCode.
	/// </summary>
	/// <param name="indirectLaborCode">The IndirectLaborCode details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutIndirectLaborCode(ERPIndirectLaborCodeDto indirectLaborCode);

	/// <summary>
	/// Processes the request to retrieve all IndirectLaborCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of IndirectLaborCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of IndirectLaborCodes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPIndirectLaborCodeDto>>> Process_GetAllIndirectLaborCodes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific IndirectLaborCode.
	/// </summary>
	/// <param name="indirectLaborCodeId">The Unique Id of the IndirectLaborCode to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the IndirectLaborCode DTO.</returns>
	Task<ERPResponseMessageDto<ERPIndirectLaborCodeDto>> Process_GetIndirectLaborCode(Guid indirectLaborCodeId);

	/// <summary>
	/// Processes the creating or updating of a IndirectLaborCode record.
	/// </summary>
	/// <param name="indirectLaborCode">The IndirectLaborCode data transfer object (DTO) containing the details of the IndirectLaborCode to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the IndirectLaborCode details.</returns>
	Task<ERPResponseMessageDto<ERPIndirectLaborCodeDto>> Process_PutIndirectLaborCode(ERPIndirectLaborCodeDto indirectLaborCode);

	/// <summary>
	/// Validates the request for deleting a IndirectLaborCode record.
	/// </summary>
	/// <param name="indirectLaborCodeId">The Unique Id of the IndirectLaborCode.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteIndirectLaborCode(Guid indirectLaborCodeId);

	/// <summary>
	/// Processes the request to delete a IndirectLaborCode record.
	/// </summary>
	/// <param name="indirectLaborCodeId">The Unique Id of the IndirectLaborCode.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPIndirectLaborCodeDto>> Process_DeleteIndirectLaborCode(Guid indirectLaborCodeId);
}
