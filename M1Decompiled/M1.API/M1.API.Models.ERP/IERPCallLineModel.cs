using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPCallLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all CallLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CallLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllCallLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving CallLine information based on the specified CallLine Unique Id.
	/// </summary>
	/// <param name="callLineId">The Unique Id of the CallLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetCallLine(Guid callLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating CallLine information based on the specified CallLine.
	/// </summary>
	/// <param name="callLine">The CallLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutCallLine(ERPCallLineDto callLine);

	/// <summary>
	/// Processes the request to retrieve all CallLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CallLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CallLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPCallLineDto>>> Process_GetAllCallLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific CallLine.
	/// </summary>
	/// <param name="callLineId">The Unique Id of the CallLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the CallLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPCallLineDto>> Process_GetCallLine(Guid callLineId);

	/// <summary>
	/// Processes the creating or updating of a CallLine record.
	/// </summary>
	/// <param name="callLine">The CallLine data transfer object (DTO) containing the details of the CallLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the CallLine details.</returns>
	Task<ERPResponseMessageDto<ERPCallLineDto>> Process_PutCallLine(ERPCallLineDto callLine);

	/// <summary>
	/// Validates the request for deleting a CallLine record.
	/// </summary>
	/// <param name="callLineId">The Unique Id of the CallLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteCallLine(Guid callLineId);

	/// <summary>
	/// Processes the request to delete a CallLine record.
	/// </summary>
	/// <param name="callLineId">The Unique Id of the CallLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPCallLineDto>> Process_DeleteCallLine(Guid callLineId);
}
