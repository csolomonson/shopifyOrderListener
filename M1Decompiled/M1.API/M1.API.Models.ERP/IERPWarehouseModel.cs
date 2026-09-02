using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWarehouseModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Warehouses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Warehouses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouses(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Warehouse information based on the specified Warehouse Unique Id.
	/// </summary>
	/// <param name="warehouseId">The Unique Id of the Warehouse.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWarehouse(Guid warehouseId);

	/// <summary>
	/// Validates the PUT request for creating or updating Warehouse information based on the specified Warehouse.
	/// </summary>
	/// <param name="warehouse">The Warehouse details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWarehouse(ERPWarehouseDto warehouse);

	/// <summary>
	/// Processes the request to retrieve all Warehouses with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Warehouses to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Warehouses DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWarehouseDto>>> Process_GetAllWarehouses(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Warehouse.
	/// </summary>
	/// <param name="warehouseId">The Unique Id of the Warehouse to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Warehouse DTO.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseDto>> Process_GetWarehouse(Guid warehouseId);

	/// <summary>
	/// Processes the creating or updating of a Warehouse record.
	/// </summary>
	/// <param name="warehouse">The Warehouse data transfer object (DTO) containing the details of the Warehouse to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Warehouse details.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseDto>> Process_PutWarehouse(ERPWarehouseDto warehouse);

	/// <summary>
	/// Validates the request for deleting a Warehouse record.
	/// </summary>
	/// <param name="warehouseId">The Unique Id of the Warehouse.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouse(Guid warehouseId);

	/// <summary>
	/// Processes the request to delete a Warehouse record.
	/// </summary>
	/// <param name="warehouseId">The Unique Id of the Warehouse.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWarehouseDto>> Process_DeleteWarehouse(Guid warehouseId);
}
