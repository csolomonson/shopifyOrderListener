using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWarehouseReceiptModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WarehouseReceipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseReceipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseReceipts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WarehouseReceipt information based on the specified WarehouseReceipt Unique Id.
	/// </summary>
	/// <param name="warehouseReceiptId">The Unique Id of the WarehouseReceipt.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWarehouseReceipt(Guid warehouseReceiptId);

	/// <summary>
	/// Validates the PUT request for creating or updating WarehouseReceipt information based on the specified WarehouseReceipt.
	/// </summary>
	/// <param name="warehouseReceipt">The WarehouseReceipt details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWarehouseReceipt(ERPWarehouseReceiptDto warehouseReceipt);

	/// <summary>
	/// Processes the request to retrieve all WarehouseReceipts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseReceipts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseReceipts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWarehouseReceiptDto>>> Process_GetAllWarehouseReceipts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WarehouseReceipt.
	/// </summary>
	/// <param name="warehouseReceiptId">The Unique Id of the WarehouseReceipt to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WarehouseReceipt DTO.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseReceiptDto>> Process_GetWarehouseReceipt(Guid warehouseReceiptId);

	/// <summary>
	/// Processes the creating or updating of a WarehouseReceipt record.
	/// </summary>
	/// <param name="warehouseReceipt">The WarehouseReceipt data transfer object (DTO) containing the details of the WarehouseReceipt to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the WarehouseReceipt details.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseReceiptDto>> Process_PutWarehouseReceipt(ERPWarehouseReceiptDto warehouseReceipt);

	/// <summary>
	/// Validates the request for deleting a WarehouseReceipt record.
	/// </summary>
	/// <param name="warehouseReceiptId">The Unique Id of the WarehouseReceipt.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseReceipt(Guid warehouseReceiptId);

	/// <summary>
	/// Processes the request to delete a WarehouseReceipt record.
	/// </summary>
	/// <param name="warehouseReceiptId">The Unique Id of the WarehouseReceipt.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWarehouseReceiptDto>> Process_DeleteWarehouseReceipt(Guid warehouseReceiptId);
}
