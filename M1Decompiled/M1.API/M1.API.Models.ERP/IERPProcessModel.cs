using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPProcessModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Processes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Processes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllProcesses(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Process information based on the specified Process Unique Id.
	/// </summary>
	/// <param name="processId">The Unique Id of the Process.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetProcess(Guid processId);

	/// <summary>
	/// Validates the PUT request for creating or updating Process information based on the specified Process.
	/// </summary>
	/// <param name="process">The Process details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutProcess(ERPProcessDto process);

	/// <summary>
	/// Processes the request to retrieve all Processes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Processes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Processes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPProcessDto>>> Process_GetAllProcesses(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Process.
	/// </summary>
	/// <param name="processId">The Unique Id of the Process to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Process DTO.</returns>
	Task<ERPResponseMessageDto<ERPProcessDto>> Process_GetProcess(Guid processId);

	/// <summary>
	/// Processes the creating or updating of a Process record.
	/// </summary>
	/// <param name="process">The Process data transfer object (DTO) containing the details of the Process to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Process details.</returns>
	Task<ERPResponseMessageDto<ERPProcessDto>> Process_PutProcess(ERPProcessDto process);

	/// <summary>
	/// Validates the request for deleting a Process record.
	/// </summary>
	/// <param name="processId">The Unique Id of the Process.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteProcess(Guid processId);

	/// <summary>
	/// Processes the request to delete a Process record.
	/// </summary>
	/// <param name="processId">The Unique Id of the Process.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPProcessDto>> Process_DeleteProcess(Guid processId);
}
