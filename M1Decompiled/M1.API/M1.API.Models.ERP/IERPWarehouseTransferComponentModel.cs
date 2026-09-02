using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWarehouseTransferComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WarehouseTransferComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseTransferComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseTransferComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WarehouseTransferComponent information based on the specified WarehouseTransferComponent Unique Id.
	/// </summary>
	/// <param name="warehouseTransferComponentId">The Unique Id of the WarehouseTransferComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWarehouseTransferComponent(Guid warehouseTransferComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating WarehouseTransferComponent information based on the specified WarehouseTransferComponent.
	/// </summary>
	/// <param name="warehouseTransferComponent">The WarehouseTransferComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWarehouseTransferComponent(ERPWarehouseTransferComponentDto warehouseTransferComponent);

	/// <summary>
	/// Processes the request to retrieve all WarehouseTransferComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseTransferComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseTransferComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWarehouseTransferComponentDto>>> Process_GetAllWarehouseTransferComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WarehouseTransferComponent.
	/// </summary>
	/// <param name="warehouseTransferComponentId">The Unique Id of the WarehouseTransferComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WarehouseTransferComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseTransferComponentDto>> Process_GetWarehouseTransferComponent(Guid warehouseTransferComponentId);

	/// <summary>
	/// Processes the creating or updating of a WarehouseTransferComponent record.
	/// </summary>
	/// <param name="warehouseTransferComponent">The WarehouseTransferComponent data transfer object (DTO) containing the details of the WarehouseTransferComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the WarehouseTransferComponent details.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseTransferComponentDto>> Process_PutWarehouseTransferComponent(ERPWarehouseTransferComponentDto warehouseTransferComponent);

	/// <summary>
	/// Validates the request for deleting a WarehouseTransferComponent record.
	/// </summary>
	/// <param name="warehouseTransferComponentId">The Unique Id of the WarehouseTransferComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseTransferComponent(Guid warehouseTransferComponentId);

	/// <summary>
	/// Processes the request to delete a WarehouseTransferComponent record.
	/// </summary>
	/// <param name="warehouseTransferComponentId">The Unique Id of the WarehouseTransferComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWarehouseTransferComponentDto>> Process_DeleteWarehouseTransferComponent(Guid warehouseTransferComponentId);
}
