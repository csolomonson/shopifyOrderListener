using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPGLCategoryModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all GLCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllGLCategories(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving GLCategory information based on the specified GLCategory Unique Id.
	/// </summary>
	/// <param name="gLCategoryId">The Unique Id of the GLCategory.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetGLCategory(Guid gLCategoryId);

	/// <summary>
	/// Validates the PUT request for creating or updating GLCategory information based on the specified GLCategory.
	/// </summary>
	/// <param name="gLCategory">The GLCategory details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutGLCategory(ERPGLCategoryDto gLCategory);

	/// <summary>
	/// Processes the request to retrieve all GLCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of GLCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of GLCategories DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPGLCategoryDto>>> Process_GetAllGLCategories(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific GLCategory.
	/// </summary>
	/// <param name="gLCategoryId">The Unique Id of the GLCategory to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the GLCategory DTO.</returns>
	Task<ERPResponseMessageDto<ERPGLCategoryDto>> Process_GetGLCategory(Guid gLCategoryId);

	/// <summary>
	/// Processes the creating or updating of a GLCategory record.
	/// </summary>
	/// <param name="gLCategory">The GLCategory data transfer object (DTO) containing the details of the GLCategory to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the GLCategory details.</returns>
	Task<ERPResponseMessageDto<ERPGLCategoryDto>> Process_PutGLCategory(ERPGLCategoryDto gLCategory);

	/// <summary>
	/// Validates the request for deleting a GLCategory record.
	/// </summary>
	/// <param name="gLCategoryId">The Unique Id of the GLCategory.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteGLCategory(Guid gLCategoryId);

	/// <summary>
	/// Processes the request to delete a GLCategory record.
	/// </summary>
	/// <param name="gLCategoryId">The Unique Id of the GLCategory.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPGLCategoryDto>> Process_DeleteGLCategory(Guid gLCategoryId);
}
