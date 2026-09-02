using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPShipmentPackageModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ShipmentPackages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentPackages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllShipmentPackages(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ShipmentPackage information based on the specified ShipmentPackage Unique Id.
	/// </summary>
	/// <param name="shipmentPackageId">The Unique Id of the ShipmentPackage.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetShipmentPackage(Guid shipmentPackageId);

	/// <summary>
	/// Validates the PUT request for creating or updating ShipmentPackage information based on the specified ShipmentPackage.
	/// </summary>
	/// <param name="shipmentPackage">The ShipmentPackage details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutShipmentPackage(ERPShipmentPackageDto shipmentPackage);

	/// <summary>
	/// Processes the request to retrieve all ShipmentPackages with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentPackages to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShipmentPackages DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPShipmentPackageDto>>> Process_GetAllShipmentPackages(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ShipmentPackage.
	/// </summary>
	/// <param name="shipmentPackageId">The Unique Id of the ShipmentPackage to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ShipmentPackage DTO.</returns>
	Task<ERPResponseMessageDto<ERPShipmentPackageDto>> Process_GetShipmentPackage(Guid shipmentPackageId);

	/// <summary>
	/// Processes the creating or updating of a ShipmentPackage record.
	/// </summary>
	/// <param name="shipmentPackage">The ShipmentPackage data transfer object (DTO) containing the details of the ShipmentPackage to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ShipmentPackage details.</returns>
	Task<ERPResponseMessageDto<ERPShipmentPackageDto>> Process_PutShipmentPackage(ERPShipmentPackageDto shipmentPackage);

	/// <summary>
	/// Validates the request for deleting a ShipmentPackage record.
	/// </summary>
	/// <param name="shipmentPackageId">The Unique Id of the ShipmentPackage.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteShipmentPackage(Guid shipmentPackageId);

	/// <summary>
	/// Processes the request to delete a ShipmentPackage record.
	/// </summary>
	/// <param name="shipmentPackageId">The Unique Id of the ShipmentPackage.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPShipmentPackageDto>> Process_DeleteShipmentPackage(Guid shipmentPackageId);
}
