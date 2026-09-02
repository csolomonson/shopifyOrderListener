using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPurchaseOrderModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PurchaseOrders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrders(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PurchaseOrder information based on the specified PurchaseOrder Unique Id.
	/// </summary>
	/// <param name="purchaseOrderId">The Unique Id of the PurchaseOrder.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrder(Guid purchaseOrderId);

	/// <summary>
	/// Validates the PUT request for creating or updating PurchaseOrder information based on the specified PurchaseOrder.
	/// </summary>
	/// <param name="purchaseOrder">The PurchaseOrder details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrder(ERPPurchaseOrderDto purchaseOrder);

	/// <summary>
	/// Processes the request to retrieve all PurchaseOrders with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrders to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchaseOrders DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPurchaseOrderDto>>> Process_GetAllPurchaseOrders(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PurchaseOrder.
	/// </summary>
	/// <param name="purchaseOrderId">The Unique Id of the PurchaseOrder to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PurchaseOrder DTO.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderDto>> Process_GetPurchaseOrder(Guid purchaseOrderId);

	/// <summary>
	/// Processes the creating or updating of a PurchaseOrder record.
	/// </summary>
	/// <param name="purchaseOrder">The PurchaseOrder data transfer object (DTO) containing the details of the PurchaseOrder to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PurchaseOrder details.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderDto>> Process_PutPurchaseOrder(ERPPurchaseOrderDto purchaseOrder);

	/// <summary>
	/// Validates the request for deleting a PurchaseOrder record.
	/// </summary>
	/// <param name="purchaseOrderId">The Unique Id of the PurchaseOrder.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrder(Guid purchaseOrderId);

	/// <summary>
	/// Processes the request to delete a PurchaseOrder record.
	/// </summary>
	/// <param name="purchaseOrderId">The Unique Id of the PurchaseOrder.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderDto>> Process_DeletePurchaseOrder(Guid purchaseOrderId);
}
