using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPToolMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ToolMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ToolMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllToolMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ToolMemo information based on the specified ToolMemo Unique Id.
	/// </summary>
	/// <param name="toolMemoId">The Unique Id of the ToolMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetToolMemo(Guid toolMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating ToolMemo information based on the specified ToolMemo.
	/// </summary>
	/// <param name="toolMemo">The ToolMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutToolMemo(ERPToolMemoDto toolMemo);

	/// <summary>
	/// Processes the request to retrieve all ToolMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ToolMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ToolMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPToolMemoDto>>> Process_GetAllToolMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ToolMemo.
	/// </summary>
	/// <param name="toolMemoId">The Unique Id of the ToolMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ToolMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPToolMemoDto>> Process_GetToolMemo(Guid toolMemoId);

	/// <summary>
	/// Processes the creating or updating of a ToolMemo record.
	/// </summary>
	/// <param name="toolMemo">The ToolMemo data transfer object (DTO) containing the details of the ToolMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ToolMemo details.</returns>
	Task<ERPResponseMessageDto<ERPToolMemoDto>> Process_PutToolMemo(ERPToolMemoDto toolMemo);

	/// <summary>
	/// Validates the request for deleting a ToolMemo record.
	/// </summary>
	/// <param name="toolMemoId">The Unique Id of the ToolMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteToolMemo(Guid toolMemoId);

	/// <summary>
	/// Processes the request to delete a ToolMemo record.
	/// </summary>
	/// <param name="toolMemoId">The Unique Id of the ToolMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPToolMemoDto>> Process_DeleteToolMemo(Guid toolMemoId);
}
