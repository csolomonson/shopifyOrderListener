using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWarehouseRequisitionModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WarehouseRequisitions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseRequisitions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseRequisitions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WarehouseRequisition information based on the specified WarehouseRequisition Unique Id.
	/// </summary>
	/// <param name="warehouseRequisitionId">The Unique Id of the WarehouseRequisition.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWarehouseRequisition(Guid warehouseRequisitionId);

	/// <summary>
	/// Validates the PUT request for creating or updating WarehouseRequisition information based on the specified WarehouseRequisition.
	/// </summary>
	/// <param name="warehouseRequisition">The WarehouseRequisition details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWarehouseRequisition(ERPWarehouseRequisitionDto warehouseRequisition);

	/// <summary>
	/// Processes the request to retrieve all WarehouseRequisitions with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseRequisitions to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseRequisitions DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWarehouseRequisitionDto>>> Process_GetAllWarehouseRequisitions(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WarehouseRequisition.
	/// </summary>
	/// <param name="warehouseRequisitionId">The Unique Id of the WarehouseRequisition to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WarehouseRequisition DTO.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseRequisitionDto>> Process_GetWarehouseRequisition(Guid warehouseRequisitionId);

	/// <summary>
	/// Processes the creating or updating of a WarehouseRequisition record.
	/// </summary>
	/// <param name="warehouseRequisition">The WarehouseRequisition data transfer object (DTO) containing the details of the WarehouseRequisition to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the WarehouseRequisition details.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseRequisitionDto>> Process_PutWarehouseRequisition(ERPWarehouseRequisitionDto warehouseRequisition);

	/// <summary>
	/// Validates the request for deleting a WarehouseRequisition record.
	/// </summary>
	/// <param name="warehouseRequisitionId">The Unique Id of the WarehouseRequisition.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseRequisition(Guid warehouseRequisitionId);

	/// <summary>
	/// Processes the request to delete a WarehouseRequisition record.
	/// </summary>
	/// <param name="warehouseRequisitionId">The Unique Id of the WarehouseRequisition.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWarehouseRequisitionDto>> Process_DeleteWarehouseRequisition(Guid warehouseRequisitionId);
}
