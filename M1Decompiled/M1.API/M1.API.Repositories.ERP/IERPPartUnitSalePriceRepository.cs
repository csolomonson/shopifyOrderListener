using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPPartUnitSalePriceRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a PartUnitSalePrice with the specified Unique Id exists.
	/// </summary>
	/// <param name="partUnitSalePriceId">The Unique Id of the PartUnitSalePrice to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the PartUnitSalePrice exists or not.</returns>
	Task<bool> DoesPartUnitSalePriceExist(Guid partUnitSalePriceId);

	/// <summary>
	/// Retrieves all PartUnitSalePrices with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartUnitSalePrices to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartUnitSalePrices DTOs.</returns>
	Task<ICollection<ERPPartUnitSalePriceInformationDto>> GetAllPartUnitSalePrices(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific PartUnitSalePrice.
	/// </summary>
	/// <param name="partUnitSalePriceId">The Unique Id of the PartUnitSalePrice to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the PartUnitSalePrice DTO.</returns>
	Task<ERPPartUnitSalePriceInformationDto> GetPartUnitSalePrice(Guid partUnitSalePriceId);

	/// <summary>
	/// Saves the provided ERP partUnitSalePrice.
	/// </summary>
	/// <param name="partUnitSalePrice">The ERP partUnitSalePrice to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SavePartUnitSalePrice(ERPPartUnitSalePriceDto partUnitSalePrice);
}
