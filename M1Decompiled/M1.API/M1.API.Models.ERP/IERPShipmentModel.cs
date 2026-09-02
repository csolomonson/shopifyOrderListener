using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;

namespace M1.API.Models.ERP;

public interface IERPShipmentModel : IERPBaseModel, IAPIBaseModel, IDisposable
{
	/// <summary>
	/// Validates the request for retrieving all Shipments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Shipments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetAllShipments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Validates the request for retrieving Shipment information based on the specified Shipment Unique Id.
	/// </summary>
	/// <param name="shipmentId">The Unique Id of the Shipment.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_GetShipment(Guid shipmentId);

	/// <summary>
	/// Validates the PUT request for creating or updating Shipment information based on the specified Shipment.
	/// </summary>
	/// <param name="shipment">The Shipment details to be validated.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains the API validation information DTO.</returns>
	Task<APIValidationInfoDto> ValidateRequest_PutShipment(ERPShipmentDto shipment);

	/// <summary>
	/// Processes the request to retrieve all Shipments with optional pagination.
	/// </summary>
	/// <param name="pageSize">The maximum number of Shipments to retrieve per page.</param>
	/// <param name="pageNumber">The page number of the results.</param>
	/// <param name="filter">The filter applied to the record set.</param>
	/// <param name="orderBy">The order by clause applied to the record set.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with a list of Shipments DTOs.</returns>
	Task<ERPResponseMessageDto<IList<ERPShipmentDto>>> Process_GetAllShipments(int pageSize, int pageNumber, string[] filter, string orderBy);

	/// <summary>
	/// Processes the request to retrieve detailed information about a specific Shipment.
	/// </summary>
	/// <param name="shipmentId">The Unique Id of the Shipment to retrieve information for.</param>
	/// <returns>A task representing the asynchronous operation. The task result contains a ERP response message DTO with the Shipment DTO.</returns>
	Task<ERPResponseMessageDto<ERPShipmentDto>> Process_GetShipment(Guid shipmentId);

	/// <summary>
	/// Processes the creating or updating of a Shipment record.
	/// </summary>
	/// <param name="shipment">The Shipment data transfer object (DTO) containing the details of the Shipment to be created or updated.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="T:M1.API.DTOs.ERP.ERPResponseMessageDto`1" /> with the response message and the Shipment details.</returns>
	Task<ERPResponseMessageDto<ERPShipmentDto>> Process_PutShipment(ERPShipmentDto shipment);

	/// <summary>
	/// Validates the request for deleting a Shipment record.
	/// </summary>
	/// <param name="shipmentId">The Unique Id of the Shipment.</param>
	/// <returns></returns>
	Task<APIValidationInfoDto> ValidateRequest_DeleteShipment(Guid shipmentId);

	/// <summary>
	/// Processes the request to delete a Shipment record.
	/// </summary>
	/// <param name="shipmentId">The Unique Id of the Shipment.</param>
	/// <returns></returns>
	Task<ERPResponseMessageDto<ERPShipmentDto>> Process_DeleteShipment(Guid shipmentId);
}
