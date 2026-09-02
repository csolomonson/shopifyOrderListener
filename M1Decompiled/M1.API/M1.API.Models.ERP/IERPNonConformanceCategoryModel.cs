using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPNonConformanceCategoryModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all NonConformanceCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of NonConformanceCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllNonConformanceCategories(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving NonConformanceCategory information based on the specified NonConformanceCategory Unique Id.
	/// </summary>
	/// <param name="nonConformanceCategoryId">The Unique Id of the NonConformanceCategory.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetNonConformanceCategory(Guid nonConformanceCategoryId);

	/// <summary>
	/// Validates the PUT request for creating or updating NonConformanceCategory information based on the specified NonConformanceCategory.
	/// </summary>
	/// <param name="nonConformanceCategory">The NonConformanceCategory details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutNonConformanceCategory(ERPNonConformanceCategoryDto nonConformanceCategory);

	/// <summary>
	/// Processes the request to retrieve all NonConformanceCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of NonConformanceCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of NonConformanceCategories DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPNonConformanceCategoryDto>>> Process_GetAllNonConformanceCategories(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific NonConformanceCategory.
	/// </summary>
	/// <param name="nonConformanceCategoryId">The Unique Id of the NonConformanceCategory to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the NonConformanceCategory DTO.</returns>
	Task<ERPResponseMessageDto<ERPNonConformanceCategoryDto>> Process_GetNonConformanceCategory(Guid nonConformanceCategoryId);

	/// <summary>
	/// Processes the creating or updating of a NonConformanceCategory record.
	/// </summary>
	/// <param name="nonConformanceCategory">The NonConformanceCategory data transfer object (DTO) containing the details of the NonConformanceCategory to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the NonConformanceCategory details.</returns>
	Task<ERPResponseMessageDto<ERPNonConformanceCategoryDto>> Process_PutNonConformanceCategory(ERPNonConformanceCategoryDto nonConformanceCategory);

	/// <summary>
	/// Validates the request for deleting a NonConformanceCategory record.
	/// </summary>
	/// <param name="nonConformanceCategoryId">The Unique Id of the NonConformanceCategory.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteNonConformanceCategory(Guid nonConformanceCategoryId);

	/// <summary>
	/// Processes the request to delete a NonConformanceCategory record.
	/// </summary>
	/// <param name="nonConformanceCategoryId">The Unique Id of the NonConformanceCategory.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPNonConformanceCategoryDto>> Process_DeleteNonConformanceCategory(Guid nonConformanceCategoryId);
}
