using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPShippingPaymentTypeModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ShippingPaymentTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShippingPaymentTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllShippingPaymentTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ShippingPaymentType information based on the specified ShippingPaymentType Unique Id.
	/// </summary>
	/// <param name="shippingPaymentTypeId">The Unique Id of the ShippingPaymentType.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetShippingPaymentType(Guid shippingPaymentTypeId);

	/// <summary>
	/// Validates the PUT request for creating or updating ShippingPaymentType information based on the specified ShippingPaymentType.
	/// </summary>
	/// <param name="shippingPaymentType">The ShippingPaymentType details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutShippingPaymentType(ERPShippingPaymentTypeDto shippingPaymentType);

	/// <summary>
	/// Processes the request to retrieve all ShippingPaymentTypes with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShippingPaymentTypes to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShippingPaymentTypes DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPShippingPaymentTypeDto>>> Process_GetAllShippingPaymentTypes(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ShippingPaymentType.
	/// </summary>
	/// <param name="shippingPaymentTypeId">The Unique Id of the ShippingPaymentType to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ShippingPaymentType DTO.</returns>
	Task<ERPResponseMessageDto<ERPShippingPaymentTypeDto>> Process_GetShippingPaymentType(Guid shippingPaymentTypeId);

	/// <summary>
	/// Processes the creating or updating of a ShippingPaymentType record.
	/// </summary>
	/// <param name="shippingPaymentType">The ShippingPaymentType data transfer object (DTO) containing the details of the ShippingPaymentType to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ShippingPaymentType details.</returns>
	Task<ERPResponseMessageDto<ERPShippingPaymentTypeDto>> Process_PutShippingPaymentType(ERPShippingPaymentTypeDto shippingPaymentType);

	/// <summary>
	/// Validates the request for deleting a ShippingPaymentType record.
	/// </summary>
	/// <param name="shippingPaymentTypeId">The Unique Id of the ShippingPaymentType.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteShippingPaymentType(Guid shippingPaymentTypeId);

	/// <summary>
	/// Processes the request to delete a ShippingPaymentType record.
	/// </summary>
	/// <param name="shippingPaymentTypeId">The Unique Id of the ShippingPaymentType.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPShippingPaymentTypeDto>> Process_DeleteShippingPaymentType(Guid shippingPaymentTypeId);
}
