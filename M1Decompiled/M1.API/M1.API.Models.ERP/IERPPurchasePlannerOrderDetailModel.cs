using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPurchasePlannerOrderDetailModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PurchasePlannerOrderDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchasePlannerOrderDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPurchasePlannerOrderDetails(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PurchasePlannerOrderDetail information based on the specified PurchasePlannerOrderDetail Unique Id.
	/// </summary>
	/// <param name="purchasePlannerOrderDetailId">The Unique Id of the PurchasePlannerOrderDetail.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPurchasePlannerOrderDetail(Guid purchasePlannerOrderDetailId);

	/// <summary>
	/// Validates the PUT request for creating or updating PurchasePlannerOrderDetail information based on the specified PurchasePlannerOrderDetail.
	/// </summary>
	/// <param name="purchasePlannerOrderDetail">The PurchasePlannerOrderDetail details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPurchasePlannerOrderDetail(ERPPurchasePlannerOrderDetailDto purchasePlannerOrderDetail);

	/// <summary>
	/// Processes the request to retrieve all PurchasePlannerOrderDetails with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchasePlannerOrderDetails to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchasePlannerOrderDetails DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPurchasePlannerOrderDetailDto>>> Process_GetAllPurchasePlannerOrderDetails(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PurchasePlannerOrderDetail.
	/// </summary>
	/// <param name="purchasePlannerOrderDetailId">The Unique Id of the PurchasePlannerOrderDetail to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PurchasePlannerOrderDetail DTO.</returns>
	Task<ERPResponseMessageDto<ERPPurchasePlannerOrderDetailDto>> Process_GetPurchasePlannerOrderDetail(Guid purchasePlannerOrderDetailId);

	/// <summary>
	/// Processes the creating or updating of a PurchasePlannerOrderDetail record.
	/// </summary>
	/// <param name="purchasePlannerOrderDetail">The PurchasePlannerOrderDetail data transfer object (DTO) containing the details of the PurchasePlannerOrderDetail to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PurchasePlannerOrderDetail details.</returns>
	Task<ERPResponseMessageDto<ERPPurchasePlannerOrderDetailDto>> Process_PutPurchasePlannerOrderDetail(ERPPurchasePlannerOrderDetailDto purchasePlannerOrderDetail);

	/// <summary>
	/// Validates the request for deleting a PurchasePlannerOrderDetail record.
	/// </summary>
	/// <param name="purchasePlannerOrderDetailId">The Unique Id of the PurchasePlannerOrderDetail.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePurchasePlannerOrderDetail(Guid purchasePlannerOrderDetailId);

	/// <summary>
	/// Processes the request to delete a PurchasePlannerOrderDetail record.
	/// </summary>
	/// <param name="purchasePlannerOrderDetailId">The Unique Id of the PurchasePlannerOrderDetail.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPurchasePlannerOrderDetailDto>> Process_DeletePurchasePlannerOrderDetail(Guid purchasePlannerOrderDetailId);
}
