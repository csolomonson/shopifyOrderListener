using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPWarehouseBinModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all WarehouseBins with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseBins to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllWarehouseBins(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving WarehouseBin information based on the specified WarehouseBin Unique Id.
	/// </summary>
	/// <param name="warehouseBinId">The Unique Id of the WarehouseBin.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetWarehouseBin(Guid warehouseBinId);

	/// <summary>
	/// Validates the PUT request for creating or updating WarehouseBin information based on the specified WarehouseBin.
	/// </summary>
	/// <param name="warehouseBin">The WarehouseBin details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutWarehouseBin(ERPWarehouseBinDto warehouseBin);

	/// <summary>
	/// Processes the request to retrieve all WarehouseBins with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of WarehouseBins to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of WarehouseBins DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPWarehouseBinDto>>> Process_GetAllWarehouseBins(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific WarehouseBin.
	/// </summary>
	/// <param name="warehouseBinId">The Unique Id of the WarehouseBin to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the WarehouseBin DTO.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseBinDto>> Process_GetWarehouseBin(Guid warehouseBinId);

	/// <summary>
	/// Processes the creating or updating of a WarehouseBin record.
	/// </summary>
	/// <param name="warehouseBin">The WarehouseBin data transfer object (DTO) containing the details of the WarehouseBin to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the WarehouseBin details.</returns>
	Task<ERPResponseMessageDto<ERPWarehouseBinDto>> Process_PutWarehouseBin(ERPWarehouseBinDto warehouseBin);

	/// <summary>
	/// Validates the request for deleting a WarehouseBin record.
	/// </summary>
	/// <param name="warehouseBinId">The Unique Id of the WarehouseBin.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteWarehouseBin(Guid warehouseBinId);

	/// <summary>
	/// Processes the request to delete a WarehouseBin record.
	/// </summary>
	/// <param name="warehouseBinId">The Unique Id of the WarehouseBin.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPWarehouseBinDto>> Process_DeleteWarehouseBin(Guid warehouseBinId);
}
