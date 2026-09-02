using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPProductCategoryModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ProductCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllProductCategories(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ProductCategory information based on the specified ProductCategory Unique Id.
	/// </summary>
	/// <param name="productCategoryId">The Unique Id of the ProductCategory.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetProductCategory(Guid productCategoryId);

	/// <summary>
	/// Processes the request to retrieve all ProductCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductCategories DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPProductCategoryDto>>> Process_GetAllProductCategories(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ProductCategory.
	/// </summary>
	/// <param name="productCategoryId">The Unique Id of the ProductCategory to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ProductCategory DTO.</returns>
	Task<ERPResponseMessageDto<ERPProductCategoryDto>> Process_GetProductCategory(Guid productCategoryId);
}
