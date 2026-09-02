using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPQuoteMaterialRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a QuoteMaterial with the specified Unique Id exists.
	/// </summary>
	/// <param name="quoteMaterialId">The Unique Id of the QuoteMaterial to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the QuoteMaterial exists or not.</returns>
	Task<bool> DoesQuoteMaterialExist(Guid quoteMaterialId);

	/// <summary>
	/// Retrieves all QuoteMaterials with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteMaterials to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of QuoteMaterials DTOs.</returns>
	Task<ICollection<ERPQuoteMaterialInformationDto>> GetAllQuoteMaterials(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific QuoteMaterial.
	/// </summary>
	/// <param name="quoteMaterialId">The Unique Id of the QuoteMaterial to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the QuoteMaterial DTO.</returns>
	Task<ERPQuoteMaterialInformationDto> GetQuoteMaterial(Guid quoteMaterialId);

	/// <summary>
	/// Saves the provided ERP quoteMaterial.
	/// </summary>
	/// <param name="quoteMaterial">The ERP quoteMaterial to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveQuoteMaterial(ERPQuoteMaterialDto quoteMaterial);
}
