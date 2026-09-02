using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPurchaseOrderComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PurchaseOrderComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrderComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PurchaseOrderComponent information based on the specified PurchaseOrderComponent Unique Id.
	/// </summary>
	/// <param name="purchaseOrderComponentId">The Unique Id of the PurchaseOrderComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrderComponent(Guid purchaseOrderComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating PurchaseOrderComponent information based on the specified PurchaseOrderComponent.
	/// </summary>
	/// <param name="purchaseOrderComponent">The PurchaseOrderComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrderComponent(ERPPurchaseOrderComponentDto purchaseOrderComponent);

	/// <summary>
	/// Processes the request to retrieve all PurchaseOrderComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchaseOrderComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPurchaseOrderComponentDto>>> Process_GetAllPurchaseOrderComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PurchaseOrderComponent.
	/// </summary>
	/// <param name="purchaseOrderComponentId">The Unique Id of the PurchaseOrderComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PurchaseOrderComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderComponentDto>> Process_GetPurchaseOrderComponent(Guid purchaseOrderComponentId);

	/// <summary>
	/// Processes the creating or updating of a PurchaseOrderComponent record.
	/// </summary>
	/// <param name="purchaseOrderComponent">The PurchaseOrderComponent data transfer object (DTO) containing the details of the PurchaseOrderComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PurchaseOrderComponent details.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderComponentDto>> Process_PutPurchaseOrderComponent(ERPPurchaseOrderComponentDto purchaseOrderComponent);

	/// <summary>
	/// Validates the request for deleting a PurchaseOrderComponent record.
	/// </summary>
	/// <param name="purchaseOrderComponentId">The Unique Id of the PurchaseOrderComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrderComponent(Guid purchaseOrderComponentId);

	/// <summary>
	/// Processes the request to delete a PurchaseOrderComponent record.
	/// </summary>
	/// <param name="purchaseOrderComponentId">The Unique Id of the PurchaseOrderComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderComponentDto>> Process_DeletePurchaseOrderComponent(Guid purchaseOrderComponentId);
}
