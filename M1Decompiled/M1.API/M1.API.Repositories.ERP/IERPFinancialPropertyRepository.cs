using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPFinancialPropertyRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a FinancialProperty with the specified Unique Id exists.
	/// </summary>
	/// <param name="financialPropertyId">The Unique Id of the FinancialProperty to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the FinancialProperty exists or not.</returns>
	Task<bool> DoesFinancialPropertyExist(Guid financialPropertyId);

	/// <summary>
	/// Retrieves all FinancialProperties with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of FinancialProperties to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of FinancialProperties DTOs.</returns>
	Task<ICollection<ERPFinancialPropertyInformationDto>> GetAllFinancialProperties(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific FinancialProperty.
	/// </summary>
	/// <param name="financialPropertyId">The Unique Id of the FinancialProperty to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the FinancialProperty DTO.</returns>
	Task<ERPFinancialPropertyInformationDto> GetFinancialProperty(Guid financialPropertyId);
}
