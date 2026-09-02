using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPProductCategoryRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ProductCategory with the specified Unique Id exists.
	/// </summary>
	/// <param name="productCategoryId">The Unique Id of the ProductCategory to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ProductCategory exists or not.</returns>
	Task<bool> DoesProductCategoryExist(Guid productCategoryId);

	/// <summary>
	/// Retrieves all ProductCategories with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductCategories to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductCategories DTOs.</returns>
	Task<ICollection<ERPProductCategoryInformationDto>> GetAllProductCategories(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ProductCategory.
	/// </summary>
	/// <param name="productCategoryId">The Unique Id of the ProductCategory to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ProductCategory DTO.</returns>
	Task<ERPProductCategoryInformationDto> GetProductCategory(Guid productCategoryId);
}
