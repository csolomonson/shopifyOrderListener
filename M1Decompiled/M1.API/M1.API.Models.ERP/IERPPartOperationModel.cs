using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartOperationModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartOperations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartOperations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartOperations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartOperation information based on the specified PartOperation Unique Id.
	/// </summary>
	/// <param name="partOperationId">The Unique Id of the PartOperation.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartOperation(Guid partOperationId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartOperation information based on the specified PartOperation.
	/// </summary>
	/// <param name="partOperation">The PartOperation details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartOperation(ERPPartOperationDto partOperation);

	/// <summary>
	/// Processes the request to retrieve all PartOperations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartOperations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartOperations DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartOperationDto>>> Process_GetAllPartOperations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartOperation.
	/// </summary>
	/// <param name="partOperationId">The Unique Id of the PartOperation to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartOperation DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartOperationDto>> Process_GetPartOperation(Guid partOperationId);

	/// <summary>
	/// Processes the creating or updating of a PartOperation record.
	/// </summary>
	/// <param name="partOperation">The PartOperation data transfer object (DTO) containing the details of the PartOperation to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartOperation details.</returns>
	Task<ERPResponseMessageDto<ERPPartOperationDto>> Process_PutPartOperation(ERPPartOperationDto partOperation);

	/// <summary>
	/// Validates the request for deleting a PartOperation record.
	/// </summary>
	/// <param name="partOperationId">The Unique Id of the PartOperation.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartOperation(Guid partOperationId);

	/// <summary>
	/// Processes the request to delete a PartOperation record.
	/// </summary>
	/// <param name="partOperationId">The Unique Id of the PartOperation.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartOperationDto>> Process_DeletePartOperation(Guid partOperationId);
}
