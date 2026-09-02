using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWarehouseRequisitionComponentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WarehouseRequisitionComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseRequisitionComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseRequisitionComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WarehouseRequisitionComponent information based on the specified WarehouseRequisitionComponent Unique Id.
	/// </summary>
	/// <param name="warehouseRequisitionComponentId">The Unique Id of the WarehouseRequisitionComponent.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWarehouseRequisitionComponent(Guid warehouseRequisitionComponentId);

	/// <summary>
	/// Validates the PUT request for creating or updating WarehouseRequisitionComponent information based on the specified WarehouseRequisitionComponent.
	/// </summary>
	/// <param name="warehouseRequisitionComponent">The WarehouseRequisitionComponent details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWarehouseRequisitionComponent(ERPWarehouseRequisitionComponentDto warehouseRequisitionComponent);

	/// <summary>
	/// Processes the request to retrieve all WarehouseRequisitionComponents with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseRequisitionComponents to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseRequisitionComponents DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWarehouseRequisitionComponentDto>>> Process_GetAllWarehouseRequisitionComponents(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WarehouseRequisitionComponent.
	/// </summary>
	/// <param name="warehouseRequisitionComponentId">The Unique Id of the WarehouseRequisitionComponent to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WarehouseRequisitionComponent DTO.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseRequisitionComponentDto>> Process_GetWarehouseRequisitionComponent(Guid warehouseRequisitionComponentId);

	/// <summary>
	/// Processes the creating or updating of a WarehouseRequisitionComponent record.
	/// </summary>
	/// <param name="warehouseRequisitionComponent">The WarehouseRequisitionComponent data transfer object (DTO) containing the details of the WarehouseRequisitionComponent to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the WarehouseRequisitionComponent details.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseRequisitionComponentDto>> Process_PutWarehouseRequisitionComponent(ERPWarehouseRequisitionComponentDto warehouseRequisitionComponent);

	/// <summary>
	/// Validates the request for deleting a WarehouseRequisitionComponent record.
	/// </summary>
	/// <param name="warehouseRequisitionComponentId">The Unique Id of the WarehouseRequisitionComponent.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseRequisitionComponent(Guid warehouseRequisitionComponentId);

	/// <summary>
	/// Processes the request to delete a WarehouseRequisitionComponent record.
	/// </summary>
	/// <param name="warehouseRequisitionComponentId">The Unique Id of the WarehouseRequisitionComponent.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWarehouseRequisitionComponentDto>> Process_DeleteWarehouseRequisitionComponent(Guid warehouseRequisitionComponentId);
}
