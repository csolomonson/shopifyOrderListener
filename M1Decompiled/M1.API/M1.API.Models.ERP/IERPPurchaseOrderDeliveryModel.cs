using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPurchaseOrderDeliveryModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PurchaseOrderDeliveries with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderDeliveries to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrderDeliveries(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PurchaseOrderDelivery information based on the specified PurchaseOrderDelivery Unique Id.
	/// </summary>
	/// <param name="purchaseOrderDeliveryId">The Unique Id of the PurchaseOrderDelivery.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrderDelivery(Guid purchaseOrderDeliveryId);

	/// <summary>
	/// Validates the PUT request for creating or updating PurchaseOrderDelivery information based on the specified PurchaseOrderDelivery.
	/// </summary>
	/// <param name="purchaseOrderDelivery">The PurchaseOrderDelivery details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrderDelivery(ERPPurchaseOrderDeliveryDto purchaseOrderDelivery);

	/// <summary>
	/// Processes the request to retrieve all PurchaseOrderDeliveries with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderDeliveries to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchaseOrderDeliveries DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPurchaseOrderDeliveryDto>>> Process_GetAllPurchaseOrderDeliveries(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PurchaseOrderDelivery.
	/// </summary>
	/// <param name="purchaseOrderDeliveryId">The Unique Id of the PurchaseOrderDelivery to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PurchaseOrderDelivery DTO.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderDeliveryDto>> Process_GetPurchaseOrderDelivery(Guid purchaseOrderDeliveryId);

	/// <summary>
	/// Processes the creating or updating of a PurchaseOrderDelivery record.
	/// </summary>
	/// <param name="purchaseOrderDelivery">The PurchaseOrderDelivery data transfer object (DTO) containing the details of the PurchaseOrderDelivery to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PurchaseOrderDelivery details.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderDeliveryDto>> Process_PutPurchaseOrderDelivery(ERPPurchaseOrderDeliveryDto purchaseOrderDelivery);

	/// <summary>
	/// Validates the request for deleting a PurchaseOrderDelivery record.
	/// </summary>
	/// <param name="purchaseOrderDeliveryId">The Unique Id of the PurchaseOrderDelivery.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrderDelivery(Guid purchaseOrderDeliveryId);

	/// <summary>
	/// Processes the request to delete a PurchaseOrderDelivery record.
	/// </summary>
	/// <param name="purchaseOrderDeliveryId">The Unique Id of the PurchaseOrderDelivery.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderDeliveryDto>> Process_DeletePurchaseOrderDelivery(Guid purchaseOrderDeliveryId);
}
