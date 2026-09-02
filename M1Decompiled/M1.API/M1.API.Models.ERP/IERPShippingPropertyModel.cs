using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPShippingPropertyModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ShippingProperties with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShippingProperties to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllShippingProperties(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ShippingProperty information based on the specified ShippingProperty Unique Id.
	/// </summary>
	/// <param name="shippingPropertyId">The Unique Id of the ShippingProperty.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetShippingProperty(Guid shippingPropertyId);

	/// <summary>
	/// Processes the request to retrieve all ShippingProperties with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShippingProperties to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShippingProperties DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPShippingPropertyDto>>> Process_GetAllShippingProperties(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ShippingProperty.
	/// </summary>
	/// <param name="shippingPropertyId">The Unique Id of the ShippingProperty to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ShippingProperty DTO.</returns>
	Task<ERPResponseMessageDto<ERPShippingPropertyDto>> Process_GetShippingProperty(Guid shippingPropertyId);
}
