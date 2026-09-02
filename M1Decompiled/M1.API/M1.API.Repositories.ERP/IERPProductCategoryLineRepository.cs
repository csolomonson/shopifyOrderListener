using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPProductCategoryLineRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ProductCategoryLine with the specified Unique Id exists.
	/// </summary>
	/// <param name="productCategoryLineId">The Unique Id of the ProductCategoryLine to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ProductCategoryLine exists or not.</returns>
	Task<bool> DoesProductCategoryLineExist(Guid productCategoryLineId);

	/// <summary>
	/// Retrieves all ProductCategoryLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ProductCategoryLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ProductCategoryLines DTOs.</returns>
	Task<ICollection<ERPProductCategoryLineInformationDto>> GetAllProductCategoryLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ProductCategoryLine.
	/// </summary>
	/// <param name="productCategoryLineId">The Unique Id of the ProductCategoryLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ProductCategoryLine DTO.</returns>
	Task<ERPProductCategoryLineInformationDto> GetProductCategoryLine(Guid productCategoryLineId);
}
