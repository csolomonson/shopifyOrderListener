using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartPriceRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartPrice with the specified Unique Id exists.
	/// </summary>
	/// <param name="partPriceId">The Unique Id of the PartPrice to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartPrice exists or not.</returns>
	Task<bool> DoesPartPriceExist(Guid partPriceId);

	/// <summary>
	/// Retrieves all PartPrices with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartPrices to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartPrices DTOs.</returns>
	Task<ICollection<ERPPartPriceInformationDto>> GetAllPartPrices(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartPrice.
	/// </summary>
	/// <param name="partPriceId">The Unique Id of the PartPrice to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartPrice DTO.</returns>
	Task<ERPPartPriceInformationDto> GetPartPrice(Guid partPriceId);

	/// <summary>
	/// Saves the provided ERP partPrice.
	/// </summary>
	/// <param name="partPrice">The ERP partPrice to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartPrice(ERPPartPriceDto partPrice);
}
