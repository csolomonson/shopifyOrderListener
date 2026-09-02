using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPRFQQuantityRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a RFQQuantity with the specified Unique Id exists.
	/// </summary>
	/// <param name="rFQQuantityId">The Unique Id of the RFQQuantity to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the RFQQuantity exists or not.</returns>
	Task<bool> DoesRFQQuantityExist(Guid rFQQuantityId);

	/// <summary>
	/// Retrieves all RFQQuantities with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of RFQQuantities to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of RFQQuantities DTOs.</returns>
	Task<ICollection<ERPRFQQuantityInformationDto>> GetAllRFQQuantities(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific RFQQuantity.
	/// </summary>
	/// <param name="rFQQuantityId">The Unique Id of the RFQQuantity to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the RFQQuantity DTO.</returns>
	Task<ERPRFQQuantityInformationDto> GetRFQQuantity(Guid rFQQuantityId);

	/// <summary>
	/// Saves the provided ERP rFQQuantity.
	/// </summary>
	/// <param name="rFQQuantity">The ERP rFQQuantity to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveRFQQuantity(ERPRFQQuantityDto rFQQuantity);
}
