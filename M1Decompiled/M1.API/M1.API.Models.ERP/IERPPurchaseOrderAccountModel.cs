using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPurchaseOrderAccountModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PurchaseOrderAccounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderAccounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrderAccounts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PurchaseOrderAccount information based on the specified PurchaseOrderAccount Unique Id.
	/// </summary>
	/// <param name="purchaseOrderAccountId">The Unique Id of the PurchaseOrderAccount.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrderAccount(Guid purchaseOrderAccountId);

	/// <summary>
	/// Validates the PUT request for creating or updating PurchaseOrderAccount information based on the specified PurchaseOrderAccount.
	/// </summary>
	/// <param name="purchaseOrderAccount">The PurchaseOrderAccount details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrderAccount(ERPPurchaseOrderAccountDto purchaseOrderAccount);

	/// <summary>
	/// Processes the request to retrieve all PurchaseOrderAccounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PurchaseOrderAccounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PurchaseOrderAccounts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPurchaseOrderAccountDto>>> Process_GetAllPurchaseOrderAccounts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PurchaseOrderAccount.
	/// </summary>
	/// <param name="purchaseOrderAccountId">The Unique Id of the PurchaseOrderAccount to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PurchaseOrderAccount DTO.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderAccountDto>> Process_GetPurchaseOrderAccount(Guid purchaseOrderAccountId);

	/// <summary>
	/// Processes the creating or updating of a PurchaseOrderAccount record.
	/// </summary>
	/// <param name="purchaseOrderAccount">The PurchaseOrderAccount data transfer object (DTO) containing the details of the PurchaseOrderAccount to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PurchaseOrderAccount details.</returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderAccountDto>> Process_PutPurchaseOrderAccount(ERPPurchaseOrderAccountDto purchaseOrderAccount);

	/// <summary>
	/// Validates the request for deleting a PurchaseOrderAccount record.
	/// </summary>
	/// <param name="purchaseOrderAccountId">The Unique Id of the PurchaseOrderAccount.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrderAccount(Guid purchaseOrderAccountId);

	/// <summary>
	/// Processes the request to delete a PurchaseOrderAccount record.
	/// </summary>
	/// <param name="purchaseOrderAccountId">The Unique Id of the PurchaseOrderAccount.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPurchaseOrderAccountDto>> Process_DeletePurchaseOrderAccount(Guid purchaseOrderAccountId);
}
