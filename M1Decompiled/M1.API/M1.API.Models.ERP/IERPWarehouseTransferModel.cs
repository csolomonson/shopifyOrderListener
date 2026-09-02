using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWarehouseTransferModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WarehouseTransfers with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseTransfers to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseTransfers(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WarehouseTransfer information based on the specified WarehouseTransfer Unique Id.
	/// </summary>
	/// <param name="warehouseTransferId">The Unique Id of the WarehouseTransfer.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWarehouseTransfer(Guid warehouseTransferId);

	/// <summary>
	/// Validates the PUT request for creating or updating WarehouseTransfer information based on the specified WarehouseTransfer.
	/// </summary>
	/// <param name="warehouseTransfer">The WarehouseTransfer details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWarehouseTransfer(ERPWarehouseTransferDto warehouseTransfer);

	/// <summary>
	/// Processes the request to retrieve all WarehouseTransfers with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseTransfers to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseTransfers DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWarehouseTransferDto>>> Process_GetAllWarehouseTransfers(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WarehouseTransfer.
	/// </summary>
	/// <param name="warehouseTransferId">The Unique Id of the WarehouseTransfer to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WarehouseTransfer DTO.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseTransferDto>> Process_GetWarehouseTransfer(Guid warehouseTransferId);

	/// <summary>
	/// Processes the creating or updating of a WarehouseTransfer record.
	/// </summary>
	/// <param name="warehouseTransfer">The WarehouseTransfer data transfer object (DTO) containing the details of the WarehouseTransfer to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the WarehouseTransfer details.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseTransferDto>> Process_PutWarehouseTransfer(ERPWarehouseTransferDto warehouseTransfer);

	/// <summary>
	/// Validates the request for deleting a WarehouseTransfer record.
	/// </summary>
	/// <param name="warehouseTransferId">The Unique Id of the WarehouseTransfer.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseTransfer(Guid warehouseTransferId);

	/// <summary>
	/// Processes the request to delete a WarehouseTransfer record.
	/// </summary>
	/// <param name="warehouseTransferId">The Unique Id of the WarehouseTransfer.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWarehouseTransferDto>> Process_DeleteWarehouseTransfer(Guid warehouseTransferId);
}
