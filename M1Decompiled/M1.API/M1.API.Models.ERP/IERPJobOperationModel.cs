using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPJobOperationModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all JobOperations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobOperations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllJobOperations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving JobOperation information based on the specified JobOperation Unique Id.
	/// </summary>
	/// <param name="jobOperationId">The Unique Id of the JobOperation.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetJobOperation(Guid jobOperationId);

	/// <summary>
	/// Validates the PUT request for creating or updating JobOperation information based on the specified JobOperation.
	/// </summary>
	/// <param name="jobOperation">The JobOperation details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutJobOperation(ERPJobOperationDto jobOperation);

	/// <summary>
	/// Processes the request to retrieve all JobOperations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of JobOperations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of JobOperations DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPJobOperationDto>>> Process_GetAllJobOperations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific JobOperation.
	/// </summary>
	/// <param name="jobOperationId">The Unique Id of the JobOperation to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the JobOperation DTO.</returns>
	Task<ERPResponseMessageDto<ERPJobOperationDto>> Process_GetJobOperation(Guid jobOperationId);

	/// <summary>
	/// Processes the creating or updating of a JobOperation record.
	/// </summary>
	/// <param name="jobOperation">The JobOperation data transfer object (DTO) containing the details of the JobOperation to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the JobOperation details.</returns>
	Task<ERPResponseMessageDto<ERPJobOperationDto>> Process_PutJobOperation(ERPJobOperationDto jobOperation);

	/// <summary>
	/// Validates the request for deleting a JobOperation record.
	/// </summary>
	/// <param name="jobOperationId">The Unique Id of the JobOperation.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteJobOperation(Guid jobOperationId);

	/// <summary>
	/// Processes the request to delete a JobOperation record.
	/// </summary>
	/// <param name="jobOperationId">The Unique Id of the JobOperation.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPJobOperationDto>> Process_DeleteJobOperation(Guid jobOperationId);
}
