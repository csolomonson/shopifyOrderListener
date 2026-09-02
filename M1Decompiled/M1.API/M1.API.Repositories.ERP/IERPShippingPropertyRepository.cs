using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPShippingPropertyRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ShippingProperty with the specified Unique Id exists.
	/// </summary>
	/// <param name="shippingPropertyId">The Unique Id of the ShippingProperty to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ShippingProperty exists or not.</returns>
	Task<bool> DoesShippingPropertyExist(Guid shippingPropertyId);

	/// <summary>
	/// Retrieves all ShippingProperties with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShippingProperties to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShippingProperties DTOs.</returns>
	Task<ICollection<ERPShippingPropertyInformationDto>> GetAllShippingProperties(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ShippingProperty.
	/// </summary>
	/// <param name="shippingPropertyId">The Unique Id of the ShippingProperty to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ShippingProperty DTO.</returns>
	Task<ERPShippingPropertyInformationDto> GetShippingProperty(Guid shippingPropertyId);
}
