using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWorkCenterMachineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WorkCenterMachines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WorkCenterMachines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWorkCenterMachines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WorkCenterMachine information based on the specified WorkCenterMachine Unique Id.
	/// </summary>
	/// <param name="workCenterMachineId">The Unique Id of the WorkCenterMachine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWorkCenterMachine(Guid workCenterMachineId);

	/// <summary>
	/// Validates the PUT request for creating or updating WorkCenterMachine information based on the specified WorkCenterMachine.
	/// </summary>
	/// <param name="workCenterMachine">The WorkCenterMachine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWorkCenterMachine(ERPWorkCenterMachineDto workCenterMachine);

	/// <summary>
	/// Processes the request to retrieve all WorkCenterMachines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WorkCenterMachines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WorkCenterMachines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWorkCenterMachineDto>>> Process_GetAllWorkCenterMachines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WorkCenterMachine.
	/// </summary>
	/// <param name="workCenterMachineId">The Unique Id of the WorkCenterMachine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WorkCenterMachine DTO.</returns>
	Task<ERPResponseMessageDto<ERPWorkCenterMachineDto>> Process_GetWorkCenterMachine(Guid workCenterMachineId);

	/// <summary>
	/// Processes the creating or updating of a WorkCenterMachine record.
	/// </summary>
	/// <param name="workCenterMachine">The WorkCenterMachine data transfer object (DTO) containing the details of the WorkCenterMachine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the WorkCenterMachine details.</returns>
	Task<ERPResponseMessageDto<ERPWorkCenterMachineDto>> Process_PutWorkCenterMachine(ERPWorkCenterMachineDto workCenterMachine);

	/// <summary>
	/// Validates the request for deleting a WorkCenterMachine record.
	/// </summary>
	/// <param name="workCenterMachineId">The Unique Id of the WorkCenterMachine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWorkCenterMachine(Guid workCenterMachineId);

	/// <summary>
	/// Processes the request to delete a WorkCenterMachine record.
	/// </summary>
	/// <param name="workCenterMachineId">The Unique Id of the WorkCenterMachine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWorkCenterMachineDto>> Process_DeleteWorkCenterMachine(Guid workCenterMachineId);
}
