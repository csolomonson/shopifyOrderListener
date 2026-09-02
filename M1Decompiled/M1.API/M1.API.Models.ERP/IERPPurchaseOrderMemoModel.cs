using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPurchaseOrderMemoModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PurchaseOrderMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrderMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PurchaseOrderMemo information based on the specified PurchaseOrderMemo Unique Id.
	/// </summary>
	/// <param name="purchaseOrderMemoId">The Unique Id of the PurchaseOrderMemo.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrderMemo(Guid purchaseOrderMemoId);

	/// <summary>
	/// Validates the PUT request for creating or updating PurchaseOrderMemo information based on the specified PurchaseOrderMemo.
	/// </summary>
	/// <param name="purchaseOrderMemo">The PurchaseOrderMemo details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrderMemo(ERPPurchaseOrderMemoDto purchaseOrderMemo);

	/// <summary>
	/// Processes the request to retrieve all PurchaseOrderMemos with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderMemos to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchaseOrderMemos DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPurchaseOrderMemoDto>>> Process_GetAllPurchaseOrderMemos(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PurchaseOrderMemo.
	/// </summary>
	/// <param name="purchaseOrderMemoId">The Unique Id of the PurchaseOrderMemo to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PurchaseOrderMemo DTO.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderMemoDto>> Process_GetPurchaseOrderMemo(Guid purchaseOrderMemoId);

	/// <summary>
	/// Processes the creating or updating of a PurchaseOrderMemo record.
	/// </summary>
	/// <param name="purchaseOrderMemo">The PurchaseOrderMemo data transfer object (DTO) containing the details of the PurchaseOrderMemo to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PurchaseOrderMemo details.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderMemoDto>> Process_PutPurchaseOrderMemo(ERPPurchaseOrderMemoDto purchaseOrderMemo);

	/// <summary>
	/// Validates the request for deleting a PurchaseOrderMemo record.
	/// </summary>
	/// <param name="purchaseOrderMemoId">The Unique Id of the PurchaseOrderMemo.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrderMemo(Guid purchaseOrderMemoId);

	/// <summary>
	/// Processes the request to delete a PurchaseOrderMemo record.
	/// </summary>
	/// <param name="purchaseOrderMemoId">The Unique Id of the PurchaseOrderMemo.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderMemoDto>> Process_DeletePurchaseOrderMemo(Guid purchaseOrderMemoId);
}
