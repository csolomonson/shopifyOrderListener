using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPCycleCodeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all CycleCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CycleCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllCycleCodes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving CycleCode information based on the specified CycleCode Unique Id.
	/// </summary>
	/// <param name="cycleCodeId">The Unique Id of the CycleCode.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetCycleCode(Guid cycleCodeId);

	/// <summary>
	/// Validates the PUT request for creating or updating CycleCode information based on the specified CycleCode.
	/// </summary>
	/// <param name="cycleCode">The CycleCode details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutCycleCode(ERPCycleCodeDto cycleCode);

	/// <summary>
	/// Processes the request to retrieve all CycleCodes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CycleCodes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CycleCodes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPCycleCodeDto>>> Process_GetAllCycleCodes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific CycleCode.
	/// </summary>
	/// <param name="cycleCodeId">The Unique Id of the CycleCode to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the CycleCode DTO.</returns>
	Task<ERPResponseMessageDto<ERPCycleCodeDto>> Process_GetCycleCode(Guid cycleCodeId);

	/// <summary>
	/// Processes the creating or updating of a CycleCode record.
	/// </summary>
	/// <param name="cycleCode">The CycleCode data transfer object (DTO) containing the details of the CycleCode to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the CycleCode details.</returns>
	Task<ERPResponseMessageDto<ERPCycleCodeDto>> Process_PutCycleCode(ERPCycleCodeDto cycleCode);

	/// <summary>
	/// Validates the request for deleting a CycleCode record.
	/// </summary>
	/// <param name="cycleCodeId">The Unique Id of the CycleCode.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteCycleCode(Guid cycleCodeId);

	/// <summary>
	/// Processes the request to delete a CycleCode record.
	/// </summary>
	/// <param name="cycleCodeId">The Unique Id of the CycleCode.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPCycleCodeDto>> Process_DeleteCycleCode(Guid cycleCodeId);
}
