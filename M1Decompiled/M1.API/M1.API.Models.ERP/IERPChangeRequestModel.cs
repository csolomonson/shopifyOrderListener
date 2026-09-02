using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPChangeRequestModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ChangeRequests with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ChangeRequests to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllChangeRequests(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ChangeRequest information based on the specified ChangeRequest Unique Id.
	/// </summary>
	/// <param name="changeRequestId">The Unique Id of the ChangeRequest.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetChangeRequest(Guid changeRequestId);

	/// <summary>
	/// Validates the PUT request for creating or updating ChangeRequest information based on the specified ChangeRequest.
	/// </summary>
	/// <param name="changeRequest">The ChangeRequest details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutChangeRequest(ERPChangeRequestDto changeRequest);

	/// <summary>
	/// Processes the request to retrieve all ChangeRequests with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ChangeRequests to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ChangeRequests DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPChangeRequestDto>>> Process_GetAllChangeRequests(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ChangeRequest.
	/// </summary>
	/// <param name="changeRequestId">The Unique Id of the ChangeRequest to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ChangeRequest DTO.</returns>
	Task<ERPResponseMessageDto<ERPChangeRequestDto>> Process_GetChangeRequest(Guid changeRequestId);

	/// <summary>
	/// Processes the creating or updating of a ChangeRequest record.
	/// </summary>
	/// <param name="changeRequest">The ChangeRequest data transfer object (DTO) containing the details of the ChangeRequest to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ChangeRequest details.</returns>
	Task<ERPResponseMessageDto<ERPChangeRequestDto>> Process_PutChangeRequest(ERPChangeRequestDto changeRequest);

	/// <summary>
	/// Validates the request for deleting a ChangeRequest record.
	/// </summary>
	/// <param name="changeRequestId">The Unique Id of the ChangeRequest.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteChangeRequest(Guid changeRequestId);

	/// <summary>
	/// Processes the request to delete a ChangeRequest record.
	/// </summary>
	/// <param name="changeRequestId">The Unique Id of the ChangeRequest.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPChangeRequestDto>> Process_DeleteChangeRequest(Guid changeRequestId);
}
