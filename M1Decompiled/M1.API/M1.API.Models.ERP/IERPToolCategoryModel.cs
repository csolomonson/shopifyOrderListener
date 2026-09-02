using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPToolCategoryModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ToolCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ToolCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllToolCategories(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ToolCategory information based on the specified ToolCategory Unique Id.
	/// </summary>
	/// <param name="toolCategoryId">The Unique Id of the ToolCategory.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetToolCategory(Guid toolCategoryId);

	/// <summary>
	/// Processes the request to retrieve all ToolCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ToolCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ToolCategories DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPToolCategoryDto>>> Process_GetAllToolCategories(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ToolCategory.
	/// </summary>
	/// <param name="toolCategoryId">The Unique Id of the ToolCategory to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ToolCategory DTO.</returns>
	Task<ERPResponseMessageDto<ERPToolCategoryDto>> Process_GetToolCategory(Guid toolCategoryId);
}
