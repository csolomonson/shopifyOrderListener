using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPShipmentFreightReferenceModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all ShipmentFreightReferences with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentFreightReferences to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllShipmentFreightReferences(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving ShipmentFreightReference information based on the specified ShipmentFreightReference Unique Id.
	/// </summary>
	/// <param name="shipmentFreightReferenceId">The Unique Id of the ShipmentFreightReference.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetShipmentFreightReference(Guid shipmentFreightReferenceId);

	/// <summary>
	/// Validates the PUT request for creating or updating ShipmentFreightReference information based on the specified ShipmentFreightReference.
	/// </summary>
	/// <param name="shipmentFreightReference">The ShipmentFreightReference details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutShipmentFreightReference(ERPShipmentFreightReferenceDto shipmentFreightReference);

	/// <summary>
	/// Processes the request to retrieve all ShipmentFreightReferences with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of ShipmentFreightReferences to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of ShipmentFreightReferences DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPShipmentFreightReferenceDto>>> Process_GetAllShipmentFreightReferences(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific ShipmentFreightReference.
	/// </summary>
	/// <param name="shipmentFreightReferenceId">The Unique Id of the ShipmentFreightReference to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the ShipmentFreightReference DTO.</returns>
	Task<ERPResponseMessageDto<ERPShipmentFreightReferenceDto>> Process_GetShipmentFreightReference(Guid shipmentFreightReferenceId);

	/// <summary>
	/// Processes the creating or updating of a ShipmentFreightReference record.
	/// </summary>
	/// <param name="shipmentFreightReference">The ShipmentFreightReference data transfer object (DTO) containing the details of the ShipmentFreightReference to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the ShipmentFreightReference details.</returns>
	Task<ERPResponseMessageDto<ERPShipmentFreightReferenceDto>> Process_PutShipmentFreightReference(ERPShipmentFreightReferenceDto shipmentFreightReference);

	/// <summary>
	/// Validates the request for deleting a ShipmentFreightReference record.
	/// </summary>
	/// <param name="shipmentFreightReferenceId">The Unique Id of the ShipmentFreightReference.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteShipmentFreightReference(Guid shipmentFreightReferenceId);

	/// <summary>
	/// Processes the request to delete a ShipmentFreightReference record.
	/// </summary>
	/// <param name="shipmentFreightReferenceId">The Unique Id of the ShipmentFreightReference.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPShipmentFreightReferenceDto>> Process_DeleteShipmentFreightReference(Guid shipmentFreightReferenceId);
}
