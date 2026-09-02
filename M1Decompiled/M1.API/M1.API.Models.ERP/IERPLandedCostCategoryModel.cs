using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPLandedCostCategoryModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all LandedCostCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LandedCostCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllLandedCostCategories(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving LandedCostCategory information based on the specified LandedCostCategory Unique Id.
	/// </summary>
	/// <param name="landedCostCategoryId">The Unique Id of the LandedCostCategory.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetLandedCostCategory(Guid landedCostCategoryId);

	/// <summary>
	/// Processes the request to retrieve all LandedCostCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LandedCostCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LandedCostCategories DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPLandedCostCategoryDto>>> Process_GetAllLandedCostCategories(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific LandedCostCategory.
	/// </summary>
	/// <param name="landedCostCategoryId">The Unique Id of the LandedCostCategory to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the LandedCostCategory DTO.</returns>
	Task<ERPResponseMessageDto<ERPLandedCostCategoryDto>> Process_GetLandedCostCategory(Guid landedCostCategoryId);
}
