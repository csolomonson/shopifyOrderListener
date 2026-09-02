using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPStandardMessageModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all StandardMessages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of StandardMessages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllStandardMessages(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving StandardMessage information based on the specified StandardMessage Unique Id.
	/// </summary>
	/// <param name="standardMessageId">The Unique Id of the StandardMessage.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetStandardMessage(Guid standardMessageId);

	/// <summary>
	/// Validates the PUT request for creating or updating StandardMessage information based on the specified StandardMessage.
	/// </summary>
	/// <param name="standardMessage">The StandardMessage details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutStandardMessage(ERPStandardMessageDto standardMessage);

	/// <summary>
	/// Processes the request to retrieve all StandardMessages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of StandardMessages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of StandardMessages DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPStandardMessageDto>>> Process_GetAllStandardMessages(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific StandardMessage.
	/// </summary>
	/// <param name="standardMessageId">The Unique Id of the StandardMessage to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the StandardMessage DTO.</returns>
	Task<ERPResponseMessageDto<ERPStandardMessageDto>> Process_GetStandardMessage(Guid standardMessageId);

	/// <summary>
	/// Processes the creating or updating of a StandardMessage record.
	/// </summary>
	/// <param name="standardMessage">The StandardMessage data transfer object (DTO) containing the details of the StandardMessage to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the StandardMessage details.</returns>
	Task<ERPResponseMessageDto<ERPStandardMessageDto>> Process_PutStandardMessage(ERPStandardMessageDto standardMessage);

	/// <summary>
	/// Validates the request for deleting a StandardMessage record.
	/// </summary>
	/// <param name="standardMessageId">The Unique Id of the StandardMessage.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteStandardMessage(Guid standardMessageId);

	/// <summary>
	/// Processes the request to delete a StandardMessage record.
	/// </summary>
	/// <param name="standardMessageId">The Unique Id of the StandardMessage.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPStandardMessageDto>> Process_DeleteStandardMessage(Guid standardMessageId);
}
