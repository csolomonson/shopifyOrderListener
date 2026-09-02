using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPurchaseOrderApprovalModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PurchaseOrderApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrderApprovals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PurchaseOrderApproval information based on the specified PurchaseOrderApproval Unique Id.
	/// </summary>
	/// <param name="purchaseOrderApprovalId">The Unique Id of the PurchaseOrderApproval.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrderApproval(Guid purchaseOrderApprovalId);

	/// <summary>
	/// Validates the PUT request for creating or updating PurchaseOrderApproval information based on the specified PurchaseOrderApproval.
	/// </summary>
	/// <param name="purchaseOrderApproval">The PurchaseOrderApproval details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrderApproval(ERPPurchaseOrderApprovalDto purchaseOrderApproval);

	/// <summary>
	/// Processes the request to retrieve all PurchaseOrderApprovals with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderApprovals to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchaseOrderApprovals DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPurchaseOrderApprovalDto>>> Process_GetAllPurchaseOrderApprovals(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PurchaseOrderApproval.
	/// </summary>
	/// <param name="purchaseOrderApprovalId">The Unique Id of the PurchaseOrderApproval to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PurchaseOrderApproval DTO.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderApprovalDto>> Process_GetPurchaseOrderApproval(Guid purchaseOrderApprovalId);

	/// <summary>
	/// Processes the creating or updating of a PurchaseOrderApproval record.
	/// </summary>
	/// <param name="purchaseOrderApproval">The PurchaseOrderApproval data transfer object (DTO) containing the details of the PurchaseOrderApproval to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PurchaseOrderApproval details.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderApprovalDto>> Process_PutPurchaseOrderApproval(ERPPurchaseOrderApprovalDto purchaseOrderApproval);

	/// <summary>
	/// Validates the request for deleting a PurchaseOrderApproval record.
	/// </summary>
	/// <param name="purchaseOrderApprovalId">The Unique Id of the PurchaseOrderApproval.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrderApproval(Guid purchaseOrderApprovalId);

	/// <summary>
	/// Processes the request to delete a PurchaseOrderApproval record.
	/// </summary>
	/// <param name="purchaseOrderApprovalId">The Unique Id of the PurchaseOrderApproval.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderApprovalDto>> Process_DeletePurchaseOrderApproval(Guid purchaseOrderApprovalId);
}
