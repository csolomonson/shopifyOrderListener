using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWarehouseReceiptComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WarehouseReceiptComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseReceiptComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WarehouseReceiptComponent information based on the specified WarehouseReceiptComponent Unique Id.
	/// </summary>
	/// <param name="warehouseReceiptComponentId">The Unique Id of the WarehouseReceiptComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWarehouseReceiptComponent(Guid warehouseReceiptComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating WarehouseReceiptComponent information based on the specified WarehouseReceiptComponent.
	/// </summary>
	/// <param name="warehouseReceiptComponent">The WarehouseReceiptComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWarehouseReceiptComponent(ERPWarehouseReceiptComponentDto warehouseReceiptComponent);

	/// <summary>
	/// Processes the request to retrieve all WarehouseReceiptComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseReceiptComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseReceiptComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWarehouseReceiptComponentDto>>> Process_GetAllWarehouseReceiptComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WarehouseReceiptComponent.
	/// </summary>
	/// <param name="warehouseReceiptComponentId">The Unique Id of the WarehouseReceiptComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WarehouseReceiptComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseReceiptComponentDto>> Process_GetWarehouseReceiptComponent(Guid warehouseReceiptComponentId);

	/// <summary>
	/// Processes the creating or updating of a WarehouseReceiptComponent record.
	/// </summary>
	/// <param name="warehouseReceiptComponent">The WarehouseReceiptComponent data transfer object (DTO) containing the details of the WarehouseReceiptComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the WarehouseReceiptComponent details.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseReceiptComponentDto>> Process_PutWarehouseReceiptComponent(ERPWarehouseReceiptComponentDto warehouseReceiptComponent);

	/// <summary>
	/// Validates the request for deleting a WarehouseReceiptComponent record.
	/// </summary>
	/// <param name="warehouseReceiptComponentId">The Unique Id of the WarehouseReceiptComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseReceiptComponent(Guid warehouseReceiptComponentId);

	/// <summary>
	/// Processes the request to delete a WarehouseReceiptComponent record.
	/// </summary>
	/// <param name="warehouseReceiptComponentId">The Unique Id of the WarehouseReceiptComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWarehouseReceiptComponentDto>> Process_DeleteWarehouseReceiptComponent(Guid warehouseReceiptComponentId);
}
