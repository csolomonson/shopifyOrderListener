using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPCallModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Calls with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Calls to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllCalls(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Call information based on the specified Call Unique Id.
	/// </summary>
	/// <param name="callId">The Unique Id of the Call.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetCall(Guid callId);

	/// <summary>
	/// Validates the PUT request for creating or updating Call information based on the specified Call.
	/// </summary>
	/// <param name="call">The Call details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutCall(ERPCallDto call);

	/// <summary>
	/// Processes the request to retrieve all Calls with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Calls to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Calls DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPCallDto>>> Process_GetAllCalls(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Call.
	/// </summary>
	/// <param name="callId">The Unique Id of the Call to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Call DTO.</returns>
	Task<ERPResponseMessageDto<ERPCallDto>> Process_GetCall(Guid callId);

	/// <summary>
	/// Processes the creating or updating of a Call record.
	/// </summary>
	/// <param name="call">The Call data transfer object (DTO) containing the details of the Call to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Call details.</returns>
	Task<ERPResponseMessageDto<ERPCallDto>> Process_PutCall(ERPCallDto call);

	/// <summary>
	/// Validates the request for deleting a Call record.
	/// </summary>
	/// <param name="callId">The Unique Id of the Call.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteCall(Guid callId);

	/// <summary>
	/// Processes the request to delete a Call record.
	/// </summary>
	/// <param name="callId">The Unique Id of the Call.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPCallDto>> Process_DeleteCall(Guid callId);
}
