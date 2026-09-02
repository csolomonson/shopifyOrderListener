using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWarehouseRequisitionLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WarehouseRequisitionLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseRequisitionLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseRequisitionLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WarehouseRequisitionLine information based on the specified WarehouseRequisitionLine Unique Id.
	/// </summary>
	/// <param name="warehouseRequisitionLineId">The Unique Id of the WarehouseRequisitionLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWarehouseRequisitionLine(Guid warehouseRequisitionLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating WarehouseRequisitionLine information based on the specified WarehouseRequisitionLine.
	/// </summary>
	/// <param name="warehouseRequisitionLine">The WarehouseRequisitionLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWarehouseRequisitionLine(ERPWarehouseRequisitionLineDto warehouseRequisitionLine);

	/// <summary>
	/// Processes the request to retrieve all WarehouseRequisitionLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseRequisitionLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseRequisitionLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWarehouseRequisitionLineDto>>> Process_GetAllWarehouseRequisitionLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WarehouseRequisitionLine.
	/// </summary>
	/// <param name="warehouseRequisitionLineId">The Unique Id of the WarehouseRequisitionLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WarehouseRequisitionLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseRequisitionLineDto>> Process_GetWarehouseRequisitionLine(Guid warehouseRequisitionLineId);

	/// <summary>
	/// Processes the creating or updating of a WarehouseRequisitionLine record.
	/// </summary>
	/// <param name="warehouseRequisitionLine">The WarehouseRequisitionLine data transfer object (DTO) containing the details of the WarehouseRequisitionLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the WarehouseRequisitionLine details.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseRequisitionLineDto>> Process_PutWarehouseRequisitionLine(ERPWarehouseRequisitionLineDto warehouseRequisitionLine);

	/// <summary>
	/// Validates the request for deleting a WarehouseRequisitionLine record.
	/// </summary>
	/// <param name="warehouseRequisitionLineId">The Unique Id of the WarehouseRequisitionLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseRequisitionLine(Guid warehouseRequisitionLineId);

	/// <summary>
	/// Processes the request to delete a WarehouseRequisitionLine record.
	/// </summary>
	/// <param name="warehouseRequisitionLineId">The Unique Id of the WarehouseRequisitionLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWarehouseRequisitionLineDto>> Process_DeleteWarehouseRequisitionLine(Guid warehouseRequisitionLineId);
}
