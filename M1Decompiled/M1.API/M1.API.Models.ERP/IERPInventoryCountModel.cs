using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPInventoryCountModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all InventoryCounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InventoryCounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllInventoryCounts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving InventoryCount information based on the specified InventoryCount Unique Id.
	/// </summary>
	/// <param name="inventoryCountId">The Unique Id of the InventoryCount.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetInventoryCount(Guid inventoryCountId);

	/// <summary>
	/// Validates the PUT request for creating or updating InventoryCount information based on the specified InventoryCount.
	/// </summary>
	/// <param name="inventoryCount">The InventoryCount details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutInventoryCount(ERPInventoryCountDto inventoryCount);

	/// <summary>
	/// Processes the request to retrieve all InventoryCounts with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of InventoryCounts to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of InventoryCounts DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPInventoryCountDto>>> Process_GetAllInventoryCounts(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific InventoryCount.
	/// </summary>
	/// <param name="inventoryCountId">The Unique Id of the InventoryCount to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the InventoryCount DTO.</returns>
	Task<ERPResponseMessageDto<ERPInventoryCountDto>> Process_GetInventoryCount(Guid inventoryCountId);

	/// <summary>
	/// Processes the creating or updating of a InventoryCount record.
	/// </summary>
	/// <param name="inventoryCount">The InventoryCount data transfer object (DTO) containing the details of the InventoryCount to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the InventoryCount details.</returns>
	Task<ERPResponseMessageDto<ERPInventoryCountDto>> Process_PutInventoryCount(ERPInventoryCountDto inventoryCount);

	/// <summary>
	/// Validates the request for deleting a InventoryCount record.
	/// </summary>
	/// <param name="inventoryCountId">The Unique Id of the InventoryCount.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteInventoryCount(Guid inventoryCountId);

	/// <summary>
	/// Processes the request to delete a InventoryCount record.
	/// </summary>
	/// <param name="inventoryCountId">The Unique Id of the InventoryCount.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPInventoryCountDto>> Process_DeleteInventoryCount(Guid inventoryCountId);
}
