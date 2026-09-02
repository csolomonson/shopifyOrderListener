using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPPartWarehouseLocationModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all PartWarehouseLocations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartWarehouseLocations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllPartWarehouseLocations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving PartWarehouseLocation information based on the specified PartWarehouseLocation Unique Id.
	/// </summary>
	/// <param name="partWarehouseLocationId">The Unique Id of the PartWarehouseLocation.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetPartWarehouseLocation(Guid partWarehouseLocationId);

	/// <summary>
	/// Validates the PUT request for creating or updating PartWarehouseLocation information based on the specified PartWarehouseLocation.
	/// </summary>
	/// <param name="partWarehouseLocation">The PartWarehouseLocation details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutPartWarehouseLocation(ERPPartWarehouseLocationDto partWarehouseLocation);

	/// <summary>
	/// Processes the request to retrieve all PartWarehouseLocations with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of PartWarehouseLocations to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of PartWarehouseLocations DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPPartWarehouseLocationDto>>> Process_GetAllPartWarehouseLocations(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific PartWarehouseLocation.
	/// </summary>
	/// <param name="partWarehouseLocationId">The Unique Id of the PartWarehouseLocation to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the PartWarehouseLocation DTO.</returns>
	Task<ERPResponseMessageDto<ERPPartWarehouseLocationDto>> Process_GetPartWarehouseLocation(Guid partWarehouseLocationId);

	/// <summary>
	/// Processes the creating or updating of a PartWarehouseLocation record.
	/// </summary>
	/// <param name="partWarehouseLocation">The PartWarehouseLocation data transfer object (DTO) containing the details of the PartWarehouseLocation to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the PartWarehouseLocation details.</returns>
	Task<ERPResponseMessageDto<ERPPartWarehouseLocationDto>> Process_PutPartWarehouseLocation(ERPPartWarehouseLocationDto partWarehouseLocation);

	/// <summary>
	/// Validates the request for deleting a PartWarehouseLocation record.
	/// </summary>
	/// <param name="partWarehouseLocationId">The Unique Id of the PartWarehouseLocation.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeletePartWarehouseLocation(Guid partWarehouseLocationId);

	/// <summary>
	/// Processes the request to delete a PartWarehouseLocation record.
	/// </summary>
	/// <param name="partWarehouseLocationId">The Unique Id of the PartWarehouseLocation.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPPartWarehouseLocationDto>> Process_DeletePartWarehouseLocation(Guid partWarehouseLocationId);
}
