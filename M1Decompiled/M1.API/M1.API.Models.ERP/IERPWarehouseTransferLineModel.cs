using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWarehouseTransferLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WarehouseTransferLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseTransferLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseTransferLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WarehouseTransferLine information based on the specified WarehouseTransferLine Unique Id.
	/// </summary>
	/// <param name="warehouseTransferLineId">The Unique Id of the WarehouseTransferLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWarehouseTransferLine(Guid warehouseTransferLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating WarehouseTransferLine information based on the specified WarehouseTransferLine.
	/// </summary>
	/// <param name="warehouseTransferLine">The WarehouseTransferLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWarehouseTransferLine(ERPWarehouseTransferLineDto warehouseTransferLine);

	/// <summary>
	/// Processes the request to retrieve all WarehouseTransferLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseTransferLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseTransferLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWarehouseTransferLineDto>>> Process_GetAllWarehouseTransferLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WarehouseTransferLine.
	/// </summary>
	/// <param name="warehouseTransferLineId">The Unique Id of the WarehouseTransferLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WarehouseTransferLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseTransferLineDto>> Process_GetWarehouseTransferLine(Guid warehouseTransferLineId);

	/// <summary>
	/// Processes the creating or updating of a WarehouseTransferLine record.
	/// </summary>
	/// <param name="warehouseTransferLine">The WarehouseTransferLine data transfer object (DTO) containing the details of the WarehouseTransferLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the WarehouseTransferLine details.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseTransferLineDto>> Process_PutWarehouseTransferLine(ERPWarehouseTransferLineDto warehouseTransferLine);

	/// <summary>
	/// Validates the request for deleting a WarehouseTransferLine record.
	/// </summary>
	/// <param name="warehouseTransferLineId">The Unique Id of the WarehouseTransferLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseTransferLine(Guid warehouseTransferLineId);

	/// <summary>
	/// Processes the request to delete a WarehouseTransferLine record.
	/// </summary>
	/// <param name="warehouseTransferLineId">The Unique Id of the WarehouseTransferLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWarehouseTransferLineDto>> Process_DeleteWarehouseTransferLine(Guid warehouseTransferLineId);
}
