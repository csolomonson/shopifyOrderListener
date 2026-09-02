using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPLandedCostCategoryRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a LandedCostCategory with the specified Unique Id exists.
	/// </summary>
	/// <param name="landedCostCategoryId">The Unique Id of the LandedCostCategory to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the LandedCostCategory exists or not.</returns>
	Task<bool> DoesLandedCostCategoryExist(Guid landedCostCategoryId);

	/// <summary>
	/// Retrieves all LandedCostCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of LandedCostCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of LandedCostCategories DTOs.</returns>
	Task<ICollection<ERPLandedCostCategoryInformationDto>> GetAllLandedCostCategories(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific LandedCostCategory.
	/// </summary>
	/// <param name="landedCostCategoryId">The Unique Id of the LandedCostCategory to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the LandedCostCategory DTO.</returns>
	Task<ERPLandedCostCategoryInformationDto> GetLandedCostCategory(Guid landedCostCategoryId);
}
