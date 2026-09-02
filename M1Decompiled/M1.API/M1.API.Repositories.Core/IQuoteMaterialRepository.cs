using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;

namespace M1.API.Repositories.Core;

public interface IQuoteMaterialRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a QuoteMaterial with the specified ID exists.
	/// </summary>
	/// <param name="quoteMaterialId">The ID of the QuoteMaterial to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the QuoteMaterial exists or not.</returns>
	Task<bool> DoesQuoteMaterialExists(string quoteMaterialId);

	/// <summary>
	/// Retrieves all QuoteMaterial with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of QuoteMaterials to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a BOM response message DTO with a list of QuoteMaterials DTOs.</returns>
	Task<ICollection<BOMQuoteMaterialDto>> GetAllQuoteMaterials(int? pageSize = null, int? pageNumber = null);

	/// <summary>
	/// Retrieves detailed material information for a specific quote based on the provided Quote Id.
	/// </summary>
	/// <param name="quoteId">The ID of the Quote to retrieve information for.</param>
	/// <param name="quoteLineId">The ID of the Quote to retrieve information for.</param>
	/// <param name="quoteAssemblyId">The ID of the Quote to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the QuoteMaterial DTO.</returns>
	Task<ICollection<BOMQuoteMaterialDto>> GetQuoteMaterialsAsync(string quoteId, string quoteLineId, string quoteAssemblyId);

	/// <summary>
	/// Saves the provided BOM quoteMaterial.
	/// </summary>
	/// <param name="quoteMaterial">The BOM quote material to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save quote material,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveQuoteMaterialAsync(BOMCreateQuoteMaterialDto quoteMaterial);
}
