using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPToolModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Tools with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Tools to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllTools(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Tool information based on the specified Tool Unique Id.
	/// </summary>
	/// <param name="toolId">The Unique Id of the Tool.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetTool(Guid toolId);

	/// <summary>
	/// Validates the PUT request for creating or updating Tool information based on the specified Tool.
	/// </summary>
	/// <param name="tool">The Tool details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutTool(ERPToolDto tool);

	/// <summary>
	/// Processes the request to retrieve all Tools with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Tools to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Tools DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPToolDto>>> Process_GetAllTools(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Tool.
	/// </summary>
	/// <param name="toolId">The Unique Id of the Tool to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Tool DTO.</returns>
	Task<ERPResponseMessageDto<ERPToolDto>> Process_GetTool(Guid toolId);

	/// <summary>
	/// Processes the creating or updating of a Tool record.
	/// </summary>
	/// <param name="tool">The Tool data transfer object (DTO) containing the details of the Tool to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Tool details.</returns>
	Task<ERPResponseMessageDto<ERPToolDto>> Process_PutTool(ERPToolDto tool);

	/// <summary>
	/// Validates the request for deleting a Tool record.
	/// </summary>
	/// <param name="toolId">The Unique Id of the Tool.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteTool(Guid toolId);

	/// <summary>
	/// Processes the request to delete a Tool record.
	/// </summary>
	/// <param name="toolId">The Unique Id of the Tool.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPToolDto>> Process_DeleteTool(Guid toolId);
}
