using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWarehouseReceiptLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WarehouseReceiptLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseReceiptLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseReceiptLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WarehouseReceiptLine information based on the specified WarehouseReceiptLine Unique Id.
	/// </summary>
	/// <param name="warehouseReceiptLineId">The Unique Id of the WarehouseReceiptLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWarehouseReceiptLine(Guid warehouseReceiptLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating WarehouseReceiptLine information based on the specified WarehouseReceiptLine.
	/// </summary>
	/// <param name="warehouseReceiptLine">The WarehouseReceiptLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWarehouseReceiptLine(ERPWarehouseReceiptLineDto warehouseReceiptLine);

	/// <summary>
	/// Processes the request to retrieve all WarehouseReceiptLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseReceiptLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseReceiptLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWarehouseReceiptLineDto>>> Process_GetAllWarehouseReceiptLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WarehouseReceiptLine.
	/// </summary>
	/// <param name="warehouseReceiptLineId">The Unique Id of the WarehouseReceiptLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WarehouseReceiptLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseReceiptLineDto>> Process_GetWarehouseReceiptLine(Guid warehouseReceiptLineId);

	/// <summary>
	/// Processes the creating or updating of a WarehouseReceiptLine record.
	/// </summary>
	/// <param name="warehouseReceiptLine">The WarehouseReceiptLine data transfer object (DTO) containing the details of the WarehouseReceiptLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the WarehouseReceiptLine details.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseReceiptLineDto>> Process_PutWarehouseReceiptLine(ERPWarehouseReceiptLineDto warehouseReceiptLine);

	/// <summary>
	/// Validates the request for deleting a WarehouseReceiptLine record.
	/// </summary>
	/// <param name="warehouseReceiptLineId">The Unique Id of the WarehouseReceiptLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseReceiptLine(Guid warehouseReceiptLineId);

	/// <summary>
	/// Processes the request to delete a WarehouseReceiptLine record.
	/// </summary>
	/// <param name="warehouseReceiptLineId">The Unique Id of the WarehouseReceiptLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWarehouseReceiptLineDto>> Process_DeleteWarehouseReceiptLine(Guid warehouseReceiptLineId);
}
