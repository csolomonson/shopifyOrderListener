using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPProductCategoryLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ProductCategoryLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductCategoryLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllProductCategoryLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ProductCategoryLine information based on the specified ProductCategoryLine Unique Id.
	/// </summary>
	/// <param name="productCategoryLineId">The Unique Id of the ProductCategoryLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetProductCategoryLine(Guid productCategoryLineId);

	/// <summary>
	/// Processes the request to retrieve all ProductCategoryLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductCategoryLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductCategoryLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPProductCategoryLineDto>>> Process_GetAllProductCategoryLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ProductCategoryLine.
	/// </summary>
	/// <param name="productCategoryLineId">The Unique Id of the ProductCategoryLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ProductCategoryLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPProductCategoryLineDto>> Process_GetProductCategoryLine(Guid productCategoryLineId);
}
