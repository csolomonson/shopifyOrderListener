using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPurchaseOrderLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PurchaseOrderLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrderLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PurchaseOrderLine information based on the specified PurchaseOrderLine Unique Id.
	/// </summary>
	/// <param name="purchaseOrderLineId">The Unique Id of the PurchaseOrderLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrderLine(Guid purchaseOrderLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating PurchaseOrderLine information based on the specified PurchaseOrderLine.
	/// </summary>
	/// <param name="purchaseOrderLine">The PurchaseOrderLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrderLine(ERPPurchaseOrderLineDto purchaseOrderLine);

	/// <summary>
	/// Processes the request to retrieve all PurchaseOrderLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchaseOrderLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPurchaseOrderLineDto>>> Process_GetAllPurchaseOrderLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PurchaseOrderLine.
	/// </summary>
	/// <param name="purchaseOrderLineId">The Unique Id of the PurchaseOrderLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PurchaseOrderLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderLineDto>> Process_GetPurchaseOrderLine(Guid purchaseOrderLineId);

	/// <summary>
	/// Processes the creating or updating of a PurchaseOrderLine record.
	/// </summary>
	/// <param name="purchaseOrderLine">The PurchaseOrderLine data transfer object (DTO) containing the details of the PurchaseOrderLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PurchaseOrderLine details.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderLineDto>> Process_PutPurchaseOrderLine(ERPPurchaseOrderLineDto purchaseOrderLine);

	/// <summary>
	/// Validates the request for deleting a PurchaseOrderLine record.
	/// </summary>
	/// <param name="purchaseOrderLineId">The Unique Id of the PurchaseOrderLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrderLine(Guid purchaseOrderLineId);

	/// <summary>
	/// Processes the request to delete a PurchaseOrderLine record.
	/// </summary>
	/// <param name="purchaseOrderLineId">The Unique Id of the PurchaseOrderLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderLineDto>> Process_DeletePurchaseOrderLine(Guid purchaseOrderLineId);
}
