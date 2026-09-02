using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPShippingMethodRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ShippingMethod with the specified Unique Id exists.
	/// </summary>
	/// <param name="shippingMethodId">The Unique Id of the ShippingMethod to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ShippingMethod exists or not.</returns>
	Task<bool> DoesShippingMethodExist(Guid shippingMethodId);

	/// <summary>
	/// Retrieves all ShippingMethods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShippingMethods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShippingMethods DTOs.</returns>
	Task<ICollection<ERPShippingMethodInformationDto>> GetAllShippingMethods(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ShippingMethod.
	/// </summary>
	/// <param name="shippingMethodId">The Unique Id of the ShippingMethod to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ShippingMethod DTO.</returns>
	Task<ERPShippingMethodInformationDto> GetShippingMethod(Guid shippingMethodId);

	/// <summary>
	/// Saves the provided ERP shippingMethod.
	/// </summary>
	/// <param name="shippingMethod">The ERP shippingMethod to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveShippingMethod(ERPShippingMethodDto shippingMethod);
}
