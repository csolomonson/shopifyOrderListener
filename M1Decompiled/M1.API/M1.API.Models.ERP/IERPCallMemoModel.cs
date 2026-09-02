using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPCallMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all CallMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CallMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllCallMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving CallMemo information based on the specified CallMemo Unique Id.
	/// </summary>
	/// <param name="callMemoId">The Unique Id of the CallMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetCallMemo(Guid callMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating CallMemo information based on the specified CallMemo.
	/// </summary>
	/// <param name="callMemo">The CallMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutCallMemo(ERPCallMemoDto callMemo);

	/// <summary>
	/// Processes the request to retrieve all CallMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of CallMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of CallMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPCallMemoDto>>> Process_GetAllCallMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific CallMemo.
	/// </summary>
	/// <param name="callMemoId">The Unique Id of the CallMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the CallMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPCallMemoDto>> Process_GetCallMemo(Guid callMemoId);

	/// <summary>
	/// Processes the creating or updating of a CallMemo record.
	/// </summary>
	/// <param name="callMemo">The CallMemo data transfer object (DTO) containing the details of the CallMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the CallMemo details.</returns>
	Task<ERPResponseMessageDto<ERPCallMemoDto>> Process_PutCallMemo(ERPCallMemoDto callMemo);

	/// <summary>
	/// Validates the request for deleting a CallMemo record.
	/// </summary>
	/// <param name="callMemoId">The Unique Id of the CallMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteCallMemo(Guid callMemoId);

	/// <summary>
	/// Processes the request to delete a CallMemo record.
	/// </summary>
	/// <param name="callMemoId">The Unique Id of the CallMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPCallMemoDto>> Process_DeleteCallMemo(Guid callMemoId);
}
