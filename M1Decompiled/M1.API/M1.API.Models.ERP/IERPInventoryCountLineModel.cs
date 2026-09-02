using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPInventoryCountLineModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all InventoryCountLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InventoryCountLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllInventoryCountLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving InventoryCountLine information based on the specified InventoryCountLine Unique Id.
	/// </summary>
	/// <param name="inventoryCountLineId">The Unique Id of the InventoryCountLine.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetInventoryCountLine(Guid inventoryCountLineId);

	/// <summary>
	/// Validates the PUT request for creating or updating InventoryCountLine information based on the specified InventoryCountLine.
	/// </summary>
	/// <param name="inventoryCountLine">The InventoryCountLine details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutInventoryCountLine(ERPInventoryCountLineDto inventoryCountLine);

	/// <summary>
	/// Processes the request to retrieve all InventoryCountLines with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InventoryCountLines to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of InventoryCountLines DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPInventoryCountLineDto>>> Process_GetAllInventoryCountLines(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific InventoryCountLine.
	/// </summary>
	/// <param name="inventoryCountLineId">The Unique Id of the InventoryCountLine to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the InventoryCountLine DTO.</returns>
	Task<ERPResponseMessageDto<ERPInventoryCountLineDto>> Process_GetInventoryCountLine(Guid inventoryCountLineId);

	/// <summary>
	/// Processes the creating or updating of a InventoryCountLine record.
	/// </summary>
	/// <param name="inventoryCountLine">The InventoryCountLine data transfer object (DTO) containing the details of the InventoryCountLine to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the InventoryCountLine details.</returns>
	Task<ERPResponseMessageDto<ERPInventoryCountLineDto>> Process_PutInventoryCountLine(ERPInventoryCountLineDto inventoryCountLine);

	/// <summary>
	/// Validates the request for deleting a InventoryCountLine record.
	/// </summary>
	/// <param name="inventoryCountLineId">The Unique Id of the InventoryCountLine.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteInventoryCountLine(Guid inventoryCountLineId);

	/// <summary>
	/// Processes the request to delete a InventoryCountLine record.
	/// </summary>
	/// <param name="inventoryCountLineId">The Unique Id of the InventoryCountLine.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPInventoryCountLineDto>> Process_DeleteInventoryCountLine(Guid inventoryCountLineId);
}
