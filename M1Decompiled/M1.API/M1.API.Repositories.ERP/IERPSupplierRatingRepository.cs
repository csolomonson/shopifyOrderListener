using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPSupplierRatingRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a SupplierRating with the specified Unique Id exists.
	/// </summary>
	/// <param name="supplierRatingId">The Unique Id of the SupplierRating to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the SupplierRating exists or not.</returns>
	Task<bool> DoesSupplierRatingExist(Guid supplierRatingId);

	/// <summary>
	/// Retrieves all SupplierRatings with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of SupplierRatings to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of SupplierRatings DTOs.</returns>
	Task<ICollection<ERPSupplierRatingInformationDto>> GetAllSupplierRatings(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific SupplierRating.
	/// </summary>
	/// <param name="supplierRatingId">The Unique Id of the SupplierRating to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the SupplierRating DTO.</returns>
	Task<ERPSupplierRatingInformationDto> GetSupplierRating(Guid supplierRatingId);

	/// <summary>
	/// Saves the provided ERP supplierRating.
	/// </summary>
	/// <param name="supplierRating">The ERP supplierRating to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveSupplierRating(ERPSupplierRatingDto supplierRating);
}
