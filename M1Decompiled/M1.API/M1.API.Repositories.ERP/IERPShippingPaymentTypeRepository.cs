using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Repositories.ERP;

public interface IERPShippingPaymentTypeRepository : IAPIBaseRepository, IDisposable
{
	/// <summary>
	/// Checks if a ShippingPaymentType with the specified Unique Id exists.
	/// </summary>
	/// <param name="shippingPaymentTypeId">The Unique Id of the ShippingPaymentType to check.</param>
	/// <returns>A task representing the asynchronous operation. The task result indicates whether the ShippingPaymentType exists or not.</returns>
	Task<bool> DoesShippingPaymentTypeExist(Guid shippingPaymentTypeId);

	/// <summary>
	/// Retrieves all ShippingPaymentTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShippingPaymentTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter for the query results.</param>
	/// <param name="orderBy">The order by for the query results.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShippingPaymentTypes DTOs.</returns>
	Task<ICollection<ERPShippingPaymentTypeInformationDto>> GetAllShippingPaymentTypes(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null);

	/// <summary>
	/// Retrieves detailed information about a specific ShippingPaymentType.
	/// </summary>
	/// <param name="shippingPaymentTypeId">The Unique Id of the ShippingPaymentType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the ShippingPaymentType DTO.</returns>
	Task<ERPShippingPaymentTypeInformationDto> GetShippingPaymentType(Guid shippingPaymentTypeId);

	/// <summary>
	/// Saves the provided ERP shippingPaymentType.
	/// </summary>
	/// <param name="shippingPaymentType">The ERP shippingPaymentType to be saved.</param>
	/// <returns>
	/// An asynchronous task that returns an <see cref="T:M1.API.DTOs.Core.APIValidationInfoDto" /> containing information about the save operation,
	/// including any errors, warnings, and the HTTP status code.
	/// </returns>
	Task<APIValidationInfoDto> SaveShippingPaymentType(ERPShippingPaymentTypeDto shippingPaymentType);
}
