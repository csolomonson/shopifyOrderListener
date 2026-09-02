using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPShippingMethodModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ShippingMethods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShippingMethods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllShippingMethods(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ShippingMethod information based on the specified ShippingMethod Unique Id.
	/// </summary>
	/// <param name="shippingMethodId">The Unique Id of the ShippingMethod.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetShippingMethod(Guid shippingMethodId);

	/// <summary>
	/// Validates the PUT request for creating or updating ShippingMethod information based on the specified ShippingMethod.
	/// </summary>
	/// <param name="shippingMethod">The ShippingMethod details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutShippingMethod(ERPShippingMethodDto shippingMethod);

	/// <summary>
	/// Processes the request to retrieve all ShippingMethods with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShippingMethods to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShippingMethods DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPShippingMethodDto>>> Process_GetAllShippingMethods(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ShippingMethod.
	/// </summary>
	/// <param name="shippingMethodId">The Unique Id of the ShippingMethod to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ShippingMethod DTO.</returns>
	Task<ERPResponseMessageDto<ERPShippingMethodDto>> Process_GetShippingMethod(Guid shippingMethodId);

	/// <summary>
	/// Processes the creating or updating of a ShippingMethod record.
	/// </summary>
	/// <param name="shippingMethod">The ShippingMethod data transfer object (DTO) containing the details of the ShippingMethod to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ShippingMethod details.</returns>
	Task<ERPResponseMessageDto<ERPShippingMethodDto>> Process_PutShippingMethod(ERPShippingMethodDto shippingMethod);

	/// <summary>
	/// Validates the request for deleting a ShippingMethod record.
	/// </summary>
	/// <param name="shippingMethodId">The Unique Id of the ShippingMethod.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteShippingMethod(Guid shippingMethodId);

	/// <summary>
	/// Processes the request to delete a ShippingMethod record.
	/// </summary>
	/// <param name="shippingMethodId">The Unique Id of the ShippingMethod.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPShippingMethodDto>> Process_DeleteShippingMethod(Guid shippingMethodId);
}
